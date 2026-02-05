using System;
using System.Runtime.InteropServices;

namespace System.ComponentModel
{
	// Token: 0x0200052B RID: 1323
	[ComVisible(true)]
	[ClassInterface(ClassInterfaceType.AutoDispatch)]
	[DesignerCategory("Component")]
	public class Component : MarshalByRefObject, IComponent, IDisposable
	{
		// Token: 0x06003202 RID: 12802 RVA: 0x000E0598 File Offset: 0x000DE798
		~Component()
		{
			this.Dispose(false);
		}

		// Token: 0x17000C46 RID: 3142
		// (get) Token: 0x06003203 RID: 12803 RVA: 0x000E05C8 File Offset: 0x000DE7C8
		protected virtual bool CanRaiseEvents
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000C47 RID: 3143
		// (get) Token: 0x06003204 RID: 12804 RVA: 0x000E05CB File Offset: 0x000DE7CB
		internal bool CanRaiseEventsInternal
		{
			get
			{
				return this.CanRaiseEvents;
			}
		}

		// Token: 0x1400004A RID: 74
		// (add) Token: 0x06003205 RID: 12805 RVA: 0x000E05D3 File Offset: 0x000DE7D3
		// (remove) Token: 0x06003206 RID: 12806 RVA: 0x000E05E6 File Offset: 0x000DE7E6
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public event EventHandler Disposed
		{
			add
			{
				this.Events.AddHandler(Component.EventDisposed, value);
			}
			remove
			{
				this.Events.RemoveHandler(Component.EventDisposed, value);
			}
		}

		// Token: 0x17000C48 RID: 3144
		// (get) Token: 0x06003207 RID: 12807 RVA: 0x000E05F9 File Offset: 0x000DE7F9
		protected EventHandlerList Events
		{
			get
			{
				if (this.events == null)
				{
					this.events = new EventHandlerList(this);
				}
				return this.events;
			}
		}

		// Token: 0x17000C49 RID: 3145
		// (get) Token: 0x06003208 RID: 12808 RVA: 0x000E0615 File Offset: 0x000DE815
		// (set) Token: 0x06003209 RID: 12809 RVA: 0x000E061D File Offset: 0x000DE81D
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

		// Token: 0x0600320A RID: 12810 RVA: 0x000E0626 File Offset: 0x000DE826
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x0600320B RID: 12811 RVA: 0x000E0638 File Offset: 0x000DE838
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
						EventHandler eventHandler = (EventHandler)this.events[Component.EventDisposed];
						if (eventHandler != null)
						{
							eventHandler(this, EventArgs.Empty);
						}
					}
				}
			}
		}

		// Token: 0x17000C4A RID: 3146
		// (get) Token: 0x0600320C RID: 12812 RVA: 0x000E06C4 File Offset: 0x000DE8C4
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public IContainer Container
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

		// Token: 0x0600320D RID: 12813 RVA: 0x000E06E4 File Offset: 0x000DE8E4
		protected virtual object GetService(Type service)
		{
			ISite site = this.site;
			if (site != null)
			{
				return site.GetService(service);
			}
			return null;
		}

		// Token: 0x17000C4B RID: 3147
		// (get) Token: 0x0600320E RID: 12814 RVA: 0x000E0704 File Offset: 0x000DE904
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		protected bool DesignMode
		{
			get
			{
				ISite site = this.site;
				return site != null && site.DesignMode;
			}
		}

		// Token: 0x0600320F RID: 12815 RVA: 0x000E0724 File Offset: 0x000DE924
		public override string ToString()
		{
			ISite site = this.site;
			if (site != null)
			{
				return site.Name + " [" + base.GetType().FullName + "]";
			}
			return base.GetType().FullName;
		}

		// Token: 0x04002952 RID: 10578
		private static readonly object EventDisposed = new object();

		// Token: 0x04002953 RID: 10579
		private ISite site;

		// Token: 0x04002954 RID: 10580
		private EventHandlerList events;
	}
}
