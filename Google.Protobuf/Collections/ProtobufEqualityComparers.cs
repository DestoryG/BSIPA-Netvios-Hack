using System;
using System.Collections.Generic;

namespace Google.Protobuf.Collections
{
	// Token: 0x02000086 RID: 134
	public static class ProtobufEqualityComparers
	{
		// Token: 0x06000870 RID: 2160 RVA: 0x0001DD48 File Offset: 0x0001BF48
		public static EqualityComparer<T> GetEqualityComparer<T>()
		{
			if (typeof(T) == typeof(double))
			{
				return (EqualityComparer<T>)ProtobufEqualityComparers.BitwiseDoubleEqualityComparer;
			}
			if (typeof(T) == typeof(float))
			{
				return (EqualityComparer<T>)ProtobufEqualityComparers.BitwiseSingleEqualityComparer;
			}
			if (typeof(T) == typeof(double?))
			{
				return (EqualityComparer<T>)ProtobufEqualityComparers.BitwiseNullableDoubleEqualityComparer;
			}
			if (!(typeof(T) == typeof(float?)))
			{
				return EqualityComparer<T>.Default;
			}
			return (EqualityComparer<T>)ProtobufEqualityComparers.BitwiseNullableSingleEqualityComparer;
		}

		// Token: 0x1700024F RID: 591
		// (get) Token: 0x06000871 RID: 2161 RVA: 0x0001DDF2 File Offset: 0x0001BFF2
		public static EqualityComparer<double> BitwiseDoubleEqualityComparer { get; } = new ProtobufEqualityComparers.BitwiseDoubleEqualityComparerImpl();

		// Token: 0x17000250 RID: 592
		// (get) Token: 0x06000872 RID: 2162 RVA: 0x0001DDF9 File Offset: 0x0001BFF9
		public static EqualityComparer<float> BitwiseSingleEqualityComparer { get; } = new ProtobufEqualityComparers.BitwiseSingleEqualityComparerImpl();

		// Token: 0x17000251 RID: 593
		// (get) Token: 0x06000873 RID: 2163 RVA: 0x0001DE00 File Offset: 0x0001C000
		public static EqualityComparer<double?> BitwiseNullableDoubleEqualityComparer { get; } = new ProtobufEqualityComparers.BitwiseNullableDoubleEqualityComparerImpl();

		// Token: 0x17000252 RID: 594
		// (get) Token: 0x06000874 RID: 2164 RVA: 0x0001DE07 File Offset: 0x0001C007
		public static EqualityComparer<float?> BitwiseNullableSingleEqualityComparer { get; } = new ProtobufEqualityComparers.BitwiseNullableSingleEqualityComparerImpl();

		// Token: 0x02000106 RID: 262
		private class BitwiseDoubleEqualityComparerImpl : EqualityComparer<double>
		{
			// Token: 0x06000A66 RID: 2662 RVA: 0x000211C6 File Offset: 0x0001F3C6
			public override bool Equals(double x, double y)
			{
				return BitConverter.DoubleToInt64Bits(x) == BitConverter.DoubleToInt64Bits(y);
			}

			// Token: 0x06000A67 RID: 2663 RVA: 0x000211D8 File Offset: 0x0001F3D8
			public override int GetHashCode(double obj)
			{
				return BitConverter.DoubleToInt64Bits(obj).GetHashCode();
			}
		}

		// Token: 0x02000107 RID: 263
		private class BitwiseSingleEqualityComparerImpl : EqualityComparer<float>
		{
			// Token: 0x06000A69 RID: 2665 RVA: 0x000211FB File Offset: 0x0001F3FB
			public override bool Equals(float x, float y)
			{
				return BitConverter.DoubleToInt64Bits((double)x) == BitConverter.DoubleToInt64Bits((double)y);
			}

			// Token: 0x06000A6A RID: 2666 RVA: 0x00021210 File Offset: 0x0001F410
			public override int GetHashCode(float obj)
			{
				return BitConverter.DoubleToInt64Bits((double)obj).GetHashCode();
			}
		}

		// Token: 0x02000108 RID: 264
		private class BitwiseNullableDoubleEqualityComparerImpl : EqualityComparer<double?>
		{
			// Token: 0x06000A6C RID: 2668 RVA: 0x00021234 File Offset: 0x0001F434
			public override bool Equals(double? x, double? y)
			{
				return (x == null && y == null) || (x != null && y != null && ProtobufEqualityComparers.BitwiseDoubleEqualityComparer.Equals(x.Value, y.Value));
			}

			// Token: 0x06000A6D RID: 2669 RVA: 0x00021281 File Offset: 0x0001F481
			public override int GetHashCode(double? obj)
			{
				if (obj != null)
				{
					return ProtobufEqualityComparers.BitwiseDoubleEqualityComparer.GetHashCode(obj.Value);
				}
				return 293864;
			}
		}

		// Token: 0x02000109 RID: 265
		private class BitwiseNullableSingleEqualityComparerImpl : EqualityComparer<float?>
		{
			// Token: 0x06000A6F RID: 2671 RVA: 0x000212AC File Offset: 0x0001F4AC
			public override bool Equals(float? x, float? y)
			{
				return (x == null && y == null) || (x != null && y != null && ProtobufEqualityComparers.BitwiseSingleEqualityComparer.Equals(x.Value, y.Value));
			}

			// Token: 0x06000A70 RID: 2672 RVA: 0x000212F9 File Offset: 0x0001F4F9
			public override int GetHashCode(float? obj)
			{
				if (obj != null)
				{
					return ProtobufEqualityComparers.BitwiseSingleEqualityComparer.GetHashCode(obj.Value);
				}
				return 293864;
			}
		}
	}
}
