using SadGUI.View_Models;
using System;
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

namespace SadGUI.Views
{
    /// <summary>
    /// Interaction logic for VideoControl.xaml
    /// </summary>
    public partial class VideoControl : UserControl
    {
        public VideoControl()
        {
            InitializeComponent();

            //EventHandler StartButton_Click
            //{
            //    VideoControlViewModel.Singleton.Start(image);
            //}
        }

        private void StopButton_Click(object sender, RoutedEventArgs e)
        {
            VideoControlViewModel.Singleton.StopVideo();
        }

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            VideoControlViewModel.Singleton.Start(image);
        }
    }
}
