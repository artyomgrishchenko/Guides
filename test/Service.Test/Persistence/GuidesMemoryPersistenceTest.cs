using PipServices3.Commons.Config;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Wexxle.Guide.Persistence
{
	public class MemoryGuidesPersistenceTest : IDisposable
	{
		readonly GuidesMemoryPersistence _persistence;
		readonly GuidesPersistenceFixture _fixture;

		public MemoryGuidesPersistenceTest()
		{
			_persistence = new GuidesMemoryPersistence();
			_persistence.Configure(new ConfigParams());

			_fixture = new GuidesPersistenceFixture(_persistence);

			_persistence.OpenAsync(null).Wait();
		}

		public void Dispose()
		{
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
