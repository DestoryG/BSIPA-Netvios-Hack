using System;
using UnityEngine;

namespace CameraPlus
{
	// Token: 0x02000005 RID: 5
	internal class ProfileChanger : MonoBehaviour
	{
		// Token: 0x06000013 RID: 19 RVA: 0x000028D4 File Offset: 0x00000AD4
		public void ProfileChange(string ProifileName)
		{
			CameraPlusBehaviour[] array = Resources.FindObjectsOfTypeAll<CameraPlusBehaviour>();
			for (int i = 0; i < array.Length; i++)
			{
				CameraUtilities.RemoveCamera(array[i], true);
			}
			foreach (CameraPlusInstance cameraPlusInstance in Plugin.Instance.Cameras.Values)
			{
				Object.Destroy(cameraPlusInstance.Instance.gameObject);
			}
			Plugin.Instance.Cameras.Clear();
			CameraProfiles.SetProfile(ProifileName);
			CameraUtilities.ReloadCameras();
		}
	}
}
