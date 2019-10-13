using PipServices3.Commons.Config;
using PipServices3.Commons.Refer;
using System.Threading.Tasks;
using Xunit;
using Guides.Persistence;
using Guides.Logic;
using Guides.Services.Version1;
using System;

namespace Guides.Clients.Version1
{
    public class GuidesDirectClientV1Test
    {
        private GuidesMemoryPersistence _persistence;
        private GuidesController _controller;
        private GuidesDirectClientV1 _client;
        private GuidesClientV1Fixture _fixture;

        public GuidesDirectClientV1Test()
        {
            _persistence = new GuidesMemoryPersistence();
            _controller = new GuidesController();
            _client = new GuidesDirectClientV1();

            IReferences references = References.FromTuples(
                new Descriptor("guides", "persistence", "memory", "default", "1.0"), _persistence,
                new Descriptor("guides", "controller", "default", "default", "1.0"), _controller,
                new Descriptor("guides", "client", "direct", "default", "1.0"), _client
            );

            _controller.SetReferences(references);

            _client.SetReferences(references);

            _fixture = new GuidesClientV1Fixture(_client);

            _client.OpenAsync(null).Wait();
        }

        [Fact]
        public async Task TestCrudOperationsAsync()
        {
            await _fixture.TestCrudOperationsAsync();
        }

    }
}
