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
using FrameProcessing;
using System.Windows.Controls;

using System.Windows.Media;



namespace DarknetYOLOv4.FrameHandler
{
    public abstract class FrameHandlerBase
    {

        private Thread _cameraThread;
        protected int FPS = 24;
        protected double currentVideoTime = 0;
        protected int FrameN = 0;
        public bool isFPSFixed = false;
        public int FixedFPSValue;

        private double framesToSkip = 0;
        private double spareAfterSkip = 0;


        protected Size ProcessingSize = new Size(416, 416);
        protected Size OriginalSize = new Size(1280, 720);

        protected VideoCapture cap;
        
        public bool isPlaying = false;
        public string StatusText = "";
        //protected ObjectDetectorForm videoForm;


        public string currentVideo = @"https://live.cmirit.ru:443/live/smart16_1920x1080.stream/playlist.m3u8";
        public bool SnapshotRequired = false;
        private string snapshotFileName = "snapshot.jpg";
        public string snapShotDirectory = @"E:\Репа\EmguCVYolov4\Darknet-Yolov4-EmguCV\bin\Debug\snapshots";

        protected int frameProcessTime = 0;
        protected int algorithmExecTime = 0;


        protected System.Windows.Controls.Image currentImgControl;
        protected TextBlock currentStatusControl;

        public virtual void Initialize()
        {
            cap = new VideoCapture(currentVideo);
            cap.Set(Emgu.CV.CvEnum.CapProp.Buffersize, 3);
        }

        protected void SaveSnapshot(Mat frame)
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
        public void SaveSnapshot(Mat frame, Rectangle ROI)
        {
            if (ROI.X < 0) ROI.X = 0;
            if (ROI.Y < 0) ROI.Y = 0;
            Image<Bgr, Byte> snapshot = frame.ToImage<Bgr, Byte>();
                snapshot = snapshot.Copy(ROI);

                string newFileName;
                for (int i = 1; true; i++)
                {
                    //this is so that you can alter the name and keep the file format 
                    newFileName = snapShotDirectory + $"\\ROI{i}_" + snapshotFileName;
                    if (!File.Exists(newFileName))
                    {
                        break;
                    }
                }
                snapshot.Save(newFileName);
                Console.WriteLine($"Snapshot with ROI saved: {newFileName}");

        }

        public void Stop()
        {
            //так не надо наверное
            isPlaying = false;
            _cameraThread.Abort();
            _cameraThread = null;
        }
        public void Play(System.Windows.Controls.Image controlImg, TextBlock statusTextControl)
        {
            currentImgControl = controlImg;
            currentStatusControl = statusTextControl;
            CvInvoke.DestroyAllWindows();
            isPlaying = true;
            _cameraThread = new Thread(new ThreadStart(PlayFrames));
            _cameraThread.Start();
        }

        private void PlayFrames()
        {
            Initialize();


            isPlaying = true;
            while (isPlaying)
            {

                ExecuteFrame().Wait();
                // frame.Dispose();
            }

        }

        private async Task ExecuteFrame()
        {
            Mat currentFrame;
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

            else
            {
                await Task.Delay(TimeSpan.FromMilliseconds(spareAfterSkip));
            }

            currentFrame = GetFrame();
            if (currentFrame == null) return;
            if (SnapshotRequired) SaveSnapshot(currentFrame);

            //ProcessFrame(frame);
            Stopwatch algExecWatch = new Stopwatch();
            algExecWatch.Start();

            List<FrameProcessResult> results = ProcessFrame(currentFrame);

            algExecWatch.Stop();
            algorithmExecTime = Convert.ToInt32(algExecWatch.ElapsedMilliseconds);


            if (results != null && results.Count > 0)
                currentFrame = ProcessResults(results, currentFrame);

            ShowFrame(currentFrame);

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
            GC.Collect();

        }
        private void ShowFrame(Mat _frame)
        {

            Image<Bgr, Byte> img = _frame.ToImage<Bgr, Byte>();

            System.Drawing.Bitmap bm = img.ToBitmap();
            //  currentImgControl.Source = BitmapSourceConvert.ToBitmapSource(bm);

            currentImgControl.Dispatcher.Invoke(new Action(() => { currentImgControl.Source = BitmapSourceConvert.ToBitmapSource(bm); }));
            //CvInvoke.Imshow("1", _frame);
            // CvInvoke.WaitKey(1);
        }

        protected Mat GetFrame()
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

        protected abstract Mat ProcessResults(List<FrameProcessResult> results, Mat frame);
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
            // videoForm.label1.Invoke(new Action(() => videoForm.label1.Text = StatusText));

            if (currentStatusControl != null)
                currentStatusControl.Dispatcher.Invoke(new Action(() => { currentStatusControl.Text = StatusText; }));
        }


    }
}
