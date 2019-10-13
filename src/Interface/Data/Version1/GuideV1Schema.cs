using PipServices3.Commons.Convert;
using PipServices3.Commons.Validate;

namespace Guides.Data.Version1
{
    public class GuideV1Schema : ObjectSchema
    {
        public GuideV1Schema()
        {
            this.WithOptionalProperty("id"				, TypeCode.String);
			this.WithRequiredProperty("name"			, TypeCode.String);
			this.WithOptionalProperty("type"			, TypeCode.String);
			this.WithOptionalProperty("app"				, TypeCode.String);
			this.WithOptionalProperty("min_ver"			, TypeCode.Integer);
			this.WithOptionalProperty("max_ver"			, TypeCode.Integer);
			this.WithOptionalProperty("create_time"		, TypeCode.DateTime);
			this.WithOptionalProperty("pages"			, null);
			this.WithOptionalProperty("tags"			, null);
			this.WithOptionalProperty("all_tags"        , null);
			this.WithOptionalProperty("status"			, TypeCode.String);
			this.WithOptionalProperty("custom_hdr"		, TypeCode.Object);
			this.WithOptionalProperty("custom_dat"		, TypeCode.Object);
        }
    }
}
