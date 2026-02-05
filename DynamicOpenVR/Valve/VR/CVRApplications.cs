using System;
using System.Runtime.InteropServices;
using System.Text;

namespace Valve.VR
{
	// Token: 0x02000016 RID: 22
	public class CVRApplications
	{
		// Token: 0x06000042 RID: 66 RVA: 0x000027B1 File Offset: 0x000009B1
		internal CVRApplications(IntPtr pInterface)
		{
			this.FnTable = (IVRApplications)Marshal.PtrToStructure(pInterface, typeof(IVRApplications));
		}

		// Token: 0x06000043 RID: 67 RVA: 0x000027D4 File Offset: 0x000009D4
		public EVRApplicationError AddApplicationManifest(string pchApplicationManifestFullPath, bool bTemporary)
		{
			return this.FnTable.AddApplicationManifest(pchApplicationManifestFullPath, bTemporary);
		}

		// Token: 0x06000044 RID: 68 RVA: 0x000027E8 File Offset: 0x000009E8
		public EVRApplicationError RemoveApplicationManifest(string pchApplicationManifestFullPath)
		{
			return this.FnTable.RemoveApplicationManifest(pchApplicationManifestFullPath);
		}

		// Token: 0x06000045 RID: 69 RVA: 0x000027FB File Offset: 0x000009FB
		public bool IsApplicationInstalled(string pchAppKey)
		{
			return this.FnTable.IsApplicationInstalled(pchAppKey);
		}

		// Token: 0x06000046 RID: 70 RVA: 0x0000280E File Offset: 0x00000A0E
		public uint GetApplicationCount()
		{
			return this.FnTable.GetApplicationCount();
		}

		// Token: 0x06000047 RID: 71 RVA: 0x00002820 File Offset: 0x00000A20
		public EVRApplicationError GetApplicationKeyByIndex(uint unApplicationIndex, StringBuilder pchAppKeyBuffer, uint unAppKeyBufferLen)
		{
			return this.FnTable.GetApplicationKeyByIndex(unApplicationIndex, pchAppKeyBuffer, unAppKeyBufferLen);
		}

		// Token: 0x06000048 RID: 72 RVA: 0x00002835 File Offset: 0x00000A35
		public EVRApplicationError GetApplicationKeyByProcessId(uint unProcessId, StringBuilder pchAppKeyBuffer, uint unAppKeyBufferLen)
		{
			return this.FnTable.GetApplicationKeyByProcessId(unProcessId, pchAppKeyBuffer, unAppKeyBufferLen);
		}

		// Token: 0x06000049 RID: 73 RVA: 0x0000284A File Offset: 0x00000A4A
		public EVRApplicationError LaunchApplication(string pchAppKey)
		{
			return this.FnTable.LaunchApplication(pchAppKey);
		}

		// Token: 0x0600004A RID: 74 RVA: 0x0000285D File Offset: 0x00000A5D
		public EVRApplicationError LaunchTemplateApplication(string pchTemplateAppKey, string pchNewAppKey, AppOverrideKeys_t[] pKeys)
		{
			return this.FnTable.LaunchTemplateApplication(pchTemplateAppKey, pchNewAppKey, pKeys, (uint)pKeys.Length);
		}

		// Token: 0x0600004B RID: 75 RVA: 0x00002875 File Offset: 0x00000A75
		public EVRApplicationError LaunchApplicationFromMimeType(string pchMimeType, string pchArgs)
		{
			return this.FnTable.LaunchApplicationFromMimeType(pchMimeType, pchArgs);
		}

		// Token: 0x0600004C RID: 76 RVA: 0x00002889 File Offset: 0x00000A89
		public EVRApplicationError LaunchDashboardOverlay(string pchAppKey)
		{
			return this.FnTable.LaunchDashboardOverlay(pchAppKey);
		}

		// Token: 0x0600004D RID: 77 RVA: 0x0000289C File Offset: 0x00000A9C
		public bool CancelApplicationLaunch(string pchAppKey)
		{
			return this.FnTable.CancelApplicationLaunch(pchAppKey);
		}

		// Token: 0x0600004E RID: 78 RVA: 0x000028AF File Offset: 0x00000AAF
		public EVRApplicationError IdentifyApplication(uint unProcessId, string pchAppKey)
		{
			return this.FnTable.IdentifyApplication(unProcessId, pchAppKey);
		}

		// Token: 0x0600004F RID: 79 RVA: 0x000028C3 File Offset: 0x00000AC3
		public uint GetApplicationProcessId(string pchAppKey)
		{
			return this.FnTable.GetApplicationProcessId(pchAppKey);
		}

		// Token: 0x06000050 RID: 80 RVA: 0x000028D6 File Offset: 0x00000AD6
		public string GetApplicationsErrorNameFromEnum(EVRApplicationError error)
		{
			return Marshal.PtrToStringAnsi(this.FnTable.GetApplicationsErrorNameFromEnum(error));
		}

		// Token: 0x06000051 RID: 81 RVA: 0x000028EE File Offset: 0x00000AEE
		public uint GetApplicationPropertyString(string pchAppKey, EVRApplicationProperty eProperty, StringBuilder pchPropertyValueBuffer, uint unPropertyValueBufferLen, ref EVRApplicationError peError)
		{
			return this.FnTable.GetApplicationPropertyString(pchAppKey, eProperty, pchPropertyValueBuffer, unPropertyValueBufferLen, ref peError);
		}

		// Token: 0x06000052 RID: 82 RVA: 0x00002907 File Offset: 0x00000B07
		public bool GetApplicationPropertyBool(string pchAppKey, EVRApplicationProperty eProperty, ref EVRApplicationError peError)
		{
			return this.FnTable.GetApplicationPropertyBool(pchAppKey, eProperty, ref peError);
		}

		// Token: 0x06000053 RID: 83 RVA: 0x0000291C File Offset: 0x00000B1C
		public ulong GetApplicationPropertyUint64(string pchAppKey, EVRApplicationProperty eProperty, ref EVRApplicationError peError)
		{
			return this.FnTable.GetApplicationPropertyUint64(pchAppKey, eProperty, ref peError);
		}

		// Token: 0x06000054 RID: 84 RVA: 0x00002931 File Offset: 0x00000B31
		public EVRApplicationError SetApplicationAutoLaunch(string pchAppKey, bool bAutoLaunch)
		{
			return this.FnTable.SetApplicationAutoLaunch(pchAppKey, bAutoLaunch);
		}

		// Token: 0x06000055 RID: 85 RVA: 0x00002945 File Offset: 0x00000B45
		public bool GetApplicationAutoLaunch(string pchAppKey)
		{
			return this.FnTable.GetApplicationAutoLaunch(pchAppKey);
		}

		// Token: 0x06000056 RID: 86 RVA: 0x00002958 File Offset: 0x00000B58
		public EVRApplicationError SetDefaultApplicationForMimeType(string pchAppKey, string pchMimeType)
		{
			return this.FnTable.SetDefaultApplicationForMimeType(pchAppKey, pchMimeType);
		}

		// Token: 0x06000057 RID: 87 RVA: 0x0000296C File Offset: 0x00000B6C
		public bool GetDefaultApplicationForMimeType(string pchMimeType, StringBuilder pchAppKeyBuffer, uint unAppKeyBufferLen)
		{
			return this.FnTable.GetDefaultApplicationForMimeType(pchMimeType, pchAppKeyBuffer, unAppKeyBufferLen);
		}

		// Token: 0x06000058 RID: 88 RVA: 0x00002981 File Offset: 0x00000B81
		public bool GetApplicationSupportedMimeTypes(string pchAppKey, StringBuilder pchMimeTypesBuffer, uint unMimeTypesBuffer)
		{
			return this.FnTable.GetApplicationSupportedMimeTypes(pchAppKey, pchMimeTypesBuffer, unMimeTypesBuffer);
		}

		// Token: 0x06000059 RID: 89 RVA: 0x00002996 File Offset: 0x00000B96
		public uint GetApplicationsThatSupportMimeType(string pchMimeType, StringBuilder pchAppKeysThatSupportBuffer, uint unAppKeysThatSupportBuffer)
		{
			return this.FnTable.GetApplicationsThatSupportMimeType(pchMimeType, pchAppKeysThatSupportBuffer, unAppKeysThatSupportBuffer);
		}

		// Token: 0x0600005A RID: 90 RVA: 0x000029AB File Offset: 0x00000BAB
		public uint GetApplicationLaunchArguments(uint unHandle, StringBuilder pchArgs, uint unArgs)
		{
			return this.FnTable.GetApplicationLaunchArguments(unHandle, pchArgs, unArgs);
		}

		// Token: 0x0600005B RID: 91 RVA: 0x000029C0 File Offset: 0x00000BC0
		public EVRApplicationError GetStartingApplication(StringBuilder pchAppKeyBuffer, uint unAppKeyBufferLen)
		{
			return this.FnTable.GetStartingApplication(pchAppKeyBuffer, unAppKeyBufferLen);
		}

		// Token: 0x0600005C RID: 92 RVA: 0x000029D4 File Offset: 0x00000BD4
		public EVRApplicationTransitionState GetTransitionState()
		{
			return this.FnTable.GetTransitionState();
		}

		// Token: 0x0600005D RID: 93 RVA: 0x000029E6 File Offset: 0x00000BE6
		public EVRApplicationError PerformApplicationPrelaunchCheck(string pchAppKey)
		{
			return this.FnTable.PerformApplicationPrelaunchCheck(pchAppKey);
		}

		// Token: 0x0600005E RID: 94 RVA: 0x000029F9 File Offset: 0x00000BF9
		public string GetApplicationsTransitionStateNameFromEnum(EVRApplicationTransitionState state)
		{
			return Marshal.PtrToStringAnsi(this.FnTable.GetApplicationsTransitionStateNameFromEnum(state));
		}

		// Token: 0x0600005F RID: 95 RVA: 0x00002A11 File Offset: 0x00000C11
		public bool IsQuitUserPromptRequested()
		{
			return this.FnTable.IsQuitUserPromptRequested();
		}

		// Token: 0x06000060 RID: 96 RVA: 0x00002A23 File Offset: 0x00000C23
		public EVRApplicationError LaunchInternalProcess(string pchBinaryPath, string pchArguments, string pchWorkingDirectory)
		{
			return this.FnTable.LaunchInternalProcess(pchBinaryPath, pchArguments, pchWorkingDirectory);
		}

		// Token: 0x06000061 RID: 97 RVA: 0x00002A38 File Offset: 0x00000C38
		public uint GetCurrentSceneProcessId()
		{
			return this.FnTable.GetCurrentSceneProcessId();
		}

		// Token: 0x0400014A RID: 330
		private IVRApplications FnTable;
	}
}
