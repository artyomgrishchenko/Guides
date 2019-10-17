using Wexxle.Guide.Data.Version1;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Wexxle.Attachment.Client.Version1;
using Wexxle.Attachment.Data.Version1;

namespace Wexxle.Guide.Logic
{
	public class AttachmentsConnector
	{
		private IAttachmentsClientV1 _attachmentsClient;

		public AttachmentsConnector(IAttachmentsClientV1 attachmentsClientV1)
		{
			_attachmentsClient = attachmentsClientV1;
		}

		private string[] ExtractAttachmentIds(GuideV1 guide)
		{
			List<string> ids = new List<string>();

			if (guide.Pages != null)
			{
				foreach (var page in guide.Pages)
				{
					if (page.PicId != null)
						ids.Add(page.PicId);
				}
			}

			return ids.ToArray();
		}

		async public Task<BlobAttachmentV1[]> AddAttachmentsAsync(string correlationId, GuideV1 guide)
		{
			if (_attachmentsClient == null || guide == null)
			{
				return null;
			}

			var ids = ExtractAttachmentIds(guide);
			var reference = new ReferenceV1(guide.Id, "guide");
			return await _attachmentsClient.AddAttachmentsAsync(correlationId, reference, ids);
		}

		async public Task<BlobAttachmentV1[]> UpdateAttachmentsAsync(string correlationId, GuideV1 oldGuide, GuideV1 newGuide)
		{
			if (_attachmentsClient == null || oldGuide == null || newGuide == null)
			{
				return null;
			}

			var oldIds = ExtractAttachmentIds(oldGuide);
			var newIds = ExtractAttachmentIds(newGuide);
			var reference = new ReferenceV1(newGuide.Id, "guide");
			return await _attachmentsClient.UpdateAttachmentsAsync(correlationId, reference, oldIds, newIds);
		}

		async public Task<BlobAttachmentV1[]> RemoveAttachmentsAsync(string correlationId, GuideV1 guide)
		{
			if (_attachmentsClient == null || guide == null)
			{
				return null;
			}

			var ids = this.ExtractAttachmentIds(guide);
			var reference = new ReferenceV1(guide.Id, "guide");
			return await _attachmentsClient.RemoveAttachmentsAsync(correlationId, reference, ids);
		}
	}
}
