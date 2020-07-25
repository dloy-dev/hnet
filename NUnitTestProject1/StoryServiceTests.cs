using AngularClient.Models;
using AngularClient.Services;
using Moq;
using Newtonsoft.Json;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NUnitTestProject1
{
    [TestFixture]
    [Category("UnitTests")]
    public class StoryServiceTests : TestBase
    {
        private StoryService storyService;
        private StoryParameters storyParameters;
        private string TESTITEM1;
        private string TESTITEM2;
        private string TESTITEM3;
        private string TESTITEM4;
        private string TESTITEM5;
        private string TESTITEM6;

        private StoryDTO DTO1;
        private StoryDTO DTO2;
        private StoryDTO DTO3;
        private StoryDTO DTO4;
        private StoryDTO DTO5;
        private StoryDTO DTO6;

        private const string NEWEST_STORIES_URL = "https://hacker-news.firebaseio.com/v0/newstories.json?print=pretty";
        private const string STORY_ITEM_URL = "https://hacker-news.firebaseio.com/v0/item/{0}.json?print=pretty";
        private const string nullString = null;
        private const int DEFAULT_PAGE_SIZE = 10;

        [SetUp]
        public void Setup()
        {
            storyService = new StoryService(mockCache.Object, mockDataAccess.Object);
            storyService.SetBypassCache(true);
            storyParameters = new StoryParameters();
            storyParameters.PageSize = DEFAULT_PAGE_SIZE;

            createJsonObjects();

            mockDataAccess.Setup(s => s.GetExternalResponseAsync(string.Format(STORY_ITEM_URL, "1"))).ReturnsAsync(TESTITEM1);
            mockDataAccess.Setup(s => s.GetExternalResponseAsync(string.Format(STORY_ITEM_URL, "2"))).ReturnsAsync(TESTITEM2);
            mockDataAccess.Setup(s => s.GetExternalResponseAsync(string.Format(STORY_ITEM_URL, "3"))).ReturnsAsync(TESTITEM3);
            mockDataAccess.Setup(s => s.GetExternalResponseAsync(string.Format(STORY_ITEM_URL, "4"))).ReturnsAsync(TESTITEM4);
            mockDataAccess.Setup(s => s.GetExternalResponseAsync(string.Format(STORY_ITEM_URL, "5"))).ReturnsAsync(TESTITEM5);
            mockDataAccess.Setup(s => s.GetExternalResponseAsync(string.Format(STORY_ITEM_URL, "6"))).ReturnsAsync(TESTITEM6);
            mockDataAccess.Setup(s => s.GetExternalResponseAsync(string.Format(STORY_ITEM_URL, "0"))).ReturnsAsync(nullString);
        }

        private void createJsonObjects()
        {
            DTO1 = new StoryDTO
            {
                id = 1,
                title = "Test Title 1"
            };

            DTO2 = new StoryDTO
            {
                id = 2,
                title = "Startup Is Starting Commotion with It's Branding"
            };

            DTO3 = new StoryDTO
            {
                id = 3,
                title = "Another Testing Title"
            };

            DTO4 = new StoryDTO
            {
                id = 4,
                title = "This Is a Great Title"
            };

            DTO5 = new StoryDTO
            {
                id = 5,
                title = "Yet Another Fantastic Story"
            };

            DTO6 = new StoryDTO
            {
                id = 6,
                title = "Don't Be Fooled By This Title"
            };

            TESTITEM1 = JsonConvert.SerializeObject(DTO1);
            TESTITEM2 = JsonConvert.SerializeObject(DTO2);
            TESTITEM3 = JsonConvert.SerializeObject(DTO3);
            TESTITEM4 = JsonConvert.SerializeObject(DTO4);
            TESTITEM5 = JsonConvert.SerializeObject(DTO5);
            TESTITEM6 = JsonConvert.SerializeObject(DTO6);
        }

        [Test]
        public async Task TestGetStoriesWhenNewestStoriesIdListNullReturnsNull()
        {
            mockDataAccess.Setup(s => s.GetExternalResponseAsync(NEWEST_STORIES_URL)).ReturnsAsync(nullString);

            Assert.IsNull(await storyService.GetStories(storyParameters));
        }

        [Test]
        public async Task TestGetStoriesWhenNewestStoriesIdListEmptyReturnsNull()
        {
            string idListJsonString = "[]";

            mockDataAccess.Setup(s => s.GetExternalResponseAsync(NEWEST_STORIES_URL)).ReturnsAsync(idListJsonString);

            Assert.IsNull(await storyService.GetStories(storyParameters));
        }

        [Test]
        public async Task TestGetStoriesOnlyReturnsNonNullStories()
        {
            string idListJsonString = "[0, 1, 2]";

            mockDataAccess.Setup(s => s.GetExternalResponseAsync(NEWEST_STORIES_URL)).ReturnsAsync(idListJsonString);

            IList<StoryDTO> result = await storyService.GetStories(storyParameters);

            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count);
        }

        [Test]
        public async Task TestGetStoriesOnlyReturnsNonNullStringStories()
        {
            string idListJsonString = "[0, 1, 2]";

            mockDataAccess.Setup(s => s.GetExternalResponseAsync(string.Format(STORY_ITEM_URL, "0"))).ReturnsAsync("null\n");
            mockDataAccess.Setup(s => s.GetExternalResponseAsync(NEWEST_STORIES_URL)).ReturnsAsync(idListJsonString);

            IList<StoryDTO> result = await storyService.GetStories(storyParameters);

            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count);
        }

        [Test]
        public async Task TestGetStoriesWhenAllStoriesFilteredOutReturnsNull()
        {
            string idListJsonString = "[1, 2, 3, 4, 5, 6]";
            storyParameters.Search = "RANDOM SEARCH STRING";

            mockDataAccess.Setup(s => s.GetExternalResponseAsync(NEWEST_STORIES_URL)).ReturnsAsync(idListJsonString);

            IList<StoryDTO> result = await storyService.GetStories(storyParameters);

            Assert.IsNull(result);
        }

        [Test]
        public async Task TestGetStoriesWithSearch()
        {
            string idListJsonString = "[1, 2, 3, 4, 5, 6]";
            storyParameters.Search = "title";

            mockDataAccess.Setup(s => s.GetExternalResponseAsync(NEWEST_STORIES_URL)).ReturnsAsync(idListJsonString);

            IList<StoryDTO> result = await storyService.GetStories(storyParameters);

            Assert.AreEqual(4, result.Count);
        }

        [Test]
        public async Task TestGetStoriesWithSearchIgnoresSpecialCharacters()
        {
            string idListJsonString = "[1, 2, 3, 4, 5, 6]";
            storyParameters.Search = "<!Test Title 1!>";

            mockDataAccess.Setup(s => s.GetExternalResponseAsync(NEWEST_STORIES_URL)).ReturnsAsync(idListJsonString);

            IList<StoryDTO> result = await storyService.GetStories(storyParameters);

            Assert.AreEqual(1, result.Count);
        }

        [Test]
        public async Task TestGetStoriesWithPagingReturnsFirstPageResults()
        {
            string idListJsonString = "[1, 2, 3, 4, 5, 6]";
            storyParameters.PageNumber = 1;
            storyParameters.PageSize = 5;

            mockDataAccess.Setup(s => s.GetExternalResponseAsync(NEWEST_STORIES_URL)).ReturnsAsync(idListJsonString);

            List<StoryDTO> result = (await storyService.GetStories(storyParameters)).ToList();

            Assert.AreEqual(5, result.Count);

            Assert.AreEqual(DTO1.id, result[0].id);
            Assert.AreEqual(DTO2.id, result[1].id);
            Assert.AreEqual(DTO3.id, result[2].id);
            Assert.AreEqual(DTO4.id, result[3].id);
            Assert.AreEqual(DTO5.id, result[4].id);
        }

        [Test]
        public async Task TestGetStoriesWhenPageSizeLargerThanListCountReturnsOnePageResults()
        {
            string idListJsonString = "[1, 2, 3, 4]";
            storyParameters.PageNumber = 1;
            storyParameters.PageSize = 5;

            mockDataAccess.Setup(s => s.GetExternalResponseAsync(NEWEST_STORIES_URL)).ReturnsAsync(idListJsonString);

            List<StoryDTO> result = (await storyService.GetStories(storyParameters)).ToList();

            Assert.AreEqual(4, result.Count);
            Assert.AreEqual(DTO1.id, result[0].id);
            Assert.AreEqual(DTO2.id, result[1].id);
            Assert.AreEqual(DTO3.id, result[2].id);
            Assert.AreEqual(DTO4.id, result[3].id);
        }

        [Test]
        public async Task TestGetStoriesWhenPageNumberExpectedResultsLargerThanIdListCountReturnsNoResults()
        {
            string idListJsonString = "[1, 2, 3, 4, 5]";
            storyParameters.PageNumber = 2;
            storyParameters.PageSize = 5;

            mockDataAccess.Setup(s => s.GetExternalResponseAsync(NEWEST_STORIES_URL)).ReturnsAsync(idListJsonString);

            var result = await storyService.GetStories(storyParameters);
            Assert.IsNull(result);
        }
    }
}
