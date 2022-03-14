
using System.Threading.Tasks;

using Emgu.CV;


namespace DarknetYOLOv4.FrameHandler
{
    internal class FramePlayer : FrameHandlerBase
    {
        public override async Task ProcessFrame(Mat frame)
        {
            SetStatus
                (
                 $"\nVideoFPS: {FPS}"
                + $"\nFrameNo: {FrameN}"
                );

            // CvInvoke.Imshow("test", frame);

            videoForm.pictureBox1.Image = frame.ToBitmap();
            await Task.Delay((1000 / FPS));//1000 
        }
    }
}
