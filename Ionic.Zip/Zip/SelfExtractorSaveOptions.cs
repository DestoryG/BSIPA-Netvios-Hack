using System;

namespace Ionic.Zip
{
	// Token: 0x02000041 RID: 65
	public class SelfExtractorSaveOptions
	{
		// Token: 0x170000D1 RID: 209
		// (get) Token: 0x0600031B RID: 795 RVA: 0x00011AAC File Offset: 0x0000FCAC
		// (set) Token: 0x0600031C RID: 796 RVA: 0x00011AB4 File Offset: 0x0000FCB4
		public SelfExtractorFlavor Flavor { get; set; }

		// Token: 0x170000D2 RID: 210
		// (get) Token: 0x0600031D RID: 797 RVA: 0x00011ABD File Offset: 0x0000FCBD
		// (set) Token: 0x0600031E RID: 798 RVA: 0x00011AC5 File Offset: 0x0000FCC5
		public string PostExtractCommandLine { get; set; }

		// Token: 0x170000D3 RID: 211
		// (get) Token: 0x0600031F RID: 799 RVA: 0x00011ACE File Offset: 0x0000FCCE
		// (set) Token: 0x06000320 RID: 800 RVA: 0x00011AD6 File Offset: 0x0000FCD6
		public string DefaultExtractDirectory { get; set; }

		// Token: 0x170000D4 RID: 212
		// (get) Token: 0x06000321 RID: 801 RVA: 0x00011ADF File Offset: 0x0000FCDF
		// (set) Token: 0x06000322 RID: 802 RVA: 0x00011AE7 File Offset: 0x0000FCE7
		public string IconFile { get; set; }

		// Token: 0x170000D5 RID: 213
		// (get) Token: 0x06000323 RID: 803 RVA: 0x00011AF0 File Offset: 0x0000FCF0
		// (set) Token: 0x06000324 RID: 804 RVA: 0x00011AF8 File Offset: 0x0000FCF8
		public bool Quiet { get; set; }

		// Token: 0x170000D6 RID: 214
		// (get) Token: 0x06000325 RID: 805 RVA: 0x00011B01 File Offset: 0x0000FD01
		// (set) Token: 0x06000326 RID: 806 RVA: 0x00011B09 File Offset: 0x0000FD09
		public ExtractExistingFileAction ExtractExistingFile { get; set; }

		// Token: 0x170000D7 RID: 215
		// (get) Token: 0x06000327 RID: 807 RVA: 0x00011B12 File Offset: 0x0000FD12
		// (set) Token: 0x06000328 RID: 808 RVA: 0x00011B1A File Offset: 0x0000FD1A
		public bool RemoveUnpackedFilesAfterExecute { get; set; }

		// Token: 0x170000D8 RID: 216
		// (get) Token: 0x06000329 RID: 809 RVA: 0x00011B23 File Offset: 0x0000FD23
		// (set) Token: 0x0600032A RID: 810 RVA: 0x00011B2B File Offset: 0x0000FD2B
		public Version FileVersion { get; set; }

		// Token: 0x170000D9 RID: 217
		// (get) Token: 0x0600032B RID: 811 RVA: 0x00011B34 File Offset: 0x0000FD34
		// (set) Token: 0x0600032C RID: 812 RVA: 0x00011B3C File Offset: 0x0000FD3C
		public string ProductVersion { get; set; }

		// Token: 0x170000DA RID: 218
		// (get) Token: 0x0600032D RID: 813 RVA: 0x00011B45 File Offset: 0x0000FD45
		// (set) Token: 0x0600032E RID: 814 RVA: 0x00011B4D File Offset: 0x0000FD4D
		public string Copyright { get; set; }

		// Token: 0x170000DB RID: 219
		// (get) Token: 0x0600032F RID: 815 RVA: 0x00011B56 File Offset: 0x0000FD56
		// (set) Token: 0x06000330 RID: 816 RVA: 0x00011B5E File Offset: 0x0000FD5E
		public string Description { get; set; }

		// Token: 0x170000DC RID: 220
		// (get) Token: 0x06000331 RID: 817 RVA: 0x00011B67 File Offset: 0x0000FD67
		// (set) Token: 0x06000332 RID: 818 RVA: 0x00011B6F File Offset: 0x0000FD6F
		public string ProductName { get; set; }

		// Token: 0x170000DD RID: 221
		// (get) Token: 0x06000333 RID: 819 RVA: 0x00011B78 File Offset: 0x0000FD78
		// (set) Token: 0x06000334 RID: 820 RVA: 0x00011B80 File Offset: 0x0000FD80
		public string SfxExeWindowTitle { get; set; }

		// Token: 0x170000DE RID: 222
		// (get) Token: 0x06000335 RID: 821 RVA: 0x00011B89 File Offset: 0x0000FD89
		// (set) Token: 0x06000336 RID: 822 RVA: 0x00011B91 File Offset: 0x0000FD91
		public string AdditionalCompilerSwitches { get; set; }
	}
}
