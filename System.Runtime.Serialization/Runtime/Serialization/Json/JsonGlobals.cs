using System;
using System.Xml;

namespace System.Runtime.Serialization.Json
{
	// Token: 0x0200010D RID: 269
	internal static class JsonGlobals
	{
		// Token: 0x040007F3 RID: 2035
		public static readonly int DataContractXsdBaseNamespaceLength = "http://schemas.datacontract.org/2004/07/".Length;

		// Token: 0x040007F4 RID: 2036
		public static readonly XmlDictionaryString dDictionaryString = new XmlDictionary().Add("d");

		// Token: 0x040007F5 RID: 2037
		public static readonly char[] floatingPointCharacters = new char[] { '.', 'e' };

		// Token: 0x040007F6 RID: 2038
		public static readonly XmlDictionaryString itemDictionaryString = new XmlDictionary().Add("item");

		// Token: 0x040007F7 RID: 2039
		public static readonly XmlDictionaryString rootDictionaryString = new XmlDictionary().Add("root");

		// Token: 0x040007F8 RID: 2040
		public static readonly long unixEpochTicks = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).Ticks;

		// Token: 0x040007F9 RID: 2041
		public const string applicationJsonMediaType = "application/json";

		// Token: 0x040007FA RID: 2042
		public const string arrayString = "array";

		// Token: 0x040007FB RID: 2043
		public const string booleanString = "boolean";

		// Token: 0x040007FC RID: 2044
		public const string CacheControlString = "Cache-Control";

		// Token: 0x040007FD RID: 2045
		public const byte CollectionByte = 91;

		// Token: 0x040007FE RID: 2046
		public const char CollectionChar = '[';

		// Token: 0x040007FF RID: 2047
		public const string DateTimeEndGuardReader = ")/";

		// Token: 0x04000800 RID: 2048
		public const string DateTimeEndGuardWriter = ")\\/";

		// Token: 0x04000801 RID: 2049
		public const string DateTimeStartGuardReader = "/Date(";

		// Token: 0x04000802 RID: 2050
		public const string DateTimeStartGuardWriter = "\\/Date(";

		// Token: 0x04000803 RID: 2051
		public const string dString = "d";

		// Token: 0x04000804 RID: 2052
		public const byte EndCollectionByte = 93;

		// Token: 0x04000805 RID: 2053
		public const char EndCollectionChar = ']';

		// Token: 0x04000806 RID: 2054
		public const byte EndObjectByte = 125;

		// Token: 0x04000807 RID: 2055
		public const char EndObjectChar = '}';

		// Token: 0x04000808 RID: 2056
		public const string ExpiresString = "Expires";

		// Token: 0x04000809 RID: 2057
		public const string IfModifiedSinceString = "If-Modified-Since";

		// Token: 0x0400080A RID: 2058
		public const string itemString = "item";

		// Token: 0x0400080B RID: 2059
		public const string jsonerrorString = "jsonerror";

		// Token: 0x0400080C RID: 2060
		public const string KeyString = "Key";

		// Token: 0x0400080D RID: 2061
		public const string LastModifiedString = "Last-Modified";

		// Token: 0x0400080E RID: 2062
		public const int maxScopeSize = 25;

		// Token: 0x0400080F RID: 2063
		public const byte MemberSeparatorByte = 44;

		// Token: 0x04000810 RID: 2064
		public const char MemberSeparatorChar = ',';

		// Token: 0x04000811 RID: 2065
		public const byte NameValueSeparatorByte = 58;

		// Token: 0x04000812 RID: 2066
		public const char NameValueSeparatorChar = ':';

		// Token: 0x04000813 RID: 2067
		public const string NameValueSeparatorString = ":";

		// Token: 0x04000814 RID: 2068
		public const string nullString = "null";

		// Token: 0x04000815 RID: 2069
		public const string numberString = "number";

		// Token: 0x04000816 RID: 2070
		public const byte ObjectByte = 123;

		// Token: 0x04000817 RID: 2071
		public const char ObjectChar = '{';

		// Token: 0x04000818 RID: 2072
		public const string objectString = "object";

		// Token: 0x04000819 RID: 2073
		public const string publicString = "public";

		// Token: 0x0400081A RID: 2074
		public const byte QuoteByte = 34;

		// Token: 0x0400081B RID: 2075
		public const char QuoteChar = '"';

		// Token: 0x0400081C RID: 2076
		public const string rootString = "root";

		// Token: 0x0400081D RID: 2077
		public const string serverTypeString = "__type";

		// Token: 0x0400081E RID: 2078
		public const string stringString = "string";

		// Token: 0x0400081F RID: 2079
		public const string textJsonMediaType = "text/json";

		// Token: 0x04000820 RID: 2080
		public const string trueString = "true";

		// Token: 0x04000821 RID: 2081
		public const string typeString = "type";

		// Token: 0x04000822 RID: 2082
		public const string ValueString = "Value";

		// Token: 0x04000823 RID: 2083
		public const char WhitespaceChar = ' ';

		// Token: 0x04000824 RID: 2084
		public const string xmlnsPrefix = "xmlns";

		// Token: 0x04000825 RID: 2085
		public const string xmlPrefix = "xml";
	}
}
