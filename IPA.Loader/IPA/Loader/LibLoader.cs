using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using IPA.Logging;

namespace IPA.Loader
{
	// Token: 0x02000044 RID: 68
	internal static class LibLoader
	{
		// Token: 0x1700004F RID: 79
		// (get) Token: 0x060001AB RID: 427 RVA: 0x00007620 File Offset: 0x00005820
		internal static string LibraryPath
		{
			get
			{
				return Path.Combine(Environment.CurrentDirectory, "Libs");
			}
		}

		// Token: 0x17000050 RID: 80
		// (get) Token: 0x060001AC RID: 428 RVA: 0x00007631 File Offset: 0x00005831
		internal static string NativeLibraryPath
		{
			get
			{
				return Path.Combine(LibLoader.LibraryPath, "Native");
			}
		}

		// Token: 0x060001AD RID: 429 RVA: 0x00007642 File Offset: 0x00005842
		internal static void Configure()
		{
			LibLoader.SetupAssemblyFilenames(true);
			AppDomain.CurrentDomain.AssemblyResolve -= LibLoader.AssemblyLibLoader;
			AppDomain.CurrentDomain.AssemblyResolve += LibLoader.AssemblyLibLoader;
		}

		// Token: 0x060001AE RID: 430 RVA: 0x00007678 File Offset: 0x00005878
		internal static void SetupAssemblyFilenames(bool force = false)
		{
			if (LibLoader.FilenameLocations == null || force)
			{
				LibLoader.FilenameLocations = new Dictionary<string, string>();
				foreach (FileInfo fn in LibLoader.TraverseTree(LibLoader.LibraryPath, (string s) => s != LibLoader.NativeLibraryPath))
				{
					if (LibLoader.FilenameLocations.ContainsKey(fn.Name))
					{
						LibLoader.Log(Logger.Level.Critical, "Multiple instances of " + fn.Name + " exist in Libs! Ignoring " + fn.FullName);
					}
					else
					{
						LibLoader.FilenameLocations.Add(fn.Name, fn.FullName);
					}
				}
				if (!LibLoader.SetDefaultDllDirectories(LibLoader.LoadLibraryFlags.LOAD_LIBRARY_SEARCH_APPLICATION_DIR | LibLoader.LoadLibraryFlags.LOAD_LIBRARY_SEARCH_DEFAULT_DIRS | LibLoader.LoadLibraryFlags.LOAD_LIBRARY_SEARCH_SYSTEM32 | LibLoader.LoadLibraryFlags.LOAD_LIBRARY_SEARCH_USER_DIRS))
				{
					Win32Exception err = new Win32Exception();
					LibLoader.Log(Logger.Level.Critical, "Error configuring DLL search path");
					LibLoader.Log(Logger.Level.Critical, err);
					return;
				}
				if (Directory.Exists(LibLoader.NativeLibraryPath))
				{
					LibLoader.<SetupAssemblyFilenames>g__AddDir|6_0(LibLoader.NativeLibraryPath);
					LibLoader.TraverseTree(LibLoader.NativeLibraryPath, delegate(string dir)
					{
						LibLoader.<SetupAssemblyFilenames>g__AddDir|6_0(dir);
						return true;
					}).All((FileInfo f) => true);
				}
				string[] array = Environment.GetEnvironmentVariable("path").Split(new char[] { Path.PathSeparator });
				for (int i = 0; i < array.Length; i++)
				{
					LibLoader.<SetupAssemblyFilenames>g__AddDir|6_0(array[i]);
				}
			}
		}

		// Token: 0x060001AF RID: 431 RVA: 0x0000780C File Offset: 0x00005A0C
		public static Assembly AssemblyLibLoader(object source, ResolveEventArgs e)
		{
			return LibLoader.LoadLibrary(new AssemblyName(e.Name));
		}

		// Token: 0x060001B0 RID: 432 RVA: 0x00007820 File Offset: 0x00005A20
		internal static Assembly LoadLibrary(AssemblyName asmName)
		{
			LibLoader.Log(Logger.Level.Debug, string.Format("Resolving library {0}", asmName));
			LibLoader.SetupAssemblyFilenames(false);
			string testFile = asmName.Name + ".dll";
			LibLoader.Log(Logger.Level.Debug, "Looking for file " + asmName.Name + ".dll");
			string path;
			if (LibLoader.FilenameLocations.TryGetValue(testFile, out path))
			{
				LibLoader.Log(Logger.Level.Debug, "Found file " + testFile + " as " + path);
				if (File.Exists(path))
				{
					return Assembly.LoadFrom(path);
				}
				LibLoader.Log(Logger.Level.Critical, "but " + path + " no longer exists!");
			}
			else if (LibLoader.FilenameLocations.TryGetValue(testFile = string.Format("{0}.{1}.dll", asmName.Name, asmName.Version), out path))
			{
				LibLoader.Log(Logger.Level.Debug, "Found file " + testFile + " as " + path);
				LibLoader.Log(Logger.Level.Warning, string.Concat(new string[] { "File ", testFile, " should be renamed to just ", asmName.Name, ".dll" }));
				if (File.Exists(path))
				{
					return Assembly.LoadFrom(path);
				}
				LibLoader.Log(Logger.Level.Critical, "but " + path + " no longer exists!");
			}
			LibLoader.Log(Logger.Level.Critical, string.Format("No library {0} found", asmName));
			return null;
		}

		// Token: 0x060001B1 RID: 433 RVA: 0x0000796D File Offset: 0x00005B6D
		internal static void Log(Logger.Level lvl, string message)
		{
			if (Logger.LogCreated)
			{
				LibLoader.AssemblyLibLoaderCallLogger(lvl, message);
				return;
			}
			if ((lvl & (Logger.Level)StandardLogger.PrintFilter) != Logger.Level.None)
			{
				Console.WriteLine(string.Format("[{0}] {1}", lvl, message));
			}
		}

		// Token: 0x060001B2 RID: 434 RVA: 0x0000799D File Offset: 0x00005B9D
		internal static void Log(Logger.Level lvl, Exception message)
		{
			if (Logger.LogCreated)
			{
				LibLoader.AssemblyLibLoaderCallLogger(lvl, message);
				return;
			}
			if ((lvl & (Logger.Level)StandardLogger.PrintFilter) != Logger.Level.None)
			{
				Console.WriteLine(string.Format("[{0}] {1}", lvl, message));
			}
		}

		// Token: 0x060001B3 RID: 435 RVA: 0x000079CD File Offset: 0x00005BCD
		private static void AssemblyLibLoaderCallLogger(Logger.Level lvl, string message)
		{
			Logger.libLoader.Log(lvl, message);
		}

		// Token: 0x060001B4 RID: 436 RVA: 0x000079DB File Offset: 0x00005BDB
		private static void AssemblyLibLoaderCallLogger(Logger.Level lvl, Exception message)
		{
			Logger.libLoader.Log(lvl, message);
		}

		// Token: 0x060001B5 RID: 437 RVA: 0x000079E9 File Offset: 0x00005BE9
		private static IEnumerable<FileInfo> TraverseTree(string root, Func<string, bool> dirValidator = null)
		{
			if (dirValidator == null)
			{
				dirValidator = (string s) => true;
			}
			Stack<string> dirs = new Stack<string>(32);
			if (!Directory.Exists(root))
			{
				throw new ArgumentException();
			}
			dirs.Push(root);
			while (dirs.Count > 0)
			{
				string currentDir = dirs.Pop();
				string[] subDirs;
				try
				{
					subDirs = Directory.GetDirectories(currentDir);
				}
				catch (UnauthorizedAccessException)
				{
					continue;
				}
				catch (DirectoryNotFoundException)
				{
					continue;
				}
				string[] files;
				try
				{
					files = Directory.GetFiles(currentDir);
				}
				catch (UnauthorizedAccessException)
				{
					continue;
				}
				catch (DirectoryNotFoundException)
				{
					continue;
				}
				foreach (string str in subDirs)
				{
					if (dirValidator(str))
					{
						dirs.Push(str);
					}
				}
				string[] array2 = files;
				int j = 0;
				while (j < array2.Length)
				{
					string file = array2[j];
					FileInfo nextValue;
					try
					{
						nextValue = new FileInfo(file);
					}
					catch (FileNotFoundException)
					{
						goto IL_0132;
					}
					goto IL_011A;
					IL_0132:
					j++;
					continue;
					IL_011A:
					yield return nextValue;
					goto IL_0132;
				}
				array2 = null;
			}
			yield break;
		}

		// Token: 0x060001B6 RID: 438
		[DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
		private static extern IntPtr AddDllDirectory(string lpPathName);

		// Token: 0x060001B7 RID: 439
		[DllImport("kernel32.dll", SetLastError = true)]
		private static extern bool SetDefaultDllDirectories(LibLoader.LoadLibraryFlags dwFlags);

		// Token: 0x060001B8 RID: 440 RVA: 0x00007A00 File Offset: 0x00005C00
		[CompilerGenerated]
		internal static void <SetupAssemblyFilenames>g__AddDir|6_0(string path)
		{
			if (LibLoader.AddDllDirectory(path) == IntPtr.Zero)
			{
				Win32Exception err = new Win32Exception();
				LibLoader.Log(Logger.Level.Warning, "Could not add DLL directory " + path);
				LibLoader.Log(Logger.Level.Warning, err);
			}
		}

		// Token: 0x040000A1 RID: 161
		internal static Dictionary<string, string> FilenameLocations;

		// Token: 0x020000F8 RID: 248
		[Flags]
		private enum LoadLibraryFlags : uint
		{
			// Token: 0x04000356 RID: 854
			None = 0U,
			// Token: 0x04000357 RID: 855
			LOAD_LIBRARY_SEARCH_APPLICATION_DIR = 512U,
			// Token: 0x04000358 RID: 856
			LOAD_LIBRARY_SEARCH_DEFAULT_DIRS = 4096U,
			// Token: 0x04000359 RID: 857
			LOAD_LIBRARY_SEARCH_SYSTEM32 = 2048U,
			// Token: 0x0400035A RID: 858
			LOAD_LIBRARY_SEARCH_USER_DIRS = 1024U
		}
	}
}
