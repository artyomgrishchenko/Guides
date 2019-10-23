
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Wexxle.Guide.Data.Version1
{
	[DataContract]
	public class GuidePageV1
	{
		[DataMember(Name = "title")] public Dictionary<string, string> Title { get; set; } = new Dictionary<string, string>();
		[DataMember(Name = "content")] public Dictionary<string, string> Content { get; set; } = new Dictionary<string, string>();
		[DataMember(Name = "more_url")] public string MoreUrl	{ get; set; }
		[DataMember(Name = "color")] public string Color		{ get; set; }
		[DataMember(Name = "pic_id")] public string PicId		{ get; set; }
		[DataMember(Name = "pic_uri")] public string PicUri		{ get; set; }
	}
}
