using Emgu.CV;
using Emgu.CV.Legacy;
using Emgu.CV.Util;
using Emgu.CV.Structure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using DarknetYOLOv4.Extensions.CVExtensions;

namespace DarknetYOLOv4.FrameHandler
{
    internal class FrameMOG2KCF : FrameHandlerBase
    {
        private FrameMOG2 MOG2Handler;

        private VectorOfRect trackedObjs;

        MultiTracker _multiTracker;

        TrackerMOSSE tracker;

        public override void Initialize(object form)
        {
            base.Initialize(form);
            trackedObjs = new VectorOfRect();
            MOG2Handler = new FrameMOG2();
            MOG2Handler.Initialize(form);
            _multiTracker = new MultiTracker();
            tracker = new TrackerMOSSE();
        }
        public override List<FrameProcessResult> ProcessFrame(Mat frame)
        {
            List<FrameProcessResult> results = new List<FrameProcessResult>();
            List<FrameProcessResult> movingObjects = MOG2Handler.ProcessFrame(frame);
            Rectangle[] trackedRects = new Rectangle[0];

            if (trackedObjs != null)
                trackedRects = trackedObjs.ToArray();

            if (movingObjects != null)
                foreach (FrameProcessResult moving in movingObjects)
                {
                    if (moving.Rectangle.isCloseToAnyRect(trackedRects, 1000))
                        continue;
                    else
                    {
                        _multiTracker.Add(tracker, frame, moving.Rectangle);
                    }
                }

            _multiTracker.Update(frame, trackedObjs);
            trackedRects = trackedObjs.ToArray();

            foreach (Rectangle tracked in trackedRects)
            {
                results.Add(new FrameProcessResult(tracked));
            }

            return results;
        }


        protected override void ProcessResults(List<FrameProcessResult> results, Mat frame)
        {
            foreach (FrameProcessResult result in results)
            {
                CvInvoke.Rectangle(frame, result.Rectangle, new MCvScalar(0, 0, 255), 6);
            }

            videoForm.pictureBox1.Image = frame.ToBitmap();

        }
    }
}
