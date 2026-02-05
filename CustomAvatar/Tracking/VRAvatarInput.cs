using System;
using UnityEngine;

namespace CustomAvatar.Tracking
{
	// Token: 0x02000026 RID: 38
	internal class VRAvatarInput : AvatarInput
	{
		// Token: 0x06000081 RID: 129 RVA: 0x000049AC File Offset: 0x00002BAC
		public VRAvatarInput()
		{
			this._deviceManager.deviceAdded += delegate(TrackedDeviceState device, DeviceUse use)
			{
				base.InvokeInputChanged();
			};
			this._deviceManager.deviceRemoved += delegate(TrackedDeviceState device, DeviceUse use)
			{
				base.InvokeInputChanged();
			};
			this._deviceManager.deviceTrackingAcquired += delegate(TrackedDeviceState device, DeviceUse use)
			{
				base.InvokeInputChanged();
			};
			this._deviceManager.deviceTrackingLost += delegate(TrackedDeviceState device, DeviceUse use)
			{
				base.InvokeInputChanged();
			};
		}

		// Token: 0x06000082 RID: 130 RVA: 0x00004A2C File Offset: 0x00002C2C
		public override bool TryGetHeadPose(out Pose pose)
		{
			return this.TryGetPose(this._deviceManager.head, out pose);
		}

		// Token: 0x06000083 RID: 131 RVA: 0x00004A40 File Offset: 0x00002C40
		public override bool TryGetLeftHandPose(out Pose pose)
		{
			return this.TryGetPose(this._deviceManager.leftHand, out pose);
		}

		// Token: 0x06000084 RID: 132 RVA: 0x00004A54 File Offset: 0x00002C54
		public override bool TryGetRightHandPose(out Pose pose)
		{
			return this.TryGetPose(this._deviceManager.rightHand, out pose);
		}

		// Token: 0x06000085 RID: 133 RVA: 0x00004A68 File Offset: 0x00002C68
		public override bool TryGetWaistPose(out Pose pose)
		{
			return this.TryGetPose(this._deviceManager.waist, out pose);
		}

		// Token: 0x06000086 RID: 134 RVA: 0x00004A7C File Offset: 0x00002C7C
		public override bool TryGetLeftFootPose(out Pose pose)
		{
			return this.TryGetPose(this._deviceManager.leftFoot, out pose);
		}

		// Token: 0x06000087 RID: 135 RVA: 0x00004A90 File Offset: 0x00002C90
		public override bool TryGetRightFootPose(out Pose pose)
		{
			return this.TryGetPose(this._deviceManager.rightFoot, out pose);
		}

		// Token: 0x06000088 RID: 136 RVA: 0x00004AA4 File Offset: 0x00002CA4
		private bool TryGetPose(TrackedDeviceState device, out Pose pose)
		{
			bool flag = !device.found || !device.tracked;
			bool flag2;
			if (flag)
			{
				pose = Pose.identity;
				flag2 = false;
			}
			else
			{
				pose = new Pose(device.position, device.rotation);
				flag2 = true;
			}
			return flag2;
		}

		// Token: 0x04000132 RID: 306
		private readonly TrackedDeviceManager _deviceManager = PersistentSingleton<TrackedDeviceManager>.instance;
	}
}
