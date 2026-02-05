using System;
using System.Diagnostics;
using System.Reflection;
using RootMotion;
using RootMotion.FinalIK;
using UnityEngine;
using UnityEngine.Events;

namespace CustomAvatar
{
	// Token: 0x02000019 RID: 25
	[Serializable]
	public class VRIKManager : MonoBehaviour
	{
		// Token: 0x06000045 RID: 69 RVA: 0x00003590 File Offset: 0x00001790
		[ContextMenu("Auto-detect References")]
		public void AutoDetectReferences()
		{
			Animator componentInChildren = base.transform.GetComponentInChildren<Animator>();
			bool flag = componentInChildren == null || !componentInChildren.isHuman;
			if (!flag)
			{
				this.references_root = base.transform;
				this.references_pelvis = componentInChildren.GetBoneTransform(0);
				this.references_spine = componentInChildren.GetBoneTransform(7);
				this.references_chest = componentInChildren.GetBoneTransform(8);
				this.references_neck = componentInChildren.GetBoneTransform(9);
				this.references_head = componentInChildren.GetBoneTransform(10);
				this.references_leftShoulder = componentInChildren.GetBoneTransform(11);
				this.references_leftUpperArm = componentInChildren.GetBoneTransform(13);
				this.references_leftForearm = componentInChildren.GetBoneTransform(15);
				this.references_leftHand = componentInChildren.GetBoneTransform(17);
				this.references_rightShoulder = componentInChildren.GetBoneTransform(12);
				this.references_rightUpperArm = componentInChildren.GetBoneTransform(14);
				this.references_rightForearm = componentInChildren.GetBoneTransform(16);
				this.references_rightHand = componentInChildren.GetBoneTransform(18);
				this.references_leftThigh = componentInChildren.GetBoneTransform(1);
				this.references_leftCalf = componentInChildren.GetBoneTransform(3);
				this.references_leftFoot = componentInChildren.GetBoneTransform(5);
				this.references_leftToes = componentInChildren.GetBoneTransform(19);
				this.references_rightThigh = componentInChildren.GetBoneTransform(2);
				this.references_rightCalf = componentInChildren.GetBoneTransform(4);
				this.references_rightFoot = componentInChildren.GetBoneTransform(6);
				this.references_rightToes = componentInChildren.GetBoneTransform(20);
			}
		}

		// Token: 0x14000002 RID: 2
		// (add) Token: 0x06000046 RID: 70 RVA: 0x000036F4 File Offset: 0x000018F4
		// (remove) Token: 0x06000047 RID: 71 RVA: 0x0000372C File Offset: 0x0000192C
		[field: DebuggerBrowsable(DebuggerBrowsableState.Never)]
		internal event Action referencesUpdated;

		// Token: 0x06000048 RID: 72 RVA: 0x00003761 File Offset: 0x00001961
		private void Reset()
		{
			this.AutoDetectReferences();
		}

		// Token: 0x06000049 RID: 73 RVA: 0x0000376C File Offset: 0x0000196C
		private void Awake()
		{
			foreach (VRIK vrik in base.GetComponentsInChildren<VRIK>())
			{
				Object.Destroy(vrik);
			}
			this.vrik = base.gameObject.AddComponent<VRIK>();
		}

		// Token: 0x0600004A RID: 74 RVA: 0x000037AE File Offset: 0x000019AE
		private void Start()
		{
			this.SetVrikReferences();
		}

		// Token: 0x0600004B RID: 75 RVA: 0x000037B8 File Offset: 0x000019B8
		internal void SetVrikReferences()
		{
			foreach (FieldInfo fieldInfo in base.GetType().GetFields())
			{
				string[] array = fieldInfo.Name.Split(new char[] { '_' });
				object obj = this.vrik;
				try
				{
					for (int j = 0; j < array.Length - 1; j++)
					{
						FieldInfo field = obj.GetType().GetField(array[j]);
						obj = ((field != null) ? field.GetValue(obj) : null);
						bool flag = obj == null;
						if (flag)
						{
							break;
						}
					}
					bool flag2 = obj == null;
					if (flag2)
					{
						break;
					}
					FieldInfo field2 = obj.GetType().GetField(array[array.Length - 1]);
					object value = fieldInfo.GetValue(this);
					bool flag3 = field2.FieldType.IsEnum && fieldInfo.FieldType != field2.FieldType;
					if (flag3)
					{
						Type underlyingType = Enum.GetUnderlyingType(fieldInfo.FieldType);
						Type underlyingType2 = Enum.GetUnderlyingType(field2.FieldType);
						bool flag4 = underlyingType != underlyingType2;
						if (flag4)
						{
						}
						field2.SetValue(obj, Convert.ChangeType(value, underlyingType2));
					}
					else
					{
						bool flag5 = fieldInfo.FieldType != field2.FieldType;
						if (flag5)
						{
						}
						field2.SetValue(obj, value);
					}
				}
				catch (Exception ex)
				{
				}
			}
			bool flag6 = !this.vrik.references.isFilled;
			if (flag6)
			{
				this.vrik.AutoDetectReferences();
			}
			Action action = this.referencesUpdated;
			if (action != null)
			{
				action();
			}
		}

		// Token: 0x040000AC RID: 172
		[Tooltip("If true, will fix all the Transforms used by the solver to their initial state in each Update. This prevents potential problems with unanimated bones and animator culling with a small cost of performance. Not recommended for CCD and FABRIK solvers.")]
		public bool fixTransforms = true;

		// Token: 0x040000AD RID: 173
		[Header("References")]
		public Transform references_root;

		// Token: 0x040000AE RID: 174
		public Transform references_pelvis;

		// Token: 0x040000AF RID: 175
		public Transform references_spine;

		// Token: 0x040000B0 RID: 176
		[Tooltip("Optional")]
		public Transform references_chest;

		// Token: 0x040000B1 RID: 177
		[Tooltip("Optional")]
		public Transform references_neck;

		// Token: 0x040000B2 RID: 178
		public Transform references_head;

		// Token: 0x040000B3 RID: 179
		[Tooltip("Optional")]
		public Transform references_leftShoulder;

		// Token: 0x040000B4 RID: 180
		public Transform references_leftUpperArm;

		// Token: 0x040000B5 RID: 181
		public Transform references_leftForearm;

		// Token: 0x040000B6 RID: 182
		public Transform references_leftHand;

		// Token: 0x040000B7 RID: 183
		[Tooltip("Optional")]
		public Transform references_rightShoulder;

		// Token: 0x040000B8 RID: 184
		public Transform references_rightUpperArm;

		// Token: 0x040000B9 RID: 185
		public Transform references_rightForearm;

		// Token: 0x040000BA RID: 186
		public Transform references_rightHand;

		// Token: 0x040000BB RID: 187
		[Tooltip("VRIK also supports legless characters. If you do not wish to use legs, leave all leg references empty.")]
		public Transform references_leftThigh;

		// Token: 0x040000BC RID: 188
		[Tooltip("VRIK also supports legless characters. If you do not wish to use legs, leave all leg references empty.")]
		public Transform references_leftCalf;

		// Token: 0x040000BD RID: 189
		[Tooltip("VRIK also supports legless characters. If you do not wish to use legs, leave all leg references empty.")]
		public Transform references_leftFoot;

		// Token: 0x040000BE RID: 190
		[Tooltip("Optional")]
		public Transform references_leftToes;

		// Token: 0x040000BF RID: 191
		[Tooltip("VRIK also supports legless characters. If you do not wish to use legs, leave all leg references empty.")]
		public Transform references_rightThigh;

		// Token: 0x040000C0 RID: 192
		[Tooltip("VRIK also supports legless characters. If you do not wish to use legs, leave all leg references empty.")]
		public Transform references_rightCalf;

		// Token: 0x040000C1 RID: 193
		[Tooltip("VRIK also supports legless characters. If you do not wish to use legs, leave all leg references empty.")]
		public Transform references_rightFoot;

		// Token: 0x040000C2 RID: 194
		[Tooltip("Optional")]
		public Transform references_rightToes;

		// Token: 0x040000C3 RID: 195
		[Header("Solver")]
		[Tooltip("If true, will keep the toes planted even if head target is out of reach.")]
		public bool solver_plantFeet = true;

		// Token: 0x040000C4 RID: 196
		[Header("Spine")]
		[Tooltip("The head target.")]
		public Transform solver_spine_headTarget;

		// Token: 0x040000C5 RID: 197
		[Tooltip("Positional weight of the head target.")]
		[Range(0f, 1f)]
		public float solver_spine_positionWeight = 1f;

		// Token: 0x040000C6 RID: 198
		[Tooltip("Rotational weight of the head target.")]
		[Range(0f, 1f)]
		public float solver_spine_rotationWeight = 1f;

		// Token: 0x040000C7 RID: 199
		[Tooltip("The pelvis target, useful with seated rigs.")]
		public Transform solver_spine_pelvisTarget;

		// Token: 0x040000C8 RID: 200
		[Tooltip("Positional weight of the pelvis target.")]
		[Range(0f, 1f)]
		public float solver_spine_pelvisPositionWeight;

		// Token: 0x040000C9 RID: 201
		[Tooltip("Rotational weight of the pelvis target.")]
		[Range(0f, 1f)]
		public float solver_spine_pelvisRotationWeight;

		// Token: 0x040000CA RID: 202
		[Tooltip("If 'Chest Goal Weight' is greater than 0, the chest will be turned towards this Transform.")]
		public Transform solver_spine_chestGoal;

		// Token: 0x040000CB RID: 203
		[Tooltip("Rotational weight of the chest target.")]
		[Range(0f, 1f)]
		public float solver_spine_chestGoalWeight;

		// Token: 0x040000CC RID: 204
		[Tooltip("Minimum height of the head from the root of the character.")]
		public float solver_spine_minHeadHeight = 0.8f;

		// Token: 0x040000CD RID: 205
		[Tooltip("Determines how much the body will follow the position of the head.")]
		[Range(0f, 1f)]
		public float solver_spine_bodyPosStiffness = 0.55f;

		// Token: 0x040000CE RID: 206
		[Tooltip("Determines how much the body will follow the rotation of the head.")]
		[Range(0f, 1f)]
		public float solver_spine_bodyRotStiffness = 0.1f;

		// Token: 0x040000CF RID: 207
		[Tooltip("Determines how much the chest will rotate to the rotation of the head.")]
		[Range(0f, 1f)]
		public float solver_spine_neckStiffness = 0.2f;

		// Token: 0x040000D0 RID: 208
		[Tooltip("The amount of rotation applied to the chest based on hand positions.")]
		[Range(0f, 1f)]
		public float solver_spine_rotateChestByHands = 1f;

		// Token: 0x040000D1 RID: 209
		[Tooltip("Clamps chest rotation.")]
		[Range(0f, 1f)]
		public float solver_spine_chestClampWeight = 0.5f;

		// Token: 0x040000D2 RID: 210
		[Tooltip("Clamps head rotation.")]
		[Range(0f, 1f)]
		public float solver_spine_headClampWeight = 0.6f;

		// Token: 0x040000D3 RID: 211
		[Tooltip("Moves the body horizontally along -character.forward axis by that value when the player is crouching.")]
		public float solver_spine_moveBodyBackWhenCrouching = 0.5f;

		// Token: 0x040000D4 RID: 212
		[Tooltip("How much will the pelvis maintain it's animated position?")]
		[Range(0f, 1f)]
		public float solver_spine_maintainPelvisPosition = 0.2f;

		// Token: 0x040000D5 RID: 213
		[Tooltip("Will automatically rotate the root of the character if the head target has turned past this angle.")]
		[Range(0f, 180f)]
		public float solver_spine_maxRootAngle = 25f;

		// Token: 0x040000D6 RID: 214
		[Header("Left Arm")]
		[Tooltip("The hand target")]
		public Transform solver_leftArm_target;

		// Token: 0x040000D7 RID: 215
		[Tooltip("The elbow will be bent towards this Transform if 'Bend Goal Weight' > 0.")]
		public Transform solver_leftArm_bendGoal;

		// Token: 0x040000D8 RID: 216
		[Tooltip("Positional weight of the hand target.")]
		[Range(0f, 1f)]
		public float solver_leftArm_positionWeight = 1f;

		// Token: 0x040000D9 RID: 217
		[Tooltip("Rotational weight of the hand target")]
		[Range(0f, 1f)]
		public float solver_leftArm_rotationWeight = 1f;

		// Token: 0x040000DA RID: 218
		[Tooltip("Different techniques for shoulder bone rotation.")]
		public IKSolverVR.Arm.ShoulderRotationMode solver_leftArm_shoulderRotationMode = 0;

		// Token: 0x040000DB RID: 219
		[Tooltip("The weight of shoulder rotation")]
		[Range(0f, 1f)]
		public float solver_leftArm_shoulderRotationWeight = 1f;

		// Token: 0x040000DC RID: 220
		[Tooltip("The weight of twisting the shoulders back when arms are lifted up.")]
		[Range(0f, 1f)]
		public float solver_leftArm_shoulderTwistWeight = 1f;

		// Token: 0x040000DD RID: 221
		[Tooltip("If greater than 0, will bend the elbow towards the 'Bend Goal' Transform.")]
		[Range(0f, 1f)]
		public float solver_leftArm_bendGoalWeight;

		// Token: 0x040000DE RID: 222
		[Tooltip("Angular offset of the elbow bending direction.")]
		[Range(-180f, 180f)]
		public float solver_leftArm_swivelOffset;

		// Token: 0x040000DF RID: 223
		[Tooltip("Local axis of the hand bone that points from the wrist towards the palm. Used for defining hand bone orientation.")]
		public Vector3 solver_leftArm_wristToPalmAxis = Vector3.zero;

		// Token: 0x040000E0 RID: 224
		[Tooltip("Local axis of the hand bone that points from the palm towards the thumb. Used for defining hand bone orientation.")]
		public Vector3 solver_leftArm_palmToThumbAxis = Vector3.zero;

		// Token: 0x040000E1 RID: 225
		[Tooltip("Use this to make the arm shorter/longer.")]
		[Range(0.01f, 2f)]
		public float solver_leftArm_armLengthMlp = 1f;

		// Token: 0x040000E2 RID: 226
		[Tooltip("Evaluates stretching of the arm by target distance relative to arm length. Value at time 1 represents stretching amount at the point where distance to the target is equal to arm length. Value at time 2 represents stretching amount at the point where distance to the target is double the arm length. Value represents the amount of stretching. Linear stretching would be achieved with a linear curve going up by 45 degrees. Increase the range of stretching by moving the last key up and right at the same amount. Smoothing in the curve can help reduce elbow snapping (start stretching the arm slightly before target distance reaches arm length).")]
		public AnimationCurve solver_leftArm_stretchCurve = new AnimationCurve();

		// Token: 0x040000E3 RID: 227
		[Header("Right Arm")]
		[Tooltip("The hand target")]
		public Transform solver_rightArm_target;

		// Token: 0x040000E4 RID: 228
		[Tooltip("The elbow will be bent towards this Transform if 'Bend Goal Weight' > 0.")]
		public Transform solver_rightArm_bendGoal;

		// Token: 0x040000E5 RID: 229
		[Tooltip("Positional weight of the hand target.")]
		[Range(0f, 1f)]
		public float solver_rightArm_positionWeight = 1f;

		// Token: 0x040000E6 RID: 230
		[Tooltip("Rotational weight of the hand target")]
		[Range(0f, 1f)]
		public float solver_rightArm_rotationWeight = 1f;

		// Token: 0x040000E7 RID: 231
		[Tooltip("Different techniques for shoulder bone rotation.")]
		public IKSolverVR.Arm.ShoulderRotationMode solver_rightArm_shoulderRotationMode = 0;

		// Token: 0x040000E8 RID: 232
		[Tooltip("The weight of shoulder rotation")]
		[Range(0f, 1f)]
		public float solver_rightArm_shoulderRotationWeight = 1f;

		// Token: 0x040000E9 RID: 233
		[Tooltip("The weight of twisting the shoulders back when arms are lifted up.")]
		[Range(0f, 1f)]
		public float solver_rightArm_shoulderTwistWeight = 1f;

		// Token: 0x040000EA RID: 234
		[Tooltip("If greater than 0, will bend the elbow towards the 'Bend Goal' Transform.")]
		[Range(0f, 1f)]
		public float solver_rightArm_bendGoalWeight;

		// Token: 0x040000EB RID: 235
		[Tooltip("Angular offset of the elbow bending direction.")]
		[Range(-180f, 180f)]
		public float solver_rightArm_swivelOffset;

		// Token: 0x040000EC RID: 236
		[Tooltip("Local axis of the hand bone that points from the wrist towards the palm. Used for defining hand bone orientation.")]
		public Vector3 solver_rightArm_wristToPalmAxis = Vector3.zero;

		// Token: 0x040000ED RID: 237
		[Tooltip("Local axis of the hand bone that points from the palm towards the thumb. Used for defining hand bone orientation.")]
		public Vector3 solver_rightArm_palmToThumbAxis = Vector3.zero;

		// Token: 0x040000EE RID: 238
		[Tooltip("Use this to make the arm shorter/longer.")]
		[Range(0.01f, 2f)]
		public float solver_rightArm_armLengthMlp = 1f;

		// Token: 0x040000EF RID: 239
		[Tooltip("Evaluates stretching of the arm by target distance relative to arm length. Value at time 1 represents stretching amount at the point where distance to the target is equal to arm length. Value at time 2 represents stretching amount at the point where distance to the target is double the arm length. Value represents the amount of stretching. Linear stretching would be achieved with a linear curve going up by 45 degrees. Increase the range of stretching by moving the last key up and right at the same amount. Smoothing in the curve can help reduce elbow snapping (start stretching the arm slightly before target distance reaches arm length).")]
		public AnimationCurve solver_rightArm_stretchCurve = new AnimationCurve();

		// Token: 0x040000F0 RID: 240
		[Header("Left Leg")]
		[Tooltip("The toe/foot target.")]
		public Transform solver_leftLeg_target;

		// Token: 0x040000F1 RID: 241
		[Tooltip("The knee will be bent towards this Transform if 'Bend Goal Weight' > 0.")]
		public Transform solver_leftLeg_bendGoal;

		// Token: 0x040000F2 RID: 242
		[Tooltip("Positional weight of the toe/foot target.")]
		[Range(0f, 1f)]
		public float solver_leftLeg_positionWeight;

		// Token: 0x040000F3 RID: 243
		[Tooltip("Rotational weight of the toe/foot target.")]
		[Range(0f, 1f)]
		public float solver_leftLeg_rotationWeight;

		// Token: 0x040000F4 RID: 244
		[Tooltip("If greater than 0, will bend the knee towards the 'Bend Goal' Transform.")]
		[Range(0f, 1f)]
		public float solver_leftLeg_bendGoalWeight;

		// Token: 0x040000F5 RID: 245
		[Tooltip("Angular offset of the knee bending direction.")]
		[Range(-180f, 180f)]
		public float solver_leftLeg_swivelOffset;

		// Token: 0x040000F6 RID: 246
		[Tooltip("If 0, the bend plane will be locked to the rotation of the pelvis and rotating the foot will have no effect on the knee direction. If 1, to the target rotation of the leg so that the knee will bend towards the forward axis of the foot. Values in between will be slerped between the two.")]
		[Range(0f, 1f)]
		public float solver_leftLeg_bendToTargetWeight = 0.5f;

		// Token: 0x040000F7 RID: 247
		[Tooltip("Use this to make the leg shorter/longer.")]
		[Range(0.01f, 2f)]
		public float solver_leftLeg_legLengthMlp = 1f;

		// Token: 0x040000F8 RID: 248
		[Tooltip("Evaluates stretching of the leg by target distance relative to leg length. Value at time 1 represents stretching amount at the point where distance to the target is equal to leg length. Value at time 1 represents stretching amount at the point where distance to the target is double the leg length. Value represents the amount of stretching. Linear stretching would be achieved with a linear curve going up by 45 degrees. Increase the range of stretching by moving the last key up and right at the same amount. Smoothing in the curve can help reduce knee snapping (start stretching the arm slightly before target distance reaches leg length).")]
		public AnimationCurve solver_leftLeg_stretchCurve = new AnimationCurve();

		// Token: 0x040000F9 RID: 249
		[Header("Right Leg")]
		[Tooltip("The toe/foot target.")]
		public Transform solver_rightLeg_target;

		// Token: 0x040000FA RID: 250
		[Tooltip("The knee will be bent towards this Transform if 'Bend Goal Weight' > 0.")]
		public Transform solver_rightLeg_bendGoal;

		// Token: 0x040000FB RID: 251
		[Tooltip("Positional weight of the toe/foot target.")]
		[Range(0f, 1f)]
		public float solver_rightLeg_positionWeight;

		// Token: 0x040000FC RID: 252
		[Tooltip("Rotational weight of the toe/foot target.")]
		[Range(0f, 1f)]
		public float solver_rightLeg_rotationWeight;

		// Token: 0x040000FD RID: 253
		[Tooltip("If greater than 0, will bend the knee towards the 'Bend Goal' Transform.")]
		[Range(0f, 1f)]
		public float solver_rightLeg_bendGoalWeight;

		// Token: 0x040000FE RID: 254
		[Tooltip("Angular offset of the knee bending direction.")]
		[Range(-180f, 180f)]
		public float solver_rightLeg_swivelOffset;

		// Token: 0x040000FF RID: 255
		[Tooltip("If 0, the bend plane will be locked to the rotation of the pelvis and rotating the foot will have no effect on the knee direction. If 1, to the target rotation of the leg so that the knee will bend towards the forward axis of the foot. Values in between will be slerped between the two.")]
		[Range(0f, 1f)]
		public float solver_rightLeg_bendToTargetWeight = 0.5f;

		// Token: 0x04000100 RID: 256
		[Tooltip("Use this to make the leg shorter/longer.")]
		[Range(0.01f, 2f)]
		public float solver_rightLeg_legLengthMlp = 1f;

		// Token: 0x04000101 RID: 257
		[Tooltip("Evaluates stretching of the leg by target distance relative to leg length. Value at time 1 represents stretching amount at the point where distance to the target is equal to leg length. Value at time 1 represents stretching amount at the point where distance to the target is double the leg length. Value represents the amount of stretching. Linear stretching would be achieved with a linear curve going up by 45 degrees. Increase the range of stretching by moving the last key up and right at the same amount. Smoothing in the curve can help reduce knee snapping (start stretching the arm slightly before target distance reaches leg length).")]
		public AnimationCurve solver_rightLeg_stretchCurve = new AnimationCurve();

		// Token: 0x04000102 RID: 258
		[Header("Locomotion")]
		[Tooltip("Used for blending in/out of procedural locomotion.")]
		[Range(0f, 1f)]
		public float solver_locomotion_weight = 1f;

		// Token: 0x04000103 RID: 259
		[Tooltip("Tries to maintain this distance between the legs.")]
		public float solver_locomotion_footDistance = 0.3f;

		// Token: 0x04000104 RID: 260
		[Tooltip("Makes a step only if step target position is at least this far from the current footstep or the foot does not reach the current footstep anymore or footstep angle is past the 'Angle Threshold'.")]
		public float solver_locomotion_stepThreshold = 0.4f;

		// Token: 0x04000105 RID: 261
		[Tooltip("Makes a step only if step target position is at least 'Step Threshold' far from the current footstep or the foot does not reach the current footstep anymore or footstep angle is past this value.")]
		public float solver_locomotion_angleThreshold = 60f;

		// Token: 0x04000106 RID: 262
		[Tooltip("Multiplies angle of the center of mass - center of pressure vector. Larger value makes the character step sooner if losing balance.")]
		public float solver_locomotion_comAngleMlp = 1f;

		// Token: 0x04000107 RID: 263
		[Tooltip("Maximum magnitude of head/hand target velocity used in prediction.")]
		public float solver_locomotion_maxVelocity = 0.4f;

		// Token: 0x04000108 RID: 264
		[Tooltip("The amount of head/hand target velocity prediction.")]
		public float solver_locomotion_velocityFactor = 0.4f;

		// Token: 0x04000109 RID: 265
		[Tooltip("How much can a leg be extended before it is forced to step to another position? 1 means fully stretched.")]
		[Range(0.9f, 1f)]
		public float solver_locomotion_maxLegStretch = 1f;

		// Token: 0x0400010A RID: 266
		[Tooltip("The speed of lerping the root of the character towards the horizontal mid-point of the footsteps.")]
		public float solver_locomotion_rootSpeed = 20f;

		// Token: 0x0400010B RID: 267
		[Tooltip("The speed of steps.")]
		public float solver_locomotion_stepSpeed = 3f;

		// Token: 0x0400010C RID: 268
		[Tooltip("The height of the foot by normalized step progress (0 - 1).")]
		public AnimationCurve solver_locomotion_stepHeight = new AnimationCurve();

		// Token: 0x0400010D RID: 269
		[Tooltip("The height offset of the heel by normalized step progress (0 - 1).")]
		public AnimationCurve solver_locomotion_heelHeight = new AnimationCurve();

		// Token: 0x0400010E RID: 270
		[Tooltip("Rotates the foot while the leg is not stepping to relax the twist rotation of the leg if ideal rotation is past this angle.")]
		[Range(0f, 180f)]
		public float solver_locomotion_relaxLegTwistMinAngle = 20f;

		// Token: 0x0400010F RID: 271
		[Tooltip("The speed of rotating the foot while the leg is not stepping to relax the twist rotation of the leg.")]
		public float solver_locomotion_relaxLegTwistSpeed = 400f;

		// Token: 0x04000110 RID: 272
		[Tooltip("Interpolation mode of the step.")]
		public InterpolationMode solver_locomotion_stepInterpolation = 3;

		// Token: 0x04000111 RID: 273
		[Tooltip("Offset for the approximated center of mass.")]
		public Vector3 solver_locomotion_offset;

		// Token: 0x04000112 RID: 274
		[Tooltip("Called when the left foot has finished a step.")]
		public UnityEvent solver_locomotion_onLeftFootstep = new UnityEvent();

		// Token: 0x04000113 RID: 275
		[Tooltip("Called when the right foot has finished a step")]
		public UnityEvent solver_locomotion_onRightFootstep = new UnityEvent();

		// Token: 0x04000115 RID: 277
		internal VRIK vrik;
	}
}
