using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using IPA.Config.Data;

namespace IPA.Config.Stores.Converters
{
	/// <summary>
	/// A converter for instances of <see cref="T:System.Collections.Generic.IReadOnlyDictionary`2" />.
	/// </summary>
	/// <typeparam name="TValue">the value type of the dictionary</typeparam>
	// Token: 0x02000073 RID: 115
	public class IReadOnlyDictionaryConverter<TValue> : ValueConverter<IReadOnlyDictionary<string, TValue>>
	{
		/// <summary>
		/// Gets the converter for the dictionary's value type.
		/// </summary>
		// Token: 0x1700009A RID: 154
		// (get) Token: 0x06000340 RID: 832 RVA: 0x00012AC2 File Offset: 0x00010CC2
		protected ValueConverter<TValue> BaseConverter { get; }

		/// <summary>
		/// Constructs an <see cref="T:IPA.Config.Stores.Converters.IReadOnlyDictionaryConverter`1" /> using the default converter for the value type.
		/// </summary>
		// Token: 0x06000341 RID: 833 RVA: 0x00012ACA File Offset: 0x00010CCA
		public IReadOnlyDictionaryConverter()
			: this(Converter<TValue>.Default)
		{
		}

		/// <summary>
		/// Constructs an <see cref="T:IPA.Config.Stores.Converters.IReadOnlyDictionaryConverter`1" /> using the specified converter for the value.
		/// </summary>
		/// <param name="converter">the converter for the value</param>
		// Token: 0x06000342 RID: 834 RVA: 0x00012AD7 File Offset: 0x00010CD7
		public IReadOnlyDictionaryConverter(ValueConverter<TValue> converter)
		{
			this.BaseConverter = converter;
		}

		/// <summary>
		/// Converts a <see cref="T:IPA.Config.Data.Map" /> to an <see cref="T:System.Collections.Generic.IDictionary`2" /> that is represented by it.
		/// </summary>
		/// <param name="value">the <see cref="T:IPA.Config.Data.Map" /> to convert</param>
		/// <param name="parent">the parent that will own the resulting object</param>
		/// <returns>the deserialized dictionary</returns>
		// Token: 0x06000343 RID: 835 RVA: 0x00012AE8 File Offset: 0x00010CE8
		public override IReadOnlyDictionary<string, TValue> FromValue(Value value, object parent)
		{
			Map map = value as Map;
			object obj;
			if (map == null)
			{
				obj = null;
			}
			else
			{
				IEnumerable<global::System.ValueTuple<string, TValue>> enumerable = map.Select((KeyValuePair<string, Value> kvp) => new global::System.ValueTuple<string, TValue>(kvp.Key, this.BaseConverter.FromValue(kvp.Value, parent)));
				if (enumerable == null)
				{
					obj = null;
				}
				else
				{
					obj = enumerable.ToDictionary(([global::System.Runtime.CompilerServices.TupleElementNames(new string[] { "Key", "val" })] global::System.ValueTuple<string, TValue> p) => p.Item1, ([global::System.Runtime.CompilerServices.TupleElementNames(new string[] { "Key", "val" })] global::System.ValueTuple<string, TValue> p) => p.Item2);
				}
			}
			object obj2 = obj;
			if (obj2 == null)
			{
				throw new ArgumentException("Value not a map", "value");
			}
			return obj2;
		}

		/// <summary>
		/// Serializes an <see cref="T:System.Collections.Generic.IDictionary`2" /> into a <see cref="T:IPA.Config.Data.Map" /> containing its values.
		/// </summary>
		/// <param name="obj">the dictionary to serialize</param>
		/// <param name="parent">the object that owns the dictionary</param>
		/// <returns>the dictionary serialized as a <see cref="T:IPA.Config.Data.Map" /></returns>
		// Token: 0x06000344 RID: 836 RVA: 0x00012B88 File Offset: 0x00010D88
		public override Value ToValue(IReadOnlyDictionary<string, TValue> obj, object parent)
		{
			return Value.From(obj.Select((KeyValuePair<string, TValue> p) => new KeyValuePair<string, Value>(p.Key, this.BaseConverter.ToValue(p.Value, parent))));
		}
	}
}
