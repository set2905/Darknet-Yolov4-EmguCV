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
{/// <summary>
/// Хуета и не работает нормально надо придумать что-то другое
/// </summary>
    internal class FrameMOG2KCF : FrameHandlerBase
    {
        private FrameMOG2 MOG2Handler;

        private VectorOfRect trackedObjs;

        MultiTracker _multiTracker;

        private TrackerMOSSE tracker;

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
            Console.Clear();

            Rectangle[] trackedRects = new Rectangle[0];
            List<FrameProcessResult> results = new List<FrameProcessResult>();
            List<FrameProcessResult> movingObjects = MOG2Handler.ProcessFrame(frame);


            if (trackedObjs != null)
            {
                trackedRects = _multiTracker.GetObjects();
            }

            if (movingObjects != null /*&& movingObjects.Count != trackedRects.Length*/)
            {
                foreach (FrameProcessResult moving in movingObjects)
                {
                    if (moving.Rectangle.isCloseToAnyRect(trackedRects, 500))
                    {
                        continue;
                    }
                    else
                    {
                        _multiTracker.Add(tracker, frame, moving.Rectangle);
                    }
                }
                Console.WriteLine($"Tracked: {trackedRects.Length}");
                Console.WriteLine($"Moving: {movingObjects.Count}");
            }

            _multiTracker.Update(frame, trackedObjs);
            trackedRects = _multiTracker.GetObjects();


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
                CvInvoke.Rectangle(frame, result.Rectangle, new MCvScalar(0, 255, 255), 3);
            }

            videoForm.pictureBox1.Image = frame.ToBitmap();

        }
    }
}
