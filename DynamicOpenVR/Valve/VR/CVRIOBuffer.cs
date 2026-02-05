using System;
using System.Runtime.InteropServices;

namespace Valve.VR
{
	// Token: 0x02000022 RID: 34
	public class CVRIOBuffer
	{
		// Token: 0x0600014C RID: 332 RVA: 0x00003FFC File Offset: 0x000021FC
		internal CVRIOBuffer(IntPtr pInterface)
		{
			this.FnTable = (IVRIOBuffer)Marshal.PtrToStructure(pInterface, typeof(IVRIOBuffer));
		}

		// Token: 0x0600014D RID: 333 RVA: 0x0000401F File Offset: 0x0000221F
		public EIOBufferError Open(string pchPath, EIOBufferMode mode, uint unElementSize, uint unElements, ref ulong pulBuffer)
		{
			pulBuffer = 0UL;
			return this.FnTable.Open(pchPath, mode, unElementSize, unElements, ref pulBuffer);
		}

		// Token: 0x0600014E RID: 334 RVA: 0x0000403D File Offset: 0x0000223D
		public EIOBufferError Close(ulong ulBuffer)
		{
			return this.FnTable.Close(ulBuffer);
		}

		// Token: 0x0600014F RID: 335 RVA: 0x00004050 File Offset: 0x00002250
		public EIOBufferError Read(ulong ulBuffer, IntPtr pDst, uint unBytes, ref uint punRead)
		{
			punRead = 0U;
			return this.FnTable.Read(ulBuffer, pDst, unBytes, ref punRead);
		}

		// Token: 0x06000150 RID: 336 RVA: 0x0000406B File Offset: 0x0000226B
		public EIOBufferError Write(ulong ulBuffer, IntPtr pSrc, uint unBytes)
		{
			return this.FnTable.Write(ulBuffer, pSrc, unBytes);
		}

		// Token: 0x06000151 RID: 337 RVA: 0x00004080 File Offset: 0x00002280
		public ulong PropertyContainer(ulong ulBuffer)
		{
			return this.FnTable.PropertyContainer(ulBuffer);
		}

		// Token: 0x06000152 RID: 338 RVA: 0x00004093 File Offset: 0x00002293
		public bool HasReaders(ulong ulBuffer)
		{
			return this.FnTable.HasReaders(ulBuffer);
		}

		// Token: 0x04000156 RID: 342
		private IVRIOBuffer FnTable;
	}
}
