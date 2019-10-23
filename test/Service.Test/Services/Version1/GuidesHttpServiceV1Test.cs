using System;
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
using PipServices3.Commons.Data;
using PipServices3.Commons.Random;
using PipServices3.Components.Cache;
using Wexxle.Guide.Data;
using Wexxle.Attachment.Client.Version1;

namespace Wexxle.Guide.Services.Version1
{
	public class GuidesHttpServiceV1BeforeAll
	{
		public ConfigParams HttpConfig = ConfigParams.FromTuples(
			"connection.protocol", "http",
			"connection.host", "localhost",
			"connection.port", "3000"
		);

		public IReferences References;
		public GuidesMemoryPersistence Persistence;
		public GuidesController Controller;
		public GuidesHttpServiceV1 Service;
		private NullCache _cache;

		public GuidesHttpServiceV1BeforeAll()
		{
			Persistence = new GuidesMemoryPersistence();
			Controller = new GuidesController();
			Service = new GuidesHttpServiceV1();
			_cache = new NullCache();

			References = PipServices3.Commons.Refer.References.FromTuples(
							  new Descriptor("wexxle-guides", "persistence", "memory", "default", "1.0"), Persistence,
							  new Descriptor("pip-services3", "cache", "null", "default", "1.0"), _cache,
							  new Descriptor("wexxle-guides", "controller", "default", "default", "1.0"), Controller,
							  new Descriptor("wexxle-guides", "service", "http", "default", "1.0"), Service,
						new Descriptor("wexxle-attachments", "client", "null", "default", "1.0"), new AttachmentsNullClientV1()
			);

			Controller.SetReferences(References);

			Service.Configure(HttpConfig);
			Service.SetReferences(References);

			Task.Run(() => Service.OpenAsync(null));
			Thread.Sleep(1000);
		}
	}

	public class GuidesHttpServiceV1Test : IClassFixture<GuidesHttpServiceV1BeforeAll>, IDisposable
	{
	    private static ConfigParams _httpConfig;
	    private readonly PagingParams _defaultPagingParams = new PagingParams(0, 10) { Total = true };
	    private readonly GuidesMemoryPersistence _persistence;

		public GuidesHttpServiceV1Test(GuidesHttpServiceV1BeforeAll beforeAll)
        {
	        _persistence = beforeAll.Persistence;
	        _httpConfig = beforeAll.HttpConfig;
        }
		public void Dispose()
		{
			_persistence.ClearAsync(null).Wait();
		}

		[Fact]
		public async Task It_Should_Create_Guide()
		{
			// arrange 
			var guide = TestModel.CreateGuide();

			// act
			var result = await Invoke<GuideV1>("create_guide", new {guide, paging = _defaultPagingParams, });

			// assert
			TestModel.AssertEqual(guide, result);
		}

		[Fact]
		public async Task It_Should_Update_Guide()
		{
			// arrange 
			var guide = await Invoke<GuideV1>("create_guide", new { guide = TestModel.CreateGuide(), paging = _defaultPagingParams, });

			// act
			guide.Name = RandomText.Word();

			var result = await Invoke<GuideV1>("update_guide", new { guide = guide, paging = _defaultPagingParams, });

			// assert
			TestModel.AssertEqual(guide, result);
		}

		[Fact]
		public async Task It_Should_Delete_Guide()
		{
			// arrange 
			var guide = await Invoke<GuideV1>("create_guide", new { guide = TestModel.CreateGuide(), paging = _defaultPagingParams, });

			// act
			var deletedGuide = await Invoke<GuideV1>("delete_guide_by_id", new { id = guide.Id, paging = _defaultPagingParams, });
			var result = await Invoke<GuideV1>("get_guide_by_id", new { id = guide.Id, paging = _defaultPagingParams, });

			// assert
			TestModel.AssertEqual(guide, deletedGuide);
			Assert.Null(result);
		}

		[Fact]
		public async Task It_Should_Get_Guide_By_Id()
		{
			// arrange 
			var guide = await Invoke<GuideV1>("create_guide", new { guide = TestModel.CreateGuide(), paging = _defaultPagingParams, });

			// act
			var result = await Invoke<GuideV1>("get_guide_by_id", new { id = guide.Id, paging = _defaultPagingParams, });

			

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
					var response = await httpClient.PostAsync($"{_httpConfig.GetAsString("connection.protocol")}://{_httpConfig.GetAsString("connection.host")}:{_httpConfig.GetAsString("connection.port")}/v1/Guides/" + route, content);
					var responseValue = response.Content.ReadAsStringAsync().Result;
					var result = JsonConverter.FromJson<T>(responseValue);
					return result;
				}
			}
		}
	}
}
