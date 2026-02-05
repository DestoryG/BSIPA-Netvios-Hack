using System;
using System.Runtime.InteropServices;

namespace System.CodeDom
{
	// Token: 0x0200063C RID: 1596
	[ClassInterface(ClassInterfaceType.AutoDispatch)]
	[ComVisible(true)]
	[Serializable]
	public class CodeLinePragma
	{
		// Token: 0x060039FC RID: 14844 RVA: 0x000F33BB File Offset: 0x000F15BB
		public CodeLinePragma()
		{
		}

		// Token: 0x060039FD RID: 14845 RVA: 0x000F33C3 File Offset: 0x000F15C3
		public CodeLinePragma(string fileName, int lineNumber)
		{
			this.FileName = fileName;
			this.LineNumber = lineNumber;
		}

		// Token: 0x17000DE5 RID: 3557
		// (get) Token: 0x060039FE RID: 14846 RVA: 0x000F33D9 File Offset: 0x000F15D9
		// (set) Token: 0x060039FF RID: 14847 RVA: 0x000F33EF File Offset: 0x000F15EF
		public string FileName
		{
			get
			{
				if (this.fileName != null)
				{
					return this.fileName;
				}
				return string.Empty;
			}
			set
			{
				this.fileName = value;
			}
		}

		// Token: 0x17000DE6 RID: 3558
		// (get) Token: 0x06003A00 RID: 14848 RVA: 0x000F33F8 File Offset: 0x000F15F8
		// (set) Token: 0x06003A01 RID: 14849 RVA: 0x000F3400 File Offset: 0x000F1600
		public int LineNumber
		{
			get
			{
				return this.lineNumber;
			}
			set
			{
				this.lineNumber = value;
			}
		}

		// Token: 0x04002BCA RID: 11210
		private string fileName;

		// Token: 0x04002BCB RID: 11211
		private int lineNumber;
	}
}
