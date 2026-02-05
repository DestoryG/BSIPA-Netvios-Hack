using System;
using System.Globalization;

namespace System.Net
{
	// Token: 0x0200011A RID: 282
	internal static class ValidationHelper
	{
		// Token: 0x06000B21 RID: 2849 RVA: 0x0003D730 File Offset: 0x0003B930
		public static string[] MakeEmptyArrayNull(string[] stringArray)
		{
			if (stringArray == null || stringArray.Length == 0)
			{
				return null;
			}
			return stringArray;
		}

		// Token: 0x06000B22 RID: 2850 RVA: 0x0003D73C File Offset: 0x0003B93C
		public static string MakeStringNull(string stringValue)
		{
			if (stringValue == null || stringValue.Length == 0)
			{
				return null;
			}
			return stringValue;
		}

		// Token: 0x06000B23 RID: 2851 RVA: 0x0003D74C File Offset: 0x0003B94C
		public static string ExceptionMessage(Exception exception)
		{
			if (exception == null)
			{
				return string.Empty;
			}
			if (exception.InnerException == null)
			{
				return exception.Message;
			}
			return exception.Message + " (" + ValidationHelper.ExceptionMessage(exception.InnerException) + ")";
		}

		// Token: 0x06000B24 RID: 2852 RVA: 0x0003D788 File Offset: 0x0003B988
		public static string ToString(object objectValue)
		{
			if (objectValue == null)
			{
				return "(null)";
			}
			if (objectValue is string && ((string)objectValue).Length == 0)
			{
				return "(string.empty)";
			}
			if (objectValue is Exception)
			{
				return ValidationHelper.ExceptionMessage(objectValue as Exception);
			}
			if (objectValue is IntPtr)
			{
				return "0x" + ((IntPtr)objectValue).ToString("x");
			}
			return objectValue.ToString();
		}

		// Token: 0x06000B25 RID: 2853 RVA: 0x0003D7FC File Offset: 0x0003B9FC
		public static string HashString(object objectValue)
		{
			if (objectValue == null)
			{
				return "(null)";
			}
			if (objectValue is string && ((string)objectValue).Length == 0)
			{
				return "(string.empty)";
			}
			return objectValue.GetHashCode().ToString(NumberFormatInfo.InvariantInfo);
		}

		// Token: 0x06000B26 RID: 2854 RVA: 0x0003D840 File Offset: 0x0003BA40
		public static bool IsInvalidHttpString(string stringValue)
		{
			return stringValue.IndexOfAny(ValidationHelper.InvalidParamChars) != -1;
		}

		// Token: 0x06000B27 RID: 2855 RVA: 0x0003D853 File Offset: 0x0003BA53
		public static bool IsBlankString(string stringValue)
		{
			return stringValue == null || stringValue.Length == 0;
		}

		// Token: 0x06000B28 RID: 2856 RVA: 0x0003D863 File Offset: 0x0003BA63
		public static bool ValidateTcpPort(int port)
		{
			return port >= 0 && port <= 65535;
		}

		// Token: 0x06000B29 RID: 2857 RVA: 0x0003D876 File Offset: 0x0003BA76
		public static bool ValidateRange(int actual, int fromAllowed, int toAllowed)
		{
			return actual >= fromAllowed && actual <= toAllowed;
		}

		// Token: 0x06000B2A RID: 2858 RVA: 0x0003D888 File Offset: 0x0003BA88
		internal static void ValidateSegment(ArraySegment<byte> segment)
		{
			if (segment.Array == null)
			{
				throw new ArgumentNullException("segment");
			}
			if (segment.Offset < 0 || segment.Count < 0 || segment.Count > segment.Array.Length - segment.Offset)
			{
				throw new ArgumentOutOfRangeException("segment");
			}
		}

		// Token: 0x04000F76 RID: 3958
		public static string[] EmptyArray = new string[0];

		// Token: 0x04000F77 RID: 3959
		internal static readonly char[] InvalidMethodChars = new char[] { ' ', '\r', '\n', '\t' };

		// Token: 0x04000F78 RID: 3960
		internal static readonly char[] InvalidParamChars = new char[]
		{
			'(', ')', '<', '>', '@', ',', ';', ':', '\\', '"',
			'\'', '/', '[', ']', '?', '=', '{', '}', ' ', '\t',
			'\r', '\n'
		};
	}
}
