using System.Threading.Tasks;
using PipServices3.Commons.Data;
using Wexxle.Guide.Data.Version1;

namespace Wexxle.Guide.Persistence
{
    public interface IGuidesPersistence
    {
        Task<DataPage<GuideV1>> GetPageByFilterAsync(string correlationId, FilterParams filter, PagingParams paging);
        Task<GuideV1> GetOneByIdAsync(string correlationId, string id);
        Task<GuideV1> CreateAsync(string correlationId, GuideV1 item);
        Task<GuideV1> UpdateAsync(string correlationId, GuideV1 item);
        Task<GuideV1> DeleteByIdAsync(string correlationId, string id);
    }
}
