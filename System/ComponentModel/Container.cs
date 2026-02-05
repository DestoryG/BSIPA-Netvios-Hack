using System;
using System.Security.Permissions;

namespace System.ComponentModel
{
	// Token: 0x0200052F RID: 1327
	[HostProtection(SecurityAction.LinkDemand, SharedState = true)]
	public class Container : IContainer, IDisposable
	{
		// Token: 0x0600321F RID: 12831 RVA: 0x000E0C2C File Offset: 0x000DEE2C
		~Container()
		{
			this.Dispose(false);
		}

		// Token: 0x06003220 RID: 12832 RVA: 0x000E0C5C File Offset: 0x000DEE5C
		public virtual void Add(IComponent component)
		{
			this.Add(component, null);
		}

		// Token: 0x06003221 RID: 12833 RVA: 0x000E0C68 File Offset: 0x000DEE68
		public virtual void Add(IComponent component, string name)
		{
			object obj = this.syncObj;
			lock (obj)
			{
				if (component != null)
				{
					ISite site = component.Site;
					if (site == null || site.Container != this)
					{
						if (this.sites == null)
						{
							this.sites = new ISite[4];
						}
						else
						{
							this.ValidateName(component, name);
							if (this.sites.Length == this.siteCount)
							{
								ISite[] array = new ISite[this.siteCount * 2];
								Array.Copy(this.sites, 0, array, 0, this.siteCount);
								this.sites = array;
							}
						}
						if (site != null)
						{
							site.Container.Remove(component);
						}
						ISite site2 = this.CreateSite(component, name);
						ISite[] array2 = this.sites;
						int num = this.siteCount;
						this.siteCount = num + 1;
						array2[num] = site2;
						component.Site = site2;
						this.components = null;
					}
				}
			}
		}

		// Token: 0x06003222 RID: 12834 RVA: 0x000E0D60 File Offset: 0x000DEF60
		protected virtual ISite CreateSite(IComponent component, string name)
		{
			return new Container.Site(component, this, name);
		}

		// Token: 0x06003223 RID: 12835 RVA: 0x000E0D6A File Offset: 0x000DEF6A
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x06003224 RID: 12836 RVA: 0x000E0D7C File Offset: 0x000DEF7C
		protected virtual void Dispose(bool disposing)
		{
			if (disposing)
			{
				object obj = this.syncObj;
				lock (obj)
				{
					while (this.siteCount > 0)
					{
						ISite[] array = this.sites;
						int num = this.siteCount - 1;
						this.siteCount = num;
						ISite site = array[num];
						site.Component.Site = null;
						site.Component.Dispose();
					}
					this.sites = null;
					this.components = null;
				}
			}
		}

		// Token: 0x06003225 RID: 12837 RVA: 0x000E0E04 File Offset: 0x000DF004
		protected virtual object GetService(Type service)
		{
			if (!(service == typeof(IContainer)))
			{
				return null;
			}
			return this;
		}

		// Token: 0x17000C4F RID: 3151
		// (get) Token: 0x06003226 RID: 12838 RVA: 0x000E0E1C File Offset: 0x000DF01C
		public virtual ComponentCollection Components
		{
			get
			{
				object obj = this.syncObj;
				ComponentCollection componentCollection2;
				lock (obj)
				{
					if (this.components == null)
					{
						IComponent[] array = new IComponent[this.siteCount];
						for (int i = 0; i < this.siteCount; i++)
						{
							array[i] = this.sites[i].Component;
						}
						this.components = new ComponentCollection(array);
						if (this.filter == null && this.checkedFilter)
						{
							this.checkedFilter = false;
						}
					}
					if (!this.checkedFilter)
					{
						this.filter = this.GetService(typeof(ContainerFilterService)) as ContainerFilterService;
						this.checkedFilter = true;
					}
					if (this.filter != null)
					{
						ComponentCollection componentCollection = this.filter.FilterComponents(this.components);
						if (componentCollection != null)
						{
							this.components = componentCollection;
						}
					}
					componentCollection2 = this.components;
				}
				return componentCollection2;
			}
		}

		// Token: 0x06003227 RID: 12839 RVA: 0x000E0F0C File Offset: 0x000DF10C
		public virtual void Remove(IComponent component)
		{
			this.Remove(component, false);
		}

		// Token: 0x06003228 RID: 12840 RVA: 0x000E0F18 File Offset: 0x000DF118
		private void Remove(IComponent component, bool preserveSite)
		{
			object obj = this.syncObj;
			lock (obj)
			{
				if (component != null)
				{
					ISite site = component.Site;
					if (site != null && site.Container == this)
					{
						if (!preserveSite)
						{
							component.Site = null;
						}
						for (int i = 0; i < this.siteCount; i++)
						{
							if (this.sites[i] == site)
							{
								this.siteCount--;
								Array.Copy(this.sites, i + 1, this.sites, i, this.siteCount - i);
								this.sites[this.siteCount] = null;
								this.components = null;
								break;
							}
						}
					}
				}
			}
		}

		// Token: 0x06003229 RID: 12841 RVA: 0x000E0FD8 File Offset: 0x000DF1D8
		protected void RemoveWithoutUnsiting(IComponent component)
		{
			this.Remove(component, true);
		}

		// Token: 0x0600322A RID: 12842 RVA: 0x000E0FE4 File Offset: 0x000DF1E4
		protected virtual void ValidateName(IComponent component, string name)
		{
			if (component == null)
			{
				throw new ArgumentNullException("component");
			}
			if (name != null)
			{
				for (int i = 0; i < Math.Min(this.siteCount, this.sites.Length); i++)
				{
					ISite site = this.sites[i];
					if (site != null && site.Name != null && string.Equals(site.Name, name, StringComparison.OrdinalIgnoreCase) && site.Component != component)
					{
						InheritanceAttribute inheritanceAttribute = (InheritanceAttribute)TypeDescriptor.GetAttributes(site.Component)[typeof(InheritanceAttribute)];
						if (inheritanceAttribute.InheritanceLevel != InheritanceLevel.InheritedReadOnly)
						{
							throw new ArgumentException(SR.GetString("DuplicateComponentName", new object[] { name }));
						}
					}
				}
			}
		}

		// Token: 0x04002957 RID: 10583
		private ISite[] sites;

		// Token: 0x04002958 RID: 10584
		private int siteCount;

		// Token: 0x04002959 RID: 10585
		private ComponentCollection components;

		// Token: 0x0400295A RID: 10586
		private ContainerFilterService filter;

		// Token: 0x0400295B RID: 10587
		private bool checkedFilter;

		// Token: 0x0400295C RID: 10588
		private object syncObj = new object();

		// Token: 0x02000891 RID: 2193
		private class Site : ISite, IServiceProvider
		{
			// Token: 0x06004586 RID: 17798 RVA: 0x00121CC8 File Offset: 0x0011FEC8
			internal Site(IComponent component, Container container, string name)
			{
				this.component = component;
				this.container = container;
				this.name = name;
			}

			// Token: 0x17000FB6 RID: 4022
			// (get) Token: 0x06004587 RID: 17799 RVA: 0x00121CE5 File Offset: 0x0011FEE5
			public IComponent Component
			{
				get
				{
					return this.component;
				}
			}

			// Token: 0x17000FB7 RID: 4023
			// (get) Token: 0x06004588 RID: 17800 RVA: 0x00121CED File Offset: 0x0011FEED
			public IContainer Container
			{
				get
				{
					return this.container;
				}
			}

			// Token: 0x06004589 RID: 17801 RVA: 0x00121CF5 File Offset: 0x0011FEF5
			public object GetService(Type service)
			{
				if (!(service == typeof(ISite)))
				{
					return this.container.GetService(service);
				}
				return this;
			}

			// Token: 0x17000FB8 RID: 4024
			// (get) Token: 0x0600458A RID: 17802 RVA: 0x00121D17 File Offset: 0x0011FF17
			public bool DesignMode
			{
				get
				{
					return false;
				}
			}

			// Token: 0x17000FB9 RID: 4025
			// (get) Token: 0x0600458B RID: 17803 RVA: 0x00121D1A File Offset: 0x0011FF1A
			// (set) Token: 0x0600458C RID: 17804 RVA: 0x00121D22 File Offset: 0x0011FF22
			public string Name
			{
				get
				{
					return this.name;
				}
				set
				{
					if (value == null || this.name == null || !value.Equals(this.name))
					{
						this.container.ValidateName(this.component, value);
						this.name = value;
					}
				}
			}

			// Token: 0x040037B9 RID: 14265
			private IComponent component;

			// Token: 0x040037BA RID: 14266
			private Container container;

			// Token: 0x040037BB RID: 14267
			private string name;
		}
	}
}
