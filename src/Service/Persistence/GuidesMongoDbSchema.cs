using Guides.Data.Version1;
using MongoDB.Bson.Serialization.Attributes;
using PipServices.Commons.Data;

namespace Guides.Persistence
{
    [BsonIgnoreExtraElements]
    public class GuidesMongoDbSchema : IStringIdentifiable
    {
        [BsonElement("id")]
        public string Id { get; set; }
        [BsonElement("site_id")]
        public string SiteId { get; set; }
        [BsonElement("type")]
        public string Type { get; set; }
        [BsonElement("udi")]
        public string Udi { get; set; }
        [BsonElement("label")]
        public string Label { get; set; }
        [BsonElement("center")]
        public CenterObjectV1 Center { get; set; }
        [BsonElement("radius")]
        public double Radius { get; set; }

    }
}
