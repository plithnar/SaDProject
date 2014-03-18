using MissileLauncher;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SadGUI.View_Models
{
    public class MissileLauncherViewModel: ViewModelBase
    {
        IMissileLauncher m_launcher;
        public DelegateCommand FireCommand { get; set; }
        public DelegateCommand UpCommand { get; set; }
        public DelegateCommand DownCommand { get; set; }
        public DelegateCommand LeftCommand { get; set; }
        public DelegateCommand RightCommand { get; set; }
        public DelegateCommand ReloadCommand { get; set; }

        public MissileLauncherViewModel(IMissileLauncher launcher)
        {
            m_launcher = launcher;

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
        }

        void Fire()
        {
            try
            {
                m_launcher.fire();
            }
            catch (InvalidOperationException)
            {
                MessageBox.Show("Launcher is out of ammo!");
            }
        }

        void Up()
        {
            m_launcher.moveBy(0, 5);
        }
        void Down()
        {
            m_launcher.moveBy(0, -5);
        }
        void Left()
        {
            m_launcher.moveBy(-5, 0);
        }

        void Right()
        {
            m_launcher.moveBy(5, 0);
        }

        void Reload()
        {
            m_launcher.reload();
        }
    }
}
