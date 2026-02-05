using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Security;
using System.Security.Permissions;
using System.Threading;

namespace System
{
	// Token: 0x02000060 RID: 96
	internal sealed class PinnableBufferCache
	{
		// Token: 0x06000425 RID: 1061 RVA: 0x0001DAD8 File Offset: 0x0001BCD8
		public PinnableBufferCache(string cacheName, int numberOfElements)
			: this(cacheName, () => new byte[numberOfElements])
		{
		}

		// Token: 0x06000426 RID: 1062 RVA: 0x0001DB05 File Offset: 0x0001BD05
		public byte[] AllocateBuffer()
		{
			return (byte[])this.Allocate();
		}

		// Token: 0x06000427 RID: 1063 RVA: 0x0001DB12 File Offset: 0x0001BD12
		public void FreeBuffer(byte[] buffer)
		{
			this.Free(buffer);
		}

		// Token: 0x06000428 RID: 1064 RVA: 0x0001DB1C File Offset: 0x0001BD1C
		[SecuritySafeCritical]
		[EnvironmentPermission(SecurityAction.Assert, Unrestricted = true)]
		internal PinnableBufferCache(string cacheName, Func<object> factory)
		{
			this.m_NotGen2 = new List<object>(16);
			this.m_factory = factory;
			string text = "PinnableBufferCache_" + cacheName + "_Disabled";
			try
			{
				string environmentVariable = Environment.GetEnvironmentVariable(text);
				if (environmentVariable != null)
				{
					global::System.PinnableBufferCacheEventSource.Log.DebugMessage("Creating " + cacheName + " PinnableBufferCacheDisabled=" + environmentVariable);
					int num = environmentVariable.IndexOf(cacheName, StringComparison.OrdinalIgnoreCase);
					if (0 <= num)
					{
						global::System.PinnableBufferCacheEventSource.Log.DebugMessage("Disabling " + cacheName);
						return;
					}
				}
			}
			catch
			{
			}
			string text2 = "PinnableBufferCache_" + cacheName + "_MinCount";
			try
			{
				string environmentVariable2 = Environment.GetEnvironmentVariable(text2);
				if (environmentVariable2 != null && int.TryParse(environmentVariable2, out this.m_minBufferCount))
				{
					this.CreateNewBuffers();
				}
			}
			catch
			{
			}
			global::System.PinnableBufferCacheEventSource.Log.Create(cacheName);
			this.m_CacheName = cacheName;
		}

		// Token: 0x06000429 RID: 1065 RVA: 0x0001DC14 File Offset: 0x0001BE14
		[SecuritySafeCritical]
		internal object Allocate()
		{
			if (this.m_CacheName == null)
			{
				return this.m_factory();
			}
			object obj;
			if (!this.m_FreeList.TryPop(out obj))
			{
				this.Restock(out obj);
			}
			if (global::System.PinnableBufferCacheEventSource.Log.IsEnabled())
			{
				int num = Interlocked.Increment(ref this.m_numAllocCalls);
				if (num >= 1024)
				{
					lock (this)
					{
						int num2 = Interlocked.Exchange(ref this.m_numAllocCalls, 0);
						if (num2 >= 1024)
						{
							int num3 = 0;
							foreach (object obj2 in this.m_FreeList)
							{
								if (GC.GetGeneration(obj2) < GC.MaxGeneration)
								{
									num3++;
								}
							}
							global::System.PinnableBufferCacheEventSource.Log.WalkFreeListResult(this.m_CacheName, this.m_FreeList.Count, num3);
						}
					}
				}
				global::System.PinnableBufferCacheEventSource.Log.AllocateBuffer(this.m_CacheName, global::System.PinnableBufferCacheEventSource.AddressOf(obj), obj.GetHashCode(), GC.GetGeneration(obj), this.m_FreeList.Count);
			}
			return obj;
		}

		// Token: 0x0600042A RID: 1066 RVA: 0x0001DD50 File Offset: 0x0001BF50
		[SecuritySafeCritical]
		internal void Free(object buffer)
		{
			if (this.m_CacheName == null)
			{
				return;
			}
			if (global::System.PinnableBufferCacheEventSource.Log.IsEnabled())
			{
				global::System.PinnableBufferCacheEventSource.Log.FreeBuffer(this.m_CacheName, global::System.PinnableBufferCacheEventSource.AddressOf(buffer), buffer.GetHashCode(), this.m_FreeList.Count);
			}
			if (buffer == null)
			{
				if (global::System.PinnableBufferCacheEventSource.Log.IsEnabled())
				{
					global::System.PinnableBufferCacheEventSource.Log.FreeBufferNull(this.m_CacheName, this.m_FreeList.Count);
				}
				return;
			}
			if (this.m_gen1CountAtLastRestock + 3 > GC.CollectionCount(GC.MaxGeneration - 1))
			{
				lock (this)
				{
					if (GC.GetGeneration(buffer) < GC.MaxGeneration)
					{
						this.m_moreThanFreeListNeeded = true;
						global::System.PinnableBufferCacheEventSource.Log.FreeBufferStillTooYoung(this.m_CacheName, this.m_NotGen2.Count);
						this.m_NotGen2.Add(buffer);
						this.m_gen1CountAtLastRestock = GC.CollectionCount(GC.MaxGeneration - 1);
						return;
					}
				}
			}
			this.m_FreeList.Push(buffer);
		}

		// Token: 0x0600042B RID: 1067 RVA: 0x0001DE60 File Offset: 0x0001C060
		[SecuritySafeCritical]
		private void Restock(out object returnBuffer)
		{
			lock (this)
			{
				if (!this.m_FreeList.TryPop(out returnBuffer))
				{
					if (this.m_restockSize == 0)
					{
						global::System.Gen2GcCallback.Register(new Func<object, bool>(global::System.PinnableBufferCache.Gen2GcCallbackFunc), this);
					}
					this.m_moreThanFreeListNeeded = true;
					global::System.PinnableBufferCacheEventSource.Log.AllocateBufferFreeListEmpty(this.m_CacheName, this.m_NotGen2.Count);
					if (this.m_NotGen2.Count == 0)
					{
						this.CreateNewBuffers();
					}
					int num = this.m_NotGen2.Count - 1;
					if (GC.GetGeneration(this.m_NotGen2[num]) < GC.MaxGeneration && GC.GetGeneration(this.m_NotGen2[0]) == GC.MaxGeneration)
					{
						num = 0;
					}
					returnBuffer = this.m_NotGen2[num];
					this.m_NotGen2.RemoveAt(num);
					if (global::System.PinnableBufferCacheEventSource.Log.IsEnabled() && GC.GetGeneration(returnBuffer) < GC.MaxGeneration)
					{
						global::System.PinnableBufferCacheEventSource.Log.AllocateBufferFromNotGen2(this.m_CacheName, this.m_NotGen2.Count);
					}
					if (!this.AgePendingBuffers() && this.m_NotGen2.Count == this.m_restockSize / 2)
					{
						global::System.PinnableBufferCacheEventSource.Log.DebugMessage("Proactively adding more buffers to aging pool");
						this.CreateNewBuffers();
					}
				}
			}
		}

		// Token: 0x0600042C RID: 1068 RVA: 0x0001DFC4 File Offset: 0x0001C1C4
		[SecuritySafeCritical]
		private bool AgePendingBuffers()
		{
			if (this.m_gen1CountAtLastRestock < GC.CollectionCount(GC.MaxGeneration - 1))
			{
				int num = 0;
				List<object> list = new List<object>();
				global::System.PinnableBufferCacheEventSource.Log.AllocateBufferAged(this.m_CacheName, this.m_NotGen2.Count);
				for (int i = 0; i < this.m_NotGen2.Count; i++)
				{
					object obj = this.m_NotGen2[i];
					if (GC.GetGeneration(obj) >= GC.MaxGeneration)
					{
						this.m_FreeList.Push(obj);
						num++;
					}
					else
					{
						list.Add(obj);
					}
				}
				global::System.PinnableBufferCacheEventSource.Log.AgePendingBuffersResults(this.m_CacheName, num, list.Count);
				this.m_NotGen2 = list;
				return true;
			}
			return false;
		}

		// Token: 0x0600042D RID: 1069 RVA: 0x0001E078 File Offset: 0x0001C278
		private void CreateNewBuffers()
		{
			if (this.m_restockSize == 0)
			{
				this.m_restockSize = 4;
			}
			else if (this.m_restockSize < 16)
			{
				this.m_restockSize = 16;
			}
			else if (this.m_restockSize < 256)
			{
				this.m_restockSize *= 2;
			}
			else if (this.m_restockSize < 4096)
			{
				this.m_restockSize = this.m_restockSize * 3 / 2;
			}
			else
			{
				this.m_restockSize = 4096;
			}
			if (this.m_minBufferCount > this.m_buffersUnderManagement)
			{
				this.m_restockSize = Math.Max(this.m_restockSize, this.m_minBufferCount - this.m_buffersUnderManagement);
			}
			global::System.PinnableBufferCacheEventSource.Log.AllocateBufferCreatingNewBuffers(this.m_CacheName, this.m_buffersUnderManagement, this.m_restockSize);
			for (int i = 0; i < this.m_restockSize; i++)
			{
				object obj = this.m_factory();
				object obj2 = new object();
				this.m_NotGen2.Add(obj);
			}
			this.m_buffersUnderManagement += this.m_restockSize;
			this.m_gen1CountAtLastRestock = GC.CollectionCount(GC.MaxGeneration - 1);
		}

		// Token: 0x0600042E RID: 1070 RVA: 0x0001E18D File Offset: 0x0001C38D
		[SecuritySafeCritical]
		private static bool Gen2GcCallbackFunc(object targetObj)
		{
			return ((global::System.PinnableBufferCache)targetObj).TrimFreeListIfNeeded();
		}

		// Token: 0x0600042F RID: 1071 RVA: 0x0001E19C File Offset: 0x0001C39C
		[SecuritySafeCritical]
		private bool TrimFreeListIfNeeded()
		{
			int tickCount = Environment.TickCount;
			int num = tickCount - this.m_msecNoUseBeyondFreeListSinceThisTime;
			global::System.PinnableBufferCacheEventSource.Log.TrimCheck(this.m_CacheName, this.m_buffersUnderManagement, this.m_moreThanFreeListNeeded, num);
			if (this.m_moreThanFreeListNeeded)
			{
				this.m_moreThanFreeListNeeded = false;
				this.m_trimmingExperimentInProgress = false;
				this.m_msecNoUseBeyondFreeListSinceThisTime = tickCount;
				return true;
			}
			if (0 <= num && num < 10000)
			{
				return true;
			}
			lock (this)
			{
				if (this.m_moreThanFreeListNeeded)
				{
					this.m_moreThanFreeListNeeded = false;
					this.m_trimmingExperimentInProgress = false;
					this.m_msecNoUseBeyondFreeListSinceThisTime = tickCount;
					return true;
				}
				int count = this.m_FreeList.Count;
				if (this.m_NotGen2.Count > 0)
				{
					if (!this.m_trimmingExperimentInProgress)
					{
						global::System.PinnableBufferCacheEventSource.Log.TrimFlush(this.m_CacheName, this.m_buffersUnderManagement, count, this.m_NotGen2.Count);
						this.AgePendingBuffers();
						this.m_trimmingExperimentInProgress = true;
						return true;
					}
					global::System.PinnableBufferCacheEventSource.Log.TrimFree(this.m_CacheName, this.m_buffersUnderManagement, count, this.m_NotGen2.Count);
					this.m_buffersUnderManagement -= this.m_NotGen2.Count;
					int num2 = this.m_buffersUnderManagement / 4;
					if (num2 < this.m_restockSize)
					{
						this.m_restockSize = Math.Max(num2, 16);
					}
					this.m_NotGen2.Clear();
					this.m_trimmingExperimentInProgress = false;
					return true;
				}
				else
				{
					int num3 = count / 4 + 1;
					if (count * 15 <= this.m_buffersUnderManagement || this.m_buffersUnderManagement - num3 <= this.m_minBufferCount)
					{
						global::System.PinnableBufferCacheEventSource.Log.TrimFreeSizeOK(this.m_CacheName, this.m_buffersUnderManagement, count);
						return true;
					}
					global::System.PinnableBufferCacheEventSource.Log.TrimExperiment(this.m_CacheName, this.m_buffersUnderManagement, count, num3);
					for (int i = 0; i < num3; i++)
					{
						object obj;
						if (this.m_FreeList.TryPop(out obj))
						{
							this.m_NotGen2.Add(obj);
						}
					}
					this.m_msecNoUseBeyondFreeListSinceThisTime = tickCount;
					this.m_trimmingExperimentInProgress = true;
				}
			}
			return true;
		}

		// Token: 0x0400051F RID: 1311
		private const int DefaultNumberOfBuffers = 16;

		// Token: 0x04000520 RID: 1312
		private string m_CacheName;

		// Token: 0x04000521 RID: 1313
		private Func<object> m_factory;

		// Token: 0x04000522 RID: 1314
		private ConcurrentStack<object> m_FreeList = new ConcurrentStack<object>();

		// Token: 0x04000523 RID: 1315
		private List<object> m_NotGen2;

		// Token: 0x04000524 RID: 1316
		private int m_gen1CountAtLastRestock;

		// Token: 0x04000525 RID: 1317
		private int m_msecNoUseBeyondFreeListSinceThisTime;

		// Token: 0x04000526 RID: 1318
		private bool m_moreThanFreeListNeeded;

		// Token: 0x04000527 RID: 1319
		private int m_buffersUnderManagement;

		// Token: 0x04000528 RID: 1320
		private int m_restockSize;

		// Token: 0x04000529 RID: 1321
		private bool m_trimmingExperimentInProgress;

		// Token: 0x0400052A RID: 1322
		private int m_minBufferCount;

		// Token: 0x0400052B RID: 1323
		private int m_numAllocCalls;
	}
}
