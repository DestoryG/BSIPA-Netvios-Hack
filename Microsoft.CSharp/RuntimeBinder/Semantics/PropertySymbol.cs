using System;
using System.Reflection;

namespace Microsoft.CSharp.RuntimeBinder.Semantics
{
	// Token: 0x0200006B RID: 107
	internal class PropertySymbol : MethodOrPropertySymbol
	{
		// Token: 0x17000085 RID: 133
		// (get) Token: 0x060003AB RID: 939 RVA: 0x00016A80 File Offset: 0x00014C80
		// (set) Token: 0x060003AC RID: 940 RVA: 0x00016A88 File Offset: 0x00014C88
		public MethodSymbol GetterMethod { get; set; }

		// Token: 0x17000086 RID: 134
		// (get) Token: 0x060003AD RID: 941 RVA: 0x00016A91 File Offset: 0x00014C91
		// (set) Token: 0x060003AE RID: 942 RVA: 0x00016A99 File Offset: 0x00014C99
		public MethodSymbol SetterMethod { get; set; }

		// Token: 0x17000087 RID: 135
		// (get) Token: 0x060003AF RID: 943 RVA: 0x00016AA2 File Offset: 0x00014CA2
		// (set) Token: 0x060003B0 RID: 944 RVA: 0x00016AAA File Offset: 0x00014CAA
		public PropertyInfo AssociatedPropertyInfo { get; set; }

		// Token: 0x17000088 RID: 136
		// (get) Token: 0x060003B1 RID: 945 RVA: 0x00016AB3 File Offset: 0x00014CB3
		// (set) Token: 0x060003B2 RID: 946 RVA: 0x00016ABB File Offset: 0x00014CBB
		public bool Bogus { get; set; }
	}
}
