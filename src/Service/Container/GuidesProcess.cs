using Wexxle.Guide.Build;
using PipServices3.Container;
using PipServices3.Rpc.Build;

namespace Wexxle.Guide.Container
{
    public class GuidesProcess : ProcessContainer
    {
        public GuidesProcess()
            : base("guides", "Guides microservice")
        {
            _factories.Add(new DefaultRpcFactory());
            _factories.Add(new GuidesServiceFactory());
        }
    }
}
