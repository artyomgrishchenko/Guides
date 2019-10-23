using PipServices3.Commons.Config;
using PipServices3.Commons.Convert;
using PipServices3.Commons.Refer;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Wexxle.Guide.Persistence;
using Wexxle.Guide.Logic;
using Wexxle.Guide.Data.Version1;
using System.Threading;
using PipServices3.Commons.Random;
using Wexxle.Guide.Data;
using Wexxle.Attachment.Client.Version1;

namespace Wexxle.Guide.Services.Version1
{
    public class GuidesHttpServiceV1Test
    {
		private static readonly ConfigParams HttpConfig = ConfigParams.FromTuples(
            "connection.protocol", "http",
            "connection.host", "localhost",
            "connection.port", "3000"
        );

        private readonly GuidesMemoryPersistence _persistence;
        private readonly GuidesController _controller;
        private readonly GuidesHttpServiceV1 _service;

        public GuidesHttpServiceV1Test()
        {
			_persistence = new GuidesMemoryPersistence();
            _controller = new GuidesController();
            _service = new GuidesHttpServiceV1();

            IReferences references = References.FromTuples(
                new Descriptor("wexxle-guides", "persistence", "memory", "default", "1.0"), _persistence,
                new Descriptor("wexxle-guides", "controller", "default", "default", "1.0"), _controller,
                new Descriptor("wexxle-guides", "service", "http", "default", "1.0"), _service,
				new Descriptor("wexxle-attachments", "client", "null", "default", "1.0"), new AttachmentsNullClientV1()
			);

            _controller.SetReferences(references);

            _service.Configure(HttpConfig);
            _service.SetReferences(references);

            Task.Run(() => _service.OpenAsync(null));
            Thread.Sleep(1000); // Just let service a sec to be initialized
        }

		[Fact]
		public async Task It_Should_Create_Guide()
		{
			// arrange 
			var guide = TestModel.CreateGuide();

			// act
			var result = await Invoke<GuideV1>("create_guide", new { guide = guide });

			// assert
			TestModel.AssertEqual(guide, result);
		}

		[Fact]
		public async Task It_Should_Update_Guide()
		{
			// arrange 
			var guide = await Invoke<GuideV1>("create_guide", new { guide = TestModel.CreateGuide() });

			// act
			guide.Name = RandomText.Word();

			var result = await Invoke<GuideV1>("update_guide", new { guide = guide });

			// assert
			TestModel.AssertEqual(guide, result);
		}

		[Fact]
		public async Task It_Should_Delete_Guide()
		{
			// arrange 
			var guide = await Invoke<GuideV1>("create_guide", new { guide = TestModel.CreateGuide() });

			// act
			var deletedGuide = await Invoke<GuideV1>("delete_guide_by_id", new { id = guide.Id });
			var result = await Invoke<GuideV1>("get_guide_by_id", new { id = guide.Id });

			// assert
			TestModel.AssertEqual(guide, deletedGuide);
			Assert.Null(result);
		}

		[Fact]
		public async Task It_Should_Get_Guide_By_Id()
		{
			// arrange 
			var guide = await Invoke<GuideV1>("create_guide", new { guide = TestModel.CreateGuide() });

			// act
			var result = await Invoke<GuideV1>("get_guide_by_id", new { id = guide.Id });

			

			// assert
			TestModel.AssertEqual(guide, result);
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
