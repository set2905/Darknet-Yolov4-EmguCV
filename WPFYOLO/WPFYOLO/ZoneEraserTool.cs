using DarknetYOLOv4.FrameHandler;
using Emgu.CV;
using Emgu.CV.Structure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace WPFYOLO
{
    internal class ZoneEraserTool : ZoneTool
    {
        public ZoneEraserTool()
        {

        }
        public override void SetZone(Image<Bgra, byte> imgIntruderZoneOverlay, Image FrameUserDraw)
        {
            IntruderZone.DeleteClosestZoneElement(IntruderZone.EndLocation);
            IntruderZone.ReDraw(imgIntruderZoneOverlay, FrameUserDraw);

        }
    }
}
