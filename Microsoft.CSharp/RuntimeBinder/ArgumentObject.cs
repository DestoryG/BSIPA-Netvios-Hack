using System;

namespace Microsoft.CSharp.RuntimeBinder
{
	// Token: 0x02000005 RID: 5
	internal struct ArgumentObject
	{
		// Token: 0x0600000A RID: 10 RVA: 0x000020B6 File Offset: 0x000002B6
		public ArgumentObject(object value, CSharpArgumentInfo info, Type type)
		{
			this.Value = value;
			this.Info = info;
			this.Type = type;
		}

		// Token: 0x0400008B RID: 139
		internal readonly object Value;

		// Token: 0x0400008C RID: 140
		internal readonly CSharpArgumentInfo Info;

		// Token: 0x0400008D RID: 141
		internal readonly Type Type;
	}
}
