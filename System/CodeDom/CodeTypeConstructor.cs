using System;
using System.Runtime.InteropServices;

namespace System.CodeDom
{
	// Token: 0x0200065B RID: 1627
	[ClassInterface(ClassInterfaceType.AutoDispatch)]
	[ComVisible(true)]
	[Serializable]
	public class CodeTypeConstructor : CodeMemberMethod
	{
		// Token: 0x06003AE3 RID: 15075 RVA: 0x000F47A0 File Offset: 0x000F29A0
		public CodeTypeConstructor()
		{
			base.Name = ".cctor";
		}
	}
}
