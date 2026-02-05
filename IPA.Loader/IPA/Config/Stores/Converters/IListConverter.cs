using System;
using System.Collections.Generic;

namespace IPA.Config.Stores.Converters
{
	/// <summary>
	/// A <see cref="T:IPA.Config.Stores.Converters.CollectionConverter`2" /> for an <see cref="T:System.Collections.Generic.IList`1" />, creating a <see cref="T:System.Collections.Generic.List`1" /> when deserializing.
	/// </summary>
	/// <typeparam name="T">the element type of the <see cref="T:System.Collections.Generic.IList`1" /></typeparam>
	/// <seealso cref="T:IPA.Config.Stores.Converters.CollectionConverter`2" />
	// Token: 0x0200008D RID: 141
	public class IListConverter<T> : CollectionConverter<T, IList<T>>
	{
		/// <summary>
		/// Creates an <see cref="T:IPA.Config.Stores.Converters.IListConverter`1" /> using the default converter for <typeparamref name="T" />.
		/// </summary>
		/// <seealso cref="M:IPA.Config.Stores.Converters.CollectionConverter`2.#ctor" />
		// Token: 0x06000392 RID: 914 RVA: 0x000130E4 File Offset: 0x000112E4
		public IListConverter()
		{
		}

		/// <summary>
		/// Creates an <see cref="T:IPA.Config.Stores.Converters.IListConverter`1" /> using the specified underlying converter for values.
		/// </summary>
		/// <param name="underlying">the underlying <see cref="T:IPA.Config.Stores.ValueConverter`1" /> to use for the values</param>
		// Token: 0x06000393 RID: 915 RVA: 0x000130EC File Offset: 0x000112EC
		public IListConverter(ValueConverter<T> underlying)
			: base(underlying)
		{
		}

		/// <summary>
		/// Creates a new <see cref="T:System.Collections.Generic.IList`1" /> (a <see cref="T:System.Collections.Generic.List`1" />) for deserialization.
		/// </summary>
		/// <param name="size">the size to initialize it to</param>
		/// <param name="parent">the object that will own the new object</param>
		/// <returns>the new <see cref="T:System.Collections.Generic.IList`1" /></returns>
		// Token: 0x06000394 RID: 916 RVA: 0x000130F5 File Offset: 0x000112F5
		protected override IList<T> Create(int size, object parent)
		{
			return new List<T>(size);
		}
	}
}
