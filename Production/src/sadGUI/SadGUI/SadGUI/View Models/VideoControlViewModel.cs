using Emgu.CV;
using Emgu.CV.Structure;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Threading;
using System.Windows;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace SadGUI.View_Models
{
    class VideoControlViewModel : ViewModelBase
    {
        static private VideoControlViewModel m_camera = null;
        private Capture m_capture = null;
        private System.Windows.Controls.Image m_image = null;
        private bool m_running = false;
        private bool m_disabled = false;

        public VideoControlViewModel()
        {
            Action StartAction = Startup;
        }

        public bool IsRunning()
        {
            return m_running;
        }

        static public VideoControlViewModel Singleton
        {
            get
            {
                if(m_camera == null)
                {
                    m_camera = new VideoControlViewModel();
                }
                return m_camera;
            }
        }

        public DelegateCommand StartCommand { get; set; }

        public void Startup()
        {
            //Start(image);
        }
        public void Start(System.Windows.Controls.Image image)
        {
            if(image != null)
            {
                m_image = image;
            }

            if(m_running == true)
            {
                return;
            }
            if(m_capture == null)
            {
                try
                {
                    m_capture = new Capture();
                    m_disabled = false;
                }
                catch 
                {

                    SetDisabled();
                    return;
                }
            }
            if(m_capture == null)
            {
                SetDisabled();
                return;
            }

            m_running = true;

            Image<Bgr, Byte> frame = m_capture.QueryFrame();
            Image<Gray, Byte> gFrame = frame.Convert<Gray, Byte>();
            m_image.Source = ConvertImageToBitmap(frame);

            BackgroundWorker ImageWorker = new BackgroundWorker();

            ImageWorker.DoWork += new DoWorkEventHandler(
            delegate(object o, DoWorkEventArgs e)
            {
                BackgroundWorker background = o as BackgroundWorker;
                
                while (m_running)                
                {
                    frame = m_capture.QueryFrame();
                    
                    if (frame != null)   
                    {
                        gFrame = frame.Convert<Gray, Byte>();
                        m_image.Source = ConvertImageToBitmap(frame);
                    }
                        
                    Thread.Sleep(1000 / 60);
                    
                }
                
            });

            ImageWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(
            delegate(object o, RunWorkerCompletedEventArgs args)
            {
                m_image.Source = ConvertImageToBitmap(null as Image<Bgr, Byte>);
            });

            ImageWorker.RunWorkerAsync();

        }

        private void SetDisabled()
        {
            m_disabled = true;
            m_running = false;
        }

        private static BitmapSource ConvertImageToBitmap(IImage image)
        {
            using(Bitmap source = image.Bitmap)
            {
                var hbitmap = source.GetHbitmap();

                var bitmap = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(hbitmap, IntPtr.Zero,
                                                     Int32Rect.Empty,
                                                     BitmapSizeOptions.FromEmptyOptions());
                bitmap.Freeze();
                return bitmap;
            }
        }
        
        public void Stop()
        {
            m_running = false;
        }
    }
}
