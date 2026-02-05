using System;

namespace IPA.Config.Stores.Converters
{
	/// <summary>
	/// A converter for instances of <see cref="T:System.Collections.Generic.IReadOnlyDictionary`2" />, specifying a value converter as a type parameter.
	/// </summary>
	/// <typeparam name="TValue">the value type of the dictionary</typeparam>
	/// <typeparam name="TConverter">the converter type for values</typeparam>
	// Token: 0x02000074 RID: 116
	public sealed class IReadOnlyDictionaryConverter<TValue, TConverter> : IReadOnlyDictionaryConverter<TValue> where TConverter : ValueConverter<TValue>, new()
	{
		/// <summary>
		/// Constructs a new <see cref="T:IPA.Config.Stores.Converters.IReadOnlyDictionaryConverter`2" /> with a new instance of 
		/// <typeparamref name="TConverter" /> as the value converter.
		/// </summary>
		// Token: 0x06000345 RID: 837 RVA: 0x00012BC0 File Offset: 0x00010DC0
		public IReadOnlyDictionaryConverter()
			: base(new TConverter())
		{
		}
	}
}
