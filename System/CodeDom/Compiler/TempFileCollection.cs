using System;
using System.Collections;
using System.IO;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Security.Principal;
using Microsoft.Win32;
using Microsoft.Win32.SafeHandles;

namespace System.CodeDom.Compiler
{
	// Token: 0x02000685 RID: 1669
	[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
	[PermissionSet(SecurityAction.InheritanceDemand, Name = "FullTrust")]
	[Serializable]
	public class TempFileCollection : ICollection, IEnumerable, IDisposable
	{
		// Token: 0x06003D6F RID: 15727 RVA: 0x000FC0D5 File Offset: 0x000FA2D5
		public TempFileCollection()
			: this(null, false)
		{
		}

		// Token: 0x06003D70 RID: 15728 RVA: 0x000FC0DF File Offset: 0x000FA2DF
		public TempFileCollection(string tempDir)
			: this(tempDir, false)
		{
		}

		// Token: 0x06003D71 RID: 15729 RVA: 0x000FC0EC File Offset: 0x000FA2EC
		[SecurityPermission(SecurityAction.Assert, ControlPrincipal = true)]
		public TempFileCollection(string tempDir, bool keepFiles)
		{
			this.keepFiles = keepFiles;
			this.tempDir = tempDir;
			this.files = new Hashtable(StringComparer.OrdinalIgnoreCase);
			WindowsImpersonationContext windowsImpersonationContext = Executor.RevertImpersonation();
			try
			{
				this.currentIdentity = WindowsIdentity.GetCurrent();
			}
			finally
			{
				Executor.ReImpersonate(windowsImpersonationContext);
			}
		}

		// Token: 0x06003D72 RID: 15730 RVA: 0x000FC148 File Offset: 0x000FA348
		void IDisposable.Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x06003D73 RID: 15731 RVA: 0x000FC157 File Offset: 0x000FA357
		protected virtual void Dispose(bool disposing)
		{
			this.Delete();
			this.DeleteHighIntegrityDirectory();
		}

		// Token: 0x06003D74 RID: 15732 RVA: 0x000FC168 File Offset: 0x000FA368
		~TempFileCollection()
		{
			this.Dispose(false);
		}

		// Token: 0x06003D75 RID: 15733 RVA: 0x000FC198 File Offset: 0x000FA398
		public string AddExtension(string fileExtension)
		{
			return this.AddExtension(fileExtension, this.keepFiles);
		}

		// Token: 0x06003D76 RID: 15734 RVA: 0x000FC1A8 File Offset: 0x000FA3A8
		public string AddExtension(string fileExtension, bool keepFile)
		{
			if (fileExtension == null || fileExtension.Length == 0)
			{
				throw new ArgumentException(SR.GetString("InvalidNullEmptyArgument", new object[] { "fileExtension" }), "fileExtension");
			}
			string text = this.BasePath + "." + fileExtension;
			this.AddFile(text, keepFile);
			return text;
		}

		// Token: 0x06003D77 RID: 15735 RVA: 0x000FC200 File Offset: 0x000FA400
		public void AddFile(string fileName, bool keepFile)
		{
			if (fileName == null || fileName.Length == 0)
			{
				throw new ArgumentException(SR.GetString("InvalidNullEmptyArgument", new object[] { "fileName" }), "fileName");
			}
			if (this.files[fileName] != null)
			{
				throw new ArgumentException(SR.GetString("DuplicateFileName", new object[] { fileName }), "fileName");
			}
			this.files.Add(fileName, keepFile);
		}

		// Token: 0x06003D78 RID: 15736 RVA: 0x000FC27A File Offset: 0x000FA47A
		public IEnumerator GetEnumerator()
		{
			return this.files.Keys.GetEnumerator();
		}

		// Token: 0x06003D79 RID: 15737 RVA: 0x000FC28C File Offset: 0x000FA48C
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.files.Keys.GetEnumerator();
		}

		// Token: 0x06003D7A RID: 15738 RVA: 0x000FC29E File Offset: 0x000FA49E
		void ICollection.CopyTo(Array array, int start)
		{
			this.files.Keys.CopyTo(array, start);
		}

		// Token: 0x06003D7B RID: 15739 RVA: 0x000FC2B2 File Offset: 0x000FA4B2
		public void CopyTo(string[] fileNames, int start)
		{
			this.files.Keys.CopyTo(fileNames, start);
		}

		// Token: 0x17000E9D RID: 3741
		// (get) Token: 0x06003D7C RID: 15740 RVA: 0x000FC2C6 File Offset: 0x000FA4C6
		public int Count
		{
			get
			{
				return this.files.Count;
			}
		}

		// Token: 0x17000E9E RID: 3742
		// (get) Token: 0x06003D7D RID: 15741 RVA: 0x000FC2D3 File Offset: 0x000FA4D3
		int ICollection.Count
		{
			get
			{
				return this.files.Count;
			}
		}

		// Token: 0x17000E9F RID: 3743
		// (get) Token: 0x06003D7E RID: 15742 RVA: 0x000FC2E0 File Offset: 0x000FA4E0
		object ICollection.SyncRoot
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17000EA0 RID: 3744
		// (get) Token: 0x06003D7F RID: 15743 RVA: 0x000FC2E3 File Offset: 0x000FA4E3
		bool ICollection.IsSynchronized
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000EA1 RID: 3745
		// (get) Token: 0x06003D80 RID: 15744 RVA: 0x000FC2E6 File Offset: 0x000FA4E6
		public string TempDir
		{
			get
			{
				if (this.tempDir != null)
				{
					return this.tempDir;
				}
				return string.Empty;
			}
		}

		// Token: 0x17000EA2 RID: 3746
		// (get) Token: 0x06003D81 RID: 15745 RVA: 0x000FC2FC File Offset: 0x000FA4FC
		public string BasePath
		{
			get
			{
				this.EnsureTempNameCreated();
				return this.basePath;
			}
		}

		// Token: 0x06003D82 RID: 15746 RVA: 0x000FC30C File Offset: 0x000FA50C
		private void EnsureTempNameCreated()
		{
			if (this.basePath == null)
			{
				string text = null;
				bool flag = false;
				int num = 5000;
				do
				{
					try
					{
						this.basePath = this.GetTempFileName(this.TempDir);
						string fullPath = Path.GetFullPath(this.basePath);
						new FileIOPermission(FileIOPermissionAccess.AllAccess, fullPath).Demand();
						text = this.basePath + ".tmp";
						using (new FileStream(text, FileMode.CreateNew, FileAccess.Write))
						{
						}
						flag = true;
					}
					catch (IOException ex)
					{
						num--;
						uint num2 = 2147942480U;
						if (num == 0 || (long)Marshal.GetHRForException(ex) != (long)((ulong)num2))
						{
							throw;
						}
						flag = false;
					}
				}
				while (!flag);
				this.files.Add(text, this.keepFiles);
			}
		}

		// Token: 0x17000EA3 RID: 3747
		// (get) Token: 0x06003D83 RID: 15747 RVA: 0x000FC3E8 File Offset: 0x000FA5E8
		// (set) Token: 0x06003D84 RID: 15748 RVA: 0x000FC3F0 File Offset: 0x000FA5F0
		public bool KeepFiles
		{
			get
			{
				return this.keepFiles;
			}
			set
			{
				this.keepFiles = value;
			}
		}

		// Token: 0x06003D85 RID: 15749 RVA: 0x000FC3FC File Offset: 0x000FA5FC
		private bool KeepFile(string fileName)
		{
			object obj = this.files[fileName];
			return obj != null && (bool)obj;
		}

		// Token: 0x06003D86 RID: 15750 RVA: 0x000FC424 File Offset: 0x000FA624
		public void Delete()
		{
			if (this.files != null && this.files.Count > 0)
			{
				string[] array = new string[this.files.Count];
				this.files.Keys.CopyTo(array, 0);
				foreach (string text in array)
				{
					if (!this.KeepFile(text))
					{
						this.Delete(text);
						this.files.Remove(text);
					}
				}
			}
		}

		// Token: 0x06003D87 RID: 15751 RVA: 0x000FC49C File Offset: 0x000FA69C
		private void DeleteHighIntegrityDirectory()
		{
			try
			{
				if (this.currentIdentity != null && Directory.Exists(this.highIntegrityDirectory))
				{
					TempFileCollection.RemoveAceOnTempDirectory(this.highIntegrityDirectory, this.currentIdentity.User.ToString());
					if (Directory.GetFiles(this.highIntegrityDirectory).Length == 0)
					{
						Directory.Delete(this.highIntegrityDirectory, true);
					}
				}
			}
			catch
			{
			}
		}

		// Token: 0x06003D88 RID: 15752 RVA: 0x000FC508 File Offset: 0x000FA708
		internal void SafeDelete()
		{
			WindowsImpersonationContext windowsImpersonationContext = Executor.RevertImpersonation();
			try
			{
				this.Delete();
			}
			finally
			{
				Executor.ReImpersonate(windowsImpersonationContext);
			}
		}

		// Token: 0x06003D89 RID: 15753 RVA: 0x000FC53C File Offset: 0x000FA73C
		private void Delete(string fileName)
		{
			try
			{
				File.Delete(fileName);
			}
			catch
			{
			}
		}

		// Token: 0x06003D8A RID: 15754 RVA: 0x000FC564 File Offset: 0x000FA764
		private string GetTempFileName(string tempDir)
		{
			string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(Path.GetRandomFileName());
			if (string.IsNullOrEmpty(tempDir))
			{
				tempDir = Path.GetTempPath();
				if (!LocalAppContextSwitches.DisableTempFileCollectionDirectoryFeature && this.currentIdentity != null && new WindowsPrincipal(this.currentIdentity).IsInRole(WindowsBuiltInRole.Administrator))
				{
					tempDir = Path.Combine(tempDir, fileNameWithoutExtension);
					TempFileCollection.CreateTempDirectoryWithAce(tempDir, this.currentIdentity.User.ToString());
					this.highIntegrityDirectory = tempDir;
				}
			}
			string text;
			if (tempDir.EndsWith("\\", StringComparison.Ordinal))
			{
				text = tempDir + fileNameWithoutExtension;
			}
			else
			{
				text = tempDir + "\\" + fileNameWithoutExtension;
			}
			return text;
		}

		// Token: 0x06003D8B RID: 15755 RVA: 0x000FC600 File Offset: 0x000FA800
		private static void CreateTempDirectoryWithAce(string directory, string identity)
		{
			string text = "D:(D;OI;SD;;;" + identity + ")(A;OICI;FA;;;BA)S:(ML;OI;NW;;;HI)";
			SafeLocalMemHandle safeLocalMemHandle = null;
			SafeLocalMemHandle.ConvertStringSecurityDescriptorToSecurityDescriptor(text, 1, out safeLocalMemHandle, IntPtr.Zero);
			Microsoft.Win32.NativeMethods.CreateDirectory(directory, safeLocalMemHandle);
		}

		// Token: 0x06003D8C RID: 15756 RVA: 0x000FC638 File Offset: 0x000FA838
		private static void RemoveAceOnTempDirectory(string directory, string identity)
		{
			string text = "D:(A;OICI;FA;;;" + identity + ")(A;OICI;FA;;;BA)";
			SafeLocalMemHandle safeLocalMemHandle = null;
			SafeLocalMemHandle.ConvertStringSecurityDescriptorToSecurityDescriptor(text, 1, out safeLocalMemHandle, IntPtr.Zero);
			Microsoft.Win32.NativeMethods.SetNamedSecurityInfo(directory, safeLocalMemHandle);
		}

		// Token: 0x04002CC1 RID: 11457
		private string basePath;

		// Token: 0x04002CC2 RID: 11458
		private string tempDir;

		// Token: 0x04002CC3 RID: 11459
		private bool keepFiles;

		// Token: 0x04002CC4 RID: 11460
		private Hashtable files;

		// Token: 0x04002CC5 RID: 11461
		[NonSerialized]
		private WindowsIdentity currentIdentity;

		// Token: 0x04002CC6 RID: 11462
		[NonSerialized]
		private string highIntegrityDirectory;
	}
}
