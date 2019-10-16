using PipServices3.Commons.Refer;
using PipServices3.Components.Build;
using Wexxle.Guide.Clients.Version1;
using Wexxle.Attachment.Client.Version1;

namespace Wexxle.Guide.Build
{
    public class GuidesClientFactory : Factory
    {
        public static Descriptor NullClientDescriptor = new Descriptor("wexxle-guides", "client", "null", "*", "1.0");
        public static Descriptor DirectClientDescriptor = new Descriptor("wexxle-guides", "client", "direct", "*", "1.0");
        public static Descriptor HttpClientDescriptor = new Descriptor("wexxle-guides", "client", "http", "*", "1.0");

		public static Descriptor AttachmentsDescriptor = new Descriptor("wexxle-attachments", "client", "http", "*", "1.0");
		public GuidesClientFactory()
        {
            RegisterAsType(GuidesClientFactory.NullClientDescriptor, typeof(GuidesNullClientV1));
            RegisterAsType(GuidesClientFactory.DirectClientDescriptor, typeof(GuidesDirectClientV1));
            RegisterAsType(GuidesClientFactory.HttpClientDescriptor, typeof(GuidesHttpClientV1));

			RegisterAsType(GuidesClientFactory.AttachmentsDescriptor, typeof(AttachmentsHttpClientV1));
		}
    }
}
