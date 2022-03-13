using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DarknetYOLOv4
{
    internal class DetectionResultOutput
    {
        public PictureBox VideoBox;
        public Label StatusText;

        public DetectionResultOutput(PictureBox VideoBox, Label StatusText)
        {
            this.VideoBox = VideoBox;
            this.StatusText = StatusText;
        }

    }
}
