using System;

namespace Microsoft.CSharp.RuntimeBinder.Errors
{
	// Token: 0x020000C8 RID: 200
	internal sealed class ErrArgIds : ErrArg
	{
		// Token: 0x0600067D RID: 1661 RVA: 0x0001E9B9 File Offset: 0x0001CBB9
		public ErrArgIds(MessageID ids)
		{
			this.eak = ErrArgKind.Ids;
			this.eaf = ErrArgFlags.None;
			this.ids = ids;
		}
	}
}
