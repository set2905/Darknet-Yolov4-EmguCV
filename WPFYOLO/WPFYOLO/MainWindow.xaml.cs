using System;
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
        }

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void SetFileButton_Click(object sender, RoutedEventArgs e)
        {

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
                PlayModeComboCox.Enabled = true;
                label1.Text = "Video Stopped";
                StartButton.Text = "START";
                // UpdateVideoCovers();
                return;
            }
            else
            {
                ToggleVideoChoice(false);
                FrameDisplay.IsEnabled = true;
                FrameDisplay.Visibility = Visibility.Visible;
                PlayModeComboCox.Enabled = false;
                StartButton.Text = "STOP";
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

            currentFrameHandler.FixedFPSValue = (int)FixedFpsValueBox.Value;
            currentFrameHandler.isFPSFixed = isFpsFixedBox.Checked;
            FixedFpsValueBox.Enabled = currentFrameHandler.isFPSFixed;
            currentFrameHandler.Play(this);

        }
    }
}
