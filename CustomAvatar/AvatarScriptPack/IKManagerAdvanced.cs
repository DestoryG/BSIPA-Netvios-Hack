using System;
using System.Reflection;
using CustomAvatar;
using RootMotion;
using RootMotion.FinalIK;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace AvatarScriptPack
{
	// Token: 0x0200000C RID: 12
	[Obsolete("Use VRIKManager")]
	internal class IKManagerAdvanced : MonoBehaviour
	{
		// Token: 0x0600000C RID: 12 RVA: 0x000020E8 File Offset: 0x000002E8
		public void Start()
		{
			Plugin.logger.Warn("Avatar is still using the legacy IKManagerAdvanced; please migrate to VRIKManager");
			VRIKManager vrikmanager = base.gameObject.AddComponent<VRIKManager>();
			vrikmanager.solver_spine_headTarget = this.HeadTarget;
			vrikmanager.solver_leftArm_target = this.LeftHandTarget;
			vrikmanager.solver_rightArm_target = this.RightHandTarget;
			Type type = base.GetType();
			FieldInfo[] fields = type.GetFields(BindingFlags.Instance | BindingFlags.Public);
			foreach (FieldInfo fieldInfo in fields)
			{
				string[] array2 = fieldInfo.Name.Split(new char[] { '_' });
				object value = fieldInfo.GetValue(this);
				bool flag = array2.Length > 1;
				if (flag)
				{
					bool flag2 = "Spine" == array2[0];
					if (flag2)
					{
						IKManagerAdvanced.SetProperty(vrikmanager, "solver_spine_" + array2[1], value);
					}
					else
					{
						bool flag3 = "LeftArm" == array2[0];
						if (flag3)
						{
							IKManagerAdvanced.SetProperty(vrikmanager, "solver_leftArm_" + array2[1], value);
						}
						else
						{
							bool flag4 = "RightArm" == array2[0];
							if (flag4)
							{
								IKManagerAdvanced.SetProperty(vrikmanager, "solver_rightArm_" + array2[1], value);
							}
							else
							{
								bool flag5 = "LeftLeg" == array2[0];
								if (flag5)
								{
									IKManagerAdvanced.SetProperty(vrikmanager, "solver_leftLeg_" + array2[1], value);
								}
								else
								{
									bool flag6 = "RightLeg" == array2[0];
									if (flag6)
									{
										IKManagerAdvanced.SetProperty(vrikmanager, "solver_rightLeg_" + array2[1], value);
									}
									else
									{
										bool flag7 = "Locomotion" == array2[0];
										if (flag7)
										{
											IKManagerAdvanced.SetProperty(vrikmanager, "solver_locomotion_" + array2[1], value);
										}
									}
								}
							}
						}
					}
				}
			}
		}

		// Token: 0x0600000D RID: 13 RVA: 0x000022C0 File Offset: 0x000004C0
		public static void SetProperty(object obj, string fieldName, object value)
		{
			bool flag = obj == null;
			if (!flag)
			{
				try
				{
					FieldInfo field = obj.GetType().GetField(fieldName);
					bool flag2 = field == null;
					if (flag2)
					{
						Plugin.logger.Warn(string.Format("{0} does not exist on {1}", fieldName, obj.GetType()));
					}
					else
					{
						Plugin.logger.Debug(string.Format("Set {0} = {1}", field.Name, value));
						bool isEnum = field.FieldType.IsEnum;
						if (isEnum)
						{
							bool flag3 = value == null;
							if (flag3)
							{
								Plugin.logger.Warn("Tried to set Enum type to null");
							}
							else
							{
								Type underlyingType = Enum.GetUnderlyingType(value.GetType());
								Type underlyingType2 = Enum.GetUnderlyingType(field.FieldType);
								Plugin.logger.Debug(string.Format("Converting enum value {0} ({1}) -> {2} ({3})", new object[]
								{
									value.GetType(),
									underlyingType,
									field.FieldType,
									underlyingType2
								}));
								field.SetValue(obj, Convert.ChangeType(value, underlyingType2));
							}
						}
						else
						{
							field.SetValue(obj, Convert.ChangeType(value, field.FieldType));
						}
					}
				}
				catch (Exception ex)
				{
					Plugin.logger.Error(ex);
				}
			}
		}

		// Token: 0x04000004 RID: 4
		[Space(5f)]
		[Header("IK Targets")]
		[Tooltip("The head target.")]
		public Transform HeadTarget;

		// Token: 0x04000005 RID: 5
		[Tooltip("The hand target.")]
		public Transform LeftHandTarget;

		// Token: 0x04000006 RID: 6
		[Tooltip("The hand target.")]
		public Transform RightHandTarget;

		// Token: 0x04000007 RID: 7
		[Space(5f)]
		[Header("Full Body Tracking")]
		[Tooltip("The pelvis target, useful with seated rigs.")]
		public Transform Spine_pelvisTarget;

		// Token: 0x04000008 RID: 8
		[Range(0f, 1f)]
		[Tooltip("Positional weight of the pelvis target.")]
		public float Spine_pelvisPositionWeight;

		// Token: 0x04000009 RID: 9
		[Range(0f, 1f)]
		[Tooltip("Rotational weight of the pelvis target.")]
		public float Spine_pelvisRotationWeight;

		// Token: 0x0400000A RID: 10
		[Tooltip("The toe/foot target.")]
		public Transform LeftLeg_target;

		// Token: 0x0400000B RID: 11
		[Range(0f, 1f)]
		[Tooltip("Positional weight of the toe/foot target.")]
		public float LeftLeg_positionWeight;

		// Token: 0x0400000C RID: 12
		[Range(0f, 1f)]
		[Tooltip("Rotational weight of the toe/foot target.")]
		public float LeftLeg_rotationWeight;

		// Token: 0x0400000D RID: 13
		[Tooltip("The toe/foot target.")]
		public Transform RightLeg_target;

		// Token: 0x0400000E RID: 14
		[Range(0f, 1f)]
		[Tooltip("Positional weight of the toe/foot target.")]
		public float RightLeg_positionWeight;

		// Token: 0x0400000F RID: 15
		[Range(0f, 1f)]
		[Tooltip("Rotational weight of the toe/foot target.")]
		public float RightLeg_rotationWeight;

		// Token: 0x04000010 RID: 16
		[Space(20f)]
		[Range(0f, 1f)]
		[Tooltip("Positional weight of the head target.")]
		public float Head_positionWeight = 1f;

		// Token: 0x04000011 RID: 17
		[Range(0f, 1f)]
		[Tooltip("Rotational weight of the head target.")]
		public float Head_rotationWeight = 1f;

		// Token: 0x04000012 RID: 18
		[Tooltip("If 'Chest Goal Weight' is greater than 0, the chest will be turned towards this Transform.")]
		public Transform Spine_chestGoal;

		// Token: 0x04000013 RID: 19
		[Range(0f, 1f)]
		[Tooltip("Rotational weight of the chest target.")]
		public float Spine_chestGoalWeight;

		// Token: 0x04000014 RID: 20
		[Tooltip("Minimum height of the head from the root of the character.")]
		public float Spine_minHeadHeight = 0.8f;

		// Token: 0x04000015 RID: 21
		[Range(0f, 1f)]
		[Tooltip("Determines how much the body will follow the position of the head.")]
		public float Spine_bodyPosStiffness = 0.55f;

		// Token: 0x04000016 RID: 22
		[Range(0f, 1f)]
		[Tooltip("Determines how much the body will follow the rotation of the head.")]
		public float Spine_bodyRotStiffness = 0.1f;

		// Token: 0x04000017 RID: 23
		[Range(0f, 1f)]
		[FormerlySerializedAs("chestRotationWeight")]
		[Tooltip("Determines how much the chest will rotate to the rotation of the head.")]
		public float Spine_neckStiffness = 0.2f;

		// Token: 0x04000018 RID: 24
		[Range(0f, 1f)]
		[Tooltip("Clamps chest rotation.")]
		public float Spine_chestClampWeight = 0.5f;

		// Token: 0x04000019 RID: 25
		[Range(0f, 1f)]
		[Tooltip("Clamps head rotation.")]
		public float Spine_headClampWeight = 0.6f;

		// Token: 0x0400001A RID: 26
		[Range(0f, 1f)]
		[Tooltip("How much will the pelvis maintain it's animated position?")]
		public float Spine_maintainPelvisPosition = 0.2f;

		// Token: 0x0400001B RID: 27
		[Range(0f, 180f)]
		[Tooltip("Will automatically rotate the root of the character if the head target has turned past this angle.")]
		public float Spine_maxRootAngle = 25f;

		// Token: 0x0400001C RID: 28
		[Space(20f)]
		[Tooltip("The elbow will be bent towards this Transform if 'Bend Goal Weight' > 0.")]
		public Transform LeftArm_bendGoal;

		// Token: 0x0400001D RID: 29
		[Range(0f, 1f)]
		[Tooltip("Positional weight of the hand target.")]
		public float LeftArm_positionWeight = 1f;

		// Token: 0x0400001E RID: 30
		[Range(0f, 1f)]
		[Tooltip("Rotational weight of the hand target")]
		public float LeftArm_rotationWeight = 1f;

		// Token: 0x0400001F RID: 31
		[Tooltip("Different techniques for shoulder bone rotation.")]
		public IKSolverVR.Arm.ShoulderRotationMode LeftArm_shoulderRotationMode;

		// Token: 0x04000020 RID: 32
		[Range(0f, 1f)]
		[Tooltip("The weight of shoulder rotation")]
		public float LeftArm_shoulderRotationWeight = 1f;

		// Token: 0x04000021 RID: 33
		[Range(0f, 1f)]
		[Tooltip("If greater than 0, will bend the elbow towards the 'Bend Goal' Transform.")]
		public float LeftArm_bendGoalWeight;

		// Token: 0x04000022 RID: 34
		[Range(-180f, 180f)]
		[Tooltip("Angular offset of the elbow bending direction.")]
		public float LeftArm_swivelOffset;

		// Token: 0x04000023 RID: 35
		[Tooltip("Local axis of the hand bone that points from the wrist towards the palm. Used for defining hand bone orientation.")]
		public Vector3 LeftArm_wristToPalmAxis = Vector3.zero;

		// Token: 0x04000024 RID: 36
		[Tooltip("Local axis of the hand bone that points from the palm towards the thumb. Used for defining hand bone orientation.")]
		public Vector3 LeftArm_palmToThumbAxis = Vector3.zero;

		// Token: 0x04000025 RID: 37
		[Space(20f)]
		[Tooltip("The elbow will be bent towards this Transform if 'Bend Goal Weight' > 0.")]
		public Transform RightArm_bendGoal;

		// Token: 0x04000026 RID: 38
		[Range(0f, 1f)]
		[Tooltip("Positional weight of the hand target.")]
		public float RightArm_positionWeight = 1f;

		// Token: 0x04000027 RID: 39
		[Range(0f, 1f)]
		[Tooltip("Rotational weight of the hand target")]
		public float RightArm_rotationWeight = 1f;

		// Token: 0x04000028 RID: 40
		[Tooltip("Different techniques for shoulder bone rotation.")]
		public IKSolverVR.Arm.ShoulderRotationMode RightArm_shoulderRotationMode;

		// Token: 0x04000029 RID: 41
		[Range(0f, 1f)]
		[Tooltip("The weight of shoulder rotation")]
		public float RightArm_shoulderRotationWeight = 1f;

		// Token: 0x0400002A RID: 42
		[Range(0f, 1f)]
		[Tooltip("If greater than 0, will bend the elbow towards the 'Bend Goal' Transform.")]
		public float RightArm_bendGoalWeight;

		// Token: 0x0400002B RID: 43
		[Range(-180f, 180f)]
		[Tooltip("Angular offset of the elbow bending direction.")]
		public float RightArm_swivelOffset;

		// Token: 0x0400002C RID: 44
		[Tooltip("Local axis of the hand bone that points from the wrist towards the palm. Used for defining hand bone orientation.")]
		public Vector3 RightArm_wristToPalmAxis = Vector3.zero;

		// Token: 0x0400002D RID: 45
		[Tooltip("Local axis of the hand bone that points from the palm towards the thumb. Used for defining hand bone orientation.")]
		public Vector3 RightArm_palmToThumbAxis = Vector3.zero;

		// Token: 0x0400002E RID: 46
		[Space(20f)]
		[Tooltip("The knee will be bent towards this Transform if 'Bend Goal Weight' > 0.")]
		public Transform LeftLeg_bendGoal;

		// Token: 0x0400002F RID: 47
		[Range(0f, 1f)]
		[Tooltip("If greater than 0, will bend the knee towards the 'Bend Goal' Transform.")]
		public float LeftLeg_bendGoalWeight;

		// Token: 0x04000030 RID: 48
		[Range(-180f, 180f)]
		[Tooltip("Angular offset of the knee bending direction.")]
		public float LeftLeg_swivelOffset;

		// Token: 0x04000031 RID: 49
		[Space(20f)]
		[Tooltip("The knee will be bent towards this Transform if 'Bend Goal Weight' > 0.")]
		public Transform RightLeg_bendGoal;

		// Token: 0x04000032 RID: 50
		[Range(0f, 1f)]
		[Tooltip("If greater than 0, will bend the knee towards the 'Bend Goal' Transform.")]
		public float RightLeg_bendGoalWeight;

		// Token: 0x04000033 RID: 51
		[Range(-180f, 180f)]
		[Tooltip("Angular offset of the knee bending direction.")]
		public float RightLeg_swivelOffset;

		// Token: 0x04000034 RID: 52
		[Space(20f)]
		[Range(0f, 1f)]
		[Tooltip("Used for blending in/out of procedural locomotion.")]
		public float Locomotion_weight = 1f;

		// Token: 0x04000035 RID: 53
		[Tooltip("Tries to maintain this distance between the legs.")]
		public float Locomotion_footDistance = 0.3f;

		// Token: 0x04000036 RID: 54
		[Tooltip("Makes a step only if step target position is at least this far from the current footstep or the foot does not reach the current footstep anymore or footstep angle is past the 'Angle Threshold'.")]
		public float Locomotion_stepThreshold = 0.4f;

		// Token: 0x04000037 RID: 55
		[Tooltip("Makes a step only if step target position is at least 'Step Threshold' far from the current footstep or the foot does not reach the current footstep anymore or footstep angle is past this value.")]
		public float Locomotion_angleThreshold = 60f;

		// Token: 0x04000038 RID: 56
		[Tooltip("Multiplies angle of the center of mass - center of pressure vector. Larger value makes the character step sooner if losing balance.")]
		public float Locomotion_comAngleMlp = 1f;

		// Token: 0x04000039 RID: 57
		[Tooltip("Maximum magnitude of head/hand target velocity used in prediction.")]
		public float Locomotion_maxVelocity = 0.4f;

		// Token: 0x0400003A RID: 58
		[Tooltip("The amount of head/hand target velocity prediction.")]
		public float Locomotion_velocityFactor = 0.4f;

		// Token: 0x0400003B RID: 59
		[Range(0.9f, 1f)]
		[Tooltip("How much can a leg be extended before it is forced to step to another position? 1 means fully stretched.")]
		public float Locomotion_maxLegStretch = 1f;

		// Token: 0x0400003C RID: 60
		[Tooltip("The speed of lerping the root of the character towards the horizontal mid-point of the footsteps.")]
		public float Locomotion_rootSpeed = 20f;

		// Token: 0x0400003D RID: 61
		[Tooltip("The speed of steps.")]
		public float Locomotion_stepSpeed = 3f;

		// Token: 0x0400003E RID: 62
		[Tooltip("The height of the foot by normalized step progress (0 - 1).")]
		public AnimationCurve Locomotion_stepHeight;

		// Token: 0x0400003F RID: 63
		[Tooltip("The height offset of the heel by normalized step progress (0 - 1).")]
		public AnimationCurve Locomotion_heelHeight;

		// Token: 0x04000040 RID: 64
		[Range(0f, 180f)]
		[Tooltip("Rotates the foot while the leg is not stepping to relax the twist rotation of the leg if ideal rotation is past this angle.")]
		public float Locomotion_relaxLegTwistMinAngle = 20f;

		// Token: 0x04000041 RID: 65
		[Tooltip("The speed of rotating the foot while the leg is not stepping to relax the twist rotation of the leg.")]
		public float Locomotion_relaxLegTwistSpeed = 400f;

		// Token: 0x04000042 RID: 66
		[Tooltip("Interpolation mode of the step.")]
		public InterpolationMode Locomotion_stepInterpolation = 3;

		// Token: 0x04000043 RID: 67
		[Tooltip("Offset for the approximated center of mass.")]
		public Vector3 Locomotion_offset;

		// Token: 0x04000044 RID: 68
		[Tooltip("Called when the left foot has finished a step.")]
		public UnityEvent Locomotion_onLeftFootstep = new UnityEvent();

		// Token: 0x04000045 RID: 69
		[Tooltip("Called when the right foot has finished a step")]
		public UnityEvent Locomotion_onRightFootstep = new UnityEvent();
	}
}
