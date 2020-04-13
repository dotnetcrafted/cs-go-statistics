using System;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using CsStat.StrapiApi;
using ServerQueries;
using ServerQueries.Models;
using ServerTools.Enums;
using ServerTools.Extensions;

namespace ServerTools
{
    public partial class frmMain : Form
    {
        private IStrapiApi _strapiApi;
        private IServerQueries _serverQueries;
        public frmMain()
        {
            InitializeComponent();
            _strapiApi = new StrapiApi();
            _serverQueries = new ServerQueries.ServerQueries();
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            cmbMap.DataSource = _strapiApi.GetAllMapInfos().Select(x => x.MapName).ToList();
            cmbMap.Text = "Map";
            Init();
        }

        private void Init()
        {
            
            var serverInfo =  _serverQueries.GetServerInfo();

            if (serverInfo.IsAlive)
            {
                txtConsole.WriteLine("Server is running");
                txtConsole.WriteLine($"Map: {serverInfo.Map}");
                txtConsole.WriteLine($"Players: {serverInfo.PlayersCount}");
                btnStart.Enabled = false;
                cmbMap.SelectedItem = serverInfo.Map;
            }
            else
            {
                txtConsole.WriteLine("Server is down" + Environment.NewLine, LineTypes.Error);
            }

        }
        
        private void btnStop_Click(object sender, EventArgs e)
        {
            txtConsole.WriteLine("Server has been stopped", LineTypes.Warning);
            btnStart.Enabled = true;
            btnStop.Enabled = false;
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            txtConsole.WriteLine("Server has been started", LineTypes.Success);
            btnStop.Enabled = true;
            btnStart.Enabled = false;
        }

        private void btnRestart_Click(object sender, EventArgs e)
        {
            txtConsole.WriteLine("Server has been restarted", LineTypes.Success);
        }
    }
}
