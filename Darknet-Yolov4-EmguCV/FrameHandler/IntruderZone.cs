
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
        public static List<Line> Lines = new List<Line>();
        public static List<Rectangle> ZoneRectangles=new List<Rectangle>();
        public static System.Drawing.Point StartLocation;
        public static System.Drawing.Point EndLocation;


        public static void AddLine()
        {
            Line line = new Line(StartLocation, EndLocation);
            Lines.Add(line);
        }


        public static Rectangle AddRectangle()
        {
            Rectangle _rect = new Rectangle();
            _rect.X = Math.Min(StartLocation.X, EndLocation.X);
            _rect.Y = Math.Min(StartLocation.Y, EndLocation.Y);
            _rect.Width = Math.Abs(StartLocation.X - EndLocation.X);
            _rect.Height = Math.Abs(StartLocation.Y - EndLocation.Y);

            if (_rect.Width * _rect.Height < 5000) return Rectangle.Empty;
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

        public static void Clear()
        {
            ZoneRectangles.Clear();
            Lines.Clear();
        }
    }

    public class Line
    {
        public Point first;
        public Point last;

        public Line(Point first, Point last)
        {
            this.first = first;
            this.last = last;
        }

    }


}
