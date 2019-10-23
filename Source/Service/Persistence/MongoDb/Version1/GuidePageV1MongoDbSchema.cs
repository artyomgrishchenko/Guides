using System;
using System.Collections.Generic;
using System.Text;
using MongoDB.Bson.Serialization.Attributes;

namespace Wexxle.Guide.Persistence.MongoDb.Version1
{
	[BsonIgnoreExtraElements]
	[BsonNoId]
	public class GuidePageV1MongoDbSchema
	{
		[BsonElement("title")] public Dictionary<string, string> Title { get; set; } = new Dictionary<string, string>();
		[BsonElement("content")] public Dictionary<string, string> Content { get; set; } = new Dictionary<string, string>();
		[BsonElement("more_url")] public string MoreUrl { get; set; }
		[BsonElement("color")] public string Color { get; set; }
		[BsonElement("pic_id")] public string PicId { get; set; }
		[BsonElement("pic_uri")] public string PicUri { get; set; }

	}
}
