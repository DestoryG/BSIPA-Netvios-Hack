using System;
using System.Collections.Specialized;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace System.CodeDom
{
	// Token: 0x02000629 RID: 1577
	[ClassInterface(ClassInterfaceType.AutoDispatch)]
	[ComVisible(true)]
	[Serializable]
	public class CodeCompileUnit : CodeObject
	{
		// Token: 0x17000DC3 RID: 3523
		// (get) Token: 0x0600398F RID: 14735 RVA: 0x000F2C87 File Offset: 0x000F0E87
		public CodeNamespaceCollection Namespaces
		{
			get
			{
				return this.namespaces;
			}
		}

		// Token: 0x17000DC4 RID: 3524
		// (get) Token: 0x06003990 RID: 14736 RVA: 0x000F2C8F File Offset: 0x000F0E8F
		public StringCollection ReferencedAssemblies
		{
			get
			{
				if (this.assemblies == null)
				{
					this.assemblies = new StringCollection();
				}
				return this.assemblies;
			}
		}

		// Token: 0x17000DC5 RID: 3525
		// (get) Token: 0x06003991 RID: 14737 RVA: 0x000F2CAA File Offset: 0x000F0EAA
		public CodeAttributeDeclarationCollection AssemblyCustomAttributes
		{
			get
			{
				if (this.attributes == null)
				{
					this.attributes = new CodeAttributeDeclarationCollection();
				}
				return this.attributes;
			}
		}

		// Token: 0x17000DC6 RID: 3526
		// (get) Token: 0x06003992 RID: 14738 RVA: 0x000F2CC5 File Offset: 0x000F0EC5
		public CodeDirectiveCollection StartDirectives
		{
			get
			{
				if (this.startDirectives == null)
				{
					this.startDirectives = new CodeDirectiveCollection();
				}
				return this.startDirectives;
			}
		}

		// Token: 0x17000DC7 RID: 3527
		// (get) Token: 0x06003993 RID: 14739 RVA: 0x000F2CE0 File Offset: 0x000F0EE0
		public CodeDirectiveCollection EndDirectives
		{
			get
			{
				if (this.endDirectives == null)
				{
					this.endDirectives = new CodeDirectiveCollection();
				}
				return this.endDirectives;
			}
		}

		// Token: 0x04002BAA RID: 11178
		private CodeNamespaceCollection namespaces = new CodeNamespaceCollection();

		// Token: 0x04002BAB RID: 11179
		private StringCollection assemblies;

		// Token: 0x04002BAC RID: 11180
		private CodeAttributeDeclarationCollection attributes;

		// Token: 0x04002BAD RID: 11181
		[OptionalField]
		private CodeDirectiveCollection startDirectives;

		// Token: 0x04002BAE RID: 11182
		[OptionalField]
		private CodeDirectiveCollection endDirectives;
	}
}
