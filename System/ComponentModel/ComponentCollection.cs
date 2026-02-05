using System;
using System.Collections;
using System.Runtime.InteropServices;
using System.Security.Permissions;

namespace System.ComponentModel
{
	// Token: 0x0200052C RID: 1324
	[ComVisible(true)]
	[HostProtection(SecurityAction.LinkDemand, Synchronization = true)]
	public class ComponentCollection : ReadOnlyCollectionBase
	{
		// Token: 0x06003212 RID: 12818 RVA: 0x000E077B File Offset: 0x000DE97B
		public ComponentCollection(IComponent[] components)
		{
			base.InnerList.AddRange(components);
		}

		// Token: 0x17000C4C RID: 3148
		public virtual IComponent this[string name]
		{
			get
			{
				if (name != null)
				{
					IList innerList = base.InnerList;
					foreach (object obj in innerList)
					{
						IComponent component = (IComponent)obj;
						if (component != null && component.Site != null && component.Site.Name != null && string.Equals(component.Site.Name, name, StringComparison.OrdinalIgnoreCase))
						{
							return component;
						}
					}
				}
				return null;
			}
		}

		// Token: 0x17000C4D RID: 3149
		public virtual IComponent this[int index]
		{
			get
			{
				return (IComponent)base.InnerList[index];
			}
		}

		// Token: 0x06003215 RID: 12821 RVA: 0x000E0833 File Offset: 0x000DEA33
		public void CopyTo(IComponent[] array, int index)
		{
			base.InnerList.CopyTo(array, index);
		}
	}
}
