﻿using MissileLauncher;
using SadGUI.View_Models;
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
        private MissileLauncherViewModel m_launcherViewModel;
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

        public DelegateCommand StartGame { get; private set; }

        public DelegateCommand StopGame { get; private set; }

        public DelegateCommand AbortGame { get; private set; }

        public MainViewModel()
        {

            MissileLauncherSelector = new MissileLauncherSelectorViewModel();

            TargetList = new TargetListViewModel();

            ModeSelector = new ModeSelectorViewModel();

            ModeSelector.ModeChanged += ModeChanged;

            ConnectionList = new ConnectionListViewModel();

            MissileLauncherController.Instance.LauncherChanged += LauncherChanged;

            Launcher = MissileLauncherController.Instance.Launcher;


            Action startAction = Start;
            StartGame = new DelegateCommand(startAction);

            Action stopAction = Stop;
            StopGame = new DelegateCommand(stopAction);
            StopGame.Executable = false;

            Action abortAction = Stop;
            AbortGame = new DelegateCommand(abortAction);
            AbortGame.Executable = false;
        }

        public void ModeChanged(object sender, EventArgs e)
        {
            Stop();
        }

        public void LauncherChanged()
        {
            Launcher = MissileLauncherController.Instance.Launcher;

            Stop();
        }

        void Start()
        {
            Stop();
            StartGame.Executable = false;
            StopGame.Executable = true;
            AbortGame.Executable = true;
            if (ModeSelector.SelectedMode == Modes.Automatic)
            {
                for (int i = 0; i < TargetList.Targets.Count; i++)
                {
                    var target = TargetList.Targets[i].TargetInfo;
                    TargetList.SelectedTarget = TargetList.Targets[i];
                    if (!target.Friend && MissileLauncherController.Instance.Launcher.Ammo > 0)
                    {
                        MissileLauncherController.Instance.Launcher.Kill(TargetList.Targets[i]);
                    }
                    else if (MissileLauncherController.Instance.Launcher.Ammo == 0)
                    {
                        MessageBox.Show("Launcher is out of Ammo!");
                        break;
                    }

                }
                Stop();
            }
            else
            {
                MissileLauncherController.Instance.Launcher.ManualControl = true;
                TargetList.ManualControl = true;
            }
        }

        void Stop()
        {
            MissileLauncherController.Instance.Launcher.ManualControl = false;
            TargetList.ManualControl = false;
            StopGame.Executable = false;
            AbortGame.Executable = false;
            StartGame.Executable = true;
        }
    }
}