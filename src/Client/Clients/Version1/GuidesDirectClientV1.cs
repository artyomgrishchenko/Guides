using System.Threading.Tasks;
using Guides.Clients.Version1;
using Guides.Data.Version1;
using Guides.Logic;
using PipServices3.Commons.Data;
using PipServices3.Commons.Refer;
using PipServices3.Rpc.Clients;

namespace Guides.Clients.Version1
{
    public class GuidesDirectClientV1 : DirectClient<IGuidesController>, IGuidesClientV1
    {
        public GuidesDirectClientV1() : base()
        {
            _dependencyResolver.Put("controller", new Descriptor("guides", "controller", "*", "*", "1.0"));
        }

        public async Task<DataPage<GuideV1>> GetGuidesAsync(
            string correlationId, FilterParams filter, PagingParams paging)
        {
            using (Instrument(correlationId, "guides.get_guides"))
            {
                return await _controller.GetGuidesAsync(correlationId, filter, paging);
            }
        }

        public async Task<GuideV1> GetGuideByIdAsync(string correlationId, string id)
        {
            using (Instrument(correlationId, "guides.get_guide_by_id"))
            {
                return await _controller.GetGuideByIdAsync(correlationId, id);
            }
        }

        public async Task<GuideV1> CreateGuideAsync(string correlationId, GuideV1 guide)
        {
            using (Instrument(correlationId, "guides.create_guide"))
            {
                return await _controller.CreateGuideAsync(correlationId, guide);
            }
        }

        public async Task<GuideV1> UpdateGuideAsync(string correlationId, GuideV1 guide)
        {
            using (Instrument(correlationId, "guides.update_guide"))
            {
                return await _controller.UpdateGuideAsync(correlationId, guide);
            }
        }

        public async Task<GuideV1> DeleteGuideByIdAsync(string correlationId, string id)
        {
            using (Instrument(correlationId, "guides.delete_guide_by_id"))
            {
                return await _controller.DeleteGuideByIdAsync(correlationId, id);
            }
        }
    }
}
