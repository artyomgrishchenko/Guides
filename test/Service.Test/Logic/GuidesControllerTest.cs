﻿using PipServices3.Commons.Refer;
using Xunit;
using PipServices3.Commons.Data;
using PipServices3.Commons.Config;
using Guides.Persistence;
using Guides.Data.Version1;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Guides.Logic
{
    public class GuidesControllerTest: IDisposable
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
			Tags = new List<string> {"tag1","tag2","tag3" },
			AllTags = new List<string> { "tag1", "tag2", "tag3", "tag4","tag5" },
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

        private GuidesController _controller;
        private GuidesMemoryPersistence _persistence;

        public GuidesControllerTest()
        {
			GUIDE1.Pages = new List<GuidePageV1> { GUIDEPAGE1, GUIDEPAGE2 };

			_persistence = new GuidesMemoryPersistence();
            _persistence.Configure(new ConfigParams());

            _controller = new GuidesController();

            var references = References.FromTuples(
                new Descriptor("guides", "persistence", "memory", "*", "1.0"), _persistence,
                new Descriptor("guides", "controller", "default", "*", "1.0"), _controller
            );

            _controller.SetReferences(references);

            _persistence.OpenAsync(null).Wait();
        }

        public void Dispose()
        {
            _persistence.CloseAsync(null).Wait();
        }

        [Fact]
        public async Task TestCrudOperationsAsync()
        {
            // Create the first guide
            var guide = await _controller.CreateGuideAsync(null, GUIDE1);

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
			guide = await _controller.CreateGuideAsync(null, GUIDE2);

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

			// Get all guides
			var page = await _controller.GetGuidesAsync(
                null,
                new FilterParams(),
                new PagingParams()
            );

            Assert.NotNull(page);
            Assert.Equal(2, page.Data.Count);

            var guide1 = page.Data[0];

            // Update the guide
            guide1.Name = "GuideName";

            guide = await _controller.UpdateGuideAsync(null, guide1);

            Assert.NotNull(guide);
            Assert.Equal(guide1.Id, guide.Id);
            Assert.Equal("GuideName", guide.Name);

            // Delete the guide
            guide = await _controller.DeleteGuideByIdAsync(null, guide1.Id);

            Assert.NotNull(guide);
            Assert.Equal(guide1.Id, guide.Id);

            // Try to get deleted guide
            guide = await _controller.GetGuideByIdAsync(null, guide1.Id);

            Assert.Null(guide);
        }

    }
}