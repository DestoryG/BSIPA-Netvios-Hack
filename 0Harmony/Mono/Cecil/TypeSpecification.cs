using System;

namespace Mono.Cecil
{
	// Token: 0x0200017E RID: 382
	internal abstract class TypeSpecification : TypeReference
	{
		// Token: 0x170002A8 RID: 680
		// (get) Token: 0x06000C0D RID: 3085 RVA: 0x00027E01 File Offset: 0x00026001
		public TypeReference ElementType
		{
			get
			{
				return this.element_type;
			}
		}

		// Token: 0x170002A9 RID: 681
		// (get) Token: 0x06000C0E RID: 3086 RVA: 0x00027E09 File Offset: 0x00026009
		// (set) Token: 0x06000C0F RID: 3087 RVA: 0x00010FA6 File Offset: 0x0000F1A6
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

		// Token: 0x170002AA RID: 682
		// (get) Token: 0x06000C10 RID: 3088 RVA: 0x00027E16 File Offset: 0x00026016
		// (set) Token: 0x06000C11 RID: 3089 RVA: 0x00010FA6 File Offset: 0x0000F1A6
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

		// Token: 0x170002AB RID: 683
		// (get) Token: 0x06000C12 RID: 3090 RVA: 0x00027E23 File Offset: 0x00026023
		// (set) Token: 0x06000C13 RID: 3091 RVA: 0x00010FA6 File Offset: 0x0000F1A6
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

		// Token: 0x170002AC RID: 684
		// (get) Token: 0x06000C14 RID: 3092 RVA: 0x00027E30 File Offset: 0x00026030
		public override ModuleDefinition Module
		{
			get
			{
				return this.element_type.Module;
			}
		}

		// Token: 0x170002AD RID: 685
		// (get) Token: 0x06000C15 RID: 3093 RVA: 0x00027E3D File Offset: 0x0002603D
		public override string FullName
		{
			get
			{
				return this.element_type.FullName;
			}
		}

		// Token: 0x170002AE RID: 686
		// (get) Token: 0x06000C16 RID: 3094 RVA: 0x00027E4A File Offset: 0x0002604A
		public override bool ContainsGenericParameter
		{
			get
			{
				return this.element_type.ContainsGenericParameter;
			}
		}

		// Token: 0x170002AF RID: 687
		// (get) Token: 0x06000C17 RID: 3095 RVA: 0x0002039F File Offset: 0x0001E59F
		public override MetadataType MetadataType
		{
			get
			{
				return (MetadataType)this.etype;
			}
		}

		// Token: 0x06000C18 RID: 3096 RVA: 0x00027E57 File Offset: 0x00026057
		internal TypeSpecification(TypeReference type)
			: base(null, null)
		{
			this.element_type = type;
			this.token = new MetadataToken(TokenType.TypeSpec);
		}

		// Token: 0x06000C19 RID: 3097 RVA: 0x00027E78 File Offset: 0x00026078
		public override TypeReference GetElementType()
		{
			return this.element_type.GetElementType();
		}

		// Token: 0x04000526 RID: 1318
		private readonly TypeReference element_type;
	}
}
