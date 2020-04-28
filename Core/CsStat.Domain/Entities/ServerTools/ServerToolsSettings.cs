using System;
using System.Collections.Generic;
using MongoRepository;

namespace CsStat.Domain.Entities.ServerTools
{
    public class ServerToolsSettings : Entity
    {
        public bool AutoUpdate { get; set; }
        public bool AutoRestart { get; set; }
        public List<string> RestartTime { get; set; }
        public List<string> MapPool { get; set; }
        public string StartDate { get; set; }
        public string StartServer { get; set; }
        public string StopServer { get; set; }
        public string UpdateServer { get; set; }
        public int PlayingDays { get; set; }
        public string CurrentMap { get; set; }

        public ServerToolsSettings()
        {
            AutoUpdate = AutoRestart = true;
            RestartTime = new List<string>{"12:05","17:55"};
            MapPool = new List<string>();
            StartDate = DateTime.Today.ToShortDateString();
            StartServer = "-game csgo -console -usercon -maxplayers_override 32 -tickrate 64 +map %1 +ip 192.168.100.241 " 
                          + "-port 27015  +game_type 0 +game_mode 1 -secure +sv_lan 1 -net_port_try 1 -nobots -condebug " 
                          + "-autoupdate +sv_setsteamaccount AD0745F164F73F8C394A88AB3B71A92F";

            StopServer = "taskkill /f /im srcds.exe";
            UpdateServer = @"%1\steamcmd\steamcmd.exe +login anonymous +force_install_dir ../server/ +app_update 740 +quit";
            PlayingDays = 3;
            CurrentMap = "";
        }
    }
}