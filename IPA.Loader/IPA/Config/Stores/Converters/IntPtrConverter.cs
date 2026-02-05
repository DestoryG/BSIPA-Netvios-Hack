using System;
using IPA.Config.Data;

namespace IPA.Config.Stores.Converters
{
	// Token: 0x02000079 RID: 121
	internal class IntPtrConverter : ValueConverter<IntPtr>
	{
		// Token: 0x06000352 RID: 850 RVA: 0x00012CC6 File Offset: 0x00010EC6
		public override IntPtr FromValue(Value value, object parent)
		{
			return (IntPtr)Converter<long>.Default.FromValue(value, parent);
		}

		// Token: 0x06000353 RID: 851 RVA: 0x00012CD9 File Offset: 0x00010ED9
		public override Value ToValue(IntPtr obj, object parent)
		{
			return Value.From((long)obj);
		}
	}
}
