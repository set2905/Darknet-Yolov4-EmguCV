using System;
using System.IO;
using System.Threading;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using DarknetYOLOv4;
using DarknetYOLOv4.FrameHandler;
using DarknetYOLOv4.UIExtensions;
using Emgu.CV;
using Emgu.CV.Structure;
using System.Runtime.InteropServices;
using FrameProcessing;

using Microsoft.Win32;


namespace WPFYOLO
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {



        private Thread _buttonCoversThread;


        public PlayMode currentPlayMode;
        private FrameHandlerBase currentFrameHandler;

        List<Button> VideoButtons = new List<Button>();
        List<Image> VideoButtonsCovers = new List<Image>();
        private string[] videos = new string[4]
        {
            @"https://live.cmirit.ru:443/live/park-pob08_1920x1080.stream/playlist.m3u8",
            @"https://live.cmirit.ru:443/live/10school-03_1920x1080.stream/playlist.m3u8",
            @"https://live.cmirit.ru:443/live/smart14_1920x1080.stream/playlist.m3u8",
            //@"https://live.cmirit.ru:443/live/smart16_1920x1080.stream/playlist.m3u8",
            @"https://live.cmirit.ru:443/live/jdvhod.stream/playlist.m3u8"
        };

        public MainWindow()
        {
            InitializeComponent();

            FixedFPSValueUpDown.ValueChanged += new RoutedPropertyChangedEventHandler<object>(FixedFPSUpDownChanged);
            FixedFPSValueUpDown.Minimum = 0;
            FixedFPSValueUpDown.Value = 12;

            VideoButtons.Add(FrameButton1);
            VideoButtons.Add(FrameButton2);
            VideoButtons.Add(FrameButton3);
            VideoButtons.Add(FrameButton4);
            VideoButtonsCovers.Add(capBtn1);
            VideoButtonsCovers.Add(capBtn2);
            VideoButtonsCovers.Add(capBtn3);
            VideoButtonsCovers.Add(capBtn4);

            ToggleVideoChoice(true);
            PlayModeComboBox.SelectedIndex = 0;

            // _buttonCoversThread = new Thread(new ThreadStart(UpdateVideoCovers));
            //_buttonCoversThread.Start();
            UpdateVideoCovers();

        }
        private void SetVideo(int index)
        {
            SetPlayMode();
            if (currentFrameHandler != null)
                currentFrameHandler.currentVideo = videos[index];

            // ToggleFrameHandler();

        }

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {

            ToggleFrameHandler();
        }

        private void SetFileButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog fdialog = new OpenFileDialog();
            if (fdialog.ShowDialog() == true)
            {
                SetPlayMode();
                if (currentFrameHandler != null)
                    currentFrameHandler.currentVideo = fdialog.FileName;
                ToggleFrameHandler();
            }
        }

        private void ScreenShotButton_Click(object sender, RoutedEventArgs e)
        {

        }
        private void ToggleVideoChoice(bool value)
        {
            if (!value)
            {
                FrameButton1.Visibility = Visibility.Hidden;
                FrameButton2.Visibility = Visibility.Hidden;
                FrameButton3.Visibility = Visibility.Hidden;
                FrameButton4.Visibility = Visibility.Hidden;
            }
            else
            {
                FrameButton1.Visibility = Visibility.Visible;
                FrameButton2.Visibility = Visibility.Visible;
                FrameButton3.Visibility = Visibility.Visible;
                FrameButton4.Visibility = Visibility.Visible;
            }

        }

        private void ToggleFrameHandler()
        {
            if (currentFrameHandler != null && currentFrameHandler.isPlaying)
            {
                currentFrameHandler.Stop();
                currentFrameHandler = null;

                ToggleVideoChoice(true);
                FrameDisplay.IsEnabled = false;
                FrameDisplay.Visibility = Visibility.Hidden;
                PlayModeComboBox.IsEnabled = true;
                StatusTextBox.Text = "Video Stopped";
                StartButtonText.Text = "START";

                return;
            }
            else
            {
                ToggleVideoChoice(false);
                FrameDisplay.IsEnabled = true;
                FrameDisplay.Visibility = Visibility.Visible;
                PlayModeComboBox.IsEnabled = false;
                StartButtonText.Text = "STOP";
            }

           

          //  SetPlayMode();

            if(currentFrameHandler==null) currentFrameHandler = new FramePlayer();

            currentFrameHandler.FixedFPSValue = (int)FixedFPSValueUpDown.Value;
            currentFrameHandler.isFPSFixed = FixedFPSCheckBox.IsChecked.HasValue ? FixedFPSCheckBox.IsChecked.Value : false;
            FixedFPSValueUpDown.IsEnabled = currentFrameHandler.isFPSFixed;
            currentFrameHandler.Play(FrameDisplay);
        }

        private void UpdateVideoCovers()
        {
            if (VideoButtons.Count != VideoButtonsCovers.Count) return;
            for (int i = 0; i < VideoButtonsCovers.Count; i++)
            {
                SetCover(VideoButtonsCovers[i], videos[i]);
            }

        }
        private void FrameButton1_Click(object sender, RoutedEventArgs e)
        {
            SetVideo(VideoButtons.IndexOf(FrameButton1));
            ToggleFrameHandler();
        }
        private void FrameButton2_Click(object sender, RoutedEventArgs e)
        {
            SetVideo(VideoButtons.IndexOf(FrameButton2));
            ToggleFrameHandler();
        }
        private void FrameButton3_Click(object sender, RoutedEventArgs e)
        {
            SetVideo(VideoButtons.IndexOf(FrameButton3));
            ToggleFrameHandler();
        }
        private void FrameButton4_Click(object sender, RoutedEventArgs e)
        {
            SetVideo(VideoButtons.IndexOf(FrameButton4));
            ToggleFrameHandler();
        }




        void FixedFPSUpDownChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (currentFrameHandler == null) return;
            currentFrameHandler.FixedFPSValue = (int)FixedFPSValueUpDown.Value;
        }

        private void FixedFPSCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            if (currentFrameHandler == null) return;
            currentFrameHandler.isFPSFixed = FixedFPSCheckBox.IsChecked.HasValue ? FixedFPSCheckBox.IsChecked.Value : false;
            FixedFPSValueUpDown.IsEnabled = currentFrameHandler.isFPSFixed;
        }

        private void PlayModeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            currentPlayMode = (PlayMode)PlayModeComboBox.SelectedItem;
        }

        private void SetPlayMode()
        {
            switch (currentPlayMode)
            {
                case PlayMode.Play:
                    currentFrameHandler = new FramePlayer();
                    break;
                case PlayMode.YOLO:
                    currentFrameHandler = new FrameObjectDetectorYOLO();
                    break;
                case PlayMode.MOG2:
                    currentFrameHandler = new FrameMOG2();
                    break;

                case PlayMode.YOLOTrackMoving:
                    currentFrameHandler = new FrameYoloTracker();
                    break;

            }
        }

        public void SetCover(Image imageControl, string VideoPath)
        {
            VideoCapture CoverCapture;
            CoverCapture = new VideoCapture(VideoPath);

            Mat frame = new Mat();
            CoverCapture.Grab();

            CoverCapture.Retrieve(frame);
            if (frame.GetData() == null) return;
           // int w = (int)Math.Round(imageControl.Width);
           // int h = (int)Math.Round(imageControl.Height);
            //CvInvoke.Resize(frame, frame, new Size(w, h));
            Image<Bgr, Byte> img = frame.ToImage<Bgr, Byte>();

                System.Drawing.Bitmap bm = img.ToBitmap();

            imageControl.Source = BitmapSourceConvert.ToBitmapSource(bm);

            img.Dispose();
            CoverCapture.Dispose();
        }

    }






}
