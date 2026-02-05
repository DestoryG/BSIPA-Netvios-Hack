using System;
using Valve.VR;

namespace DynamicOpenVR.Exceptions
{
	// Token: 0x020000CD RID: 205
	public class OpenVRInputException : Exception
	{
		// Token: 0x17000019 RID: 25
		// (get) Token: 0x060001AE RID: 430 RVA: 0x00005E45 File Offset: 0x00004045
		public EVRInputError Error { get; }

		// Token: 0x060001AF RID: 431 RVA: 0x00005E4D File Offset: 0x0000404D
		internal OpenVRInputException(string message, EVRInputError error)
			: base(message)
		{
			this.Error = error;
		}
	}
}
