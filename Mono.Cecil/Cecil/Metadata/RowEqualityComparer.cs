using System;
using System.Collections.Generic;

namespace Mono.Cecil.Metadata
{
	// Token: 0x020000ED RID: 237
	internal sealed class RowEqualityComparer : IEqualityComparer<Row<string, string>>, IEqualityComparer<Row<uint, uint>>, IEqualityComparer<Row<uint, uint, uint>>
	{
		// Token: 0x06000984 RID: 2436 RVA: 0x0001EC9A File Offset: 0x0001CE9A
		public bool Equals(Row<string, string> x, Row<string, string> y)
		{
			return x.Col1 == y.Col1 && x.Col2 == y.Col2;
		}

		// Token: 0x06000985 RID: 2437 RVA: 0x0001ECC4 File Offset: 0x0001CEC4
		public int GetHashCode(Row<string, string> obj)
		{
			string col = obj.Col1;
			string col2 = obj.Col2;
			return ((col != null) ? col.GetHashCode() : 0) ^ ((col2 != null) ? col2.GetHashCode() : 0);
		}

		// Token: 0x06000986 RID: 2438 RVA: 0x0001ECF8 File Offset: 0x0001CEF8
		public bool Equals(Row<uint, uint> x, Row<uint, uint> y)
		{
			return x.Col1 == y.Col1 && x.Col2 == y.Col2;
		}

		// Token: 0x06000987 RID: 2439 RVA: 0x0001ED18 File Offset: 0x0001CF18
		public int GetHashCode(Row<uint, uint> obj)
		{
			return (int)(obj.Col1 ^ obj.Col2);
		}

		// Token: 0x06000988 RID: 2440 RVA: 0x0001ED27 File Offset: 0x0001CF27
		public bool Equals(Row<uint, uint, uint> x, Row<uint, uint, uint> y)
		{
			return x.Col1 == y.Col1 && x.Col2 == y.Col2 && x.Col3 == y.Col3;
		}

		// Token: 0x06000989 RID: 2441 RVA: 0x0001ED55 File Offset: 0x0001CF55
		public int GetHashCode(Row<uint, uint, uint> obj)
		{
			return (int)(obj.Col1 ^ obj.Col2 ^ obj.Col3);
		}
	}
}
