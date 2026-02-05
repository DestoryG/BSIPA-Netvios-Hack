using System;
using System.Runtime.InteropServices;
using System.Text;

namespace Valve.VR
{
	// Token: 0x0200001D RID: 29
	public class CVRSettings
	{
		// Token: 0x06000116 RID: 278 RVA: 0x00003AF6 File Offset: 0x00001CF6
		internal CVRSettings(IntPtr pInterface)
		{
			this.FnTable = (IVRSettings)Marshal.PtrToStructure(pInterface, typeof(IVRSettings));
		}

		// Token: 0x06000117 RID: 279 RVA: 0x00003B19 File Offset: 0x00001D19
		public string GetSettingsErrorNameFromEnum(EVRSettingsError eError)
		{
			return Marshal.PtrToStringAnsi(this.FnTable.GetSettingsErrorNameFromEnum(eError));
		}

		// Token: 0x06000118 RID: 280 RVA: 0x00003B31 File Offset: 0x00001D31
		public bool Sync(bool bForce, ref EVRSettingsError peError)
		{
			return this.FnTable.Sync(bForce, ref peError);
		}

		// Token: 0x06000119 RID: 281 RVA: 0x00003B45 File Offset: 0x00001D45
		public void SetBool(string pchSection, string pchSettingsKey, bool bValue, ref EVRSettingsError peError)
		{
			this.FnTable.SetBool(pchSection, pchSettingsKey, bValue, ref peError);
		}

		// Token: 0x0600011A RID: 282 RVA: 0x00003B5C File Offset: 0x00001D5C
		public void SetInt32(string pchSection, string pchSettingsKey, int nValue, ref EVRSettingsError peError)
		{
			this.FnTable.SetInt32(pchSection, pchSettingsKey, nValue, ref peError);
		}

		// Token: 0x0600011B RID: 283 RVA: 0x00003B73 File Offset: 0x00001D73
		public void SetFloat(string pchSection, string pchSettingsKey, float flValue, ref EVRSettingsError peError)
		{
			this.FnTable.SetFloat(pchSection, pchSettingsKey, flValue, ref peError);
		}

		// Token: 0x0600011C RID: 284 RVA: 0x00003B8A File Offset: 0x00001D8A
		public void SetString(string pchSection, string pchSettingsKey, string pchValue, ref EVRSettingsError peError)
		{
			this.FnTable.SetString(pchSection, pchSettingsKey, pchValue, ref peError);
		}

		// Token: 0x0600011D RID: 285 RVA: 0x00003BA1 File Offset: 0x00001DA1
		public bool GetBool(string pchSection, string pchSettingsKey, ref EVRSettingsError peError)
		{
			return this.FnTable.GetBool(pchSection, pchSettingsKey, ref peError);
		}

		// Token: 0x0600011E RID: 286 RVA: 0x00003BB6 File Offset: 0x00001DB6
		public int GetInt32(string pchSection, string pchSettingsKey, ref EVRSettingsError peError)
		{
			return this.FnTable.GetInt32(pchSection, pchSettingsKey, ref peError);
		}

		// Token: 0x0600011F RID: 287 RVA: 0x00003BCB File Offset: 0x00001DCB
		public float GetFloat(string pchSection, string pchSettingsKey, ref EVRSettingsError peError)
		{
			return this.FnTable.GetFloat(pchSection, pchSettingsKey, ref peError);
		}

		// Token: 0x06000120 RID: 288 RVA: 0x00003BE0 File Offset: 0x00001DE0
		public void GetString(string pchSection, string pchSettingsKey, StringBuilder pchValue, uint unValueLen, ref EVRSettingsError peError)
		{
			this.FnTable.GetString(pchSection, pchSettingsKey, pchValue, unValueLen, ref peError);
		}

		// Token: 0x06000121 RID: 289 RVA: 0x00003BF9 File Offset: 0x00001DF9
		public void RemoveSection(string pchSection, ref EVRSettingsError peError)
		{
			this.FnTable.RemoveSection(pchSection, ref peError);
		}

		// Token: 0x06000122 RID: 290 RVA: 0x00003C0D File Offset: 0x00001E0D
		public void RemoveKeyInSection(string pchSection, string pchSettingsKey, ref EVRSettingsError peError)
		{
			this.FnTable.RemoveKeyInSection(pchSection, pchSettingsKey, ref peError);
		}

		// Token: 0x04000151 RID: 337
		private IVRSettings FnTable;
	}
}
