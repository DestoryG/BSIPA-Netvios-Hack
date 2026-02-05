using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CameraPlus
{
	// Token: 0x0200000F RID: 15
	public class SongCameraMovement : CameraMovement
	{
		// Token: 0x06000079 RID: 121 RVA: 0x000075D1 File Offset: 0x000057D1
		public override bool Init(CameraPlusBehaviour cameraPlus)
		{
			if (Utils.IsModInstalled("SongLoaderPlugin"))
			{
				this._cameraPlus = cameraPlus;
				Plugin instance = Plugin.Instance;
				instance.ActiveSceneChanged = (Action<Scene, Scene>)Delegate.Combine(instance.ActiveSceneChanged, new Action<Scene, Scene>(this.OnActiveSceneChanged));
				return true;
			}
			return false;
		}

		// Token: 0x0600007A RID: 122 RVA: 0x00007610 File Offset: 0x00005810
		public override void OnActiveSceneChanged(Scene from, Scene to)
		{
			if (!(to.name == "GameCore") && this.dataLoaded)
			{
				this.dataLoaded = false;
				this._cameraPlus.ThirdPersonPos = this._cameraPlus.Config.Position;
				this._cameraPlus.ThirdPersonRot = this._cameraPlus.Config.Rotation;
			}
			base.OnActiveSceneChanged(from, to);
		}

		// Token: 0x0600007B RID: 123 RVA: 0x0000767D File Offset: 0x0000587D
		public override void Shutdown()
		{
			Plugin instance = Plugin.Instance;
			instance.ActiveSceneChanged = (Action<Scene, Scene>)Delegate.Remove(instance.ActiveSceneChanged, new Action<Scene, Scene>(this.OnActiveSceneChanged));
			Object.Destroy(this);
		}
	}
}
