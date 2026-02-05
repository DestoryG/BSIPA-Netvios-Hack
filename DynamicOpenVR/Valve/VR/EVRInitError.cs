using System;

namespace Valve.VR
{
	// Token: 0x02000044 RID: 68
	public enum EVRInitError
	{
		// Token: 0x04000396 RID: 918
		None,
		// Token: 0x04000397 RID: 919
		Unknown,
		// Token: 0x04000398 RID: 920
		Init_InstallationNotFound = 100,
		// Token: 0x04000399 RID: 921
		Init_InstallationCorrupt,
		// Token: 0x0400039A RID: 922
		Init_VRClientDLLNotFound,
		// Token: 0x0400039B RID: 923
		Init_FileNotFound,
		// Token: 0x0400039C RID: 924
		Init_FactoryNotFound,
		// Token: 0x0400039D RID: 925
		Init_InterfaceNotFound,
		// Token: 0x0400039E RID: 926
		Init_InvalidInterface,
		// Token: 0x0400039F RID: 927
		Init_UserConfigDirectoryInvalid,
		// Token: 0x040003A0 RID: 928
		Init_HmdNotFound,
		// Token: 0x040003A1 RID: 929
		Init_NotInitialized,
		// Token: 0x040003A2 RID: 930
		Init_PathRegistryNotFound,
		// Token: 0x040003A3 RID: 931
		Init_NoConfigPath,
		// Token: 0x040003A4 RID: 932
		Init_NoLogPath,
		// Token: 0x040003A5 RID: 933
		Init_PathRegistryNotWritable,
		// Token: 0x040003A6 RID: 934
		Init_AppInfoInitFailed,
		// Token: 0x040003A7 RID: 935
		Init_Retry,
		// Token: 0x040003A8 RID: 936
		Init_InitCanceledByUser,
		// Token: 0x040003A9 RID: 937
		Init_AnotherAppLaunching,
		// Token: 0x040003AA RID: 938
		Init_SettingsInitFailed,
		// Token: 0x040003AB RID: 939
		Init_ShuttingDown,
		// Token: 0x040003AC RID: 940
		Init_TooManyObjects,
		// Token: 0x040003AD RID: 941
		Init_NoServerForBackgroundApp,
		// Token: 0x040003AE RID: 942
		Init_NotSupportedWithCompositor,
		// Token: 0x040003AF RID: 943
		Init_NotAvailableToUtilityApps,
		// Token: 0x040003B0 RID: 944
		Init_Internal,
		// Token: 0x040003B1 RID: 945
		Init_HmdDriverIdIsNone,
		// Token: 0x040003B2 RID: 946
		Init_HmdNotFoundPresenceFailed,
		// Token: 0x040003B3 RID: 947
		Init_VRMonitorNotFound,
		// Token: 0x040003B4 RID: 948
		Init_VRMonitorStartupFailed,
		// Token: 0x040003B5 RID: 949
		Init_LowPowerWatchdogNotSupported,
		// Token: 0x040003B6 RID: 950
		Init_InvalidApplicationType,
		// Token: 0x040003B7 RID: 951
		Init_NotAvailableToWatchdogApps,
		// Token: 0x040003B8 RID: 952
		Init_WatchdogDisabledInSettings,
		// Token: 0x040003B9 RID: 953
		Init_VRDashboardNotFound,
		// Token: 0x040003BA RID: 954
		Init_VRDashboardStartupFailed,
		// Token: 0x040003BB RID: 955
		Init_VRHomeNotFound,
		// Token: 0x040003BC RID: 956
		Init_VRHomeStartupFailed,
		// Token: 0x040003BD RID: 957
		Init_RebootingBusy,
		// Token: 0x040003BE RID: 958
		Init_FirmwareUpdateBusy,
		// Token: 0x040003BF RID: 959
		Init_FirmwareRecoveryBusy,
		// Token: 0x040003C0 RID: 960
		Init_USBServiceBusy,
		// Token: 0x040003C1 RID: 961
		Init_VRWebHelperStartupFailed,
		// Token: 0x040003C2 RID: 962
		Init_TrackerManagerInitFailed,
		// Token: 0x040003C3 RID: 963
		Init_AlreadyRunning,
		// Token: 0x040003C4 RID: 964
		Driver_Failed = 200,
		// Token: 0x040003C5 RID: 965
		Driver_Unknown,
		// Token: 0x040003C6 RID: 966
		Driver_HmdUnknown,
		// Token: 0x040003C7 RID: 967
		Driver_NotLoaded,
		// Token: 0x040003C8 RID: 968
		Driver_RuntimeOutOfDate,
		// Token: 0x040003C9 RID: 969
		Driver_HmdInUse,
		// Token: 0x040003CA RID: 970
		Driver_NotCalibrated,
		// Token: 0x040003CB RID: 971
		Driver_CalibrationInvalid,
		// Token: 0x040003CC RID: 972
		Driver_HmdDisplayNotFound,
		// Token: 0x040003CD RID: 973
		Driver_TrackedDeviceInterfaceUnknown,
		// Token: 0x040003CE RID: 974
		Driver_HmdDriverIdOutOfBounds = 211,
		// Token: 0x040003CF RID: 975
		Driver_HmdDisplayMirrored,
		// Token: 0x040003D0 RID: 976
		IPC_ServerInitFailed = 300,
		// Token: 0x040003D1 RID: 977
		IPC_ConnectFailed,
		// Token: 0x040003D2 RID: 978
		IPC_SharedStateInitFailed,
		// Token: 0x040003D3 RID: 979
		IPC_CompositorInitFailed,
		// Token: 0x040003D4 RID: 980
		IPC_MutexInitFailed,
		// Token: 0x040003D5 RID: 981
		IPC_Failed,
		// Token: 0x040003D6 RID: 982
		IPC_CompositorConnectFailed,
		// Token: 0x040003D7 RID: 983
		IPC_CompositorInvalidConnectResponse,
		// Token: 0x040003D8 RID: 984
		IPC_ConnectFailedAfterMultipleAttempts,
		// Token: 0x040003D9 RID: 985
		Compositor_Failed = 400,
		// Token: 0x040003DA RID: 986
		Compositor_D3D11HardwareRequired,
		// Token: 0x040003DB RID: 987
		Compositor_FirmwareRequiresUpdate,
		// Token: 0x040003DC RID: 988
		Compositor_OverlayInitFailed,
		// Token: 0x040003DD RID: 989
		Compositor_ScreenshotsInitFailed,
		// Token: 0x040003DE RID: 990
		Compositor_UnableToCreateDevice,
		// Token: 0x040003DF RID: 991
		Compositor_SharedStateIsNull,
		// Token: 0x040003E0 RID: 992
		Compositor_NotificationManagerIsNull,
		// Token: 0x040003E1 RID: 993
		Compositor_ResourceManagerClientIsNull,
		// Token: 0x040003E2 RID: 994
		Compositor_MessageOverlaySharedStateInitFailure,
		// Token: 0x040003E3 RID: 995
		Compositor_PropertiesInterfaceIsNull,
		// Token: 0x040003E4 RID: 996
		Compositor_CreateFullscreenWindowFailed,
		// Token: 0x040003E5 RID: 997
		Compositor_SettingsInterfaceIsNull,
		// Token: 0x040003E6 RID: 998
		Compositor_FailedToShowWindow,
		// Token: 0x040003E7 RID: 999
		Compositor_DistortInterfaceIsNull,
		// Token: 0x040003E8 RID: 1000
		Compositor_DisplayFrequencyFailure,
		// Token: 0x040003E9 RID: 1001
		Compositor_RendererInitializationFailed,
		// Token: 0x040003EA RID: 1002
		Compositor_DXGIFactoryInterfaceIsNull,
		// Token: 0x040003EB RID: 1003
		Compositor_DXGIFactoryCreateFailed,
		// Token: 0x040003EC RID: 1004
		Compositor_DXGIFactoryQueryFailed,
		// Token: 0x040003ED RID: 1005
		Compositor_InvalidAdapterDesktop,
		// Token: 0x040003EE RID: 1006
		Compositor_InvalidHmdAttachment,
		// Token: 0x040003EF RID: 1007
		Compositor_InvalidOutputDesktop,
		// Token: 0x040003F0 RID: 1008
		Compositor_InvalidDeviceProvided,
		// Token: 0x040003F1 RID: 1009
		Compositor_D3D11RendererInitializationFailed,
		// Token: 0x040003F2 RID: 1010
		Compositor_FailedToFindDisplayMode,
		// Token: 0x040003F3 RID: 1011
		Compositor_FailedToCreateSwapChain,
		// Token: 0x040003F4 RID: 1012
		Compositor_FailedToGetBackBuffer,
		// Token: 0x040003F5 RID: 1013
		Compositor_FailedToCreateRenderTarget,
		// Token: 0x040003F6 RID: 1014
		Compositor_FailedToCreateDXGI2SwapChain,
		// Token: 0x040003F7 RID: 1015
		Compositor_FailedtoGetDXGI2BackBuffer,
		// Token: 0x040003F8 RID: 1016
		Compositor_FailedToCreateDXGI2RenderTarget,
		// Token: 0x040003F9 RID: 1017
		Compositor_FailedToGetDXGIDeviceInterface,
		// Token: 0x040003FA RID: 1018
		Compositor_SelectDisplayMode,
		// Token: 0x040003FB RID: 1019
		Compositor_FailedToCreateNvAPIRenderTargets,
		// Token: 0x040003FC RID: 1020
		Compositor_NvAPISetDisplayMode,
		// Token: 0x040003FD RID: 1021
		Compositor_FailedToCreateDirectModeDisplay,
		// Token: 0x040003FE RID: 1022
		Compositor_InvalidHmdPropertyContainer,
		// Token: 0x040003FF RID: 1023
		Compositor_UpdateDisplayFrequency,
		// Token: 0x04000400 RID: 1024
		Compositor_CreateRasterizerState,
		// Token: 0x04000401 RID: 1025
		Compositor_CreateWireframeRasterizerState,
		// Token: 0x04000402 RID: 1026
		Compositor_CreateSamplerState,
		// Token: 0x04000403 RID: 1027
		Compositor_CreateClampToBorderSamplerState,
		// Token: 0x04000404 RID: 1028
		Compositor_CreateAnisoSamplerState,
		// Token: 0x04000405 RID: 1029
		Compositor_CreateOverlaySamplerState,
		// Token: 0x04000406 RID: 1030
		Compositor_CreatePanoramaSamplerState,
		// Token: 0x04000407 RID: 1031
		Compositor_CreateFontSamplerState,
		// Token: 0x04000408 RID: 1032
		Compositor_CreateNoBlendState,
		// Token: 0x04000409 RID: 1033
		Compositor_CreateBlendState,
		// Token: 0x0400040A RID: 1034
		Compositor_CreateAlphaBlendState,
		// Token: 0x0400040B RID: 1035
		Compositor_CreateBlendStateMaskR,
		// Token: 0x0400040C RID: 1036
		Compositor_CreateBlendStateMaskG,
		// Token: 0x0400040D RID: 1037
		Compositor_CreateBlendStateMaskB,
		// Token: 0x0400040E RID: 1038
		Compositor_CreateDepthStencilState,
		// Token: 0x0400040F RID: 1039
		Compositor_CreateDepthStencilStateNoWrite,
		// Token: 0x04000410 RID: 1040
		Compositor_CreateDepthStencilStateNoDepth,
		// Token: 0x04000411 RID: 1041
		Compositor_CreateFlushTexture,
		// Token: 0x04000412 RID: 1042
		Compositor_CreateDistortionSurfaces,
		// Token: 0x04000413 RID: 1043
		Compositor_CreateConstantBuffer,
		// Token: 0x04000414 RID: 1044
		Compositor_CreateHmdPoseConstantBuffer,
		// Token: 0x04000415 RID: 1045
		Compositor_CreateHmdPoseStagingConstantBuffer,
		// Token: 0x04000416 RID: 1046
		Compositor_CreateSharedFrameInfoConstantBuffer,
		// Token: 0x04000417 RID: 1047
		Compositor_CreateOverlayConstantBuffer,
		// Token: 0x04000418 RID: 1048
		Compositor_CreateSceneTextureIndexConstantBuffer,
		// Token: 0x04000419 RID: 1049
		Compositor_CreateReadableSceneTextureIndexConstantBuffer,
		// Token: 0x0400041A RID: 1050
		Compositor_CreateLayerGraphicsTextureIndexConstantBuffer,
		// Token: 0x0400041B RID: 1051
		Compositor_CreateLayerComputeTextureIndexConstantBuffer,
		// Token: 0x0400041C RID: 1052
		Compositor_CreateLayerComputeSceneTextureIndexConstantBuffer,
		// Token: 0x0400041D RID: 1053
		Compositor_CreateComputeHmdPoseConstantBuffer,
		// Token: 0x0400041E RID: 1054
		Compositor_CreateGeomConstantBuffer,
		// Token: 0x0400041F RID: 1055
		Compositor_CreatePanelMaskConstantBuffer,
		// Token: 0x04000420 RID: 1056
		Compositor_CreatePixelSimUBO,
		// Token: 0x04000421 RID: 1057
		Compositor_CreateMSAARenderTextures,
		// Token: 0x04000422 RID: 1058
		Compositor_CreateResolveRenderTextures,
		// Token: 0x04000423 RID: 1059
		Compositor_CreateComputeResolveRenderTextures,
		// Token: 0x04000424 RID: 1060
		Compositor_CreateDriverDirectModeResolveTextures,
		// Token: 0x04000425 RID: 1061
		Compositor_OpenDriverDirectModeResolveTextures,
		// Token: 0x04000426 RID: 1062
		Compositor_CreateFallbackSyncTexture,
		// Token: 0x04000427 RID: 1063
		Compositor_ShareFallbackSyncTexture,
		// Token: 0x04000428 RID: 1064
		Compositor_CreateOverlayIndexBuffer,
		// Token: 0x04000429 RID: 1065
		Compositor_CreateOverlayVertextBuffer,
		// Token: 0x0400042A RID: 1066
		Compositor_CreateTextVertexBuffer,
		// Token: 0x0400042B RID: 1067
		Compositor_CreateTextIndexBuffer,
		// Token: 0x0400042C RID: 1068
		Compositor_CreateMirrorTextures,
		// Token: 0x0400042D RID: 1069
		Compositor_CreateLastFrameRenderTexture,
		// Token: 0x0400042E RID: 1070
		VendorSpecific_UnableToConnectToOculusRuntime = 1000,
		// Token: 0x0400042F RID: 1071
		VendorSpecific_WindowsNotInDevMode,
		// Token: 0x04000430 RID: 1072
		VendorSpecific_HmdFound_CantOpenDevice = 1101,
		// Token: 0x04000431 RID: 1073
		VendorSpecific_HmdFound_UnableToRequestConfigStart,
		// Token: 0x04000432 RID: 1074
		VendorSpecific_HmdFound_NoStoredConfig,
		// Token: 0x04000433 RID: 1075
		VendorSpecific_HmdFound_ConfigTooBig,
		// Token: 0x04000434 RID: 1076
		VendorSpecific_HmdFound_ConfigTooSmall,
		// Token: 0x04000435 RID: 1077
		VendorSpecific_HmdFound_UnableToInitZLib,
		// Token: 0x04000436 RID: 1078
		VendorSpecific_HmdFound_CantReadFirmwareVersion,
		// Token: 0x04000437 RID: 1079
		VendorSpecific_HmdFound_UnableToSendUserDataStart,
		// Token: 0x04000438 RID: 1080
		VendorSpecific_HmdFound_UnableToGetUserDataStart,
		// Token: 0x04000439 RID: 1081
		VendorSpecific_HmdFound_UnableToGetUserDataNext,
		// Token: 0x0400043A RID: 1082
		VendorSpecific_HmdFound_UserDataAddressRange,
		// Token: 0x0400043B RID: 1083
		VendorSpecific_HmdFound_UserDataError,
		// Token: 0x0400043C RID: 1084
		VendorSpecific_HmdFound_ConfigFailedSanityCheck,
		// Token: 0x0400043D RID: 1085
		Steam_SteamInstallationNotFound = 2000,
		// Token: 0x0400043E RID: 1086
		LastError
	}
}
