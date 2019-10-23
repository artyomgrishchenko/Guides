using System;
using System.Threading.Tasks;
using PipServices3.Commons.Config;
using PipServices3.Commons.Convert;
using Xunit;

namespace Wexxle.Guide.Persistence
{
    public class GuidesMongoDbPersistenceTest : IDisposable
        {
        private readonly bool _enabled = false;
        private readonly GuidesMongoDbPersistence _persistence;
        private readonly GuidesPersistenceFixture _fixture;

        public GuidesMongoDbPersistenceTest()
        {
            var mongoEnabled = Environment.GetEnvironmentVariable("MONGO_ENABLED") ?? "true";
            var mongoDb = Environment.GetEnvironmentVariable("MONGO_DB") ?? "test";
            var mongoCollection = Environment.GetEnvironmentVariable("MONGO_COLLECTION") ?? "guides";
            var mongoServiceHost = Environment.GetEnvironmentVariable("MONGO_SERVICE_HOST") ?? "localhost";
            var mongoServicePort = Environment.GetEnvironmentVariable("MONGO_SERVICE_PORT") ?? "27017";
            var mongoServiceUri = Environment.GetEnvironmentVariable("MONGO_SERVICE_URI");

            _enabled = BooleanConverter.ToBoolean(mongoEnabled);

            if (_enabled)
            {
                var config = ConfigParams.FromTuples(
                    "collection", mongoCollection,
                    "connection.database", mongoDb,
                    "connection.host", mongoServiceHost,
                    "connection.port", mongoServicePort,
                    "connection.uri", mongoServiceUri
                );

                _persistence = new GuidesMongoDbPersistence();
                _persistence.Configure(config);
                _persistence.OpenAsync(null).Wait();
                _persistence.ClearAsync(null).Wait();

                _fixture = new GuidesPersistenceFixture(_persistence);
            }
        }

        public void Dispose()
        {
            if (_enabled)
                _persistence.CloseAsync(null).Wait();
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
