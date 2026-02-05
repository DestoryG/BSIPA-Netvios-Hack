using System;
using Google.Protobuf.Reflection;

namespace Google.Protobuf.WellKnownTypes
{
	// Token: 0x02000035 RID: 53
	public static class StructReflection
	{
		// Token: 0x17000070 RID: 112
		// (get) Token: 0x060002D8 RID: 728 RVA: 0x0000CE32 File Offset: 0x0000B032
		public static FileDescriptor Descriptor
		{
			get
			{
				return StructReflection.descriptor;
			}
		}

		// Token: 0x040000BE RID: 190
		private static FileDescriptor descriptor = FileDescriptor.FromGeneratedCode(Convert.FromBase64String(string.Concat(new string[]
		{
			"Chxnb29nbGUvcHJvdG9idWYvc3RydWN0LnByb3RvEg9nb29nbGUucHJvdG9i", "dWYihAEKBlN0cnVjdBIzCgZmaWVsZHMYASADKAsyIy5nb29nbGUucHJvdG9i", "dWYuU3RydWN0LkZpZWxkc0VudHJ5GkUKC0ZpZWxkc0VudHJ5EgsKA2tleRgB", "IAEoCRIlCgV2YWx1ZRgCIAEoCzIWLmdvb2dsZS5wcm90b2J1Zi5WYWx1ZToC", "OAEi6gEKBVZhbHVlEjAKCm51bGxfdmFsdWUYASABKA4yGi5nb29nbGUucHJv", "dG9idWYuTnVsbFZhbHVlSAASFgoMbnVtYmVyX3ZhbHVlGAIgASgBSAASFgoM", "c3RyaW5nX3ZhbHVlGAMgASgJSAASFAoKYm9vbF92YWx1ZRgEIAEoCEgAEi8K", "DHN0cnVjdF92YWx1ZRgFIAEoCzIXLmdvb2dsZS5wcm90b2J1Zi5TdHJ1Y3RI", "ABIwCgpsaXN0X3ZhbHVlGAYgASgLMhouZ29vZ2xlLnByb3RvYnVmLkxpc3RW", "YWx1ZUgAQgYKBGtpbmQiMwoJTGlzdFZhbHVlEiYKBnZhbHVlcxgBIAMoCzIW",
			"Lmdvb2dsZS5wcm90b2J1Zi5WYWx1ZSobCglOdWxsVmFsdWUSDgoKTlVMTF9W", "QUxVRRAAQoEBChNjb20uZ29vZ2xlLnByb3RvYnVmQgtTdHJ1Y3RQcm90b1AB", "WjFnaXRodWIuY29tL2dvbGFuZy9wcm90b2J1Zi9wdHlwZXMvc3RydWN0O3N0", "cnVjdHBi+AEBogIDR1BCqgIeR29vZ2xlLlByb3RvYnVmLldlbGxLbm93blR5", "cGVzYgZwcm90bzM="
		})), new FileDescriptor[0], new GeneratedClrTypeInfo(new Type[] { typeof(NullValue) }, null, new GeneratedClrTypeInfo[]
		{
			new GeneratedClrTypeInfo(typeof(Struct), Struct.Parser, new string[] { "Fields" }, null, null, null, new GeneratedClrTypeInfo[1]),
			new GeneratedClrTypeInfo(typeof(Value), Value.Parser, new string[] { "NullValue", "NumberValue", "StringValue", "BoolValue", "StructValue", "ListValue" }, new string[] { "Kind" }, null, null, null),
			new GeneratedClrTypeInfo(typeof(ListValue), ListValue.Parser, new string[] { "Values" }, null, null, null, null)
		}));
	}
}
