using MissileLauncher;
using SadGUI.View_Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Targets;

namespace SadGUI
{
    public class MainViewModel
    {
        public MissileLauncherViewModel MissileLauncher { get; set; }

        public TargetListViewModel TargetList { get; set; }

        public ObservableCollection<TargetViewModel> Targets { get; set; }

        public MainViewModel()
        {
            var Mock = new Mock();
            var DreamCheeky = new DreamCheeky();

            Targets = new ObservableCollection<TargetViewModel>();

            Targets.Add(new TargetViewModel(new Target("asdf", 0, 0, 0, false, 10, 10)));

            MissileLauncher = new MissileLauncherViewModel(DreamCheeky);

            TargetList = new TargetListViewModel(DreamCheeky);
        }
    }
}
