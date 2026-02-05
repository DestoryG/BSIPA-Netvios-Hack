using System;
using System.Runtime.CompilerServices;

namespace IPA.Config.Stores.Converters
{
	/// <summary>
	/// Provides generic utilities for converters for certain types.
	/// </summary>
	/// <typeparam name="T">the type of the <see cref="T:IPA.Config.Stores.ValueConverter`1" /> that this works on</typeparam>
	// Token: 0x02000069 RID: 105
	public static class Converter<T>
	{
		/// <summary>
		/// Gets the default <see cref="T:IPA.Config.Stores.ValueConverter`1" /> for the current type.
		/// </summary>
		// Token: 0x17000097 RID: 151
		// (get) Token: 0x06000322 RID: 802 RVA: 0x000126C9 File Offset: 0x000108C9
		public static ValueConverter<T> Default
		{
			get
			{
				ValueConverter<T> valueConverter;
				if ((valueConverter = Converter<T>.defaultConverter) == null)
				{
					valueConverter = (Converter<T>.defaultConverter = Converter<T>.MakeDefault());
				}
				return valueConverter;
			}
		}

		// Token: 0x06000323 RID: 803 RVA: 0x000126DF File Offset: 0x000108DF
		internal static ValueConverter<T> MakeDefault()
		{
			return Converter<T>.<MakeDefault>g__MakeInstOf|3_0(Converter.GetDefaultConverterType(typeof(T)));
		}

		// Token: 0x06000325 RID: 805 RVA: 0x000126F7 File Offset: 0x000108F7
		[CompilerGenerated]
		internal static ValueConverter<T> <MakeDefault>g__MakeInstOf|3_0(Type ty)
		{
			return Activator.CreateInstance(ty) as ValueConverter<T>;
		}

		// Token: 0x04000120 RID: 288
		private static ValueConverter<T> defaultConverter;
	}
}
