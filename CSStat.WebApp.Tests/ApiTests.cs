using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using CSStat.CsLogsApi.Extensions;
using CsStat.Domain.Definitions;
using NUnit.Framework;

namespace CSStat.WebApp.Tests
{
    public static class ApiTests
    {
        [Test]
        [TestCase(@"d:\Projects\counterstrikestat\Latest\CSStat.WebApp.Tests\TestParse\testString.txt")]
        public static void Test(string file)
        {
            var logLine = string.Empty;

            using (var sr = new StreamReader(file))
            {
                logLine = sr.ReadToEnd();
            }

            var splitLine = logLine.Split('"').ToList();

            var api = new CsStat.LogApi.CsLogsApi();

            var a = api.ParseLine(logLine);

            Console.WriteLine($"Incoming text: {logLine}");
            Console.WriteLine(Environment.NewLine);
            splitLine.ForEach(Console.WriteLine);
            Console.WriteLine(Environment.NewLine);
            Console.WriteLine(
                $"PlayerName: {a.PlayerName},PlayerTeam: {a.PlayerTeam.GetDescription()},Action: {a.Action},VictimName: {a.VictimName},VictimTeam: {a.VictimTeam.GetDescription()},Gun: {a.Gun.GetDescription()},IsHeadshot: {a.IsHeadShot},DateTime: {a.DateTime}"
                    .Replace(',', '\n'));
        }

        [Test]
        [TestCase(@"d:\Projects\counterstrikestat\Latest\CSStat.WebApp.Tests\TestParse\logs.txt")]
        public static void GetWeapons(string file)
        {
            string logs;

            using (var sr = new StreamReader(file))
            {
                logs = sr.ReadToEnd();
            }

            var splitLogs = logs.Split('\n').ToList();
            var a = new List<string>();

            foreach (var log in splitLogs)
            {
                var splitLog = log.Split('"');
                if (splitLog.Length < 6)
                {
                    continue;
                }

                a.Add(splitLog[5]);
            }

            a.FilterInt().Distinct().ToList().ForEach(Console.WriteLine);

        }

        [Test]
        public static void AttributeTest()
        {
            var gun = Guns.Ak;
            var attributeList = gun.GetAttributeList();

            foreach (var attribute in attributeList)
            {
                Console.WriteLine($"{attribute.Key},{attribute.Value}");
            }

        }

            private static IEnumerable<string> FilterInt(this IEnumerable<string> list)
        {
            foreach (var item in list)
            {
                if (!int.TryParse(item, out var i))
                {
                    yield return item;
                }
            }
        }
    }
}

