using System;
using System.Xml;

namespace System.Configuration
{
	// Token: 0x02000080 RID: 128
	internal struct StoredSetting
	{
		// Token: 0x06000510 RID: 1296 RVA: 0x00021198 File Offset: 0x0001F398
		internal StoredSetting(SettingsSerializeAs serializeAs, XmlNode value)
		{
			this.SerializeAs = serializeAs;
			this.Value = value;
		}

		// Token: 0x04000C18 RID: 3096
		internal SettingsSerializeAs SerializeAs;

		// Token: 0x04000C19 RID: 3097
		internal XmlNode Value;
	}
}
