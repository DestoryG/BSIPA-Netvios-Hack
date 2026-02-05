using System;
using System.Collections;
using System.Collections.Specialized;
using System.Runtime.InteropServices;
using System.Security.Permissions;

namespace System.ComponentModel.Design
{
	// Token: 0x020005FC RID: 1532
	[ComVisible(true)]
	[HostProtection(SecurityAction.LinkDemand, SharedState = true)]
	[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
	[PermissionSet(SecurityAction.InheritanceDemand, Name = "FullTrust")]
	public class MenuCommand
	{
		// Token: 0x0600385F RID: 14431 RVA: 0x000F0E3E File Offset: 0x000EF03E
		public MenuCommand(EventHandler handler, CommandID command)
		{
			this.execHandler = handler;
			this.commandID = command;
			this.status = 3;
		}

		// Token: 0x17000D7C RID: 3452
		// (get) Token: 0x06003860 RID: 14432 RVA: 0x000F0E5B File Offset: 0x000EF05B
		// (set) Token: 0x06003861 RID: 14433 RVA: 0x000F0E68 File Offset: 0x000EF068
		public virtual bool Checked
		{
			get
			{
				return (this.status & 4) != 0;
			}
			set
			{
				this.SetStatus(4, value);
			}
		}

		// Token: 0x17000D7D RID: 3453
		// (get) Token: 0x06003862 RID: 14434 RVA: 0x000F0E72 File Offset: 0x000EF072
		// (set) Token: 0x06003863 RID: 14435 RVA: 0x000F0E7F File Offset: 0x000EF07F
		public virtual bool Enabled
		{
			get
			{
				return (this.status & 2) != 0;
			}
			set
			{
				this.SetStatus(2, value);
			}
		}

		// Token: 0x06003864 RID: 14436 RVA: 0x000F0E8C File Offset: 0x000EF08C
		private void SetStatus(int mask, bool value)
		{
			int num = this.status;
			if (value)
			{
				num |= mask;
			}
			else
			{
				num &= ~mask;
			}
			if (num != this.status)
			{
				this.status = num;
				this.OnCommandChanged(EventArgs.Empty);
			}
		}

		// Token: 0x17000D7E RID: 3454
		// (get) Token: 0x06003865 RID: 14437 RVA: 0x000F0EC9 File Offset: 0x000EF0C9
		public virtual IDictionary Properties
		{
			get
			{
				if (this.properties == null)
				{
					this.properties = new HybridDictionary();
				}
				return this.properties;
			}
		}

		// Token: 0x17000D7F RID: 3455
		// (get) Token: 0x06003866 RID: 14438 RVA: 0x000F0EE4 File Offset: 0x000EF0E4
		// (set) Token: 0x06003867 RID: 14439 RVA: 0x000F0EF1 File Offset: 0x000EF0F1
		public virtual bool Supported
		{
			get
			{
				return (this.status & 1) != 0;
			}
			set
			{
				this.SetStatus(1, value);
			}
		}

		// Token: 0x17000D80 RID: 3456
		// (get) Token: 0x06003868 RID: 14440 RVA: 0x000F0EFB File Offset: 0x000EF0FB
		// (set) Token: 0x06003869 RID: 14441 RVA: 0x000F0F09 File Offset: 0x000EF109
		public virtual bool Visible
		{
			get
			{
				return (this.status & 16) == 0;
			}
			set
			{
				this.SetStatus(16, !value);
			}
		}

		// Token: 0x14000067 RID: 103
		// (add) Token: 0x0600386A RID: 14442 RVA: 0x000F0F17 File Offset: 0x000EF117
		// (remove) Token: 0x0600386B RID: 14443 RVA: 0x000F0F30 File Offset: 0x000EF130
		public event EventHandler CommandChanged
		{
			add
			{
				this.statusHandler = (EventHandler)Delegate.Combine(this.statusHandler, value);
			}
			remove
			{
				this.statusHandler = (EventHandler)Delegate.Remove(this.statusHandler, value);
			}
		}

		// Token: 0x17000D81 RID: 3457
		// (get) Token: 0x0600386C RID: 14444 RVA: 0x000F0F49 File Offset: 0x000EF149
		public virtual CommandID CommandID
		{
			get
			{
				return this.commandID;
			}
		}

		// Token: 0x0600386D RID: 14445 RVA: 0x000F0F54 File Offset: 0x000EF154
		public virtual void Invoke()
		{
			if (this.execHandler != null)
			{
				try
				{
					this.execHandler(this, EventArgs.Empty);
				}
				catch (CheckoutException ex)
				{
					if (ex != CheckoutException.Canceled)
					{
						throw;
					}
				}
			}
		}

		// Token: 0x0600386E RID: 14446 RVA: 0x000F0F9C File Offset: 0x000EF19C
		public virtual void Invoke(object arg)
		{
			this.Invoke();
		}

		// Token: 0x17000D82 RID: 3458
		// (get) Token: 0x0600386F RID: 14447 RVA: 0x000F0FA4 File Offset: 0x000EF1A4
		public virtual int OleStatus
		{
			get
			{
				return this.status;
			}
		}

		// Token: 0x06003870 RID: 14448 RVA: 0x000F0FAC File Offset: 0x000EF1AC
		protected virtual void OnCommandChanged(EventArgs e)
		{
			if (this.statusHandler != null)
			{
				this.statusHandler(this, e);
			}
		}

		// Token: 0x06003871 RID: 14449 RVA: 0x000F0FC4 File Offset: 0x000EF1C4
		public override string ToString()
		{
			string text = this.CommandID.ToString() + " : ";
			if ((this.status & 1) != 0)
			{
				text += "Supported";
			}
			if ((this.status & 2) != 0)
			{
				text += "|Enabled";
			}
			if ((this.status & 16) == 0)
			{
				text += "|Visible";
			}
			if ((this.status & 4) != 0)
			{
				text += "|Checked";
			}
			return text;
		}

		// Token: 0x04002B05 RID: 11013
		private EventHandler execHandler;

		// Token: 0x04002B06 RID: 11014
		private EventHandler statusHandler;

		// Token: 0x04002B07 RID: 11015
		private CommandID commandID;

		// Token: 0x04002B08 RID: 11016
		private int status;

		// Token: 0x04002B09 RID: 11017
		private IDictionary properties;

		// Token: 0x04002B0A RID: 11018
		private const int ENABLED = 2;

		// Token: 0x04002B0B RID: 11019
		private const int INVISIBLE = 16;

		// Token: 0x04002B0C RID: 11020
		private const int CHECKED = 4;

		// Token: 0x04002B0D RID: 11021
		private const int SUPPORTED = 1;
	}
}
