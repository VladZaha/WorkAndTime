﻿using Effort;
using System;
using System.Diagnostics;
using System.Linq;
using System.Timers;
using System.Windows;

namespace WorkAndTime
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Stopwatch _stopWatch;
        private Timer _timer;
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
        }

        private void Button_StopTimer_Click(object sender, RoutedEventArgs e)
        {
            _stopWatch.Stop();
            _timer.Stop();
            Button_StartTimer.IsEnabled = true;
            Button_StopTimer.IsEnabled = false;
            Button_ShowResults.IsEnabled = true;
        }

    }
}
