using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using System.Timers;
using System.Windows.Forms;

using System.Threading.Tasks;
using Emgu.CV.Dnn;
using Emgu.CV;
using Emgu.CV.Util;
using Emgu.CV.Structure;
using System.Drawing;
using System.Diagnostics;
using System.Reflection;
using DarknetYolo;
using DarknetYolo.Models;

namespace YOLOv4_TEST
{
    class Program
    {

        static void Main(string[] args)
        {
            FrameObjectDetector.StreamObjectDetect();
        }
    }
}
