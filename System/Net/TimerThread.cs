using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Threading;

namespace System.Net
{
	// Token: 0x0200021C RID: 540
	internal static class TimerThread
	{
		// Token: 0x060013DE RID: 5086 RVA: 0x00069394 File Offset: 0x00067594
		static TimerThread()
		{
			AppDomain.CurrentDomain.DomainUnload += TimerThread.OnDomainUnload;
		}

		// Token: 0x060013DF RID: 5087 RVA: 0x0006940C File Offset: 0x0006760C
		internal static TimerThread.Queue CreateQueue(int durationMilliseconds)
		{
			if (durationMilliseconds == -1)
			{
				return new TimerThread.InfiniteTimerQueue();
			}
			if (durationMilliseconds < 0)
			{
				throw new ArgumentOutOfRangeException("durationMilliseconds");
			}
			LinkedList<WeakReference> linkedList = TimerThread.s_NewQueues;
			TimerThread.TimerQueue timerQueue;
			lock (linkedList)
			{
				timerQueue = new TimerThread.TimerQueue(durationMilliseconds);
				WeakReference weakReference = new WeakReference(timerQueue);
				TimerThread.s_NewQueues.AddLast(weakReference);
			}
			return timerQueue;
		}

		// Token: 0x060013E0 RID: 5088 RVA: 0x0006947C File Offset: 0x0006767C
		internal static TimerThread.Queue GetOrCreateQueue(int durationMilliseconds)
		{
			if (durationMilliseconds == -1)
			{
				return new TimerThread.InfiniteTimerQueue();
			}
			if (durationMilliseconds < 0)
			{
				throw new ArgumentOutOfRangeException("durationMilliseconds");
			}
			WeakReference weakReference = (WeakReference)TimerThread.s_QueuesCache[durationMilliseconds];
			TimerThread.TimerQueue timerQueue;
			if (weakReference == null || (timerQueue = (TimerThread.TimerQueue)weakReference.Target) == null)
			{
				LinkedList<WeakReference> linkedList = TimerThread.s_NewQueues;
				lock (linkedList)
				{
					weakReference = (WeakReference)TimerThread.s_QueuesCache[durationMilliseconds];
					if (weakReference == null || (timerQueue = (TimerThread.TimerQueue)weakReference.Target) == null)
					{
						timerQueue = new TimerThread.TimerQueue(durationMilliseconds);
						weakReference = new WeakReference(timerQueue);
						TimerThread.s_NewQueues.AddLast(weakReference);
						TimerThread.s_QueuesCache[durationMilliseconds] = weakReference;
						if (++TimerThread.s_CacheScanIteration % 32 == 0)
						{
							List<int> list = new List<int>();
							foreach (object obj in TimerThread.s_QueuesCache)
							{
								DictionaryEntry dictionaryEntry = (DictionaryEntry)obj;
								if (((WeakReference)dictionaryEntry.Value).Target == null)
								{
									list.Add((int)dictionaryEntry.Key);
								}
							}
							for (int i = 0; i < list.Count; i++)
							{
								TimerThread.s_QueuesCache.Remove(list[i]);
							}
						}
					}
				}
			}
			return timerQueue;
		}

		// Token: 0x060013E1 RID: 5089 RVA: 0x00069620 File Offset: 0x00067820
		private static void Prod()
		{
			TimerThread.s_ThreadReadyEvent.Set();
			if (Interlocked.CompareExchange(ref TimerThread.s_ThreadState, 1, 0) == 0)
			{
				new Thread(new ThreadStart(TimerThread.ThreadProc)).Start();
			}
		}

		// Token: 0x060013E2 RID: 5090 RVA: 0x00069660 File Offset: 0x00067860
		private static void ThreadProc()
		{
			Thread.CurrentThread.IsBackground = true;
			LinkedList<WeakReference> linkedList = TimerThread.s_Queues;
			lock (linkedList)
			{
				if (Interlocked.CompareExchange(ref TimerThread.s_ThreadState, 1, 1) == 1)
				{
					bool flag2 = true;
					while (flag2)
					{
						try
						{
							TimerThread.s_ThreadReadyEvent.Reset();
							for (;;)
							{
								if (TimerThread.s_NewQueues.Count > 0)
								{
									LinkedList<WeakReference> linkedList2 = TimerThread.s_NewQueues;
									lock (linkedList2)
									{
										for (LinkedListNode<WeakReference> linkedListNode = TimerThread.s_NewQueues.First; linkedListNode != null; linkedListNode = TimerThread.s_NewQueues.First)
										{
											TimerThread.s_NewQueues.Remove(linkedListNode);
											TimerThread.s_Queues.AddLast(linkedListNode);
										}
									}
								}
								int tickCount = Environment.TickCount;
								int num = 0;
								bool flag4 = false;
								LinkedListNode<WeakReference> linkedListNode2 = TimerThread.s_Queues.First;
								while (linkedListNode2 != null)
								{
									TimerThread.TimerQueue timerQueue = (TimerThread.TimerQueue)linkedListNode2.Value.Target;
									if (timerQueue == null)
									{
										LinkedListNode<WeakReference> next = linkedListNode2.Next;
										TimerThread.s_Queues.Remove(linkedListNode2);
										linkedListNode2 = next;
									}
									else
									{
										int num2;
										if (timerQueue.Fire(out num2) && (!flag4 || TimerThread.IsTickBetween(tickCount, num, num2)))
										{
											num = num2;
											flag4 = true;
										}
										linkedListNode2 = linkedListNode2.Next;
									}
								}
								int tickCount2 = Environment.TickCount;
								int num3 = (int)(flag4 ? (TimerThread.IsTickBetween(tickCount, num, tickCount2) ? (Math.Min((uint)(num - tickCount2), 2147483632U) + 15U) : 0U) : 30000U);
								int num4 = WaitHandle.WaitAny(TimerThread.s_ThreadEvents, num3, false);
								if (num4 == 0)
								{
									break;
								}
								if (num4 == 258 && !flag4)
								{
									Interlocked.CompareExchange(ref TimerThread.s_ThreadState, 0, 1);
									if (!TimerThread.s_ThreadReadyEvent.WaitOne(0, false) || Interlocked.CompareExchange(ref TimerThread.s_ThreadState, 1, 0) != 0)
									{
										goto IL_01AC;
									}
								}
							}
							flag2 = false;
							continue;
							IL_01AC:
							flag2 = false;
						}
						catch (Exception ex)
						{
							if (NclUtilities.IsFatal(ex))
							{
								throw;
							}
							if (Logging.On)
							{
								Logging.PrintError(Logging.Web, "TimerThread#" + Thread.CurrentThread.ManagedThreadId.ToString(NumberFormatInfo.InvariantInfo) + "::ThreadProc() - Exception:" + ex.ToString());
							}
							Thread.Sleep(1000);
						}
					}
				}
			}
		}

		// Token: 0x060013E3 RID: 5091 RVA: 0x000698D4 File Offset: 0x00067AD4
		private static void StopTimerThread()
		{
			Interlocked.Exchange(ref TimerThread.s_ThreadState, 2);
			TimerThread.s_ThreadShutdownEvent.Set();
		}

		// Token: 0x060013E4 RID: 5092 RVA: 0x000698ED File Offset: 0x00067AED
		private static bool IsTickBetween(int start, int end, int comparand)
		{
			return start <= comparand == end <= comparand != start <= end;
		}

		// Token: 0x060013E5 RID: 5093 RVA: 0x0006990C File Offset: 0x00067B0C
		private static void OnDomainUnload(object sender, EventArgs e)
		{
			try
			{
				TimerThread.StopTimerThread();
			}
			catch
			{
			}
		}

		// Token: 0x040015E2 RID: 5602
		private const int c_ThreadIdleTimeoutMilliseconds = 30000;

		// Token: 0x040015E3 RID: 5603
		private const int c_CacheScanPerIterations = 32;

		// Token: 0x040015E4 RID: 5604
		private const int c_TickCountResolution = 15;

		// Token: 0x040015E5 RID: 5605
		private static LinkedList<WeakReference> s_Queues = new LinkedList<WeakReference>();

		// Token: 0x040015E6 RID: 5606
		private static LinkedList<WeakReference> s_NewQueues = new LinkedList<WeakReference>();

		// Token: 0x040015E7 RID: 5607
		private static int s_ThreadState = 0;

		// Token: 0x040015E8 RID: 5608
		private static AutoResetEvent s_ThreadReadyEvent = new AutoResetEvent(false);

		// Token: 0x040015E9 RID: 5609
		private static ManualResetEvent s_ThreadShutdownEvent = new ManualResetEvent(false);

		// Token: 0x040015EA RID: 5610
		private static WaitHandle[] s_ThreadEvents = new WaitHandle[]
		{
			TimerThread.s_ThreadShutdownEvent,
			TimerThread.s_ThreadReadyEvent
		};

		// Token: 0x040015EB RID: 5611
		private static int s_CacheScanIteration;

		// Token: 0x040015EC RID: 5612
		private static Hashtable s_QueuesCache = new Hashtable();

		// Token: 0x0200075D RID: 1885
		internal abstract class Queue
		{
			// Token: 0x06004215 RID: 16917 RVA: 0x00112640 File Offset: 0x00110840
			internal Queue(int durationMilliseconds)
			{
				this.m_DurationMilliseconds = durationMilliseconds;
			}

			// Token: 0x17000F1B RID: 3867
			// (get) Token: 0x06004216 RID: 16918 RVA: 0x0011264F File Offset: 0x0011084F
			internal int Duration
			{
				get
				{
					return this.m_DurationMilliseconds;
				}
			}

			// Token: 0x06004217 RID: 16919 RVA: 0x00112657 File Offset: 0x00110857
			internal TimerThread.Timer CreateTimer()
			{
				return this.CreateTimer(null, null);
			}

			// Token: 0x06004218 RID: 16920
			internal abstract TimerThread.Timer CreateTimer(TimerThread.Callback callback, object context);

			// Token: 0x04003229 RID: 12841
			private readonly int m_DurationMilliseconds;
		}

		// Token: 0x0200075E RID: 1886
		internal abstract class Timer : IDisposable
		{
			// Token: 0x06004219 RID: 16921 RVA: 0x00112661 File Offset: 0x00110861
			internal Timer(int durationMilliseconds)
			{
				this.m_DurationMilliseconds = durationMilliseconds;
				this.m_StartTimeMilliseconds = Environment.TickCount;
			}

			// Token: 0x17000F1C RID: 3868
			// (get) Token: 0x0600421A RID: 16922 RVA: 0x0011267B File Offset: 0x0011087B
			internal int Duration
			{
				get
				{
					return this.m_DurationMilliseconds;
				}
			}

			// Token: 0x17000F1D RID: 3869
			// (get) Token: 0x0600421B RID: 16923 RVA: 0x00112683 File Offset: 0x00110883
			internal int StartTime
			{
				get
				{
					return this.m_StartTimeMilliseconds;
				}
			}

			// Token: 0x17000F1E RID: 3870
			// (get) Token: 0x0600421C RID: 16924 RVA: 0x0011268B File Offset: 0x0011088B
			internal int Expiration
			{
				get
				{
					return this.m_StartTimeMilliseconds + this.m_DurationMilliseconds;
				}
			}

			// Token: 0x17000F1F RID: 3871
			// (get) Token: 0x0600421D RID: 16925 RVA: 0x0011269C File Offset: 0x0011089C
			internal int TimeRemaining
			{
				get
				{
					if (this.HasExpired)
					{
						return 0;
					}
					if (this.Duration == -1)
					{
						return -1;
					}
					int tickCount = Environment.TickCount;
					int num = (int)(TimerThread.IsTickBetween(this.StartTime, this.Expiration, tickCount) ? Math.Min((uint)(this.Expiration - tickCount), 2147483647U) : 0U);
					if (num >= 2)
					{
						return num;
					}
					return num + 1;
				}
			}

			// Token: 0x0600421E RID: 16926
			internal abstract bool Cancel();

			// Token: 0x17000F20 RID: 3872
			// (get) Token: 0x0600421F RID: 16927
			internal abstract bool HasExpired { get; }

			// Token: 0x06004220 RID: 16928 RVA: 0x001126F7 File Offset: 0x001108F7
			public void Dispose()
			{
				this.Cancel();
			}

			// Token: 0x0400322A RID: 12842
			private readonly int m_StartTimeMilliseconds;

			// Token: 0x0400322B RID: 12843
			private readonly int m_DurationMilliseconds;
		}

		// Token: 0x0200075F RID: 1887
		// (Invoke) Token: 0x06004222 RID: 16930
		internal delegate void Callback(TimerThread.Timer timer, int timeNoticed, object context);

		// Token: 0x02000760 RID: 1888
		private enum TimerThreadState
		{
			// Token: 0x0400322D RID: 12845
			Idle,
			// Token: 0x0400322E RID: 12846
			Running,
			// Token: 0x0400322F RID: 12847
			Stopped
		}

		// Token: 0x02000761 RID: 1889
		private class TimerQueue : TimerThread.Queue
		{
			// Token: 0x06004225 RID: 16933 RVA: 0x00112700 File Offset: 0x00110900
			internal TimerQueue(int durationMilliseconds)
				: base(durationMilliseconds)
			{
				this.m_Timers = new TimerThread.TimerNode();
				this.m_Timers.Next = this.m_Timers;
				this.m_Timers.Prev = this.m_Timers;
			}

			// Token: 0x06004226 RID: 16934 RVA: 0x00112738 File Offset: 0x00110938
			internal override TimerThread.Timer CreateTimer(TimerThread.Callback callback, object context)
			{
				TimerThread.TimerNode timerNode = new TimerThread.TimerNode(callback, context, base.Duration, this.m_Timers);
				bool flag = false;
				TimerThread.TimerNode timers = this.m_Timers;
				lock (timers)
				{
					if (this.m_Timers.Next == this.m_Timers)
					{
						if (this.m_ThisHandle == IntPtr.Zero)
						{
							this.m_ThisHandle = (IntPtr)GCHandle.Alloc(this);
						}
						flag = true;
					}
					timerNode.Next = this.m_Timers;
					timerNode.Prev = this.m_Timers.Prev;
					this.m_Timers.Prev.Next = timerNode;
					this.m_Timers.Prev = timerNode;
				}
				if (flag)
				{
					TimerThread.Prod();
				}
				return timerNode;
			}

			// Token: 0x06004227 RID: 16935 RVA: 0x00112804 File Offset: 0x00110A04
			internal bool Fire(out int nextExpiration)
			{
				TimerThread.TimerNode timerNode;
				do
				{
					timerNode = this.m_Timers.Next;
					if (timerNode == this.m_Timers)
					{
						TimerThread.TimerNode timers = this.m_Timers;
						lock (timers)
						{
							timerNode = this.m_Timers.Next;
							if (timerNode == this.m_Timers)
							{
								if (this.m_ThisHandle != IntPtr.Zero)
								{
									((GCHandle)this.m_ThisHandle).Free();
									this.m_ThisHandle = IntPtr.Zero;
								}
								nextExpiration = 0;
								return false;
							}
						}
					}
				}
				while (timerNode.Fire());
				nextExpiration = timerNode.Expiration;
				return true;
			}

			// Token: 0x04003230 RID: 12848
			private IntPtr m_ThisHandle;

			// Token: 0x04003231 RID: 12849
			private readonly TimerThread.TimerNode m_Timers;
		}

		// Token: 0x02000762 RID: 1890
		private class InfiniteTimerQueue : TimerThread.Queue
		{
			// Token: 0x06004228 RID: 16936 RVA: 0x001128B8 File Offset: 0x00110AB8
			internal InfiniteTimerQueue()
				: base(-1)
			{
			}

			// Token: 0x06004229 RID: 16937 RVA: 0x001128C1 File Offset: 0x00110AC1
			internal override TimerThread.Timer CreateTimer(TimerThread.Callback callback, object context)
			{
				return new TimerThread.InfiniteTimer();
			}
		}

		// Token: 0x02000763 RID: 1891
		private class TimerNode : TimerThread.Timer
		{
			// Token: 0x0600422A RID: 16938 RVA: 0x001128C8 File Offset: 0x00110AC8
			internal TimerNode(TimerThread.Callback callback, object context, int durationMilliseconds, object queueLock)
				: base(durationMilliseconds)
			{
				if (callback != null)
				{
					this.m_Callback = callback;
					this.m_Context = context;
				}
				this.m_TimerState = TimerThread.TimerNode.TimerState.Ready;
				this.m_QueueLock = queueLock;
			}

			// Token: 0x0600422B RID: 16939 RVA: 0x001128F1 File Offset: 0x00110AF1
			internal TimerNode()
				: base(0)
			{
				this.m_TimerState = TimerThread.TimerNode.TimerState.Sentinel;
			}

			// Token: 0x17000F21 RID: 3873
			// (get) Token: 0x0600422C RID: 16940 RVA: 0x00112901 File Offset: 0x00110B01
			internal override bool HasExpired
			{
				get
				{
					return this.m_TimerState == TimerThread.TimerNode.TimerState.Fired;
				}
			}

			// Token: 0x17000F22 RID: 3874
			// (get) Token: 0x0600422D RID: 16941 RVA: 0x0011290C File Offset: 0x00110B0C
			// (set) Token: 0x0600422E RID: 16942 RVA: 0x00112914 File Offset: 0x00110B14
			internal TimerThread.TimerNode Next
			{
				get
				{
					return this.next;
				}
				set
				{
					this.next = value;
				}
			}

			// Token: 0x17000F23 RID: 3875
			// (get) Token: 0x0600422F RID: 16943 RVA: 0x0011291D File Offset: 0x00110B1D
			// (set) Token: 0x06004230 RID: 16944 RVA: 0x00112925 File Offset: 0x00110B25
			internal TimerThread.TimerNode Prev
			{
				get
				{
					return this.prev;
				}
				set
				{
					this.prev = value;
				}
			}

			// Token: 0x06004231 RID: 16945 RVA: 0x00112930 File Offset: 0x00110B30
			internal override bool Cancel()
			{
				if (this.m_TimerState == TimerThread.TimerNode.TimerState.Ready)
				{
					object queueLock = this.m_QueueLock;
					lock (queueLock)
					{
						if (this.m_TimerState == TimerThread.TimerNode.TimerState.Ready)
						{
							this.Next.Prev = this.Prev;
							this.Prev.Next = this.Next;
							this.Next = null;
							this.Prev = null;
							this.m_Callback = null;
							this.m_Context = null;
							this.m_TimerState = TimerThread.TimerNode.TimerState.Cancelled;
							return true;
						}
					}
					return false;
				}
				return false;
			}

			// Token: 0x06004232 RID: 16946 RVA: 0x001129C8 File Offset: 0x00110BC8
			internal bool Fire()
			{
				if (this.m_TimerState != TimerThread.TimerNode.TimerState.Ready)
				{
					return true;
				}
				int tickCount = Environment.TickCount;
				if (TimerThread.IsTickBetween(base.StartTime, base.Expiration, tickCount))
				{
					return false;
				}
				bool flag = false;
				object queueLock = this.m_QueueLock;
				lock (queueLock)
				{
					if (this.m_TimerState == TimerThread.TimerNode.TimerState.Ready)
					{
						this.m_TimerState = TimerThread.TimerNode.TimerState.Fired;
						this.Next.Prev = this.Prev;
						this.Prev.Next = this.Next;
						this.Next = null;
						this.Prev = null;
						flag = this.m_Callback != null;
					}
				}
				if (flag)
				{
					try
					{
						TimerThread.Callback callback = this.m_Callback;
						object context = this.m_Context;
						this.m_Callback = null;
						this.m_Context = null;
						callback(this, tickCount, context);
					}
					catch (Exception ex)
					{
						if (NclUtilities.IsFatal(ex))
						{
							throw;
						}
						if (Logging.On)
						{
							Logging.PrintError(Logging.Web, "TimerThreadTimer#" + base.StartTime.ToString(NumberFormatInfo.InvariantInfo) + "::Fire() - " + SR.GetString("net_log_exception_in_callback", new object[] { ex }));
						}
					}
				}
				return true;
			}

			// Token: 0x04003232 RID: 12850
			private TimerThread.TimerNode.TimerState m_TimerState;

			// Token: 0x04003233 RID: 12851
			private TimerThread.Callback m_Callback;

			// Token: 0x04003234 RID: 12852
			private object m_Context;

			// Token: 0x04003235 RID: 12853
			private object m_QueueLock;

			// Token: 0x04003236 RID: 12854
			private TimerThread.TimerNode next;

			// Token: 0x04003237 RID: 12855
			private TimerThread.TimerNode prev;

			// Token: 0x02000918 RID: 2328
			private enum TimerState
			{
				// Token: 0x04003D71 RID: 15729
				Ready,
				// Token: 0x04003D72 RID: 15730
				Fired,
				// Token: 0x04003D73 RID: 15731
				Cancelled,
				// Token: 0x04003D74 RID: 15732
				Sentinel
			}
		}

		// Token: 0x02000764 RID: 1892
		private class InfiniteTimer : TimerThread.Timer
		{
			// Token: 0x06004233 RID: 16947 RVA: 0x00112B0C File Offset: 0x00110D0C
			internal InfiniteTimer()
				: base(-1)
			{
			}

			// Token: 0x17000F24 RID: 3876
			// (get) Token: 0x06004234 RID: 16948 RVA: 0x00112B15 File Offset: 0x00110D15
			internal override bool HasExpired
			{
				get
				{
					return false;
				}
			}

			// Token: 0x06004235 RID: 16949 RVA: 0x00112B18 File Offset: 0x00110D18
			internal override bool Cancel()
			{
				return Interlocked.Exchange(ref this.cancelled, 1) == 0;
			}

			// Token: 0x04003238 RID: 12856
			private int cancelled;
		}
	}
}
