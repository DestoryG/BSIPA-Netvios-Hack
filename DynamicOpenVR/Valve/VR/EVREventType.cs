using System;

namespace Valve.VR
{
	// Token: 0x02000031 RID: 49
	public enum EVREventType
	{
		// Token: 0x04000255 RID: 597
		VREvent_None,
		// Token: 0x04000256 RID: 598
		VREvent_TrackedDeviceActivated = 100,
		// Token: 0x04000257 RID: 599
		VREvent_TrackedDeviceDeactivated,
		// Token: 0x04000258 RID: 600
		VREvent_TrackedDeviceUpdated,
		// Token: 0x04000259 RID: 601
		VREvent_TrackedDeviceUserInteractionStarted,
		// Token: 0x0400025A RID: 602
		VREvent_TrackedDeviceUserInteractionEnded,
		// Token: 0x0400025B RID: 603
		VREvent_IpdChanged,
		// Token: 0x0400025C RID: 604
		VREvent_EnterStandbyMode,
		// Token: 0x0400025D RID: 605
		VREvent_LeaveStandbyMode,
		// Token: 0x0400025E RID: 606
		VREvent_TrackedDeviceRoleChanged,
		// Token: 0x0400025F RID: 607
		VREvent_WatchdogWakeUpRequested,
		// Token: 0x04000260 RID: 608
		VREvent_LensDistortionChanged,
		// Token: 0x04000261 RID: 609
		VREvent_PropertyChanged,
		// Token: 0x04000262 RID: 610
		VREvent_WirelessDisconnect,
		// Token: 0x04000263 RID: 611
		VREvent_WirelessReconnect,
		// Token: 0x04000264 RID: 612
		VREvent_ButtonPress = 200,
		// Token: 0x04000265 RID: 613
		VREvent_ButtonUnpress,
		// Token: 0x04000266 RID: 614
		VREvent_ButtonTouch,
		// Token: 0x04000267 RID: 615
		VREvent_ButtonUntouch,
		// Token: 0x04000268 RID: 616
		VREvent_DualAnalog_Press = 250,
		// Token: 0x04000269 RID: 617
		VREvent_DualAnalog_Unpress,
		// Token: 0x0400026A RID: 618
		VREvent_DualAnalog_Touch,
		// Token: 0x0400026B RID: 619
		VREvent_DualAnalog_Untouch,
		// Token: 0x0400026C RID: 620
		VREvent_DualAnalog_Move,
		// Token: 0x0400026D RID: 621
		VREvent_DualAnalog_ModeSwitch1,
		// Token: 0x0400026E RID: 622
		VREvent_DualAnalog_ModeSwitch2,
		// Token: 0x0400026F RID: 623
		VREvent_DualAnalog_Cancel,
		// Token: 0x04000270 RID: 624
		VREvent_MouseMove = 300,
		// Token: 0x04000271 RID: 625
		VREvent_MouseButtonDown,
		// Token: 0x04000272 RID: 626
		VREvent_MouseButtonUp,
		// Token: 0x04000273 RID: 627
		VREvent_FocusEnter,
		// Token: 0x04000274 RID: 628
		VREvent_FocusLeave,
		// Token: 0x04000275 RID: 629
		VREvent_ScrollDiscrete,
		// Token: 0x04000276 RID: 630
		VREvent_TouchPadMove,
		// Token: 0x04000277 RID: 631
		VREvent_OverlayFocusChanged,
		// Token: 0x04000278 RID: 632
		VREvent_ReloadOverlays,
		// Token: 0x04000279 RID: 633
		VREvent_ScrollSmooth,
		// Token: 0x0400027A RID: 634
		VREvent_InputFocusCaptured = 400,
		// Token: 0x0400027B RID: 635
		VREvent_InputFocusReleased,
		// Token: 0x0400027C RID: 636
		VREvent_SceneFocusLost,
		// Token: 0x0400027D RID: 637
		VREvent_SceneFocusGained,
		// Token: 0x0400027E RID: 638
		VREvent_SceneApplicationChanged,
		// Token: 0x0400027F RID: 639
		VREvent_SceneFocusChanged,
		// Token: 0x04000280 RID: 640
		VREvent_InputFocusChanged,
		// Token: 0x04000281 RID: 641
		VREvent_SceneApplicationSecondaryRenderingStarted,
		// Token: 0x04000282 RID: 642
		VREvent_SceneApplicationUsingWrongGraphicsAdapter,
		// Token: 0x04000283 RID: 643
		VREvent_ActionBindingReloaded,
		// Token: 0x04000284 RID: 644
		VREvent_HideRenderModels,
		// Token: 0x04000285 RID: 645
		VREvent_ShowRenderModels,
		// Token: 0x04000286 RID: 646
		VREvent_ConsoleOpened = 420,
		// Token: 0x04000287 RID: 647
		VREvent_ConsoleClosed,
		// Token: 0x04000288 RID: 648
		VREvent_OverlayShown = 500,
		// Token: 0x04000289 RID: 649
		VREvent_OverlayHidden,
		// Token: 0x0400028A RID: 650
		VREvent_DashboardActivated,
		// Token: 0x0400028B RID: 651
		VREvent_DashboardDeactivated,
		// Token: 0x0400028C RID: 652
		VREvent_DashboardRequested = 505,
		// Token: 0x0400028D RID: 653
		VREvent_ResetDashboard,
		// Token: 0x0400028E RID: 654
		VREvent_RenderToast,
		// Token: 0x0400028F RID: 655
		VREvent_ImageLoaded,
		// Token: 0x04000290 RID: 656
		VREvent_ShowKeyboard,
		// Token: 0x04000291 RID: 657
		VREvent_HideKeyboard,
		// Token: 0x04000292 RID: 658
		VREvent_OverlayGamepadFocusGained,
		// Token: 0x04000293 RID: 659
		VREvent_OverlayGamepadFocusLost,
		// Token: 0x04000294 RID: 660
		VREvent_OverlaySharedTextureChanged,
		// Token: 0x04000295 RID: 661
		VREvent_ScreenshotTriggered = 516,
		// Token: 0x04000296 RID: 662
		VREvent_ImageFailed,
		// Token: 0x04000297 RID: 663
		VREvent_DashboardOverlayCreated,
		// Token: 0x04000298 RID: 664
		VREvent_SwitchGamepadFocus,
		// Token: 0x04000299 RID: 665
		VREvent_RequestScreenshot,
		// Token: 0x0400029A RID: 666
		VREvent_ScreenshotTaken,
		// Token: 0x0400029B RID: 667
		VREvent_ScreenshotFailed,
		// Token: 0x0400029C RID: 668
		VREvent_SubmitScreenshotToDashboard,
		// Token: 0x0400029D RID: 669
		VREvent_ScreenshotProgressToDashboard,
		// Token: 0x0400029E RID: 670
		VREvent_PrimaryDashboardDeviceChanged,
		// Token: 0x0400029F RID: 671
		VREvent_RoomViewShown,
		// Token: 0x040002A0 RID: 672
		VREvent_RoomViewHidden,
		// Token: 0x040002A1 RID: 673
		VREvent_ShowUI,
		// Token: 0x040002A2 RID: 674
		VREvent_ShowDevTools,
		// Token: 0x040002A3 RID: 675
		VREvent_Notification_Shown = 600,
		// Token: 0x040002A4 RID: 676
		VREvent_Notification_Hidden,
		// Token: 0x040002A5 RID: 677
		VREvent_Notification_BeginInteraction,
		// Token: 0x040002A6 RID: 678
		VREvent_Notification_Destroyed,
		// Token: 0x040002A7 RID: 679
		VREvent_Quit = 700,
		// Token: 0x040002A8 RID: 680
		VREvent_ProcessQuit,
		// Token: 0x040002A9 RID: 681
		VREvent_QuitAborted_UserPrompt,
		// Token: 0x040002AA RID: 682
		VREvent_QuitAcknowledged,
		// Token: 0x040002AB RID: 683
		VREvent_DriverRequestedQuit,
		// Token: 0x040002AC RID: 684
		VREvent_RestartRequested,
		// Token: 0x040002AD RID: 685
		VREvent_ChaperoneDataHasChanged = 800,
		// Token: 0x040002AE RID: 686
		VREvent_ChaperoneUniverseHasChanged,
		// Token: 0x040002AF RID: 687
		VREvent_ChaperoneTempDataHasChanged,
		// Token: 0x040002B0 RID: 688
		VREvent_ChaperoneSettingsHaveChanged,
		// Token: 0x040002B1 RID: 689
		VREvent_SeatedZeroPoseReset,
		// Token: 0x040002B2 RID: 690
		VREvent_ChaperoneFlushCache,
		// Token: 0x040002B3 RID: 691
		VREvent_AudioSettingsHaveChanged = 820,
		// Token: 0x040002B4 RID: 692
		VREvent_BackgroundSettingHasChanged = 850,
		// Token: 0x040002B5 RID: 693
		VREvent_CameraSettingsHaveChanged,
		// Token: 0x040002B6 RID: 694
		VREvent_ReprojectionSettingHasChanged,
		// Token: 0x040002B7 RID: 695
		VREvent_ModelSkinSettingsHaveChanged,
		// Token: 0x040002B8 RID: 696
		VREvent_EnvironmentSettingsHaveChanged,
		// Token: 0x040002B9 RID: 697
		VREvent_PowerSettingsHaveChanged,
		// Token: 0x040002BA RID: 698
		VREvent_EnableHomeAppSettingsHaveChanged,
		// Token: 0x040002BB RID: 699
		VREvent_SteamVRSectionSettingChanged,
		// Token: 0x040002BC RID: 700
		VREvent_LighthouseSectionSettingChanged,
		// Token: 0x040002BD RID: 701
		VREvent_NullSectionSettingChanged,
		// Token: 0x040002BE RID: 702
		VREvent_UserInterfaceSectionSettingChanged,
		// Token: 0x040002BF RID: 703
		VREvent_NotificationsSectionSettingChanged,
		// Token: 0x040002C0 RID: 704
		VREvent_KeyboardSectionSettingChanged,
		// Token: 0x040002C1 RID: 705
		VREvent_PerfSectionSettingChanged,
		// Token: 0x040002C2 RID: 706
		VREvent_DashboardSectionSettingChanged,
		// Token: 0x040002C3 RID: 707
		VREvent_WebInterfaceSectionSettingChanged,
		// Token: 0x040002C4 RID: 708
		VREvent_TrackersSectionSettingChanged,
		// Token: 0x040002C5 RID: 709
		VREvent_LastKnownSectionSettingChanged,
		// Token: 0x040002C6 RID: 710
		VREvent_DismissedWarningsSectionSettingChanged,
		// Token: 0x040002C7 RID: 711
		VREvent_StatusUpdate = 900,
		// Token: 0x040002C8 RID: 712
		VREvent_WebInterface_InstallDriverCompleted = 950,
		// Token: 0x040002C9 RID: 713
		VREvent_MCImageUpdated = 1000,
		// Token: 0x040002CA RID: 714
		VREvent_FirmwareUpdateStarted = 1100,
		// Token: 0x040002CB RID: 715
		VREvent_FirmwareUpdateFinished,
		// Token: 0x040002CC RID: 716
		VREvent_KeyboardClosed = 1200,
		// Token: 0x040002CD RID: 717
		VREvent_KeyboardCharInput,
		// Token: 0x040002CE RID: 718
		VREvent_KeyboardDone,
		// Token: 0x040002CF RID: 719
		VREvent_ApplicationTransitionStarted = 1300,
		// Token: 0x040002D0 RID: 720
		VREvent_ApplicationTransitionAborted,
		// Token: 0x040002D1 RID: 721
		VREvent_ApplicationTransitionNewAppStarted,
		// Token: 0x040002D2 RID: 722
		VREvent_ApplicationListUpdated,
		// Token: 0x040002D3 RID: 723
		VREvent_ApplicationMimeTypeLoad,
		// Token: 0x040002D4 RID: 724
		VREvent_ApplicationTransitionNewAppLaunchComplete,
		// Token: 0x040002D5 RID: 725
		VREvent_ProcessConnected,
		// Token: 0x040002D6 RID: 726
		VREvent_ProcessDisconnected,
		// Token: 0x040002D7 RID: 727
		VREvent_Compositor_MirrorWindowShown = 1400,
		// Token: 0x040002D8 RID: 728
		VREvent_Compositor_MirrorWindowHidden,
		// Token: 0x040002D9 RID: 729
		VREvent_Compositor_ChaperoneBoundsShown = 1410,
		// Token: 0x040002DA RID: 730
		VREvent_Compositor_ChaperoneBoundsHidden,
		// Token: 0x040002DB RID: 731
		VREvent_Compositor_DisplayDisconnected,
		// Token: 0x040002DC RID: 732
		VREvent_Compositor_DisplayReconnected,
		// Token: 0x040002DD RID: 733
		VREvent_Compositor_HDCPError,
		// Token: 0x040002DE RID: 734
		VREvent_Compositor_ApplicationNotResponding,
		// Token: 0x040002DF RID: 735
		VREvent_Compositor_ApplicationResumed,
		// Token: 0x040002E0 RID: 736
		VREvent_TrackedCamera_StartVideoStream = 1500,
		// Token: 0x040002E1 RID: 737
		VREvent_TrackedCamera_StopVideoStream,
		// Token: 0x040002E2 RID: 738
		VREvent_TrackedCamera_PauseVideoStream,
		// Token: 0x040002E3 RID: 739
		VREvent_TrackedCamera_ResumeVideoStream,
		// Token: 0x040002E4 RID: 740
		VREvent_TrackedCamera_EditingSurface = 1550,
		// Token: 0x040002E5 RID: 741
		VREvent_PerformanceTest_EnableCapture = 1600,
		// Token: 0x040002E6 RID: 742
		VREvent_PerformanceTest_DisableCapture,
		// Token: 0x040002E7 RID: 743
		VREvent_PerformanceTest_FidelityLevel,
		// Token: 0x040002E8 RID: 744
		VREvent_MessageOverlay_Closed = 1650,
		// Token: 0x040002E9 RID: 745
		VREvent_MessageOverlayCloseRequested,
		// Token: 0x040002EA RID: 746
		VREvent_Input_HapticVibration = 1700,
		// Token: 0x040002EB RID: 747
		VREvent_Input_BindingLoadFailed,
		// Token: 0x040002EC RID: 748
		VREvent_Input_BindingLoadSuccessful,
		// Token: 0x040002ED RID: 749
		VREvent_Input_ActionManifestReloaded,
		// Token: 0x040002EE RID: 750
		VREvent_Input_ActionManifestLoadFailed,
		// Token: 0x040002EF RID: 751
		VREvent_Input_ProgressUpdate,
		// Token: 0x040002F0 RID: 752
		VREvent_Input_TrackerActivated,
		// Token: 0x040002F1 RID: 753
		VREvent_Input_BindingsUpdated,
		// Token: 0x040002F2 RID: 754
		VREvent_SpatialAnchors_PoseUpdated = 1800,
		// Token: 0x040002F3 RID: 755
		VREvent_SpatialAnchors_DescriptorUpdated,
		// Token: 0x040002F4 RID: 756
		VREvent_SpatialAnchors_RequestPoseUpdate,
		// Token: 0x040002F5 RID: 757
		VREvent_SpatialAnchors_RequestDescriptorUpdate,
		// Token: 0x040002F6 RID: 758
		VREvent_SystemReport_Started = 1900,
		// Token: 0x040002F7 RID: 759
		VREvent_VendorSpecific_Reserved_Start = 10000,
		// Token: 0x040002F8 RID: 760
		VREvent_VendorSpecific_Reserved_End = 19999
	}
}
