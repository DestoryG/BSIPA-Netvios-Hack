using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace System.CodeDom
{
	// Token: 0x0200063F RID: 1599
	[ClassInterface(ClassInterfaceType.AutoDispatch)]
	[ComVisible(true)]
	[Serializable]
	public class CodeMemberMethod : CodeTypeMember
	{
		// Token: 0x1400006A RID: 106
		// (add) Token: 0x06003A10 RID: 14864 RVA: 0x000F34F4 File Offset: 0x000F16F4
		// (remove) Token: 0x06003A11 RID: 14865 RVA: 0x000F352C File Offset: 0x000F172C
		public event EventHandler PopulateParameters;

		// Token: 0x1400006B RID: 107
		// (add) Token: 0x06003A12 RID: 14866 RVA: 0x000F3564 File Offset: 0x000F1764
		// (remove) Token: 0x06003A13 RID: 14867 RVA: 0x000F359C File Offset: 0x000F179C
		public event EventHandler PopulateStatements;

		// Token: 0x1400006C RID: 108
		// (add) Token: 0x06003A14 RID: 14868 RVA: 0x000F35D4 File Offset: 0x000F17D4
		// (remove) Token: 0x06003A15 RID: 14869 RVA: 0x000F360C File Offset: 0x000F180C
		public event EventHandler PopulateImplementationTypes;

		// Token: 0x17000DEC RID: 3564
		// (get) Token: 0x06003A16 RID: 14870 RVA: 0x000F3641 File Offset: 0x000F1841
		// (set) Token: 0x06003A17 RID: 14871 RVA: 0x000F366B File Offset: 0x000F186B
		public CodeTypeReference ReturnType
		{
			get
			{
				if (this.returnType == null)
				{
					this.returnType = new CodeTypeReference(typeof(void).FullName);
				}
				return this.returnType;
			}
			set
			{
				this.returnType = value;
			}
		}

		// Token: 0x17000DED RID: 3565
		// (get) Token: 0x06003A18 RID: 14872 RVA: 0x000F3674 File Offset: 0x000F1874
		public CodeStatementCollection Statements
		{
			get
			{
				if ((this.populated & 2) == 0)
				{
					this.populated |= 2;
					if (this.PopulateStatements != null)
					{
						this.PopulateStatements(this, EventArgs.Empty);
					}
				}
				return this.statements;
			}
		}

		// Token: 0x17000DEE RID: 3566
		// (get) Token: 0x06003A19 RID: 14873 RVA: 0x000F36AD File Offset: 0x000F18AD
		public CodeParameterDeclarationExpressionCollection Parameters
		{
			get
			{
				if ((this.populated & 1) == 0)
				{
					this.populated |= 1;
					if (this.PopulateParameters != null)
					{
						this.PopulateParameters(this, EventArgs.Empty);
					}
				}
				return this.parameters;
			}
		}

		// Token: 0x17000DEF RID: 3567
		// (get) Token: 0x06003A1A RID: 14874 RVA: 0x000F36E6 File Offset: 0x000F18E6
		// (set) Token: 0x06003A1B RID: 14875 RVA: 0x000F36EE File Offset: 0x000F18EE
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

		// Token: 0x17000DF0 RID: 3568
		// (get) Token: 0x06003A1C RID: 14876 RVA: 0x000F36F8 File Offset: 0x000F18F8
		public CodeTypeReferenceCollection ImplementationTypes
		{
			get
			{
				if (this.implementationTypes == null)
				{
					this.implementationTypes = new CodeTypeReferenceCollection();
				}
				if ((this.populated & 4) == 0)
				{
					this.populated |= 4;
					if (this.PopulateImplementationTypes != null)
					{
						this.PopulateImplementationTypes(this, EventArgs.Empty);
					}
				}
				return this.implementationTypes;
			}
		}

		// Token: 0x17000DF1 RID: 3569
		// (get) Token: 0x06003A1D RID: 14877 RVA: 0x000F374F File Offset: 0x000F194F
		public CodeAttributeDeclarationCollection ReturnTypeCustomAttributes
		{
			get
			{
				if (this.returnAttributes == null)
				{
					this.returnAttributes = new CodeAttributeDeclarationCollection();
				}
				return this.returnAttributes;
			}
		}

		// Token: 0x17000DF2 RID: 3570
		// (get) Token: 0x06003A1E RID: 14878 RVA: 0x000F376A File Offset: 0x000F196A
		[ComVisible(false)]
		public CodeTypeParameterCollection TypeParameters
		{
			get
			{
				if (this.typeParameters == null)
				{
					this.typeParameters = new CodeTypeParameterCollection();
				}
				return this.typeParameters;
			}
		}

		// Token: 0x04002BD1 RID: 11217
		private CodeParameterDeclarationExpressionCollection parameters = new CodeParameterDeclarationExpressionCollection();

		// Token: 0x04002BD2 RID: 11218
		private CodeStatementCollection statements = new CodeStatementCollection();

		// Token: 0x04002BD3 RID: 11219
		private CodeTypeReference returnType;

		// Token: 0x04002BD4 RID: 11220
		private CodeTypeReference privateImplements;

		// Token: 0x04002BD5 RID: 11221
		private CodeTypeReferenceCollection implementationTypes;

		// Token: 0x04002BD6 RID: 11222
		private CodeAttributeDeclarationCollection returnAttributes;

		// Token: 0x04002BD7 RID: 11223
		[OptionalField]
		private CodeTypeParameterCollection typeParameters;

		// Token: 0x04002BD8 RID: 11224
		private int populated;

		// Token: 0x04002BD9 RID: 11225
		private const int ParametersCollection = 1;

		// Token: 0x04002BDA RID: 11226
		private const int StatementsCollection = 2;

		// Token: 0x04002BDB RID: 11227
		private const int ImplTypesCollection = 4;
	}
}
