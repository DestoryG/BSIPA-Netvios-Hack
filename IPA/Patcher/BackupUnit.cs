using System;
using System.Collections.Generic;
using System.IO;

namespace IPA.Patcher
{
	// Token: 0x02000008 RID: 8
	public class BackupUnit
	{
		// Token: 0x17000018 RID: 24
		// (get) Token: 0x0600004F RID: 79 RVA: 0x00003494 File Offset: 0x00001694
		private string Name { get; }

		// Token: 0x06000050 RID: 80 RVA: 0x0000349C File Offset: 0x0000169C
		public BackupUnit(PatchContext context)
			: this(context, DateTime.Now.ToString("yyyy-MM-dd_h-mm-ss"))
		{
		}

		// Token: 0x06000051 RID: 81 RVA: 0x000034C4 File Offset: 0x000016C4
		private BackupUnit(PatchContext context, string name)
		{
			this.Name = name;
			this._context = context;
			this._backupPath = new DirectoryInfo(Path.Combine(this._context.BackupPath, this.Name));
			this._manifestFile = new FileInfo(Path.Combine(this._backupPath.FullName, BackupUnit._ManifestFileName));
		}

		// Token: 0x06000052 RID: 82 RVA: 0x00003534 File Offset: 0x00001734
		public static BackupUnit FromDirectory(DirectoryInfo directory, PatchContext context)
		{
			BackupUnit backupUnit = new BackupUnit(context, directory.Name);
			if (backupUnit._manifestFile.Exists)
			{
				foreach (string text in File.ReadAllText(backupUnit._manifestFile.FullName).Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries))
				{
					backupUnit._files.Add(text);
				}
			}
			else
			{
				foreach (FileInfo fileInfo in directory.GetFiles("*", SearchOption.AllDirectories))
				{
					if (!(fileInfo.Name == BackupUnit._ManifestFileName))
					{
						string text2 = fileInfo.FullName.Substring(directory.FullName.Length + 1);
						backupUnit._files.Add(text2);
					}
				}
			}
			return backupUnit;
		}

		// Token: 0x06000053 RID: 83 RVA: 0x000035FF File Offset: 0x000017FF
		public void Add(string file)
		{
			this.Add(new FileInfo(file));
		}

		// Token: 0x06000054 RID: 84 RVA: 0x0000360D File Offset: 0x0000180D
		internal void Delete()
		{
			this._backupPath.Delete(true);
		}

		// Token: 0x06000055 RID: 85 RVA: 0x0000361C File Offset: 0x0000181C
		public void Add(FileInfo file)
		{
			if (!file.FullName.StartsWith(this._context.ProjectRoot))
			{
				Console.Error.WriteLine("Invalid file path for backup! {0}", file);
				return;
			}
			string text = file.FullName.Substring(this._context.ProjectRoot.Length + 1);
			FileInfo fileInfo = new FileInfo(Path.Combine(this._backupPath.FullName, text));
			if (this._files.Contains(text))
			{
				Console.WriteLine("Skipping backup of {0}", text);
				return;
			}
			DirectoryInfo directory = fileInfo.Directory;
			if (directory != null)
			{
				directory.Create();
			}
			if (file.Exists)
			{
				file.CopyTo(fileInfo.FullName);
			}
			if (!File.Exists(this._manifestFile.FullName))
			{
				this._manifestFile.Create().Close();
			}
			StreamWriter streamWriter = this._manifestFile.AppendText();
			streamWriter.WriteLine(text);
			streamWriter.Close();
			this._files.Add(text);
		}

		// Token: 0x06000056 RID: 86 RVA: 0x0000370C File Offset: 0x0000190C
		public void Restore()
		{
			foreach (string text in this._files)
			{
				Console.WriteLine("Restoring {0}", text);
				FileInfo fileInfo = new FileInfo(Path.Combine(this._backupPath.FullName, text));
				FileInfo fileInfo2 = new FileInfo(Path.Combine(this._context.ProjectRoot, text));
				if (fileInfo.Exists && fileInfo.Length > 0L)
				{
					Console.WriteLine("  {0} => {1}", fileInfo.FullName, fileInfo2.FullName);
					DirectoryInfo directory = fileInfo2.Directory;
					if (directory != null)
					{
						directory.Create();
					}
					fileInfo.CopyTo(fileInfo2.FullName, true);
				}
				else
				{
					Console.WriteLine("  x {0}", fileInfo2.FullName);
					if (fileInfo2.Exists)
					{
						fileInfo2.Delete();
					}
				}
			}
		}

		// Token: 0x04000029 RID: 41
		private readonly DirectoryInfo _backupPath;

		// Token: 0x0400002A RID: 42
		private readonly PatchContext _context;

		// Token: 0x0400002B RID: 43
		private readonly List<string> _files = new List<string>();

		// Token: 0x0400002C RID: 44
		private readonly FileInfo _manifestFile;

		// Token: 0x0400002D RID: 45
		private static string _ManifestFileName = "$manifest$.txt";
	}
}
