using System;
using System.Runtime.InteropServices;

namespace System.CodeDom
{
	// Token: 0x02000640 RID: 1600
	[ClassInterface(ClassInterfaceType.AutoDispatch)]
	[ComVisible(true)]
	[Serializable]
	public class CodeMemberProperty : CodeTypeMember
	{
		// Token: 0x17000DF3 RID: 3571
		// (get) Token: 0x06003A20 RID: 14880 RVA: 0x000F37A3 File Offset: 0x000F19A3
		// (set) Token: 0x06003A21 RID: 14881 RVA: 0x000F37AB File Offset: 0x000F19AB
		public CodeTypeReference PrivateImplementationType
		{
			get
			{
				return this.privateImplements;
			}
			set
			{
				this.privateImplements = value;
			}
		}

		// Token: 0x17000DF4 RID: 3572
		// (get) Token: 0x06003A22 RID: 14882 RVA: 0x000F37B4 File Offset: 0x000F19B4
		public CodeTypeReferenceCollection ImplementationTypes
		{
			get
			{
				if (this.implementationTypes == null)
				{
					this.implementationTypes = new CodeTypeReferenceCollection();
				}
				return this.implementationTypes;
			}
		}

		// Token: 0x17000DF5 RID: 3573
		// (get) Token: 0x06003A23 RID: 14883 RVA: 0x000F37CF File Offset: 0x000F19CF
		// (set) Token: 0x06003A24 RID: 14884 RVA: 0x000F37EF File Offset: 0x000F19EF
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

		// Token: 0x17000DF6 RID: 3574
		// (get) Token: 0x06003A25 RID: 14885 RVA: 0x000F37F8 File Offset: 0x000F19F8
		// (set) Token: 0x06003A26 RID: 14886 RVA: 0x000F3812 File Offset: 0x000F1A12
		public bool HasGet
		{
			get
			{
				return this.hasGet || this.getStatements.Count > 0;
			}
			set
			{
				this.hasGet = value;
				if (!value)
				{
					this.getStatements.Clear();
				}
			}
		}

		// Token: 0x17000DF7 RID: 3575
		// (get) Token: 0x06003A27 RID: 14887 RVA: 0x000F3829 File Offset: 0x000F1A29
		// (set) Token: 0x06003A28 RID: 14888 RVA: 0x000F3843 File Offset: 0x000F1A43
		public bool HasSet
		{
			get
			{
				return this.hasSet || this.setStatements.Count > 0;
			}
			set
			{
				this.hasSet = value;
				if (!value)
				{
					this.setStatements.Clear();
				}
			}
		}

		// Token: 0x17000DF8 RID: 3576
		// (get) Token: 0x06003A29 RID: 14889 RVA: 0x000F385A File Offset: 0x000F1A5A
		public CodeStatementCollection GetStatements
		{
			get
			{
				return this.getStatements;
			}
		}

		// Token: 0x17000DF9 RID: 3577
		// (get) Token: 0x06003A2A RID: 14890 RVA: 0x000F3862 File Offset: 0x000F1A62
		public CodeStatementCollection SetStatements
		{
			get
			{
				return this.setStatements;
			}
		}

		// Token: 0x17000DFA RID: 3578
		// (get) Token: 0x06003A2B RID: 14891 RVA: 0x000F386A File Offset: 0x000F1A6A
		public CodeParameterDeclarationExpressionCollection Parameters
		{
			get
			{
				return this.parameters;
			}
		}

		// Token: 0x04002BDF RID: 11231
		private CodeTypeReference type;

		// Token: 0x04002BE0 RID: 11232
		private CodeParameterDeclarationExpressionCollection parameters = new CodeParameterDeclarationExpressionCollection();

		// Token: 0x04002BE1 RID: 11233
		private bool hasGet;

		// Token: 0x04002BE2 RID: 11234
		private bool hasSet;

		// Token: 0x04002BE3 RID: 11235
		private CodeStatementCollection getStatements = new CodeStatementCollection();

		// Token: 0x04002BE4 RID: 11236
		private CodeStatementCollection setStatements = new CodeStatementCollection();

		// Token: 0x04002BE5 RID: 11237
		private CodeTypeReference privateImplements;

		// Token: 0x04002BE6 RID: 11238
		private CodeTypeReferenceCollection implementationTypes;
	}
}
