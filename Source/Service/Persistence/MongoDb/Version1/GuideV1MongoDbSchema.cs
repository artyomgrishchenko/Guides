using System;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using PipServices3.Commons.Data;
using Wexxle.Guide.Data.Version1;

namespace Wexxle.Guide.Persistence.MongoDb.Version1
{
	[BsonIgnoreExtraElements]
	[BsonNoId]
	public class GuideV1MongoDbSchema : IStringIdentifiable
	{

		[BsonElement("id")] public string Id { get; set; }
		[BsonElement("name")] public string Name { get; set; }
		[BsonElement("type")] public string Type { get; set; }
		[BsonElement("app ")] public string App { get; set; }
		[BsonElement("min_ver")] public long? MinVer { get; set; }
		[BsonElement("max_ver")] public long? MaxVer { get; set; }
		[BsonElement("create_time")] public DateTime CreateTime { get; set; }
		[BsonElement("pages")] public List<GuidePageV1MongoDbSchema> Pages { get; set; } = new List<GuidePageV1MongoDbSchema>();
		[BsonElement("tags")] public List<string> Tags { get; set; } = new List<string>();
		[BsonElement("all_tags")] public List<string> AllTags { get; set; } = new List<string>();
		[BsonElement("status")] public string Status { get; set; }
		[BsonElement("custom_hdr")] public object CustomHdr { get; set; }
		[BsonElement("custom_dat")] public object CustomDat { get; set; }

	}
}
