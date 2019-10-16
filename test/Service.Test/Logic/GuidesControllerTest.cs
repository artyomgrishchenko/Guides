using PipServices3.Commons.Refer;
using Xunit;
using PipServices3.Commons.Data;
using PipServices3.Commons.Config;
using Wexxle.Guide.Persistence;
using Wexxle.Guide.Data.Version1;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using Wexxle.Guide.Data;
using Wexxle.Attachment.Client.Version1;

namespace Wexxle.Guide.Logic
{
    public class GuidesControllerTest: IDisposable
    {
        private GuidesController _controller;
        private GuidesMemoryPersistence _persistence;

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
        public async Task TestCrudOperationsAsync()
        {
			TestModel testModel = new TestModel();
			var guide = await testModel.TestCreateGuidesAsync(_controller.CreateGuideAsync);


			// Get all guides
			var page = await _controller.GetGuidesAsync(
                null,
                new FilterParams(),
                new PagingParams()
            );

            Assert.NotNull(page);
            Assert.Equal(3, page.Data.Count);

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
