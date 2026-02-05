using System;
using System.Runtime.InteropServices;
using System.Text;

namespace Valve.VR
{
	// Token: 0x02000005 RID: 5
	public struct IVRApplications
	{
		// Token: 0x0400003F RID: 63
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVRApplications._AddApplicationManifest AddApplicationManifest;

		// Token: 0x04000040 RID: 64
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVRApplications._RemoveApplicationManifest RemoveApplicationManifest;

		// Token: 0x04000041 RID: 65
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVRApplications._IsApplicationInstalled IsApplicationInstalled;

		// Token: 0x04000042 RID: 66
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVRApplications._GetApplicationCount GetApplicationCount;

		// Token: 0x04000043 RID: 67
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVRApplications._GetApplicationKeyByIndex GetApplicationKeyByIndex;

		// Token: 0x04000044 RID: 68
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVRApplications._GetApplicationKeyByProcessId GetApplicationKeyByProcessId;

		// Token: 0x04000045 RID: 69
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVRApplications._LaunchApplication LaunchApplication;

		// Token: 0x04000046 RID: 70
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVRApplications._LaunchTemplateApplication LaunchTemplateApplication;

		// Token: 0x04000047 RID: 71
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVRApplications._LaunchApplicationFromMimeType LaunchApplicationFromMimeType;

		// Token: 0x04000048 RID: 72
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVRApplications._LaunchDashboardOverlay LaunchDashboardOverlay;

		// Token: 0x04000049 RID: 73
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVRApplications._CancelApplicationLaunch CancelApplicationLaunch;

		// Token: 0x0400004A RID: 74
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVRApplications._IdentifyApplication IdentifyApplication;

		// Token: 0x0400004B RID: 75
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVRApplications._GetApplicationProcessId GetApplicationProcessId;

		// Token: 0x0400004C RID: 76
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVRApplications._GetApplicationsErrorNameFromEnum GetApplicationsErrorNameFromEnum;

		// Token: 0x0400004D RID: 77
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVRApplications._GetApplicationPropertyString GetApplicationPropertyString;

		// Token: 0x0400004E RID: 78
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVRApplications._GetApplicationPropertyBool GetApplicationPropertyBool;

		// Token: 0x0400004F RID: 79
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVRApplications._GetApplicationPropertyUint64 GetApplicationPropertyUint64;

		// Token: 0x04000050 RID: 80
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVRApplications._SetApplicationAutoLaunch SetApplicationAutoLaunch;

		// Token: 0x04000051 RID: 81
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVRApplications._GetApplicationAutoLaunch GetApplicationAutoLaunch;

		// Token: 0x04000052 RID: 82
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVRApplications._SetDefaultApplicationForMimeType SetDefaultApplicationForMimeType;

		// Token: 0x04000053 RID: 83
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVRApplications._GetDefaultApplicationForMimeType GetDefaultApplicationForMimeType;

		// Token: 0x04000054 RID: 84
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVRApplications._GetApplicationSupportedMimeTypes GetApplicationSupportedMimeTypes;

		// Token: 0x04000055 RID: 85
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVRApplications._GetApplicationsThatSupportMimeType GetApplicationsThatSupportMimeType;

		// Token: 0x04000056 RID: 86
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVRApplications._GetApplicationLaunchArguments GetApplicationLaunchArguments;

		// Token: 0x04000057 RID: 87
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVRApplications._GetStartingApplication GetStartingApplication;

		// Token: 0x04000058 RID: 88
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVRApplications._GetTransitionState GetTransitionState;

		// Token: 0x04000059 RID: 89
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVRApplications._PerformApplicationPrelaunchCheck PerformApplicationPrelaunchCheck;

		// Token: 0x0400005A RID: 90
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVRApplications._GetApplicationsTransitionStateNameFromEnum GetApplicationsTransitionStateNameFromEnum;

		// Token: 0x0400005B RID: 91
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVRApplications._IsQuitUserPromptRequested IsQuitUserPromptRequested;

		// Token: 0x0400005C RID: 92
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVRApplications._LaunchInternalProcess LaunchInternalProcess;

		// Token: 0x0400005D RID: 93
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVRApplications._GetCurrentSceneProcessId GetCurrentSceneProcessId;

		// Token: 0x02000127 RID: 295
		// (Invoke) Token: 0x06000350 RID: 848
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate EVRApplicationError _AddApplicationManifest(string pchApplicationManifestFullPath, bool bTemporary);

		// Token: 0x02000128 RID: 296
		// (Invoke) Token: 0x06000354 RID: 852
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate EVRApplicationError _RemoveApplicationManifest(string pchApplicationManifestFullPath);

		// Token: 0x02000129 RID: 297
		// (Invoke) Token: 0x06000358 RID: 856
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate bool _IsApplicationInstalled(string pchAppKey);

		// Token: 0x0200012A RID: 298
		// (Invoke) Token: 0x0600035C RID: 860
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate uint _GetApplicationCount();

		// Token: 0x0200012B RID: 299
		// (Invoke) Token: 0x06000360 RID: 864
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate EVRApplicationError _GetApplicationKeyByIndex(uint unApplicationIndex, StringBuilder pchAppKeyBuffer, uint unAppKeyBufferLen);

		// Token: 0x0200012C RID: 300
		// (Invoke) Token: 0x06000364 RID: 868
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate EVRApplicationError _GetApplicationKeyByProcessId(uint unProcessId, StringBuilder pchAppKeyBuffer, uint unAppKeyBufferLen);

		// Token: 0x0200012D RID: 301
		// (Invoke) Token: 0x06000368 RID: 872
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate EVRApplicationError _LaunchApplication(string pchAppKey);

		// Token: 0x0200012E RID: 302
		// (Invoke) Token: 0x0600036C RID: 876
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate EVRApplicationError _LaunchTemplateApplication(string pchTemplateAppKey, string pchNewAppKey, [In] [Out] AppOverrideKeys_t[] pKeys, uint unKeys);

		// Token: 0x0200012F RID: 303
		// (Invoke) Token: 0x06000370 RID: 880
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate EVRApplicationError _LaunchApplicationFromMimeType(string pchMimeType, string pchArgs);

		// Token: 0x02000130 RID: 304
		// (Invoke) Token: 0x06000374 RID: 884
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate EVRApplicationError _LaunchDashboardOverlay(string pchAppKey);

		// Token: 0x02000131 RID: 305
		// (Invoke) Token: 0x06000378 RID: 888
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate bool _CancelApplicationLaunch(string pchAppKey);

		// Token: 0x02000132 RID: 306
		// (Invoke) Token: 0x0600037C RID: 892
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate EVRApplicationError _IdentifyApplication(uint unProcessId, string pchAppKey);

		// Token: 0x02000133 RID: 307
		// (Invoke) Token: 0x06000380 RID: 896
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate uint _GetApplicationProcessId(string pchAppKey);

		// Token: 0x02000134 RID: 308
		// (Invoke) Token: 0x06000384 RID: 900
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate IntPtr _GetApplicationsErrorNameFromEnum(EVRApplicationError error);

		// Token: 0x02000135 RID: 309
		// (Invoke) Token: 0x06000388 RID: 904
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate uint _GetApplicationPropertyString(string pchAppKey, EVRApplicationProperty eProperty, StringBuilder pchPropertyValueBuffer, uint unPropertyValueBufferLen, ref EVRApplicationError peError);

		// Token: 0x02000136 RID: 310
		// (Invoke) Token: 0x0600038C RID: 908
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate bool _GetApplicationPropertyBool(string pchAppKey, EVRApplicationProperty eProperty, ref EVRApplicationError peError);

		// Token: 0x02000137 RID: 311
		// (Invoke) Token: 0x06000390 RID: 912
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate ulong _GetApplicationPropertyUint64(string pchAppKey, EVRApplicationProperty eProperty, ref EVRApplicationError peError);

		// Token: 0x02000138 RID: 312
		// (Invoke) Token: 0x06000394 RID: 916
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate EVRApplicationError _SetApplicationAutoLaunch(string pchAppKey, bool bAutoLaunch);

		// Token: 0x02000139 RID: 313
		// (Invoke) Token: 0x06000398 RID: 920
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate bool _GetApplicationAutoLaunch(string pchAppKey);

		// Token: 0x0200013A RID: 314
		// (Invoke) Token: 0x0600039C RID: 924
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate EVRApplicationError _SetDefaultApplicationForMimeType(string pchAppKey, string pchMimeType);

		// Token: 0x0200013B RID: 315
		// (Invoke) Token: 0x060003A0 RID: 928
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate bool _GetDefaultApplicationForMimeType(string pchMimeType, StringBuilder pchAppKeyBuffer, uint unAppKeyBufferLen);

		// Token: 0x0200013C RID: 316
		// (Invoke) Token: 0x060003A4 RID: 932
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate bool _GetApplicationSupportedMimeTypes(string pchAppKey, StringBuilder pchMimeTypesBuffer, uint unMimeTypesBuffer);

		// Token: 0x0200013D RID: 317
		// (Invoke) Token: 0x060003A8 RID: 936
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate uint _GetApplicationsThatSupportMimeType(string pchMimeType, StringBuilder pchAppKeysThatSupportBuffer, uint unAppKeysThatSupportBuffer);

		// Token: 0x0200013E RID: 318
		// (Invoke) Token: 0x060003AC RID: 940
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate uint _GetApplicationLaunchArguments(uint unHandle, StringBuilder pchArgs, uint unArgs);

		// Token: 0x0200013F RID: 319
		// (Invoke) Token: 0x060003B0 RID: 944
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate EVRApplicationError _GetStartingApplication(StringBuilder pchAppKeyBuffer, uint unAppKeyBufferLen);

		// Token: 0x02000140 RID: 320
		// (Invoke) Token: 0x060003B4 RID: 948
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate EVRApplicationTransitionState _GetTransitionState();

		// Token: 0x02000141 RID: 321
		// (Invoke) Token: 0x060003B8 RID: 952
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate EVRApplicationError _PerformApplicationPrelaunchCheck(string pchAppKey);

		// Token: 0x02000142 RID: 322
		// (Invoke) Token: 0x060003BC RID: 956
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate IntPtr _GetApplicationsTransitionStateNameFromEnum(EVRApplicationTransitionState state);

		// Token: 0x02000143 RID: 323
		// (Invoke) Token: 0x060003C0 RID: 960
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate bool _IsQuitUserPromptRequested();

		// Token: 0x02000144 RID: 324
		// (Invoke) Token: 0x060003C4 RID: 964
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate EVRApplicationError _LaunchInternalProcess(string pchBinaryPath, string pchArguments, string pchWorkingDirectory);

		// Token: 0x02000145 RID: 325
		// (Invoke) Token: 0x060003C8 RID: 968
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate uint _GetCurrentSceneProcessId();
	}
}
