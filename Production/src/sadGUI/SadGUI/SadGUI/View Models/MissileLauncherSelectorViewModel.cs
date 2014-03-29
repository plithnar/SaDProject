using MissileLauncher;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace SadGUI.View_Models
{
    public class MissileLauncherSelectorViewModel: ViewModelBase
    {
        public MissileLauncherTypes m_selected;

        public IEnumerable<MissileLauncherTypes> LauncherTypes { get; set; }
        public MissileLauncherTypes SelectedLauncher
        { 
            get
            {
                return m_selected;
            }
            set
            {
                m_selected = value;
                MissileLauncherViewModel.Instance.LauncherType = value;
            }
        }

        public MissileLauncherSelectorViewModel()
        {
            SelectedLauncher = MissileLauncherTypes.Mock;

            this.LauncherTypes = Enum.GetValues(typeof (MissileLauncherTypes)).OfType<MissileLauncherTypes>().ToArray();
        }

    }
}
