using System;
using System.Threading;

namespace System.Net.Sockets
{
	// Token: 0x0200038D RID: 909
	internal class OverlappedCache
	{
		// Token: 0x06002225 RID: 8741 RVA: 0x000A3672 File Offset: 0x000A1872
		internal OverlappedCache(Overlapped overlapped, object[] pinnedObjectsArray, IOCompletionCallback callback)
		{
			this.m_Overlapped = overlapped;
			this.m_PinnedObjects = pinnedObjectsArray;
			this.m_PinnedObjectsArray = pinnedObjectsArray;
			this.m_NativeOverlapped = new SafeNativeOverlapped(overlapped.UnsafePack(callback, pinnedObjectsArray));
		}

		// Token: 0x06002226 RID: 8742 RVA: 0x000A36A2 File Offset: 0x000A18A2
		internal OverlappedCache(Overlapped overlapped, object pinnedObjects, IOCompletionCallback callback, bool alreadyTriedCast)
		{
			this.m_Overlapped = overlapped;
			this.m_PinnedObjects = pinnedObjects;
			this.m_PinnedObjectsArray = (alreadyTriedCast ? null : NclConstants.EmptyObjectArray);
			this.m_NativeOverlapped = new SafeNativeOverlapped(overlapped.UnsafePack(callback, pinnedObjects));
		}

		// Token: 0x170008BE RID: 2238
		// (get) Token: 0x06002227 RID: 8743 RVA: 0x000A36DD File Offset: 0x000A18DD
		internal Overlapped Overlapped
		{
			get
			{
				return this.m_Overlapped;
			}
		}

		// Token: 0x170008BF RID: 2239
		// (get) Token: 0x06002228 RID: 8744 RVA: 0x000A36E5 File Offset: 0x000A18E5
		internal SafeNativeOverlapped NativeOverlapped
		{
			get
			{
				return this.m_NativeOverlapped;
			}
		}

		// Token: 0x170008C0 RID: 2240
		// (get) Token: 0x06002229 RID: 8745 RVA: 0x000A36ED File Offset: 0x000A18ED
		internal object PinnedObjects
		{
			get
			{
				return this.m_PinnedObjects;
			}
		}

		// Token: 0x170008C1 RID: 2241
		// (get) Token: 0x0600222A RID: 8746 RVA: 0x000A36F8 File Offset: 0x000A18F8
		internal object[] PinnedObjectsArray
		{
			get
			{
				object[] array = this.m_PinnedObjectsArray;
				if (array != null && array.Length == 0)
				{
					array = this.m_PinnedObjects as object[];
					if (array != null && array.Length == 0)
					{
						this.m_PinnedObjectsArray = null;
					}
					else
					{
						this.m_PinnedObjectsArray = array;
					}
				}
				return this.m_PinnedObjectsArray;
			}
		}

		// Token: 0x0600222B RID: 8747 RVA: 0x000A373C File Offset: 0x000A193C
		internal void Free()
		{
			this.InternalFree();
			GC.SuppressFinalize(this);
		}

		// Token: 0x0600222C RID: 8748 RVA: 0x000A374A File Offset: 0x000A194A
		private void InternalFree()
		{
			this.m_Overlapped = null;
			this.m_PinnedObjects = null;
			if (this.m_NativeOverlapped != null)
			{
				if (!this.m_NativeOverlapped.IsInvalid)
				{
					this.m_NativeOverlapped.Dispose();
				}
				this.m_NativeOverlapped = null;
			}
		}

		// Token: 0x0600222D RID: 8749 RVA: 0x000A3784 File Offset: 0x000A1984
		internal static void InterlockedFree(ref OverlappedCache overlappedCache)
		{
			OverlappedCache overlappedCache2 = ((overlappedCache == null) ? null : Interlocked.Exchange<OverlappedCache>(ref overlappedCache, null));
			if (overlappedCache2 != null)
			{
				overlappedCache2.Free();
			}
		}

		// Token: 0x0600222E RID: 8750 RVA: 0x000A37AC File Offset: 0x000A19AC
		~OverlappedCache()
		{
			if (!NclUtilities.HasShutdownStarted)
			{
				this.InternalFree();
			}
		}

		// Token: 0x04001F5C RID: 8028
		internal Overlapped m_Overlapped;

		// Token: 0x04001F5D RID: 8029
		internal SafeNativeOverlapped m_NativeOverlapped;

		// Token: 0x04001F5E RID: 8030
		internal object m_PinnedObjects;

		// Token: 0x04001F5F RID: 8031
		internal object[] m_PinnedObjectsArray;
	}
}
