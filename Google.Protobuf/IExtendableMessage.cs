using System;
using Google.Protobuf.Collections;

namespace Google.Protobuf
{
	// Token: 0x02000015 RID: 21
	public interface IExtendableMessage<T> : IMessage<T>, IMessage, IEquatable<T>, IDeepCloneable<T> where T : IExtendableMessage<T>
	{
		// Token: 0x0600013F RID: 319
		TValue GetExtension<TValue>(Extension<T, TValue> extension);

		// Token: 0x06000140 RID: 320
		RepeatedField<TValue> GetExtension<TValue>(RepeatedExtension<T, TValue> extension);

		// Token: 0x06000141 RID: 321
		RepeatedField<TValue> GetOrInitializeExtension<TValue>(RepeatedExtension<T, TValue> extension);

		// Token: 0x06000142 RID: 322
		void SetExtension<TValue>(Extension<T, TValue> extension, TValue value);

		// Token: 0x06000143 RID: 323
		bool HasExtension<TValue>(Extension<T, TValue> extension);

		// Token: 0x06000144 RID: 324
		void ClearExtension<TValue>(Extension<T, TValue> extension);

		// Token: 0x06000145 RID: 325
		void ClearExtension<TValue>(RepeatedExtension<T, TValue> extension);
	}
}
