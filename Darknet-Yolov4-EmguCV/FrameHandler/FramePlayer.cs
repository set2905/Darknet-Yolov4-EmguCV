
using System.Threading.Tasks;
using System;
using System.Drawing;
using System.Collections.Generic;
using System.Diagnostics;
using Emgu.CV;
using Emgu.CV.Structure;

namespace DarknetYOLOv4.FrameHandler
{
    internal class FramePlayer : FrameHandlerBase
    {
        public override List<Rectangle> ProcessFrame(Mat frame)
        {
            Stopwatch watch = new Stopwatch();
            watch.Start();

            CvInvoke.WaitKey(1);
            videoForm.pictureBox1.Image = frame.ToBitmap();
            watch.Stop();
            potentialFrameTime = Convert.ToInt32(watch.ElapsedMilliseconds);
            return null;
        }

        protected override void ProcessResults(List<Rectangle> rects, Mat frame)
        { }
    }
}
