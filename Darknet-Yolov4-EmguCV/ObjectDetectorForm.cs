﻿using System;
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
        Play,
        BackgroundSubstraction
    }
    public partial class ObjectDetectorForm : Form
    {

        private Thread _cameraThread;
        public PlayMode currentPlayMode;
        private FrameHandlerBase currentFrameHandler;
        public string video = @"https://live.cmirit.ru:443/live/park-pob08_1920x1080.stream/playlist.m3u8";

        private void ToggleFrameHandler()
        {
            if (currentFrameHandler != null)
            {
                //так не надо наверное
                _cameraThread.Abort();
                _cameraThread = null;
                currentFrameHandler = null;
                pictureBox1.Image = null;
                pictureBox1.Enabled = false;
                PlayModeComboCox.Enabled = true;
                label1.Text = "Video Stopped";
                StartButton.Text = "START";
                return;
            }
            else
            {
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


            _cameraThread = new Thread(new ParameterizedThreadStart(currentFrameHandler.PlayFrames));
            _cameraThread.Start(this);
        }

        public ObjectDetectorForm()
        {
            InitializeComponent();

            PlayModeComboCox.DataSource = Enum.GetValues(typeof(PlayMode));
            FixedFpsValueBox.Value = 12;
        }


        private void ObjectDetectorForm_Load(object sender, EventArgs e)
        {

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
                video = fdialog.FileName;
            }
        }
    }
}
