using System;
using System.Configuration;

namespace System.Diagnostics
{
	// Token: 0x020004A8 RID: 1192
	[ConfigurationCollection(typeof(SwitchElement))]
	internal class SwitchElementsCollection : ConfigurationElementCollection
	{
		// Token: 0x17000AB0 RID: 2736
		public SwitchElement this[string name]
		{
			get
			{
				return (SwitchElement)base.BaseGet(name);
			}
		}

		// Token: 0x17000AB1 RID: 2737
		// (get) Token: 0x06002C24 RID: 11300 RVA: 0x000C7348 File Offset: 0x000C5548
		public override ConfigurationElementCollectionType CollectionType
		{
			get
			{
				return ConfigurationElementCollectionType.AddRemoveClearMap;
			}
		}

		// Token: 0x06002C25 RID: 11301 RVA: 0x000C734B File Offset: 0x000C554B
		protected override ConfigurationElement CreateNewElement()
		{
			return new SwitchElement();
		}

		// Token: 0x06002C26 RID: 11302 RVA: 0x000C7352 File Offset: 0x000C5552
		protected override object GetElementKey(ConfigurationElement element)
		{
			return ((SwitchElement)element).Name;
		}
	}
}
