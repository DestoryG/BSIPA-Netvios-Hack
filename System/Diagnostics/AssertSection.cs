using System;
using System.Configuration;

namespace System.Diagnostics
{
	// Token: 0x02000490 RID: 1168
	internal class AssertSection : ConfigurationElement
	{
		// Token: 0x06002B48 RID: 11080 RVA: 0x000C4A84 File Offset: 0x000C2C84
		static AssertSection()
		{
			AssertSection._properties.Add(AssertSection._propAssertUIEnabled);
			AssertSection._properties.Add(AssertSection._propLogFile);
		}

		// Token: 0x17000A75 RID: 2677
		// (get) Token: 0x06002B49 RID: 11081 RVA: 0x000C4AF8 File Offset: 0x000C2CF8
		[ConfigurationProperty("assertuienabled", DefaultValue = true)]
		public bool AssertUIEnabled
		{
			get
			{
				return (bool)base[AssertSection._propAssertUIEnabled];
			}
		}

		// Token: 0x17000A76 RID: 2678
		// (get) Token: 0x06002B4A RID: 11082 RVA: 0x000C4B0A File Offset: 0x000C2D0A
		[ConfigurationProperty("logfilename", DefaultValue = "")]
		public string LogFileName
		{
			get
			{
				return (string)base[AssertSection._propLogFile];
			}
		}

		// Token: 0x17000A77 RID: 2679
		// (get) Token: 0x06002B4B RID: 11083 RVA: 0x000C4B1C File Offset: 0x000C2D1C
		protected override ConfigurationPropertyCollection Properties
		{
			get
			{
				return AssertSection._properties;
			}
		}

		// Token: 0x04002673 RID: 9843
		private static readonly ConfigurationPropertyCollection _properties = new ConfigurationPropertyCollection();

		// Token: 0x04002674 RID: 9844
		private static readonly ConfigurationProperty _propAssertUIEnabled = new ConfigurationProperty("assertuienabled", typeof(bool), true, ConfigurationPropertyOptions.None);

		// Token: 0x04002675 RID: 9845
		private static readonly ConfigurationProperty _propLogFile = new ConfigurationProperty("logfilename", typeof(string), string.Empty, ConfigurationPropertyOptions.None);
	}
}
