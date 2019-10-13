using PipServices3.Commons.Refer;
using PipServices3.Components.Build;
using Guides.Persistence;
using Guides.Logic;
using Guides.Services.Version1;

namespace Guides.Build
{
    public class GuidesServiceFactory : Factory
    {
        public static Descriptor Descriptor = new Descriptor("guides", "factory", "service", "default", "1.0");
        public static Descriptor MemoryPersistenceDescriptor = new Descriptor("guides", "persistence", "memory", "*", "1.0");
        public static Descriptor MongoDbPersistenceDescriptor = new Descriptor("guides", "persistence", "mongodb", "*", "1.0");
        public static Descriptor ControllerDescriptor = new Descriptor("guides", "controller", "default", "*", "1.0");
        public static Descriptor HttpServiceDescriptor = new Descriptor("guides", "service", "http", "*", "1.0");


        public GuidesServiceFactory()
        {
            RegisterAsType(MemoryPersistenceDescriptor, typeof(GuidesMemoryPersistence));
            RegisterAsType(MongoDbPersistenceDescriptor, typeof(GuidesMongoDbPersistence));
            RegisterAsType(ControllerDescriptor, typeof(GuidesController));
            RegisterAsType(HttpServiceDescriptor, typeof(GuidesHttpServiceV1));
        }
    }
}
