
using System.Threading.Tasks;
using System;
using System.Diagnostics;
using Emgu.CV;


namespace DarknetYOLOv4.FrameHandler
{
    internal class FramePlayer : FrameHandlerBase
    {
        public override void ProcessFrame(Mat frame)
        {
            Stopwatch watch = new Stopwatch();
            watch.Start();

            CvInvoke.WaitKey(1);
            videoForm.pictureBox1.Image = frame.ToBitmap();
            watch.Stop();
            potentialFrameTime = Convert.ToInt32(watch.ElapsedMilliseconds);

            SetStatusPlayMode();
        }
    }
}
