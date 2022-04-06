
using System.Threading.Tasks;
using System;
using System.Drawing;
using System.Collections.Generic;
using System.Diagnostics;
using Emgu.CV;
using Emgu.CV.Structure;
using FrameProcessing;

namespace DarknetYOLOv4.FrameHandler
{
    internal class FramePlayer : FrameHandlerBase
    {
        public override List<FrameProcessResult> ProcessFrame(Mat frame)
        {
            CvInvoke.WaitKey(1);
           // videoForm.pictureBox1.Image = frame.ToBitmap();

            return null;
        }

        protected override Mat ProcessResults(List<FrameProcessResult> rects, Mat frame)
        {
            return frame;
        }
    }
}
