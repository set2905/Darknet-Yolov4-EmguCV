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
    internal class FrameKNN : FrameHandlerBase
    {
        IBackgroundSubtractor backgroundSubtractor;

        protected override void Initialize(Object form)
        {
            base.Initialize(form);
            backgroundSubtractor = new BackgroundSubtractorKNN(1500, 16, true);
        }

        public override async Task ProcessFrame(Mat frame)
        {


            Mat resizedFrame = new Mat();
            //resizedFrame=frame;
            CvInvoke.Resize(frame, resizedFrame, ProcessingSize);

            try
            {
                Stopwatch watch = new Stopwatch();
                watch.Start();

                Mat smoothFrame = new Mat();

                CvInvoke.GaussianBlur(resizedFrame, smoothFrame, new Size(9, 9), 4);

                Mat foregroundMask = new Mat();
                backgroundSubtractor.Apply(smoothFrame, foregroundMask);


                CvInvoke.Threshold(foregroundMask, foregroundMask, 150, 400, ThresholdType.Binary);
                // CvInvoke.MorphologyEx(foregroundMask, foregroundMask, MorphOp.Close,
                //    Mat.Ones(3, 7, DepthType.Cv8U, 1), new Point(-1, -1), 1, BorderType.Reflect, new MCvScalar(0));

                int minArea = 250;
                VectorOfVectorOfPoint contours = new VectorOfVectorOfPoint();
                CvInvoke.FindContours(foregroundMask, contours, null, RetrType.External, ChainApproxMethod.ChainApproxSimple);

                // CvInvoke.AddWeighted(frame,.5f,foregroundMask,.1f,0,frame);
              //  frame = frame | foregroundMask;

                watch.Stop();


                SetStatus
                        (
                        $"Frame Processing time: {watch.ElapsedMilliseconds} ms."
                        + $"\nFPS: {Math.Ceiling(1000f / watch.ElapsedMilliseconds)}"
                        + $"\nVideoFPS: {FPS}"
                        + $"\nFrameNo: {FrameN}"
                        );

                for (int i = 0; i < contours.Size; i++)
                {
                    Rectangle bbox = CvInvoke.BoundingRectangle(contours[i]);
                    int area = bbox.Width * bbox.Height;
                    float ar = (float)bbox.Width / bbox.Height;

                    if (area > minArea /*&& ar < 1.0*/)
                    {
                        CvInvoke.Rectangle(frame, RemapRect(bbox, ProcessingSize, OriginalSize), new MCvScalar(0, 0, 255), 2);

                    }

                }


                videoForm.pictureBox1.Image = frame.ToBitmap();
                await Task.Delay((1000 / FPS));//1000 

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            // CvInvoke.Imshow("test", frame);
            CvInvoke.WaitKey(1);
            await Task.Delay((1000 / FPS));//1000 

        }

        private Rectangle RemapRect(Rectangle original, Size from, Size to)
        {
            Rectangle remapped = new Rectangle();
            remapped.Width = original.Size.Width * (to.Width / from.Width);
            remapped.Height = original.Size.Height * (to.Height / from.Height);

            remapped.X = original.X * (to.Width / from.Width);
            remapped.Y = original.Y * (to.Height / from.Height)+ original.Y/2;//хз почему так
            return remapped;
        }
    }
}
