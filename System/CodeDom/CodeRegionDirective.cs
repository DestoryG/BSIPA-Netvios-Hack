using System;
using System.Runtime.InteropServices;

namespace System.CodeDom
{
	// Token: 0x0200064F RID: 1615
	[ClassInterface(ClassInterfaceType.AutoDispatch)]
	[ComVisible(true)]
	[Serializable]
	public class CodeRegionDirective : CodeDirective
	{
		// Token: 0x06003AA6 RID: 15014 RVA: 0x000F4374 File Offset: 0x000F2574
		public CodeRegionDirective()
		{
		}

		// Token: 0x06003AA7 RID: 15015 RVA: 0x000F437C File Offset: 0x000F257C
		public CodeRegionDirective(CodeRegionMode regionMode, string regionText)
		{
			this.RegionText = regionText;
			this.regionMode = regionMode;
		}

		// Token: 0x17000E1B RID: 3611
		// (get) Token: 0x06003AA8 RID: 15016 RVA: 0x000F4392 File Offset: 0x000F2592
		// (set) Token: 0x06003AA9 RID: 15017 RVA: 0x000F43A8 File Offset: 0x000F25A8
		public string RegionText
		{
			get
			{
				if (this.regionText != null)
				{
					return this.regionText;
				}
				return string.Empty;
			}
			set
			{
				this.regionText = value;
			}
		}

		// Token: 0x17000E1C RID: 3612
		// (get) Token: 0x06003AAA RID: 15018 RVA: 0x000F43B1 File Offset: 0x000F25B1
		// (set) Token: 0x06003AAB RID: 15019 RVA: 0x000F43B9 File Offset: 0x000F25B9
		public CodeRegionMode RegionMode
		{
			get
			{
				return this.regionMode;
			}
			set
			{
				this.regionMode = value;
			}
		}

		// Token: 0x04002C08 RID: 11272
		private string regionText;

		// Token: 0x04002C09 RID: 11273
		private CodeRegionMode regionMode;
	}
}
