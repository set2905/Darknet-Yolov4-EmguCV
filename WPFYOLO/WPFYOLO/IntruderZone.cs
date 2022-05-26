
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

namespace WPFYOLO
{
    public static class IntruderZone
    {
        public static Rectangle rect;
        public static System.Drawing.Point StartLocation;
        public static System.Drawing.Point EndLocation;

        public static Rectangle GetRectangle()
        {
            rect = new Rectangle();
            rect.X = Math.Min(StartLocation.X, EndLocation.X);
            rect.Y = Math.Min(StartLocation.Y, EndLocation.Y);
            rect.Width = Math.Abs(StartLocation.X - EndLocation.X);
            rect.Height = Math.Abs(StartLocation.Y - EndLocation.Y);

            return rect;
        }
    }
}
