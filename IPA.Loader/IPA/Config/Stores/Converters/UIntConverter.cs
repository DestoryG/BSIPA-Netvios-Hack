using System;
using IPA.Config.Data;

namespace IPA.Config.Stores.Converters
{
	// Token: 0x0200007C RID: 124
	internal class UIntConverter : ValueConverter<uint>
	{
		// Token: 0x0600035B RID: 859 RVA: 0x00012D3B File Offset: 0x00010F3B
		public override uint FromValue(Value value, object parent)
		{
			return (uint)Converter<long>.Default.FromValue(value, parent);
		}

		// Token: 0x0600035C RID: 860 RVA: 0x00012D4A File Offset: 0x00010F4A
		public override Value ToValue(uint obj, object parent)
		{
			return Value.From((long)((ulong)obj));
		}
	}
}
