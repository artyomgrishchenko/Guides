using System.Threading.Tasks;
using Wexxle.Guide.Data.Version1;
using PipServices3.Commons.Data;

namespace Wexxle.Guide.Clients.Version1
{
    public class GuidesNullClientV1 : IGuidesClientV1
    {
        public async Task<DataPage<GuideV1>> GetGuidesAsync(string correlationId, FilterParams filter, PagingParams paging)
        {
            return await Task.FromResult(new DataPage<GuideV1>());
        }

        public async Task<GuideV1> GetGuideByIdAsync(string correlationId, string id)
        {
            return await Task.FromResult(new GuideV1());
        }

        public async Task<GuideV1> GetGuideByUdiAsync(string correlationId, string udi)
        {
            return await Task.FromResult(new GuideV1());
        }

        public async Task<GuideV1> CreateGuideAsync(string correlationId, GuideV1 guide)
        {
            return await Task.FromResult(new GuideV1());
        }

        public async Task<GuideV1> UpdateGuideAsync(string correlationId, GuideV1 guide)
        {
            return await Task.FromResult(new GuideV1());
        }

        public async Task<GuideV1> DeleteGuideByIdAsync(string correlationId, string id)
        {
            return await Task.FromResult(new GuideV1());
        }
    }
}
