using System;

namespace Microsoft.CSharp.RuntimeBinder.Semantics
{
	// Token: 0x0200009D RID: 157
	internal abstract class ExprVisitorBase
	{
		// Token: 0x060004E7 RID: 1255 RVA: 0x00018970 File Offset: 0x00016B70
		public Expr Visit(Expr pExpr)
		{
			if (pExpr == null)
			{
				return null;
			}
			Expr expr;
			if (this.IsCachedExpr(pExpr, out expr))
			{
				return expr;
			}
			ExprStatement exprStatement;
			if ((exprStatement = pExpr as ExprStatement) != null)
			{
				return this.CacheExprMapping(pExpr, this.DispatchStatementList(exprStatement));
			}
			return this.CacheExprMapping(pExpr, this.Dispatch(pExpr));
		}

		// Token: 0x060004E8 RID: 1256 RVA: 0x000189B8 File Offset: 0x00016BB8
		private ExprStatement DispatchStatementList(ExprStatement expr)
		{
			ExprStatement exprStatement = expr;
			ExprStatement exprStatement2 = exprStatement;
			while (exprStatement2 != null)
			{
				ExprStatement optionalNextStatement = exprStatement2.OptionalNextStatement;
				exprStatement2.OptionalNextStatement = null;
				ExprStatement exprStatement3 = this.Dispatch(exprStatement2) as ExprStatement;
				if (exprStatement2 == exprStatement)
				{
					exprStatement = exprStatement3;
				}
				else
				{
					exprStatement2.OptionalNextStatement = exprStatement3;
				}
				while (exprStatement2.OptionalNextStatement != null)
				{
					exprStatement2 = exprStatement2.OptionalNextStatement;
				}
				exprStatement2.OptionalNextStatement = optionalNextStatement;
			}
			return exprStatement;
		}

		// Token: 0x060004E9 RID: 1257 RVA: 0x00018A11 File Offset: 0x00016C11
		private bool IsCachedExpr(Expr pExpr, out Expr pTransformedExpr)
		{
			pTransformedExpr = null;
			return false;
		}

		// Token: 0x060004EA RID: 1258 RVA: 0x00018A17 File Offset: 0x00016C17
		private Expr CacheExprMapping(Expr pExpr, Expr pTransformedExpr)
		{
			return pTransformedExpr;
		}

		// Token: 0x060004EB RID: 1259 RVA: 0x00018A1C File Offset: 0x00016C1C
		protected virtual Expr Dispatch(Expr pExpr)
		{
			switch (pExpr.Kind)
			{
			case ExpressionKind.Block:
				return this.VisitBLOCK(pExpr as ExprBlock);
			case ExpressionKind.Return:
				return this.VisitRETURN(pExpr as ExprReturn);
			case ExpressionKind.BinaryOp:
				return this.VisitBINOP(pExpr as ExprBinOp);
			case ExpressionKind.UnaryOp:
				return this.VisitUNARYOP(pExpr as ExprUnaryOp);
			case ExpressionKind.Assignment:
				return this.VisitASSIGNMENT(pExpr as ExprAssignment);
			case ExpressionKind.List:
				return this.VisitLIST(pExpr as ExprList);
			case ExpressionKind.ArrayIndex:
				return this.VisitARRAYINDEX(pExpr as ExprArrayIndex);
			case ExpressionKind.Call:
				return this.VisitCALL(pExpr as ExprCall);
			case ExpressionKind.Field:
				return this.VisitFIELD(pExpr as ExprField);
			case ExpressionKind.Local:
				return this.VisitLOCAL(pExpr as ExprLocal);
			case ExpressionKind.Constant:
				return this.VisitCONSTANT(pExpr as ExprConstant);
			case ExpressionKind.Class:
				return this.VisitCLASS(pExpr as ExprClass);
			case ExpressionKind.Property:
				return this.VisitPROP(pExpr as ExprProperty);
			case ExpressionKind.Multi:
				return this.VisitMULTI(pExpr as ExprMulti);
			case ExpressionKind.MultiGet:
				return this.VisitMULTIGET(pExpr as ExprMultiGet);
			case ExpressionKind.Wrap:
				return this.VisitWRAP(pExpr as ExprWrap);
			case ExpressionKind.Concat:
				return this.VisitCONCAT(pExpr as ExprConcat);
			case ExpressionKind.ArrayInit:
				return this.VisitARRINIT(pExpr as ExprArrayInit);
			case ExpressionKind.Cast:
				return this.VisitCAST(pExpr as ExprCast);
			case ExpressionKind.UserDefinedConversion:
				return this.VisitUSERDEFINEDCONVERSION(pExpr as ExprUserDefinedConversion);
			case ExpressionKind.TypeOf:
				return this.VisitTYPEOF(pExpr as ExprTypeOf);
			case ExpressionKind.ZeroInit:
				return this.VisitZEROINIT(pExpr as ExprZeroInit);
			case ExpressionKind.UserLogicalOp:
				return this.VisitUSERLOGOP(pExpr as ExprUserLogicalOp);
			case ExpressionKind.MemberGroup:
				return this.VisitMEMGRP(pExpr as ExprMemberGroup);
			case ExpressionKind.BoundLambda:
				return this.VisitBOUNDLAMBDA(pExpr as ExprBoundLambda);
			case ExpressionKind.HoistedLocalExpression:
				return this.VisitHOISTEDLOCALEXPR(pExpr as ExprHoistedLocalExpr);
			case ExpressionKind.FieldInfo:
				return this.VisitFIELDINFO(pExpr as ExprFieldInfo);
			case ExpressionKind.MethodInfo:
				return this.VisitMETHODINFO(pExpr as ExprMethodInfo);
			case ExpressionKind.EqualsParam:
				return this.VisitEQUALS(pExpr as ExprBinOp);
			case ExpressionKind.Compare:
				return this.VisitCOMPARE(pExpr as ExprBinOp);
			case ExpressionKind.True:
				return this.VisitTRUE(pExpr as ExprUnaryOp);
			case ExpressionKind.False:
				return this.VisitFALSE(pExpr as ExprUnaryOp);
			case ExpressionKind.Inc:
				return this.VisitINC(pExpr as ExprUnaryOp);
			case ExpressionKind.Dec:
				return this.VisitDEC(pExpr as ExprUnaryOp);
			case ExpressionKind.LogicalNot:
				return this.VisitLOGNOT(pExpr as ExprUnaryOp);
			case ExpressionKind.Eq:
				return this.VisitEQ(pExpr as ExprBinOp);
			case ExpressionKind.NotEq:
				return this.VisitNE(pExpr as ExprBinOp);
			case ExpressionKind.LessThan:
				return this.VisitLT(pExpr as ExprBinOp);
			case ExpressionKind.LessThanOrEqual:
				return this.VisitLE(pExpr as ExprBinOp);
			case ExpressionKind.GreaterThan:
				return this.VisitGT(pExpr as ExprBinOp);
			case ExpressionKind.GreaterThanOrEqual:
				return this.VisitGE(pExpr as ExprBinOp);
			case ExpressionKind.Add:
				return this.VisitADD(pExpr as ExprBinOp);
			case ExpressionKind.Subtract:
				return this.VisitSUB(pExpr as ExprBinOp);
			case ExpressionKind.Multiply:
				return this.VisitMUL(pExpr as ExprBinOp);
			case ExpressionKind.Divide:
				return this.VisitDIV(pExpr as ExprBinOp);
			case ExpressionKind.Modulo:
				return this.VisitMOD(pExpr as ExprBinOp);
			case ExpressionKind.Negate:
				return this.VisitNEG(pExpr as ExprUnaryOp);
			case ExpressionKind.UnaryPlus:
				return this.VisitUPLUS(pExpr as ExprUnaryOp);
			case ExpressionKind.BitwiseAnd:
				return this.VisitBITAND(pExpr as ExprBinOp);
			case ExpressionKind.BitwiseOr:
				return this.VisitBITOR(pExpr as ExprBinOp);
			case ExpressionKind.BitwiseExclusiveOr:
				return this.VisitBITXOR(pExpr as ExprBinOp);
			case ExpressionKind.BitwiseNot:
				return this.VisitBITNOT(pExpr as ExprUnaryOp);
			case ExpressionKind.LeftShirt:
				return this.VisitLSHIFT(pExpr as ExprBinOp);
			case ExpressionKind.RightShift:
				return this.VisitRSHIFT(pExpr as ExprBinOp);
			case ExpressionKind.LogicalAnd:
				return this.VisitLOGAND(pExpr as ExprBinOp);
			case ExpressionKind.LogicalOr:
				return this.VisitLOGOR(pExpr as ExprBinOp);
			case ExpressionKind.Sequence:
				return this.VisitSEQUENCE(pExpr as ExprBinOp);
			case ExpressionKind.SequenceReverse:
				return this.VisitSEQREV(pExpr as ExprBinOp);
			case ExpressionKind.Save:
				return this.VisitSAVE(pExpr as ExprBinOp);
			case ExpressionKind.Swap:
				return this.VisitSWAP(pExpr as ExprBinOp);
			case ExpressionKind.Indir:
				return this.VisitINDIR(pExpr as ExprBinOp);
			case ExpressionKind.Addr:
				return this.VisitADDR(pExpr as ExprUnaryOp);
			case ExpressionKind.StringEq:
				return this.VisitSTRINGEQ(pExpr as ExprBinOp);
			case ExpressionKind.StringNotEq:
				return this.VisitSTRINGNE(pExpr as ExprBinOp);
			case ExpressionKind.DelegateEq:
				return this.VisitDELEGATEEQ(pExpr as ExprBinOp);
			case ExpressionKind.DelegateNotEq:
				return this.VisitDELEGATENE(pExpr as ExprBinOp);
			case ExpressionKind.DelegateAdd:
				return this.VisitDELEGATEADD(pExpr as ExprBinOp);
			case ExpressionKind.DelegateSubtract:
				return this.VisitDELEGATESUB(pExpr as ExprBinOp);
			case ExpressionKind.DecimalNegate:
				return this.VisitDECIMALNEG(pExpr as ExprUnaryOp);
			case ExpressionKind.DecimalInc:
				return this.VisitDECIMALINC(pExpr as ExprUnaryOp);
			case ExpressionKind.DecimalDec:
				return this.VisitDECIMALDEC(pExpr as ExprUnaryOp);
			}
			throw Error.InternalCompilerError();
		}

		// Token: 0x060004EC RID: 1260 RVA: 0x00018F08 File Offset: 0x00017108
		private void VisitChildren(Expr pExpr)
		{
			Expr expr;
			switch (pExpr.Kind)
			{
			case ExpressionKind.Block:
				expr = this.Visit((pExpr as ExprBlock).OptionalStatements);
				(pExpr as ExprBlock).OptionalStatements = expr as ExprStatement;
				return;
			case ExpressionKind.Return:
				expr = this.Visit((pExpr as ExprReturn).OptionalObject);
				(pExpr as ExprReturn).OptionalObject = expr;
				return;
			case ExpressionKind.NoOp:
			case ExpressionKind.Local:
			case ExpressionKind.Class:
			case ExpressionKind.MultiGet:
			case ExpressionKind.Wrap:
			case ExpressionKind.ZeroInit:
			case ExpressionKind.HoistedLocalExpression:
			case ExpressionKind.FieldInfo:
			case ExpressionKind.MethodInfo:
				return;
			case ExpressionKind.UnaryOp:
			case ExpressionKind.True:
			case ExpressionKind.False:
			case ExpressionKind.Inc:
			case ExpressionKind.Dec:
			case ExpressionKind.LogicalNot:
			case ExpressionKind.Negate:
			case ExpressionKind.UnaryPlus:
			case ExpressionKind.BitwiseNot:
			case ExpressionKind.Addr:
			case ExpressionKind.DecimalNegate:
			case ExpressionKind.DecimalInc:
			case ExpressionKind.DecimalDec:
				expr = this.Visit((pExpr as ExprUnaryOp).Child);
				(pExpr as ExprUnaryOp).Child = expr;
				return;
			case ExpressionKind.Assignment:
				expr = this.Visit((pExpr as ExprAssignment).LHS);
				(pExpr as ExprAssignment).LHS = expr;
				expr = this.Visit((pExpr as ExprAssignment).RHS);
				(pExpr as ExprAssignment).RHS = expr;
				return;
			case ExpressionKind.List:
			{
				ExprList exprList = (ExprList)pExpr;
				Expr optionalNextListNode;
				for (;;)
				{
					exprList.OptionalElement = this.Visit(exprList.OptionalElement);
					optionalNextListNode = exprList.OptionalNextListNode;
					if (optionalNextListNode == null)
					{
						break;
					}
					ExprList exprList2;
					if ((exprList2 = optionalNextListNode as ExprList) == null)
					{
						goto Block_3;
					}
					exprList = exprList2;
				}
				return;
				Block_3:
				exprList.OptionalNextListNode = this.Visit(optionalNextListNode);
				return;
			}
			case ExpressionKind.ArrayIndex:
				expr = this.Visit((pExpr as ExprArrayIndex).Array);
				(pExpr as ExprArrayIndex).Array = expr;
				expr = this.Visit((pExpr as ExprArrayIndex).Index);
				(pExpr as ExprArrayIndex).Index = expr;
				return;
			case ExpressionKind.Call:
				expr = this.Visit((pExpr as ExprCall).OptionalArguments);
				(pExpr as ExprCall).OptionalArguments = expr;
				expr = this.Visit((pExpr as ExprCall).MemberGroup);
				(pExpr as ExprCall).MemberGroup = expr as ExprMemberGroup;
				return;
			case ExpressionKind.Field:
				expr = this.Visit((pExpr as ExprField).OptionalObject);
				(pExpr as ExprField).OptionalObject = expr;
				return;
			case ExpressionKind.Constant:
				expr = this.Visit((pExpr as ExprConstant).OptionalConstructorCall);
				(pExpr as ExprConstant).OptionalConstructorCall = expr;
				return;
			case ExpressionKind.Property:
				expr = this.Visit((pExpr as ExprProperty).OptionalArguments);
				(pExpr as ExprProperty).OptionalArguments = expr;
				expr = this.Visit((pExpr as ExprProperty).MemberGroup);
				(pExpr as ExprProperty).MemberGroup = expr as ExprMemberGroup;
				return;
			case ExpressionKind.Multi:
				expr = this.Visit((pExpr as ExprMulti).Left);
				(pExpr as ExprMulti).Left = expr;
				expr = this.Visit((pExpr as ExprMulti).Operator);
				(pExpr as ExprMulti).Operator = expr;
				return;
			case ExpressionKind.Concat:
				expr = this.Visit((pExpr as ExprConcat).FirstArgument);
				(pExpr as ExprConcat).FirstArgument = expr;
				expr = this.Visit((pExpr as ExprConcat).SecondArgument);
				(pExpr as ExprConcat).SecondArgument = expr;
				return;
			case ExpressionKind.ArrayInit:
				expr = this.Visit((pExpr as ExprArrayInit).OptionalArguments);
				(pExpr as ExprArrayInit).OptionalArguments = expr;
				expr = this.Visit((pExpr as ExprArrayInit).OptionalArgumentDimensions);
				(pExpr as ExprArrayInit).OptionalArgumentDimensions = expr;
				return;
			case ExpressionKind.Cast:
				expr = this.Visit((pExpr as ExprCast).Argument);
				(pExpr as ExprCast).Argument = expr;
				expr = this.Visit((pExpr as ExprCast).DestinationType);
				(pExpr as ExprCast).DestinationType = expr as ExprClass;
				return;
			case ExpressionKind.UserDefinedConversion:
				expr = this.Visit((pExpr as ExprUserDefinedConversion).UserDefinedCall);
				(pExpr as ExprUserDefinedConversion).UserDefinedCall = expr;
				return;
			case ExpressionKind.TypeOf:
				expr = this.Visit((pExpr as ExprTypeOf).SourceType);
				(pExpr as ExprTypeOf).SourceType = expr as ExprClass;
				return;
			case ExpressionKind.UserLogicalOp:
				expr = this.Visit((pExpr as ExprUserLogicalOp).TrueFalseCall);
				(pExpr as ExprUserLogicalOp).TrueFalseCall = expr;
				expr = this.Visit((pExpr as ExprUserLogicalOp).OperatorCall);
				(pExpr as ExprUserLogicalOp).OperatorCall = expr as ExprCall;
				expr = this.Visit((pExpr as ExprUserLogicalOp).FirstOperandToExamine);
				(pExpr as ExprUserLogicalOp).FirstOperandToExamine = expr;
				return;
			case ExpressionKind.MemberGroup:
				expr = this.Visit((pExpr as ExprMemberGroup).OptionalObject);
				(pExpr as ExprMemberGroup).OptionalObject = expr;
				return;
			case ExpressionKind.BoundLambda:
				expr = this.Visit((pExpr as ExprBoundLambda).OptionalBody);
				(pExpr as ExprBoundLambda).OptionalBody = expr as ExprBlock;
				return;
			}
			expr = this.Visit((pExpr as ExprBinOp).OptionalLeftChild);
			(pExpr as ExprBinOp).OptionalLeftChild = expr;
			expr = this.Visit((pExpr as ExprBinOp).OptionalRightChild);
			(pExpr as ExprBinOp).OptionalRightChild = expr;
		}

		// Token: 0x060004ED RID: 1261 RVA: 0x0001944E File Offset: 0x0001764E
		protected virtual Expr VisitEXPR(Expr pExpr)
		{
			this.VisitChildren(pExpr);
			return pExpr;
		}

		// Token: 0x060004EE RID: 1262 RVA: 0x00019458 File Offset: 0x00017658
		protected virtual Expr VisitBLOCK(ExprBlock pExpr)
		{
			return this.VisitSTMT(pExpr);
		}

		// Token: 0x060004EF RID: 1263 RVA: 0x00019461 File Offset: 0x00017661
		protected virtual Expr VisitRETURN(ExprReturn pExpr)
		{
			return this.VisitSTMT(pExpr);
		}

		// Token: 0x060004F0 RID: 1264 RVA: 0x0001946A File Offset: 0x0001766A
		protected virtual Expr VisitCLASS(ExprClass pExpr)
		{
			return this.VisitEXPR(pExpr);
		}

		// Token: 0x060004F1 RID: 1265 RVA: 0x00019473 File Offset: 0x00017673
		protected virtual Expr VisitSTMT(ExprStatement pExpr)
		{
			return this.VisitEXPR(pExpr);
		}

		// Token: 0x060004F2 RID: 1266 RVA: 0x0001947C File Offset: 0x0001767C
		protected virtual Expr VisitBINOP(ExprBinOp pExpr)
		{
			return this.VisitEXPR(pExpr);
		}

		// Token: 0x060004F3 RID: 1267 RVA: 0x00019485 File Offset: 0x00017685
		protected virtual Expr VisitLIST(ExprList pExpr)
		{
			return this.VisitEXPR(pExpr);
		}

		// Token: 0x060004F4 RID: 1268 RVA: 0x0001948E File Offset: 0x0001768E
		protected virtual Expr VisitASSIGNMENT(ExprAssignment pExpr)
		{
			return this.VisitEXPR(pExpr);
		}

		// Token: 0x060004F5 RID: 1269 RVA: 0x00019497 File Offset: 0x00017697
		protected virtual Expr VisitARRAYINDEX(ExprArrayIndex pExpr)
		{
			return this.VisitEXPR(pExpr);
		}

		// Token: 0x060004F6 RID: 1270 RVA: 0x000194A0 File Offset: 0x000176A0
		protected virtual Expr VisitUNARYOP(ExprUnaryOp pExpr)
		{
			return this.VisitEXPR(pExpr);
		}

		// Token: 0x060004F7 RID: 1271 RVA: 0x000194A9 File Offset: 0x000176A9
		protected virtual Expr VisitUSERLOGOP(ExprUserLogicalOp pExpr)
		{
			return this.VisitEXPR(pExpr);
		}

		// Token: 0x060004F8 RID: 1272 RVA: 0x000194B2 File Offset: 0x000176B2
		protected virtual Expr VisitTYPEOF(ExprTypeOf pExpr)
		{
			return this.VisitEXPR(pExpr);
		}

		// Token: 0x060004F9 RID: 1273 RVA: 0x000194BB File Offset: 0x000176BB
		protected virtual Expr VisitCAST(ExprCast pExpr)
		{
			return this.VisitEXPR(pExpr);
		}

		// Token: 0x060004FA RID: 1274 RVA: 0x000194C4 File Offset: 0x000176C4
		protected virtual Expr VisitUSERDEFINEDCONVERSION(ExprUserDefinedConversion pExpr)
		{
			return this.VisitEXPR(pExpr);
		}

		// Token: 0x060004FB RID: 1275 RVA: 0x000194CD File Offset: 0x000176CD
		protected virtual Expr VisitZEROINIT(ExprZeroInit pExpr)
		{
			return this.VisitEXPR(pExpr);
		}

		// Token: 0x060004FC RID: 1276 RVA: 0x000194D6 File Offset: 0x000176D6
		protected virtual Expr VisitMEMGRP(ExprMemberGroup pExpr)
		{
			return this.VisitEXPR(pExpr);
		}

		// Token: 0x060004FD RID: 1277 RVA: 0x000194DF File Offset: 0x000176DF
		protected virtual Expr VisitCALL(ExprCall pExpr)
		{
			return this.VisitEXPR(pExpr);
		}

		// Token: 0x060004FE RID: 1278 RVA: 0x000194E8 File Offset: 0x000176E8
		protected virtual Expr VisitPROP(ExprProperty pExpr)
		{
			return this.VisitEXPR(pExpr);
		}

		// Token: 0x060004FF RID: 1279 RVA: 0x000194F1 File Offset: 0x000176F1
		protected virtual Expr VisitFIELD(ExprField pExpr)
		{
			return this.VisitEXPR(pExpr);
		}

		// Token: 0x06000500 RID: 1280 RVA: 0x000194FA File Offset: 0x000176FA
		protected virtual Expr VisitLOCAL(ExprLocal pExpr)
		{
			return this.VisitEXPR(pExpr);
		}

		// Token: 0x06000501 RID: 1281 RVA: 0x00019503 File Offset: 0x00017703
		protected virtual Expr VisitCONSTANT(ExprConstant pExpr)
		{
			return this.VisitEXPR(pExpr);
		}

		// Token: 0x06000502 RID: 1282 RVA: 0x0001950C File Offset: 0x0001770C
		protected virtual Expr VisitMULTIGET(ExprMultiGet pExpr)
		{
			return this.VisitEXPR(pExpr);
		}

		// Token: 0x06000503 RID: 1283 RVA: 0x00019515 File Offset: 0x00017715
		protected virtual Expr VisitMULTI(ExprMulti pExpr)
		{
			return this.VisitEXPR(pExpr);
		}

		// Token: 0x06000504 RID: 1284 RVA: 0x0001951E File Offset: 0x0001771E
		protected virtual Expr VisitWRAP(ExprWrap pExpr)
		{
			return this.VisitEXPR(pExpr);
		}

		// Token: 0x06000505 RID: 1285 RVA: 0x00019527 File Offset: 0x00017727
		protected virtual Expr VisitCONCAT(ExprConcat pExpr)
		{
			return this.VisitEXPR(pExpr);
		}

		// Token: 0x06000506 RID: 1286 RVA: 0x00019530 File Offset: 0x00017730
		protected virtual Expr VisitARRINIT(ExprArrayInit pExpr)
		{
			return this.VisitEXPR(pExpr);
		}

		// Token: 0x06000507 RID: 1287 RVA: 0x00019539 File Offset: 0x00017739
		protected virtual Expr VisitBOUNDLAMBDA(ExprBoundLambda pExpr)
		{
			return this.VisitEXPR(pExpr);
		}

		// Token: 0x06000508 RID: 1288 RVA: 0x00019542 File Offset: 0x00017742
		protected virtual Expr VisitHOISTEDLOCALEXPR(ExprHoistedLocalExpr pExpr)
		{
			return this.VisitEXPR(pExpr);
		}

		// Token: 0x06000509 RID: 1289 RVA: 0x0001954B File Offset: 0x0001774B
		protected virtual Expr VisitFIELDINFO(ExprFieldInfo pExpr)
		{
			return this.VisitEXPR(pExpr);
		}

		// Token: 0x0600050A RID: 1290 RVA: 0x00019554 File Offset: 0x00017754
		protected virtual Expr VisitMETHODINFO(ExprMethodInfo pExpr)
		{
			return this.VisitEXPR(pExpr);
		}

		// Token: 0x0600050B RID: 1291 RVA: 0x0001955D File Offset: 0x0001775D
		protected virtual Expr VisitEQUALS(ExprBinOp pExpr)
		{
			return this.VisitBINOP(pExpr);
		}

		// Token: 0x0600050C RID: 1292 RVA: 0x00019566 File Offset: 0x00017766
		protected virtual Expr VisitCOMPARE(ExprBinOp pExpr)
		{
			return this.VisitBINOP(pExpr);
		}

		// Token: 0x0600050D RID: 1293 RVA: 0x0001956F File Offset: 0x0001776F
		protected virtual Expr VisitEQ(ExprBinOp pExpr)
		{
			return this.VisitBINOP(pExpr);
		}

		// Token: 0x0600050E RID: 1294 RVA: 0x00019578 File Offset: 0x00017778
		protected virtual Expr VisitNE(ExprBinOp pExpr)
		{
			return this.VisitBINOP(pExpr);
		}

		// Token: 0x0600050F RID: 1295 RVA: 0x00019581 File Offset: 0x00017781
		protected virtual Expr VisitLE(ExprBinOp pExpr)
		{
			return this.VisitBINOP(pExpr);
		}

		// Token: 0x06000510 RID: 1296 RVA: 0x0001958A File Offset: 0x0001778A
		protected virtual Expr VisitGE(ExprBinOp pExpr)
		{
			return this.VisitBINOP(pExpr);
		}

		// Token: 0x06000511 RID: 1297 RVA: 0x00019593 File Offset: 0x00017793
		protected virtual Expr VisitADD(ExprBinOp pExpr)
		{
			return this.VisitBINOP(pExpr);
		}

		// Token: 0x06000512 RID: 1298 RVA: 0x0001959C File Offset: 0x0001779C
		protected virtual Expr VisitSUB(ExprBinOp pExpr)
		{
			return this.VisitBINOP(pExpr);
		}

		// Token: 0x06000513 RID: 1299 RVA: 0x000195A5 File Offset: 0x000177A5
		protected virtual Expr VisitDIV(ExprBinOp pExpr)
		{
			return this.VisitBINOP(pExpr);
		}

		// Token: 0x06000514 RID: 1300 RVA: 0x000195AE File Offset: 0x000177AE
		protected virtual Expr VisitBITAND(ExprBinOp pExpr)
		{
			return this.VisitBINOP(pExpr);
		}

		// Token: 0x06000515 RID: 1301 RVA: 0x000195B7 File Offset: 0x000177B7
		protected virtual Expr VisitBITOR(ExprBinOp pExpr)
		{
			return this.VisitBINOP(pExpr);
		}

		// Token: 0x06000516 RID: 1302 RVA: 0x000195C0 File Offset: 0x000177C0
		protected virtual Expr VisitLSHIFT(ExprBinOp pExpr)
		{
			return this.VisitBINOP(pExpr);
		}

		// Token: 0x06000517 RID: 1303 RVA: 0x000195C9 File Offset: 0x000177C9
		protected virtual Expr VisitLOGAND(ExprBinOp pExpr)
		{
			return this.VisitBINOP(pExpr);
		}

		// Token: 0x06000518 RID: 1304 RVA: 0x000195D2 File Offset: 0x000177D2
		protected virtual Expr VisitSEQUENCE(ExprBinOp pExpr)
		{
			return this.VisitBINOP(pExpr);
		}

		// Token: 0x06000519 RID: 1305 RVA: 0x000195DB File Offset: 0x000177DB
		protected virtual Expr VisitSAVE(ExprBinOp pExpr)
		{
			return this.VisitBINOP(pExpr);
		}

		// Token: 0x0600051A RID: 1306 RVA: 0x000195E4 File Offset: 0x000177E4
		protected virtual Expr VisitINDIR(ExprBinOp pExpr)
		{
			return this.VisitBINOP(pExpr);
		}

		// Token: 0x0600051B RID: 1307 RVA: 0x000195ED File Offset: 0x000177ED
		protected virtual Expr VisitSTRINGEQ(ExprBinOp pExpr)
		{
			return this.VisitBINOP(pExpr);
		}

		// Token: 0x0600051C RID: 1308 RVA: 0x000195F6 File Offset: 0x000177F6
		protected virtual Expr VisitDELEGATEEQ(ExprBinOp pExpr)
		{
			return this.VisitBINOP(pExpr);
		}

		// Token: 0x0600051D RID: 1309 RVA: 0x000195FF File Offset: 0x000177FF
		protected virtual Expr VisitDELEGATEADD(ExprBinOp pExpr)
		{
			return this.VisitBINOP(pExpr);
		}

		// Token: 0x0600051E RID: 1310 RVA: 0x00019608 File Offset: 0x00017808
		protected virtual Expr VisitLT(ExprBinOp pExpr)
		{
			return this.VisitBINOP(pExpr);
		}

		// Token: 0x0600051F RID: 1311 RVA: 0x00019611 File Offset: 0x00017811
		protected virtual Expr VisitMUL(ExprBinOp pExpr)
		{
			return this.VisitBINOP(pExpr);
		}

		// Token: 0x06000520 RID: 1312 RVA: 0x0001961A File Offset: 0x0001781A
		protected virtual Expr VisitBITXOR(ExprBinOp pExpr)
		{
			return this.VisitBINOP(pExpr);
		}

		// Token: 0x06000521 RID: 1313 RVA: 0x00019623 File Offset: 0x00017823
		protected virtual Expr VisitRSHIFT(ExprBinOp pExpr)
		{
			return this.VisitBINOP(pExpr);
		}

		// Token: 0x06000522 RID: 1314 RVA: 0x0001962C File Offset: 0x0001782C
		protected virtual Expr VisitLOGOR(ExprBinOp pExpr)
		{
			return this.VisitBINOP(pExpr);
		}

		// Token: 0x06000523 RID: 1315 RVA: 0x00019635 File Offset: 0x00017835
		protected virtual Expr VisitSEQREV(ExprBinOp pExpr)
		{
			return this.VisitBINOP(pExpr);
		}

		// Token: 0x06000524 RID: 1316 RVA: 0x0001963E File Offset: 0x0001783E
		protected virtual Expr VisitSTRINGNE(ExprBinOp pExpr)
		{
			return this.VisitBINOP(pExpr);
		}

		// Token: 0x06000525 RID: 1317 RVA: 0x00019647 File Offset: 0x00017847
		protected virtual Expr VisitDELEGATENE(ExprBinOp pExpr)
		{
			return this.VisitBINOP(pExpr);
		}

		// Token: 0x06000526 RID: 1318 RVA: 0x00019650 File Offset: 0x00017850
		protected virtual Expr VisitGT(ExprBinOp pExpr)
		{
			return this.VisitBINOP(pExpr);
		}

		// Token: 0x06000527 RID: 1319 RVA: 0x00019659 File Offset: 0x00017859
		protected virtual Expr VisitMOD(ExprBinOp pExpr)
		{
			return this.VisitBINOP(pExpr);
		}

		// Token: 0x06000528 RID: 1320 RVA: 0x00019662 File Offset: 0x00017862
		protected virtual Expr VisitSWAP(ExprBinOp pExpr)
		{
			return this.VisitBINOP(pExpr);
		}

		// Token: 0x06000529 RID: 1321 RVA: 0x0001966B File Offset: 0x0001786B
		protected virtual Expr VisitDELEGATESUB(ExprBinOp pExpr)
		{
			return this.VisitBINOP(pExpr);
		}

		// Token: 0x0600052A RID: 1322 RVA: 0x00019674 File Offset: 0x00017874
		protected virtual Expr VisitTRUE(ExprUnaryOp pExpr)
		{
			return this.VisitUNARYOP(pExpr);
		}

		// Token: 0x0600052B RID: 1323 RVA: 0x0001967D File Offset: 0x0001787D
		protected virtual Expr VisitINC(ExprUnaryOp pExpr)
		{
			return this.VisitUNARYOP(pExpr);
		}

		// Token: 0x0600052C RID: 1324 RVA: 0x00019686 File Offset: 0x00017886
		protected virtual Expr VisitLOGNOT(ExprUnaryOp pExpr)
		{
			return this.VisitUNARYOP(pExpr);
		}

		// Token: 0x0600052D RID: 1325 RVA: 0x0001968F File Offset: 0x0001788F
		protected virtual Expr VisitNEG(ExprUnaryOp pExpr)
		{
			return this.VisitUNARYOP(pExpr);
		}

		// Token: 0x0600052E RID: 1326 RVA: 0x00019698 File Offset: 0x00017898
		protected virtual Expr VisitBITNOT(ExprUnaryOp pExpr)
		{
			return this.VisitUNARYOP(pExpr);
		}

		// Token: 0x0600052F RID: 1327 RVA: 0x000196A1 File Offset: 0x000178A1
		protected virtual Expr VisitADDR(ExprUnaryOp pExpr)
		{
			return this.VisitUNARYOP(pExpr);
		}

		// Token: 0x06000530 RID: 1328 RVA: 0x000196AA File Offset: 0x000178AA
		protected virtual Expr VisitDECIMALNEG(ExprUnaryOp pExpr)
		{
			return this.VisitUNARYOP(pExpr);
		}

		// Token: 0x06000531 RID: 1329 RVA: 0x000196B3 File Offset: 0x000178B3
		protected virtual Expr VisitDECIMALDEC(ExprUnaryOp pExpr)
		{
			return this.VisitUNARYOP(pExpr);
		}

		// Token: 0x06000532 RID: 1330 RVA: 0x000196BC File Offset: 0x000178BC
		protected virtual Expr VisitFALSE(ExprUnaryOp pExpr)
		{
			return this.VisitUNARYOP(pExpr);
		}

		// Token: 0x06000533 RID: 1331 RVA: 0x000196C5 File Offset: 0x000178C5
		protected virtual Expr VisitDEC(ExprUnaryOp pExpr)
		{
			return this.VisitUNARYOP(pExpr);
		}

		// Token: 0x06000534 RID: 1332 RVA: 0x000196CE File Offset: 0x000178CE
		protected virtual Expr VisitUPLUS(ExprUnaryOp pExpr)
		{
			return this.VisitUNARYOP(pExpr);
		}

		// Token: 0x06000535 RID: 1333 RVA: 0x000196D7 File Offset: 0x000178D7
		protected virtual Expr VisitDECIMALINC(ExprUnaryOp pExpr)
		{
			return this.VisitUNARYOP(pExpr);
		}
	}
}
