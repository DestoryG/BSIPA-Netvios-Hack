using System;
using System.Runtime.InteropServices;

namespace System.CodeDom
{
	// Token: 0x0200062B RID: 1579
	[ClassInterface(ClassInterfaceType.AutoDispatch)]
	[ComVisible(true)]
	[Serializable]
	public class CodeConstructor : CodeMemberMethod
	{
		// Token: 0x0600399B RID: 14747 RVA: 0x000F2DA8 File Offset: 0x000F0FA8
		public CodeConstructor()
		{
			base.Name = ".ctor";
		}

		// Token: 0x17000DCB RID: 3531
		// (get) Token: 0x0600399C RID: 14748 RVA: 0x000F2DD1 File Offset: 0x000F0FD1
		public CodeExpressionCollection BaseConstructorArgs
		{
			get
			{
				return this.baseConstructorArgs;
			}
		}

		// Token: 0x17000DCC RID: 3532
		// (get) Token: 0x0600399D RID: 14749 RVA: 0x000F2DD9 File Offset: 0x000F0FD9
		public CodeExpressionCollection ChainedConstructorArgs
		{
			get
			{
				return this.chainedConstructorArgs;
			}
		}

		// Token: 0x04002BB2 RID: 11186
		private CodeExpressionCollection baseConstructorArgs = new CodeExpressionCollection();

		// Token: 0x04002BB3 RID: 11187
		private CodeExpressionCollection chainedConstructorArgs = new CodeExpressionCollection();
	}
}
