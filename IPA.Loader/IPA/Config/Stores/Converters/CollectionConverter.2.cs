using System;
using System.Collections.Generic;

namespace IPA.Config.Stores.Converters
{
	/// <summary>
	/// A <see cref="T:IPA.Config.Stores.Converters.CollectionConverter`2" /> which default constructs a converter for use as the value converter.
	/// </summary>
	/// <typeparam name="T">the value type of the collection</typeparam>
	/// <typeparam name="TCollection">the type of the colleciton</typeparam>
	/// <typeparam name="TConverter">the type of the converter to use for <typeparamref name="T" /></typeparam>
	/// <seealso cref="T:IPA.Config.Stores.Converters.CollectionConverter`2" />
	// Token: 0x02000088 RID: 136
	public sealed class CollectionConverter<T, TCollection, TConverter> : CollectionConverter<T, TCollection> where TCollection : ICollection<T> where TConverter : ValueConverter<T>, new()
	{
		/// <summary>
		/// Creates a <see cref="T:IPA.Config.Stores.Converters.CollectionConverter`2" /> using a default constructed <typeparamref name="TConverter" />
		/// element type. Equivalent to calling <see cref="M:IPA.Config.Stores.Converters.CollectionConverter`2.#ctor(IPA.Config.Stores.ValueConverter{`0})" />
		/// with a default-constructed <typeparamref name="TConverter" />.
		/// </summary>
		/// <seealso cref="M:IPA.Config.Stores.Converters.CollectionConverter`2.#ctor(IPA.Config.Stores.ValueConverter{`0})" />
		// Token: 0x06000389 RID: 905 RVA: 0x0001307D File Offset: 0x0001127D
		public CollectionConverter()
			: base(new TConverter())
		{
		}
	}
}
