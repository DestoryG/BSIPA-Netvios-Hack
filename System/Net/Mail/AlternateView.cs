using System;
using System.IO;
using System.Net.Mime;
using System.Text;

namespace System.Net.Mail
{
	// Token: 0x02000254 RID: 596
	public class AlternateView : AttachmentBase
	{
		// Token: 0x060016A5 RID: 5797 RVA: 0x00075351 File Offset: 0x00073551
		internal AlternateView()
		{
		}

		// Token: 0x060016A6 RID: 5798 RVA: 0x00075359 File Offset: 0x00073559
		public AlternateView(string fileName)
			: base(fileName)
		{
		}

		// Token: 0x060016A7 RID: 5799 RVA: 0x00075362 File Offset: 0x00073562
		public AlternateView(string fileName, string mediaType)
			: base(fileName, mediaType)
		{
		}

		// Token: 0x060016A8 RID: 5800 RVA: 0x0007536C File Offset: 0x0007356C
		public AlternateView(string fileName, ContentType contentType)
			: base(fileName, contentType)
		{
		}

		// Token: 0x060016A9 RID: 5801 RVA: 0x00075376 File Offset: 0x00073576
		public AlternateView(Stream contentStream)
			: base(contentStream)
		{
		}

		// Token: 0x060016AA RID: 5802 RVA: 0x0007537F File Offset: 0x0007357F
		public AlternateView(Stream contentStream, string mediaType)
			: base(contentStream, mediaType)
		{
		}

		// Token: 0x060016AB RID: 5803 RVA: 0x00075389 File Offset: 0x00073589
		public AlternateView(Stream contentStream, ContentType contentType)
			: base(contentStream, contentType)
		{
		}

		// Token: 0x170004BB RID: 1211
		// (get) Token: 0x060016AC RID: 5804 RVA: 0x00075393 File Offset: 0x00073593
		public LinkedResourceCollection LinkedResources
		{
			get
			{
				if (this.disposed)
				{
					throw new ObjectDisposedException(base.GetType().FullName);
				}
				if (this.linkedResources == null)
				{
					this.linkedResources = new LinkedResourceCollection();
				}
				return this.linkedResources;
			}
		}

		// Token: 0x170004BC RID: 1212
		// (get) Token: 0x060016AD RID: 5805 RVA: 0x000753C7 File Offset: 0x000735C7
		// (set) Token: 0x060016AE RID: 5806 RVA: 0x000753CF File Offset: 0x000735CF
		public Uri BaseUri
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

		// Token: 0x060016AF RID: 5807 RVA: 0x000753D8 File Offset: 0x000735D8
		public static AlternateView CreateAlternateViewFromString(string content)
		{
			AlternateView alternateView = new AlternateView();
			alternateView.SetContentFromString(content, null, string.Empty);
			return alternateView;
		}

		// Token: 0x060016B0 RID: 5808 RVA: 0x000753FC File Offset: 0x000735FC
		public static AlternateView CreateAlternateViewFromString(string content, Encoding contentEncoding, string mediaType)
		{
			AlternateView alternateView = new AlternateView();
			alternateView.SetContentFromString(content, contentEncoding, mediaType);
			return alternateView;
		}

		// Token: 0x060016B1 RID: 5809 RVA: 0x0007541C File Offset: 0x0007361C
		public static AlternateView CreateAlternateViewFromString(string content, ContentType contentType)
		{
			AlternateView alternateView = new AlternateView();
			alternateView.SetContentFromString(content, contentType);
			return alternateView;
		}

		// Token: 0x060016B2 RID: 5810 RVA: 0x00075438 File Offset: 0x00073638
		protected override void Dispose(bool disposing)
		{
			if (this.disposed)
			{
				return;
			}
			if (disposing && this.linkedResources != null)
			{
				this.linkedResources.Dispose();
			}
			base.Dispose(disposing);
		}

		// Token: 0x04001762 RID: 5986
		private LinkedResourceCollection linkedResources;
	}
}
