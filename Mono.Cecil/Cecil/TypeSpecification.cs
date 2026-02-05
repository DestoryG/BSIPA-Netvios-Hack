using System;

namespace Mono.Cecil
{
	// Token: 0x020000C0 RID: 192
	public abstract class TypeSpecification : TypeReference
	{
		// Token: 0x17000288 RID: 648
		// (get) Token: 0x06000841 RID: 2113 RVA: 0x000190FB File Offset: 0x000172FB
		public TypeReference ElementType
		{
			get
			{
				return this.element_type;
			}
		}

		// Token: 0x17000289 RID: 649
		// (get) Token: 0x06000842 RID: 2114 RVA: 0x00019103 File Offset: 0x00017303
		// (set) Token: 0x06000843 RID: 2115 RVA: 0x00002C55 File Offset: 0x00000E55
		public override string Name
		{
			get
			{
				return this.element_type.Name;
			}
			set
			{
				throw new InvalidOperationException();
			}
		}

		// Token: 0x1700028A RID: 650
		// (get) Token: 0x06000844 RID: 2116 RVA: 0x00019110 File Offset: 0x00017310
		// (set) Token: 0x06000845 RID: 2117 RVA: 0x00002C55 File Offset: 0x00000E55
		public override string Namespace
		{
			get
			{
				return this.element_type.Namespace;
			}
			set
			{
				throw new InvalidOperationException();
			}
		}

		// Token: 0x1700028B RID: 651
		// (get) Token: 0x06000846 RID: 2118 RVA: 0x0001911D File Offset: 0x0001731D
		// (set) Token: 0x06000847 RID: 2119 RVA: 0x00002C55 File Offset: 0x00000E55
		public override IMetadataScope Scope
		{
			get
			{
				return this.element_type.Scope;
			}
			set
			{
				throw new InvalidOperationException();
			}
		}

		// Token: 0x1700028C RID: 652
		// (get) Token: 0x06000848 RID: 2120 RVA: 0x0001912A File Offset: 0x0001732A
		public override ModuleDefinition Module
		{
			get
			{
				return this.element_type.Module;
			}
		}

		// Token: 0x1700028D RID: 653
		// (get) Token: 0x06000849 RID: 2121 RVA: 0x00019137 File Offset: 0x00017337
		public override string FullName
		{
			get
			{
				return this.element_type.FullName;
			}
		}

		// Token: 0x1700028E RID: 654
		// (get) Token: 0x0600084A RID: 2122 RVA: 0x00019144 File Offset: 0x00017344
		public override bool ContainsGenericParameter
		{
			get
			{
				return this.element_type.ContainsGenericParameter;
			}
		}

		// Token: 0x1700028F RID: 655
		// (get) Token: 0x0600084B RID: 2123 RVA: 0x00011CC3 File Offset: 0x0000FEC3
		public override MetadataType MetadataType
		{
			get
			{
				return (MetadataType)this.etype;
			}
		}

		// Token: 0x0600084C RID: 2124 RVA: 0x00019151 File Offset: 0x00017351
		internal TypeSpecification(TypeReference type)
			: base(null, null)
		{
			this.element_type = type;
			this.token = new MetadataToken(TokenType.TypeSpec);
		}

		// Token: 0x0600084D RID: 2125 RVA: 0x00019172 File Offset: 0x00017372
		public override TypeReference GetElementType()
		{
			return this.element_type.GetElementType();
		}

		// Token: 0x040002D4 RID: 724
		private readonly TypeReference element_type;
	}
}
