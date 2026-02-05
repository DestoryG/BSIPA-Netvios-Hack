using System;
using Google.Protobuf.Reflection;

namespace Google.Protobuf.WellKnownTypes
{
	// Token: 0x02000044 RID: 68
	public static class WrappersReflection
	{
		// Token: 0x170000B3 RID: 179
		// (get) Token: 0x060003C1 RID: 961 RVA: 0x0000FF7C File Offset: 0x0000E17C
		public static FileDescriptor Descriptor
		{
			get
			{
				return WrappersReflection.descriptor;
			}
		}

		// Token: 0x0400012A RID: 298
		private static FileDescriptor descriptor = FileDescriptor.FromGeneratedCode(Convert.FromBase64String(string.Concat(new string[] { "Ch5nb29nbGUvcHJvdG9idWYvd3JhcHBlcnMucHJvdG8SD2dvb2dsZS5wcm90", "b2J1ZiIcCgtEb3VibGVWYWx1ZRINCgV2YWx1ZRgBIAEoASIbCgpGbG9hdFZh", "bHVlEg0KBXZhbHVlGAEgASgCIhsKCkludDY0VmFsdWUSDQoFdmFsdWUYASAB", "KAMiHAoLVUludDY0VmFsdWUSDQoFdmFsdWUYASABKAQiGwoKSW50MzJWYWx1", "ZRINCgV2YWx1ZRgBIAEoBSIcCgtVSW50MzJWYWx1ZRINCgV2YWx1ZRgBIAEo", "DSIaCglCb29sVmFsdWUSDQoFdmFsdWUYASABKAgiHAoLU3RyaW5nVmFsdWUS", "DQoFdmFsdWUYASABKAkiGwoKQnl0ZXNWYWx1ZRINCgV2YWx1ZRgBIAEoDEJ8", "ChNjb20uZ29vZ2xlLnByb3RvYnVmQg1XcmFwcGVyc1Byb3RvUAFaKmdpdGh1", "Yi5jb20vZ29sYW5nL3Byb3RvYnVmL3B0eXBlcy93cmFwcGVyc/gBAaICA0dQ", "QqoCHkdvb2dsZS5Qcm90b2J1Zi5XZWxsS25vd25UeXBlc2IGcHJvdG8z" })), new FileDescriptor[0], new GeneratedClrTypeInfo(null, null, new GeneratedClrTypeInfo[]
		{
			new GeneratedClrTypeInfo(typeof(DoubleValue), DoubleValue.Parser, new string[] { "Value" }, null, null, null, null),
			new GeneratedClrTypeInfo(typeof(FloatValue), FloatValue.Parser, new string[] { "Value" }, null, null, null, null),
			new GeneratedClrTypeInfo(typeof(Int64Value), Int64Value.Parser, new string[] { "Value" }, null, null, null, null),
			new GeneratedClrTypeInfo(typeof(UInt64Value), UInt64Value.Parser, new string[] { "Value" }, null, null, null, null),
			new GeneratedClrTypeInfo(typeof(Int32Value), Int32Value.Parser, new string[] { "Value" }, null, null, null, null),
			new GeneratedClrTypeInfo(typeof(UInt32Value), UInt32Value.Parser, new string[] { "Value" }, null, null, null, null),
			new GeneratedClrTypeInfo(typeof(BoolValue), BoolValue.Parser, new string[] { "Value" }, null, null, null, null),
			new GeneratedClrTypeInfo(typeof(StringValue), StringValue.Parser, new string[] { "Value" }, null, null, null, null),
			new GeneratedClrTypeInfo(typeof(BytesValue), BytesValue.Parser, new string[] { "Value" }, null, null, null, null)
		}));

		// Token: 0x0400012B RID: 299
		internal const int WrapperValueFieldNumber = 1;
	}
}
