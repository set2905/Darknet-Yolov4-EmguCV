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
using DarknetYolo.Models;

using DarknetYOLOv4;



namespace YOLOv4_TEST
{
    internal class FrameObjectDetector
    {
        private static int FPS = 5;

        static string labels = @"..\..\NetworkModels\coco.names";
        static string weights = @"..\..\NetworkModels\yolov4-tiny.weights";
        static string cfg = @"..\..\NetworkModels\yolov4-tiny.cfg";
        static string video = @"https://live.cmirit.ru:443/live/axis4_1920x1080.stream/playlist.m3u8";
        static VideoCapture cap;
        static DarknetYOLO model;
        static System.Timers.Timer FrameTicker;



        public  void StreamObjectDetect(Object form)
        {
            FrameTicker = new System.Timers.Timer();
            cap = new VideoCapture(video);


            Console.WriteLine("[INFO] Loading Model...");
            model = new DarknetYOLO(labels, weights, cfg, PreferredBackend.Cuda, PreferredTarget.Cuda);
            model.NMSThreshold = 0.4f;
            model.ConfidenceThreshold = 0.5f;

            while (true)
            {
                ProcessFrame((ObjectDetectorForm)form).Wait();
            }

        }

        public  async Task ProcessFrame(ObjectDetectorForm form)
        {
            int FrameN;
            Mat frame = new Mat();
            try
            {

                cap.Read(frame);

                CvInvoke.Resize(frame, frame, new Size(1280, 768));

                FPS = Convert.ToInt32(cap.GetCaptureProperty(Emgu.CV.CvEnum.CapProp.Fps));
                FrameN = Convert.ToInt32(cap.GetCaptureProperty(Emgu.CV.CvEnum.CapProp.PosFrames));


                Console.WriteLine(Convert.ToString(FrameN));
            }
            catch (Exception e)
            {
                Console.WriteLine("VideoEnded");
                frame = null;
            }
            if (frame == null)
            {
                Console.WriteLine("FrameIsNull");
                return;
            }
            Stopwatch watch = new Stopwatch();
            watch.Start();
            List<YoloPrediction> results = model.Predict(frame.ToBitmap(), 512, 512);
            watch.Stop();


            Console.WriteLine($"Frame Processing time: {watch.ElapsedMilliseconds} ms." + $" FPS: {1000f / watch.ElapsedMilliseconds}" + $" VideoFPS: {FPS}");

            form.label1.Invoke(new Action(() => form.label1.Text = $"Frame Processing time: {watch.ElapsedMilliseconds} ms." + $" FPS: {1000f / watch.ElapsedMilliseconds}" + $" VideoFPS: {FPS}"));
           // output.StatusText.Text = $"Frame Processing time: {watch.ElapsedMilliseconds} ms." + $"Processing FPS: {1000f / watch.ElapsedMilliseconds}" + $" VideoFPS: {FPS}";
            foreach (var item in results)
            {
                string text = item.Label + " " + item.Confidence;
                CvInvoke.Rectangle(frame, new Rectangle(item.Rectangle.X - 2, item.Rectangle.Y - 33, item.Rectangle.Width + 4, 40), new MCvScalar(255, 0, 0), -1);
                CvInvoke.PutText(frame, text, new Point(item.Rectangle.X, item.Rectangle.Y - 15), Emgu.CV.CvEnum.FontFace.HersheySimplex, 0.6, new MCvScalar(255, 255, 255), 2);
                CvInvoke.Rectangle(frame, item.Rectangle, new MCvScalar(255, 0, 0), 3);
            }
           // CvInvoke.Imshow("test", frame);
            CvInvoke.WaitKey(1);

            form.pictureBox1.Image = frame.ToBitmap();
            await Task.Delay((1000 / FPS));//1000 

        }
    }
}
