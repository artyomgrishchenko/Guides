using PipServices3.Commons.Convert;
using PipServices3.Commons.Validate;

namespace Guides.Data.Version1
{
    public class GuideV1Schema : ObjectSchema
    {
        public GuideV1Schema()
        {
            this.WithOptionalProperty("id"				, TypeCode.String);
			this.WithOptionalProperty("name"			, TypeCode.String);
			this.WithRequiredProperty("type"			, TypeCode.String);
			this.WithOptionalProperty("app"				, TypeCode.String);
			this.WithOptionalProperty("min_ver"			, TypeCode.Long);
			this.WithOptionalProperty("max_ver"			, TypeCode.Long);
			this.WithOptionalProperty("create_time"		, TypeCode.DateTime);
			this.WithOptionalProperty("pages"			, new ArraySchema(new GuidePageV1Schema()));
			this.WithOptionalProperty("tags"			, TypeCode.Array);
			this.WithOptionalProperty("all_tags"        , TypeCode.Array);
			this.WithOptionalProperty("status"			, TypeCode.String);
			this.WithOptionalProperty("custom_hdr"		, TypeCode.Object);
			this.WithOptionalProperty("custom_dat"		, TypeCode.Object);
        }
    }
}
