using System;
using System.Runtime.InteropServices;

namespace System.CodeDom
{
	// Token: 0x02000659 RID: 1625
	[ClassInterface(ClassInterfaceType.AutoDispatch)]
	[ComVisible(true)]
	[Serializable]
	public class CodeThrowExceptionStatement : CodeStatement
	{
		// Token: 0x06003AD9 RID: 15065 RVA: 0x000F4690 File Offset: 0x000F2890
		public CodeThrowExceptionStatement()
		{
		}

		// Token: 0x06003ADA RID: 15066 RVA: 0x000F4698 File Offset: 0x000F2898
		public CodeThrowExceptionStatement(CodeExpression toThrow)
		{
			this.ToThrow = toThrow;
		}

		// Token: 0x17000E28 RID: 3624
		// (get) Token: 0x06003ADB RID: 15067 RVA: 0x000F46A7 File Offset: 0x000F28A7
		// (set) Token: 0x06003ADC RID: 15068 RVA: 0x000F46AF File Offset: 0x000F28AF
		public CodeExpression ToThrow
		{
			get
			{
				return this.toThrow;
			}
			set
			{
				this.toThrow = value;
			}
		}

		// Token: 0x04002C18 RID: 11288
		private CodeExpression toThrow;
	}
}
