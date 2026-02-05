using System;
using UnityEngine;
using Valve.VR;

namespace DynamicOpenVR.IO
{
	// Token: 0x020000DB RID: 219
	public class PoseInput : OVRInput
	{
		// Token: 0x1700002F RID: 47
		// (get) Token: 0x060001FB RID: 507 RVA: 0x00006220 File Offset: 0x00004420
		public bool active
		{
			get
			{
				return this._actionData.bActive;
			}
		}

		// Token: 0x17000030 RID: 48
		// (get) Token: 0x060001FC RID: 508 RVA: 0x0000622D File Offset: 0x0000442D
		public bool deviceConnected
		{
			get
			{
				return this._actionData.pose.bDeviceIsConnected;
			}
		}

		// Token: 0x17000031 RID: 49
		// (get) Token: 0x060001FD RID: 509 RVA: 0x0000623F File Offset: 0x0000443F
		public bool isPoseValid
		{
			get
			{
				return this._actionData.pose.bPoseIsValid;
			}
		}

		// Token: 0x17000032 RID: 50
		// (get) Token: 0x060001FE RID: 510 RVA: 0x00006251 File Offset: 0x00004451
		public Pose pose
		{
			get
			{
				return this._pose;
			}
		}

		// Token: 0x17000033 RID: 51
		// (get) Token: 0x060001FF RID: 511 RVA: 0x00006259 File Offset: 0x00004459
		public Vector3 position
		{
			get
			{
				return this._pose.position;
			}
		}

		// Token: 0x17000034 RID: 52
		// (get) Token: 0x06000200 RID: 512 RVA: 0x00006266 File Offset: 0x00004466
		public Quaternion rotation
		{
			get
			{
				return this._pose.rotation;
			}
		}

		// Token: 0x17000035 RID: 53
		// (get) Token: 0x06000201 RID: 513 RVA: 0x00006273 File Offset: 0x00004473
		public Vector3 velocity
		{
			get
			{
				return this.ToVector3(this._actionData.pose.vVelocity);
			}
		}

		// Token: 0x17000036 RID: 54
		// (get) Token: 0x06000202 RID: 514 RVA: 0x0000628B File Offset: 0x0000448B
		public Vector3 angularVelocity
		{
			get
			{
				return this.ToVector3(this._actionData.pose.vAngularVelocity);
			}
		}

		// Token: 0x17000037 RID: 55
		// (get) Token: 0x06000203 RID: 515 RVA: 0x000062A3 File Offset: 0x000044A3
		public bool isTracking
		{
			get
			{
				return this._actionData.pose.eTrackingResult == ETrackingResult.Running_OK;
			}
		}

		// Token: 0x06000204 RID: 516 RVA: 0x000062BC File Offset: 0x000044BC
		public PoseInput(string name)
			: base(name)
		{
		}

		// Token: 0x17000038 RID: 56
		// (get) Token: 0x06000205 RID: 517 RVA: 0x000062C5 File Offset: 0x000044C5
		public override bool isActive
		{
			get
			{
				return this._actionData.bActive;
			}
		}

		// Token: 0x06000206 RID: 518 RVA: 0x000062D4 File Offset: 0x000044D4
		internal override void UpdateData()
		{
			this._actionData = OpenVRWrapper.GetPoseActionData(base.handle, ETrackingUniverseOrigin.TrackingUniverseStanding);
			HmdMatrix34_t mDeviceToAbsoluteTracking = this._actionData.pose.mDeviceToAbsoluteTracking;
			this._pose = new Pose(this.GetPosition(mDeviceToAbsoluteTracking), this.GetRotation(mDeviceToAbsoluteTracking));
		}

		// Token: 0x06000207 RID: 519 RVA: 0x0000631D File Offset: 0x0000451D
		private Vector3 GetPosition(HmdMatrix34_t rawMatrix)
		{
			return new Vector3(rawMatrix.m3, rawMatrix.m7, -rawMatrix.m11);
		}

		// Token: 0x06000208 RID: 520 RVA: 0x00006338 File Offset: 0x00004538
		private Quaternion GetRotation(HmdMatrix34_t rawMatrix)
		{
			if (this.IsRotationValid(rawMatrix))
			{
				float num = Mathf.Sqrt(Mathf.Max(0f, 1f + rawMatrix.m0 + rawMatrix.m5 + rawMatrix.m10)) / 2f;
				float num2 = Mathf.Sqrt(Mathf.Max(0f, 1f + rawMatrix.m0 - rawMatrix.m5 - rawMatrix.m10)) / 2f;
				float num3 = Mathf.Sqrt(Mathf.Max(0f, 1f - rawMatrix.m0 + rawMatrix.m5 - rawMatrix.m10)) / 2f;
				float num4 = Mathf.Sqrt(Mathf.Max(0f, 1f - rawMatrix.m0 - rawMatrix.m5 + rawMatrix.m10)) / 2f;
				PoseInput.CopySign(ref num2, rawMatrix.m6 - rawMatrix.m9);
				PoseInput.CopySign(ref num3, rawMatrix.m8 - rawMatrix.m2);
				PoseInput.CopySign(ref num4, rawMatrix.m4 - rawMatrix.m1);
				return new Quaternion(num2, num3, num4, num);
			}
			return Quaternion.identity;
		}

		// Token: 0x06000209 RID: 521 RVA: 0x0000645C File Offset: 0x0000465C
		private static void CopySign(ref float sizeVal, float signVal)
		{
			if (signVal > 0f != sizeVal > 0f)
			{
				sizeVal = -sizeVal;
			}
		}

		// Token: 0x0600020A RID: 522 RVA: 0x00006476 File Offset: 0x00004676
		private Vector3 ToVector3(HmdVector3_t vector)
		{
			return new Vector3(vector.v0, vector.v1, vector.v2);
		}

		// Token: 0x0600020B RID: 523 RVA: 0x00006490 File Offset: 0x00004690
		private bool IsRotationValid(HmdMatrix34_t rawMatrix)
		{
			return (rawMatrix.m2 != 0f || rawMatrix.m6 != 0f || rawMatrix.m10 != 0f) && (rawMatrix.m1 != 0f || rawMatrix.m5 != 0f || rawMatrix.m9 != 0f);
		}

		// Token: 0x04000884 RID: 2180
		private InputPoseActionData_t _actionData;

		// Token: 0x04000885 RID: 2181
		private Pose _pose;
	}
}
