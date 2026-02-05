using System;
using System.Collections.Concurrent;
using IPA.Logging;
using UnityEngine;

namespace CameraPlus
{
	// Token: 0x02000008 RID: 8
	public class ContextMenu : MonoBehaviour
	{
		// Token: 0x1700000C RID: 12
		// (get) Token: 0x06000033 RID: 51 RVA: 0x000034DC File Offset: 0x000016DC
		internal Vector2 menuPos
		{
			get
			{
				return new Vector2(Mathf.Min(this.mousePosition.x / ((float)Screen.width / 1600f), (float)Screen.width * (0.80625f / ((float)Screen.width / 1600f))), Mathf.Min(((float)Screen.height - this.mousePosition.y) / ((float)Screen.height / 900f), (float)Screen.height * (0.5555556f / ((float)Screen.height / 900f))));
			}
		}

		// Token: 0x06000034 RID: 52 RVA: 0x00003561 File Offset: 0x00001761
		public void Awake()
		{
		}

		// Token: 0x06000035 RID: 53 RVA: 0x00003563 File Offset: 0x00001763
		public void EnableMenu(Vector2 mousePos, CameraPlusBehaviour parentBehaviour)
		{
			base.enabled = true;
			this.mousePosition = mousePos;
			this.showMenu = true;
			this.parentBehaviour = parentBehaviour;
			this.layoutMode = false;
			this.profileMode = false;
		}

		// Token: 0x06000036 RID: 54 RVA: 0x0000358F File Offset: 0x0000178F
		public void DisableMenu()
		{
			if (!this)
			{
				return;
			}
			base.enabled = false;
			this.showMenu = false;
		}

		// Token: 0x06000037 RID: 55 RVA: 0x000035A8 File Offset: 0x000017A8
		private void OnGUI()
		{
			if (this.showMenu)
			{
				float num = 1600f;
				float num2 = 900f;
				Vector3 vector;
				vector.x = (float)Screen.width / num;
				vector.y = (float)Screen.height / num2;
				vector.z = 1f;
				Matrix4x4 matrix = GUI.matrix;
				GUI.matrix = Matrix4x4.TRS(new Vector3(0f, 0f, 0f), Quaternion.identity, vector);
				GUI.Box(new Rect(this.menuPos.x - 5f, this.menuPos.y, 310f, 470f), "CameraPlus");
				GUI.Box(new Rect(this.menuPos.x - 5f, this.menuPos.y, 310f, 470f), "CameraPlus");
				GUI.Box(new Rect(this.menuPos.x - 5f, this.menuPos.y, 310f, 470f), "CameraPlus");
				if (!this.layoutMode && !this.profileMode)
				{
					if (GUI.Button(new Rect(this.menuPos.x, this.menuPos.y + 25f, 120f, 30f), new GUIContent("Add New Camera")))
					{
						ConcurrentDictionary<string, CameraPlusInstance> concurrentDictionary = Plugin.Instance.Cameras;
						lock (concurrentDictionary)
						{
							string nextCameraName = CameraUtilities.GetNextCameraName();
							Logger.Log("Adding new config with name " + nextCameraName + ".cfg", Logger.Level.Info);
							CameraUtilities.AddNewCamera(nextCameraName, null, false);
							CameraUtilities.ReloadCameras();
							this.parentBehaviour.CloseContextMenu();
						}
					}
					if (GUI.Button(new Rect(this.menuPos.x + 130f, this.menuPos.y + 25f, 170f, 30f), new GUIContent("Remove Selected Camera")))
					{
						ConcurrentDictionary<string, CameraPlusInstance> concurrentDictionary = Plugin.Instance.Cameras;
						lock (concurrentDictionary)
						{
							if (CameraUtilities.RemoveCamera(this.parentBehaviour, true))
							{
								this.parentBehaviour._isCameraDestroyed = true;
								this.parentBehaviour.CreateScreenRenderTexture();
								this.parentBehaviour.CloseContextMenu();
								Logger.Log("Camera removed!", Logger.Level.Notice);
							}
						}
					}
					if (GUI.Button(new Rect(this.menuPos.x, this.menuPos.y + 65f, 170f, 30f), new GUIContent("Duplicate Selected Camera")))
					{
						ConcurrentDictionary<string, CameraPlusInstance> concurrentDictionary = Plugin.Instance.Cameras;
						lock (concurrentDictionary)
						{
							string nextCameraName2 = CameraUtilities.GetNextCameraName();
							Logger.Log("Adding " + nextCameraName2, Logger.Level.Notice);
							CameraUtilities.AddNewCamera(nextCameraName2, this.parentBehaviour.Config, false);
							CameraUtilities.ReloadCameras();
							this.parentBehaviour.CloseContextMenu();
						}
					}
					if (GUI.Button(new Rect(this.menuPos.x + 180f, this.menuPos.y + 65f, 120f, 30f), new GUIContent("Layout")))
					{
						this.layoutMode = true;
					}
					if (GUI.Button(new Rect(this.menuPos.x, this.menuPos.y + 105f, 120f, 30f), new GUIContent(this.parentBehaviour.Config.use360Camera ? "First Person" : (this.parentBehaviour.Config.thirdPerson ? " 360 Third Person" : "Third Person"))))
					{
						if (this.parentBehaviour.Config.use360Camera)
						{
							this.parentBehaviour.Config.thirdPerson = !this.parentBehaviour.Config.thirdPerson;
							this.parentBehaviour.ThirdPerson = this.parentBehaviour.Config.thirdPerson;
							this.parentBehaviour.ThirdPersonPos = this.parentBehaviour.Config.Position;
							this.parentBehaviour.ThirdPersonRot = this.parentBehaviour.Config.Rotation;
							this.parentBehaviour.Config.use360Camera = false;
						}
						else if (this.parentBehaviour.Config.thirdPerson)
						{
							this.parentBehaviour.Config.use360Camera = true;
						}
						else
						{
							this.parentBehaviour.Config.thirdPerson = !this.parentBehaviour.Config.thirdPerson;
							this.parentBehaviour.ThirdPerson = this.parentBehaviour.Config.thirdPerson;
							this.parentBehaviour.ThirdPersonPos = this.parentBehaviour.Config.Position;
							this.parentBehaviour.ThirdPersonRot = this.parentBehaviour.Config.Rotation;
						}
						this.parentBehaviour.CreateScreenRenderTexture();
						this.parentBehaviour.CloseContextMenu();
						this.parentBehaviour.Config.Save();
					}
					if (GUI.Button(new Rect(this.menuPos.x + 130f, this.menuPos.y + 105f, 170f, 30f), new GUIContent(this.parentBehaviour.Config.showThirdPersonCamera ? "Hide Third Person Camera" : "Show Third Person Camera")))
					{
						this.parentBehaviour.Config.showThirdPersonCamera = !this.parentBehaviour.Config.showThirdPersonCamera;
						this.parentBehaviour.Config.Save();
						this.parentBehaviour.CreateScreenRenderTexture();
						this.parentBehaviour.CloseContextMenu();
					}
					if (GUI.Button(new Rect(this.menuPos.x, this.menuPos.y + 145f, 170f, 30f), new GUIContent(this.parentBehaviour.Config.forceFirstPersonUpRight ? "Don't Force Camera Upright" : "Force Camera Upright")))
					{
						this.parentBehaviour.Config.forceFirstPersonUpRight = !this.parentBehaviour.Config.forceFirstPersonUpRight;
						this.parentBehaviour.Config.Save();
						this.parentBehaviour.CloseContextMenu();
					}
					if (GUI.Button(new Rect(this.menuPos.x + 180f, this.menuPos.y + 145f, 120f, 30f), new GUIContent(this.parentBehaviour.Config.transparentWalls ? "Solid Walls" : "Transparent Walls")))
					{
						this.parentBehaviour.Config.transparentWalls = !this.parentBehaviour.Config.transparentWalls;
						this.parentBehaviour.SetCullingMask();
						this.parentBehaviour.CloseContextMenu();
						this.parentBehaviour.Config.Save();
					}
					if (GUI.Button(new Rect(this.menuPos.x, this.menuPos.y + 185f, 120f, 30f), new GUIContent(this.parentBehaviour.Config.avatar ? "Hide Avatar" : "Show Avatar")))
					{
						this.parentBehaviour.Config.avatar = !this.parentBehaviour.Config.avatar;
						this.parentBehaviour.SetCullingMask();
						this.parentBehaviour.CloseContextMenu();
						this.parentBehaviour.Config.Save();
					}
					if (GUI.Button(new Rect(this.menuPos.x + 130f, this.menuPos.y + 185f, 170f, 30f), new GUIContent((this.parentBehaviour.Config.debri == "link") ? "Forced display Debri" : ((this.parentBehaviour.Config.debri == "show") ? "Forced non-display Debri" : "Debri Linked In-Game"))))
					{
						if (this.parentBehaviour.Config.debri == "link")
						{
							this.parentBehaviour.Config.debri = "show";
						}
						else if (this.parentBehaviour.Config.debri == "show")
						{
							this.parentBehaviour.Config.debri = "hide";
						}
						else
						{
							this.parentBehaviour.Config.debri = "link";
						}
						this.parentBehaviour.SetCullingMask();
						this.parentBehaviour.CloseContextMenu();
						this.parentBehaviour.Config.Save();
					}
					if (GUI.Button(new Rect(this.menuPos.x, this.menuPos.y + 225f, 300f, 30f), new GUIContent("Profile Saver")))
					{
						this.profileMode = true;
					}
					if (GUI.Button(new Rect(this.menuPos.x, this.menuPos.y + 430f, 300f, 30f), new GUIContent("Close Menu")))
					{
						this.parentBehaviour.CloseContextMenu();
					}
					if (GUI.Button(new Rect(this.menuPos.x, this.menuPos.y + 265f, 300f, 30f), new GUIContent("Spawn 38 Cameras")))
					{
						this.parentBehaviour.StartCoroutine(CameraUtilities.Spawn38Cameras());
						this.parentBehaviour.CloseContextMenu();
					}
				}
				else if (this.layoutMode)
				{
					if (GUI.Button(new Rect(this.menuPos.x, this.menuPos.y + 25f, 290f, 30f), new GUIContent("Reset Camera Position and Rotation")))
					{
						this.parentBehaviour.Config.Position = this.parentBehaviour.Config.DefaultPosition;
						this.parentBehaviour.Config.Rotation = this.parentBehaviour.Config.DefaultRotation;
						this.parentBehaviour.Config.FirstPersonPositionOffset = this.parentBehaviour.Config.DefaultFirstPersonPositionOffset;
						this.parentBehaviour.Config.FirstPersonRotationOffset = this.parentBehaviour.Config.DefaultFirstPersonRotationOffset;
						this.parentBehaviour.ThirdPersonPos = this.parentBehaviour.Config.DefaultPosition;
						this.parentBehaviour.ThirdPersonRot = this.parentBehaviour.Config.DefaultRotation;
						this.parentBehaviour.Config.Save();
						this.parentBehaviour.CloseContextMenu();
					}
					GUI.Box(new Rect(this.menuPos.x, this.menuPos.y + 60f, 140f, 55f), "Layer: " + this.parentBehaviour.Config.layer.ToString());
					if (GUI.Button(new Rect(this.menuPos.x + 5f, this.menuPos.y + 80f, 60f, 30f), new GUIContent("-")))
					{
						this.parentBehaviour.Config.layer--;
						this.parentBehaviour.CreateScreenRenderTexture();
						this.parentBehaviour.Config.Save();
					}
					if (GUI.Button(new Rect(this.menuPos.x + 75f, this.menuPos.y + 80f, 60f, 30f), new GUIContent("+")))
					{
						this.parentBehaviour.Config.layer++;
						this.parentBehaviour.CreateScreenRenderTexture();
						this.parentBehaviour.Config.Save();
					}
					GUI.Box(new Rect(this.menuPos.x + 155f, this.menuPos.y + 60f, 140f, 55f), "FOV: " + this.parentBehaviour.Config.fov.ToString());
					if (GUI.Button(new Rect(this.menuPos.x + 160f, this.menuPos.y + 80f, 60f, 30f), new GUIContent("-")))
					{
						this.parentBehaviour.Config.fov -= 1f;
						this.parentBehaviour.SetFOV();
						this.parentBehaviour.CreateScreenRenderTexture();
						this.parentBehaviour.Config.Save();
					}
					if (GUI.Button(new Rect(this.menuPos.x + 230f, this.menuPos.y + 80f, 60f, 30f), new GUIContent("+")))
					{
						this.parentBehaviour.Config.fov += 1f;
						this.parentBehaviour.SetFOV();
						this.parentBehaviour.CreateScreenRenderTexture();
						this.parentBehaviour.Config.Save();
					}
					GUI.Box(new Rect(this.menuPos.x, this.menuPos.y + 120f, 140f, 55f), "Render Scale: " + this.parentBehaviour.Config.renderScale.ToString("F1"));
					if (GUI.Button(new Rect(this.menuPos.x + 5f, this.menuPos.y + 140f, 60f, 30f), new GUIContent("-")))
					{
						this.parentBehaviour.Config.renderScale -= 0.1f;
						this.parentBehaviour.CreateScreenRenderTexture();
						this.parentBehaviour.Config.Save();
					}
					if (GUI.Button(new Rect(this.menuPos.x + 75f, this.menuPos.y + 140f, 60f, 30f), new GUIContent("+")))
					{
						this.parentBehaviour.Config.renderScale += 0.1f;
						this.parentBehaviour.CreateScreenRenderTexture();
						this.parentBehaviour.Config.Save();
					}
					if (GUI.Button(new Rect(this.menuPos.x + 155f, this.menuPos.y + 140f, 140f, 30f), new GUIContent(this.parentBehaviour.Config.fitToCanvas ? " Don't Fit To Canvas" : "Fit To Canvas")))
					{
						this.parentBehaviour.Config.fitToCanvas = !this.parentBehaviour.Config.fitToCanvas;
						this.parentBehaviour.CreateScreenRenderTexture();
						this.parentBehaviour.Config.Save();
					}
					GUI.Box(new Rect(this.menuPos.x, this.menuPos.y + 180f, 210f, 55f), "Amount movement : " + this.amountMove.ToString("F2"));
					if (GUI.Button(new Rect(this.menuPos.x + 5f, this.menuPos.y + 200f, 60f, 30f), new GUIContent("0.01")))
					{
						this.amountMove = 0.01f;
						this.parentBehaviour.CreateScreenRenderTexture();
					}
					if (GUI.Button(new Rect(this.menuPos.x + 75f, this.menuPos.y + 200f, 60f, 30f), new GUIContent("0.10")))
					{
						this.amountMove = 0.1f;
						this.parentBehaviour.CreateScreenRenderTexture();
					}
					if (GUI.Button(new Rect(this.menuPos.x + 145f, this.menuPos.y + 200f, 60f, 30f), new GUIContent("1.00")))
					{
						this.amountMove = 1f;
						this.parentBehaviour.CreateScreenRenderTexture();
					}
					GUI.Box(new Rect(this.menuPos.x, this.menuPos.y + 240f, 95f, 55f), "X Pos :" + (this.parentBehaviour.Config.use360Camera ? this.parentBehaviour.Config.cam360RightOffset.ToString("F2") : (this.parentBehaviour.Config.thirdPerson ? this.parentBehaviour.Config.posx.ToString("F2") : this.parentBehaviour.Config.firstPersonPosOffsetX.ToString("F2"))));
					if (GUI.Button(new Rect(this.menuPos.x + 5f, this.menuPos.y + 260f, 40f, 30f), new GUIContent("-")))
					{
						if (this.parentBehaviour.Config.use360Camera)
						{
							this.parentBehaviour.Config.cam360RightOffset -= this.amountMove;
						}
						else if (this.parentBehaviour.Config.thirdPerson)
						{
							this.parentBehaviour.Config.posx -= this.amountMove;
						}
						else
						{
							this.parentBehaviour.Config.firstPersonPosOffsetX -= this.amountMove;
						}
						this.parentBehaviour.CreateScreenRenderTexture();
						this.parentBehaviour.Config.Save();
					}
					if (GUI.Button(new Rect(this.menuPos.x + 50f, this.menuPos.y + 260f, 40f, 30f), new GUIContent("+")))
					{
						if (this.parentBehaviour.Config.use360Camera)
						{
							this.parentBehaviour.Config.cam360RightOffset += this.amountMove;
						}
						else if (this.parentBehaviour.Config.thirdPerson)
						{
							this.parentBehaviour.Config.posx += this.amountMove;
						}
						else
						{
							this.parentBehaviour.Config.firstPersonPosOffsetX += this.amountMove;
						}
						this.parentBehaviour.CreateScreenRenderTexture();
						this.parentBehaviour.Config.Save();
					}
					GUI.Box(new Rect(this.menuPos.x + 100f, this.menuPos.y + 240f, 95f, 55f), "Y Pos :" + (this.parentBehaviour.Config.use360Camera ? this.parentBehaviour.Config.cam360UpOffset.ToString("F2") : (this.parentBehaviour.Config.thirdPerson ? this.parentBehaviour.Config.posy.ToString("F2") : this.parentBehaviour.Config.firstPersonPosOffsetY.ToString("F2"))));
					if (GUI.Button(new Rect(this.menuPos.x + 105f, this.menuPos.y + 260f, 40f, 30f), new GUIContent("-")))
					{
						if (this.parentBehaviour.Config.use360Camera)
						{
							this.parentBehaviour.Config.cam360UpOffset -= this.amountMove;
						}
						else if (this.parentBehaviour.Config.thirdPerson)
						{
							this.parentBehaviour.Config.posy -= this.amountMove;
						}
						else
						{
							this.parentBehaviour.Config.firstPersonPosOffsetY -= this.amountMove;
						}
						this.parentBehaviour.CreateScreenRenderTexture();
						this.parentBehaviour.Config.Save();
					}
					if (GUI.Button(new Rect(this.menuPos.x + 150f, this.menuPos.y + 260f, 40f, 30f), new GUIContent("+")))
					{
						if (this.parentBehaviour.Config.use360Camera)
						{
							this.parentBehaviour.Config.cam360UpOffset += this.amountMove;
						}
						else if (this.parentBehaviour.Config.thirdPerson)
						{
							this.parentBehaviour.Config.posy += this.amountMove;
						}
						else
						{
							this.parentBehaviour.Config.firstPersonPosOffsetY += this.amountMove;
						}
						this.parentBehaviour.CreateScreenRenderTexture();
						this.parentBehaviour.Config.Save();
					}
					GUI.Box(new Rect(this.menuPos.x + 205f, this.menuPos.y + 240f, 95f, 55f), "Z Pos :" + (this.parentBehaviour.Config.use360Camera ? this.parentBehaviour.Config.cam360ForwardOffset.ToString("F2") : (this.parentBehaviour.Config.thirdPerson ? this.parentBehaviour.Config.posz.ToString("F2") : this.parentBehaviour.Config.firstPersonPosOffsetZ.ToString("F2"))));
					if (GUI.Button(new Rect(this.menuPos.x + 210f, this.menuPos.y + 260f, 40f, 30f), new GUIContent("-")))
					{
						if (this.parentBehaviour.Config.use360Camera)
						{
							this.parentBehaviour.Config.cam360ForwardOffset -= this.amountMove;
						}
						else if (this.parentBehaviour.Config.thirdPerson)
						{
							this.parentBehaviour.Config.posz -= this.amountMove;
						}
						else
						{
							this.parentBehaviour.Config.firstPersonPosOffsetZ -= this.amountMove;
						}
						this.parentBehaviour.CreateScreenRenderTexture();
						this.parentBehaviour.Config.Save();
					}
					if (GUI.Button(new Rect(this.menuPos.x + 255f, this.menuPos.y + 260f, 40f, 30f), new GUIContent("+")))
					{
						if (this.parentBehaviour.Config.use360Camera)
						{
							this.parentBehaviour.Config.cam360ForwardOffset += this.amountMove;
						}
						else if (this.parentBehaviour.Config.thirdPerson)
						{
							this.parentBehaviour.Config.posz += this.amountMove;
						}
						else
						{
							this.parentBehaviour.Config.firstPersonPosOffsetZ += this.amountMove;
						}
						this.parentBehaviour.CreateScreenRenderTexture();
						this.parentBehaviour.Config.Save();
					}
					GUI.Box(new Rect(this.menuPos.x, this.menuPos.y + 300f, 290f, 55f), "Amount rotation : " + this.amountRot.ToString("F2"));
					if (GUI.Button(new Rect(this.menuPos.x + 5f, this.menuPos.y + 320f, 50f, 30f), new GUIContent("0.01")))
					{
						this.amountRot = 0.01f;
						this.parentBehaviour.CreateScreenRenderTexture();
					}
					if (GUI.Button(new Rect(this.menuPos.x + 60f, this.menuPos.y + 320f, 50f, 30f), new GUIContent("0.10")))
					{
						this.amountRot = 0.1f;
						this.parentBehaviour.CreateScreenRenderTexture();
					}
					if (GUI.Button(new Rect(this.menuPos.x + 115f, this.menuPos.y + 320f, 50f, 30f), new GUIContent("1.00")))
					{
						this.amountRot = 1f;
						this.parentBehaviour.CreateScreenRenderTexture();
					}
					if (GUI.Button(new Rect(this.menuPos.x + 170f, this.menuPos.y + 320f, 50f, 30f), new GUIContent("10")))
					{
						this.amountRot = 10f;
						this.parentBehaviour.CreateScreenRenderTexture();
					}
					if (GUI.Button(new Rect(this.menuPos.x + 225f, this.menuPos.y + 320f, 50f, 30f), new GUIContent("45")))
					{
						this.amountRot = 45f;
						this.parentBehaviour.CreateScreenRenderTexture();
					}
					GUI.Box(new Rect(this.menuPos.x, this.menuPos.y + 360f, 95f, 55f), "X Rot :" + (this.parentBehaviour.Config.use360Camera ? this.parentBehaviour.Config.cam360XTilt.ToString("F2") : (this.parentBehaviour.Config.thirdPerson ? this.parentBehaviour.Config.angx.ToString("F2") : this.parentBehaviour.Config.firstPersonRotOffsetX.ToString("F2"))));
					if (GUI.Button(new Rect(this.menuPos.x + 5f, this.menuPos.y + 380f, 40f, 30f), new GUIContent("-")))
					{
						if (this.parentBehaviour.Config.use360Camera)
						{
							this.parentBehaviour.Config.cam360XTilt -= this.amountRot;
						}
						else if (this.parentBehaviour.Config.thirdPerson)
						{
							this.parentBehaviour.Config.angx -= this.amountRot;
						}
						else
						{
							this.parentBehaviour.Config.firstPersonRotOffsetX -= this.amountRot;
						}
						this.parentBehaviour.CreateScreenRenderTexture();
						this.parentBehaviour.Config.Save();
					}
					if (GUI.Button(new Rect(this.menuPos.x + 50f, this.menuPos.y + 380f, 40f, 30f), new GUIContent("+")))
					{
						if (this.parentBehaviour.Config.use360Camera)
						{
							this.parentBehaviour.Config.cam360XTilt += this.amountRot;
						}
						else if (this.parentBehaviour.Config.thirdPerson)
						{
							this.parentBehaviour.Config.angx += this.amountRot;
						}
						else
						{
							this.parentBehaviour.Config.firstPersonRotOffsetX += this.amountRot;
						}
						this.parentBehaviour.CreateScreenRenderTexture();
						this.parentBehaviour.Config.Save();
					}
					GUI.Box(new Rect(this.menuPos.x + 100f, this.menuPos.y + 360f, 95f, 55f), "Y Rot :" + (this.parentBehaviour.Config.use360Camera ? this.parentBehaviour.Config.cam360YTilt.ToString("F2") : (this.parentBehaviour.Config.thirdPerson ? this.parentBehaviour.Config.angy.ToString("F2") : this.parentBehaviour.Config.firstPersonRotOffsetY.ToString("F2"))));
					if (GUI.Button(new Rect(this.menuPos.x + 105f, this.menuPos.y + 380f, 40f, 30f), new GUIContent("-")))
					{
						if (this.parentBehaviour.Config.use360Camera)
						{
							this.parentBehaviour.Config.cam360YTilt -= this.amountRot;
						}
						else if (this.parentBehaviour.Config.thirdPerson)
						{
							this.parentBehaviour.Config.angy -= this.amountRot;
						}
						else
						{
							this.parentBehaviour.Config.firstPersonRotOffsetY -= this.amountRot;
						}
						this.parentBehaviour.CreateScreenRenderTexture();
						this.parentBehaviour.Config.Save();
					}
					if (GUI.Button(new Rect(this.menuPos.x + 150f, this.menuPos.y + 380f, 40f, 30f), new GUIContent("+")))
					{
						if (this.parentBehaviour.Config.use360Camera)
						{
							this.parentBehaviour.Config.cam360YTilt += this.amountRot;
						}
						else if (this.parentBehaviour.Config.thirdPerson)
						{
							this.parentBehaviour.Config.angy += this.amountRot;
						}
						else
						{
							this.parentBehaviour.Config.firstPersonRotOffsetY += this.amountRot;
						}
						this.parentBehaviour.CreateScreenRenderTexture();
						this.parentBehaviour.Config.Save();
					}
					GUI.Box(new Rect(this.menuPos.x + 205f, this.menuPos.y + 360f, 95f, 55f), "Z Rot :" + (this.parentBehaviour.Config.use360Camera ? this.parentBehaviour.Config.cam360ZTilt.ToString("F2") : (this.parentBehaviour.Config.thirdPerson ? this.parentBehaviour.Config.angz.ToString("F2") : this.parentBehaviour.Config.firstPersonRotOffsetZ.ToString("F2"))));
					if (GUI.Button(new Rect(this.menuPos.x + 210f, this.menuPos.y + 380f, 40f, 30f), new GUIContent("-")))
					{
						if (this.parentBehaviour.Config.use360Camera)
						{
							this.parentBehaviour.Config.cam360ZTilt -= this.amountRot;
						}
						else if (this.parentBehaviour.Config.thirdPerson)
						{
							this.parentBehaviour.Config.angz -= this.amountRot;
						}
						else
						{
							this.parentBehaviour.Config.firstPersonRotOffsetZ -= this.amountRot;
						}
						this.parentBehaviour.CreateScreenRenderTexture();
						this.parentBehaviour.Config.Save();
					}
					if (GUI.Button(new Rect(this.menuPos.x + 255f, this.menuPos.y + 380f, 40f, 30f), new GUIContent("+")))
					{
						if (this.parentBehaviour.Config.use360Camera)
						{
							this.parentBehaviour.Config.cam360ZTilt += this.amountRot;
						}
						else if (this.parentBehaviour.Config.thirdPerson)
						{
							this.parentBehaviour.Config.angz += this.amountRot;
						}
						else
						{
							this.parentBehaviour.Config.firstPersonRotOffsetZ += this.amountRot;
						}
						this.parentBehaviour.CreateScreenRenderTexture();
						this.parentBehaviour.Config.Save();
					}
					if (GUI.Button(new Rect(this.menuPos.x, this.menuPos.y + 430f, 290f, 30f), new GUIContent("Close Layout Menu")))
					{
						this.layoutMode = false;
					}
				}
				else if (this.profileMode)
				{
					if (GUI.Button(new Rect(this.menuPos.x, this.menuPos.y + 45f, 140f, 30f), new GUIContent("<")))
					{
						CameraProfiles.TrySetLast(CameraProfiles.currentlySelected);
					}
					if (GUI.Button(new Rect(this.menuPos.x + 155f, this.menuPos.y + 45f, 140f, 30f), new GUIContent(">")))
					{
						CameraProfiles.SetNext(CameraProfiles.currentlySelected);
					}
					if (GUI.Button(new Rect(this.menuPos.x + 30f, this.menuPos.y + 85f, 230f, 100f), new GUIContent("Currently Selected:\n" + CameraProfiles.currentlySelected)))
					{
						CameraProfiles.SetNext(CameraProfiles.currentlySelected);
					}
					if (GUI.Button(new Rect(this.menuPos.x, this.menuPos.y + 225f, 140f, 30f), new GUIContent("Save")))
					{
						CameraProfiles.SaveCurrent();
					}
					if (GUI.Button(new Rect(this.menuPos.x + 150f, this.menuPos.y + 225f, 140f, 30f), new GUIContent("Delete")))
					{
						CameraProfiles.DeleteProfile(CameraProfiles.currentlySelected);
					}
					if (GUI.Button(new Rect(this.menuPos.x, this.menuPos.y + 265f, 290f, 30f), new GUIContent("Load Selected")))
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
						CameraProfiles.SetProfile(CameraProfiles.currentlySelected);
						CameraUtilities.ReloadCameras();
					}
					if (GUI.Button(new Rect(this.menuPos.x, this.menuPos.y + 305f, 290f, 30f), new GUIContent("Close Profile Menu")))
					{
						this.profileMode = false;
					}
				}
				GUI.matrix = matrix;
			}
		}

		// Token: 0x0400003C RID: 60
		internal Vector2 mousePosition;

		// Token: 0x0400003D RID: 61
		internal bool showMenu;

		// Token: 0x0400003E RID: 62
		internal bool layoutMode;

		// Token: 0x0400003F RID: 63
		internal bool profileMode;

		// Token: 0x04000040 RID: 64
		internal float amountMove = 0.1f;

		// Token: 0x04000041 RID: 65
		internal float amountRot = 0.1f;

		// Token: 0x04000042 RID: 66
		internal CameraPlusBehaviour parentBehaviour;
	}
}
