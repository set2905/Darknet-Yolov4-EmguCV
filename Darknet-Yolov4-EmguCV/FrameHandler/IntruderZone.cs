
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

using Emgu.CV;
using Emgu.CV.Structure;
using DarknetYOLOv4.Extensions.CVExtensions;
using FrameProcessing;

namespace DarknetYOLOv4.FrameHandler
{
    public static class IntruderZone
    {
        public static List<Point> TempPoints = new List<Point>();
        public static List<Quad> Quads = new List<Quad>();
        public static List<Line> Lines = new List<Line>();
        public static List<Rectangle> ZoneRectangles = new List<Rectangle>();
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

        public static void AddQuad(Quad quad)
        {
            Quads.Add(quad);
        }

        public static bool isObjectIntruder(Point currentPosition, Point firstPosition)
        {
            foreach (Rectangle rectangle in ZoneRectangles)
            {
                if (rectangle.Contains(currentPosition)) return true;
            }

            if (currentPosition != firstPosition)
            {
                Line moveSegment = new Line(currentPosition, firstPosition);
                foreach (Line line in Lines)
                {
                    if (line.IsIntersecting(moveSegment)) return true;
                }
            }

           foreach(Quad quad in Quads)
            {
                if (quad.isPointInside(currentPosition)) return true;
            }


            return false;
        }

        public static void ReDraw(Image<Bgra, byte> imgIntruderZoneOverlay, System.Windows.Controls.Image FrameUserDraw)
        {
            imgIntruderZoneOverlay = imgIntruderZoneOverlay.CopyBlank();
            // FrameUserDraw.Source = BitmapSourceConvert.ToBitmapSource(imgIntruderZoneOverlay.ToBitmap());

            foreach (Rectangle rect in ZoneRectangles)
            {
                imgIntruderZoneOverlay.Draw(rect, new Bgra(255, 255, 255, 180), 4);
            }
            foreach (Line ln in Lines)
            {
                imgIntruderZoneOverlay.DrawPolyline(new Point[2] { ln.first, ln.last }, false, new Bgra(255, 0, 0, 180), 4);
            }
            foreach (Quad quad in Quads)
            {
                imgIntruderZoneOverlay.DrawPolyline(quad.points, true, new Bgra(255, 0, 100, 150), 4);
            }
            foreach (Point point in TempPoints)
            {
                CvInvoke.Circle(imgIntruderZoneOverlay, point, 3, new MCvScalar(255, 0, 255, 255), -1);
            }
            Image<Bgra, byte> temp = imgIntruderZoneOverlay.CopyBlank();
            imgIntruderZoneOverlay.CopyTo(temp);
            FrameUserDraw.Source = BitmapSourceConvert.ToBitmapSource(temp.ToBitmap());
        }

        public static void DeleteClosestZoneElement(Point location)
        {
            double closestDistance = double.MaxValue;
            //Point closestPoint = Point.Empty;
            Object closest = null;

            foreach (Line line in Lines)
            {
                double dist = line.GetDistanceToLine(location);
                if (dist < closestDistance)
                {
                    closest = line;
                    closestDistance = dist;
                }
            }

            foreach (Rectangle rect in ZoneRectangles)
            {
                double dist = rect.GetDistanceToRect(location);
                if (dist < closestDistance)
                {
                    closest = rect;
                    closestDistance = dist;
                }
            }

            if (closest != null)
            {
                if (closest.GetType() == typeof(Line))
                    Lines.Remove((Line)closest);
                if (closest.GetType() == typeof(Rectangle))
                    ZoneRectangles.Remove((Rectangle)closest);
            }
        }

        public static void Clear()
        {
            ZoneRectangles.Clear();
            Lines.Clear();
            Quads.Clear();
            TempPoints.Clear();
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

    public class Quad
    {
        public Point[] points = new Point[4];
        public double Area;
        public Quad(Point point1, Point point2, Point point3, Point point4)
        {
            this.points[0] = point1;
            this.points[1] = point2;
            this.points[2] = point3;
            this.points[3] = point4;
            Area = GetArea();
        }

        public double GetArea()
        {
            //надо кешировать
            double Tri1 = RectangleExtensions.GetTriangleArea(points[0], points[1], points[2]);
            double Tri2 = RectangleExtensions.GetTriangleArea(points[2], points[3], points[0]);
            return Tri1 + Tri2;

        }

        public bool isPointInside(Point p)
        {
            double Tri1 = RectangleExtensions.GetTriangleArea(points[0], points[1], p);
            double Tri2 = RectangleExtensions.GetTriangleArea(points[1], points[2], p);
            double Tri3 = RectangleExtensions.GetTriangleArea(points[2], points[3], p);
            double Tri4 = RectangleExtensions.GetTriangleArea(points[3], points[0], p);
            double Area = Tri1 + Tri2 + Tri3 + Tri4;

            if (Area-0.001 > this.Area)
                return false;
            else
                return true;
        }
    }


}
