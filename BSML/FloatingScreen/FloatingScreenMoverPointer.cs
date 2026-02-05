using System;
using UnityEngine;
using VRUIControls;

namespace BeatSaberMarkupLanguage.FloatingScreen
{
	// Token: 0x02000093 RID: 147
	public class FloatingScreenMoverPointer : MonoBehaviour
	{
		// Token: 0x060002DE RID: 734 RVA: 0x0000E528 File Offset: 0x0000C728
		public virtual void Init(FloatingScreen floatingScreen)
		{
			this._floatingScreen = floatingScreen;
			this._screenHandle = floatingScreen.handle.transform;
			this._realPos = floatingScreen.transform.position;
			this._realRot = floatingScreen.transform.rotation;
			this._vrPointer = base.GetComponent<VRPointer>();
			this._isFpfc = Environment.CommandLine.Contains("fpfc");
		}

		// Token: 0x060002DF RID: 735 RVA: 0x0000E590 File Offset: 0x0000C790
		protected virtual void Update()
		{
			if (this._vrPointer.vrController != null && (this._vrPointer.vrController.triggerValue > 0.9f || Input.GetMouseButton(0)))
			{
				if (this._grabbingController != null)
				{
					return;
				}
				RaycastHit raycastHit;
				if (Physics.Raycast(this._vrPointer.vrController.position, this._vrPointer.vrController.forward, ref raycastHit, 50f))
				{
					if (raycastHit.transform != this._screenHandle)
					{
						return;
					}
					this._grabbingController = this._vrPointer.vrController;
					this._grabPos = this._vrPointer.vrController.transform.InverseTransformPoint(this._floatingScreen.transform.position);
					this._grabRot = Quaternion.Inverse(this._vrPointer.vrController.transform.rotation) * this._floatingScreen.transform.rotation;
					Action<Vector3, Quaternion> onGrab = this.OnGrab;
					if (onGrab != null)
					{
						onGrab(this._floatingScreen.transform.position, this._floatingScreen.transform.rotation);
					}
				}
			}
			if (this._grabbingController == null || (!this._isFpfc && this._grabbingController.triggerValue > 0.9f) || (this._isFpfc && Input.GetMouseButton(0)))
			{
				return;
			}
			this._grabbingController = null;
			Action<Vector3, Quaternion> onRelease = this.OnRelease;
			if (onRelease == null)
			{
				return;
			}
			onRelease(this._floatingScreen.transform.position, this._floatingScreen.transform.rotation);
		}

		// Token: 0x060002E0 RID: 736 RVA: 0x0000E73C File Offset: 0x0000C93C
		protected virtual void LateUpdate()
		{
			if (this._grabbingController != null)
			{
				float num = this._grabbingController.verticalAxisValue * Time.unscaledDeltaTime;
				if (this._grabPos.magnitude > 0.25f)
				{
					this._grabPos -= Vector3.forward * num;
				}
				else
				{
					this._grabPos -= Vector3.forward * Mathf.Clamp(num, float.MinValue, 0f);
				}
				this._realPos = this._grabbingController.transform.TransformPoint(this._grabPos);
				this._realRot = this._grabbingController.transform.rotation * this._grabRot;
				this._floatingScreen.transform.position = Vector3.Lerp(this._floatingScreen.transform.position, this._realPos, 10f * Time.unscaledDeltaTime);
				this._floatingScreen.transform.rotation = Quaternion.Slerp(this._floatingScreen.transform.rotation, this._realRot, 5f * Time.unscaledDeltaTime);
				return;
			}
		}

		// Token: 0x040000A6 RID: 166
		protected const float MinScrollDistance = 0.25f;

		// Token: 0x040000A7 RID: 167
		protected const float MaxLaserDistance = 50f;

		// Token: 0x040000A8 RID: 168
		protected VRPointer _vrPointer;

		// Token: 0x040000A9 RID: 169
		protected FloatingScreen _floatingScreen;

		// Token: 0x040000AA RID: 170
		protected Transform _screenHandle;

		// Token: 0x040000AB RID: 171
		protected VRController _grabbingController;

		// Token: 0x040000AC RID: 172
		protected Vector3 _grabPos;

		// Token: 0x040000AD RID: 173
		protected Quaternion _grabRot;

		// Token: 0x040000AE RID: 174
		protected Vector3 _realPos;

		// Token: 0x040000AF RID: 175
		protected Quaternion _realRot;

		// Token: 0x040000B0 RID: 176
		protected bool _isFpfc;

		// Token: 0x040000B1 RID: 177
		public Action<Vector3, Quaternion> OnGrab;

		// Token: 0x040000B2 RID: 178
		public Action<Vector3, Quaternion> OnRelease;
	}
}
