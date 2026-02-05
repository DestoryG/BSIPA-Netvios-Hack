using System;

namespace IniParser.Model
{
	// Token: 0x0200000D RID: 13
	public class IniDataCaseInsensitive : IniData
	{
		// Token: 0x0600008A RID: 138 RVA: 0x00003B82 File Offset: 0x00001D82
		public IniDataCaseInsensitive()
			: base(new SectionDataCollection(StringComparer.OrdinalIgnoreCase))
		{
			base.Global = new KeyDataCollection(StringComparer.OrdinalIgnoreCase);
		}

		// Token: 0x0600008B RID: 139 RVA: 0x00003BA7 File Offset: 0x00001DA7
		public IniDataCaseInsensitive(SectionDataCollection sdc)
			: base(new SectionDataCollection(sdc, StringComparer.OrdinalIgnoreCase))
		{
			base.Global = new KeyDataCollection(StringComparer.OrdinalIgnoreCase);
		}

		// Token: 0x0600008C RID: 140 RVA: 0x00003BD0 File Offset: 0x00001DD0
		public IniDataCaseInsensitive(IniData ori)
			: this(new SectionDataCollection(ori.Sections, StringComparer.OrdinalIgnoreCase))
		{
			base.Global = (KeyDataCollection)ori.Global.Clone();
			base.Configuration = ori.Configuration.Clone();
		}
	}
}
