using System;
using System.IO;

namespace IPA.Utilities
{
	/// <summary>
	/// A class providing various extension methods.
	/// </summary>
	// Token: 0x0200001E RID: 30
	public static class Extensions
	{
		/// <summary>
		/// Gets the default value for a given <see cref="T:System.Type" />.
		/// </summary>
		/// <param name="type">the <see cref="T:System.Type" /> to get the default value for</param>
		/// <returns>the default value of <paramref name="type" /></returns>
		// Token: 0x0600008C RID: 140 RVA: 0x0000398F File Offset: 0x00001B8F
		public static object GetDefault(this Type type)
		{
			if (!type.IsValueType)
			{
				return null;
			}
			return Activator.CreateInstance(type);
		}

		/// <summary>
		/// Unwraps a <see cref="T:System.Nullable`1" /> where T is <see cref="T:System.Boolean" /> such that if the value is null, it gives <see langword="false" />.
		/// </summary>
		/// <param name="self">the bool? to unwrap</param>
		/// <returns>the unwrapped value, or <see langword="false" /> if it was <see langword="null" /></returns>
		// Token: 0x0600008D RID: 141 RVA: 0x000039A1 File Offset: 0x00001BA1
		public static bool Unwrap(this bool? self)
		{
			return self != null && self.Value;
		}

		/// <summary>
		/// Returns true if <paramref name="path" /> starts with the path <paramref name="baseDirPath" />.
		/// The comparison is case-insensitive, handles / and \ slashes as folder separators and
		/// only matches if the base dir folder name is matched exactly ("c:\foobar\file.txt" is not a sub path of "c:\foo").
		/// </summary>
		// Token: 0x0600008E RID: 142 RVA: 0x000039B8 File Offset: 0x00001BB8
		public static bool IsSubPathOf(this string path, string baseDirPath)
		{
			string fullPath = Path.GetFullPath(path.Replace('/', '\\').WithEnding("\\"));
			string normalizedBaseDirPath = Path.GetFullPath(baseDirPath.Replace('/', '\\').WithEnding("\\"));
			return fullPath.StartsWith(normalizedBaseDirPath, StringComparison.OrdinalIgnoreCase);
		}

		/// <summary>
		/// Returns <paramref name="str" /> with the minimal concatenation of <paramref name="ending" /> (starting from end) that
		/// results in satisfying .EndsWith(ending).
		/// </summary>
		/// <example>"hel".WithEnding("llo") returns "hello", which is the result of "hel" + "lo".</example>
		// Token: 0x0600008F RID: 143 RVA: 0x00003A00 File Offset: 0x00001C00
		public static string WithEnding(this string str, string ending)
		{
			if (str == null)
			{
				return ending;
			}
			for (int i = 0; i <= ending.Length; i++)
			{
				string tmp = str + ending.Right(i);
				if (tmp.EndsWith(ending))
				{
					return tmp;
				}
			}
			return str;
		}

		/// <summary>Gets the rightmost <paramref name="length" /> characters from a string.</summary>
		/// <param name="value">The string to retrieve the substring from.</param>
		/// <param name="length">The number of characters to retrieve.</param>
		/// <returns>The substring.</returns>
		// Token: 0x06000090 RID: 144 RVA: 0x00003A40 File Offset: 0x00001C40
		public static string Right(this string value, int length)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			if (length < 0)
			{
				throw new ArgumentOutOfRangeException("length", length, "Length is less than zero");
			}
			if (length >= value.Length)
			{
				return value;
			}
			return value.Substring(value.Length - length);
		}
	}
}
