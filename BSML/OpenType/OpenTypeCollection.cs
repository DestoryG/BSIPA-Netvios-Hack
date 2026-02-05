using System;
using System.Collections;
using System.Collections.Generic;

namespace BeatSaberMarkupLanguage.OpenType
{
	// Token: 0x02000079 RID: 121
	public class OpenTypeCollection : IEnumerable<OpenTypeFont>, IEnumerable, IDisposable
	{
		// Token: 0x1700009F RID: 159
		// (get) Token: 0x06000219 RID: 537 RVA: 0x0000C4EE File Offset: 0x0000A6EE
		public OpenTypeCollectionReader Reader { get; }

		// Token: 0x0600021A RID: 538 RVA: 0x0000C4F6 File Offset: 0x0000A6F6
		public OpenTypeCollection(OpenTypeCollectionReader reader, bool lazyLoad = true)
			: this(reader.ReadCollectionHeader(), reader, lazyLoad)
		{
		}

		// Token: 0x0600021B RID: 539 RVA: 0x0000C506 File Offset: 0x0000A706
		public OpenTypeCollection(CollectionHeader header, OpenTypeCollectionReader reader, bool lazyLoad = true)
		{
			this.header = header;
			this.lazy = lazyLoad;
			if (lazyLoad)
			{
				this.Reader = reader;
				return;
			}
			this.LoadAll(reader);
		}

		// Token: 0x0600021C RID: 540 RVA: 0x0000C52E File Offset: 0x0000A72E
		private void LoadAll(OpenTypeCollectionReader reader)
		{
			this.fonts = this.ReadFonts(reader);
		}

		// Token: 0x170000A0 RID: 160
		// (get) Token: 0x0600021D RID: 541 RVA: 0x0000C540 File Offset: 0x0000A740
		public IEnumerable<OpenTypeFont> Fonts
		{
			get
			{
				OpenTypeFont[] array;
				if ((array = this.fonts) == null)
				{
					array = (this.fonts = this.ReadFonts(this.Reader));
				}
				return array;
			}
		}

		// Token: 0x0600021E RID: 542 RVA: 0x0000C56C File Offset: 0x0000A76C
		private OpenTypeFont[] ReadFonts(OpenTypeCollectionReader reader)
		{
			return reader.ReadFonts(this.header, this.lazy);
		}

		// Token: 0x0600021F RID: 543 RVA: 0x0000C580 File Offset: 0x0000A780
		public IEnumerator<OpenTypeFont> GetEnumerator()
		{
			return this.Fonts.GetEnumerator();
		}

		// Token: 0x06000220 RID: 544 RVA: 0x0000C580 File Offset: 0x0000A780
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.Fonts.GetEnumerator();
		}

		// Token: 0x06000221 RID: 545 RVA: 0x0000C58D File Offset: 0x0000A78D
		protected virtual void Dispose(bool disposing)
		{
			if (!this.disposedValue)
			{
				this.disposedValue = true;
			}
		}

		// Token: 0x06000222 RID: 546 RVA: 0x0000C5A0 File Offset: 0x0000A7A0
		public void Dispose()
		{
			this.Dispose(true);
		}

		// Token: 0x0400006C RID: 108
		private readonly CollectionHeader header;

		// Token: 0x0400006D RID: 109
		private OpenTypeFont[] fonts;

		// Token: 0x0400006E RID: 110
		private readonly bool lazy;

		// Token: 0x04000070 RID: 112
		private bool disposedValue;
	}
}
