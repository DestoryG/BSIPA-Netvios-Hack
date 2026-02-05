using System;

namespace System.Runtime.Serialization
{
	// Token: 0x02000062 RID: 98
	public interface ISerializationSurrogateProvider
	{
		// Token: 0x06000702 RID: 1794
		Type GetSurrogateType(Type type);

		// Token: 0x06000703 RID: 1795
		object GetObjectToSerialize(object obj, Type targetType);

		// Token: 0x06000704 RID: 1796
		object GetDeserializedObject(object obj, Type targetType);
	}
}
