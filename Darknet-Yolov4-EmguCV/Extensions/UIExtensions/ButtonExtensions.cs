using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using Emgu.CV;
using Emgu.CV.Structure;
using System.Windows.Forms;
namespace DarknetYOLOv4.UIExtensions
{
    public static class ButtonExtensions
    {
        public static void SetCover(this Button b, string VideoPath)
        {
            VideoCapture CoverCapture;
            CoverCapture = new VideoCapture(VideoPath);


            
            Mat frame = new Mat();
            CoverCapture.Grab();

            CoverCapture.Retrieve(frame);
            if (frame.GetData() == null) return;
            CvInvoke.Resize(frame, frame, new Size(b.Width, b.Height));
            Image<Bgr, Byte> img = frame.ToImage<Bgr, Byte>();

            b.Image = img.ToBitmap();

            img.Dispose();
            CoverCapture.Dispose();
        }

    }
}
