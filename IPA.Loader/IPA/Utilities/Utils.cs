using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using Mono.Cecil;
using Mono.Collections.Generic;
using SemVer;

namespace IPA.Utilities
{
	/// <summary>
	/// A class providing static utility functions that in any other language would just *exist*.
	/// </summary>
	// Token: 0x0200001F RID: 31
	public static class Utils
	{
		/// <summary>
		/// Converts a hex string to a byte array.
		/// </summary>
		/// <param name="hex">the hex stream</param>
		/// <returns>the corresponding byte array</returns>
		// Token: 0x06000091 RID: 145 RVA: 0x00003A90 File Offset: 0x00001C90
		public static byte[] StringToByteArray(string hex)
		{
			int numberChars = hex.Length;
			byte[] bytes = new byte[numberChars / 2];
			for (int i = 0; i < numberChars; i += 2)
			{
				bytes[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);
			}
			return bytes;
		}

		/// <summary>
		/// Converts a byte array to a hex string.
		/// </summary>
		/// <param name="ba">the byte array</param>
		/// <returns>the hex form of the array</returns>
		// Token: 0x06000092 RID: 146 RVA: 0x00003AD0 File Offset: 0x00001CD0
		public static string ByteArrayToString(byte[] ba)
		{
			StringBuilder hex = new StringBuilder(ba.Length * 2);
			foreach (byte b in ba)
			{
				hex.AppendFormat("{0:x2}", b);
			}
			return hex.ToString();
		}

		/// <summary>
		/// Uses unsafe code to compare 2 byte arrays quickly.
		/// </summary>
		/// <param name="a1">array 1</param>
		/// <param name="a2">array 2</param>
		/// <returns>whether or not they are byte-for-byte equal</returns>
		// Token: 0x06000093 RID: 147 RVA: 0x00003B14 File Offset: 0x00001D14
		public unsafe static bool UnsafeCompare(byte[] a1, byte[] a2)
		{
			if (a1 == a2)
			{
				return true;
			}
			if (a1 == null || a2 == null || a1.Length != a2.Length)
			{
				return false;
			}
			byte* p;
			if (a1 == null || a1.Length == 0)
			{
				p = null;
			}
			else
			{
				p = &a1[0];
			}
			byte* p2;
			if (a2 == null || a2.Length == 0)
			{
				p2 = null;
			}
			else
			{
				p2 = &a2[0];
			}
			byte* x = p;
			byte* x2 = p2;
			int i = a1.Length;
			int j = 0;
			while (j < i / 8)
			{
				if (*(long*)x != *(long*)x2)
				{
					return false;
				}
				j++;
				x += 8;
				x2 += 8;
			}
			if ((i & 4) != 0)
			{
				if (*(int*)x != *(int*)x2)
				{
					return false;
				}
				x += 4;
				x2 += 4;
			}
			if ((i & 2) != 0)
			{
				if (*(short*)x != *(short*)x2)
				{
					return false;
				}
				x += 2;
				x2 += 2;
			}
			return (i & 1) == 0 || *x == *x2;
		}

		/// <summary>
		/// Gets a path relative to the provided folder.
		/// </summary>
		/// <param name="file">the file to relativize</param>
		/// <param name="folder">the source folder</param>
		/// <returns>a path to get from <paramref name="folder" /> to <paramref name="file" /></returns>
		// Token: 0x06000094 RID: 148 RVA: 0x00003BE4 File Offset: 0x00001DE4
		public static string GetRelativePath(string file, string folder)
		{
			Uri pathUri = new Uri(file);
			if (!folder.EndsWith(Path.DirectorySeparatorChar.ToString()))
			{
				folder += Path.DirectorySeparatorChar.ToString();
			}
			return Uri.UnescapeDataString(new Uri(folder).MakeRelativeUri(pathUri).ToString().Replace('/', Path.DirectorySeparatorChar));
		}

		/// <summary>
		/// Copies all files from <paramref name="source" /> to <paramref name="target" />.
		/// </summary>
		/// <param name="source">the source directory</param>
		/// <param name="target">the destination directory</param>
		/// <param name="appendFileName">the filename of the file to append together</param>
		/// <param name="onCopyException">a delegate called when there is an error copying. Return true to keep going.</param>
		// Token: 0x06000095 RID: 149 RVA: 0x00003C44 File Offset: 0x00001E44
		public static void CopyAll(DirectoryInfo source, DirectoryInfo target, string appendFileName = "", Func<Exception, FileInfo, bool> onCopyException = null)
		{
			if (source.FullName.ToLower() == target.FullName.ToLower())
			{
				return;
			}
			if (!Directory.Exists(target.FullName))
			{
				Directory.CreateDirectory(target.FullName);
			}
			foreach (FileInfo fi in source.GetFiles())
			{
				try
				{
					if (fi.Name == appendFileName)
					{
						File.AppendAllLines(Path.Combine(target.ToString(), fi.Name), File.ReadAllLines(fi.FullName));
					}
					else
					{
						fi.CopyTo(Path.Combine(target.ToString(), fi.Name), true);
					}
				}
				catch (Exception e)
				{
					if (!((onCopyException != null) ? new bool?(onCopyException(e, fi)) : null).Unwrap())
					{
						throw;
					}
				}
			}
			foreach (DirectoryInfo diSourceSubDir in source.GetDirectories())
			{
				DirectoryInfo nextTargetSubDir = target.CreateSubdirectory(diSourceSubDir.Name);
				Utils.CopyAll(diSourceSubDir, nextTargetSubDir, appendFileName, onCopyException);
			}
		}

		/// <summary>
		/// Whether you can safely use <see cref="P:System.DateTime.Now" /> without Mono throwing a fit.
		/// </summary>
		/// <value><see langword="true" /> if you can use <see cref="P:System.DateTime.Now" /> safely, <see langword="false" /> otherwise</value>
		// Token: 0x17000017 RID: 23
		// (get) Token: 0x06000096 RID: 150 RVA: 0x00003D60 File Offset: 0x00001F60
		// (set) Token: 0x06000097 RID: 151 RVA: 0x00003D67 File Offset: 0x00001F67
		public static bool CanUseDateTimeNowSafely { get; private set; } = true;

		/// <summary>
		/// Returns <see cref="P:System.DateTime.Now" /> if supported, otherwise <see cref="P:System.DateTime.UtcNow" />.
		/// </summary>
		/// <returns>the current <see cref="T:System.DateTime" /> if supported, otherwise some indeterminant increasing value.</returns>
		// Token: 0x06000098 RID: 152 RVA: 0x00003D70 File Offset: 0x00001F70
		public static DateTime CurrentTime()
		{
			if (Utils.DateTimeSafetyUnknown)
			{
				DateTime time = DateTime.MinValue;
				try
				{
					time = DateTime.Now;
				}
				catch (TimeZoneNotFoundException)
				{
					Utils.CanUseDateTimeNowSafely = false;
				}
				Utils.DateTimeSafetyUnknown = false;
				return time;
			}
			if (Utils.CanUseDateTimeNowSafely)
			{
				return DateTime.Now;
			}
			return DateTime.UtcNow;
		}

		/// <summary>
		/// Compares a pair of <see cref="T:SemVer.Version" />s ignoring both the prerelease and build fields.
		/// </summary>
		/// <param name="l">the left value</param>
		/// <param name="r">the right value</param>
		/// <returns>&lt; 0 if l is less than r, 0 if they are equal in the numeric portion, or &gt; 0 if l is greater than r</returns>
		// Token: 0x06000099 RID: 153 RVA: 0x00003DC8 File Offset: 0x00001FC8
		public static int VersionCompareNoPrerelease(global::SemVer.Version l, global::SemVer.Version r)
		{
			int cmpVal = l.Major - r.Major;
			if (cmpVal != 0)
			{
				return cmpVal;
			}
			cmpVal = l.Minor - r.Minor;
			if (cmpVal != 0)
			{
				return cmpVal;
			}
			return l.Patch - r.Patch;
		}

		/// <summary>
		/// Creates a scope guard for a given <see cref="T:System.Action" />.
		/// </summary>
		/// <param name="action">the <see cref="T:System.Action" /> to run on dispose</param>
		/// <returns>a <see cref="T:IPA.Utilities.Utils.ScopeGuardObject" /> that will run <paramref name="action" /> on disposal</returns>
		/// <example>
		/// <code>
		/// using var _ = Utils.ScopeGuard(() =&gt; RunOnScopeExit(value));
		/// </code>
		/// </example>
		// Token: 0x0600009A RID: 154 RVA: 0x00003E0A File Offset: 0x0000200A
		public static Utils.ScopeGuardObject ScopeGuard(Action action)
		{
			return new Utils.ScopeGuardObject(action);
		}

		// Token: 0x0600009B RID: 155 RVA: 0x00003E14 File Offset: 0x00002014
		internal static bool HasInterface(this TypeDefinition type, string interfaceFullName)
		{
			bool? flag;
			if (type == null)
			{
				flag = null;
			}
			else
			{
				Collection<InterfaceImplementation> interfaces = type.Interfaces;
				flag = ((interfaces != null) ? new bool?(interfaces.Any((InterfaceImplementation i) => i.InterfaceType.FullName == interfaceFullName)) : null);
			}
			bool? flag2 = flag;
			if (!flag2.GetValueOrDefault())
			{
				bool? flag3;
				if (type == null)
				{
					flag3 = null;
				}
				else
				{
					Collection<InterfaceImplementation> interfaces2 = type.Interfaces;
					flag3 = ((interfaces2 != null) ? new bool?(interfaces2.Any(delegate(InterfaceImplementation t)
					{
						TypeDefinition typeDefinition;
						if (t == null)
						{
							typeDefinition = null;
						}
						else
						{
							TypeReference interfaceType = t.InterfaceType;
							typeDefinition = ((interfaceType != null) ? interfaceType.Resolve() : null);
						}
						return typeDefinition.HasInterface(interfaceFullName);
					})) : null);
				}
				flag2 = flag3;
				return flag2.GetValueOrDefault();
			}
			return true;
		}

		// Token: 0x0600009C RID: 156 RVA: 0x00003EB4 File Offset: 0x000020B4
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal static IEnumerable<string> StrJP(this IEnumerable<string> a)
		{
			return a;
		}

		// Token: 0x0600009D RID: 157 RVA: 0x00003EB7 File Offset: 0x000020B7
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal static IEnumerable<string> StrJP<T>(this IEnumerable<T> a)
		{
			return a.Select((T o) => string.Format("{0}", o));
		}

		// Token: 0x0400002D RID: 45
		private static bool DateTimeSafetyUnknown = true;

		/// <summary>
		/// An object used to manage scope guards.
		/// </summary>
		/// <example>
		/// <code>
		/// using var _ = new Utils.ScopeGuardObject(() =&gt; RunOnScopeExit(value));
		/// </code>
		/// </example>
		/// <seealso cref="M:IPA.Utilities.Utils.ScopeGuard(System.Action)" />
		// Token: 0x020000C6 RID: 198
		public struct ScopeGuardObject : IDisposable
		{
			/// <summary>
			/// Creates a new scope guard that will invoke <paramref name="action" /> when disposed.
			/// </summary>
			/// <param name="action">the action to run on dispose</param>
			// Token: 0x060004A8 RID: 1192 RVA: 0x00015D89 File Offset: 0x00013F89
			public ScopeGuardObject(Action action)
			{
				this.action = action;
			}

			// Token: 0x060004A9 RID: 1193 RVA: 0x00015D92 File Offset: 0x00013F92
			void IDisposable.Dispose()
			{
				Action action = this.action;
				if (action == null)
				{
					return;
				}
				action();
			}

			// Token: 0x040001B3 RID: 435
			private readonly Action action;
		}
	}
}
