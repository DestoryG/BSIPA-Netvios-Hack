using System;
using System.Collections.Generic;
using UnityEngine;

namespace CustomAvatar.StereoRendering
{
	// Token: 0x0200002D RID: 45
	[ExecuteInEditMode]
	[DisallowMultipleComponent]
	[RequireComponent(typeof(Renderer))]
	internal class StereoRenderer : MonoBehaviour
	{
		// Token: 0x17000018 RID: 24
		// (get) Token: 0x060000BA RID: 186 RVA: 0x000059EC File Offset: 0x00003BEC
		// (set) Token: 0x060000BB RID: 187 RVA: 0x00005A23 File Offset: 0x00003C23
		public Vector3 canvasOriginPos
		{
			get
			{
				bool flag = this.canvasOrigin == null;
				Vector3 vector;
				if (flag)
				{
					vector = this.m_canvasOriginWorldPosition;
				}
				else
				{
					vector = this.canvasOrigin.position;
				}
				return vector;
			}
			set
			{
				this.m_canvasOriginWorldPosition = value;
			}
		}

		// Token: 0x17000019 RID: 25
		// (get) Token: 0x060000BC RID: 188 RVA: 0x00005A30 File Offset: 0x00003C30
		// (set) Token: 0x060000BD RID: 189 RVA: 0x00005A67 File Offset: 0x00003C67
		public Vector3 canvasOriginEuler
		{
			get
			{
				bool flag = this.canvasOrigin == null;
				Vector3 vector;
				if (flag)
				{
					vector = this.m_canvasOriginWorldRotation;
				}
				else
				{
					vector = this.canvasOrigin.eulerAngles;
				}
				return vector;
			}
			set
			{
				this.m_canvasOriginWorldRotation = value;
			}
		}

		// Token: 0x1700001A RID: 26
		// (get) Token: 0x060000BE RID: 190 RVA: 0x00005A74 File Offset: 0x00003C74
		// (set) Token: 0x060000BF RID: 191 RVA: 0x00005A91 File Offset: 0x00003C91
		public Quaternion canvasOriginRot
		{
			get
			{
				return Quaternion.Euler(this.canvasOriginEuler);
			}
			set
			{
				this.canvasOriginEuler = value.eulerAngles;
			}
		}

		// Token: 0x1700001B RID: 27
		// (get) Token: 0x060000C0 RID: 192 RVA: 0x00005AA4 File Offset: 0x00003CA4
		public Vector3 canvasOriginForward
		{
			get
			{
				return this.canvasOriginRot * Vector3.forward;
			}
		}

		// Token: 0x1700001C RID: 28
		// (get) Token: 0x060000C1 RID: 193 RVA: 0x00005AC8 File Offset: 0x00003CC8
		public Vector3 canvasOriginUp
		{
			get
			{
				return this.canvasOriginRot * Vector3.up;
			}
		}

		// Token: 0x1700001D RID: 29
		// (get) Token: 0x060000C2 RID: 194 RVA: 0x00005AEC File Offset: 0x00003CEC
		public Vector3 canvasOriginRight
		{
			get
			{
				return this.canvasOriginRot * Vector3.right;
			}
		}

		// Token: 0x1700001E RID: 30
		// (get) Token: 0x060000C3 RID: 195 RVA: 0x00005B10 File Offset: 0x00003D10
		// (set) Token: 0x060000C4 RID: 196 RVA: 0x00005B33 File Offset: 0x00003D33
		public Vector3 localCanvasOriginPos
		{
			get
			{
				return base.transform.InverseTransformPoint(this.canvasOriginPos);
			}
			set
			{
				this.canvasOriginPos = base.transform.InverseTransformPoint(value);
			}
		}

		// Token: 0x1700001F RID: 31
		// (get) Token: 0x060000C5 RID: 197 RVA: 0x00005B4C File Offset: 0x00003D4C
		// (set) Token: 0x060000C6 RID: 198 RVA: 0x00005B88 File Offset: 0x00003D88
		public Vector3 localCanvasOriginEuler
		{
			get
			{
				return (Quaternion.Inverse(base.transform.rotation) * Quaternion.Euler(this.canvasOriginEuler)).eulerAngles;
			}
			set
			{
				this.canvasOriginEuler = (base.transform.rotation * Quaternion.Euler(value)).eulerAngles;
			}
		}

		// Token: 0x17000020 RID: 32
		// (get) Token: 0x060000C7 RID: 199 RVA: 0x00005BBC File Offset: 0x00003DBC
		// (set) Token: 0x060000C8 RID: 200 RVA: 0x00005BE9 File Offset: 0x00003DE9
		public Quaternion localCanvasOriginRot
		{
			get
			{
				return Quaternion.Inverse(base.transform.rotation) * this.canvasOriginRot;
			}
			set
			{
				this.canvasOriginRot = base.transform.rotation * value;
			}
		}

		// Token: 0x17000021 RID: 33
		// (get) Token: 0x060000C9 RID: 201 RVA: 0x00005C04 File Offset: 0x00003E04
		// (set) Token: 0x060000CA RID: 202 RVA: 0x00005C3B File Offset: 0x00003E3B
		public Vector3 anchorPos
		{
			get
			{
				bool flag = this.anchorTransform == null;
				Vector3 vector;
				if (flag)
				{
					vector = this.m_anchorWorldPosition;
				}
				else
				{
					vector = this.anchorTransform.position;
				}
				return vector;
			}
			set
			{
				this.m_anchorWorldPosition = value;
			}
		}

		// Token: 0x17000022 RID: 34
		// (get) Token: 0x060000CB RID: 203 RVA: 0x00005C48 File Offset: 0x00003E48
		// (set) Token: 0x060000CC RID: 204 RVA: 0x00005C7F File Offset: 0x00003E7F
		public Vector3 anchorEuler
		{
			get
			{
				bool flag = this.anchorTransform == null;
				Vector3 vector;
				if (flag)
				{
					vector = this.m_anchorWorldRotation;
				}
				else
				{
					vector = this.anchorTransform.eulerAngles;
				}
				return vector;
			}
			set
			{
				this.m_anchorWorldRotation = value;
			}
		}

		// Token: 0x17000023 RID: 35
		// (get) Token: 0x060000CD RID: 205 RVA: 0x00005C8C File Offset: 0x00003E8C
		// (set) Token: 0x060000CE RID: 206 RVA: 0x00005CA9 File Offset: 0x00003EA9
		public Quaternion anchorRot
		{
			get
			{
				return Quaternion.Euler(this.anchorEuler);
			}
			set
			{
				this.anchorEuler = value.eulerAngles;
			}
		}

		// Token: 0x17000024 RID: 36
		// (get) Token: 0x060000CF RID: 207 RVA: 0x00005CBC File Offset: 0x00003EBC
		public Vector3 anchorForward
		{
			get
			{
				return this.anchorRot * Vector3.forward;
			}
		}

		// Token: 0x17000025 RID: 37
		// (get) Token: 0x060000D0 RID: 208 RVA: 0x00005CE0 File Offset: 0x00003EE0
		public Vector3 anchorUp
		{
			get
			{
				return this.anchorRot * Vector3.up;
			}
		}

		// Token: 0x060000D1 RID: 209 RVA: 0x00005D04 File Offset: 0x00003F04
		private void Start()
		{
			bool flag = this.stereoCameraHead == null;
			if (flag)
			{
				this.CreateStereoCameraRig();
			}
			Renderer component = base.GetComponent<Renderer>();
			this.stereoMaterial = component.materials[0];
			StereoRenderManager.Instance.AddToManager(this);
		}

		// Token: 0x060000D2 RID: 210 RVA: 0x00005D4A File Offset: 0x00003F4A
		private void OnDestroy()
		{
			StereoRenderManager.Instance.RemoveFromManager(this);
		}

		// Token: 0x060000D3 RID: 211 RVA: 0x00005D5C File Offset: 0x00003F5C
		private void CreateStereoCameraRig()
		{
			this.stereoCameraHead = new GameObject("Stereo Camera Head [" + base.gameObject.name + "]");
			this.stereoCameraHead.transform.parent = base.transform;
			this.stereoCameraEye = new GameObject("Stereo Camera Eye [" + base.gameObject.name + "]")
			{
				transform = 
				{
					parent = this.stereoCameraHead.transform
				}
			}.AddComponent<Camera>();
			this.stereoCameraEye.enabled = false;
		}

		// Token: 0x060000D4 RID: 212 RVA: 0x00005DF8 File Offset: 0x00003FF8
		private void Update()
		{
			bool flag = this.isMirror;
			if (flag)
			{
				this.anchorPos = this.canvasOriginPos;
				this.anchorRot = this.canvasOriginRot;
			}
		}

		// Token: 0x060000D5 RID: 213 RVA: 0x00005E2C File Offset: 0x0000402C
		private void OnWillRenderObject()
		{
			bool flag = Camera.current.GetComponent<VRRenderEventDetector>() != null;
			if (flag)
			{
				this.canvasVisible = true;
			}
		}

		// Token: 0x060000D6 RID: 214 RVA: 0x00005E58 File Offset: 0x00004058
		public void Render(VRRenderEventDetector detector)
		{
			this.MoveStereoCameraBasedOnHmdPose(detector);
			bool flag = this.preRenderListeners != null;
			if (flag)
			{
				this.preRenderListeners();
			}
			bool flag2 = this.canvasVisible;
			if (flag2)
			{
				this.ignoreObjOriginalLayer.Clear();
				for (int i = 0; i < this.ignoreWhenRender.Count; i++)
				{
					this.ignoreObjOriginalLayer.Add(this.ignoreWhenRender[i].layer);
				}
				bool flag3 = this.isMirror;
				if (flag3)
				{
					GL.invertCulling = true;
				}
				this.RenderToTwoStereoTextures(detector);
				bool flag4 = this.isMirror;
				if (flag4)
				{
					GL.invertCulling = false;
				}
				for (int j = 0; j < this.ignoreWhenRender.Count; j++)
				{
					this.ignoreWhenRender[j].layer = this.ignoreObjOriginalLayer[j];
				}
				this.canvasVisible = false;
			}
			bool flag5 = this.postRenderListeners != null;
			if (flag5)
			{
				this.postRenderListeners();
			}
		}

		// Token: 0x060000D7 RID: 215 RVA: 0x00005F6C File Offset: 0x0000416C
		public void MoveStereoCameraBasedOnHmdPose(VRRenderEventDetector detector)
		{
			Vector3 position = detector.transform.position;
			Quaternion rotation = detector.transform.rotation;
			bool flag = this.isMirror;
			if (flag)
			{
				float num = -Vector3.Dot(this.canvasOriginUp, this.canvasOriginPos);
				Vector4 vector;
				vector..ctor(this.canvasOriginUp.x, this.canvasOriginUp.y, this.canvasOriginUp.z, num);
				this.reflectionMat = Matrix4x4.zero;
				this.CalculateReflectionMatrix(ref this.reflectionMat, vector);
				Vector3 vector2 = this.reflectionMat.MultiplyPoint(position);
				this.stereoCameraHead.transform.position = vector2;
				this.stereoCameraHead.transform.rotation = rotation;
			}
			else
			{
				Vector3 vector3 = position - this.canvasOriginPos;
				Quaternion quaternion = this.anchorRot * Quaternion.Inverse(this.canvasOriginRot);
				Vector3 vector4 = quaternion * vector3;
				this.stereoCameraHead.transform.position = this.anchorPos + vector4;
				this.stereoCameraHead.transform.rotation = quaternion * rotation;
			}
		}

		// Token: 0x060000D8 RID: 216 RVA: 0x00006098 File Offset: 0x00004298
		private void RenderToTwoStereoTextures(VRRenderEventDetector detector)
		{
			float num = 0.06567926f;
			Vector3 vector;
			vector..ctor(-num / 2f, 0f, 0f);
			Vector3 vector2;
			vector2..ctor(num / 2f, 0f, 0f);
			int hashCode = detector.GetHashCode();
			int num2 = (int)(this.textureResolutionScale * (float)detector.Camera.pixelWidth);
			int num3 = (int)(this.textureResolutionScale * (float)detector.Camera.pixelHeight);
			bool flag = !this.leftEyeTextures.ContainsKey(hashCode);
			if (flag)
			{
				this.leftEyeTextures.Add(hashCode, this.CreateRenderTexture(num2, num3, 32, 4));
			}
			bool flag2 = !this.rightEyeTextures.ContainsKey(hashCode);
			if (flag2)
			{
				this.rightEyeTextures.Add(hashCode, this.CreateRenderTexture(num2, num3, 32, 4));
			}
			Matrix4x4 matrix4x = detector.Camera.projectionMatrix;
			Matrix4x4 matrix4x2 = detector.Camera.projectionMatrix;
			bool stereoEnabled = detector.Camera.stereoEnabled;
			if (stereoEnabled)
			{
				matrix4x = detector.Camera.GetStereoProjectionMatrix(0);
				matrix4x2 = detector.Camera.GetStereoProjectionMatrix(1);
			}
			this.RenderEye(vector, matrix4x, detector.Camera.worldToCameraMatrix, this.leftEyeTextures[hashCode], "_LeftEyeTexture");
			bool stereoEnabled2 = detector.Camera.stereoEnabled;
			if (stereoEnabled2)
			{
				Matrix4x4 worldToCameraMatrix = detector.Camera.worldToCameraMatrix;
				worldToCameraMatrix.m03 -= num;
				this.RenderEye(vector2, matrix4x2, worldToCameraMatrix, this.rightEyeTextures[hashCode], "_RightEyeTexture");
			}
		}

		// Token: 0x060000D9 RID: 217 RVA: 0x0000622C File Offset: 0x0000442C
		private RenderTexture CreateRenderTexture(int renderWidth, int renderHeight, int depth = 32, int aaLevel = 4)
		{
			return new RenderTexture(renderWidth, renderHeight, depth, 0, 0)
			{
				antiAliasing = aaLevel
			};
		}

		// Token: 0x060000DA RID: 218 RVA: 0x00006254 File Offset: 0x00004454
		private void RenderEye(Vector3 eyeOffset, Matrix4x4 projMat, Matrix4x4 worldToCameraMat, RenderTexture targetTexture, string textureName)
		{
			this.stereoCameraEye.transform.localPosition = eyeOffset;
			bool flag = this.isMirror;
			if (flag)
			{
				this.stereoCameraEye.worldToCameraMatrix = worldToCameraMat * this.reflectionMat;
			}
			this.stereoCameraEye.projectionMatrix = projMat;
			bool flag2 = this.useScissor;
			if (flag2)
			{
				Rect scissorRect = this.GetScissorRect(projMat * worldToCameraMat);
				this.stereoCameraEye.rect = scissorRect;
				this.stereoCameraEye.projectionMatrix = this.GetScissorMatrix(scissorRect) * this.stereoCameraEye.projectionMatrix;
			}
			else
			{
				this.stereoCameraEye.rect = this.fullViewport;
			}
			bool flag3 = this.useObliqueClip;
			if (flag3)
			{
				Vector4 obliqueNearClipPlane = this.GetObliqueNearClipPlane();
				this.stereoCameraEye.projectionMatrix = this.stereoCameraEye.CalculateObliqueMatrix(obliqueNearClipPlane);
			}
			this.stereoCameraEye.targetTexture = targetTexture;
			this.stereoCameraEye.Render();
			this.stereoMaterial.SetTexture(textureName, targetTexture);
		}

		// Token: 0x060000DB RID: 219 RVA: 0x0000635C File Offset: 0x0000455C
		public void CalculateReflectionMatrix(ref Matrix4x4 reflectionMat, Vector4 normal)
		{
			reflectionMat.m00 = 1f - 2f * normal[0] * normal[0];
			reflectionMat.m01 = -2f * normal[0] * normal[1];
			reflectionMat.m02 = -2f * normal[0] * normal[2];
			reflectionMat.m03 = -2f * normal[3] * normal[0];
			reflectionMat.m10 = -2f * normal[1] * normal[0];
			reflectionMat.m11 = 1f - 2f * normal[1] * normal[1];
			reflectionMat.m12 = -2f * normal[1] * normal[2];
			reflectionMat.m13 = -2f * normal[3] * normal[1];
			reflectionMat.m20 = -2f * normal[2] * normal[0];
			reflectionMat.m21 = -2f * normal[2] * normal[1];
			reflectionMat.m22 = 1f - 2f * normal[2] * normal[2];
			reflectionMat.m23 = -2f * normal[3] * normal[2];
			reflectionMat.m30 = 0f;
			reflectionMat.m31 = 0f;
			reflectionMat.m32 = 0f;
			reflectionMat.m33 = 1f;
		}

		// Token: 0x060000DC RID: 220 RVA: 0x00006504 File Offset: 0x00004704
		private Vector4 GetCameraSpacePlane(Camera cam, Vector3 pt, Vector3 normal)
		{
			Matrix4x4 worldToCameraMatrix = cam.worldToCameraMatrix;
			Vector3 vector = worldToCameraMatrix.MultiplyPoint(pt);
			Vector3 normalized = worldToCameraMatrix.MultiplyVector(normal).normalized;
			return new Vector4(normalized.x, normalized.y, normalized.z, -Vector3.Dot(vector, normalized));
		}

		// Token: 0x060000DD RID: 221 RVA: 0x00006558 File Offset: 0x00004758
		private Vector4 GetObliqueNearClipPlane()
		{
			bool flag = !this.isMirror;
			Vector4 vector;
			if (flag)
			{
				vector = this.GetCameraSpacePlane(this.stereoCameraEye, this.anchorPos, this.anchorForward);
			}
			else
			{
				float num = -Vector3.Dot(this.canvasOriginUp, this.canvasOriginPos) - this.reflectionOffset;
				Vector4 vector2;
				vector2..ctor(this.canvasOriginUp.x, this.canvasOriginUp.y, this.canvasOriginUp.z, num);
				vector = this.GetCameraSpacePlane(this.stereoCameraEye, this.canvasOriginPos, vector2);
			}
			return vector;
		}

		// Token: 0x060000DE RID: 222 RVA: 0x000065F8 File Offset: 0x000047F8
		private Rect GetScissorRect(Matrix4x4 mat)
		{
			Renderer component = base.GetComponent<Renderer>();
			Vector3 center = component.bounds.center;
			Vector3 extents = component.bounds.extents;
			Vector3[] array = new Vector3[]
			{
				this.WorldPointToViewport(mat, new Vector3(center.x - extents.x, center.y - extents.y, center.z - extents.z)),
				this.WorldPointToViewport(mat, new Vector3(center.x + extents.x, center.y - extents.y, center.z - extents.z)),
				this.WorldPointToViewport(mat, new Vector3(center.x - extents.x, center.y - extents.y, center.z + extents.z)),
				this.WorldPointToViewport(mat, new Vector3(center.x + extents.x, center.y - extents.y, center.z + extents.z)),
				this.WorldPointToViewport(mat, new Vector3(center.x - extents.x, center.y + extents.y, center.z - extents.z)),
				this.WorldPointToViewport(mat, new Vector3(center.x + extents.x, center.y + extents.y, center.z - extents.z)),
				this.WorldPointToViewport(mat, new Vector3(center.x - extents.x, center.y + extents.y, center.z + extents.z)),
				this.WorldPointToViewport(mat, new Vector3(center.x + extents.x, center.y + extents.y, center.z + extents.z))
			};
			bool flag = false;
			Vector2 vector = array[0];
			Vector2 vector2 = array[0];
			foreach (Vector3 vector3 in array)
			{
				bool flag2 = vector3.z < 0f;
				if (flag2)
				{
					flag = true;
					break;
				}
				vector = Vector2.Min(vector, vector3);
				vector2 = Vector2.Max(vector2, vector3);
			}
			bool flag3 = flag;
			Rect rect;
			if (flag3)
			{
				rect = this.fullViewport;
			}
			else
			{
				vector = Vector2.Max(vector, Vector2.zero);
				vector2 = Vector2.Min(vector2, Vector2.one);
				rect = new Rect(vector.x, vector.y, vector2.x - vector.x, vector2.y - vector.y);
			}
			return rect;
		}

		// Token: 0x060000DF RID: 223 RVA: 0x000068E8 File Offset: 0x00004AE8
		private Matrix4x4 GetScissorMatrix(Rect rect)
		{
			Matrix4x4 matrix4x = Matrix4x4.TRS(new Vector3(1f / rect.width - 1f, 1f / rect.height - 1f, 0f), Quaternion.identity, new Vector3(1f / rect.width, 1f / rect.height, 1f));
			Matrix4x4 matrix4x2 = Matrix4x4.TRS(new Vector3(-rect.x * 2f / rect.width, -rect.y * 2f / rect.height, 0f), Quaternion.identity, Vector3.one);
			return matrix4x2 * matrix4x;
		}

		// Token: 0x060000E0 RID: 224 RVA: 0x000069A8 File Offset: 0x00004BA8
		private Vector3 WorldPointToViewport(Matrix4x4 mat, Vector3 point)
		{
			Vector3 vector;
			vector.x = mat.m00 * point.x + mat.m01 * point.y + mat.m02 * point.z + mat.m03;
			vector.y = mat.m10 * point.x + mat.m11 * point.y + mat.m12 * point.z + mat.m13;
			vector.z = mat.m20 * point.x + mat.m21 * point.y + mat.m22 * point.z + mat.m23;
			float num = mat.m30 * point.x + mat.m31 * point.y + mat.m32 * point.z + mat.m33;
			num = 1f / num;
			vector.x *= num;
			vector.y *= num;
			vector.z = num;
			point = vector;
			point.x = point.x * 0.5f + 0.5f;
			point.y = point.y * 0.5f + 0.5f;
			return point;
		}

		// Token: 0x060000E1 RID: 225 RVA: 0x00006AF0 File Offset: 0x00004CF0
		public void AddPreRenderListener(Action listener)
		{
			bool flag = listener == null;
			if (!flag)
			{
				this.preRenderListeners = (Action)Delegate.Combine(this.preRenderListeners, listener);
			}
		}

		// Token: 0x060000E2 RID: 226 RVA: 0x00006B20 File Offset: 0x00004D20
		public void AddPostRenderListener(Action listener)
		{
			bool flag = listener == null;
			if (!flag)
			{
				this.postRenderListeners = (Action)Delegate.Combine(this.postRenderListeners, listener);
			}
		}

		// Token: 0x060000E3 RID: 227 RVA: 0x00006B50 File Offset: 0x00004D50
		public void RemovePreRenderListener(Action listener)
		{
			bool flag = listener == null;
			if (!flag)
			{
				this.preRenderListeners = (Action)Delegate.Remove(this.preRenderListeners, listener);
			}
		}

		// Token: 0x060000E4 RID: 228 RVA: 0x00006B80 File Offset: 0x00004D80
		public void RemovePostRenderListener(Action listener)
		{
			bool flag = listener == null;
			if (!flag)
			{
				this.postRenderListeners = (Action)Delegate.Remove(this.postRenderListeners, listener);
			}
		}

		// Token: 0x04000156 RID: 342
		public Transform canvasOrigin;

		// Token: 0x04000157 RID: 343
		[SerializeField]
		private Vector3 m_canvasOriginWorldPosition = new Vector3(0f, 0f, 0f);

		// Token: 0x04000158 RID: 344
		[SerializeField]
		private Vector3 m_canvasOriginWorldRotation = new Vector3(0f, 0f, 0f);

		// Token: 0x04000159 RID: 345
		public Transform anchorTransform;

		// Token: 0x0400015A RID: 346
		[SerializeField]
		private Vector3 m_anchorWorldPosition = new Vector3(0f, 0f, 0f);

		// Token: 0x0400015B RID: 347
		[SerializeField]
		private Vector3 m_anchorWorldRotation = new Vector3(0f, 0f, 0f);

		// Token: 0x0400015C RID: 348
		private bool canvasVisible = false;

		// Token: 0x0400015D RID: 349
		public bool shouldRender = true;

		// Token: 0x0400015E RID: 350
		public bool useObliqueClip = true;

		// Token: 0x0400015F RID: 351
		public bool useScissor = true;

		// Token: 0x04000160 RID: 352
		public GameObject stereoCameraHead = null;

		// Token: 0x04000161 RID: 353
		public Camera stereoCameraEye = null;

		// Token: 0x04000162 RID: 354
		private Dictionary<int, RenderTexture> leftEyeTextures = new Dictionary<int, RenderTexture>();

		// Token: 0x04000163 RID: 355
		private Dictionary<int, RenderTexture> rightEyeTextures = new Dictionary<int, RenderTexture>();

		// Token: 0x04000164 RID: 356
		public float textureResolutionScale = 1f;

		// Token: 0x04000165 RID: 357
		private Material stereoMaterial;

		// Token: 0x04000166 RID: 358
		[SerializeField]
		private List<GameObject> ignoreWhenRender = new List<GameObject>();

		// Token: 0x04000167 RID: 359
		private List<int> ignoreObjOriginalLayer = new List<int>();

		// Token: 0x04000168 RID: 360
		public float reflectionOffset = 0.05f;

		// Token: 0x04000169 RID: 361
		private Rect fullViewport = new Rect(0f, 0f, 1f, 1f);

		// Token: 0x0400016A RID: 362
		public bool isMirror = false;

		// Token: 0x0400016B RID: 363
		private Matrix4x4 reflectionMat;

		// Token: 0x0400016C RID: 364
		private Action preRenderListeners;

		// Token: 0x0400016D RID: 365
		private Action postRenderListeners;
	}
}
