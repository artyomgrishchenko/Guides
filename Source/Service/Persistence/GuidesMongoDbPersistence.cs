using System.Threading.Tasks;
using Wexxle.Guide.Data.Version1;
using MongoDB.Driver;
using PipServices3.Commons.Data;
using PipServices3.Commons.Data.Mapper;
using PipServices3.MongoDb.Persistence;
using Wexxle.Guide.Persistence.MongoDb.Version1;

namespace Wexxle.Guide.Persistence
{
    public class GuidesMongoDbPersistence : IdentifiableMongoDbPersistence<GuideV1MongoDbSchema, string>, IGuidesPersistence
    {
        public GuidesMongoDbPersistence()
            : base("guides")
        { }

        private new FilterDefinition<GuideV1MongoDbSchema> ComposeFilter(FilterParams filterParams)
        {
            filterParams = filterParams ?? new FilterParams();

            var builder = Builders<GuideV1MongoDbSchema>.Filter;
            var filter = builder.Empty;

            var id = filterParams.GetAsNullableString("id");
            if (!string.IsNullOrEmpty(id))
                filter &= builder.Eq(b => b.Id, id);

			var name = filterParams.GetAsNullableString("name");
			if (!string.IsNullOrEmpty(name))
                filter &= builder.Eq(b => b.Name, name);
            
            var type = filterParams.GetAsNullableString("type");
            if (!string.IsNullOrEmpty(type))
                filter &= builder.Eq(b => b.Type, type);
            
            var app = filterParams.GetAsNullableString("app");
            if (!string.IsNullOrEmpty(app))
                filter &= builder.Eq(b => b.App, app);

            var tag = filterParams.GetAsNullableString("tag");
			if (!string.IsNullOrEmpty(tag))
				filter &= builder.Where(b => b.Tags.Contains(tag));

			var ids = filterParams.GetAsNullableString("ids");
			var idsList = !string.IsNullOrEmpty(ids) ? ids.Split(',') : null;
			if (idsList != null)
				filter &= builder.In(b => b.Id, idsList);

			return filter;
        }

        public async Task<DataPage<GuideV1>> GetPageByFilterAsync(string correlationId, FilterParams filter, PagingParams paging)
        {
			var result = await base.GetPageByFilterAsync(correlationId, ComposeFilter(filter), paging);
			var data = result.Data.ConvertAll(ToPublic);

			return new DataPage<GuideV1>
			{
				Data = data,
				Total = result.Total
			};
		}

        public async Task<GuideV1> GetByIdAsync(string correlationId, string id)
        {
	        var result = await GetOneByIdAsync(correlationId, id);

	        return ToPublic(result);
        }

		public async Task<GuideV1> CreateAsync(string correlationId, GuideV1 item)
        {
			var result = await CreateAsync(correlationId, FromPublic(item));

			return ToPublic(result);
		}

        public async Task<GuideV1> UpdateAsync(string correlationId, GuideV1 item)
        {
			var result = await UpdateAsync(correlationId, FromPublic(item));

			return ToPublic(result);

		}

        public new async Task<GuideV1> DeleteByIdAsync(string correlationId, string id)
        {
			var result = await base.DeleteByIdAsync(correlationId, id);

			return ToPublic(result);
		}

        private static GuideV1 ToPublic(GuideV1MongoDbSchema value)
        {
	        return value == null ? null : ObjectMapper.MapTo<GuideV1>(value);
        }

        private static GuideV1MongoDbSchema FromPublic(GuideV1 value)
        {
	        return ObjectMapper.MapTo<GuideV1MongoDbSchema>(value);
        }
	}
}

