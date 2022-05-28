using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using DarknetYOLOv4.FrameHandler;
using Emgu.CV;
using Emgu.CV.Structure;
using FrameProcessing;

namespace WPFYOLO
{
    internal class ZoneRectangleTool : ZoneTool
    {
        public ZoneRectangleTool()
        {

        }
        public override void SetZone(Image<Bgra, byte> imgIntruderZoneOverlay,System.Windows.Controls.Image FrameUserDraw)
        {
            System.Drawing.Rectangle newRect = IntruderZone.AddRectangle();
            
            if (newRect != null && !newRect.IsEmpty)
            {
                /*
                imgIntruderZoneOverlay.Draw(newRect, new Bgra(255, 255, 255, 180), 4);
                Image<Bgra, byte> temp = imgIntruderZoneOverlay.CopyBlank();
                imgIntruderZoneOverlay.CopyTo(temp);
                FrameUserDraw.Source = BitmapSourceConvert.ToBitmapSource(temp.ToBitmap());*/

                IntruderZone.ReDraw(imgIntruderZoneOverlay, FrameUserDraw);
            }
        }
    }
}
