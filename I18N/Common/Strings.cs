using System;

namespace I18N.Common
{
	// Token: 0x0200000D RID: 13
	public sealed class Strings
	{
		// Token: 0x06000068 RID: 104 RVA: 0x00004788 File Offset: 0x00002988
		public static string GetString(string tag)
		{
			if (tag == "ArgRange_Array")
			{
				return "Argument index is out of array range.";
			}
			if (tag == "Arg_InsufficientSpace")
			{
				return "Insufficient space in the argument array.";
			}
			if (tag == "ArgRange_NonNegative")
			{
				return "Non-negative value is expected.";
			}
			if (tag == "NotSupp_MissingCodeTable")
			{
				return "This encoding is not supported. Code table is missing.";
			}
			if (tag == "ArgRange_StringIndex")
			{
				return "String index is out of range.";
			}
			if (!(tag == "ArgRange_StringRange"))
			{
				throw new ArgumentException(string.Format("Unexpected error tag name:  {0}", tag));
			}
			return "String length is out of range.";
		}
	}
}
