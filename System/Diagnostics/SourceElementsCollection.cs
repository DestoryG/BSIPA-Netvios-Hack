using System;
using System.Configuration;

namespace System.Diagnostics
{
	// Token: 0x020004A1 RID: 1185
	[ConfigurationCollection(typeof(SourceElement), AddItemName = "source", CollectionType = ConfigurationElementCollectionType.BasicMap)]
	internal class SourceElementsCollection : ConfigurationElementCollection
	{
		// Token: 0x17000A9C RID: 2716
		public SourceElement this[string name]
		{
			get
			{
				return (SourceElement)base.BaseGet(name);
			}
		}

		// Token: 0x17000A9D RID: 2717
		// (get) Token: 0x06002BE8 RID: 11240 RVA: 0x000C6843 File Offset: 0x000C4A43
		protected override string ElementName
		{
			get
			{
				return "source";
			}
		}

		// Token: 0x17000A9E RID: 2718
		// (get) Token: 0x06002BE9 RID: 11241 RVA: 0x000C684A File Offset: 0x000C4A4A
		public override ConfigurationElementCollectionType CollectionType
		{
			get
			{
				return ConfigurationElementCollectionType.BasicMap;
			}
		}

		// Token: 0x06002BEA RID: 11242 RVA: 0x000C6850 File Offset: 0x000C4A50
		protected override ConfigurationElement CreateNewElement()
		{
			SourceElement sourceElement = new SourceElement();
			sourceElement.Listeners.InitializeDefaultInternal();
			return sourceElement;
		}

		// Token: 0x06002BEB RID: 11243 RVA: 0x000C686F File Offset: 0x000C4A6F
		protected override object GetElementKey(ConfigurationElement element)
		{
			return ((SourceElement)element).Name;
		}
	}
}
