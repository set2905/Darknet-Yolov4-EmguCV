using System;
using System.Windows;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Emgu.CV;
using Emgu.CV.Structure;

namespace WPFYOLO
{
    internal abstract class ZoneTool
    {
        public abstract void SetZone(Image<Bgra, byte> imgIntruderZoneOverlay, System.Windows.Controls.Image FrameUserDraw);
    }
}
