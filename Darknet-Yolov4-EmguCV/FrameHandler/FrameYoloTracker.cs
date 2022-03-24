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
using FrameProcessing;

namespace DarknetYOLOv4.FrameHandler
{
    internal class FrameYoloTracker : FrameHandlerBase
    {
        FrameMOG2 MovementDetector;
        FrameObjectDetectorYOLO ObjectDetector;
        List<FrameProcessResult> DetectionResults;
        public List<TrackedObject> trackedObjs = new List<TrackedObject>();
        public override void Initialize(Object form)
        {
            base.Initialize(form);
            MovementDetector = new FrameMOG2();
            ObjectDetector = new FrameObjectDetectorYOLO();

            MovementDetector.Initialize(form);
            ObjectDetector.Initialize(form);


        }

        private void GetPredictionOnFrame(int n, Mat frame)
        {
            if (FrameN % n == 0)
            {
                GetPredictions(frame);
            }
        }

        public bool isIntersectingWithAnyTracked(FrameProcessResult res)
        {
            foreach (TrackedObject tracked in trackedObjs)
            {
                Rectangle intersect = Rectangle.Intersect(tracked.Bbox, res.Rectangle);
                if (intersect.Area() > tracked.Bbox.Area() * 0.25)
                {
                    return true;
                }
            }
            return false;
        }

        private void GetPredictions(Mat frame)
        {
            DetectionResults = ObjectDetector.ProcessFrame(frame);
            //это вернуть когда веса будут хорошие
            /* foreach(TrackedObject tracked in trackedObjs)
             {
                 tracked.Tracker.Dispose();
             }
             trackedObjs.Clear();*/

            foreach (FrameProcessResult res in DetectionResults)
            {
                if (!isIntersectingWithAnyTracked(res))
                {
                    TrackedObject newTracked = new TrackedObject(res.Rectangle, frame);
                    newTracked.label = res.Label;
                    trackedObjs.Add(newTracked);

                }
            }
        }



        private bool isNewObjectMoving(Mat frame)
        {
            List<FrameProcessResult> results = new List<FrameProcessResult>();
            results = MovementDetector.ProcessFrame(frame);
            if (results == null) return false;

            foreach (FrameProcessResult res in results)
            {
                // CvInvoke.Rectangle(frame, res.Rectangle, new MCvScalar(0, 0, 255), 6);
                if (isIntersectingWithAnyTracked(res))
                    continue;

                return true;
            }

            return false;
        }


        public override List<FrameProcessResult> ProcessFrame(Mat frame)
        {
            // GetPredictionOnFrame(10, frame);
            if (isNewObjectMoving(frame))
            {
                GetPredictions(frame);
            }

            List<FrameProcessResult> results = new List<FrameProcessResult>();

            for (int i = trackedObjs.Count - 1; i >= 0; i--)
            {
                if (!trackedObjs[i].TryUpdate(frame))
                {
                    trackedObjs.RemoveAt(i);
                    continue;
                }

                results.Add(new FrameProcessResult(trackedObjs[i].Bbox, trackedObjs[i].label));
            }
            return results;
        }

        protected override void ProcessResults(List<FrameProcessResult> results, Mat frame)
        {
            foreach (FrameProcessResult result in results)
            {
                CvInvoke.Rectangle(frame, result.Rectangle, new MCvScalar(0, 255, 255), 3);
                CvInvoke.PutText(frame, result.Label, new Point(result.Rectangle.X, result.Rectangle.Y - 15), Emgu.CV.CvEnum.FontFace.HersheySimplex, 0.6, new MCvScalar(0, 0, 0), 2);
            }

            foreach (TrackedObject tracked in trackedObjs)
            {
                for (int i = 0; i < tracked.PreviousPositions.Length; i++)
                {
                    if (tracked.PreviousPositions[i] != null)
                        CvInvoke.Circle(frame, tracked.PreviousPositions[i], 5, tracked.color, -1);
                }
            }

            videoForm.pictureBox1.Image = frame.ToBitmap();
        }
    }
}

public class TrackedObject
{
    public MCvScalar color;
    public TrackerMOSSE Tracker;
    public Rectangle Bbox;
    public string label = "Unindetified";
    public Point[] PreviousPositions;
    public int TrailCacheSize = 25;
    private int currentTrailIndex = 0;

    public TrackedObject(Rectangle bbox, Mat frame)
    {
        Bbox = bbox;
        Tracker = new TrackerMOSSE();
        Tracker.Init(frame, Bbox);
        PreviousPositions = new Point[TrailCacheSize];

        System.Random rnd = new System.Random();
        color = new MCvScalar(rnd.Next(0, 255), rnd.Next(0, 255), rnd.Next(0, 255));
    }

    public bool TryUpdate(Mat frame)
    {

        if (currentTrailIndex < PreviousPositions.Length)
        {
            Point currentPos = Bbox.Center();
            currentPos.Y += Bbox.Height / 2;

            if (currentTrailIndex != 0 && !RectangleExtensions.isPointsClose(PreviousPositions[currentTrailIndex - 1], currentPos, 20))
            {
                PreviousPositions[currentTrailIndex] = currentPos;
                currentTrailIndex++;
            }
            if (currentTrailIndex == 0) currentTrailIndex++;

        }
        else currentTrailIndex = 0;


        if (!Tracker.Update(frame, out Bbox))
        {
            Tracker.Dispose();
            return false;
        }
        return true;
    }

}
