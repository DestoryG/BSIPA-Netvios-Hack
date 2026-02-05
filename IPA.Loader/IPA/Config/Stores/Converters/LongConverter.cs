using System;
using IPA.Config.Data;

namespace IPA.Config.Stores.Converters
{
	// Token: 0x02000077 RID: 119
	internal class LongConverter : ValueConverter<long>
	{
		// Token: 0x0600034C RID: 844 RVA: 0x00012C34 File Offset: 0x00010E34
		public override long FromValue(Value value, object parent)
		{
			long? num = Converter.IntValue(value);
			if (num == null)
			{
				throw new ArgumentException("Value not a numeric value", "value");
			}
			return num.GetValueOrDefault();
		}

		// Token: 0x0600034D RID: 845 RVA: 0x00012C68 File Offset: 0x00010E68
		public override Value ToValue(long obj, object parent)
		{
			return Value.From(obj);
		}
	}
}
