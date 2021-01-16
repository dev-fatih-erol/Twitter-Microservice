using System.Threading.Tasks;
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
 
        public async Task Create(Info info)
        {
            await _dbContext.Infos.InsertOneAsync(info);
        }
    }
}