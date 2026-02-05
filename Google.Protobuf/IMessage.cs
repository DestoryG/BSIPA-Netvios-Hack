using System;
using Google.Protobuf.Reflection;

namespace Google.Protobuf
{
	// Token: 0x02000016 RID: 22
	public interface IMessage
	{
		// Token: 0x06000146 RID: 326
		void MergeFrom(CodedInputStream input);

		// Token: 0x06000147 RID: 327
		void WriteTo(CodedOutputStream output);

		// Token: 0x06000148 RID: 328
		int CalculateSize();

		// Token: 0x17000025 RID: 37
		// (get) Token: 0x06000149 RID: 329
		MessageDescriptor Descriptor { get; }
	}
}
