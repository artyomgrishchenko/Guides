using PipServices3.Commons.Commands;
using PipServices3.Commons.Config;
using PipServices3.Commons.Data;
using PipServices3.Commons.Refer;
using Guides.Persistence;
using System.Threading.Tasks;
using Guides.Data.Version1;
using Guides.Logic;

namespace Guides.Logic
{
    public class GuidesController : IGuidesController, IConfigurable, IReferenceable, ICommandable
    {
        private IGuidesPersistence _persistence;
        private GuidesCommandSet _commandSet;

        public GuidesController()
        {}

        public void Configure(ConfigParams config)
        {}

        public void SetReferences(IReferences references)
        {
            _persistence = references.GetOneRequired<IGuidesPersistence>(
                new Descriptor("guides", "persistence", "*", "*", "1.0")
            );
        }

        public CommandSet GetCommandSet()
        {
            if (_commandSet == null)
                _commandSet = new GuidesCommandSet(this);
            return _commandSet;
        }

        public async Task<DataPage<GuideV1>> GetGuidesAsync(string correlationId, FilterParams filter, PagingParams paging)
        {
            return await _persistence.GetPageByFilterAsync(correlationId, filter, paging);
        }

        public async Task<GuideV1> GetGuideByIdAsync(string correlationId, string id)
        {
            return await _persistence.GetOneByIdAsync(correlationId, id);
        }

        public async Task<GuideV1> CreateGuideAsync(string correlationId, GuideV1 guide)
        {
            guide.Id = guide.Id ?? IdGenerator.NextLong();
            guide.Type = guide.Type ?? GuideTypeV1.NewRelease;

            return await _persistence.CreateAsync(correlationId, guide);
        }

        public async Task<GuideV1> UpdateGuideAsync(string correlationId, GuideV1 guide)
        {
            guide.Type = guide.Type ?? GuideTypeV1.NewRelease;

            return await _persistence.UpdateAsync(correlationId, guide);
        }

        public async Task<GuideV1> DeleteGuideByIdAsync(string correlationId, string id)
        {
            return await _persistence.DeleteByIdAsync(correlationId, id);
        }
    }
}
