using System;
using System.IO;
using System.Threading;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using DarknetYOLOv4;
using DarknetYOLOv4.FrameHandler;
using DarknetYOLOv4.UIExtensions;

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
        public string currentVideo = @"https://live.cmirit.ru:443/live/park-pob08_1920x1080.stream/playlist.m3u8";
        List<Button> VideoButtons = new List<Button>();
        private string[] videos = new string[4]
        {
            @"https://live.cmirit.ru:443/live/park-pob08_1920x1080.stream/playlist.m3u8",
            @"https://live.cmirit.ru:443/live/10school-03_1920x1080.stream/playlist.m3u8",
            @"https://live.cmirit.ru:443/live/smart14_1920x1080.stream/playlist.m3u8",
            @"https://live.cmirit.ru:443/live/smart16_1920x1080.stream/playlist.m3u8"
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

            ToggleVideoChoice(true);

        }
        private void SetVideo(int index)
        {
            currentVideo = videos[index];

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
                currentVideo = fdialog.FileName;
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

            currentFrameHandler = new FramePlayer();

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

            currentFrameHandler.FixedFPSValue = (int)FixedFPSValueUpDown.Value;
            currentFrameHandler.isFPSFixed = FixedFPSCheckBox.IsChecked.HasValue ? FixedFPSCheckBox.IsChecked.Value : false;
            FixedFPSValueUpDown.IsEnabled = currentFrameHandler.isFPSFixed;
            // currentFrameHandler.Play(this);
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
    }



}
