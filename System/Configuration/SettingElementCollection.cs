using System;

namespace System.Configuration
{
	// Token: 0x020000B8 RID: 184
	public sealed class SettingElementCollection : ConfigurationElementCollection
	{
		// Token: 0x17000100 RID: 256
		// (get) Token: 0x0600061E RID: 1566 RVA: 0x00023E7D File Offset: 0x0002207D
		public override ConfigurationElementCollectionType CollectionType
		{
			get
			{
				return ConfigurationElementCollectionType.BasicMap;
			}
		}

		// Token: 0x17000101 RID: 257
		// (get) Token: 0x0600061F RID: 1567 RVA: 0x00023E80 File Offset: 0x00022080
		protected override string ElementName
		{
			get
			{
				return "setting";
			}
		}

		// Token: 0x06000620 RID: 1568 RVA: 0x00023E87 File Offset: 0x00022087
		protected override ConfigurationElement CreateNewElement()
		{
			return new SettingElement();
		}

		// Token: 0x06000621 RID: 1569 RVA: 0x00023E8E File Offset: 0x0002208E
		protected override object GetElementKey(ConfigurationElement element)
		{
			return ((SettingElement)element).Key;
		}

		// Token: 0x06000622 RID: 1570 RVA: 0x00023E9B File Offset: 0x0002209B
		public SettingElement Get(string elementKey)
		{
			return (SettingElement)base.BaseGet(elementKey);
		}

		// Token: 0x06000623 RID: 1571 RVA: 0x00023EA9 File Offset: 0x000220A9
		public void Add(SettingElement element)
		{
			this.BaseAdd(element);
		}

		// Token: 0x06000624 RID: 1572 RVA: 0x00023EB2 File Offset: 0x000220B2
		public void Remove(SettingElement element)
		{
			base.BaseRemove(this.GetElementKey(element));
		}

		// Token: 0x06000625 RID: 1573 RVA: 0x00023EC1 File Offset: 0x000220C1
		public void Clear()
		{
			base.BaseClear();
		}
	}
}
