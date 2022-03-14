﻿using System;
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
    internal class FrameHandlerBase
    {
        private int FPS = 5;

        string labels = @"..\..\NetworkModels\coco.names";
        string weights = @"..\..\NetworkModels\yolov4-tiny.weights";
        string cfg = @"..\..\NetworkModels\yolov4-tiny.cfg";
        string video = @"https://live.cmirit.ru:443/live/smart14_1920x1080.stream/playlist.m3u8";
        VideoCapture cap;
        DarknetYOLO model;
        public bool isPlaying = false;
        public string StatusText = "";

        public void PlayFrames(Object form)
        {
            ObjectDetectorForm videoForm = (ObjectDetectorForm)form;
            cap = new VideoCapture(video);

            LoadModel(videoForm);

            isPlaying = true;
            while (isPlaying)
            {
                ProcessFrame(videoForm).Wait();
            }

        }

        public async Task ProcessFrame(ObjectDetectorForm form)
        {

            int FrameN = 0;
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
                SetStatus(form, "VideoEnded");
                frame = null;
            }
            if (frame == null)
            {
                SetStatus(form, "FrameIsNull");

                return;
            }
            Stopwatch watch = new Stopwatch();
            watch.Start();
            List<YoloPrediction> results = model.Predict(frame.ToBitmap(), 512, 512);
            watch.Stop();

            SetStatus(form, $"Frame Processing time: {watch.ElapsedMilliseconds} ms." + $"\nFPS: {Math.Ceiling(1000f / watch.ElapsedMilliseconds)}" + $"\nVideoFPS: {FPS}" + $"\nFrameNo: {FrameN}");


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

        private void SetStatus(ObjectDetectorForm form, string status)
        {
            StatusText = status;
            Console.WriteLine(StatusText);
            form.label1.Invoke(new Action(() => form.label1.Text = StatusText));
        }

        private void LoadModel(ObjectDetectorForm form)
        {
            SetStatus(form, "[INFO] Loading Model...");
            model = new DarknetYOLO(labels, weights, cfg, PreferredBackend.Cuda, PreferredTarget.Cuda);
            model.NMSThreshold = 0.4f;
            model.ConfidenceThreshold = 0.5f;
        }
    }
}