using System;
using System.Runtime.InteropServices;

namespace BeatSaverDownloader.Misc
{
	// Token: 0x0200001F RID: 31
	public class FileOperationAPIWrapper
	{
		// Token: 0x06000158 RID: 344
		[DllImport("shell32.dll", CharSet = CharSet.Auto)]
		private static extern int SHFileOperation(ref FileOperationAPIWrapper.SHFILEOPSTRUCT FileOp);

		// Token: 0x06000159 RID: 345 RVA: 0x0000650C File Offset: 0x0000470C
		public static bool Send(string path, FileOperationAPIWrapper.FileOperationFlags flags)
		{
			bool flag;
			try
			{
				FileOperationAPIWrapper.SHFILEOPSTRUCT shfileopstruct = new FileOperationAPIWrapper.SHFILEOPSTRUCT
				{
					wFunc = FileOperationAPIWrapper.FileOperationType.FO_DELETE,
					pFrom = path + "\0\0",
					fFlags = (FileOperationAPIWrapper.FileOperationFlags.FOF_ALLOWUNDO | flags)
				};
				FileOperationAPIWrapper.SHFileOperation(ref shfileopstruct);
				flag = true;
			}
			catch (Exception)
			{
				flag = false;
			}
			return flag;
		}

		// Token: 0x0600015A RID: 346 RVA: 0x0000656C File Offset: 0x0000476C
		public static bool Send(string path)
		{
			return FileOperationAPIWrapper.Send(path, FileOperationAPIWrapper.FileOperationFlags.FOF_NOCONFIRMATION | FileOperationAPIWrapper.FileOperationFlags.FOF_WANTNUKEWARNING);
		}

		// Token: 0x0600015B RID: 347 RVA: 0x00006579 File Offset: 0x00004779
		public static bool MoveToRecycleBin(string path)
		{
			return FileOperationAPIWrapper.Send(path, FileOperationAPIWrapper.FileOperationFlags.FOF_SILENT | FileOperationAPIWrapper.FileOperationFlags.FOF_NOCONFIRMATION | FileOperationAPIWrapper.FileOperationFlags.FOF_NOERRORUI);
		}

		// Token: 0x0600015C RID: 348 RVA: 0x00006588 File Offset: 0x00004788
		private static bool deleteFile(string path, FileOperationAPIWrapper.FileOperationFlags flags)
		{
			bool flag;
			try
			{
				FileOperationAPIWrapper.SHFILEOPSTRUCT shfileopstruct = new FileOperationAPIWrapper.SHFILEOPSTRUCT
				{
					wFunc = FileOperationAPIWrapper.FileOperationType.FO_DELETE,
					pFrom = path + "\0\0",
					fFlags = flags
				};
				FileOperationAPIWrapper.SHFileOperation(ref shfileopstruct);
				flag = true;
			}
			catch (Exception)
			{
				flag = false;
			}
			return flag;
		}

		// Token: 0x0600015D RID: 349 RVA: 0x000065E4 File Offset: 0x000047E4
		public static bool DeleteCompletelySilent(string path)
		{
			return FileOperationAPIWrapper.deleteFile(path, FileOperationAPIWrapper.FileOperationFlags.FOF_SILENT | FileOperationAPIWrapper.FileOperationFlags.FOF_NOCONFIRMATION | FileOperationAPIWrapper.FileOperationFlags.FOF_NOERRORUI);
		}

		// Token: 0x0200004B RID: 75
		[Flags]
		public enum FileOperationFlags : ushort
		{
			// Token: 0x0400014F RID: 335
			FOF_SILENT = 4,
			// Token: 0x04000150 RID: 336
			FOF_NOCONFIRMATION = 16,
			// Token: 0x04000151 RID: 337
			FOF_ALLOWUNDO = 64,
			// Token: 0x04000152 RID: 338
			FOF_SIMPLEPROGRESS = 256,
			// Token: 0x04000153 RID: 339
			FOF_NOERRORUI = 1024,
			// Token: 0x04000154 RID: 340
			FOF_WANTNUKEWARNING = 16384
		}

		// Token: 0x0200004C RID: 76
		public enum FileOperationType : uint
		{
			// Token: 0x04000156 RID: 342
			FO_MOVE = 1U,
			// Token: 0x04000157 RID: 343
			FO_COPY,
			// Token: 0x04000158 RID: 344
			FO_DELETE,
			// Token: 0x04000159 RID: 345
			FO_RENAME
		}

		// Token: 0x0200004D RID: 77
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
		private struct SHFILEOPSTRUCT
		{
			// Token: 0x0400015A RID: 346
			public IntPtr hwnd;

			// Token: 0x0400015B RID: 347
			[MarshalAs(UnmanagedType.U4)]
			public FileOperationAPIWrapper.FileOperationType wFunc;

			// Token: 0x0400015C RID: 348
			public string pFrom;

			// Token: 0x0400015D RID: 349
			public string pTo;

			// Token: 0x0400015E RID: 350
			public FileOperationAPIWrapper.FileOperationFlags fFlags;

			// Token: 0x0400015F RID: 351
			[MarshalAs(UnmanagedType.Bool)]
			public bool fAnyOperationsAborted;

			// Token: 0x04000160 RID: 352
			public IntPtr hNameMappings;

			// Token: 0x04000161 RID: 353
			public string lpszProgressTitle;
		}
	}
}
