using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SadGUI.View_Models
{
    public enum Modes { Manual, Automatic }
    public class ModeSelectorViewModel: ViewModelBase
    {
        public Modes m_selected;

        public IEnumerable<Modes> ModeTypes { get; set; }
        public Modes SelectedMode
        { 
            get
            {
                return m_selected;
            }
            set
            {
                m_selected = value;
                if (ModeChanged != null)
                    ModeChanged(this, null);
            }
        }

        public event EventHandler ModeChanged;

        public ModeSelectorViewModel()
        {
            SelectedMode = Modes.Manual;

            ModeTypes = Enum.GetValues(typeof (Modes)).OfType<Modes>().ToArray();
        }
    }
}
