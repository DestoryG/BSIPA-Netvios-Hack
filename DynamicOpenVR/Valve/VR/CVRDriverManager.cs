using System;
using System.Runtime.InteropServices;
using System.Text;

namespace Valve.VR
{
	// Token: 0x02000020 RID: 32
	public class CVRDriverManager
	{
		// Token: 0x0600012E RID: 302 RVA: 0x00003D34 File Offset: 0x00001F34
		internal CVRDriverManager(IntPtr pInterface)
		{
			this.FnTable = (IVRDriverManager)Marshal.PtrToStructure(pInterface, typeof(IVRDriverManager));
		}

		// Token: 0x0600012F RID: 303 RVA: 0x00003D57 File Offset: 0x00001F57
		public uint GetDriverCount()
		{
			return this.FnTable.GetDriverCount();
		}

		// Token: 0x06000130 RID: 304 RVA: 0x00003D69 File Offset: 0x00001F69
		public uint GetDriverName(uint nDriver, StringBuilder pchValue, uint unBufferSize)
		{
			return this.FnTable.GetDriverName(nDriver, pchValue, unBufferSize);
		}

		// Token: 0x06000131 RID: 305 RVA: 0x00003D7E File Offset: 0x00001F7E
		public ulong GetDriverHandle(string pchDriverName)
		{
			return this.FnTable.GetDriverHandle(pchDriverName);
		}

		// Token: 0x04000154 RID: 340
		private IVRDriverManager FnTable;
	}
}
