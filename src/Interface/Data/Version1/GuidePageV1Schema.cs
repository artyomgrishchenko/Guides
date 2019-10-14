using PipServices3.Commons.Convert;
using PipServices3.Commons.Validate;

namespace Guides.Data.Version1
{
    public class GuidePageV1Schema : ObjectSchema
    {
        public GuidePageV1Schema()
        {
            this.WithOptionalProperty("title"	, TypeCode.Map);
			this.WithRequiredProperty("content"	, TypeCode.Map);
			this.WithOptionalProperty("moreUrl"	, TypeCode.String);
			this.WithOptionalProperty("color"	, TypeCode.String);
			this.WithOptionalProperty("picId"	, TypeCode.String);
			this.WithOptionalProperty("picUri"	, TypeCode.String);
		}
    }
}
