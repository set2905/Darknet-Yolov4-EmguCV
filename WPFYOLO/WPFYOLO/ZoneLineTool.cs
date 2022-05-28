using DarknetYOLOv4.FrameHandler;
using Emgu.CV;
using Emgu.CV.Structure;
using FrameProcessing;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace WPFYOLO
{
    internal class ZoneLineTool : ZoneTool
    {
        public override void SetZone(Image<Bgra, byte> imgIntruderZoneOverlay, System.Windows.Controls.Image FrameUserDraw)
        {
            IntruderZone.AddLine();

            imgIntruderZoneOverlay.DrawPolyline(new Point[2] { IntruderZone.StartLocation,IntruderZone.EndLocation },false, new Bgra(255, 0, 0, 180), 4);
            Image<Bgra, byte> temp = imgIntruderZoneOverlay.CopyBlank();
            imgIntruderZoneOverlay.CopyTo(temp);
            FrameUserDraw.Source = BitmapSourceConvert.ToBitmapSource(temp.ToBitmap());

            IntruderZone.ReDraw(imgIntruderZoneOverlay, FrameUserDraw);
        }
    }


}
