using System;
using System.Threading;
using Mono.Cecil.Metadata;
using Mono.Collections.Generic;

namespace Mono.Cecil
{
	// Token: 0x02000114 RID: 276
	internal sealed class GenericParameter : TypeReference, ICustomAttributeProvider, IMetadataTokenProvider
	{
		// Token: 0x170000E2 RID: 226
		// (get) Token: 0x060007AA RID: 1962 RVA: 0x0002019D File Offset: 0x0001E39D
		// (set) Token: 0x060007AB RID: 1963 RVA: 0x000201A5 File Offset: 0x0001E3A5
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

		// Token: 0x170000E3 RID: 227
		// (get) Token: 0x060007AC RID: 1964 RVA: 0x000201AE File Offset: 0x0001E3AE
		public int Position
		{
			get
			{
				return this.position;
			}
		}

		// Token: 0x170000E4 RID: 228
		// (get) Token: 0x060007AD RID: 1965 RVA: 0x000201B6 File Offset: 0x0001E3B6
		public GenericParameterType Type
		{
			get
			{
				return this.type;
			}
		}

		// Token: 0x170000E5 RID: 229
		// (get) Token: 0x060007AE RID: 1966 RVA: 0x000201BE File Offset: 0x0001E3BE
		public IGenericParameterProvider Owner
		{
			get
			{
				return this.owner;
			}
		}

		// Token: 0x170000E6 RID: 230
		// (get) Token: 0x060007AF RID: 1967 RVA: 0x000201C8 File Offset: 0x0001E3C8
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

		// Token: 0x170000E7 RID: 231
		// (get) Token: 0x060007B0 RID: 1968 RVA: 0x00020224 File Offset: 0x0001E424
		public Collection<GenericParameterConstraint> Constraints
		{
			get
			{
				if (this.constraints != null)
				{
					return this.constraints;
				}
				if (base.HasImage)
				{
					return this.Module.Read<GenericParameter, GenericParameterConstraintCollection>(ref this.constraints, this, (GenericParameter generic_parameter, MetadataReader reader) => reader.ReadGenericConstraints(generic_parameter));
				}
				Interlocked.CompareExchange<GenericParameterConstraintCollection>(ref this.constraints, new GenericParameterConstraintCollection(this), null);
				return this.constraints;
			}
		}

		// Token: 0x170000E8 RID: 232
		// (get) Token: 0x060007B1 RID: 1969 RVA: 0x00020293 File Offset: 0x0001E493
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

		// Token: 0x170000E9 RID: 233
		// (get) Token: 0x060007B2 RID: 1970 RVA: 0x000202B8 File Offset: 0x0001E4B8
		public Collection<CustomAttribute> CustomAttributes
		{
			get
			{
				return this.custom_attributes ?? this.GetCustomAttributes(ref this.custom_attributes, this.Module);
			}
		}

		// Token: 0x170000EA RID: 234
		// (get) Token: 0x060007B3 RID: 1971 RVA: 0x000202D6 File Offset: 0x0001E4D6
		// (set) Token: 0x060007B4 RID: 1972 RVA: 0x00010FA6 File Offset: 0x0000F1A6
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

		// Token: 0x170000EB RID: 235
		// (get) Token: 0x060007B5 RID: 1973 RVA: 0x00020316 File Offset: 0x0001E516
		// (set) Token: 0x060007B6 RID: 1974 RVA: 0x00010FA6 File Offset: 0x0000F1A6
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

		// Token: 0x170000EC RID: 236
		// (get) Token: 0x060007B7 RID: 1975 RVA: 0x00020323 File Offset: 0x0001E523
		public MethodReference DeclaringMethod
		{
			get
			{
				return this.owner as MethodReference;
			}
		}

		// Token: 0x170000ED RID: 237
		// (get) Token: 0x060007B8 RID: 1976 RVA: 0x00020330 File Offset: 0x0001E530
		public override ModuleDefinition Module
		{
			get
			{
				return this.module ?? this.owner.Module;
			}
		}

		// Token: 0x170000EE RID: 238
		// (get) Token: 0x060007B9 RID: 1977 RVA: 0x00020348 File Offset: 0x0001E548
		public override string Name
		{
			get
			{
				if (!string.IsNullOrEmpty(base.Name))
				{
					return base.Name;
				}
				return base.Name = ((this.type == GenericParameterType.Method) ? "!!" : "!") + this.position.ToString();
			}
		}

		// Token: 0x170000EF RID: 239
		// (get) Token: 0x060007BA RID: 1978 RVA: 0x0001E9E9 File Offset: 0x0001CBE9
		// (set) Token: 0x060007BB RID: 1979 RVA: 0x00010FA6 File Offset: 0x0000F1A6
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

		// Token: 0x170000F0 RID: 240
		// (get) Token: 0x060007BC RID: 1980 RVA: 0x00020397 File Offset: 0x0001E597
		public override string FullName
		{
			get
			{
				return this.Name;
			}
		}

		// Token: 0x170000F1 RID: 241
		// (get) Token: 0x060007BD RID: 1981 RVA: 0x00010F39 File Offset: 0x0000F139
		public override bool IsGenericParameter
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170000F2 RID: 242
		// (get) Token: 0x060007BE RID: 1982 RVA: 0x00010F39 File Offset: 0x0000F139
		public override bool ContainsGenericParameter
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170000F3 RID: 243
		// (get) Token: 0x060007BF RID: 1983 RVA: 0x0002039F File Offset: 0x0001E59F
		public override MetadataType MetadataType
		{
			get
			{
				return (MetadataType)this.etype;
			}
		}

		// Token: 0x170000F4 RID: 244
		// (get) Token: 0x060007C0 RID: 1984 RVA: 0x000203A7 File Offset: 0x0001E5A7
		// (set) Token: 0x060007C1 RID: 1985 RVA: 0x000203B6 File Offset: 0x0001E5B6
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

		// Token: 0x170000F5 RID: 245
		// (get) Token: 0x060007C2 RID: 1986 RVA: 0x000203CC File Offset: 0x0001E5CC
		// (set) Token: 0x060007C3 RID: 1987 RVA: 0x000203DB File Offset: 0x0001E5DB
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

		// Token: 0x170000F6 RID: 246
		// (get) Token: 0x060007C4 RID: 1988 RVA: 0x000203F1 File Offset: 0x0001E5F1
		// (set) Token: 0x060007C5 RID: 1989 RVA: 0x00020400 File Offset: 0x0001E600
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

		// Token: 0x170000F7 RID: 247
		// (get) Token: 0x060007C6 RID: 1990 RVA: 0x00020416 File Offset: 0x0001E616
		// (set) Token: 0x060007C7 RID: 1991 RVA: 0x00020424 File Offset: 0x0001E624
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

		// Token: 0x170000F8 RID: 248
		// (get) Token: 0x060007C8 RID: 1992 RVA: 0x00020439 File Offset: 0x0001E639
		// (set) Token: 0x060007C9 RID: 1993 RVA: 0x00020447 File Offset: 0x0001E647
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

		// Token: 0x170000F9 RID: 249
		// (get) Token: 0x060007CA RID: 1994 RVA: 0x0002045C File Offset: 0x0001E65C
		// (set) Token: 0x060007CB RID: 1995 RVA: 0x0002046B File Offset: 0x0001E66B
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

		// Token: 0x060007CC RID: 1996 RVA: 0x00020481 File Offset: 0x0001E681
		public GenericParameter(IGenericParameterProvider owner)
			: this(string.Empty, owner)
		{
		}

		// Token: 0x060007CD RID: 1997 RVA: 0x00020490 File Offset: 0x0001E690
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

		// Token: 0x060007CE RID: 1998 RVA: 0x000204F0 File Offset: 0x0001E6F0
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

		// Token: 0x060007CF RID: 1999 RVA: 0x00020544 File Offset: 0x0001E744
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

		// Token: 0x060007D0 RID: 2000 RVA: 0x0001FFF9 File Offset: 0x0001E1F9
		public override TypeDefinition Resolve()
		{
			return null;
		}

		// Token: 0x040002DA RID: 730
		internal int position;

		// Token: 0x040002DB RID: 731
		internal GenericParameterType type;

		// Token: 0x040002DC RID: 732
		internal IGenericParameterProvider owner;

		// Token: 0x040002DD RID: 733
		private ushort attributes;

		// Token: 0x040002DE RID: 734
		private GenericParameterConstraintCollection constraints;

		// Token: 0x040002DF RID: 735
		private Collection<CustomAttribute> custom_attributes;
	}
}
