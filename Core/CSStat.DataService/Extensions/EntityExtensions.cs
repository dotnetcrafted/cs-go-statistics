using MongoRepository.DAL;

namespace DataService.Extensions
{
    public static class EntityExtensions
    {
        public static MongoReference<T> AsReference<T>(this T value) where T : Entity
        {
            return new MongoReference<T>(value);
        }
    }
}