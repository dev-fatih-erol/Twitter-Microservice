using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace UserInfo.Api.Data.Entities
{
    public class Info
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public int Tweets { get; set; }

        public int Following { get; set; }

        public int Followers { get; set; }

        public int Likes { get; set; }

        public int UserId { get; set; }
    }
}