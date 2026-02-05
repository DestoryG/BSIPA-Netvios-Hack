using System;
using System.Runtime.InteropServices;

namespace Valve.VR
{
	// Token: 0x020000C7 RID: 199
	public class OpenVR
	{
		// Token: 0x0600016D RID: 365 RVA: 0x000048C6 File Offset: 0x00002AC6
		public static uint InitInternal(ref EVRInitError peError, EVRApplicationType eApplicationType)
		{
			return OpenVRInterop.InitInternal(ref peError, eApplicationType);
		}

		// Token: 0x0600016E RID: 366 RVA: 0x000048CF File Offset: 0x00002ACF
		public static uint InitInternal2(ref EVRInitError peError, EVRApplicationType eApplicationType, string pchStartupInfo)
		{
			return OpenVRInterop.InitInternal2(ref peError, eApplicationType, pchStartupInfo);
		}

		// Token: 0x0600016F RID: 367 RVA: 0x000048D9 File Offset: 0x00002AD9
		public static void ShutdownInternal()
		{
			OpenVRInterop.ShutdownInternal();
		}

		// Token: 0x06000170 RID: 368 RVA: 0x000048E0 File Offset: 0x00002AE0
		public static bool IsHmdPresent()
		{
			return OpenVRInterop.IsHmdPresent();
		}

		// Token: 0x06000171 RID: 369 RVA: 0x000048E7 File Offset: 0x00002AE7
		public static bool IsRuntimeInstalled()
		{
			return OpenVRInterop.IsRuntimeInstalled();
		}

		// Token: 0x06000172 RID: 370 RVA: 0x000048EE File Offset: 0x00002AEE
		public static string RuntimePath()
		{
			return OpenVRInterop.RuntimePath();
		}

		// Token: 0x06000173 RID: 371 RVA: 0x000048F5 File Offset: 0x00002AF5
		public static string GetStringForHmdError(EVRInitError error)
		{
			return Marshal.PtrToStringAnsi(OpenVRInterop.GetStringForHmdError(error));
		}

		// Token: 0x06000174 RID: 372 RVA: 0x00004902 File Offset: 0x00002B02
		public static IntPtr GetGenericInterface(string pchInterfaceVersion, ref EVRInitError peError)
		{
			return OpenVRInterop.GetGenericInterface(pchInterfaceVersion, ref peError);
		}

		// Token: 0x06000175 RID: 373 RVA: 0x0000490B File Offset: 0x00002B0B
		public static bool IsInterfaceVersionValid(string pchInterfaceVersion)
		{
			return OpenVRInterop.IsInterfaceVersionValid(pchInterfaceVersion);
		}

		// Token: 0x06000176 RID: 374 RVA: 0x00004913 File Offset: 0x00002B13
		public static uint GetInitToken()
		{
			return OpenVRInterop.GetInitToken();
		}

		// Token: 0x17000003 RID: 3
		// (get) Token: 0x06000177 RID: 375 RVA: 0x0000491A File Offset: 0x00002B1A
		// (set) Token: 0x06000178 RID: 376 RVA: 0x00004921 File Offset: 0x00002B21
		private static uint VRToken { get; set; }

		// Token: 0x17000004 RID: 4
		// (get) Token: 0x06000179 RID: 377 RVA: 0x00004929 File Offset: 0x00002B29
		private static OpenVR.COpenVRContext OpenVRInternal_ModuleContext
		{
			get
			{
				if (OpenVR._OpenVRInternal_ModuleContext == null)
				{
					OpenVR._OpenVRInternal_ModuleContext = new OpenVR.COpenVRContext();
				}
				return OpenVR._OpenVRInternal_ModuleContext;
			}
		}

		// Token: 0x17000005 RID: 5
		// (get) Token: 0x0600017A RID: 378 RVA: 0x00004941 File Offset: 0x00002B41
		public static CVRSystem System
		{
			get
			{
				return OpenVR.OpenVRInternal_ModuleContext.VRSystem();
			}
		}

		// Token: 0x17000006 RID: 6
		// (get) Token: 0x0600017B RID: 379 RVA: 0x0000494D File Offset: 0x00002B4D
		public static CVRChaperone Chaperone
		{
			get
			{
				return OpenVR.OpenVRInternal_ModuleContext.VRChaperone();
			}
		}

		// Token: 0x17000007 RID: 7
		// (get) Token: 0x0600017C RID: 380 RVA: 0x00004959 File Offset: 0x00002B59
		public static CVRChaperoneSetup ChaperoneSetup
		{
			get
			{
				return OpenVR.OpenVRInternal_ModuleContext.VRChaperoneSetup();
			}
		}

		// Token: 0x17000008 RID: 8
		// (get) Token: 0x0600017D RID: 381 RVA: 0x00004965 File Offset: 0x00002B65
		public static CVRCompositor Compositor
		{
			get
			{
				return OpenVR.OpenVRInternal_ModuleContext.VRCompositor();
			}
		}

		// Token: 0x17000009 RID: 9
		// (get) Token: 0x0600017E RID: 382 RVA: 0x00004971 File Offset: 0x00002B71
		public static CVROverlay Overlay
		{
			get
			{
				return OpenVR.OpenVRInternal_ModuleContext.VROverlay();
			}
		}

		// Token: 0x1700000A RID: 10
		// (get) Token: 0x0600017F RID: 383 RVA: 0x0000497D File Offset: 0x00002B7D
		public static CVRRenderModels RenderModels
		{
			get
			{
				return OpenVR.OpenVRInternal_ModuleContext.VRRenderModels();
			}
		}

		// Token: 0x1700000B RID: 11
		// (get) Token: 0x06000180 RID: 384 RVA: 0x00004989 File Offset: 0x00002B89
		public static CVRExtendedDisplay ExtendedDisplay
		{
			get
			{
				return OpenVR.OpenVRInternal_ModuleContext.VRExtendedDisplay();
			}
		}

		// Token: 0x1700000C RID: 12
		// (get) Token: 0x06000181 RID: 385 RVA: 0x00004995 File Offset: 0x00002B95
		public static CVRSettings Settings
		{
			get
			{
				return OpenVR.OpenVRInternal_ModuleContext.VRSettings();
			}
		}

		// Token: 0x1700000D RID: 13
		// (get) Token: 0x06000182 RID: 386 RVA: 0x000049A1 File Offset: 0x00002BA1
		public static CVRApplications Applications
		{
			get
			{
				return OpenVR.OpenVRInternal_ModuleContext.VRApplications();
			}
		}

		// Token: 0x1700000E RID: 14
		// (get) Token: 0x06000183 RID: 387 RVA: 0x000049AD File Offset: 0x00002BAD
		public static CVRScreenshots Screenshots
		{
			get
			{
				return OpenVR.OpenVRInternal_ModuleContext.VRScreenshots();
			}
		}

		// Token: 0x1700000F RID: 15
		// (get) Token: 0x06000184 RID: 388 RVA: 0x000049B9 File Offset: 0x00002BB9
		public static CVRTrackedCamera TrackedCamera
		{
			get
			{
				return OpenVR.OpenVRInternal_ModuleContext.VRTrackedCamera();
			}
		}

		// Token: 0x17000010 RID: 16
		// (get) Token: 0x06000185 RID: 389 RVA: 0x000049C5 File Offset: 0x00002BC5
		public static CVRInput Input
		{
			get
			{
				return OpenVR.OpenVRInternal_ModuleContext.VRInput();
			}
		}

		// Token: 0x17000011 RID: 17
		// (get) Token: 0x06000186 RID: 390 RVA: 0x000049D1 File Offset: 0x00002BD1
		public static CVRIOBuffer IOBuffer
		{
			get
			{
				return OpenVR.OpenVRInternal_ModuleContext.VRIOBuffer();
			}
		}

		// Token: 0x17000012 RID: 18
		// (get) Token: 0x06000187 RID: 391 RVA: 0x000049DD File Offset: 0x00002BDD
		public static CVRSpatialAnchors SpatialAnchors
		{
			get
			{
				return OpenVR.OpenVRInternal_ModuleContext.VRSpatialAnchors();
			}
		}

		// Token: 0x17000013 RID: 19
		// (get) Token: 0x06000188 RID: 392 RVA: 0x000049E9 File Offset: 0x00002BE9
		public static CVRNotifications Notifications
		{
			get
			{
				return OpenVR.OpenVRInternal_ModuleContext.VRNotifications();
			}
		}

		// Token: 0x06000189 RID: 393 RVA: 0x000049F8 File Offset: 0x00002BF8
		public static CVRSystem Init(ref EVRInitError peError, EVRApplicationType eApplicationType = EVRApplicationType.VRApplication_Scene, string pchStartupInfo = "")
		{
			try
			{
				OpenVR.VRToken = OpenVR.InitInternal2(ref peError, eApplicationType, pchStartupInfo);
			}
			catch (EntryPointNotFoundException)
			{
				OpenVR.VRToken = OpenVR.InitInternal(ref peError, eApplicationType);
			}
			OpenVR.OpenVRInternal_ModuleContext.Clear();
			if (peError != EVRInitError.None)
			{
				return null;
			}
			if (!OpenVR.IsInterfaceVersionValid("IVRSystem_019"))
			{
				OpenVR.ShutdownInternal();
				peError = EVRInitError.Init_InterfaceNotFound;
				return null;
			}
			return OpenVR.System;
		}

		// Token: 0x0600018A RID: 394 RVA: 0x00004A60 File Offset: 0x00002C60
		public static void Shutdown()
		{
			OpenVR.ShutdownInternal();
		}

		// Token: 0x04000773 RID: 1907
		public const uint k_nDriverNone = 4294967295U;

		// Token: 0x04000774 RID: 1908
		public const uint k_unMaxDriverDebugResponseSize = 32768U;

		// Token: 0x04000775 RID: 1909
		public const uint k_unTrackedDeviceIndex_Hmd = 0U;

		// Token: 0x04000776 RID: 1910
		public const uint k_unMaxTrackedDeviceCount = 64U;

		// Token: 0x04000777 RID: 1911
		public const uint k_unTrackedDeviceIndexOther = 4294967294U;

		// Token: 0x04000778 RID: 1912
		public const uint k_unTrackedDeviceIndexInvalid = 4294967295U;

		// Token: 0x04000779 RID: 1913
		public const ulong k_ulInvalidPropertyContainer = 0UL;

		// Token: 0x0400077A RID: 1914
		public const uint k_unInvalidPropertyTag = 0U;

		// Token: 0x0400077B RID: 1915
		public const ulong k_ulInvalidDriverHandle = 0UL;

		// Token: 0x0400077C RID: 1916
		public const uint k_unFloatPropertyTag = 1U;

		// Token: 0x0400077D RID: 1917
		public const uint k_unInt32PropertyTag = 2U;

		// Token: 0x0400077E RID: 1918
		public const uint k_unUint64PropertyTag = 3U;

		// Token: 0x0400077F RID: 1919
		public const uint k_unBoolPropertyTag = 4U;

		// Token: 0x04000780 RID: 1920
		public const uint k_unStringPropertyTag = 5U;

		// Token: 0x04000781 RID: 1921
		public const uint k_unHmdMatrix34PropertyTag = 20U;

		// Token: 0x04000782 RID: 1922
		public const uint k_unHmdMatrix44PropertyTag = 21U;

		// Token: 0x04000783 RID: 1923
		public const uint k_unHmdVector3PropertyTag = 22U;

		// Token: 0x04000784 RID: 1924
		public const uint k_unHmdVector4PropertyTag = 23U;

		// Token: 0x04000785 RID: 1925
		public const uint k_unHmdVector2PropertyTag = 24U;

		// Token: 0x04000786 RID: 1926
		public const uint k_unHmdQuadPropertyTag = 25U;

		// Token: 0x04000787 RID: 1927
		public const uint k_unHiddenAreaPropertyTag = 30U;

		// Token: 0x04000788 RID: 1928
		public const uint k_unPathHandleInfoTag = 31U;

		// Token: 0x04000789 RID: 1929
		public const uint k_unActionPropertyTag = 32U;

		// Token: 0x0400078A RID: 1930
		public const uint k_unInputValuePropertyTag = 33U;

		// Token: 0x0400078B RID: 1931
		public const uint k_unWildcardPropertyTag = 34U;

		// Token: 0x0400078C RID: 1932
		public const uint k_unHapticVibrationPropertyTag = 35U;

		// Token: 0x0400078D RID: 1933
		public const uint k_unSkeletonPropertyTag = 36U;

		// Token: 0x0400078E RID: 1934
		public const uint k_unSpatialAnchorPosePropertyTag = 40U;

		// Token: 0x0400078F RID: 1935
		public const uint k_unJsonPropertyTag = 41U;

		// Token: 0x04000790 RID: 1936
		public const uint k_unActiveActionSetPropertyTag = 42U;

		// Token: 0x04000791 RID: 1937
		public const uint k_unOpenVRInternalReserved_Start = 1000U;

		// Token: 0x04000792 RID: 1938
		public const uint k_unOpenVRInternalReserved_End = 10000U;

		// Token: 0x04000793 RID: 1939
		public const uint k_unMaxPropertyStringSize = 32768U;

		// Token: 0x04000794 RID: 1940
		public const ulong k_ulInvalidActionHandle = 0UL;

		// Token: 0x04000795 RID: 1941
		public const ulong k_ulInvalidActionSetHandle = 0UL;

		// Token: 0x04000796 RID: 1942
		public const ulong k_ulInvalidInputValueHandle = 0UL;

		// Token: 0x04000797 RID: 1943
		public const uint k_unControllerStateAxisCount = 5U;

		// Token: 0x04000798 RID: 1944
		public const ulong k_ulOverlayHandleInvalid = 0UL;

		// Token: 0x04000799 RID: 1945
		public const uint k_unMaxDistortionFunctionParameters = 8U;

		// Token: 0x0400079A RID: 1946
		public const uint k_unScreenshotHandleInvalid = 0U;

		// Token: 0x0400079B RID: 1947
		public const string IVRSystem_Version = "IVRSystem_019";

		// Token: 0x0400079C RID: 1948
		public const string IVRExtendedDisplay_Version = "IVRExtendedDisplay_001";

		// Token: 0x0400079D RID: 1949
		public const string IVRTrackedCamera_Version = "IVRTrackedCamera_005";

		// Token: 0x0400079E RID: 1950
		public const uint k_unMaxApplicationKeyLength = 128U;

		// Token: 0x0400079F RID: 1951
		public const string k_pch_MimeType_HomeApp = "vr/home";

		// Token: 0x040007A0 RID: 1952
		public const string k_pch_MimeType_GameTheater = "vr/game_theater";

		// Token: 0x040007A1 RID: 1953
		public const string IVRApplications_Version = "IVRApplications_006";

		// Token: 0x040007A2 RID: 1954
		public const string IVRChaperone_Version = "IVRChaperone_003";

		// Token: 0x040007A3 RID: 1955
		public const string IVRChaperoneSetup_Version = "IVRChaperoneSetup_006";

		// Token: 0x040007A4 RID: 1956
		public const string IVRCompositor_Version = "IVRCompositor_022";

		// Token: 0x040007A5 RID: 1957
		public const uint k_unVROverlayMaxKeyLength = 128U;

		// Token: 0x040007A6 RID: 1958
		public const uint k_unVROverlayMaxNameLength = 128U;

		// Token: 0x040007A7 RID: 1959
		public const uint k_unMaxOverlayCount = 64U;

		// Token: 0x040007A8 RID: 1960
		public const uint k_unMaxOverlayIntersectionMaskPrimitivesCount = 32U;

		// Token: 0x040007A9 RID: 1961
		public const string IVROverlay_Version = "IVROverlay_019";

		// Token: 0x040007AA RID: 1962
		public const string k_pch_Controller_Component_GDC2015 = "gdc2015";

		// Token: 0x040007AB RID: 1963
		public const string k_pch_Controller_Component_Base = "base";

		// Token: 0x040007AC RID: 1964
		public const string k_pch_Controller_Component_Tip = "tip";

		// Token: 0x040007AD RID: 1965
		public const string k_pch_Controller_Component_HandGrip = "handgrip";

		// Token: 0x040007AE RID: 1966
		public const string k_pch_Controller_Component_Status = "status";

		// Token: 0x040007AF RID: 1967
		public const string IVRRenderModels_Version = "IVRRenderModels_006";

		// Token: 0x040007B0 RID: 1968
		public const uint k_unNotificationTextMaxSize = 256U;

		// Token: 0x040007B1 RID: 1969
		public const string IVRNotifications_Version = "IVRNotifications_002";

		// Token: 0x040007B2 RID: 1970
		public const uint k_unMaxSettingsKeyLength = 128U;

		// Token: 0x040007B3 RID: 1971
		public const string IVRSettings_Version = "IVRSettings_002";

		// Token: 0x040007B4 RID: 1972
		public const string k_pch_SteamVR_Section = "steamvr";

		// Token: 0x040007B5 RID: 1973
		public const string k_pch_SteamVR_RequireHmd_String = "requireHmd";

		// Token: 0x040007B6 RID: 1974
		public const string k_pch_SteamVR_ForcedDriverKey_String = "forcedDriver";

		// Token: 0x040007B7 RID: 1975
		public const string k_pch_SteamVR_ForcedHmdKey_String = "forcedHmd";

		// Token: 0x040007B8 RID: 1976
		public const string k_pch_SteamVR_DisplayDebug_Bool = "displayDebug";

		// Token: 0x040007B9 RID: 1977
		public const string k_pch_SteamVR_DebugProcessPipe_String = "debugProcessPipe";

		// Token: 0x040007BA RID: 1978
		public const string k_pch_SteamVR_DisplayDebugX_Int32 = "displayDebugX";

		// Token: 0x040007BB RID: 1979
		public const string k_pch_SteamVR_DisplayDebugY_Int32 = "displayDebugY";

		// Token: 0x040007BC RID: 1980
		public const string k_pch_SteamVR_SendSystemButtonToAllApps_Bool = "sendSystemButtonToAllApps";

		// Token: 0x040007BD RID: 1981
		public const string k_pch_SteamVR_LogLevel_Int32 = "loglevel";

		// Token: 0x040007BE RID: 1982
		public const string k_pch_SteamVR_IPD_Float = "ipd";

		// Token: 0x040007BF RID: 1983
		public const string k_pch_SteamVR_Background_String = "background";

		// Token: 0x040007C0 RID: 1984
		public const string k_pch_SteamVR_BackgroundUseDomeProjection_Bool = "backgroundUseDomeProjection";

		// Token: 0x040007C1 RID: 1985
		public const string k_pch_SteamVR_BackgroundCameraHeight_Float = "backgroundCameraHeight";

		// Token: 0x040007C2 RID: 1986
		public const string k_pch_SteamVR_BackgroundDomeRadius_Float = "backgroundDomeRadius";

		// Token: 0x040007C3 RID: 1987
		public const string k_pch_SteamVR_GridColor_String = "gridColor";

		// Token: 0x040007C4 RID: 1988
		public const string k_pch_SteamVR_PlayAreaColor_String = "playAreaColor";

		// Token: 0x040007C5 RID: 1989
		public const string k_pch_SteamVR_ShowStage_Bool = "showStage";

		// Token: 0x040007C6 RID: 1990
		public const string k_pch_SteamVR_ActivateMultipleDrivers_Bool = "activateMultipleDrivers";

		// Token: 0x040007C7 RID: 1991
		public const string k_pch_SteamVR_UsingSpeakers_Bool = "usingSpeakers";

		// Token: 0x040007C8 RID: 1992
		public const string k_pch_SteamVR_SpeakersForwardYawOffsetDegrees_Float = "speakersForwardYawOffsetDegrees";

		// Token: 0x040007C9 RID: 1993
		public const string k_pch_SteamVR_BaseStationPowerManagement_Bool = "basestationPowerManagement";

		// Token: 0x040007CA RID: 1994
		public const string k_pch_SteamVR_NeverKillProcesses_Bool = "neverKillProcesses";

		// Token: 0x040007CB RID: 1995
		public const string k_pch_SteamVR_SupersampleScale_Float = "supersampleScale";

		// Token: 0x040007CC RID: 1996
		public const string k_pch_SteamVR_MaxRecommendedResolution_Int32 = "maxRecommendedResolution";

		// Token: 0x040007CD RID: 1997
		public const string k_pch_SteamVR_MotionSmoothing_Bool = "motionSmoothing";

		// Token: 0x040007CE RID: 1998
		public const string k_pch_SteamVR_MotionSmoothingOverride_Int32 = "motionSmoothingOverride";

		// Token: 0x040007CF RID: 1999
		public const string k_pch_SteamVR_ForceFadeOnBadTracking_Bool = "forceFadeOnBadTracking";

		// Token: 0x040007D0 RID: 2000
		public const string k_pch_SteamVR_DefaultMirrorView_Int32 = "mirrorView";

		// Token: 0x040007D1 RID: 2001
		public const string k_pch_SteamVR_ShowMirrorView_Bool = "showMirrorView";

		// Token: 0x040007D2 RID: 2002
		public const string k_pch_SteamVR_MirrorViewGeometry_String = "mirrorViewGeometry";

		// Token: 0x040007D3 RID: 2003
		public const string k_pch_SteamVR_MirrorViewGeometryMaximized_String = "mirrorViewGeometryMaximized";

		// Token: 0x040007D4 RID: 2004
		public const string k_pch_SteamVR_StartMonitorFromAppLaunch = "startMonitorFromAppLaunch";

		// Token: 0x040007D5 RID: 2005
		public const string k_pch_SteamVR_StartCompositorFromAppLaunch_Bool = "startCompositorFromAppLaunch";

		// Token: 0x040007D6 RID: 2006
		public const string k_pch_SteamVR_StartDashboardFromAppLaunch_Bool = "startDashboardFromAppLaunch";

		// Token: 0x040007D7 RID: 2007
		public const string k_pch_SteamVR_StartOverlayAppsFromDashboard_Bool = "startOverlayAppsFromDashboard";

		// Token: 0x040007D8 RID: 2008
		public const string k_pch_SteamVR_EnableHomeApp = "enableHomeApp";

		// Token: 0x040007D9 RID: 2009
		public const string k_pch_SteamVR_CycleBackgroundImageTimeSec_Int32 = "CycleBackgroundImageTimeSec";

		// Token: 0x040007DA RID: 2010
		public const string k_pch_SteamVR_RetailDemo_Bool = "retailDemo";

		// Token: 0x040007DB RID: 2011
		public const string k_pch_SteamVR_IpdOffset_Float = "ipdOffset";

		// Token: 0x040007DC RID: 2012
		public const string k_pch_SteamVR_AllowSupersampleFiltering_Bool = "allowSupersampleFiltering";

		// Token: 0x040007DD RID: 2013
		public const string k_pch_SteamVR_SupersampleManualOverride_Bool = "supersampleManualOverride";

		// Token: 0x040007DE RID: 2014
		public const string k_pch_SteamVR_EnableLinuxVulkanAsync_Bool = "enableLinuxVulkanAsync";

		// Token: 0x040007DF RID: 2015
		public const string k_pch_SteamVR_AllowDisplayLockedMode_Bool = "allowDisplayLockedMode";

		// Token: 0x040007E0 RID: 2016
		public const string k_pch_SteamVR_HaveStartedTutorialForNativeChaperoneDriver_Bool = "haveStartedTutorialForNativeChaperoneDriver";

		// Token: 0x040007E1 RID: 2017
		public const string k_pch_SteamVR_ForceWindows32bitVRMonitor = "forceWindows32BitVRMonitor";

		// Token: 0x040007E2 RID: 2018
		public const string k_pch_SteamVR_DebugInput = "debugInput";

		// Token: 0x040007E3 RID: 2019
		public const string k_pch_SteamVR_DebugInputBinding = "debugInputBinding";

		// Token: 0x040007E4 RID: 2020
		public const string k_pch_SteamVR_InputBindingUIBlock = "inputBindingUI";

		// Token: 0x040007E5 RID: 2021
		public const string k_pch_SteamVR_RenderCameraMode = "renderCameraMode";

		// Token: 0x040007E6 RID: 2022
		public const string k_pch_SteamVR_EnableSharedResourceJournaling = "enableSharedResourceJournaling";

		// Token: 0x040007E7 RID: 2023
		public const string k_pch_SteamVR_EnableSafeMode = "enableSafeMode";

		// Token: 0x040007E8 RID: 2024
		public const string k_pch_SteamVR_PreferredRefreshRate = "preferredRefreshRate";

		// Token: 0x040007E9 RID: 2025
		public const string k_pch_SteamVR_LastVersionNotice = "lastVersionNotice";

		// Token: 0x040007EA RID: 2026
		public const string k_pch_SteamVR_LastVersionNoticeDate = "lastVersionNoticeDate";

		// Token: 0x040007EB RID: 2027
		public const string k_pch_DirectMode_Section = "direct_mode";

		// Token: 0x040007EC RID: 2028
		public const string k_pch_DirectMode_Enable_Bool = "enable";

		// Token: 0x040007ED RID: 2029
		public const string k_pch_DirectMode_Count_Int32 = "count";

		// Token: 0x040007EE RID: 2030
		public const string k_pch_DirectMode_EdidVid_Int32 = "edidVid";

		// Token: 0x040007EF RID: 2031
		public const string k_pch_DirectMode_EdidPid_Int32 = "edidPid";

		// Token: 0x040007F0 RID: 2032
		public const string k_pch_Lighthouse_Section = "driver_lighthouse";

		// Token: 0x040007F1 RID: 2033
		public const string k_pch_Lighthouse_DisableIMU_Bool = "disableimu";

		// Token: 0x040007F2 RID: 2034
		public const string k_pch_Lighthouse_DisableIMUExceptHMD_Bool = "disableimuexcepthmd";

		// Token: 0x040007F3 RID: 2035
		public const string k_pch_Lighthouse_UseDisambiguation_String = "usedisambiguation";

		// Token: 0x040007F4 RID: 2036
		public const string k_pch_Lighthouse_DisambiguationDebug_Int32 = "disambiguationdebug";

		// Token: 0x040007F5 RID: 2037
		public const string k_pch_Lighthouse_PrimaryBasestation_Int32 = "primarybasestation";

		// Token: 0x040007F6 RID: 2038
		public const string k_pch_Lighthouse_DBHistory_Bool = "dbhistory";

		// Token: 0x040007F7 RID: 2039
		public const string k_pch_Lighthouse_EnableBluetooth_Bool = "enableBluetooth";

		// Token: 0x040007F8 RID: 2040
		public const string k_pch_Lighthouse_PowerManagedBaseStations_String = "PowerManagedBaseStations";

		// Token: 0x040007F9 RID: 2041
		public const string k_pch_Lighthouse_PowerManagedBaseStations2_String = "PowerManagedBaseStations2";

		// Token: 0x040007FA RID: 2042
		public const string k_pch_Lighthouse_EnableImuFallback_Bool = "enableImuFallback";

		// Token: 0x040007FB RID: 2043
		public const string k_pch_Lighthouse_NewPairing_Bool = "newPairing";

		// Token: 0x040007FC RID: 2044
		public const string k_pch_Null_Section = "driver_null";

		// Token: 0x040007FD RID: 2045
		public const string k_pch_Null_SerialNumber_String = "serialNumber";

		// Token: 0x040007FE RID: 2046
		public const string k_pch_Null_ModelNumber_String = "modelNumber";

		// Token: 0x040007FF RID: 2047
		public const string k_pch_Null_WindowX_Int32 = "windowX";

		// Token: 0x04000800 RID: 2048
		public const string k_pch_Null_WindowY_Int32 = "windowY";

		// Token: 0x04000801 RID: 2049
		public const string k_pch_Null_WindowWidth_Int32 = "windowWidth";

		// Token: 0x04000802 RID: 2050
		public const string k_pch_Null_WindowHeight_Int32 = "windowHeight";

		// Token: 0x04000803 RID: 2051
		public const string k_pch_Null_RenderWidth_Int32 = "renderWidth";

		// Token: 0x04000804 RID: 2052
		public const string k_pch_Null_RenderHeight_Int32 = "renderHeight";

		// Token: 0x04000805 RID: 2053
		public const string k_pch_Null_SecondsFromVsyncToPhotons_Float = "secondsFromVsyncToPhotons";

		// Token: 0x04000806 RID: 2054
		public const string k_pch_Null_DisplayFrequency_Float = "displayFrequency";

		// Token: 0x04000807 RID: 2055
		public const string k_pch_UserInterface_Section = "userinterface";

		// Token: 0x04000808 RID: 2056
		public const string k_pch_UserInterface_StatusAlwaysOnTop_Bool = "StatusAlwaysOnTop";

		// Token: 0x04000809 RID: 2057
		public const string k_pch_UserInterface_MinimizeToTray_Bool = "MinimizeToTray";

		// Token: 0x0400080A RID: 2058
		public const string k_pch_UserInterface_HidePopupsWhenStatusMinimized_Bool = "HidePopupsWhenStatusMinimized";

		// Token: 0x0400080B RID: 2059
		public const string k_pch_UserInterface_Screenshots_Bool = "screenshots";

		// Token: 0x0400080C RID: 2060
		public const string k_pch_UserInterface_ScreenshotType_Int = "screenshotType";

		// Token: 0x0400080D RID: 2061
		public const string k_pch_Notifications_Section = "notifications";

		// Token: 0x0400080E RID: 2062
		public const string k_pch_Notifications_DoNotDisturb_Bool = "DoNotDisturb";

		// Token: 0x0400080F RID: 2063
		public const string k_pch_Keyboard_Section = "keyboard";

		// Token: 0x04000810 RID: 2064
		public const string k_pch_Keyboard_TutorialCompletions = "TutorialCompletions";

		// Token: 0x04000811 RID: 2065
		public const string k_pch_Keyboard_ScaleX = "ScaleX";

		// Token: 0x04000812 RID: 2066
		public const string k_pch_Keyboard_ScaleY = "ScaleY";

		// Token: 0x04000813 RID: 2067
		public const string k_pch_Keyboard_OffsetLeftX = "OffsetLeftX";

		// Token: 0x04000814 RID: 2068
		public const string k_pch_Keyboard_OffsetRightX = "OffsetRightX";

		// Token: 0x04000815 RID: 2069
		public const string k_pch_Keyboard_OffsetY = "OffsetY";

		// Token: 0x04000816 RID: 2070
		public const string k_pch_Keyboard_Smoothing = "Smoothing";

		// Token: 0x04000817 RID: 2071
		public const string k_pch_Perf_Section = "perfcheck";

		// Token: 0x04000818 RID: 2072
		public const string k_pch_Perf_PerfGraphInHMD_Bool = "perfGraphInHMD";

		// Token: 0x04000819 RID: 2073
		public const string k_pch_Perf_AllowTimingStore_Bool = "allowTimingStore";

		// Token: 0x0400081A RID: 2074
		public const string k_pch_Perf_SaveTimingsOnExit_Bool = "saveTimingsOnExit";

		// Token: 0x0400081B RID: 2075
		public const string k_pch_Perf_TestData_Float = "perfTestData";

		// Token: 0x0400081C RID: 2076
		public const string k_pch_Perf_LinuxGPUProfiling_Bool = "linuxGPUProfiling";

		// Token: 0x0400081D RID: 2077
		public const string k_pch_CollisionBounds_Section = "collisionBounds";

		// Token: 0x0400081E RID: 2078
		public const string k_pch_CollisionBounds_Style_Int32 = "CollisionBoundsStyle";

		// Token: 0x0400081F RID: 2079
		public const string k_pch_CollisionBounds_GroundPerimeterOn_Bool = "CollisionBoundsGroundPerimeterOn";

		// Token: 0x04000820 RID: 2080
		public const string k_pch_CollisionBounds_CenterMarkerOn_Bool = "CollisionBoundsCenterMarkerOn";

		// Token: 0x04000821 RID: 2081
		public const string k_pch_CollisionBounds_PlaySpaceOn_Bool = "CollisionBoundsPlaySpaceOn";

		// Token: 0x04000822 RID: 2082
		public const string k_pch_CollisionBounds_FadeDistance_Float = "CollisionBoundsFadeDistance";

		// Token: 0x04000823 RID: 2083
		public const string k_pch_CollisionBounds_ColorGammaR_Int32 = "CollisionBoundsColorGammaR";

		// Token: 0x04000824 RID: 2084
		public const string k_pch_CollisionBounds_ColorGammaG_Int32 = "CollisionBoundsColorGammaG";

		// Token: 0x04000825 RID: 2085
		public const string k_pch_CollisionBounds_ColorGammaB_Int32 = "CollisionBoundsColorGammaB";

		// Token: 0x04000826 RID: 2086
		public const string k_pch_CollisionBounds_ColorGammaA_Int32 = "CollisionBoundsColorGammaA";

		// Token: 0x04000827 RID: 2087
		public const string k_pch_Camera_Section = "camera";

		// Token: 0x04000828 RID: 2088
		public const string k_pch_Camera_EnableCamera_Bool = "enableCamera";

		// Token: 0x04000829 RID: 2089
		public const string k_pch_Camera_EnableCameraInDashboard_Bool = "enableCameraInDashboard";

		// Token: 0x0400082A RID: 2090
		public const string k_pch_Camera_EnableCameraForCollisionBounds_Bool = "enableCameraForCollisionBounds";

		// Token: 0x0400082B RID: 2091
		public const string k_pch_Camera_EnableCameraForRoomView_Bool = "enableCameraForRoomView";

		// Token: 0x0400082C RID: 2092
		public const string k_pch_Camera_BoundsColorGammaR_Int32 = "cameraBoundsColorGammaR";

		// Token: 0x0400082D RID: 2093
		public const string k_pch_Camera_BoundsColorGammaG_Int32 = "cameraBoundsColorGammaG";

		// Token: 0x0400082E RID: 2094
		public const string k_pch_Camera_BoundsColorGammaB_Int32 = "cameraBoundsColorGammaB";

		// Token: 0x0400082F RID: 2095
		public const string k_pch_Camera_BoundsColorGammaA_Int32 = "cameraBoundsColorGammaA";

		// Token: 0x04000830 RID: 2096
		public const string k_pch_Camera_BoundsStrength_Int32 = "cameraBoundsStrength";

		// Token: 0x04000831 RID: 2097
		public const string k_pch_Camera_RoomViewMode_Int32 = "cameraRoomViewMode";

		// Token: 0x04000832 RID: 2098
		public const string k_pch_audio_Section = "audio";

		// Token: 0x04000833 RID: 2099
		public const string k_pch_audio_OnPlaybackDevice_String = "onPlaybackDevice";

		// Token: 0x04000834 RID: 2100
		public const string k_pch_audio_OnRecordDevice_String = "onRecordDevice";

		// Token: 0x04000835 RID: 2101
		public const string k_pch_audio_OnPlaybackMirrorDevice_String = "onPlaybackMirrorDevice";

		// Token: 0x04000836 RID: 2102
		public const string k_pch_audio_OffPlaybackDevice_String = "offPlaybackDevice";

		// Token: 0x04000837 RID: 2103
		public const string k_pch_audio_OffRecordDevice_String = "offRecordDevice";

		// Token: 0x04000838 RID: 2104
		public const string k_pch_audio_VIVEHDMIGain = "viveHDMIGain";

		// Token: 0x04000839 RID: 2105
		public const string k_pch_Power_Section = "power";

		// Token: 0x0400083A RID: 2106
		public const string k_pch_Power_PowerOffOnExit_Bool = "powerOffOnExit";

		// Token: 0x0400083B RID: 2107
		public const string k_pch_Power_TurnOffScreensTimeout_Float = "turnOffScreensTimeout";

		// Token: 0x0400083C RID: 2108
		public const string k_pch_Power_TurnOffControllersTimeout_Float = "turnOffControllersTimeout";

		// Token: 0x0400083D RID: 2109
		public const string k_pch_Power_ReturnToWatchdogTimeout_Float = "returnToWatchdogTimeout";

		// Token: 0x0400083E RID: 2110
		public const string k_pch_Power_AutoLaunchSteamVROnButtonPress = "autoLaunchSteamVROnButtonPress";

		// Token: 0x0400083F RID: 2111
		public const string k_pch_Power_PauseCompositorOnStandby_Bool = "pauseCompositorOnStandby";

		// Token: 0x04000840 RID: 2112
		public const string k_pch_Dashboard_Section = "dashboard";

		// Token: 0x04000841 RID: 2113
		public const string k_pch_Dashboard_EnableDashboard_Bool = "enableDashboard";

		// Token: 0x04000842 RID: 2114
		public const string k_pch_Dashboard_ArcadeMode_Bool = "arcadeMode";

		// Token: 0x04000843 RID: 2115
		public const string k_pch_Dashboard_UseWebDashboard = "useWebDashboard";

		// Token: 0x04000844 RID: 2116
		public const string k_pch_Dashboard_UseWebSettings = "useWebSettings";

		// Token: 0x04000845 RID: 2117
		public const string k_pch_Dashboard_UseWebIPD = "useWebIPD";

		// Token: 0x04000846 RID: 2118
		public const string k_pch_Dashboard_UseWebPowerMenu = "useWebPowerMenu";

		// Token: 0x04000847 RID: 2119
		public const string k_pch_modelskin_Section = "modelskins";

		// Token: 0x04000848 RID: 2120
		public const string k_pch_Driver_Enable_Bool = "enable";

		// Token: 0x04000849 RID: 2121
		public const string k_pch_WebInterface_Section = "WebInterface";

		// Token: 0x0400084A RID: 2122
		public const string k_pch_WebInterface_WebEnable_Bool = "WebEnable";

		// Token: 0x0400084B RID: 2123
		public const string k_pch_WebInterface_WebPort_String = "WebPort";

		// Token: 0x0400084C RID: 2124
		public const string k_pch_VRWebHelper_Section = "VRWebHelper";

		// Token: 0x0400084D RID: 2125
		public const string k_pch_VRWebHelper_DebuggerEnabled_Bool = "DebuggerEnabled";

		// Token: 0x0400084E RID: 2126
		public const string k_pch_VRWebHelper_DebuggerPort_Int32 = "DebuggerPort";

		// Token: 0x0400084F RID: 2127
		public const string k_pch_TrackingOverride_Section = "TrackingOverrides";

		// Token: 0x04000850 RID: 2128
		public const string k_pch_App_BindingAutosaveURLSuffix_String = "AutosaveURL";

		// Token: 0x04000851 RID: 2129
		public const string k_pch_App_BindingCurrentURLSuffix_String = "CurrentURL";

		// Token: 0x04000852 RID: 2130
		public const string k_pch_App_NeedToUpdateAutosaveSuffix_Bool = "NeedToUpdateAutosave";

		// Token: 0x04000853 RID: 2131
		public const string k_pch_Trackers_Section = "trackers";

		// Token: 0x04000854 RID: 2132
		public const string k_pch_DesktopUI_Section = "DesktopUI";

		// Token: 0x04000855 RID: 2133
		public const string k_pch_LastKnown_Section = "LastKnown";

		// Token: 0x04000856 RID: 2134
		public const string k_pch_LastKnown_HMDManufacturer_String = "HMDManufacturer";

		// Token: 0x04000857 RID: 2135
		public const string k_pch_LastKnown_HMDModel_String = "HMDModel";

		// Token: 0x04000858 RID: 2136
		public const string k_pch_DismissedWarnings_Section = "DismissedWarnings";

		// Token: 0x04000859 RID: 2137
		public const string IVRScreenshots_Version = "IVRScreenshots_001";

		// Token: 0x0400085A RID: 2138
		public const string IVRResources_Version = "IVRResources_001";

		// Token: 0x0400085B RID: 2139
		public const string IVRDriverManager_Version = "IVRDriverManager_001";

		// Token: 0x0400085C RID: 2140
		public const uint k_unMaxActionNameLength = 64U;

		// Token: 0x0400085D RID: 2141
		public const uint k_unMaxActionSetNameLength = 64U;

		// Token: 0x0400085E RID: 2142
		public const uint k_unMaxActionOriginCount = 16U;

		// Token: 0x0400085F RID: 2143
		public const uint k_unMaxBoneNameLength = 32U;

		// Token: 0x04000860 RID: 2144
		public const string IVRInput_Version = "IVRInput_005";

		// Token: 0x04000861 RID: 2145
		public const ulong k_ulInvalidIOBufferHandle = 0UL;

		// Token: 0x04000862 RID: 2146
		public const string IVRIOBuffer_Version = "IVRIOBuffer_002";

		// Token: 0x04000863 RID: 2147
		public const uint k_ulInvalidSpatialAnchorHandle = 0U;

		// Token: 0x04000864 RID: 2148
		public const string IVRSpatialAnchors_Version = "IVRSpatialAnchors_001";

		// Token: 0x04000866 RID: 2150
		private const string FnTable_Prefix = "FnTable:";

		// Token: 0x04000867 RID: 2151
		private static OpenVR.COpenVRContext _OpenVRInternal_ModuleContext;

		// Token: 0x02000239 RID: 569
		private class COpenVRContext
		{
			// Token: 0x06000783 RID: 1923 RVA: 0x000068AF File Offset: 0x00004AAF
			public COpenVRContext()
			{
				this.Clear();
			}

			// Token: 0x06000784 RID: 1924 RVA: 0x000068C0 File Offset: 0x00004AC0
			public void Clear()
			{
				this.m_pVRSystem = null;
				this.m_pVRChaperone = null;
				this.m_pVRChaperoneSetup = null;
				this.m_pVRCompositor = null;
				this.m_pVROverlay = null;
				this.m_pVRRenderModels = null;
				this.m_pVRExtendedDisplay = null;
				this.m_pVRSettings = null;
				this.m_pVRApplications = null;
				this.m_pVRScreenshots = null;
				this.m_pVRTrackedCamera = null;
				this.m_pVRInput = null;
				this.m_pVRIOBuffer = null;
				this.m_pVRSpatialAnchors = null;
				this.m_pVRNotifications = null;
			}

			// Token: 0x06000785 RID: 1925 RVA: 0x00006936 File Offset: 0x00004B36
			private void CheckClear()
			{
				if (OpenVR.VRToken != OpenVR.GetInitToken())
				{
					this.Clear();
					OpenVR.VRToken = OpenVR.GetInitToken();
				}
			}

			// Token: 0x06000786 RID: 1926 RVA: 0x00006954 File Offset: 0x00004B54
			public CVRSystem VRSystem()
			{
				this.CheckClear();
				if (this.m_pVRSystem == null)
				{
					EVRInitError evrinitError = EVRInitError.None;
					IntPtr genericInterface = OpenVRInterop.GetGenericInterface("FnTable:IVRSystem_019", ref evrinitError);
					if (genericInterface != IntPtr.Zero && evrinitError == EVRInitError.None)
					{
						this.m_pVRSystem = new CVRSystem(genericInterface);
					}
				}
				return this.m_pVRSystem;
			}

			// Token: 0x06000787 RID: 1927 RVA: 0x000069A0 File Offset: 0x00004BA0
			public CVRChaperone VRChaperone()
			{
				this.CheckClear();
				if (this.m_pVRChaperone == null)
				{
					EVRInitError evrinitError = EVRInitError.None;
					IntPtr genericInterface = OpenVRInterop.GetGenericInterface("FnTable:IVRChaperone_003", ref evrinitError);
					if (genericInterface != IntPtr.Zero && evrinitError == EVRInitError.None)
					{
						this.m_pVRChaperone = new CVRChaperone(genericInterface);
					}
				}
				return this.m_pVRChaperone;
			}

			// Token: 0x06000788 RID: 1928 RVA: 0x000069EC File Offset: 0x00004BEC
			public CVRChaperoneSetup VRChaperoneSetup()
			{
				this.CheckClear();
				if (this.m_pVRChaperoneSetup == null)
				{
					EVRInitError evrinitError = EVRInitError.None;
					IntPtr genericInterface = OpenVRInterop.GetGenericInterface("FnTable:IVRChaperoneSetup_006", ref evrinitError);
					if (genericInterface != IntPtr.Zero && evrinitError == EVRInitError.None)
					{
						this.m_pVRChaperoneSetup = new CVRChaperoneSetup(genericInterface);
					}
				}
				return this.m_pVRChaperoneSetup;
			}

			// Token: 0x06000789 RID: 1929 RVA: 0x00006A38 File Offset: 0x00004C38
			public CVRCompositor VRCompositor()
			{
				this.CheckClear();
				if (this.m_pVRCompositor == null)
				{
					EVRInitError evrinitError = EVRInitError.None;
					IntPtr genericInterface = OpenVRInterop.GetGenericInterface("FnTable:IVRCompositor_022", ref evrinitError);
					if (genericInterface != IntPtr.Zero && evrinitError == EVRInitError.None)
					{
						this.m_pVRCompositor = new CVRCompositor(genericInterface);
					}
				}
				return this.m_pVRCompositor;
			}

			// Token: 0x0600078A RID: 1930 RVA: 0x00006A84 File Offset: 0x00004C84
			public CVROverlay VROverlay()
			{
				this.CheckClear();
				if (this.m_pVROverlay == null)
				{
					EVRInitError evrinitError = EVRInitError.None;
					IntPtr genericInterface = OpenVRInterop.GetGenericInterface("FnTable:IVROverlay_019", ref evrinitError);
					if (genericInterface != IntPtr.Zero && evrinitError == EVRInitError.None)
					{
						this.m_pVROverlay = new CVROverlay(genericInterface);
					}
				}
				return this.m_pVROverlay;
			}

			// Token: 0x0600078B RID: 1931 RVA: 0x00006AD0 File Offset: 0x00004CD0
			public CVRRenderModels VRRenderModels()
			{
				this.CheckClear();
				if (this.m_pVRRenderModels == null)
				{
					EVRInitError evrinitError = EVRInitError.None;
					IntPtr genericInterface = OpenVRInterop.GetGenericInterface("FnTable:IVRRenderModels_006", ref evrinitError);
					if (genericInterface != IntPtr.Zero && evrinitError == EVRInitError.None)
					{
						this.m_pVRRenderModels = new CVRRenderModels(genericInterface);
					}
				}
				return this.m_pVRRenderModels;
			}

			// Token: 0x0600078C RID: 1932 RVA: 0x00006B1C File Offset: 0x00004D1C
			public CVRExtendedDisplay VRExtendedDisplay()
			{
				this.CheckClear();
				if (this.m_pVRExtendedDisplay == null)
				{
					EVRInitError evrinitError = EVRInitError.None;
					IntPtr genericInterface = OpenVRInterop.GetGenericInterface("FnTable:IVRExtendedDisplay_001", ref evrinitError);
					if (genericInterface != IntPtr.Zero && evrinitError == EVRInitError.None)
					{
						this.m_pVRExtendedDisplay = new CVRExtendedDisplay(genericInterface);
					}
				}
				return this.m_pVRExtendedDisplay;
			}

			// Token: 0x0600078D RID: 1933 RVA: 0x00006B68 File Offset: 0x00004D68
			public CVRSettings VRSettings()
			{
				this.CheckClear();
				if (this.m_pVRSettings == null)
				{
					EVRInitError evrinitError = EVRInitError.None;
					IntPtr genericInterface = OpenVRInterop.GetGenericInterface("FnTable:IVRSettings_002", ref evrinitError);
					if (genericInterface != IntPtr.Zero && evrinitError == EVRInitError.None)
					{
						this.m_pVRSettings = new CVRSettings(genericInterface);
					}
				}
				return this.m_pVRSettings;
			}

			// Token: 0x0600078E RID: 1934 RVA: 0x00006BB4 File Offset: 0x00004DB4
			public CVRApplications VRApplications()
			{
				this.CheckClear();
				if (this.m_pVRApplications == null)
				{
					EVRInitError evrinitError = EVRInitError.None;
					IntPtr genericInterface = OpenVRInterop.GetGenericInterface("FnTable:IVRApplications_006", ref evrinitError);
					if (genericInterface != IntPtr.Zero && evrinitError == EVRInitError.None)
					{
						this.m_pVRApplications = new CVRApplications(genericInterface);
					}
				}
				return this.m_pVRApplications;
			}

			// Token: 0x0600078F RID: 1935 RVA: 0x00006C00 File Offset: 0x00004E00
			public CVRScreenshots VRScreenshots()
			{
				this.CheckClear();
				if (this.m_pVRScreenshots == null)
				{
					EVRInitError evrinitError = EVRInitError.None;
					IntPtr genericInterface = OpenVRInterop.GetGenericInterface("FnTable:IVRScreenshots_001", ref evrinitError);
					if (genericInterface != IntPtr.Zero && evrinitError == EVRInitError.None)
					{
						this.m_pVRScreenshots = new CVRScreenshots(genericInterface);
					}
				}
				return this.m_pVRScreenshots;
			}

			// Token: 0x06000790 RID: 1936 RVA: 0x00006C4C File Offset: 0x00004E4C
			public CVRTrackedCamera VRTrackedCamera()
			{
				this.CheckClear();
				if (this.m_pVRTrackedCamera == null)
				{
					EVRInitError evrinitError = EVRInitError.None;
					IntPtr genericInterface = OpenVRInterop.GetGenericInterface("FnTable:IVRTrackedCamera_005", ref evrinitError);
					if (genericInterface != IntPtr.Zero && evrinitError == EVRInitError.None)
					{
						this.m_pVRTrackedCamera = new CVRTrackedCamera(genericInterface);
					}
				}
				return this.m_pVRTrackedCamera;
			}

			// Token: 0x06000791 RID: 1937 RVA: 0x00006C98 File Offset: 0x00004E98
			public CVRInput VRInput()
			{
				this.CheckClear();
				if (this.m_pVRInput == null)
				{
					EVRInitError evrinitError = EVRInitError.None;
					IntPtr genericInterface = OpenVRInterop.GetGenericInterface("FnTable:IVRInput_005", ref evrinitError);
					if (genericInterface != IntPtr.Zero && evrinitError == EVRInitError.None)
					{
						this.m_pVRInput = new CVRInput(genericInterface);
					}
				}
				return this.m_pVRInput;
			}

			// Token: 0x06000792 RID: 1938 RVA: 0x00006CE4 File Offset: 0x00004EE4
			public CVRIOBuffer VRIOBuffer()
			{
				this.CheckClear();
				if (this.m_pVRIOBuffer == null)
				{
					EVRInitError evrinitError = EVRInitError.None;
					IntPtr genericInterface = OpenVRInterop.GetGenericInterface("FnTable:IVRIOBuffer_002", ref evrinitError);
					if (genericInterface != IntPtr.Zero && evrinitError == EVRInitError.None)
					{
						this.m_pVRIOBuffer = new CVRIOBuffer(genericInterface);
					}
				}
				return this.m_pVRIOBuffer;
			}

			// Token: 0x06000793 RID: 1939 RVA: 0x00006D30 File Offset: 0x00004F30
			public CVRSpatialAnchors VRSpatialAnchors()
			{
				this.CheckClear();
				if (this.m_pVRSpatialAnchors == null)
				{
					EVRInitError evrinitError = EVRInitError.None;
					IntPtr genericInterface = OpenVRInterop.GetGenericInterface("FnTable:IVRSpatialAnchors_001", ref evrinitError);
					if (genericInterface != IntPtr.Zero && evrinitError == EVRInitError.None)
					{
						this.m_pVRSpatialAnchors = new CVRSpatialAnchors(genericInterface);
					}
				}
				return this.m_pVRSpatialAnchors;
			}

			// Token: 0x06000794 RID: 1940 RVA: 0x00006D7C File Offset: 0x00004F7C
			public CVRNotifications VRNotifications()
			{
				this.CheckClear();
				if (this.m_pVRNotifications == null)
				{
					EVRInitError evrinitError = EVRInitError.None;
					IntPtr genericInterface = OpenVRInterop.GetGenericInterface("FnTable:IVRNotifications_002", ref evrinitError);
					if (genericInterface != IntPtr.Zero && evrinitError == EVRInitError.None)
					{
						this.m_pVRNotifications = new CVRNotifications(genericInterface);
					}
				}
				return this.m_pVRNotifications;
			}

			// Token: 0x040008B2 RID: 2226
			private CVRSystem m_pVRSystem;

			// Token: 0x040008B3 RID: 2227
			private CVRChaperone m_pVRChaperone;

			// Token: 0x040008B4 RID: 2228
			private CVRChaperoneSetup m_pVRChaperoneSetup;

			// Token: 0x040008B5 RID: 2229
			private CVRCompositor m_pVRCompositor;

			// Token: 0x040008B6 RID: 2230
			private CVROverlay m_pVROverlay;

			// Token: 0x040008B7 RID: 2231
			private CVRRenderModels m_pVRRenderModels;

			// Token: 0x040008B8 RID: 2232
			private CVRExtendedDisplay m_pVRExtendedDisplay;

			// Token: 0x040008B9 RID: 2233
			private CVRSettings m_pVRSettings;

			// Token: 0x040008BA RID: 2234
			private CVRApplications m_pVRApplications;

			// Token: 0x040008BB RID: 2235
			private CVRScreenshots m_pVRScreenshots;

			// Token: 0x040008BC RID: 2236
			private CVRTrackedCamera m_pVRTrackedCamera;

			// Token: 0x040008BD RID: 2237
			private CVRInput m_pVRInput;

			// Token: 0x040008BE RID: 2238
			private CVRIOBuffer m_pVRIOBuffer;

			// Token: 0x040008BF RID: 2239
			private CVRSpatialAnchors m_pVRSpatialAnchors;

			// Token: 0x040008C0 RID: 2240
			private CVRNotifications m_pVRNotifications;
		}
	}
}
