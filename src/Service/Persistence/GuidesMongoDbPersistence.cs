using System.Threading.Tasks;
using Guides.Data.Version1;
using MongoDB.Driver;
using PipServices3.Commons.Data;
using PipServices3.MongoDb.Persistence;

namespace Guides.Persistence
{
    public class GuidesMongoDbPersistence : IdentifiableMongoDbPersistence<GuideV1, string>, IGuidesPersistence
    {
        public GuidesMongoDbPersistence()
            : base("guides")
        { }

        private new FilterDefinition<GuideV1> ComposeFilter(FilterParams filterParams)
        {
            filterParams = filterParams ?? new FilterParams();

            var builder = Builders<GuideV1>.Filter;
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

        public async Task<DataPage<GuideV1>> GetPageByFilterAsync(
            string correlationId, FilterParams filter, PagingParams paging)
        {
            return await GetPageByFilterAsync(correlationId, ComposeFilter(filter), paging);
        }
    }
}

