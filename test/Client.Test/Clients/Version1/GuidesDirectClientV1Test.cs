using PipServices3.Commons.Refer;
using System.Threading.Tasks;
using Xunit;
using Wexxle.Guide.Persistence;
using Wexxle.Guide.Logic;
using Wexxle.Attachment.Client.Version1;

namespace Wexxle.Guide.Clients.Version1
{
    public class GuidesDirectClientV1Test
    {
	    private readonly GuidesClientV1Fixture _fixture;

        public GuidesDirectClientV1Test()
        {
	        var persistence = new GuidesMemoryPersistence();
            var controller = new GuidesController();
            var client = new GuidesDirectClientV1();

            IReferences references = References.FromTuples(
                new Descriptor("wexxle-guides", "persistence", "memory", "default", "1.0"), persistence,
                new Descriptor("wexxle-guides", "controller", "default", "default", "1.0"), controller,
                new Descriptor("wexxle-guides", "client", "direct", "default", "1.0"), client,
				new Descriptor("wexxle-attachments", "client", "null", "default", "1.0"), new AttachmentsNullClientV1()
			);

            controller.SetReferences(references);

            client.SetReferences(references);

            _fixture = new GuidesClientV1Fixture(client);

            client.OpenAsync(null).Wait();
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


	}
}
