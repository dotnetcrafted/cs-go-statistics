using System.Collections.Generic;
using System.IO;
using System.Net;
using Newtonsoft.Json;
using NUnit.Framework;

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