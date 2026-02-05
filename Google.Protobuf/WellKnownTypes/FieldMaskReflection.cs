using System;
using Google.Protobuf.Reflection;

namespace Google.Protobuf.WellKnownTypes
{
	// Token: 0x02000031 RID: 49
	public static class FieldMaskReflection
	{
		// Token: 0x17000066 RID: 102
		// (get) Token: 0x060002A2 RID: 674 RVA: 0x0000C481 File Offset: 0x0000A681
		public static FileDescriptor Descriptor
		{
			get
			{
				return FieldMaskReflection.descriptor;
			}
		}

		// Token: 0x040000B1 RID: 177
		private static FileDescriptor descriptor = FileDescriptor.FromGeneratedCode(Convert.FromBase64String(string.Concat(new string[] { "CiBnb29nbGUvcHJvdG9idWYvZmllbGRfbWFzay5wcm90bxIPZ29vZ2xlLnBy", "b3RvYnVmIhoKCUZpZWxkTWFzaxINCgVwYXRocxgBIAMoCUKMAQoTY29tLmdv", "b2dsZS5wcm90b2J1ZkIORmllbGRNYXNrUHJvdG9QAVo5Z29vZ2xlLmdvbGFu", "Zy5vcmcvZ2VucHJvdG8vcHJvdG9idWYvZmllbGRfbWFzaztmaWVsZF9tYXNr", "+AEBogIDR1BCqgIeR29vZ2xlLlByb3RvYnVmLldlbGxLbm93blR5cGVzYgZw", "cm90bzM=" })), new FileDescriptor[0], new GeneratedClrTypeInfo(null, null, new GeneratedClrTypeInfo[]
		{
			new GeneratedClrTypeInfo(typeof(FieldMask), FieldMask.Parser, new string[] { "Paths" }, null, null, null, null)
		}));
	}
}
