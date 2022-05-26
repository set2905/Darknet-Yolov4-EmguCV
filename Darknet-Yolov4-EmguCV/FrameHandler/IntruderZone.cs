
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using Emgu.CV;
using Emgu.CV.Structure;

namespace DarknetYOLOv4.FrameHandler
{
    public static class IntruderZone
    {
        public static List<Rectangle> ZoneRectangles=new List<Rectangle>();
        public static System.Drawing.Point StartLocation;
        public static System.Drawing.Point EndLocation;

        public static Rectangle AddRectangle()
        {
            Rectangle _rect = new Rectangle();
            _rect.X = Math.Min(StartLocation.X, EndLocation.X);
            _rect.Y = Math.Min(StartLocation.Y, EndLocation.Y);
            _rect.Width = Math.Abs(StartLocation.X - EndLocation.X);
            _rect.Height = Math.Abs(StartLocation.Y - EndLocation.Y);
            ZoneRectangles.Add(_rect);
            return _rect;
        }

        public static bool isPointIntruder(Point point)
        {
            foreach(Rectangle rectangle in ZoneRectangles)
            {
                if (rectangle.Contains(point)) return true;
            }
            return false;
        }
    }
}
