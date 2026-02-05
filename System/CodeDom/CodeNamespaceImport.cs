using System;
using System.Runtime.InteropServices;

namespace System.CodeDom
{
	// Token: 0x02000646 RID: 1606
	[ClassInterface(ClassInterfaceType.AutoDispatch)]
	[ComVisible(true)]
	[Serializable]
	public class CodeNamespaceImport : CodeObject
	{
		// Token: 0x06003A5A RID: 14938 RVA: 0x000F3DB0 File Offset: 0x000F1FB0
		public CodeNamespaceImport()
		{
		}

		// Token: 0x06003A5B RID: 14939 RVA: 0x000F3DB8 File Offset: 0x000F1FB8
		public CodeNamespaceImport(string nameSpace)
		{
			this.Namespace = nameSpace;
		}

		// Token: 0x17000E06 RID: 3590
		// (get) Token: 0x06003A5C RID: 14940 RVA: 0x000F3DC7 File Offset: 0x000F1FC7
		// (set) Token: 0x06003A5D RID: 14941 RVA: 0x000F3DCF File Offset: 0x000F1FCF
		public CodeLinePragma LinePragma
		{
			get
			{
				return this.linePragma;
			}
			set
			{
				this.linePragma = value;
			}
		}

		// Token: 0x17000E07 RID: 3591
		// (get) Token: 0x06003A5E RID: 14942 RVA: 0x000F3DD8 File Offset: 0x000F1FD8
		// (set) Token: 0x06003A5F RID: 14943 RVA: 0x000F3DEE File Offset: 0x000F1FEE
		public string Namespace
		{
			get
			{
				if (this.nameSpace != null)
				{
					return this.nameSpace;
				}
				return string.Empty;
			}
			set
			{
				this.nameSpace = value;
			}
		}

		// Token: 0x04002BF9 RID: 11257
		private string nameSpace;

		// Token: 0x04002BFA RID: 11258
		private CodeLinePragma linePragma;
	}
}
