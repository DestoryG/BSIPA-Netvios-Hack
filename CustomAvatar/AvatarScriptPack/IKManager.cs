using System;
using CustomAvatar;
using UnityEngine;

namespace AvatarScriptPack
{
	// Token: 0x0200000B RID: 11
	[Obsolete("Use VRIKManager")]
	internal class IKManager : MonoBehaviour
	{
		// Token: 0x0600000A RID: 10 RVA: 0x00002090 File Offset: 0x00000290
		public void Start()
		{
			Plugin.logger.Warn("Avatar is still using the legacy IKManager; please migrate to VRIKManager");
			VRIKManager vrikmanager = base.gameObject.AddComponent<VRIKManager>();
			vrikmanager.solver_spine_headTarget = this.HeadTarget;
			vrikmanager.solver_leftArm_target = this.LeftHandTarget;
			vrikmanager.solver_rightArm_target = this.RightHandTarget;
		}

		// Token: 0x04000001 RID: 1
		public Transform HeadTarget;

		// Token: 0x04000002 RID: 2
		public Transform LeftHandTarget;

		// Token: 0x04000003 RID: 3
		public Transform RightHandTarget;
	}
}
