using System.Collections.Generic;
using CsStat.Domain.Definitions;
using CsStat.Domain.Models;

namespace CsStat.StrapiApi
{
    public interface IStrapiApi
    {
        string GetArticles();
        List<AchieveModel> GetAchieves();
        MapInfoModel GetMapInfo(string mapName);
        List<MapInfoModel> GetAllMapInfos();
        ImageInfoModel GetImage(string imageName);
        List<ImageInfoModel> GetAllImages();
        List<WeaponModel> GetAllWeapons();
        WeaponModel GetWeapon(Weapons weapon);
    }
}