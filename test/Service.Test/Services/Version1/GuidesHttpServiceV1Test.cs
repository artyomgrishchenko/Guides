using PipServices3.Commons.Config;
using PipServices3.Commons.Convert;
using PipServices3.Commons.Data;
using PipServices3.Commons.Refer;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Guides.Persistence;
using Guides.Logic;
using Guides.Data.Version1;
using System.Threading;
using System.Collections.Generic;
using System;

namespace Guides.Services.Version1
{
    public class GuidesHttpServiceV1Test
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
			CreateTime = DateTime.UtcNow,
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
			CreateTime = DateTime.UtcNow,
		};

		private static readonly ConfigParams HttpConfig = ConfigParams.FromTuples(
            "connection.protocol", "http",
            "connection.host", "localhost",
            "connection.port", "3000"
        );

        private GuidesMemoryPersistence _persistence;
        private GuidesController _controller;
        private GuidesHttpServiceV1 _service;

        public GuidesHttpServiceV1Test()
        {
			GUIDE1.Pages = new List<GuidePageV1> { GUIDEPAGE1, GUIDEPAGE2 };

			_persistence = new GuidesMemoryPersistence();
            _controller = new GuidesController();
            _service = new GuidesHttpServiceV1();

            IReferences references = References.FromTuples(
                new Descriptor("guides", "persistence", "memory", "default", "1.0"), _persistence,
                new Descriptor("guides", "controller", "default", "default", "1.0"), _controller,
                new Descriptor("guides", "service", "http", "default", "1.0"), _service
            );

            _controller.SetReferences(references);

            _service.Configure(HttpConfig);
            _service.SetReferences(references);

            Task.Run(() => _service.OpenAsync(null));
            Thread.Sleep(1000); // Just let service a sec to be initialized
        }

        [Fact]
        public async Task TestCrudOperationsAsync()
        {
            // Create the first guide
            var guide = await Invoke<GuideV1>("create_guide", new { guide = GUIDE1 });

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
			guide = await Invoke<GuideV1>("create_guide", new { guide = GUIDE2 });

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
			var page = await Invoke<DataPage<GuideV1>>(
                "get_guides",
                new
                {
                    filter = new FilterParams(),
                    paging = new PagingParams()
                }
            );

            Assert.NotNull(page);
            Assert.Equal(2, page.Data.Count);

            var guide1 = page.Data[0];

			// Update the guide
			guide1.Name = "GuideName";

			guide = await Invoke<GuideV1>("update_guide", new { guide = guide1 });

			Assert.NotNull(guide);
			Assert.Equal(guide1.Id, guide.Id);
			Assert.Equal("GuideName", guide.Name);

            // Delete the guide
            guide = await Invoke<GuideV1>("delete_guide_by_id", new { guide_id = guide1.Id });

            Assert.NotNull(guide);
            Assert.Equal(guide1.Id, guide.Id);

            // Try to get deleted guide
            guide = await Invoke<GuideV1>("get_guide_by_id", new { guide_id = guide1.Id });

            Assert.Null(guide);
        }

        private static async Task<T> Invoke<T>(string route, dynamic request)
        {
            using (var httpClient = new HttpClient())
            {
                var requestValue = JsonConverter.ToJson(request);
                using (var content = new StringContent(requestValue, Encoding.UTF8, "application/json"))
                {
                    var response = await httpClient.PostAsync("http://localhost:3000/v1/guides/" + route, content);
                    var responseValue = response.Content.ReadAsStringAsync().Result;
                    return JsonConverter.FromJson<T>(responseValue);
                }
            }
        }
    }
}
