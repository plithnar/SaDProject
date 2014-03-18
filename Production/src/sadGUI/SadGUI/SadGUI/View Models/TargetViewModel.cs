using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Targets;

namespace SadGUI.View_Models
{
    public class TargetViewModel: ViewModelBase
    {
        private Target m_target;
        public string Name { get; private set; }

        public TargetViewModel(Target target)
        {
            m_target = target;
            Name = target.Name;
        }
    }
}
