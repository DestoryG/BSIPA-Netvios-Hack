using System;
using Mono.CompilerServices.SymbolWriter;

namespace Mono.Cecil.Mdb
{
	// Token: 0x02000222 RID: 546
	internal static class MethodEntryExtensions
	{
		// Token: 0x0600104A RID: 4170 RVA: 0x00037A77 File Offset: 0x00035C77
		public static bool HasColumnInfo(this MethodEntry entry)
		{
			return (entry.MethodFlags & MethodEntry.Flags.ColumnsInfoIncluded) > (MethodEntry.Flags)0;
		}

		// Token: 0x0600104B RID: 4171 RVA: 0x00037A84 File Offset: 0x00035C84
		public static bool HasEndInfo(this MethodEntry entry)
		{
			return (entry.MethodFlags & MethodEntry.Flags.EndInfoIncluded) > (MethodEntry.Flags)0;
		}
	}
}
