using System;
using IPA.Config.Data;

namespace IPA.Config.Stores.Converters
{
	// Token: 0x02000083 RID: 131
	internal class DoubleConverter : ValueConverter<double>
	{
		// Token: 0x06000370 RID: 880 RVA: 0x00012E49 File Offset: 0x00011049
		public override double FromValue(Value value, object parent)
		{
			return (double)Converter<decimal>.Default.FromValue(value, parent);
		}

		// Token: 0x06000371 RID: 881 RVA: 0x00012E5D File Offset: 0x0001105D
		public override Value ToValue(double obj, object parent)
		{
			return Value.From((decimal)obj);
		}
	}
}
