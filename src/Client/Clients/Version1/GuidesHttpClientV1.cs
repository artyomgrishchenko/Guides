using System.Threading.Tasks;
using Wexxle.Guide.Data.Version1;
using PipServices3.Commons.Data;
using PipServices3.Rpc.Clients;

namespace Wexxle.Guide.Clients.Version1
{
    public class GuidesHttpClientV1 : CommandableHttpClient, IGuidesClientV1
    {
        public GuidesHttpClientV1()
            : base("v1/guides")
        { }

        public async Task<DataPage<GuideV1>> GetGuidesAsync(string correlationId, FilterParams filter, PagingParams paging)
        {
            return await CallCommandAsync<DataPage<GuideV1>>(
                "get_guides",
                correlationId,
                new
                {
                    filter = filter,
                    paging = paging
                }
            );
        }

        public async Task<GuideV1> GetGuideByIdAsync(string correlationId, string id)
        {
            return await CallCommandAsync<GuideV1>(
                "get_guide_by_id",
                correlationId,
                new
                {
                    guide_id = id
                }
            );
        }

        public async Task<GuideV1> GetGuideByUdiAsync(string correlationId, string udi)
        {
            return await CallCommandAsync<GuideV1>(
                "get_guide_by_udi",
                correlationId,
                new
                {
                    udi = udi
                }
            );
        }

        public async Task<GuideV1> CreateGuideAsync(string correlationId, GuideV1 guide)
        {
            return await CallCommandAsync<GuideV1>(
                "create_guide",
                correlationId,
                new
                {
                    guide = guide
                }
            );
        }

        public async Task<GuideV1> UpdateGuideAsync(string correlationId, GuideV1 guide)
        {
            return await CallCommandAsync<GuideV1>(
                "update_guide",
                correlationId,
                new
                {
                    guide = guide
                }
            );
        }

        public async Task<GuideV1> DeleteGuideByIdAsync(string correlationId, string id)
        {
            return await CallCommandAsync<GuideV1>(
                "delete_guide_by_id",
                correlationId,
                new
                {
                    guide_id = id
                }
            );
        }

    }
}
