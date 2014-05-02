using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Targets;
using TargetServerCommunicator;
using TargetServerCommunicator.Servers;

namespace SadGUI.View_Models
{
    public class GameListViewModel: ViewModelBase
    {
        private IGameServer m_gameServer;
        public DelegateCommand ClearGameListCommand { get; private set; }
        public DelegateCommand LoadGameListCommand { get; private set; }

        public string ServerIP { get; set; }
        public int ServerPort { get; set; }

        public ObservableCollection<string> GameList { get; private set; }
        public string SelectedGame { get; set; }

        public GameServerType ServerType { get; set; }

        public GameListViewModel()
        {
            m_gameServer = null;

            GameList = new ObservableCollection<string>();

            Action clearGameListAction = ClearGameList;
            ClearGameListCommand = new DelegateCommand(clearGameListAction);
            ClearGameListCommand.Executable = false;

            Action loadGameList = LoadGameList;
            LoadGameListCommand = new DelegateCommand(loadGameList);
        }

        public void StartGame()
        {
            try
            {
                if (m_gameServer != null & ServerType == GameServerType.WebClient)
                    m_gameServer.StartGame(SelectedGame);
            }
            catch (System.Net.WebException)
            {
                MessageBox.Show("Could not connect to server to start game.");
            }
        }

        public void StopGame()
        {
            try
            {
                if (m_gameServer != null & ServerType == GameServerType.WebClient)
                    m_gameServer.StopRunningGame();
            }
            catch (System.Net.WebException)
            {
                MessageBox.Show("Could not connect to server to stop game.");
            }
        }

        void ClearGameList()
        {
            GameList.Clear();
            ClearGameListCommand.Executable = false;
            m_gameServer = null;
        }

        void LoadGameList()
        {
            ClearGameListCommand.Executable = true;
            if (ServerIP == "Mock" && ServerPort == 0)
            {
                ServerType = GameServerType.Mock;
                m_gameServer = GameServerFactory.Create(GameServerType.Mock, "The Fighting Mongooses", "", 0);
            }
            else
            {
                ServerType = GameServerType.WebClient;
                m_gameServer = GameServerFactory.Create(GameServerType.WebClient, "The Fighting Mongooses", ServerIP, ServerPort);
            }
            var list = new List<string>();
            try
            {
                list = m_gameServer.RetrieveGameList().ToList();
                SelectedGame = list[0];
                MessageBox.Show("Successful connection");
            }
            catch (System.Net.WebException)
            {
                MessageBox.Show("Invalid webserver.");
            }
            catch (FileLoadException)
            {
                MessageBox.Show("Could not parse JSon.");
            }
            GameList.Clear();
            foreach (var game in list)
            {
                GameList.Add(game);
            }
        }

        public List<Target> GetTargetList()
        {
            var targets = new List<Target>();
            if (SelectedGame == "" || m_gameServer == null) return targets;
            var targetList = new List<TargetServerCommunicator.Data.Target>();
            try
            {
                targetList = m_gameServer.RetrieveTargetList(SelectedGame).ToList();
            }
            catch (System.Net.WebException)
            {
                MessageBox.Show("Invalid webserver.");
            }

            foreach (var target in targetList)
            {
                var tar = new Target(target.name, (double) target.x, (double) target.y, (double) target.z, Convert.ToBoolean(target.status), (int) target.points, (int)target.spawnRate, target.hit);
                targets.Add(tar);
            }

            return targets;
        }
    }
}
