using System;
using System.Runtime.InteropServices;

namespace System.CodeDom
{
	// Token: 0x02000622 RID: 1570
	[ClassInterface(ClassInterfaceType.AutoDispatch)]
	[ComVisible(true)]
	[Serializable]
	public class CodeCastExpression : CodeExpression
	{
		// Token: 0x0600394E RID: 14670 RVA: 0x000F27FC File Offset: 0x000F09FC
		public CodeCastExpression()
		{
		}

		// Token: 0x0600394F RID: 14671 RVA: 0x000F2804 File Offset: 0x000F0A04
		public CodeCastExpression(CodeTypeReference targetType, CodeExpression expression)
		{
			this.TargetType = targetType;
			this.Expression = expression;
		}

		// Token: 0x06003950 RID: 14672 RVA: 0x000F281A File Offset: 0x000F0A1A
		public CodeCastExpression(string targetType, CodeExpression expression)
		{
			this.TargetType = new CodeTypeReference(targetType);
			this.Expression = expression;
		}

		// Token: 0x06003951 RID: 14673 RVA: 0x000F2835 File Offset: 0x000F0A35
		public CodeCastExpression(Type targetType, CodeExpression expression)
		{
			this.TargetType = new CodeTypeReference(targetType);
			this.Expression = expression;
		}

		// Token: 0x17000DB6 RID: 3510
		// (get) Token: 0x06003952 RID: 14674 RVA: 0x000F2850 File Offset: 0x000F0A50
		// (set) Token: 0x06003953 RID: 14675 RVA: 0x000F2870 File Offset: 0x000F0A70
		public CodeTypeReference TargetType
		{
			get
			{
				if (this.targetType == null)
				{
					this.targetType = new CodeTypeReference("");
				}
				return this.targetType;
			}
			set
			{
				this.targetType = value;
			}
		}

		// Token: 0x17000DB7 RID: 3511
		// (get) Token: 0x06003954 RID: 14676 RVA: 0x000F2879 File Offset: 0x000F0A79
		// (set) Token: 0x06003955 RID: 14677 RVA: 0x000F2881 File Offset: 0x000F0A81
		public CodeExpression Expression
		{
			get
			{
				return this.expression;
			}
			set
			{
				this.expression = value;
			}
		}

		// Token: 0x04002B9F RID: 11167
		private CodeTypeReference targetType;

		// Token: 0x04002BA0 RID: 11168
		private CodeExpression expression;
	}
}
