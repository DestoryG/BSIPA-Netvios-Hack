using System;
using System.IO;
using System.Reflection;

namespace IPA
{
	// Token: 0x02000004 RID: 4
	public class PatchContext
	{
		// Token: 0x17000007 RID: 7
		// (get) Token: 0x06000016 RID: 22 RVA: 0x000025EB File Offset: 0x000007EB
		// (set) Token: 0x06000017 RID: 23 RVA: 0x000025F3 File Offset: 0x000007F3
		public string Executable { get; private set; }

		// Token: 0x17000008 RID: 8
		// (get) Token: 0x06000018 RID: 24 RVA: 0x000025FC File Offset: 0x000007FC
		// (set) Token: 0x06000019 RID: 25 RVA: 0x00002604 File Offset: 0x00000804
		public string DataPathSrc { get; private set; }

		// Token: 0x17000009 RID: 9
		// (get) Token: 0x0600001A RID: 26 RVA: 0x0000260D File Offset: 0x0000080D
		// (set) Token: 0x0600001B RID: 27 RVA: 0x00002615 File Offset: 0x00000815
		public string LibsPathSrc { get; private set; }

		// Token: 0x1700000A RID: 10
		// (get) Token: 0x0600001C RID: 28 RVA: 0x0000261E File Offset: 0x0000081E
		// (set) Token: 0x0600001D RID: 29 RVA: 0x00002626 File Offset: 0x00000826
		public string PluginsFolder { get; private set; }

		// Token: 0x1700000B RID: 11
		// (get) Token: 0x0600001E RID: 30 RVA: 0x0000262F File Offset: 0x0000082F
		// (set) Token: 0x0600001F RID: 31 RVA: 0x00002637 File Offset: 0x00000837
		public string ProjectName { get; private set; }

		// Token: 0x1700000C RID: 12
		// (get) Token: 0x06000020 RID: 32 RVA: 0x00002640 File Offset: 0x00000840
		// (set) Token: 0x06000021 RID: 33 RVA: 0x00002648 File Offset: 0x00000848
		public string DataPathDst { get; private set; }

		// Token: 0x1700000D RID: 13
		// (get) Token: 0x06000022 RID: 34 RVA: 0x00002651 File Offset: 0x00000851
		// (set) Token: 0x06000023 RID: 35 RVA: 0x00002659 File Offset: 0x00000859
		public string LibsPathDst { get; private set; }

		// Token: 0x1700000E RID: 14
		// (get) Token: 0x06000024 RID: 36 RVA: 0x00002662 File Offset: 0x00000862
		// (set) Token: 0x06000025 RID: 37 RVA: 0x0000266A File Offset: 0x0000086A
		public string ManagedPath { get; private set; }

		// Token: 0x1700000F RID: 15
		// (get) Token: 0x06000026 RID: 38 RVA: 0x00002673 File Offset: 0x00000873
		// (set) Token: 0x06000027 RID: 39 RVA: 0x0000267B File Offset: 0x0000087B
		public string EngineFile { get; private set; }

		// Token: 0x17000010 RID: 16
		// (get) Token: 0x06000028 RID: 40 RVA: 0x00002684 File Offset: 0x00000884
		// (set) Token: 0x06000029 RID: 41 RVA: 0x0000268C File Offset: 0x0000088C
		public string AssemblyFile { get; private set; }

		// Token: 0x17000011 RID: 17
		// (get) Token: 0x0600002A RID: 42 RVA: 0x00002695 File Offset: 0x00000895
		// (set) Token: 0x0600002B RID: 43 RVA: 0x0000269D File Offset: 0x0000089D
		public string ProjectRoot { get; private set; }

		// Token: 0x17000012 RID: 18
		// (get) Token: 0x0600002C RID: 44 RVA: 0x000026A6 File Offset: 0x000008A6
		// (set) Token: 0x0600002D RID: 45 RVA: 0x000026AE File Offset: 0x000008AE
		public string IPARoot { get; private set; }

		// Token: 0x17000013 RID: 19
		// (get) Token: 0x0600002E RID: 46 RVA: 0x000026B7 File Offset: 0x000008B7
		// (set) Token: 0x0600002F RID: 47 RVA: 0x000026BF File Offset: 0x000008BF
		public string ShortcutPath { get; private set; }

		// Token: 0x17000014 RID: 20
		// (get) Token: 0x06000030 RID: 48 RVA: 0x000026C8 File Offset: 0x000008C8
		// (set) Token: 0x06000031 RID: 49 RVA: 0x000026D0 File Offset: 0x000008D0
		public string IPA { get; private set; }

		// Token: 0x17000015 RID: 21
		// (get) Token: 0x06000032 RID: 50 RVA: 0x000026D9 File Offset: 0x000008D9
		// (set) Token: 0x06000033 RID: 51 RVA: 0x000026E1 File Offset: 0x000008E1
		public string BackupPath { get; private set; }

		// Token: 0x06000034 RID: 52 RVA: 0x000026EA File Offset: 0x000008EA
		private PatchContext()
		{
		}

		// Token: 0x06000035 RID: 53 RVA: 0x000026F4 File Offset: 0x000008F4
		public static PatchContext Create(string exe)
		{
			PatchContext patchContext = new PatchContext
			{
				Executable = exe
			};
			PatchContext patchContext2 = patchContext;
			DirectoryInfo directory = new FileInfo(patchContext.Executable).Directory;
			patchContext2.ProjectRoot = ((directory != null) ? directory.FullName : null);
			PatchContext patchContext3 = patchContext;
			string directoryName = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
			if (directoryName == null)
			{
				throw new InvalidOperationException();
			}
			patchContext3.IPARoot = Path.Combine(directoryName, "IPA");
			patchContext.IPA = Assembly.GetExecutingAssembly().Location;
			patchContext.DataPathSrc = Path.Combine(patchContext.IPARoot, "Data");
			patchContext.LibsPathSrc = Path.Combine(patchContext.IPARoot, "Libs");
			PatchContext patchContext4 = patchContext;
			string projectRoot = patchContext.ProjectRoot;
			if (projectRoot == null)
			{
				throw new InvalidOperationException();
			}
			patchContext4.PluginsFolder = Path.Combine(projectRoot, "Plugins");
			patchContext.ProjectName = Path.GetFileNameWithoutExtension(patchContext.Executable);
			patchContext.DataPathDst = Path.Combine(patchContext.ProjectRoot, patchContext.ProjectName + "_Data");
			patchContext.LibsPathDst = Path.Combine(patchContext.ProjectRoot, "Libs");
			patchContext.ManagedPath = Path.Combine(patchContext.DataPathDst, "Managed");
			patchContext.EngineFile = Path.Combine(patchContext.ManagedPath, "UnityEngine.CoreModule.dll");
			patchContext.AssemblyFile = Path.Combine(patchContext.ManagedPath, "Assembly-CSharp.dll");
			patchContext.BackupPath = Path.Combine(patchContext.IPARoot, "Backups", patchContext.ProjectName);
			string text = patchContext.ProjectName + " (Patch & Launch)";
			patchContext.ShortcutPath = Path.Combine(patchContext.ProjectRoot, text) + ".lnk";
			Directory.CreateDirectory(patchContext.BackupPath);
			return patchContext;
		}
	}
}
