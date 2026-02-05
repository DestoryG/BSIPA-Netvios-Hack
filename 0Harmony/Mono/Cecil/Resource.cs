using System;

namespace Mono.Cecil
{
	// Token: 0x02000166 RID: 358
	internal abstract class Resource
	{
		// Token: 0x1700023E RID: 574
		// (get) Token: 0x06000B1B RID: 2843 RVA: 0x00025EBD File Offset: 0x000240BD
		// (set) Token: 0x06000B1C RID: 2844 RVA: 0x00025EC5 File Offset: 0x000240C5
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

		// Token: 0x1700023F RID: 575
		// (get) Token: 0x06000B1D RID: 2845 RVA: 0x00025ECE File Offset: 0x000240CE
		// (set) Token: 0x06000B1E RID: 2846 RVA: 0x00025ED6 File Offset: 0x000240D6
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

		// Token: 0x17000240 RID: 576
		// (get) Token: 0x06000B1F RID: 2847
		public abstract ResourceType ResourceType { get; }

		// Token: 0x17000241 RID: 577
		// (get) Token: 0x06000B20 RID: 2848 RVA: 0x00025EDF File Offset: 0x000240DF
		// (set) Token: 0x06000B21 RID: 2849 RVA: 0x00025EEE File Offset: 0x000240EE
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

		// Token: 0x17000242 RID: 578
		// (get) Token: 0x06000B22 RID: 2850 RVA: 0x00025F04 File Offset: 0x00024104
		// (set) Token: 0x06000B23 RID: 2851 RVA: 0x00025F13 File Offset: 0x00024113
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

		// Token: 0x06000B24 RID: 2852 RVA: 0x00025F29 File Offset: 0x00024129
		internal Resource(string name, ManifestResourceAttributes attributes)
		{
			this.name = name;
			this.attributes = (uint)attributes;
		}

		// Token: 0x0400046C RID: 1132
		private string name;

		// Token: 0x0400046D RID: 1133
		private uint attributes;
	}
}
