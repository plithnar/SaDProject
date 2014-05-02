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
        private int m_points;
        private int m_flashRate;
        private int m_timestamp;

        public int HitTime { get; set; }

        public int TimeStamp
        {
            get
            {
                return m_timestamp;
            }
            set
            {
                m_timestamp = value;
                OnPropertyChanged("TimeStamp");
            }
        }
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
                return Math.Round(m_x, 3);
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
                return Math.Round(m_y, 3);
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
                return Math.Round(m_z, 3);
            }
            set
            {
                m_z = value;
                OnPropertyChanged("Z");
            }
        }

        public int Points
        {
            get
            {
                return m_points;
            }
            set
            {
                m_points = value;
                OnPropertyChanged("Points");
            }
        }

        public int FlashRate
        {
            get
            {
                return m_flashRate;
            }
            set
            {
                m_flashRate = value;
                OnPropertyChanged("FlashRate");
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
            Points = target.Points;
            FlashRate = target.FlashRate;
            TimeStamp = target.HitTime;
        }
    }
}
