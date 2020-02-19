using System.Collections.Generic;
using CsStat.Domain.Models;

namespace CsStat.StrapiApi
{
    public interface IStrapiApi
    {
        string GetArticles();
        List<AchieveModel> GetAchieves();
    }
}