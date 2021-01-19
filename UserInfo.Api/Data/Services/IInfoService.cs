using System.Threading.Tasks;
using UserInfo.Api.Data.Entities;

namespace UserInfo.Api.Data.Services
{
    public interface IInfoService
    {
        Task<Info> GetByUserId(int userId);

        Task Create(Info info);
    }
}