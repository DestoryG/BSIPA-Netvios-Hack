using System;
using System.Reflection;
using System.Runtime.InteropServices;

namespace IPA
{
	// Token: 0x02000006 RID: 6
	public static class Shortcut
	{
		// Token: 0x06000049 RID: 73 RVA: 0x00003294 File Offset: 0x00001494
		public static void Create(string fileName, string targetPath, string arguments, string workingDirectory, string description, string hotkey, string iconPath)
		{
			Shortcut.IWshShortcut wshShortcut = (Shortcut.IWshShortcut)Shortcut.MType.InvokeMember("CreateShortcut", BindingFlags.InvokeMethod, null, Shortcut.MShell, new object[] { fileName });
			wshShortcut.Description = description;
			wshShortcut.Hotkey = hotkey;
			wshShortcut.TargetPath = targetPath;
			wshShortcut.WorkingDirectory = workingDirectory;
			wshShortcut.Arguments = arguments;
			if (!string.IsNullOrEmpty(iconPath))
			{
				wshShortcut.IconLocation = iconPath;
			}
			wshShortcut.Save();
		}

		// Token: 0x04000026 RID: 38
		private static readonly Type MType = Type.GetTypeFromProgID("WScript.Shell");

		// Token: 0x04000027 RID: 39
		private static readonly object MShell = Activator.CreateInstance(Shortcut.MType);

		// Token: 0x0200000F RID: 15
		[TypeLibType(4160)]
		[Guid("F935DC23-1CF0-11D0-ADB9-00C04FD58A0B")]
		[ComImport]
		private interface IWshShortcut
		{
			// Token: 0x1700001B RID: 27
			// (get) Token: 0x0600006F RID: 111
			[DispId(0)]
			string FullName
			{
				[DispId(0)]
				[return: MarshalAs(UnmanagedType.BStr)]
				get;
			}

			// Token: 0x1700001C RID: 28
			// (get) Token: 0x06000070 RID: 112
			// (set) Token: 0x06000071 RID: 113
			[DispId(1000)]
			string Arguments
			{
				[DispId(1000)]
				[return: MarshalAs(UnmanagedType.BStr)]
				get;
				[DispId(1000)]
				[param: MarshalAs(UnmanagedType.BStr)]
				[param: In]
				set;
			}

			// Token: 0x1700001D RID: 29
			// (get) Token: 0x06000072 RID: 114
			// (set) Token: 0x06000073 RID: 115
			[DispId(1001)]
			string Description
			{
				[DispId(1001)]
				[return: MarshalAs(UnmanagedType.BStr)]
				get;
				[DispId(1001)]
				[param: MarshalAs(UnmanagedType.BStr)]
				[param: In]
				set;
			}

			// Token: 0x1700001E RID: 30
			// (get) Token: 0x06000074 RID: 116
			// (set) Token: 0x06000075 RID: 117
			[DispId(1002)]
			string Hotkey
			{
				[DispId(1002)]
				[return: MarshalAs(UnmanagedType.BStr)]
				get;
				[DispId(1002)]
				[param: MarshalAs(UnmanagedType.BStr)]
				[param: In]
				set;
			}

			// Token: 0x1700001F RID: 31
			// (get) Token: 0x06000076 RID: 118
			// (set) Token: 0x06000077 RID: 119
			[DispId(1003)]
			string IconLocation
			{
				[DispId(1003)]
				[return: MarshalAs(UnmanagedType.BStr)]
				get;
				[DispId(1003)]
				[param: MarshalAs(UnmanagedType.BStr)]
				[param: In]
				set;
			}

			// Token: 0x17000020 RID: 32
			// (set) Token: 0x06000078 RID: 120
			[DispId(1004)]
			string RelativePath
			{
				[DispId(1004)]
				[param: MarshalAs(UnmanagedType.BStr)]
				[param: In]
				set;
			}

			// Token: 0x17000021 RID: 33
			// (get) Token: 0x06000079 RID: 121
			// (set) Token: 0x0600007A RID: 122
			[DispId(1005)]
			string TargetPath
			{
				[DispId(1005)]
				[return: MarshalAs(UnmanagedType.BStr)]
				get;
				[DispId(1005)]
				[param: MarshalAs(UnmanagedType.BStr)]
				[param: In]
				set;
			}

			// Token: 0x17000022 RID: 34
			// (get) Token: 0x0600007B RID: 123
			// (set) Token: 0x0600007C RID: 124
			[DispId(1006)]
			int WindowStyle
			{
				[DispId(1006)]
				get;
				[DispId(1006)]
				[param: In]
				set;
			}

			// Token: 0x17000023 RID: 35
			// (get) Token: 0x0600007D RID: 125
			// (set) Token: 0x0600007E RID: 126
			[DispId(1007)]
			string WorkingDirectory
			{
				[DispId(1007)]
				[return: MarshalAs(UnmanagedType.BStr)]
				get;
				[DispId(1007)]
				[param: MarshalAs(UnmanagedType.BStr)]
				[param: In]
				set;
			}

			// Token: 0x0600007F RID: 127
			[TypeLibFunc(64)]
			[DispId(2000)]
			void Load([MarshalAs(UnmanagedType.BStr)] [In] string pathLink);

			// Token: 0x06000080 RID: 128
			[DispId(2001)]
			void Save();
		}
	}
}
