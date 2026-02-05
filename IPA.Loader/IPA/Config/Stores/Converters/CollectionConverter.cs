using System;
using System.Collections.Generic;
using System.Linq;
using IPA.Config.Data;

namespace IPA.Config.Stores.Converters
{
	/// <summary>
	/// A base class for all <see cref="T:System.Collections.Generic.ICollection`1" /> type converters, providing most of the functionality.
	/// </summary>
	/// <typeparam name="T">the type of the items in the collection</typeparam>
	/// <typeparam name="TCollection">the instantiated type of collection</typeparam>
	// Token: 0x02000087 RID: 135
	public class CollectionConverter<T, TCollection> : ValueConverter<TCollection> where TCollection : ICollection<T>
	{
		/// <summary>
		/// Creates a <see cref="T:IPA.Config.Stores.Converters.CollectionConverter`2" /> using the default converter for the
		/// element type. Equivalent to calling <see cref="M:IPA.Config.Stores.Converters.CollectionConverter`2.#ctor(IPA.Config.Stores.ValueConverter{`0})" />
		/// with <see cref="P:IPA.Config.Stores.Converters.Converter`1.Default" />.
		/// </summary>
		/// <seealso cref="M:IPA.Config.Stores.Converters.CollectionConverter`2.#ctor(IPA.Config.Stores.ValueConverter{`0})" />
		// Token: 0x06000382 RID: 898 RVA: 0x00012F76 File Offset: 0x00011176
		public CollectionConverter()
			: this(Converter<T>.Default)
		{
		}

		/// <summary>
		/// Creates a <see cref="T:IPA.Config.Stores.Converters.CollectionConverter`2" /> using the specified underlying converter.
		/// </summary>
		/// <param name="underlying">the <see cref="T:IPA.Config.Stores.ValueConverter`1" /> to use to convert the values</param>
		// Token: 0x06000383 RID: 899 RVA: 0x00012F83 File Offset: 0x00011183
		public CollectionConverter(ValueConverter<T> underlying)
		{
			this.BaseConverter = underlying;
		}

		/// <summary>
		/// Gets the converter for the collection's value type.
		/// </summary>
		// Token: 0x1700009B RID: 155
		// (get) Token: 0x06000384 RID: 900 RVA: 0x00012F92 File Offset: 0x00011192
		protected ValueConverter<T> BaseConverter { get; }

		/// <summary>
		/// Creates a collection of type <typeparamref name="TCollection" /> using the <paramref name="size" /> and
		/// <paramref name="parent" />.
		/// </summary>
		/// <param name="size">the initial size of the collecion</param>
		/// <param name="parent">the object that will own the new collection</param>
		/// <returns>a new instance of <typeparamref name="TCollection" /></returns>
		/// <seealso cref="M:IPA.Config.Stores.ValueConverter`1.FromValue(IPA.Config.Data.Value,System.Object)" />
		// Token: 0x06000385 RID: 901 RVA: 0x00012F9A File Offset: 0x0001119A
		protected virtual TCollection Create(int size, object parent)
		{
			return Activator.CreateInstance<TCollection>();
		}

		/// <summary>
		/// Populates the colleciton <paramref name="col" /> with the deserialized values from <paramref name="list" />
		/// with the parent <paramref name="parent" />.
		/// </summary>
		/// <param name="col">the collection to populate</param>
		/// <param name="list">the values to populate it with</param>
		/// <param name="parent">the object that will own the new objects</param>
		/// <seealso cref="M:IPA.Config.Stores.ValueConverter`1.FromValue(IPA.Config.Data.Value,System.Object)" />
		// Token: 0x06000386 RID: 902 RVA: 0x00012FA4 File Offset: 0x000111A4
		protected void PopulateFromValue(TCollection col, List list, object parent)
		{
			foreach (Value it in list)
			{
				col.Add(this.BaseConverter.FromValue(it, parent));
			}
		}

		/// <summary>
		/// Deserializes a <see cref="T:IPA.Config.Data.List" /> in <paramref name="value" /> into a new <typeparamref name="TCollection" />
		/// owned by <paramref name="parent" />.
		/// </summary>
		/// <param name="value">the <see cref="T:IPA.Config.Data.List" /> to convert to a <typeparamref name="TCollection" /></param>
		/// <param name="parent">the object that will own the resulting <typeparamref name="TCollection" /></param>
		/// <returns>a new <typeparamref name="TCollection" /> holding the deserialized content of <paramref name="value" /></returns>
		/// <seealso cref="M:IPA.Config.Stores.ValueConverter`1.FromValue(IPA.Config.Data.Value,System.Object)" />
		// Token: 0x06000387 RID: 903 RVA: 0x00013000 File Offset: 0x00011200
		public override TCollection FromValue(Value value, object parent)
		{
			List list = value as List;
			if (list == null)
			{
				throw new ArgumentException("Argument not a List", "value");
			}
			TCollection col = this.Create(list.Count, parent);
			this.PopulateFromValue(col, list, parent);
			return col;
		}

		/// <summary>
		/// Serializes a <typeparamref name="TCollection" /> into a <see cref="T:IPA.Config.Data.List" />.
		/// </summary>
		/// <param name="obj">the <typeparamref name="TCollection" /> to serialize</param>
		/// <param name="parent">the object owning <paramref name="obj" /></param>
		/// <returns>the <see cref="T:IPA.Config.Data.List" /> that <paramref name="obj" /> was serialized into</returns>
		/// <seealso cref="M:IPA.Config.Stores.ValueConverter`1.ToValue(`0,System.Object)" />
		// Token: 0x06000388 RID: 904 RVA: 0x00013040 File Offset: 0x00011240
		public override Value ToValue(TCollection obj, object parent)
		{
			return Value.From(obj.Select((T t) => this.BaseConverter.ToValue(t, parent)));
		}
	}
}
