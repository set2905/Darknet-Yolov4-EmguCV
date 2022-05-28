using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Diagnostics;
using DarknetYOLOv4.FrameHandler;

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

        //Govno
        public static bool IsIntersecting(this Line p12, Line p34)
        {
            bool isIntersecting = false;

            Point p1 = p12.first;
            Point p2 = p12.last;
            Point p3 = p34.first;
            Point p4 = p34.last;


            double denominator = (p4.Y - p3.Y) * (p2.X - p1.X) - (p4.X - p3.X) * (p2.Y - p1.Y);

            //Make sure the denominator is > 0, if so the lines are parallel
            if (denominator != 0)
            {
                double u_a = ((p4.X - p3.X) * (p1.Y - p3.Y) - (p4.Y - p3.Y) * (p1.X - p3.X)) / denominator;
                double u_b = ((p2.X - p1.X) * (p1.Y - p3.Y) - (p2.Y - p1.Y) * (p1.X - p3.X)) / denominator;

                //Is intersecting if u_a and u_b are between 0 and 1
                if (u_a >= 0 && u_a <= 1 && u_b >= 0 && u_b <= 1)
                {
                   // Trace.WriteLine(p1 + " " + p2 + " " + p3 + " " + p4 + " "+denominator);
                    isIntersecting = true;
                }
            }

            return isIntersecting;
        }


    }
}
