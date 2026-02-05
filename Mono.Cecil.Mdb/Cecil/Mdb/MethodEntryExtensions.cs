using System;
using Mono.CompilerServices.SymbolWriter;

namespace Mono.Cecil.Mdb
{
	// Token: 0x0200001E RID: 30
	internal static class MethodEntryExtensions
	{
		// Token: 0x060000EA RID: 234 RVA: 0x00005D3A File Offset: 0x00003F3A
		public static bool HasColumnInfo(this MethodEntry entry)
		{
			return (entry.MethodFlags & MethodEntry.Flags.ColumnsInfoIncluded) > (MethodEntry.Flags)0;
		}

		// Token: 0x060000EB RID: 235 RVA: 0x00005D47 File Offset: 0x00003F47
		public static bool HasEndInfo(this MethodEntry entry)
		{
			return (entry.MethodFlags & MethodEntry.Flags.EndInfoIncluded) > (MethodEntry.Flags)0;
		}
	}
}
