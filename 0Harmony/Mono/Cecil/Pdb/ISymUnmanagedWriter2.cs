using System;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using Mono.Cecil.Cil;

namespace Mono.Cecil.Pdb
{
	// Token: 0x02000228 RID: 552
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("0B97726E-9E6D-4f05-9A26-424022093CAA")]
	[ComImport]
	internal interface ISymUnmanagedWriter2
	{
		// Token: 0x06001062 RID: 4194
		void DefineDocument([MarshalAs(UnmanagedType.LPWStr)] [In] string url, [In] ref Guid langauge, [In] ref Guid languageVendor, [In] ref Guid documentType, [MarshalAs(UnmanagedType.Interface)] out ISymUnmanagedDocumentWriter pRetVal);

		// Token: 0x06001063 RID: 4195
		void SetUserEntryPoint([In] int methodToken);

		// Token: 0x06001064 RID: 4196
		void OpenMethod([In] int methodToken);

		// Token: 0x06001065 RID: 4197
		void CloseMethod();

		// Token: 0x06001066 RID: 4198
		void OpenScope([In] int startOffset, out int pRetVal);

		// Token: 0x06001067 RID: 4199
		void CloseScope([In] int endOffset);

		// Token: 0x06001068 RID: 4200
		void SetScopeRange_Placeholder();

		// Token: 0x06001069 RID: 4201
		void DefineLocalVariable_Placeholder();

		// Token: 0x0600106A RID: 4202
		void DefineParameter_Placeholder();

		// Token: 0x0600106B RID: 4203
		void DefineField_Placeholder();

		// Token: 0x0600106C RID: 4204
		void DefineGlobalVariable_Placeholder();

		// Token: 0x0600106D RID: 4205
		void Close();

		// Token: 0x0600106E RID: 4206
		void SetSymAttribute(uint parent, string name, uint data, IntPtr signature);

		// Token: 0x0600106F RID: 4207
		void OpenNamespace([MarshalAs(UnmanagedType.LPWStr)] [In] string name);

		// Token: 0x06001070 RID: 4208
		void CloseNamespace();

		// Token: 0x06001071 RID: 4209
		void UsingNamespace([MarshalAs(UnmanagedType.LPWStr)] [In] string fullName);

		// Token: 0x06001072 RID: 4210
		void SetMethodSourceRange_Placeholder();

		// Token: 0x06001073 RID: 4211
		void Initialize([MarshalAs(UnmanagedType.IUnknown)] [In] object emitter, [MarshalAs(UnmanagedType.LPWStr)] [In] string filename, [In] IStream pIStream, [In] bool fFullBuild);

		// Token: 0x06001074 RID: 4212
		void GetDebugInfo(out ImageDebugDirectory pIDD, [In] int cData, out int pcData, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 1)] [In] [Out] byte[] data);

		// Token: 0x06001075 RID: 4213
		void DefineSequencePoints([MarshalAs(UnmanagedType.Interface)] [In] ISymUnmanagedDocumentWriter document, [In] int spCount, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 1)] [In] int[] offsets, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 1)] [In] int[] lines, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 1)] [In] int[] columns, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 1)] [In] int[] endLines, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 1)] [In] int[] endColumns);

		// Token: 0x06001076 RID: 4214
		void RemapToken_Placeholder();

		// Token: 0x06001077 RID: 4215
		void Initialize2_Placeholder();

		// Token: 0x06001078 RID: 4216
		void DefineConstant_Placeholder();

		// Token: 0x06001079 RID: 4217
		void Abort_Placeholder();

		// Token: 0x0600107A RID: 4218
		void DefineLocalVariable2([MarshalAs(UnmanagedType.LPWStr)] [In] string name, [In] int attributes, [In] int sigToken, [In] int addrKind, [In] int addr1, [In] int addr2, [In] int addr3, [In] int startOffset, [In] int endOffset);

		// Token: 0x0600107B RID: 4219
		void DefineGlobalVariable2_Placeholder();

		// Token: 0x0600107C RID: 4220
		void DefineConstant2([MarshalAs(UnmanagedType.LPWStr)] [In] string name, [MarshalAs(UnmanagedType.Struct)] [In] object variant, [In] int sigToken);
	}
}
