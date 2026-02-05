using System;

namespace Mono.Cecil
{
	// Token: 0x0200010B RID: 267
	internal sealed class ExportedType : IMetadataTokenProvider
	{
		// Token: 0x17000085 RID: 133
		// (get) Token: 0x060006F5 RID: 1781 RVA: 0x0001F2D7 File Offset: 0x0001D4D7
		// (set) Token: 0x060006F6 RID: 1782 RVA: 0x0001F2DF File Offset: 0x0001D4DF
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

		// Token: 0x17000086 RID: 134
		// (get) Token: 0x060006F7 RID: 1783 RVA: 0x0001F2E8 File Offset: 0x0001D4E8
		// (set) Token: 0x060006F8 RID: 1784 RVA: 0x0001F2F0 File Offset: 0x0001D4F0
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

		// Token: 0x17000087 RID: 135
		// (get) Token: 0x060006F9 RID: 1785 RVA: 0x0001F2F9 File Offset: 0x0001D4F9
		// (set) Token: 0x060006FA RID: 1786 RVA: 0x0001F301 File Offset: 0x0001D501
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

		// Token: 0x17000088 RID: 136
		// (get) Token: 0x060006FB RID: 1787 RVA: 0x0001F30A File Offset: 0x0001D50A
		// (set) Token: 0x060006FC RID: 1788 RVA: 0x0001F326 File Offset: 0x0001D526
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

		// Token: 0x17000089 RID: 137
		// (get) Token: 0x060006FD RID: 1789 RVA: 0x0001F344 File Offset: 0x0001D544
		// (set) Token: 0x060006FE RID: 1790 RVA: 0x0001F34C File Offset: 0x0001D54C
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

		// Token: 0x1700008A RID: 138
		// (get) Token: 0x060006FF RID: 1791 RVA: 0x0001F355 File Offset: 0x0001D555
		// (set) Token: 0x06000700 RID: 1792 RVA: 0x0001F35D File Offset: 0x0001D55D
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

		// Token: 0x1700008B RID: 139
		// (get) Token: 0x06000701 RID: 1793 RVA: 0x0001F366 File Offset: 0x0001D566
		// (set) Token: 0x06000702 RID: 1794 RVA: 0x0001F36E File Offset: 0x0001D56E
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

		// Token: 0x1700008C RID: 140
		// (get) Token: 0x06000703 RID: 1795 RVA: 0x0001F377 File Offset: 0x0001D577
		// (set) Token: 0x06000704 RID: 1796 RVA: 0x0001F386 File Offset: 0x0001D586
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

		// Token: 0x1700008D RID: 141
		// (get) Token: 0x06000705 RID: 1797 RVA: 0x0001F39C File Offset: 0x0001D59C
		// (set) Token: 0x06000706 RID: 1798 RVA: 0x0001F3AB File Offset: 0x0001D5AB
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

		// Token: 0x1700008E RID: 142
		// (get) Token: 0x06000707 RID: 1799 RVA: 0x0001F3C1 File Offset: 0x0001D5C1
		// (set) Token: 0x06000708 RID: 1800 RVA: 0x0001F3D0 File Offset: 0x0001D5D0
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

		// Token: 0x1700008F RID: 143
		// (get) Token: 0x06000709 RID: 1801 RVA: 0x0001F3E6 File Offset: 0x0001D5E6
		// (set) Token: 0x0600070A RID: 1802 RVA: 0x0001F3F5 File Offset: 0x0001D5F5
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

		// Token: 0x17000090 RID: 144
		// (get) Token: 0x0600070B RID: 1803 RVA: 0x0001F40B File Offset: 0x0001D60B
		// (set) Token: 0x0600070C RID: 1804 RVA: 0x0001F41A File Offset: 0x0001D61A
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

		// Token: 0x17000091 RID: 145
		// (get) Token: 0x0600070D RID: 1805 RVA: 0x0001F430 File Offset: 0x0001D630
		// (set) Token: 0x0600070E RID: 1806 RVA: 0x0001F43F File Offset: 0x0001D63F
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

		// Token: 0x17000092 RID: 146
		// (get) Token: 0x0600070F RID: 1807 RVA: 0x0001F455 File Offset: 0x0001D655
		// (set) Token: 0x06000710 RID: 1808 RVA: 0x0001F464 File Offset: 0x0001D664
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

		// Token: 0x17000093 RID: 147
		// (get) Token: 0x06000711 RID: 1809 RVA: 0x0001F47A File Offset: 0x0001D67A
		// (set) Token: 0x06000712 RID: 1810 RVA: 0x0001F489 File Offset: 0x0001D689
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

		// Token: 0x17000094 RID: 148
		// (get) Token: 0x06000713 RID: 1811 RVA: 0x0001F49F File Offset: 0x0001D69F
		// (set) Token: 0x06000714 RID: 1812 RVA: 0x0001F4AF File Offset: 0x0001D6AF
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

		// Token: 0x17000095 RID: 149
		// (get) Token: 0x06000715 RID: 1813 RVA: 0x0001F4C6 File Offset: 0x0001D6C6
		// (set) Token: 0x06000716 RID: 1814 RVA: 0x0001F4D6 File Offset: 0x0001D6D6
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

		// Token: 0x17000096 RID: 150
		// (get) Token: 0x06000717 RID: 1815 RVA: 0x0001F4ED File Offset: 0x0001D6ED
		// (set) Token: 0x06000718 RID: 1816 RVA: 0x0001F4FE File Offset: 0x0001D6FE
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

		// Token: 0x17000097 RID: 151
		// (get) Token: 0x06000719 RID: 1817 RVA: 0x0001F516 File Offset: 0x0001D716
		// (set) Token: 0x0600071A RID: 1818 RVA: 0x0001F526 File Offset: 0x0001D726
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

		// Token: 0x17000098 RID: 152
		// (get) Token: 0x0600071B RID: 1819 RVA: 0x0001F53D File Offset: 0x0001D73D
		// (set) Token: 0x0600071C RID: 1820 RVA: 0x0001F54E File Offset: 0x0001D74E
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

		// Token: 0x17000099 RID: 153
		// (get) Token: 0x0600071D RID: 1821 RVA: 0x0001F566 File Offset: 0x0001D766
		// (set) Token: 0x0600071E RID: 1822 RVA: 0x0001F578 File Offset: 0x0001D778
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

		// Token: 0x1700009A RID: 154
		// (get) Token: 0x0600071F RID: 1823 RVA: 0x0001F591 File Offset: 0x0001D791
		// (set) Token: 0x06000720 RID: 1824 RVA: 0x0001F5A3 File Offset: 0x0001D7A3
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

		// Token: 0x1700009B RID: 155
		// (get) Token: 0x06000721 RID: 1825 RVA: 0x0001F5BC File Offset: 0x0001D7BC
		// (set) Token: 0x06000722 RID: 1826 RVA: 0x0001F5CE File Offset: 0x0001D7CE
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

		// Token: 0x1700009C RID: 156
		// (get) Token: 0x06000723 RID: 1827 RVA: 0x0001F5E7 File Offset: 0x0001D7E7
		// (set) Token: 0x06000724 RID: 1828 RVA: 0x0001F5F9 File Offset: 0x0001D7F9
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

		// Token: 0x1700009D RID: 157
		// (get) Token: 0x06000725 RID: 1829 RVA: 0x0001F612 File Offset: 0x0001D812
		// (set) Token: 0x06000726 RID: 1830 RVA: 0x0001F624 File Offset: 0x0001D824
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

		// Token: 0x1700009E RID: 158
		// (get) Token: 0x06000727 RID: 1831 RVA: 0x0001F63D File Offset: 0x0001D83D
		// (set) Token: 0x06000728 RID: 1832 RVA: 0x0001F650 File Offset: 0x0001D850
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

		// Token: 0x1700009F RID: 159
		// (get) Token: 0x06000729 RID: 1833 RVA: 0x0001F66A File Offset: 0x0001D86A
		// (set) Token: 0x0600072A RID: 1834 RVA: 0x0001F681 File Offset: 0x0001D881
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

		// Token: 0x170000A0 RID: 160
		// (get) Token: 0x0600072B RID: 1835 RVA: 0x0001F69F File Offset: 0x0001D89F
		// (set) Token: 0x0600072C RID: 1836 RVA: 0x0001F6B6 File Offset: 0x0001D8B6
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

		// Token: 0x170000A1 RID: 161
		// (get) Token: 0x0600072D RID: 1837 RVA: 0x0001F6D4 File Offset: 0x0001D8D4
		// (set) Token: 0x0600072E RID: 1838 RVA: 0x0001F6E6 File Offset: 0x0001D8E6
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

		// Token: 0x170000A2 RID: 162
		// (get) Token: 0x0600072F RID: 1839 RVA: 0x0001F6FF File Offset: 0x0001D8FF
		// (set) Token: 0x06000730 RID: 1840 RVA: 0x0001F711 File Offset: 0x0001D911
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

		// Token: 0x170000A3 RID: 163
		// (get) Token: 0x06000731 RID: 1841 RVA: 0x0001F72A File Offset: 0x0001D92A
		// (set) Token: 0x06000732 RID: 1842 RVA: 0x0001F73C File Offset: 0x0001D93C
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

		// Token: 0x170000A4 RID: 164
		// (get) Token: 0x06000733 RID: 1843 RVA: 0x0001F755 File Offset: 0x0001D955
		// (set) Token: 0x06000734 RID: 1844 RVA: 0x0001F767 File Offset: 0x0001D967
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

		// Token: 0x170000A5 RID: 165
		// (get) Token: 0x06000735 RID: 1845 RVA: 0x0001F780 File Offset: 0x0001D980
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

		// Token: 0x06000736 RID: 1846 RVA: 0x0001F7D9 File Offset: 0x0001D9D9
		public ExportedType(string @namespace, string name, ModuleDefinition module, IMetadataScope scope)
		{
			this.@namespace = @namespace;
			this.name = name;
			this.scope = scope;
			this.module = module;
		}

		// Token: 0x06000737 RID: 1847 RVA: 0x0001F7FE File Offset: 0x0001D9FE
		public override string ToString()
		{
			return this.FullName;
		}

		// Token: 0x06000738 RID: 1848 RVA: 0x0001F806 File Offset: 0x0001DA06
		public TypeDefinition Resolve()
		{
			return this.module.Resolve(this.CreateReference());
		}

		// Token: 0x06000739 RID: 1849 RVA: 0x0001F819 File Offset: 0x0001DA19
		internal TypeReference CreateReference()
		{
			return new TypeReference(this.@namespace, this.name, this.module, this.scope)
			{
				DeclaringType = ((this.declaring_type != null) ? this.declaring_type.CreateReference() : null)
			};
		}

		// Token: 0x040002AE RID: 686
		private string @namespace;

		// Token: 0x040002AF RID: 687
		private string name;

		// Token: 0x040002B0 RID: 688
		private uint attributes;

		// Token: 0x040002B1 RID: 689
		private IMetadataScope scope;

		// Token: 0x040002B2 RID: 690
		private ModuleDefinition module;

		// Token: 0x040002B3 RID: 691
		private int identifier;

		// Token: 0x040002B4 RID: 692
		private ExportedType declaring_type;

		// Token: 0x040002B5 RID: 693
		internal MetadataToken token;
	}
}
