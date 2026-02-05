using System;
using System.Runtime.CompilerServices;

namespace System.Collections.Specialized
{
	// Token: 0x020003B2 RID: 946
	[TypeForwardedFrom("WindowsBase, Version=3.0.0.0, Culture=Neutral, PublicKeyToken=31bf3856ad364e35")]
	[global::__DynamicallyInvokable]
	public class NotifyCollectionChangedEventArgs : EventArgs
	{
		// Token: 0x06002370 RID: 9072 RVA: 0x000A7AA4 File Offset: 0x000A5CA4
		[global::__DynamicallyInvokable]
		public NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction action)
		{
			if (action != NotifyCollectionChangedAction.Reset)
			{
				throw new ArgumentException(SR.GetString("WrongActionForCtor", new object[] { NotifyCollectionChangedAction.Reset }), "action");
			}
			this.InitializeAdd(action, null, -1);
		}

		// Token: 0x06002371 RID: 9073 RVA: 0x000A7AF8 File Offset: 0x000A5CF8
		[global::__DynamicallyInvokable]
		public NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction action, object changedItem)
		{
			if (action != NotifyCollectionChangedAction.Add && action != NotifyCollectionChangedAction.Remove && action != NotifyCollectionChangedAction.Reset)
			{
				throw new ArgumentException(SR.GetString("MustBeResetAddOrRemoveActionForCtor"), "action");
			}
			if (action != NotifyCollectionChangedAction.Reset)
			{
				this.InitializeAddOrRemove(action, new object[] { changedItem }, -1);
				return;
			}
			if (changedItem != null)
			{
				throw new ArgumentException(SR.GetString("ResetActionRequiresNullItem"), "action");
			}
			this.InitializeAdd(action, null, -1);
		}

		// Token: 0x06002372 RID: 9074 RVA: 0x000A7B74 File Offset: 0x000A5D74
		[global::__DynamicallyInvokable]
		public NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction action, object changedItem, int index)
		{
			if (action != NotifyCollectionChangedAction.Add && action != NotifyCollectionChangedAction.Remove && action != NotifyCollectionChangedAction.Reset)
			{
				throw new ArgumentException(SR.GetString("MustBeResetAddOrRemoveActionForCtor"), "action");
			}
			if (action != NotifyCollectionChangedAction.Reset)
			{
				this.InitializeAddOrRemove(action, new object[] { changedItem }, index);
				return;
			}
			if (changedItem != null)
			{
				throw new ArgumentException(SR.GetString("ResetActionRequiresNullItem"), "action");
			}
			if (index != -1)
			{
				throw new ArgumentException(SR.GetString("ResetActionRequiresIndexMinus1"), "action");
			}
			this.InitializeAdd(action, null, -1);
		}

		// Token: 0x06002373 RID: 9075 RVA: 0x000A7C08 File Offset: 0x000A5E08
		[global::__DynamicallyInvokable]
		public NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction action, IList changedItems)
		{
			if (action != NotifyCollectionChangedAction.Add && action != NotifyCollectionChangedAction.Remove && action != NotifyCollectionChangedAction.Reset)
			{
				throw new ArgumentException(SR.GetString("MustBeResetAddOrRemoveActionForCtor"), "action");
			}
			if (action == NotifyCollectionChangedAction.Reset)
			{
				if (changedItems != null)
				{
					throw new ArgumentException(SR.GetString("ResetActionRequiresNullItem"), "action");
				}
				this.InitializeAdd(action, null, -1);
				return;
			}
			else
			{
				if (changedItems == null)
				{
					throw new ArgumentNullException("changedItems");
				}
				this.InitializeAddOrRemove(action, changedItems, -1);
				return;
			}
		}

		// Token: 0x06002374 RID: 9076 RVA: 0x000A7C88 File Offset: 0x000A5E88
		[global::__DynamicallyInvokable]
		public NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction action, IList changedItems, int startingIndex)
		{
			if (action != NotifyCollectionChangedAction.Add && action != NotifyCollectionChangedAction.Remove && action != NotifyCollectionChangedAction.Reset)
			{
				throw new ArgumentException(SR.GetString("MustBeResetAddOrRemoveActionForCtor"), "action");
			}
			if (action == NotifyCollectionChangedAction.Reset)
			{
				if (changedItems != null)
				{
					throw new ArgumentException(SR.GetString("ResetActionRequiresNullItem"), "action");
				}
				if (startingIndex != -1)
				{
					throw new ArgumentException(SR.GetString("ResetActionRequiresIndexMinus1"), "action");
				}
				this.InitializeAdd(action, null, -1);
				return;
			}
			else
			{
				if (changedItems == null)
				{
					throw new ArgumentNullException("changedItems");
				}
				if (startingIndex < -1)
				{
					throw new ArgumentException(SR.GetString("IndexCannotBeNegative"), "startingIndex");
				}
				this.InitializeAddOrRemove(action, changedItems, startingIndex);
				return;
			}
		}

		// Token: 0x06002375 RID: 9077 RVA: 0x000A7D38 File Offset: 0x000A5F38
		[global::__DynamicallyInvokable]
		public NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction action, object newItem, object oldItem)
		{
			if (action != NotifyCollectionChangedAction.Replace)
			{
				throw new ArgumentException(SR.GetString("WrongActionForCtor", new object[] { NotifyCollectionChangedAction.Replace }), "action");
			}
			this.InitializeMoveOrReplace(action, new object[] { newItem }, new object[] { oldItem }, -1, -1);
		}

		// Token: 0x06002376 RID: 9078 RVA: 0x000A7DA0 File Offset: 0x000A5FA0
		[global::__DynamicallyInvokable]
		public NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction action, object newItem, object oldItem, int index)
		{
			if (action != NotifyCollectionChangedAction.Replace)
			{
				throw new ArgumentException(SR.GetString("WrongActionForCtor", new object[] { NotifyCollectionChangedAction.Replace }), "action");
			}
			this.InitializeMoveOrReplace(action, new object[] { newItem }, new object[] { oldItem }, index, index);
		}

		// Token: 0x06002377 RID: 9079 RVA: 0x000A7E0C File Offset: 0x000A600C
		[global::__DynamicallyInvokable]
		public NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction action, IList newItems, IList oldItems)
		{
			if (action != NotifyCollectionChangedAction.Replace)
			{
				throw new ArgumentException(SR.GetString("WrongActionForCtor", new object[] { NotifyCollectionChangedAction.Replace }), "action");
			}
			if (newItems == null)
			{
				throw new ArgumentNullException("newItems");
			}
			if (oldItems == null)
			{
				throw new ArgumentNullException("oldItems");
			}
			this.InitializeMoveOrReplace(action, newItems, oldItems, -1, -1);
		}

		// Token: 0x06002378 RID: 9080 RVA: 0x000A7E7C File Offset: 0x000A607C
		[global::__DynamicallyInvokable]
		public NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction action, IList newItems, IList oldItems, int startingIndex)
		{
			if (action != NotifyCollectionChangedAction.Replace)
			{
				throw new ArgumentException(SR.GetString("WrongActionForCtor", new object[] { NotifyCollectionChangedAction.Replace }), "action");
			}
			if (newItems == null)
			{
				throw new ArgumentNullException("newItems");
			}
			if (oldItems == null)
			{
				throw new ArgumentNullException("oldItems");
			}
			this.InitializeMoveOrReplace(action, newItems, oldItems, startingIndex, startingIndex);
		}

		// Token: 0x06002379 RID: 9081 RVA: 0x000A7EF0 File Offset: 0x000A60F0
		[global::__DynamicallyInvokable]
		public NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction action, object changedItem, int index, int oldIndex)
		{
			if (action != NotifyCollectionChangedAction.Move)
			{
				throw new ArgumentException(SR.GetString("WrongActionForCtor", new object[] { NotifyCollectionChangedAction.Move }), "action");
			}
			if (index < 0)
			{
				throw new ArgumentException(SR.GetString("IndexCannotBeNegative"), "index");
			}
			object[] array = new object[] { changedItem };
			this.InitializeMoveOrReplace(action, array, array, index, oldIndex);
		}

		// Token: 0x0600237A RID: 9082 RVA: 0x000A7F6C File Offset: 0x000A616C
		[global::__DynamicallyInvokable]
		public NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction action, IList changedItems, int index, int oldIndex)
		{
			if (action != NotifyCollectionChangedAction.Move)
			{
				throw new ArgumentException(SR.GetString("WrongActionForCtor", new object[] { NotifyCollectionChangedAction.Move }), "action");
			}
			if (index < 0)
			{
				throw new ArgumentException(SR.GetString("IndexCannotBeNegative"), "index");
			}
			this.InitializeMoveOrReplace(action, changedItems, changedItems, index, oldIndex);
		}

		// Token: 0x0600237B RID: 9083 RVA: 0x000A7FDC File Offset: 0x000A61DC
		internal NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction action, IList newItems, IList oldItems, int newIndex, int oldIndex)
		{
			this._action = action;
			this._newItems = ((newItems == null) ? null : ArrayList.ReadOnly(newItems));
			this._oldItems = ((oldItems == null) ? null : ArrayList.ReadOnly(oldItems));
			this._newStartingIndex = newIndex;
			this._oldStartingIndex = oldIndex;
		}

		// Token: 0x0600237C RID: 9084 RVA: 0x000A8038 File Offset: 0x000A6238
		private void InitializeAddOrRemove(NotifyCollectionChangedAction action, IList changedItems, int startingIndex)
		{
			if (action == NotifyCollectionChangedAction.Add)
			{
				this.InitializeAdd(action, changedItems, startingIndex);
				return;
			}
			if (action == NotifyCollectionChangedAction.Remove)
			{
				this.InitializeRemove(action, changedItems, startingIndex);
			}
		}

		// Token: 0x0600237D RID: 9085 RVA: 0x000A8054 File Offset: 0x000A6254
		private void InitializeAdd(NotifyCollectionChangedAction action, IList newItems, int newStartingIndex)
		{
			this._action = action;
			this._newItems = ((newItems == null) ? null : ArrayList.ReadOnly(newItems));
			this._newStartingIndex = newStartingIndex;
		}

		// Token: 0x0600237E RID: 9086 RVA: 0x000A8076 File Offset: 0x000A6276
		private void InitializeRemove(NotifyCollectionChangedAction action, IList oldItems, int oldStartingIndex)
		{
			this._action = action;
			this._oldItems = ((oldItems == null) ? null : ArrayList.ReadOnly(oldItems));
			this._oldStartingIndex = oldStartingIndex;
		}

		// Token: 0x0600237F RID: 9087 RVA: 0x000A8098 File Offset: 0x000A6298
		private void InitializeMoveOrReplace(NotifyCollectionChangedAction action, IList newItems, IList oldItems, int startingIndex, int oldStartingIndex)
		{
			this.InitializeAdd(action, newItems, startingIndex);
			this.InitializeRemove(action, oldItems, oldStartingIndex);
		}

		// Token: 0x170008F8 RID: 2296
		// (get) Token: 0x06002380 RID: 9088 RVA: 0x000A80AE File Offset: 0x000A62AE
		[global::__DynamicallyInvokable]
		public NotifyCollectionChangedAction Action
		{
			[global::__DynamicallyInvokable]
			get
			{
				return this._action;
			}
		}

		// Token: 0x170008F9 RID: 2297
		// (get) Token: 0x06002381 RID: 9089 RVA: 0x000A80B6 File Offset: 0x000A62B6
		[global::__DynamicallyInvokable]
		public IList NewItems
		{
			[global::__DynamicallyInvokable]
			get
			{
				return this._newItems;
			}
		}

		// Token: 0x170008FA RID: 2298
		// (get) Token: 0x06002382 RID: 9090 RVA: 0x000A80BE File Offset: 0x000A62BE
		[global::__DynamicallyInvokable]
		public IList OldItems
		{
			[global::__DynamicallyInvokable]
			get
			{
				return this._oldItems;
			}
		}

		// Token: 0x170008FB RID: 2299
		// (get) Token: 0x06002383 RID: 9091 RVA: 0x000A80C6 File Offset: 0x000A62C6
		[global::__DynamicallyInvokable]
		public int NewStartingIndex
		{
			[global::__DynamicallyInvokable]
			get
			{
				return this._newStartingIndex;
			}
		}

		// Token: 0x170008FC RID: 2300
		// (get) Token: 0x06002384 RID: 9092 RVA: 0x000A80CE File Offset: 0x000A62CE
		[global::__DynamicallyInvokable]
		public int OldStartingIndex
		{
			[global::__DynamicallyInvokable]
			get
			{
				return this._oldStartingIndex;
			}
		}

		// Token: 0x04001FD6 RID: 8150
		private NotifyCollectionChangedAction _action;

		// Token: 0x04001FD7 RID: 8151
		private IList _newItems;

		// Token: 0x04001FD8 RID: 8152
		private IList _oldItems;

		// Token: 0x04001FD9 RID: 8153
		private int _newStartingIndex = -1;

		// Token: 0x04001FDA RID: 8154
		private int _oldStartingIndex = -1;
	}
}
