using Emgu.CV;
using Emgu.CV.Legacy;
using Emgu.CV.Structure;
using System;
using System.Collections.Generic;
using System.Drawing;
using DarknetYOLOv4.Extensions.CVExtensions;
using FrameProcessing;

namespace DarknetYOLOv4.FrameHandler
{
    internal class FrameYoloTracker : FrameHandlerBase
    {
        FrameMOG2 MovementDetector;
        FrameObjectDetectorYOLO ObjectDetector;
        List<FrameProcessResult> DetectionResults;
        public List<TrackedObject> trackedObjs = new List<TrackedObject>();
        private System.Random rnd = new System.Random();
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

        public bool isIntersectingWithAnyTracked(List<TrackedObject> tr, FrameProcessResult res, Mat frame)
        {
            foreach (TrackedObject tracked in tr)
            {
                Rectangle intersect = Rectangle.Intersect(tracked.Bbox, res.Rectangle);
                if (intersect.Area() > tracked.Bbox.Area() * 0.25)
                {
                    if (tracked.lostTrack)
                    {
                        tracked.InitTracker(res.Rectangle, frame);
                    }
                    return true;
                }
            }
            return false;
        }

        public bool isIntersectingWithAnyTracked(List<TrackedObject> tr, FrameProcessResult res, Mat frame, out int i)
        {
            foreach (TrackedObject tracked in tr)
            {
                Rectangle intersect = Rectangle.Intersect(tracked.Bbox, res.Rectangle);
                if (intersect.Area() > tracked.Bbox.Area() * 0.1)
                {
                    if (tracked.lostTrack)
                    {
                        tracked.InitTracker(res.Rectangle, frame);
                    }
                    i = tr.IndexOf(tracked);
                    return true;
                }
            }
            i = -1;
            return false;
        }

        private void GetPredictions(Mat frame)
        {
            DetectionResults = ObjectDetector.ProcessFrame(frame);
            //это вернуть когда веса будут хорошие
            List<TrackedObject> cached = new List<TrackedObject>(trackedObjs);
            RefreshTracked();
            //-----------------------------------

            foreach (FrameProcessResult res in DetectionResults)
            {
                int i;
                if (isIntersectingWithAnyTracked(cached, res, frame, out i))
                {
                    trackedObjs.Add(cached[i]);
                    continue;
                }

                if (i >= 0)
                    cached[i].Tracker.Dispose();

                TrackedObject newTracked = new TrackedObject(res.Rectangle, frame, rnd);
                newTracked.label = res.Label;
                trackedObjs.Add(newTracked);

            }
        }

        private void RefreshTracked()
        {
            /*
                        foreach (TrackedObject tracked in trackedObjs)
                        {
                            tracked.Tracker.Dispose();
                        }*/
            trackedObjs.Clear();
        }

        private bool isNewObjectMoving(Mat frame, out List<FrameProcessResult> results)
        {
            results = new List<FrameProcessResult>();
            results = MovementDetector.ProcessFrame(frame);
            if (results == null) return false;

            foreach (FrameProcessResult res in results)
            {
                if (isIntersectingWithAnyTracked(trackedObjs, res, frame))
                {
                    continue;
                }

                return true;
            }
            return false;
        }


        public override List<FrameProcessResult> ProcessFrame(Mat frame)
        {
            List<FrameProcessResult> moving;
            if (isNewObjectMoving(frame, out moving))
            {
                GetPredictions(frame);
            }


            List<FrameProcessResult> results = new List<FrameProcessResult>();

            for (int i = trackedObjs.Count - 1; i >= 0; i--)
            {
                if (trackedObjs[i].Disposed == true)
                {
                    trackedObjs.RemoveAt(i);
                    continue;
                }

                trackedObjs[i].TryUpdate(frame);
                results.Add(new FrameProcessResult(trackedObjs[i].Bbox, trackedObjs[i].label + " " + trackedObjs[i].status));
            }

            /* if (moving != null)
                 foreach (FrameProcessResult m in moving)
                 {
                     CvInvoke.Rectangle(frame, m.Rectangle, new MCvScalar(0, 0, 255), 6);
                 }*/
            return results;
        }

        protected override Mat ProcessResults(List<FrameProcessResult> results, Mat frame)
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
            return frame;
        }
    }
}

public class TrackedObject
{
    public MCvScalar color;
    public TrackerMOSSE Tracker;
    public Rectangle Bbox;
    public string label = "Unindetified";
    public string status = "normal";
    public Point[] PreviousPositions;
    public int TrailCacheSize = 20;
    private int currentTrailIndex = 0;

    public bool lostTrack = false;
    public bool Disposed = false;
    private int lostTrackLifeTime = 50;
    private int lostTrackCountDown = 0;

    public TrackedObject(Rectangle bbox, Mat frame, System.Random rnd)
    {
        Bbox = bbox;
        InitTracker(Bbox, frame);
        PreviousPositions = new Point[TrailCacheSize];

       // System.Random rnd = new System.Random();
        color = new MCvScalar(rnd.Next(0, 255), rnd.Next(0, 255), rnd.Next(0, 255));
    }

    public void InitTracker(Rectangle bbox, Mat frame)
    {
        Tracker = new TrackerMOSSE();
        Tracker.Init(frame, bbox);
        lostTrackCountDown = 0;
        lostTrack = false;
    }

    public bool TryUpdate(Mat frame)
    {
        Rectangle cachedBbox = Bbox;
        UpdateTrail();
        if (lostTrack)
        {
            status = "Lost Track: " + (lostTrackLifeTime - lostTrackCountDown).ToString();
            PerformCountDown();
            return false;
        }


        if (!Tracker.Update(frame, out Bbox))
        {
            // Tracker.Dispose();
            lostTrack = true;
            Bbox = cachedBbox;
            return false;
        }
        status = "normal";
        lostTrackCountDown = 0;
        return true;

    }

    private void PerformCountDown()
    {
        lostTrackCountDown++;
        if (lostTrackCountDown > lostTrackLifeTime)
        {
            Tracker.Dispose();
            Disposed = true;
        }
    }

    private void UpdateTrail()
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
            else
            {
                if (currentTrailIndex == 0) currentTrailIndex++;
               // status = "Not Moving: " + (lostTrackLifeTime - lostTrackCountDown).ToString();
                //PerformCountDown();

            }

        }
        else currentTrailIndex = 0;
    }

}
