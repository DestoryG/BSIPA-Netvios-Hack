using System;
using System.Runtime.CompilerServices;

namespace HarmonyLib
{
	// Token: 0x02000040 RID: 64
	internal class Sandbox
	{
		// Token: 0x06000147 RID: 327 RVA: 0x000091A0 File Offset: 0x000073A0
		[MethodImpl(MethodImplOptions.NoInlining)]
		internal Sandbox.SomeStruct GetStruct(IntPtr x, IntPtr y)
		{
			throw new Exception("This method should've been detoured!");
		}

		// Token: 0x06000148 RID: 328 RVA: 0x000091AC File Offset: 0x000073AC
		internal static void GetStructReplacement(Sandbox self, IntPtr ptr, IntPtr a, IntPtr b)
		{
			Sandbox.hasStructReturnBuffer = a == Sandbox.magicValue && b == Sandbox.magicValue;
		}

		// Token: 0x040000CF RID: 207
		internal static bool hasStructReturnBuffer;

		// Token: 0x040000D0 RID: 208
		internal static readonly IntPtr magicValue = (IntPtr)305419896;

		// Token: 0x02000041 RID: 65
		internal struct SomeStruct
		{
			// Token: 0x040000D1 RID: 209
			private readonly byte b1;

			// Token: 0x040000D2 RID: 210
			private readonly byte b2;

			// Token: 0x040000D3 RID: 211
			private readonly byte b3;
		}
	}
}
