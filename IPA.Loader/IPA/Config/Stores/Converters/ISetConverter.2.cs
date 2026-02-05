using System;

namespace IPA.Config.Stores.Converters
{
	/// <summary>
	/// An <see cref="T:IPA.Config.Stores.Converters.ISetConverter`1" /> which default constructs a converter for use as the value converter.
	/// </summary>
	/// <typeparam name="T">the value type of the collection</typeparam>
	/// <typeparam name="TConverter">the type of the converter to use for <typeparamref name="T" /></typeparam>
	/// <seealso cref="T:IPA.Config.Stores.Converters.ISetConverter`1" />
	// Token: 0x0200008A RID: 138
	public sealed class ISetConverter<T, TConverter> : ISetConverter<T> where TConverter : ValueConverter<T>, new()
	{
		/// <summary>
		/// Creates an <see cref="T:IPA.Config.Stores.Converters.ISetConverter`1" /> using a default constructed <typeparamref name="TConverter" />
		/// element type. Equivalent to calling <see cref="M:IPA.Config.Stores.Converters.ISetConverter`1.#ctor(IPA.Config.Stores.ValueConverter{`0})" />
		/// with a default-constructed <typeparamref name="TConverter" />.
		/// </summary>
		/// <seealso cref="M:IPA.Config.Stores.Converters.ISetConverter`1.#ctor(IPA.Config.Stores.ValueConverter{`0})" />
		// Token: 0x0600038D RID: 909 RVA: 0x000130A7 File Offset: 0x000112A7
		public ISetConverter()
			: base(new TConverter())
		{
		}
	}
}
