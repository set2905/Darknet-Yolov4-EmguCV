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
using DarknetYOLOv4.UIExtensions;

using System.Threading;

namespace DarknetYOLOv4
{
    public enum PlayMode
    {
        DetectorYOLO,
        Play,
        BackgroundSubstraction
    }
    public partial class ObjectDetectorForm : Form
    {


        private Thread _buttonCoversThread;


        public PlayMode currentPlayMode;
        private FrameHandlerBase currentFrameHandler;
        public string currentVideo = @"https://live.cmirit.ru:443/live/park-pob08_1920x1080.stream/playlist.m3u8";
        List<Button> VideoButtons = new List<Button>();
        private string[] videos = new string[4]
        {
            @"https://live.cmirit.ru:443/live/park-pob08_1920x1080.stream/playlist.m3u8",
            @"https://live.cmirit.ru:443/live/200-11_1920x1080.stream/playlist.m3u8",
            @"https://live.cmirit.ru:443/live/smart14_1920x1080.stream/playlist.m3u8",
            @"https://live.cmirit.ru:443/live/smart16_1920x1080.stream/playlist.m3u8"
        };


        private void ToggleFrameHandler()
        {
            if (currentFrameHandler != null)
            {
                currentFrameHandler.Stop();
                currentFrameHandler = null;

                VideoChoicePanel.Enabled = true;
                VideoChoicePanel.Visible = true;
                pictureBox1.Image = null;
                pictureBox1.Enabled = false;
                PlayModeComboCox.Enabled = true;
                label1.Text = "Video Stopped";
                StartButton.Text = "START";
                // UpdateVideoCovers();
                return;
            }
            else
            {
                VideoChoicePanel.Enabled = false;
                VideoChoicePanel.Visible = false;
                pictureBox1.Enabled = true;
                PlayModeComboCox.Enabled = false;
                StartButton.Text = "STOP";
            }


            currentFrameHandler = new FramePlayer();

            switch (currentPlayMode)
            {
                case PlayMode.Play:
                    currentFrameHandler = new FramePlayer();
                    break;
                case PlayMode.DetectorYOLO:
                    currentFrameHandler = new FrameObjectDetectorYOLO();
                    break;
                case PlayMode.BackgroundSubstraction:
                    currentFrameHandler = new FrameMOG2();
                    break;

            }

            currentFrameHandler.FixedFPSValue = (int)FixedFpsValueBox.Value;
            currentFrameHandler.isFPSFixed = isFpsFixedBox.Checked;
            FixedFpsValueBox.Enabled = currentFrameHandler.isFPSFixed;
            currentFrameHandler.Play(this);

        }

        private void SetVideo(int index)
        {
            currentVideo = videos[index];

        }

        public ObjectDetectorForm()
        {
            InitializeComponent();

            foreach (Button b in VideoChoicePanel.Controls)
            {
                VideoButtons.Add(b);
            }

            VideoChoicePanel.Enabled = true;
            VideoChoicePanel.Visible = true;

            PlayModeComboCox.DataSource = Enum.GetValues(typeof(PlayMode));
            FixedFpsValueBox.Value = 12;
        }


        private void ObjectDetectorForm_Load(object sender, EventArgs e)
        {
            _buttonCoversThread = new Thread(new ThreadStart(UpdateVideoCovers));
            _buttonCoversThread.Start();

            // UpdateVideoCovers();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ToggleFrameHandler();
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

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            if (currentFrameHandler == null) return;
            currentFrameHandler.FixedFPSValue = (int)FixedFpsValueBox.Value;
        }

        private void isFpsFixedBox_CheckedChanged(object sender, EventArgs e)
        {
            if (currentFrameHandler == null) return;
            currentFrameHandler.isFPSFixed = isFpsFixedBox.Checked;
            FixedFpsValueBox.Enabled = currentFrameHandler.isFPSFixed;
        }

        private void FileDialogButton_Click(object sender, EventArgs e)
        {
            OpenFileDialog fdialog = new OpenFileDialog();
            if (fdialog.ShowDialog() == DialogResult.OK)
            {
                currentVideo = fdialog.FileName;
                ToggleFrameHandler();
            }
        }

        private void UpdateVideoCovers()
        {

            foreach (Button b in VideoButtons)
            {
                b.SetCover(videos[VideoButtons.IndexOf(b)]);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            SetVideo(VideoButtons.IndexOf(VideoButton3));
            ToggleFrameHandler();

        }

        private void VideoButton1_Click(object sender, EventArgs e)
        {
            SetVideo(VideoButtons.IndexOf(VideoButton1));
            ToggleFrameHandler();
        }

        private void VideoButton2_Click(object sender, EventArgs e)
        {
            SetVideo(VideoButtons.IndexOf(VideoButton2));
            ToggleFrameHandler();
        }

        private void VideoButton4_Click(object sender, EventArgs e)
        {
            SetVideo(VideoButtons.IndexOf(VideoButton4));
            ToggleFrameHandler();
        }

        private void ScreenShotButton_Click(object sender, EventArgs e)
        {
            if (currentFrameHandler == null) return;

            if (currentFrameHandler.snapShotDirectory == null)
            {
                var dirDialog = new FolderBrowserDialog();
                if (dirDialog.ShowDialog() == DialogResult.OK)
                {
                    currentFrameHandler.snapShotDirectory = dirDialog.SelectedPath;
                }
                Console.WriteLine($"new Snapshot path is: {currentFrameHandler.snapShotDirectory}");

            }
            currentFrameHandler.SnapshotRequired = true;
        }
    }
}
