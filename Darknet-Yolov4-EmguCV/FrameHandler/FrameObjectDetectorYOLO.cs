using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;
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

using DarknetYOLOv4;

namespace DarknetYOLOv4.FrameHandler
{
    internal class FrameObjectDetectorYOLO : FrameHandlerBase
    {
        public virtual async Task ProcessFrame(Mat frame)
        {
            Stopwatch watch = new Stopwatch();
            watch.Start();
            List<YoloPrediction> results = model.Predict(frame.ToBitmap(), 512, 512);
            watch.Stop();

            SetStatus
                (
                $"Frame Processing time: {watch.ElapsedMilliseconds} ms."
                + $"\nFPS: {Math.Ceiling(1000f / watch.ElapsedMilliseconds)}"
                + $"\nVideoFPS: {FPS}"
                + $"\nFrameNo: {FrameN}"
                );

            foreach (var item in results)
            {
                string text = item.Label + " " + item.Confidence;
                CvInvoke.Rectangle(frame, new Rectangle(item.Rectangle.X - 2, item.Rectangle.Y - 33, item.Rectangle.Width + 4, 40), new MCvScalar(255, 0, 0), -1);
                CvInvoke.PutText(frame, text, new Point(item.Rectangle.X, item.Rectangle.Y - 15), Emgu.CV.CvEnum.FontFace.HersheySimplex, 0.6, new MCvScalar(255, 255, 255), 2);
                CvInvoke.Rectangle(frame, item.Rectangle, new MCvScalar(255, 0, 0), 3);
            }
            // CvInvoke.Imshow("test", frame);
            CvInvoke.WaitKey(1);
            videoForm.pictureBox1.Image = frame.ToBitmap();
            await Task.Delay((1000 / FPS));//1000 

        }

    }
}
