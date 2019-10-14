using Guides.Data;
using Guides.Data.Version1;
using PipServices3.Commons.Data;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Guides.Clients.Version1
{
    public class GuidesClientV1Fixture
    {
		private IGuidesClientV1 _client;

        public GuidesClientV1Fixture(IGuidesClientV1 client)
        {
			_client = client;
        }

        public async Task TestCrudOperationsAsync()
        {
			TestModel testModel = new TestModel();
			var guide = await testModel.TestCreateGuidesAsync(_client.CreateGuideAsync);

			// Get all guides
			var page = await _client.GetGuidesAsync(
                null,
                new FilterParams(),
                new PagingParams()
            );

            Assert.NotNull(page);
            Assert.Equal(3, page.Data.Count);

            var guide1 = page.Data[0];
			var guide2 = page.Data[1];
			var guide3 = page.Data[2];

			// Update the guide
			guide1.Name = "GuideName";

			guide = await _client.UpdateGuideAsync(null, guide1);

			Assert.NotNull(guide);
			Assert.Equal(guide1.Id, guide.Id);
			Assert.Equal("GuideName", guide.Name);

			// Delete the guide
			guide = await _client.DeleteGuideByIdAsync(null, guide1.Id);

			Assert.NotNull(guide);
			Assert.Equal(guide1.Id, guide.Id);

			// Try to get deleted guide
			guide = await _client.GetGuideByIdAsync(null, guide1.Id);

			Assert.Null(guide);

			// Clean up for the last test
			await _client.DeleteGuideByIdAsync(null, guide2.Id);
			await _client.DeleteGuideByIdAsync(null, guide3.Id);
		}
    }
}
