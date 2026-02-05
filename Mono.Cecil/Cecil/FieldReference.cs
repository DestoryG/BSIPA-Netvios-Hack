using System;

namespace Mono.Cecil
{
	// Token: 0x0200005C RID: 92
	public class FieldReference : MemberReference
	{
		// Token: 0x170000AD RID: 173
		// (get) Token: 0x060003F8 RID: 1016 RVA: 0x00011725 File Offset: 0x0000F925
		// (set) Token: 0x060003F9 RID: 1017 RVA: 0x0001172D File Offset: 0x0000F92D
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

		// Token: 0x170000AE RID: 174
		// (get) Token: 0x060003FA RID: 1018 RVA: 0x00011736 File Offset: 0x0000F936
		public override string FullName
		{
			get
			{
				return this.field_type.FullName + " " + base.MemberFullName();
			}
		}

		// Token: 0x170000AF RID: 175
		// (get) Token: 0x060003FB RID: 1019 RVA: 0x00011753 File Offset: 0x0000F953
		public override bool ContainsGenericParameter
		{
			get
			{
				return this.field_type.ContainsGenericParameter || base.ContainsGenericParameter;
			}
		}

		// Token: 0x060003FC RID: 1020 RVA: 0x0001176A File Offset: 0x0000F96A
		internal FieldReference()
		{
			this.token = new MetadataToken(TokenType.MemberRef);
		}

		// Token: 0x060003FD RID: 1021 RVA: 0x00011782 File Offset: 0x0000F982
		public FieldReference(string name, TypeReference fieldType)
			: base(name)
		{
			Mixin.CheckType(fieldType, Mixin.Argument.fieldType);
			this.field_type = fieldType;
			this.token = new MetadataToken(TokenType.MemberRef);
		}

		// Token: 0x060003FE RID: 1022 RVA: 0x000117AA File Offset: 0x0000F9AA
		public FieldReference(string name, TypeReference fieldType, TypeReference declaringType)
			: this(name, fieldType)
		{
			Mixin.CheckType(declaringType, Mixin.Argument.declaringType);
			this.DeclaringType = declaringType;
		}

		// Token: 0x060003FF RID: 1023 RVA: 0x000117C3 File Offset: 0x0000F9C3
		protected override IMemberDefinition ResolveDefinition()
		{
			return this.Resolve();
		}

		// Token: 0x06000400 RID: 1024 RVA: 0x000117CB File Offset: 0x0000F9CB
		public new virtual FieldDefinition Resolve()
		{
			ModuleDefinition module = this.Module;
			if (module == null)
			{
				throw new NotSupportedException();
			}
			return module.Resolve(this);
		}

		// Token: 0x040000C4 RID: 196
		private TypeReference field_type;
	}
}
