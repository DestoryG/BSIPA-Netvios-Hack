using System;
using System.Collections.Generic;
using UnityEngine;

namespace CustomAvatar.StereoRendering
{
	// Token: 0x0200002C RID: 44
	internal static class MirrorHelper
	{
		// Token: 0x060000B9 RID: 185 RVA: 0x000059C8 File Offset: 0x00003BC8
		public static IEnumerator<AsyncOperation> SpawnMirror(Vector3 position, Quaternion rotation, Vector3 scale, Transform container)
		{
			AssetBundleCreateRequest shadersBundleCreateRequest = AssetBundle.LoadFromFileAsync("CustomAvatars/Shaders/customavatars.assetbundle");
			yield return shadersBundleCreateRequest;
			bool flag = !shadersBundleCreateRequest.isDone || shadersBundleCreateRequest.assetBundle == null;
			if (flag)
			{
				Plugin.logger.Error("Failed to load stereo mirror shader");
				yield break;
			}
			AssetBundleRequest assetBundleRequest = shadersBundleCreateRequest.assetBundle.LoadAssetAsync<Shader>("Assets/Shaders/StereoRenderShader-Unlit.shader");
			yield return assetBundleRequest;
			shadersBundleCreateRequest.assetBundle.Unload(false);
			bool flag2 = !assetBundleRequest.isDone || assetBundleRequest.asset == null;
			if (flag2)
			{
				Plugin.logger.Error("Failed to load stereo mirror shader");
				yield break;
			}
			Shader stereoRenderShader = assetBundleRequest.asset as Shader;
			GameObject mirrorPlane = GameObject.CreatePrimitive(4);
			mirrorPlane.transform.SetParent(container);
			mirrorPlane.name = "Stereo Mirror";
			mirrorPlane.transform.localScale = scale;
			mirrorPlane.transform.localPosition = position + new Vector3(0f, scale.z * 5f, 0f);
			mirrorPlane.transform.localRotation = rotation;
			Material material = new Material(stereoRenderShader);
			material.SetFloat("_Cutout", 0.01f);
			Renderer renderer = mirrorPlane.GetComponent<Renderer>();
			renderer.sharedMaterial = material;
			GameObject stereoCameraHead = new GameObject("Stereo Camera Head [Stereo Mirror]");
			stereoCameraHead.transform.SetParent(mirrorPlane.transform, false);
			stereoCameraHead.transform.localScale = new Vector3(1f / scale.x, 1f / scale.y, 1f / scale.z);
			GameObject stereoCameraEyeObject = new GameObject("Stereo Camera Eye [Stereo Mirror]");
			stereoCameraEyeObject.transform.SetParent(mirrorPlane.transform, false);
			Camera stereoCameraEye = stereoCameraEyeObject.AddComponent<Camera>();
			stereoCameraEye.enabled = false;
			stereoCameraEye.cullingMask = 1032;
			stereoCameraEye.clearFlags = 2;
			stereoCameraEye.backgroundColor = new Color(0f, 0f, 0f, 1f);
			StereoRenderer stereoRenderer = mirrorPlane.AddComponent<StereoRenderer>();
			stereoRenderer.stereoCameraHead = stereoCameraHead;
			stereoRenderer.stereoCameraEye = stereoCameraEye;
			stereoRenderer.isMirror = true;
			stereoRenderer.useScissor = false;
			stereoRenderer.canvasOriginPos = mirrorPlane.transform.position + new Vector3(-10f, 0f, 0f);
			stereoRenderer.canvasOriginRot = mirrorPlane.transform.rotation;
			yield break;
		}
	}
}
