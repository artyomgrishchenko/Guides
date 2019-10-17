using PipServices3.Commons.Convert;
using PipServices3.Commons.Validate;

namespace Wexxle.Guide.Data.Version1
{
    public class GuidePageV1Schema : ObjectSchema
    {
        public GuidePageV1Schema()
        {
            this.WithOptionalProperty("title"	, TypeCode.Map);
			this.WithRequiredProperty("content"	, TypeCode.Map);
			this.WithOptionalProperty("more_url"	, TypeCode.String);
			this.WithOptionalProperty("color"	, TypeCode.String);
			this.WithOptionalProperty("pic_id", TypeCode.String);
			this.WithOptionalProperty("pic_uri", TypeCode.String);
		}
    }
}
