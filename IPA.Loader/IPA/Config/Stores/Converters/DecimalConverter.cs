using System;
using IPA.Config.Data;

namespace IPA.Config.Stores.Converters
{
	// Token: 0x02000081 RID: 129
	internal class DecimalConverter : ValueConverter<decimal>
	{
		// Token: 0x0600036A RID: 874 RVA: 0x00012DDC File Offset: 0x00010FDC
		public override decimal FromValue(Value value, object parent)
		{
			decimal? num = Converter.FloatValue(value);
			if (num == null)
			{
				throw new ArgumentException("Value not a numeric value", "value");
			}
			return num.GetValueOrDefault();
		}

		// Token: 0x0600036B RID: 875 RVA: 0x00012E10 File Offset: 0x00011010
		public override Value ToValue(decimal obj, object parent)
		{
			return Value.From(obj);
		}
	}
}
