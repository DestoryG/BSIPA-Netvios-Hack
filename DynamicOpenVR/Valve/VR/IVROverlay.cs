using System;
using System.Runtime.InteropServices;
using System.Text;

namespace Valve.VR
{
	// Token: 0x02000009 RID: 9
	public struct IVROverlay
	{
		// Token: 0x040000A5 RID: 165
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVROverlay._FindOverlay FindOverlay;

		// Token: 0x040000A6 RID: 166
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVROverlay._CreateOverlay CreateOverlay;

		// Token: 0x040000A7 RID: 167
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVROverlay._DestroyOverlay DestroyOverlay;

		// Token: 0x040000A8 RID: 168
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVROverlay._SetHighQualityOverlay SetHighQualityOverlay;

		// Token: 0x040000A9 RID: 169
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVROverlay._GetHighQualityOverlay GetHighQualityOverlay;

		// Token: 0x040000AA RID: 170
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVROverlay._GetOverlayKey GetOverlayKey;

		// Token: 0x040000AB RID: 171
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVROverlay._GetOverlayName GetOverlayName;

		// Token: 0x040000AC RID: 172
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVROverlay._SetOverlayName SetOverlayName;

		// Token: 0x040000AD RID: 173
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVROverlay._GetOverlayImageData GetOverlayImageData;

		// Token: 0x040000AE RID: 174
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVROverlay._GetOverlayErrorNameFromEnum GetOverlayErrorNameFromEnum;

		// Token: 0x040000AF RID: 175
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVROverlay._SetOverlayRenderingPid SetOverlayRenderingPid;

		// Token: 0x040000B0 RID: 176
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVROverlay._GetOverlayRenderingPid GetOverlayRenderingPid;

		// Token: 0x040000B1 RID: 177
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVROverlay._SetOverlayFlag SetOverlayFlag;

		// Token: 0x040000B2 RID: 178
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVROverlay._GetOverlayFlag GetOverlayFlag;

		// Token: 0x040000B3 RID: 179
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVROverlay._SetOverlayColor SetOverlayColor;

		// Token: 0x040000B4 RID: 180
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVROverlay._GetOverlayColor GetOverlayColor;

		// Token: 0x040000B5 RID: 181
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVROverlay._SetOverlayAlpha SetOverlayAlpha;

		// Token: 0x040000B6 RID: 182
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVROverlay._GetOverlayAlpha GetOverlayAlpha;

		// Token: 0x040000B7 RID: 183
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVROverlay._SetOverlayTexelAspect SetOverlayTexelAspect;

		// Token: 0x040000B8 RID: 184
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVROverlay._GetOverlayTexelAspect GetOverlayTexelAspect;

		// Token: 0x040000B9 RID: 185
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVROverlay._SetOverlaySortOrder SetOverlaySortOrder;

		// Token: 0x040000BA RID: 186
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVROverlay._GetOverlaySortOrder GetOverlaySortOrder;

		// Token: 0x040000BB RID: 187
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVROverlay._SetOverlayWidthInMeters SetOverlayWidthInMeters;

		// Token: 0x040000BC RID: 188
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVROverlay._GetOverlayWidthInMeters GetOverlayWidthInMeters;

		// Token: 0x040000BD RID: 189
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVROverlay._SetOverlayAutoCurveDistanceRangeInMeters SetOverlayAutoCurveDistanceRangeInMeters;

		// Token: 0x040000BE RID: 190
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVROverlay._GetOverlayAutoCurveDistanceRangeInMeters GetOverlayAutoCurveDistanceRangeInMeters;

		// Token: 0x040000BF RID: 191
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVROverlay._SetOverlayTextureColorSpace SetOverlayTextureColorSpace;

		// Token: 0x040000C0 RID: 192
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVROverlay._GetOverlayTextureColorSpace GetOverlayTextureColorSpace;

		// Token: 0x040000C1 RID: 193
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVROverlay._SetOverlayTextureBounds SetOverlayTextureBounds;

		// Token: 0x040000C2 RID: 194
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVROverlay._GetOverlayTextureBounds GetOverlayTextureBounds;

		// Token: 0x040000C3 RID: 195
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVROverlay._GetOverlayRenderModel GetOverlayRenderModel;

		// Token: 0x040000C4 RID: 196
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVROverlay._SetOverlayRenderModel SetOverlayRenderModel;

		// Token: 0x040000C5 RID: 197
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVROverlay._GetOverlayTransformType GetOverlayTransformType;

		// Token: 0x040000C6 RID: 198
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVROverlay._SetOverlayTransformAbsolute SetOverlayTransformAbsolute;

		// Token: 0x040000C7 RID: 199
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVROverlay._GetOverlayTransformAbsolute GetOverlayTransformAbsolute;

		// Token: 0x040000C8 RID: 200
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVROverlay._SetOverlayTransformTrackedDeviceRelative SetOverlayTransformTrackedDeviceRelative;

		// Token: 0x040000C9 RID: 201
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVROverlay._GetOverlayTransformTrackedDeviceRelative GetOverlayTransformTrackedDeviceRelative;

		// Token: 0x040000CA RID: 202
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVROverlay._SetOverlayTransformTrackedDeviceComponent SetOverlayTransformTrackedDeviceComponent;

		// Token: 0x040000CB RID: 203
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVROverlay._GetOverlayTransformTrackedDeviceComponent GetOverlayTransformTrackedDeviceComponent;

		// Token: 0x040000CC RID: 204
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVROverlay._GetOverlayTransformOverlayRelative GetOverlayTransformOverlayRelative;

		// Token: 0x040000CD RID: 205
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVROverlay._SetOverlayTransformOverlayRelative SetOverlayTransformOverlayRelative;

		// Token: 0x040000CE RID: 206
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVROverlay._ShowOverlay ShowOverlay;

		// Token: 0x040000CF RID: 207
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVROverlay._HideOverlay HideOverlay;

		// Token: 0x040000D0 RID: 208
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVROverlay._IsOverlayVisible IsOverlayVisible;

		// Token: 0x040000D1 RID: 209
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVROverlay._GetTransformForOverlayCoordinates GetTransformForOverlayCoordinates;

		// Token: 0x040000D2 RID: 210
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVROverlay._PollNextOverlayEvent PollNextOverlayEvent;

		// Token: 0x040000D3 RID: 211
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVROverlay._GetOverlayInputMethod GetOverlayInputMethod;

		// Token: 0x040000D4 RID: 212
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVROverlay._SetOverlayInputMethod SetOverlayInputMethod;

		// Token: 0x040000D5 RID: 213
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVROverlay._GetOverlayMouseScale GetOverlayMouseScale;

		// Token: 0x040000D6 RID: 214
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVROverlay._SetOverlayMouseScale SetOverlayMouseScale;

		// Token: 0x040000D7 RID: 215
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVROverlay._ComputeOverlayIntersection ComputeOverlayIntersection;

		// Token: 0x040000D8 RID: 216
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVROverlay._IsHoverTargetOverlay IsHoverTargetOverlay;

		// Token: 0x040000D9 RID: 217
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVROverlay._GetGamepadFocusOverlay GetGamepadFocusOverlay;

		// Token: 0x040000DA RID: 218
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVROverlay._SetGamepadFocusOverlay SetGamepadFocusOverlay;

		// Token: 0x040000DB RID: 219
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVROverlay._SetOverlayNeighbor SetOverlayNeighbor;

		// Token: 0x040000DC RID: 220
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVROverlay._MoveGamepadFocusToNeighbor MoveGamepadFocusToNeighbor;

		// Token: 0x040000DD RID: 221
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVROverlay._SetOverlayDualAnalogTransform SetOverlayDualAnalogTransform;

		// Token: 0x040000DE RID: 222
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVROverlay._GetOverlayDualAnalogTransform GetOverlayDualAnalogTransform;

		// Token: 0x040000DF RID: 223
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVROverlay._SetOverlayTexture SetOverlayTexture;

		// Token: 0x040000E0 RID: 224
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVROverlay._ClearOverlayTexture ClearOverlayTexture;

		// Token: 0x040000E1 RID: 225
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVROverlay._SetOverlayRaw SetOverlayRaw;

		// Token: 0x040000E2 RID: 226
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVROverlay._SetOverlayFromFile SetOverlayFromFile;

		// Token: 0x040000E3 RID: 227
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVROverlay._GetOverlayTexture GetOverlayTexture;

		// Token: 0x040000E4 RID: 228
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVROverlay._ReleaseNativeOverlayHandle ReleaseNativeOverlayHandle;

		// Token: 0x040000E5 RID: 229
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVROverlay._GetOverlayTextureSize GetOverlayTextureSize;

		// Token: 0x040000E6 RID: 230
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVROverlay._CreateDashboardOverlay CreateDashboardOverlay;

		// Token: 0x040000E7 RID: 231
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVROverlay._IsDashboardVisible IsDashboardVisible;

		// Token: 0x040000E8 RID: 232
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVROverlay._IsActiveDashboardOverlay IsActiveDashboardOverlay;

		// Token: 0x040000E9 RID: 233
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVROverlay._SetDashboardOverlaySceneProcess SetDashboardOverlaySceneProcess;

		// Token: 0x040000EA RID: 234
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVROverlay._GetDashboardOverlaySceneProcess GetDashboardOverlaySceneProcess;

		// Token: 0x040000EB RID: 235
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVROverlay._ShowDashboard ShowDashboard;

		// Token: 0x040000EC RID: 236
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVROverlay._GetPrimaryDashboardDevice GetPrimaryDashboardDevice;

		// Token: 0x040000ED RID: 237
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVROverlay._ShowKeyboard ShowKeyboard;

		// Token: 0x040000EE RID: 238
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVROverlay._ShowKeyboardForOverlay ShowKeyboardForOverlay;

		// Token: 0x040000EF RID: 239
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVROverlay._GetKeyboardText GetKeyboardText;

		// Token: 0x040000F0 RID: 240
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVROverlay._HideKeyboard HideKeyboard;

		// Token: 0x040000F1 RID: 241
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVROverlay._SetKeyboardTransformAbsolute SetKeyboardTransformAbsolute;

		// Token: 0x040000F2 RID: 242
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVROverlay._SetKeyboardPositionForOverlay SetKeyboardPositionForOverlay;

		// Token: 0x040000F3 RID: 243
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVROverlay._SetOverlayIntersectionMask SetOverlayIntersectionMask;

		// Token: 0x040000F4 RID: 244
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVROverlay._GetOverlayFlags GetOverlayFlags;

		// Token: 0x040000F5 RID: 245
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVROverlay._ShowMessageOverlay ShowMessageOverlay;

		// Token: 0x040000F6 RID: 246
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVROverlay._CloseMessageOverlay CloseMessageOverlay;

		// Token: 0x0200018D RID: 397
		// (Invoke) Token: 0x060004E8 RID: 1256
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate EVROverlayError _FindOverlay(string pchOverlayKey, ref ulong pOverlayHandle);

		// Token: 0x0200018E RID: 398
		// (Invoke) Token: 0x060004EC RID: 1260
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate EVROverlayError _CreateOverlay(string pchOverlayKey, string pchOverlayName, ref ulong pOverlayHandle);

		// Token: 0x0200018F RID: 399
		// (Invoke) Token: 0x060004F0 RID: 1264
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate EVROverlayError _DestroyOverlay(ulong ulOverlayHandle);

		// Token: 0x02000190 RID: 400
		// (Invoke) Token: 0x060004F4 RID: 1268
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate EVROverlayError _SetHighQualityOverlay(ulong ulOverlayHandle);

		// Token: 0x02000191 RID: 401
		// (Invoke) Token: 0x060004F8 RID: 1272
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate ulong _GetHighQualityOverlay();

		// Token: 0x02000192 RID: 402
		// (Invoke) Token: 0x060004FC RID: 1276
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate uint _GetOverlayKey(ulong ulOverlayHandle, StringBuilder pchValue, uint unBufferSize, ref EVROverlayError pError);

		// Token: 0x02000193 RID: 403
		// (Invoke) Token: 0x06000500 RID: 1280
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate uint _GetOverlayName(ulong ulOverlayHandle, StringBuilder pchValue, uint unBufferSize, ref EVROverlayError pError);

		// Token: 0x02000194 RID: 404
		// (Invoke) Token: 0x06000504 RID: 1284
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate EVROverlayError _SetOverlayName(ulong ulOverlayHandle, string pchName);

		// Token: 0x02000195 RID: 405
		// (Invoke) Token: 0x06000508 RID: 1288
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate EVROverlayError _GetOverlayImageData(ulong ulOverlayHandle, IntPtr pvBuffer, uint unBufferSize, ref uint punWidth, ref uint punHeight);

		// Token: 0x02000196 RID: 406
		// (Invoke) Token: 0x0600050C RID: 1292
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate IntPtr _GetOverlayErrorNameFromEnum(EVROverlayError error);

		// Token: 0x02000197 RID: 407
		// (Invoke) Token: 0x06000510 RID: 1296
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate EVROverlayError _SetOverlayRenderingPid(ulong ulOverlayHandle, uint unPID);

		// Token: 0x02000198 RID: 408
		// (Invoke) Token: 0x06000514 RID: 1300
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate uint _GetOverlayRenderingPid(ulong ulOverlayHandle);

		// Token: 0x02000199 RID: 409
		// (Invoke) Token: 0x06000518 RID: 1304
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate EVROverlayError _SetOverlayFlag(ulong ulOverlayHandle, VROverlayFlags eOverlayFlag, bool bEnabled);

		// Token: 0x0200019A RID: 410
		// (Invoke) Token: 0x0600051C RID: 1308
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate EVROverlayError _GetOverlayFlag(ulong ulOverlayHandle, VROverlayFlags eOverlayFlag, ref bool pbEnabled);

		// Token: 0x0200019B RID: 411
		// (Invoke) Token: 0x06000520 RID: 1312
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate EVROverlayError _SetOverlayColor(ulong ulOverlayHandle, float fRed, float fGreen, float fBlue);

		// Token: 0x0200019C RID: 412
		// (Invoke) Token: 0x06000524 RID: 1316
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate EVROverlayError _GetOverlayColor(ulong ulOverlayHandle, ref float pfRed, ref float pfGreen, ref float pfBlue);

		// Token: 0x0200019D RID: 413
		// (Invoke) Token: 0x06000528 RID: 1320
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate EVROverlayError _SetOverlayAlpha(ulong ulOverlayHandle, float fAlpha);

		// Token: 0x0200019E RID: 414
		// (Invoke) Token: 0x0600052C RID: 1324
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate EVROverlayError _GetOverlayAlpha(ulong ulOverlayHandle, ref float pfAlpha);

		// Token: 0x0200019F RID: 415
		// (Invoke) Token: 0x06000530 RID: 1328
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate EVROverlayError _SetOverlayTexelAspect(ulong ulOverlayHandle, float fTexelAspect);

		// Token: 0x020001A0 RID: 416
		// (Invoke) Token: 0x06000534 RID: 1332
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate EVROverlayError _GetOverlayTexelAspect(ulong ulOverlayHandle, ref float pfTexelAspect);

		// Token: 0x020001A1 RID: 417
		// (Invoke) Token: 0x06000538 RID: 1336
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate EVROverlayError _SetOverlaySortOrder(ulong ulOverlayHandle, uint unSortOrder);

		// Token: 0x020001A2 RID: 418
		// (Invoke) Token: 0x0600053C RID: 1340
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate EVROverlayError _GetOverlaySortOrder(ulong ulOverlayHandle, ref uint punSortOrder);

		// Token: 0x020001A3 RID: 419
		// (Invoke) Token: 0x06000540 RID: 1344
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate EVROverlayError _SetOverlayWidthInMeters(ulong ulOverlayHandle, float fWidthInMeters);

		// Token: 0x020001A4 RID: 420
		// (Invoke) Token: 0x06000544 RID: 1348
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate EVROverlayError _GetOverlayWidthInMeters(ulong ulOverlayHandle, ref float pfWidthInMeters);

		// Token: 0x020001A5 RID: 421
		// (Invoke) Token: 0x06000548 RID: 1352
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate EVROverlayError _SetOverlayAutoCurveDistanceRangeInMeters(ulong ulOverlayHandle, float fMinDistanceInMeters, float fMaxDistanceInMeters);

		// Token: 0x020001A6 RID: 422
		// (Invoke) Token: 0x0600054C RID: 1356
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate EVROverlayError _GetOverlayAutoCurveDistanceRangeInMeters(ulong ulOverlayHandle, ref float pfMinDistanceInMeters, ref float pfMaxDistanceInMeters);

		// Token: 0x020001A7 RID: 423
		// (Invoke) Token: 0x06000550 RID: 1360
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate EVROverlayError _SetOverlayTextureColorSpace(ulong ulOverlayHandle, EColorSpace eTextureColorSpace);

		// Token: 0x020001A8 RID: 424
		// (Invoke) Token: 0x06000554 RID: 1364
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate EVROverlayError _GetOverlayTextureColorSpace(ulong ulOverlayHandle, ref EColorSpace peTextureColorSpace);

		// Token: 0x020001A9 RID: 425
		// (Invoke) Token: 0x06000558 RID: 1368
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate EVROverlayError _SetOverlayTextureBounds(ulong ulOverlayHandle, ref VRTextureBounds_t pOverlayTextureBounds);

		// Token: 0x020001AA RID: 426
		// (Invoke) Token: 0x0600055C RID: 1372
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate EVROverlayError _GetOverlayTextureBounds(ulong ulOverlayHandle, ref VRTextureBounds_t pOverlayTextureBounds);

		// Token: 0x020001AB RID: 427
		// (Invoke) Token: 0x06000560 RID: 1376
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate uint _GetOverlayRenderModel(ulong ulOverlayHandle, StringBuilder pchValue, uint unBufferSize, ref HmdColor_t pColor, ref EVROverlayError pError);

		// Token: 0x020001AC RID: 428
		// (Invoke) Token: 0x06000564 RID: 1380
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate EVROverlayError _SetOverlayRenderModel(ulong ulOverlayHandle, string pchRenderModel, ref HmdColor_t pColor);

		// Token: 0x020001AD RID: 429
		// (Invoke) Token: 0x06000568 RID: 1384
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate EVROverlayError _GetOverlayTransformType(ulong ulOverlayHandle, ref VROverlayTransformType peTransformType);

		// Token: 0x020001AE RID: 430
		// (Invoke) Token: 0x0600056C RID: 1388
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate EVROverlayError _SetOverlayTransformAbsolute(ulong ulOverlayHandle, ETrackingUniverseOrigin eTrackingOrigin, ref HmdMatrix34_t pmatTrackingOriginToOverlayTransform);

		// Token: 0x020001AF RID: 431
		// (Invoke) Token: 0x06000570 RID: 1392
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate EVROverlayError _GetOverlayTransformAbsolute(ulong ulOverlayHandle, ref ETrackingUniverseOrigin peTrackingOrigin, ref HmdMatrix34_t pmatTrackingOriginToOverlayTransform);

		// Token: 0x020001B0 RID: 432
		// (Invoke) Token: 0x06000574 RID: 1396
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate EVROverlayError _SetOverlayTransformTrackedDeviceRelative(ulong ulOverlayHandle, uint unTrackedDevice, ref HmdMatrix34_t pmatTrackedDeviceToOverlayTransform);

		// Token: 0x020001B1 RID: 433
		// (Invoke) Token: 0x06000578 RID: 1400
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate EVROverlayError _GetOverlayTransformTrackedDeviceRelative(ulong ulOverlayHandle, ref uint punTrackedDevice, ref HmdMatrix34_t pmatTrackedDeviceToOverlayTransform);

		// Token: 0x020001B2 RID: 434
		// (Invoke) Token: 0x0600057C RID: 1404
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate EVROverlayError _SetOverlayTransformTrackedDeviceComponent(ulong ulOverlayHandle, uint unDeviceIndex, string pchComponentName);

		// Token: 0x020001B3 RID: 435
		// (Invoke) Token: 0x06000580 RID: 1408
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate EVROverlayError _GetOverlayTransformTrackedDeviceComponent(ulong ulOverlayHandle, ref uint punDeviceIndex, StringBuilder pchComponentName, uint unComponentNameSize);

		// Token: 0x020001B4 RID: 436
		// (Invoke) Token: 0x06000584 RID: 1412
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate EVROverlayError _GetOverlayTransformOverlayRelative(ulong ulOverlayHandle, ref ulong ulOverlayHandleParent, ref HmdMatrix34_t pmatParentOverlayToOverlayTransform);

		// Token: 0x020001B5 RID: 437
		// (Invoke) Token: 0x06000588 RID: 1416
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate EVROverlayError _SetOverlayTransformOverlayRelative(ulong ulOverlayHandle, ulong ulOverlayHandleParent, ref HmdMatrix34_t pmatParentOverlayToOverlayTransform);

		// Token: 0x020001B6 RID: 438
		// (Invoke) Token: 0x0600058C RID: 1420
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate EVROverlayError _ShowOverlay(ulong ulOverlayHandle);

		// Token: 0x020001B7 RID: 439
		// (Invoke) Token: 0x06000590 RID: 1424
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate EVROverlayError _HideOverlay(ulong ulOverlayHandle);

		// Token: 0x020001B8 RID: 440
		// (Invoke) Token: 0x06000594 RID: 1428
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate bool _IsOverlayVisible(ulong ulOverlayHandle);

		// Token: 0x020001B9 RID: 441
		// (Invoke) Token: 0x06000598 RID: 1432
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate EVROverlayError _GetTransformForOverlayCoordinates(ulong ulOverlayHandle, ETrackingUniverseOrigin eTrackingOrigin, HmdVector2_t coordinatesInOverlay, ref HmdMatrix34_t pmatTransform);

		// Token: 0x020001BA RID: 442
		// (Invoke) Token: 0x0600059C RID: 1436
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate bool _PollNextOverlayEvent(ulong ulOverlayHandle, ref VREvent_t pEvent, uint uncbVREvent);

		// Token: 0x020001BB RID: 443
		// (Invoke) Token: 0x060005A0 RID: 1440
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate EVROverlayError _GetOverlayInputMethod(ulong ulOverlayHandle, ref VROverlayInputMethod peInputMethod);

		// Token: 0x020001BC RID: 444
		// (Invoke) Token: 0x060005A4 RID: 1444
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate EVROverlayError _SetOverlayInputMethod(ulong ulOverlayHandle, VROverlayInputMethod eInputMethod);

		// Token: 0x020001BD RID: 445
		// (Invoke) Token: 0x060005A8 RID: 1448
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate EVROverlayError _GetOverlayMouseScale(ulong ulOverlayHandle, ref HmdVector2_t pvecMouseScale);

		// Token: 0x020001BE RID: 446
		// (Invoke) Token: 0x060005AC RID: 1452
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate EVROverlayError _SetOverlayMouseScale(ulong ulOverlayHandle, ref HmdVector2_t pvecMouseScale);

		// Token: 0x020001BF RID: 447
		// (Invoke) Token: 0x060005B0 RID: 1456
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate bool _ComputeOverlayIntersection(ulong ulOverlayHandle, ref VROverlayIntersectionParams_t pParams, ref VROverlayIntersectionResults_t pResults);

		// Token: 0x020001C0 RID: 448
		// (Invoke) Token: 0x060005B4 RID: 1460
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate bool _IsHoverTargetOverlay(ulong ulOverlayHandle);

		// Token: 0x020001C1 RID: 449
		// (Invoke) Token: 0x060005B8 RID: 1464
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate ulong _GetGamepadFocusOverlay();

		// Token: 0x020001C2 RID: 450
		// (Invoke) Token: 0x060005BC RID: 1468
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate EVROverlayError _SetGamepadFocusOverlay(ulong ulNewFocusOverlay);

		// Token: 0x020001C3 RID: 451
		// (Invoke) Token: 0x060005C0 RID: 1472
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate EVROverlayError _SetOverlayNeighbor(EOverlayDirection eDirection, ulong ulFrom, ulong ulTo);

		// Token: 0x020001C4 RID: 452
		// (Invoke) Token: 0x060005C4 RID: 1476
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate EVROverlayError _MoveGamepadFocusToNeighbor(EOverlayDirection eDirection, ulong ulFrom);

		// Token: 0x020001C5 RID: 453
		// (Invoke) Token: 0x060005C8 RID: 1480
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate EVROverlayError _SetOverlayDualAnalogTransform(ulong ulOverlay, EDualAnalogWhich eWhich, ref HmdVector2_t pvCenter, float fRadius);

		// Token: 0x020001C6 RID: 454
		// (Invoke) Token: 0x060005CC RID: 1484
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate EVROverlayError _GetOverlayDualAnalogTransform(ulong ulOverlay, EDualAnalogWhich eWhich, ref HmdVector2_t pvCenter, ref float pfRadius);

		// Token: 0x020001C7 RID: 455
		// (Invoke) Token: 0x060005D0 RID: 1488
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate EVROverlayError _SetOverlayTexture(ulong ulOverlayHandle, ref Texture_t pTexture);

		// Token: 0x020001C8 RID: 456
		// (Invoke) Token: 0x060005D4 RID: 1492
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate EVROverlayError _ClearOverlayTexture(ulong ulOverlayHandle);

		// Token: 0x020001C9 RID: 457
		// (Invoke) Token: 0x060005D8 RID: 1496
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate EVROverlayError _SetOverlayRaw(ulong ulOverlayHandle, IntPtr pvBuffer, uint unWidth, uint unHeight, uint unDepth);

		// Token: 0x020001CA RID: 458
		// (Invoke) Token: 0x060005DC RID: 1500
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate EVROverlayError _SetOverlayFromFile(ulong ulOverlayHandle, string pchFilePath);

		// Token: 0x020001CB RID: 459
		// (Invoke) Token: 0x060005E0 RID: 1504
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate EVROverlayError _GetOverlayTexture(ulong ulOverlayHandle, ref IntPtr pNativeTextureHandle, IntPtr pNativeTextureRef, ref uint pWidth, ref uint pHeight, ref uint pNativeFormat, ref ETextureType pAPIType, ref EColorSpace pColorSpace, ref VRTextureBounds_t pTextureBounds);

		// Token: 0x020001CC RID: 460
		// (Invoke) Token: 0x060005E4 RID: 1508
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate EVROverlayError _ReleaseNativeOverlayHandle(ulong ulOverlayHandle, IntPtr pNativeTextureHandle);

		// Token: 0x020001CD RID: 461
		// (Invoke) Token: 0x060005E8 RID: 1512
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate EVROverlayError _GetOverlayTextureSize(ulong ulOverlayHandle, ref uint pWidth, ref uint pHeight);

		// Token: 0x020001CE RID: 462
		// (Invoke) Token: 0x060005EC RID: 1516
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate EVROverlayError _CreateDashboardOverlay(string pchOverlayKey, string pchOverlayFriendlyName, ref ulong pMainHandle, ref ulong pThumbnailHandle);

		// Token: 0x020001CF RID: 463
		// (Invoke) Token: 0x060005F0 RID: 1520
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate bool _IsDashboardVisible();

		// Token: 0x020001D0 RID: 464
		// (Invoke) Token: 0x060005F4 RID: 1524
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate bool _IsActiveDashboardOverlay(ulong ulOverlayHandle);

		// Token: 0x020001D1 RID: 465
		// (Invoke) Token: 0x060005F8 RID: 1528
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate EVROverlayError _SetDashboardOverlaySceneProcess(ulong ulOverlayHandle, uint unProcessId);

		// Token: 0x020001D2 RID: 466
		// (Invoke) Token: 0x060005FC RID: 1532
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate EVROverlayError _GetDashboardOverlaySceneProcess(ulong ulOverlayHandle, ref uint punProcessId);

		// Token: 0x020001D3 RID: 467
		// (Invoke) Token: 0x06000600 RID: 1536
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate void _ShowDashboard(string pchOverlayToShow);

		// Token: 0x020001D4 RID: 468
		// (Invoke) Token: 0x06000604 RID: 1540
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate uint _GetPrimaryDashboardDevice();

		// Token: 0x020001D5 RID: 469
		// (Invoke) Token: 0x06000608 RID: 1544
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate EVROverlayError _ShowKeyboard(int eInputMode, int eLineInputMode, string pchDescription, uint unCharMax, string pchExistingText, bool bUseMinimalMode, ulong uUserValue);

		// Token: 0x020001D6 RID: 470
		// (Invoke) Token: 0x0600060C RID: 1548
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate EVROverlayError _ShowKeyboardForOverlay(ulong ulOverlayHandle, int eInputMode, int eLineInputMode, string pchDescription, uint unCharMax, string pchExistingText, bool bUseMinimalMode, ulong uUserValue);

		// Token: 0x020001D7 RID: 471
		// (Invoke) Token: 0x06000610 RID: 1552
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate uint _GetKeyboardText(StringBuilder pchText, uint cchText);

		// Token: 0x020001D8 RID: 472
		// (Invoke) Token: 0x06000614 RID: 1556
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate void _HideKeyboard();

		// Token: 0x020001D9 RID: 473
		// (Invoke) Token: 0x06000618 RID: 1560
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate void _SetKeyboardTransformAbsolute(ETrackingUniverseOrigin eTrackingOrigin, ref HmdMatrix34_t pmatTrackingOriginToKeyboardTransform);

		// Token: 0x020001DA RID: 474
		// (Invoke) Token: 0x0600061C RID: 1564
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate void _SetKeyboardPositionForOverlay(ulong ulOverlayHandle, HmdRect2_t avoidRect);

		// Token: 0x020001DB RID: 475
		// (Invoke) Token: 0x06000620 RID: 1568
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate EVROverlayError _SetOverlayIntersectionMask(ulong ulOverlayHandle, ref VROverlayIntersectionMaskPrimitive_t pMaskPrimitives, uint unNumMaskPrimitives, uint unPrimitiveSize);

		// Token: 0x020001DC RID: 476
		// (Invoke) Token: 0x06000624 RID: 1572
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate EVROverlayError _GetOverlayFlags(ulong ulOverlayHandle, ref uint pFlags);

		// Token: 0x020001DD RID: 477
		// (Invoke) Token: 0x06000628 RID: 1576
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate VRMessageOverlayResponse _ShowMessageOverlay(string pchText, string pchCaption, string pchButton0Text, string pchButton1Text, string pchButton2Text, string pchButton3Text);

		// Token: 0x020001DE RID: 478
		// (Invoke) Token: 0x0600062C RID: 1580
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate void _CloseMessageOverlay();
	}
}
