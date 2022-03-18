
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
        public override List<FrameProcessResult> ProcessFrame(Mat frame)
        {
            CvInvoke.WaitKey(1);
            videoForm.pictureBox1.Image = frame.ToBitmap();

            return null;
        }

        protected override void ProcessResults(List<FrameProcessResult> rects, Mat frame)
        { }
    }
}
