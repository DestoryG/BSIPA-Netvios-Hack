using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Threading;

namespace System.Collections.Concurrent
{
	// Token: 0x020003D0 RID: 976
	[ComVisible(false)]
	[DebuggerTypeProxy(typeof(SystemThreadingCollections_BlockingCollectionDebugView<>))]
	[DebuggerDisplay("Count = {Count}, Type = {m_collection}")]
	[global::__DynamicallyInvokable]
	[HostProtection(SecurityAction.LinkDemand, Synchronization = true, ExternalThreading = true)]
	public class BlockingCollection<T> : IEnumerable<T>, IEnumerable, ICollection, IDisposable, IReadOnlyCollection<T>
	{
		// Token: 0x17000959 RID: 2393
		// (get) Token: 0x0600254B RID: 9547 RVA: 0x000AD878 File Offset: 0x000ABA78
		[global::__DynamicallyInvokable]
		public int BoundedCapacity
		{
			[global::__DynamicallyInvokable]
			get
			{
				this.CheckDisposed();
				return this.m_boundedCapacity;
			}
		}

		// Token: 0x1700095A RID: 2394
		// (get) Token: 0x0600254C RID: 9548 RVA: 0x000AD886 File Offset: 0x000ABA86
		[global::__DynamicallyInvokable]
		public bool IsAddingCompleted
		{
			[global::__DynamicallyInvokable]
			get
			{
				this.CheckDisposed();
				return this.m_currentAdders == int.MinValue;
			}
		}

		// Token: 0x1700095B RID: 2395
		// (get) Token: 0x0600254D RID: 9549 RVA: 0x000AD89D File Offset: 0x000ABA9D
		[global::__DynamicallyInvokable]
		public bool IsCompleted
		{
			[global::__DynamicallyInvokable]
			get
			{
				this.CheckDisposed();
				return this.IsAddingCompleted && this.m_occupiedNodes.CurrentCount == 0;
			}
		}

		// Token: 0x1700095C RID: 2396
		// (get) Token: 0x0600254E RID: 9550 RVA: 0x000AD8BD File Offset: 0x000ABABD
		[global::__DynamicallyInvokable]
		public int Count
		{
			[global::__DynamicallyInvokable]
			get
			{
				this.CheckDisposed();
				return this.m_occupiedNodes.CurrentCount;
			}
		}

		// Token: 0x1700095D RID: 2397
		// (get) Token: 0x0600254F RID: 9551 RVA: 0x000AD8D0 File Offset: 0x000ABAD0
		[global::__DynamicallyInvokable]
		bool ICollection.IsSynchronized
		{
			[global::__DynamicallyInvokable]
			get
			{
				this.CheckDisposed();
				return false;
			}
		}

		// Token: 0x1700095E RID: 2398
		// (get) Token: 0x06002550 RID: 9552 RVA: 0x000AD8D9 File Offset: 0x000ABAD9
		[global::__DynamicallyInvokable]
		object ICollection.SyncRoot
		{
			[global::__DynamicallyInvokable]
			get
			{
				throw new NotSupportedException(SR.GetString("ConcurrentCollection_SyncRoot_NotSupported"));
			}
		}

		// Token: 0x06002551 RID: 9553 RVA: 0x000AD8EA File Offset: 0x000ABAEA
		[global::__DynamicallyInvokable]
		public BlockingCollection()
			: this(new ConcurrentQueue<T>())
		{
		}

		// Token: 0x06002552 RID: 9554 RVA: 0x000AD8F7 File Offset: 0x000ABAF7
		[global::__DynamicallyInvokable]
		public BlockingCollection(int boundedCapacity)
			: this(new ConcurrentQueue<T>(), boundedCapacity)
		{
		}

		// Token: 0x06002553 RID: 9555 RVA: 0x000AD908 File Offset: 0x000ABB08
		[global::__DynamicallyInvokable]
		public BlockingCollection(IProducerConsumerCollection<T> collection, int boundedCapacity)
		{
			if (boundedCapacity < 1)
			{
				throw new ArgumentOutOfRangeException("boundedCapacity", boundedCapacity, SR.GetString("BlockingCollection_ctor_BoundedCapacityRange"));
			}
			if (collection == null)
			{
				throw new ArgumentNullException("collection");
			}
			int count = collection.Count;
			if (count > boundedCapacity)
			{
				throw new ArgumentException(SR.GetString("BlockingCollection_ctor_CountMoreThanCapacity"));
			}
			this.Initialize(collection, boundedCapacity, count);
		}

		// Token: 0x06002554 RID: 9556 RVA: 0x000AD96C File Offset: 0x000ABB6C
		[global::__DynamicallyInvokable]
		public BlockingCollection(IProducerConsumerCollection<T> collection)
		{
			if (collection == null)
			{
				throw new ArgumentNullException("collection");
			}
			this.Initialize(collection, -1, collection.Count);
		}

		// Token: 0x06002555 RID: 9557 RVA: 0x000AD990 File Offset: 0x000ABB90
		private void Initialize(IProducerConsumerCollection<T> collection, int boundedCapacity, int collectionCount)
		{
			this.m_collection = collection;
			this.m_boundedCapacity = boundedCapacity;
			this.m_isDisposed = false;
			this.m_ConsumersCancellationTokenSource = new CancellationTokenSource();
			this.m_ProducersCancellationTokenSource = new CancellationTokenSource();
			if (boundedCapacity == -1)
			{
				this.m_freeNodes = null;
			}
			else
			{
				this.m_freeNodes = new SemaphoreSlim(boundedCapacity - collectionCount);
			}
			this.m_occupiedNodes = new SemaphoreSlim(collectionCount);
		}

		// Token: 0x06002556 RID: 9558 RVA: 0x000AD9F0 File Offset: 0x000ABBF0
		[global::__DynamicallyInvokable]
		public void Add(T item)
		{
			this.TryAddWithNoTimeValidation(item, -1, default(CancellationToken));
		}

		// Token: 0x06002557 RID: 9559 RVA: 0x000ADA0F File Offset: 0x000ABC0F
		[global::__DynamicallyInvokable]
		public void Add(T item, CancellationToken cancellationToken)
		{
			this.TryAddWithNoTimeValidation(item, -1, cancellationToken);
		}

		// Token: 0x06002558 RID: 9560 RVA: 0x000ADA1C File Offset: 0x000ABC1C
		[global::__DynamicallyInvokable]
		public bool TryAdd(T item)
		{
			return this.TryAddWithNoTimeValidation(item, 0, default(CancellationToken));
		}

		// Token: 0x06002559 RID: 9561 RVA: 0x000ADA3C File Offset: 0x000ABC3C
		[global::__DynamicallyInvokable]
		public bool TryAdd(T item, TimeSpan timeout)
		{
			BlockingCollection<T>.ValidateTimeout(timeout);
			return this.TryAddWithNoTimeValidation(item, (int)timeout.TotalMilliseconds, default(CancellationToken));
		}

		// Token: 0x0600255A RID: 9562 RVA: 0x000ADA68 File Offset: 0x000ABC68
		[global::__DynamicallyInvokable]
		public bool TryAdd(T item, int millisecondsTimeout)
		{
			BlockingCollection<T>.ValidateMillisecondsTimeout(millisecondsTimeout);
			return this.TryAddWithNoTimeValidation(item, millisecondsTimeout, default(CancellationToken));
		}

		// Token: 0x0600255B RID: 9563 RVA: 0x000ADA8C File Offset: 0x000ABC8C
		[global::__DynamicallyInvokable]
		public bool TryAdd(T item, int millisecondsTimeout, CancellationToken cancellationToken)
		{
			BlockingCollection<T>.ValidateMillisecondsTimeout(millisecondsTimeout);
			return this.TryAddWithNoTimeValidation(item, millisecondsTimeout, cancellationToken);
		}

		// Token: 0x0600255C RID: 9564 RVA: 0x000ADAA0 File Offset: 0x000ABCA0
		private bool TryAddWithNoTimeValidation(T item, int millisecondsTimeout, CancellationToken cancellationToken)
		{
			this.CheckDisposed();
			if (cancellationToken.IsCancellationRequested)
			{
				throw new OperationCanceledException(SR.GetString("Common_OperationCanceled"), cancellationToken);
			}
			if (this.IsAddingCompleted)
			{
				throw new InvalidOperationException(SR.GetString("BlockingCollection_Completed"));
			}
			bool flag = true;
			if (this.m_freeNodes != null)
			{
				CancellationTokenSource cancellationTokenSource = null;
				try
				{
					flag = this.m_freeNodes.Wait(0);
					if (!flag && millisecondsTimeout != 0)
					{
						cancellationTokenSource = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken, this.m_ProducersCancellationTokenSource.Token);
						flag = this.m_freeNodes.Wait(millisecondsTimeout, cancellationTokenSource.Token);
					}
				}
				catch (OperationCanceledException)
				{
					if (cancellationToken.IsCancellationRequested)
					{
						throw new OperationCanceledException(SR.GetString("Common_OperationCanceled"), cancellationToken);
					}
					throw new InvalidOperationException(SR.GetString("BlockingCollection_Add_ConcurrentCompleteAdd"));
				}
				finally
				{
					if (cancellationTokenSource != null)
					{
						cancellationTokenSource.Dispose();
					}
				}
			}
			if (flag)
			{
				SpinWait spinWait = default(SpinWait);
				for (;;)
				{
					int currentAdders = this.m_currentAdders;
					if ((currentAdders & -2147483648) != 0)
					{
						break;
					}
					if (Interlocked.CompareExchange(ref this.m_currentAdders, currentAdders + 1, currentAdders) == currentAdders)
					{
						goto IL_011D;
					}
					spinWait.SpinOnce();
				}
				spinWait.Reset();
				while (this.m_currentAdders != -2147483648)
				{
					spinWait.SpinOnce();
				}
				throw new InvalidOperationException(SR.GetString("BlockingCollection_Completed"));
				IL_011D:
				try
				{
					bool flag2 = false;
					try
					{
						cancellationToken.ThrowIfCancellationRequested();
						flag2 = this.m_collection.TryAdd(item);
					}
					catch
					{
						if (this.m_freeNodes != null)
						{
							this.m_freeNodes.Release();
						}
						throw;
					}
					if (!flag2)
					{
						throw new InvalidOperationException(SR.GetString("BlockingCollection_Add_Failed"));
					}
					this.m_occupiedNodes.Release();
				}
				finally
				{
					Interlocked.Decrement(ref this.m_currentAdders);
				}
			}
			return flag;
		}

		// Token: 0x0600255D RID: 9565 RVA: 0x000ADC60 File Offset: 0x000ABE60
		[global::__DynamicallyInvokable]
		public T Take()
		{
			T t;
			if (!this.TryTake(out t, -1, CancellationToken.None))
			{
				throw new InvalidOperationException(SR.GetString("BlockingCollection_CantTakeWhenDone"));
			}
			return t;
		}

		// Token: 0x0600255E RID: 9566 RVA: 0x000ADC90 File Offset: 0x000ABE90
		[global::__DynamicallyInvokable]
		public T Take(CancellationToken cancellationToken)
		{
			T t;
			if (!this.TryTake(out t, -1, cancellationToken))
			{
				throw new InvalidOperationException(SR.GetString("BlockingCollection_CantTakeWhenDone"));
			}
			return t;
		}

		// Token: 0x0600255F RID: 9567 RVA: 0x000ADCBA File Offset: 0x000ABEBA
		[global::__DynamicallyInvokable]
		public bool TryTake(out T item)
		{
			return this.TryTake(out item, 0, CancellationToken.None);
		}

		// Token: 0x06002560 RID: 9568 RVA: 0x000ADCC9 File Offset: 0x000ABEC9
		[global::__DynamicallyInvokable]
		public bool TryTake(out T item, TimeSpan timeout)
		{
			BlockingCollection<T>.ValidateTimeout(timeout);
			return this.TryTakeWithNoTimeValidation(out item, (int)timeout.TotalMilliseconds, CancellationToken.None, null);
		}

		// Token: 0x06002561 RID: 9569 RVA: 0x000ADCE6 File Offset: 0x000ABEE6
		[global::__DynamicallyInvokable]
		public bool TryTake(out T item, int millisecondsTimeout)
		{
			BlockingCollection<T>.ValidateMillisecondsTimeout(millisecondsTimeout);
			return this.TryTakeWithNoTimeValidation(out item, millisecondsTimeout, CancellationToken.None, null);
		}

		// Token: 0x06002562 RID: 9570 RVA: 0x000ADCFC File Offset: 0x000ABEFC
		[global::__DynamicallyInvokable]
		public bool TryTake(out T item, int millisecondsTimeout, CancellationToken cancellationToken)
		{
			BlockingCollection<T>.ValidateMillisecondsTimeout(millisecondsTimeout);
			return this.TryTakeWithNoTimeValidation(out item, millisecondsTimeout, cancellationToken, null);
		}

		// Token: 0x06002563 RID: 9571 RVA: 0x000ADD10 File Offset: 0x000ABF10
		private bool TryTakeWithNoTimeValidation(out T item, int millisecondsTimeout, CancellationToken cancellationToken, CancellationTokenSource combinedTokenSource)
		{
			this.CheckDisposed();
			item = default(T);
			if (cancellationToken.IsCancellationRequested)
			{
				throw new OperationCanceledException(SR.GetString("Common_OperationCanceled"), cancellationToken);
			}
			if (this.IsCompleted)
			{
				return false;
			}
			bool flag = false;
			CancellationTokenSource cancellationTokenSource = combinedTokenSource;
			try
			{
				flag = this.m_occupiedNodes.Wait(0);
				if (!flag && millisecondsTimeout != 0)
				{
					if (combinedTokenSource == null)
					{
						cancellationTokenSource = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken, this.m_ConsumersCancellationTokenSource.Token);
					}
					flag = this.m_occupiedNodes.Wait(millisecondsTimeout, cancellationTokenSource.Token);
				}
			}
			catch (OperationCanceledException)
			{
				if (cancellationToken.IsCancellationRequested)
				{
					throw new OperationCanceledException(SR.GetString("Common_OperationCanceled"), cancellationToken);
				}
				return false;
			}
			finally
			{
				if (cancellationTokenSource != null && combinedTokenSource == null)
				{
					cancellationTokenSource.Dispose();
				}
			}
			if (flag)
			{
				bool flag2 = false;
				bool flag3 = true;
				try
				{
					cancellationToken.ThrowIfCancellationRequested();
					flag2 = this.m_collection.TryTake(out item);
					flag3 = false;
					if (!flag2)
					{
						throw new InvalidOperationException(SR.GetString("BlockingCollection_Take_CollectionModified"));
					}
				}
				finally
				{
					if (flag2)
					{
						if (this.m_freeNodes != null)
						{
							this.m_freeNodes.Release();
						}
					}
					else if (flag3)
					{
						this.m_occupiedNodes.Release();
					}
					if (this.IsCompleted)
					{
						this.CancelWaitingConsumers();
					}
				}
			}
			return flag;
		}

		// Token: 0x06002564 RID: 9572 RVA: 0x000ADE58 File Offset: 0x000AC058
		[global::__DynamicallyInvokable]
		public static int AddToAny(BlockingCollection<T>[] collections, T item)
		{
			return BlockingCollection<T>.TryAddToAny(collections, item, -1, CancellationToken.None);
		}

		// Token: 0x06002565 RID: 9573 RVA: 0x000ADE67 File Offset: 0x000AC067
		[global::__DynamicallyInvokable]
		public static int AddToAny(BlockingCollection<T>[] collections, T item, CancellationToken cancellationToken)
		{
			return BlockingCollection<T>.TryAddToAny(collections, item, -1, cancellationToken);
		}

		// Token: 0x06002566 RID: 9574 RVA: 0x000ADE72 File Offset: 0x000AC072
		[global::__DynamicallyInvokable]
		public static int TryAddToAny(BlockingCollection<T>[] collections, T item)
		{
			return BlockingCollection<T>.TryAddToAny(collections, item, 0, CancellationToken.None);
		}

		// Token: 0x06002567 RID: 9575 RVA: 0x000ADE81 File Offset: 0x000AC081
		[global::__DynamicallyInvokable]
		public static int TryAddToAny(BlockingCollection<T>[] collections, T item, TimeSpan timeout)
		{
			BlockingCollection<T>.ValidateTimeout(timeout);
			return BlockingCollection<T>.TryAddToAnyCore(collections, item, (int)timeout.TotalMilliseconds, CancellationToken.None);
		}

		// Token: 0x06002568 RID: 9576 RVA: 0x000ADE9D File Offset: 0x000AC09D
		[global::__DynamicallyInvokable]
		public static int TryAddToAny(BlockingCollection<T>[] collections, T item, int millisecondsTimeout)
		{
			BlockingCollection<T>.ValidateMillisecondsTimeout(millisecondsTimeout);
			return BlockingCollection<T>.TryAddToAnyCore(collections, item, millisecondsTimeout, CancellationToken.None);
		}

		// Token: 0x06002569 RID: 9577 RVA: 0x000ADEB2 File Offset: 0x000AC0B2
		[global::__DynamicallyInvokable]
		public static int TryAddToAny(BlockingCollection<T>[] collections, T item, int millisecondsTimeout, CancellationToken cancellationToken)
		{
			BlockingCollection<T>.ValidateMillisecondsTimeout(millisecondsTimeout);
			return BlockingCollection<T>.TryAddToAnyCore(collections, item, millisecondsTimeout, cancellationToken);
		}

		// Token: 0x0600256A RID: 9578 RVA: 0x000ADEC4 File Offset: 0x000AC0C4
		private static int TryAddToAnyCore(BlockingCollection<T>[] collections, T item, int millisecondsTimeout, CancellationToken externalCancellationToken)
		{
			BlockingCollection<T>.ValidateCollectionsArray(collections, true);
			int num = millisecondsTimeout;
			uint num2 = 0U;
			if (millisecondsTimeout != -1)
			{
				num2 = (uint)Environment.TickCount;
			}
			int num3 = BlockingCollection<T>.TryAddToAnyFast(collections, item);
			if (num3 > -1)
			{
				return num3;
			}
			CancellationToken[] array;
			List<WaitHandle> handles = BlockingCollection<T>.GetHandles(collections, externalCancellationToken, true, out array);
			while (millisecondsTimeout == -1 || num >= 0)
			{
				num3 = -1;
				using (CancellationTokenSource cancellationTokenSource = CancellationTokenSource.CreateLinkedTokenSource(array))
				{
					handles.Add(cancellationTokenSource.Token.WaitHandle);
					num3 = WaitHandle.WaitAny(handles.ToArray(), num, false);
					handles.RemoveAt(handles.Count - 1);
					if (cancellationTokenSource.IsCancellationRequested)
					{
						if (externalCancellationToken.IsCancellationRequested)
						{
							throw new OperationCanceledException(SR.GetString("Common_OperationCanceled"), externalCancellationToken);
						}
						throw new ArgumentException(SR.GetString("BlockingCollection_CantAddAnyWhenCompleted"), "collections");
					}
				}
				if (num3 == 258)
				{
					return -1;
				}
				if (collections[num3].TryAdd(item))
				{
					return num3;
				}
				if (millisecondsTimeout != -1)
				{
					num = BlockingCollection<T>.UpdateTimeOut(num2, millisecondsTimeout);
				}
			}
			return -1;
		}

		// Token: 0x0600256B RID: 9579 RVA: 0x000ADFD0 File Offset: 0x000AC1D0
		private static int TryAddToAnyFast(BlockingCollection<T>[] collections, T item)
		{
			for (int i = 0; i < collections.Length; i++)
			{
				if (collections[i].m_freeNodes == null)
				{
					collections[i].TryAdd(item);
					return i;
				}
			}
			return -1;
		}

		// Token: 0x0600256C RID: 9580 RVA: 0x000AE004 File Offset: 0x000AC204
		private static List<WaitHandle> GetHandles(BlockingCollection<T>[] collections, CancellationToken externalCancellationToken, bool isAddOperation, out CancellationToken[] cancellationTokens)
		{
			List<WaitHandle> list = new List<WaitHandle>(collections.Length + 1);
			List<CancellationToken> list2 = new List<CancellationToken>(collections.Length + 1);
			list2.Add(externalCancellationToken);
			if (isAddOperation)
			{
				for (int i = 0; i < collections.Length; i++)
				{
					if (collections[i].m_freeNodes != null)
					{
						list.Add(collections[i].m_freeNodes.AvailableWaitHandle);
						list2.Add(collections[i].m_ProducersCancellationTokenSource.Token);
					}
				}
			}
			else
			{
				for (int j = 0; j < collections.Length; j++)
				{
					if (!collections[j].IsCompleted)
					{
						list.Add(collections[j].m_occupiedNodes.AvailableWaitHandle);
						list2.Add(collections[j].m_ConsumersCancellationTokenSource.Token);
					}
				}
			}
			cancellationTokens = list2.ToArray();
			return list;
		}

		// Token: 0x0600256D RID: 9581 RVA: 0x000AE0B8 File Offset: 0x000AC2B8
		private static int UpdateTimeOut(uint startTime, int originalWaitMillisecondsTimeout)
		{
			if (originalWaitMillisecondsTimeout == 0)
			{
				return 0;
			}
			uint num = (uint)(Environment.TickCount - (int)startTime);
			if (num > 2147483647U)
			{
				return 0;
			}
			int num2 = originalWaitMillisecondsTimeout - (int)num;
			if (num2 <= 0)
			{
				return 0;
			}
			return num2;
		}

		// Token: 0x0600256E RID: 9582 RVA: 0x000AE0E7 File Offset: 0x000AC2E7
		[global::__DynamicallyInvokable]
		public static int TakeFromAny(BlockingCollection<T>[] collections, out T item)
		{
			return BlockingCollection<T>.TakeFromAny(collections, out item, CancellationToken.None);
		}

		// Token: 0x0600256F RID: 9583 RVA: 0x000AE0F8 File Offset: 0x000AC2F8
		[global::__DynamicallyInvokable]
		public static int TakeFromAny(BlockingCollection<T>[] collections, out T item, CancellationToken cancellationToken)
		{
			return BlockingCollection<T>.TryTakeFromAnyCore(collections, out item, -1, true, cancellationToken);
		}

		// Token: 0x06002570 RID: 9584 RVA: 0x000AE111 File Offset: 0x000AC311
		[global::__DynamicallyInvokable]
		public static int TryTakeFromAny(BlockingCollection<T>[] collections, out T item)
		{
			return BlockingCollection<T>.TryTakeFromAny(collections, out item, 0);
		}

		// Token: 0x06002571 RID: 9585 RVA: 0x000AE11B File Offset: 0x000AC31B
		[global::__DynamicallyInvokable]
		public static int TryTakeFromAny(BlockingCollection<T>[] collections, out T item, TimeSpan timeout)
		{
			BlockingCollection<T>.ValidateTimeout(timeout);
			return BlockingCollection<T>.TryTakeFromAnyCore(collections, out item, (int)timeout.TotalMilliseconds, false, CancellationToken.None);
		}

		// Token: 0x06002572 RID: 9586 RVA: 0x000AE138 File Offset: 0x000AC338
		[global::__DynamicallyInvokable]
		public static int TryTakeFromAny(BlockingCollection<T>[] collections, out T item, int millisecondsTimeout)
		{
			BlockingCollection<T>.ValidateMillisecondsTimeout(millisecondsTimeout);
			return BlockingCollection<T>.TryTakeFromAnyCore(collections, out item, millisecondsTimeout, false, CancellationToken.None);
		}

		// Token: 0x06002573 RID: 9587 RVA: 0x000AE14E File Offset: 0x000AC34E
		[global::__DynamicallyInvokable]
		public static int TryTakeFromAny(BlockingCollection<T>[] collections, out T item, int millisecondsTimeout, CancellationToken cancellationToken)
		{
			BlockingCollection<T>.ValidateMillisecondsTimeout(millisecondsTimeout);
			return BlockingCollection<T>.TryTakeFromAnyCore(collections, out item, millisecondsTimeout, false, cancellationToken);
		}

		// Token: 0x06002574 RID: 9588 RVA: 0x000AE160 File Offset: 0x000AC360
		private static int TryTakeFromAnyCore(BlockingCollection<T>[] collections, out T item, int millisecondsTimeout, bool isTakeOperation, CancellationToken externalCancellationToken)
		{
			BlockingCollection<T>.ValidateCollectionsArray(collections, false);
			for (int i = 0; i < collections.Length; i++)
			{
				if (!collections[i].IsCompleted && collections[i].m_occupiedNodes.CurrentCount > 0 && collections[i].TryTake(out item))
				{
					return i;
				}
			}
			return BlockingCollection<T>.TryTakeFromAnyCoreSlow(collections, out item, millisecondsTimeout, isTakeOperation, externalCancellationToken);
		}

		// Token: 0x06002575 RID: 9589 RVA: 0x000AE1B4 File Offset: 0x000AC3B4
		private static int TryTakeFromAnyCoreSlow(BlockingCollection<T>[] collections, out T item, int millisecondsTimeout, bool isTakeOperation, CancellationToken externalCancellationToken)
		{
			int num = millisecondsTimeout;
			uint num2 = 0U;
			if (millisecondsTimeout != -1)
			{
				num2 = (uint)Environment.TickCount;
			}
			while (millisecondsTimeout == -1 || num >= 0)
			{
				CancellationToken[] array;
				List<WaitHandle> handles = BlockingCollection<T>.GetHandles(collections, externalCancellationToken, false, out array);
				if (handles.Count == 0 && isTakeOperation)
				{
					throw new ArgumentException(SR.GetString("BlockingCollection_CantTakeAnyWhenAllDone"), "collections");
				}
				if (handles.Count != 0)
				{
					using (CancellationTokenSource cancellationTokenSource = CancellationTokenSource.CreateLinkedTokenSource(array))
					{
						handles.Add(cancellationTokenSource.Token.WaitHandle);
						int num3 = WaitHandle.WaitAny(handles.ToArray(), num, false);
						if (cancellationTokenSource.IsCancellationRequested && externalCancellationToken.IsCancellationRequested)
						{
							throw new OperationCanceledException(SR.GetString("Common_OperationCanceled"), externalCancellationToken);
						}
						if (!cancellationTokenSource.IsCancellationRequested)
						{
							if (num3 == 258)
							{
								break;
							}
							if (collections.Length != handles.Count - 1)
							{
								for (int i = 0; i < collections.Length; i++)
								{
									if (collections[i].m_occupiedNodes.AvailableWaitHandle == handles[num3])
									{
										num3 = i;
										break;
									}
								}
							}
							if (collections[num3].TryTake(out item))
							{
								return num3;
							}
						}
					}
					if (millisecondsTimeout != -1)
					{
						num = BlockingCollection<T>.UpdateTimeOut(num2, millisecondsTimeout);
						continue;
					}
					continue;
				}
				break;
			}
			item = default(T);
			return -1;
		}

		// Token: 0x06002576 RID: 9590 RVA: 0x000AE308 File Offset: 0x000AC508
		[global::__DynamicallyInvokable]
		public void CompleteAdding()
		{
			this.CheckDisposed();
			if (this.IsAddingCompleted)
			{
				return;
			}
			SpinWait spinWait = default(SpinWait);
			for (;;)
			{
				int currentAdders = this.m_currentAdders;
				if ((currentAdders & -2147483648) != 0)
				{
					break;
				}
				if (Interlocked.CompareExchange(ref this.m_currentAdders, currentAdders | -2147483648, currentAdders) == currentAdders)
				{
					goto Block_4;
				}
				spinWait.SpinOnce();
			}
			spinWait.Reset();
			while (this.m_currentAdders != -2147483648)
			{
				spinWait.SpinOnce();
			}
			return;
			Block_4:
			spinWait.Reset();
			while (this.m_currentAdders != -2147483648)
			{
				spinWait.SpinOnce();
			}
			if (this.Count == 0)
			{
				this.CancelWaitingConsumers();
			}
			this.CancelWaitingProducers();
		}

		// Token: 0x06002577 RID: 9591 RVA: 0x000AE3B3 File Offset: 0x000AC5B3
		private void CancelWaitingConsumers()
		{
			this.m_ConsumersCancellationTokenSource.Cancel();
		}

		// Token: 0x06002578 RID: 9592 RVA: 0x000AE3C0 File Offset: 0x000AC5C0
		private void CancelWaitingProducers()
		{
			this.m_ProducersCancellationTokenSource.Cancel();
		}

		// Token: 0x06002579 RID: 9593 RVA: 0x000AE3CD File Offset: 0x000AC5CD
		[global::__DynamicallyInvokable]
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x0600257A RID: 9594 RVA: 0x000AE3DC File Offset: 0x000AC5DC
		[global::__DynamicallyInvokable]
		protected virtual void Dispose(bool disposing)
		{
			if (!this.m_isDisposed)
			{
				if (this.m_freeNodes != null)
				{
					this.m_freeNodes.Dispose();
				}
				this.m_occupiedNodes.Dispose();
				this.m_isDisposed = true;
			}
		}

		// Token: 0x0600257B RID: 9595 RVA: 0x000AE40B File Offset: 0x000AC60B
		[global::__DynamicallyInvokable]
		public T[] ToArray()
		{
			this.CheckDisposed();
			return this.m_collection.ToArray();
		}

		// Token: 0x0600257C RID: 9596 RVA: 0x000AE41E File Offset: 0x000AC61E
		[global::__DynamicallyInvokable]
		public void CopyTo(T[] array, int index)
		{
			((ICollection)this).CopyTo(array, index);
		}

		// Token: 0x0600257D RID: 9597 RVA: 0x000AE428 File Offset: 0x000AC628
		[global::__DynamicallyInvokable]
		void ICollection.CopyTo(Array array, int index)
		{
			this.CheckDisposed();
			T[] array2 = this.m_collection.ToArray();
			try
			{
				Array.Copy(array2, 0, array, index, array2.Length);
			}
			catch (ArgumentNullException)
			{
				throw new ArgumentNullException("array");
			}
			catch (ArgumentOutOfRangeException)
			{
				throw new ArgumentOutOfRangeException("index", index, SR.GetString("BlockingCollection_CopyTo_NonNegative"));
			}
			catch (ArgumentException)
			{
				throw new ArgumentException(SR.GetString("BlockingCollection_CopyTo_TooManyElems"), "index");
			}
			catch (RankException)
			{
				throw new ArgumentException(SR.GetString("BlockingCollection_CopyTo_MultiDim"), "array");
			}
			catch (InvalidCastException)
			{
				throw new ArgumentException(SR.GetString("BlockingCollection_CopyTo_IncorrectType"), "array");
			}
			catch (ArrayTypeMismatchException)
			{
				throw new ArgumentException(SR.GetString("BlockingCollection_CopyTo_IncorrectType"), "array");
			}
		}

		// Token: 0x0600257E RID: 9598 RVA: 0x000AE524 File Offset: 0x000AC724
		[global::__DynamicallyInvokable]
		public IEnumerable<T> GetConsumingEnumerable()
		{
			return this.GetConsumingEnumerable(CancellationToken.None);
		}

		// Token: 0x0600257F RID: 9599 RVA: 0x000AE531 File Offset: 0x000AC731
		[global::__DynamicallyInvokable]
		public IEnumerable<T> GetConsumingEnumerable(CancellationToken cancellationToken)
		{
			CancellationTokenSource linkedTokenSource = null;
			try
			{
				linkedTokenSource = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken, this.m_ConsumersCancellationTokenSource.Token);
				while (!this.IsCompleted)
				{
					T t;
					if (this.TryTakeWithNoTimeValidation(out t, -1, cancellationToken, linkedTokenSource))
					{
						yield return t;
					}
				}
			}
			finally
			{
				if (linkedTokenSource != null)
				{
					linkedTokenSource.Dispose();
				}
			}
			yield break;
			yield break;
		}

		// Token: 0x06002580 RID: 9600 RVA: 0x000AE548 File Offset: 0x000AC748
		[global::__DynamicallyInvokable]
		IEnumerator<T> IEnumerable<T>.GetEnumerator()
		{
			this.CheckDisposed();
			return this.m_collection.GetEnumerator();
		}

		// Token: 0x06002581 RID: 9601 RVA: 0x000AE55B File Offset: 0x000AC75B
		[global::__DynamicallyInvokable]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<T>)this).GetEnumerator();
		}

		// Token: 0x06002582 RID: 9602 RVA: 0x000AE564 File Offset: 0x000AC764
		private static void ValidateCollectionsArray(BlockingCollection<T>[] collections, bool isAddOperation)
		{
			if (collections == null)
			{
				throw new ArgumentNullException("collections");
			}
			if (collections.Length < 1)
			{
				throw new ArgumentException(SR.GetString("BlockingCollection_ValidateCollectionsArray_ZeroSize"), "collections");
			}
			if ((!BlockingCollection<T>.IsSTAThread && collections.Length > 63) || (BlockingCollection<T>.IsSTAThread && collections.Length > 62))
			{
				throw new ArgumentOutOfRangeException("collections", SR.GetString("BlockingCollection_ValidateCollectionsArray_LargeSize"));
			}
			for (int i = 0; i < collections.Length; i++)
			{
				if (collections[i] == null)
				{
					throw new ArgumentException(SR.GetString("BlockingCollection_ValidateCollectionsArray_NullElems"), "collections");
				}
				if (collections[i].m_isDisposed)
				{
					throw new ObjectDisposedException("collections", SR.GetString("BlockingCollection_ValidateCollectionsArray_DispElems"));
				}
				if (isAddOperation && collections[i].IsAddingCompleted)
				{
					throw new ArgumentException(SR.GetString("BlockingCollection_CantAddAnyWhenCompleted"), "collections");
				}
			}
		}

		// Token: 0x1700095F RID: 2399
		// (get) Token: 0x06002583 RID: 9603 RVA: 0x000AE634 File Offset: 0x000AC834
		private static bool IsSTAThread
		{
			get
			{
				return Thread.CurrentThread.GetApartmentState() == ApartmentState.STA;
			}
		}

		// Token: 0x06002584 RID: 9604 RVA: 0x000AE644 File Offset: 0x000AC844
		private static void ValidateTimeout(TimeSpan timeout)
		{
			long num = (long)timeout.TotalMilliseconds;
			if ((num < 0L || num > 2147483647L) && num != -1L)
			{
				throw new ArgumentOutOfRangeException("timeout", timeout, string.Format(CultureInfo.InvariantCulture, SR.GetString("BlockingCollection_TimeoutInvalid"), new object[] { int.MaxValue }));
			}
		}

		// Token: 0x06002585 RID: 9605 RVA: 0x000AE6A8 File Offset: 0x000AC8A8
		private static void ValidateMillisecondsTimeout(int millisecondsTimeout)
		{
			if (millisecondsTimeout < 0 && millisecondsTimeout != -1)
			{
				throw new ArgumentOutOfRangeException("millisecondsTimeout", millisecondsTimeout, string.Format(CultureInfo.InvariantCulture, SR.GetString("BlockingCollection_TimeoutInvalid"), new object[] { int.MaxValue }));
			}
		}

		// Token: 0x06002586 RID: 9606 RVA: 0x000AE6F5 File Offset: 0x000AC8F5
		private void CheckDisposed()
		{
			if (this.m_isDisposed)
			{
				throw new ObjectDisposedException("BlockingCollection", SR.GetString("BlockingCollection_Disposed"));
			}
		}

		// Token: 0x04002046 RID: 8262
		private IProducerConsumerCollection<T> m_collection;

		// Token: 0x04002047 RID: 8263
		private int m_boundedCapacity;

		// Token: 0x04002048 RID: 8264
		private const int NON_BOUNDED = -1;

		// Token: 0x04002049 RID: 8265
		private SemaphoreSlim m_freeNodes;

		// Token: 0x0400204A RID: 8266
		private SemaphoreSlim m_occupiedNodes;

		// Token: 0x0400204B RID: 8267
		private bool m_isDisposed;

		// Token: 0x0400204C RID: 8268
		private CancellationTokenSource m_ConsumersCancellationTokenSource;

		// Token: 0x0400204D RID: 8269
		private CancellationTokenSource m_ProducersCancellationTokenSource;

		// Token: 0x0400204E RID: 8270
		private volatile int m_currentAdders;

		// Token: 0x0400204F RID: 8271
		private const int COMPLETE_ADDING_ON_MASK = -2147483648;
	}
}
