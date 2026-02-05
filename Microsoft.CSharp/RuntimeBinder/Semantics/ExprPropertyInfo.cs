using System;

namespace Microsoft.CSharp.RuntimeBinder.Semantics
{
	// Token: 0x02000095 RID: 149
	internal sealed class ExprPropertyInfo : ExprWithType
	{
		// Token: 0x060004C9 RID: 1225 RVA: 0x00018779 File Offset: 0x00016979
		public ExprPropertyInfo(CType type, PropertySymbol propertySymbol, AggregateType propertyType)
			: base(ExpressionKind.PropertyInfo, type)
		{
			this.Property = new PropWithType(propertySymbol, propertyType);
		}

		// Token: 0x170000DB RID: 219
		// (get) Token: 0x060004CA RID: 1226 RVA: 0x00018791 File Offset: 0x00016991
		public PropWithType Property { get; }
	}
}
