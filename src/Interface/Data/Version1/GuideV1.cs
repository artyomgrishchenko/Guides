using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using PipServices3.Commons.Data;

namespace Wexxle.Guide.Data.Version1
{
	[DataContract]
	public class GuideV1 : IStringIdentifiable
	{
		[DataMember(Name = "id")] public string Id { get; set; }
		[DataMember(Name = "name")] public string Name { get; set; }
		[DataMember(Name = "type")] public string Type { get; set; }
		[DataMember(Name = "app ")] public string App { get; set; }
		[DataMember(Name = "min_ver")] public long? MinVer { get; set; }
		[DataMember(Name = "max_ver")] public long? MaxVer { get; set; }
		[DataMember(Name = "create_time")] public DateTime CreateTime { get; set; }
		// Content 
		[DataMember(Name = "pages")] public List<GuidePageV1> Pages { get; set; }
		// Search
		[DataMember(Name = "tags")] public List<string> Tags { get; set; }
		[DataMember(Name = "all_tags")] public List<string> AllTags { get; set; }
		// Status
		[DataMember(Name = "status")] public string Status { get; set; }
		// Custom fields
		[DataMember(Name = "custom_hdr")] public object CustomHdr { get; set; }
		[DataMember(Name = "custom_dat")] public object CustomDat { get; set; }
	}
}
