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




namespace DarknetYOLOv4.FrameHandler
{
    internal abstract class FrameHandlerBase
    {
        protected int FPS = 5;
        protected int FrameN = 0;

        protected string video = @"https://live.cmirit.ru:443/live/smart14_1920x1080.stream/playlist.m3u8";
        protected Size ResizedProcessing = new Size(512, 512);


        protected VideoCapture cap;
        protected DarknetYOLO model;
        public bool isPlaying = false;
        public string StatusText = "";
        protected ObjectDetectorForm videoForm;

        protected virtual void Initialize(Object form)
        {
            videoForm = (ObjectDetectorForm)form;
            cap = new VideoCapture(video);
        }
        public void PlayFrames(Object form)
        {
            Initialize(form);
            

            isPlaying = true;
            while (isPlaying)
            {
                ProcessFrame(GetFrame()).Wait();
            }

        }

        private Mat GetFrame()
        {
            
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
                SetStatus("VideoEnded");
                frame = null;
            }
            if (frame == null)
            {
                SetStatus("FrameIsNull");

                return null;
            }

            return frame;
        }

        public virtual async Task ProcessFrame(Mat frame)
        {
            await Task.Delay((1000 / FPS));//1000 
        }

        protected void SetStatus(string status)
        {
            StatusText = status;
            Console.WriteLine(StatusText);
            videoForm.label1.Invoke(new Action(() => videoForm.label1.Text = StatusText));
        }


    }
}
