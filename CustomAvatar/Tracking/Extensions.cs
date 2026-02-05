using System;
using UnityEngine.XR;

namespace CustomAvatar.Tracking
{
	// Token: 0x02000024 RID: 36
	internal static class Extensions
	{
		// Token: 0x06000080 RID: 128 RVA: 0x0000498C File Offset: 0x00002B8C
		public static bool HasCharacteristics(this InputDevice inputDevice, InputDeviceCharacteristics characteristics)
		{
			return (inputDevice.characteristics & characteristics) == characteristics;
		}
	}
}
