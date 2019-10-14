using Guides.Data;
using Guides.Data.Version1;
using PipServices3.Commons.Data;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Guides.Persistence
{
    public class GuidesPersistenceFixture
    {
		private IGuidesPersistence _persistence;

        public GuidesPersistenceFixture(IGuidesPersistence persistence)
        {
			_persistence = persistence;
        }

        private async Task TestCreateGuidesAsync()
		{
			TestModel testModel = new TestModel();
			await testModel.TestCreateGuidesAsync(_persistence.CreateAsync);
		}

		public async Task TestCrudOperationsAsync()
        {
            // Create items
            await TestCreateGuidesAsync();

            // Get all guides
            var page = await _persistence.GetPageByFilterAsync(
                null,
                new FilterParams(),
                new PagingParams()
            );

            Assert.NotNull(page);
            Assert.Equal(3, page.Data.Count);

            var guide1 = page.Data[0];

            // Update the guide
            guide1.Name = "ABCD";

            var guide = await _persistence.UpdateAsync(null, guide1);

            Assert.NotNull(guide);
            Assert.Equal(guide1.Id, guide.Id);
            Assert.Equal("ABCD", guide.Name);

            // Delete the guide
            guide = await _persistence.DeleteByIdAsync(null, guide1.Id);

            Assert.NotNull(guide);
            Assert.Equal(guide1.Id, guide.Id);

            // Try to get deleted guide
            guide = await _persistence.GetOneByIdAsync(null, guide1.Id);

            Assert.Null(guide);
        }

        public async Task TestGetWithFiltersAsync()
        {
            // Create items
            await TestCreateGuidesAsync();

            // Filter by id
            var page = await _persistence.GetPageByFilterAsync(
                null,
                FilterParams.FromTuples(
                    "id", "1"
                ),
                new PagingParams()
            );

            Assert.Single(page.Data);

            // Filter by udi
            page = await _persistence.GetPageByFilterAsync(
                null,
                FilterParams.FromTuples(
                    "name", "TestGuide2"
				),
                new PagingParams()
            );

            Assert.Single(page.Data);

            // Filter by udis
            page = await _persistence.GetPageByFilterAsync(
                null,
                FilterParams.FromTuples(
                    "tag", "tag1"
                ),
                new PagingParams()
            );

            Assert.Equal(3, page.Data.Count);

            // Filter by site_id
            page = await _persistence.GetPageByFilterAsync(
                null,
                FilterParams.FromTuples(
                    "type", "introduction"
				),
                new PagingParams()
            );

            Assert.Equal(2, page.Data.Count);
        }
    }
}
