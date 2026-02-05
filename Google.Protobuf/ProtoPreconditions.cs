using System;

namespace Google.Protobuf
{
	// Token: 0x02000023 RID: 35
	public static class ProtoPreconditions
	{
		// Token: 0x060001EA RID: 490 RVA: 0x00009B6C File Offset: 0x00007D6C
		public static T CheckNotNull<T>(T value, string name) where T : class
		{
			if (value == null)
			{
				throw new ArgumentNullException(name);
			}
			return value;
		}

		// Token: 0x060001EB RID: 491 RVA: 0x00009B7E File Offset: 0x00007D7E
		internal static T CheckNotNullUnconstrained<T>(T value, string name)
		{
			if (value == null)
			{
				throw new ArgumentNullException(name);
			}
			return value;
		}
	}
}
