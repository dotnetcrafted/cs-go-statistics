using CsStat.Domain.Models;

namespace CsStat.Domain.Extensions
{
    public static class AchieveModelExtensions
    {
        public static AchieveModel ChangeDescription(this AchieveModel achieve, object value)
        {
           achieve.Description = string.Format(achieve.Description, value);
           return achieve;
        }
    }
}