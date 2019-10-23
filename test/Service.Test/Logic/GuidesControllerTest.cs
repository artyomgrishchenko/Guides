using PipServices3.Commons.Refer;
using Xunit;
using PipServices3.Commons.Data;
using PipServices3.Commons.Config;
using Wexxle.Guide.Persistence;
using System;
using System.Threading.Tasks;
using PipServices3.Commons.Random;
using Wexxle.Guide.Data;
using Wexxle.Attachment.Client.Version1;

namespace Wexxle.Guide.Logic
{
    public class GuidesControllerTest: IDisposable
    {
        private readonly GuidesController _controller;
        private readonly GuidesMemoryPersistence _persistence;

        public GuidesControllerTest()
        {
			_persistence = new GuidesMemoryPersistence();
            _persistence.Configure(new ConfigParams());

            _controller = new GuidesController();

            var references = References.FromTuples(
                new Descriptor("wexxle-guides", "persistence", "memory", "*", "1.0"), _persistence,
                new Descriptor("wexxle-guides", "controller", "default", "*", "1.0"), _controller,
				new Descriptor("wexxle-attachments", "client", "null", "default", "1.0"), new AttachmentsNullClientV1()
			);

            _controller.SetReferences(references);

            _persistence.OpenAsync(null).Wait();
        }

        public void Dispose()
        {
            _persistence.CloseAsync(null).Wait();
        }

		[Fact]
		public async Task It_Should_Create_Guide()
		{
			// arrange 
			var guide = TestModel.CreateGuide();

			// act
			var result = await _controller.CreateGuideAsync(null, guide);

			// assert
			TestModel.AssertEqual(guide, result);
		}

		[Fact]
		public async Task It_Should_Update_Guide()
		{
			// arrange
			var guide1 = await _controller.CreateGuideAsync(null, TestModel.CreateGuide());

			// act
			guide1.Name = RandomText.Word();
			var result = await _controller.UpdateGuideAsync(null, guide1);

			// assert
			Assert.NotNull(result);
			TestModel.AssertEqual(guide1, result);
		}

		[Fact]
		public async Task It_Should_Delete_Guide()
		{
			// arrange 
			var guide = await _controller.CreateGuideAsync(null, TestModel.CreateGuide());

			// act
			var deletedGuide = await _controller.DeleteGuideByIdAsync(null, guide.Id);
			var result = await _controller.GetGuideByIdAsync(null, guide.Id);

			// assert
			TestModel.AssertEqual(guide, deletedGuide);
			Assert.Null(result);
		}

		[Fact]
		public async Task It_Should_Get_Guide_By_Id()
		{
			// arrange 
			var guide = await _controller.CreateGuideAsync(null, TestModel.CreateGuide());

			// act
			var result = await _controller.GetGuideByIdAsync(null, guide.Id);

			// assert
			TestModel.AssertEqual(guide, result);
		}

		[Fact]
		public async Task It_Should_Get_All_Guides()
		{
			// arrange 
			await _controller.CreateGuideAsync(null, TestModel.CreateGuide());
			await _controller.CreateGuideAsync(null, TestModel.CreateGuide());

			// act
			var page = await _controller.GetGuidesAsync(
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
