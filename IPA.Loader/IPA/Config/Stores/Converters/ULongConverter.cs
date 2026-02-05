using System;
using IPA.Config.Data;

namespace IPA.Config.Stores.Converters
{
	// Token: 0x02000078 RID: 120
	internal class ULongConverter : ValueConverter<ulong>
	{
		// Token: 0x0600034F RID: 847 RVA: 0x00012C78 File Offset: 0x00010E78
		public override ulong FromValue(Value value, object parent)
		{
			decimal? num = Converter.FloatValue(value);
			if (num == null)
			{
				throw new ArgumentException("Value not a numeric value", "value");
			}
			return (ulong)num.GetValueOrDefault();
		}

		// Token: 0x06000350 RID: 848 RVA: 0x00012CB1 File Offset: 0x00010EB1
		public override Value ToValue(ulong obj, object parent)
		{
			return Value.From(obj);
		}
	}
}
