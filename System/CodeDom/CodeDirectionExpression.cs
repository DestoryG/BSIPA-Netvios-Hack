using System;
using System.Runtime.InteropServices;

namespace System.CodeDom
{
	// Token: 0x0200062F RID: 1583
	[ClassInterface(ClassInterfaceType.AutoDispatch)]
	[ComVisible(true)]
	[Serializable]
	public class CodeDirectionExpression : CodeExpression
	{
		// Token: 0x060039B0 RID: 14768 RVA: 0x000F2F0B File Offset: 0x000F110B
		public CodeDirectionExpression()
		{
		}

		// Token: 0x060039B1 RID: 14769 RVA: 0x000F2F13 File Offset: 0x000F1113
		public CodeDirectionExpression(FieldDirection direction, CodeExpression expression)
		{
			this.expression = expression;
			this.direction = direction;
		}

		// Token: 0x17000DD3 RID: 3539
		// (get) Token: 0x060039B2 RID: 14770 RVA: 0x000F2F29 File Offset: 0x000F1129
		// (set) Token: 0x060039B3 RID: 14771 RVA: 0x000F2F31 File Offset: 0x000F1131
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

		// Token: 0x17000DD4 RID: 3540
		// (get) Token: 0x060039B4 RID: 14772 RVA: 0x000F2F3A File Offset: 0x000F113A
		// (set) Token: 0x060039B5 RID: 14773 RVA: 0x000F2F42 File Offset: 0x000F1142
		public FieldDirection Direction
		{
			get
			{
				return this.direction;
			}
			set
			{
				this.direction = value;
			}
		}

		// Token: 0x04002BBA RID: 11194
		private CodeExpression expression;

		// Token: 0x04002BBB RID: 11195
		private FieldDirection direction;
	}
}
