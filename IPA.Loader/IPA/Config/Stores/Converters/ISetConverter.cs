using System;
using System.Collections.Generic;

namespace IPA.Config.Stores.Converters
{
	/// <summary>
	/// A <see cref="T:IPA.Config.Stores.Converters.CollectionConverter`2" /> for an <see cref="T:System.Collections.Generic.ISet`1" />, creating a <see cref="T:System.Collections.Generic.HashSet`1" /> when deserializing.
	/// </summary>
	/// <typeparam name="T">the element type of the <see cref="T:System.Collections.Generic.ISet`1" /></typeparam>
	/// <seealso cref="T:IPA.Config.Stores.Converters.CollectionConverter`2" />
	// Token: 0x02000089 RID: 137
	public class ISetConverter<T> : CollectionConverter<T, ISet<T>>
	{
		/// <summary>
		/// Creates an <see cref="T:IPA.Config.Stores.Converters.ISetConverter`1" /> using the default converter for <typeparamref name="T" />.
		/// </summary>
		/// <seealso cref="M:IPA.Config.Stores.Converters.CollectionConverter`2.#ctor" />
		// Token: 0x0600038A RID: 906 RVA: 0x0001308F File Offset: 0x0001128F
		public ISetConverter()
		{
		}

		/// <summary>
		/// Creates an <see cref="T:IPA.Config.Stores.Converters.ISetConverter`1" /> using the specified underlying converter for values.
		/// </summary>
		/// <param name="underlying">the underlying <see cref="T:IPA.Config.Stores.ValueConverter`1" /> to use for the values</param>
		// Token: 0x0600038B RID: 907 RVA: 0x00013097 File Offset: 0x00011297
		public ISetConverter(ValueConverter<T> underlying)
			: base(underlying)
		{
		}

		/// <summary>
		/// Creates a new <see cref="T:System.Collections.Generic.ISet`1" /> (a <see cref="T:System.Collections.Generic.HashSet`1" />) for deserialization.
		/// </summary>
		/// <param name="size">the size to initialize it to</param>
		/// <param name="parent">the object that will own the new object</param>
		/// <returns>the new <see cref="T:System.Collections.Generic.ISet`1" /></returns>
		// Token: 0x0600038C RID: 908 RVA: 0x000130A0 File Offset: 0x000112A0
		protected override ISet<T> Create(int size, object parent)
		{
			return new HashSet<T>();
		}
	}
}
