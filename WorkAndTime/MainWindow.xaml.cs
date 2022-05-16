using Effort;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

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
        //Database context
        private ProjectContext _context;
        public MainWindow()
        {
            InitializeComponent();
            var conn = DbConnectionFactory.CreateTransient();
            _context = new ProjectContext(conn);
            ListBox_Projects.ItemsSource = _context.Projects.ToList();
            Button_StopTimer.IsEnabled = false;
            Button_ShowResults.IsEnabled = false;
        }

        private void Button_ShowResults_Click(object sender, RoutedEventArgs e)
        {
            //TODO Print PDF
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

            //Start screenshot timer
            timerScreen = new DispatcherTimer();
            timerScreen.Interval = TimeSpan.FromSeconds(timerScreenSetting);
            timerScreen.Tick += timerScreen_Tick;
            timerScreen.Start();

        }
        private void timer_Tick(object sender, EventArgs e)
        {
            TextBlock_Timer.Text = (DateTime.Now - startTime).ToString(@"hh\:mm\:ss");
        }
        private void timerScreen_Tick(object sender, EventArgs e)
        {

            //TODO Check saving screens
                var window = Application.Current.Windows.OfType<MainWindow>().First(); // to access controls
                System.IO.Directory.CreateDirectory(@"c:\WorkAndTime\ScreenCaptures\" + window.ListBox_Projects.SelectedItem.ToString());
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
                        bmp.Save("C:\\WorkAndTime\\ScreenCaptures\\" + window.ListBox_Projects.SelectedItem.ToString() + "\\" + filename);
                    }
                }
        }
        private void Button_StopTimer_Click(object sender, RoutedEventArgs e)
        {
            var itemProject = ListBox_Projects.SelectedItem as Project;
            var historyElement = new History() { TimePeriod = (DateTime.Now - startTime).ToString(@"hh\:mm\:ss"), Date = DateTime.Now, Progress = "0%", ProjectId = itemProject.Id, Project = itemProject };
            _context.History.Add(historyElement);
            _context.SaveChanges();
            ListBox_History.Items.Add(historyElement);
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
            _context.Projects.Add(new Project() {Name = TextBox_ProjectName.Text, Description = TextBox_ProjectDescription.Text, Year = DatePicker_ProjectDate.SelectedDate.Value.Year, Status = "inprogress" });
            _context.SaveChanges();
            ListBox_Projects.ItemsSource = _context.Projects.ToList();
        }

        private void Button_RemoveProject_Click(object sender, RoutedEventArgs e)
        {
            var itemProject = (Project)ListBox_Projects.SelectedItem;
            //TODO Remove project from database

            if(itemProject != null)
                _context.Projects.Remove(itemProject);

            _context.SaveChanges();
            ListBox_Projects.ItemsSource = _context.Projects.ToList();
        }

        private void ShowScreenshots_Click(object sender, RoutedEventArgs e)
        {
            Button btn = (Button)sender;
            var projectName = btn.Tag.ToString();
            //TODO Open screenshots folder
        }
    }
}
