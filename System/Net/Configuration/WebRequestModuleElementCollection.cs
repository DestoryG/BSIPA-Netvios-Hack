using System;
using System.Configuration;

namespace System.Net.Configuration
{
	// Token: 0x0200034B RID: 843
	[ConfigurationCollection(typeof(WebRequestModuleElement))]
	public sealed class WebRequestModuleElementCollection : ConfigurationElementCollection
	{
		// Token: 0x170007D4 RID: 2004
		public WebRequestModuleElement this[int index]
		{
			get
			{
				return (WebRequestModuleElement)base.BaseGet(index);
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

		// Token: 0x170007D5 RID: 2005
		public WebRequestModuleElement this[string name]
		{
			get
			{
				return (WebRequestModuleElement)base.BaseGet(name);
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

		// Token: 0x06001E3D RID: 7741 RVA: 0x0008DA81 File Offset: 0x0008BC81
		public void Add(WebRequestModuleElement element)
		{
			this.BaseAdd(element);
		}

		// Token: 0x06001E3E RID: 7742 RVA: 0x0008DA8A File Offset: 0x0008BC8A
		public void Clear()
		{
			base.BaseClear();
		}

		// Token: 0x06001E3F RID: 7743 RVA: 0x0008DA92 File Offset: 0x0008BC92
		protected override ConfigurationElement CreateNewElement()
		{
			return new WebRequestModuleElement();
		}

		// Token: 0x06001E40 RID: 7744 RVA: 0x0008DA99 File Offset: 0x0008BC99
		protected override object GetElementKey(ConfigurationElement element)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			return ((WebRequestModuleElement)element).Key;
		}

		// Token: 0x06001E41 RID: 7745 RVA: 0x0008DAB4 File Offset: 0x0008BCB4
		public int IndexOf(WebRequestModuleElement element)
		{
			return base.BaseIndexOf(element);
		}

		// Token: 0x06001E42 RID: 7746 RVA: 0x0008DABD File Offset: 0x0008BCBD
		public void Remove(WebRequestModuleElement element)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			base.BaseRemove(element.Key);
		}

		// Token: 0x06001E43 RID: 7747 RVA: 0x0008DAD9 File Offset: 0x0008BCD9
		public void Remove(string name)
		{
			base.BaseRemove(name);
		}

		// Token: 0x06001E44 RID: 7748 RVA: 0x0008DAE2 File Offset: 0x0008BCE2
		public void RemoveAt(int index)
		{
			base.BaseRemoveAt(index);
		}
	}
}
