using System;
using System.Threading;
using Mono.Cecil.Metadata;
using Mono.Collections.Generic;

namespace Mono.Cecil
{
	// Token: 0x02000175 RID: 373
	internal sealed class TypeDefinition : TypeReference, IMemberDefinition, ICustomAttributeProvider, IMetadataTokenProvider, ISecurityDeclarationProvider
	{
		// Token: 0x17000252 RID: 594
		// (get) Token: 0x06000B42 RID: 2882 RVA: 0x000261A6 File Offset: 0x000243A6
		// (set) Token: 0x06000B43 RID: 2883 RVA: 0x000261AE File Offset: 0x000243AE
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

		// Token: 0x17000253 RID: 595
		// (get) Token: 0x06000B44 RID: 2884 RVA: 0x000261CF File Offset: 0x000243CF
		// (set) Token: 0x06000B45 RID: 2885 RVA: 0x000261D7 File Offset: 0x000243D7
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

		// Token: 0x17000254 RID: 596
		// (get) Token: 0x06000B46 RID: 2886 RVA: 0x000261E0 File Offset: 0x000243E0
		// (set) Token: 0x06000B47 RID: 2887 RVA: 0x000261E8 File Offset: 0x000243E8
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

		// Token: 0x06000B48 RID: 2888 RVA: 0x00026210 File Offset: 0x00024410
		private void ResolveLayout()
		{
			if (!base.HasImage)
			{
				this.packing_size = -1;
				this.class_size = -1;
				return;
			}
			object syncRoot = this.Module.SyncRoot;
			lock (syncRoot)
			{
				if (this.packing_size == -2 && this.class_size == -2)
				{
					Row<short, int> row = this.Module.Read<TypeDefinition, Row<short, int>>(this, (TypeDefinition type, MetadataReader reader) => reader.ReadTypeLayout(type));
					this.packing_size = row.Col1;
					this.class_size = row.Col2;
				}
			}
		}

		// Token: 0x17000255 RID: 597
		// (get) Token: 0x06000B49 RID: 2889 RVA: 0x000262C0 File Offset: 0x000244C0
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

		// Token: 0x17000256 RID: 598
		// (get) Token: 0x06000B4A RID: 2890 RVA: 0x000262F3 File Offset: 0x000244F3
		// (set) Token: 0x06000B4B RID: 2891 RVA: 0x0002631C File Offset: 0x0002451C
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

		// Token: 0x17000257 RID: 599
		// (get) Token: 0x06000B4C RID: 2892 RVA: 0x00026325 File Offset: 0x00024525
		// (set) Token: 0x06000B4D RID: 2893 RVA: 0x0002634E File Offset: 0x0002454E
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

		// Token: 0x17000258 RID: 600
		// (get) Token: 0x06000B4E RID: 2894 RVA: 0x00026358 File Offset: 0x00024558
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

		// Token: 0x17000259 RID: 601
		// (get) Token: 0x06000B4F RID: 2895 RVA: 0x000263B4 File Offset: 0x000245B4
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
				Interlocked.CompareExchange<InterfaceImplementationCollection>(ref this.interfaces, new InterfaceImplementationCollection(this), null);
				return this.interfaces;
			}
		}

		// Token: 0x1700025A RID: 602
		// (get) Token: 0x06000B50 RID: 2896 RVA: 0x00026424 File Offset: 0x00024624
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

		// Token: 0x1700025B RID: 603
		// (get) Token: 0x06000B51 RID: 2897 RVA: 0x00026480 File Offset: 0x00024680
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
				Interlocked.CompareExchange<Collection<TypeDefinition>>(ref this.nested_types, new MemberDefinitionCollection<TypeDefinition>(this), null);
				return this.nested_types;
			}
		}

		// Token: 0x1700025C RID: 604
		// (get) Token: 0x06000B52 RID: 2898 RVA: 0x000264EF File Offset: 0x000246EF
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

		// Token: 0x1700025D RID: 605
		// (get) Token: 0x06000B53 RID: 2899 RVA: 0x00026520 File Offset: 0x00024720
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
				Interlocked.CompareExchange<Collection<MethodDefinition>>(ref this.methods, new MemberDefinitionCollection<MethodDefinition>(this), null);
				return this.methods;
			}
		}

		// Token: 0x1700025E RID: 606
		// (get) Token: 0x06000B54 RID: 2900 RVA: 0x0002658F File Offset: 0x0002478F
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

		// Token: 0x1700025F RID: 607
		// (get) Token: 0x06000B55 RID: 2901 RVA: 0x000265C0 File Offset: 0x000247C0
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
				Interlocked.CompareExchange<Collection<FieldDefinition>>(ref this.fields, new MemberDefinitionCollection<FieldDefinition>(this), null);
				return this.fields;
			}
		}

		// Token: 0x17000260 RID: 608
		// (get) Token: 0x06000B56 RID: 2902 RVA: 0x00026630 File Offset: 0x00024830
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

		// Token: 0x17000261 RID: 609
		// (get) Token: 0x06000B57 RID: 2903 RVA: 0x0002668C File Offset: 0x0002488C
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
				Interlocked.CompareExchange<Collection<EventDefinition>>(ref this.events, new MemberDefinitionCollection<EventDefinition>(this), null);
				return this.events;
			}
		}

		// Token: 0x17000262 RID: 610
		// (get) Token: 0x06000B58 RID: 2904 RVA: 0x000266FC File Offset: 0x000248FC
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

		// Token: 0x17000263 RID: 611
		// (get) Token: 0x06000B59 RID: 2905 RVA: 0x00026758 File Offset: 0x00024958
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
				Interlocked.CompareExchange<Collection<PropertyDefinition>>(ref this.properties, new MemberDefinitionCollection<PropertyDefinition>(this), null);
				return this.properties;
			}
		}

		// Token: 0x17000264 RID: 612
		// (get) Token: 0x06000B5A RID: 2906 RVA: 0x000267C7 File Offset: 0x000249C7
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

		// Token: 0x17000265 RID: 613
		// (get) Token: 0x06000B5B RID: 2907 RVA: 0x000267EC File Offset: 0x000249EC
		public Collection<SecurityDeclaration> SecurityDeclarations
		{
			get
			{
				return this.security_declarations ?? this.GetSecurityDeclarations(ref this.security_declarations, this.Module);
			}
		}

		// Token: 0x17000266 RID: 614
		// (get) Token: 0x06000B5C RID: 2908 RVA: 0x0002680A File Offset: 0x00024A0A
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

		// Token: 0x17000267 RID: 615
		// (get) Token: 0x06000B5D RID: 2909 RVA: 0x0002682F File Offset: 0x00024A2F
		public Collection<CustomAttribute> CustomAttributes
		{
			get
			{
				return this.custom_attributes ?? this.GetCustomAttributes(ref this.custom_attributes, this.Module);
			}
		}

		// Token: 0x17000268 RID: 616
		// (get) Token: 0x06000B5E RID: 2910 RVA: 0x0002684D File Offset: 0x00024A4D
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

		// Token: 0x17000269 RID: 617
		// (get) Token: 0x06000B5F RID: 2911 RVA: 0x00026872 File Offset: 0x00024A72
		public override Collection<GenericParameter> GenericParameters
		{
			get
			{
				return this.generic_parameters ?? this.GetGenericParameters(ref this.generic_parameters, this.Module);
			}
		}

		// Token: 0x1700026A RID: 618
		// (get) Token: 0x06000B60 RID: 2912 RVA: 0x00026890 File Offset: 0x00024A90
		// (set) Token: 0x06000B61 RID: 2913 RVA: 0x0002689F File Offset: 0x00024A9F
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

		// Token: 0x1700026B RID: 619
		// (get) Token: 0x06000B62 RID: 2914 RVA: 0x000268B5 File Offset: 0x00024AB5
		// (set) Token: 0x06000B63 RID: 2915 RVA: 0x000268C4 File Offset: 0x00024AC4
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

		// Token: 0x1700026C RID: 620
		// (get) Token: 0x06000B64 RID: 2916 RVA: 0x000268DA File Offset: 0x00024ADA
		// (set) Token: 0x06000B65 RID: 2917 RVA: 0x000268E9 File Offset: 0x00024AE9
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

		// Token: 0x1700026D RID: 621
		// (get) Token: 0x06000B66 RID: 2918 RVA: 0x000268FF File Offset: 0x00024AFF
		// (set) Token: 0x06000B67 RID: 2919 RVA: 0x0002690E File Offset: 0x00024B0E
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

		// Token: 0x1700026E RID: 622
		// (get) Token: 0x06000B68 RID: 2920 RVA: 0x00026924 File Offset: 0x00024B24
		// (set) Token: 0x06000B69 RID: 2921 RVA: 0x00026933 File Offset: 0x00024B33
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

		// Token: 0x1700026F RID: 623
		// (get) Token: 0x06000B6A RID: 2922 RVA: 0x00026949 File Offset: 0x00024B49
		// (set) Token: 0x06000B6B RID: 2923 RVA: 0x00026958 File Offset: 0x00024B58
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

		// Token: 0x17000270 RID: 624
		// (get) Token: 0x06000B6C RID: 2924 RVA: 0x0002696E File Offset: 0x00024B6E
		// (set) Token: 0x06000B6D RID: 2925 RVA: 0x0002697D File Offset: 0x00024B7D
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

		// Token: 0x17000271 RID: 625
		// (get) Token: 0x06000B6E RID: 2926 RVA: 0x00026993 File Offset: 0x00024B93
		// (set) Token: 0x06000B6F RID: 2927 RVA: 0x000269A2 File Offset: 0x00024BA2
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

		// Token: 0x17000272 RID: 626
		// (get) Token: 0x06000B70 RID: 2928 RVA: 0x000269B8 File Offset: 0x00024BB8
		// (set) Token: 0x06000B71 RID: 2929 RVA: 0x000269C8 File Offset: 0x00024BC8
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

		// Token: 0x17000273 RID: 627
		// (get) Token: 0x06000B72 RID: 2930 RVA: 0x000269DF File Offset: 0x00024BDF
		// (set) Token: 0x06000B73 RID: 2931 RVA: 0x000269EF File Offset: 0x00024BEF
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

		// Token: 0x17000274 RID: 628
		// (get) Token: 0x06000B74 RID: 2932 RVA: 0x00026A06 File Offset: 0x00024C06
		// (set) Token: 0x06000B75 RID: 2933 RVA: 0x00026A17 File Offset: 0x00024C17
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

		// Token: 0x17000275 RID: 629
		// (get) Token: 0x06000B76 RID: 2934 RVA: 0x00026A2F File Offset: 0x00024C2F
		// (set) Token: 0x06000B77 RID: 2935 RVA: 0x00026A3F File Offset: 0x00024C3F
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

		// Token: 0x17000276 RID: 630
		// (get) Token: 0x06000B78 RID: 2936 RVA: 0x00026A56 File Offset: 0x00024C56
		// (set) Token: 0x06000B79 RID: 2937 RVA: 0x00026A67 File Offset: 0x00024C67
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

		// Token: 0x17000277 RID: 631
		// (get) Token: 0x06000B7A RID: 2938 RVA: 0x00026A7F File Offset: 0x00024C7F
		// (set) Token: 0x06000B7B RID: 2939 RVA: 0x00026A91 File Offset: 0x00024C91
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

		// Token: 0x17000278 RID: 632
		// (get) Token: 0x06000B7C RID: 2940 RVA: 0x00026AAA File Offset: 0x00024CAA
		// (set) Token: 0x06000B7D RID: 2941 RVA: 0x00026ABC File Offset: 0x00024CBC
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

		// Token: 0x17000279 RID: 633
		// (get) Token: 0x06000B7E RID: 2942 RVA: 0x00026AD5 File Offset: 0x00024CD5
		// (set) Token: 0x06000B7F RID: 2943 RVA: 0x00026AE7 File Offset: 0x00024CE7
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

		// Token: 0x1700027A RID: 634
		// (get) Token: 0x06000B80 RID: 2944 RVA: 0x00026B00 File Offset: 0x00024D00
		// (set) Token: 0x06000B81 RID: 2945 RVA: 0x00026B12 File Offset: 0x00024D12
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

		// Token: 0x1700027B RID: 635
		// (get) Token: 0x06000B82 RID: 2946 RVA: 0x00026B2B File Offset: 0x00024D2B
		// (set) Token: 0x06000B83 RID: 2947 RVA: 0x00026B3D File Offset: 0x00024D3D
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

		// Token: 0x1700027C RID: 636
		// (get) Token: 0x06000B84 RID: 2948 RVA: 0x00026B56 File Offset: 0x00024D56
		// (set) Token: 0x06000B85 RID: 2949 RVA: 0x00026B68 File Offset: 0x00024D68
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

		// Token: 0x1700027D RID: 637
		// (get) Token: 0x06000B86 RID: 2950 RVA: 0x00026B81 File Offset: 0x00024D81
		// (set) Token: 0x06000B87 RID: 2951 RVA: 0x00026B94 File Offset: 0x00024D94
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

		// Token: 0x1700027E RID: 638
		// (get) Token: 0x06000B88 RID: 2952 RVA: 0x00026BAE File Offset: 0x00024DAE
		// (set) Token: 0x06000B89 RID: 2953 RVA: 0x00026BC5 File Offset: 0x00024DC5
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

		// Token: 0x1700027F RID: 639
		// (get) Token: 0x06000B8A RID: 2954 RVA: 0x00026BE3 File Offset: 0x00024DE3
		// (set) Token: 0x06000B8B RID: 2955 RVA: 0x00026BFA File Offset: 0x00024DFA
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

		// Token: 0x17000280 RID: 640
		// (get) Token: 0x06000B8C RID: 2956 RVA: 0x00026C18 File Offset: 0x00024E18
		// (set) Token: 0x06000B8D RID: 2957 RVA: 0x00026C2A File Offset: 0x00024E2A
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

		// Token: 0x17000281 RID: 641
		// (get) Token: 0x06000B8E RID: 2958 RVA: 0x00026C43 File Offset: 0x00024E43
		// (set) Token: 0x06000B8F RID: 2959 RVA: 0x00026C55 File Offset: 0x00024E55
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

		// Token: 0x17000282 RID: 642
		// (get) Token: 0x06000B90 RID: 2960 RVA: 0x00026C6E File Offset: 0x00024E6E
		// (set) Token: 0x06000B91 RID: 2961 RVA: 0x00026C80 File Offset: 0x00024E80
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

		// Token: 0x17000283 RID: 643
		// (get) Token: 0x06000B92 RID: 2962 RVA: 0x00026C99 File Offset: 0x00024E99
		public bool IsEnum
		{
			get
			{
				return this.base_type != null && this.base_type.IsTypeOf("System", "Enum");
			}
		}

		// Token: 0x17000284 RID: 644
		// (get) Token: 0x06000B93 RID: 2963 RVA: 0x00026CBC File Offset: 0x00024EBC
		// (set) Token: 0x06000B94 RID: 2964 RVA: 0x000039BA File Offset: 0x00001BBA
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

		// Token: 0x17000285 RID: 645
		// (get) Token: 0x06000B95 RID: 2965 RVA: 0x00026D18 File Offset: 0x00024F18
		public override bool IsPrimitive
		{
			get
			{
				ElementType elementType;
				return MetadataSystem.TryGetPrimitiveElementType(this, out elementType) && elementType.IsPrimitive();
			}
		}

		// Token: 0x17000286 RID: 646
		// (get) Token: 0x06000B96 RID: 2966 RVA: 0x00026D38 File Offset: 0x00024F38
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

		// Token: 0x17000287 RID: 647
		// (get) Token: 0x06000B97 RID: 2967 RVA: 0x00010F39 File Offset: 0x0000F139
		public override bool IsDefinition
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000288 RID: 648
		// (get) Token: 0x06000B98 RID: 2968 RVA: 0x00026D57 File Offset: 0x00024F57
		// (set) Token: 0x06000B99 RID: 2969 RVA: 0x00026D64 File Offset: 0x00024F64
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

		// Token: 0x17000289 RID: 649
		// (get) Token: 0x06000B9A RID: 2970 RVA: 0x00026D6D File Offset: 0x00024F6D
		// (set) Token: 0x06000B9B RID: 2971 RVA: 0x0001F94A File Offset: 0x0001DB4A
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

		// Token: 0x06000B9C RID: 2972 RVA: 0x00026D7A File Offset: 0x00024F7A
		public TypeDefinition(string @namespace, string name, TypeAttributes attributes)
			: base(@namespace, name)
		{
			this.attributes = (uint)attributes;
			this.token = new MetadataToken(TokenType.TypeDef);
		}

		// Token: 0x06000B9D RID: 2973 RVA: 0x00026DAB File Offset: 0x00024FAB
		public TypeDefinition(string @namespace, string name, TypeAttributes attributes, TypeReference baseType)
			: this(@namespace, name, attributes)
		{
			this.BaseType = baseType;
		}

		// Token: 0x06000B9E RID: 2974 RVA: 0x00026DC0 File Offset: 0x00024FC0
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

		// Token: 0x06000B9F RID: 2975 RVA: 0x00010978 File Offset: 0x0000EB78
		public override TypeDefinition Resolve()
		{
			return this;
		}

		// Token: 0x040004D2 RID: 1234
		private uint attributes;

		// Token: 0x040004D3 RID: 1235
		private TypeReference base_type;

		// Token: 0x040004D4 RID: 1236
		internal Range fields_range;

		// Token: 0x040004D5 RID: 1237
		internal Range methods_range;

		// Token: 0x040004D6 RID: 1238
		private short packing_size = -2;

		// Token: 0x040004D7 RID: 1239
		private int class_size = -2;

		// Token: 0x040004D8 RID: 1240
		private InterfaceImplementationCollection interfaces;

		// Token: 0x040004D9 RID: 1241
		private Collection<TypeDefinition> nested_types;

		// Token: 0x040004DA RID: 1242
		private Collection<MethodDefinition> methods;

		// Token: 0x040004DB RID: 1243
		private Collection<FieldDefinition> fields;

		// Token: 0x040004DC RID: 1244
		private Collection<EventDefinition> events;

		// Token: 0x040004DD RID: 1245
		private Collection<PropertyDefinition> properties;

		// Token: 0x040004DE RID: 1246
		private Collection<CustomAttribute> custom_attributes;

		// Token: 0x040004DF RID: 1247
		private Collection<SecurityDeclaration> security_declarations;
	}
}
