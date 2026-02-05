using System;
using Valve.VR;

namespace DynamicOpenVR.Exceptions
{
	// Token: 0x020000CE RID: 206
	public class OpenVRInitException : Exception
	{
		// Token: 0x1700001A RID: 26
		// (get) Token: 0x060001B0 RID: 432 RVA: 0x00005E5D File Offset: 0x0000405D
		public EVRInitError Error { get; }

		// Token: 0x060001B1 RID: 433 RVA: 0x00005E65 File Offset: 0x00004065
		internal OpenVRInitException(string message)
			: base(message)
		{
		}

		// Token: 0x060001B2 RID: 434 RVA: 0x00005E6E File Offset: 0x0000406E
		internal OpenVRInitException(EVRInitError error)
			: base("Failed to initialize OpenVR: " + error.ToString())
		{
			this.Error = error;
		}
	}
}
