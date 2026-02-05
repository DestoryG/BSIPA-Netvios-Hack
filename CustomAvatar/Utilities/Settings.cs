using System;
using UnityEngine;

namespace CustomAvatar.Utilities
{
	// Token: 0x02000022 RID: 34
	internal class Settings
	{
		// Token: 0x04000120 RID: 288
		public bool isAvatarVisibleInFirstPerson = true;

		// Token: 0x04000121 RID: 289
		public AvatarResizeMode resizeMode = AvatarResizeMode.Height;

		// Token: 0x04000122 RID: 290
		public bool enableFloorAdjust = false;

		// Token: 0x04000123 RID: 291
		public bool moveFloorWithRoomAdjust = false;

		// Token: 0x04000124 RID: 292
		public string previousAvatarPath = null;

		// Token: 0x04000125 RID: 293
		public float playerArmSpan = 1.7f;

		// Token: 0x04000126 RID: 294
		public bool calibrateFullBodyTrackingOnStart = false;

		// Token: 0x04000127 RID: 295
		public float cameraNearClipPlane = 0.1f;

		// Token: 0x04000128 RID: 296
		public Settings.FullBodyMotionSmoothing fullBodyMotionSmoothing = new Settings.FullBodyMotionSmoothing();

		// Token: 0x04000129 RID: 297
		public Settings.FullBodyCalibration fullBodyCalibration = new Settings.FullBodyCalibration();

		// Token: 0x02000043 RID: 67
		public class FullBodyMotionSmoothing
		{
			// Token: 0x040001D2 RID: 466
			public Settings.TrackedPointSmoothing waist = new Settings.TrackedPointSmoothing
			{
				position = 15f,
				rotation = 10f
			};

			// Token: 0x040001D3 RID: 467
			public Settings.TrackedPointSmoothing feet = new Settings.TrackedPointSmoothing
			{
				position = 13f,
				rotation = 17f
			};
		}

		// Token: 0x02000044 RID: 68
		public class TrackedPointSmoothing
		{
			// Token: 0x040001D4 RID: 468
			public float position;

			// Token: 0x040001D5 RID: 469
			public float rotation;
		}

		// Token: 0x02000045 RID: 69
		public class FullBodyCalibration
		{
			// Token: 0x040001D6 RID: 470
			public Pose leftLeg = Pose.identity;

			// Token: 0x040001D7 RID: 471
			public Pose rightLeg = Pose.identity;

			// Token: 0x040001D8 RID: 472
			public Pose pelvis = Pose.identity;
		}
	}
}
