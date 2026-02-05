using System;
using Microsoft.CSharp.RuntimeBinder.Semantics;

namespace Microsoft.CSharp.RuntimeBinder
{
	// Token: 0x0200001B RID: 27
	internal interface ICSharpBinder
	{
		// Token: 0x060000DE RID: 222
		CSharpArgumentInfo GetArgumentInfo(int index);

		// Token: 0x17000050 RID: 80
		// (get) Token: 0x060000DF RID: 223
		Type CallingContext { get; }

		// Token: 0x17000051 RID: 81
		// (get) Token: 0x060000E0 RID: 224
		bool IsChecked { get; }

		// Token: 0x17000052 RID: 82
		// (get) Token: 0x060000E1 RID: 225
		bool IsBinderThatCanHaveRefReceiver { get; }

		// Token: 0x060000E2 RID: 226
		void PopulateSymbolTableWithName(SymbolTable symbolTable, Type callingType, ArgumentObject[] arguments);

		// Token: 0x060000E3 RID: 227
		Expr DispatchPayload(RuntimeBinder runtimeBinder, ArgumentObject[] arguments, LocalVariableSymbol[] locals);

		// Token: 0x17000053 RID: 83
		// (get) Token: 0x060000E4 RID: 228
		BindingFlag BindingFlags { get; }

		// Token: 0x17000054 RID: 84
		// (get) Token: 0x060000E5 RID: 229
		string Name { get; }
	}
}
