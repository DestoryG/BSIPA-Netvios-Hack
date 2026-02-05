using System;
using System.ComponentModel.Design;
using System.Runtime.InteropServices;

namespace System.ComponentModel
{
	// Token: 0x0200058D RID: 1421
	[ComVisible(true)]
	[Designer("System.Windows.Forms.Design.ComponentDocumentDesigner, System.Design, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(IRootDesigner))]
	[DesignerCategory("Component")]
	[TypeConverter(typeof(ComponentConverter))]
	public class MarshalByValueComponent : IComponent, IDisposable, IServiceProvider
	{
		// Token: 0x0600345C RID: 13404 RVA: 0x000E4B64 File Offset: 0x000E2D64
		~MarshalByValueComponent()
		{
			this.Dispose(false);
		}

		// Token: 0x14000051 RID: 81
		// (add) Token: 0x0600345D RID: 13405 RVA: 0x000E4B94 File Offset: 0x000E2D94
		// (remove) Token: 0x0600345E RID: 13406 RVA: 0x000E4BA7 File Offset: 0x000E2DA7
		public event EventHandler Disposed
		{
			add
			{
				this.Events.AddHandler(MarshalByValueComponent.EventDisposed, value);
			}
			remove
			{
				this.Events.RemoveHandler(MarshalByValueComponent.EventDisposed, value);
			}
		}

		// Token: 0x17000CCE RID: 3278
		// (get) Token: 0x0600345F RID: 13407 RVA: 0x000E4BBA File Offset: 0x000E2DBA
		protected EventHandlerList Events
		{
			get
			{
				if (this.events == null)
				{
					this.events = new EventHandlerList();
				}
				return this.events;
			}
		}

		// Token: 0x17000CCF RID: 3279
		// (get) Token: 0x06003460 RID: 13408 RVA: 0x000E4BD5 File Offset: 0x000E2DD5
		// (set) Token: 0x06003461 RID: 13409 RVA: 0x000E4BDD File Offset: 0x000E2DDD
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public virtual ISite Site
		{
			get
			{
				return this.site;
			}
			set
			{
				this.site = value;
			}
		}

		// Token: 0x06003462 RID: 13410 RVA: 0x000E4BE6 File Offset: 0x000E2DE6
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x06003463 RID: 13411 RVA: 0x000E4BF8 File Offset: 0x000E2DF8
		protected virtual void Dispose(bool disposing)
		{
			if (disposing)
			{
				lock (this)
				{
					if (this.site != null && this.site.Container != null)
					{
						this.site.Container.Remove(this);
					}
					if (this.events != null)
					{
						EventHandler eventHandler = (EventHandler)this.events[MarshalByValueComponent.EventDisposed];
						if (eventHandler != null)
						{
							eventHandler(this, EventArgs.Empty);
						}
					}
				}
			}
		}

		// Token: 0x17000CD0 RID: 3280
		// (get) Token: 0x06003464 RID: 13412 RVA: 0x000E4C84 File Offset: 0x000E2E84
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public virtual IContainer Container
		{
			get
			{
				ISite site = this.site;
				if (site != null)
				{
					return site.Container;
				}
				return null;
			}
		}

		// Token: 0x06003465 RID: 13413 RVA: 0x000E4CA3 File Offset: 0x000E2EA3
		public virtual object GetService(Type service)
		{
			if (this.site != null)
			{
				return this.site.GetService(service);
			}
			return null;
		}

		// Token: 0x17000CD1 RID: 3281
		// (get) Token: 0x06003466 RID: 13414 RVA: 0x000E4CBC File Offset: 0x000E2EBC
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public virtual bool DesignMode
		{
			get
			{
				ISite site = this.site;
				return site != null && site.DesignMode;
			}
		}

		// Token: 0x06003467 RID: 13415 RVA: 0x000E4CDC File Offset: 0x000E2EDC
		public override string ToString()
		{
			ISite site = this.site;
			if (site != null)
			{
				return site.Name + " [" + base.GetType().FullName + "]";
			}
			return base.GetType().FullName;
		}

		// Token: 0x040029E9 RID: 10729
		private static readonly object EventDisposed = new object();

		// Token: 0x040029EA RID: 10730
		private ISite site;

		// Token: 0x040029EB RID: 10731
		private EventHandlerList events;
	}
}
