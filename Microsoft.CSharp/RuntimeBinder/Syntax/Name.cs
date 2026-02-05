using System;

namespace Microsoft.CSharp.RuntimeBinder.Syntax
{
	// Token: 0x02000026 RID: 38
	internal sealed class Name
	{
		// Token: 0x0600016B RID: 363 RVA: 0x00009CBC File Offset: 0x00007EBC
		public Name(string text)
		{
			this.Text = text;
		}

		// Token: 0x1700005D RID: 93
		// (get) Token: 0x0600016C RID: 364 RVA: 0x00009CCB File Offset: 0x00007ECB
		public string Text { get; }

		// Token: 0x0600016D RID: 365 RVA: 0x00009CD3 File Offset: 0x00007ED3
		public override string ToString()
		{
			return this.Text;
		}
	}
}
