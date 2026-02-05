using System;

namespace Mono.Cecil
{
	// Token: 0x020000AB RID: 171
	public abstract class Resource
	{
		// Token: 0x1700021E RID: 542
		// (get) Token: 0x06000761 RID: 1889 RVA: 0x000172DC File Offset: 0x000154DC
		// (set) Token: 0x06000762 RID: 1890 RVA: 0x000172E4 File Offset: 0x000154E4
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

		// Token: 0x1700021F RID: 543
		// (get) Token: 0x06000763 RID: 1891 RVA: 0x000172ED File Offset: 0x000154ED
		// (set) Token: 0x06000764 RID: 1892 RVA: 0x000172F5 File Offset: 0x000154F5
		public ManifestResourceAttributes Attributes
		{
			get
			{
				return (ManifestResourceAttributes)this.attributes;
			}
			set
			{
				this.attributes = (uint)value;
			}
		}

		// Token: 0x17000220 RID: 544
		// (get) Token: 0x06000765 RID: 1893
		public abstract ResourceType ResourceType { get; }

		// Token: 0x17000221 RID: 545
		// (get) Token: 0x06000766 RID: 1894 RVA: 0x000172FE File Offset: 0x000154FE
		// (set) Token: 0x06000767 RID: 1895 RVA: 0x0001730D File Offset: 0x0001550D
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

		// Token: 0x17000222 RID: 546
		// (get) Token: 0x06000768 RID: 1896 RVA: 0x00017323 File Offset: 0x00015523
		// (set) Token: 0x06000769 RID: 1897 RVA: 0x00017332 File Offset: 0x00015532
		public bool IsPrivate
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

		// Token: 0x0600076A RID: 1898 RVA: 0x00017348 File Offset: 0x00015548
		internal Resource(string name, ManifestResourceAttributes attributes)
		{
			this.name = name;
			this.attributes = (uint)attributes;
		}

		// Token: 0x04000232 RID: 562
		private string name;

		// Token: 0x04000233 RID: 563
		private uint attributes;
	}
}
