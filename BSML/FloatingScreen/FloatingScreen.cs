using System;
using System.Linq;
using HMUI;
using IPA.Utilities;
using UnityEngine;
using UnityEngine.UI;
using VRUIControls;

namespace BeatSaberMarkupLanguage.FloatingScreen
{
	// Token: 0x02000092 RID: 146
	public class FloatingScreen : Screen
	{
		// Token: 0x170000CC RID: 204
		// (get) Token: 0x060002D0 RID: 720 RVA: 0x0000DFB6 File Offset: 0x0000C1B6
		// (set) Token: 0x060002D1 RID: 721 RVA: 0x0000DFC8 File Offset: 0x0000C1C8
		public Vector2 ScreenSize
		{
			get
			{
				return (base.transform as RectTransform).sizeDelta;
			}
			set
			{
				(base.transform as RectTransform).sizeDelta = value;
				this.UpdateHandle();
			}
		}

		// Token: 0x170000CD RID: 205
		// (get) Token: 0x060002D2 RID: 722 RVA: 0x0000DFE1 File Offset: 0x0000C1E1
		// (set) Token: 0x060002D3 RID: 723 RVA: 0x0000DFEE File Offset: 0x0000C1EE
		public Vector3 ScreenPosition
		{
			get
			{
				return base.transform.position;
			}
			set
			{
				(base.transform as RectTransform).position = value;
			}
		}

		// Token: 0x170000CE RID: 206
		// (get) Token: 0x060002D4 RID: 724 RVA: 0x0000E001 File Offset: 0x0000C201
		// (set) Token: 0x060002D5 RID: 725 RVA: 0x0000E013 File Offset: 0x0000C213
		public Quaternion ScreenRotation
		{
			get
			{
				return (base.transform as RectTransform).rotation;
			}
			set
			{
				(base.transform as RectTransform).rotation = value;
			}
		}

		// Token: 0x170000CF RID: 207
		// (get) Token: 0x060002D6 RID: 726 RVA: 0x0000E026 File Offset: 0x0000C226
		// (set) Token: 0x060002D7 RID: 727 RVA: 0x0000E030 File Offset: 0x0000C230
		public bool ShowHandle
		{
			get
			{
				return this._showHandle;
			}
			set
			{
				this._showHandle = value;
				if (!this._showHandle)
				{
					if (!this._showHandle && this.handle != null)
					{
						this.handle.SetActive(false);
					}
					return;
				}
				if (this.handle == null)
				{
					this.CreateHandle();
					return;
				}
				this.handle.SetActive(true);
			}
		}

		// Token: 0x170000D0 RID: 208
		// (get) Token: 0x060002D8 RID: 728 RVA: 0x0000E090 File Offset: 0x0000C290
		// (set) Token: 0x060002D9 RID: 729 RVA: 0x0000E098 File Offset: 0x0000C298
		public FloatingScreen.Side HandleSide
		{
			get
			{
				return this._handleSide;
			}
			set
			{
				this._handleSide = value;
				this.UpdateHandle();
			}
		}

		// Token: 0x060002DA RID: 730 RVA: 0x0000E0A8 File Offset: 0x0000C2A8
		public static FloatingScreen CreateFloatingScreen(Vector2 screenSize, bool createHandle, Vector3 position, Quaternion rotation)
		{
			FloatingScreen component = new GameObject("BSMLFloatingScreen", new Type[]
			{
				typeof(FloatingScreen),
				typeof(CanvasScaler),
				typeof(RectMask2D),
				typeof(Image),
				typeof(VRGraphicRaycaster),
				typeof(SetMainCameraToCanvas)
			}).GetComponent<FloatingScreen>();
			Canvas component2 = component.GetComponent<Canvas>();
			component2.additionalShaderChannels = 3;
			component2.sortingOrder = 4;
			CanvasScaler component3 = component.GetComponent<CanvasScaler>();
			component3.dynamicPixelsPerUnit = 3.44f;
			component3.referencePixelsPerUnit = 10f;
			Image component4 = component.GetComponent<Image>();
			component4.sprite = Resources.FindObjectsOfTypeAll<Sprite>().First((Sprite x) => x.name == "MainScreenMask");
			component4.type = 1;
			component4.color = new Color(0.7450981f, 0.7450981f, 0.7450981f, 1f);
			component4.material = Resources.FindObjectsOfTypeAll<Material>().First((Material x) => x.name == "UIFogBG");
			component4.preserveAspect = true;
			SetMainCameraToCanvas component5 = component.GetComponent<SetMainCameraToCanvas>();
			component5.SetField("_canvas", component2);
			component5.SetField("_mainCamera", Resources.FindObjectsOfTypeAll<MainCamera>().FirstOrDefault(delegate(MainCamera camera)
			{
				Camera camera2 = camera.camera;
				return camera2 == null || camera2.stereoTargetEye > 0;
			}) ?? Resources.FindObjectsOfTypeAll<MainCamera>().FirstOrDefault<MainCamera>());
			component.ScreenSize = screenSize;
			component.transform.localScale = new Vector3(0.02f, 0.02f, 0.02f);
			component.ShowHandle = createHandle;
			component.transform.position = position;
			component.transform.rotation = rotation;
			return component;
		}

		// Token: 0x060002DB RID: 731 RVA: 0x0000E270 File Offset: 0x0000C470
		private void CreateHandle()
		{
			VRPointer[] array = Resources.FindObjectsOfTypeAll<VRPointer>();
			if (array.Count<VRPointer>() != 0)
			{
				VRPointer vrpointer = array.First<VRPointer>();
				if (this.screenMover)
				{
					Object.Destroy(this.screenMover);
				}
				this.screenMover = vrpointer.gameObject.AddComponent<FloatingScreenMoverPointer>();
				this.handle = GameObject.CreatePrimitive(3);
				this.handle.transform.SetParent(base.transform);
				this.UpdateHandle();
				this.screenMover.Init(this);
				return;
			}
			Logger.log.Warn("Failed to get VRPointer!");
		}

		// Token: 0x060002DC RID: 732 RVA: 0x0000E300 File Offset: 0x0000C500
		public void UpdateHandle()
		{
			if (this.handle == null)
			{
				return;
			}
			switch (this.HandleSide)
			{
			case FloatingScreen.Side.Left:
				this.handle.transform.localPosition = new Vector3(-this.ScreenSize.x / 2f, 0f, 0f);
				this.handle.transform.localScale = new Vector3(this.ScreenSize.x / 15f, this.ScreenSize.y * 0.8f, this.ScreenSize.x / 15f);
				return;
			case FloatingScreen.Side.Right:
				this.handle.transform.localPosition = new Vector3(this.ScreenSize.x / 2f, 0f, 0f);
				this.handle.transform.localScale = new Vector3(this.ScreenSize.x / 15f, this.ScreenSize.y * 0.8f, this.ScreenSize.x / 15f);
				return;
			case FloatingScreen.Side.Bottom:
				this.handle.transform.localPosition = new Vector3(0f, -this.ScreenSize.y / 2f, 0f);
				this.handle.transform.localScale = new Vector3(this.ScreenSize.x * 0.8f, this.ScreenSize.y / 15f, this.ScreenSize.y / 15f);
				return;
			case FloatingScreen.Side.Top:
				this.handle.transform.localPosition = new Vector3(0f, this.ScreenSize.y / 2f, 0f);
				this.handle.transform.localScale = new Vector3(this.ScreenSize.x * 0.8f, this.ScreenSize.y / 15f, this.ScreenSize.y / 15f);
				return;
			default:
				return;
			}
		}

		// Token: 0x040000A2 RID: 162
		public FloatingScreenMoverPointer screenMover;

		// Token: 0x040000A3 RID: 163
		public GameObject handle;

		// Token: 0x040000A4 RID: 164
		private bool _showHandle;

		// Token: 0x040000A5 RID: 165
		private FloatingScreen.Side _handleSide;

		// Token: 0x0200013B RID: 315
		public enum Side
		{
			// Token: 0x040002B5 RID: 693
			Left,
			// Token: 0x040002B6 RID: 694
			Right,
			// Token: 0x040002B7 RID: 695
			Bottom,
			// Token: 0x040002B8 RID: 696
			Top
		}
	}
}
