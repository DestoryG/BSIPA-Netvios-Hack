using System;
using IPA.Config.Data;

namespace IPA.Config.Stores.Converters
{
	// Token: 0x0200007B RID: 123
	internal class IntConverter : ValueConverter<int>
	{
		// Token: 0x06000358 RID: 856 RVA: 0x00012D1B File Offset: 0x00010F1B
		public override int FromValue(Value value, object parent)
		{
			return (int)Converter<long>.Default.FromValue(value, parent);
		}

		// Token: 0x06000359 RID: 857 RVA: 0x00012D2A File Offset: 0x00010F2A
		public override Value ToValue(int obj, object parent)
		{
			return Value.From((long)obj);
		}
	}
}
