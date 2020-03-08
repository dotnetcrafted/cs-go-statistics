using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using Newtonsoft.Json;
using NUnit.Framework;
using ServerQueries.Source;


namespace CSStat.WebApp.Tests
{
    public class AchivementsTest
    {
        [Test(ExpectedResult = (List<AchievementModelTest>) null)]
        public List<AchievementModelTest> GetAchievements()
        {
            var json = GetJson("https://admin.csfuse8.site/achievements");
            var achievements = JsonConvert.DeserializeObject<List<AchievementModelTest>>(json);

            return new List<AchievementModelTest>();
        }

        [Test]
        public void TestQuery()
        {
            IQueryConnection conn = new QueryConnection();
            conn.Host = "192.168.100.241";
            conn.Port = 27015;

            conn.Connect();
            var info = conn.GetInfo();
            //var players = conn.GetPlayers();
            //var rules = conn.GetRules();
            //conn.Disconnect();
            Console.WriteLine(info.ToString());
        }

        private string GetJson(string url)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            try
            {
                WebResponse response = request.GetResponse();
                using (Stream responseStream = response.GetResponseStream())
                {
                    StreamReader reader = new StreamReader(responseStream, System.Text.Encoding.UTF8);
                    return reader.ReadToEnd();
                }
            }
            catch (WebException ex)
            {
                WebResponse errorResponse = ex.Response;
                using (Stream responseStream = errorResponse.GetResponseStream())
                {
                    StreamReader reader = new StreamReader(responseStream, System.Text.Encoding.GetEncoding("utf-8"));
                    return reader.ReadToEnd();
                }
            }


        }
    }
}