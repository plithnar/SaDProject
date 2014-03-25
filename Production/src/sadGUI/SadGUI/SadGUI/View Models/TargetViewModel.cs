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
        public Target TargetInfo { get; set; }

        private string m_name;

        private bool m_friendly;

        private double m_x, m_y, m_z;
        private string m_status;
        private string m_friend;
        private bool m_alive;
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
                return TargetInfo.Friend;
            }
            set
            {
                m_friendly = value;

                OnPropertyChanged("Friendly");
                OnPropertyChanged("Friend");
            }
        }

        public bool Alive
        {
            get
            {
                return m_alive;
            }
            set
            {
                m_alive = value;
                OnPropertyChanged("Alive");
                OnPropertyChanged("Status");
            }
        }

        public string Status
        {
            get
            {
                if (Alive)
                    return "Alive";
                else
                    return "Dead";
            }
        }

        public string Friend
        {
            get
            {
                if (Friendly)
                    return "Yes";
                else
                    return "No";
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
            TargetInfo = target;
            Name = target.Name;
            Friendly = target.Friend;
            X = target.X;
            Y = target.Y;
            Z = target.Z;
            Alive = target.Alive;
        }
    }
}
