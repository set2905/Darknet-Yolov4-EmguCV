using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using YOLOv4_TEST;

using System.Threading;

namespace DarknetYOLOv4
{
    public partial class ObjectDetectorForm : Form
    {

        private Thread _cameraThread;


        public ObjectDetectorForm()
        {
            InitializeComponent();
        }

        private void ObjectDetectorForm_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {

            if (_cameraThread != null)
            {
                _cameraThread.Abort();
                _cameraThread = null;
                return;
            }

            _cameraThread = new Thread(new ParameterizedThreadStart(new FrameHandlerBase().PlayFrames));
            _cameraThread.Start(this);

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }


    }
}
