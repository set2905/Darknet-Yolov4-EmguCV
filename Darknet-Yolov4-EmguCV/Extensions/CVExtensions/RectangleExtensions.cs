using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace DarknetYOLOv4.Extensions.CVExtensions
{
    internal static class RectangleExtensions
    {
        public static double Area(this Rectangle rect)
        {
            return rect.Width * rect.Height;
        }

        public static bool isCloseToAnyRect(this Rectangle original, Rectangle[] rects, double dist)
        {
            if (rects == null) return false;
            
            foreach(Rectangle rect in rects)
            {
                if (isPointsClose(original.Center(), rect.Center(), dist)) return true;

            }
            return false;

        }
        public static bool isPointsClose(Point p1, Point p2, double dist)
        {
            double distance = GetDistance(p1,p2);
            if (distance <= dist) return true;

            return false;

        }

        public static Point Center(this Rectangle rect)
        {
            return new Point(rect.Left + rect.Width / 2,
                             rect.Top + rect.Height / 2);
        }

        public static double GetDistance(Point p1, Point p2)
        {
            return Math.Sqrt(Math.Pow((p2.X - p1.X), 2) + Math.Pow((p2.Y - p1.Y), 2));
        }

      /*  public static bool isPointInsideRectangle(Point point, Rectangle rect)
        {
            return rect.Contains(point);
        }*/




    }
}
