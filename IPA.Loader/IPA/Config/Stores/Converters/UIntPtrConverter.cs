using System;
using IPA.Config.Data;

namespace IPA.Config.Stores.Converters
{
	// Token: 0x0200007A RID: 122
	internal class UIntPtrConverter : ValueConverter<UIntPtr>
	{
		// Token: 0x06000355 RID: 853 RVA: 0x00012CEE File Offset: 0x00010EEE
		public override UIntPtr FromValue(Value value, object parent)
		{
			return (UIntPtr)Converter<ulong>.Default.FromValue(value, parent);
		}

		// Token: 0x06000356 RID: 854 RVA: 0x00012D01 File Offset: 0x00010F01
		public override Value ToValue(UIntPtr obj, object parent)
		{
			return Value.From((ulong)obj);
		}
	}
}
