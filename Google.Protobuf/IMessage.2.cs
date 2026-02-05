using System;

namespace Google.Protobuf
{
	// Token: 0x02000017 RID: 23
	public interface IMessage<T> : IMessage, IEquatable<T>, IDeepCloneable<T> where T : IMessage<T>
	{
		// Token: 0x0600014A RID: 330
		void MergeFrom(T message);
	}
}
