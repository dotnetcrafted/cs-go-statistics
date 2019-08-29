using MongoRepository;

namespace CsStat.Domain.Entities
{
    public class Player : Entity, IBaseEntity
    {
        public string NickName { get; set; }
        public string FirstName { get; set; }
        public string SecondName { get; set; }
        public string FullName => $"{FirstName} {NickName} {SecondName}";
        public string ImagePath { get; set; }
    }
}