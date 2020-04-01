namespace CSStat.WebApp.Tests
{
    public class AchievementModelTest
    {
        
        public string AchievementId { get; set; }
        
        public string Name { get; set; }

        public string Description { get; set; }
        public AchieveIcon Icon { get; set; }
    }

    public class AchieveIcon
    {
        public string Url { get; set; }
    }
}