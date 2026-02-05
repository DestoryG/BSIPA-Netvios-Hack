using System;
using UnityEngine;

namespace CustomAvatar.StereoRendering
{
	// Token: 0x0200002F RID: 47
	[RequireComponent(typeof(Camera))]
	[DisallowMultipleComponent]
	internal class VRRenderEventDetector : MonoBehaviour
	{
		// Token: 0x17000028 RID: 40
		// (get) Token: 0x060000ED RID: 237 RVA: 0x00006DB3 File Offset: 0x00004FB3
		// (set) Token: 0x060000EE RID: 238 RVA: 0x00006DBB File Offset: 0x00004FBB
		public Camera Camera { get; private set; }

		// Token: 0x060000EF RID: 239 RVA: 0x00006DC4 File Offset: 0x00004FC4
		public void Start()
		{
			this.Camera = base.GetComponent<Camera>();
		}

		// Token: 0x060000F0 RID: 240 RVA: 0x00006DD4 File Offset: 0x00004FD4
		private void OnPreRender()
		{
			StereoRenderManager.Instance.InvokeStereoRenderers(this);
		}
	}
}
