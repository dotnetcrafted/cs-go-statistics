using System.Collections.Generic;
using System.IO;
using System.Net;
using CsStat.Domain;
using CsStat.Domain.Models;
using Newtonsoft.Json;

namespace CsStat.StrapiApi
{
    public class StrapiApi : IStrapiApi
    {
        public string GetArticles()
        {
            return GetJsonFromUrl(Settings.ArticlesPath);
        }

        public List<AchieveModel> GetAchieves()
        {
            var json = GetJsonFromUrl(Settings.AchievementsPath);

            return string.IsNullOrEmpty(json)
                ? new List<AchieveModel>()
                : JsonConvert.DeserializeObject<List<AchieveModel>>(json);
        }

        private string GetJsonFromUrl(string url)
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
