using System;
using System.Runtime.InteropServices;
using System.Text;

namespace Valve.VR
{
	// Token: 0x0200001B RID: 27
	public class CVRRenderModels
	{
		// Token: 0x060000FF RID: 255 RVA: 0x0000387F File Offset: 0x00001A7F
		internal CVRRenderModels(IntPtr pInterface)
		{
			this.FnTable = (IVRRenderModels)Marshal.PtrToStructure(pInterface, typeof(IVRRenderModels));
		}

		// Token: 0x06000100 RID: 256 RVA: 0x000038A2 File Offset: 0x00001AA2
		public EVRRenderModelError LoadRenderModel_Async(string pchRenderModelName, ref IntPtr ppRenderModel)
		{
			return this.FnTable.LoadRenderModel_Async(pchRenderModelName, ref ppRenderModel);
		}

		// Token: 0x06000101 RID: 257 RVA: 0x000038B6 File Offset: 0x00001AB6
		public void FreeRenderModel(IntPtr pRenderModel)
		{
			this.FnTable.FreeRenderModel(pRenderModel);
		}

		// Token: 0x06000102 RID: 258 RVA: 0x000038C9 File Offset: 0x00001AC9
		public EVRRenderModelError LoadTexture_Async(int textureId, ref IntPtr ppTexture)
		{
			return this.FnTable.LoadTexture_Async(textureId, ref ppTexture);
		}

		// Token: 0x06000103 RID: 259 RVA: 0x000038DD File Offset: 0x00001ADD
		public void FreeTexture(IntPtr pTexture)
		{
			this.FnTable.FreeTexture(pTexture);
		}

		// Token: 0x06000104 RID: 260 RVA: 0x000038F0 File Offset: 0x00001AF0
		public EVRRenderModelError LoadTextureD3D11_Async(int textureId, IntPtr pD3D11Device, ref IntPtr ppD3D11Texture2D)
		{
			return this.FnTable.LoadTextureD3D11_Async(textureId, pD3D11Device, ref ppD3D11Texture2D);
		}

		// Token: 0x06000105 RID: 261 RVA: 0x00003905 File Offset: 0x00001B05
		public EVRRenderModelError LoadIntoTextureD3D11_Async(int textureId, IntPtr pDstTexture)
		{
			return this.FnTable.LoadIntoTextureD3D11_Async(textureId, pDstTexture);
		}

		// Token: 0x06000106 RID: 262 RVA: 0x00003919 File Offset: 0x00001B19
		public void FreeTextureD3D11(IntPtr pD3D11Texture2D)
		{
			this.FnTable.FreeTextureD3D11(pD3D11Texture2D);
		}

		// Token: 0x06000107 RID: 263 RVA: 0x0000392C File Offset: 0x00001B2C
		public uint GetRenderModelName(uint unRenderModelIndex, StringBuilder pchRenderModelName, uint unRenderModelNameLen)
		{
			return this.FnTable.GetRenderModelName(unRenderModelIndex, pchRenderModelName, unRenderModelNameLen);
		}

		// Token: 0x06000108 RID: 264 RVA: 0x00003941 File Offset: 0x00001B41
		public uint GetRenderModelCount()
		{
			return this.FnTable.GetRenderModelCount();
		}

		// Token: 0x06000109 RID: 265 RVA: 0x00003953 File Offset: 0x00001B53
		public uint GetComponentCount(string pchRenderModelName)
		{
			return this.FnTable.GetComponentCount(pchRenderModelName);
		}

		// Token: 0x0600010A RID: 266 RVA: 0x00003966 File Offset: 0x00001B66
		public uint GetComponentName(string pchRenderModelName, uint unComponentIndex, StringBuilder pchComponentName, uint unComponentNameLen)
		{
			return this.FnTable.GetComponentName(pchRenderModelName, unComponentIndex, pchComponentName, unComponentNameLen);
		}

		// Token: 0x0600010B RID: 267 RVA: 0x0000397D File Offset: 0x00001B7D
		public ulong GetComponentButtonMask(string pchRenderModelName, string pchComponentName)
		{
			return this.FnTable.GetComponentButtonMask(pchRenderModelName, pchComponentName);
		}

		// Token: 0x0600010C RID: 268 RVA: 0x00003991 File Offset: 0x00001B91
		public uint GetComponentRenderModelName(string pchRenderModelName, string pchComponentName, StringBuilder pchComponentRenderModelName, uint unComponentRenderModelNameLen)
		{
			return this.FnTable.GetComponentRenderModelName(pchRenderModelName, pchComponentName, pchComponentRenderModelName, unComponentRenderModelNameLen);
		}

		// Token: 0x0600010D RID: 269 RVA: 0x000039A8 File Offset: 0x00001BA8
		public bool GetComponentStateForDevicePath(string pchRenderModelName, string pchComponentName, ulong devicePath, ref RenderModel_ControllerMode_State_t pState, ref RenderModel_ComponentState_t pComponentState)
		{
			return this.FnTable.GetComponentStateForDevicePath(pchRenderModelName, pchComponentName, devicePath, ref pState, ref pComponentState);
		}

		// Token: 0x0600010E RID: 270 RVA: 0x000039C4 File Offset: 0x00001BC4
		public bool GetComponentState(string pchRenderModelName, string pchComponentName, ref VRControllerState_t pControllerState, ref RenderModel_ControllerMode_State_t pState, ref RenderModel_ComponentState_t pComponentState)
		{
			if (Environment.OSVersion.Platform == PlatformID.MacOSX || Environment.OSVersion.Platform == PlatformID.Unix)
			{
				VRControllerState_t_Packed vrcontrollerState_t_Packed = new VRControllerState_t_Packed(pControllerState);
				CVRRenderModels.GetComponentStateUnion getComponentStateUnion;
				getComponentStateUnion.pGetComponentStatePacked = null;
				getComponentStateUnion.pGetComponentState = this.FnTable.GetComponentState;
				bool flag = getComponentStateUnion.pGetComponentStatePacked(pchRenderModelName, pchComponentName, ref vrcontrollerState_t_Packed, ref pState, ref pComponentState);
				vrcontrollerState_t_Packed.Unpack(ref pControllerState);
				return flag;
			}
			return this.FnTable.GetComponentState(pchRenderModelName, pchComponentName, ref pControllerState, ref pState, ref pComponentState);
		}

		// Token: 0x0600010F RID: 271 RVA: 0x00003A45 File Offset: 0x00001C45
		public bool RenderModelHasComponent(string pchRenderModelName, string pchComponentName)
		{
			return this.FnTable.RenderModelHasComponent(pchRenderModelName, pchComponentName);
		}

		// Token: 0x06000110 RID: 272 RVA: 0x00003A59 File Offset: 0x00001C59
		public uint GetRenderModelThumbnailURL(string pchRenderModelName, StringBuilder pchThumbnailURL, uint unThumbnailURLLen, ref EVRRenderModelError peError)
		{
			return this.FnTable.GetRenderModelThumbnailURL(pchRenderModelName, pchThumbnailURL, unThumbnailURLLen, ref peError);
		}

		// Token: 0x06000111 RID: 273 RVA: 0x00003A70 File Offset: 0x00001C70
		public uint GetRenderModelOriginalPath(string pchRenderModelName, StringBuilder pchOriginalPath, uint unOriginalPathLen, ref EVRRenderModelError peError)
		{
			return this.FnTable.GetRenderModelOriginalPath(pchRenderModelName, pchOriginalPath, unOriginalPathLen, ref peError);
		}

		// Token: 0x06000112 RID: 274 RVA: 0x00003A87 File Offset: 0x00001C87
		public string GetRenderModelErrorNameFromEnum(EVRRenderModelError error)
		{
			return Marshal.PtrToStringAnsi(this.FnTable.GetRenderModelErrorNameFromEnum(error));
		}

		// Token: 0x0400014F RID: 335
		private IVRRenderModels FnTable;

		// Token: 0x02000237 RID: 567
		// (Invoke) Token: 0x06000780 RID: 1920
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate bool _GetComponentStatePacked(string pchRenderModelName, string pchComponentName, ref VRControllerState_t_Packed pControllerState, ref RenderModel_ControllerMode_State_t pState, ref RenderModel_ComponentState_t pComponentState);

		// Token: 0x02000238 RID: 568
		[StructLayout(LayoutKind.Explicit)]
		private struct GetComponentStateUnion
		{
			// Token: 0x040008B0 RID: 2224
			[FieldOffset(0)]
			public IVRRenderModels._GetComponentState pGetComponentState;

			// Token: 0x040008B1 RID: 2225
			[FieldOffset(0)]
			public CVRRenderModels._GetComponentStatePacked pGetComponentStatePacked;
		}
	}
}
