using System;
using Microsoft.CSharp.RuntimeBinder.Syntax;

namespace Microsoft.CSharp.RuntimeBinder.Semantics
{
	// Token: 0x0200005A RID: 90
	internal sealed class PredefinedPropertyInfo
	{
		// Token: 0x06000307 RID: 775 RVA: 0x00014E0E File Offset: 0x0001300E
		public PredefinedPropertyInfo(PREDEFPROP property, PredefinedName name, PREDEFMETH getter)
		{
			this.property = property;
			this.name = name;
			this.getter = getter;
		}

		// Token: 0x0400046D RID: 1133
		public PREDEFPROP property;

		// Token: 0x0400046E RID: 1134
		public PredefinedName name;

		// Token: 0x0400046F RID: 1135
		public PREDEFMETH getter;
	}
}
