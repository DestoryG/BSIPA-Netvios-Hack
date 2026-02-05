using System;

namespace Microsoft.CSharp.RuntimeBinder.Semantics
{
	// Token: 0x02000086 RID: 134
	internal abstract class Expr
	{
		// Token: 0x06000479 RID: 1145 RVA: 0x000182E4 File Offset: 0x000164E4
		protected Expr(ExpressionKind kind)
		{
			this.Kind = kind;
		}

		// Token: 0x170000B5 RID: 181
		// (get) Token: 0x0600047A RID: 1146 RVA: 0x000182F3 File Offset: 0x000164F3
		// (set) Token: 0x0600047B RID: 1147 RVA: 0x000182FB File Offset: 0x000164FB
		internal object RuntimeObject { get; set; }

		// Token: 0x170000B6 RID: 182
		// (get) Token: 0x0600047C RID: 1148 RVA: 0x00018304 File Offset: 0x00016504
		// (set) Token: 0x0600047D RID: 1149 RVA: 0x0001830C File Offset: 0x0001650C
		internal CType RuntimeObjectActualType { get; set; }

		// Token: 0x170000B7 RID: 183
		// (get) Token: 0x0600047E RID: 1150 RVA: 0x00018315 File Offset: 0x00016515
		public ExpressionKind Kind { get; }

		// Token: 0x170000B8 RID: 184
		// (get) Token: 0x0600047F RID: 1151 RVA: 0x0001831D File Offset: 0x0001651D
		// (set) Token: 0x06000480 RID: 1152 RVA: 0x00018325 File Offset: 0x00016525
		public EXPRFLAG Flags { get; set; }

		// Token: 0x170000B9 RID: 185
		// (get) Token: 0x06000481 RID: 1153 RVA: 0x0001832E File Offset: 0x0001652E
		// (set) Token: 0x06000482 RID: 1154 RVA: 0x00018336 File Offset: 0x00016536
		public bool IsOptionalArgument { get; set; }

		// Token: 0x170000BA RID: 186
		// (get) Token: 0x06000483 RID: 1155 RVA: 0x0001833F File Offset: 0x0001653F
		// (set) Token: 0x06000484 RID: 1156 RVA: 0x00018347 File Offset: 0x00016547
		public string ErrorString { get; set; }

		// Token: 0x170000BB RID: 187
		// (get) Token: 0x06000485 RID: 1157 RVA: 0x00018350 File Offset: 0x00016550
		// (set) Token: 0x06000486 RID: 1158 RVA: 0x00018358 File Offset: 0x00016558
		public CType Type { get; protected set; }

		// Token: 0x170000BC RID: 188
		// (get) Token: 0x06000487 RID: 1159 RVA: 0x00018361 File Offset: 0x00016561
		public bool IsOK
		{
			get
			{
				return !this.HasError;
			}
		}

		// Token: 0x170000BD RID: 189
		// (get) Token: 0x06000488 RID: 1160 RVA: 0x0001836C File Offset: 0x0001656C
		// (set) Token: 0x06000489 RID: 1161 RVA: 0x00018374 File Offset: 0x00016574
		public bool HasError { get; private set; }

		// Token: 0x0600048A RID: 1162 RVA: 0x0001837D File Offset: 0x0001657D
		public void SetError()
		{
			this.HasError = true;
		}
	}
}
