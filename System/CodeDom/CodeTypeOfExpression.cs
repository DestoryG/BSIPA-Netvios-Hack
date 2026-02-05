using System;
using System.Runtime.InteropServices;

namespace System.CodeDom
{
	// Token: 0x02000661 RID: 1633
	[ClassInterface(ClassInterfaceType.AutoDispatch)]
	[ComVisible(true)]
	[Serializable]
	public class CodeTypeOfExpression : CodeExpression
	{
		// Token: 0x06003B24 RID: 15140 RVA: 0x000F4E1C File Offset: 0x000F301C
		public CodeTypeOfExpression()
		{
		}

		// Token: 0x06003B25 RID: 15141 RVA: 0x000F4E24 File Offset: 0x000F3024
		public CodeTypeOfExpression(CodeTypeReference type)
		{
			this.Type = type;
		}

		// Token: 0x06003B26 RID: 15142 RVA: 0x000F4E33 File Offset: 0x000F3033
		public CodeTypeOfExpression(string type)
		{
			this.Type = new CodeTypeReference(type);
		}

		// Token: 0x06003B27 RID: 15143 RVA: 0x000F4E47 File Offset: 0x000F3047
		public CodeTypeOfExpression(Type type)
		{
			this.Type = new CodeTypeReference(type);
		}

		// Token: 0x17000E40 RID: 3648
		// (get) Token: 0x06003B28 RID: 15144 RVA: 0x000F4E5B File Offset: 0x000F305B
		// (set) Token: 0x06003B29 RID: 15145 RVA: 0x000F4E7B File Offset: 0x000F307B
		public CodeTypeReference Type
		{
			get
			{
				if (this.type == null)
				{
					this.type = new CodeTypeReference("");
				}
				return this.type;
			}
			set
			{
				this.type = value;
			}
		}

		// Token: 0x04002C31 RID: 11313
		private CodeTypeReference type;
	}
}
