using System;
using System.Collections.Generic;
using System.IO;
using AvatarScriptPack;
using CustomAvatar.Exceptions;
using UnityEngine;

namespace CustomAvatar.Avatar
{
	// Token: 0x02000039 RID: 57
	public class LoadedAvatar
	{
		// Token: 0x1700002D RID: 45
		// (get) Token: 0x06000133 RID: 307 RVA: 0x00008DE4 File Offset: 0x00006FE4
		public string fullPath { get; }

		// Token: 0x1700002E RID: 46
		// (get) Token: 0x06000134 RID: 308 RVA: 0x00008DEC File Offset: 0x00006FEC
		public GameObject gameObject { get; }

		// Token: 0x1700002F RID: 47
		// (get) Token: 0x06000135 RID: 309 RVA: 0x00008DF4 File Offset: 0x00006FF4
		public AvatarDescriptor descriptor { get; }

		// Token: 0x17000030 RID: 48
		// (get) Token: 0x06000136 RID: 310 RVA: 0x00008DFC File Offset: 0x00006FFC
		public float eyeHeight { get; }

		// Token: 0x17000031 RID: 49
		// (get) Token: 0x06000137 RID: 311 RVA: 0x00008E04 File Offset: 0x00007004
		public bool supportsFingerTracking { get; }

		// Token: 0x17000032 RID: 50
		// (get) Token: 0x06000138 RID: 312 RVA: 0x00008E0C File Offset: 0x0000700C
		public bool isIKAvatar
		{
			get
			{
				VRIKManager vrikmanager;
				if ((vrikmanager = this.gameObject.GetComponentInChildren<VRIKManager>()) == null)
				{
					vrikmanager = this.gameObject.GetComponentInChildren<IKManager>() ?? this.gameObject.GetComponentInChildren<IKManagerAdvanced>();
				}
				return vrikmanager != null;
			}
		}

		// Token: 0x06000139 RID: 313 RVA: 0x00008E3C File Offset: 0x0000703C
		public LoadedAvatar(string fullPath, GameObject avatarGameObject)
		{
			if (fullPath == null)
			{
				throw new ArgumentNullException("avatarGameObject");
			}
			this.fullPath = fullPath;
			if (!avatarGameObject)
			{
				throw new ArgumentNullException("avatarGameObject");
			}
			this.gameObject = avatarGameObject;
			AvatarDescriptor component = avatarGameObject.GetComponent<AvatarDescriptor>();
			if (component == null)
			{
				throw new AvatarLoadException("Avatar at '" + fullPath + "' does not have an AvatarDescriptor");
			}
			this.descriptor = component;
			this.supportsFingerTracking = avatarGameObject.GetComponentInChildren<Animator>() && avatarGameObject.GetComponentInChildren<PoseManager>();
			this.eyeHeight = this.GetEyeHeight();
		}

		// Token: 0x0600013A RID: 314 RVA: 0x00008ED4 File Offset: 0x000070D4
		public static IEnumerator<AsyncOperation> FromFileCoroutine(string fileName, Action<LoadedAvatar> success, Action<Exception> error)
		{
			Plugin.logger.Info("Loading avatar " + fileName);
			AssetBundleCreateRequest assetBundleCreateRequest = AssetBundle.LoadFromFileAsync(Path.Combine(AvatarManager.kCustomAvatarsPath, fileName));
			yield return assetBundleCreateRequest;
			bool flag = !assetBundleCreateRequest.isDone || !assetBundleCreateRequest.assetBundle;
			if (flag)
			{
				error(new AvatarLoadException("Avatar game object not found"));
				yield break;
			}
			AssetBundleRequest assetBundleRequest = assetBundleCreateRequest.assetBundle.LoadAssetWithSubAssetsAsync<GameObject>("_CustomAvatar");
			yield return assetBundleRequest;
			assetBundleCreateRequest.assetBundle.Unload(false);
			bool flag2 = !assetBundleRequest.isDone || assetBundleRequest.asset == null;
			if (flag2)
			{
				error(new AvatarLoadException("Could not load asset bundle"));
				yield break;
			}
			try
			{
				success(new LoadedAvatar(fileName, assetBundleRequest.asset as GameObject));
				yield break;
			}
			catch (Exception ex2)
			{
				Exception ex = ex2;
				error(ex);
				yield break;
			}
			yield break;
		}

		// Token: 0x0600013B RID: 315 RVA: 0x00008EF4 File Offset: 0x000070F4
		private float GetEyeHeight()
		{
			bool flag = !this.isIKAvatar;
			float num;
			if (flag)
			{
				num = 1.6999999f;
			}
			else
			{
				num = this.GetViewPoint().y;
			}
			return num;
		}

		// Token: 0x0600013C RID: 316 RVA: 0x00008F28 File Offset: 0x00007128
		private Vector3 GetViewPoint()
		{
			Transform transform = this.gameObject.transform.Find("Head");
			if (transform == null)
			{
				throw new AvatarLoadException("Avatar '" + this.descriptor.name + "' does not have a Head transform");
			}
			Transform transform2 = transform;
			Vector3 headTargetOffset = this.GetHeadTargetOffset();
			bool flag = headTargetOffset.magnitude > 0.001f;
			if (flag)
			{
				Plugin.logger.Warn(string.Format("Head bone and head target are not at the same position; offset: ({0}, {1}, {2})", headTargetOffset.x, headTargetOffset.y, headTargetOffset.z));
			}
			return this.gameObject.transform.InverseTransformPoint(transform2.position - headTargetOffset);
		}

		// Token: 0x0600013D RID: 317 RVA: 0x00008FE4 File Offset: 0x000071E4
		private Vector3 GetHeadTargetOffset()
		{
			Transform transform = null;
			Transform transform2 = null;
			VRIK componentInChildren = this.gameObject.GetComponentInChildren<VRIK>();
			IKManager componentInChildren2 = this.gameObject.GetComponentInChildren<IKManager>();
			IKManagerAdvanced componentInChildren3 = this.gameObject.GetComponentInChildren<IKManagerAdvanced>();
			VRIKManager componentInChildren4 = this.gameObject.GetComponentInChildren<VRIKManager>();
			bool flag = componentInChildren4;
			if (flag)
			{
				bool flag2 = !componentInChildren4.references_head;
				if (flag2)
				{
					componentInChildren4.AutoDetectReferences();
				}
				transform = componentInChildren4.references_head;
				transform2 = componentInChildren4.solver_spine_headTarget;
			}
			else
			{
				bool flag3 = componentInChildren;
				if (flag3)
				{
					componentInChildren.AutoDetectReferences();
					transform = componentInChildren.references.head;
					bool flag4 = componentInChildren3;
					if (flag4)
					{
						transform2 = componentInChildren3.HeadTarget;
					}
					else
					{
						bool flag5 = componentInChildren2;
						if (flag5)
						{
							transform2 = componentInChildren2.HeadTarget;
						}
					}
				}
			}
			bool flag6 = !transform;
			Vector3 vector;
			if (flag6)
			{
				Plugin.logger.Warn("Could not find head reference; height adjust may be broken");
				vector = Vector3.zero;
			}
			else
			{
				bool flag7 = !transform2;
				if (flag7)
				{
					Plugin.logger.Warn("Could not find head target; height adjust may be broken");
					vector = Vector3.zero;
				}
				else
				{
					vector = transform2.position - transform.position;
				}
			}
			return vector;
		}

		// Token: 0x0600013E RID: 318 RVA: 0x00009120 File Offset: 0x00007320
		public float GetArmSpan()
		{
			Animator componentInChildren = this.gameObject.GetComponentInChildren<Animator>();
			bool flag = !componentInChildren;
			float num;
			if (flag)
			{
				num = 0f;
			}
			else
			{
				Vector3 position = componentInChildren.GetBoneTransform(11).position;
				Vector3 position2 = componentInChildren.GetBoneTransform(13).position;
				Vector3 position3 = componentInChildren.GetBoneTransform(15).position;
				Vector3 position4 = this.gameObject.transform.Find("LeftHand").position;
				Vector3 position5 = componentInChildren.GetBoneTransform(12).position;
				Vector3 position6 = componentInChildren.GetBoneTransform(14).position;
				Vector3 position7 = componentInChildren.GetBoneTransform(16).position;
				Vector3 position8 = this.gameObject.transform.Find("RightHand").position;
				float num2 = Vector3.Distance(position, position2) + Vector3.Distance(position2, position3) + Vector3.Distance(position3, position4);
				float num3 = Vector3.Distance(position5, position6) + Vector3.Distance(position6, position7) + Vector3.Distance(position7, position8);
				float num4 = Vector3.Distance(position, position5);
				float num5 = num2 + num4 + num3;
				Plugin.logger.Debug("Avatar arm span: " + num5.ToString());
				num = num5;
			}
			return num;
		}

		// Token: 0x040001AC RID: 428
		private const string kGameObjectName = "_CustomAvatar";
	}
}
