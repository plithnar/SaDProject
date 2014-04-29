using MissileLauncher;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using Targets;

namespace SadGUI.View_Models
{
    enum LauncherAction { MoveTo, MoveBy, Fire, Kill, Reload, Recalibrate };
    class LauncherCommand
    {
        public double Phi { get; private set; }
        public double Theta { get; private set; }
        public string Message { get; private set; }
        public LauncherAction Action { get; private set; }
        public LauncherCommand(LauncherAction action, double phi=0.0, double theta=0.0, string message="")
        {
            Action = action;
            Phi = phi;
            Theta = theta;
            Message = message;
        }
    }

    public class MissileLauncherViewModel: ViewModelBase
    {
        private const double moveAmount = 15;
        private IMissileLauncher m_launcher;
        private string m_name;
        private int m_ammo;
        private int m_capacity;
        private double m_phi;
        private double m_theta;
        private bool m_manualControl;
        private BackgroundWorker m_launcherWorker;
        private Queue<LauncherCommand> m_commands;

        private BackgroundWorker m_timer;

        private static MissileLauncherViewModel m_instance;

        private static TwitterControlViewModel m_twitter;

        public static MissileLauncherViewModel Instance
        {
            get
            {
                if (m_instance == null)
                    m_instance = new MissileLauncherViewModel();
                return m_instance;
            }
        }

        public bool GameRunning
        {
            get;
            private set;
        }

        public string Name
        {
            get
            {
                return m_name;
            }
            private set
            {
                m_name = value;
                OnPropertyChanged("Name");
            }
        }

        public int Ammo
        {
            get
            {
                return m_ammo;
            }
            private set
            {
                m_ammo = value;
                OnPropertyChanged("Ammo");
            }
        }

        public int Capacity
        {
            get
            {
                return m_capacity;
            }
            private set
            {
                m_capacity = value;
                OnPropertyChanged("Capacity");
            }
        }

        public double Phi
        {
            get
            {
                return m_phi;
            }
            private set
            {
                m_phi = value;
                OnPropertyChanged("Phi");
            }
        }

        public double Theta
        {
            get
            {
                return m_theta;
            }
            private set
            {
                m_theta = value;
                OnPropertyChanged("Theta");
            }
        }

        public MissileLauncherTypes LauncherType
        {
            get
            {
                return m_launcherType;
            }
            set
            {
                m_launcherType = value;
                m_launcher = MissileLauncherFactory.Create(value);
            }
        }

        public string GameTime
        {
            get
            {
                return m_gameTime;
            }
            private set
            {
                m_gameTime = value;
                OnPropertyChanged("GameTime");
            }
        }
        
        public event EventHandler LauncherChanged;
        private MissileLauncherTypes m_launcherType;
        private string m_gameTime;
        public DelegateCommand FireCommand { get; private set; }
        public DelegateCommand UpCommand { get; private set; }
        public DelegateCommand DownCommand { get; private set; }
        public DelegateCommand LeftCommand { get; private set; }
        public DelegateCommand RightCommand { get; private set; }
        public DelegateCommand ReloadCommand { get; private set; }
        public DelegateCommand CenterCommand { get; private set; }

        public bool ManualControl
        {
            get
            {
                return m_manualControl;
            }
            set
            {
                m_manualControl = value;
                FireCommand.Executable = m_manualControl;
                UpCommand.Executable = m_manualControl;
                DownCommand.Executable = m_manualControl;
                LeftCommand.Executable = m_manualControl;
                RightCommand.Executable = m_manualControl;
                CenterCommand.Executable = m_manualControl;
            }
        }

        private void AddCommand(LauncherCommand command)
        {
            if (GameRunning)
                m_commands.Enqueue(command);
        }

        private void ExecuteCommands(object o, DoWorkEventArgs e)
        {
            m_launcher.calibrate();
            try
            {
                while (!m_launcherWorker.CancellationPending)
                {
                    if (m_commands.Count > 0)
                    {
                        var currentCommand = m_commands.Dequeue();
                        switch (currentCommand.Action)
                        {
                            case LauncherAction.Fire:
                                try
                                {
                                    m_launcher.fire();
                                    Ammo = m_launcher.CurrentMissiles;
                                }
                                catch (InvalidOperationException)
                                {
                                    MessageBox.Show("Launcher is out of ammo.");
                                    m_commands.Clear();
                                }
                                break;
                            case LauncherAction.Reload:
                                m_launcher.reload();
                                Ammo = m_launcher.CurrentMissiles;
                                break;
                            case LauncherAction.MoveBy:
                                m_launcher.moveBy(currentCommand.Phi, currentCommand.Theta);
                                Phi = m_launcher.Phi;
                                Theta = m_launcher.Theta;
                                break;
                            case LauncherAction.MoveTo:
                                m_launcher.moveTo(currentCommand.Phi, currentCommand.Theta);
                                Phi = m_launcher.Phi;
                                Theta = m_launcher.Theta;
                                break;
                            case LauncherAction.Kill:
                                m_launcher.moveTo(currentCommand.Phi, currentCommand.Theta);
                                Phi = m_launcher.Phi;
                                Theta = m_launcher.Theta;
                                try
                                {
                                    m_launcher.fire();
                                    Ammo = m_launcher.CurrentMissiles;
                                    m_twitter.Tweet(currentCommand.Message);
                                }
                                catch (InvalidOperationException)
                                {
                                    MessageBox.Show("Launcher is out of ammo.");
                                    m_commands.Clear();
                                }
                                break;
                            case LauncherAction.Recalibrate:
                                m_launcher.calibrate();
                                break;
                        }
                    }
                }
            }
            finally
            {
                Phi = m_launcher.Phi;
                Theta = m_launcher.Theta;
                Ammo = m_launcher.CurrentMissiles;
            }
        }

        private BackgroundWorker InitLauncherWorker()
        {
            var launcherWorker = new BackgroundWorker();
            launcherWorker.DoWork += ExecuteCommands;
            launcherWorker.WorkerSupportsCancellation = true;
            return launcherWorker;
        }

        private void KeepTime(object o, DoWorkEventArgs e)
        {
            int time = 0;
            while (!m_timer.CancellationPending)
            {
                int hours = time / 3600;
                int minutes = (time % 3600) / 60;
                int seconds = (time % 3600) % 60;
                var strb = new StringBuilder();
                strb.AppendFormat("{0}:{1}:{2}", hours, minutes, seconds);
                GameTime = strb.ToString();
                time++;
                Thread.Sleep(1000);
            }
        }

        private void StartTime()
        {
            m_timer.RunWorkerAsync();
        }

        private void EndTime()
        {
            m_timer.CancelAsync();
            GameTime = "0:0:0";
        }

        public void Start(object sender, EventArgs e)
        {
            StartTime();
            GameRunning = true;
            m_twitter.Tweet(string.Format("Game started at {0}!", DateTime.Now.ToString("HH:mm ss tt")));
        }

        public void Stop(object sender, EventArgs e)
        {
            GameRunning = false;
            EndTime();
            m_commands.Clear();
            m_commands.Enqueue(new LauncherCommand(LauncherAction.Recalibrate));
            Phi = m_launcher.Phi;
            Theta = m_launcher.Theta;
            m_twitter.Tweet(string.Format("Game stopped at {0}!", DateTime.Now.ToString("HH:mm ss tt")));
        }

        public void Abort(object sender, EventArgs e)
        {
            EndTime();
            GameRunning = false;
            m_commands.Clear();
            m_twitter.Tweet(string.Format("Game aborted at {0}!", DateTime.Now.ToString("HH:mm ss tt")));
        }

        private MissileLauncherViewModel()
        {
            var launcherType = MissileLauncherTypes.Mock;
            m_launcher = MissileLauncherFactory.Create(launcherType);

            Action fireAction = Fire;
            FireCommand = new DelegateCommand(fireAction);

            Action upAction = Up;
            UpCommand = new DelegateCommand(upAction);

            Action downAction = Down;
            DownCommand = new DelegateCommand(downAction);

            Action leftAction = Left;
            LeftCommand = new DelegateCommand(leftAction);

            Action rightAction = Right;
            RightCommand = new DelegateCommand(rightAction);

            Action reloadAction = Reload;
            ReloadCommand = new DelegateCommand(reloadAction);

            Action centerAction = Center;
            CenterCommand = new DelegateCommand(centerAction);

            GameRunning = false;
            Name = m_launcher.Name;
            Ammo = m_launcher.CurrentMissiles;
            Capacity = m_launcher.MaxMissiles;
            Phi = m_launcher.Phi;
            Theta = m_launcher.Theta;

            ManualControl = false;

            m_commands = new Queue<LauncherCommand>();
            m_launcherWorker = InitLauncherWorker();
            m_launcherWorker.RunWorkerAsync();

            GameTime = "0:0:0";
            m_timer = new BackgroundWorker();
            m_timer.DoWork += KeepTime;
            m_timer.WorkerSupportsCancellation = true;
            m_twitter = TwitterControlViewModel.Twitter;
        }

        public void Kill(TargetViewModel targetvm)
        {
            var target = targetvm.TargetInfo;
            if (target != null)
            {
                if (target.Friend)
                {
                    MessageBox.Show("Cannot kill that target. It is friendly.");
                    return;
                }
                double phi = Conversions.calcPhi(target.X, target.Y);
                double theta = Conversions.calcTheta(target.X, target.Y, target.Z);

                string tweetText = "Shot at " + target.Name + " at " + m_gameTime;

                m_commands.Enqueue(new LauncherCommand(LauncherAction.Kill, phi, theta, tweetText));
                
            }
        }

        void Fire()
        {
            AddCommand(new LauncherCommand(LauncherAction.Fire));
        }

        void Up()
        {
            AddCommand(new LauncherCommand(LauncherAction.MoveBy, 0, moveAmount));
        }
        void Down()
        {
            AddCommand(new LauncherCommand(LauncherAction.MoveBy, 0, -1*moveAmount));
        }
        void Left()
        {
            AddCommand(new LauncherCommand(LauncherAction.MoveBy, -1*moveAmount, 0));
        }

        void Right()
        {
            AddCommand(new LauncherCommand(LauncherAction.MoveBy, moveAmount, 0));
        }

        void Center()
        {
            //AddCommand(new LauncherCommand(LauncherAction.MoveTo));
            AddCommand(new LauncherCommand(LauncherAction.Recalibrate));
        }

        void Reload()
        {
            m_launcher.reload();
            Ammo = m_launcher.CurrentMissiles;
        }
    }
}
