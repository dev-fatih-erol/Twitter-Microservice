using MongoDB.Driver;
using UserInfo.Api.Data.Configurations;
using UserInfo.Api.Data.Entities;

namespace UserInfo.Api.Data
{
    public class UserInfoDbContext
    {
        private readonly IMongoDatabase _database;

        public UserInfoDbContext(IMongoConfiguration configuration)
        {
            var client = new MongoClient(configuration.ConnectionString);

            _database = client.GetDatabase(MongoUrl.Create(configuration.ConnectionString).DatabaseName);
        }

        public IMongoCollection<Info> Infos => _database.GetCollection<Info>(nameof(Info));
    }
}