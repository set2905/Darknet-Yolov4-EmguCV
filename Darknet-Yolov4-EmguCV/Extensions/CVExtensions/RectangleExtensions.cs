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

        public static double GetDistanceToRect(this Rectangle rect, Point p)
        {
            int dx = Math.Max(rect.Location.X - p.X, p.X - rect.Location.X);
            var dy = Math.Max(rect.Location.Y-rect.Height - p.Y, p.Y - rect.Location.Y-rect.Width);
            return Math.Sqrt(dx * dx + dy * dy);
        }

        public static double GetDistance(Point p1, Point p2)
        {
            return Math.Sqrt(Math.Pow((p2.X - p1.X), 2) + Math.Pow((p2.Y - p1.Y), 2));
        }


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

        public static double GetTriangleArea(Point p1, Point p2, Point p3)
        {
            double side1 = GetDistance(p1,p2);
            double side2 = GetDistance(p2, p3);
            double side3 = GetDistance(p3, p1);

            double semiperimeter = (side1 + side2 + side3) / 2;
            double Area = Math.Sqrt(semiperimeter * (semiperimeter - side1) * (semiperimeter - side2) * (semiperimeter - side3));
            return Area;
        }

        public static double GetDistanceToLine(this Line line, Point point)
        {
            int x = point.X;
            int y = point.Y;
            int x1 = line.first.X;
            int y1 = line.first.Y;
            int x2 = line.last.X;
            int y2 = line.last.Y;

            int A = x - x1;
            int B = y - y1;
            int C = x2 - x1;
            int D = y2 - y1;

            int dot = A * C + B * D;
            int len_sq = C * C + D * D;
            int param = -1;
            if (len_sq != 0) //in case of 0 length line
                param = dot / len_sq;

            int xx, yy;

            if (param < 0)
            {
                xx = x1;
                yy = y1;
            }
            else if (param > 1)
            {
                xx = x2;
                yy = y2;
            }
            else
            {
                xx = x1 + param * C;
                yy = y1 + param * D;
            }

            int dx = x - xx;
            int dy = y - yy;
            return Math.Sqrt(dx * dx + dy * dy);
        }


    }
}
