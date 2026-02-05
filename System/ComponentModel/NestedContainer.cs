using System;
using System.Globalization;
using System.Security.Permissions;

namespace System.ComponentModel
{
	// Token: 0x02000593 RID: 1427
	[HostProtection(SecurityAction.LinkDemand, SharedState = true)]
	public class NestedContainer : Container, INestedContainer, IContainer, IDisposable
	{
		// Token: 0x060034FC RID: 13564 RVA: 0x000E7335 File Offset: 0x000E5535
		public NestedContainer(IComponent owner)
		{
			if (owner == null)
			{
				throw new ArgumentNullException("owner");
			}
			this._owner = owner;
			this._owner.Disposed += this.OnOwnerDisposed;
		}

		// Token: 0x17000CF3 RID: 3315
		// (get) Token: 0x060034FD RID: 13565 RVA: 0x000E7369 File Offset: 0x000E5569
		public IComponent Owner
		{
			get
			{
				return this._owner;
			}
		}

		// Token: 0x17000CF4 RID: 3316
		// (get) Token: 0x060034FE RID: 13566 RVA: 0x000E7374 File Offset: 0x000E5574
		protected virtual string OwnerName
		{
			get
			{
				string text = null;
				if (this._owner != null && this._owner.Site != null)
				{
					INestedSite nestedSite = this._owner.Site as INestedSite;
					if (nestedSite != null)
					{
						text = nestedSite.FullName;
					}
					else
					{
						text = this._owner.Site.Name;
					}
				}
				return text;
			}
		}

		// Token: 0x060034FF RID: 13567 RVA: 0x000E73C7 File Offset: 0x000E55C7
		protected override ISite CreateSite(IComponent component, string name)
		{
			if (component == null)
			{
				throw new ArgumentNullException("component");
			}
			return new NestedContainer.Site(component, this, name);
		}

		// Token: 0x06003500 RID: 13568 RVA: 0x000E73DF File Offset: 0x000E55DF
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				this._owner.Disposed -= this.OnOwnerDisposed;
			}
			base.Dispose(disposing);
		}

		// Token: 0x06003501 RID: 13569 RVA: 0x000E7402 File Offset: 0x000E5602
		protected override object GetService(Type service)
		{
			if (service == typeof(INestedContainer))
			{
				return this;
			}
			return base.GetService(service);
		}

		// Token: 0x06003502 RID: 13570 RVA: 0x000E741F File Offset: 0x000E561F
		private void OnOwnerDisposed(object sender, EventArgs e)
		{
			base.Dispose();
		}

		// Token: 0x04002A29 RID: 10793
		private IComponent _owner;

		// Token: 0x0200089A RID: 2202
		private class Site : INestedSite, ISite, IServiceProvider
		{
			// Token: 0x0600459D RID: 17821 RVA: 0x001231A2 File Offset: 0x001213A2
			internal Site(IComponent component, NestedContainer container, string name)
			{
				this.component = component;
				this.container = container;
				this.name = name;
			}

			// Token: 0x17000FBB RID: 4027
			// (get) Token: 0x0600459E RID: 17822 RVA: 0x001231BF File Offset: 0x001213BF
			public IComponent Component
			{
				get
				{
					return this.component;
				}
			}

			// Token: 0x17000FBC RID: 4028
			// (get) Token: 0x0600459F RID: 17823 RVA: 0x001231C7 File Offset: 0x001213C7
			public IContainer Container
			{
				get
				{
					return this.container;
				}
			}

			// Token: 0x060045A0 RID: 17824 RVA: 0x001231CF File Offset: 0x001213CF
			public object GetService(Type service)
			{
				if (!(service == typeof(ISite)))
				{
					return this.container.GetService(service);
				}
				return this;
			}

			// Token: 0x17000FBD RID: 4029
			// (get) Token: 0x060045A1 RID: 17825 RVA: 0x001231F4 File Offset: 0x001213F4
			public bool DesignMode
			{
				get
				{
					IComponent owner = this.container.Owner;
					return owner != null && owner.Site != null && owner.Site.DesignMode;
				}
			}

			// Token: 0x17000FBE RID: 4030
			// (get) Token: 0x060045A2 RID: 17826 RVA: 0x00123228 File Offset: 0x00121428
			public string FullName
			{
				get
				{
					if (this.name != null)
					{
						string ownerName = this.container.OwnerName;
						string text = this.name;
						if (ownerName != null)
						{
							text = string.Format(CultureInfo.InvariantCulture, "{0}.{1}", new object[] { ownerName, text });
						}
						return text;
					}
					return this.name;
				}
			}

			// Token: 0x17000FBF RID: 4031
			// (get) Token: 0x060045A3 RID: 17827 RVA: 0x00123279 File Offset: 0x00121479
			// (set) Token: 0x060045A4 RID: 17828 RVA: 0x00123281 File Offset: 0x00121481
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

			// Token: 0x040037D8 RID: 14296
			private IComponent component;

			// Token: 0x040037D9 RID: 14297
			private NestedContainer container;

			// Token: 0x040037DA RID: 14298
			private string name;
		}
	}
}
