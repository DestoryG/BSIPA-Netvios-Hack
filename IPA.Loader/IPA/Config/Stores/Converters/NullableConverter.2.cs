using System;

namespace IPA.Config.Stores.Converters
{
	/// <summary>
	/// A converter for a <see cref="T:System.Nullable`1" /> that default-constructs a converter of type <typeparamref name="TConverter" />
	/// to use as the underlying converter. Use this in the <see cref="T:IPA.Config.Stores.Attributes.UseConverterAttribute" />.
	/// </summary>
	/// <typeparam name="T">the underlying type of the <see cref="T:System.Nullable`1" /></typeparam>
	/// <typeparam name="TConverter">the type to use as an underlying converter</typeparam>
	/// <seealso cref="T:IPA.Config.Stores.Converters.NullableConverter`1" />
	// Token: 0x0200006B RID: 107
	public sealed class NullableConverter<T, TConverter> : NullableConverter<T> where T : struct where TConverter : ValueConverter<T>, new()
	{
		/// <summary>
		/// Creates a converter with a new <typeparamref name="TConverter" /> as the underlying converter.
		/// </summary>
		/// <seealso cref="M:IPA.Config.Stores.Converters.NullableConverter`1.#ctor(IPA.Config.Stores.ValueConverter{`0})" />
		// Token: 0x0600032A RID: 810 RVA: 0x0001276C File Offset: 0x0001096C
		public NullableConverter()
			: base(new TConverter())
		{
		}
	}
}
