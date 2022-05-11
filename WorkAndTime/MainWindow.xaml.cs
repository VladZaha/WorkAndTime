using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace WorkAndTime
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public class History
        {
            public string TimePeriod { get; set; }
            public DateTime Date { get; set; }
            public string Progress { get; set; }
        }
        /*DispatcherTimer timer;
        DateTime startTime;*/
        private const string _startTimeDisplay = "00:00:00";
        private Stopwatch _stopWatch;
        private Timer _timer;

        public MainWindow()
        {
            InitializeComponent();

            _stopWatch = new Stopwatch();
            _timer = new Timer(1000);

            //_timer.Elapsed += OnTimerElapse;
            Button_StopTimer.IsEnabled = false;
            Button_ShowResults.IsEnabled = false;
        }

        private void OnTimerElapse(object sender, ElapsedEventArgs e)
        {
            Application.Current.Dispatcher.Invoke(() => TextBlock_Timer.Text = _stopWatch.Elapsed.ToString(@"hh\:mm\:ss"));
        }

        private void Button_ShowResults_Click(object sender, RoutedEventArgs e)
        {
            ListBox_History.Items.Add(new History() { TimePeriod = _stopWatch.Elapsed.ToString(@"hh\:mm\:ss"), Date = DateTime.Now, Progress = "0%" });
            _stopWatch = new Stopwatch();
            _timer = new Timer(1000);
            TextBlock_Timer.Text = "00:00:00";
            Button_StartTimer.IsEnabled = true;
            Button_StopTimer.IsEnabled = false;
            Button_ShowResults.IsEnabled = false;
            /*
            var Renderer = new ChromePdfRenderer();
            var PDF = Renderer.RenderHtmlAsPdf("<h1>Hello World<h1><a href=\"#\">Link</a>);
            PDF.SaveAs("html-string.pdf");
            System.Diagnostics.Process.Start("html-string.pdf");
            */
        }
        private void Button_StartTimer_Click(object sender, RoutedEventArgs e)
        {
            _stopWatch.Start();
            _timer.Start();
            _timer.Elapsed += OnTimerElapse;
            Button_StartTimer.IsEnabled = false;
            Button_StopTimer.IsEnabled = true;
            Button_ShowResults.IsEnabled = false;
            /*startTime = DateTime.Now;
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += timer_Tick;
            timer.Start();
            Button_StartTimer.IsEnabled = false;
            Button_StopTimer.IsEnabled = true;*/
        }

        /*private void timer_Tick(object sender, EventArgs e)
        {
            TextBlock_Timer.Text = (DateTime.Now - startTime).ToString(@"hh\:mm\:ss");
        }*/

        private void Button_StopTimer_Click(object sender, RoutedEventArgs e)
        {
            _stopWatch.Stop();
            _timer.Stop();
            Button_StartTimer.IsEnabled = true;
            Button_StopTimer.IsEnabled = false;
            Button_ShowResults.IsEnabled = true;
            /*
            timer.Tick -= timer_Tick;
            timer.Stop();
            Button_StopTimer.IsEnabled = false;
            Button_StartTimer.IsEnabled = true;
            */
        }
    }
}
