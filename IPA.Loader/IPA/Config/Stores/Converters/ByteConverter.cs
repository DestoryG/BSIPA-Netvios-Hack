using System;
using IPA.Config.Data;

namespace IPA.Config.Stores.Converters
{
	// Token: 0x0200007F RID: 127
	internal class ByteConverter : ValueConverter<byte>
	{
		// Token: 0x06000364 RID: 868 RVA: 0x00012D9B File Offset: 0x00010F9B
		public override byte FromValue(Value value, object parent)
		{
			return (byte)Converter<long>.Default.FromValue(value, parent);
		}

		// Token: 0x06000365 RID: 869 RVA: 0x00012DAA File Offset: 0x00010FAA
		public override Value ToValue(byte obj, object parent)
		{
			return Value.From((long)((ulong)obj));
		}
	}
}
