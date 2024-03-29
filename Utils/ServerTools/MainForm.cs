﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using AutoMapper;
using BusinessFacade.Repositories;
using BusinessFacade.Repositories.Implementations;
using CsStat.Domain;
using CsStat.Domain.Entities.ServerTools;
using CsStat.LogApi;
using CsStat.StrapiApi;
using CsStat.SystemFacade.Extensions;
using DataService;
using DemoInfo;
using ErrorLogger;
using ReadFile.ReadDemo;
using ReadFile.ReadDemo.Profiles;
using ReadFile.SingleFileReader;
using ServerTools.Enums;
using ServerTools.Extensions;

namespace ServerTools
{
    public partial class frmMain : Form
    {
        private IStrapiApi _strapiApi;
        private IServerToolsRepository _serverToolsRepository;
        private ServerToolsSettings _settings;
        private static ILogger _logger;
        private ServerCommands _serverCommands;
        private Progress<string> _progress;

        private static MapperConfiguration Config =>
            new MapperConfiguration(cfg => { cfg.AddProfile<DemoProfile>(); });


        public frmMain()
        {
            InitializeComponent();
            _strapiApi = new StrapiApi();
            var mongoRepositoryFactory = new MongoRepositoryFactory(new ConnectionStringFactory());
            _serverToolsRepository = new ServerToolsRepository(mongoRepositoryFactory);
            _logger = new Logger(mongoRepositoryFactory);
        }

        private async void frmMain_Load(object sender, EventArgs e)
        {
            try
            {
                _settings = _serverToolsRepository.GetSettings();
                _serverCommands = new ServerCommands(_settings);
                InitSettings();
                _progress = new Progress<string>(info => { txtConsole.WriteLine(info); });
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Init error");
            }

            try
            {
                RestartServer();
                timerRestart.Start();
                timerChangeMap.Start();
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Start server error");
            }


            try
            {
                var progressLogReader = new Progress<string>(info => { txtLogReader.WriteLine(info); });
                await Task.Run(() => LogReader(progressLogReader));
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Start log parser error");
            }

            try
            {
                var progressDemoReader = new Progress<string>(info =>
                {
                    var lineType = LineTypes.Text;

                    if (info.Contains(Team.Terrorist.ToString()))
                    {
                        lineType = LineTypes.TerroristWin;
                    }

                    if (info.Contains(Team.CounterTerrorist.ToString()))
                    {
                        lineType = LineTypes.CtWin;
                    }

                    if (info.ToLower().Contains("exception"))
                    {
                        lineType = LineTypes.Error;
                    }

                    txtDemoReader.WriteLine(info, lineType);
                });

                await Task.Run(() => Demo(progressDemoReader));
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Start demo parser error");
            }
        }
        
        private void InitSettings()
        {
            chkAutoUpdate.Checked = _settings.AutoUpdate;
            chkAutoRestart.Checked = _settings.AutoRestart;
            listPool.BindDataSource(_settings.MapPool);
            timePickerLunch.Text = _settings.RestartTime[0];
            timePickerEvening.Text = _settings.RestartTime[1];
            dateChangeDay.Value = _settings.ChangeDay.ToDate(DateTime.MinValue);
            txtStart.Text = _settings.StartServer;
            txtStop.Text = _settings.StopServer;
            txtUpdate.Text = _settings.UpdateServer;
            numDays.Value = _settings.PlayingDays;
            var maps = _strapiApi.GetAllMapInfos().Select(x => x.MapName).ToList();
            cmbMap.DataSource = maps;
            cmbMap.Text = _settings.CurrentMap;
            listAll.BindDataSource(maps.Except(listPool.ToList()).ToList());
        }

        private void LogReader(IProgress<string> progress)
        {
            progress.Report("Start");

            var parser = new CsLogsApi();
            var logRepository = new BaseRepository(new MongoRepositoryFactory(new ConnectionStringFactory()));
            var fileRepository = new LogFileRepository(new MongoRepositoryFactory(new ConnectionStringFactory()));

            progress.Report($"Read logs from \"{Settings.ConsoleLogsPath}\"");

            var watcher = new Reader(Settings.ConsoleLogsPath, parser, logRepository, fileRepository, progress);

            watcher.Start();
        }

        private void Demo(IProgress<string> progress)
        {
            progress.Report("Start");
            progress.Report($"Reading demo files from \"{Settings.DemosFolderPath}\" folder");

            var demoFileRepository = new DemoFileRepository(new MongoRepositoryFactory(new ConnectionStringFactory()));
            var demoRepository = new BaseRepository(new MongoRepositoryFactory(new ConnectionStringFactory()));

            var demoReader = new DemoReader(Settings.DemosFolderPath, demoFileRepository, demoRepository,
                Config.CreateMapper(), progress);

            demoReader.Start(Settings.TimerInterval);
        }

        private async void btnStop_Click(object sender, EventArgs e)
        {
            try
            {
                var task = await Task.Run(() => _serverCommands.StopServer(_progress));

                if (task == 0)
                {
                    btnStart.Enabled = true;
                    btnStop.Enabled = false;
                    txtConsole.WriteLine("Server has been stopped", LineTypes.Warning);
                }
            }
            catch (Exception ex)
            {
                txtConsole.WriteLine(ex.Message, LineTypes.Error);
            }
        }

        private async void btnStart_Click(object sender, EventArgs e)
        {
            var map = cmbMap.Text;

            if (map == "Map")
            {
                txtConsole.WriteLine("Please select the map", LineTypes.Error);
                cmbMap.Focus();
                cmbMap.DroppedDown = true;
                return;
            }

            try
            {
                txtConsole.WriteLine("Server has been started", LineTypes.Success);
                btnStop.Enabled = true;
                btnStart.Enabled = false;
                _settings.CurrentMap = map;
                _serverToolsRepository.SaveSettings(_settings);

                await Task.Run(() => _serverCommands.StartServer(_progress, map));
            }
            catch (Exception ex)
            {
                txtConsole.WriteLine(ex.Message, LineTypes.Error);
            }

        }

        private async void btnUpdate_Click(object sender, EventArgs e)
        {
            var map = cmbMap.Text;
            try
            {
                var stop = await Task.Run(() => _serverCommands.StopServer(_progress));

                if (stop == 0)
                {
                    var update = await Task.Run(() => _serverCommands.UpdateServer(_progress));

                    if (update == 0)
                    {
                        btnStop.Enabled = true;
                        btnStart.Enabled = false;
                        await Task.Run(() => _serverCommands.StartServer(_progress, map));
                    }
                }
            }
            catch (Exception ex)
            {
                txtConsole.WriteLine(ex.Message, LineTypes.Error);
            }
        }

        private void OnHover(object sender, EventArgs e)
        {
            var label = (Label) sender;

            label.ForeColor = Color.DeepSkyBlue;
        }

        private void OnLeave(object sender, EventArgs e)
        {
            var label = (Label) sender;

            label.ForeColor = Color.Black;
        }

        private void lblDown_Click(object sender, EventArgs e)
        {
            listPool.MoveSelectedItemDown();
        }

        private void lblUp_Click(object sender, EventArgs e)
        {
            listPool.MoveSelectedItemUp();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            var times = new List<string> { timePickerLunch.Text, timePickerEvening.Text };
            _settings.AutoRestart = chkAutoRestart.Checked;
            _settings.AutoUpdate = chkAutoUpdate.Checked;
            _settings.MapPool = listPool.ToList();
            _settings.RestartTime = times;
            _settings.StartServer = txtStart.Text;
            _settings.StopServer = txtStop.Text;
            _settings.UpdateServer = txtUpdate.Text;
            _settings.PlayingDays = (int) numDays.Value;
            _settings.ChangeDay = dateChangeDay.Value.ToShortDateString();

            try
            {
                _serverToolsRepository.SaveSettings(_settings);
                lblStatus.Text = "Settings has been saved";
                timerStatus.Start();
            }
            catch (Exception ex)
            {
                var message = "Error occurred during saving server tools settings";
                lblStatus.Text = message;
                _logger.Error(ex, message);
            }

        }

        private void lblAddOne_Click(object sender, EventArgs e)
        {
            if (listAll.SelectedItem == null || listAll.SelectedIndex < 0)
                return;

            var selected = listAll.SelectedItem;

            listPool.Items.Insert(listPool.Items.Count, selected);
            listAll.Items.Remove(selected);
        }

        private void lblRemoveOne_Click(object sender, EventArgs e)
        {
            if (listPool.SelectedItem == null || listPool.SelectedIndex < 0)
                return;

            var selected = listPool.SelectedItem;

            listAll.Insert(selected);
            listPool.Items.Remove(selected);
        }

        private void lblAddAll_Click(object sender, EventArgs e)
        {
            if(listAll.Items.Count < 1)
                return;

            foreach (var item in listAll.Items)
            {
                listPool.Insert(item);
            }

            listAll.Items.Clear();
        }

        private void timerStatus_Tick(object sender, EventArgs e)
        {
            lblStatus.Text = "Ready";
        }

        private void lblClearAll_Click(object sender, EventArgs e)
        {
            ClearList();
        }

        private void lblResetMapPool_Click(object sender, EventArgs e)
        {
            ClearList();
            var time = DateTime.Today.AddDaysExcludeWeekends((int)numDays.Value);
            _settings.ChangeDay = time.ToShortDateString();
            dateChangeDay.Value = time;
        }

        private void ClearList()
        {
            if (listPool.Items.Count < 1)
                return;

            foreach (var item in listPool.Items)
            {
                listAll.Insert(item);
            }

            listPool.Items.Clear();
        }
        
        private void txtConsole_TextChanged(object sender, EventArgs e)
        {
            txtConsole.ScrollToCaret();

            if (!(sender is RichTextBox item))
            {
                return;
            }

            if (item.LastLine().Contains("Please update and restart") && _settings.AutoUpdate)
            {
                btnUpdate.PerformClick();
            }

            if (item.LastLine().Contains("CUtlLinkedList overflow"))
            {
                RestartServer();
            }
        }

        private void timerRestart_Tick(object sender, EventArgs e)
        {
            if (!_settings.AutoRestart)
            {
                timerRestart.Start();
                return;
            }
            
            if (DateTime.Now.ToShortTimeFormat().GetMinutes() == _settings.RestartTime[0].GetMinutes()  
                || DateTime.Now.ToShortTimeFormat().GetMinutes() == _settings.RestartTime[1].GetMinutes())
            {
                try
                {
                    RestartServer();
                }
                catch (Exception ex)
                {
                    txtConsole.WriteLine(ex.Message, LineTypes.Error);
                }
            }

            timerRestart.Start();
        }

        private async void RestartServer()
        {
            timerRestart.Stop();
            txtConsole.Clear();
            txtConsole.WriteLine("Restart server", LineTypes.Warning);
            var map = cmbMap.Text;
            var stop = await Task.Run(() => _serverCommands.StopServer(_progress));

            if (stop == 0)
            {
                btnStop.Enabled = true;
                btnStart.Enabled = false;
                await Task.Run(() => _serverCommands.StartServer(_progress, map));
            }
        }

        private void ChangeMap()
        {
            if (!string.Equals(DateTime.Now.ToShortDateString(), _settings.ChangeDay, StringComparison.InvariantCultureIgnoreCase))
            {
                return;
            }

            cmbMap.Text = listPool.NextItem(cmbMap.Text);
            RestartServer();
            _settings.ChangeDay = DateTime.Today.AddDaysExcludeWeekends(_settings.PlayingDays).ToShortDateString();
            _settings.CurrentMap = cmbMap.Text;
            _serverToolsRepository.SaveSettings(_settings);
            dateChangeDay.Value = _settings.ChangeDay.ToDate(DateTime.MinValue);
            timerChangeMap.Start();
        }

        private void timerChangeMap_Tick(object sender, EventArgs e)
        {
            ChangeMap();
        }

        private void txtLogReader_TextChanged(object sender, EventArgs e)
        {
            txtLogReader.ScrollToCaret();
        }

        private void txtDemoReader_TextChanged(object sender, EventArgs e)
        {
            txtDemoReader.ScrollToCaret();
        }

        private void tabControl_TabIndexChanged(object sender, EventArgs e)
        {
        }

        private void frmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (MessageBox.Show("Do you want to close the application?", "CS: GO Server Tools",
                    MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                Task.Run(() => _serverCommands.StopServer(_progress));
                e.Cancel = false;
            }
            else
            {
                e.Cancel = true;
            }
        }

        private void tabControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtConsole.Refresh();
            txtConsole.Update();
            txtDemoReader.Refresh();
            txtDemoReader.Update();
            txtLogReader.Refresh();
            txtLogReader.Update();
        }

        private void button1_Click(object sender, EventArgs e)
        {
        }

        private void dateChangeDay_ValueChanged(object sender, EventArgs e)
        {

        }
    }
}
