using System;

namespace System.Runtime.Serialization
{
	// Token: 0x020000D9 RID: 217
	internal sealed class TypeInformation
	{
		// Token: 0x06000C23 RID: 3107 RVA: 0x00034518 File Offset: 0x00032718
		internal TypeInformation(string fullTypeName, string assemblyString, bool hasTypeForwardedFrom)
		{
			this.fullTypeName = fullTypeName;
			this.assemblyString = assemblyString;
			this.hasTypeForwardedFrom = hasTypeForwardedFrom;
		}

		// Token: 0x17000276 RID: 630
		// (get) Token: 0x06000C24 RID: 3108 RVA: 0x00034535 File Offset: 0x00032735
		internal string FullTypeName
		{
			get
			{
				return this.fullTypeName;
			}
		}

		// Token: 0x17000277 RID: 631
		// (get) Token: 0x06000C25 RID: 3109 RVA: 0x0003453D File Offset: 0x0003273D
		internal string AssemblyString
		{
			get
			{
				return this.assemblyString;
			}
		}

		// Token: 0x17000278 RID: 632
		// (get) Token: 0x06000C26 RID: 3110 RVA: 0x00034545 File Offset: 0x00032745
		internal bool HasTypeForwardedFrom
		{
			get
			{
				return this.hasTypeForwardedFrom;
			}
		}

		// Token: 0x040004F6 RID: 1270
		private string fullTypeName;

		// Token: 0x040004F7 RID: 1271
		private string assemblyString;

		// Token: 0x040004F8 RID: 1272
		private bool hasTypeForwardedFrom;
	}
}
