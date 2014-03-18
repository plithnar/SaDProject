using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace SadGUI
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            MainWindow window = new MainWindow();

/*            var targets = new List<Target>();

            for (int i = 0; i < 4; i++)
            {
                var target = new Target("target " + i, i, 5, 5, true, 10, 1);
                targets.Add(target);
            }*/

            MainViewModel viewModel = new MainViewModel();

            window.DataContext = viewModel;
            window.ShowDialog();
        }
    }
}
