using System;
using IPA.Config.Data;

namespace IPA.Config.Stores.Converters
{
	/// <summary>
	/// A converter for an enum of type <typeparamref name="T" />, that converts the enum to its string representation and back.
	/// </summary>
	/// <typeparam name="T">the enum type</typeparam>
	// Token: 0x0200006C RID: 108
	public sealed class EnumConverter<T> : ValueConverter<T> where T : Enum
	{
		/// <summary>
		/// Converts a <see cref="T:IPA.Config.Data.Value" /> that is a <see cref="T:IPA.Config.Data.Text" /> node to the corresponding enum value.
		/// </summary>
		/// <param name="value">the <see cref="T:IPA.Config.Data.Value" /> to convert</param>
		/// <param name="parent">the object which will own the created object</param>
		/// <returns>the deserialized enum value</returns>
		/// <exception cref="T:System.ArgumentException">if <paramref name="value" /> is not a <see cref="T:IPA.Config.Data.Text" /> node</exception>
		// Token: 0x0600032B RID: 811 RVA: 0x00012780 File Offset: 0x00010980
		public override T FromValue(Value value, object parent)
		{
			Text t = value as Text;
			if (t == null)
			{
				throw new ArgumentException("Value not a string", "value");
			}
			return (T)((object)Enum.Parse(typeof(T), t.Value));
		}

		/// <summary>
		/// Converts an enum of type <typeparamref name="T" /> to a <see cref="T:IPA.Config.Data.Value" /> node corresponding to its value.
		/// </summary>
		/// <param name="obj">the value to serialize</param>
		/// <param name="parent">the object which owns <paramref name="obj" /></param>
		/// <returns>a <see cref="T:IPA.Config.Data.Text" /> node representing <paramref name="obj" /></returns>
		// Token: 0x0600032C RID: 812 RVA: 0x000127C1 File Offset: 0x000109C1
		public override Value ToValue(T obj, object parent)
		{
			return Value.Text(obj.ToString());
		}
	}
}
