using System;
using IPA.Config.Data;

namespace IPA.Config.Stores.Converters
{
	/// <summary>
	/// A converter for an enum of type <typeparamref name="T" />, that converts the enum to its underlying value for serialization.
	/// </summary>
	/// <typeparam name="T">the enum type</typeparam>
	// Token: 0x0200006E RID: 110
	public sealed class NumericEnumConverter<T> : ValueConverter<T> where T : Enum
	{
		/// <summary>
		/// Converts a <see cref="T:IPA.Config.Data.Value" /> that is a numeric node to the corresponding enum value.
		/// </summary>
		/// <param name="value">the <see cref="T:IPA.Config.Data.Value" /> to convert</param>
		/// <param name="parent">the object which will own the created object</param>
		/// <returns>the deserialized enum value</returns>
		/// <exception cref="T:System.ArgumentException">if <paramref name="value" /> is not a numeric node</exception>
		// Token: 0x06000331 RID: 817 RVA: 0x00012840 File Offset: 0x00010A40
		public override T FromValue(Value value, object parent)
		{
			Type typeFromHandle = typeof(T);
			long? num = Converter.IntValue(value);
			if (num == null)
			{
				throw new ArgumentException("Value not a numeric node", "value");
			}
			return (T)((object)Enum.ToObject(typeFromHandle, num.GetValueOrDefault()));
		}

		/// <summary>
		/// Converts an enum of type <typeparamref name="T" /> to a <see cref="T:IPA.Config.Data.Value" /> node corresponding to its value.
		/// </summary>
		/// <param name="obj">the value to serialize</param>
		/// <param name="parent">the object which owns <paramref name="obj" /></param>
		/// <returns>an <see cref="T:IPA.Config.Data.Integer" /> node representing <paramref name="obj" /></returns>
		// Token: 0x06000332 RID: 818 RVA: 0x00012888 File Offset: 0x00010A88
		public override Value ToValue(T obj, object parent)
		{
			return Value.Integer(Convert.ToInt64(obj));
		}
	}
}
