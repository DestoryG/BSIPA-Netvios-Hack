using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reflection;
using System.Security.Permissions;

namespace System.ComponentModel
{
	// Token: 0x0200051D RID: 1309
	[HostProtection(SecurityAction.LinkDemand, SharedState = true)]
	[Serializable]
	public class BindingList<T> : Collection<T>, IBindingList, IList, ICollection, IEnumerable, ICancelAddNew, IRaiseItemChangedEvents
	{
		// Token: 0x0600317F RID: 12671 RVA: 0x000DF7AF File Offset: 0x000DD9AF
		public BindingList()
		{
			this.Initialize();
		}

		// Token: 0x06003180 RID: 12672 RVA: 0x000DF7E7 File Offset: 0x000DD9E7
		public BindingList(IList<T> list)
			: base(list)
		{
			this.Initialize();
		}

		// Token: 0x06003181 RID: 12673 RVA: 0x000DF820 File Offset: 0x000DDA20
		private void Initialize()
		{
			this.allowNew = this.ItemTypeHasDefaultConstructor;
			if (typeof(INotifyPropertyChanged).IsAssignableFrom(typeof(T)))
			{
				this.raiseItemChangedEvents = true;
				foreach (T t in base.Items)
				{
					this.HookPropertyChanged(t);
				}
			}
		}

		// Token: 0x17000C18 RID: 3096
		// (get) Token: 0x06003182 RID: 12674 RVA: 0x000DF89C File Offset: 0x000DDA9C
		private bool ItemTypeHasDefaultConstructor
		{
			get
			{
				Type typeFromHandle = typeof(T);
				return typeFromHandle.IsPrimitive || typeFromHandle.GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.CreateInstance, null, new Type[0], null) != null;
			}
		}

		// Token: 0x14000048 RID: 72
		// (add) Token: 0x06003183 RID: 12675 RVA: 0x000DF8DC File Offset: 0x000DDADC
		// (remove) Token: 0x06003184 RID: 12676 RVA: 0x000DF918 File Offset: 0x000DDB18
		public event AddingNewEventHandler AddingNew
		{
			add
			{
				bool flag = this.AllowNew;
				this.onAddingNew = (AddingNewEventHandler)Delegate.Combine(this.onAddingNew, value);
				if (flag != this.AllowNew)
				{
					this.FireListChanged(ListChangedType.Reset, -1);
				}
			}
			remove
			{
				bool flag = this.AllowNew;
				this.onAddingNew = (AddingNewEventHandler)Delegate.Remove(this.onAddingNew, value);
				if (flag != this.AllowNew)
				{
					this.FireListChanged(ListChangedType.Reset, -1);
				}
			}
		}

		// Token: 0x06003185 RID: 12677 RVA: 0x000DF954 File Offset: 0x000DDB54
		protected virtual void OnAddingNew(AddingNewEventArgs e)
		{
			if (this.onAddingNew != null)
			{
				this.onAddingNew(this, e);
			}
		}

		// Token: 0x06003186 RID: 12678 RVA: 0x000DF96C File Offset: 0x000DDB6C
		private object FireAddingNew()
		{
			AddingNewEventArgs addingNewEventArgs = new AddingNewEventArgs(null);
			this.OnAddingNew(addingNewEventArgs);
			return addingNewEventArgs.NewObject;
		}

		// Token: 0x14000049 RID: 73
		// (add) Token: 0x06003187 RID: 12679 RVA: 0x000DF98D File Offset: 0x000DDB8D
		// (remove) Token: 0x06003188 RID: 12680 RVA: 0x000DF9A6 File Offset: 0x000DDBA6
		public event ListChangedEventHandler ListChanged
		{
			add
			{
				this.onListChanged = (ListChangedEventHandler)Delegate.Combine(this.onListChanged, value);
			}
			remove
			{
				this.onListChanged = (ListChangedEventHandler)Delegate.Remove(this.onListChanged, value);
			}
		}

		// Token: 0x06003189 RID: 12681 RVA: 0x000DF9BF File Offset: 0x000DDBBF
		protected virtual void OnListChanged(ListChangedEventArgs e)
		{
			if (this.onListChanged != null)
			{
				this.onListChanged(this, e);
			}
		}

		// Token: 0x17000C19 RID: 3097
		// (get) Token: 0x0600318A RID: 12682 RVA: 0x000DF9D6 File Offset: 0x000DDBD6
		// (set) Token: 0x0600318B RID: 12683 RVA: 0x000DF9DE File Offset: 0x000DDBDE
		public bool RaiseListChangedEvents
		{
			get
			{
				return this.raiseListChangedEvents;
			}
			set
			{
				if (this.raiseListChangedEvents != value)
				{
					this.raiseListChangedEvents = value;
				}
			}
		}

		// Token: 0x0600318C RID: 12684 RVA: 0x000DF9F0 File Offset: 0x000DDBF0
		public void ResetBindings()
		{
			this.FireListChanged(ListChangedType.Reset, -1);
		}

		// Token: 0x0600318D RID: 12685 RVA: 0x000DF9FA File Offset: 0x000DDBFA
		public void ResetItem(int position)
		{
			this.FireListChanged(ListChangedType.ItemChanged, position);
		}

		// Token: 0x0600318E RID: 12686 RVA: 0x000DFA04 File Offset: 0x000DDC04
		private void FireListChanged(ListChangedType type, int index)
		{
			if (this.raiseListChangedEvents)
			{
				this.OnListChanged(new ListChangedEventArgs(type, index));
			}
		}

		// Token: 0x0600318F RID: 12687 RVA: 0x000DFA1C File Offset: 0x000DDC1C
		protected override void ClearItems()
		{
			this.EndNew(this.addNewPos);
			if (this.raiseItemChangedEvents)
			{
				foreach (T t in base.Items)
				{
					this.UnhookPropertyChanged(t);
				}
			}
			base.ClearItems();
			this.FireListChanged(ListChangedType.Reset, -1);
		}

		// Token: 0x06003190 RID: 12688 RVA: 0x000DFA8C File Offset: 0x000DDC8C
		protected override void InsertItem(int index, T item)
		{
			this.EndNew(this.addNewPos);
			base.InsertItem(index, item);
			if (this.raiseItemChangedEvents)
			{
				this.HookPropertyChanged(item);
			}
			this.FireListChanged(ListChangedType.ItemAdded, index);
		}

		// Token: 0x06003191 RID: 12689 RVA: 0x000DFABC File Offset: 0x000DDCBC
		protected override void RemoveItem(int index)
		{
			if (!this.allowRemove && (this.addNewPos < 0 || this.addNewPos != index))
			{
				throw new NotSupportedException();
			}
			this.EndNew(this.addNewPos);
			if (this.raiseItemChangedEvents)
			{
				this.UnhookPropertyChanged(base[index]);
			}
			base.RemoveItem(index);
			this.FireListChanged(ListChangedType.ItemDeleted, index);
		}

		// Token: 0x06003192 RID: 12690 RVA: 0x000DFB19 File Offset: 0x000DDD19
		protected override void SetItem(int index, T item)
		{
			if (this.raiseItemChangedEvents)
			{
				this.UnhookPropertyChanged(base[index]);
			}
			base.SetItem(index, item);
			if (this.raiseItemChangedEvents)
			{
				this.HookPropertyChanged(item);
			}
			this.FireListChanged(ListChangedType.ItemChanged, index);
		}

		// Token: 0x06003193 RID: 12691 RVA: 0x000DFB4F File Offset: 0x000DDD4F
		public virtual void CancelNew(int itemIndex)
		{
			if (this.addNewPos >= 0 && this.addNewPos == itemIndex)
			{
				this.RemoveItem(this.addNewPos);
				this.addNewPos = -1;
			}
		}

		// Token: 0x06003194 RID: 12692 RVA: 0x000DFB76 File Offset: 0x000DDD76
		public virtual void EndNew(int itemIndex)
		{
			if (this.addNewPos >= 0 && this.addNewPos == itemIndex)
			{
				this.addNewPos = -1;
			}
		}

		// Token: 0x06003195 RID: 12693 RVA: 0x000DFB91 File Offset: 0x000DDD91
		public T AddNew()
		{
			return (T)((object)((IBindingList)this).AddNew());
		}

		// Token: 0x06003196 RID: 12694 RVA: 0x000DFBA0 File Offset: 0x000DDDA0
		object IBindingList.AddNew()
		{
			object obj = this.AddNewCore();
			this.addNewPos = ((obj != null) ? base.IndexOf((T)((object)obj)) : (-1));
			return obj;
		}

		// Token: 0x17000C1A RID: 3098
		// (get) Token: 0x06003197 RID: 12695 RVA: 0x000DFBCD File Offset: 0x000DDDCD
		private bool AddingNewHandled
		{
			get
			{
				return this.onAddingNew != null && this.onAddingNew.GetInvocationList().Length != 0;
			}
		}

		// Token: 0x06003198 RID: 12696 RVA: 0x000DFBE8 File Offset: 0x000DDDE8
		protected virtual object AddNewCore()
		{
			object obj = this.FireAddingNew();
			if (obj == null)
			{
				Type typeFromHandle = typeof(T);
				obj = SecurityUtils.SecureCreateInstance(typeFromHandle);
			}
			base.Add((T)((object)obj));
			return obj;
		}

		// Token: 0x17000C1B RID: 3099
		// (get) Token: 0x06003199 RID: 12697 RVA: 0x000DFC1E File Offset: 0x000DDE1E
		// (set) Token: 0x0600319A RID: 12698 RVA: 0x000DFC40 File Offset: 0x000DDE40
		public bool AllowNew
		{
			get
			{
				if (this.userSetAllowNew || this.allowNew)
				{
					return this.allowNew;
				}
				return this.AddingNewHandled;
			}
			set
			{
				bool flag = this.AllowNew;
				this.userSetAllowNew = true;
				this.allowNew = value;
				if (flag != value)
				{
					this.FireListChanged(ListChangedType.Reset, -1);
				}
			}
		}

		// Token: 0x17000C1C RID: 3100
		// (get) Token: 0x0600319B RID: 12699 RVA: 0x000DFC6E File Offset: 0x000DDE6E
		bool IBindingList.AllowNew
		{
			get
			{
				return this.AllowNew;
			}
		}

		// Token: 0x17000C1D RID: 3101
		// (get) Token: 0x0600319C RID: 12700 RVA: 0x000DFC76 File Offset: 0x000DDE76
		// (set) Token: 0x0600319D RID: 12701 RVA: 0x000DFC7E File Offset: 0x000DDE7E
		public bool AllowEdit
		{
			get
			{
				return this.allowEdit;
			}
			set
			{
				if (this.allowEdit != value)
				{
					this.allowEdit = value;
					this.FireListChanged(ListChangedType.Reset, -1);
				}
			}
		}

		// Token: 0x17000C1E RID: 3102
		// (get) Token: 0x0600319E RID: 12702 RVA: 0x000DFC98 File Offset: 0x000DDE98
		bool IBindingList.AllowEdit
		{
			get
			{
				return this.AllowEdit;
			}
		}

		// Token: 0x17000C1F RID: 3103
		// (get) Token: 0x0600319F RID: 12703 RVA: 0x000DFCA0 File Offset: 0x000DDEA0
		// (set) Token: 0x060031A0 RID: 12704 RVA: 0x000DFCA8 File Offset: 0x000DDEA8
		public bool AllowRemove
		{
			get
			{
				return this.allowRemove;
			}
			set
			{
				if (this.allowRemove != value)
				{
					this.allowRemove = value;
					this.FireListChanged(ListChangedType.Reset, -1);
				}
			}
		}

		// Token: 0x17000C20 RID: 3104
		// (get) Token: 0x060031A1 RID: 12705 RVA: 0x000DFCC2 File Offset: 0x000DDEC2
		bool IBindingList.AllowRemove
		{
			get
			{
				return this.AllowRemove;
			}
		}

		// Token: 0x17000C21 RID: 3105
		// (get) Token: 0x060031A2 RID: 12706 RVA: 0x000DFCCA File Offset: 0x000DDECA
		bool IBindingList.SupportsChangeNotification
		{
			get
			{
				return this.SupportsChangeNotificationCore;
			}
		}

		// Token: 0x17000C22 RID: 3106
		// (get) Token: 0x060031A3 RID: 12707 RVA: 0x000DFCD2 File Offset: 0x000DDED2
		protected virtual bool SupportsChangeNotificationCore
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000C23 RID: 3107
		// (get) Token: 0x060031A4 RID: 12708 RVA: 0x000DFCD5 File Offset: 0x000DDED5
		bool IBindingList.SupportsSearching
		{
			get
			{
				return this.SupportsSearchingCore;
			}
		}

		// Token: 0x17000C24 RID: 3108
		// (get) Token: 0x060031A5 RID: 12709 RVA: 0x000DFCDD File Offset: 0x000DDEDD
		protected virtual bool SupportsSearchingCore
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000C25 RID: 3109
		// (get) Token: 0x060031A6 RID: 12710 RVA: 0x000DFCE0 File Offset: 0x000DDEE0
		bool IBindingList.SupportsSorting
		{
			get
			{
				return this.SupportsSortingCore;
			}
		}

		// Token: 0x17000C26 RID: 3110
		// (get) Token: 0x060031A7 RID: 12711 RVA: 0x000DFCE8 File Offset: 0x000DDEE8
		protected virtual bool SupportsSortingCore
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000C27 RID: 3111
		// (get) Token: 0x060031A8 RID: 12712 RVA: 0x000DFCEB File Offset: 0x000DDEEB
		bool IBindingList.IsSorted
		{
			get
			{
				return this.IsSortedCore;
			}
		}

		// Token: 0x17000C28 RID: 3112
		// (get) Token: 0x060031A9 RID: 12713 RVA: 0x000DFCF3 File Offset: 0x000DDEF3
		protected virtual bool IsSortedCore
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000C29 RID: 3113
		// (get) Token: 0x060031AA RID: 12714 RVA: 0x000DFCF6 File Offset: 0x000DDEF6
		PropertyDescriptor IBindingList.SortProperty
		{
			get
			{
				return this.SortPropertyCore;
			}
		}

		// Token: 0x17000C2A RID: 3114
		// (get) Token: 0x060031AB RID: 12715 RVA: 0x000DFCFE File Offset: 0x000DDEFE
		protected virtual PropertyDescriptor SortPropertyCore
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17000C2B RID: 3115
		// (get) Token: 0x060031AC RID: 12716 RVA: 0x000DFD01 File Offset: 0x000DDF01
		ListSortDirection IBindingList.SortDirection
		{
			get
			{
				return this.SortDirectionCore;
			}
		}

		// Token: 0x17000C2C RID: 3116
		// (get) Token: 0x060031AD RID: 12717 RVA: 0x000DFD09 File Offset: 0x000DDF09
		protected virtual ListSortDirection SortDirectionCore
		{
			get
			{
				return ListSortDirection.Ascending;
			}
		}

		// Token: 0x060031AE RID: 12718 RVA: 0x000DFD0C File Offset: 0x000DDF0C
		void IBindingList.ApplySort(PropertyDescriptor prop, ListSortDirection direction)
		{
			this.ApplySortCore(prop, direction);
		}

		// Token: 0x060031AF RID: 12719 RVA: 0x000DFD16 File Offset: 0x000DDF16
		protected virtual void ApplySortCore(PropertyDescriptor prop, ListSortDirection direction)
		{
			throw new NotSupportedException();
		}

		// Token: 0x060031B0 RID: 12720 RVA: 0x000DFD1D File Offset: 0x000DDF1D
		void IBindingList.RemoveSort()
		{
			this.RemoveSortCore();
		}

		// Token: 0x060031B1 RID: 12721 RVA: 0x000DFD25 File Offset: 0x000DDF25
		protected virtual void RemoveSortCore()
		{
			throw new NotSupportedException();
		}

		// Token: 0x060031B2 RID: 12722 RVA: 0x000DFD2C File Offset: 0x000DDF2C
		int IBindingList.Find(PropertyDescriptor prop, object key)
		{
			return this.FindCore(prop, key);
		}

		// Token: 0x060031B3 RID: 12723 RVA: 0x000DFD36 File Offset: 0x000DDF36
		protected virtual int FindCore(PropertyDescriptor prop, object key)
		{
			throw new NotSupportedException();
		}

		// Token: 0x060031B4 RID: 12724 RVA: 0x000DFD3D File Offset: 0x000DDF3D
		void IBindingList.AddIndex(PropertyDescriptor prop)
		{
		}

		// Token: 0x060031B5 RID: 12725 RVA: 0x000DFD3F File Offset: 0x000DDF3F
		void IBindingList.RemoveIndex(PropertyDescriptor prop)
		{
		}

		// Token: 0x060031B6 RID: 12726 RVA: 0x000DFD44 File Offset: 0x000DDF44
		private void HookPropertyChanged(T item)
		{
			INotifyPropertyChanged notifyPropertyChanged = item as INotifyPropertyChanged;
			if (notifyPropertyChanged != null)
			{
				if (this.propertyChangedEventHandler == null)
				{
					this.propertyChangedEventHandler = new PropertyChangedEventHandler(this.Child_PropertyChanged);
				}
				notifyPropertyChanged.PropertyChanged += this.propertyChangedEventHandler;
			}
		}

		// Token: 0x060031B7 RID: 12727 RVA: 0x000DFD88 File Offset: 0x000DDF88
		private void UnhookPropertyChanged(T item)
		{
			INotifyPropertyChanged notifyPropertyChanged = item as INotifyPropertyChanged;
			if (notifyPropertyChanged != null && this.propertyChangedEventHandler != null)
			{
				notifyPropertyChanged.PropertyChanged -= this.propertyChangedEventHandler;
			}
		}

		// Token: 0x060031B8 RID: 12728 RVA: 0x000DFDB8 File Offset: 0x000DDFB8
		private void Child_PropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			if (this.RaiseListChangedEvents)
			{
				if (sender == null || e == null || string.IsNullOrEmpty(e.PropertyName))
				{
					this.ResetBindings();
					return;
				}
				T t;
				try
				{
					t = (T)((object)sender);
				}
				catch (InvalidCastException)
				{
					this.ResetBindings();
					return;
				}
				int num = this.lastChangeIndex;
				if (num >= 0 && num < base.Count)
				{
					T t2 = base[num];
					if (t2.Equals(t))
					{
						goto IL_007B;
					}
				}
				num = base.IndexOf(t);
				this.lastChangeIndex = num;
				IL_007B:
				if (num == -1)
				{
					this.UnhookPropertyChanged(t);
					this.ResetBindings();
					return;
				}
				if (this.itemTypeProperties == null)
				{
					this.itemTypeProperties = TypeDescriptor.GetProperties(typeof(T));
				}
				PropertyDescriptor propertyDescriptor = this.itemTypeProperties.Find(e.PropertyName, true);
				ListChangedEventArgs listChangedEventArgs = new ListChangedEventArgs(ListChangedType.ItemChanged, num, propertyDescriptor);
				this.OnListChanged(listChangedEventArgs);
			}
		}

		// Token: 0x17000C2D RID: 3117
		// (get) Token: 0x060031B9 RID: 12729 RVA: 0x000DFEA4 File Offset: 0x000DE0A4
		bool IRaiseItemChangedEvents.RaisesItemChangedEvents
		{
			get
			{
				return this.raiseItemChangedEvents;
			}
		}

		// Token: 0x04002925 RID: 10533
		private int addNewPos = -1;

		// Token: 0x04002926 RID: 10534
		private bool raiseListChangedEvents = true;

		// Token: 0x04002927 RID: 10535
		private bool raiseItemChangedEvents;

		// Token: 0x04002928 RID: 10536
		[NonSerialized]
		private PropertyDescriptorCollection itemTypeProperties;

		// Token: 0x04002929 RID: 10537
		[NonSerialized]
		private PropertyChangedEventHandler propertyChangedEventHandler;

		// Token: 0x0400292A RID: 10538
		[NonSerialized]
		private AddingNewEventHandler onAddingNew;

		// Token: 0x0400292B RID: 10539
		[NonSerialized]
		private ListChangedEventHandler onListChanged;

		// Token: 0x0400292C RID: 10540
		[NonSerialized]
		private int lastChangeIndex = -1;

		// Token: 0x0400292D RID: 10541
		private bool allowNew = true;

		// Token: 0x0400292E RID: 10542
		private bool allowEdit = true;

		// Token: 0x0400292F RID: 10543
		private bool allowRemove = true;

		// Token: 0x04002930 RID: 10544
		private bool userSetAllowNew;
	}
}
