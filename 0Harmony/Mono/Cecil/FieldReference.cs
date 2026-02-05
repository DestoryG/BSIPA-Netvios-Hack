using System;

namespace Mono.Cecil
{
	// Token: 0x0200010F RID: 271
	internal class FieldReference : MemberReference
	{
		// Token: 0x170000C3 RID: 195
		// (get) Token: 0x06000776 RID: 1910 RVA: 0x0001FDD9 File Offset: 0x0001DFD9
		// (set) Token: 0x06000777 RID: 1911 RVA: 0x0001FDE1 File Offset: 0x0001DFE1
		public TypeReference FieldType
		{
			get
			{
				return this.field_type;
			}
			set
			{
				this.field_type = value;
			}
		}

		// Token: 0x170000C4 RID: 196
		// (get) Token: 0x06000778 RID: 1912 RVA: 0x0001FDEA File Offset: 0x0001DFEA
		public override string FullName
		{
			get
			{
				return this.field_type.FullName + " " + base.MemberFullName();
			}
		}

		// Token: 0x170000C5 RID: 197
		// (get) Token: 0x06000779 RID: 1913 RVA: 0x0001FE07 File Offset: 0x0001E007
		public override bool ContainsGenericParameter
		{
			get
			{
				return this.field_type.ContainsGenericParameter || base.ContainsGenericParameter;
			}
		}

		// Token: 0x0600077A RID: 1914 RVA: 0x0001FE1E File Offset: 0x0001E01E
		internal FieldReference()
		{
			this.token = new MetadataToken(TokenType.MemberRef);
		}

		// Token: 0x0600077B RID: 1915 RVA: 0x0001FE36 File Offset: 0x0001E036
		public FieldReference(string name, TypeReference fieldType)
			: base(name)
		{
			Mixin.CheckType(fieldType, Mixin.Argument.fieldType);
			this.field_type = fieldType;
			this.token = new MetadataToken(TokenType.MemberRef);
		}

		// Token: 0x0600077C RID: 1916 RVA: 0x0001FE5E File Offset: 0x0001E05E
		public FieldReference(string name, TypeReference fieldType, TypeReference declaringType)
			: this(name, fieldType)
		{
			Mixin.CheckType(declaringType, Mixin.Argument.declaringType);
			this.DeclaringType = declaringType;
		}

		// Token: 0x0600077D RID: 1917 RVA: 0x0001FE77 File Offset: 0x0001E077
		protected override IMemberDefinition ResolveDefinition()
		{
			return this.Resolve();
		}

		// Token: 0x0600077E RID: 1918 RVA: 0x0001FE7F File Offset: 0x0001E07F
		public new virtual FieldDefinition Resolve()
		{
			ModuleDefinition module = this.Module;
			if (module == null)
			{
				throw new NotSupportedException();
			}
			return module.Resolve(this);
		}

		// Token: 0x040002D3 RID: 723
		private TypeReference field_type;
	}
}
