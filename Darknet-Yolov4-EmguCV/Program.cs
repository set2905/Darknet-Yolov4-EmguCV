using System;
using System.IO;
using System.Collections.Generic;
using System.Windows.Forms;

using DarknetYOLOv4;

namespace YOLOv4_TEST
{
    class Program
    {
        [STAThread]
        static void Main()
        {
            

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new ObjectDetectorForm());

           // FrameObjectDetector.StreamObjectDetect();
        }
    }
}
