using System;
using System.Configuration;

namespace System.Net.Configuration
{
	// Token: 0x02000325 RID: 805
	[ConfigurationCollection(typeof(AuthenticationModuleElement))]
	public sealed class AuthenticationModuleElementCollection : ConfigurationElementCollection
	{
		// Token: 0x1700071D RID: 1821
		public AuthenticationModuleElement this[int index]
		{
			get
			{
				return (AuthenticationModuleElement)base.BaseGet(index);
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

		// Token: 0x1700071E RID: 1822
		public AuthenticationModuleElement this[string name]
		{
			get
			{
				return (AuthenticationModuleElement)base.BaseGet(name);
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

		// Token: 0x06001CE2 RID: 7394 RVA: 0x0008A58E File Offset: 0x0008878E
		public void Add(AuthenticationModuleElement element)
		{
			this.BaseAdd(element);
		}

		// Token: 0x06001CE3 RID: 7395 RVA: 0x0008A597 File Offset: 0x00088797
		public void Clear()
		{
			base.BaseClear();
		}

		// Token: 0x06001CE4 RID: 7396 RVA: 0x0008A59F File Offset: 0x0008879F
		protected override ConfigurationElement CreateNewElement()
		{
			return new AuthenticationModuleElement();
		}

		// Token: 0x06001CE5 RID: 7397 RVA: 0x0008A5A6 File Offset: 0x000887A6
		protected override object GetElementKey(ConfigurationElement element)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			return ((AuthenticationModuleElement)element).Key;
		}

		// Token: 0x06001CE6 RID: 7398 RVA: 0x0008A5C1 File Offset: 0x000887C1
		public int IndexOf(AuthenticationModuleElement element)
		{
			return base.BaseIndexOf(element);
		}

		// Token: 0x06001CE7 RID: 7399 RVA: 0x0008A5CA File Offset: 0x000887CA
		public void Remove(AuthenticationModuleElement element)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			base.BaseRemove(element.Key);
		}

		// Token: 0x06001CE8 RID: 7400 RVA: 0x0008A5E6 File Offset: 0x000887E6
		public void Remove(string name)
		{
			base.BaseRemove(name);
		}

		// Token: 0x06001CE9 RID: 7401 RVA: 0x0008A5EF File Offset: 0x000887EF
		public void RemoveAt(int index)
		{
			base.BaseRemoveAt(index);
		}
	}
}
