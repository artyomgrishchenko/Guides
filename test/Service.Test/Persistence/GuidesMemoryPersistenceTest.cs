using System;
using System.Threading.Tasks;
using PipServices3.Commons.Config;
using Xunit;

namespace Guides.Persistence
{
    public class MemoryGuidesPersistenceTest: IDisposable
    {
        public GuidesMemoryPersistence _persistence;
        public GuidesPersistenceFixture _fixture;

        public MemoryGuidesPersistenceTest()
        {
            _persistence = new GuidesMemoryPersistence();
            _persistence.Configure(new ConfigParams());

            _fixture = new GuidesPersistenceFixture(_persistence);

            _persistence.OpenAsync(null).Wait();
        }

        public void Dispose()
        {
            _persistence.CloseAsync(null).Wait();    
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
