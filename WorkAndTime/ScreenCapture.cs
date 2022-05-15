using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace WorkAndTime
{
    public class ScreenCapture
    {
        int interval { get; set; }
        
        public ScreenCapture() // constructor
        {
            var window = Application.Current.Windows.OfType<MainWindow>().First(); // to access controls
  
            //if (!window.menuNever.IsPressed)
            //{                
            //    if (window.menu30.IsPressed)
            //    {
            //        interval = 30;
            //    }
            //    if (window.menu10.IsPressed)
            //    {
            //        interval = 10;
            //    }
            //    if (window.menu5.IsPressed)
            //    {
            //        interval = 5;
            //    }
            //} 
            //else interval = 0;
        }

        public void ManageCaptures()
        {
            
            if (interval != 0)
            {
                var window = Application.Current.Windows.OfType<MainWindow>().First(); // to access controls
                // create folder for screen captures, if it does not exist
                System.IO.Directory.CreateDirectory(@"c:\WorkAndTime\ScreenCaptures\" + window.ListBox_Projects.SelectedItem.ToString());
                // create timer
                DispatcherTimer dtClockTime = new DispatcherTimer();

                dtClockTime.Interval = new TimeSpan(0, interval, 0); //in Hours, Minutes, Seconds
                dtClockTime.Tick += dtClockTime_Tick;

                dtClockTime.Start();
            }

        }

            private void dtClockTime_Tick(object sender, EventArgs e)
            {
                PrintScreen();
            }

            // makes screen capture and save it to the folder 'ScreenCaptures'
            private void PrintScreen()
            {
                double screenLeft = SystemParameters.VirtualScreenLeft;
                double screenTop = SystemParameters.VirtualScreenTop;
                double screenWidth = SystemParameters.VirtualScreenWidth;
                double screenHeight = SystemParameters.VirtualScreenHeight;

                var window = Application.Current.Windows.OfType<MainWindow>().First(); // to access controls

                using (Bitmap bmp = new Bitmap((int)screenWidth, (int)screenHeight)) {
                        using (Graphics g = Graphics.FromImage(bmp))
                        {
                            String filename = DateTime.Now.ToString("ddMMyyyy-hhmmss") + ".png";
                            g.CopyFromScreen((int)screenLeft, (int)screenTop, 0, 0, bmp.Size);
                            bmp.Save("C:\\WorkAndTime\\ScreenCaptures\\" + window.ListBox_Projects.SelectedItem.ToString() + "\\" + filename);                
                        }
                }
            } 
    }
}
