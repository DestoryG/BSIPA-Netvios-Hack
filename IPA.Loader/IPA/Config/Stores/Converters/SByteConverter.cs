using System;
using IPA.Config.Data;

namespace IPA.Config.Stores.Converters
{
	// Token: 0x02000080 RID: 128
	internal class SByteConverter : ValueConverter<sbyte>
	{
		// Token: 0x06000367 RID: 871 RVA: 0x00012DBB File Offset: 0x00010FBB
		public override sbyte FromValue(Value value, object parent)
		{
			return (sbyte)Converter<long>.Default.FromValue(value, parent);
		}

		// Token: 0x06000368 RID: 872 RVA: 0x00012DCA File Offset: 0x00010FCA
		public override Value ToValue(sbyte obj, object parent)
		{
			return Value.From((long)obj);
		}
	}
}
