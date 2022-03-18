using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DarknetYOLOv4.FrameHandler
{
    public class FrameProcessResult
    {
        public Rectangle Rectangle { get; set; }

        public string Label { get; set; }

        public double Value { get; set; }


        public FrameProcessResult() { }
        public FrameProcessResult(Rectangle Rectangle)
        {
            this.Rectangle = Rectangle;
        }

        public FrameProcessResult(Rectangle Rectangle, string Label)
        {
            this.Rectangle = Rectangle;
            this.Label = Label;
        }

        public FrameProcessResult(Rectangle Rectangle, string Label, double Value)
        {
            this.Rectangle = Rectangle;
            this.Label = Label;
            this.Value = Value;
        }
    }
}
