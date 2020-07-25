using AngularClient.Controllers;
using AngularClient.Models;
using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;
using System.Linq;
using System.Threading.Tasks;

namespace NUnitTestProject1
{
    [TestFixture]
    [Category("APITests")]
    public class StoriesAPIControllerTests
    {
        private StoriesController _controller;
        private StoryServiceFake _service;
        private StoryParameters _parameters;

        public StoriesAPIControllerTests()
        { }

        [SetUp]
        public void Setup()
        {
            _service = new StoryServiceFake();
            _controller = new StoriesController(_service);
            _parameters = new StoryParameters();
        }

        [Test]
        public async Task TestGetWhenCalledReturnsOkResult()
        {
            var result = await _controller.Get(_parameters);

            Assert.IsInstanceOf<ActionResult<StoryResponse>>(result);
        }

        [Test]
        public async Task TestGetWhenCalledReturnsAllItems()
        {
            var result = (await _controller.Get(_parameters)).Result as OkObjectResult;

            var response = result.Value as StoryResponse;
            Assert.AreEqual(3, response.Stories.Count());
        }
    }
}
