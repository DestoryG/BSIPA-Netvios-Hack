using System;

namespace Microsoft.CSharp.RuntimeBinder.Semantics
{
	// Token: 0x0200004A RID: 74
	internal enum ExpressionKind
	{
		// Token: 0x0400034B RID: 843
		Block,
		// Token: 0x0400034C RID: 844
		Return,
		// Token: 0x0400034D RID: 845
		NoOp,
		// Token: 0x0400034E RID: 846
		BinaryOp,
		// Token: 0x0400034F RID: 847
		UnaryOp,
		// Token: 0x04000350 RID: 848
		Assignment,
		// Token: 0x04000351 RID: 849
		List,
		// Token: 0x04000352 RID: 850
		ArrayIndex,
		// Token: 0x04000353 RID: 851
		Call,
		// Token: 0x04000354 RID: 852
		Field,
		// Token: 0x04000355 RID: 853
		Local,
		// Token: 0x04000356 RID: 854
		Constant,
		// Token: 0x04000357 RID: 855
		Class,
		// Token: 0x04000358 RID: 856
		Property,
		// Token: 0x04000359 RID: 857
		Multi,
		// Token: 0x0400035A RID: 858
		MultiGet,
		// Token: 0x0400035B RID: 859
		Wrap,
		// Token: 0x0400035C RID: 860
		Concat,
		// Token: 0x0400035D RID: 861
		ArrayInit,
		// Token: 0x0400035E RID: 862
		Cast,
		// Token: 0x0400035F RID: 863
		UserDefinedConversion,
		// Token: 0x04000360 RID: 864
		TypeOf,
		// Token: 0x04000361 RID: 865
		ZeroInit,
		// Token: 0x04000362 RID: 866
		UserLogicalOp,
		// Token: 0x04000363 RID: 867
		MemberGroup,
		// Token: 0x04000364 RID: 868
		BoundLambda,
		// Token: 0x04000365 RID: 869
		HoistedLocalExpression,
		// Token: 0x04000366 RID: 870
		FieldInfo,
		// Token: 0x04000367 RID: 871
		MethodInfo,
		// Token: 0x04000368 RID: 872
		PropertyInfo,
		// Token: 0x04000369 RID: 873
		NamedArgumentSpecification,
		// Token: 0x0400036A RID: 874
		ExpressionKindCount,
		// Token: 0x0400036B RID: 875
		EqualsParam,
		// Token: 0x0400036C RID: 876
		FirstOp = 32,
		// Token: 0x0400036D RID: 877
		Compare,
		// Token: 0x0400036E RID: 878
		True,
		// Token: 0x0400036F RID: 879
		False,
		// Token: 0x04000370 RID: 880
		Inc,
		// Token: 0x04000371 RID: 881
		Dec,
		// Token: 0x04000372 RID: 882
		LogicalNot,
		// Token: 0x04000373 RID: 883
		Eq,
		// Token: 0x04000374 RID: 884
		RelationalMin = 39,
		// Token: 0x04000375 RID: 885
		NotEq,
		// Token: 0x04000376 RID: 886
		LessThan,
		// Token: 0x04000377 RID: 887
		LessThanOrEqual,
		// Token: 0x04000378 RID: 888
		GreaterThan,
		// Token: 0x04000379 RID: 889
		GreaterThanOrEqual,
		// Token: 0x0400037A RID: 890
		RelationalMax = 44,
		// Token: 0x0400037B RID: 891
		Add,
		// Token: 0x0400037C RID: 892
		Subtract,
		// Token: 0x0400037D RID: 893
		Multiply,
		// Token: 0x0400037E RID: 894
		Divide,
		// Token: 0x0400037F RID: 895
		Modulo,
		// Token: 0x04000380 RID: 896
		Negate,
		// Token: 0x04000381 RID: 897
		UnaryPlus,
		// Token: 0x04000382 RID: 898
		BitwiseAnd,
		// Token: 0x04000383 RID: 899
		BitwiseOr,
		// Token: 0x04000384 RID: 900
		BitwiseExclusiveOr,
		// Token: 0x04000385 RID: 901
		BitwiseNot,
		// Token: 0x04000386 RID: 902
		LeftShirt,
		// Token: 0x04000387 RID: 903
		RightShift,
		// Token: 0x04000388 RID: 904
		LogicalAnd,
		// Token: 0x04000389 RID: 905
		LogicalOr,
		// Token: 0x0400038A RID: 906
		Sequence,
		// Token: 0x0400038B RID: 907
		SequenceReverse,
		// Token: 0x0400038C RID: 908
		Save,
		// Token: 0x0400038D RID: 909
		Swap,
		// Token: 0x0400038E RID: 910
		Indir,
		// Token: 0x0400038F RID: 911
		Addr,
		// Token: 0x04000390 RID: 912
		StringEq,
		// Token: 0x04000391 RID: 913
		StringNotEq,
		// Token: 0x04000392 RID: 914
		DelegateEq,
		// Token: 0x04000393 RID: 915
		DelegateNotEq,
		// Token: 0x04000394 RID: 916
		DelegateAdd,
		// Token: 0x04000395 RID: 917
		DelegateSubtract,
		// Token: 0x04000396 RID: 918
		DecimalNegate,
		// Token: 0x04000397 RID: 919
		DecimalInc,
		// Token: 0x04000398 RID: 920
		DecimalDec,
		// Token: 0x04000399 RID: 921
		MultiOffset,
		// Token: 0x0400039A RID: 922
		TypeLimit = 31
	}
}
