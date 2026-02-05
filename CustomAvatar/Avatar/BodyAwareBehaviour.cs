using System;
using UnityEngine;

namespace CustomAvatar.Avatar
{
	// Token: 0x02000038 RID: 56
	internal abstract class BodyAwareBehaviour : MonoBehaviour
	{
		// Token: 0x06000131 RID: 305 RVA: 0x00008D3C File Offset: 0x00006F3C
		protected virtual void Start()
		{
			this._head = base.transform.Find("Head");
			this._body = base.transform.Find("Body");
			this._leftHand = base.transform.Find("LeftHand");
			this._rightHand = base.transform.Find("RightHand");
			this._leftLeg = base.transform.Find("LeftLeg");
			this._rightLeg = base.transform.Find("RightLeg");
			this._pelvis = base.transform.Find("Pelvis");
		}

		// Token: 0x040001A5 RID: 421
		protected Transform _head;

		// Token: 0x040001A6 RID: 422
		protected Transform _body;

		// Token: 0x040001A7 RID: 423
		protected Transform _leftHand;

		// Token: 0x040001A8 RID: 424
		protected Transform _rightHand;

		// Token: 0x040001A9 RID: 425
		protected Transform _leftLeg;

		// Token: 0x040001AA RID: 426
		protected Transform _rightLeg;

		// Token: 0x040001AB RID: 427
		protected Transform _pelvis;
	}
}
