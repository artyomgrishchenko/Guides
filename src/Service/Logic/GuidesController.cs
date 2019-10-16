using PipServices3.Commons.Commands;
using PipServices3.Commons.Config;
using PipServices3.Commons.Data;
using PipServices3.Commons.Refer;
using Guides.Persistence;
using System.Threading.Tasks;
using Guides.Data.Version1;
using Guides.Logic;
using System;
using Wexxle.Guide.Logic;
using Wexxle.Attachment.Client.Version1;

namespace Guides.Logic
{
    public class GuidesController : IGuidesController, IConfigurable, IReferenceable, ICommandable
    {
        private IGuidesPersistence _persistence;
        private GuidesCommandSet _commandSet;
		private AttachmentsConnector _attachmentsConnector;
		private IAttachmentsClientV1 _attachmentsClient;

        public GuidesController()
        {}

        public void Configure(ConfigParams config)
        {}

        public void SetReferences(IReferences references)
        {
            _persistence = references.GetOneRequired<IGuidesPersistence>(
                new Descriptor("guides", "persistence", "*", "*", "1.0")
            );

			_attachmentsClient = references.GetOneRequired<IAttachmentsClientV1>(
				new Descriptor("attachments", "client", "*", "*", "1.0")
				);

			_attachmentsConnector = new AttachmentsConnector(_attachmentsClient);
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
			guide.CreateTime = DateTime.UtcNow;

			var newGuide = await _persistence.CreateAsync(correlationId, guide);
			await _attachmentsConnector.AddAttachmentsAsync(correlationId, newGuide);

			return newGuide;
			
        }

        public async Task<GuideV1> UpdateGuideAsync(string correlationId, GuideV1 guide)
        {
			var newGuide = await _persistence.UpdateAsync(correlationId, guide);
			await _attachmentsConnector.UpdateAttachmentsAsync(correlationId, guide, newGuide);

			return newGuide;
        }

        public async Task<GuideV1> DeleteGuideByIdAsync(string correlationId, string id)
        {
			var oldGuide =  await _persistence.DeleteByIdAsync(correlationId, id);
			await _attachmentsConnector.RemoveAttachmentsAsync(correlationId, oldGuide);

			return oldGuide;
		}
    }
}
