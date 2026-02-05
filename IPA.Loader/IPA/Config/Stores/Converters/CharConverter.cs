using System;
using IPA.Config.Data;

namespace IPA.Config.Stores.Converters
{
	// Token: 0x02000076 RID: 118
	internal class CharConverter : ValueConverter<char>
	{
		// Token: 0x06000349 RID: 841 RVA: 0x00012BF5 File Offset: 0x00010DF5
		public override char FromValue(Value value, object parent)
		{
			Text text = value as Text;
			if (text == null)
			{
				throw new ArgumentException("Value not a text node", "value");
			}
			return text.Value[0];
		}

		// Token: 0x0600034A RID: 842 RVA: 0x00012C1C File Offset: 0x00010E1C
		public override Value ToValue(char obj, object parent)
		{
			return Value.From(char.ToString(obj));
		}
	}
}
