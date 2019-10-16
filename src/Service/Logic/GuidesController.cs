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
using PipServices3.Components.Logic;

namespace Guides.Logic
{
    public class GuidesController : AbstractController, IGuidesController,  ICommandable
    {
        private IGuidesPersistence _persistence;
        private GuidesCommandSet _commandSet;
		private AttachmentsConnector _attachmentsConnector;
		private IAttachmentsClientV1 _attachmentsClient;

		public override string Component => "Guides";

		public GuidesController()
        {
			_dependencyResolver.Put("persistence", new Descriptor("wexxle-guides", "persistence", "*", "*", "1.0"));
			_dependencyResolver.Put("attachments", new Descriptor("wexxle-attachments", "client", "*", "*", "1.0"));
		}

		public override void Configure(ConfigParams config)
		{
			base.Configure(config);

			_dependencyResolver.Configure(config);
		}

		public override void SetReferences(IReferences references)
		{
			base.SetReferences(references);

			_dependencyResolver.SetReferences(references);

			_persistence = _dependencyResolver.GetOneRequired<IGuidesPersistence>("persistence");

			_attachmentsClient = _dependencyResolver.GetOneRequired<IAttachmentsClientV1>("attachments");

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
