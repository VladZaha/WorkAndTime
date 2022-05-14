using Effort;
using System;
using System.Diagnostics;
using System.Linq;
using System.Timers;
using System.Windows;
using System.Windows.Threading;

namespace WorkAndTime
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        DispatcherTimer timer;
        DateTime startTime;

        //Database context
        private ProjectContext _context;
        public MainWindow()
        {
            InitializeComponent();
            var conn = DbConnectionFactory.CreateTransient();
            _context = new ProjectContext(conn);

            //_context.Projects.Add(new Project() { Id=1,Name = "Project 1" });
            //_context.Projects.Add(new Project() { Id =2, Name = "Project 2" });
            //_context.SaveChanges();
            //ListBox_Projects.ItemsSource = _context.Projects.Select(c=>c.Name).ToList();

            Button_StopTimer.IsEnabled = false;
            Button_ShowResults.IsEnabled = false;
        }

        private void Button_ShowResults_Click(object sender, RoutedEventArgs e)
        {

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
        }
        private void timer_Tick(object sender, EventArgs e)
        {
            TextBlock_Timer.Text = (DateTime.Now - startTime).ToString(@"hh\:mm\:ss");
        }
        private void Button_StopTimer_Click(object sender, RoutedEventArgs e)
        {
            ListBox_History.Items.Add(new History() { TimePeriod = (DateTime.Now - startTime).ToString(@"hh\:mm\:ss"), Date = DateTime.Now, Progress = "0%" });
            TextBlock_Timer.Text = "00:00:00";
            timer.Stop();
            Button_StopTimer.IsEnabled = false;
            Button_StartTimer.IsEnabled = true;
        }

    }
}
