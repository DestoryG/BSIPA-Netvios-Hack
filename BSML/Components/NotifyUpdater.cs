using System;
using System.Collections.Generic;
using System.Reflection;
using BeatSaberMarkupLanguage.Notify;
using IPA.Logging;
using UnityEngine;

namespace BeatSaberMarkupLanguage.Components
{
	// Token: 0x020000A4 RID: 164
	public class NotifyUpdater : MonoBehaviour
	{
		// Token: 0x170000E2 RID: 226
		// (get) Token: 0x06000357 RID: 855 RVA: 0x0001062C File Offset: 0x0000E82C
		// (set) Token: 0x06000358 RID: 856 RVA: 0x00010634 File Offset: 0x0000E834
		public INotifiableHost NotifyHost
		{
			get
			{
				return this._notifyHost;
			}
			set
			{
				if (this._notifyHost != null)
				{
					this._notifyHost.PropertyChanged -= this.NotifyHost_PropertyChanged;
				}
				this._notifyHost = value;
				if (this._notifyHost != null)
				{
					this._notifyHost.PropertyChanged -= this.NotifyHost_PropertyChanged;
					this._notifyHost.PropertyChanged += this.NotifyHost_PropertyChanged;
				}
			}
		}

		// Token: 0x06000359 RID: 857 RVA: 0x000106A0 File Offset: 0x0000E8A0
		private void NotifyHost_PropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			if (this == null)
			{
				this.OnDestroy();
				return;
			}
			PropertyInfo property = sender.GetType().GetProperty(e.PropertyName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
			Action<object> action = null;
			if (this.ActionDict.TryGetValue(e.PropertyName, out action) && action != null)
			{
				action(property.GetValue(sender));
			}
		}

		// Token: 0x170000E3 RID: 227
		// (get) Token: 0x0600035A RID: 858 RVA: 0x000106F8 File Offset: 0x0000E8F8
		// (set) Token: 0x0600035B RID: 859 RVA: 0x00010700 File Offset: 0x0000E900
		private Dictionary<string, Action<object>> ActionDict { get; set; } = new Dictionary<string, Action<object>>();

		// Token: 0x0600035C RID: 860 RVA: 0x00010709 File Offset: 0x0000E909
		public bool AddAction(string propertyName, Action<object> action)
		{
			this.ActionDict.Add(propertyName, action);
			return true;
		}

		// Token: 0x0600035D RID: 861 RVA: 0x00010719 File Offset: 0x0000E919
		public bool RemoveAction(string propertyName)
		{
			return this.ActionDict != null && this.ActionDict.Remove(propertyName);
		}

		// Token: 0x0600035E RID: 862 RVA: 0x00010731 File Offset: 0x0000E931
		private void OnDestroy()
		{
			Logger log = Logger.log;
			if (log != null)
			{
				log.Debug("NotifyUpdater destroyed.");
			}
			this.ActionDict.Clear();
			this.NotifyHost = null;
		}

		// Token: 0x040000F4 RID: 244
		private INotifiableHost _notifyHost;
	}
}
