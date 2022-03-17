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
using Emgu.CV.CvEnum;
using System.Drawing;
using System.Diagnostics;
using System.Reflection;


using DarknetYOLOv4;

namespace DarknetYOLOv4.FrameHandler
{
    internal class FrameMOG2 : FrameHandlerBase
    {
        IBackgroundSubtractor backgroundSubtractor;
        bool CanSnapshot = false;
        int SnapshotCDSeconds = 120;
        double lastSnapshotAt = 0;

        protected override void Initialize(Object form)
        {
            base.Initialize(form);
            backgroundSubtractor = new BackgroundSubtractorMOG2(200, 16, true);
        }

        public override List<Rectangle> ProcessFrame(Mat frame)
        {
            Mat resizedFrame = new Mat();

            CvInvoke.Resize(frame, resizedFrame, ProcessingSize);
            VectorOfVectorOfPoint contours;


            try
            {
                Stopwatch watch = new Stopwatch();
                watch.Start();
                Mat smoothFrame = new Mat();
                CvInvoke.GaussianBlur(resizedFrame, smoothFrame, new Size(7, 7), 1);

                Mat foregroundMask = new Mat();
                backgroundSubtractor.Apply(smoothFrame, foregroundMask);

                CvInvoke.Threshold(foregroundMask, foregroundMask, 150, 400, ThresholdType.Binary);
                CvInvoke.MorphologyEx(foregroundMask, foregroundMask, MorphOp.Close,
                     Mat.Ones(3, 7, DepthType.Cv8U, 1), new Point(-1, -1), 1, BorderType.Reflect, new MCvScalar(0));
                CvInvoke.Resize(foregroundMask, foregroundMask, OriginalSize);

                contours = new VectorOfVectorOfPoint();
                CvInvoke.FindContours(foregroundMask, contours, null, RetrType.External, ChainApproxMethod.ChainApproxSimple);
                watch.Stop();


                potentialFrameTime = Convert.ToInt32(watch.ElapsedMilliseconds);
                
                //-------+15ms-------
                /*
                 CvInvoke.Threshold(foregroundMask, foregroundMask, 180, 250, ThresholdType.Binary);
                 Image<Bgra, Byte> frameImg = frame.ToImage<Bgra, Byte>();
                 Image<Bgra, Byte> foregroundImg = BlackTransparent(foregroundMask.ToImage<Bgr, Byte>());
                 CvInvoke.AddWeighted(frameImg, 1f, foregroundImg, .3f, 0, frame);
                 frameImg.Dispose();
                 foregroundImg.Dispose();*/

                //------------------
                
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            //скрин
            if (!CanSnapshot)
            {
                double currentSec = GetCurrentSeconds();
                if (currentSec % SnapshotCDSeconds == 0 && lastSnapshotAt != currentSec)
                {
                    CanSnapshot = true;
                }
            }


            CvInvoke.WaitKey(1);
           return  ProcessResults(contours, frame);


        }
        private List<Rectangle> ProcessResults(VectorOfVectorOfPoint contours, Mat frame)
        {
            if (contours.Size == 0) return null;
            int minArea = 5000;
            List<Rectangle> rects = new List<Rectangle>();
            for (int i = 0; i < contours.Size; i++)
            {
                Rectangle bbox = CvInvoke.BoundingRectangle(contours[i]);
                int area = bbox.Width * bbox.Height;
                float ar = (float)bbox.Width / bbox.Height;

                if (area > minArea && ar < 1.0)
                {
                    CvInvoke.Rectangle(frame, bbox, new MCvScalar(0, 0, 255), 6);
                    string text = Convert.ToString(area);
                    //площадь объекта
                    CvInvoke.PutText(frame, text, new Point(bbox.X, bbox.Y - 15), Emgu.CV.CvEnum.FontFace.HersheySimplex, 0.6, new MCvScalar(0, 0, 0), 2);
                    rects.Add(bbox);
                }

            }

            if (contours.Size > 0 && CanSnapshot)
            {
                CanSnapshot = false;
                lastSnapshotAt = GetCurrentSeconds();
                SnapshotRequired = true;
            }
            videoForm.pictureBox1.Image = frame.ToBitmap();
            return rects;
        }

        public Image<Bgra, Byte> BlackTransparent(Image<Bgr, Byte> image)
        {
            Mat imageMat = image.Mat;
            Mat finalMat = new Mat(imageMat.Rows, imageMat.Cols, Emgu.CV.CvEnum.DepthType.Cv8U, 4);
            Mat tmp = new Mat(imageMat.Rows, imageMat.Cols, Emgu.CV.CvEnum.DepthType.Cv8U, 1);
            Mat alpha = new Mat(imageMat.Rows, imageMat.Cols, Emgu.CV.CvEnum.DepthType.Cv8U, 1);

            CvInvoke.CvtColor(imageMat, tmp, Emgu.CV.CvEnum.ColorConversion.Bgr2Gray);
            CvInvoke.Threshold(tmp, alpha, 100, 255, Emgu.CV.CvEnum.ThresholdType.Binary);


            VectorOfMat rgb = new VectorOfMat(3);

            CvInvoke.Split(imageMat, rgb);

            Mat[] rgba = { rgb[0], rgb[1], rgb[2], alpha };

            VectorOfMat vector = new VectorOfMat(rgba);

            CvInvoke.Merge(vector, finalMat);

            return finalMat.ToImage<Bgra, Byte>();
        }



        /*
        private Rectangle RemapRect(Rectangle original, Size from, Size to)
        {
            Rectangle remapped = new Rectangle();
            remapped.Width = original.Size.Width * (to.Width / from.Width);
            remapped.Height = original.Size.Height * (to.Height / from.Height);

            remapped.X = original.X * (to.Width / from.Width);
            remapped.Y = original.Y * (to.Height / from.Height);//хз почему так
            return remapped;
        }*/
    }
}
