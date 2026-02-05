using System;
using System.Runtime.InteropServices;
using System.Text;

namespace Valve.VR
{
	// Token: 0x0200001A RID: 26
	public class CVROverlay
	{
		// Token: 0x060000AC RID: 172 RVA: 0x00003096 File Offset: 0x00001296
		internal CVROverlay(IntPtr pInterface)
		{
			this.FnTable = (IVROverlay)Marshal.PtrToStructure(pInterface, typeof(IVROverlay));
		}

		// Token: 0x060000AD RID: 173 RVA: 0x000030B9 File Offset: 0x000012B9
		public EVROverlayError FindOverlay(string pchOverlayKey, ref ulong pOverlayHandle)
		{
			pOverlayHandle = 0UL;
			return this.FnTable.FindOverlay(pchOverlayKey, ref pOverlayHandle);
		}

		// Token: 0x060000AE RID: 174 RVA: 0x000030D1 File Offset: 0x000012D1
		public EVROverlayError CreateOverlay(string pchOverlayKey, string pchOverlayName, ref ulong pOverlayHandle)
		{
			pOverlayHandle = 0UL;
			return this.FnTable.CreateOverlay(pchOverlayKey, pchOverlayName, ref pOverlayHandle);
		}

		// Token: 0x060000AF RID: 175 RVA: 0x000030EA File Offset: 0x000012EA
		public EVROverlayError DestroyOverlay(ulong ulOverlayHandle)
		{
			return this.FnTable.DestroyOverlay(ulOverlayHandle);
		}

		// Token: 0x060000B0 RID: 176 RVA: 0x000030FD File Offset: 0x000012FD
		public EVROverlayError SetHighQualityOverlay(ulong ulOverlayHandle)
		{
			return this.FnTable.SetHighQualityOverlay(ulOverlayHandle);
		}

		// Token: 0x060000B1 RID: 177 RVA: 0x00003110 File Offset: 0x00001310
		public ulong GetHighQualityOverlay()
		{
			return this.FnTable.GetHighQualityOverlay();
		}

		// Token: 0x060000B2 RID: 178 RVA: 0x00003122 File Offset: 0x00001322
		public uint GetOverlayKey(ulong ulOverlayHandle, StringBuilder pchValue, uint unBufferSize, ref EVROverlayError pError)
		{
			return this.FnTable.GetOverlayKey(ulOverlayHandle, pchValue, unBufferSize, ref pError);
		}

		// Token: 0x060000B3 RID: 179 RVA: 0x00003139 File Offset: 0x00001339
		public uint GetOverlayName(ulong ulOverlayHandle, StringBuilder pchValue, uint unBufferSize, ref EVROverlayError pError)
		{
			return this.FnTable.GetOverlayName(ulOverlayHandle, pchValue, unBufferSize, ref pError);
		}

		// Token: 0x060000B4 RID: 180 RVA: 0x00003150 File Offset: 0x00001350
		public EVROverlayError SetOverlayName(ulong ulOverlayHandle, string pchName)
		{
			return this.FnTable.SetOverlayName(ulOverlayHandle, pchName);
		}

		// Token: 0x060000B5 RID: 181 RVA: 0x00003164 File Offset: 0x00001364
		public EVROverlayError GetOverlayImageData(ulong ulOverlayHandle, IntPtr pvBuffer, uint unBufferSize, ref uint punWidth, ref uint punHeight)
		{
			punWidth = 0U;
			punHeight = 0U;
			return this.FnTable.GetOverlayImageData(ulOverlayHandle, pvBuffer, unBufferSize, ref punWidth, ref punHeight);
		}

		// Token: 0x060000B6 RID: 182 RVA: 0x00003185 File Offset: 0x00001385
		public string GetOverlayErrorNameFromEnum(EVROverlayError error)
		{
			return Marshal.PtrToStringAnsi(this.FnTable.GetOverlayErrorNameFromEnum(error));
		}

		// Token: 0x060000B7 RID: 183 RVA: 0x0000319D File Offset: 0x0000139D
		public EVROverlayError SetOverlayRenderingPid(ulong ulOverlayHandle, uint unPID)
		{
			return this.FnTable.SetOverlayRenderingPid(ulOverlayHandle, unPID);
		}

		// Token: 0x060000B8 RID: 184 RVA: 0x000031B1 File Offset: 0x000013B1
		public uint GetOverlayRenderingPid(ulong ulOverlayHandle)
		{
			return this.FnTable.GetOverlayRenderingPid(ulOverlayHandle);
		}

		// Token: 0x060000B9 RID: 185 RVA: 0x000031C4 File Offset: 0x000013C4
		public EVROverlayError SetOverlayFlag(ulong ulOverlayHandle, VROverlayFlags eOverlayFlag, bool bEnabled)
		{
			return this.FnTable.SetOverlayFlag(ulOverlayHandle, eOverlayFlag, bEnabled);
		}

		// Token: 0x060000BA RID: 186 RVA: 0x000031D9 File Offset: 0x000013D9
		public EVROverlayError GetOverlayFlag(ulong ulOverlayHandle, VROverlayFlags eOverlayFlag, ref bool pbEnabled)
		{
			pbEnabled = false;
			return this.FnTable.GetOverlayFlag(ulOverlayHandle, eOverlayFlag, ref pbEnabled);
		}

		// Token: 0x060000BB RID: 187 RVA: 0x000031F1 File Offset: 0x000013F1
		public EVROverlayError SetOverlayColor(ulong ulOverlayHandle, float fRed, float fGreen, float fBlue)
		{
			return this.FnTable.SetOverlayColor(ulOverlayHandle, fRed, fGreen, fBlue);
		}

		// Token: 0x060000BC RID: 188 RVA: 0x00003208 File Offset: 0x00001408
		public EVROverlayError GetOverlayColor(ulong ulOverlayHandle, ref float pfRed, ref float pfGreen, ref float pfBlue)
		{
			pfRed = 0f;
			pfGreen = 0f;
			pfBlue = 0f;
			return this.FnTable.GetOverlayColor(ulOverlayHandle, ref pfRed, ref pfGreen, ref pfBlue);
		}

		// Token: 0x060000BD RID: 189 RVA: 0x00003235 File Offset: 0x00001435
		public EVROverlayError SetOverlayAlpha(ulong ulOverlayHandle, float fAlpha)
		{
			return this.FnTable.SetOverlayAlpha(ulOverlayHandle, fAlpha);
		}

		// Token: 0x060000BE RID: 190 RVA: 0x00003249 File Offset: 0x00001449
		public EVROverlayError GetOverlayAlpha(ulong ulOverlayHandle, ref float pfAlpha)
		{
			pfAlpha = 0f;
			return this.FnTable.GetOverlayAlpha(ulOverlayHandle, ref pfAlpha);
		}

		// Token: 0x060000BF RID: 191 RVA: 0x00003264 File Offset: 0x00001464
		public EVROverlayError SetOverlayTexelAspect(ulong ulOverlayHandle, float fTexelAspect)
		{
			return this.FnTable.SetOverlayTexelAspect(ulOverlayHandle, fTexelAspect);
		}

		// Token: 0x060000C0 RID: 192 RVA: 0x00003278 File Offset: 0x00001478
		public EVROverlayError GetOverlayTexelAspect(ulong ulOverlayHandle, ref float pfTexelAspect)
		{
			pfTexelAspect = 0f;
			return this.FnTable.GetOverlayTexelAspect(ulOverlayHandle, ref pfTexelAspect);
		}

		// Token: 0x060000C1 RID: 193 RVA: 0x00003293 File Offset: 0x00001493
		public EVROverlayError SetOverlaySortOrder(ulong ulOverlayHandle, uint unSortOrder)
		{
			return this.FnTable.SetOverlaySortOrder(ulOverlayHandle, unSortOrder);
		}

		// Token: 0x060000C2 RID: 194 RVA: 0x000032A7 File Offset: 0x000014A7
		public EVROverlayError GetOverlaySortOrder(ulong ulOverlayHandle, ref uint punSortOrder)
		{
			punSortOrder = 0U;
			return this.FnTable.GetOverlaySortOrder(ulOverlayHandle, ref punSortOrder);
		}

		// Token: 0x060000C3 RID: 195 RVA: 0x000032BE File Offset: 0x000014BE
		public EVROverlayError SetOverlayWidthInMeters(ulong ulOverlayHandle, float fWidthInMeters)
		{
			return this.FnTable.SetOverlayWidthInMeters(ulOverlayHandle, fWidthInMeters);
		}

		// Token: 0x060000C4 RID: 196 RVA: 0x000032D2 File Offset: 0x000014D2
		public EVROverlayError GetOverlayWidthInMeters(ulong ulOverlayHandle, ref float pfWidthInMeters)
		{
			pfWidthInMeters = 0f;
			return this.FnTable.GetOverlayWidthInMeters(ulOverlayHandle, ref pfWidthInMeters);
		}

		// Token: 0x060000C5 RID: 197 RVA: 0x000032ED File Offset: 0x000014ED
		public EVROverlayError SetOverlayAutoCurveDistanceRangeInMeters(ulong ulOverlayHandle, float fMinDistanceInMeters, float fMaxDistanceInMeters)
		{
			return this.FnTable.SetOverlayAutoCurveDistanceRangeInMeters(ulOverlayHandle, fMinDistanceInMeters, fMaxDistanceInMeters);
		}

		// Token: 0x060000C6 RID: 198 RVA: 0x00003302 File Offset: 0x00001502
		public EVROverlayError GetOverlayAutoCurveDistanceRangeInMeters(ulong ulOverlayHandle, ref float pfMinDistanceInMeters, ref float pfMaxDistanceInMeters)
		{
			pfMinDistanceInMeters = 0f;
			pfMaxDistanceInMeters = 0f;
			return this.FnTable.GetOverlayAutoCurveDistanceRangeInMeters(ulOverlayHandle, ref pfMinDistanceInMeters, ref pfMaxDistanceInMeters);
		}

		// Token: 0x060000C7 RID: 199 RVA: 0x00003325 File Offset: 0x00001525
		public EVROverlayError SetOverlayTextureColorSpace(ulong ulOverlayHandle, EColorSpace eTextureColorSpace)
		{
			return this.FnTable.SetOverlayTextureColorSpace(ulOverlayHandle, eTextureColorSpace);
		}

		// Token: 0x060000C8 RID: 200 RVA: 0x00003339 File Offset: 0x00001539
		public EVROverlayError GetOverlayTextureColorSpace(ulong ulOverlayHandle, ref EColorSpace peTextureColorSpace)
		{
			return this.FnTable.GetOverlayTextureColorSpace(ulOverlayHandle, ref peTextureColorSpace);
		}

		// Token: 0x060000C9 RID: 201 RVA: 0x0000334D File Offset: 0x0000154D
		public EVROverlayError SetOverlayTextureBounds(ulong ulOverlayHandle, ref VRTextureBounds_t pOverlayTextureBounds)
		{
			return this.FnTable.SetOverlayTextureBounds(ulOverlayHandle, ref pOverlayTextureBounds);
		}

		// Token: 0x060000CA RID: 202 RVA: 0x00003361 File Offset: 0x00001561
		public EVROverlayError GetOverlayTextureBounds(ulong ulOverlayHandle, ref VRTextureBounds_t pOverlayTextureBounds)
		{
			return this.FnTable.GetOverlayTextureBounds(ulOverlayHandle, ref pOverlayTextureBounds);
		}

		// Token: 0x060000CB RID: 203 RVA: 0x00003375 File Offset: 0x00001575
		public uint GetOverlayRenderModel(ulong ulOverlayHandle, StringBuilder pchValue, uint unBufferSize, ref HmdColor_t pColor, ref EVROverlayError pError)
		{
			return this.FnTable.GetOverlayRenderModel(ulOverlayHandle, pchValue, unBufferSize, ref pColor, ref pError);
		}

		// Token: 0x060000CC RID: 204 RVA: 0x0000338E File Offset: 0x0000158E
		public EVROverlayError SetOverlayRenderModel(ulong ulOverlayHandle, string pchRenderModel, ref HmdColor_t pColor)
		{
			return this.FnTable.SetOverlayRenderModel(ulOverlayHandle, pchRenderModel, ref pColor);
		}

		// Token: 0x060000CD RID: 205 RVA: 0x000033A3 File Offset: 0x000015A3
		public EVROverlayError GetOverlayTransformType(ulong ulOverlayHandle, ref VROverlayTransformType peTransformType)
		{
			return this.FnTable.GetOverlayTransformType(ulOverlayHandle, ref peTransformType);
		}

		// Token: 0x060000CE RID: 206 RVA: 0x000033B7 File Offset: 0x000015B7
		public EVROverlayError SetOverlayTransformAbsolute(ulong ulOverlayHandle, ETrackingUniverseOrigin eTrackingOrigin, ref HmdMatrix34_t pmatTrackingOriginToOverlayTransform)
		{
			return this.FnTable.SetOverlayTransformAbsolute(ulOverlayHandle, eTrackingOrigin, ref pmatTrackingOriginToOverlayTransform);
		}

		// Token: 0x060000CF RID: 207 RVA: 0x000033CC File Offset: 0x000015CC
		public EVROverlayError GetOverlayTransformAbsolute(ulong ulOverlayHandle, ref ETrackingUniverseOrigin peTrackingOrigin, ref HmdMatrix34_t pmatTrackingOriginToOverlayTransform)
		{
			return this.FnTable.GetOverlayTransformAbsolute(ulOverlayHandle, ref peTrackingOrigin, ref pmatTrackingOriginToOverlayTransform);
		}

		// Token: 0x060000D0 RID: 208 RVA: 0x000033E1 File Offset: 0x000015E1
		public EVROverlayError SetOverlayTransformTrackedDeviceRelative(ulong ulOverlayHandle, uint unTrackedDevice, ref HmdMatrix34_t pmatTrackedDeviceToOverlayTransform)
		{
			return this.FnTable.SetOverlayTransformTrackedDeviceRelative(ulOverlayHandle, unTrackedDevice, ref pmatTrackedDeviceToOverlayTransform);
		}

		// Token: 0x060000D1 RID: 209 RVA: 0x000033F6 File Offset: 0x000015F6
		public EVROverlayError GetOverlayTransformTrackedDeviceRelative(ulong ulOverlayHandle, ref uint punTrackedDevice, ref HmdMatrix34_t pmatTrackedDeviceToOverlayTransform)
		{
			punTrackedDevice = 0U;
			return this.FnTable.GetOverlayTransformTrackedDeviceRelative(ulOverlayHandle, ref punTrackedDevice, ref pmatTrackedDeviceToOverlayTransform);
		}

		// Token: 0x060000D2 RID: 210 RVA: 0x0000340E File Offset: 0x0000160E
		public EVROverlayError SetOverlayTransformTrackedDeviceComponent(ulong ulOverlayHandle, uint unDeviceIndex, string pchComponentName)
		{
			return this.FnTable.SetOverlayTransformTrackedDeviceComponent(ulOverlayHandle, unDeviceIndex, pchComponentName);
		}

		// Token: 0x060000D3 RID: 211 RVA: 0x00003423 File Offset: 0x00001623
		public EVROverlayError GetOverlayTransformTrackedDeviceComponent(ulong ulOverlayHandle, ref uint punDeviceIndex, StringBuilder pchComponentName, uint unComponentNameSize)
		{
			punDeviceIndex = 0U;
			return this.FnTable.GetOverlayTransformTrackedDeviceComponent(ulOverlayHandle, ref punDeviceIndex, pchComponentName, unComponentNameSize);
		}

		// Token: 0x060000D4 RID: 212 RVA: 0x0000343D File Offset: 0x0000163D
		public EVROverlayError GetOverlayTransformOverlayRelative(ulong ulOverlayHandle, ref ulong ulOverlayHandleParent, ref HmdMatrix34_t pmatParentOverlayToOverlayTransform)
		{
			ulOverlayHandleParent = 0UL;
			return this.FnTable.GetOverlayTransformOverlayRelative(ulOverlayHandle, ref ulOverlayHandleParent, ref pmatParentOverlayToOverlayTransform);
		}

		// Token: 0x060000D5 RID: 213 RVA: 0x00003456 File Offset: 0x00001656
		public EVROverlayError SetOverlayTransformOverlayRelative(ulong ulOverlayHandle, ulong ulOverlayHandleParent, ref HmdMatrix34_t pmatParentOverlayToOverlayTransform)
		{
			return this.FnTable.SetOverlayTransformOverlayRelative(ulOverlayHandle, ulOverlayHandleParent, ref pmatParentOverlayToOverlayTransform);
		}

		// Token: 0x060000D6 RID: 214 RVA: 0x0000346B File Offset: 0x0000166B
		public EVROverlayError ShowOverlay(ulong ulOverlayHandle)
		{
			return this.FnTable.ShowOverlay(ulOverlayHandle);
		}

		// Token: 0x060000D7 RID: 215 RVA: 0x0000347E File Offset: 0x0000167E
		public EVROverlayError HideOverlay(ulong ulOverlayHandle)
		{
			return this.FnTable.HideOverlay(ulOverlayHandle);
		}

		// Token: 0x060000D8 RID: 216 RVA: 0x00003491 File Offset: 0x00001691
		public bool IsOverlayVisible(ulong ulOverlayHandle)
		{
			return this.FnTable.IsOverlayVisible(ulOverlayHandle);
		}

		// Token: 0x060000D9 RID: 217 RVA: 0x000034A4 File Offset: 0x000016A4
		public EVROverlayError GetTransformForOverlayCoordinates(ulong ulOverlayHandle, ETrackingUniverseOrigin eTrackingOrigin, HmdVector2_t coordinatesInOverlay, ref HmdMatrix34_t pmatTransform)
		{
			return this.FnTable.GetTransformForOverlayCoordinates(ulOverlayHandle, eTrackingOrigin, coordinatesInOverlay, ref pmatTransform);
		}

		// Token: 0x060000DA RID: 218 RVA: 0x000034BC File Offset: 0x000016BC
		public bool PollNextOverlayEvent(ulong ulOverlayHandle, ref VREvent_t pEvent, uint uncbVREvent)
		{
			if (Environment.OSVersion.Platform == PlatformID.MacOSX || Environment.OSVersion.Platform == PlatformID.Unix)
			{
				VREvent_t_Packed vrevent_t_Packed = default(VREvent_t_Packed);
				CVROverlay.PollNextOverlayEventUnion pollNextOverlayEventUnion;
				pollNextOverlayEventUnion.pPollNextOverlayEventPacked = null;
				pollNextOverlayEventUnion.pPollNextOverlayEvent = this.FnTable.PollNextOverlayEvent;
				bool flag = pollNextOverlayEventUnion.pPollNextOverlayEventPacked(ulOverlayHandle, ref vrevent_t_Packed, (uint)Marshal.SizeOf(typeof(VREvent_t_Packed)));
				vrevent_t_Packed.Unpack(ref pEvent);
				return flag;
			}
			return this.FnTable.PollNextOverlayEvent(ulOverlayHandle, ref pEvent, uncbVREvent);
		}

		// Token: 0x060000DB RID: 219 RVA: 0x0000353E File Offset: 0x0000173E
		public EVROverlayError GetOverlayInputMethod(ulong ulOverlayHandle, ref VROverlayInputMethod peInputMethod)
		{
			return this.FnTable.GetOverlayInputMethod(ulOverlayHandle, ref peInputMethod);
		}

		// Token: 0x060000DC RID: 220 RVA: 0x00003552 File Offset: 0x00001752
		public EVROverlayError SetOverlayInputMethod(ulong ulOverlayHandle, VROverlayInputMethod eInputMethod)
		{
			return this.FnTable.SetOverlayInputMethod(ulOverlayHandle, eInputMethod);
		}

		// Token: 0x060000DD RID: 221 RVA: 0x00003566 File Offset: 0x00001766
		public EVROverlayError GetOverlayMouseScale(ulong ulOverlayHandle, ref HmdVector2_t pvecMouseScale)
		{
			return this.FnTable.GetOverlayMouseScale(ulOverlayHandle, ref pvecMouseScale);
		}

		// Token: 0x060000DE RID: 222 RVA: 0x0000357A File Offset: 0x0000177A
		public EVROverlayError SetOverlayMouseScale(ulong ulOverlayHandle, ref HmdVector2_t pvecMouseScale)
		{
			return this.FnTable.SetOverlayMouseScale(ulOverlayHandle, ref pvecMouseScale);
		}

		// Token: 0x060000DF RID: 223 RVA: 0x0000358E File Offset: 0x0000178E
		public bool ComputeOverlayIntersection(ulong ulOverlayHandle, ref VROverlayIntersectionParams_t pParams, ref VROverlayIntersectionResults_t pResults)
		{
			return this.FnTable.ComputeOverlayIntersection(ulOverlayHandle, ref pParams, ref pResults);
		}

		// Token: 0x060000E0 RID: 224 RVA: 0x000035A3 File Offset: 0x000017A3
		public bool IsHoverTargetOverlay(ulong ulOverlayHandle)
		{
			return this.FnTable.IsHoverTargetOverlay(ulOverlayHandle);
		}

		// Token: 0x060000E1 RID: 225 RVA: 0x000035B6 File Offset: 0x000017B6
		public ulong GetGamepadFocusOverlay()
		{
			return this.FnTable.GetGamepadFocusOverlay();
		}

		// Token: 0x060000E2 RID: 226 RVA: 0x000035C8 File Offset: 0x000017C8
		public EVROverlayError SetGamepadFocusOverlay(ulong ulNewFocusOverlay)
		{
			return this.FnTable.SetGamepadFocusOverlay(ulNewFocusOverlay);
		}

		// Token: 0x060000E3 RID: 227 RVA: 0x000035DB File Offset: 0x000017DB
		public EVROverlayError SetOverlayNeighbor(EOverlayDirection eDirection, ulong ulFrom, ulong ulTo)
		{
			return this.FnTable.SetOverlayNeighbor(eDirection, ulFrom, ulTo);
		}

		// Token: 0x060000E4 RID: 228 RVA: 0x000035F0 File Offset: 0x000017F0
		public EVROverlayError MoveGamepadFocusToNeighbor(EOverlayDirection eDirection, ulong ulFrom)
		{
			return this.FnTable.MoveGamepadFocusToNeighbor(eDirection, ulFrom);
		}

		// Token: 0x060000E5 RID: 229 RVA: 0x00003604 File Offset: 0x00001804
		public EVROverlayError SetOverlayDualAnalogTransform(ulong ulOverlay, EDualAnalogWhich eWhich, ref HmdVector2_t pvCenter, float fRadius)
		{
			return this.FnTable.SetOverlayDualAnalogTransform(ulOverlay, eWhich, ref pvCenter, fRadius);
		}

		// Token: 0x060000E6 RID: 230 RVA: 0x0000361B File Offset: 0x0000181B
		public EVROverlayError GetOverlayDualAnalogTransform(ulong ulOverlay, EDualAnalogWhich eWhich, ref HmdVector2_t pvCenter, ref float pfRadius)
		{
			pfRadius = 0f;
			return this.FnTable.GetOverlayDualAnalogTransform(ulOverlay, eWhich, ref pvCenter, ref pfRadius);
		}

		// Token: 0x060000E7 RID: 231 RVA: 0x0000363A File Offset: 0x0000183A
		public EVROverlayError SetOverlayTexture(ulong ulOverlayHandle, ref Texture_t pTexture)
		{
			return this.FnTable.SetOverlayTexture(ulOverlayHandle, ref pTexture);
		}

		// Token: 0x060000E8 RID: 232 RVA: 0x0000364E File Offset: 0x0000184E
		public EVROverlayError ClearOverlayTexture(ulong ulOverlayHandle)
		{
			return this.FnTable.ClearOverlayTexture(ulOverlayHandle);
		}

		// Token: 0x060000E9 RID: 233 RVA: 0x00003661 File Offset: 0x00001861
		public EVROverlayError SetOverlayRaw(ulong ulOverlayHandle, IntPtr pvBuffer, uint unWidth, uint unHeight, uint unDepth)
		{
			return this.FnTable.SetOverlayRaw(ulOverlayHandle, pvBuffer, unWidth, unHeight, unDepth);
		}

		// Token: 0x060000EA RID: 234 RVA: 0x0000367A File Offset: 0x0000187A
		public EVROverlayError SetOverlayFromFile(ulong ulOverlayHandle, string pchFilePath)
		{
			return this.FnTable.SetOverlayFromFile(ulOverlayHandle, pchFilePath);
		}

		// Token: 0x060000EB RID: 235 RVA: 0x00003690 File Offset: 0x00001890
		public EVROverlayError GetOverlayTexture(ulong ulOverlayHandle, ref IntPtr pNativeTextureHandle, IntPtr pNativeTextureRef, ref uint pWidth, ref uint pHeight, ref uint pNativeFormat, ref ETextureType pAPIType, ref EColorSpace pColorSpace, ref VRTextureBounds_t pTextureBounds)
		{
			pWidth = 0U;
			pHeight = 0U;
			pNativeFormat = 0U;
			return this.FnTable.GetOverlayTexture(ulOverlayHandle, ref pNativeTextureHandle, pNativeTextureRef, ref pWidth, ref pHeight, ref pNativeFormat, ref pAPIType, ref pColorSpace, ref pTextureBounds);
		}

		// Token: 0x060000EC RID: 236 RVA: 0x000036C8 File Offset: 0x000018C8
		public EVROverlayError ReleaseNativeOverlayHandle(ulong ulOverlayHandle, IntPtr pNativeTextureHandle)
		{
			return this.FnTable.ReleaseNativeOverlayHandle(ulOverlayHandle, pNativeTextureHandle);
		}

		// Token: 0x060000ED RID: 237 RVA: 0x000036DC File Offset: 0x000018DC
		public EVROverlayError GetOverlayTextureSize(ulong ulOverlayHandle, ref uint pWidth, ref uint pHeight)
		{
			pWidth = 0U;
			pHeight = 0U;
			return this.FnTable.GetOverlayTextureSize(ulOverlayHandle, ref pWidth, ref pHeight);
		}

		// Token: 0x060000EE RID: 238 RVA: 0x000036F7 File Offset: 0x000018F7
		public EVROverlayError CreateDashboardOverlay(string pchOverlayKey, string pchOverlayFriendlyName, ref ulong pMainHandle, ref ulong pThumbnailHandle)
		{
			pMainHandle = 0UL;
			pThumbnailHandle = 0UL;
			return this.FnTable.CreateDashboardOverlay(pchOverlayKey, pchOverlayFriendlyName, ref pMainHandle, ref pThumbnailHandle);
		}

		// Token: 0x060000EF RID: 239 RVA: 0x00003717 File Offset: 0x00001917
		public bool IsDashboardVisible()
		{
			return this.FnTable.IsDashboardVisible();
		}

		// Token: 0x060000F0 RID: 240 RVA: 0x00003729 File Offset: 0x00001929
		public bool IsActiveDashboardOverlay(ulong ulOverlayHandle)
		{
			return this.FnTable.IsActiveDashboardOverlay(ulOverlayHandle);
		}

		// Token: 0x060000F1 RID: 241 RVA: 0x0000373C File Offset: 0x0000193C
		public EVROverlayError SetDashboardOverlaySceneProcess(ulong ulOverlayHandle, uint unProcessId)
		{
			return this.FnTable.SetDashboardOverlaySceneProcess(ulOverlayHandle, unProcessId);
		}

		// Token: 0x060000F2 RID: 242 RVA: 0x00003750 File Offset: 0x00001950
		public EVROverlayError GetDashboardOverlaySceneProcess(ulong ulOverlayHandle, ref uint punProcessId)
		{
			punProcessId = 0U;
			return this.FnTable.GetDashboardOverlaySceneProcess(ulOverlayHandle, ref punProcessId);
		}

		// Token: 0x060000F3 RID: 243 RVA: 0x00003767 File Offset: 0x00001967
		public void ShowDashboard(string pchOverlayToShow)
		{
			this.FnTable.ShowDashboard(pchOverlayToShow);
		}

		// Token: 0x060000F4 RID: 244 RVA: 0x0000377A File Offset: 0x0000197A
		public uint GetPrimaryDashboardDevice()
		{
			return this.FnTable.GetPrimaryDashboardDevice();
		}

		// Token: 0x060000F5 RID: 245 RVA: 0x0000378C File Offset: 0x0000198C
		public EVROverlayError ShowKeyboard(int eInputMode, int eLineInputMode, string pchDescription, uint unCharMax, string pchExistingText, bool bUseMinimalMode, ulong uUserValue)
		{
			return this.FnTable.ShowKeyboard(eInputMode, eLineInputMode, pchDescription, unCharMax, pchExistingText, bUseMinimalMode, uUserValue);
		}

		// Token: 0x060000F6 RID: 246 RVA: 0x000037AC File Offset: 0x000019AC
		public EVROverlayError ShowKeyboardForOverlay(ulong ulOverlayHandle, int eInputMode, int eLineInputMode, string pchDescription, uint unCharMax, string pchExistingText, bool bUseMinimalMode, ulong uUserValue)
		{
			return this.FnTable.ShowKeyboardForOverlay(ulOverlayHandle, eInputMode, eLineInputMode, pchDescription, unCharMax, pchExistingText, bUseMinimalMode, uUserValue);
		}

		// Token: 0x060000F7 RID: 247 RVA: 0x000037D6 File Offset: 0x000019D6
		public uint GetKeyboardText(StringBuilder pchText, uint cchText)
		{
			return this.FnTable.GetKeyboardText(pchText, cchText);
		}

		// Token: 0x060000F8 RID: 248 RVA: 0x000037EA File Offset: 0x000019EA
		public void HideKeyboard()
		{
			this.FnTable.HideKeyboard();
		}

		// Token: 0x060000F9 RID: 249 RVA: 0x000037FC File Offset: 0x000019FC
		public void SetKeyboardTransformAbsolute(ETrackingUniverseOrigin eTrackingOrigin, ref HmdMatrix34_t pmatTrackingOriginToKeyboardTransform)
		{
			this.FnTable.SetKeyboardTransformAbsolute(eTrackingOrigin, ref pmatTrackingOriginToKeyboardTransform);
		}

		// Token: 0x060000FA RID: 250 RVA: 0x00003810 File Offset: 0x00001A10
		public void SetKeyboardPositionForOverlay(ulong ulOverlayHandle, HmdRect2_t avoidRect)
		{
			this.FnTable.SetKeyboardPositionForOverlay(ulOverlayHandle, avoidRect);
		}

		// Token: 0x060000FB RID: 251 RVA: 0x00003824 File Offset: 0x00001A24
		public EVROverlayError SetOverlayIntersectionMask(ulong ulOverlayHandle, ref VROverlayIntersectionMaskPrimitive_t pMaskPrimitives, uint unNumMaskPrimitives, uint unPrimitiveSize)
		{
			return this.FnTable.SetOverlayIntersectionMask(ulOverlayHandle, ref pMaskPrimitives, unNumMaskPrimitives, unPrimitiveSize);
		}

		// Token: 0x060000FC RID: 252 RVA: 0x0000383B File Offset: 0x00001A3B
		public EVROverlayError GetOverlayFlags(ulong ulOverlayHandle, ref uint pFlags)
		{
			pFlags = 0U;
			return this.FnTable.GetOverlayFlags(ulOverlayHandle, ref pFlags);
		}

		// Token: 0x060000FD RID: 253 RVA: 0x00003852 File Offset: 0x00001A52
		public VRMessageOverlayResponse ShowMessageOverlay(string pchText, string pchCaption, string pchButton0Text, string pchButton1Text, string pchButton2Text, string pchButton3Text)
		{
			return this.FnTable.ShowMessageOverlay(pchText, pchCaption, pchButton0Text, pchButton1Text, pchButton2Text, pchButton3Text);
		}

		// Token: 0x060000FE RID: 254 RVA: 0x0000386D File Offset: 0x00001A6D
		public void CloseMessageOverlay()
		{
			this.FnTable.CloseMessageOverlay();
		}

		// Token: 0x0400014E RID: 334
		private IVROverlay FnTable;

		// Token: 0x02000235 RID: 565
		// (Invoke) Token: 0x0600077C RID: 1916
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate bool _PollNextOverlayEventPacked(ulong ulOverlayHandle, ref VREvent_t_Packed pEvent, uint uncbVREvent);

		// Token: 0x02000236 RID: 566
		[StructLayout(LayoutKind.Explicit)]
		private struct PollNextOverlayEventUnion
		{
			// Token: 0x040008AE RID: 2222
			[FieldOffset(0)]
			public IVROverlay._PollNextOverlayEvent pPollNextOverlayEvent;

			// Token: 0x040008AF RID: 2223
			[FieldOffset(0)]
			public CVROverlay._PollNextOverlayEventPacked pPollNextOverlayEventPacked;
		}
	}
}
