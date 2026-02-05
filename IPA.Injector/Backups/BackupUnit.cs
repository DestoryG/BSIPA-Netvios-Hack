using System;
using System.Collections.Generic;
using System.IO;
using IPA.Utilities;

namespace IPA.Injector.Backups
{
	// Token: 0x0200000A RID: 10
	internal class BackupUnit
	{
		// Token: 0x17000001 RID: 1
		// (get) Token: 0x06000026 RID: 38 RVA: 0x000033CF File Offset: 0x000015CF
		// (set) Token: 0x06000027 RID: 39 RVA: 0x000033D7 File Offset: 0x000015D7
		public string Name { get; private set; }

		// Token: 0x06000028 RID: 40 RVA: 0x000033E0 File Offset: 0x000015E0
		public BackupUnit(string dir)
			: this(dir, Utils.CurrentTime().ToString("yyyy-MM-dd_h-mm-ss"))
		{
		}

		// Token: 0x06000029 RID: 41 RVA: 0x00003408 File Offset: 0x00001608
		private BackupUnit(string dir, string name)
		{
			this.Name = name;
			this._backupPath = new DirectoryInfo(Path.Combine(dir, this.Name));
			this._manifestFile = new FileInfo(Path.Combine(this._backupPath.FullName, "$manifest$.txt"));
		}

		// Token: 0x0600002A RID: 42 RVA: 0x00003464 File Offset: 0x00001664
		public static BackupUnit FromDirectory(DirectoryInfo directory, string dir)
		{
			BackupUnit unit = new BackupUnit(dir, directory.Name);
			if (unit._manifestFile.Exists)
			{
				foreach (string line in File.ReadAllText(unit._manifestFile.FullName).Split(new string[]
				{
					Environment.NewLine,
					"\n",
					"\r"
				}, StringSplitOptions.RemoveEmptyEntries))
				{
					unit._files.Add(line);
				}
			}
			else
			{
				foreach (FileInfo file in directory.GetFiles("*", SearchOption.AllDirectories))
				{
					if (!(file.Name == "$manifest$.txt"))
					{
						string relativePath = file.FullName.Substring(directory.FullName.Length + 1);
						unit._files.Add(relativePath);
					}
				}
			}
			return unit;
		}

		// Token: 0x0600002B RID: 43 RVA: 0x00003541 File Offset: 0x00001741
		public void Add(string file)
		{
			this.Add(new FileInfo(file));
		}

		// Token: 0x0600002C RID: 44 RVA: 0x0000354F File Offset: 0x0000174F
		internal void Delete()
		{
			this._backupPath.Delete(true);
		}

		// Token: 0x0600002D RID: 45 RVA: 0x00003560 File Offset: 0x00001760
		public void Add(FileInfo file)
		{
			string relativePath = Utils.GetRelativePath(file.FullName, Environment.CurrentDirectory);
			FileInfo backupPath = new FileInfo(Path.Combine(this._backupPath.FullName, relativePath));
			DirectoryInfo directory = backupPath.Directory;
			if (directory != null)
			{
				directory.Create();
			}
			if (file.Exists)
			{
				if (File.Exists(backupPath.FullName))
				{
					File.Delete(backupPath.FullName);
				}
				file.CopyTo(backupPath.FullName);
			}
			if (this._files.Contains(relativePath))
			{
				return;
			}
			if (!File.Exists(this._manifestFile.FullName))
			{
				this._manifestFile.Create().Close();
			}
			StreamWriter streamWriter = this._manifestFile.AppendText();
			streamWriter.WriteLine(relativePath);
			streamWriter.Close();
			this._files.Add(relativePath);
		}

		// Token: 0x0400000B RID: 11
		private readonly DirectoryInfo _backupPath;

		// Token: 0x0400000C RID: 12
		private readonly HashSet<string> _files = new HashSet<string>();

		// Token: 0x0400000D RID: 13
		private readonly FileInfo _manifestFile;

		// Token: 0x0400000E RID: 14
		private const string ManifestFileName = "$manifest$.txt";
	}
}
