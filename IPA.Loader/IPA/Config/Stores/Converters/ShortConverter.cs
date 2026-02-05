using System;
using IPA.Config.Data;

namespace IPA.Config.Stores.Converters
{
	// Token: 0x0200007D RID: 125
	internal class ShortConverter : ValueConverter<short>
	{
		// Token: 0x0600035E RID: 862 RVA: 0x00012D5B File Offset: 0x00010F5B
		public override short FromValue(Value value, object parent)
		{
			return (short)Converter<long>.Default.FromValue(value, parent);
		}

		// Token: 0x0600035F RID: 863 RVA: 0x00012D6A File Offset: 0x00010F6A
		public override Value ToValue(short obj, object parent)
		{
			return Value.From((long)obj);
		}
	}
}
