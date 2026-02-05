using System;
using Mono.Cecil.Metadata;
using Mono.Collections.Generic;

namespace Mono.Cecil
{
	// Token: 0x02000061 RID: 97
	public sealed class GenericParameter : TypeReference, ICustomAttributeProvider, IMetadataTokenProvider
	{
		// Token: 0x170000CC RID: 204
		// (get) Token: 0x0600042A RID: 1066 RVA: 0x00011ACC File Offset: 0x0000FCCC
		// (set) Token: 0x0600042B RID: 1067 RVA: 0x00011AD4 File Offset: 0x0000FCD4
		public GenericParameterAttributes Attributes
		{
			get
			{
				return (GenericParameterAttributes)this.attributes;
			}
			set
			{
				this.attributes = (ushort)value;
			}
		}

		// Token: 0x170000CD RID: 205
		// (get) Token: 0x0600042C RID: 1068 RVA: 0x00011ADD File Offset: 0x0000FCDD
		public int Position
		{
			get
			{
				return this.position;
			}
		}

		// Token: 0x170000CE RID: 206
		// (get) Token: 0x0600042D RID: 1069 RVA: 0x00011AE5 File Offset: 0x0000FCE5
		public GenericParameterType Type
		{
			get
			{
				return this.type;
			}
		}

		// Token: 0x170000CF RID: 207
		// (get) Token: 0x0600042E RID: 1070 RVA: 0x00011AED File Offset: 0x0000FCED
		public IGenericParameterProvider Owner
		{
			get
			{
				return this.owner;
			}
		}

		// Token: 0x170000D0 RID: 208
		// (get) Token: 0x0600042F RID: 1071 RVA: 0x00011AF8 File Offset: 0x0000FCF8
		public bool HasConstraints
		{
			get
			{
				if (this.constraints != null)
				{
					return this.constraints.Count > 0;
				}
				if (base.HasImage)
				{
					return this.Module.Read<GenericParameter, bool>(this, (GenericParameter generic_parameter, MetadataReader reader) => reader.HasGenericConstraints(generic_parameter));
				}
				return false;
			}
		}

		// Token: 0x170000D1 RID: 209
		// (get) Token: 0x06000430 RID: 1072 RVA: 0x00011B54 File Offset: 0x0000FD54
		public Collection<TypeReference> Constraints
		{
			get
			{
				if (this.constraints != null)
				{
					return this.constraints;
				}
				if (base.HasImage)
				{
					return this.Module.Read<GenericParameter, Collection<TypeReference>>(ref this.constraints, this, (GenericParameter generic_parameter, MetadataReader reader) => reader.ReadGenericConstraints(generic_parameter));
				}
				return this.constraints = new Collection<TypeReference>();
			}
		}

		// Token: 0x170000D2 RID: 210
		// (get) Token: 0x06000431 RID: 1073 RVA: 0x00011BB8 File Offset: 0x0000FDB8
		public bool HasCustomAttributes
		{
			get
			{
				if (this.custom_attributes != null)
				{
					return this.custom_attributes.Count > 0;
				}
				return this.GetHasCustomAttributes(this.Module);
			}
		}

		// Token: 0x170000D3 RID: 211
		// (get) Token: 0x06000432 RID: 1074 RVA: 0x00011BDD File Offset: 0x0000FDDD
		public Collection<CustomAttribute> CustomAttributes
		{
			get
			{
				return this.custom_attributes ?? this.GetCustomAttributes(ref this.custom_attributes, this.Module);
			}
		}

		// Token: 0x170000D4 RID: 212
		// (get) Token: 0x06000433 RID: 1075 RVA: 0x00011BFB File Offset: 0x0000FDFB
		// (set) Token: 0x06000434 RID: 1076 RVA: 0x00002C55 File Offset: 0x00000E55
		public override IMetadataScope Scope
		{
			get
			{
				if (this.owner == null)
				{
					return null;
				}
				if (this.owner.GenericParameterType != GenericParameterType.Method)
				{
					return ((TypeReference)this.owner).Scope;
				}
				return ((MethodReference)this.owner).DeclaringType.Scope;
			}
			set
			{
				throw new InvalidOperationException();
			}
		}

		// Token: 0x170000D5 RID: 213
		// (get) Token: 0x06000435 RID: 1077 RVA: 0x00011C3B File Offset: 0x0000FE3B
		// (set) Token: 0x06000436 RID: 1078 RVA: 0x00002C55 File Offset: 0x00000E55
		public override TypeReference DeclaringType
		{
			get
			{
				return this.owner as TypeReference;
			}
			set
			{
				throw new InvalidOperationException();
			}
		}

		// Token: 0x170000D6 RID: 214
		// (get) Token: 0x06000437 RID: 1079 RVA: 0x00011C48 File Offset: 0x0000FE48
		public MethodReference DeclaringMethod
		{
			get
			{
				return this.owner as MethodReference;
			}
		}

		// Token: 0x170000D7 RID: 215
		// (get) Token: 0x06000438 RID: 1080 RVA: 0x00011C55 File Offset: 0x0000FE55
		public override ModuleDefinition Module
		{
			get
			{
				return this.module ?? this.owner.Module;
			}
		}

		// Token: 0x170000D8 RID: 216
		// (get) Token: 0x06000439 RID: 1081 RVA: 0x00011C6C File Offset: 0x0000FE6C
		public override string Name
		{
			get
			{
				if (!string.IsNullOrEmpty(base.Name))
				{
					return base.Name;
				}
				return base.Name = ((this.type == GenericParameterType.Method) ? "!!" : "!") + this.position;
			}
		}

		// Token: 0x170000D9 RID: 217
		// (get) Token: 0x0600043A RID: 1082 RVA: 0x00010431 File Offset: 0x0000E631
		// (set) Token: 0x0600043B RID: 1083 RVA: 0x00002C55 File Offset: 0x00000E55
		public override string Namespace
		{
			get
			{
				return string.Empty;
			}
			set
			{
				throw new InvalidOperationException();
			}
		}

		// Token: 0x170000DA RID: 218
		// (get) Token: 0x0600043C RID: 1084 RVA: 0x00011CBB File Offset: 0x0000FEBB
		public override string FullName
		{
			get
			{
				return this.Name;
			}
		}

		// Token: 0x170000DB RID: 219
		// (get) Token: 0x0600043D RID: 1085 RVA: 0x00002BE8 File Offset: 0x00000DE8
		public override bool IsGenericParameter
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170000DC RID: 220
		// (get) Token: 0x0600043E RID: 1086 RVA: 0x00002BE8 File Offset: 0x00000DE8
		public override bool ContainsGenericParameter
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170000DD RID: 221
		// (get) Token: 0x0600043F RID: 1087 RVA: 0x00011CC3 File Offset: 0x0000FEC3
		public override MetadataType MetadataType
		{
			get
			{
				return (MetadataType)this.etype;
			}
		}

		// Token: 0x170000DE RID: 222
		// (get) Token: 0x06000440 RID: 1088 RVA: 0x00011CCB File Offset: 0x0000FECB
		// (set) Token: 0x06000441 RID: 1089 RVA: 0x00011CDA File Offset: 0x0000FEDA
		public bool IsNonVariant
		{
			get
			{
				return this.attributes.GetMaskedAttributes(3, 0U);
			}
			set
			{
				this.attributes = this.attributes.SetMaskedAttributes(3, 0U, value);
			}
		}

		// Token: 0x170000DF RID: 223
		// (get) Token: 0x06000442 RID: 1090 RVA: 0x00011CF0 File Offset: 0x0000FEF0
		// (set) Token: 0x06000443 RID: 1091 RVA: 0x00011CFF File Offset: 0x0000FEFF
		public bool IsCovariant
		{
			get
			{
				return this.attributes.GetMaskedAttributes(3, 1U);
			}
			set
			{
				this.attributes = this.attributes.SetMaskedAttributes(3, 1U, value);
			}
		}

		// Token: 0x170000E0 RID: 224
		// (get) Token: 0x06000444 RID: 1092 RVA: 0x00011D15 File Offset: 0x0000FF15
		// (set) Token: 0x06000445 RID: 1093 RVA: 0x00011D24 File Offset: 0x0000FF24
		public bool IsContravariant
		{
			get
			{
				return this.attributes.GetMaskedAttributes(3, 2U);
			}
			set
			{
				this.attributes = this.attributes.SetMaskedAttributes(3, 2U, value);
			}
		}

		// Token: 0x170000E1 RID: 225
		// (get) Token: 0x06000446 RID: 1094 RVA: 0x00011D3A File Offset: 0x0000FF3A
		// (set) Token: 0x06000447 RID: 1095 RVA: 0x00011D48 File Offset: 0x0000FF48
		public bool HasReferenceTypeConstraint
		{
			get
			{
				return this.attributes.GetAttributes(4);
			}
			set
			{
				this.attributes = this.attributes.SetAttributes(4, value);
			}
		}

		// Token: 0x170000E2 RID: 226
		// (get) Token: 0x06000448 RID: 1096 RVA: 0x00011D5D File Offset: 0x0000FF5D
		// (set) Token: 0x06000449 RID: 1097 RVA: 0x00011D6B File Offset: 0x0000FF6B
		public bool HasNotNullableValueTypeConstraint
		{
			get
			{
				return this.attributes.GetAttributes(8);
			}
			set
			{
				this.attributes = this.attributes.SetAttributes(8, value);
			}
		}

		// Token: 0x170000E3 RID: 227
		// (get) Token: 0x0600044A RID: 1098 RVA: 0x00011D80 File Offset: 0x0000FF80
		// (set) Token: 0x0600044B RID: 1099 RVA: 0x00011D8F File Offset: 0x0000FF8F
		public bool HasDefaultConstructorConstraint
		{
			get
			{
				return this.attributes.GetAttributes(16);
			}
			set
			{
				this.attributes = this.attributes.SetAttributes(16, value);
			}
		}

		// Token: 0x0600044C RID: 1100 RVA: 0x00011DA5 File Offset: 0x0000FFA5
		public GenericParameter(IGenericParameterProvider owner)
			: this(string.Empty, owner)
		{
		}

		// Token: 0x0600044D RID: 1101 RVA: 0x00011DB4 File Offset: 0x0000FFB4
		public GenericParameter(string name, IGenericParameterProvider owner)
			: base(string.Empty, name)
		{
			if (owner == null)
			{
				throw new ArgumentNullException();
			}
			this.position = -1;
			this.owner = owner;
			this.type = owner.GenericParameterType;
			this.etype = GenericParameter.ConvertGenericParameterType(this.type);
			this.token = new MetadataToken(TokenType.GenericParam);
		}

		// Token: 0x0600044E RID: 1102 RVA: 0x00011E14 File Offset: 0x00010014
		internal GenericParameter(int position, GenericParameterType type, ModuleDefinition module)
			: base(string.Empty, string.Empty)
		{
			Mixin.CheckModule(module);
			this.position = position;
			this.type = type;
			this.etype = GenericParameter.ConvertGenericParameterType(type);
			this.module = module;
			this.token = new MetadataToken(TokenType.GenericParam);
		}

		// Token: 0x0600044F RID: 1103 RVA: 0x00011E68 File Offset: 0x00010068
		private static ElementType ConvertGenericParameterType(GenericParameterType type)
		{
			if (type == GenericParameterType.Type)
			{
				return ElementType.Var;
			}
			if (type != GenericParameterType.Method)
			{
				throw new ArgumentOutOfRangeException();
			}
			return ElementType.MVar;
		}

		// Token: 0x06000450 RID: 1104 RVA: 0x00011945 File Offset: 0x0000FB45
		public override TypeDefinition Resolve()
		{
			return null;
		}

		// Token: 0x040000CB RID: 203
		internal int position;

		// Token: 0x040000CC RID: 204
		internal GenericParameterType type;

		// Token: 0x040000CD RID: 205
		internal IGenericParameterProvider owner;

		// Token: 0x040000CE RID: 206
		private ushort attributes;

		// Token: 0x040000CF RID: 207
		private Collection<TypeReference> constraints;

		// Token: 0x040000D0 RID: 208
		private Collection<CustomAttribute> custom_attributes;
	}
}
