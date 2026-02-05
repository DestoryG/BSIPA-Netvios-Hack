using System;
using System.Diagnostics.Tracing;
using System.Security;

namespace System
{
	// Token: 0x02000062 RID: 98
	[EventSource(Name = "Microsoft-DotNETRuntime-PinnableBufferCache-System")]
	internal sealed class PinnableBufferCacheEventSource : EventSource
	{
		// Token: 0x06000434 RID: 1076 RVA: 0x0001E498 File Offset: 0x0001C698
		[Event(1, Level = EventLevel.Verbose)]
		public void DebugMessage(string message)
		{
			if (base.IsEnabled())
			{
				base.WriteEvent(1, message);
			}
		}

		// Token: 0x06000435 RID: 1077 RVA: 0x0001E4AA File Offset: 0x0001C6AA
		[Event(2, Level = EventLevel.Verbose)]
		public void DebugMessage1(string message, long value)
		{
			if (base.IsEnabled())
			{
				base.WriteEvent(2, message, value);
			}
		}

		// Token: 0x06000436 RID: 1078 RVA: 0x0001E4BD File Offset: 0x0001C6BD
		[Event(3, Level = EventLevel.Verbose)]
		public void DebugMessage2(string message, long value1, long value2)
		{
			if (base.IsEnabled())
			{
				base.WriteEvent(3, new object[] { message, value1, value2 });
			}
		}

		// Token: 0x06000437 RID: 1079 RVA: 0x0001E4EA File Offset: 0x0001C6EA
		[Event(18, Level = EventLevel.Verbose)]
		public void DebugMessage3(string message, long value1, long value2, long value3)
		{
			if (base.IsEnabled())
			{
				base.WriteEvent(18, new object[] { message, value1, value2, value3 });
			}
		}

		// Token: 0x06000438 RID: 1080 RVA: 0x0001E522 File Offset: 0x0001C722
		[Event(4)]
		public void Create(string cacheName)
		{
			if (base.IsEnabled())
			{
				base.WriteEvent(4, cacheName);
			}
		}

		// Token: 0x06000439 RID: 1081 RVA: 0x0001E534 File Offset: 0x0001C734
		[Event(5, Level = EventLevel.Verbose)]
		public void AllocateBuffer(string cacheName, ulong objectId, int objectHash, int objectGen, int freeCountAfter)
		{
			if (base.IsEnabled())
			{
				base.WriteEvent(5, new object[] { cacheName, objectId, objectHash, objectGen, freeCountAfter });
			}
		}

		// Token: 0x0600043A RID: 1082 RVA: 0x0001E580 File Offset: 0x0001C780
		[Event(6)]
		public void AllocateBufferFromNotGen2(string cacheName, int notGen2CountAfter)
		{
			if (base.IsEnabled())
			{
				base.WriteEvent(6, cacheName, notGen2CountAfter);
			}
		}

		// Token: 0x0600043B RID: 1083 RVA: 0x0001E593 File Offset: 0x0001C793
		[Event(7)]
		public void AllocateBufferCreatingNewBuffers(string cacheName, int totalBuffsBefore, int objectCount)
		{
			if (base.IsEnabled())
			{
				base.WriteEvent(7, cacheName, totalBuffsBefore, objectCount);
			}
		}

		// Token: 0x0600043C RID: 1084 RVA: 0x0001E5A7 File Offset: 0x0001C7A7
		[Event(8)]
		public void AllocateBufferAged(string cacheName, int agedCount)
		{
			if (base.IsEnabled())
			{
				base.WriteEvent(8, cacheName, agedCount);
			}
		}

		// Token: 0x0600043D RID: 1085 RVA: 0x0001E5BA File Offset: 0x0001C7BA
		[Event(9)]
		public void AllocateBufferFreeListEmpty(string cacheName, int notGen2CountBefore)
		{
			if (base.IsEnabled())
			{
				base.WriteEvent(9, cacheName, notGen2CountBefore);
			}
		}

		// Token: 0x0600043E RID: 1086 RVA: 0x0001E5CE File Offset: 0x0001C7CE
		[Event(10, Level = EventLevel.Verbose)]
		public void FreeBuffer(string cacheName, ulong objectId, int objectHash, int freeCountBefore)
		{
			if (base.IsEnabled())
			{
				base.WriteEvent(10, new object[] { cacheName, objectId, objectHash, freeCountBefore });
			}
		}

		// Token: 0x0600043F RID: 1087 RVA: 0x0001E606 File Offset: 0x0001C806
		[Event(11)]
		public void FreeBufferStillTooYoung(string cacheName, int notGen2CountBefore)
		{
			if (base.IsEnabled())
			{
				base.WriteEvent(11, cacheName, notGen2CountBefore);
			}
		}

		// Token: 0x06000440 RID: 1088 RVA: 0x0001E61A File Offset: 0x0001C81A
		[Event(13)]
		public void TrimCheck(string cacheName, int totalBuffs, bool neededMoreThanFreeList, int deltaMSec)
		{
			if (base.IsEnabled())
			{
				base.WriteEvent(13, new object[] { cacheName, totalBuffs, neededMoreThanFreeList, deltaMSec });
			}
		}

		// Token: 0x06000441 RID: 1089 RVA: 0x0001E652 File Offset: 0x0001C852
		[Event(14)]
		public void TrimFree(string cacheName, int totalBuffs, int freeListCount, int toBeFreed)
		{
			if (base.IsEnabled())
			{
				base.WriteEvent(14, new object[] { cacheName, totalBuffs, freeListCount, toBeFreed });
			}
		}

		// Token: 0x06000442 RID: 1090 RVA: 0x0001E68A File Offset: 0x0001C88A
		[Event(15)]
		public void TrimExperiment(string cacheName, int totalBuffs, int freeListCount, int numTrimTrial)
		{
			if (base.IsEnabled())
			{
				base.WriteEvent(15, new object[] { cacheName, totalBuffs, freeListCount, numTrimTrial });
			}
		}

		// Token: 0x06000443 RID: 1091 RVA: 0x0001E6C2 File Offset: 0x0001C8C2
		[Event(16)]
		public void TrimFreeSizeOK(string cacheName, int totalBuffs, int freeListCount)
		{
			if (base.IsEnabled())
			{
				base.WriteEvent(16, cacheName, totalBuffs, freeListCount);
			}
		}

		// Token: 0x06000444 RID: 1092 RVA: 0x0001E6D7 File Offset: 0x0001C8D7
		[Event(17)]
		public void TrimFlush(string cacheName, int totalBuffs, int freeListCount, int notGen2CountBefore)
		{
			if (base.IsEnabled())
			{
				base.WriteEvent(17, new object[] { cacheName, totalBuffs, freeListCount, notGen2CountBefore });
			}
		}

		// Token: 0x06000445 RID: 1093 RVA: 0x0001E70F File Offset: 0x0001C90F
		[Event(20)]
		public void AgePendingBuffersResults(string cacheName, int promotedToFreeListCount, int heldBackCount)
		{
			if (base.IsEnabled())
			{
				base.WriteEvent(20, cacheName, promotedToFreeListCount, heldBackCount);
			}
		}

		// Token: 0x06000446 RID: 1094 RVA: 0x0001E724 File Offset: 0x0001C924
		[Event(21)]
		public void WalkFreeListResult(string cacheName, int freeListCount, int gen0BuffersInFreeList)
		{
			if (base.IsEnabled())
			{
				base.WriteEvent(21, cacheName, freeListCount, gen0BuffersInFreeList);
			}
		}

		// Token: 0x06000447 RID: 1095 RVA: 0x0001E739 File Offset: 0x0001C939
		[Event(22)]
		public void FreeBufferNull(string cacheName, int freeCountBefore)
		{
			if (base.IsEnabled())
			{
				base.WriteEvent(22, cacheName, freeCountBefore);
			}
		}

		// Token: 0x06000448 RID: 1096 RVA: 0x0001E750 File Offset: 0x0001C950
		internal static ulong AddressOf(object obj)
		{
			byte[] array = obj as byte[];
			if (array != null)
			{
				return (ulong)PinnableBufferCacheEventSource.AddressOfByteArray(array);
			}
			return 0UL;
		}

		// Token: 0x06000449 RID: 1097 RVA: 0x0001E770 File Offset: 0x0001C970
		[SecuritySafeCritical]
		internal unsafe static long AddressOfByteArray(byte[] array)
		{
			if (array == null)
			{
				return 0L;
			}
			byte* ptr;
			if (array == null || array.Length == 0)
			{
				ptr = null;
			}
			else
			{
				ptr = &array[0];
			}
			return ptr - 2 * sizeof(void*);
		}

		// Token: 0x0400052E RID: 1326
		public static readonly PinnableBufferCacheEventSource Log = new PinnableBufferCacheEventSource();
	}
}
