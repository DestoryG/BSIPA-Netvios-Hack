using System;

namespace Mono.Cecil
{
	// Token: 0x02000059 RID: 89
	public sealed class ExportedType : IMetadataTokenProvider
	{
		// Token: 0x1700006F RID: 111
		// (get) Token: 0x0600037B RID: 891 RVA: 0x00010CBE File Offset: 0x0000EEBE
		// (set) Token: 0x0600037C RID: 892 RVA: 0x00010CC6 File Offset: 0x0000EEC6
		public string Namespace
		{
			get
			{
				return this.@namespace;
			}
			set
			{
				this.@namespace = value;
			}
		}

		// Token: 0x17000070 RID: 112
		// (get) Token: 0x0600037D RID: 893 RVA: 0x00010CCF File Offset: 0x0000EECF
		// (set) Token: 0x0600037E RID: 894 RVA: 0x00010CD7 File Offset: 0x0000EED7
		public string Name
		{
			get
			{
				return this.name;
			}
			set
			{
				this.name = value;
			}
		}

		// Token: 0x17000071 RID: 113
		// (get) Token: 0x0600037F RID: 895 RVA: 0x00010CE0 File Offset: 0x0000EEE0
		// (set) Token: 0x06000380 RID: 896 RVA: 0x00010CE8 File Offset: 0x0000EEE8
		public TypeAttributes Attributes
		{
			get
			{
				return (TypeAttributes)this.attributes;
			}
			set
			{
				this.attributes = (uint)value;
			}
		}

		// Token: 0x17000072 RID: 114
		// (get) Token: 0x06000381 RID: 897 RVA: 0x00010CF1 File Offset: 0x0000EEF1
		// (set) Token: 0x06000382 RID: 898 RVA: 0x00010D0D File Offset: 0x0000EF0D
		public IMetadataScope Scope
		{
			get
			{
				if (this.declaring_type != null)
				{
					return this.declaring_type.Scope;
				}
				return this.scope;
			}
			set
			{
				if (this.declaring_type != null)
				{
					this.declaring_type.Scope = value;
					return;
				}
				this.scope = value;
			}
		}

		// Token: 0x17000073 RID: 115
		// (get) Token: 0x06000383 RID: 899 RVA: 0x00010D2B File Offset: 0x0000EF2B
		// (set) Token: 0x06000384 RID: 900 RVA: 0x00010D33 File Offset: 0x0000EF33
		public ExportedType DeclaringType
		{
			get
			{
				return this.declaring_type;
			}
			set
			{
				this.declaring_type = value;
			}
		}

		// Token: 0x17000074 RID: 116
		// (get) Token: 0x06000385 RID: 901 RVA: 0x00010D3C File Offset: 0x0000EF3C
		// (set) Token: 0x06000386 RID: 902 RVA: 0x00010D44 File Offset: 0x0000EF44
		public MetadataToken MetadataToken
		{
			get
			{
				return this.token;
			}
			set
			{
				this.token = value;
			}
		}

		// Token: 0x17000075 RID: 117
		// (get) Token: 0x06000387 RID: 903 RVA: 0x00010D4D File Offset: 0x0000EF4D
		// (set) Token: 0x06000388 RID: 904 RVA: 0x00010D55 File Offset: 0x0000EF55
		public int Identifier
		{
			get
			{
				return this.identifier;
			}
			set
			{
				this.identifier = value;
			}
		}

		// Token: 0x17000076 RID: 118
		// (get) Token: 0x06000389 RID: 905 RVA: 0x00010D5E File Offset: 0x0000EF5E
		// (set) Token: 0x0600038A RID: 906 RVA: 0x00010D6D File Offset: 0x0000EF6D
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

		// Token: 0x17000077 RID: 119
		// (get) Token: 0x0600038B RID: 907 RVA: 0x00010D83 File Offset: 0x0000EF83
		// (set) Token: 0x0600038C RID: 908 RVA: 0x00010D92 File Offset: 0x0000EF92
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

		// Token: 0x17000078 RID: 120
		// (get) Token: 0x0600038D RID: 909 RVA: 0x00010DA8 File Offset: 0x0000EFA8
		// (set) Token: 0x0600038E RID: 910 RVA: 0x00010DB7 File Offset: 0x0000EFB7
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

		// Token: 0x17000079 RID: 121
		// (get) Token: 0x0600038F RID: 911 RVA: 0x00010DCD File Offset: 0x0000EFCD
		// (set) Token: 0x06000390 RID: 912 RVA: 0x00010DDC File Offset: 0x0000EFDC
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

		// Token: 0x1700007A RID: 122
		// (get) Token: 0x06000391 RID: 913 RVA: 0x00010DF2 File Offset: 0x0000EFF2
		// (set) Token: 0x06000392 RID: 914 RVA: 0x00010E01 File Offset: 0x0000F001
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

		// Token: 0x1700007B RID: 123
		// (get) Token: 0x06000393 RID: 915 RVA: 0x00010E17 File Offset: 0x0000F017
		// (set) Token: 0x06000394 RID: 916 RVA: 0x00010E26 File Offset: 0x0000F026
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

		// Token: 0x1700007C RID: 124
		// (get) Token: 0x06000395 RID: 917 RVA: 0x00010E3C File Offset: 0x0000F03C
		// (set) Token: 0x06000396 RID: 918 RVA: 0x00010E4B File Offset: 0x0000F04B
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

		// Token: 0x1700007D RID: 125
		// (get) Token: 0x06000397 RID: 919 RVA: 0x00010E61 File Offset: 0x0000F061
		// (set) Token: 0x06000398 RID: 920 RVA: 0x00010E70 File Offset: 0x0000F070
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

		// Token: 0x1700007E RID: 126
		// (get) Token: 0x06000399 RID: 921 RVA: 0x00010E86 File Offset: 0x0000F086
		// (set) Token: 0x0600039A RID: 922 RVA: 0x00010E96 File Offset: 0x0000F096
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

		// Token: 0x1700007F RID: 127
		// (get) Token: 0x0600039B RID: 923 RVA: 0x00010EAD File Offset: 0x0000F0AD
		// (set) Token: 0x0600039C RID: 924 RVA: 0x00010EBD File Offset: 0x0000F0BD
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

		// Token: 0x17000080 RID: 128
		// (get) Token: 0x0600039D RID: 925 RVA: 0x00010ED4 File Offset: 0x0000F0D4
		// (set) Token: 0x0600039E RID: 926 RVA: 0x00010EE5 File Offset: 0x0000F0E5
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

		// Token: 0x17000081 RID: 129
		// (get) Token: 0x0600039F RID: 927 RVA: 0x00010EFD File Offset: 0x0000F0FD
		// (set) Token: 0x060003A0 RID: 928 RVA: 0x00010F0D File Offset: 0x0000F10D
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

		// Token: 0x17000082 RID: 130
		// (get) Token: 0x060003A1 RID: 929 RVA: 0x00010F24 File Offset: 0x0000F124
		// (set) Token: 0x060003A2 RID: 930 RVA: 0x00010F35 File Offset: 0x0000F135
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

		// Token: 0x17000083 RID: 131
		// (get) Token: 0x060003A3 RID: 931 RVA: 0x00010F4D File Offset: 0x0000F14D
		// (set) Token: 0x060003A4 RID: 932 RVA: 0x00010F5F File Offset: 0x0000F15F
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

		// Token: 0x17000084 RID: 132
		// (get) Token: 0x060003A5 RID: 933 RVA: 0x00010F78 File Offset: 0x0000F178
		// (set) Token: 0x060003A6 RID: 934 RVA: 0x00010F8A File Offset: 0x0000F18A
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

		// Token: 0x17000085 RID: 133
		// (get) Token: 0x060003A7 RID: 935 RVA: 0x00010FA3 File Offset: 0x0000F1A3
		// (set) Token: 0x060003A8 RID: 936 RVA: 0x00010FB5 File Offset: 0x0000F1B5
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

		// Token: 0x17000086 RID: 134
		// (get) Token: 0x060003A9 RID: 937 RVA: 0x00010FCE File Offset: 0x0000F1CE
		// (set) Token: 0x060003AA RID: 938 RVA: 0x00010FE0 File Offset: 0x0000F1E0
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

		// Token: 0x17000087 RID: 135
		// (get) Token: 0x060003AB RID: 939 RVA: 0x00010FF9 File Offset: 0x0000F1F9
		// (set) Token: 0x060003AC RID: 940 RVA: 0x0001100B File Offset: 0x0000F20B
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

		// Token: 0x17000088 RID: 136
		// (get) Token: 0x060003AD RID: 941 RVA: 0x00011024 File Offset: 0x0000F224
		// (set) Token: 0x060003AE RID: 942 RVA: 0x00011037 File Offset: 0x0000F237
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

		// Token: 0x17000089 RID: 137
		// (get) Token: 0x060003AF RID: 943 RVA: 0x00011051 File Offset: 0x0000F251
		// (set) Token: 0x060003B0 RID: 944 RVA: 0x00011068 File Offset: 0x0000F268
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

		// Token: 0x1700008A RID: 138
		// (get) Token: 0x060003B1 RID: 945 RVA: 0x00011086 File Offset: 0x0000F286
		// (set) Token: 0x060003B2 RID: 946 RVA: 0x0001109D File Offset: 0x0000F29D
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

		// Token: 0x1700008B RID: 139
		// (get) Token: 0x060003B3 RID: 947 RVA: 0x000110BB File Offset: 0x0000F2BB
		// (set) Token: 0x060003B4 RID: 948 RVA: 0x000110CD File Offset: 0x0000F2CD
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

		// Token: 0x1700008C RID: 140
		// (get) Token: 0x060003B5 RID: 949 RVA: 0x000110E6 File Offset: 0x0000F2E6
		// (set) Token: 0x060003B6 RID: 950 RVA: 0x000110F8 File Offset: 0x0000F2F8
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

		// Token: 0x1700008D RID: 141
		// (get) Token: 0x060003B7 RID: 951 RVA: 0x00011111 File Offset: 0x0000F311
		// (set) Token: 0x060003B8 RID: 952 RVA: 0x00011123 File Offset: 0x0000F323
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

		// Token: 0x1700008E RID: 142
		// (get) Token: 0x060003B9 RID: 953 RVA: 0x0001113C File Offset: 0x0000F33C
		// (set) Token: 0x060003BA RID: 954 RVA: 0x0001114E File Offset: 0x0000F34E
		public bool IsForwarder
		{
			get
			{
				return this.attributes.GetAttributes(2097152U);
			}
			set
			{
				this.attributes = this.attributes.SetAttributes(2097152U, value);
			}
		}

		// Token: 0x1700008F RID: 143
		// (get) Token: 0x060003BB RID: 955 RVA: 0x00011168 File Offset: 0x0000F368
		public string FullName
		{
			get
			{
				string text = (string.IsNullOrEmpty(this.@namespace) ? this.name : (this.@namespace + "." + this.name));
				if (this.declaring_type != null)
				{
					return this.declaring_type.FullName + "/" + text;
				}
				return text;
			}
		}

		// Token: 0x060003BC RID: 956 RVA: 0x000111C1 File Offset: 0x0000F3C1
		public ExportedType(string @namespace, string name, ModuleDefinition module, IMetadataScope scope)
		{
			this.@namespace = @namespace;
			this.name = name;
			this.scope = scope;
			this.module = module;
		}

		// Token: 0x060003BD RID: 957 RVA: 0x000111E6 File Offset: 0x0000F3E6
		public override string ToString()
		{
			return this.FullName;
		}

		// Token: 0x060003BE RID: 958 RVA: 0x000111EE File Offset: 0x0000F3EE
		public TypeDefinition Resolve()
		{
			return this.module.Resolve(this.CreateReference());
		}

		// Token: 0x060003BF RID: 959 RVA: 0x00011201 File Offset: 0x0000F401
		internal TypeReference CreateReference()
		{
			return new TypeReference(this.@namespace, this.name, this.module, this.scope)
			{
				DeclaringType = ((this.declaring_type != null) ? this.declaring_type.CreateReference() : null)
			};
		}

		// Token: 0x040000A2 RID: 162
		private string @namespace;

		// Token: 0x040000A3 RID: 163
		private string name;

		// Token: 0x040000A4 RID: 164
		private uint attributes;

		// Token: 0x040000A5 RID: 165
		private IMetadataScope scope;

		// Token: 0x040000A6 RID: 166
		private ModuleDefinition module;

		// Token: 0x040000A7 RID: 167
		private int identifier;

		// Token: 0x040000A8 RID: 168
		private ExportedType declaring_type;

		// Token: 0x040000A9 RID: 169
		internal MetadataToken token;
	}
}
