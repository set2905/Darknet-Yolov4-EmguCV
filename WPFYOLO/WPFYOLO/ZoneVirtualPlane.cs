using Emgu.CV;
using Emgu.CV.Structure;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using DarknetYOLOv4.FrameHandler;
namespace WPFYOLO
{
    internal class ZoneVirtualPlane : ZoneTool
    {
        private int currentIndex = 0;
        public Point[] points = new Point[4];

        public override void SetZone(Image<Bgra, byte> imgIntruderZoneOverlay, System.Windows.Controls.Image FrameUserDraw)
        {
            if (!SetNextPoint(IntruderZone.EndLocation))
            {
                IntruderZone.AddQuad(new Quad(points[0], points[1], points[2], points[3]));
                IntruderZone.TempPoints.Clear();
                IntruderZone.ReDraw(imgIntruderZoneOverlay, FrameUserDraw);
            }
            else
            {
                IntruderZone.TempPoints.Add(IntruderZone.EndLocation);
                IntruderZone.ReDraw(imgIntruderZoneOverlay, FrameUserDraw);
            }
        }

        public bool SetNextPoint(Point point)
        {
            if (currentIndex == points.Length - 1)
            {
                points[currentIndex] = point;
                currentIndex = 0;
                return false;
            }
            points[currentIndex] = point;
            currentIndex++;
            return true;
        }
    }
}
