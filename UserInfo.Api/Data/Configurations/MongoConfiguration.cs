namespace UserInfo.Api.Data.Configurations
{
    public class MongoConfiguration : IMongoConfiguration
    {
        public string ConnectionString { get; set; }
    }
}