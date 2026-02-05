using System;

namespace Microsoft.CSharp.RuntimeBinder.Semantics
{
	// Token: 0x02000085 RID: 133
	internal sealed class ExprConstant : ExprWithType
	{
		// Token: 0x06000472 RID: 1138 RVA: 0x00018200 File Offset: 0x00016400
		public ExprConstant(CType type, ConstVal value)
			: base(ExpressionKind.Constant, type)
		{
			this.Val = value;
		}

		// Token: 0x170000B0 RID: 176
		// (get) Token: 0x06000473 RID: 1139 RVA: 0x00018212 File Offset: 0x00016412
		// (set) Token: 0x06000474 RID: 1140 RVA: 0x0001821A File Offset: 0x0001641A
		public Expr OptionalConstructorCall { get; set; }

		// Token: 0x170000B1 RID: 177
		// (get) Token: 0x06000475 RID: 1141 RVA: 0x00018224 File Offset: 0x00016424
		public bool IsZero
		{
			get
			{
				return this.Val.IsZero(base.Type.constValKind());
			}
		}

		// Token: 0x170000B2 RID: 178
		// (get) Token: 0x06000476 RID: 1142 RVA: 0x0001824A File Offset: 0x0001644A
		public ConstVal Val { get; }

		// Token: 0x170000B3 RID: 179
		// (get) Token: 0x06000477 RID: 1143 RVA: 0x00018254 File Offset: 0x00016454
		public ulong UInt64Value
		{
			get
			{
				return this.Val.UInt64Val;
			}
		}

		// Token: 0x170000B4 RID: 180
		// (get) Token: 0x06000478 RID: 1144 RVA: 0x00018270 File Offset: 0x00016470
		public long Int64Value
		{
			get
			{
				switch (base.Type.fundType())
				{
				case FUNDTYPE.FT_I1:
				case FUNDTYPE.FT_I2:
				case FUNDTYPE.FT_I4:
				case FUNDTYPE.FT_U1:
				case FUNDTYPE.FT_U2:
					return (long)this.Val.Int32Val;
				case FUNDTYPE.FT_U4:
					return (long)((ulong)this.Val.UInt32Val);
				case FUNDTYPE.FT_I8:
				case FUNDTYPE.FT_U8:
					return this.Val.Int64Val;
				default:
					return 0L;
				}
			}
		}
	}
}
