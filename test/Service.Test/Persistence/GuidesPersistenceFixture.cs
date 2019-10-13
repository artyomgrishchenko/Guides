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
		private GuidePageV1 GUIDEPAGE1 = new GuidePageV1
		{
			Title = new Dictionary<string, string>() { { "Page1.Title1", "Some in title1" }, { "Page1.Title2", "Some in title2" } },
			Content = new Dictionary<string, string>() { { "Page1.Content1", "Some in content1" }, { "Page1.Content2", "Some in content2" } },
			Color = "yellow",
			PicId = "1",
			PicUri = "000001"
		};

		private GuidePageV1 GUIDEPAGE2 = new GuidePageV1
		{
			Title = new Dictionary<string, string>() { { "Page2.Title1", "Some in title1" }, { "Page2.Title2", "Some in title2" } },
			Content = new Dictionary<string, string>() { { "Page2.Content1", "Some in content1" }, { "Page2.Content2", "Some in content2" } },
			Color = "blue",
			PicId = "2",
			PicUri = "000002"
		};

		private GuideV1 GUIDE1 = new GuideV1
		{
			Id = "1",
			Name = "TestGuide1",
			Type = GuideTypeV1.Home,
			App = "App1",
			Min_ver = 12,
			Max_ver = 50,
			Tags = new List<string> { "tag1", "tag2", "tag3" },
			AllTags = new List<string> { "tag1", "tag2", "tag3", "tag4", "tag5" },
			Status = "active",
		};
		private GuideV1 GUIDE2 = new GuideV1
		{
			Id = "2",
			Name = "TestGuide2",
			Type = GuideTypeV1.Introduction,
			App = "App2",
			Min_ver = 5,
			Max_ver = 11,
			Tags = new List<string> { "tag1", "tag2", "tag3" },
			AllTags = new List<string> { "tag1", "tag2", "tag3", "tag4", "tag5" },
			Status = "new",
		};
		private GuideV1 GUIDE3 = new GuideV1
		{
			Id = "3",
			Name = "TestGuide3",
			Type = GuideTypeV1.Introduction,
			App = "App3",
			Min_ver = 11,
			Max_ver = 14,
			Tags = new List<string> { "tag1", "tag2", "tag3" },
			AllTags = new List<string> { "tag1", "tag2", "tag3", "tag4", "tag5" },
			Status = "new",
		};


		private IGuidesPersistence _persistence;

        public GuidesPersistenceFixture(IGuidesPersistence persistence)
        {
			GUIDE1.Pages = new List<GuidePageV1> { GUIDEPAGE1, GUIDEPAGE2 };

			_persistence = persistence;
        }

        private async Task TestCreateGuidesAsync()
        {
            // Create the first guide
            var guide = await _persistence.CreateAsync(null, GUIDE1);

			Assert.NotNull(guide);
			Assert.Equal(GUIDE1.App, guide.App);
			Assert.Equal(GUIDE1.Max_ver, guide.Max_ver);
			Assert.Equal(GUIDE1.Min_ver, guide.Min_ver);
			Assert.Equal(GUIDE1.Name, guide.Name);
			Assert.Equal(GUIDE1.Status, guide.Status);
			Assert.Equal(GUIDE1.Type, guide.Type);

			Assert.NotNull(guide.Pages);
			Assert.NotNull(guide.Tags);
			Assert.NotNull(guide.AllTags);

			// Create the second guide
			guide = await _persistence.CreateAsync(null, GUIDE2);

			Assert.NotNull(guide);
			Assert.Equal(GUIDE2.App, guide.App);
			Assert.Equal(GUIDE2.Max_ver, guide.Max_ver);
			Assert.Equal(GUIDE2.Min_ver, guide.Min_ver);
			Assert.Equal(GUIDE2.Name, guide.Name);
			Assert.Equal(GUIDE2.Status, guide.Status);
			Assert.Equal(GUIDE2.Type, guide.Type);

			Assert.Null(guide.Pages);
			Assert.NotNull(guide.Tags);
			Assert.NotNull(guide.AllTags);

			// Create the third guide
			guide = await _persistence.CreateAsync(null, GUIDE3);

			Assert.NotNull(guide);
			Assert.Equal(GUIDE2.App, guide.App);
			Assert.Equal(GUIDE2.Max_ver, guide.Max_ver);
			Assert.Equal(GUIDE2.Min_ver, guide.Min_ver);
			Assert.Equal(GUIDE2.Name, guide.Name);
			Assert.Equal(GUIDE2.Status, guide.Status);
			Assert.Equal(GUIDE2.Type, guide.Type);

			Assert.Null(guide.Pages);
			Assert.NotNull(guide.Tags);
			Assert.NotNull(guide.AllTags);
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
