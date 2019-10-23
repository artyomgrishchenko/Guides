using PipServices3.Commons.Refer;
using PipServices3.Rpc.Services;

namespace Wexxle.Guide.Services.Version1
{
    public class GuidesHttpServiceV1: CommandableHttpService
    {
        public GuidesHttpServiceV1()
            : base("v1/guides")
        {
            _dependencyResolver.Put("controller", new Descriptor("wexxle-guides", "controller", "default", "*", "1.0"));
        }
    }
}
