using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Security.Permissions;
using System.Threading;

namespace System.Collections.Concurrent
{
	// Token: 0x020003D2 RID: 978
	[ComVisible(false)]
	[DebuggerTypeProxy(typeof(SystemThreadingCollection_IProducerConsumerCollectionDebugView<>))]
	[DebuggerDisplay("Count = {Count}")]
	[global::__DynamicallyInvokable]
	[HostProtection(SecurityAction.LinkDemand, Synchronization = true, ExternalThreading = true)]
	[Serializable]
	public class ConcurrentBag<T> : IProducerConsumerCollection<T>, IEnumerable<T>, IEnumerable, ICollection, IReadOnlyCollection<T>
	{
		// Token: 0x06002589 RID: 9609 RVA: 0x000AE73E File Offset: 0x000AC93E
		[global::__DynamicallyInvokable]
		public ConcurrentBag()
		{
			this.Initialize(null);
		}

		// Token: 0x0600258A RID: 9610 RVA: 0x000AE74D File Offset: 0x000AC94D
		[global::__DynamicallyInvokable]
		public ConcurrentBag(IEnumerable<T> collection)
		{
			if (collection == null)
			{
				throw new ArgumentNullException("collection", SR.GetString("ConcurrentBag_Ctor_ArgumentNullException"));
			}
			this.Initialize(collection);
		}

		// Token: 0x0600258B RID: 9611 RVA: 0x000AE774 File Offset: 0x000AC974
		private void Initialize(IEnumerable<T> collection)
		{
			this.m_locals = new ThreadLocal<ConcurrentBag<T>.ThreadLocalList>();
			if (collection != null)
			{
				ConcurrentBag<T>.ThreadLocalList threadList = this.GetThreadList(true);
				foreach (T t in collection)
				{
					threadList.Add(t, false);
				}
			}
		}

		// Token: 0x0600258C RID: 9612 RVA: 0x000AE7D4 File Offset: 0x000AC9D4
		[global::__DynamicallyInvokable]
		public void Add(T item)
		{
			ConcurrentBag<T>.ThreadLocalList threadList = this.GetThreadList(true);
			this.AddInternal(threadList, item);
		}

		// Token: 0x0600258D RID: 9613 RVA: 0x000AE7F4 File Offset: 0x000AC9F4
		private void AddInternal(ConcurrentBag<T>.ThreadLocalList list, T item)
		{
			bool flag = false;
			try
			{
				Interlocked.Exchange(ref list.m_currentOp, 1);
				if (list.Count < 2 || this.m_needSync)
				{
					list.m_currentOp = 0;
					Monitor.Enter(list, ref flag);
				}
				list.Add(item, flag);
			}
			finally
			{
				list.m_currentOp = 0;
				if (flag)
				{
					Monitor.Exit(list);
				}
			}
		}

		// Token: 0x0600258E RID: 9614 RVA: 0x000AE860 File Offset: 0x000ACA60
		[global::__DynamicallyInvokable]
		bool IProducerConsumerCollection<T>.TryAdd(T item)
		{
			this.Add(item);
			return true;
		}

		// Token: 0x0600258F RID: 9615 RVA: 0x000AE86A File Offset: 0x000ACA6A
		[global::__DynamicallyInvokable]
		public bool TryTake(out T result)
		{
			return this.TryTakeOrPeek(out result, true);
		}

		// Token: 0x06002590 RID: 9616 RVA: 0x000AE874 File Offset: 0x000ACA74
		[global::__DynamicallyInvokable]
		public bool TryPeek(out T result)
		{
			return this.TryTakeOrPeek(out result, false);
		}

		// Token: 0x06002591 RID: 9617 RVA: 0x000AE880 File Offset: 0x000ACA80
		private bool TryTakeOrPeek(out T result, bool take)
		{
			ConcurrentBag<T>.ThreadLocalList threadList = this.GetThreadList(false);
			if (threadList == null || threadList.Count == 0)
			{
				return this.Steal(out result, take);
			}
			bool flag = false;
			try
			{
				if (take)
				{
					Interlocked.Exchange(ref threadList.m_currentOp, 2);
					if (threadList.Count <= 2 || this.m_needSync)
					{
						threadList.m_currentOp = 0;
						Monitor.Enter(threadList, ref flag);
						if (threadList.Count == 0)
						{
							if (flag)
							{
								try
								{
								}
								finally
								{
									flag = false;
									Monitor.Exit(threadList);
								}
							}
							return this.Steal(out result, true);
						}
					}
					threadList.Remove(out result);
				}
				else if (!threadList.Peek(out result))
				{
					return this.Steal(out result, false);
				}
			}
			finally
			{
				threadList.m_currentOp = 0;
				if (flag)
				{
					Monitor.Exit(threadList);
				}
			}
			return true;
		}

		// Token: 0x06002592 RID: 9618 RVA: 0x000AE950 File Offset: 0x000ACB50
		private ConcurrentBag<T>.ThreadLocalList GetThreadList(bool forceCreate)
		{
			ConcurrentBag<T>.ThreadLocalList threadLocalList = this.m_locals.Value;
			if (threadLocalList != null)
			{
				return threadLocalList;
			}
			if (forceCreate)
			{
				object globalListsLock = this.GlobalListsLock;
				lock (globalListsLock)
				{
					if (this.m_headList == null)
					{
						threadLocalList = new ConcurrentBag<T>.ThreadLocalList(Thread.CurrentThread);
						this.m_headList = threadLocalList;
						this.m_tailList = threadLocalList;
					}
					else
					{
						threadLocalList = this.GetUnownedList();
						if (threadLocalList == null)
						{
							threadLocalList = new ConcurrentBag<T>.ThreadLocalList(Thread.CurrentThread);
							this.m_tailList.m_nextList = threadLocalList;
							this.m_tailList = threadLocalList;
						}
					}
					this.m_locals.Value = threadLocalList;
					return threadLocalList;
				}
			}
			return null;
		}

		// Token: 0x06002593 RID: 9619 RVA: 0x000AEA08 File Offset: 0x000ACC08
		private ConcurrentBag<T>.ThreadLocalList GetUnownedList()
		{
			for (ConcurrentBag<T>.ThreadLocalList threadLocalList = this.m_headList; threadLocalList != null; threadLocalList = threadLocalList.m_nextList)
			{
				if (threadLocalList.m_ownerThread.ThreadState == global::System.Threading.ThreadState.Stopped)
				{
					threadLocalList.m_ownerThread = Thread.CurrentThread;
					return threadLocalList;
				}
			}
			return null;
		}

		// Token: 0x06002594 RID: 9620 RVA: 0x000AEA4C File Offset: 0x000ACC4C
		private bool Steal(out T result, bool take)
		{
			if (take)
			{
				CDSCollectionETWBCLProvider.Log.ConcurrentBag_TryTakeSteals();
			}
			else
			{
				CDSCollectionETWBCLProvider.Log.ConcurrentBag_TryPeekSteals();
			}
			List<int> list = new List<int>();
			for (;;)
			{
				list.Clear();
				bool flag = false;
				ConcurrentBag<T>.ThreadLocalList threadLocalList;
				for (threadLocalList = this.m_headList; threadLocalList != null; threadLocalList = threadLocalList.m_nextList)
				{
					list.Add(threadLocalList.m_version);
					if (threadLocalList.m_head != null && this.TrySteal(threadLocalList, out result, take))
					{
						return true;
					}
				}
				threadLocalList = this.m_headList;
				foreach (int num in list)
				{
					if (num != threadLocalList.m_version)
					{
						flag = true;
						if (threadLocalList.m_head != null && this.TrySteal(threadLocalList, out result, take))
						{
							return true;
						}
					}
					threadLocalList = threadLocalList.m_nextList;
				}
				if (!flag)
				{
					goto Block_6;
				}
			}
			return true;
			Block_6:
			result = default(T);
			return false;
		}

		// Token: 0x06002595 RID: 9621 RVA: 0x000AEB44 File Offset: 0x000ACD44
		private bool TrySteal(ConcurrentBag<T>.ThreadLocalList list, out T result, bool take)
		{
			bool flag2;
			lock (list)
			{
				if (this.CanSteal(list))
				{
					list.Steal(out result, take);
					flag2 = true;
				}
				else
				{
					result = default(T);
					flag2 = false;
				}
			}
			return flag2;
		}

		// Token: 0x06002596 RID: 9622 RVA: 0x000AEB98 File Offset: 0x000ACD98
		private bool CanSteal(ConcurrentBag<T>.ThreadLocalList list)
		{
			if (list.Count <= 2 && list.m_currentOp != 0)
			{
				SpinWait spinWait = default(SpinWait);
				while (list.m_currentOp != 0)
				{
					spinWait.SpinOnce();
				}
			}
			return list.Count > 0;
		}

		// Token: 0x06002597 RID: 9623 RVA: 0x000AEBE0 File Offset: 0x000ACDE0
		[global::__DynamicallyInvokable]
		public void CopyTo(T[] array, int index)
		{
			if (array == null)
			{
				throw new ArgumentNullException("array", SR.GetString("ConcurrentBag_CopyTo_ArgumentNullException"));
			}
			if (index < 0)
			{
				throw new ArgumentOutOfRangeException("index", SR.GetString("ConcurrentBag_CopyTo_ArgumentOutOfRangeException"));
			}
			if (this.m_headList == null)
			{
				return;
			}
			bool flag = false;
			try
			{
				this.FreezeBag(ref flag);
				this.ToList().CopyTo(array, index);
			}
			finally
			{
				this.UnfreezeBag(flag);
			}
		}

		// Token: 0x06002598 RID: 9624 RVA: 0x000AEC5C File Offset: 0x000ACE5C
		[global::__DynamicallyInvokable]
		void ICollection.CopyTo(Array array, int index)
		{
			if (array == null)
			{
				throw new ArgumentNullException("array", SR.GetString("ConcurrentBag_CopyTo_ArgumentNullException"));
			}
			bool flag = false;
			try
			{
				this.FreezeBag(ref flag);
				((ICollection)this.ToList()).CopyTo(array, index);
			}
			finally
			{
				this.UnfreezeBag(flag);
			}
		}

		// Token: 0x06002599 RID: 9625 RVA: 0x000AECB4 File Offset: 0x000ACEB4
		[global::__DynamicallyInvokable]
		public T[] ToArray()
		{
			if (this.m_headList == null)
			{
				return new T[0];
			}
			bool flag = false;
			T[] array;
			try
			{
				this.FreezeBag(ref flag);
				array = this.ToList().ToArray();
			}
			finally
			{
				this.UnfreezeBag(flag);
			}
			return array;
		}

		// Token: 0x0600259A RID: 9626 RVA: 0x000AED04 File Offset: 0x000ACF04
		[global::__DynamicallyInvokable]
		public IEnumerator<T> GetEnumerator()
		{
			if (this.m_headList == null)
			{
				return new List<T>().GetEnumerator();
			}
			bool flag = false;
			IEnumerator<T> enumerator;
			try
			{
				this.FreezeBag(ref flag);
				enumerator = this.ToList().GetEnumerator();
			}
			finally
			{
				this.UnfreezeBag(flag);
			}
			return enumerator;
		}

		// Token: 0x0600259B RID: 9627 RVA: 0x000AED64 File Offset: 0x000ACF64
		[global::__DynamicallyInvokable]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		// Token: 0x0600259C RID: 9628 RVA: 0x000AED6C File Offset: 0x000ACF6C
		[OnSerializing]
		private void OnSerializing(StreamingContext context)
		{
			this.m_serializationArray = this.ToArray();
		}

		// Token: 0x0600259D RID: 9629 RVA: 0x000AED7C File Offset: 0x000ACF7C
		[OnDeserialized]
		private void OnDeserialized(StreamingContext context)
		{
			this.m_locals = new ThreadLocal<ConcurrentBag<T>.ThreadLocalList>();
			ConcurrentBag<T>.ThreadLocalList threadList = this.GetThreadList(true);
			foreach (T t in this.m_serializationArray)
			{
				threadList.Add(t, false);
			}
			this.m_headList = threadList;
			this.m_tailList = threadList;
			this.m_serializationArray = null;
		}

		// Token: 0x17000961 RID: 2401
		// (get) Token: 0x0600259E RID: 9630 RVA: 0x000AEDDC File Offset: 0x000ACFDC
		[global::__DynamicallyInvokable]
		public int Count
		{
			[global::__DynamicallyInvokable]
			get
			{
				if (this.m_headList == null)
				{
					return 0;
				}
				bool flag = false;
				int countInternal;
				try
				{
					this.FreezeBag(ref flag);
					countInternal = this.GetCountInternal();
				}
				finally
				{
					this.UnfreezeBag(flag);
				}
				return countInternal;
			}
		}

		// Token: 0x17000962 RID: 2402
		// (get) Token: 0x0600259F RID: 9631 RVA: 0x000AEE24 File Offset: 0x000AD024
		[global::__DynamicallyInvokable]
		public bool IsEmpty
		{
			[global::__DynamicallyInvokable]
			get
			{
				if (this.m_headList == null)
				{
					return true;
				}
				bool flag = false;
				bool flag2;
				try
				{
					this.FreezeBag(ref flag);
					for (ConcurrentBag<T>.ThreadLocalList threadLocalList = this.m_headList; threadLocalList != null; threadLocalList = threadLocalList.m_nextList)
					{
						if (threadLocalList.m_head != null)
						{
							return false;
						}
					}
					flag2 = true;
				}
				finally
				{
					this.UnfreezeBag(flag);
				}
				return flag2;
			}
		}

		// Token: 0x17000963 RID: 2403
		// (get) Token: 0x060025A0 RID: 9632 RVA: 0x000AEE8C File Offset: 0x000AD08C
		[global::__DynamicallyInvokable]
		bool ICollection.IsSynchronized
		{
			[global::__DynamicallyInvokable]
			get
			{
				return false;
			}
		}

		// Token: 0x17000964 RID: 2404
		// (get) Token: 0x060025A1 RID: 9633 RVA: 0x000AEE8F File Offset: 0x000AD08F
		[global::__DynamicallyInvokable]
		object ICollection.SyncRoot
		{
			[global::__DynamicallyInvokable]
			get
			{
				throw new NotSupportedException(SR.GetString("ConcurrentCollection_SyncRoot_NotSupported"));
			}
		}

		// Token: 0x17000965 RID: 2405
		// (get) Token: 0x060025A2 RID: 9634 RVA: 0x000AEEA0 File Offset: 0x000AD0A0
		private object GlobalListsLock
		{
			get
			{
				return this.m_locals;
			}
		}

		// Token: 0x060025A3 RID: 9635 RVA: 0x000AEEA8 File Offset: 0x000AD0A8
		private void FreezeBag(ref bool lockTaken)
		{
			Monitor.Enter(this.GlobalListsLock, ref lockTaken);
			this.m_needSync = true;
			this.AcquireAllLocks();
			this.WaitAllOperations();
		}

		// Token: 0x060025A4 RID: 9636 RVA: 0x000AEEC9 File Offset: 0x000AD0C9
		private void UnfreezeBag(bool lockTaken)
		{
			this.ReleaseAllLocks();
			this.m_needSync = false;
			if (lockTaken)
			{
				Monitor.Exit(this.GlobalListsLock);
			}
		}

		// Token: 0x060025A5 RID: 9637 RVA: 0x000AEEE8 File Offset: 0x000AD0E8
		private void AcquireAllLocks()
		{
			bool flag = false;
			for (ConcurrentBag<T>.ThreadLocalList threadLocalList = this.m_headList; threadLocalList != null; threadLocalList = threadLocalList.m_nextList)
			{
				try
				{
					Monitor.Enter(threadLocalList, ref flag);
				}
				finally
				{
					if (flag)
					{
						threadLocalList.m_lockTaken = true;
						flag = false;
					}
				}
			}
		}

		// Token: 0x060025A6 RID: 9638 RVA: 0x000AEF38 File Offset: 0x000AD138
		private void ReleaseAllLocks()
		{
			for (ConcurrentBag<T>.ThreadLocalList threadLocalList = this.m_headList; threadLocalList != null; threadLocalList = threadLocalList.m_nextList)
			{
				if (threadLocalList.m_lockTaken)
				{
					threadLocalList.m_lockTaken = false;
					Monitor.Exit(threadLocalList);
				}
			}
		}

		// Token: 0x060025A7 RID: 9639 RVA: 0x000AEF74 File Offset: 0x000AD174
		private void WaitAllOperations()
		{
			for (ConcurrentBag<T>.ThreadLocalList threadLocalList = this.m_headList; threadLocalList != null; threadLocalList = threadLocalList.m_nextList)
			{
				if (threadLocalList.m_currentOp != 0)
				{
					SpinWait spinWait = default(SpinWait);
					while (threadLocalList.m_currentOp != 0)
					{
						spinWait.SpinOnce();
					}
				}
			}
		}

		// Token: 0x060025A8 RID: 9640 RVA: 0x000AEFC0 File Offset: 0x000AD1C0
		private int GetCountInternal()
		{
			int num = 0;
			checked
			{
				for (ConcurrentBag<T>.ThreadLocalList threadLocalList = this.m_headList; threadLocalList != null; threadLocalList = threadLocalList.m_nextList)
				{
					num += threadLocalList.Count;
				}
				return num;
			}
		}

		// Token: 0x060025A9 RID: 9641 RVA: 0x000AEFF0 File Offset: 0x000AD1F0
		private List<T> ToList()
		{
			List<T> list = new List<T>();
			for (ConcurrentBag<T>.ThreadLocalList threadLocalList = this.m_headList; threadLocalList != null; threadLocalList = threadLocalList.m_nextList)
			{
				for (ConcurrentBag<T>.Node node = threadLocalList.m_head; node != null; node = node.m_next)
				{
					list.Add(node.m_value);
				}
			}
			return list;
		}

		// Token: 0x04002051 RID: 8273
		[NonSerialized]
		private ThreadLocal<ConcurrentBag<T>.ThreadLocalList> m_locals;

		// Token: 0x04002052 RID: 8274
		[NonSerialized]
		private volatile ConcurrentBag<T>.ThreadLocalList m_headList;

		// Token: 0x04002053 RID: 8275
		[NonSerialized]
		private volatile ConcurrentBag<T>.ThreadLocalList m_tailList;

		// Token: 0x04002054 RID: 8276
		[NonSerialized]
		private bool m_needSync;

		// Token: 0x04002055 RID: 8277
		private T[] m_serializationArray;

		// Token: 0x0200080B RID: 2059
		[Serializable]
		internal class Node
		{
			// Token: 0x060044DD RID: 17629 RVA: 0x0012061B File Offset: 0x0011E81B
			public Node(T value)
			{
				this.m_value = value;
			}

			// Token: 0x0400355D RID: 13661
			public readonly T m_value;

			// Token: 0x0400355E RID: 13662
			public ConcurrentBag<T>.Node m_next;

			// Token: 0x0400355F RID: 13663
			public ConcurrentBag<T>.Node m_prev;
		}

		// Token: 0x0200080C RID: 2060
		internal class ThreadLocalList
		{
			// Token: 0x060044DE RID: 17630 RVA: 0x0012062A File Offset: 0x0011E82A
			internal ThreadLocalList(Thread ownerThread)
			{
				this.m_ownerThread = ownerThread;
			}

			// Token: 0x060044DF RID: 17631 RVA: 0x0012063C File Offset: 0x0011E83C
			internal void Add(T item, bool updateCount)
			{
				ConcurrentBag<T>.Node node;
				checked
				{
					this.m_count++;
					node = new ConcurrentBag<T>.Node(item);
				}
				if (this.m_head == null)
				{
					this.m_head = node;
					this.m_tail = node;
					this.m_version++;
				}
				else
				{
					node.m_next = this.m_head;
					this.m_head.m_prev = node;
					this.m_head = node;
				}
				if (updateCount)
				{
					this.m_count -= this.m_stealCount;
					this.m_stealCount = 0;
				}
			}

			// Token: 0x060044E0 RID: 17632 RVA: 0x001206D0 File Offset: 0x0011E8D0
			internal void Remove(out T result)
			{
				ConcurrentBag<T>.Node head = this.m_head;
				this.m_head = this.m_head.m_next;
				if (this.m_head != null)
				{
					this.m_head.m_prev = null;
				}
				else
				{
					this.m_tail = null;
				}
				this.m_count--;
				result = head.m_value;
			}

			// Token: 0x060044E1 RID: 17633 RVA: 0x00120738 File Offset: 0x0011E938
			internal bool Peek(out T result)
			{
				ConcurrentBag<T>.Node head = this.m_head;
				if (head != null)
				{
					result = head.m_value;
					return true;
				}
				result = default(T);
				return false;
			}

			// Token: 0x060044E2 RID: 17634 RVA: 0x00120768 File Offset: 0x0011E968
			internal void Steal(out T result, bool remove)
			{
				ConcurrentBag<T>.Node tail = this.m_tail;
				if (remove)
				{
					this.m_tail = this.m_tail.m_prev;
					if (this.m_tail != null)
					{
						this.m_tail.m_next = null;
					}
					else
					{
						this.m_head = null;
					}
					this.m_stealCount++;
				}
				result = tail.m_value;
			}

			// Token: 0x17000F9F RID: 3999
			// (get) Token: 0x060044E3 RID: 17635 RVA: 0x001207D3 File Offset: 0x0011E9D3
			internal int Count
			{
				get
				{
					return this.m_count - this.m_stealCount;
				}
			}

			// Token: 0x04003560 RID: 13664
			internal volatile ConcurrentBag<T>.Node m_head;

			// Token: 0x04003561 RID: 13665
			private volatile ConcurrentBag<T>.Node m_tail;

			// Token: 0x04003562 RID: 13666
			internal volatile int m_currentOp;

			// Token: 0x04003563 RID: 13667
			private int m_count;

			// Token: 0x04003564 RID: 13668
			internal int m_stealCount;

			// Token: 0x04003565 RID: 13669
			internal volatile ConcurrentBag<T>.ThreadLocalList m_nextList;

			// Token: 0x04003566 RID: 13670
			internal bool m_lockTaken;

			// Token: 0x04003567 RID: 13671
			internal Thread m_ownerThread;

			// Token: 0x04003568 RID: 13672
			internal volatile int m_version;
		}

		// Token: 0x0200080D RID: 2061
		internal enum ListOperation
		{
			// Token: 0x0400356A RID: 13674
			None,
			// Token: 0x0400356B RID: 13675
			Add,
			// Token: 0x0400356C RID: 13676
			Take
		}
	}
}
