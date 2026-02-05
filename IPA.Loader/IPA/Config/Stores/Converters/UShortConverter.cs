using System;
using IPA.Config.Data;

namespace IPA.Config.Stores.Converters
{
	// Token: 0x0200007E RID: 126
	internal class UShortConverter : ValueConverter<ushort>
	{
		// Token: 0x06000361 RID: 865 RVA: 0x00012D7B File Offset: 0x00010F7B
		public override ushort FromValue(Value value, object parent)
		{
			return (ushort)Converter<long>.Default.FromValue(value, parent);
		}

		// Token: 0x06000362 RID: 866 RVA: 0x00012D8A File Offset: 0x00010F8A
		public override Value ToValue(ushort obj, object parent)
		{
			return Value.From((long)((ulong)obj));
		}
	}
}
