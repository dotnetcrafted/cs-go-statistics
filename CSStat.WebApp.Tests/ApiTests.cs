using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using CSStat.CsLogsApi.Extensions;
using NUnit.Framework;

namespace CSStat.WebApp.Tests
{
    public class ApiTests
    {
        [Test]
        [TestCase(@"d:\Projects\counterstrikestat\Latest\CSStat.WebApp.Tests\TestParse\testString.txt")]
        public void Test(string file)
        {
            var logLine = string.Empty;

            using (var sr = new StreamReader(file) )
            {
                logLine = sr.ReadToEnd();
            }

            var splitLine = logLine.Split('"');

            var api = new CsStat.LogApi.CsLogsApi();

            var a = api.ParseLine(logLine);

            Console.WriteLine($"Incoming text: {logLine}");
            Console.WriteLine(Environment.NewLine);
            Console.WriteLine($"PlayerName: {a.PlayerName},Action: {a.Action},VictimName: {a.VictimName},Gun: {a.Gun.GetDescription()},IsHeadshot: {a.IsHeadShot},DateTime: {a.DateTime}".Replace(',','\n'));
        }
    }
}
