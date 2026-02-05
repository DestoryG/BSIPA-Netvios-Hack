using System;
using System.Runtime.InteropServices;
using System.Text;

namespace Valve.VR
{
	// Token: 0x0200000A RID: 10
	public struct IVRRenderModels
	{
		// Token: 0x040000F7 RID: 247
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVRRenderModels._LoadRenderModel_Async LoadRenderModel_Async;

		// Token: 0x040000F8 RID: 248
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVRRenderModels._FreeRenderModel FreeRenderModel;

		// Token: 0x040000F9 RID: 249
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVRRenderModels._LoadTexture_Async LoadTexture_Async;

		// Token: 0x040000FA RID: 250
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVRRenderModels._FreeTexture FreeTexture;

		// Token: 0x040000FB RID: 251
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVRRenderModels._LoadTextureD3D11_Async LoadTextureD3D11_Async;

		// Token: 0x040000FC RID: 252
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVRRenderModels._LoadIntoTextureD3D11_Async LoadIntoTextureD3D11_Async;

		// Token: 0x040000FD RID: 253
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVRRenderModels._FreeTextureD3D11 FreeTextureD3D11;

		// Token: 0x040000FE RID: 254
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVRRenderModels._GetRenderModelName GetRenderModelName;

		// Token: 0x040000FF RID: 255
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVRRenderModels._GetRenderModelCount GetRenderModelCount;

		// Token: 0x04000100 RID: 256
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVRRenderModels._GetComponentCount GetComponentCount;

		// Token: 0x04000101 RID: 257
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVRRenderModels._GetComponentName GetComponentName;

		// Token: 0x04000102 RID: 258
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVRRenderModels._GetComponentButtonMask GetComponentButtonMask;

		// Token: 0x04000103 RID: 259
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVRRenderModels._GetComponentRenderModelName GetComponentRenderModelName;

		// Token: 0x04000104 RID: 260
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVRRenderModels._GetComponentStateForDevicePath GetComponentStateForDevicePath;

		// Token: 0x04000105 RID: 261
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVRRenderModels._GetComponentState GetComponentState;

		// Token: 0x04000106 RID: 262
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVRRenderModels._RenderModelHasComponent RenderModelHasComponent;

		// Token: 0x04000107 RID: 263
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVRRenderModels._GetRenderModelThumbnailURL GetRenderModelThumbnailURL;

		// Token: 0x04000108 RID: 264
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVRRenderModels._GetRenderModelOriginalPath GetRenderModelOriginalPath;

		// Token: 0x04000109 RID: 265
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVRRenderModels._GetRenderModelErrorNameFromEnum GetRenderModelErrorNameFromEnum;

		// Token: 0x020001DF RID: 479
		// (Invoke) Token: 0x06000630 RID: 1584
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate EVRRenderModelError _LoadRenderModel_Async(string pchRenderModelName, ref IntPtr ppRenderModel);

		// Token: 0x020001E0 RID: 480
		// (Invoke) Token: 0x06000634 RID: 1588
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate void _FreeRenderModel(IntPtr pRenderModel);

		// Token: 0x020001E1 RID: 481
		// (Invoke) Token: 0x06000638 RID: 1592
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate EVRRenderModelError _LoadTexture_Async(int textureId, ref IntPtr ppTexture);

		// Token: 0x020001E2 RID: 482
		// (Invoke) Token: 0x0600063C RID: 1596
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate void _FreeTexture(IntPtr pTexture);

		// Token: 0x020001E3 RID: 483
		// (Invoke) Token: 0x06000640 RID: 1600
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate EVRRenderModelError _LoadTextureD3D11_Async(int textureId, IntPtr pD3D11Device, ref IntPtr ppD3D11Texture2D);

		// Token: 0x020001E4 RID: 484
		// (Invoke) Token: 0x06000644 RID: 1604
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate EVRRenderModelError _LoadIntoTextureD3D11_Async(int textureId, IntPtr pDstTexture);

		// Token: 0x020001E5 RID: 485
		// (Invoke) Token: 0x06000648 RID: 1608
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate void _FreeTextureD3D11(IntPtr pD3D11Texture2D);

		// Token: 0x020001E6 RID: 486
		// (Invoke) Token: 0x0600064C RID: 1612
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate uint _GetRenderModelName(uint unRenderModelIndex, StringBuilder pchRenderModelName, uint unRenderModelNameLen);

		// Token: 0x020001E7 RID: 487
		// (Invoke) Token: 0x06000650 RID: 1616
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate uint _GetRenderModelCount();

		// Token: 0x020001E8 RID: 488
		// (Invoke) Token: 0x06000654 RID: 1620
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate uint _GetComponentCount(string pchRenderModelName);

		// Token: 0x020001E9 RID: 489
		// (Invoke) Token: 0x06000658 RID: 1624
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate uint _GetComponentName(string pchRenderModelName, uint unComponentIndex, StringBuilder pchComponentName, uint unComponentNameLen);

		// Token: 0x020001EA RID: 490
		// (Invoke) Token: 0x0600065C RID: 1628
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate ulong _GetComponentButtonMask(string pchRenderModelName, string pchComponentName);

		// Token: 0x020001EB RID: 491
		// (Invoke) Token: 0x06000660 RID: 1632
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate uint _GetComponentRenderModelName(string pchRenderModelName, string pchComponentName, StringBuilder pchComponentRenderModelName, uint unComponentRenderModelNameLen);

		// Token: 0x020001EC RID: 492
		// (Invoke) Token: 0x06000664 RID: 1636
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate bool _GetComponentStateForDevicePath(string pchRenderModelName, string pchComponentName, ulong devicePath, ref RenderModel_ControllerMode_State_t pState, ref RenderModel_ComponentState_t pComponentState);

		// Token: 0x020001ED RID: 493
		// (Invoke) Token: 0x06000668 RID: 1640
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate bool _GetComponentState(string pchRenderModelName, string pchComponentName, ref VRControllerState_t pControllerState, ref RenderModel_ControllerMode_State_t pState, ref RenderModel_ComponentState_t pComponentState);

		// Token: 0x020001EE RID: 494
		// (Invoke) Token: 0x0600066C RID: 1644
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate bool _RenderModelHasComponent(string pchRenderModelName, string pchComponentName);

		// Token: 0x020001EF RID: 495
		// (Invoke) Token: 0x06000670 RID: 1648
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate uint _GetRenderModelThumbnailURL(string pchRenderModelName, StringBuilder pchThumbnailURL, uint unThumbnailURLLen, ref EVRRenderModelError peError);

		// Token: 0x020001F0 RID: 496
		// (Invoke) Token: 0x06000674 RID: 1652
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate uint _GetRenderModelOriginalPath(string pchRenderModelName, StringBuilder pchOriginalPath, uint unOriginalPathLen, ref EVRRenderModelError peError);

		// Token: 0x020001F1 RID: 497
		// (Invoke) Token: 0x06000678 RID: 1656
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate IntPtr _GetRenderModelErrorNameFromEnum(EVRRenderModelError error);
	}
}
