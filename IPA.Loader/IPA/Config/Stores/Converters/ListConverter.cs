using System;
using System.Collections.Generic;

namespace IPA.Config.Stores.Converters
{
	/// <summary>
	/// A <see cref="T:IPA.Config.Stores.Converters.CollectionConverter`2" /> for a <see cref="T:System.Collections.Generic.List`1" />.
	/// </summary>
	/// <typeparam name="T">the element type of the <see cref="T:System.Collections.Generic.List`1" /></typeparam>
	/// <seealso cref="T:IPA.Config.Stores.Converters.CollectionConverter`2" />
	// Token: 0x0200008B RID: 139
	public class ListConverter<T> : CollectionConverter<T, List<T>>
	{
		/// <summary>
		/// Creates an <see cref="T:IPA.Config.Stores.Converters.ListConverter`1" /> using the default converter for <typeparamref name="T" />.
		/// </summary>
		/// <seealso cref="M:IPA.Config.Stores.Converters.CollectionConverter`2.#ctor" />
		// Token: 0x0600038E RID: 910 RVA: 0x000130B9 File Offset: 0x000112B9
		public ListConverter()
		{
		}

		/// <summary>
		/// Creates an <see cref="T:IPA.Config.Stores.Converters.ListConverter`1" /> using the specified underlying converter for values.
		/// </summary>
		/// <param name="underlying">the underlying <see cref="T:IPA.Config.Stores.ValueConverter`1" /> to use for the values</param>
		// Token: 0x0600038F RID: 911 RVA: 0x000130C1 File Offset: 0x000112C1
		public ListConverter(ValueConverter<T> underlying)
			: base(underlying)
		{
		}

		/// <summary>
		/// Creates a new <see cref="T:System.Collections.Generic.List`1" /> for deserialization.
		/// </summary>
		/// <param name="size">the size to initialize it to</param>
		/// <param name="parent">the object that will own the new object</param>
		/// <returns>the new <see cref="T:System.Collections.Generic.List`1" /></returns>
		// Token: 0x06000390 RID: 912 RVA: 0x000130CA File Offset: 0x000112CA
		protected override List<T> Create(int size, object parent)
		{
			return new List<T>(size);
		}
	}
}
