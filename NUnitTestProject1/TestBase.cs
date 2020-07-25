using AngularClient.Services;
using Microsoft.Extensions.Caching.Memory;
using Moq;
using NUnit.Framework;

namespace NUnitTestProject1
{
    public class TestBase
    {
        protected Mock<IMemoryCache> mockCache;
        protected Mock<IStoryDataAccess> mockDataAccess;

        [SetUp]
        public void SetupBase()
        {
            mockCache = new Mock<IMemoryCache>();
            mockDataAccess = new Mock<IStoryDataAccess>();
        }
    }
}
