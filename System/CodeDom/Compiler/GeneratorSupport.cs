using System;

namespace System.CodeDom.Compiler
{
	// Token: 0x0200067D RID: 1661
	[Flags]
	[Serializable]
	public enum GeneratorSupport
	{
		// Token: 0x04002C94 RID: 11412
		ArraysOfArrays = 1,
		// Token: 0x04002C95 RID: 11413
		EntryPointMethod = 2,
		// Token: 0x04002C96 RID: 11414
		GotoStatements = 4,
		// Token: 0x04002C97 RID: 11415
		MultidimensionalArrays = 8,
		// Token: 0x04002C98 RID: 11416
		StaticConstructors = 16,
		// Token: 0x04002C99 RID: 11417
		TryCatchStatements = 32,
		// Token: 0x04002C9A RID: 11418
		ReturnTypeAttributes = 64,
		// Token: 0x04002C9B RID: 11419
		DeclareValueTypes = 128,
		// Token: 0x04002C9C RID: 11420
		DeclareEnums = 256,
		// Token: 0x04002C9D RID: 11421
		DeclareDelegates = 512,
		// Token: 0x04002C9E RID: 11422
		DeclareInterfaces = 1024,
		// Token: 0x04002C9F RID: 11423
		DeclareEvents = 2048,
		// Token: 0x04002CA0 RID: 11424
		AssemblyAttributes = 4096,
		// Token: 0x04002CA1 RID: 11425
		ParameterAttributes = 8192,
		// Token: 0x04002CA2 RID: 11426
		ReferenceParameters = 16384,
		// Token: 0x04002CA3 RID: 11427
		ChainedConstructorArguments = 32768,
		// Token: 0x04002CA4 RID: 11428
		NestedTypes = 65536,
		// Token: 0x04002CA5 RID: 11429
		MultipleInterfaceMembers = 131072,
		// Token: 0x04002CA6 RID: 11430
		PublicStaticMembers = 262144,
		// Token: 0x04002CA7 RID: 11431
		ComplexExpressions = 524288,
		// Token: 0x04002CA8 RID: 11432
		Win32Resources = 1048576,
		// Token: 0x04002CA9 RID: 11433
		Resources = 2097152,
		// Token: 0x04002CAA RID: 11434
		PartialTypes = 4194304,
		// Token: 0x04002CAB RID: 11435
		GenericTypeReference = 8388608,
		// Token: 0x04002CAC RID: 11436
		GenericTypeDeclaration = 16777216,
		// Token: 0x04002CAD RID: 11437
		DeclareIndexerProperties = 33554432
	}
}
