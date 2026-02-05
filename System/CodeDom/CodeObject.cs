using System;
using System.Collections;
using System.Collections.Specialized;
using System.Runtime.InteropServices;

namespace System.CodeDom
{
	// Token: 0x02000648 RID: 1608
	[ClassInterface(ClassInterfaceType.AutoDispatch)]
	[ComVisible(true)]
	[Serializable]
	public class CodeObject
	{
		// Token: 0x17000E10 RID: 3600
		// (get) Token: 0x06003A7A RID: 14970 RVA: 0x000F4026 File Offset: 0x000F2226
		public IDictionary UserData
		{
			get
			{
				if (this.userData == null)
				{
					this.userData = new ListDictionary();
				}
				return this.userData;
			}
		}

		// Token: 0x04002BFD RID: 11261
		private IDictionary userData;
	}
}
