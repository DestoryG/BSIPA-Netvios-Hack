using System;
using System.IO;
using System.Net.Mime;
using System.Text;

namespace System.Net.Mail
{
	// Token: 0x02000268 RID: 616
	public class LinkedResource : AttachmentBase
	{
		// Token: 0x0600171E RID: 5918 RVA: 0x000765B8 File Offset: 0x000747B8
		internal LinkedResource()
		{
		}

		// Token: 0x0600171F RID: 5919 RVA: 0x000765C0 File Offset: 0x000747C0
		public LinkedResource(string fileName)
			: base(fileName)
		{
		}

		// Token: 0x06001720 RID: 5920 RVA: 0x000765C9 File Offset: 0x000747C9
		public LinkedResource(string fileName, string mediaType)
			: base(fileName, mediaType)
		{
		}

		// Token: 0x06001721 RID: 5921 RVA: 0x000765D3 File Offset: 0x000747D3
		public LinkedResource(string fileName, ContentType contentType)
			: base(fileName, contentType)
		{
		}

		// Token: 0x06001722 RID: 5922 RVA: 0x000765DD File Offset: 0x000747DD
		public LinkedResource(Stream contentStream)
			: base(contentStream)
		{
		}

		// Token: 0x06001723 RID: 5923 RVA: 0x000765E6 File Offset: 0x000747E6
		public LinkedResource(Stream contentStream, string mediaType)
			: base(contentStream, mediaType)
		{
		}

		// Token: 0x06001724 RID: 5924 RVA: 0x000765F0 File Offset: 0x000747F0
		public LinkedResource(Stream contentStream, ContentType contentType)
			: base(contentStream, contentType)
		{
		}

		// Token: 0x170004C8 RID: 1224
		// (get) Token: 0x06001725 RID: 5925 RVA: 0x000765FA File Offset: 0x000747FA
		// (set) Token: 0x06001726 RID: 5926 RVA: 0x00076602 File Offset: 0x00074802
		public Uri ContentLink
		{
			get
			{
				return base.ContentLocation;
			}
			set
			{
				base.ContentLocation = value;
			}
		}

		// Token: 0x06001727 RID: 5927 RVA: 0x0007660C File Offset: 0x0007480C
		public static LinkedResource CreateLinkedResourceFromString(string content)
		{
			LinkedResource linkedResource = new LinkedResource();
			linkedResource.SetContentFromString(content, null, string.Empty);
			return linkedResource;
		}

		// Token: 0x06001728 RID: 5928 RVA: 0x00076630 File Offset: 0x00074830
		public static LinkedResource CreateLinkedResourceFromString(string content, Encoding contentEncoding, string mediaType)
		{
			LinkedResource linkedResource = new LinkedResource();
			linkedResource.SetContentFromString(content, contentEncoding, mediaType);
			return linkedResource;
		}

		// Token: 0x06001729 RID: 5929 RVA: 0x00076650 File Offset: 0x00074850
		public static LinkedResource CreateLinkedResourceFromString(string content, ContentType contentType)
		{
			LinkedResource linkedResource = new LinkedResource();
			linkedResource.SetContentFromString(content, contentType);
			return linkedResource;
		}
	}
}
