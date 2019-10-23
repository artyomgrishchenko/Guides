using System.Threading.Tasks;
using PipServices3.Commons.Config;
using Xunit;

namespace Wexxle.Guide.Persistence
{
    public class GuidesFilePersistenceTest
    {
	    private readonly GuidesPersistenceFixture _fixture;

        public GuidesFilePersistenceTest()
        {
	        ConfigParams config = ConfigParams.FromTuples(
                "path", "guides.json"
            );
            var persistence = new GuidesFilePersistence();
            persistence.Configure(config);
            persistence.OpenAsync(null).Wait();
            persistence.ClearAsync(null).Wait();

            _fixture = new GuidesPersistenceFixture(persistence);
        }

        [Fact]
        public async Task It_Should_Create_Guide()
        {
	        await _fixture.It_Should_Create_Guide();
        }

        [Fact]
        public async Task It_Should_Delete_Guide()
        {
	        await _fixture.It_Should_Delete_Guide();
        }

        [Fact]
        public async Task It_Should_Update_Guide()
        {
	        await _fixture.It_Should_Update_Guide();
        }

        [Fact]
        public async Task It_Should_Get_Guide_By_Id()
        {
	        await _fixture.It_Should_Get_Guide_By_Id();
        }

        [Fact]
        public async Task It_Should_Get_All_Guides()
        {
	        await _fixture.It_Should_Get_All_Guides();
        }

        [Fact]
        public async Task It_Should_Get_Guides_By_Filters()
        {
	        await _fixture.It_Should_Get_Guides_By_Filters();
        }
	}
}
