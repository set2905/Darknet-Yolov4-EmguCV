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
using DarknetYolo;

namespace DarknetYOLOv4.FrameHandler
{
    internal class YOLOKCFtest : FrameHandlerBase
    {
        FrameObjectDetectorYOLO yoloDetector;
        List<FrameProcessResult> yoloInitResults;
        public List<TrackedObject> trackedObjs = new List<TrackedObject>();
        public override void Initialize(Object form)
        {
            base.Initialize(form);
            yoloDetector = new FrameObjectDetectorYOLO();
            yoloDetector.Initialize(form);


        }

        private void GetPredictionOnFrame(int n, Mat frame)
        {
            if (FrameN % n == 0)
            {
                // Mat initFrame = GetFrame();
                yoloInitResults = yoloDetector.ProcessFrame(frame);

                foreach (FrameProcessResult res in yoloInitResults)
                {
                    foreach (TrackedObject tracked in trackedObjs)
                    {
                        if (res.Rectangle.IntersectsWith(tracked.Bbox))
                        {
                            // tracked.Tracker.Init(frame, res.Rectangle);
                            return;
                        }
                    }
                    trackedObjs.Add(new TrackedObject(res.Rectangle, frame));

                }
                Console.WriteLine($"trackedObjs:{trackedObjs.Count}");
            }
        }
        public override List<FrameProcessResult> ProcessFrame(Mat frame)
        {
            GetPredictionOnFrame(50, frame);
            List<FrameProcessResult> results = new List<FrameProcessResult>();

            for (int i = trackedObjs.Count - 1; i >= 0; i--)
            {
                if (!trackedObjs[i].TryUpdate(frame))
                {
                    trackedObjs.RemoveAt(i);
                    continue;
                }

                results.Add(new FrameProcessResult(trackedObjs[i].Bbox, i.ToString()));
            }
            /*  foreach (TrackedObject tracked in trackedObjs)
          {
              // tracked.Tracker.Update(frame, out tracked.Bbox);
              if (!tracked.TryUpdate(frame))
              {
                  trackedObjs.Remove(tracked);
                  continue;
              }

              results.Add(new FrameProcessResult(tracked.Bbox));
          }*/
            return results;
        }

        protected override void ProcessResults(List<FrameProcessResult> results, Mat frame)
        {
            foreach (FrameProcessResult result in results)
            {
                CvInvoke.Rectangle(frame, result.Rectangle, new MCvScalar(0, 255, 255), 3);
                CvInvoke.PutText(frame, result.Label, new Point(result.Rectangle.X, result.Rectangle.Y - 15), Emgu.CV.CvEnum.FontFace.HersheySimplex, 0.6, new MCvScalar(255, 255, 255), 2);
            }

            videoForm.pictureBox1.Image = frame.ToBitmap();
        }

    }
}

public class TrackedObject
{
    public TrackerKCF Tracker;
    public Rectangle Bbox;

    public TrackedObject(Rectangle bbox, Mat frame)
    {
        Bbox = bbox;
        Tracker = new TrackerKCF();
        Tracker.Init(frame, Bbox);
    }

    public bool TryUpdate(Mat frame)
    {
        if (!Tracker.Update(frame, out Bbox))
        {
            Tracker.Dispose();
            return false;
        }
        return true;
    }

}
