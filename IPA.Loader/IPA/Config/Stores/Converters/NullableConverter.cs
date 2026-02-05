using System;
using IPA.Config.Data;

namespace IPA.Config.Stores.Converters
{
	/// <summary>
	/// A converter for a <see cref="T:System.Nullable`1" />.
	/// </summary>
	/// <typeparam name="T">the underlying type of the <see cref="T:System.Nullable`1" /></typeparam>
	// Token: 0x0200006A RID: 106
	public class NullableConverter<T> : ValueConverter<T?> where T : struct
	{
		/// <summary>
		/// Creates a converter with the default converter for the base type.
		/// Equivalent to 
		/// <code>
		/// new NullableConverter(Converter&lt;T&gt;.Default)
		/// </code>
		/// </summary>
		/// <seealso cref="M:IPA.Config.Stores.Converters.NullableConverter`1.#ctor(IPA.Config.Stores.ValueConverter{`0})" />
		/// <seealso cref="P:IPA.Config.Stores.Converters.Converter`1.Default" />
		// Token: 0x06000326 RID: 806 RVA: 0x00012704 File Offset: 0x00010904
		public NullableConverter()
			: this(Converter<T>.Default)
		{
		}

		/// <summary>
		/// Creates a converter with the given underlying <see cref="T:IPA.Config.Stores.ValueConverter`1" />.
		/// </summary>
		/// <param name="underlying">the undlerlying <see cref="T:IPA.Config.Stores.ValueConverter`1" /> to use</param>
		// Token: 0x06000327 RID: 807 RVA: 0x00012711 File Offset: 0x00010911
		public NullableConverter(ValueConverter<T> underlying)
		{
			this.baseConverter = underlying;
		}

		/// <summary>
		/// Converts a <see cref="T:IPA.Config.Data.Value" /> tree to a value.
		/// </summary>
		/// <param name="value">the <see cref="T:IPA.Config.Data.Value" /> tree to convert</param>
		/// <param name="parent">the object which will own the created object</param>
		/// <returns>the object represented by <paramref name="value" /></returns>
		// Token: 0x06000328 RID: 808 RVA: 0x00012720 File Offset: 0x00010920
		public override T? FromValue(Value value, object parent)
		{
			if (value != null)
			{
				return new T?(this.baseConverter.FromValue(value, parent));
			}
			return null;
		}

		/// <summary>
		/// Converts a nullable <typeparamref name="T" /> to a <see cref="T:IPA.Config.Data.Value" /> tree.
		/// </summary>
		/// <param name="obj">the value to serialize</param>
		/// <param name="parent">the object which owns <paramref name="obj" /></param>
		/// <returns>a <see cref="T:IPA.Config.Data.Value" /> tree representing <paramref name="obj" />.</returns>
		// Token: 0x06000329 RID: 809 RVA: 0x0001274C File Offset: 0x0001094C
		public override Value ToValue(T? obj, object parent)
		{
			if (obj != null)
			{
				return this.baseConverter.ToValue(obj.Value, parent);
			}
			return null;
		}

		// Token: 0x04000121 RID: 289
		private readonly ValueConverter<T> baseConverter;
	}
}
