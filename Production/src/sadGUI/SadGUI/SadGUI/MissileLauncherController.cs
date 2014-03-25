using MissileLauncher;
using SadGUI.View_Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SadGUI
{
    public class MissileLauncherController
    {
        private static MissileLauncherController m_missileLauncherController;

        private MissileLauncherViewModel m_launcher;

        public MissileLauncherViewModel Launcher
        {
            get
            {
                return m_launcher;
            }
            private set
            {
                m_launcher = value;
                if (LauncherChanged != null)
                    LauncherChanged();
            }
        }

        public delegate void LauncherChangeAlert();

        public event LauncherChangeAlert LauncherChanged;

        public MissileLauncherTypes LauncherType
        {
            get
            {
                return m_launcherType;
            }
            set
            {
                m_launcherType = value;
                Launcher = new MissileLauncherViewModel(value);
            }
        }
        public static MissileLauncherController Instance
        {
            get
            {
                if (m_missileLauncherController == null)
                {
                    m_missileLauncherController = new MissileLauncherController();
                }
                return m_missileLauncherController;
            }
        }

        private MissileLauncherController()
        {
            Launcher = null;
        }

        public MissileLauncherTypes m_launcherType { get; set; }
    }
}
