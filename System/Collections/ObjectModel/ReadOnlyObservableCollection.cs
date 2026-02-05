using System;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace System.Collections.ObjectModel
{
	// Token: 0x020003BB RID: 955
	[TypeForwardedFrom("WindowsBase, Version=3.0.0.0, Culture=Neutral, PublicKeyToken=31bf3856ad364e35")]
	[global::__DynamicallyInvokable]
	[Serializable]
	public class ReadOnlyObservableCollection<T> : ReadOnlyCollection<T>, INotifyCollectionChanged, INotifyPropertyChanged
	{
		// Token: 0x06002400 RID: 9216 RVA: 0x000A8F34 File Offset: 0x000A7134
		[global::__DynamicallyInvokable]
		public ReadOnlyObservableCollection(ObservableCollection<T> list)
			: base(list)
		{
			((INotifyCollectionChanged)base.Items).CollectionChanged += this.HandleCollectionChanged;
			((INotifyPropertyChanged)base.Items).PropertyChanged += this.HandlePropertyChanged;
		}

		// Token: 0x1400002E RID: 46
		// (add) Token: 0x06002401 RID: 9217 RVA: 0x000A8F80 File Offset: 0x000A7180
		// (remove) Token: 0x06002402 RID: 9218 RVA: 0x000A8F89 File Offset: 0x000A7189
		[global::__DynamicallyInvokable]
		event NotifyCollectionChangedEventHandler INotifyCollectionChanged.CollectionChanged
		{
			[global::__DynamicallyInvokable]
			add
			{
				this.CollectionChanged += value;
			}
			[global::__DynamicallyInvokable]
			remove
			{
				this.CollectionChanged -= value;
			}
		}

		// Token: 0x1400002F RID: 47
		// (add) Token: 0x06002403 RID: 9219 RVA: 0x000A8F94 File Offset: 0x000A7194
		// (remove) Token: 0x06002404 RID: 9220 RVA: 0x000A8FCC File Offset: 0x000A71CC
		[global::__DynamicallyInvokable]
		[method: global::__DynamicallyInvokable]
		[field: NonSerialized]
		protected virtual event NotifyCollectionChangedEventHandler CollectionChanged;

		// Token: 0x06002405 RID: 9221 RVA: 0x000A9001 File Offset: 0x000A7201
		[global::__DynamicallyInvokable]
		protected virtual void OnCollectionChanged(NotifyCollectionChangedEventArgs args)
		{
			if (this.CollectionChanged != null)
			{
				this.CollectionChanged(this, args);
			}
		}

		// Token: 0x14000030 RID: 48
		// (add) Token: 0x06002406 RID: 9222 RVA: 0x000A9018 File Offset: 0x000A7218
		// (remove) Token: 0x06002407 RID: 9223 RVA: 0x000A9021 File Offset: 0x000A7221
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

		// Token: 0x14000031 RID: 49
		// (add) Token: 0x06002408 RID: 9224 RVA: 0x000A902C File Offset: 0x000A722C
		// (remove) Token: 0x06002409 RID: 9225 RVA: 0x000A9064 File Offset: 0x000A7264
		[global::__DynamicallyInvokable]
		[method: global::__DynamicallyInvokable]
		[field: NonSerialized]
		protected virtual event PropertyChangedEventHandler PropertyChanged;

		// Token: 0x0600240A RID: 9226 RVA: 0x000A9099 File Offset: 0x000A7299
		[global::__DynamicallyInvokable]
		protected virtual void OnPropertyChanged(PropertyChangedEventArgs args)
		{
			if (this.PropertyChanged != null)
			{
				this.PropertyChanged(this, args);
			}
		}

		// Token: 0x0600240B RID: 9227 RVA: 0x000A90B0 File Offset: 0x000A72B0
		private void HandleCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			this.OnCollectionChanged(e);
		}

		// Token: 0x0600240C RID: 9228 RVA: 0x000A90B9 File Offset: 0x000A72B9
		private void HandlePropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			this.OnPropertyChanged(e);
		}
	}
}
