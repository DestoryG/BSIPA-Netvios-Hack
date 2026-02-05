using System;
using System.Globalization;
using System.Security.Permissions;

namespace System.CodeDom.Compiler
{
	// Token: 0x02000675 RID: 1653
	[PermissionSet(SecurityAction.InheritanceDemand, Name = "FullTrust")]
	[Serializable]
	public class CompilerError
	{
		// Token: 0x06003CBF RID: 15551 RVA: 0x000FAAFB File Offset: 0x000F8CFB
		public CompilerError()
		{
			this.line = 0;
			this.column = 0;
			this.errorNumber = string.Empty;
			this.errorText = string.Empty;
			this.fileName = string.Empty;
		}

		// Token: 0x06003CC0 RID: 15552 RVA: 0x000FAB32 File Offset: 0x000F8D32
		public CompilerError(string fileName, int line, int column, string errorNumber, string errorText)
		{
			this.line = line;
			this.column = column;
			this.errorNumber = errorNumber;
			this.errorText = errorText;
			this.fileName = fileName;
		}

		// Token: 0x17000E6F RID: 3695
		// (get) Token: 0x06003CC1 RID: 15553 RVA: 0x000FAB5F File Offset: 0x000F8D5F
		// (set) Token: 0x06003CC2 RID: 15554 RVA: 0x000FAB67 File Offset: 0x000F8D67
		public int Line
		{
			get
			{
				return this.line;
			}
			set
			{
				this.line = value;
			}
		}

		// Token: 0x17000E70 RID: 3696
		// (get) Token: 0x06003CC3 RID: 15555 RVA: 0x000FAB70 File Offset: 0x000F8D70
		// (set) Token: 0x06003CC4 RID: 15556 RVA: 0x000FAB78 File Offset: 0x000F8D78
		public int Column
		{
			get
			{
				return this.column;
			}
			set
			{
				this.column = value;
			}
		}

		// Token: 0x17000E71 RID: 3697
		// (get) Token: 0x06003CC5 RID: 15557 RVA: 0x000FAB81 File Offset: 0x000F8D81
		// (set) Token: 0x06003CC6 RID: 15558 RVA: 0x000FAB89 File Offset: 0x000F8D89
		public string ErrorNumber
		{
			get
			{
				return this.errorNumber;
			}
			set
			{
				this.errorNumber = value;
			}
		}

		// Token: 0x17000E72 RID: 3698
		// (get) Token: 0x06003CC7 RID: 15559 RVA: 0x000FAB92 File Offset: 0x000F8D92
		// (set) Token: 0x06003CC8 RID: 15560 RVA: 0x000FAB9A File Offset: 0x000F8D9A
		public string ErrorText
		{
			get
			{
				return this.errorText;
			}
			set
			{
				this.errorText = value;
			}
		}

		// Token: 0x17000E73 RID: 3699
		// (get) Token: 0x06003CC9 RID: 15561 RVA: 0x000FABA3 File Offset: 0x000F8DA3
		// (set) Token: 0x06003CCA RID: 15562 RVA: 0x000FABAB File Offset: 0x000F8DAB
		public bool IsWarning
		{
			get
			{
				return this.warning;
			}
			set
			{
				this.warning = value;
			}
		}

		// Token: 0x17000E74 RID: 3700
		// (get) Token: 0x06003CCB RID: 15563 RVA: 0x000FABB4 File Offset: 0x000F8DB4
		// (set) Token: 0x06003CCC RID: 15564 RVA: 0x000FABBC File Offset: 0x000F8DBC
		public string FileName
		{
			get
			{
				return this.fileName;
			}
			set
			{
				this.fileName = value;
			}
		}

		// Token: 0x06003CCD RID: 15565 RVA: 0x000FABC8 File Offset: 0x000F8DC8
		public override string ToString()
		{
			if (this.FileName.Length > 0)
			{
				return string.Format(CultureInfo.InvariantCulture, "{0}({1},{2}) : {3} {4}: {5}", new object[]
				{
					this.FileName,
					this.Line,
					this.Column,
					this.IsWarning ? "warning" : "error",
					this.ErrorNumber,
					this.ErrorText
				});
			}
			return string.Format(CultureInfo.InvariantCulture, "{0} {1}: {2}", new object[]
			{
				this.IsWarning ? "warning" : "error",
				this.ErrorNumber,
				this.ErrorText
			});
		}

		// Token: 0x04002C69 RID: 11369
		private int line;

		// Token: 0x04002C6A RID: 11370
		private int column;

		// Token: 0x04002C6B RID: 11371
		private string errorNumber;

		// Token: 0x04002C6C RID: 11372
		private bool warning;

		// Token: 0x04002C6D RID: 11373
		private string errorText;

		// Token: 0x04002C6E RID: 11374
		private string fileName;
	}
}
