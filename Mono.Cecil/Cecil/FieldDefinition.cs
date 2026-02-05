using System;
using Mono.Collections.Generic;

namespace Mono.Cecil
{
	// Token: 0x0200005B RID: 91
	public sealed class FieldDefinition : FieldReference, IMemberDefinition, ICustomAttributeProvider, IMetadataTokenProvider, IConstantProvider, IMarshalInfoProvider
	{
		// Token: 0x060003C0 RID: 960 RVA: 0x0001123C File Offset: 0x0000F43C
		private void ResolveLayout()
		{
			if (this.offset != -2)
			{
				return;
			}
			if (!base.HasImage)
			{
				this.offset = -1;
				return;
			}
			this.offset = this.Module.Read<FieldDefinition, int>(this, (FieldDefinition field, MetadataReader reader) => reader.ReadFieldLayout(field));
		}

		// Token: 0x17000090 RID: 144
		// (get) Token: 0x060003C1 RID: 961 RVA: 0x00011295 File Offset: 0x0000F495
		public bool HasLayoutInfo
		{
			get
			{
				if (this.offset >= 0)
				{
					return true;
				}
				this.ResolveLayout();
				return this.offset >= 0;
			}
		}

		// Token: 0x17000091 RID: 145
		// (get) Token: 0x060003C2 RID: 962 RVA: 0x000112B4 File Offset: 0x0000F4B4
		// (set) Token: 0x060003C3 RID: 963 RVA: 0x000112DD File Offset: 0x0000F4DD
		public int Offset
		{
			get
			{
				if (this.offset >= 0)
				{
					return this.offset;
				}
				this.ResolveLayout();
				if (this.offset < 0)
				{
					return -1;
				}
				return this.offset;
			}
			set
			{
				this.offset = value;
			}
		}

		// Token: 0x17000092 RID: 146
		// (get) Token: 0x060003C4 RID: 964 RVA: 0x000112E6 File Offset: 0x0000F4E6
		// (set) Token: 0x060003C5 RID: 965 RVA: 0x000112F3 File Offset: 0x0000F4F3
		internal new FieldDefinitionProjection WindowsRuntimeProjection
		{
			get
			{
				return (FieldDefinitionProjection)this.projection;
			}
			set
			{
				this.projection = value;
			}
		}

		// Token: 0x060003C6 RID: 966 RVA: 0x000112FC File Offset: 0x0000F4FC
		private void ResolveRVA()
		{
			if (this.rva != -2)
			{
				return;
			}
			if (!base.HasImage)
			{
				return;
			}
			this.rva = this.Module.Read<FieldDefinition, int>(this, (FieldDefinition field, MetadataReader reader) => reader.ReadFieldRVA(field));
		}

		// Token: 0x17000093 RID: 147
		// (get) Token: 0x060003C7 RID: 967 RVA: 0x0001134E File Offset: 0x0000F54E
		public int RVA
		{
			get
			{
				if (this.rva > 0)
				{
					return this.rva;
				}
				this.ResolveRVA();
				if (this.rva <= 0)
				{
					return 0;
				}
				return this.rva;
			}
		}

		// Token: 0x17000094 RID: 148
		// (get) Token: 0x060003C8 RID: 968 RVA: 0x00011377 File Offset: 0x0000F577
		// (set) Token: 0x060003C9 RID: 969 RVA: 0x000113A7 File Offset: 0x0000F5A7
		public byte[] InitialValue
		{
			get
			{
				if (this.initial_value != null)
				{
					return this.initial_value;
				}
				this.ResolveRVA();
				if (this.initial_value == null)
				{
					this.initial_value = Empty<byte>.Array;
				}
				return this.initial_value;
			}
			set
			{
				this.initial_value = value;
				this.rva = 0;
			}
		}

		// Token: 0x17000095 RID: 149
		// (get) Token: 0x060003CA RID: 970 RVA: 0x000113B7 File Offset: 0x0000F5B7
		// (set) Token: 0x060003CB RID: 971 RVA: 0x000113BF File Offset: 0x0000F5BF
		public FieldAttributes Attributes
		{
			get
			{
				return (FieldAttributes)this.attributes;
			}
			set
			{
				if (base.IsWindowsRuntimeProjection && value != (FieldAttributes)this.attributes)
				{
					throw new InvalidOperationException();
				}
				this.attributes = (ushort)value;
			}
		}

		// Token: 0x17000096 RID: 150
		// (get) Token: 0x060003CC RID: 972 RVA: 0x000113DF File Offset: 0x0000F5DF
		// (set) Token: 0x060003CD RID: 973 RVA: 0x00011403 File Offset: 0x0000F603
		public bool HasConstant
		{
			get
			{
				this.ResolveConstant(ref this.constant, this.Module);
				return this.constant != Mixin.NoValue;
			}
			set
			{
				if (!value)
				{
					this.constant = Mixin.NoValue;
				}
			}
		}

		// Token: 0x17000097 RID: 151
		// (get) Token: 0x060003CE RID: 974 RVA: 0x00011413 File Offset: 0x0000F613
		// (set) Token: 0x060003CF RID: 975 RVA: 0x00011425 File Offset: 0x0000F625
		public object Constant
		{
			get
			{
				if (!this.HasConstant)
				{
					return null;
				}
				return this.constant;
			}
			set
			{
				this.constant = value;
			}
		}

		// Token: 0x17000098 RID: 152
		// (get) Token: 0x060003D0 RID: 976 RVA: 0x0001142E File Offset: 0x0000F62E
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

		// Token: 0x17000099 RID: 153
		// (get) Token: 0x060003D1 RID: 977 RVA: 0x00011453 File Offset: 0x0000F653
		public Collection<CustomAttribute> CustomAttributes
		{
			get
			{
				return this.custom_attributes ?? this.GetCustomAttributes(ref this.custom_attributes, this.Module);
			}
		}

		// Token: 0x1700009A RID: 154
		// (get) Token: 0x060003D2 RID: 978 RVA: 0x00011471 File Offset: 0x0000F671
		public bool HasMarshalInfo
		{
			get
			{
				return this.marshal_info != null || this.GetHasMarshalInfo(this.Module);
			}
		}

		// Token: 0x1700009B RID: 155
		// (get) Token: 0x060003D3 RID: 979 RVA: 0x00011489 File Offset: 0x0000F689
		// (set) Token: 0x060003D4 RID: 980 RVA: 0x000114A7 File Offset: 0x0000F6A7
		public MarshalInfo MarshalInfo
		{
			get
			{
				return this.marshal_info ?? this.GetMarshalInfo(ref this.marshal_info, this.Module);
			}
			set
			{
				this.marshal_info = value;
			}
		}

		// Token: 0x1700009C RID: 156
		// (get) Token: 0x060003D5 RID: 981 RVA: 0x000114B0 File Offset: 0x0000F6B0
		// (set) Token: 0x060003D6 RID: 982 RVA: 0x000114BF File Offset: 0x0000F6BF
		public bool IsCompilerControlled
		{
			get
			{
				return this.attributes.GetMaskedAttributes(7, 0U);
			}
			set
			{
				this.attributes = this.attributes.SetMaskedAttributes(7, 0U, value);
			}
		}

		// Token: 0x1700009D RID: 157
		// (get) Token: 0x060003D7 RID: 983 RVA: 0x000114D5 File Offset: 0x0000F6D5
		// (set) Token: 0x060003D8 RID: 984 RVA: 0x000114E4 File Offset: 0x0000F6E4
		public bool IsPrivate
		{
			get
			{
				return this.attributes.GetMaskedAttributes(7, 1U);
			}
			set
			{
				this.attributes = this.attributes.SetMaskedAttributes(7, 1U, value);
			}
		}

		// Token: 0x1700009E RID: 158
		// (get) Token: 0x060003D9 RID: 985 RVA: 0x000114FA File Offset: 0x0000F6FA
		// (set) Token: 0x060003DA RID: 986 RVA: 0x00011509 File Offset: 0x0000F709
		public bool IsFamilyAndAssembly
		{
			get
			{
				return this.attributes.GetMaskedAttributes(7, 2U);
			}
			set
			{
				this.attributes = this.attributes.SetMaskedAttributes(7, 2U, value);
			}
		}

		// Token: 0x1700009F RID: 159
		// (get) Token: 0x060003DB RID: 987 RVA: 0x0001151F File Offset: 0x0000F71F
		// (set) Token: 0x060003DC RID: 988 RVA: 0x0001152E File Offset: 0x0000F72E
		public bool IsAssembly
		{
			get
			{
				return this.attributes.GetMaskedAttributes(7, 3U);
			}
			set
			{
				this.attributes = this.attributes.SetMaskedAttributes(7, 3U, value);
			}
		}

		// Token: 0x170000A0 RID: 160
		// (get) Token: 0x060003DD RID: 989 RVA: 0x00011544 File Offset: 0x0000F744
		// (set) Token: 0x060003DE RID: 990 RVA: 0x00011553 File Offset: 0x0000F753
		public bool IsFamily
		{
			get
			{
				return this.attributes.GetMaskedAttributes(7, 4U);
			}
			set
			{
				this.attributes = this.attributes.SetMaskedAttributes(7, 4U, value);
			}
		}

		// Token: 0x170000A1 RID: 161
		// (get) Token: 0x060003DF RID: 991 RVA: 0x00011569 File Offset: 0x0000F769
		// (set) Token: 0x060003E0 RID: 992 RVA: 0x00011578 File Offset: 0x0000F778
		public bool IsFamilyOrAssembly
		{
			get
			{
				return this.attributes.GetMaskedAttributes(7, 5U);
			}
			set
			{
				this.attributes = this.attributes.SetMaskedAttributes(7, 5U, value);
			}
		}

		// Token: 0x170000A2 RID: 162
		// (get) Token: 0x060003E1 RID: 993 RVA: 0x0001158E File Offset: 0x0000F78E
		// (set) Token: 0x060003E2 RID: 994 RVA: 0x0001159D File Offset: 0x0000F79D
		public bool IsPublic
		{
			get
			{
				return this.attributes.GetMaskedAttributes(7, 6U);
			}
			set
			{
				this.attributes = this.attributes.SetMaskedAttributes(7, 6U, value);
			}
		}

		// Token: 0x170000A3 RID: 163
		// (get) Token: 0x060003E3 RID: 995 RVA: 0x000115B3 File Offset: 0x0000F7B3
		// (set) Token: 0x060003E4 RID: 996 RVA: 0x000115C2 File Offset: 0x0000F7C2
		public bool IsStatic
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

		// Token: 0x170000A4 RID: 164
		// (get) Token: 0x060003E5 RID: 997 RVA: 0x000115D8 File Offset: 0x0000F7D8
		// (set) Token: 0x060003E6 RID: 998 RVA: 0x000115E7 File Offset: 0x0000F7E7
		public bool IsInitOnly
		{
			get
			{
				return this.attributes.GetAttributes(32);
			}
			set
			{
				this.attributes = this.attributes.SetAttributes(32, value);
			}
		}

		// Token: 0x170000A5 RID: 165
		// (get) Token: 0x060003E7 RID: 999 RVA: 0x000115FD File Offset: 0x0000F7FD
		// (set) Token: 0x060003E8 RID: 1000 RVA: 0x0001160C File Offset: 0x0000F80C
		public bool IsLiteral
		{
			get
			{
				return this.attributes.GetAttributes(64);
			}
			set
			{
				this.attributes = this.attributes.SetAttributes(64, value);
			}
		}

		// Token: 0x170000A6 RID: 166
		// (get) Token: 0x060003E9 RID: 1001 RVA: 0x00011622 File Offset: 0x0000F822
		// (set) Token: 0x060003EA RID: 1002 RVA: 0x00011634 File Offset: 0x0000F834
		public bool IsNotSerialized
		{
			get
			{
				return this.attributes.GetAttributes(128);
			}
			set
			{
				this.attributes = this.attributes.SetAttributes(128, value);
			}
		}

		// Token: 0x170000A7 RID: 167
		// (get) Token: 0x060003EB RID: 1003 RVA: 0x0001164D File Offset: 0x0000F84D
		// (set) Token: 0x060003EC RID: 1004 RVA: 0x0001165F File Offset: 0x0000F85F
		public bool IsSpecialName
		{
			get
			{
				return this.attributes.GetAttributes(512);
			}
			set
			{
				this.attributes = this.attributes.SetAttributes(512, value);
			}
		}

		// Token: 0x170000A8 RID: 168
		// (get) Token: 0x060003ED RID: 1005 RVA: 0x00011678 File Offset: 0x0000F878
		// (set) Token: 0x060003EE RID: 1006 RVA: 0x0001168A File Offset: 0x0000F88A
		public bool IsPInvokeImpl
		{
			get
			{
				return this.attributes.GetAttributes(8192);
			}
			set
			{
				this.attributes = this.attributes.SetAttributes(8192, value);
			}
		}

		// Token: 0x170000A9 RID: 169
		// (get) Token: 0x060003EF RID: 1007 RVA: 0x000116A3 File Offset: 0x0000F8A3
		// (set) Token: 0x060003F0 RID: 1008 RVA: 0x000116B5 File Offset: 0x0000F8B5
		public bool IsRuntimeSpecialName
		{
			get
			{
				return this.attributes.GetAttributes(1024);
			}
			set
			{
				this.attributes = this.attributes.SetAttributes(1024, value);
			}
		}

		// Token: 0x170000AA RID: 170
		// (get) Token: 0x060003F1 RID: 1009 RVA: 0x000116CE File Offset: 0x0000F8CE
		// (set) Token: 0x060003F2 RID: 1010 RVA: 0x000116E0 File Offset: 0x0000F8E0
		public bool HasDefault
		{
			get
			{
				return this.attributes.GetAttributes(32768);
			}
			set
			{
				this.attributes = this.attributes.SetAttributes(32768, value);
			}
		}

		// Token: 0x170000AB RID: 171
		// (get) Token: 0x060003F3 RID: 1011 RVA: 0x00002BE8 File Offset: 0x00000DE8
		public override bool IsDefinition
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170000AC RID: 172
		// (get) Token: 0x060003F4 RID: 1012 RVA: 0x00010BA8 File Offset: 0x0000EDA8
		// (set) Token: 0x060003F5 RID: 1013 RVA: 0x00010BB5 File Offset: 0x0000EDB5
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

		// Token: 0x060003F6 RID: 1014 RVA: 0x000116F9 File Offset: 0x0000F8F9
		public FieldDefinition(string name, FieldAttributes attributes, TypeReference fieldType)
			: base(name, fieldType)
		{
			this.attributes = (ushort)attributes;
		}

		// Token: 0x060003F7 RID: 1015 RVA: 0x00002740 File Offset: 0x00000940
		public override FieldDefinition Resolve()
		{
			return this;
		}

		// Token: 0x040000BD RID: 189
		private ushort attributes;

		// Token: 0x040000BE RID: 190
		private Collection<CustomAttribute> custom_attributes;

		// Token: 0x040000BF RID: 191
		private int offset = -2;

		// Token: 0x040000C0 RID: 192
		internal int rva = -2;

		// Token: 0x040000C1 RID: 193
		private byte[] initial_value;

		// Token: 0x040000C2 RID: 194
		private object constant = Mixin.NotResolved;

		// Token: 0x040000C3 RID: 195
		private MarshalInfo marshal_info;
	}
}
