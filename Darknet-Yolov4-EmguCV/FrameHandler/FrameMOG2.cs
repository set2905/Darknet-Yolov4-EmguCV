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

        protected override void Initialize(Object form)
        {
            base.Initialize(form);
            backgroundSubtractor = new BackgroundSubtractorMOG2();
        }

        public override async Task ProcessFrame(Mat frame)
        {


            Mat resizedFrame = new Mat();
            //resizedFrame=frame;
            CvInvoke.Resize(frame, resizedFrame, ResizedProcessing);

            try
            {
                Stopwatch watch = new Stopwatch();
                watch.Start();

                Mat smoothFrame = new Mat();

                CvInvoke.GaussianBlur(resizedFrame, smoothFrame, new Size(5, 5), 1);

                Mat foregroundMask = new Mat();
                backgroundSubtractor.Apply(smoothFrame, foregroundMask);


                CvInvoke.Threshold(foregroundMask, foregroundMask, 150, 400, ThresholdType.Binary);
                 CvInvoke.MorphologyEx(foregroundMask, foregroundMask, MorphOp.Close,
                     Mat.Ones(3, 7, DepthType.Cv8U, 1), new Point(-1, -1), 1, BorderType.Reflect, new MCvScalar(0));

                int minArea = 500;
                VectorOfVectorOfPoint contours = new VectorOfVectorOfPoint();
                CvInvoke.FindContours(foregroundMask, contours, null, RetrType.External, ChainApproxMethod.ChainApproxSimple);
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

                    if (area > minArea && ar < 1.0)
                    {
                        CvInvoke.Rectangle(resizedFrame, bbox, new MCvScalar(0, 0, 255), 2);

                    }

                }
                //VectorOfVectorOfPoint allContours = new VectorOfVectorOfPoint();
                // CvInvoke.FindContours(foregroundMask, allContours, null, RetrType.Tree, ChainApproxMethod.ChainApproxSimple);
                // CvInvoke.DrawContours(resizedFrame, allContours, 0, new MCvScalar(0, 255, 0));



                videoForm.pictureBox1.Image = foregroundMask.ToBitmap();
                await Task.Delay((1000 / FPS));//1000 

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            // CvInvoke.Imshow("test", frame);
            CvInvoke.WaitKey(1);
            //videoForm.pictureBox1.Image = frame.ToBitmap();
            await Task.Delay((1000 / FPS));//1000 

        }
    }
}
