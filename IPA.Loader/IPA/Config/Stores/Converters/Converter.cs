using System;
using System.Collections.Generic;
using System.Linq;
using IPA.Config.Data;

namespace IPA.Config.Stores.Converters
{
	/// <summary>
	/// Provides utility functions for custom converters.
	/// </summary>
	// Token: 0x02000068 RID: 104
	public static class Converter
	{
		/// <summary>
		/// Gets the integral value of a <see cref="T:IPA.Config.Data.Value" />, coercing a <see cref="T:IPA.Config.Data.FloatingPoint" /> if necessary,
		/// or <see langword="null" /> if <paramref name="val" /> is not an <see cref="T:IPA.Config.Data.Integer" /> or <see cref="T:IPA.Config.Data.FloatingPoint" />.
		/// </summary>
		/// <param name="val">the <see cref="T:IPA.Config.Data.Value" /> to get the integral value of</param>
		/// <returns>the integral value of <paramref name="val" />, or <see langword="null" /></returns>
		// Token: 0x0600031F RID: 799 RVA: 0x0001238C File Offset: 0x0001058C
		public static long? IntValue(Value val)
		{
			Integer inte = val as Integer;
			if (inte != null)
			{
				return new long?(inte.Value);
			}
			FloatingPoint fp = val as FloatingPoint;
			if (fp == null)
			{
				return null;
			}
			Integer integer = fp.AsInteger();
			if (integer == null)
			{
				return null;
			}
			return new long?(integer.Value);
		}

		/// <summary>
		/// Gets the floaing point value of a <see cref="T:IPA.Config.Data.Value" />, coercing an <see cref="T:IPA.Config.Data.Integer" /> if necessary,
		/// or <see langword="null" /> if <paramref name="val" /> is not an <see cref="T:IPA.Config.Data.Integer" /> or <see cref="T:IPA.Config.Data.FloatingPoint" />.
		/// </summary>
		/// <param name="val">the <see cref="T:IPA.Config.Data.Value" /> to get the floaing point value of</param>
		/// <returns>the floaing point value of <paramref name="val" />, or <see langword="null" /></returns>
		// Token: 0x06000320 RID: 800 RVA: 0x000123E4 File Offset: 0x000105E4
		public static decimal? FloatValue(Value val)
		{
			FloatingPoint fp = val as FloatingPoint;
			if (fp != null)
			{
				return new decimal?(fp.Value);
			}
			Integer inte = val as Integer;
			if (inte == null)
			{
				return null;
			}
			FloatingPoint floatingPoint = inte.AsFloat();
			if (floatingPoint == null)
			{
				return null;
			}
			return new decimal?(floatingPoint.Value);
		}

		// Token: 0x06000321 RID: 801 RVA: 0x0001243C File Offset: 0x0001063C
		internal static Type GetDefaultConverterType(Type t)
		{
			if (t.IsEnum)
			{
				return typeof(CaseInsensitiveEnumConverter<>).MakeGenericType(new Type[] { t });
			}
			if (t.IsGenericType)
			{
				Type generic = t.GetGenericTypeDefinition();
				Type[] args = t.GetGenericArguments();
				if (generic == typeof(List<>))
				{
					return typeof(ListConverter<>).MakeGenericType(args);
				}
				if (generic == typeof(IList<>))
				{
					return typeof(IListConverter<>).MakeGenericType(args);
				}
				if (generic == typeof(Dictionary<, >) && args[0] == typeof(string))
				{
					return typeof(DictionaryConverter<>).MakeGenericType(new Type[] { args[1] });
				}
				if (generic == typeof(IDictionary<, >) && args[0] == typeof(string))
				{
					return typeof(IDictionaryConverter<>).MakeGenericType(new Type[] { args[1] });
				}
				if (generic == typeof(ISet<>))
				{
					return typeof(ISetConverter<>).MakeGenericType(args);
				}
				if (generic == typeof(IReadOnlyDictionary<, >) && args[0] == typeof(string))
				{
					return typeof(IReadOnlyDictionaryConverter<>).MakeGenericType(new Type[] { args[1] });
				}
			}
			Type iCollBase = t.GetInterfaces().FirstOrDefault((Type i) => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(ICollection<>));
			if (iCollBase != null && t.GetConstructor(Type.EmptyTypes) != null)
			{
				Type valueType = iCollBase.GetGenericArguments().First<Type>();
				return typeof(CollectionConverter<, >).MakeGenericType(new Type[] { valueType, t });
			}
			if (t == typeof(string))
			{
				return typeof(StringConverter);
			}
			if (!t.IsValueType)
			{
				return typeof(CustomObjectConverter<>).MakeGenericType(new Type[] { t });
			}
			if (t.IsGenericType && t.GetGenericTypeDefinition() == typeof(Nullable<>))
			{
				return typeof(NullableConverter<>).MakeGenericType(new Type[] { Nullable.GetUnderlyingType(t) });
			}
			return (Activator.CreateInstance(typeof(Converter.ValConv<>).MakeGenericType(new Type[] { t })) as Converter.IValConv).Get();
		}

		// Token: 0x02000146 RID: 326
		internal interface IValConv
		{
			// Token: 0x06000664 RID: 1636
			Type Get();
		}

		// Token: 0x02000147 RID: 327
		internal interface IValConv<T>
		{
			// Token: 0x06000665 RID: 1637
			Type Get();
		}

		// Token: 0x02000148 RID: 328
		internal class ValConv<T> : Converter.IValConv, Converter.IValConv<T> where T : struct
		{
			// Token: 0x06000666 RID: 1638 RVA: 0x000182EE File Offset: 0x000164EE
			public Type Get()
			{
				return Converter.ValConv<T>.Impl.Get();
			}

			// Token: 0x06000667 RID: 1639 RVA: 0x000182FA File Offset: 0x000164FA
			Type Converter.IValConv<T>.Get()
			{
				return typeof(CustomValueTypeConverter<T>);
			}

			// Token: 0x04000438 RID: 1080
			private static readonly Converter.IValConv<T> Impl = (Converter.ValConvImpls.Impl as Converter.IValConv<T>) ?? new Converter.ValConv<T>();
		}

		// Token: 0x02000149 RID: 329
		private class ValConvImpls : Converter.IValConv<char>, Converter.IValConv<IntPtr>, Converter.IValConv<UIntPtr>, Converter.IValConv<long>, Converter.IValConv<ulong>, Converter.IValConv<int>, Converter.IValConv<uint>, Converter.IValConv<short>, Converter.IValConv<ushort>, Converter.IValConv<sbyte>, Converter.IValConv<byte>, Converter.IValConv<float>, Converter.IValConv<double>, Converter.IValConv<decimal>, Converter.IValConv<bool>
		{
			// Token: 0x0600066A RID: 1642 RVA: 0x00018328 File Offset: 0x00016528
			Type Converter.IValConv<char>.Get()
			{
				return typeof(CharConverter);
			}

			// Token: 0x0600066B RID: 1643 RVA: 0x00018334 File Offset: 0x00016534
			Type Converter.IValConv<long>.Get()
			{
				return typeof(LongConverter);
			}

			// Token: 0x0600066C RID: 1644 RVA: 0x00018340 File Offset: 0x00016540
			Type Converter.IValConv<ulong>.Get()
			{
				return typeof(ULongConverter);
			}

			// Token: 0x0600066D RID: 1645 RVA: 0x0001834C File Offset: 0x0001654C
			Type Converter.IValConv<IntPtr>.Get()
			{
				return typeof(IntPtrConverter);
			}

			// Token: 0x0600066E RID: 1646 RVA: 0x00018358 File Offset: 0x00016558
			Type Converter.IValConv<UIntPtr>.Get()
			{
				return typeof(UIntPtrConverter);
			}

			// Token: 0x0600066F RID: 1647 RVA: 0x00018364 File Offset: 0x00016564
			Type Converter.IValConv<int>.Get()
			{
				return typeof(IntConverter);
			}

			// Token: 0x06000670 RID: 1648 RVA: 0x00018370 File Offset: 0x00016570
			Type Converter.IValConv<uint>.Get()
			{
				return typeof(UIntConverter);
			}

			// Token: 0x06000671 RID: 1649 RVA: 0x0001837C File Offset: 0x0001657C
			Type Converter.IValConv<short>.Get()
			{
				return typeof(ShortConverter);
			}

			// Token: 0x06000672 RID: 1650 RVA: 0x00018388 File Offset: 0x00016588
			Type Converter.IValConv<ushort>.Get()
			{
				return typeof(UShortConverter);
			}

			// Token: 0x06000673 RID: 1651 RVA: 0x00018394 File Offset: 0x00016594
			Type Converter.IValConv<byte>.Get()
			{
				return typeof(ByteConverter);
			}

			// Token: 0x06000674 RID: 1652 RVA: 0x000183A0 File Offset: 0x000165A0
			Type Converter.IValConv<sbyte>.Get()
			{
				return typeof(SByteConverter);
			}

			// Token: 0x06000675 RID: 1653 RVA: 0x000183AC File Offset: 0x000165AC
			Type Converter.IValConv<float>.Get()
			{
				return typeof(FloatConverter);
			}

			// Token: 0x06000676 RID: 1654 RVA: 0x000183B8 File Offset: 0x000165B8
			Type Converter.IValConv<double>.Get()
			{
				return typeof(DoubleConverter);
			}

			// Token: 0x06000677 RID: 1655 RVA: 0x000183C4 File Offset: 0x000165C4
			Type Converter.IValConv<decimal>.Get()
			{
				return typeof(DecimalConverter);
			}

			// Token: 0x06000678 RID: 1656 RVA: 0x000183D0 File Offset: 0x000165D0
			Type Converter.IValConv<bool>.Get()
			{
				return typeof(BooleanConverter);
			}

			// Token: 0x04000439 RID: 1081
			internal static readonly Converter.ValConvImpls Impl = new Converter.ValConvImpls();
		}
	}
}
