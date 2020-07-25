using AngularClient.Models;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AngularClient.Services
{
    public interface IStoryService 
    {
        Task<IList<StoryDTO>> GetStories(StoryParameters storyParameters);
        void SetBypassCache(bool bypassCache);
    }

    public class StoryService : IStoryService
    {
        private const string BASE_URL = "https://hacker-news.firebaseio.com";
        private const string NEW_STORIES_ENDPOINT = "/v0/newstories.json?print=pretty";
        private const string STORY_ITEM_ENDPOINT = "/v0/item/{0}.json?print=pretty";
        private const int MAX_STORY_COUNT = 200;
        private IStoryDataAccess _storyDataAccess;
        private IMemoryCache _cache;
        private bool _bypassCache = false;

        public StoryService(IMemoryCache cache, IStoryDataAccess storyDataAccess)
        {
            _cache = cache;
            _storyDataAccess = storyDataAccess;
        }

        public async Task<IList<StoryDTO>> GetStories(StoryParameters storyParameters)
        {
            var newestStoryIds = await getNewestStoryIdList();

            // No stories were returned
            if(newestStoryIds == null)
            {
                return null;
            }

            List<StoryDTO> stories = await getStoryDTOList(newestStoryIds);

            // No stories to return
            if (stories == null)
            {
                return null;
            }

            if(!string.IsNullOrEmpty(storyParameters.Search))
            {
                stories = applySearchFilter(stories, storyParameters.Search);

                // No stories to return
                if (stories == null)
                {
                    return null;
                }
            }

            List<StoryDTO> pagedStories = applyPaging(storyParameters.PageNumber, storyParameters.PageSize, stories);

            // No results on this page
            if(pagedStories == null)
            {
                return null;
            }

            storyParameters.TotalElements = stories.Count;
            return pagedStories;
        }

        public void SetBypassCache(bool bypassCache)
        {
            _bypassCache = bypassCache;
        }

        private async Task<List<StoryDTO>> getStoryDTOList(List<int> ids)
        {
            var stories = new List<StoryDTO>();

            // No ids to check
            if(ids == null  || ids.Count <= 0)
            {
                return null;
            }

            foreach (int id in ids)
            {
                string jstory = "";
                string key = getCacheKey(id.ToString());

                if (_bypassCache || !_cache.TryGetValue(key, out jstory))
                {
                    try
                    {
                        string endpoint = string.Format(STORY_ITEM_ENDPOINT, id);
                        jstory = await _storyDataAccess.GetExternalResponseAsync(BASE_URL + endpoint);

                    } catch(Exception e)
                    {
                        // Sometimes API will error out, return what we have
                        return stories;
                    }

                    // API call did not return data
                    if (string.IsNullOrEmpty(jstory))
                    {
                        continue;
                    }

                    if(!_bypassCache)
                    {
                        MemoryCacheEntryOptions options = new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromMinutes(5));
                        _cache.Set(key, jstory, options);
                    }
                }

                var story = JsonSerializer.Deserialize<StoryDTO>(jstory);
                if(story != null)
                {
                    stories.Add(story);
                }
            }

            return stories;
        }

        private async Task<List<int>> getNewestStoryIdList()
        {
            string jStoryIdList = "";
            string key = getCacheKey("list");

            if(_bypassCache || !_cache.TryGetValue(key, out jStoryIdList))
            {
                jStoryIdList = await _storyDataAccess.GetExternalResponseAsync(BASE_URL + NEW_STORIES_ENDPOINT);

                // API call did not return data
                if(jStoryIdList == null)
                {
                    return null;
                }

                if(!_bypassCache)
                {
                    _cache.Set(key, jStoryIdList, TimeSpan.FromMinutes(5));
                }
            }

            List<int> totalStoryList = JsonSerializer.Deserialize<List<int>>(jStoryIdList);

            if(totalStoryList.Count < MAX_STORY_COUNT)
            {
                return totalStoryList.GetRange(0, totalStoryList.Count);
            }

            return totalStoryList.GetRange(0, MAX_STORY_COUNT);
        }

        private List<StoryDTO> applyPaging(int pageNumber, int pageSize, List<StoryDTO> stories)
        {
            int skip = (pageNumber - 1) * pageSize;
            int listStart = skip < 1 ? 0 : skip - 1;
            int count = pageSize;

            // Not enough results to fill the page, return the rest of the list
            if (stories.Count < pageSize)
            {
                count = stories.Count;
            }

            // We're skipping all or more entries than we have (page number shouldn't be this high)
            if (skip >= stories.Count)
            {
                return null;
            }

            return stories.GetRange(listStart, count);
        }

        private List<StoryDTO> applySearchFilter(List<StoryDTO> stories, string query)
        {
            List<StoryDTO> filteredStories = new List<StoryDTO>();

            string cleanFilter = Utilities.CleanInput(query);

            cleanFilter = cleanFilter.Replace(" ", "%");
            cleanFilter = "%" + cleanFilter + "%";

            foreach(StoryDTO story in stories)
            {
                if (Utilities.ContainsLike(story.title.ToLower(), cleanFilter.ToLower()))
                {
                    filteredStories.Add(story);
                }
            }

            return filteredStories;
        }

        private string getCacheKey(string key)
        {
            return $"/story/{key}";
        }

    }
}
