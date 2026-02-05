using System;

namespace System.Configuration
{
	// Token: 0x02000072 RID: 114
	[ConfigurationCollection(typeof(SchemeSettingElement), CollectionType = ConfigurationElementCollectionType.AddRemoveClearMap, AddItemName = "add", ClearItemsName = "clear", RemoveItemName = "remove")]
	public sealed class SchemeSettingElementCollection : ConfigurationElementCollection
	{
		// Token: 0x06000499 RID: 1177 RVA: 0x0001F516 File Offset: 0x0001D716
		public SchemeSettingElementCollection()
		{
			base.AddElementName = "add";
			base.ClearElementName = "clear";
			base.RemoveElementName = "remove";
		}

		// Token: 0x17000098 RID: 152
		// (get) Token: 0x0600049A RID: 1178 RVA: 0x0001F53F File Offset: 0x0001D73F
		public override ConfigurationElementCollectionType CollectionType
		{
			get
			{
				return ConfigurationElementCollectionType.AddRemoveClearMap;
			}
		}

		// Token: 0x17000099 RID: 153
		public SchemeSettingElement this[int index]
		{
			get
			{
				return (SchemeSettingElement)base.BaseGet(index);
			}
		}

		// Token: 0x1700009A RID: 154
		public SchemeSettingElement this[string name]
		{
			get
			{
				return (SchemeSettingElement)base.BaseGet(name);
			}
		}

		// Token: 0x0600049D RID: 1181 RVA: 0x0001F55E File Offset: 0x0001D75E
		public int IndexOf(SchemeSettingElement element)
		{
			return base.BaseIndexOf(element);
		}

		// Token: 0x0600049E RID: 1182 RVA: 0x0001F567 File Offset: 0x0001D767
		protected override ConfigurationElement CreateNewElement()
		{
			return new SchemeSettingElement();
		}

		// Token: 0x0600049F RID: 1183 RVA: 0x0001F56E File Offset: 0x0001D76E
		protected override object GetElementKey(ConfigurationElement element)
		{
			return ((SchemeSettingElement)element).Name;
		}

		// Token: 0x04000BE9 RID: 3049
		internal const string AddItemName = "add";

		// Token: 0x04000BEA RID: 3050
		internal const string ClearItemsName = "clear";

		// Token: 0x04000BEB RID: 3051
		internal const string RemoveItemName = "remove";
	}
}
