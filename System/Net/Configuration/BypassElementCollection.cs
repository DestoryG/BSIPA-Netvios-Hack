using System;
using System.Configuration;

namespace System.Net.Configuration
{
	// Token: 0x02000329 RID: 809
	[ConfigurationCollection(typeof(BypassElement))]
	public sealed class BypassElementCollection : ConfigurationElementCollection
	{
		// Token: 0x17000726 RID: 1830
		public BypassElement this[int index]
		{
			get
			{
				return (BypassElement)base.BaseGet(index);
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

		// Token: 0x17000727 RID: 1831
		public BypassElement this[string name]
		{
			get
			{
				return (BypassElement)base.BaseGet(name);
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

		// Token: 0x06001CFE RID: 7422 RVA: 0x0008A9D0 File Offset: 0x00088BD0
		public void Add(BypassElement element)
		{
			this.BaseAdd(element);
		}

		// Token: 0x06001CFF RID: 7423 RVA: 0x0008A9D9 File Offset: 0x00088BD9
		public void Clear()
		{
			base.BaseClear();
		}

		// Token: 0x06001D00 RID: 7424 RVA: 0x0008A9E1 File Offset: 0x00088BE1
		protected override ConfigurationElement CreateNewElement()
		{
			return new BypassElement();
		}

		// Token: 0x06001D01 RID: 7425 RVA: 0x0008A9E8 File Offset: 0x00088BE8
		protected override object GetElementKey(ConfigurationElement element)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			return ((BypassElement)element).Key;
		}

		// Token: 0x06001D02 RID: 7426 RVA: 0x0008AA03 File Offset: 0x00088C03
		public int IndexOf(BypassElement element)
		{
			return base.BaseIndexOf(element);
		}

		// Token: 0x06001D03 RID: 7427 RVA: 0x0008AA0C File Offset: 0x00088C0C
		public void Remove(BypassElement element)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			base.BaseRemove(element.Key);
		}

		// Token: 0x06001D04 RID: 7428 RVA: 0x0008AA28 File Offset: 0x00088C28
		public void Remove(string name)
		{
			base.BaseRemove(name);
		}

		// Token: 0x06001D05 RID: 7429 RVA: 0x0008AA31 File Offset: 0x00088C31
		public void RemoveAt(int index)
		{
			base.BaseRemoveAt(index);
		}

		// Token: 0x17000728 RID: 1832
		// (get) Token: 0x06001D06 RID: 7430 RVA: 0x0008AA3A File Offset: 0x00088C3A
		protected override bool ThrowOnDuplicate
		{
			get
			{
				return false;
			}
		}
	}
}
