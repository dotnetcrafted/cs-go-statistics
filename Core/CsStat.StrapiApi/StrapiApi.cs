using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using CsStat.Domain;
using CsStat.Domain.Definitions;
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

        public List<MapInfoModel> GetAllMapInfos()
        {
            var json = GetJsonFromUrl(Settings.MapInfoPath);

            return string.IsNullOrEmpty(json)
                ? new List<MapInfoModel>()
                : JsonConvert.DeserializeObject<List<MapInfoModel>>(json);
        }

        public MapInfoModel GetMapInfo(string mapName)
        {
            return GetAllMapInfos().FirstOrDefault(x => x.MapName.ToLower().Equals(mapName.ToLower())) ?? new MapInfoModel();
        }

        public ImageInfoModel GetImage(string imageName)
        {
            return GetAllImages().FirstOrDefault(x => x.CodeName.ToLower().Equals(imageName.ToLower())) ?? new ImageInfoModel();
        }

        public List<ImageInfoModel> GetAllImages()
        {
            var json = GetJsonFromUrl(Settings.ImagesPath);

            return string.IsNullOrEmpty(json)
                ? new List<ImageInfoModel>()
                : JsonConvert.DeserializeObject<List<ImageInfoModel>>(json);

        }

        public List<WeaponModel> GetAllWeapons()
        {
            var json = GetJsonFromUrl(Settings.WeaponsPath);

            return string.IsNullOrEmpty(json)
                ? new List<WeaponModel>()
                : JsonConvert.DeserializeObject<List<WeaponModel>>(json);
        }

        public WeaponModel GetWeapon(Weapons weapon)
        {
            return GetAllWeapons().FirstOrDefault(x => x.Id == (int) weapon);
        }

        private string GetJsonFromUrl(string url)
        {
            var request = (HttpWebRequest)WebRequest.Create(url);
            try
            {
                var response = request.GetResponse();
                using (var responseStream = response.GetResponseStream())
                {
                    var reader = new StreamReader(responseStream, System.Text.Encoding.UTF8);
                    return reader.ReadToEnd();
                }
            }
            catch (WebException ex)
            {
                var errorResponse = ex.Response;
                using (var responseStream = errorResponse.GetResponseStream())
                {
                    var reader = new StreamReader(responseStream, System.Text.Encoding.GetEncoding("utf-8"));
                    return reader.ReadToEnd();
                }
            }
        }
    }
}
