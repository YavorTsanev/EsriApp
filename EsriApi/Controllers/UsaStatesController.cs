namespace EsriApi.Controllers
{
    using Dtos;
    using Microsoft.AspNetCore.Mvc;
    using Services.Contracts;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    [Route("api/[controller]")]
    [ApiController]
    public class UsaStatesController : ControllerBase
    {
        private readonly IUsaCountiesService _usaCountiesService;

        public UsaStatesController(IUsaCountiesService usaCountiesService)
        {
            _usaCountiesService = usaCountiesService;
        }

        [HttpGet("[action]")]
        public async Task<ICollection<UsaStateResponseModel>> GetAllAsync()
        {
            return await _usaCountiesService.GetAllAsync();
        }

        [HttpGet("[action]")]
        public async Task<ActionResult<UsaStateResponseModel>> GetByStateNameAsync(string stateName)
        {
            if (stateName == null)
            {
                return BadRequest("State name is empty");
            }
            var result = await _usaCountiesService.GetByStateNameAsync(stateName);
            return result ?? new UsaStateResponseModel();
        }
    }
}
