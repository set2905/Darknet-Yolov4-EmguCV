using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
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


        // Given three collinear points p, q, r, the function checks if
        // point q lies on line segment 'pr'
        public static bool Contains(this Line pr, Point q)
        {
            if (q.X <= Math.Max(pr.first.X, pr.last.X) && q.X >= Math.Min(pr.first.X, pr.last.X) &&
                q.Y <= Math.Max(pr.first.Y, pr.last.Y) && q.Y >= Math.Min(pr.first.Y, pr.last.Y))
                return true;

            return false;
        }

        // To find orientation of ordered triplet (p, q, r).
        // The function returns following values
        // 0 --> p, q and r are collinear
        // 1 --> Clockwise
        // 2 --> Counterclockwise
       private static int GetOrientation(this Line pr, Point q)
        {
            Point p = pr.first;
            Point r = pr.last;
            // See https://www.geeksforgeeks.org/orientation-3-ordered-points/
            // for details of below formula.
            int val = (q.Y - p.Y) * (r.X - q.X) -
                    (q.X - p.X) * (r.X - q.X);

            if (val == 0) return 0; // collinear

            return (val > 0) ? 1 : 2; // clock or counterclock wise
        }

        public static bool IsIntersecting(this Line pq1, Line pq2)
        {
            Point p1 = pq1.first;
            Point p2 = pq2.first;
            Point q1 = pq1.last;
            Point q2 = pq2.last;
            // Find the four orientations needed for general and
            // special cases
            int o1 = pq1.GetOrientation(p2);
            int o2 = pq1.GetOrientation(q2);
            int o3 = pq2.GetOrientation(p1);
            int o4 = pq2.GetOrientation(q1);

            // General case
            if (o1 != o2 && o3 != o4)
                return true;

            // Special Cases
            // p1, q1 and p2 are collinear and p2 lies on segment p1q1
            if (o1 == 0 && pq1.Contains(p2)) return true;

            // p1, q1 and q2 are collinear and q2 lies on segment p1q1
            if (o2 == 0 && pq1.Contains(q2)) return true;

            // p2, q2 and p1 are collinear and p1 lies on segment p2q2
            if (o3 == 0 && pq2.Contains(p1)) return true;

            // p2, q2 and q1 are collinear and q1 lies on segment p2q2
            if (o4 == 0 && pq2.Contains(q1)) return true;

            return false; // Doesn't fall in any of the above cases
        }


    }
}
