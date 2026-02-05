using System;
using System.Collections;

namespace System.Configuration
{
	// Token: 0x020000AA RID: 170
	public class SettingsPropertyCollection : IEnumerable, ICloneable, ICollection
	{
		// Token: 0x060005D0 RID: 1488 RVA: 0x000231B0 File Offset: 0x000213B0
		public SettingsPropertyCollection()
		{
			this._Hashtable = new Hashtable(10, StringComparer.CurrentCultureIgnoreCase);
		}

		// Token: 0x060005D1 RID: 1489 RVA: 0x000231CC File Offset: 0x000213CC
		public void Add(SettingsProperty property)
		{
			if (this._ReadOnly)
			{
				throw new NotSupportedException();
			}
			this.OnAdd(property);
			this._Hashtable.Add(property.Name, property);
			try
			{
				this.OnAddComplete(property);
			}
			catch
			{
				this._Hashtable.Remove(property.Name);
				throw;
			}
		}

		// Token: 0x060005D2 RID: 1490 RVA: 0x00023230 File Offset: 0x00021430
		public void Remove(string name)
		{
			if (this._ReadOnly)
			{
				throw new NotSupportedException();
			}
			SettingsProperty settingsProperty = (SettingsProperty)this._Hashtable[name];
			if (settingsProperty == null)
			{
				return;
			}
			this.OnRemove(settingsProperty);
			this._Hashtable.Remove(name);
			try
			{
				this.OnRemoveComplete(settingsProperty);
			}
			catch
			{
				this._Hashtable.Add(name, settingsProperty);
				throw;
			}
		}

		// Token: 0x170000ED RID: 237
		public SettingsProperty this[string name]
		{
			get
			{
				return this._Hashtable[name] as SettingsProperty;
			}
		}

		// Token: 0x060005D4 RID: 1492 RVA: 0x000232B3 File Offset: 0x000214B3
		public IEnumerator GetEnumerator()
		{
			return this._Hashtable.Values.GetEnumerator();
		}

		// Token: 0x060005D5 RID: 1493 RVA: 0x000232C5 File Offset: 0x000214C5
		public object Clone()
		{
			return new SettingsPropertyCollection(this._Hashtable);
		}

		// Token: 0x060005D6 RID: 1494 RVA: 0x000232D2 File Offset: 0x000214D2
		public void SetReadOnly()
		{
			if (this._ReadOnly)
			{
				return;
			}
			this._ReadOnly = true;
		}

		// Token: 0x060005D7 RID: 1495 RVA: 0x000232E4 File Offset: 0x000214E4
		public void Clear()
		{
			if (this._ReadOnly)
			{
				throw new NotSupportedException();
			}
			this.OnClear();
			this._Hashtable.Clear();
			this.OnClearComplete();
		}

		// Token: 0x060005D8 RID: 1496 RVA: 0x0002330B File Offset: 0x0002150B
		protected virtual void OnAdd(SettingsProperty property)
		{
		}

		// Token: 0x060005D9 RID: 1497 RVA: 0x0002330D File Offset: 0x0002150D
		protected virtual void OnAddComplete(SettingsProperty property)
		{
		}

		// Token: 0x060005DA RID: 1498 RVA: 0x0002330F File Offset: 0x0002150F
		protected virtual void OnClear()
		{
		}

		// Token: 0x060005DB RID: 1499 RVA: 0x00023311 File Offset: 0x00021511
		protected virtual void OnClearComplete()
		{
		}

		// Token: 0x060005DC RID: 1500 RVA: 0x00023313 File Offset: 0x00021513
		protected virtual void OnRemove(SettingsProperty property)
		{
		}

		// Token: 0x060005DD RID: 1501 RVA: 0x00023315 File Offset: 0x00021515
		protected virtual void OnRemoveComplete(SettingsProperty property)
		{
		}

		// Token: 0x170000EE RID: 238
		// (get) Token: 0x060005DE RID: 1502 RVA: 0x00023317 File Offset: 0x00021517
		public int Count
		{
			get
			{
				return this._Hashtable.Count;
			}
		}

		// Token: 0x170000EF RID: 239
		// (get) Token: 0x060005DF RID: 1503 RVA: 0x00023324 File Offset: 0x00021524
		public bool IsSynchronized
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170000F0 RID: 240
		// (get) Token: 0x060005E0 RID: 1504 RVA: 0x00023327 File Offset: 0x00021527
		public object SyncRoot
		{
			get
			{
				return this;
			}
		}

		// Token: 0x060005E1 RID: 1505 RVA: 0x0002332A File Offset: 0x0002152A
		public void CopyTo(Array array, int index)
		{
			this._Hashtable.Values.CopyTo(array, index);
		}

		// Token: 0x060005E2 RID: 1506 RVA: 0x0002333E File Offset: 0x0002153E
		private SettingsPropertyCollection(Hashtable h)
		{
			this._Hashtable = (Hashtable)h.Clone();
		}

		// Token: 0x04000C52 RID: 3154
		private Hashtable _Hashtable;

		// Token: 0x04000C53 RID: 3155
		private bool _ReadOnly;
	}
}
