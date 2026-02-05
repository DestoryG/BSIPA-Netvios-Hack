using System;
using IPA.Config.Data;

namespace IPA.Config.Stores.Converters
{
	// Token: 0x02000075 RID: 117
	internal class StringConverter : ValueConverter<string>
	{
		// Token: 0x06000346 RID: 838 RVA: 0x00012BD2 File Offset: 0x00010DD2
		public override string FromValue(Value value, object parent)
		{
			Text text = value as Text;
			if (text == null)
			{
				return null;
			}
			return text.Value;
		}

		// Token: 0x06000347 RID: 839 RVA: 0x00012BE5 File Offset: 0x00010DE5
		public override Value ToValue(string obj, object parent)
		{
			return Value.From(obj);
		}
	}
}
