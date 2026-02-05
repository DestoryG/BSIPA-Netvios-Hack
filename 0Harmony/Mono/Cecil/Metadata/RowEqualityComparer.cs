using System;
using System.Collections.Generic;

namespace Mono.Cecil.Metadata
{
	// Token: 0x020001B1 RID: 433
	internal sealed class RowEqualityComparer : IEqualityComparer<Row<string, string>>, IEqualityComparer<Row<uint, uint>>, IEqualityComparer<Row<uint, uint, uint>>
	{
		// Token: 0x06000D68 RID: 3432 RVA: 0x0002DE32 File Offset: 0x0002C032
		public bool Equals(Row<string, string> x, Row<string, string> y)
		{
			return x.Col1 == y.Col1 && x.Col2 == y.Col2;
		}

		// Token: 0x06000D69 RID: 3433 RVA: 0x0002DE5C File Offset: 0x0002C05C
		public int GetHashCode(Row<string, string> obj)
		{
			string col = obj.Col1;
			string col2 = obj.Col2;
			return ((col != null) ? col.GetHashCode() : 0) ^ ((col2 != null) ? col2.GetHashCode() : 0);
		}

		// Token: 0x06000D6A RID: 3434 RVA: 0x0002DE90 File Offset: 0x0002C090
		public bool Equals(Row<uint, uint> x, Row<uint, uint> y)
		{
			return x.Col1 == y.Col1 && x.Col2 == y.Col2;
		}

		// Token: 0x06000D6B RID: 3435 RVA: 0x0002DEB0 File Offset: 0x0002C0B0
		public int GetHashCode(Row<uint, uint> obj)
		{
			return (int)(obj.Col1 ^ obj.Col2);
		}

		// Token: 0x06000D6C RID: 3436 RVA: 0x0002DEBF File Offset: 0x0002C0BF
		public bool Equals(Row<uint, uint, uint> x, Row<uint, uint, uint> y)
		{
			return x.Col1 == y.Col1 && x.Col2 == y.Col2 && x.Col3 == y.Col3;
		}

		// Token: 0x06000D6D RID: 3437 RVA: 0x0002DEED File Offset: 0x0002C0ED
		public int GetHashCode(Row<uint, uint, uint> obj)
		{
			return (int)(obj.Col1 ^ obj.Col2 ^ obj.Col3);
		}
	}
}
