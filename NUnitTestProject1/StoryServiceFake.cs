using AngularClient.Models;
using AngularClient.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NUnitTestProject1
{
    public class StoryServiceFake : IStoryService
    {
        private List<StoryDTO> stories;

        public StoryServiceFake()
        {
            stories = new List<StoryDTO>
            {
                new StoryDTO
                {
                    by = "danielhochman",
                    decendents = 0,
                    id = 1,
                    score = 1,
                    time = 1595430684,
                    title = "Clutch, the Open-Source Platform for Infrastructure Tooling",
                    type = "story",
                    url = "https://eng.lyft.com/announcing-clutch-the-open-source-platform-for-infrastructure-tooling-143d00de9713"
                },
                new StoryDTO
                {
                    by = "sarahkaren2208",
                    decendents = 0,
                    id = 2,
                    score = 1,
                    time = 1595430674,
                    title = "Startup Is Starting Commotion with It's Branding",
                    type = "story",
                    url = "https://www.myleon.co/"
                },
                new StoryDTO
                {
                    by = "Final Test Author",
                    decendents = 0,
                    id = 3,
                    score = 1,
                    time = 1595430674,
                    title = "Final Completely Fake Title",
                    type = "story",
                    url = "https://www.google.com"
                }

            };

        }

        public void SetBypassCache(bool bypassCache)
        {
            throw new System.NotImplementedException();
        }

        Task<IList<StoryDTO>> IStoryService.GetStories(StoryParameters storyParameters)
        {
            return Task.FromResult((IList<StoryDTO>)stories);
        }
    }
}
