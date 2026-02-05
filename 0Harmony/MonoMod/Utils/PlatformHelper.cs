using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;

namespace MonoMod.Utils
{
	// Token: 0x02000335 RID: 821
	internal static class PlatformHelper
	{
		// Token: 0x060012E1 RID: 4833 RVA: 0x0004457C File Offset: 0x0004277C
		static PlatformHelper()
		{
			PropertyInfo property = typeof(Environment).GetProperty("Platform", BindingFlags.Static | BindingFlags.NonPublic);
			string text;
			if (property != null)
			{
				text = property.GetValue(null, new object[0]).ToString();
			}
			else
			{
				text = Environment.OSVersion.Platform.ToString();
			}
			text = text.ToLowerInvariant();
			if (text.Contains("win"))
			{
				PlatformHelper.Current = Platform.Windows;
			}
			else if (text.Contains("mac") || text.Contains("osx"))
			{
				PlatformHelper.Current = Platform.MacOS;
			}
			else if (text.Contains("lin") || text.Contains("unix"))
			{
				PlatformHelper.Current = Platform.Linux;
			}
			if (PlatformHelper.Is(Platform.Linux) && Directory.Exists("/data") && File.Exists("/system/build.prop"))
			{
				PlatformHelper.Current = Platform.Android;
			}
			else if (PlatformHelper.Is(Platform.Unix) && Directory.Exists("/Applications") && Directory.Exists("/System"))
			{
				PlatformHelper.Current = Platform.iOS;
			}
			PropertyInfo property2 = typeof(Environment).GetProperty("Is64BitOperatingSystem");
			MethodInfo methodInfo = ((property2 != null) ? property2.GetGetMethod() : null);
			if (methodInfo != null)
			{
				PlatformHelper.Current |= (((bool)methodInfo.Invoke(null, new object[0])) ? Platform.Bits64 : ((Platform)0));
			}
			else
			{
				PlatformHelper.Current |= ((IntPtr.Size >= 8) ? Platform.Bits64 : ((Platform)0));
			}
			if ((PlatformHelper.Is(Platform.Unix) || PlatformHelper.Is(Platform.Unknown)) && Type.GetType("Mono.Runtime") != null)
			{
				try
				{
					string text2;
					using (Process process = Process.Start(new ProcessStartInfo("uname", "-m")
					{
						UseShellExecute = false,
						RedirectStandardOutput = true
					}))
					{
						text2 = process.StandardOutput.ReadLine().Trim();
					}
					if (text2.StartsWith("aarch") || text2.StartsWith("arm"))
					{
						PlatformHelper.Current |= Platform.ARM;
					}
					goto IL_0246;
				}
				catch (Exception)
				{
					goto IL_0246;
				}
			}
			PortableExecutableKinds portableExecutableKinds;
			ImageFileMachine imageFileMachine;
			typeof(object).Module.GetPEKind(out portableExecutableKinds, out imageFileMachine);
			if (imageFileMachine == ImageFileMachine.ARM)
			{
				PlatformHelper.Current |= Platform.ARM;
			}
			IL_0246:
			PlatformHelper.LibrarySuffix = (PlatformHelper.Is(Platform.MacOS) ? "dylib" : (PlatformHelper.Is(Platform.Unix) ? "so" : "dll"));
		}

		// Token: 0x17000392 RID: 914
		// (get) Token: 0x060012E2 RID: 4834 RVA: 0x00044814 File Offset: 0x00042A14
		// (set) Token: 0x060012E3 RID: 4835 RVA: 0x0004481B File Offset: 0x00042A1B
		public static Platform Current { get; private set; } = Platform.Unknown;

		// Token: 0x17000393 RID: 915
		// (get) Token: 0x060012E4 RID: 4836 RVA: 0x00044823 File Offset: 0x00042A23
		// (set) Token: 0x060012E5 RID: 4837 RVA: 0x0004482A File Offset: 0x00042A2A
		public static string LibrarySuffix { get; private set; }

		// Token: 0x060012E6 RID: 4838 RVA: 0x00044832 File Offset: 0x00042A32
		public static bool Is(Platform platform)
		{
			return (PlatformHelper.Current & platform) == platform;
		}
	}
}
