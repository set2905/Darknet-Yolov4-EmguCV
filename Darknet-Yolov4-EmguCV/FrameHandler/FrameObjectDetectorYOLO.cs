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
using FrameProcessing;

using DarknetYOLOv4;

namespace DarknetYOLOv4.FrameHandler
{
    public class FrameObjectDetectorYOLO : FrameHandlerBase
    {

        protected string labels = @"E:\YOLOv4Network\obj.names";
        protected string weights = @"E:\YOLOv4Network\custom-yolov4-tiny-detector_best.weights";
        protected string cfg = @"E:\YOLOv4Network\custom-yolov4-tiny-detector.cfg";
        protected DarknetYOLO model;
        public override void Initialize()
        {
            base.Initialize();
            LoadModel();
        }
        public override List<FrameProcessResult> ProcessFrame(Mat frame)
        {
            List<FrameProcessResult> results = model.Predict(frame.ToBitmap(),ProcessingSize.Height, ProcessingSize.Width);
           // CvInvoke.WaitKey(1);

            
            return results;
        }

        protected override Mat ProcessResults(List<FrameProcessResult> results, Mat frame)
        {
            foreach (FrameProcessResult item in results)
            {
                string text = item.Label + " " + item.Value;
                CvInvoke.Rectangle(frame, new Rectangle(item.Rectangle.X - 2, item.Rectangle.Y - 33, item.Rectangle.Width + 4, 40), new MCvScalar(255, 0, 0), -1);
                CvInvoke.PutText(frame, text, new Point(item.Rectangle.X, item.Rectangle.Y - 15), Emgu.CV.CvEnum.FontFace.HersheySimplex, 0.6, new MCvScalar(255, 255, 255), 2);
                CvInvoke.Rectangle(frame, item.Rectangle, new MCvScalar(0, 0, 255), 3);
            }
            // videoForm.pictureBox1.Image = frame.ToBitmap();
            return frame;
        }

            private void LoadModel()
        {
            var filePath = Directory.GetCurrentDirectory();
            

            labels = Path.Combine(filePath, "NetModel\\obj.names");
            weights = Path.Combine(filePath, "NetModel\\custom-yolov4-tiny-detector_best.weights");
            cfg = Path.Combine(filePath, "NetModel\\custom-yolov4-tiny-detector.cfg");

            SetStatus("[INFO] Loading Model...");
            model = new DarknetYOLO(labels, weights, cfg, PreferredBackend.Cuda, PreferredTarget.Cuda);
            model.NMSThreshold = 0.4f;
            model.ConfidenceThreshold = 0.5f;
        }

    }
}
