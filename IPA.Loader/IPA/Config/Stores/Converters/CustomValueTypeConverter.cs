using System;
using IPA.Config.Data;

namespace IPA.Config.Stores.Converters
{
	/// <summary>
	/// A <see cref="T:IPA.Config.Stores.ValueConverter`1" /> for custom value types, serialized identically to the reference types serialized with
	/// <see cref="M:IPA.Config.Stores.GeneratedStore.Generated``1(IPA.Config.Config,System.Boolean)" />.
	/// </summary>
	/// <typeparam name="T">the type of the value to convert</typeparam>
	// Token: 0x02000086 RID: 134
	public class CustomValueTypeConverter<T> : ValueConverter<T> where T : struct
	{
		/// <summary>
		/// Deserializes <paramref name="value" /> into a <typeparamref name="T" /> with the given <paramref name="parent" />.
		/// </summary>
		/// <param name="value">the <see cref="T:IPA.Config.Data.Value" /> to deserialize</param>
		/// <param name="parent">the parent object that will own the deserialized value</param>
		/// <returns>the deserialized value</returns>
		/// <seealso cref="M:IPA.Config.Stores.ValueConverter`1.FromValue(IPA.Config.Data.Value,System.Object)" />
		// Token: 0x0600037C RID: 892 RVA: 0x00012F2C File Offset: 0x0001112C
		public static T Deserialize(Value value, object parent)
		{
			return CustomValueTypeConverter<T>.deserialize(value, parent);
		}

		/// <summary>
		/// Serializes <paramref name="obj" /> into a corresponding <see cref="T:IPA.Config.Data.Value" /> structure.
		/// </summary>
		/// <param name="obj">the object to serialize</param>
		/// <returns>the <see cref="T:IPA.Config.Data.Value" /> tree that represents <paramref name="obj" /></returns>
		/// <seealso cref="M:IPA.Config.Stores.ValueConverter`1.ToValue(`0,System.Object)" />
		// Token: 0x0600037D RID: 893 RVA: 0x00012F3A File Offset: 0x0001113A
		public static Value Serialize(T obj)
		{
			return CustomValueTypeConverter<T>.serialize(obj);
		}

		/// <summary>
		/// Deserializes <paramref name="value" /> into a <typeparamref name="T" /> with the given <paramref name="parent" />.
		/// </summary>
		/// <param name="value">the <see cref="T:IPA.Config.Data.Value" /> to deserialize</param>
		/// <param name="parent">the parent object that will own the deserialized value</param>
		/// <returns>the deserialized value</returns>
		/// <seealso cref="M:IPA.Config.Stores.ValueConverter`1.FromValue(IPA.Config.Data.Value,System.Object)" />
		// Token: 0x0600037E RID: 894 RVA: 0x00012F47 File Offset: 0x00011147
		public override T FromValue(Value value, object parent)
		{
			return CustomValueTypeConverter<T>.Deserialize(value, parent);
		}

		/// <summary>
		/// Serializes <paramref name="obj" /> into a <see cref="T:IPA.Config.Data.Value" /> structure, given <paramref name="parent" />.
		/// </summary>
		/// <param name="obj">the object to serialize</param>
		/// <param name="parent">the parent object that owns <paramref name="obj" /></param>
		/// <returns>the <see cref="T:IPA.Config.Data.Value" /> tree that represents <paramref name="obj" /></returns>
		/// <seealso cref="M:IPA.Config.Stores.ValueConverter`1.ToValue(`0,System.Object)" />
		// Token: 0x0600037F RID: 895 RVA: 0x00012F50 File Offset: 0x00011150
		public override Value ToValue(T obj, object parent)
		{
			return CustomValueTypeConverter<T>.Serialize(obj);
		}

		// Token: 0x04000126 RID: 294
		private static readonly GeneratedStoreImpl.SerializeObject<T> serialize = GeneratedStoreImpl.GetSerializerDelegate<T>();

		// Token: 0x04000127 RID: 295
		private static readonly GeneratedStoreImpl.DeserializeObject<T> deserialize = GeneratedStoreImpl.GetDeserializerDelegate<T>();
	}
}
