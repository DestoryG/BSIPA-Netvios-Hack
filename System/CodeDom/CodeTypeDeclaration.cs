using System;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace System.CodeDom
{
	// Token: 0x0200065C RID: 1628
	[ClassInterface(ClassInterfaceType.AutoDispatch)]
	[ComVisible(true)]
	[Serializable]
	public class CodeTypeDeclaration : CodeTypeMember
	{
		// Token: 0x14000070 RID: 112
		// (add) Token: 0x06003AE4 RID: 15076 RVA: 0x000F47B4 File Offset: 0x000F29B4
		// (remove) Token: 0x06003AE5 RID: 15077 RVA: 0x000F47EC File Offset: 0x000F29EC
		public event EventHandler PopulateBaseTypes;

		// Token: 0x14000071 RID: 113
		// (add) Token: 0x06003AE6 RID: 15078 RVA: 0x000F4824 File Offset: 0x000F2A24
		// (remove) Token: 0x06003AE7 RID: 15079 RVA: 0x000F485C File Offset: 0x000F2A5C
		public event EventHandler PopulateMembers;

		// Token: 0x06003AE8 RID: 15080 RVA: 0x000F4891 File Offset: 0x000F2A91
		public CodeTypeDeclaration()
		{
		}

		// Token: 0x06003AE9 RID: 15081 RVA: 0x000F48B6 File Offset: 0x000F2AB6
		public CodeTypeDeclaration(string name)
		{
			base.Name = name;
		}

		// Token: 0x17000E2C RID: 3628
		// (get) Token: 0x06003AEA RID: 15082 RVA: 0x000F48E2 File Offset: 0x000F2AE2
		// (set) Token: 0x06003AEB RID: 15083 RVA: 0x000F48EA File Offset: 0x000F2AEA
		public TypeAttributes TypeAttributes
		{
			get
			{
				return this.attributes;
			}
			set
			{
				this.attributes = value;
			}
		}

		// Token: 0x17000E2D RID: 3629
		// (get) Token: 0x06003AEC RID: 15084 RVA: 0x000F48F3 File Offset: 0x000F2AF3
		public CodeTypeReferenceCollection BaseTypes
		{
			get
			{
				if ((this.populated & 1) == 0)
				{
					this.populated |= 1;
					if (this.PopulateBaseTypes != null)
					{
						this.PopulateBaseTypes(this, EventArgs.Empty);
					}
				}
				return this.baseTypes;
			}
		}

		// Token: 0x17000E2E RID: 3630
		// (get) Token: 0x06003AED RID: 15085 RVA: 0x000F492C File Offset: 0x000F2B2C
		// (set) Token: 0x06003AEE RID: 15086 RVA: 0x000F494C File Offset: 0x000F2B4C
		public bool IsClass
		{
			get
			{
				return (this.attributes & TypeAttributes.ClassSemanticsMask) == TypeAttributes.NotPublic && !this.isEnum && !this.isStruct;
			}
			set
			{
				if (value)
				{
					this.attributes &= ~TypeAttributes.ClassSemanticsMask;
					this.attributes |= TypeAttributes.NotPublic;
					this.isStruct = false;
					this.isEnum = false;
				}
			}
		}

		// Token: 0x17000E2F RID: 3631
		// (get) Token: 0x06003AEF RID: 15087 RVA: 0x000F497C File Offset: 0x000F2B7C
		// (set) Token: 0x06003AF0 RID: 15088 RVA: 0x000F4984 File Offset: 0x000F2B84
		public bool IsStruct
		{
			get
			{
				return this.isStruct;
			}
			set
			{
				if (value)
				{
					this.attributes &= ~TypeAttributes.ClassSemanticsMask;
					this.isStruct = true;
					this.isEnum = false;
					return;
				}
				this.isStruct = false;
			}
		}

		// Token: 0x17000E30 RID: 3632
		// (get) Token: 0x06003AF1 RID: 15089 RVA: 0x000F49AE File Offset: 0x000F2BAE
		// (set) Token: 0x06003AF2 RID: 15090 RVA: 0x000F49B6 File Offset: 0x000F2BB6
		public bool IsEnum
		{
			get
			{
				return this.isEnum;
			}
			set
			{
				if (value)
				{
					this.attributes &= ~TypeAttributes.ClassSemanticsMask;
					this.isStruct = false;
					this.isEnum = true;
					return;
				}
				this.isEnum = false;
			}
		}

		// Token: 0x17000E31 RID: 3633
		// (get) Token: 0x06003AF3 RID: 15091 RVA: 0x000F49E0 File Offset: 0x000F2BE0
		// (set) Token: 0x06003AF4 RID: 15092 RVA: 0x000F49F0 File Offset: 0x000F2BF0
		public bool IsInterface
		{
			get
			{
				return (this.attributes & TypeAttributes.ClassSemanticsMask) == TypeAttributes.ClassSemanticsMask;
			}
			set
			{
				if (value)
				{
					this.attributes &= ~TypeAttributes.ClassSemanticsMask;
					this.attributes |= TypeAttributes.ClassSemanticsMask;
					this.isStruct = false;
					this.isEnum = false;
					return;
				}
				this.attributes &= ~TypeAttributes.ClassSemanticsMask;
			}
		}

		// Token: 0x17000E32 RID: 3634
		// (get) Token: 0x06003AF5 RID: 15093 RVA: 0x000F4A3C File Offset: 0x000F2C3C
		// (set) Token: 0x06003AF6 RID: 15094 RVA: 0x000F4A44 File Offset: 0x000F2C44
		public bool IsPartial
		{
			get
			{
				return this.isPartial;
			}
			set
			{
				this.isPartial = value;
			}
		}

		// Token: 0x17000E33 RID: 3635
		// (get) Token: 0x06003AF7 RID: 15095 RVA: 0x000F4A4D File Offset: 0x000F2C4D
		public CodeTypeMemberCollection Members
		{
			get
			{
				if ((this.populated & 2) == 0)
				{
					this.populated |= 2;
					if (this.PopulateMembers != null)
					{
						this.PopulateMembers(this, EventArgs.Empty);
					}
				}
				return this.members;
			}
		}

		// Token: 0x17000E34 RID: 3636
		// (get) Token: 0x06003AF8 RID: 15096 RVA: 0x000F4A86 File Offset: 0x000F2C86
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

		// Token: 0x04002C1C RID: 11292
		private TypeAttributes attributes = TypeAttributes.Public;

		// Token: 0x04002C1D RID: 11293
		private CodeTypeReferenceCollection baseTypes = new CodeTypeReferenceCollection();

		// Token: 0x04002C1E RID: 11294
		private CodeTypeMemberCollection members = new CodeTypeMemberCollection();

		// Token: 0x04002C1F RID: 11295
		private bool isEnum;

		// Token: 0x04002C20 RID: 11296
		private bool isStruct;

		// Token: 0x04002C21 RID: 11297
		private int populated;

		// Token: 0x04002C22 RID: 11298
		private const int BaseTypesCollection = 1;

		// Token: 0x04002C23 RID: 11299
		private const int MembersCollection = 2;

		// Token: 0x04002C24 RID: 11300
		[OptionalField]
		private CodeTypeParameterCollection typeParameters;

		// Token: 0x04002C25 RID: 11301
		[OptionalField]
		private bool isPartial;
	}
}
