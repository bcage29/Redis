using Microsoft.AspNetCore.Mvc;
using SampleAPI.Contracts;
using SampleAPI.Helpers;
using System.Threading.Tasks;

namespace SampleAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class CacheController : ControllerBase
    {
        private readonly ICacheService _cacheService;

        public CacheController(ICacheService cacheService)
        {
            _cacheService = cacheService;
        }

        [HttpGet]
        public async Task<string> Add()
        {
            var rand = StringHelper.RandomString(5);

            var addSuccess = await _cacheService.AddAsync(rand, rand);
            if (addSuccess)
                return rand;
            else
                return false.ToString();
        }

        [HttpGet("{id}")]
        public async Task<string> Get(string id)
        {
            return await _cacheService.GetAsync(id);
        }
    }
}
