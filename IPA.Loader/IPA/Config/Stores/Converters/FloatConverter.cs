using System;
using IPA.Config.Data;

namespace IPA.Config.Stores.Converters
{
	// Token: 0x02000082 RID: 130
	internal class FloatConverter : ValueConverter<float>
	{
		// Token: 0x0600036D RID: 877 RVA: 0x00012E20 File Offset: 0x00011020
		public override float FromValue(Value value, object parent)
		{
			return (float)Converter<decimal>.Default.FromValue(value, parent);
		}

		// Token: 0x0600036E RID: 878 RVA: 0x00012E34 File Offset: 0x00011034
		public override Value ToValue(float obj, object parent)
		{
			return Value.From((decimal)obj);
		}
	}
}
