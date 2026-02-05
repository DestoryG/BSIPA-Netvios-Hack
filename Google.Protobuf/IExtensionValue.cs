using System;

namespace Google.Protobuf
{
	// Token: 0x0200000C RID: 12
	internal interface IExtensionValue : IEquatable<IExtensionValue>, IDeepCloneable<IExtensionValue>
	{
		// Token: 0x060000E0 RID: 224
		void MergeFrom(CodedInputStream input);

		// Token: 0x060000E1 RID: 225
		void MergeFrom(IExtensionValue value);

		// Token: 0x060000E2 RID: 226
		void WriteTo(CodedOutputStream output);

		// Token: 0x060000E3 RID: 227
		int CalculateSize();

		// Token: 0x060000E4 RID: 228
		bool IsInitialized();
	}
}
