using System;
using System.Configuration;

namespace System.Diagnostics
{
	// Token: 0x0200049F RID: 1183
	internal class PerfCounterSection : ConfigurationElement
	{
		// Token: 0x06002BDF RID: 11231 RVA: 0x000C67A8 File Offset: 0x000C49A8
		static PerfCounterSection()
		{
			PerfCounterSection._properties.Add(PerfCounterSection._propFileMappingSize);
		}

		// Token: 0x17000A99 RID: 2713
		// (get) Token: 0x06002BE0 RID: 11232 RVA: 0x000C67E7 File Offset: 0x000C49E7
		[ConfigurationProperty("filemappingsize", DefaultValue = 524288)]
		public int FileMappingSize
		{
			get
			{
				return (int)base[PerfCounterSection._propFileMappingSize];
			}
		}

		// Token: 0x17000A9A RID: 2714
		// (get) Token: 0x06002BE1 RID: 11233 RVA: 0x000C67F9 File Offset: 0x000C49F9
		protected override ConfigurationPropertyCollection Properties
		{
			get
			{
				return PerfCounterSection._properties;
			}
		}

		// Token: 0x04002691 RID: 9873
		private static readonly ConfigurationPropertyCollection _properties = new ConfigurationPropertyCollection();

		// Token: 0x04002692 RID: 9874
		private static readonly ConfigurationProperty _propFileMappingSize = new ConfigurationProperty("filemappingsize", typeof(int), 524288, ConfigurationPropertyOptions.None);
	}
}
