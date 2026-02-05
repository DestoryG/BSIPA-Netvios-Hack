using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using IPA.Config.Data;

namespace IPA.Config.Stores.Converters
{
	/// <summary>
	/// A converter for instances of <see cref="T:System.Collections.Generic.IDictionary`2" />.
	/// </summary>
	/// <typeparam name="TValue">the value type of the dictionary</typeparam>
	// Token: 0x0200006F RID: 111
	public class IDictionaryConverter<TValue> : ValueConverter<IDictionary<string, TValue>>
	{
		/// <summary>
		/// Gets the converter for the dictionary's value type.
		/// </summary>
		// Token: 0x17000098 RID: 152
		// (get) Token: 0x06000334 RID: 820 RVA: 0x000128A2 File Offset: 0x00010AA2
		protected ValueConverter<TValue> BaseConverter { get; }

		/// <summary>
		/// Constructs an <see cref="T:IPA.Config.Stores.Converters.IDictionaryConverter`1" /> using the default converter for the value type.
		/// </summary>
		// Token: 0x06000335 RID: 821 RVA: 0x000128AA File Offset: 0x00010AAA
		public IDictionaryConverter()
			: this(Converter<TValue>.Default)
		{
		}

		/// <summary>
		/// Constructs an <see cref="T:IPA.Config.Stores.Converters.IDictionaryConverter`1" /> using the specified converter for the value.
		/// </summary>
		/// <param name="converter">the converter for the value</param>
		// Token: 0x06000336 RID: 822 RVA: 0x000128B7 File Offset: 0x00010AB7
		public IDictionaryConverter(ValueConverter<TValue> converter)
		{
			this.BaseConverter = converter;
		}

		/// <summary>
		/// Converts a <see cref="T:IPA.Config.Data.Map" /> to an <see cref="T:System.Collections.Generic.IDictionary`2" /> that is represented by it.
		/// </summary>
		/// <param name="value">the <see cref="T:IPA.Config.Data.Map" /> to convert</param>
		/// <param name="parent">the parent that will own the resulting object</param>
		/// <returns>the deserialized dictionary</returns>
		// Token: 0x06000337 RID: 823 RVA: 0x000128C8 File Offset: 0x00010AC8
		public override IDictionary<string, TValue> FromValue(Value value, object parent)
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
		// Token: 0x06000338 RID: 824 RVA: 0x00012968 File Offset: 0x00010B68
		public override Value ToValue(IDictionary<string, TValue> obj, object parent)
		{
			return Value.From(obj.Select((KeyValuePair<string, TValue> p) => new KeyValuePair<string, Value>(p.Key, this.BaseConverter.ToValue(p.Value, parent))));
		}
	}
}
