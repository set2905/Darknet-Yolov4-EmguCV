using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DarknetYOLOv4.FrameHandler;

using System.Threading;

namespace DarknetYOLOv4
{
    public enum PlayMode
    {
        DetectorYOLO,
        Play
    }
    public partial class ObjectDetectorForm : Form
    {

        private Thread _cameraThread;
        public PlayMode currentPlayMode;
        private FrameHandlerBase currentFrameHandler;

        public ObjectDetectorForm()
        {
            InitializeComponent();

            PlayModeComboCox.DataSource = Enum.GetValues(typeof(PlayMode));
        }


        private void ObjectDetectorForm_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {

            if (currentFrameHandler != null)
            {
                //так не должно быть
                _cameraThread.Abort();
                _cameraThread = null;
                currentFrameHandler = null;
                StartButton.Text = "START";
                return;
            }
            else StartButton.Text = "STOP";

            currentFrameHandler = new FramePlayer();

            switch (currentPlayMode)
            {
                case PlayMode.Play:
                    currentFrameHandler = new FramePlayer();
                    break;
                case PlayMode.DetectorYOLO:
                    currentFrameHandler = new FrameObjectDetectorYOLO();
                    break;

            }

            _cameraThread = new Thread(new ParameterizedThreadStart(currentFrameHandler.PlayFrames));
            _cameraThread.Start(this);

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void PlayModeComboCox_SelectedIndexChanged(object sender, EventArgs e)
        {
            
            currentPlayMode = (PlayMode)PlayModeComboCox.SelectedItem;

        }
    }
}
