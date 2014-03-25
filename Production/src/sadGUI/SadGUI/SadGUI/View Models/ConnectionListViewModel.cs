using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SadGUI.View_Models
{
    public class ConnectionListViewModel: ViewModelBase
    {
        public DelegateCommand NewConnectionCommand { get; set; }
        public DelegateCommand DisconnectCommand { get; set; }
    
        public ConnectionListViewModel()
        {
            NewConnectionCommand = new DelegateCommand(null);
            NewConnectionCommand.Executable = false;

            DisconnectCommand = new DelegateCommand(null);
            DisconnectCommand.Executable = false;
        }
    }
}
