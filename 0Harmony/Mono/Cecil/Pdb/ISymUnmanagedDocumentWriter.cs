using System;
using System.Runtime.InteropServices;

namespace Mono.Cecil.Pdb
{
	// Token: 0x02000227 RID: 551
	[Guid("B01FAFEB-C450-3A4D-BEEC-B4CEEC01E006")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[ComImport]
	internal interface ISymUnmanagedDocumentWriter
	{
		// Token: 0x06001060 RID: 4192
		void SetSource(uint sourceSize, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 0)] byte[] source);

		// Token: 0x06001061 RID: 4193
		void SetCheckSum(Guid algorithmId, uint checkSumSize, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 1)] byte[] checkSum);
	}
}
