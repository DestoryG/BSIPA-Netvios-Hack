using System;

namespace CustomAvatar.Tracking
{
	// Token: 0x0200002B RID: 43
	internal enum TrackedDeviceRole
	{
		// Token: 0x04000148 RID: 328
		Unknown,
		// Token: 0x04000149 RID: 329
		[TrackedDeviceType("vive")]
		ViveHeadset,
		// Token: 0x0400014A RID: 330
		[TrackedDeviceType("knuckles")]
		ValveIndexController,
		// Token: 0x0400014B RID: 331
		[TrackedDeviceType("vive_tracker")]
		ViveTracker,
		// Token: 0x0400014C RID: 332
		[TrackedDeviceType("vive_tracker_handed")]
		HeldInHand,
		// Token: 0x0400014D RID: 333
		[TrackedDeviceType("vive_tracker_left_foot")]
		LeftFoot,
		// Token: 0x0400014E RID: 334
		[TrackedDeviceType("vive_tracker_right_foot")]
		RightFoot,
		// Token: 0x0400014F RID: 335
		[TrackedDeviceType("vive_tracker_left_shoulder")]
		LeftShoulder,
		// Token: 0x04000150 RID: 336
		[TrackedDeviceType("vive_tracker_right_shoulder")]
		RightShoulder,
		// Token: 0x04000151 RID: 337
		[TrackedDeviceType("vive_tracker_waist")]
		Waist,
		// Token: 0x04000152 RID: 338
		[TrackedDeviceType("vive_tracker_chest")]
		Chest,
		// Token: 0x04000153 RID: 339
		[TrackedDeviceType("vive_tracker_camera")]
		Camera,
		// Token: 0x04000154 RID: 340
		[TrackedDeviceType("vive_tracker_keyboard")]
		Keyboard,
		// Token: 0x04000155 RID: 341
		[TrackedDeviceType("kinect_device")]
		KinectToVrTracker
	}
}
