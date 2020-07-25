using AngularClient.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AngularClient.Services
{
    public class StoryRepository
    {
        private IStoryService _storyService;

        public StoryRepository(IStoryService storyService)
        {
            _storyService = storyService;
        }

        public async Task<IList<Story>> GetStories(StoryParameters storyParameters)
        {
            List<Story> stories = new List<Story>();
            var storydtos = await _storyService.GetStories(storyParameters);

            if(storydtos == null)
            {
                //Log statement
                return stories;
            }

            foreach(StoryDTO dto in storydtos)
            {
                var story = MapDTOToItem(dto);
                stories.Add(story);
            }

            return stories;
        }

        private Story MapDTOToItem(StoryDTO dto)
        {
            return new Story
            {
                Id = dto.id,
                Author = dto.by,
                Time = Utilities.UnixTimeStampToDateTime(dto.time),
                Title = dto.title,
                Url = dto.url
            };
        }

        
    }
}
