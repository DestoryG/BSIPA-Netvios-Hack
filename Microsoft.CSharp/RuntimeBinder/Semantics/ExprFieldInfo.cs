using System;

namespace Microsoft.CSharp.RuntimeBinder.Semantics
{
	// Token: 0x0200008B RID: 139
	internal sealed class ExprFieldInfo : ExprWithType
	{
		// Token: 0x0600049D RID: 1181 RVA: 0x00018507 File Offset: 0x00016707
		public ExprFieldInfo(FieldSymbol field, AggregateType fieldType, CType type)
			: base(ExpressionKind.FieldInfo, type)
		{
			this.Field = field;
			this.FieldType = fieldType;
		}

		// Token: 0x170000C3 RID: 195
		// (get) Token: 0x0600049E RID: 1182 RVA: 0x00018520 File Offset: 0x00016720
		public FieldSymbol Field { get; }

		// Token: 0x170000C4 RID: 196
		// (get) Token: 0x0600049F RID: 1183 RVA: 0x00018528 File Offset: 0x00016728
		public AggregateType FieldType { get; }
	}
}
