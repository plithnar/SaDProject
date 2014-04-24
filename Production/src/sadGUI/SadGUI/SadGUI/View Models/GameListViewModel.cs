﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
                m_gameServer = GameServerFactory.Create(GameServerType.Mock, "The Fighting Mongooses", "", 0);
            }
            else
            {
                m_gameServer = GameServerFactory.Create(GameServerType.WebClient, "The Fighting Mongooses", ServerIP, ServerPort);
            }
            var list = new List<string>();
            try
            {
                list = m_gameServer.RetrieveGameList().ToList();
            }
            catch (System.Net.WebException)
            {
                MessageBox.Show("Invalid webserver.");
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
                var tar = new Target(target.name, (double) target.x, (double) target.y, (double) target.z, false, (int) target.points, 0);
                targets.Add(tar);
            }

            return targets;
        }
    }
}
