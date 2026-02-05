using System;

namespace Valve.VR
{
	// Token: 0x0200004F RID: 79
	public enum EVRApplicationProperty
	{
		// Token: 0x0400048F RID: 1167
		Name_String,
		// Token: 0x04000490 RID: 1168
		LaunchType_String = 11,
		// Token: 0x04000491 RID: 1169
		WorkingDirectory_String,
		// Token: 0x04000492 RID: 1170
		BinaryPath_String,
		// Token: 0x04000493 RID: 1171
		Arguments_String,
		// Token: 0x04000494 RID: 1172
		URL_String,
		// Token: 0x04000495 RID: 1173
		Description_String = 50,
		// Token: 0x04000496 RID: 1174
		NewsURL_String,
		// Token: 0x04000497 RID: 1175
		ImagePath_String,
		// Token: 0x04000498 RID: 1176
		Source_String,
		// Token: 0x04000499 RID: 1177
		ActionManifestURL_String,
		// Token: 0x0400049A RID: 1178
		IsDashboardOverlay_Bool = 60,
		// Token: 0x0400049B RID: 1179
		IsTemplate_Bool,
		// Token: 0x0400049C RID: 1180
		IsInstanced_Bool,
		// Token: 0x0400049D RID: 1181
		IsInternal_Bool,
		// Token: 0x0400049E RID: 1182
		WantsCompositorPauseInStandby_Bool,
		// Token: 0x0400049F RID: 1183
		LastLaunchTime_Uint64 = 70
	}
}
