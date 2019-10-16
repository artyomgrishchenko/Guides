
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Wexxle.Guide.Data.Version1
{
	[DataContract]
	public class GuidePageV1
	{
		[DataMember(Name = "title")] public Dictionary<string, string> Title { get; set; }
		[DataMember(Name = "content")] public Dictionary<string, string> Content { get; set; }
		[DataMember(Name = "moreUrl")] public string MoreUrl	{ get; set; }
		[DataMember(Name = "color")] public string Color		{ get; set; }
		[DataMember(Name = "picId")] public string PicId		{ get; set; }
		[DataMember(Name = "picUri")] public string PicUri		{ get; set; }
	}
}
