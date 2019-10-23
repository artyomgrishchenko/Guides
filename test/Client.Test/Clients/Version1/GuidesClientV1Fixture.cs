using Wexxle.Guide.Data;
using PipServices3.Commons.Data;
using System.Threading.Tasks;
using PipServices3.Commons.Random;
using Xunit;

namespace Wexxle.Guide.Clients.Version1
{
    public class GuidesClientV1Fixture
    {
		private readonly IGuidesClientV1 _client;

        public GuidesClientV1Fixture(IGuidesClientV1 client)
        {
			_client = client;
        }

        public async Task It_Should_Create_Guide()
        {
	        // arrange 
	        var guide = TestModel.CreateGuide();

	        // act
	        var result = await _client.CreateGuideAsync(null, guide);

	        // assert
	        TestModel.AssertEqual(guide, result);

		}

		public async Task It_Should_Update_Guide()
        {
	        // arrange
	        var guide1 = await _client.CreateGuideAsync(null, TestModel.CreateGuide());

	        // act
	        guide1.Name = RandomText.Word();
	        var result = await _client.UpdateGuideAsync(null, guide1);

	        // assert
	        Assert.NotNull(result);
	        TestModel.AssertEqual(guide1, result);
        }

        public async Task It_Should_Delete_Guide()
        {
	        // arrange 
	        var guide = await _client.CreateGuideAsync(null, TestModel.CreateGuide());

	        // act
	        var deletedGuide = await _client.DeleteGuideByIdAsync(null, guide.Id);
	        var result = await _client.GetGuideByIdAsync(null, guide.Id);

	        // assert
	        TestModel.AssertEqual(guide, deletedGuide);
	        Assert.Null(result);
        }

        public async Task It_Should_Get_Guide_By_Id()
        {
	        // arrange 
	        var guide = await _client.CreateGuideAsync(null, TestModel.CreateGuide());

	        // act
	        var result = await _client.GetGuideByIdAsync(null, guide.Id);

	        // assert
	        TestModel.AssertEqual(guide, result);
        }

        public async Task It_Should_Get_All_Guides()
        {
	        // arrange 
	        await _client.CreateGuideAsync(null, TestModel.CreateGuide());
	        await _client.CreateGuideAsync(null, TestModel.CreateGuide());

	        // act
	        var page = await _client.GetGuidesAsync(
		        null,
		        new FilterParams(),
		        new PagingParams()
	        );

	        // assert
	        Assert.NotNull(page);
	        Assert.Equal(2, page.Data.Count);
        }

    }
}
