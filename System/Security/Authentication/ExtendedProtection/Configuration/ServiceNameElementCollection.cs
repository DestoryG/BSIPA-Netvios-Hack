using System;
using System.Configuration;

namespace System.Security.Authentication.ExtendedProtection.Configuration
{
	// Token: 0x0200044B RID: 1099
	[ConfigurationCollection(typeof(ServiceNameElement))]
	public sealed class ServiceNameElementCollection : ConfigurationElementCollection
	{
		// Token: 0x170009FD RID: 2557
		public ServiceNameElement this[int index]
		{
			get
			{
				return (ServiceNameElement)base.BaseGet(index);
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

		// Token: 0x170009FE RID: 2558
		public ServiceNameElement this[string name]
		{
			get
			{
				return (ServiceNameElement)base.BaseGet(name);
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

		// Token: 0x060028AA RID: 10410 RVA: 0x000BA84E File Offset: 0x000B8A4E
		public void Add(ServiceNameElement element)
		{
			this.BaseAdd(element);
		}

		// Token: 0x060028AB RID: 10411 RVA: 0x000BA857 File Offset: 0x000B8A57
		public void Clear()
		{
			base.BaseClear();
		}

		// Token: 0x060028AC RID: 10412 RVA: 0x000BA85F File Offset: 0x000B8A5F
		protected override ConfigurationElement CreateNewElement()
		{
			return new ServiceNameElement();
		}

		// Token: 0x060028AD RID: 10413 RVA: 0x000BA866 File Offset: 0x000B8A66
		protected override object GetElementKey(ConfigurationElement element)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			return ((ServiceNameElement)element).Key;
		}

		// Token: 0x060028AE RID: 10414 RVA: 0x000BA881 File Offset: 0x000B8A81
		public int IndexOf(ServiceNameElement element)
		{
			return base.BaseIndexOf(element);
		}

		// Token: 0x060028AF RID: 10415 RVA: 0x000BA88A File Offset: 0x000B8A8A
		public void Remove(ServiceNameElement element)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			base.BaseRemove(element.Key);
		}

		// Token: 0x060028B0 RID: 10416 RVA: 0x000BA8A6 File Offset: 0x000B8AA6
		public void Remove(string name)
		{
			base.BaseRemove(name);
		}

		// Token: 0x060028B1 RID: 10417 RVA: 0x000BA8AF File Offset: 0x000B8AAF
		public void RemoveAt(int index)
		{
			base.BaseRemoveAt(index);
		}
	}
}
