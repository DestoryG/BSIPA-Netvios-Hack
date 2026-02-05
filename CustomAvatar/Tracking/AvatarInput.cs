using System;
using System.Diagnostics;
using UnityEngine;

namespace CustomAvatar.Tracking
{
	// Token: 0x02000023 RID: 35
	public abstract class AvatarInput
	{
		// Token: 0x14000004 RID: 4
		// (add) Token: 0x06000076 RID: 118 RVA: 0x00004848 File Offset: 0x00002A48
		// (remove) Token: 0x06000077 RID: 119 RVA: 0x00004880 File Offset: 0x00002A80
		[field: DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event Action inputChanged;

		// Token: 0x06000078 RID: 120 RVA: 0x000048B5 File Offset: 0x00002AB5
		protected void InvokeInputChanged()
		{
			Action action = this.inputChanged;
			if (action != null)
			{
				action();
			}
		}

		// Token: 0x06000079 RID: 121 RVA: 0x000048CC File Offset: 0x00002ACC
		public virtual bool TryGetHeadPose(out Pose pose)
		{
			pose = Pose.identity;
			return false;
		}

		// Token: 0x0600007A RID: 122 RVA: 0x000048EC File Offset: 0x00002AEC
		public virtual bool TryGetLeftHandPose(out Pose pose)
		{
			pose = Pose.identity;
			return false;
		}

		// Token: 0x0600007B RID: 123 RVA: 0x0000490C File Offset: 0x00002B0C
		public virtual bool TryGetRightHandPose(out Pose pose)
		{
			pose = Pose.identity;
			return false;
		}

		// Token: 0x0600007C RID: 124 RVA: 0x0000492C File Offset: 0x00002B2C
		public virtual bool TryGetWaistPose(out Pose pose)
		{
			pose = Pose.identity;
			return false;
		}

		// Token: 0x0600007D RID: 125 RVA: 0x0000494C File Offset: 0x00002B4C
		public virtual bool TryGetLeftFootPose(out Pose pose)
		{
			pose = Pose.identity;
			return false;
		}

		// Token: 0x0600007E RID: 126 RVA: 0x0000496C File Offset: 0x00002B6C
		public virtual bool TryGetRightFootPose(out Pose pose)
		{
			pose = Pose.identity;
			return false;
		}
	}
}
