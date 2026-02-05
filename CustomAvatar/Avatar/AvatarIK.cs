using System;
using System.Collections.Generic;
using CustomAvatar.Tracking;
using CustomAvatar.Utilities;
using RootMotion.FinalIK;
using UnityEngine;

namespace CustomAvatar.Avatar
{
	// Token: 0x02000035 RID: 53
	internal class AvatarIK : BodyAwareBehaviour
	{
		// Token: 0x06000115 RID: 277 RVA: 0x00007774 File Offset: 0x00005974
		private void Awake()
		{
			this._preUpdateDelegate = typeof(DynamicBone).CreatePrivateMethodDelegate("PreUpdate");
			this._updateDynamicBonesDelegate = typeof(DynamicBone).CreatePrivateMethodDelegate("UpdateDynamicBones");
			foreach (TwistRelaxer twistRelaxer in base.GetComponentsInChildren<TwistRelaxer>())
			{
				bool enabled = twistRelaxer.enabled;
				if (enabled)
				{
					twistRelaxer.enabled = false;
					this._twistRelaxers.Add(twistRelaxer);
				}
			}
		}

		// Token: 0x06000116 RID: 278 RVA: 0x000077F4 File Offset: 0x000059F4
		protected override void Start()
		{
			base.Start();
			this._vrikManager = base.GetComponentInChildren<VRIKManager>();
			this._dynamicBones = base.GetComponentsInChildren<DynamicBone>();
			this._vrik = this._vrikManager.vrik;
			this._vrik.fixTransforms = false;
			this._fixTransforms = this._vrikManager.fixTransforms;
			this._vrikManager.referencesUpdated += this.OnReferencesUpdated;
			this.OnReferencesUpdated();
			foreach (TwistRelaxer twistRelaxer in this._twistRelaxers)
			{
				twistRelaxer.ik = this._vrik;
				twistRelaxer.enabled = true;
			}
			this.input.inputChanged += this.OnInputChanged;
		}

		// Token: 0x06000117 RID: 279 RVA: 0x000078E0 File Offset: 0x00005AE0
		private void Update()
		{
			bool flag = this._vrik && this._fixTransforms;
			if (flag)
			{
				this._vrik.solver.FixTransforms();
			}
			foreach (DynamicBone dynamicBone in this._dynamicBones)
			{
				bool flag2 = !dynamicBone.enabled;
				if (!flag2)
				{
					dynamicBone.SetPrivateField("m_Weight", 1);
					this._preUpdateDelegate(dynamicBone);
					dynamicBone.SetPrivateField("m_Weight", 0);
				}
			}
		}

		// Token: 0x06000118 RID: 280 RVA: 0x0000797C File Offset: 0x00005B7C
		private void LateUpdate()
		{
			bool flag = this._vrik;
			if (flag)
			{
				this._vrik.UpdateSolverExternal();
			}
			foreach (DynamicBone dynamicBone in this._dynamicBones)
			{
				bool flag2 = !dynamicBone.enabled;
				if (!flag2)
				{
					dynamicBone.SetPrivateField("m_Weight", 1);
					this._updateDynamicBonesDelegate(dynamicBone, Time.deltaTime);
					dynamicBone.SetPrivateField("m_Weight", 0);
				}
			}
		}

		// Token: 0x06000119 RID: 281 RVA: 0x00007A0A File Offset: 0x00005C0A
		private void OnDestroy()
		{
			this.input.inputChanged -= this.OnInputChanged;
		}

		// Token: 0x0600011A RID: 282 RVA: 0x00007A25 File Offset: 0x00005C25
		private void OnInputChanged()
		{
			Plugin.logger.Info("Tracking device change detected, updating VRIK references");
			this.UpdateSolverTargets();
		}

		// Token: 0x0600011B RID: 283 RVA: 0x00007A3F File Offset: 0x00005C3F
		private void OnReferencesUpdated()
		{
			this.CreateTargetsIfMissing();
			this.UpdateSolverTargets();
		}

		// Token: 0x0600011C RID: 284 RVA: 0x00007A50 File Offset: 0x00005C50
		private void CreateTargetsIfMissing()
		{
			this._vrikManager.solver_spine_headTarget = this.CreateTargetIfMissing(this._vrikManager.solver_spine_headTarget, this._vrik.references.head, this._head);
			this._vrikManager.solver_leftArm_target = this.CreateTargetIfMissing(this._vrikManager.solver_leftArm_target, this._vrik.references.leftHand, this._leftHand);
			this._vrikManager.solver_rightArm_target = this.CreateTargetIfMissing(this._vrikManager.solver_rightArm_target, this._vrik.references.rightHand, this._rightHand);
			this._vrikManager.solver_spine_pelvisTarget = this.CreateTargetIfMissing(this._vrikManager.solver_spine_pelvisTarget, this._vrik.references.pelvis, this._pelvis);
			this._vrikManager.solver_leftLeg_target = this.CreateTargetIfMissing(this._vrikManager.solver_leftLeg_target, this._vrik.references.leftToes ?? this._vrik.references.leftFoot, this._leftLeg);
			this._vrikManager.solver_rightLeg_target = this.CreateTargetIfMissing(this._vrikManager.solver_rightLeg_target, this._vrik.references.rightToes ?? this._vrik.references.rightFoot, this._rightLeg);
		}

		// Token: 0x0600011D RID: 285 RVA: 0x00007BB4 File Offset: 0x00005DB4
		private Transform CreateTargetIfMissing(Transform target, Transform reference, Transform parent)
		{
			bool flag = target || !parent;
			Transform transform;
			if (flag)
			{
				transform = target;
			}
			else
			{
				Transform transform2 = new GameObject().transform;
				transform2.SetParent(parent, false);
				transform2.position = reference.position;
				transform2.rotation = reference.rotation;
				Plugin.logger.Info("Created IK target for '" + parent.name + "'");
				transform = transform2;
			}
			return transform;
		}

		// Token: 0x0600011E RID: 286 RVA: 0x00007C34 File Offset: 0x00005E34
		private void UpdateSolverTargets()
		{
			Plugin.logger.Info("Updating solver targets");
			this._vrik.solver.spine.headTarget = this._vrikManager.solver_spine_headTarget;
			this._vrik.solver.leftArm.target = this._vrikManager.solver_leftArm_target;
			this._vrik.solver.rightArm.target = this._vrikManager.solver_rightArm_target;
			Plugin.logger.Info("Updating conditional solver targets");
			Pose pose;
			bool flag = this.input.TryGetLeftFootPose(out pose);
			if (flag)
			{
				Plugin.logger.Debug("Left foot enabled");
				this._vrik.solver.leftLeg.target = this._vrikManager.solver_leftLeg_target;
				this._vrik.solver.leftLeg.positionWeight = this._vrikManager.solver_leftLeg_positionWeight;
				this._vrik.solver.leftLeg.rotationWeight = this._vrikManager.solver_leftLeg_rotationWeight;
			}
			else
			{
				Plugin.logger.Debug("Left foot disabled");
				this._vrik.solver.leftLeg.target = null;
				this._vrik.solver.leftLeg.positionWeight = 0f;
				this._vrik.solver.leftLeg.rotationWeight = 0f;
			}
			bool flag2 = this.input.TryGetRightFootPose(out pose);
			if (flag2)
			{
				Plugin.logger.Debug("Right foot enabled");
				this._vrik.solver.rightLeg.target = this._vrikManager.solver_rightLeg_target;
				this._vrik.solver.rightLeg.positionWeight = this._vrikManager.solver_rightLeg_positionWeight;
				this._vrik.solver.rightLeg.rotationWeight = this._vrikManager.solver_rightLeg_rotationWeight;
			}
			else
			{
				Plugin.logger.Debug("Right foot disabled");
				this._vrik.solver.rightLeg.target = null;
				this._vrik.solver.rightLeg.positionWeight = 0f;
				this._vrik.solver.rightLeg.rotationWeight = 0f;
			}
			bool flag3 = this.input.TryGetWaistPose(out pose);
			if (flag3)
			{
				Plugin.logger.Debug("Pelvis enabled");
				this._vrik.solver.spine.pelvisTarget = this._vrikManager.solver_spine_pelvisTarget;
				this._vrik.solver.spine.pelvisPositionWeight = this._vrikManager.solver_spine_pelvisPositionWeight;
				this._vrik.solver.spine.pelvisRotationWeight = this._vrikManager.solver_spine_pelvisRotationWeight;
				this._vrik.solver.plantFeet = false;
			}
			else
			{
				Plugin.logger.Debug("Pelvis disabled");
				this._vrik.solver.spine.pelvisTarget = null;
				this._vrik.solver.spine.pelvisPositionWeight = 0f;
				this._vrik.solver.spine.pelvisRotationWeight = 0f;
				this._vrik.solver.plantFeet = this._vrikManager.solver_plantFeet;
			}
		}

		// Token: 0x0400018E RID: 398
		public AvatarInput input;

		// Token: 0x0400018F RID: 399
		private VRIK _vrik;

		// Token: 0x04000190 RID: 400
		private VRIKManager _vrikManager;

		// Token: 0x04000191 RID: 401
		private bool _fixTransforms;

		// Token: 0x04000192 RID: 402
		private List<TwistRelaxer> _twistRelaxers = new List<TwistRelaxer>();

		// Token: 0x04000193 RID: 403
		private DynamicBone[] _dynamicBones;

		// Token: 0x04000194 RID: 404
		private Action<DynamicBone> _preUpdateDelegate;

		// Token: 0x04000195 RID: 405
		private Action<DynamicBone, float> _updateDynamicBonesDelegate;
	}
}
