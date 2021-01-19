using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using UserInfo.Api.Data.Services;

namespace UserInfo.Api.Controllers
{
    public class InfoController : Controller
    {
        private readonly IInfoService _infoService;

        public InfoController(IInfoService infoService)
        {
            _infoService = infoService;
        }

        [HttpGet]
        [Route("Info/{userId:int}")]
        public async Task<IActionResult> GetByUserId(int userId)
        {
            var info = await _infoService.GetByUserId(userId);

            if (info != null)
            {
                return Ok(info);
            }

            return NotFound();
        }
    }
}