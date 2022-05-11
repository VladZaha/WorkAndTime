using System;
using System.Diagnostics;
using System.Drawing;
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
        private Stopwatch _stopWatch;
        private Timer _timer;
        private DispatcherTimer _timerScreen;
        private int interval = 10;
        public MainWindow()
        {
            InitializeComponent();

            _stopWatch = new Stopwatch();
            _timer = new Timer(1000);
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
        }
        private void Button_StartTimer_Click(object sender, RoutedEventArgs e)
        {
            _stopWatch.Start();
            _timer.Start();
            _timer.Elapsed += OnTimerElapse;
            Button_StartTimer.IsEnabled = false;
            Button_StopTimer.IsEnabled = true;
            Button_ShowResults.IsEnabled = false;
            _timerScreen = new DispatcherTimer();
            _timerScreen.Interval = new TimeSpan(0,0,interval);
            _timerScreen.Tick += new EventHandler(timer_Tick);
            _timerScreen.Start();
           

        }
        private void timer_Tick(object sender, EventArgs e)
        {
            int counterScreens = 1;
            while (_timerScreen.IsEnabled)
            {
                counterScreens++;
            }
        }

        private void Button_StopTimer_Click(object sender, RoutedEventArgs e)
        {
            _stopWatch.Stop();
            _timer.Stop();
            Button_StartTimer.IsEnabled = true;
            Button_StopTimer.IsEnabled = false;
            Button_ShowResults.IsEnabled = true;
            _timerScreen.Stop();
        }
        private void ScreenShot()
        {
            double screenLeft = SystemParameters.VirtualScreenLeft;
            double screenTop = SystemParameters.VirtualScreenTop;
            double screenWidth = SystemParameters.VirtualScreenWidth;
            double screenHeight = SystemParameters.VirtualScreenHeight;


            using(Bitmap bmp = new Bitmap((int)screenWidth, (int)screenHeight))
            {
                using (Graphics g = Graphics.FromImage(bmp))
                {
                    String filename = "ScreenCapture-" + DateTime.Now.ToString("ddMMyyyy-hhmmss") + ".png";
                    Opacity = .0;
                    g.CopyFromScreen((int)screenLeft, (int)screenTop, 0, 0, bmp.Size);
                    bmp.Save(filename);
                    Opacity = 1;
                }
            }
        }
    }
}
