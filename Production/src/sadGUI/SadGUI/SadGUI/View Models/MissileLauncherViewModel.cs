using MissileLauncher;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Targets;

namespace SadGUI.View_Models
{
    public class MissileLauncherViewModel: ViewModelBase
    {
        private const double moveAmount = 5;
        private IMissileLauncher m_launcher;
        private string m_name;
        private int m_ammo;
        private int m_capacity;
        private double m_phi;
        private double m_theta;
        private bool m_manualControl;

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

        public int Ammo
        {
            get
            {
                return m_ammo;
            }
            set
            {
                m_ammo = value;
                OnPropertyChanged("Ammo");
            }
        }

        public int Capacity
        {
            get
            {
                return m_capacity;
            }
            set
            {
                m_capacity = value;
                OnPropertyChanged("Capacity");
            }
        }

        public double Phi
        {
            get
            {
                return m_phi;
            }
            set
            {
                m_phi = value;
                OnPropertyChanged("Phi");
            }
        }

        public double Theta
        {
            get
            {
                return m_theta;
            }
            set
            {
                m_theta = value;
                OnPropertyChanged("Theta");
            }
        }

        public DelegateCommand FireCommand { get; set; }
        public DelegateCommand UpCommand { get; set; }
        public DelegateCommand DownCommand { get; set; }
        public DelegateCommand LeftCommand { get; set; }
        public DelegateCommand RightCommand { get; set; }
        public DelegateCommand ReloadCommand { get; set; }

        public bool ManualControl
        {
            get
            {
                return m_manualControl;
            }
            set
            {
                m_manualControl = value;
                FireCommand.Executable = m_manualControl;
                UpCommand.Executable = m_manualControl;
                DownCommand.Executable = m_manualControl;
                LeftCommand.Executable = m_manualControl;
                RightCommand.Executable = m_manualControl;
            }
        }

        public MissileLauncherViewModel(MissileLauncherTypes launcherType)
        {
            m_launcher = MissileLauncherFactory.Create(launcherType);

            Action fireAction = Fire;
            FireCommand = new DelegateCommand(fireAction);

            Action upAction = Up;
            UpCommand = new DelegateCommand(upAction);

            Action downAction = Down;
            DownCommand = new DelegateCommand(downAction);

            Action leftAction = Left;
            LeftCommand = new DelegateCommand(leftAction);

            Action rightAction = Right;
            RightCommand = new DelegateCommand(rightAction);

            Action reloadAction = Reload;
            ReloadCommand = new DelegateCommand(reloadAction);


            Name = m_launcher.Name;
            Ammo = m_launcher.CurrentMissiles;
            Capacity = m_launcher.MaxMissiles;
            Phi = m_launcher.Phi;
            Theta = m_launcher.Theta;

            ManualControl = false;
        }

        public void Kill(TargetViewModel targetvm)
        {
            var target = targetvm.TargetInfo;
            if (target != null)
            {
                if (target.Friend)
                {
                    MessageBox.Show("Cannot kill that target. It is friendly.");
                    return;
                }
                double phi = Conversions.calcPhi(target.X, target.Y);
                double theta = Conversions.calcTheta(target.X, target.Y, target.Z);
                m_launcher.moveTo(phi, theta);
                Phi = m_launcher.Phi;
                Theta = m_launcher.Theta;
                try
                {
                    m_launcher.fire();
                    Ammo--;
                    targetvm.Alive = false;
                }
                catch (InvalidOperationException)
                {
                    MessageBox.Show("Launcher is out of ammo!");
                }
            }
        }

        void Fire()
        {
            try
            {
                m_launcher.fire();
                Ammo--;
            }
            catch (InvalidOperationException)
            {
                MessageBox.Show("Launcher is out of ammo!");
            }
        }

        void Up()
        {
            m_launcher.moveBy(0, moveAmount);
            Theta = m_launcher.Theta;
        }
        void Down()
        {
            m_launcher.moveBy(0, -1*moveAmount);
            Theta = m_launcher.Theta;
        }
        void Left()
        {
            m_launcher.moveBy(-1*moveAmount, 0);
            Phi = m_launcher.Phi;
        }

        void Right()
        {
            m_launcher.moveBy(moveAmount, 0);
            Phi = m_launcher.Phi;
        }

        void Reload()
        {
            m_launcher.reload();
            Ammo = m_launcher.MaxMissiles;
        }


    }
}
