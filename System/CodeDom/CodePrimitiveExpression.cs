using System;
using System.Runtime.InteropServices;

namespace System.CodeDom
{
	// Token: 0x0200064C RID: 1612
	[ClassInterface(ClassInterfaceType.AutoDispatch)]
	[ComVisible(true)]
	[Serializable]
	public class CodePrimitiveExpression : CodeExpression
	{
		// Token: 0x06003A9B RID: 15003 RVA: 0x000F42E0 File Offset: 0x000F24E0
		public CodePrimitiveExpression()
		{
		}

		// Token: 0x06003A9C RID: 15004 RVA: 0x000F42E8 File Offset: 0x000F24E8
		public CodePrimitiveExpression(object value)
		{
			this.Value = value;
		}

		// Token: 0x17000E18 RID: 3608
		// (get) Token: 0x06003A9D RID: 15005 RVA: 0x000F42F7 File Offset: 0x000F24F7
		// (set) Token: 0x06003A9E RID: 15006 RVA: 0x000F42FF File Offset: 0x000F24FF
		public object Value
		{
			get
			{
				return this.value;
			}
			set
			{
				this.value = value;
			}
		}

		// Token: 0x04002C04 RID: 11268
		private object value;
	}
}
