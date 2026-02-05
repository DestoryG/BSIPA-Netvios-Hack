using System;
using IPA.Config.Data;

namespace IPA.Config.Stores.Converters
{
	// Token: 0x02000084 RID: 132
	internal class BooleanConverter : ValueConverter<bool>
	{
		// Token: 0x06000373 RID: 883 RVA: 0x00012E72 File Offset: 0x00011072
		public override bool FromValue(Value value, object parent)
		{
			IPA.Config.Data.Boolean boolean = value as IPA.Config.Data.Boolean;
			if (boolean == null)
			{
				throw new ArgumentException("Value not a Boolean", "value");
			}
			return boolean.Value;
		}

		// Token: 0x06000374 RID: 884 RVA: 0x00012E93 File Offset: 0x00011093
		public override Value ToValue(bool obj, object parent)
		{
			return Value.From(obj);
		}
	}
}
