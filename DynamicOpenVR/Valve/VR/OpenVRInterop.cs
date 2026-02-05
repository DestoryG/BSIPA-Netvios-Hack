using System;
using System.Runtime.InteropServices;

namespace Valve.VR
{
	// Token: 0x02000024 RID: 36
	public class OpenVRInterop
	{
		// Token: 0x06000158 RID: 344
		[DllImport("openvr_api", CallingConvention = CallingConvention.Cdecl, EntryPoint = "VR_InitInternal")]
		internal static extern uint InitInternal(ref EVRInitError peError, EVRApplicationType eApplicationType);

		// Token: 0x06000159 RID: 345
		[DllImport("openvr_api", CallingConvention = CallingConvention.Cdecl, EntryPoint = "VR_InitInternal2")]
		internal static extern uint InitInternal2(ref EVRInitError peError, EVRApplicationType eApplicationType, [MarshalAs(UnmanagedType.LPStr)] [In] string pStartupInfo);

		// Token: 0x0600015A RID: 346
		[DllImport("openvr_api", CallingConvention = CallingConvention.Cdecl, EntryPoint = "VR_ShutdownInternal")]
		internal static extern void ShutdownInternal();

		// Token: 0x0600015B RID: 347
		[DllImport("openvr_api", CallingConvention = CallingConvention.Cdecl, EntryPoint = "VR_IsHmdPresent")]
		internal static extern bool IsHmdPresent();

		// Token: 0x0600015C RID: 348
		[DllImport("openvr_api", CallingConvention = CallingConvention.Cdecl, EntryPoint = "VR_IsRuntimeInstalled")]
		internal static extern bool IsRuntimeInstalled();

		// Token: 0x0600015D RID: 349
		[DllImport("openvr_api", CallingConvention = CallingConvention.Cdecl, EntryPoint = "VR_RuntimePath")]
		internal static extern string RuntimePath();

		// Token: 0x0600015E RID: 350
		[DllImport("openvr_api", CallingConvention = CallingConvention.Cdecl, EntryPoint = "VR_GetStringForHmdError")]
		internal static extern IntPtr GetStringForHmdError(EVRInitError error);

		// Token: 0x0600015F RID: 351
		[DllImport("openvr_api", CallingConvention = CallingConvention.Cdecl, EntryPoint = "VR_GetGenericInterface")]
		internal static extern IntPtr GetGenericInterface([MarshalAs(UnmanagedType.LPStr)] [In] string pchInterfaceVersion, ref EVRInitError peError);

		// Token: 0x06000160 RID: 352
		[DllImport("openvr_api", CallingConvention = CallingConvention.Cdecl, EntryPoint = "VR_IsInterfaceVersionValid")]
		internal static extern bool IsInterfaceVersionValid([MarshalAs(UnmanagedType.LPStr)] [In] string pchInterfaceVersion);

		// Token: 0x06000161 RID: 353
		[DllImport("openvr_api", CallingConvention = CallingConvention.Cdecl, EntryPoint = "VR_GetInitToken")]
		internal static extern uint GetInitToken();
	}
}
