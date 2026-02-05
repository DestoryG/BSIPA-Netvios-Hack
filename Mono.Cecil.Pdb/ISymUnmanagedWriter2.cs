using System;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using Mono.Cecil.Cil;

namespace Mono.Cecil.Pdb
{
	// Token: 0x02000003 RID: 3
	[Guid("0B97726E-9E6D-4f05-9A26-424022093CAA")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[ComImport]
	internal interface ISymUnmanagedWriter2
	{
		// Token: 0x06000001 RID: 1
		void DefineDocument([MarshalAs(UnmanagedType.LPWStr)] [In] string url, [In] ref Guid langauge, [In] ref Guid languageVendor, [In] ref Guid documentType, [MarshalAs(UnmanagedType.Interface)] out ISymUnmanagedDocumentWriter pRetVal);

		// Token: 0x06000002 RID: 2
		void SetUserEntryPoint([In] int methodToken);

		// Token: 0x06000003 RID: 3
		void OpenMethod([In] int methodToken);

		// Token: 0x06000004 RID: 4
		void CloseMethod();

		// Token: 0x06000005 RID: 5
		void OpenScope([In] int startOffset, out int pRetVal);

		// Token: 0x06000006 RID: 6
		void CloseScope([In] int endOffset);

		// Token: 0x06000007 RID: 7
		void SetScopeRange_Placeholder();

		// Token: 0x06000008 RID: 8
		void DefineLocalVariable_Placeholder();

		// Token: 0x06000009 RID: 9
		void DefineParameter_Placeholder();

		// Token: 0x0600000A RID: 10
		void DefineField_Placeholder();

		// Token: 0x0600000B RID: 11
		void DefineGlobalVariable_Placeholder();

		// Token: 0x0600000C RID: 12
		void Close();

		// Token: 0x0600000D RID: 13
		void SetSymAttribute(uint parent, string name, uint data, IntPtr signature);

		// Token: 0x0600000E RID: 14
		void OpenNamespace([MarshalAs(UnmanagedType.LPWStr)] [In] string name);

		// Token: 0x0600000F RID: 15
		void CloseNamespace();

		// Token: 0x06000010 RID: 16
		void UsingNamespace([MarshalAs(UnmanagedType.LPWStr)] [In] string fullName);

		// Token: 0x06000011 RID: 17
		void SetMethodSourceRange_Placeholder();

		// Token: 0x06000012 RID: 18
		void Initialize([MarshalAs(UnmanagedType.IUnknown)] [In] object emitter, [MarshalAs(UnmanagedType.LPWStr)] [In] string filename, [In] IStream pIStream, [In] bool fFullBuild);

		// Token: 0x06000013 RID: 19
		void GetDebugInfo(out ImageDebugDirectory pIDD, [In] int cData, out int pcData, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 1)] [In] [Out] byte[] data);

		// Token: 0x06000014 RID: 20
		void DefineSequencePoints([MarshalAs(UnmanagedType.Interface)] [In] ISymUnmanagedDocumentWriter document, [In] int spCount, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 1)] [In] int[] offsets, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 1)] [In] int[] lines, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 1)] [In] int[] columns, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 1)] [In] int[] endLines, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 1)] [In] int[] endColumns);

		// Token: 0x06000015 RID: 21
		void RemapToken_Placeholder();

		// Token: 0x06000016 RID: 22
		void Initialize2_Placeholder();

		// Token: 0x06000017 RID: 23
		void DefineConstant_Placeholder();

		// Token: 0x06000018 RID: 24
		void Abort_Placeholder();

		// Token: 0x06000019 RID: 25
		void DefineLocalVariable2([MarshalAs(UnmanagedType.LPWStr)] [In] string name, [In] int attributes, [In] int sigToken, [In] int addrKind, [In] int addr1, [In] int addr2, [In] int addr3, [In] int startOffset, [In] int endOffset);

		// Token: 0x0600001A RID: 26
		void DefineGlobalVariable2_Placeholder();

		// Token: 0x0600001B RID: 27
		void DefineConstant2([MarshalAs(UnmanagedType.LPWStr)] [In] string name, [MarshalAs(UnmanagedType.Struct)] [In] object variant, [In] int sigToken);
	}
}
