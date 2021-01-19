using System.Threading.Tasks;
using MongoDB.Driver;
using UserInfo.Api.Data.Entities;

namespace UserInfo.Api.Data.Services
{
    public class InfoService : IInfoService
    {
        private readonly UserInfoDbContext _dbContext;

        public InfoService(UserInfoDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Info> GetByUserId(int userId)
        {
            return await _dbContext.Infos.Find(c => c.UserId == userId).SingleOrDefaultAsync();
        }

        public async Task Create(Info info)
        {
            await _dbContext.Infos.InsertOneAsync(info);
        }
    }
}