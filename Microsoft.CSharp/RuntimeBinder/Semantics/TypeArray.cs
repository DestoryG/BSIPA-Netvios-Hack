using System;
using System.Diagnostics;

namespace Microsoft.CSharp.RuntimeBinder.Semantics
{
	// Token: 0x020000AE RID: 174
	internal sealed class TypeArray
	{
		// Token: 0x060005D8 RID: 1496 RVA: 0x0001C89C File Offset: 0x0001AA9C
		public TypeArray(CType[] types)
		{
			this.Items = types;
		}

		// Token: 0x170000EB RID: 235
		// (get) Token: 0x060005D9 RID: 1497 RVA: 0x0001C8AB File Offset: 0x0001AAAB
		public int Count
		{
			get
			{
				return this.Items.Length;
			}
		}

		// Token: 0x170000EC RID: 236
		// (get) Token: 0x060005DA RID: 1498 RVA: 0x0001C8B5 File Offset: 0x0001AAB5
		public CType[] Items { get; }

		// Token: 0x170000ED RID: 237
		public CType this[int i]
		{
			get
			{
				return this.Items[i];
			}
		}

		// Token: 0x060005DC RID: 1500 RVA: 0x0001C8C7 File Offset: 0x0001AAC7
		[Conditional("DEBUG")]
		public void AssertValid()
		{
		}

		// Token: 0x060005DD RID: 1501 RVA: 0x0001C8C9 File Offset: 0x0001AAC9
		public void CopyItems(int i, int c, CType[] dest)
		{
			Array.Copy(this.Items, i, dest, 0, c);
		}
	}
}
