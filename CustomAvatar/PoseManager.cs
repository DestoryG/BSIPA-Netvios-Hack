using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace CustomAvatar
{
	// Token: 0x02000017 RID: 23
	public class PoseManager : MonoBehaviour
	{
		// Token: 0x0600003C RID: 60 RVA: 0x000032DE File Offset: 0x000014DE
		public void SaveOpenHand(Animator animator)
		{
			this.SaveValues("openHand", animator);
		}

		// Token: 0x0600003D RID: 61 RVA: 0x000032EE File Offset: 0x000014EE
		public void SaveClosedHand(Animator animator)
		{
			this.SaveValues("closedHand", animator);
		}

		// Token: 0x0600003E RID: 62 RVA: 0x000032FE File Offset: 0x000014FE
		public void ApplyOpenHand(Animator animator)
		{
			this.ApplyValues("openHand", animator);
		}

		// Token: 0x0600003F RID: 63 RVA: 0x0000330E File Offset: 0x0000150E
		public void ApplyClosedHand(Animator animator)
		{
			this.ApplyValues("closedHand", animator);
		}

		// Token: 0x06000040 RID: 64 RVA: 0x00003320 File Offset: 0x00001520
		private void SaveValues(string prefix, Animator animator)
		{
			bool flag = !animator.isHuman;
			if (!flag)
			{
				IEnumerable<FieldInfo> fields = base.GetType().GetFields();
				Func<FieldInfo, bool> <>9__0;
				Func<FieldInfo, bool> func;
				if ((func = <>9__0) == null)
				{
					func = (<>9__0 = (FieldInfo f) => f.Name.StartsWith(prefix));
				}
				foreach (FieldInfo fieldInfo in fields.Where(func))
				{
					string text = fieldInfo.Name.Split(new char[] { '_' })[1];
					HumanBodyBones humanBodyBones;
					bool flag2 = Enum.TryParse<HumanBodyBones>(text, out humanBodyBones);
					if (flag2)
					{
						fieldInfo.SetValue(this, this.TransformToLocalPose(animator.GetBoneTransform(humanBodyBones)));
					}
					else
					{
						Debug.LogError("Could not find HumanBodyBones." + text);
					}
				}
			}
		}

		// Token: 0x06000041 RID: 65 RVA: 0x00003414 File Offset: 0x00001614
		private void ApplyValues(string prefix, Animator animator)
		{
			bool flag = !animator.isHuman;
			if (!flag)
			{
				IEnumerable<FieldInfo> fields = base.GetType().GetFields();
				Func<FieldInfo, bool> <>9__0;
				Func<FieldInfo, bool> func;
				if ((func = <>9__0) == null)
				{
					func = (<>9__0 = (FieldInfo f) => f.Name.StartsWith(prefix));
				}
				foreach (FieldInfo fieldInfo in fields.Where(func))
				{
					string text = fieldInfo.Name.Split(new char[] { '_' })[1];
					HumanBodyBones humanBodyBones;
					bool flag2 = Enum.TryParse<HumanBodyBones>(text, out humanBodyBones);
					if (flag2)
					{
						Pose pose = (Pose)fieldInfo.GetValue(this);
						bool flag3 = pose.Equals(default(Pose));
						if (!flag3)
						{
							Transform boneTransform = animator.GetBoneTransform(humanBodyBones);
							boneTransform.localPosition = pose.position;
							boneTransform.localRotation = pose.rotation;
						}
					}
					else
					{
						Debug.LogError("Could not find HumanBodyBones." + text);
					}
				}
			}
		}

		// Token: 0x06000042 RID: 66 RVA: 0x00003540 File Offset: 0x00001740
		private Pose TransformToLocalPose(Transform transform)
		{
			bool flag = transform == null;
			Pose pose;
			if (flag)
			{
				pose = default(Pose);
			}
			else
			{
				pose = new Pose(transform.localPosition, transform.localRotation);
			}
			return pose;
		}

		// Token: 0x04000064 RID: 100
		[HideInInspector]
		public Pose openHand_LeftThumbProximal;

		// Token: 0x04000065 RID: 101
		[HideInInspector]
		public Pose openHand_LeftThumbIntermediate;

		// Token: 0x04000066 RID: 102
		[HideInInspector]
		public Pose openHand_LeftThumbDistal;

		// Token: 0x04000067 RID: 103
		[HideInInspector]
		public Pose openHand_LeftIndexProximal;

		// Token: 0x04000068 RID: 104
		[HideInInspector]
		public Pose openHand_LeftIndexIntermediate;

		// Token: 0x04000069 RID: 105
		[HideInInspector]
		public Pose openHand_LeftIndexDistal;

		// Token: 0x0400006A RID: 106
		[HideInInspector]
		public Pose openHand_LeftMiddleProximal;

		// Token: 0x0400006B RID: 107
		[HideInInspector]
		public Pose openHand_LeftMiddleIntermediate;

		// Token: 0x0400006C RID: 108
		[HideInInspector]
		public Pose openHand_LeftMiddleDistal;

		// Token: 0x0400006D RID: 109
		[HideInInspector]
		public Pose openHand_LeftRingProximal;

		// Token: 0x0400006E RID: 110
		[HideInInspector]
		public Pose openHand_LeftRingIntermediate;

		// Token: 0x0400006F RID: 111
		[HideInInspector]
		public Pose openHand_LeftRingDistal;

		// Token: 0x04000070 RID: 112
		[HideInInspector]
		public Pose openHand_LeftLittleProximal;

		// Token: 0x04000071 RID: 113
		[HideInInspector]
		public Pose openHand_LeftLittleIntermediate;

		// Token: 0x04000072 RID: 114
		[HideInInspector]
		public Pose openHand_LeftLittleDistal;

		// Token: 0x04000073 RID: 115
		[HideInInspector]
		public Pose openHand_RightThumbProximal;

		// Token: 0x04000074 RID: 116
		[HideInInspector]
		public Pose openHand_RightThumbIntermediate;

		// Token: 0x04000075 RID: 117
		[HideInInspector]
		public Pose openHand_RightThumbDistal;

		// Token: 0x04000076 RID: 118
		[HideInInspector]
		public Pose openHand_RightIndexProximal;

		// Token: 0x04000077 RID: 119
		[HideInInspector]
		public Pose openHand_RightIndexIntermediate;

		// Token: 0x04000078 RID: 120
		[HideInInspector]
		public Pose openHand_RightIndexDistal;

		// Token: 0x04000079 RID: 121
		[HideInInspector]
		public Pose openHand_RightMiddleProximal;

		// Token: 0x0400007A RID: 122
		[HideInInspector]
		public Pose openHand_RightMiddleIntermediate;

		// Token: 0x0400007B RID: 123
		[HideInInspector]
		public Pose openHand_RightMiddleDistal;

		// Token: 0x0400007C RID: 124
		[HideInInspector]
		public Pose openHand_RightRingProximal;

		// Token: 0x0400007D RID: 125
		[HideInInspector]
		public Pose openHand_RightRingIntermediate;

		// Token: 0x0400007E RID: 126
		[HideInInspector]
		public Pose openHand_RightRingDistal;

		// Token: 0x0400007F RID: 127
		[HideInInspector]
		public Pose openHand_RightLittleProximal;

		// Token: 0x04000080 RID: 128
		[HideInInspector]
		public Pose openHand_RightLittleIntermediate;

		// Token: 0x04000081 RID: 129
		[HideInInspector]
		public Pose openHand_RightLittleDistal;

		// Token: 0x04000082 RID: 130
		[HideInInspector]
		public Pose closedHand_LeftThumbProximal;

		// Token: 0x04000083 RID: 131
		[HideInInspector]
		public Pose closedHand_LeftThumbIntermediate;

		// Token: 0x04000084 RID: 132
		[HideInInspector]
		public Pose closedHand_LeftThumbDistal;

		// Token: 0x04000085 RID: 133
		[HideInInspector]
		public Pose closedHand_LeftIndexProximal;

		// Token: 0x04000086 RID: 134
		[HideInInspector]
		public Pose closedHand_LeftIndexIntermediate;

		// Token: 0x04000087 RID: 135
		[HideInInspector]
		public Pose closedHand_LeftIndexDistal;

		// Token: 0x04000088 RID: 136
		[HideInInspector]
		public Pose closedHand_LeftMiddleProximal;

		// Token: 0x04000089 RID: 137
		[HideInInspector]
		public Pose closedHand_LeftMiddleIntermediate;

		// Token: 0x0400008A RID: 138
		[HideInInspector]
		public Pose closedHand_LeftMiddleDistal;

		// Token: 0x0400008B RID: 139
		[HideInInspector]
		public Pose closedHand_LeftRingProximal;

		// Token: 0x0400008C RID: 140
		[HideInInspector]
		public Pose closedHand_LeftRingIntermediate;

		// Token: 0x0400008D RID: 141
		[HideInInspector]
		public Pose closedHand_LeftRingDistal;

		// Token: 0x0400008E RID: 142
		[HideInInspector]
		public Pose closedHand_LeftLittleProximal;

		// Token: 0x0400008F RID: 143
		[HideInInspector]
		public Pose closedHand_LeftLittleIntermediate;

		// Token: 0x04000090 RID: 144
		[HideInInspector]
		public Pose closedHand_LeftLittleDistal;

		// Token: 0x04000091 RID: 145
		[HideInInspector]
		public Pose closedHand_RightThumbProximal;

		// Token: 0x04000092 RID: 146
		[HideInInspector]
		public Pose closedHand_RightThumbIntermediate;

		// Token: 0x04000093 RID: 147
		[HideInInspector]
		public Pose closedHand_RightThumbDistal;

		// Token: 0x04000094 RID: 148
		[HideInInspector]
		public Pose closedHand_RightIndexProximal;

		// Token: 0x04000095 RID: 149
		[HideInInspector]
		public Pose closedHand_RightIndexIntermediate;

		// Token: 0x04000096 RID: 150
		[HideInInspector]
		public Pose closedHand_RightIndexDistal;

		// Token: 0x04000097 RID: 151
		[HideInInspector]
		public Pose closedHand_RightMiddleProximal;

		// Token: 0x04000098 RID: 152
		[HideInInspector]
		public Pose closedHand_RightMiddleIntermediate;

		// Token: 0x04000099 RID: 153
		[HideInInspector]
		public Pose closedHand_RightMiddleDistal;

		// Token: 0x0400009A RID: 154
		[HideInInspector]
		public Pose closedHand_RightRingProximal;

		// Token: 0x0400009B RID: 155
		[HideInInspector]
		public Pose closedHand_RightRingIntermediate;

		// Token: 0x0400009C RID: 156
		[HideInInspector]
		public Pose closedHand_RightRingDistal;

		// Token: 0x0400009D RID: 157
		[HideInInspector]
		public Pose closedHand_RightLittleProximal;

		// Token: 0x0400009E RID: 158
		[HideInInspector]
		public Pose closedHand_RightLittleIntermediate;

		// Token: 0x0400009F RID: 159
		[HideInInspector]
		public Pose closedHand_RightLittleDistal;
	}
}
