using System;
using System.Runtime.InteropServices;

namespace System.CodeDom
{
	// Token: 0x02000623 RID: 1571
	[ClassInterface(ClassInterfaceType.AutoDispatch)]
	[ComVisible(true)]
	[Serializable]
	public class CodeCatchClause
	{
		// Token: 0x06003956 RID: 14678 RVA: 0x000F288A File Offset: 0x000F0A8A
		public CodeCatchClause()
		{
		}

		// Token: 0x06003957 RID: 14679 RVA: 0x000F2892 File Offset: 0x000F0A92
		public CodeCatchClause(string localName)
		{
			this.localName = localName;
		}

		// Token: 0x06003958 RID: 14680 RVA: 0x000F28A1 File Offset: 0x000F0AA1
		public CodeCatchClause(string localName, CodeTypeReference catchExceptionType)
		{
			this.localName = localName;
			this.catchExceptionType = catchExceptionType;
		}

		// Token: 0x06003959 RID: 14681 RVA: 0x000F28B7 File Offset: 0x000F0AB7
		public CodeCatchClause(string localName, CodeTypeReference catchExceptionType, params CodeStatement[] statements)
		{
			this.localName = localName;
			this.catchExceptionType = catchExceptionType;
			this.Statements.AddRange(statements);
		}

		// Token: 0x17000DB8 RID: 3512
		// (get) Token: 0x0600395A RID: 14682 RVA: 0x000F28D9 File Offset: 0x000F0AD9
		// (set) Token: 0x0600395B RID: 14683 RVA: 0x000F28EF File Offset: 0x000F0AEF
		public string LocalName
		{
			get
			{
				if (this.localName != null)
				{
					return this.localName;
				}
				return string.Empty;
			}
			set
			{
				this.localName = value;
			}
		}

		// Token: 0x17000DB9 RID: 3513
		// (get) Token: 0x0600395C RID: 14684 RVA: 0x000F28F8 File Offset: 0x000F0AF8
		// (set) Token: 0x0600395D RID: 14685 RVA: 0x000F291D File Offset: 0x000F0B1D
		public CodeTypeReference CatchExceptionType
		{
			get
			{
				if (this.catchExceptionType == null)
				{
					this.catchExceptionType = new CodeTypeReference(typeof(Exception));
				}
				return this.catchExceptionType;
			}
			set
			{
				this.catchExceptionType = value;
			}
		}

		// Token: 0x17000DBA RID: 3514
		// (get) Token: 0x0600395E RID: 14686 RVA: 0x000F2926 File Offset: 0x000F0B26
		public CodeStatementCollection Statements
		{
			get
			{
				if (this.statements == null)
				{
					this.statements = new CodeStatementCollection();
				}
				return this.statements;
			}
		}

		// Token: 0x04002BA1 RID: 11169
		private CodeStatementCollection statements;

		// Token: 0x04002BA2 RID: 11170
		private CodeTypeReference catchExceptionType;

		// Token: 0x04002BA3 RID: 11171
		private string localName;
	}
}
