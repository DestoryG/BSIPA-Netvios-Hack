using System;

namespace IPA.Config.Stores.Converters
{
	/// <summary>
	/// A converter for instances of <see cref="T:System.Collections.Generic.IDictionary`2" />, specifying a value converter as a type parameter.
	/// </summary>
	/// <typeparam name="TValue">the value type of the dictionary</typeparam>
	/// <typeparam name="TConverter">the converter type for values</typeparam>
	// Token: 0x02000070 RID: 112
	public sealed class IDictionaryConverter<TValue, TConverter> : IDictionaryConverter<TValue> where TConverter : ValueConverter<TValue>, new()
	{
		/// <summary>
		/// Constructs a new <see cref="T:IPA.Config.Stores.Converters.IDictionaryConverter`2" /> with a new instance of 
		/// <typeparamref name="TConverter" /> as the value converter.
		/// </summary>
		// Token: 0x06000339 RID: 825 RVA: 0x000129A0 File Offset: 0x00010BA0
		public IDictionaryConverter()
			: base(new TConverter())
		{
		}
	}
}
