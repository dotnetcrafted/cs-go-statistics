using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using MongoRepository.DAL;

namespace DataService
{
    public class MongoReference<T> where T : Entity
    {
        private readonly string typeName = typeof(T).Name;
        public MongoDBRef Ref { get; set; }

        [BsonIgnore]
        public T Value
        {
            get
            {
                var value = new MongoRepository<T>().GetById(Ref.Id.ToString());
                return value;
            }
        }

        protected MongoReference() { }
        public MongoReference(T value)
        {
            Ref = new MongoDBRef(typeName, new ObjectId(value.Id));
        }

        private sealed class TypeNameRefEqualityComparer : IEqualityComparer<MongoReference<T>>
        {
            public bool Equals(MongoReference<T> x, MongoReference<T> y)
            {
                if (ReferenceEquals(x, y)) return true;
                if (ReferenceEquals(x, null)) return false;
                if (ReferenceEquals(y, null)) return false;
                if (x.GetType() != y.GetType()) return false;
                return string.Equals(x.typeName, y.typeName) && x.Ref.Equals(y.Ref);
            }

            public int GetHashCode(MongoReference<T> obj)
            {
                unchecked
                {
                    return (obj.typeName.GetHashCode() * 397) ^ obj.Ref.GetHashCode();
                }
            }
        }

        private static readonly IEqualityComparer<MongoReference<T>> TypeNameRefComparerInstance = new TypeNameRefEqualityComparer();

        public static IEqualityComparer<MongoReference<T>> TypeNameRefComparer
        {
            get { return TypeNameRefComparerInstance; }
        }
    }
}