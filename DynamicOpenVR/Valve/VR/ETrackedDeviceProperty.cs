using System;

namespace Valve.VR
{
	// Token: 0x0200002D RID: 45
	public enum ETrackedDeviceProperty
	{
		// Token: 0x04000188 RID: 392
		Prop_Invalid,
		// Token: 0x04000189 RID: 393
		Prop_TrackingSystemName_String = 1000,
		// Token: 0x0400018A RID: 394
		Prop_ModelNumber_String,
		// Token: 0x0400018B RID: 395
		Prop_SerialNumber_String,
		// Token: 0x0400018C RID: 396
		Prop_RenderModelName_String,
		// Token: 0x0400018D RID: 397
		Prop_WillDriftInYaw_Bool,
		// Token: 0x0400018E RID: 398
		Prop_ManufacturerName_String,
		// Token: 0x0400018F RID: 399
		Prop_TrackingFirmwareVersion_String,
		// Token: 0x04000190 RID: 400
		Prop_HardwareRevision_String,
		// Token: 0x04000191 RID: 401
		Prop_AllWirelessDongleDescriptions_String,
		// Token: 0x04000192 RID: 402
		Prop_ConnectedWirelessDongle_String,
		// Token: 0x04000193 RID: 403
		Prop_DeviceIsWireless_Bool,
		// Token: 0x04000194 RID: 404
		Prop_DeviceIsCharging_Bool,
		// Token: 0x04000195 RID: 405
		Prop_DeviceBatteryPercentage_Float,
		// Token: 0x04000196 RID: 406
		Prop_StatusDisplayTransform_Matrix34,
		// Token: 0x04000197 RID: 407
		Prop_Firmware_UpdateAvailable_Bool,
		// Token: 0x04000198 RID: 408
		Prop_Firmware_ManualUpdate_Bool,
		// Token: 0x04000199 RID: 409
		Prop_Firmware_ManualUpdateURL_String,
		// Token: 0x0400019A RID: 410
		Prop_HardwareRevision_Uint64,
		// Token: 0x0400019B RID: 411
		Prop_FirmwareVersion_Uint64,
		// Token: 0x0400019C RID: 412
		Prop_FPGAVersion_Uint64,
		// Token: 0x0400019D RID: 413
		Prop_VRCVersion_Uint64,
		// Token: 0x0400019E RID: 414
		Prop_RadioVersion_Uint64,
		// Token: 0x0400019F RID: 415
		Prop_DongleVersion_Uint64,
		// Token: 0x040001A0 RID: 416
		Prop_BlockServerShutdown_Bool,
		// Token: 0x040001A1 RID: 417
		Prop_CanUnifyCoordinateSystemWithHmd_Bool,
		// Token: 0x040001A2 RID: 418
		Prop_ContainsProximitySensor_Bool,
		// Token: 0x040001A3 RID: 419
		Prop_DeviceProvidesBatteryStatus_Bool,
		// Token: 0x040001A4 RID: 420
		Prop_DeviceCanPowerOff_Bool,
		// Token: 0x040001A5 RID: 421
		Prop_Firmware_ProgrammingTarget_String,
		// Token: 0x040001A6 RID: 422
		Prop_DeviceClass_Int32,
		// Token: 0x040001A7 RID: 423
		Prop_HasCamera_Bool,
		// Token: 0x040001A8 RID: 424
		Prop_DriverVersion_String,
		// Token: 0x040001A9 RID: 425
		Prop_Firmware_ForceUpdateRequired_Bool,
		// Token: 0x040001AA RID: 426
		Prop_ViveSystemButtonFixRequired_Bool,
		// Token: 0x040001AB RID: 427
		Prop_ParentDriver_Uint64,
		// Token: 0x040001AC RID: 428
		Prop_ResourceRoot_String,
		// Token: 0x040001AD RID: 429
		Prop_RegisteredDeviceType_String,
		// Token: 0x040001AE RID: 430
		Prop_InputProfilePath_String,
		// Token: 0x040001AF RID: 431
		Prop_NeverTracked_Bool,
		// Token: 0x040001B0 RID: 432
		Prop_NumCameras_Int32,
		// Token: 0x040001B1 RID: 433
		Prop_CameraFrameLayout_Int32,
		// Token: 0x040001B2 RID: 434
		Prop_CameraStreamFormat_Int32,
		// Token: 0x040001B3 RID: 435
		Prop_AdditionalDeviceSettingsPath_String,
		// Token: 0x040001B4 RID: 436
		Prop_Identifiable_Bool,
		// Token: 0x040001B5 RID: 437
		Prop_BootloaderVersion_Uint64,
		// Token: 0x040001B6 RID: 438
		Prop_AdditionalSystemReportData_String,
		// Token: 0x040001B7 RID: 439
		Prop_CompositeFirmwareVersion_String,
		// Token: 0x040001B8 RID: 440
		Prop_ReportsTimeSinceVSync_Bool = 2000,
		// Token: 0x040001B9 RID: 441
		Prop_SecondsFromVsyncToPhotons_Float,
		// Token: 0x040001BA RID: 442
		Prop_DisplayFrequency_Float,
		// Token: 0x040001BB RID: 443
		Prop_UserIpdMeters_Float,
		// Token: 0x040001BC RID: 444
		Prop_CurrentUniverseId_Uint64,
		// Token: 0x040001BD RID: 445
		Prop_PreviousUniverseId_Uint64,
		// Token: 0x040001BE RID: 446
		Prop_DisplayFirmwareVersion_Uint64,
		// Token: 0x040001BF RID: 447
		Prop_IsOnDesktop_Bool,
		// Token: 0x040001C0 RID: 448
		Prop_DisplayMCType_Int32,
		// Token: 0x040001C1 RID: 449
		Prop_DisplayMCOffset_Float,
		// Token: 0x040001C2 RID: 450
		Prop_DisplayMCScale_Float,
		// Token: 0x040001C3 RID: 451
		Prop_EdidVendorID_Int32,
		// Token: 0x040001C4 RID: 452
		Prop_DisplayMCImageLeft_String,
		// Token: 0x040001C5 RID: 453
		Prop_DisplayMCImageRight_String,
		// Token: 0x040001C6 RID: 454
		Prop_DisplayGCBlackClamp_Float,
		// Token: 0x040001C7 RID: 455
		Prop_EdidProductID_Int32,
		// Token: 0x040001C8 RID: 456
		Prop_CameraToHeadTransform_Matrix34,
		// Token: 0x040001C9 RID: 457
		Prop_DisplayGCType_Int32,
		// Token: 0x040001CA RID: 458
		Prop_DisplayGCOffset_Float,
		// Token: 0x040001CB RID: 459
		Prop_DisplayGCScale_Float,
		// Token: 0x040001CC RID: 460
		Prop_DisplayGCPrescale_Float,
		// Token: 0x040001CD RID: 461
		Prop_DisplayGCImage_String,
		// Token: 0x040001CE RID: 462
		Prop_LensCenterLeftU_Float,
		// Token: 0x040001CF RID: 463
		Prop_LensCenterLeftV_Float,
		// Token: 0x040001D0 RID: 464
		Prop_LensCenterRightU_Float,
		// Token: 0x040001D1 RID: 465
		Prop_LensCenterRightV_Float,
		// Token: 0x040001D2 RID: 466
		Prop_UserHeadToEyeDepthMeters_Float,
		// Token: 0x040001D3 RID: 467
		Prop_CameraFirmwareVersion_Uint64,
		// Token: 0x040001D4 RID: 468
		Prop_CameraFirmwareDescription_String,
		// Token: 0x040001D5 RID: 469
		Prop_DisplayFPGAVersion_Uint64,
		// Token: 0x040001D6 RID: 470
		Prop_DisplayBootloaderVersion_Uint64,
		// Token: 0x040001D7 RID: 471
		Prop_DisplayHardwareVersion_Uint64,
		// Token: 0x040001D8 RID: 472
		Prop_AudioFirmwareVersion_Uint64,
		// Token: 0x040001D9 RID: 473
		Prop_CameraCompatibilityMode_Int32,
		// Token: 0x040001DA RID: 474
		Prop_ScreenshotHorizontalFieldOfViewDegrees_Float,
		// Token: 0x040001DB RID: 475
		Prop_ScreenshotVerticalFieldOfViewDegrees_Float,
		// Token: 0x040001DC RID: 476
		Prop_DisplaySuppressed_Bool,
		// Token: 0x040001DD RID: 477
		Prop_DisplayAllowNightMode_Bool,
		// Token: 0x040001DE RID: 478
		Prop_DisplayMCImageWidth_Int32,
		// Token: 0x040001DF RID: 479
		Prop_DisplayMCImageHeight_Int32,
		// Token: 0x040001E0 RID: 480
		Prop_DisplayMCImageNumChannels_Int32,
		// Token: 0x040001E1 RID: 481
		Prop_DisplayMCImageData_Binary,
		// Token: 0x040001E2 RID: 482
		Prop_SecondsFromPhotonsToVblank_Float,
		// Token: 0x040001E3 RID: 483
		Prop_DriverDirectModeSendsVsyncEvents_Bool,
		// Token: 0x040001E4 RID: 484
		Prop_DisplayDebugMode_Bool,
		// Token: 0x040001E5 RID: 485
		Prop_GraphicsAdapterLuid_Uint64,
		// Token: 0x040001E6 RID: 486
		Prop_DriverProvidedChaperonePath_String = 2048,
		// Token: 0x040001E7 RID: 487
		Prop_ExpectedTrackingReferenceCount_Int32,
		// Token: 0x040001E8 RID: 488
		Prop_ExpectedControllerCount_Int32,
		// Token: 0x040001E9 RID: 489
		Prop_NamedIconPathControllerLeftDeviceOff_String,
		// Token: 0x040001EA RID: 490
		Prop_NamedIconPathControllerRightDeviceOff_String,
		// Token: 0x040001EB RID: 491
		Prop_NamedIconPathTrackingReferenceDeviceOff_String,
		// Token: 0x040001EC RID: 492
		Prop_DoNotApplyPrediction_Bool,
		// Token: 0x040001ED RID: 493
		Prop_CameraToHeadTransforms_Matrix34_Array,
		// Token: 0x040001EE RID: 494
		Prop_DistortionMeshResolution_Int32,
		// Token: 0x040001EF RID: 495
		Prop_DriverIsDrawingControllers_Bool,
		// Token: 0x040001F0 RID: 496
		Prop_DriverRequestsApplicationPause_Bool,
		// Token: 0x040001F1 RID: 497
		Prop_DriverRequestsReducedRendering_Bool,
		// Token: 0x040001F2 RID: 498
		Prop_MinimumIpdStepMeters_Float,
		// Token: 0x040001F3 RID: 499
		Prop_AudioBridgeFirmwareVersion_Uint64,
		// Token: 0x040001F4 RID: 500
		Prop_ImageBridgeFirmwareVersion_Uint64,
		// Token: 0x040001F5 RID: 501
		Prop_ImuToHeadTransform_Matrix34,
		// Token: 0x040001F6 RID: 502
		Prop_ImuFactoryGyroBias_Vector3,
		// Token: 0x040001F7 RID: 503
		Prop_ImuFactoryGyroScale_Vector3,
		// Token: 0x040001F8 RID: 504
		Prop_ImuFactoryAccelerometerBias_Vector3,
		// Token: 0x040001F9 RID: 505
		Prop_ImuFactoryAccelerometerScale_Vector3,
		// Token: 0x040001FA RID: 506
		Prop_ConfigurationIncludesLighthouse20Features_Bool = 2069,
		// Token: 0x040001FB RID: 507
		Prop_AdditionalRadioFeatures_Uint64,
		// Token: 0x040001FC RID: 508
		Prop_CameraWhiteBalance_Vector4_Array,
		// Token: 0x040001FD RID: 509
		Prop_CameraDistortionFunction_Int32_Array,
		// Token: 0x040001FE RID: 510
		Prop_CameraDistortionCoefficients_Float_Array,
		// Token: 0x040001FF RID: 511
		Prop_ExpectedControllerType_String,
		// Token: 0x04000200 RID: 512
		Prop_DriverRequestedMuraCorrectionMode_Int32 = 2200,
		// Token: 0x04000201 RID: 513
		Prop_DriverRequestedMuraFeather_InnerLeft_Int32,
		// Token: 0x04000202 RID: 514
		Prop_DriverRequestedMuraFeather_InnerRight_Int32,
		// Token: 0x04000203 RID: 515
		Prop_DriverRequestedMuraFeather_InnerTop_Int32,
		// Token: 0x04000204 RID: 516
		Prop_DriverRequestedMuraFeather_InnerBottom_Int32,
		// Token: 0x04000205 RID: 517
		Prop_DriverRequestedMuraFeather_OuterLeft_Int32,
		// Token: 0x04000206 RID: 518
		Prop_DriverRequestedMuraFeather_OuterRight_Int32,
		// Token: 0x04000207 RID: 519
		Prop_DriverRequestedMuraFeather_OuterTop_Int32,
		// Token: 0x04000208 RID: 520
		Prop_DriverRequestedMuraFeather_OuterBottom_Int32,
		// Token: 0x04000209 RID: 521
		Prop_AttachedDeviceId_String = 3000,
		// Token: 0x0400020A RID: 522
		Prop_SupportedButtons_Uint64,
		// Token: 0x0400020B RID: 523
		Prop_Axis0Type_Int32,
		// Token: 0x0400020C RID: 524
		Prop_Axis1Type_Int32,
		// Token: 0x0400020D RID: 525
		Prop_Axis2Type_Int32,
		// Token: 0x0400020E RID: 526
		Prop_Axis3Type_Int32,
		// Token: 0x0400020F RID: 527
		Prop_Axis4Type_Int32,
		// Token: 0x04000210 RID: 528
		Prop_ControllerRoleHint_Int32,
		// Token: 0x04000211 RID: 529
		Prop_FieldOfViewLeftDegrees_Float = 4000,
		// Token: 0x04000212 RID: 530
		Prop_FieldOfViewRightDegrees_Float,
		// Token: 0x04000213 RID: 531
		Prop_FieldOfViewTopDegrees_Float,
		// Token: 0x04000214 RID: 532
		Prop_FieldOfViewBottomDegrees_Float,
		// Token: 0x04000215 RID: 533
		Prop_TrackingRangeMinimumMeters_Float,
		// Token: 0x04000216 RID: 534
		Prop_TrackingRangeMaximumMeters_Float,
		// Token: 0x04000217 RID: 535
		Prop_ModeLabel_String,
		// Token: 0x04000218 RID: 536
		Prop_CanWirelessIdentify_Bool,
		// Token: 0x04000219 RID: 537
		Prop_Nonce_Int32,
		// Token: 0x0400021A RID: 538
		Prop_IconPathName_String = 5000,
		// Token: 0x0400021B RID: 539
		Prop_NamedIconPathDeviceOff_String,
		// Token: 0x0400021C RID: 540
		Prop_NamedIconPathDeviceSearching_String,
		// Token: 0x0400021D RID: 541
		Prop_NamedIconPathDeviceSearchingAlert_String,
		// Token: 0x0400021E RID: 542
		Prop_NamedIconPathDeviceReady_String,
		// Token: 0x0400021F RID: 543
		Prop_NamedIconPathDeviceReadyAlert_String,
		// Token: 0x04000220 RID: 544
		Prop_NamedIconPathDeviceNotReady_String,
		// Token: 0x04000221 RID: 545
		Prop_NamedIconPathDeviceStandby_String,
		// Token: 0x04000222 RID: 546
		Prop_NamedIconPathDeviceAlertLow_String,
		// Token: 0x04000223 RID: 547
		Prop_DisplayHiddenArea_Binary_Start = 5100,
		// Token: 0x04000224 RID: 548
		Prop_DisplayHiddenArea_Binary_End = 5150,
		// Token: 0x04000225 RID: 549
		Prop_ParentContainer,
		// Token: 0x04000226 RID: 550
		Prop_UserConfigPath_String = 6000,
		// Token: 0x04000227 RID: 551
		Prop_InstallPath_String,
		// Token: 0x04000228 RID: 552
		Prop_HasDisplayComponent_Bool,
		// Token: 0x04000229 RID: 553
		Prop_HasControllerComponent_Bool,
		// Token: 0x0400022A RID: 554
		Prop_HasCameraComponent_Bool,
		// Token: 0x0400022B RID: 555
		Prop_HasDriverDirectModeComponent_Bool,
		// Token: 0x0400022C RID: 556
		Prop_HasVirtualDisplayComponent_Bool,
		// Token: 0x0400022D RID: 557
		Prop_HasSpatialAnchorsSupport_Bool,
		// Token: 0x0400022E RID: 558
		Prop_ControllerType_String = 7000,
		// Token: 0x0400022F RID: 559
		Prop_LegacyInputProfile_String,
		// Token: 0x04000230 RID: 560
		Prop_ControllerHandSelectionPriority_Int32,
		// Token: 0x04000231 RID: 561
		Prop_VendorSpecific_Reserved_Start = 10000,
		// Token: 0x04000232 RID: 562
		Prop_VendorSpecific_Reserved_End = 10999,
		// Token: 0x04000233 RID: 563
		Prop_TrackedDeviceProperty_Max = 1000000
	}
}
