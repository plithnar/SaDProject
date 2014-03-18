using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SadGUI
{
    public class DelegateCommand : ICommand
    {
        public bool m_canExecute;

        public DelegateCommand(Action actionToTake)
        {
            m_canExecute = true;
            m_action = actionToTake;
        }
        public bool CanExecute(object parameter)
        {
            return m_canExecute;
        }

        public event EventHandler CanExecuteChanged;
        private Action m_action;

        public void Execute(object parameter)
        {
            m_action();
        }
    }
}
