using System;
using System.Configuration;

namespace System.Diagnostics
{
	// Token: 0x0200049D RID: 1181
	[ConfigurationCollection(typeof(ListenerElement), AddItemName = "add", CollectionType = ConfigurationElementCollectionType.BasicMap)]
	internal class SharedListenerElementsCollection : ListenerElementsCollection
	{
		// Token: 0x17000A92 RID: 2706
		// (get) Token: 0x06002BC8 RID: 11208 RVA: 0x000C6054 File Offset: 0x000C4254
		public override ConfigurationElementCollectionType CollectionType
		{
			get
			{
				return ConfigurationElementCollectionType.BasicMap;
			}
		}

		// Token: 0x06002BC9 RID: 11209 RVA: 0x000C6057 File Offset: 0x000C4257
		protected override ConfigurationElement CreateNewElement()
		{
			return new ListenerElement(false);
		}

		// Token: 0x17000A93 RID: 2707
		// (get) Token: 0x06002BCA RID: 11210 RVA: 0x000C605F File Offset: 0x000C425F
		protected override string ElementName
		{
			get
			{
				return "add";
			}
		}
	}
}
