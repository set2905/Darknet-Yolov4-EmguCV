using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;
using System.Windows.Forms;

using System.Threading.Tasks;
using Emgu.CV.Dnn;
using Emgu.CV;
using Emgu.CV.Util;
using Emgu.CV.Structure;
using System.Drawing;
using System.Diagnostics;
using System.Reflection;
using DarknetYolo;




namespace DarknetYOLOv4.FrameHandler
{
    internal abstract class FrameHandlerBase
    {
        protected int FPS = 24;
        protected int FrameN = 0;
        public bool isFPSFixed;
        public int FixedFPSValue;
        private double framesToSkip = 0;
        private double spareAfterSkip=0;


        protected Size ProcessingSize = new Size(512, 512);
        protected Size OriginalSize = new Size(1920, 1080);

        protected VideoCapture cap;
        protected DarknetYOLO model;
        public bool isPlaying = false;
        public string StatusText = "";
        protected ObjectDetectorForm videoForm;

        protected int frameProcessTime = 0;
        protected int potentialFrameTime = 0;


        protected virtual void Initialize(Object form)
        {
            videoForm = (ObjectDetectorForm)form;
            cap = new VideoCapture(videoForm.currentVideo);
            // cap.SetCaptureProperty(Emgu.CV.CvEnum.CapProp.Buffersize, 3);
        }
        public void PlayFrames(Object form)
        {
            Initialize(form);


            isPlaying = true;
            while (isPlaying)
            {
                ExecuteFrame().Wait();
            }

        }

        private async Task ExecuteFrame()
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            cap.Grab();
            if (framesToSkip >= 1)
            {
                stopwatch.Stop();
                framesToSkip -= 1;
                // SetStatusPlayMode();
                return;
            }
            else await Task.Delay(TimeSpan.FromMilliseconds(spareAfterSkip));
            Mat frame = GetFrame();
            if (frame == null) return;

            ProcessFrame(frame);
            stopwatch.Stop();
            frameProcessTime = Convert.ToInt32(stopwatch.ElapsedMilliseconds);

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
                Console.WriteLine(Convert.ToString(FrameN));

            }
            catch (Exception e)
            {
                // SetStatus("VideoEnded");
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
            // Console.WriteLine(delay);

            if (delay > 0)
                return delay;
            else return 1;

        }

        public virtual void ProcessFrame(Mat frame)
        {

        }

        protected void SetStatusPlayMode()
        {
            SetStatus
           (
            $"\nVideoFPS: {FPS}"
           + $"\nFrameNo: {FrameN}"
           + $"\nFrame Execute time: { frameProcessTime}"
           + $"\nAlgorithm Execute Time: {potentialFrameTime}"
           + $"\nAwaitDelay: {GetFPSDelay()}"
           + $"\nSkipped Frames: {framesToSkip}"
           +$"\nSpare After Skip ms: {spareAfterSkip}"
           );
        }

        protected void SetStatus(string status)
        {
            StatusText = status;
            Console.WriteLine(StatusText);
            videoForm.label1.Invoke(new Action(() => videoForm.label1.Text = StatusText));
        }


    }
}
