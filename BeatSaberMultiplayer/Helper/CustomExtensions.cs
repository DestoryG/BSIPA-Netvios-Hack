using System;
using System.Linq;
using System.Reflection;
using TMPro;
using UnityEngine;

namespace BeatSaberMultiplayer.Helper
{
	// Token: 0x02000076 RID: 118
	public static class CustomExtensions
	{
		// Token: 0x1700023B RID: 571
		// (get) Token: 0x06000870 RID: 2160 RVA: 0x000241ED File Offset: 0x000223ED
		public static Shader CustomTextShader
		{
			get
			{
				if (CustomExtensions._customTextShader == null)
				{
					CustomExtensions._customTextShader = AssetBundle.LoadFromStream(Assembly.GetCallingAssembly().GetManifestResourceStream("BeatSaberMultiplayer.Assets.Shader.asset")).LoadAsset<Shader>("Assets/TextMesh Pro/Resources/Shaders/TMP_SDF_ZeroAlphaWrite_ZWrite.shader");
				}
				return CustomExtensions._customTextShader;
			}
		}

		// Token: 0x06000871 RID: 2161 RVA: 0x00024224 File Offset: 0x00022424
		public static TextMeshPro CreateWorldText(Transform parent, string text = "TEXT")
		{
			GameObject gameObject = new GameObject("CustomUIText");
			gameObject.SetActive(false);
			TextMeshPro textMeshPro = gameObject.AddComponent<TextMeshPro>();
			TMP_FontAsset tmp_FontAsset = Object.Instantiate<TMP_FontAsset>(Resources.FindObjectsOfTypeAll<TMP_FontAsset>().First((TMP_FontAsset x) => x.name == "Teko-Medium SDF No Glow"));
			textMeshPro.renderer.sharedMaterial = tmp_FontAsset.material;
			textMeshPro.fontSharedMaterial = tmp_FontAsset.material;
			textMeshPro.font = tmp_FontAsset;
			textMeshPro.transform.SetParent(parent, true);
			textMeshPro.text = text;
			textMeshPro.fontSize = 5f;
			textMeshPro.color = Color.white;
			textMeshPro.renderer.material.shader = CustomExtensions.CustomTextShader;
			textMeshPro.gameObject.SetActive(true);
			return textMeshPro;
		}

		// Token: 0x0400044A RID: 1098
		private static Shader _customTextShader;
	}
}
