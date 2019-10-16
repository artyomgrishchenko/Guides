using PipServices3.Commons.Refer;
using PipServices3.Components.Build;
using Wexxle.Guide.Persistence;
using Wexxle.Guide.Logic;
using Wexxle.Guide.Services.Version1;

namespace Wexxle.Guide.Build
{
    public class GuidesServiceFactory : Factory
    {
        public static Descriptor Descriptor = new Descriptor("wexxle-guides", "factory", "service", "default", "1.0");
        public static Descriptor MemoryPersistenceDescriptor = new Descriptor("wexxle-guides", "persistence", "memory", "*", "1.0");
        public static Descriptor MongoDbPersistenceDescriptor = new Descriptor("wexxle-guides", "persistence", "mongodb", "*", "1.0");
        public static Descriptor ControllerDescriptor = new Descriptor("wexxle-guides", "controller", "default", "*", "1.0");
        public static Descriptor HttpServiceDescriptor = new Descriptor("wexxle-guides", "service", "http", "*", "1.0");


        public GuidesServiceFactory()
        {
            RegisterAsType(MemoryPersistenceDescriptor, typeof(GuidesMemoryPersistence));
            RegisterAsType(MongoDbPersistenceDescriptor, typeof(GuidesMongoDbPersistence));
            RegisterAsType(ControllerDescriptor, typeof(GuidesController));
            RegisterAsType(HttpServiceDescriptor, typeof(GuidesHttpServiceV1));
        }
    }
}
