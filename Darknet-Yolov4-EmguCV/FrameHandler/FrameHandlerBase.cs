using System;
using System.IO;
using System.Collections.Generic;

using System.Threading.Tasks;
using Emgu.CV.Dnn;
using Emgu.CV;
using Emgu.CV.Util;
using Emgu.CV.Structure;
using System.Drawing;
using System.Diagnostics;
using System.Reflection;
using DarknetYolo;
using System.Threading;




namespace DarknetYOLOv4.FrameHandler
{
    internal abstract class FrameHandlerBase
    {

        private Thread _cameraThread;
        protected int FPS = 24;
        protected double currentVideoTime = 0;
        protected int FrameN = 0;
        public bool isFPSFixed;
        public int FixedFPSValue;
        private double framesToSkip = 0;
        private double spareAfterSkip = 0;


        protected Size ProcessingSize = new Size(320, 320);
        protected Size OriginalSize = new Size(1280, 720);

        protected VideoCapture cap;
        protected DarknetYOLO model;
        public bool isPlaying = false;
        public string StatusText = "";
        protected ObjectDetectorForm videoForm;

        private Mat frame;
        public bool SnapshotRequired = false;
        private string snapshotFileName = "snapshot.jpg";
        public string snapShotDirectory = @"E:\Репа\EmguCVYolov4\Darknet-Yolov4-EmguCV\bin\Debug\snapshots";

        protected int frameProcessTime = 0;
        protected int algorithmExecTime = 0;


        public virtual void Initialize(Object form)
        {
            videoForm = (ObjectDetectorForm)form;
            cap = new VideoCapture(videoForm.currentVideo);
            cap.Set(Emgu.CV.CvEnum.CapProp.Buffersize, 3);
        }

        protected void SaveSnapshot()
        {
            if (SnapshotRequired)
            {
                Image<Bgr, Byte> snapshot = frame.ToImage<Bgr, Byte>();

                string newFileName;
                for (int i = 1; true; i++)
                {
                    //this is so that you can alter the name and keep the file format 
                    newFileName = snapShotDirectory + $"\\{i}_" + snapshotFileName;
                    if (!File.Exists(newFileName))
                    {
                        break;
                    }
                }
                snapshot.Save(newFileName);
                SnapshotRequired = false;
                Console.WriteLine($"Snapshot saved: {newFileName}");
            }
        }

        public void Stop()
        {
            //так не надо наверное
            _cameraThread.Abort();
            _cameraThread = null;
        }
        public void Play(ObjectDetectorForm form)
        {
            _cameraThread = new Thread(new ParameterizedThreadStart(PlayFrames));
            _cameraThread.Start(form);
        }

        private void PlayFrames(Object form)
        {
            Initialize(form);


            isPlaying = true;
            while (isPlaying)
            {

                ExecuteFrame().Wait();
                //frame.Dispose();
            }

        }

        private async Task ExecuteFrame()
        {

            if (!cap.Grab()) return;
            Stopwatch frameProcessWatch = new Stopwatch();
            frameProcessWatch.Start();

            if (framesToSkip >= 1)
            {
                frameProcessWatch.Stop();
                framesToSkip -= 1;
                // SetStatusPlayMode();
                return;
            }
            else await Task.Delay(TimeSpan.FromMilliseconds(spareAfterSkip));
            frame = GetFrame();
            if (frame == null) return;
            if (SnapshotRequired) SaveSnapshot();

            //ProcessFrame(frame);
            Stopwatch algExecWatch = new Stopwatch();
            algExecWatch.Start();

            List<FrameProcessResult> results = ProcessFrame(frame);

            algExecWatch.Stop();
            algorithmExecTime = Convert.ToInt32(algExecWatch.ElapsedMilliseconds);


            if (results != null)
                ProcessResults(results, frame);

            frameProcessWatch.Stop();
            frameProcessTime = Convert.ToInt32(frameProcessWatch.ElapsedMilliseconds);

            //если кадр обрабатываетсмя дольше чем фреймтайм видео то пропускать фреймы
            if (FPS != 0 && frameProcessTime > (1000 / FPS))
            {
                framesToSkip = frameProcessTime / (1000 / FPS);
                spareAfterSkip = frameProcessTime % (1000 / FPS);
                framesToSkip = Math.Ceiling(framesToSkip);
            }
            else
            {
                spareAfterSkip = 0;
                framesToSkip = 0;
            }

            SetStatusPlayMode();
            await Task.Delay(GetFPSDelay());

        }

        private Mat GetFrame()
        {

            Mat frame = new Mat();
            try
            {
                cap.Retrieve(frame);

                CvInvoke.Resize(frame, frame, OriginalSize);

                if (!isFPSFixed)
                    FPS = Convert.ToInt32(cap.Get(Emgu.CV.CvEnum.CapProp.Fps));
                else FPS = FixedFPSValue;
                if (FPS <= 0 || FPS > 240) FPS = 24;
                FrameN = Convert.ToInt32(cap.Get(Emgu.CV.CvEnum.CapProp.PosFrames));

                currentVideoTime = FrameN * (1000 / FPS);

            }
            catch (Exception e)
            {
                frame = null;
            }
            if (frame == null)
            {
                SetStatus("FrameIsNull");
                return null;
            }

            return frame;
        }

        protected int GetFPSDelay()
        {
            int delay = (1000 / FPS) - frameProcessTime;


            if (delay > 0)
                return delay;
            else return 1;

        }

        public abstract List<FrameProcessResult> ProcessFrame(Mat frame);

        protected abstract void ProcessResults(List<FrameProcessResult> results, Mat frame);
        protected void SetStatusPlayMode()
        {
            SetStatus
           (
            $"\nVideoFPS: {FPS}"
           + $"\nCurrent Seconds: {GetCurrentSeconds()}"
           + $"\nFrameNo: {FrameN}"
           + $"\nFrame Execute time: { frameProcessTime}"
           + $"\nAlgorithm Execute Time: {algorithmExecTime}"
           + $"\nAwaitDelay: {GetFPSDelay()}"
           + $"\nSkipped Frames: {framesToSkip}"
           + $"\nSpare After Skip ms: {spareAfterSkip}"
           );
        }

        protected double GetCurrentSeconds()
        {
            return Math.Ceiling(currentVideoTime * 0.001f);
        }

        protected void SetStatus(string status)
        {
            StatusText = status;
            //Console.WriteLine(StatusText);
            videoForm.label1.Invoke(new Action(() => videoForm.label1.Text = StatusText));
        }


    }
}
