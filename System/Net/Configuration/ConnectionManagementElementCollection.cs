using System;
using System.Configuration;

namespace System.Net.Configuration
{
	// Token: 0x0200032C RID: 812
	[ConfigurationCollection(typeof(ConnectionManagementElement))]
	public sealed class ConnectionManagementElementCollection : ConfigurationElementCollection
	{
		// Token: 0x17000734 RID: 1844
		public ConnectionManagementElement this[int index]
		{
			get
			{
				return (ConnectionManagementElement)base.BaseGet(index);
			}
			set
			{
				if (base.BaseGet(index) != null)
				{
					base.BaseRemoveAt(index);
				}
				this.BaseAdd(index, value);
			}
		}

		// Token: 0x17000735 RID: 1845
		public ConnectionManagementElement this[string name]
		{
			get
			{
				return (ConnectionManagementElement)base.BaseGet(name);
			}
			set
			{
				if (base.BaseGet(name) != null)
				{
					base.BaseRemove(name);
				}
				this.BaseAdd(value);
			}
		}

		// Token: 0x06001D1D RID: 7453 RVA: 0x0008AC23 File Offset: 0x00088E23
		public void Add(ConnectionManagementElement element)
		{
			this.BaseAdd(element);
		}

		// Token: 0x06001D1E RID: 7454 RVA: 0x0008AC2C File Offset: 0x00088E2C
		public void Clear()
		{
			base.BaseClear();
		}

		// Token: 0x06001D1F RID: 7455 RVA: 0x0008AC34 File Offset: 0x00088E34
		protected override ConfigurationElement CreateNewElement()
		{
			return new ConnectionManagementElement();
		}

		// Token: 0x06001D20 RID: 7456 RVA: 0x0008AC3B File Offset: 0x00088E3B
		protected override object GetElementKey(ConfigurationElement element)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			return ((ConnectionManagementElement)element).Key;
		}

		// Token: 0x06001D21 RID: 7457 RVA: 0x0008AC56 File Offset: 0x00088E56
		public int IndexOf(ConnectionManagementElement element)
		{
			return base.BaseIndexOf(element);
		}

		// Token: 0x06001D22 RID: 7458 RVA: 0x0008AC5F File Offset: 0x00088E5F
		public void Remove(ConnectionManagementElement element)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			base.BaseRemove(element.Key);
		}

		// Token: 0x06001D23 RID: 7459 RVA: 0x0008AC7B File Offset: 0x00088E7B
		public void Remove(string name)
		{
			base.BaseRemove(name);
		}

		// Token: 0x06001D24 RID: 7460 RVA: 0x0008AC84 File Offset: 0x00088E84
		public void RemoveAt(int index)
		{
			base.BaseRemoveAt(index);
		}
	}
}
