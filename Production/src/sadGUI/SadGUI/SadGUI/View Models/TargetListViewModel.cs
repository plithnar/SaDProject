using MissileLauncher;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Targets;

namespace SadGUI.View_Models
{
    public class TargetListViewModel: ViewModelBase
    {
        ObservableCollection<TargetViewModel> Targets;

        IMissileLauncher m_launcher;


        public DelegateCommand ClearTargetsCommand { get; set; }
        public DelegateCommand AddTargetsCommand { get; set; }
        public DelegateCommand KillTargetCommand { get; set; }
        
        public TargetListViewModel(IMissileLauncher launcher)
        {
            m_launcher = launcher;

            Targets = new ObservableCollection<TargetViewModel>();
            Targets.Add(new TargetViewModel(new Target("asdf", 0, 0, 0, false, 10, 10)));

            Action clearTargetsAction = ClearTargets;
            ClearTargetsCommand = new DelegateCommand(clearTargetsAction);

            Action addTargetsAction = AddTargets;
            AddTargetsCommand = new DelegateCommand(addTargetsAction);

            Action killTargetAction = KillTarget;
            KillTargetCommand = new DelegateCommand(killTargetAction);
        }

        void ClearTargets()
        {
            Targets.Clear();
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
                var reader = TargetFileReaderFactory.Create(filename);
                var TargetList = reader.read();

                Targets.Clear();

                foreach (var target in TargetList)
                {
                    Targets.Add(new TargetViewModel(target));
                }
            }
        }

        void KillTarget()
        {
            Console.WriteLine();
        }
    }
}
