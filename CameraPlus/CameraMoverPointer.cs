using System;
using UnityEngine;
using VRUIControls;

namespace CameraPlus
{
	// Token: 0x02000002 RID: 2
	public class CameraMoverPointer : MonoBehaviour
	{
		// Token: 0x06000001 RID: 1 RVA: 0x00002050 File Offset: 0x00000250
		public virtual void Init(CameraPlusBehaviour cameraPlus, Transform cameraCube)
		{
			this._cameraPlus = cameraPlus;
			this._cameraCube = cameraCube;
			this._realPos = this._cameraPlus.Config.Position;
			this._realRot = Quaternion.Euler(this._cameraPlus.Config.Rotation);
			this._vrPointer = base.GetComponent<VRPointer>();
		}

		// Token: 0x06000002 RID: 2 RVA: 0x000020A8 File Offset: 0x000002A8
		protected virtual void OnEnable()
		{
			try
			{
				this._cameraPlus.Config.ConfigChangedEvent += this.PluginOnConfigChangedEvent;
			}
			catch
			{
			}
		}

		// Token: 0x06000003 RID: 3 RVA: 0x000020E8 File Offset: 0x000002E8
		protected virtual void OnDisable()
		{
			this._cameraPlus.Config.ConfigChangedEvent -= this.PluginOnConfigChangedEvent;
		}

		// Token: 0x06000004 RID: 4 RVA: 0x00002107 File Offset: 0x00000307
		protected virtual void PluginOnConfigChangedEvent(Config config)
		{
			this._realPos = config.Position;
			this._realRot = Quaternion.Euler(config.Rotation);
		}

		// Token: 0x06000005 RID: 5 RVA: 0x00002128 File Offset: 0x00000328
		protected virtual void Update()
		{
			if (this._vrPointer.vrController != null && this._vrPointer.vrController.triggerValue > 0.9f)
			{
				if (this._grabbingController != null)
				{
					return;
				}
				RaycastHit raycastHit;
				if (Physics.Raycast(this._vrPointer.vrController.position, this._vrPointer.vrController.forward, ref raycastHit, 50f))
				{
					if (raycastHit.transform != this._cameraCube)
					{
						return;
					}
					this._grabbingController = this._vrPointer.vrController;
					this._grabPos = this._vrPointer.vrController.transform.InverseTransformPoint(this._cameraCube.position);
					this._grabRot = Quaternion.Inverse(this._vrPointer.vrController.transform.rotation) * this._cameraCube.rotation;
				}
			}
			if (this._grabbingController == null || this._grabbingController.triggerValue > 0.9f)
			{
				return;
			}
			this.SaveToConfig();
			this._grabbingController = null;
		}

		// Token: 0x06000006 RID: 6 RVA: 0x0000224C File Offset: 0x0000044C
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
				this._cameraPlus.ThirdPersonPos = Vector3.Lerp(this._cameraCube.position, this._realPos, this._cameraPlus.Config.positionSmooth * Time.unscaledDeltaTime);
				this._cameraPlus.ThirdPersonRot = Quaternion.Slerp(this._cameraCube.rotation, this._realRot, this._cameraPlus.Config.rotationSmooth * Time.unscaledDeltaTime).eulerAngles;
				return;
			}
		}

		// Token: 0x06000007 RID: 7 RVA: 0x00002390 File Offset: 0x00000590
		protected virtual void SaveToConfig()
		{
			Vector3 realPos = this._realPos;
			Vector3 eulerAngles = this._realRot.eulerAngles;
			Config config = this._cameraPlus.Config;
			config.posx = realPos.x;
			config.posy = realPos.y;
			config.posz = realPos.z;
			config.angx = eulerAngles.x;
			config.angy = eulerAngles.y;
			config.angz = eulerAngles.z;
			config.Save();
		}

		// Token: 0x04000001 RID: 1
		protected const float MinScrollDistance = 0.25f;

		// Token: 0x04000002 RID: 2
		protected const float MaxLaserDistance = 50f;

		// Token: 0x04000003 RID: 3
		protected VRPointer _vrPointer;

		// Token: 0x04000004 RID: 4
		protected CameraPlusBehaviour _cameraPlus;

		// Token: 0x04000005 RID: 5
		protected Transform _cameraCube;

		// Token: 0x04000006 RID: 6
		protected VRController _grabbingController;

		// Token: 0x04000007 RID: 7
		protected Vector3 _grabPos;

		// Token: 0x04000008 RID: 8
		protected Quaternion _grabRot;

		// Token: 0x04000009 RID: 9
		protected Vector3 _realPos;

		// Token: 0x0400000A RID: 10
		protected Quaternion _realRot;
	}
}
