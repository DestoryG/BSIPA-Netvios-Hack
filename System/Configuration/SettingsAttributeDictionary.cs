using System;
using System.Collections;

namespace System.Configuration
{
	// Token: 0x02000098 RID: 152
	[Serializable]
	public class SettingsAttributeDictionary : Hashtable
	{
		// Token: 0x06000595 RID: 1429 RVA: 0x000229ED File Offset: 0x00020BED
		public SettingsAttributeDictionary()
		{
		}

		// Token: 0x06000596 RID: 1430 RVA: 0x000229F5 File Offset: 0x00020BF5
		public SettingsAttributeDictionary(SettingsAttributeDictionary attributes)
			: base(attributes)
		{
		}
	}
}
