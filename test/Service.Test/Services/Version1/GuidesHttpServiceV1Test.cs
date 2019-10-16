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
using Guides.Data;

namespace Guides.Services.Version1
{
    public class GuidesHttpServiceV1Test
    {
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
			_persistence = new GuidesMemoryPersistence();
            _controller = new GuidesController();
            _service = new GuidesHttpServiceV1();

            IReferences references = References.FromTuples(
                new Descriptor("wexxle-guides", "persistence", "memory", "default", "1.0"), _persistence,
                new Descriptor("wexxle-guides", "controller", "default", "default", "1.0"), _controller,
                new Descriptor("wexxle-guides", "service", "http", "default", "1.0"), _service
            );

            _controller.SetReferences(references);

            _service.Configure(HttpConfig);
            _service.SetReferences(references);

            Task.Run(() => _service.OpenAsync(null));
            Thread.Sleep(1000); // Just let service a sec to be initialized
        }

		async Task<GuideV1> CreateAsync(string route, GuideV1 item)
		{
			return await Invoke<GuideV1>(route, new { guide = item });
		}


		[Fact]
        public async Task TestCrudOperationsAsync()
        {
			TestModel testModel = new TestModel();
			var guide = await testModel.TestCreateGuidesAsync(CreateAsync, "create_guide");

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
            Assert.Equal(3, page.Data.Count);

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
