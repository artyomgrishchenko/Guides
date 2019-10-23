using Wexxle.Guide.Data.Version1;
using PipServices3.Commons.Data;
using System.Threading.Tasks;

namespace Wexxle.Guide.Logic
{
    public interface IGuidesController
    {
        Task<DataPage<GuideV1>> GetGuidesAsync(string correlationId, FilterParams filter, PagingParams paging);
        Task<GuideV1> GetGuideByIdAsync(string correlationId, string id);
        Task<GuideV1> CreateGuideAsync(string correlationId, GuideV1 guide);
        Task<GuideV1> UpdateGuideAsync(string correlationId, GuideV1 guide);
        Task<GuideV1> DeleteGuideByIdAsync(string correlationId, string id);
    }
}
