using System;
using Mono.Cecil.Metadata;
using Mono.Collections.Generic;

namespace Mono.Cecil
{
	// Token: 0x020000B9 RID: 185
	public sealed class TypeDefinition : TypeReference, IMemberDefinition, ICustomAttributeProvider, IMetadataTokenProvider, ISecurityDeclarationProvider
	{
		// Token: 0x17000232 RID: 562
		// (get) Token: 0x06000784 RID: 1924 RVA: 0x00017577 File Offset: 0x00015777
		// (set) Token: 0x06000785 RID: 1925 RVA: 0x0001757F File Offset: 0x0001577F
		public TypeAttributes Attributes
		{
			get
			{
				return (TypeAttributes)this.attributes;
			}
			set
			{
				if (base.IsWindowsRuntimeProjection && (uint)((ushort)value) != this.attributes)
				{
					throw new InvalidOperationException();
				}
				this.attributes = (uint)value;
			}
		}

		// Token: 0x17000233 RID: 563
		// (get) Token: 0x06000786 RID: 1926 RVA: 0x000175A0 File Offset: 0x000157A0
		// (set) Token: 0x06000787 RID: 1927 RVA: 0x000175A8 File Offset: 0x000157A8
		public TypeReference BaseType
		{
			get
			{
				return this.base_type;
			}
			set
			{
				this.base_type = value;
			}
		}

		// Token: 0x17000234 RID: 564
		// (get) Token: 0x06000788 RID: 1928 RVA: 0x000175B1 File Offset: 0x000157B1
		// (set) Token: 0x06000789 RID: 1929 RVA: 0x000175B9 File Offset: 0x000157B9
		public override string Name
		{
			get
			{
				return base.Name;
			}
			set
			{
				if (base.IsWindowsRuntimeProjection && value != base.Name)
				{
					throw new InvalidOperationException();
				}
				base.Name = value;
			}
		}

		// Token: 0x0600078A RID: 1930 RVA: 0x000175E0 File Offset: 0x000157E0
		private void ResolveLayout()
		{
			if (this.packing_size != -2 || this.class_size != -2)
			{
				return;
			}
			if (!base.HasImage)
			{
				this.packing_size = -1;
				this.class_size = -1;
				return;
			}
			Row<short, int> row = this.Module.Read<TypeDefinition, Row<short, int>>(this, (TypeDefinition type, MetadataReader reader) => reader.ReadTypeLayout(type));
			this.packing_size = row.Col1;
			this.class_size = row.Col2;
		}

		// Token: 0x17000235 RID: 565
		// (get) Token: 0x0600078B RID: 1931 RVA: 0x0001765D File Offset: 0x0001585D
		public bool HasLayoutInfo
		{
			get
			{
				if (this.packing_size >= 0 || this.class_size >= 0)
				{
					return true;
				}
				this.ResolveLayout();
				return this.packing_size >= 0 || this.class_size >= 0;
			}
		}

		// Token: 0x17000236 RID: 566
		// (get) Token: 0x0600078C RID: 1932 RVA: 0x00017690 File Offset: 0x00015890
		// (set) Token: 0x0600078D RID: 1933 RVA: 0x000176B9 File Offset: 0x000158B9
		public short PackingSize
		{
			get
			{
				if (this.packing_size >= 0)
				{
					return this.packing_size;
				}
				this.ResolveLayout();
				if (this.packing_size < 0)
				{
					return -1;
				}
				return this.packing_size;
			}
			set
			{
				this.packing_size = value;
			}
		}

		// Token: 0x17000237 RID: 567
		// (get) Token: 0x0600078E RID: 1934 RVA: 0x000176C2 File Offset: 0x000158C2
		// (set) Token: 0x0600078F RID: 1935 RVA: 0x000176EB File Offset: 0x000158EB
		public int ClassSize
		{
			get
			{
				if (this.class_size >= 0)
				{
					return this.class_size;
				}
				this.ResolveLayout();
				if (this.class_size < 0)
				{
					return -1;
				}
				return this.class_size;
			}
			set
			{
				this.class_size = value;
			}
		}

		// Token: 0x17000238 RID: 568
		// (get) Token: 0x06000790 RID: 1936 RVA: 0x000176F4 File Offset: 0x000158F4
		public bool HasInterfaces
		{
			get
			{
				if (this.interfaces != null)
				{
					return this.interfaces.Count > 0;
				}
				if (base.HasImage)
				{
					return this.Module.Read<TypeDefinition, bool>(this, (TypeDefinition type, MetadataReader reader) => reader.HasInterfaces(type));
				}
				return false;
			}
		}

		// Token: 0x17000239 RID: 569
		// (get) Token: 0x06000791 RID: 1937 RVA: 0x00017750 File Offset: 0x00015950
		public Collection<InterfaceImplementation> Interfaces
		{
			get
			{
				if (this.interfaces != null)
				{
					return this.interfaces;
				}
				if (base.HasImage)
				{
					return this.Module.Read<TypeDefinition, InterfaceImplementationCollection>(ref this.interfaces, this, (TypeDefinition type, MetadataReader reader) => reader.ReadInterfaces(type));
				}
				return this.interfaces = new InterfaceImplementationCollection(this);
			}
		}

		// Token: 0x1700023A RID: 570
		// (get) Token: 0x06000792 RID: 1938 RVA: 0x000177B8 File Offset: 0x000159B8
		public bool HasNestedTypes
		{
			get
			{
				if (this.nested_types != null)
				{
					return this.nested_types.Count > 0;
				}
				if (base.HasImage)
				{
					return this.Module.Read<TypeDefinition, bool>(this, (TypeDefinition type, MetadataReader reader) => reader.HasNestedTypes(type));
				}
				return false;
			}
		}

		// Token: 0x1700023B RID: 571
		// (get) Token: 0x06000793 RID: 1939 RVA: 0x00017814 File Offset: 0x00015A14
		public Collection<TypeDefinition> NestedTypes
		{
			get
			{
				if (this.nested_types != null)
				{
					return this.nested_types;
				}
				if (base.HasImage)
				{
					return this.Module.Read<TypeDefinition, Collection<TypeDefinition>>(ref this.nested_types, this, (TypeDefinition type, MetadataReader reader) => reader.ReadNestedTypes(type));
				}
				return this.nested_types = new MemberDefinitionCollection<TypeDefinition>(this);
			}
		}

		// Token: 0x1700023C RID: 572
		// (get) Token: 0x06000794 RID: 1940 RVA: 0x00017879 File Offset: 0x00015A79
		public bool HasMethods
		{
			get
			{
				if (this.methods != null)
				{
					return this.methods.Count > 0;
				}
				return base.HasImage && this.methods_range.Length > 0U;
			}
		}

		// Token: 0x1700023D RID: 573
		// (get) Token: 0x06000795 RID: 1941 RVA: 0x000178AC File Offset: 0x00015AAC
		public Collection<MethodDefinition> Methods
		{
			get
			{
				if (this.methods != null)
				{
					return this.methods;
				}
				if (base.HasImage)
				{
					return this.Module.Read<TypeDefinition, Collection<MethodDefinition>>(ref this.methods, this, (TypeDefinition type, MetadataReader reader) => reader.ReadMethods(type));
				}
				return this.methods = new MemberDefinitionCollection<MethodDefinition>(this);
			}
		}

		// Token: 0x1700023E RID: 574
		// (get) Token: 0x06000796 RID: 1942 RVA: 0x00017911 File Offset: 0x00015B11
		public bool HasFields
		{
			get
			{
				if (this.fields != null)
				{
					return this.fields.Count > 0;
				}
				return base.HasImage && this.fields_range.Length > 0U;
			}
		}

		// Token: 0x1700023F RID: 575
		// (get) Token: 0x06000797 RID: 1943 RVA: 0x00017944 File Offset: 0x00015B44
		public Collection<FieldDefinition> Fields
		{
			get
			{
				if (this.fields != null)
				{
					return this.fields;
				}
				if (base.HasImage)
				{
					return this.Module.Read<TypeDefinition, Collection<FieldDefinition>>(ref this.fields, this, (TypeDefinition type, MetadataReader reader) => reader.ReadFields(type));
				}
				return this.fields = new MemberDefinitionCollection<FieldDefinition>(this);
			}
		}

		// Token: 0x17000240 RID: 576
		// (get) Token: 0x06000798 RID: 1944 RVA: 0x000179AC File Offset: 0x00015BAC
		public bool HasEvents
		{
			get
			{
				if (this.events != null)
				{
					return this.events.Count > 0;
				}
				if (base.HasImage)
				{
					return this.Module.Read<TypeDefinition, bool>(this, (TypeDefinition type, MetadataReader reader) => reader.HasEvents(type));
				}
				return false;
			}
		}

		// Token: 0x17000241 RID: 577
		// (get) Token: 0x06000799 RID: 1945 RVA: 0x00017A08 File Offset: 0x00015C08
		public Collection<EventDefinition> Events
		{
			get
			{
				if (this.events != null)
				{
					return this.events;
				}
				if (base.HasImage)
				{
					return this.Module.Read<TypeDefinition, Collection<EventDefinition>>(ref this.events, this, (TypeDefinition type, MetadataReader reader) => reader.ReadEvents(type));
				}
				return this.events = new MemberDefinitionCollection<EventDefinition>(this);
			}
		}

		// Token: 0x17000242 RID: 578
		// (get) Token: 0x0600079A RID: 1946 RVA: 0x00017A70 File Offset: 0x00015C70
		public bool HasProperties
		{
			get
			{
				if (this.properties != null)
				{
					return this.properties.Count > 0;
				}
				if (base.HasImage)
				{
					return this.Module.Read<TypeDefinition, bool>(this, (TypeDefinition type, MetadataReader reader) => reader.HasProperties(type));
				}
				return false;
			}
		}

		// Token: 0x17000243 RID: 579
		// (get) Token: 0x0600079B RID: 1947 RVA: 0x00017ACC File Offset: 0x00015CCC
		public Collection<PropertyDefinition> Properties
		{
			get
			{
				if (this.properties != null)
				{
					return this.properties;
				}
				if (base.HasImage)
				{
					return this.Module.Read<TypeDefinition, Collection<PropertyDefinition>>(ref this.properties, this, (TypeDefinition type, MetadataReader reader) => reader.ReadProperties(type));
				}
				return this.properties = new MemberDefinitionCollection<PropertyDefinition>(this);
			}
		}

		// Token: 0x17000244 RID: 580
		// (get) Token: 0x0600079C RID: 1948 RVA: 0x00017B31 File Offset: 0x00015D31
		public bool HasSecurityDeclarations
		{
			get
			{
				if (this.security_declarations != null)
				{
					return this.security_declarations.Count > 0;
				}
				return this.GetHasSecurityDeclarations(this.Module);
			}
		}

		// Token: 0x17000245 RID: 581
		// (get) Token: 0x0600079D RID: 1949 RVA: 0x00017B56 File Offset: 0x00015D56
		public Collection<SecurityDeclaration> SecurityDeclarations
		{
			get
			{
				return this.security_declarations ?? this.GetSecurityDeclarations(ref this.security_declarations, this.Module);
			}
		}

		// Token: 0x17000246 RID: 582
		// (get) Token: 0x0600079E RID: 1950 RVA: 0x00017B74 File Offset: 0x00015D74
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

		// Token: 0x17000247 RID: 583
		// (get) Token: 0x0600079F RID: 1951 RVA: 0x00017B99 File Offset: 0x00015D99
		public Collection<CustomAttribute> CustomAttributes
		{
			get
			{
				return this.custom_attributes ?? this.GetCustomAttributes(ref this.custom_attributes, this.Module);
			}
		}

		// Token: 0x17000248 RID: 584
		// (get) Token: 0x060007A0 RID: 1952 RVA: 0x00017BB7 File Offset: 0x00015DB7
		public override bool HasGenericParameters
		{
			get
			{
				if (this.generic_parameters != null)
				{
					return this.generic_parameters.Count > 0;
				}
				return this.GetHasGenericParameters(this.Module);
			}
		}

		// Token: 0x17000249 RID: 585
		// (get) Token: 0x060007A1 RID: 1953 RVA: 0x00017BDC File Offset: 0x00015DDC
		public override Collection<GenericParameter> GenericParameters
		{
			get
			{
				return this.generic_parameters ?? this.GetGenericParameters(ref this.generic_parameters, this.Module);
			}
		}

		// Token: 0x1700024A RID: 586
		// (get) Token: 0x060007A2 RID: 1954 RVA: 0x00017BFA File Offset: 0x00015DFA
		// (set) Token: 0x060007A3 RID: 1955 RVA: 0x00017C09 File Offset: 0x00015E09
		public bool IsNotPublic
		{
			get
			{
				return this.attributes.GetMaskedAttributes(7U, 0U);
			}
			set
			{
				this.attributes = this.attributes.SetMaskedAttributes(7U, 0U, value);
			}
		}

		// Token: 0x1700024B RID: 587
		// (get) Token: 0x060007A4 RID: 1956 RVA: 0x00017C1F File Offset: 0x00015E1F
		// (set) Token: 0x060007A5 RID: 1957 RVA: 0x00017C2E File Offset: 0x00015E2E
		public bool IsPublic
		{
			get
			{
				return this.attributes.GetMaskedAttributes(7U, 1U);
			}
			set
			{
				this.attributes = this.attributes.SetMaskedAttributes(7U, 1U, value);
			}
		}

		// Token: 0x1700024C RID: 588
		// (get) Token: 0x060007A6 RID: 1958 RVA: 0x00017C44 File Offset: 0x00015E44
		// (set) Token: 0x060007A7 RID: 1959 RVA: 0x00017C53 File Offset: 0x00015E53
		public bool IsNestedPublic
		{
			get
			{
				return this.attributes.GetMaskedAttributes(7U, 2U);
			}
			set
			{
				this.attributes = this.attributes.SetMaskedAttributes(7U, 2U, value);
			}
		}

		// Token: 0x1700024D RID: 589
		// (get) Token: 0x060007A8 RID: 1960 RVA: 0x00017C69 File Offset: 0x00015E69
		// (set) Token: 0x060007A9 RID: 1961 RVA: 0x00017C78 File Offset: 0x00015E78
		public bool IsNestedPrivate
		{
			get
			{
				return this.attributes.GetMaskedAttributes(7U, 3U);
			}
			set
			{
				this.attributes = this.attributes.SetMaskedAttributes(7U, 3U, value);
			}
		}

		// Token: 0x1700024E RID: 590
		// (get) Token: 0x060007AA RID: 1962 RVA: 0x00017C8E File Offset: 0x00015E8E
		// (set) Token: 0x060007AB RID: 1963 RVA: 0x00017C9D File Offset: 0x00015E9D
		public bool IsNestedFamily
		{
			get
			{
				return this.attributes.GetMaskedAttributes(7U, 4U);
			}
			set
			{
				this.attributes = this.attributes.SetMaskedAttributes(7U, 4U, value);
			}
		}

		// Token: 0x1700024F RID: 591
		// (get) Token: 0x060007AC RID: 1964 RVA: 0x00017CB3 File Offset: 0x00015EB3
		// (set) Token: 0x060007AD RID: 1965 RVA: 0x00017CC2 File Offset: 0x00015EC2
		public bool IsNestedAssembly
		{
			get
			{
				return this.attributes.GetMaskedAttributes(7U, 5U);
			}
			set
			{
				this.attributes = this.attributes.SetMaskedAttributes(7U, 5U, value);
			}
		}

		// Token: 0x17000250 RID: 592
		// (get) Token: 0x060007AE RID: 1966 RVA: 0x00017CD8 File Offset: 0x00015ED8
		// (set) Token: 0x060007AF RID: 1967 RVA: 0x00017CE7 File Offset: 0x00015EE7
		public bool IsNestedFamilyAndAssembly
		{
			get
			{
				return this.attributes.GetMaskedAttributes(7U, 6U);
			}
			set
			{
				this.attributes = this.attributes.SetMaskedAttributes(7U, 6U, value);
			}
		}

		// Token: 0x17000251 RID: 593
		// (get) Token: 0x060007B0 RID: 1968 RVA: 0x00017CFD File Offset: 0x00015EFD
		// (set) Token: 0x060007B1 RID: 1969 RVA: 0x00017D0C File Offset: 0x00015F0C
		public bool IsNestedFamilyOrAssembly
		{
			get
			{
				return this.attributes.GetMaskedAttributes(7U, 7U);
			}
			set
			{
				this.attributes = this.attributes.SetMaskedAttributes(7U, 7U, value);
			}
		}

		// Token: 0x17000252 RID: 594
		// (get) Token: 0x060007B2 RID: 1970 RVA: 0x00017D22 File Offset: 0x00015F22
		// (set) Token: 0x060007B3 RID: 1971 RVA: 0x00017D32 File Offset: 0x00015F32
		public bool IsAutoLayout
		{
			get
			{
				return this.attributes.GetMaskedAttributes(24U, 0U);
			}
			set
			{
				this.attributes = this.attributes.SetMaskedAttributes(24U, 0U, value);
			}
		}

		// Token: 0x17000253 RID: 595
		// (get) Token: 0x060007B4 RID: 1972 RVA: 0x00017D49 File Offset: 0x00015F49
		// (set) Token: 0x060007B5 RID: 1973 RVA: 0x00017D59 File Offset: 0x00015F59
		public bool IsSequentialLayout
		{
			get
			{
				return this.attributes.GetMaskedAttributes(24U, 8U);
			}
			set
			{
				this.attributes = this.attributes.SetMaskedAttributes(24U, 8U, value);
			}
		}

		// Token: 0x17000254 RID: 596
		// (get) Token: 0x060007B6 RID: 1974 RVA: 0x00017D70 File Offset: 0x00015F70
		// (set) Token: 0x060007B7 RID: 1975 RVA: 0x00017D81 File Offset: 0x00015F81
		public bool IsExplicitLayout
		{
			get
			{
				return this.attributes.GetMaskedAttributes(24U, 16U);
			}
			set
			{
				this.attributes = this.attributes.SetMaskedAttributes(24U, 16U, value);
			}
		}

		// Token: 0x17000255 RID: 597
		// (get) Token: 0x060007B8 RID: 1976 RVA: 0x00017D99 File Offset: 0x00015F99
		// (set) Token: 0x060007B9 RID: 1977 RVA: 0x00017DA9 File Offset: 0x00015FA9
		public bool IsClass
		{
			get
			{
				return this.attributes.GetMaskedAttributes(32U, 0U);
			}
			set
			{
				this.attributes = this.attributes.SetMaskedAttributes(32U, 0U, value);
			}
		}

		// Token: 0x17000256 RID: 598
		// (get) Token: 0x060007BA RID: 1978 RVA: 0x00017DC0 File Offset: 0x00015FC0
		// (set) Token: 0x060007BB RID: 1979 RVA: 0x00017DD1 File Offset: 0x00015FD1
		public bool IsInterface
		{
			get
			{
				return this.attributes.GetMaskedAttributes(32U, 32U);
			}
			set
			{
				this.attributes = this.attributes.SetMaskedAttributes(32U, 32U, value);
			}
		}

		// Token: 0x17000257 RID: 599
		// (get) Token: 0x060007BC RID: 1980 RVA: 0x00017DE9 File Offset: 0x00015FE9
		// (set) Token: 0x060007BD RID: 1981 RVA: 0x00017DFB File Offset: 0x00015FFB
		public bool IsAbstract
		{
			get
			{
				return this.attributes.GetAttributes(128U);
			}
			set
			{
				this.attributes = this.attributes.SetAttributes(128U, value);
			}
		}

		// Token: 0x17000258 RID: 600
		// (get) Token: 0x060007BE RID: 1982 RVA: 0x00017E14 File Offset: 0x00016014
		// (set) Token: 0x060007BF RID: 1983 RVA: 0x00017E26 File Offset: 0x00016026
		public bool IsSealed
		{
			get
			{
				return this.attributes.GetAttributes(256U);
			}
			set
			{
				this.attributes = this.attributes.SetAttributes(256U, value);
			}
		}

		// Token: 0x17000259 RID: 601
		// (get) Token: 0x060007C0 RID: 1984 RVA: 0x00017E3F File Offset: 0x0001603F
		// (set) Token: 0x060007C1 RID: 1985 RVA: 0x00017E51 File Offset: 0x00016051
		public bool IsSpecialName
		{
			get
			{
				return this.attributes.GetAttributes(1024U);
			}
			set
			{
				this.attributes = this.attributes.SetAttributes(1024U, value);
			}
		}

		// Token: 0x1700025A RID: 602
		// (get) Token: 0x060007C2 RID: 1986 RVA: 0x00017E6A File Offset: 0x0001606A
		// (set) Token: 0x060007C3 RID: 1987 RVA: 0x00017E7C File Offset: 0x0001607C
		public bool IsImport
		{
			get
			{
				return this.attributes.GetAttributes(4096U);
			}
			set
			{
				this.attributes = this.attributes.SetAttributes(4096U, value);
			}
		}

		// Token: 0x1700025B RID: 603
		// (get) Token: 0x060007C4 RID: 1988 RVA: 0x00017E95 File Offset: 0x00016095
		// (set) Token: 0x060007C5 RID: 1989 RVA: 0x00017EA7 File Offset: 0x000160A7
		public bool IsSerializable
		{
			get
			{
				return this.attributes.GetAttributes(8192U);
			}
			set
			{
				this.attributes = this.attributes.SetAttributes(8192U, value);
			}
		}

		// Token: 0x1700025C RID: 604
		// (get) Token: 0x060007C6 RID: 1990 RVA: 0x00017EC0 File Offset: 0x000160C0
		// (set) Token: 0x060007C7 RID: 1991 RVA: 0x00017ED2 File Offset: 0x000160D2
		public bool IsWindowsRuntime
		{
			get
			{
				return this.attributes.GetAttributes(16384U);
			}
			set
			{
				this.attributes = this.attributes.SetAttributes(16384U, value);
			}
		}

		// Token: 0x1700025D RID: 605
		// (get) Token: 0x060007C8 RID: 1992 RVA: 0x00017EEB File Offset: 0x000160EB
		// (set) Token: 0x060007C9 RID: 1993 RVA: 0x00017EFE File Offset: 0x000160FE
		public bool IsAnsiClass
		{
			get
			{
				return this.attributes.GetMaskedAttributes(196608U, 0U);
			}
			set
			{
				this.attributes = this.attributes.SetMaskedAttributes(196608U, 0U, value);
			}
		}

		// Token: 0x1700025E RID: 606
		// (get) Token: 0x060007CA RID: 1994 RVA: 0x00017F18 File Offset: 0x00016118
		// (set) Token: 0x060007CB RID: 1995 RVA: 0x00017F2F File Offset: 0x0001612F
		public bool IsUnicodeClass
		{
			get
			{
				return this.attributes.GetMaskedAttributes(196608U, 65536U);
			}
			set
			{
				this.attributes = this.attributes.SetMaskedAttributes(196608U, 65536U, value);
			}
		}

		// Token: 0x1700025F RID: 607
		// (get) Token: 0x060007CC RID: 1996 RVA: 0x00017F4D File Offset: 0x0001614D
		// (set) Token: 0x060007CD RID: 1997 RVA: 0x00017F64 File Offset: 0x00016164
		public bool IsAutoClass
		{
			get
			{
				return this.attributes.GetMaskedAttributes(196608U, 131072U);
			}
			set
			{
				this.attributes = this.attributes.SetMaskedAttributes(196608U, 131072U, value);
			}
		}

		// Token: 0x17000260 RID: 608
		// (get) Token: 0x060007CE RID: 1998 RVA: 0x00017F82 File Offset: 0x00016182
		// (set) Token: 0x060007CF RID: 1999 RVA: 0x00017F94 File Offset: 0x00016194
		public bool IsBeforeFieldInit
		{
			get
			{
				return this.attributes.GetAttributes(1048576U);
			}
			set
			{
				this.attributes = this.attributes.SetAttributes(1048576U, value);
			}
		}

		// Token: 0x17000261 RID: 609
		// (get) Token: 0x060007D0 RID: 2000 RVA: 0x00017FAD File Offset: 0x000161AD
		// (set) Token: 0x060007D1 RID: 2001 RVA: 0x00017FBF File Offset: 0x000161BF
		public bool IsRuntimeSpecialName
		{
			get
			{
				return this.attributes.GetAttributes(2048U);
			}
			set
			{
				this.attributes = this.attributes.SetAttributes(2048U, value);
			}
		}

		// Token: 0x17000262 RID: 610
		// (get) Token: 0x060007D2 RID: 2002 RVA: 0x00017FD8 File Offset: 0x000161D8
		// (set) Token: 0x060007D3 RID: 2003 RVA: 0x00017FEA File Offset: 0x000161EA
		public bool HasSecurity
		{
			get
			{
				return this.attributes.GetAttributes(262144U);
			}
			set
			{
				this.attributes = this.attributes.SetAttributes(262144U, value);
			}
		}

		// Token: 0x17000263 RID: 611
		// (get) Token: 0x060007D4 RID: 2004 RVA: 0x00018003 File Offset: 0x00016203
		public bool IsEnum
		{
			get
			{
				return this.base_type != null && this.base_type.IsTypeOf("System", "Enum");
			}
		}

		// Token: 0x17000264 RID: 612
		// (get) Token: 0x060007D5 RID: 2005 RVA: 0x00018024 File Offset: 0x00016224
		// (set) Token: 0x060007D6 RID: 2006 RVA: 0x00011A5E File Offset: 0x0000FC5E
		public override bool IsValueType
		{
			get
			{
				return this.base_type != null && (this.base_type.IsTypeOf("System", "Enum") || (this.base_type.IsTypeOf("System", "ValueType") && !this.IsTypeOf("System", "Enum")));
			}
			set
			{
				throw new NotSupportedException();
			}
		}

		// Token: 0x17000265 RID: 613
		// (get) Token: 0x060007D7 RID: 2007 RVA: 0x00018080 File Offset: 0x00016280
		public override bool IsPrimitive
		{
			get
			{
				ElementType elementType;
				return MetadataSystem.TryGetPrimitiveElementType(this, out elementType) && elementType.IsPrimitive();
			}
		}

		// Token: 0x17000266 RID: 614
		// (get) Token: 0x060007D8 RID: 2008 RVA: 0x000180A0 File Offset: 0x000162A0
		public override MetadataType MetadataType
		{
			get
			{
				ElementType elementType;
				if (MetadataSystem.TryGetPrimitiveElementType(this, out elementType))
				{
					return (MetadataType)elementType;
				}
				return base.MetadataType;
			}
		}

		// Token: 0x17000267 RID: 615
		// (get) Token: 0x060007D9 RID: 2009 RVA: 0x00002BE8 File Offset: 0x00000DE8
		public override bool IsDefinition
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000268 RID: 616
		// (get) Token: 0x060007DA RID: 2010 RVA: 0x000180BF File Offset: 0x000162BF
		// (set) Token: 0x060007DB RID: 2011 RVA: 0x000180CC File Offset: 0x000162CC
		public new TypeDefinition DeclaringType
		{
			get
			{
				return (TypeDefinition)base.DeclaringType;
			}
			set
			{
				base.DeclaringType = value;
			}
		}

		// Token: 0x17000269 RID: 617
		// (get) Token: 0x060007DC RID: 2012 RVA: 0x000180D5 File Offset: 0x000162D5
		// (set) Token: 0x060007DD RID: 2013 RVA: 0x000112F3 File Offset: 0x0000F4F3
		internal new TypeDefinitionProjection WindowsRuntimeProjection
		{
			get
			{
				return (TypeDefinitionProjection)this.projection;
			}
			set
			{
				this.projection = value;
			}
		}

		// Token: 0x060007DE RID: 2014 RVA: 0x000180E2 File Offset: 0x000162E2
		public TypeDefinition(string @namespace, string name, TypeAttributes attributes)
			: base(@namespace, name)
		{
			this.attributes = (uint)attributes;
			this.token = new MetadataToken(TokenType.TypeDef);
		}

		// Token: 0x060007DF RID: 2015 RVA: 0x00018113 File Offset: 0x00016313
		public TypeDefinition(string @namespace, string name, TypeAttributes attributes, TypeReference baseType)
			: this(@namespace, name, attributes)
		{
			this.BaseType = baseType;
		}

		// Token: 0x060007E0 RID: 2016 RVA: 0x00018128 File Offset: 0x00016328
		protected override void ClearFullName()
		{
			base.ClearFullName();
			if (!this.HasNestedTypes)
			{
				return;
			}
			Collection<TypeDefinition> nestedTypes = this.NestedTypes;
			for (int i = 0; i < nestedTypes.Count; i++)
			{
				nestedTypes[i].ClearFullName();
			}
		}

		// Token: 0x060007E1 RID: 2017 RVA: 0x00002740 File Offset: 0x00000940
		public override TypeDefinition Resolve()
		{
			return this;
		}

		// Token: 0x04000295 RID: 661
		private uint attributes;

		// Token: 0x04000296 RID: 662
		private TypeReference base_type;

		// Token: 0x04000297 RID: 663
		internal Range fields_range;

		// Token: 0x04000298 RID: 664
		internal Range methods_range;

		// Token: 0x04000299 RID: 665
		private short packing_size = -2;

		// Token: 0x0400029A RID: 666
		private int class_size = -2;

		// Token: 0x0400029B RID: 667
		private InterfaceImplementationCollection interfaces;

		// Token: 0x0400029C RID: 668
		private Collection<TypeDefinition> nested_types;

		// Token: 0x0400029D RID: 669
		private Collection<MethodDefinition> methods;

		// Token: 0x0400029E RID: 670
		private Collection<FieldDefinition> fields;

		// Token: 0x0400029F RID: 671
		private Collection<EventDefinition> events;

		// Token: 0x040002A0 RID: 672
		private Collection<PropertyDefinition> properties;

		// Token: 0x040002A1 RID: 673
		private Collection<CustomAttribute> custom_attributes;

		// Token: 0x040002A2 RID: 674
		private Collection<SecurityDeclaration> security_declarations;
	}
}
