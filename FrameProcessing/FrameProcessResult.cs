using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace FrameProcessing
{
    public class FrameProcessResult
    {
        public Rectangle Rectangle;

        public string Label;

        public double Value;
        public Color Color;



        public FrameProcessResult() 
        {
            Label = "";
            Value = 0;
        }
        public FrameProcessResult(Rectangle Rectangle)
        {
            Label = "";
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

        public FrameProcessResult(Rectangle Rectangle, string Label, Color _color)
        {
            this.Rectangle = Rectangle;
            this.Label = Label;
            this.Color = _color;
        }
    }
}
