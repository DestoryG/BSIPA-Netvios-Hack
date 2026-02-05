using System;
using System.Runtime.InteropServices;
using System.Text;

namespace Valve.VR
{
	// Token: 0x0200001F RID: 31
	public class CVRResources
	{
		// Token: 0x0600012B RID: 299 RVA: 0x00003CE5 File Offset: 0x00001EE5
		internal CVRResources(IntPtr pInterface)
		{
			this.FnTable = (IVRResources)Marshal.PtrToStructure(pInterface, typeof(IVRResources));
		}

		// Token: 0x0600012C RID: 300 RVA: 0x00003D08 File Offset: 0x00001F08
		public uint LoadSharedResource(string pchResourceName, string pchBuffer, uint unBufferLen)
		{
			return this.FnTable.LoadSharedResource(pchResourceName, pchBuffer, unBufferLen);
		}

		// Token: 0x0600012D RID: 301 RVA: 0x00003D1D File Offset: 0x00001F1D
		public uint GetResourceFullPath(string pchResourceName, string pchResourceTypeDirectory, StringBuilder pchPathBuffer, uint unBufferLen)
		{
			return this.FnTable.GetResourceFullPath(pchResourceName, pchResourceTypeDirectory, pchPathBuffer, unBufferLen);
		}

		// Token: 0x04000153 RID: 339
		private IVRResources FnTable;
	}
}
