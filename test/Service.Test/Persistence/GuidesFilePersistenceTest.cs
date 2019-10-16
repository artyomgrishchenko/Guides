using System;
using System.Threading.Tasks;
using PipServices3.Commons.Config;
using Xunit;

namespace Wexxle.Guide.Persistence
{
    public class GuidesFilePersistenceTest
    {
        private GuidesFilePersistence _persistence;
        private GuidesPersistenceFixture _fixture;

        public GuidesFilePersistenceTest()
        {
            ConfigParams config = ConfigParams.FromTuples(
                "path", "guides.json"
            );
            _persistence = new GuidesFilePersistence();
            _persistence.Configure(config);
            _persistence.OpenAsync(null).Wait();
            _persistence.ClearAsync(null).Wait();

            _fixture = new GuidesPersistenceFixture(_persistence);
        }

        [Fact]
        public async Task TestCrudOperationsAsync()
        {
            await _fixture.TestCrudOperationsAsync();
        }

        [Fact]
        public async Task TestGetWithFiltersAsync()
        {
            await _fixture.TestGetWithFiltersAsync();
        }
    }
}
