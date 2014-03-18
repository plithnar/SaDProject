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

        private string m_name;

        private bool m_friendly;

        private double m_x, m_y, m_z;
        public string Name
        {
            get
            {
                return m_name;
            }
            set
            {
                m_name = value;
                OnPropertyChanged("Name");
            }
        }

        public bool Friendly
        {
            get
            {
                return m_friendly;
            }
            set
            {
                m_friendly = value;
                OnPropertyChanged("Friendly");
            }
        }

        public double X
        {
            get
            {
                return m_x;
            }
            set
            {
                m_x = value;
                OnPropertyChanged("X");
            }
        }

        public double Y
        {
            get
            {
                return m_y;
            }
            set
            {
                m_y = value;
                OnPropertyChanged("Y");
            }
        }

        public double Z
        {
            get
            {
                return m_z;
            }
            set
            {
                m_z = value;
                OnPropertyChanged("Z");
            }
        }

        public TargetViewModel(Target target)
        {
            m_target = target;
            Name = target.Name;
            Friendly = target.Friend;
            X = target.X;
            Y = target.Y;
            Z = target.Z;
        }
    }
}
