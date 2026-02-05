using System;
using IPA.Config.Data;

namespace IPA.Config.Stores.Converters
{
	/// <summary>
	/// A converter for an enum of type <typeparamref name="T" />, that converts the enum to its string representation and back,
	/// ignoring the case of the serialized value for deseiralization.
	/// </summary>
	/// <typeparam name="T">the enum type</typeparam>
	// Token: 0x0200006D RID: 109
	public sealed class CaseInsensitiveEnumConverter<T> : ValueConverter<T> where T : Enum
	{
		/// <summary>
		/// Converts a <see cref="T:IPA.Config.Data.Value" /> that is a <see cref="T:IPA.Config.Data.Text" /> node to the corresponding enum value.
		/// </summary>
		/// <param name="value">the <see cref="T:IPA.Config.Data.Value" /> to convert</param>
		/// <param name="parent">the object which will own the created object</param>
		/// <returns>the deserialized enum value</returns>
		/// <exception cref="T:System.ArgumentException">if <paramref name="value" /> is not a <see cref="T:IPA.Config.Data.Text" /> node</exception>
		// Token: 0x0600032E RID: 814 RVA: 0x000127E0 File Offset: 0x000109E0
		public override T FromValue(Value value, object parent)
		{
			Text t = value as Text;
			if (t == null)
			{
				throw new ArgumentException("Value not a string", "value");
			}
			return (T)((object)Enum.Parse(typeof(T), t.Value, true));
		}

		/// <summary>
		/// Converts an enum of type <typeparamref name="T" /> to a <see cref="T:IPA.Config.Data.Value" /> node corresponding to its value.
		/// </summary>
		/// <param name="obj">the value to serialize</param>
		/// <param name="parent">the object which owns <paramref name="obj" /></param>
		/// <returns>a <see cref="T:IPA.Config.Data.Text" /> node representing <paramref name="obj" /></returns>
		// Token: 0x0600032F RID: 815 RVA: 0x00012822 File Offset: 0x00010A22
		public override Value ToValue(T obj, object parent)
		{
			return Value.Text(obj.ToString());
		}
	}
}
