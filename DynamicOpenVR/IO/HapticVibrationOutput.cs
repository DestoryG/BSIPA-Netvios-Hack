using System;

namespace DynamicOpenVR.IO
{
	// Token: 0x020000D8 RID: 216
	public class HapticVibrationOutput : OVRAction
	{
		// Token: 0x060001ED RID: 493 RVA: 0x000060E1 File Offset: 0x000042E1
		public HapticVibrationOutput(string name)
			: base(name)
		{
		}

		// Token: 0x060001EE RID: 494 RVA: 0x000060EA File Offset: 0x000042EA
		public void TriggerHapticVibration(float durationSeconds, float magnitude, float frequency = 150f)
		{
			OpenVRWrapper.TriggerHapticVibrationAction(base.handle, 0f, durationSeconds, frequency, magnitude);
		}
	}
}
