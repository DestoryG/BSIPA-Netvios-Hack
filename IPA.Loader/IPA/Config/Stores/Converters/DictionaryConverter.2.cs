using System;

namespace IPA.Config.Stores.Converters
{
	/// <summary>
	/// A converter for instances of <see cref="T:System.Collections.Generic.Dictionary`2" />, specifying a value converter as a type parameter.
	/// </summary>
	/// <typeparam name="TValue">the value type of the dictionary</typeparam>
	/// <typeparam name="TConverter">the converter type for values</typeparam>
	// Token: 0x02000072 RID: 114
	public sealed class DictionaryConverter<TValue, TConverter> : DictionaryConverter<TValue> where TConverter : ValueConverter<TValue>, new()
	{
		/// <summary>
		/// Constructs a new <see cref="T:IPA.Config.Stores.Converters.IDictionaryConverter`2" /> with a new instance of 
		/// <typeparamref name="TConverter" /> as the value converter.
		/// </summary>
		// Token: 0x0600033F RID: 831 RVA: 0x00012AB0 File Offset: 0x00010CB0
		public DictionaryConverter()
			: base(new TConverter())
		{
		}
	}
}
