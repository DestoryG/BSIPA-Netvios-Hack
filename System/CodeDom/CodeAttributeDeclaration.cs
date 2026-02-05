using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace System.CodeDom
{
	// Token: 0x0200061D RID: 1565
	[ClassInterface(ClassInterfaceType.AutoDispatch)]
	[ComVisible(true)]
	[Serializable]
	public class CodeAttributeDeclaration
	{
		// Token: 0x0600392F RID: 14639 RVA: 0x000F25B8 File Offset: 0x000F07B8
		public CodeAttributeDeclaration()
		{
		}

		// Token: 0x06003930 RID: 14640 RVA: 0x000F25CB File Offset: 0x000F07CB
		public CodeAttributeDeclaration(string name)
		{
			this.Name = name;
		}

		// Token: 0x06003931 RID: 14641 RVA: 0x000F25E5 File Offset: 0x000F07E5
		public CodeAttributeDeclaration(string name, params CodeAttributeArgument[] arguments)
		{
			this.Name = name;
			this.Arguments.AddRange(arguments);
		}

		// Token: 0x06003932 RID: 14642 RVA: 0x000F260B File Offset: 0x000F080B
		public CodeAttributeDeclaration(CodeTypeReference attributeType)
			: this(attributeType, null)
		{
		}

		// Token: 0x06003933 RID: 14643 RVA: 0x000F2615 File Offset: 0x000F0815
		public CodeAttributeDeclaration(CodeTypeReference attributeType, params CodeAttributeArgument[] arguments)
		{
			this.attributeType = attributeType;
			if (attributeType != null)
			{
				this.name = attributeType.BaseType;
			}
			if (arguments != null)
			{
				this.Arguments.AddRange(arguments);
			}
		}

		// Token: 0x17000DAF RID: 3503
		// (get) Token: 0x06003934 RID: 14644 RVA: 0x000F264D File Offset: 0x000F084D
		// (set) Token: 0x06003935 RID: 14645 RVA: 0x000F2663 File Offset: 0x000F0863
		public string Name
		{
			get
			{
				if (this.name != null)
				{
					return this.name;
				}
				return string.Empty;
			}
			set
			{
				this.name = value;
				this.attributeType = new CodeTypeReference(this.name);
			}
		}

		// Token: 0x17000DB0 RID: 3504
		// (get) Token: 0x06003936 RID: 14646 RVA: 0x000F267D File Offset: 0x000F087D
		public CodeAttributeArgumentCollection Arguments
		{
			get
			{
				return this.arguments;
			}
		}

		// Token: 0x17000DB1 RID: 3505
		// (get) Token: 0x06003937 RID: 14647 RVA: 0x000F2685 File Offset: 0x000F0885
		public CodeTypeReference AttributeType
		{
			get
			{
				return this.attributeType;
			}
		}

		// Token: 0x04002B87 RID: 11143
		private string name;

		// Token: 0x04002B88 RID: 11144
		private CodeAttributeArgumentCollection arguments = new CodeAttributeArgumentCollection();

		// Token: 0x04002B89 RID: 11145
		[OptionalField]
		private CodeTypeReference attributeType;
	}
}
