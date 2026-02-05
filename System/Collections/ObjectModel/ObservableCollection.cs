using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace System.Collections.ObjectModel
{
	// Token: 0x020003BA RID: 954
	[TypeForwardedFrom("WindowsBase, Version=3.0.0.0, Culture=Neutral, PublicKeyToken=31bf3856ad364e35")]
	[global::__DynamicallyInvokable]
	[Serializable]
	public class ObservableCollection<T> : Collection<T>, INotifyCollectionChanged, INotifyPropertyChanged
	{
		// Token: 0x060023E7 RID: 9191 RVA: 0x000A8B54 File Offset: 0x000A6D54
		[global::__DynamicallyInvokable]
		public ObservableCollection()
		{
		}

		// Token: 0x060023E8 RID: 9192 RVA: 0x000A8B67 File Offset: 0x000A6D67
		public ObservableCollection(List<T> list)
			: base((list != null) ? new List<T>(list.Count) : list)
		{
			this.CopyFrom(list);
		}

		// Token: 0x060023E9 RID: 9193 RVA: 0x000A8B92 File Offset: 0x000A6D92
		[global::__DynamicallyInvokable]
		public ObservableCollection(IEnumerable<T> collection)
		{
			if (collection == null)
			{
				throw new ArgumentNullException("collection");
			}
			this.CopyFrom(collection);
		}

		// Token: 0x060023EA RID: 9194 RVA: 0x000A8BBC File Offset: 0x000A6DBC
		private void CopyFrom(IEnumerable<T> collection)
		{
			IList<T> items = base.Items;
			if (collection != null && items != null)
			{
				foreach (T t in collection)
				{
					items.Add(t);
				}
			}
		}

		// Token: 0x060023EB RID: 9195 RVA: 0x000A8C10 File Offset: 0x000A6E10
		[global::__DynamicallyInvokable]
		public void Move(int oldIndex, int newIndex)
		{
			this.MoveItem(oldIndex, newIndex);
		}

		// Token: 0x1400002B RID: 43
		// (add) Token: 0x060023EC RID: 9196 RVA: 0x000A8C1A File Offset: 0x000A6E1A
		// (remove) Token: 0x060023ED RID: 9197 RVA: 0x000A8C23 File Offset: 0x000A6E23
		[global::__DynamicallyInvokable]
		event PropertyChangedEventHandler INotifyPropertyChanged.PropertyChanged
		{
			[global::__DynamicallyInvokable]
			add
			{
				this.PropertyChanged += value;
			}
			[global::__DynamicallyInvokable]
			remove
			{
				this.PropertyChanged -= value;
			}
		}

		// Token: 0x1400002C RID: 44
		// (add) Token: 0x060023EE RID: 9198 RVA: 0x000A8C2C File Offset: 0x000A6E2C
		// (remove) Token: 0x060023EF RID: 9199 RVA: 0x000A8C64 File Offset: 0x000A6E64
		[global::__DynamicallyInvokable]
		[method: global::__DynamicallyInvokable]
		[field: NonSerialized]
		public virtual event NotifyCollectionChangedEventHandler CollectionChanged;

		// Token: 0x060023F0 RID: 9200 RVA: 0x000A8C99 File Offset: 0x000A6E99
		[global::__DynamicallyInvokable]
		protected override void ClearItems()
		{
			this.CheckReentrancy();
			base.ClearItems();
			this.OnPropertyChanged("Count");
			this.OnPropertyChanged("Item[]");
			this.OnCollectionReset();
		}

		// Token: 0x060023F1 RID: 9201 RVA: 0x000A8CC4 File Offset: 0x000A6EC4
		[global::__DynamicallyInvokable]
		protected override void RemoveItem(int index)
		{
			this.CheckReentrancy();
			T t = base[index];
			base.RemoveItem(index);
			this.OnPropertyChanged("Count");
			this.OnPropertyChanged("Item[]");
			this.OnCollectionChanged(NotifyCollectionChangedAction.Remove, t, index);
		}

		// Token: 0x060023F2 RID: 9202 RVA: 0x000A8D0A File Offset: 0x000A6F0A
		[global::__DynamicallyInvokable]
		protected override void InsertItem(int index, T item)
		{
			this.CheckReentrancy();
			base.InsertItem(index, item);
			this.OnPropertyChanged("Count");
			this.OnPropertyChanged("Item[]");
			this.OnCollectionChanged(NotifyCollectionChangedAction.Add, item, index);
		}

		// Token: 0x060023F3 RID: 9203 RVA: 0x000A8D40 File Offset: 0x000A6F40
		[global::__DynamicallyInvokable]
		protected override void SetItem(int index, T item)
		{
			this.CheckReentrancy();
			T t = base[index];
			base.SetItem(index, item);
			this.OnPropertyChanged("Item[]");
			this.OnCollectionChanged(NotifyCollectionChangedAction.Replace, t, item, index);
		}

		// Token: 0x060023F4 RID: 9204 RVA: 0x000A8D84 File Offset: 0x000A6F84
		[global::__DynamicallyInvokable]
		protected virtual void MoveItem(int oldIndex, int newIndex)
		{
			this.CheckReentrancy();
			T t = base[oldIndex];
			base.RemoveItem(oldIndex);
			base.InsertItem(newIndex, t);
			this.OnPropertyChanged("Item[]");
			this.OnCollectionChanged(NotifyCollectionChangedAction.Move, t, newIndex, oldIndex);
		}

		// Token: 0x060023F5 RID: 9205 RVA: 0x000A8DC8 File Offset: 0x000A6FC8
		[global::__DynamicallyInvokable]
		protected virtual void OnPropertyChanged(PropertyChangedEventArgs e)
		{
			if (this.PropertyChanged != null)
			{
				this.PropertyChanged(this, e);
			}
		}

		// Token: 0x1400002D RID: 45
		// (add) Token: 0x060023F6 RID: 9206 RVA: 0x000A8DE0 File Offset: 0x000A6FE0
		// (remove) Token: 0x060023F7 RID: 9207 RVA: 0x000A8E18 File Offset: 0x000A7018
		[global::__DynamicallyInvokable]
		[method: global::__DynamicallyInvokable]
		[field: NonSerialized]
		protected virtual event PropertyChangedEventHandler PropertyChanged;

		// Token: 0x060023F8 RID: 9208 RVA: 0x000A8E50 File Offset: 0x000A7050
		[global::__DynamicallyInvokable]
		protected virtual void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
		{
			if (this.CollectionChanged != null)
			{
				using (this.BlockReentrancy())
				{
					this.CollectionChanged(this, e);
				}
			}
		}

		// Token: 0x060023F9 RID: 9209 RVA: 0x000A8E98 File Offset: 0x000A7098
		[global::__DynamicallyInvokable]
		protected IDisposable BlockReentrancy()
		{
			this._monitor.Enter();
			return this._monitor;
		}

		// Token: 0x060023FA RID: 9210 RVA: 0x000A8EAB File Offset: 0x000A70AB
		[global::__DynamicallyInvokable]
		protected void CheckReentrancy()
		{
			if (this._monitor.Busy && this.CollectionChanged != null && this.CollectionChanged.GetInvocationList().Length > 1)
			{
				throw new InvalidOperationException(SR.GetString("ObservableCollectionReentrancyNotAllowed"));
			}
		}

		// Token: 0x060023FB RID: 9211 RVA: 0x000A8EE2 File Offset: 0x000A70E2
		private void OnPropertyChanged(string propertyName)
		{
			this.OnPropertyChanged(new PropertyChangedEventArgs(propertyName));
		}

		// Token: 0x060023FC RID: 9212 RVA: 0x000A8EF0 File Offset: 0x000A70F0
		private void OnCollectionChanged(NotifyCollectionChangedAction action, object item, int index)
		{
			this.OnCollectionChanged(new NotifyCollectionChangedEventArgs(action, item, index));
		}

		// Token: 0x060023FD RID: 9213 RVA: 0x000A8F00 File Offset: 0x000A7100
		private void OnCollectionChanged(NotifyCollectionChangedAction action, object item, int index, int oldIndex)
		{
			this.OnCollectionChanged(new NotifyCollectionChangedEventArgs(action, item, index, oldIndex));
		}

		// Token: 0x060023FE RID: 9214 RVA: 0x000A8F12 File Offset: 0x000A7112
		private void OnCollectionChanged(NotifyCollectionChangedAction action, object oldItem, object newItem, int index)
		{
			this.OnCollectionChanged(new NotifyCollectionChangedEventArgs(action, newItem, oldItem, index));
		}

		// Token: 0x060023FF RID: 9215 RVA: 0x000A8F24 File Offset: 0x000A7124
		private void OnCollectionReset()
		{
			this.OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
		}

		// Token: 0x04001FED RID: 8173
		private const string CountString = "Count";

		// Token: 0x04001FEE RID: 8174
		private const string IndexerName = "Item[]";

		// Token: 0x04001FEF RID: 8175
		private ObservableCollection<T>.SimpleMonitor _monitor = new ObservableCollection<T>.SimpleMonitor();

		// Token: 0x020007F1 RID: 2033
		[TypeForwardedFrom("WindowsBase, Version=3.0.0.0, Culture=Neutral, PublicKeyToken=31bf3856ad364e35")]
		[Serializable]
		private class SimpleMonitor : IDisposable
		{
			// Token: 0x0600441B RID: 17435 RVA: 0x0011E1F0 File Offset: 0x0011C3F0
			public void Enter()
			{
				this._busyCount++;
			}

			// Token: 0x0600441C RID: 17436 RVA: 0x0011E200 File Offset: 0x0011C400
			public void Dispose()
			{
				this._busyCount--;
			}

			// Token: 0x17000F70 RID: 3952
			// (get) Token: 0x0600441D RID: 17437 RVA: 0x0011E210 File Offset: 0x0011C410
			public bool Busy
			{
				get
				{
					return this._busyCount > 0;
				}
			}

			// Token: 0x04003506 RID: 13574
			private int _busyCount;
		}
	}
}
