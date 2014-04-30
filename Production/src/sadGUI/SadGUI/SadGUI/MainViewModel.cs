using MissileLauncher;
using SadGUI.View_Models;
using Strategies;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Targets;

namespace SadGUI
{
    public class MainViewModel: ViewModelBase
    {
        private string m_serverIp;
        private string m_port;
        private MissileLauncherViewModel m_launcherViewModel = MissileLauncherViewModel.Instance;
        private IStrategy m_strategy;
        public MissileLauncherViewModel Launcher
        {
            get
            {
                return m_launcherViewModel;
            }
            set
            {
                m_launcherViewModel = value;
                OnPropertyChanged("Launcher");
            }
        }

        public TargetListViewModel TargetList { get; set; }

        public MissileLauncherSelectorViewModel MissileLauncherSelector { get; set; }

        public ModeSelectorViewModel ModeSelector { get; set; }

        public ConnectionListViewModel ConnectionList { get; set; }

        public TwitterControlViewModel TwitterControl { get; set; }

        public DelegateCommand StartGame { get; private set; }

        public DelegateCommand StopGame { get; private set; }

        public DelegateCommand AbortGame { get; private set; }

        public DelegateCommand Connect { get; private set; }

        public string ServerIP
        {
            get
            {
                return m_serverIp;
            }
            set
            {
                m_serverIp = value;
                OnPropertyChanged("ServerIP");
            }
        }
        public string ServerPort
        {
            get
            {
                return m_port;
            }
            set
            {
                m_port = value;
                OnPropertyChanged("ServerPort");
            }
        }
        
        event EventHandler StartGameEvent;
        event EventHandler StopGameEvent;
        event EventHandler AbortGameEvent;

        public MainViewModel()
        {
            m_strategy = new KillEmAllStrategy();
            ServerIP = "Mock";
            ServerPort = "0";
            MissileLauncherSelector = new MissileLauncherSelectorViewModel();
            Launcher.LauncherFired += GetNextTarget;

            TargetList = new TargetListViewModel();

            ModeSelector = new ModeSelectorViewModel();

            ModeSelector.ModeChanged += ModeChanged;

            ConnectionList = new ConnectionListViewModel();

            TwitterControl = TwitterControlViewModel.Twitter;

            Launcher.LauncherChanged += LauncherChanged;


            Action startAction = Start;
            StartGame = new DelegateCommand(startAction);

            Action stopAction = Stop;
            StopGame = new DelegateCommand(stopAction);
            StopGame.Executable = false;

            Action abortAction = Abort;
            AbortGame = new DelegateCommand(abortAction);
            AbortGame.Executable = false;

            StartGameEvent += Launcher.Start;
            StopGameEvent += Launcher.Stop;
            AbortGameEvent += Launcher.Abort;

            Action connectAction = ConnectServer;
            Connect = new DelegateCommand(connectAction);
        }

        public void ModeChanged(object sender, EventArgs e)
        {
        }

        public void LauncherChanged(object sender, EventArgs args)
        {
        }

        void ConnectServer()
        {
            if (!ServerIP.Contains('.')) ServerIP = "Mock";
            TargetList.GameListViewModel.ServerIP = ServerIP;
            if (ServerPort == "") ServerPort = "0";
            try
            {
                TargetList.GameListViewModel.ServerPort = Convert.ToInt32(ServerPort);
            }
            catch (FormatException)
            {
                MessageBox.Show("Invalid port number");
            }
        }

        void GetNextTarget(object sender, EventArgs e)
        {
            var targets = TargetList.GameListViewModel.GetTargetList();
            var target = m_strategy.GetHighestPriorityTarget(targets, Launcher.Time);
            Launcher.Kill(target);
        }

        void Start()
        {
            StartGame.Executable = false;
            StopGame.Executable = true;
            AbortGame.Executable = true;
            Connect.Executable = false;
            TargetList.GameListViewModel.StartGame();
            StartGameEvent(this, null);
            if (ModeSelector.SelectedMode == Modes.Automatic)
            {
                var targets = TargetList.GetTargets();
                Launcher.Kill(m_strategy.GetHighestPriorityTarget(targets, Launcher.Time));
            }
            else
            {
                Launcher.ManualControl = true;
                TargetList.ManualControl = true;
                StartGameEvent(this, null);
            }
        }

        void Stop()
        {
            TargetList.GameListViewModel.StopGame();
            Launcher.ManualControl = false;
            TargetList.ManualControl = false;
            StopGame.Executable = false;
            AbortGame.Executable = false;
            StartGame.Executable = true;
            Connect.Executable = true;
            StopGameEvent(this, null);
        }

        void Abort()
        {
            Launcher.ManualControl = false;
            TargetList.ManualControl = false;
            StopGame.Executable = false;
            AbortGame.Executable = false;
            StartGame.Executable = true;
            AbortGameEvent(this, null);
        }
    }
}
