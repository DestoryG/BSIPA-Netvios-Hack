using System;
using System.Collections.Generic;

namespace System.Configuration
{
	// Token: 0x02000073 RID: 115
	internal sealed class UriSectionData
	{
		// Token: 0x060004A0 RID: 1184 RVA: 0x0001F57B File Offset: 0x0001D77B
		public UriSectionData()
		{
			this.schemeSettings = new Dictionary<string, SchemeSettingInternal>();
		}

		// Token: 0x1700009B RID: 155
		// (get) Token: 0x060004A1 RID: 1185 RVA: 0x0001F58E File Offset: 0x0001D78E
		// (set) Token: 0x060004A2 RID: 1186 RVA: 0x0001F596 File Offset: 0x0001D796
		public UriIdnScope? IdnScope
		{
			get
			{
				return this.idnScope;
			}
			set
			{
				this.idnScope = value;
			}
		}

		// Token: 0x1700009C RID: 156
		// (get) Token: 0x060004A3 RID: 1187 RVA: 0x0001F59F File Offset: 0x0001D79F
		// (set) Token: 0x060004A4 RID: 1188 RVA: 0x0001F5A7 File Offset: 0x0001D7A7
		public bool? IriParsing
		{
			get
			{
				return this.iriParsing;
			}
			set
			{
				this.iriParsing = value;
			}
		}

		// Token: 0x1700009D RID: 157
		// (get) Token: 0x060004A5 RID: 1189 RVA: 0x0001F5B0 File Offset: 0x0001D7B0
		public Dictionary<string, SchemeSettingInternal> SchemeSettings
		{
			get
			{
				return this.schemeSettings;
			}
		}

		// Token: 0x04000BEC RID: 3052
		private UriIdnScope? idnScope;

		// Token: 0x04000BED RID: 3053
		private bool? iriParsing;

		// Token: 0x04000BEE RID: 3054
		private Dictionary<string, SchemeSettingInternal> schemeSettings;
	}
}
