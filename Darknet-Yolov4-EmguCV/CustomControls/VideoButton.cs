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

namespace DarknetYOLOv4.CustomControls
{
    
    public partial class VideoButton : Control
    {
        public string VideoPath = "https://live.cmirit.ru:443/live/park-pob08_1920x1080.stream/playlist.m3u8";
        public VideoCapture CoverCapture;

        public VideoButton()
        {
            InitializeComponent();

            CoverCapture = new VideoCapture(VideoPath);
            SetCover();

        }

        private void SetCover()
        {
            Mat frame = new Mat();
            CoverCapture.Grab();
            CoverCapture.Retrieve(frame);

            button1.Image = frame.ToBitmap();
        }

        protected override void OnPaint(PaintEventArgs pe)
        {
            base.OnPaint(pe);
        }

        private void button1_Click(object sender, EventArgs e)
        {


        }
    }
}
