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
    internal class YOLOKCFtest : FrameHandlerBase
    {
        FrameObjectDetectorYOLO yoloDetector;
        List<FrameProcessResult> yoloInitResults;
        public List<TrackedObject> trackedObjs=new List<TrackedObject>();
        public override void Initialize(Object form)
        {
            base.Initialize(form);
            yoloDetector = new FrameObjectDetectorYOLO();
            yoloDetector.Initialize(form);


        }

        private void GetPredictionOnFrame(int n, Mat initFrame)
        {
            if (FrameN == n)
            {
               // Mat initFrame = GetFrame();
                yoloInitResults = yoloDetector.ProcessFrame(initFrame);

                foreach (FrameProcessResult res in yoloInitResults)
                {
                    trackedObjs.Add(new TrackedObject(res.Rectangle, initFrame));
                }
            }
        }
        public override List<FrameProcessResult> ProcessFrame(Mat frame)
        {
            GetPredictionOnFrame(10,frame);
            List<FrameProcessResult> results = new List<FrameProcessResult>();
            foreach (TrackedObject tracked in trackedObjs)
            {
                tracked.Tracker.Update(frame, out tracked.Bbox);
                results.Add(new FrameProcessResult(tracked.Bbox));
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

}
