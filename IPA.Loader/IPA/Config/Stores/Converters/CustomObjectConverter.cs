using System;
using IPA.Config.Data;

namespace IPA.Config.Stores.Converters
{
	/// <summary>
	/// A <see cref="T:IPA.Config.Stores.ValueConverter`1" /> for objects normally serialized to config via <see cref="M:IPA.Config.Stores.GeneratedStore.Generated``1(IPA.Config.Config,System.Boolean)" />.
	/// </summary>
	/// <typeparam name="T">the same type parameter that would be passed into <see cref="M:IPA.Config.Stores.GeneratedStore.Generated``1(IPA.Config.Config,System.Boolean)" /></typeparam>
	/// <seealso cref="M:IPA.Config.Stores.GeneratedStore.Generated``1(IPA.Config.Config,System.Boolean)" />
	// Token: 0x02000085 RID: 133
	public class CustomObjectConverter<T> : ValueConverter<T> where T : class
	{
		/// <summary>
		/// Deserializes <paramref name="value" /> into a <typeparamref name="T" /> with the given <paramref name="parent" />.
		/// </summary>
		/// <param name="value">the <see cref="T:IPA.Config.Data.Value" /> to deserialize</param>
		/// <param name="parent">the parent object that will own the deserialized value</param>
		/// <returns>the deserialized value</returns>
		/// <seealso cref="M:IPA.Config.Stores.ValueConverter`1.FromValue(IPA.Config.Data.Value,System.Object)" />
		// Token: 0x06000376 RID: 886 RVA: 0x00012EA3 File Offset: 0x000110A3
		public static T Deserialize(Value value, object parent)
		{
			return CustomObjectConverter<T>.impl.FromValue(value, parent);
		}

		/// <summary>
		/// Serializes <paramref name="obj" /> into a <see cref="T:IPA.Config.Data.Value" /> structure, given <paramref name="parent" />.
		/// </summary>
		/// <param name="obj">the object to serialize</param>
		/// <param name="parent">the parent object that owns <paramref name="obj" /></param>
		/// <returns>the <see cref="T:IPA.Config.Data.Value" /> tree that represents <paramref name="obj" /></returns>
		/// <seealso cref="M:IPA.Config.Stores.ValueConverter`1.ToValue(`0,System.Object)" />
		// Token: 0x06000377 RID: 887 RVA: 0x00012EB1 File Offset: 0x000110B1
		public static Value Serialize(T obj, object parent)
		{
			return CustomObjectConverter<T>.impl.ToValue(obj, parent);
		}

		/// <summary>
		/// Deserializes <paramref name="value" /> into a <typeparamref name="T" /> with the given <paramref name="parent" />.
		/// </summary>
		/// <param name="value">the <see cref="T:IPA.Config.Data.Value" /> to deserialize</param>
		/// <param name="parent">the parent object that will own the deserialized value</param>
		/// <returns>the deserialized value</returns>
		/// <seealso cref="M:IPA.Config.Stores.ValueConverter`1.FromValue(IPA.Config.Data.Value,System.Object)" />
		// Token: 0x06000378 RID: 888 RVA: 0x00012EBF File Offset: 0x000110BF
		public override T FromValue(Value value, object parent)
		{
			return CustomObjectConverter<T>.Deserialize(value, parent);
		}

		/// <summary>
		/// Serializes <paramref name="obj" /> into a <see cref="T:IPA.Config.Data.Value" /> structure, given <paramref name="parent" />.
		/// </summary>
		/// <param name="obj">the object to serialize</param>
		/// <param name="parent">the parent object that owns <paramref name="obj" /></param>
		/// <returns>the <see cref="T:IPA.Config.Data.Value" /> tree that represents <paramref name="obj" /></returns>
		/// <seealso cref="M:IPA.Config.Stores.ValueConverter`1.ToValue(`0,System.Object)" />
		// Token: 0x06000379 RID: 889 RVA: 0x00012EC8 File Offset: 0x000110C8
		public override Value ToValue(T obj, object parent)
		{
			return CustomObjectConverter<T>.Serialize(obj, parent);
		}

		// Token: 0x04000125 RID: 293
		private static readonly CustomObjectConverter<T>.IImpl impl = (CustomObjectConverter<T>.IImpl)Activator.CreateInstance(typeof(CustomObjectConverter<>.Impl<>).MakeGenericType(new Type[]
		{
			typeof(T),
			GeneratedStoreImpl.GetGeneratedType(typeof(T))
		}));

		// Token: 0x02000154 RID: 340
		private interface IImpl
		{
			// Token: 0x06000696 RID: 1686
			T FromValue(Value value, object parent);

			// Token: 0x06000697 RID: 1687
			Value ToValue(T obj, object parent);
		}

		// Token: 0x02000155 RID: 341
		private class Impl<U> : CustomObjectConverter<T>.IImpl where U : class, GeneratedStoreImpl.IGeneratedStore<T>, T
		{
			// Token: 0x06000698 RID: 1688 RVA: 0x000185C3 File Offset: 0x000167C3
			private static U Create(GeneratedStoreImpl.IGeneratedStore parent)
			{
				return CustomObjectConverter<T>.Impl<U>.creator(parent) as U;
			}

			// Token: 0x06000699 RID: 1689 RVA: 0x000185DA File Offset: 0x000167DA
			public T FromValue(Value value, object parent)
			{
				U u = CustomObjectConverter<T>.Impl<U>.Create(parent as GeneratedStoreImpl.IGeneratedStore);
				u.Deserialize(value);
				return (T)((object)u);
			}

			// Token: 0x0600069A RID: 1690 RVA: 0x00018600 File Offset: 0x00016800
			public Value ToValue(T obj, object parent)
			{
				GeneratedStoreImpl.IGeneratedStore store = obj as GeneratedStoreImpl.IGeneratedStore;
				if (store != null)
				{
					return store.Serialize();
				}
				U u = CustomObjectConverter<T>.Impl<U>.Create(null);
				u.CopyFrom(obj, false);
				return u.Serialize();
			}

			// Token: 0x04000451 RID: 1105
			private static readonly GeneratedStoreImpl.GeneratedStoreCreator creator = GeneratedStoreImpl.GetCreator(typeof(T));
		}
	}
}
