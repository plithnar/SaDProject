﻿using MissileLauncher;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Targets;

namespace SadGUI.View_Models
{
    public class TargetListViewModel: ViewModelBase
    {
        private bool m_manualControl;
        public ObservableCollection<TargetViewModel> Targets { get; set; }

        public TargetViewModel SelectedTarget { get; set; }


        public DelegateCommand ClearTargetsCommand { get; set; }
        public DelegateCommand AddTargetsCommand { get; set; }
        public DelegateCommand AddServerTargetsCommand { get; set; }
        public DelegateCommand KillTargetCommand { get; set; }

        public bool ManualControl
        {
            get
            {
                return m_manualControl;
            }
            set
            {
                m_manualControl = value;
                KillTargetCommand.Executable = m_manualControl;
            }
        }
        
        public TargetListViewModel()
        {
            Targets = new ObservableCollection<TargetViewModel>();

            Action clearTargetsAction = ClearTargets;
            ClearTargetsCommand = new DelegateCommand(clearTargetsAction);
            ClearTargetsCommand.Executable = false;

            Action addTargetsAction = AddTargets;
            AddTargetsCommand = new DelegateCommand(addTargetsAction);

            Action addServerTargetsAction = AddTargets;
            AddServerTargetsCommand = new DelegateCommand(addServerTargetsAction);
            AddServerTargetsCommand.Executable = false;

            Action killTargetAction = KillTarget;
            KillTargetCommand = new DelegateCommand(killTargetAction);

            ManualControl = false;
        }

        void ClearTargets()
        {
            Targets.Clear();
            ClearTargetsCommand.Executable = false;
        }

        void AddTargets()
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.FileName = "Target Configuration File"; // Default file name
            dlg.DefaultExt = ".ini"; // Default file extension
            dlg.Filter = "Target Configuration Files (.ini)|*.ini"; // Filter files by extension 

            // Show open file dialog box
            Nullable<bool> result = dlg.ShowDialog();

            // Process open file dialog box results 
            if (result == true)
            {
                // Open document 
                string filename = dlg.FileName;

                ITargetFileReader reader = null;
                try
                {
                    reader = TargetFileReaderFactory.Create(filename);
                    var targets = reader.read();

                    Targets.Clear();

                    foreach (var target in targets)
                    {
                        Targets.Add(new TargetViewModel(target));
                    }
                    ClearTargetsCommand.Executable = true;

                }
                catch (ArgumentException)
                {
                    MessageBox.Show("Your target file contains a duplicate property for a target.");
                    return;
                }
                catch (NullReferenceException)
                {
                    MessageBox.Show("Ya, that's just a bad file.");
                    return;
                }
                catch (FileNotFoundException)
                {
                    MessageBox.Show("No file exists.");
                    return;
                }
                catch (FormatException)
                {
                    // Couldn't convert either coords, points, or flashrate
                    MessageBox.Show("Target file contains invalid integer or float.");
                    return;
                }
                catch (INITargetFileReader.InvalidName)
                {
                    // Name contained a space
                    MessageBox.Show("A target name contains a space.");
                    return;
                }
                catch (INITargetFileReader.InvalidTarget)
                {
                    // Missing an entry in a target
                    MessageBox.Show("Incomplete target.");
                    return;
                }
                catch (FileHandlers.FileReader.InvalidHeader)
                {
                    // Broken header tag
                    MessageBox.Show("Target contains invalid header line.\n\nValid header: [Target]");
                    return;
                }
            }
        }

        void KillTarget()
        {
            if (SelectedTarget != null)
            {
                MissileLauncherController.Instance.Launcher.Kill(SelectedTarget);
            }
        }
    }
}
