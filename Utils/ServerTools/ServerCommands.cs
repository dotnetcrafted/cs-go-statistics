using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using CsStat.Domain;
using CsStat.Domain.Entities.ServerTools;
using CsStat.SystemFacade.Extensions;

namespace ServerTools
{
    public class ServerCommands
    {
        private static readonly string MapCycleFile = $@"{Settings.CsPath}\server\csgo\mapcycle.txt";

        private ServerToolsSettings _settings;

        public ServerCommands(ServerToolsSettings settings)
        {
            _settings = settings;
        }

        public async Task<int> StartServer(IProgress<string> progress, string map)
        {
            using (var stream = new StreamWriter(MapCycleFile, false))
            {
                stream.Write(map);
            }

            return await Task.Run(() => ExecuteBatch($@"{Settings.CsPath}\server\srcds.exe", _settings.StartServer.Replace("%1", map), progress));
        }

        public async Task<int> StopServer(IProgress<string> progress)
        {
            return await Task.Run(() => ExecuteBatch("cmd.exe", _settings.StopServer, progress));
        }

        public async Task<int> UpdateServer(IProgress<string> progress)
        {
          return await Task.Run(() => ExecuteBatch("cmd.exe", _settings.UpdateServer.Replace("%1", Settings.CsPath), progress));
        }

        private int ExecuteBatch(string command, string arguments = "", IProgress<string> progress = null, bool isNeedWaitExit = true)
        {
            if (progress == null)
            {
                return -1;
            }
            var processInfo = new ProcessStartInfo(command, $"/c {arguments}")
            {
                CreateNoWindow = true,
                UseShellExecute = false,
                RedirectStandardError = true,
                RedirectStandardOutput = true,
                WindowStyle = ProcessWindowStyle.Minimized
            };

            var process = Process.Start(processInfo);

            if (process == null)
            {
                return -1;
            }

            process.OutputDataReceived += (sender, e) =>
            {
                if (e.Data.IsNotEmpty())
                {
                    progress.Report(e.Data);
                }
            };

            process.BeginOutputReadLine();
            process.ErrorDataReceived += (sender, e) => progress.Report(e.Data);
            process.BeginErrorReadLine();

            if (!isNeedWaitExit) 
                return 0;

            process.WaitForExit();
            process.Close();
            return 0;
        }
    }
}