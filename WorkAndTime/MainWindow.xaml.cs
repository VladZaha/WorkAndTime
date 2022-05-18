using Effort;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using Font = iTextSharp.text.Font;

namespace WorkAndTime
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        DispatcherTimer timer;
        DispatcherTimer timerScreen;
        DateTime startTime;
        int timerScreenSetting = 10;
        private ProjectContext _context;
        public MainWindow()
        {
            InitializeComponent();
            var conn = DbConnectionFactory.CreateTransient();
            _context = new ProjectContext(conn);
            if (_context.Projects.ToList().Count() == 0)
            {
                List<Project> projects = new List<Project>
                {
                    new Project
                    {
                        Id = 1,
                        Description = "Description",
                        Name = "Diplom",
                        Status = "in progress",
                        Year = 2022
                    },
                     new Project
                    {
                        Id = 2,
                        Description = "Description",
                        Name = "HomeWork",
                        Status = "in progress",
                        Year = 2022
                    },
                };
                _context.Projects.Add(projects[0]);
                _context.Projects.Add(projects[1]);
                _context.SaveChanges();
            }
            ListBox_Projects.ItemsSource = _context.Projects.ToList();
            Button_StopTimer.IsEnabled = false;
            Button_StartTimer.IsEnabled = false;
        }

        private void Button_ShowResults_Click(object sender, RoutedEventArgs e)
        {

            var projects = _context.Projects.ToList();
            var histories = _context.History.ToList();
            //шрифт
            BaseFont bfTimes = BaseFont.CreateFont(BaseFont.TIMES_BOLDITALIC, BaseFont.CP1252, false);
            BaseFont bfTimes1 = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1250, false);
            BaseFont bfTimes2 = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1257, false);
            //колір
            Font times = new Font(bfTimes, 12, Font.ITALIC, BaseColor.GREEN);
            Font times1 = new Font(bfTimes1, 24, Font.BOLD, BaseColor.RED);
            Font times2 = new Font(bfTimes2, 12, Font.BOLDITALIC, BaseColor.BLACK);
            //запис
            //робимо сам файл
            Document doc = new Document();
            PdfWriter.GetInstance(doc, new FileStream("input.pdf", FileMode.Append));
            doc.Open();
            foreach (var project in projects)
            {
                doc.Add(new Paragraph(project.Id.ToString() + ";" + project.Name + ";" + project.Description + ";" + project.Year.ToString() + ";" + project.Status + ";", times));
                doc.Add(new Paragraph("---------------------------History----------------------------", times1));
                foreach (var historyItem in histories.Where(d=>d.ProjectId == project.Id))
                {
                    string[] g = historyItem.TimePeriod.Split('-', ':');
                    int fromH = int.Parse(g[0]);
                    int fromM = int.Parse(g[1]);
                    if (fromH > 0)
                    {
                        fromH = fromH * 60;
                    }
                    fromM = fromH + fromM;
                    double x = fromM / 480 * 100;
                    doc.Add(new Paragraph("    date                        range                work time                    progres", times2));
                    doc.Add(new Paragraph(historyItem.Date.ToShortDateString() + "          " + historyItem.TimePeriod + "               " + x.ToString() + "                      " + x.ToString() + "%", times2));
                    doc.Add(new Paragraph("______________________________________", times1));
                }
            }
         
            doc.Close();
            System.Diagnostics.Process.Start("input.pdf");
        }
        private void Button_StartTimer_Click(object sender, RoutedEventArgs e)
        {
            startTime = DateTime.Now;
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += timer_Tick;
            timer.Start();
            Button_StartTimer.IsEnabled = false;
            Button_StopTimer.IsEnabled = true;
            if (timerScreenSetting > 0)
            {
                timerScreen = new DispatcherTimer();
                timerScreen.Interval = TimeSpan.FromSeconds(timerScreenSetting);
                timerScreen.Tick += timerScreen_Tick;
                timerScreen.Start();
            }
        }
        private void timer_Tick(object sender, EventArgs e)
        {
            TextBlock_Timer.Text = (DateTime.Now - startTime).ToString(@"hh\:mm\:ss");
        }
        private void timerScreen_Tick(object sender, EventArgs e)
        {
            Project folder = (Project)ListBox_Projects.SelectedItem;
            System.IO.Directory.CreateDirectory(@"c:\WorkAndTime\ScreenCaptures\" + folder.Name);
            double screenLeft = SystemParameters.VirtualScreenLeft;
            double screenTop = SystemParameters.VirtualScreenTop;
            double screenWidth = SystemParameters.VirtualScreenWidth;
            double screenHeight = SystemParameters.VirtualScreenHeight;
            using (Bitmap bmp = new Bitmap((int)screenWidth, (int)screenHeight))
            {
                using (Graphics g = Graphics.FromImage(bmp))
                {
                    String filename = DateTime.Now.ToString("ddMMyyyy-hhmmss") + ".png";
                    g.CopyFromScreen((int)screenLeft, (int)screenTop, 0, 0, bmp.Size);
                    bmp.Save("C:\\WorkAndTime\\ScreenCaptures\\" + folder.Name + "\\" + filename);
                }
            }
        }
        private void Button_StopTimer_Click(object sender, RoutedEventArgs e)
        {
            var itemProject = ListBox_Projects.SelectedItem as Project;
            var historyElement = new History() { TimePeriod = (DateTime.Now - startTime).ToString(@"hh\:mm\:ss"), Date = DateTime.Now, Progress = "0%", ProjectId = itemProject.Id, Project = itemProject };
            _context.History.Add(historyElement);
            _context.SaveChanges();
            ListBox_History.ItemsSource = _context.History.Where(d => d.ProjectId.Equals(itemProject.Id)).ToList();
            TextBlock_Timer.Text = "00:00:00";
            timer.Stop();
            timerScreen.Stop();
            Button_StopTimer.IsEnabled = false;
            Button_StartTimer.IsEnabled = true;
        }

        private void menuTimer_Click(object sender, RoutedEventArgs e)
        {
            MenuItem item = (MenuItem)sender;
            switch (item.Name)
            {
                case "menu30":
                    timerScreenSetting = 30;
                    break;
                case "menu10":
                    timerScreenSetting = 10;
                    break;
                case "menu5":
                    timerScreenSetting = 5;
                    break;
                default:
                    timerScreenSetting = 0;
                    break;
            }
        }

        private void Button_AddProject_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                _context.Projects.Add(new Project() { Name = TextBox_ProjectName.Text, Description = TextBox_ProjectDescription.Text, Year = DatePicker_ProjectDate.SelectedDate.Value.Year, Status = "inprogress" });
                _context.SaveChanges();
                ListBox_Projects.ItemsSource = _context.Projects.ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

        }

        private void Button_RemoveProject_Click(object sender, RoutedEventArgs e)
        {
            var itemProject = (Project)ListBox_Projects.SelectedItem;
            if (itemProject != null)
                _context.Projects.Remove(itemProject);
            _context.SaveChanges();
            ListBox_Projects.ItemsSource = _context.Projects.ToList();
        }

        private void ShowScreenshots_Click(object sender, RoutedEventArgs e)
        {
            Button btn = (Button)sender;
            var projectName = btn.Tag.ToString();
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.Multiselect = true;
            dlg.DefaultExt = ".png";
            dlg.Filter = "JPEG Files (*.png)|*.png|PNG Files (*.png)|*.png|JPG Files (*.jpg)|*.jpg|GIF Files (*.gif)|*.gif";
            dlg.InitialDirectory = "C:\\WorkAndTime\\ScreenCaptures\\" + projectName + "\\";
            if (dlg.ShowDialog() == true) { }
        }

        private void ListBox_Projects_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var itemProject = ListBox_Projects.SelectedItem as Project;
            Button_StartTimer.IsEnabled = true;
            ListBox_History.ItemsSource = null;
            ListBox_History.ItemsSource = _context.History.Where(d => d.ProjectId.Equals(itemProject.Id)).ToList();
        }
    }
}
