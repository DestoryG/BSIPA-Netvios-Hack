using System;
using System.Collections;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using IPA.Logging;
using LIV.SDK.Unity;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR;
using VRUIControls;

namespace CameraPlus
{
	// Token: 0x0200000B RID: 11
	public class CameraPlusBehaviour : MonoBehaviour
	{
		// Token: 0x17000011 RID: 17
		// (get) Token: 0x0600004A RID: 74 RVA: 0x00005BBF File Offset: 0x00003DBF
		// (set) Token: 0x0600004B RID: 75 RVA: 0x00005BC8 File Offset: 0x00003DC8
		public bool ThirdPerson
		{
			get
			{
				return this._thirdPerson;
			}
			set
			{
				this._thirdPerson = value;
				this._cameraCube.gameObject.SetActive(this._thirdPerson && this.Config.showThirdPersonCamera);
				this._cameraPreviewQuad.gameObject.SetActive(this._thirdPerson && this.Config.showThirdPersonCamera);
				if (value)
				{
					this._cam.cullingMask &= -65;
					this._cam.cullingMask |= 8;
					return;
				}
				this._cam.cullingMask &= -9;
				this._cam.cullingMask |= 64;
			}
		}

		// Token: 0x0600004C RID: 76 RVA: 0x00005C7C File Offset: 0x00003E7C
		public virtual void Init(Config config)
		{
			Object.DontDestroyOnLoad(base.gameObject);
			Logger.Log("Created new camera plus behaviour component!", Logger.Level.Info);
			this.Config = config;
			this._isMainCamera = Path.GetFileName(this.Config.FilePath) == Plugin.MainCamera + ".cfg";
			CameraPlusBehaviour._contextMenuEnabled = !Environment.CommandLine.Contains("fpfc");
			base.StartCoroutine(this.DelayedInit());
		}

		// Token: 0x0600004D RID: 77 RVA: 0x00005CF4 File Offset: 0x00003EF4
		protected IEnumerator DelayedInit()
		{
			yield return this._waitForMainCamera;
			this._mainCamera = Camera.main;
			if (CameraPlusBehaviour._contextMenu == null)
			{
				CameraPlusBehaviour.MenuObj = new GameObject("CameraPlusMenu");
				CameraPlusBehaviour._contextMenu = CameraPlusBehaviour.MenuObj.AddComponent<ContextMenu>();
			}
			XRSettings.showDeviceView = false;
			GameObject gameObject = Object.Instantiate<GameObject>(this._mainCamera.gameObject);
			this.Config.ConfigChangedEvent += this.PluginOnConfigChangedEvent;
			gameObject.SetActive(false);
			gameObject.name = "Camera Plus";
			gameObject.tag = "Untagged";
			while (gameObject.transform.childCount > 0)
			{
				Object.DestroyImmediate(gameObject.transform.GetChild(0).gameObject);
			}
			Object.DestroyImmediate(gameObject.GetComponent("AudioListener"));
			Object.DestroyImmediate(gameObject.GetComponent("MeshCollider"));
			this._cam = gameObject.GetComponent<Camera>();
			this._cam.stereoTargetEye = 0;
			this._cam.enabled = true;
			this._cam.name = Path.GetFileName(this.Config.FilePath);
			LIV component = this._cam.GetComponent<LIV>();
			if (component)
			{
				Object.Destroy(component);
			}
			this._screenCamera = new GameObject("Screen Camera").AddComponent<ScreenCameraBehaviour>();
			if (this._previewMaterial == null)
			{
				this._previewMaterial = new Material(Shader.Find("Hidden/BlitCopyWithDepth"));
			}
			gameObject.SetActive(true);
			Transform transform = this._mainCamera.transform;
			base.transform.position = transform.position;
			base.transform.rotation = transform.rotation;
			Logger.Log(string.Format("near clipplane \"{0}", Camera.main.nearClipPlane), Logger.Level.Info);
			gameObject.transform.parent = base.transform;
			gameObject.transform.localPosition = Vector3.zero;
			gameObject.transform.localRotation = Quaternion.identity;
			gameObject.transform.localScale = Vector3.one;
			this._cameraCubeGO = GameObject.CreatePrimitive(3);
			Object.DontDestroyOnLoad(this._cameraCubeGO);
			this._cameraCubeGO.SetActive(this.ThirdPerson);
			this._cameraCube = this._cameraCubeGO.transform;
			this._cameraCube.localScale = new Vector3(0.15f, 0.15f, 0.22f);
			this._cameraCube.name = "CameraCube";
			this._quad = GameObject.CreatePrimitive(5);
			Object.DontDestroyOnLoad(this._quad);
			Object.DestroyImmediate(this._quad.GetComponent<Collider>());
			this._quad.GetComponent<MeshRenderer>().material = this._previewMaterial;
			this._quad.transform.parent = this._cameraCube;
			this._quad.transform.localPosition = new Vector3(-1f * ((this._cam.aspect - 1f) / 2f + 1f), 0f, 0.22f);
			this._quad.transform.localEulerAngles = new Vector3(0f, 180f, 0f);
			this._quad.transform.localScale = new Vector3(this._cam.aspect, 1f, 1f);
			this._cameraPreviewQuad = this._quad;
			this.ReadConfig();
			if (this.ThirdPerson)
			{
				this.ThirdPersonPos = this.Config.Position;
				this.ThirdPersonRot = this.Config.Rotation;
				base.transform.position = this.ThirdPersonPos;
				base.transform.eulerAngles = this.ThirdPersonRot;
				this._cameraCube.position = this.ThirdPersonPos;
				this._cameraCube.eulerAngles = this.ThirdPersonRot;
			}
			if (this.Config.movementScriptPath != string.Empty)
			{
				this.AddMovementScript();
			}
			this.SetCullingMask();
			CameraMovement.CreateExampleScript();
			Plugin instance = Plugin.Instance;
			instance.ActiveSceneChanged = (Action<Scene, Scene>)Delegate.Combine(instance.ActiveSceneChanged, new Action<Scene, Scene>(this.SceneManager_activeSceneChanged));
			this.SceneManager_activeSceneChanged(default(Scene), default(Scene));
			Logger.Log("Camera \"" + Path.GetFileName(this.Config.FilePath) + "\" successfully initialized! " + Convert.ToString(this._cam.cullingMask, 16), Logger.Level.Info);
			yield break;
		}

		// Token: 0x0600004E RID: 78 RVA: 0x00005D04 File Offset: 0x00003F04
		protected virtual void OnDestroy()
		{
			this.Config.ConfigChangedEvent -= this.PluginOnConfigChangedEvent;
			Plugin instance = Plugin.Instance;
			instance.ActiveSceneChanged = (Action<Scene, Scene>)Delegate.Remove(instance.ActiveSceneChanged, new Action<Scene, Scene>(this.SceneManager_activeSceneChanged));
			CameraMovement cameraMovement = this._cameraMovement;
			if (cameraMovement != null)
			{
				cameraMovement.Shutdown();
			}
			this.CloseContextMenu();
			this._camRenderTexture.Release();
			if (this._screenCamera)
			{
				Object.Destroy(this._screenCamera.gameObject);
			}
			if (this._cameraCubeGO)
			{
				Object.Destroy(this._cameraCubeGO);
			}
			if (this._quad)
			{
				Object.Destroy(this._quad);
			}
		}

		// Token: 0x0600004F RID: 79 RVA: 0x00005DBF File Offset: 0x00003FBF
		protected virtual void PluginOnConfigChangedEvent(Config config)
		{
			this.ReadConfig();
		}

		// Token: 0x06000050 RID: 80 RVA: 0x00005DC8 File Offset: 0x00003FC8
		protected virtual void ReadConfig()
		{
			this.ThirdPerson = this.Config.thirdPerson;
			if (!this.ThirdPerson)
			{
				base.transform.position = this._mainCamera.transform.position;
				base.transform.rotation = this._mainCamera.transform.rotation;
			}
			else
			{
				this.ThirdPersonPos = this.Config.Position;
				this.ThirdPersonRot = this.Config.Rotation;
			}
			this.SetCullingMask();
			this.CreateScreenRenderTexture();
			this.SetFOV();
		}

		// Token: 0x06000051 RID: 81 RVA: 0x00005E5A File Offset: 0x0000405A
		internal virtual void CreateScreenRenderTexture()
		{
			PersistentSingleton<HMMainThreadDispatcher>.instance.Enqueue(delegate
			{
				bool flag = false;
				if (this._camRenderTexture == null)
				{
					this._camRenderTexture = new RenderTexture(1, 1, 24);
					flag = true;
				}
				else if (this.Config.fitToCanvas != this._prevFitToCanvas || this.Config.antiAliasing != this._prevAA || this.Config.screenPosX != this._prevScreenPosX || this.Config.screenPosY != this._prevScreenPosY || this.Config.renderScale != this._prevRenderScale || this.Config.screenHeight != this._prevScreenHeight || this.Config.screenWidth != this._prevScreenWidth || this.Config.layer != this._prevLayer)
				{
					flag = true;
					this._cam.targetTexture = null;
					this._screenCamera.SetRenderTexture(null);
					this._screenCamera.SetCameraInfo(new Vector2(0f, 0f), new Vector2(0f, 0f), -1000);
					this._camRenderTexture.Release();
				}
				if (!flag)
				{
					return;
				}
				if (this.Config.fitToCanvas)
				{
					this.Config.screenPosX = 0;
					this.Config.screenPosY = 0;
					this.Config.screenWidth = Screen.width;
					this.Config.screenHeight = Screen.height;
				}
				this._lastRenderUpdate = DateTime.Now;
				this._camRenderTexture.width = Mathf.Clamp(Mathf.RoundToInt((float)this.Config.screenWidth * this.Config.renderScale), 1, int.MaxValue);
				this._camRenderTexture.height = Mathf.Clamp(Mathf.RoundToInt((float)this.Config.screenHeight * this.Config.renderScale), 1, int.MaxValue);
				this._camRenderTexture.useDynamicScale = false;
				this._camRenderTexture.antiAliasing = this.Config.antiAliasing;
				this._camRenderTexture.Create();
				this._cam.targetTexture = this._camRenderTexture;
				this._previewMaterial.SetTexture("_MainTex", this._camRenderTexture);
				this._screenCamera.SetRenderTexture(this._camRenderTexture);
				this._screenCamera.SetCameraInfo(this.Config.ScreenPosition, this.Config.ScreenSize, this.Config.layer);
				this._prevFitToCanvas = this.Config.fitToCanvas;
				this._prevAA = this.Config.antiAliasing;
				this._prevRenderScale = this.Config.renderScale;
				this._prevScreenHeight = this.Config.screenHeight;
				this._prevScreenWidth = this.Config.screenWidth;
				this._prevLayer = this.Config.layer;
				this._prevScreenPosX = this.Config.screenPosX;
				this._prevScreenPosY = this.Config.screenPosY;
			});
		}

		// Token: 0x06000052 RID: 82 RVA: 0x00005E74 File Offset: 0x00004074
		public virtual void SceneManager_activeSceneChanged(Scene from, Scene to)
		{
			base.StartCoroutine(this.GetMainCamera());
			base.StartCoroutine(this.Get360Managers());
			VRPointer[] array = ((to.name == "GameCore") ? Resources.FindObjectsOfTypeAll<VRPointer>() : Resources.FindObjectsOfTypeAll<VRPointer>());
			if (array.Count<VRPointer>() == 0)
			{
				Logger.Log("Failed to get VRPointer!", Logger.Level.Warning);
				return;
			}
			VRPointer vrpointer = ((to.name != "GameCore") ? array.First<VRPointer>() : array.Last<VRPointer>());
			if (this._moverPointer)
			{
				Object.Destroy(this._moverPointer);
			}
			this._moverPointer = vrpointer.gameObject.AddComponent<CameraMoverPointer>();
			this._moverPointer.Init(this, this._cameraCube);
		}

		// Token: 0x06000053 RID: 83
		[DllImport("user32.dll")]
		private static extern IntPtr GetActiveWindow();

		// Token: 0x06000054 RID: 84 RVA: 0x00003561 File Offset: 0x00001761
		protected void OnApplicationFocus(bool hasFocus)
		{
		}

		// Token: 0x06000055 RID: 85 RVA: 0x00005F2C File Offset: 0x0000412C
		protected virtual void Update()
		{
			if (this._isMainCamera && Input.GetKeyDown(282))
			{
				this.ThirdPerson = !this.ThirdPerson;
				if (!this.ThirdPerson)
				{
					base.transform.position = this._mainCamera.transform.position;
					base.transform.rotation = this._mainCamera.transform.rotation;
				}
				else
				{
					this.ThirdPersonPos = this.Config.Position;
					this.ThirdPersonRot = this.Config.Rotation;
				}
				this.Config.thirdPerson = this.ThirdPerson;
				this.Config.Save();
			}
			this.HandleMouseEvents();
		}

		// Token: 0x06000056 RID: 86 RVA: 0x00005FE8 File Offset: 0x000041E8
		protected virtual void LateUpdate()
		{
			try
			{
				Transform transform = this._mainCamera.transform;
				if (this.ThirdPerson)
				{
					this.HandleThirdPerson360();
					base.transform.position = this.ThirdPersonPos;
					base.transform.eulerAngles = this.ThirdPersonRot;
					this._cameraCube.position = this.ThirdPersonPos;
					this._cameraCube.eulerAngles = this.ThirdPersonRot;
				}
				else
				{
					base.transform.position = Vector3.Lerp(base.transform.position, transform.position + this.Config.FirstPersonPositionOffset, this.Config.positionSmooth * Time.unscaledDeltaTime);
					if (!this.Config.forceFirstPersonUpRight)
					{
						base.transform.rotation = Quaternion.Slerp(base.transform.rotation, transform.rotation * Quaternion.Euler(this.Config.FirstPersonRotationOffset), this.Config.rotationSmooth * Time.unscaledDeltaTime);
					}
					else
					{
						Quaternion quaternion = Quaternion.Slerp(base.transform.rotation, transform.rotation * Quaternion.Euler(this.Config.FirstPersonRotationOffset), this.Config.rotationSmooth * Time.unscaledDeltaTime);
						base.transform.rotation = quaternion * Quaternion.Euler(0f, 0f, -quaternion.eulerAngles.z);
					}
				}
			}
			catch
			{
			}
		}

		// Token: 0x06000057 RID: 87 RVA: 0x0000617C File Offset: 0x0000437C
		private void HandleThirdPerson360()
		{
			if (!this._beatLineManager || !this.Config.use360Camera || !this._environmentSpawnRotation)
			{
				return;
			}
			float num5;
			if (this._beatLineManager.isMidRotationValid)
			{
				double num = (double)this._beatLineManager.midRotation;
				float num2 = Mathf.DeltaAngle((float)num, this._environmentSpawnRotation.targetRotation);
				float num3 = (float)(-(float)((double)this._beatLineManager.rotationRange) * 0.5);
				float num4 = this._beatLineManager.rotationRange * 0.5f;
				if ((double)num2 > (double)num4)
				{
					num4 = num2;
				}
				else if ((double)num2 < (double)num3)
				{
					num3 = num2;
				}
				num5 = (float)(num + ((double)num3 + (double)num4) * 0.5);
			}
			else
			{
				num5 = this._environmentSpawnRotation.targetRotation;
			}
			if (this.Config.cam360RotateControlNew)
			{
				this._yAngle = Mathf.LerpAngle(this._yAngle, num5, Mathf.Clamp(Time.deltaTime * this.Config.cam360Smoothness, 0f, 1f));
			}
			else
			{
				this._yAngle = Mathf.Lerp(this._yAngle, num5, Mathf.Clamp(Time.deltaTime * this.Config.cam360Smoothness, 0f, 1f));
			}
			this.ThirdPersonRot = new Vector3(this.Config.cam360XTilt, this._yAngle + this.Config.cam360YTilt, this.Config.cam360ZTilt);
			this.ThirdPersonPos = base.transform.forward * this.Config.cam360ForwardOffset + base.transform.right * this.Config.cam360RightOffset;
			this.ThirdPersonPos = new Vector3(this.ThirdPersonPos.x, this.Config.cam360UpOffset, this.ThirdPersonPos.z);
		}

		// Token: 0x06000058 RID: 88 RVA: 0x00006350 File Offset: 0x00004550
		protected void AddMovementScript()
		{
			if (this.Config.movementScriptPath != string.Empty)
			{
				if (this._cameraMovement)
				{
					this._cameraMovement.Shutdown();
				}
				if (this.Config.movementScriptPath == "SongMovementScript")
				{
					this._cameraMovement = this._cam.gameObject.AddComponent<SongCameraMovement>();
				}
				else
				{
					if (!File.Exists(this.Config.movementScriptPath))
					{
						return;
					}
					this._cameraMovement = this._cam.gameObject.AddComponent<CameraMovement>();
				}
				if (this._cameraMovement.Init(this))
				{
					this.ThirdPersonPos = this.Config.Position;
					this.ThirdPersonRot = this.Config.Rotation;
					this.Config.thirdPerson = true;
					this.ThirdPerson = true;
					this.CreateScreenRenderTexture();
				}
			}
		}

		// Token: 0x06000059 RID: 89 RVA: 0x00006432 File Offset: 0x00004632
		protected IEnumerator GetMainCamera()
		{
			yield return this._waitForMainCamera;
			this._mainCamera = Camera.main;
			yield break;
		}

		// Token: 0x0600005A RID: 90 RVA: 0x00006441 File Offset: 0x00004641
		protected IEnumerator Get360Managers()
		{
			yield return new WaitForSeconds(0.5f);
			this._beatLineManager = null;
			this._environmentSpawnRotation = null;
			BeatLineManager[] array = Resources.FindObjectsOfTypeAll<BeatLineManager>();
			if (array.Length != 0)
			{
				this._beatLineManager = array.FirstOrDefault<BeatLineManager>();
				this._environmentSpawnRotation = Resources.FindObjectsOfTypeAll<EnvironmentSpawnRotation>().FirstOrDefault<EnvironmentSpawnRotation>();
			}
			if (this._beatLineManager)
			{
				this._yAngle = this._beatLineManager.midRotation;
			}
			yield break;
		}

		// Token: 0x0600005B RID: 91 RVA: 0x00006450 File Offset: 0x00004650
		internal virtual void SetFOV()
		{
			if (this._cam == null)
			{
				return;
			}
			this._cam.fieldOfView = this.Config.fov;
		}

		// Token: 0x0600005C RID: 92 RVA: 0x00006478 File Offset: 0x00004678
		internal virtual void SetCullingMask()
		{
			this._cam.cullingMask = Camera.main.cullingMask;
			if (this.Config.transparentWalls)
			{
				this._cam.cullingMask &= ~(1 << TransparentWallsPatch.WallLayerMask);
			}
			else
			{
				this._cam.cullingMask |= 1 << TransparentWallsPatch.WallLayerMask;
			}
			if (this.Config.avatar)
			{
				if (this.Config.thirdPerson || this.Config.use360Camera)
				{
					this._cam.cullingMask |= 8;
					this._cam.cullingMask &= -65;
				}
				else
				{
					this._cam.cullingMask |= 64;
					this._cam.cullingMask &= -9;
				}
				this._cam.cullingMask |= 1024;
			}
			else
			{
				this._cam.cullingMask &= -9;
				this._cam.cullingMask &= -65;
				this._cam.cullingMask &= -1025;
			}
			if (this.Config.debri != "link")
			{
				if (this.Config.debri == "show")
				{
					this._cam.cullingMask |= 512;
					return;
				}
				this._cam.cullingMask &= -513;
			}
		}

		// Token: 0x0600005D RID: 93 RVA: 0x00006614 File Offset: 0x00004814
		public bool IsWithinRenderArea(Vector2 mousePos, Config c)
		{
			return mousePos.x >= (float)c.screenPosX && mousePos.x <= (float)(c.screenPosX + c.screenWidth) && mousePos.y >= (float)c.screenPosY && mousePos.y <= (float)(c.screenPosY + c.screenHeight);
		}

		// Token: 0x0600005E RID: 94 RVA: 0x00006674 File Offset: 0x00004874
		public bool IsTopmostRenderAreaAtPos(Vector2 mousePos)
		{
			if (!this.IsWithinRenderArea(mousePos, this.Config))
			{
				return false;
			}
			foreach (CameraPlusInstance cameraPlusInstance in Plugin.Instance.Cameras.Values.ToArray<CameraPlusInstance>())
			{
				if (!(cameraPlusInstance.Instance == this) && (this.IsWithinRenderArea(mousePos, cameraPlusInstance.Config) || cameraPlusInstance.Instance._mouseHeld))
				{
					if (cameraPlusInstance.Config.layer > this.Config.layer)
					{
						return false;
					}
					if (cameraPlusInstance.Config.layer == this.Config.layer && cameraPlusInstance.Instance._lastRenderUpdate > this._lastRenderUpdate)
					{
						return false;
					}
					if (cameraPlusInstance.Instance._mouseHeld && (cameraPlusInstance.Instance._isMoving || cameraPlusInstance.Instance._isResizing || cameraPlusInstance.Instance._contextMenuOpen))
					{
						return false;
					}
				}
			}
			return true;
		}

		// Token: 0x0600005F RID: 95 RVA: 0x00006774 File Offset: 0x00004974
		public static CameraPlusBehaviour GetTopmostInstanceAtCursorPos()
		{
			foreach (CameraPlusInstance cameraPlusInstance in Plugin.Instance.Cameras.Values.ToArray<CameraPlusInstance>())
			{
				if (cameraPlusInstance.Instance._isTopmostAtCursorPos)
				{
					return cameraPlusInstance.Instance;
				}
			}
			return null;
		}

		// Token: 0x06000060 RID: 96 RVA: 0x000067BD File Offset: 0x000049BD
		internal void CloseContextMenu()
		{
			CameraPlusBehaviour._contextMenu.DisableMenu();
			Object.Destroy(CameraPlusBehaviour.MenuObj);
			this._contextMenuOpen = false;
		}

		// Token: 0x06000061 RID: 97 RVA: 0x000067DC File Offset: 0x000049DC
		public static void SetCursor(CameraPlusBehaviour.CursorType type)
		{
			if (type != CameraPlusBehaviour.currentCursor)
			{
				Texture2D texture2D = null;
				switch (type)
				{
				case CameraPlusBehaviour.CursorType.Horizontal:
					texture2D = Utils.LoadTextureFromResources("CameraPlus.Resources.Resize_Horiz.png");
					break;
				case CameraPlusBehaviour.CursorType.Vertical:
					texture2D = Utils.LoadTextureFromResources("CameraPlus.Resources.Resize_Vert.png");
					break;
				case CameraPlusBehaviour.CursorType.DiagonalLeft:
					texture2D = Utils.LoadTextureFromResources("CameraPlus.Resources.Resize_DiagLeft.png");
					break;
				case CameraPlusBehaviour.CursorType.DiagonalRight:
					texture2D = Utils.LoadTextureFromResources("CameraPlus.Resources.Resize_DiagRight.png");
					break;
				}
				Cursor.SetCursor(texture2D, texture2D ? new Vector2((float)(texture2D.width / 2), (float)(texture2D.height / 2)) : new Vector2(0f, 0f), 0);
				CameraPlusBehaviour.currentCursor = type;
			}
		}

		// Token: 0x06000062 RID: 98 RVA: 0x00006880 File Offset: 0x00004A80
		protected void HandleMouseEvents()
		{
			bool mouseButton = Input.GetMouseButton(0);
			bool mouseButton2 = Input.GetMouseButton(1);
			Vector3 mousePosition = Input.mousePosition;
			if (!this._mouseHeld && (mouseButton || mouseButton2) && mousePosition.x > 0f && mousePosition.x < (float)Screen.width && mousePosition.y > 0f)
			{
				float y = mousePosition.y;
				float num = (float)Screen.height;
			}
			this._isTopmostAtCursorPos = this.IsTopmostRenderAreaAtPos(mousePosition);
			if (!this._mouseHeld && !this._isTopmostAtCursorPos)
			{
				return;
			}
			int num2 = 5;
			bool flag = Utils.WithinRange((int)mousePosition.x, -num2, num2) || Utils.WithinRange((int)mousePosition.y, -num2, num2) || Utils.WithinRange((int)mousePosition.x, this.Config.screenPosX + this.Config.screenWidth - num2, this.Config.screenPosX + this.Config.screenWidth + num2) || Utils.WithinRange((int)mousePosition.x, this.Config.screenPosX - num2, this.Config.screenPosX + num2) || Utils.WithinRange((int)mousePosition.y, this.Config.screenPosY + this.Config.screenHeight - num2, this.Config.screenPosY + this.Config.screenHeight + num2) || Utils.WithinRange((int)mousePosition.y, this.Config.screenPosY - num2, this.Config.screenPosY + num2);
			float num3 = mousePosition.x - (float)this.Config.screenPosX;
			float num4 = mousePosition.y - (float)this.Config.screenPosY;
			if (!this._mouseHeld)
			{
				if (flag)
				{
					bool flag2 = num3 <= (float)(this.Config.screenWidth / 2);
					bool flag3 = num4 <= (float)(this.Config.screenHeight / 2);
					int num5 = this.Config.screenPosX + this.Config.screenWidth / 2;
					int num6 = this.Config.screenPosY + this.Config.screenHeight / 2;
					int num7 = this.Config.screenWidth / 2 - num2;
					int num8 = this.Config.screenHeight / 2 - num2;
					this._xAxisLocked = Utils.WithinRange((int)mousePosition.x, num5 - num7 + 1, num5 + num7 - 1);
					this._yAxisLocked = Utils.WithinRange((int)mousePosition.y, num6 - num8 + 1, num6 + num8 - 1);
					if (!this.Config.fitToCanvas)
					{
						if (this._xAxisLocked)
						{
							CameraPlusBehaviour.SetCursor(CameraPlusBehaviour.CursorType.Vertical);
						}
						else if (this._yAxisLocked)
						{
							CameraPlusBehaviour.SetCursor(CameraPlusBehaviour.CursorType.Horizontal);
						}
						else if ((flag2 && flag3) || (!flag2 && !flag3))
						{
							CameraPlusBehaviour.SetCursor(CameraPlusBehaviour.CursorType.DiagonalLeft);
						}
						else if ((flag2 && !flag3) || (!flag2 && flag3))
						{
							CameraPlusBehaviour.SetCursor(CameraPlusBehaviour.CursorType.DiagonalRight);
						}
					}
					CameraPlusBehaviour.wasWithinBorder = true;
				}
				else if (!flag && CameraPlusBehaviour.wasWithinBorder)
				{
					CameraPlusBehaviour.SetCursor(CameraPlusBehaviour.CursorType.None);
					CameraPlusBehaviour.wasWithinBorder = false;
				}
			}
			if (mouseButton && !this.Config.fitToCanvas)
			{
				if (!this._mouseHeld)
				{
					this._initialOffset.x = num3;
					this._initialOffset.y = num4;
					this._lastScreenPos = this.Config.ScreenPosition;
					this._lastGrabPos = new Vector2(mousePosition.x, mousePosition.y);
					this._isLeft = this._initialOffset.x <= (float)(this.Config.screenWidth / 2);
					this._isBottom = this._initialOffset.y <= (float)(this.Config.screenHeight / 2);
					CameraPlusBehaviour.anyInstanceBusy = true;
				}
				this._mouseHeld = true;
				if (!this._isMoving && (this._isResizing || flag))
				{
					this._isResizing = true;
					if (!this._xAxisLocked)
					{
						int num9 = (this._isLeft ? ((int)(this._lastGrabPos.x - mousePosition.x)) : ((int)(mousePosition.x - this._lastGrabPos.x)));
						this.Config.screenWidth += num9;
						this.Config.screenPosX = (int)this._lastScreenPos.x - (this._isLeft ? num9 : 0);
					}
					if (!this._yAxisLocked)
					{
						int num10 = (this._isBottom ? ((int)(mousePosition.y - this._lastGrabPos.y)) : ((int)(this._lastGrabPos.y - mousePosition.y)));
						this.Config.screenHeight -= num10;
						this.Config.screenPosY = (int)this._lastScreenPos.y + (this._isBottom ? num10 : 0);
					}
					this._lastGrabPos = mousePosition;
					this._lastScreenPos = this.Config.ScreenPosition;
				}
				else
				{
					this._isMoving = true;
					this.Config.screenPosX = (int)mousePosition.x - (int)this._initialOffset.x;
					this.Config.screenPosY = (int)mousePosition.y - (int)this._initialOffset.y;
				}
				this.Config.screenWidth = Mathf.Clamp(this.Config.screenWidth, 100, Screen.width);
				this.Config.screenHeight = Mathf.Clamp(this.Config.screenHeight, 100, Screen.height);
				this.Config.screenPosX = Mathf.Clamp(this.Config.screenPosX, 0, Screen.width - this.Config.screenWidth);
				this.Config.screenPosY = Mathf.Clamp(this.Config.screenPosY, 0, Screen.height - this.Config.screenHeight);
				this.CreateScreenRenderTexture();
				return;
			}
			if (!mouseButton2 || !CameraPlusBehaviour._contextMenuEnabled)
			{
				if (this._isResizing || this._isMoving || this._mouseHeld)
				{
					if (!this._contextMenuOpen && !this._isCameraDestroyed)
					{
						this.Config.Save();
					}
					this._isResizing = false;
					this._isMoving = false;
					this._mouseHeld = false;
					CameraPlusBehaviour.anyInstanceBusy = false;
				}
				return;
			}
			if (this._mouseHeld)
			{
				return;
			}
			this.DisplayContextMenu();
			this._contextMenuOpen = true;
			CameraPlusBehaviour.anyInstanceBusy = true;
			this._mouseHeld = true;
		}

		// Token: 0x06000063 RID: 99 RVA: 0x00006EC8 File Offset: 0x000050C8
		private void DisplayContextMenu()
		{
			if (CameraPlusBehaviour._contextMenu == null)
			{
				CameraPlusBehaviour.MenuObj = new GameObject("CameraPlusMenu");
				CameraPlusBehaviour._contextMenu = CameraPlusBehaviour.MenuObj.AddComponent<ContextMenu>();
			}
			CameraPlusBehaviour._contextMenu.EnableMenu(Input.mousePosition, this);
		}

		// Token: 0x0400004B RID: 75
		protected readonly WaitUntil _waitForMainCamera = new WaitUntil(() => Camera.main);

		// Token: 0x0400004C RID: 76
		private readonly WaitForSecondsRealtime _waitForSecondsRealtime = new WaitForSecondsRealtime(1f);

		// Token: 0x0400004D RID: 77
		protected const int OnlyInThirdPerson = 3;

		// Token: 0x0400004E RID: 78
		protected const int OnlyInFirstPerson = 6;

		// Token: 0x0400004F RID: 79
		protected const int NotesDebriLayer = 9;

		// Token: 0x04000050 RID: 80
		protected const int AlwaysVisible = 10;

		// Token: 0x04000051 RID: 81
		protected bool _thirdPerson;

		// Token: 0x04000052 RID: 82
		public Vector3 ThirdPersonPos;

		// Token: 0x04000053 RID: 83
		public Vector3 ThirdPersonRot;

		// Token: 0x04000054 RID: 84
		public Config Config;

		// Token: 0x04000055 RID: 85
		protected RenderTexture _camRenderTexture;

		// Token: 0x04000056 RID: 86
		protected Material _previewMaterial;

		// Token: 0x04000057 RID: 87
		protected Camera _cam;

		// Token: 0x04000058 RID: 88
		protected Transform _cameraCube;

		// Token: 0x04000059 RID: 89
		protected ScreenCameraBehaviour _screenCamera;

		// Token: 0x0400005A RID: 90
		protected GameObject _cameraPreviewQuad;

		// Token: 0x0400005B RID: 91
		protected Camera _mainCamera;

		// Token: 0x0400005C RID: 92
		protected CameraMoverPointer _moverPointer;

		// Token: 0x0400005D RID: 93
		protected GameObject _cameraCubeGO;

		// Token: 0x0400005E RID: 94
		protected GameObject _quad;

		// Token: 0x0400005F RID: 95
		protected CameraMovement _cameraMovement;

		// Token: 0x04000060 RID: 96
		protected BeatLineManager _beatLineManager;

		// Token: 0x04000061 RID: 97
		protected EnvironmentSpawnRotation _environmentSpawnRotation;

		// Token: 0x04000062 RID: 98
		protected int _prevScreenWidth;

		// Token: 0x04000063 RID: 99
		protected int _prevScreenHeight;

		// Token: 0x04000064 RID: 100
		protected int _prevAA;

		// Token: 0x04000065 RID: 101
		protected float _prevRenderScale;

		// Token: 0x04000066 RID: 102
		protected int _prevLayer;

		// Token: 0x04000067 RID: 103
		protected int _prevScreenPosX;

		// Token: 0x04000068 RID: 104
		protected int _prevScreenPosY;

		// Token: 0x04000069 RID: 105
		protected bool _prevFitToCanvas;

		// Token: 0x0400006A RID: 106
		protected float _aspectRatio;

		// Token: 0x0400006B RID: 107
		protected float _yAngle;

		// Token: 0x0400006C RID: 108
		protected bool _wasWindowActive;

		// Token: 0x0400006D RID: 109
		protected bool _mouseHeld;

		// Token: 0x0400006E RID: 110
		protected bool _isResizing;

		// Token: 0x0400006F RID: 111
		protected bool _isMoving;

		// Token: 0x04000070 RID: 112
		protected bool _xAxisLocked;

		// Token: 0x04000071 RID: 113
		protected bool _yAxisLocked;

		// Token: 0x04000072 RID: 114
		protected bool _contextMenuOpen;

		// Token: 0x04000073 RID: 115
		internal bool _isCameraDestroyed;

		// Token: 0x04000074 RID: 116
		protected bool _isMainCamera;

		// Token: 0x04000075 RID: 117
		protected bool _isTopmostAtCursorPos;

		// Token: 0x04000076 RID: 118
		protected DateTime _lastRenderUpdate;

		// Token: 0x04000077 RID: 119
		protected Vector2 _initialOffset = new Vector2(0f, 0f);

		// Token: 0x04000078 RID: 120
		protected Vector2 _lastGrabPos = new Vector2(0f, 0f);

		// Token: 0x04000079 RID: 121
		protected Vector2 _lastScreenPos;

		// Token: 0x0400007A RID: 122
		protected bool _isBottom;

		// Token: 0x0400007B RID: 123
		protected bool _isLeft;

		// Token: 0x0400007C RID: 124
		protected static GameObject MenuObj = null;

		// Token: 0x0400007D RID: 125
		protected static ContextMenu _contextMenu = null;

		// Token: 0x0400007E RID: 126
		public static CameraPlusBehaviour.CursorType currentCursor = CameraPlusBehaviour.CursorType.None;

		// Token: 0x0400007F RID: 127
		public static bool wasWithinBorder = false;

		// Token: 0x04000080 RID: 128
		public static bool anyInstanceBusy = false;

		// Token: 0x04000081 RID: 129
		private static bool _contextMenuEnabled = true;

		// Token: 0x02000026 RID: 38
		public enum CursorType
		{
			// Token: 0x040000CB RID: 203
			None,
			// Token: 0x040000CC RID: 204
			Horizontal,
			// Token: 0x040000CD RID: 205
			Vertical,
			// Token: 0x040000CE RID: 206
			DiagonalLeft,
			// Token: 0x040000CF RID: 207
			DiagonalRight
		}
	}
}
