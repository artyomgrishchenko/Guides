using Guides.Data.Version1;
using PipServices3.Commons.Config;
using PipServices3.Data.Persistence;

namespace Guides.Persistence
{
    public class GuidesFilePersistence : GuidesMemoryPersistence
    {
        protected JsonFilePersister<GuideV1> _persister;

        public GuidesFilePersistence()
        {
            _persister = new JsonFilePersister<GuideV1>();
            _loader = _persister;
            _saver = _persister;
        }

        public override void Configure(ConfigParams config)
        {
            base.Configure(config);
            _persister.Configure(config);
        }
    }
}
