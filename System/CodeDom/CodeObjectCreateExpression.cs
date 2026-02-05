using System;
using System.Runtime.InteropServices;

namespace System.CodeDom
{
	// Token: 0x02000649 RID: 1609
	[ClassInterface(ClassInterfaceType.AutoDispatch)]
	[ComVisible(true)]
	[Serializable]
	public class CodeObjectCreateExpression : CodeExpression
	{
		// Token: 0x06003A7B RID: 14971 RVA: 0x000F4041 File Offset: 0x000F2241
		public CodeObjectCreateExpression()
		{
		}

		// Token: 0x06003A7C RID: 14972 RVA: 0x000F4054 File Offset: 0x000F2254
		public CodeObjectCreateExpression(CodeTypeReference createType, params CodeExpression[] parameters)
		{
			this.CreateType = createType;
			this.Parameters.AddRange(parameters);
		}

		// Token: 0x06003A7D RID: 14973 RVA: 0x000F407A File Offset: 0x000F227A
		public CodeObjectCreateExpression(string createType, params CodeExpression[] parameters)
		{
			this.CreateType = new CodeTypeReference(createType);
			this.Parameters.AddRange(parameters);
		}

		// Token: 0x06003A7E RID: 14974 RVA: 0x000F40A5 File Offset: 0x000F22A5
		public CodeObjectCreateExpression(Type createType, params CodeExpression[] parameters)
		{
			this.CreateType = new CodeTypeReference(createType);
			this.Parameters.AddRange(parameters);
		}

		// Token: 0x17000E11 RID: 3601
		// (get) Token: 0x06003A7F RID: 14975 RVA: 0x000F40D0 File Offset: 0x000F22D0
		// (set) Token: 0x06003A80 RID: 14976 RVA: 0x000F40F0 File Offset: 0x000F22F0
		public CodeTypeReference CreateType
		{
			get
			{
				if (this.createType == null)
				{
					this.createType = new CodeTypeReference("");
				}
				return this.createType;
			}
			set
			{
				this.createType = value;
			}
		}

		// Token: 0x17000E12 RID: 3602
		// (get) Token: 0x06003A81 RID: 14977 RVA: 0x000F40F9 File Offset: 0x000F22F9
		public CodeExpressionCollection Parameters
		{
			get
			{
				return this.parameters;
			}
		}

		// Token: 0x04002BFE RID: 11262
		private CodeTypeReference createType;

		// Token: 0x04002BFF RID: 11263
		private CodeExpressionCollection parameters = new CodeExpressionCollection();
	}
}
