using AngularClient.Models;
using AngularClient.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace AngularClient.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StoriesController : ControllerBase
    {
        private StoryRepository _storyRep;

        public StoriesController(IStoryService storyService)
        {
            _storyRep = new StoryRepository(storyService);
        }

        [HttpGet]
        public async Task<ActionResult<StoryResponse>> Get([FromQuery] StoryParameters storyParameters)
        {
            StoryResponse response = new StoryResponse();
            var result = await _storyRep.GetStories(storyParameters);

            response.Stories = result;
            response.Parameters = storyParameters;

            return Ok(response);
        }
    }
}
