using System;
using Mono.Collections.Generic;

namespace Mono.Cecil
{
	// Token: 0x0200010D RID: 269
	internal sealed class FieldDefinition : FieldReference, IMemberDefinition, ICustomAttributeProvider, IMetadataTokenProvider, IConstantProvider, IMarshalInfoProvider
	{
		// Token: 0x0600073A RID: 1850 RVA: 0x0001F854 File Offset: 0x0001DA54
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
			object syncRoot = this.Module.SyncRoot;
			lock (syncRoot)
			{
				if (this.offset == -2)
				{
					this.offset = this.Module.Read<FieldDefinition, int>(this, (FieldDefinition field, MetadataReader reader) => reader.ReadFieldLayout(field));
				}
			}
		}

		// Token: 0x170000A6 RID: 166
		// (get) Token: 0x0600073B RID: 1851 RVA: 0x0001F8EC File Offset: 0x0001DAEC
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

		// Token: 0x170000A7 RID: 167
		// (get) Token: 0x0600073C RID: 1852 RVA: 0x0001F90B File Offset: 0x0001DB0B
		// (set) Token: 0x0600073D RID: 1853 RVA: 0x0001F934 File Offset: 0x0001DB34
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

		// Token: 0x170000A8 RID: 168
		// (get) Token: 0x0600073E RID: 1854 RVA: 0x0001F93D File Offset: 0x0001DB3D
		// (set) Token: 0x0600073F RID: 1855 RVA: 0x0001F94A File Offset: 0x0001DB4A
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

		// Token: 0x06000740 RID: 1856 RVA: 0x0001F954 File Offset: 0x0001DB54
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
			object syncRoot = this.Module.SyncRoot;
			lock (syncRoot)
			{
				if (this.rva == -2)
				{
					this.rva = this.Module.Read<FieldDefinition, int>(this, (FieldDefinition field, MetadataReader reader) => reader.ReadFieldRVA(field));
				}
			}
		}

		// Token: 0x170000A9 RID: 169
		// (get) Token: 0x06000741 RID: 1857 RVA: 0x0001F9E4 File Offset: 0x0001DBE4
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

		// Token: 0x170000AA RID: 170
		// (get) Token: 0x06000742 RID: 1858 RVA: 0x0001FA0D File Offset: 0x0001DC0D
		// (set) Token: 0x06000743 RID: 1859 RVA: 0x0001FA3D File Offset: 0x0001DC3D
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

		// Token: 0x170000AB RID: 171
		// (get) Token: 0x06000744 RID: 1860 RVA: 0x0001FA4D File Offset: 0x0001DC4D
		// (set) Token: 0x06000745 RID: 1861 RVA: 0x0001FA55 File Offset: 0x0001DC55
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

		// Token: 0x170000AC RID: 172
		// (get) Token: 0x06000746 RID: 1862 RVA: 0x0001FA75 File Offset: 0x0001DC75
		// (set) Token: 0x06000747 RID: 1863 RVA: 0x0001FA99 File Offset: 0x0001DC99
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

		// Token: 0x170000AD RID: 173
		// (get) Token: 0x06000748 RID: 1864 RVA: 0x0001FAA9 File Offset: 0x0001DCA9
		// (set) Token: 0x06000749 RID: 1865 RVA: 0x0001FABB File Offset: 0x0001DCBB
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

		// Token: 0x170000AE RID: 174
		// (get) Token: 0x0600074A RID: 1866 RVA: 0x0001FAC4 File Offset: 0x0001DCC4
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

		// Token: 0x170000AF RID: 175
		// (get) Token: 0x0600074B RID: 1867 RVA: 0x0001FAE9 File Offset: 0x0001DCE9
		public Collection<CustomAttribute> CustomAttributes
		{
			get
			{
				return this.custom_attributes ?? this.GetCustomAttributes(ref this.custom_attributes, this.Module);
			}
		}

		// Token: 0x170000B0 RID: 176
		// (get) Token: 0x0600074C RID: 1868 RVA: 0x0001FB07 File Offset: 0x0001DD07
		public bool HasMarshalInfo
		{
			get
			{
				return this.marshal_info != null || this.GetHasMarshalInfo(this.Module);
			}
		}

		// Token: 0x170000B1 RID: 177
		// (get) Token: 0x0600074D RID: 1869 RVA: 0x0001FB1F File Offset: 0x0001DD1F
		// (set) Token: 0x0600074E RID: 1870 RVA: 0x0001FB3D File Offset: 0x0001DD3D
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

		// Token: 0x170000B2 RID: 178
		// (get) Token: 0x0600074F RID: 1871 RVA: 0x0001FB46 File Offset: 0x0001DD46
		// (set) Token: 0x06000750 RID: 1872 RVA: 0x0001FB55 File Offset: 0x0001DD55
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

		// Token: 0x170000B3 RID: 179
		// (get) Token: 0x06000751 RID: 1873 RVA: 0x0001FB6B File Offset: 0x0001DD6B
		// (set) Token: 0x06000752 RID: 1874 RVA: 0x0001FB7A File Offset: 0x0001DD7A
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

		// Token: 0x170000B4 RID: 180
		// (get) Token: 0x06000753 RID: 1875 RVA: 0x0001FB90 File Offset: 0x0001DD90
		// (set) Token: 0x06000754 RID: 1876 RVA: 0x0001FB9F File Offset: 0x0001DD9F
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

		// Token: 0x170000B5 RID: 181
		// (get) Token: 0x06000755 RID: 1877 RVA: 0x0001FBB5 File Offset: 0x0001DDB5
		// (set) Token: 0x06000756 RID: 1878 RVA: 0x0001FBC4 File Offset: 0x0001DDC4
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

		// Token: 0x170000B6 RID: 182
		// (get) Token: 0x06000757 RID: 1879 RVA: 0x0001FBDA File Offset: 0x0001DDDA
		// (set) Token: 0x06000758 RID: 1880 RVA: 0x0001FBE9 File Offset: 0x0001DDE9
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

		// Token: 0x170000B7 RID: 183
		// (get) Token: 0x06000759 RID: 1881 RVA: 0x0001FBFF File Offset: 0x0001DDFF
		// (set) Token: 0x0600075A RID: 1882 RVA: 0x0001FC0E File Offset: 0x0001DE0E
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

		// Token: 0x170000B8 RID: 184
		// (get) Token: 0x0600075B RID: 1883 RVA: 0x0001FC24 File Offset: 0x0001DE24
		// (set) Token: 0x0600075C RID: 1884 RVA: 0x0001FC33 File Offset: 0x0001DE33
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

		// Token: 0x170000B9 RID: 185
		// (get) Token: 0x0600075D RID: 1885 RVA: 0x0001FC49 File Offset: 0x0001DE49
		// (set) Token: 0x0600075E RID: 1886 RVA: 0x0001FC58 File Offset: 0x0001DE58
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

		// Token: 0x170000BA RID: 186
		// (get) Token: 0x0600075F RID: 1887 RVA: 0x0001FC6E File Offset: 0x0001DE6E
		// (set) Token: 0x06000760 RID: 1888 RVA: 0x0001FC7D File Offset: 0x0001DE7D
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

		// Token: 0x170000BB RID: 187
		// (get) Token: 0x06000761 RID: 1889 RVA: 0x0001FC93 File Offset: 0x0001DE93
		// (set) Token: 0x06000762 RID: 1890 RVA: 0x0001FCA2 File Offset: 0x0001DEA2
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

		// Token: 0x170000BC RID: 188
		// (get) Token: 0x06000763 RID: 1891 RVA: 0x0001FCB8 File Offset: 0x0001DEB8
		// (set) Token: 0x06000764 RID: 1892 RVA: 0x0001FCCA File Offset: 0x0001DECA
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

		// Token: 0x170000BD RID: 189
		// (get) Token: 0x06000765 RID: 1893 RVA: 0x0001FCE3 File Offset: 0x0001DEE3
		// (set) Token: 0x06000766 RID: 1894 RVA: 0x0001FCF5 File Offset: 0x0001DEF5
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

		// Token: 0x170000BE RID: 190
		// (get) Token: 0x06000767 RID: 1895 RVA: 0x0001FD0E File Offset: 0x0001DF0E
		// (set) Token: 0x06000768 RID: 1896 RVA: 0x0001FD20 File Offset: 0x0001DF20
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

		// Token: 0x170000BF RID: 191
		// (get) Token: 0x06000769 RID: 1897 RVA: 0x0001FD39 File Offset: 0x0001DF39
		// (set) Token: 0x0600076A RID: 1898 RVA: 0x0001FD4B File Offset: 0x0001DF4B
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

		// Token: 0x170000C0 RID: 192
		// (get) Token: 0x0600076B RID: 1899 RVA: 0x0001FD64 File Offset: 0x0001DF64
		// (set) Token: 0x0600076C RID: 1900 RVA: 0x0001FD76 File Offset: 0x0001DF76
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

		// Token: 0x170000C1 RID: 193
		// (get) Token: 0x0600076D RID: 1901 RVA: 0x00010F39 File Offset: 0x0000F139
		public override bool IsDefinition
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170000C2 RID: 194
		// (get) Token: 0x0600076E RID: 1902 RVA: 0x0001F1AC File Offset: 0x0001D3AC
		// (set) Token: 0x0600076F RID: 1903 RVA: 0x0001F1B9 File Offset: 0x0001D3B9
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

		// Token: 0x06000770 RID: 1904 RVA: 0x0001FD8F File Offset: 0x0001DF8F
		public FieldDefinition(string name, FieldAttributes attributes, TypeReference fieldType)
			: base(name, fieldType)
		{
			this.attributes = (ushort)attributes;
		}

		// Token: 0x06000771 RID: 1905 RVA: 0x00010978 File Offset: 0x0000EB78
		public override FieldDefinition Resolve()
		{
			return this;
		}

		// Token: 0x040002C9 RID: 713
		private ushort attributes;

		// Token: 0x040002CA RID: 714
		private Collection<CustomAttribute> custom_attributes;

		// Token: 0x040002CB RID: 715
		private int offset = -2;

		// Token: 0x040002CC RID: 716
		internal int rva = -2;

		// Token: 0x040002CD RID: 717
		private byte[] initial_value;

		// Token: 0x040002CE RID: 718
		private object constant = Mixin.NotResolved;

		// Token: 0x040002CF RID: 719
		private MarshalInfo marshal_info;
	}
}
