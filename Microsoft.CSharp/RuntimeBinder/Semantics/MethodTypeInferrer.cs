using System;
using System.Collections.Generic;
using Microsoft.CSharp.RuntimeBinder.Syntax;

namespace Microsoft.CSharp.RuntimeBinder.Semantics
{
	// Token: 0x02000052 RID: 82
	internal sealed class MethodTypeInferrer
	{
		// Token: 0x060002C1 RID: 705 RVA: 0x000133A0 File Offset: 0x000115A0
		public static bool Infer(ExpressionBinder binder, SymbolLoader symbolLoader, MethodSymbol pMethod, TypeArray pClassTypeArguments, TypeArray pMethodFormalParameterTypes, ArgInfos pMethodArguments, out TypeArray ppInferredTypeArguments)
		{
			ppInferredTypeArguments = null;
			if (pMethodFormalParameterTypes.Count == 0 || pMethod.InferenceMustFail())
			{
				return false;
			}
			MethodTypeInferrer methodTypeInferrer = new MethodTypeInferrer(binder, symbolLoader, pMethodFormalParameterTypes, pMethodArguments, pMethod.typeVars, pClassTypeArguments);
			bool flag;
			if (pMethodArguments.fHasExprs)
			{
				flag = methodTypeInferrer.InferTypeArgs();
			}
			else
			{
				flag = methodTypeInferrer.InferForMethodGroupConversion();
			}
			ppInferredTypeArguments = methodTypeInferrer.GetResults();
			return flag;
		}

		// Token: 0x060002C2 RID: 706 RVA: 0x000133FC File Offset: 0x000115FC
		private MethodTypeInferrer(ExpressionBinder exprBinder, SymbolLoader symLoader, TypeArray pMethodFormalParameterTypes, ArgInfos pMethodArguments, TypeArray pMethodTypeParameters, TypeArray pClassTypeArguments)
		{
			this._binder = exprBinder;
			this._symbolLoader = symLoader;
			this._pMethodFormalParameterTypes = pMethodFormalParameterTypes;
			this._pMethodArguments = pMethodArguments;
			this._pMethodTypeParameters = pMethodTypeParameters;
			this._pClassTypeArguments = pClassTypeArguments;
			this._pFixedResults = new CType[pMethodTypeParameters.Count];
			this._pLowerBounds = new List<CType>[pMethodTypeParameters.Count];
			this._pUpperBounds = new List<CType>[pMethodTypeParameters.Count];
			this._pExactBounds = new List<CType>[pMethodTypeParameters.Count];
			for (int i = 0; i < pMethodTypeParameters.Count; i++)
			{
				this._pLowerBounds[i] = new List<CType>();
				this._pUpperBounds[i] = new List<CType>();
				this._pExactBounds[i] = new List<CType>();
			}
			this._ppDependencies = null;
		}

		// Token: 0x060002C3 RID: 707 RVA: 0x000134C4 File Offset: 0x000116C4
		private TypeArray GetResults()
		{
			for (int i = 0; i < this._pMethodTypeParameters.Count; i++)
			{
				ErrorType errorType;
				if (this._pFixedResults[i] == null || ((errorType = this._pFixedResults[i] as ErrorType) != null && errorType.nameText == null))
				{
					this._pFixedResults[i] = this.GetTypeManager().GetErrorType(((TypeParameterType)this._pMethodTypeParameters[i]).GetName(), BSYMMGR.EmptyTypeArray());
				}
			}
			return this.GetGlobalSymbols().AllocParams(this._pMethodTypeParameters.Count, this._pFixedResults);
		}

		// Token: 0x060002C4 RID: 708 RVA: 0x00013554 File Offset: 0x00011754
		private bool IsUnfixed(int iParam)
		{
			return this._pFixedResults[iParam] == null;
		}

		// Token: 0x060002C5 RID: 709 RVA: 0x00013564 File Offset: 0x00011764
		private bool IsUnfixed(TypeParameterType pParam)
		{
			int indexInTotalParameters = pParam.GetIndexInTotalParameters();
			return this.IsUnfixed(indexInTotalParameters);
		}

		// Token: 0x060002C6 RID: 710 RVA: 0x00013580 File Offset: 0x00011780
		private bool AllFixed()
		{
			for (int i = 0; i < this._pMethodTypeParameters.Count; i++)
			{
				if (this.IsUnfixed(i))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x060002C7 RID: 711 RVA: 0x000135B0 File Offset: 0x000117B0
		private void AddLowerBound(TypeParameterType pParam, CType pBound)
		{
			int indexInTotalParameters = pParam.GetIndexInTotalParameters();
			if (!this._pLowerBounds[indexInTotalParameters].Contains(pBound))
			{
				this._pLowerBounds[indexInTotalParameters].Add(pBound);
			}
		}

		// Token: 0x060002C8 RID: 712 RVA: 0x000135E4 File Offset: 0x000117E4
		private void AddUpperBound(TypeParameterType pParam, CType pBound)
		{
			int indexInTotalParameters = pParam.GetIndexInTotalParameters();
			if (!this._pUpperBounds[indexInTotalParameters].Contains(pBound))
			{
				this._pUpperBounds[indexInTotalParameters].Add(pBound);
			}
		}

		// Token: 0x060002C9 RID: 713 RVA: 0x00013618 File Offset: 0x00011818
		private void AddExactBound(TypeParameterType pParam, CType pBound)
		{
			int indexInTotalParameters = pParam.GetIndexInTotalParameters();
			if (!this._pExactBounds[indexInTotalParameters].Contains(pBound))
			{
				this._pExactBounds[indexInTotalParameters].Add(pBound);
			}
		}

		// Token: 0x060002CA RID: 714 RVA: 0x0001364A File Offset: 0x0001184A
		private bool HasBound(int iParam)
		{
			return !this._pLowerBounds[iParam].IsEmpty<CType>() || !this._pExactBounds[iParam].IsEmpty<CType>() || !this._pUpperBounds[iParam].IsEmpty<CType>();
		}

		// Token: 0x060002CB RID: 715 RVA: 0x0001367C File Offset: 0x0001187C
		private TypeArray GetFixedDelegateParameters(AggregateType pDelegateType)
		{
			CType[] array = new CType[this._pMethodTypeParameters.Count];
			for (int i = 0; i < this._pMethodTypeParameters.Count; i++)
			{
				TypeParameterType typeParameterType = (TypeParameterType)this._pMethodTypeParameters[i];
				array[i] = (this.IsUnfixed(i) ? typeParameterType : this._pFixedResults[i]);
			}
			SubstContext substContext = new SubstContext(this._pClassTypeArguments.Items, this._pClassTypeArguments.Count, array, this._pMethodTypeParameters.Count);
			return (this.GetTypeManager().SubstType(pDelegateType, substContext) as AggregateType).GetDelegateParameters(this.GetSymbolLoader());
		}

		// Token: 0x060002CC RID: 716 RVA: 0x0001371E File Offset: 0x0001191E
		private bool InferTypeArgs()
		{
			this.InferTypeArgsFirstPhase();
			return this.InferTypeArgsSecondPhase();
		}

		// Token: 0x060002CD RID: 717 RVA: 0x0001372C File Offset: 0x0001192C
		private static bool IsReallyAType(CType pType)
		{
			return !(pType is NullType) && !(pType is VoidType) && !(pType is MethodGroupType);
		}

		// Token: 0x060002CE RID: 718 RVA: 0x0001374C File Offset: 0x0001194C
		private void InferTypeArgsFirstPhase()
		{
			for (int i = 0; i < this._pMethodArguments.carg; i++)
			{
				Expr expr = this._pMethodArguments.prgexpr[i];
				if (!expr.IsOptionalArgument)
				{
					CType ctype = this._pMethodFormalParameterTypes[i];
					CType ctype2 = expr.RuntimeObjectActualType ?? this._pMethodArguments.types[i];
					bool flag = false;
					ParameterModifierType parameterModifierType;
					if ((parameterModifierType = ctype as ParameterModifierType) != null)
					{
						ctype = parameterModifierType.GetParameterType();
						flag = true;
					}
					ParameterModifierType parameterModifierType2;
					if ((parameterModifierType2 = ctype2 as ParameterModifierType) != null)
					{
						ctype2 = parameterModifierType2.GetParameterType();
					}
					if (MethodTypeInferrer.IsReallyAType(ctype2))
					{
						if (flag)
						{
							this.ExactInference(ctype2, ctype);
						}
						else
						{
							this.LowerBoundInference(ctype2, ctype);
						}
					}
				}
			}
		}

		// Token: 0x060002CF RID: 719 RVA: 0x00013804 File Offset: 0x00011A04
		private bool InferTypeArgsSecondPhase()
		{
			this.InitializeDependencies();
			for (;;)
			{
				MethodTypeInferrer.NewInferenceResult newInferenceResult = this.DoSecondPhase();
				if (newInferenceResult == MethodTypeInferrer.NewInferenceResult.InferenceFailed)
				{
					break;
				}
				if (newInferenceResult == MethodTypeInferrer.NewInferenceResult.Success)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060002D0 RID: 720 RVA: 0x00013828 File Offset: 0x00011A28
		private MethodTypeInferrer.NewInferenceResult DoSecondPhase()
		{
			if (this.AllFixed())
			{
				return MethodTypeInferrer.NewInferenceResult.Success;
			}
			this.MakeOutputTypeInferences();
			MethodTypeInferrer.NewInferenceResult newInferenceResult = this.FixNondependentParameters();
			if (newInferenceResult != MethodTypeInferrer.NewInferenceResult.NoProgress)
			{
				return newInferenceResult;
			}
			newInferenceResult = this.FixDependentParameters();
			if (newInferenceResult != MethodTypeInferrer.NewInferenceResult.NoProgress)
			{
				return newInferenceResult;
			}
			return MethodTypeInferrer.NewInferenceResult.InferenceFailed;
		}

		// Token: 0x060002D1 RID: 721 RVA: 0x00013860 File Offset: 0x00011A60
		private void MakeOutputTypeInferences()
		{
			for (int i = 0; i < this._pMethodArguments.carg; i++)
			{
				CType ctype = this._pMethodFormalParameterTypes[i];
				ParameterModifierType parameterModifierType;
				if ((parameterModifierType = ctype as ParameterModifierType) != null)
				{
					ctype = parameterModifierType.GetParameterType();
				}
				Expr expr = this._pMethodArguments.prgexpr[i];
				if (this.HasUnfixedParamInOutputType(expr, ctype) && !this.HasUnfixedParamInInputType(expr, ctype))
				{
					CType ctype2 = this._pMethodArguments.types[i];
					ParameterModifierType parameterModifierType2;
					if ((parameterModifierType2 = ctype2 as ParameterModifierType) != null)
					{
						ctype2 = parameterModifierType2.GetParameterType();
					}
					this.OutputTypeInference(expr, ctype2, ctype);
				}
			}
		}

		// Token: 0x060002D2 RID: 722 RVA: 0x000138FC File Offset: 0x00011AFC
		private MethodTypeInferrer.NewInferenceResult FixNondependentParameters()
		{
			bool[] array = new bool[this._pMethodTypeParameters.Count];
			MethodTypeInferrer.NewInferenceResult newInferenceResult = MethodTypeInferrer.NewInferenceResult.NoProgress;
			for (int i = 0; i < this._pMethodTypeParameters.Count; i++)
			{
				if (this.IsUnfixed(i) && this.HasBound(i) && !this.DependsOnAny(i))
				{
					array[i] = true;
					newInferenceResult = MethodTypeInferrer.NewInferenceResult.MadeProgress;
				}
			}
			for (int i = 0; i < this._pMethodTypeParameters.Count; i++)
			{
				if (array[i] && !this.Fix(i))
				{
					newInferenceResult = MethodTypeInferrer.NewInferenceResult.InferenceFailed;
				}
			}
			return newInferenceResult;
		}

		// Token: 0x060002D3 RID: 723 RVA: 0x0001397C File Offset: 0x00011B7C
		private MethodTypeInferrer.NewInferenceResult FixDependentParameters()
		{
			bool[] array = new bool[this._pMethodTypeParameters.Count];
			MethodTypeInferrer.NewInferenceResult newInferenceResult = MethodTypeInferrer.NewInferenceResult.NoProgress;
			for (int i = 0; i < this._pMethodTypeParameters.Count; i++)
			{
				if (this.IsUnfixed(i) && this.HasBound(i) && this.AnyDependsOn(i))
				{
					array[i] = true;
					newInferenceResult = MethodTypeInferrer.NewInferenceResult.MadeProgress;
				}
			}
			for (int i = 0; i < this._pMethodTypeParameters.Count; i++)
			{
				if (array[i] && !this.Fix(i))
				{
					newInferenceResult = MethodTypeInferrer.NewInferenceResult.InferenceFailed;
				}
			}
			return newInferenceResult;
		}

		// Token: 0x060002D4 RID: 724 RVA: 0x000139FC File Offset: 0x00011BFC
		private bool DoesInputTypeContain(Expr pSource, CType pDest, TypeParameterType pParam)
		{
			pDest = pDest.GetDelegateTypeOfPossibleExpression();
			if (pDest.isDelegateType())
			{
				ExpressionKind kind = pSource.Kind;
				if (kind - ExpressionKind.MemberGroup <= 1)
				{
					TypeArray delegateParameters = (pDest as AggregateType).GetDelegateParameters(this.GetSymbolLoader());
					if (delegateParameters != null)
					{
						return TypeManager.ParametersContainTyVar(delegateParameters, pParam);
					}
				}
			}
			return false;
		}

		// Token: 0x060002D5 RID: 725 RVA: 0x00013A48 File Offset: 0x00011C48
		private bool HasUnfixedParamInInputType(Expr pSource, CType pDest)
		{
			for (int i = 0; i < this._pMethodTypeParameters.Count; i++)
			{
				if (this.IsUnfixed(i) && this.DoesInputTypeContain(pSource, pDest, this._pMethodTypeParameters[i] as TypeParameterType))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060002D6 RID: 726 RVA: 0x00013A94 File Offset: 0x00011C94
		private bool DoesOutputTypeContain(Expr pSource, CType pDest, TypeParameterType pParam)
		{
			pDest = pDest.GetDelegateTypeOfPossibleExpression();
			if (pDest.isDelegateType())
			{
				ExpressionKind kind = pSource.Kind;
				if (kind - ExpressionKind.MemberGroup <= 1)
				{
					CType delegateReturnType = ((AggregateType)pDest).GetDelegateReturnType(this.GetSymbolLoader());
					if (delegateReturnType != null)
					{
						return TypeManager.TypeContainsType(delegateReturnType, pParam);
					}
				}
			}
			return false;
		}

		// Token: 0x060002D7 RID: 727 RVA: 0x00013AE0 File Offset: 0x00011CE0
		private bool HasUnfixedParamInOutputType(Expr pSource, CType pDest)
		{
			for (int i = 0; i < this._pMethodTypeParameters.Count; i++)
			{
				if (this.IsUnfixed(i) && this.DoesOutputTypeContain(pSource, pDest, this._pMethodTypeParameters[i] as TypeParameterType))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060002D8 RID: 728 RVA: 0x00013B2C File Offset: 0x00011D2C
		private bool DependsDirectlyOn(int iParam, int jParam)
		{
			for (int i = 0; i < this._pMethodArguments.carg; i++)
			{
				CType ctype = this._pMethodFormalParameterTypes[i];
				ParameterModifierType parameterModifierType;
				if ((parameterModifierType = ctype as ParameterModifierType) != null)
				{
					ctype = parameterModifierType.GetParameterType();
				}
				Expr expr = this._pMethodArguments.prgexpr[i];
				if (this.DoesInputTypeContain(expr, ctype, this._pMethodTypeParameters[jParam] as TypeParameterType) && this.DoesOutputTypeContain(expr, ctype, this._pMethodTypeParameters[iParam] as TypeParameterType))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060002D9 RID: 729 RVA: 0x00013BB8 File Offset: 0x00011DB8
		private void InitializeDependencies()
		{
			this._ppDependencies = new MethodTypeInferrer.Dependency[this._pMethodTypeParameters.Count][];
			for (int i = 0; i < this._pMethodTypeParameters.Count; i++)
			{
				this._ppDependencies[i] = new MethodTypeInferrer.Dependency[this._pMethodTypeParameters.Count];
				for (int j = 0; j < this._pMethodTypeParameters.Count; j++)
				{
					if (this.DependsDirectlyOn(i, j))
					{
						this._ppDependencies[i][j] = MethodTypeInferrer.Dependency.Direct;
					}
				}
			}
			this.DeduceAllDependencies();
		}

		// Token: 0x060002DA RID: 730 RVA: 0x00013C3B File Offset: 0x00011E3B
		private bool DependsOn(int iParam, int jParam)
		{
			if (this._dependenciesDirty)
			{
				this.SetIndirectsToUnknown();
				this.DeduceAllDependencies();
			}
			return (this._ppDependencies[iParam][jParam] & MethodTypeInferrer.Dependency.DependsMask) > MethodTypeInferrer.Dependency.Unknown;
		}

		// Token: 0x060002DB RID: 731 RVA: 0x00013C64 File Offset: 0x00011E64
		private bool DependsTransitivelyOn(int iParam, int jParam)
		{
			for (int i = 0; i < this._pMethodTypeParameters.Count; i++)
			{
				if ((this._ppDependencies[iParam][i] & MethodTypeInferrer.Dependency.DependsMask) != MethodTypeInferrer.Dependency.Unknown && (this._ppDependencies[i][jParam] & MethodTypeInferrer.Dependency.DependsMask) != MethodTypeInferrer.Dependency.Unknown)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060002DC RID: 732 RVA: 0x00013CA8 File Offset: 0x00011EA8
		private void DeduceAllDependencies()
		{
			while (this.DeduceDependencies())
			{
			}
			this.SetUnknownsToNotDependent();
			this._dependenciesDirty = false;
		}

		// Token: 0x060002DD RID: 733 RVA: 0x00013CCC File Offset: 0x00011ECC
		private bool DeduceDependencies()
		{
			bool flag = false;
			for (int i = 0; i < this._pMethodTypeParameters.Count; i++)
			{
				for (int j = 0; j < this._pMethodTypeParameters.Count; j++)
				{
					if (this._ppDependencies[i][j] == MethodTypeInferrer.Dependency.Unknown && this.DependsTransitivelyOn(i, j))
					{
						this._ppDependencies[i][j] = MethodTypeInferrer.Dependency.Indirect;
						flag = true;
					}
				}
			}
			return flag;
		}

		// Token: 0x060002DE RID: 734 RVA: 0x00013D2C File Offset: 0x00011F2C
		private void SetUnknownsToNotDependent()
		{
			for (int i = 0; i < this._pMethodTypeParameters.Count; i++)
			{
				for (int j = 0; j < this._pMethodTypeParameters.Count; j++)
				{
					if (this._ppDependencies[i][j] == MethodTypeInferrer.Dependency.Unknown)
					{
						this._ppDependencies[i][j] = MethodTypeInferrer.Dependency.NotDependent;
					}
				}
			}
		}

		// Token: 0x060002DF RID: 735 RVA: 0x00013D7C File Offset: 0x00011F7C
		private void SetIndirectsToUnknown()
		{
			for (int i = 0; i < this._pMethodTypeParameters.Count; i++)
			{
				for (int j = 0; j < this._pMethodTypeParameters.Count; j++)
				{
					if (this._ppDependencies[i][j] == MethodTypeInferrer.Dependency.Indirect)
					{
						this._ppDependencies[i][j] = MethodTypeInferrer.Dependency.Unknown;
					}
				}
			}
		}

		// Token: 0x060002E0 RID: 736 RVA: 0x00013DD0 File Offset: 0x00011FD0
		private void UpdateDependenciesAfterFix(int iParam)
		{
			if (this._ppDependencies == null)
			{
				return;
			}
			for (int i = 0; i < this._pMethodTypeParameters.Count; i++)
			{
				this._ppDependencies[iParam][i] = MethodTypeInferrer.Dependency.NotDependent;
				this._ppDependencies[i][iParam] = MethodTypeInferrer.Dependency.NotDependent;
			}
			this._dependenciesDirty = true;
		}

		// Token: 0x060002E1 RID: 737 RVA: 0x00013E1C File Offset: 0x0001201C
		private bool DependsOnAny(int iParam)
		{
			for (int i = 0; i < this._pMethodTypeParameters.Count; i++)
			{
				if (this.DependsOn(iParam, i))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060002E2 RID: 738 RVA: 0x00013E4C File Offset: 0x0001204C
		private bool AnyDependsOn(int iParam)
		{
			for (int i = 0; i < this._pMethodTypeParameters.Count; i++)
			{
				if (this.DependsOn(i, iParam))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060002E3 RID: 739 RVA: 0x00013E7C File Offset: 0x0001207C
		private void OutputTypeInference(Expr pExpr, CType pSource, CType pDest)
		{
			if (this.MethodGroupReturnTypeInference(pExpr, pDest))
			{
				return;
			}
			if (MethodTypeInferrer.IsReallyAType(pSource))
			{
				this.LowerBoundInference(pSource, pDest);
			}
		}

		// Token: 0x060002E4 RID: 740 RVA: 0x00013E9C File Offset: 0x0001209C
		private bool MethodGroupReturnTypeInference(Expr pSource, CType pType)
		{
			ExprMemberGroup exprMemberGroup;
			if ((exprMemberGroup = pSource as ExprMemberGroup) == null)
			{
				return false;
			}
			pType = pType.GetDelegateTypeOfPossibleExpression();
			if (!pType.isDelegateType())
			{
				return false;
			}
			AggregateType aggregateType = pType as AggregateType;
			CType delegateReturnType = aggregateType.GetDelegateReturnType(this.GetSymbolLoader());
			if (delegateReturnType == null)
			{
				return false;
			}
			if (delegateReturnType is VoidType)
			{
				return false;
			}
			TypeArray fixedDelegateParameters = this.GetFixedDelegateParameters(aggregateType);
			if (fixedDelegateParameters == null)
			{
				return false;
			}
			ArgInfos argInfos = new ArgInfos
			{
				carg = fixedDelegateParameters.Count,
				types = fixedDelegateParameters,
				fHasExprs = false,
				prgexpr = null
			};
			ExpressionBinder.GroupToArgsBinder groupToArgsBinder = new ExpressionBinder.GroupToArgsBinder(this._binder, (BindingFlag)0, exprMemberGroup, argInfos, null, false, aggregateType);
			if (!groupToArgsBinder.Bind(false))
			{
				return false;
			}
			MethPropWithInst bestResult = groupToArgsBinder.GetResultsOfBind().GetBestResult();
			CType ctype = this.GetTypeManager().SubstType(bestResult.Meth().RetType, bestResult.GetType(), bestResult.TypeArgs);
			if (ctype is VoidType)
			{
				return false;
			}
			this.LowerBoundInference(ctype, delegateReturnType);
			return true;
		}

		// Token: 0x060002E5 RID: 741 RVA: 0x00013F89 File Offset: 0x00012189
		private void ExactInference(CType pSource, CType pDest)
		{
			if (this.ExactTypeParameterInference(pSource, pDest))
			{
				return;
			}
			if (this.ExactArrayInference(pSource, pDest))
			{
				return;
			}
			if (this.ExactNullableInference(pSource, pDest))
			{
				return;
			}
			this.ExactConstructedInference(pSource, pDest);
		}

		// Token: 0x060002E6 RID: 742 RVA: 0x00013FB8 File Offset: 0x000121B8
		private bool ExactTypeParameterInference(CType pSource, CType pDest)
		{
			TypeParameterType typeParameterType;
			if ((typeParameterType = pDest as TypeParameterType) != null && typeParameterType.IsMethodTypeParameter() && this.IsUnfixed(typeParameterType))
			{
				this.AddExactBound(typeParameterType, pSource);
				return true;
			}
			return false;
		}

		// Token: 0x060002E7 RID: 743 RVA: 0x00013FEC File Offset: 0x000121EC
		private bool ExactArrayInference(CType pSource, CType pDest)
		{
			ArrayType arrayType;
			ArrayType arrayType2;
			if ((arrayType = pSource as ArrayType) == null || (arrayType2 = pDest as ArrayType) == null)
			{
				return false;
			}
			if (arrayType.rank != arrayType2.rank || arrayType.IsSZArray != arrayType2.IsSZArray)
			{
				return false;
			}
			this.ExactInference(arrayType.GetElementType(), arrayType2.GetElementType());
			return true;
		}

		// Token: 0x060002E8 RID: 744 RVA: 0x00014040 File Offset: 0x00012240
		private bool ExactNullableInference(CType pSource, CType pDest)
		{
			NullableType nullableType;
			NullableType nullableType2;
			if ((nullableType = pSource as NullableType) == null || (nullableType2 = pDest as NullableType) == null)
			{
				return false;
			}
			this.ExactInference(nullableType.GetUnderlyingType(), nullableType2.GetUnderlyingType());
			return true;
		}

		// Token: 0x060002E9 RID: 745 RVA: 0x00014078 File Offset: 0x00012278
		private bool ExactConstructedInference(CType pSource, CType pDest)
		{
			AggregateType aggregateType;
			AggregateType aggregateType2;
			if ((aggregateType = pSource as AggregateType) == null || (aggregateType2 = pDest as AggregateType) == null || aggregateType.GetOwningAggregate() != aggregateType2.GetOwningAggregate())
			{
				return false;
			}
			this.ExactTypeArgumentInference(aggregateType, aggregateType2);
			return true;
		}

		// Token: 0x060002EA RID: 746 RVA: 0x000140B4 File Offset: 0x000122B4
		private void ExactTypeArgumentInference(AggregateType pSource, AggregateType pDest)
		{
			TypeArray typeArgsAll = pSource.GetTypeArgsAll();
			TypeArray typeArgsAll2 = pDest.GetTypeArgsAll();
			for (int i = 0; i < typeArgsAll.Count; i++)
			{
				this.ExactInference(typeArgsAll[i], typeArgsAll2[i]);
			}
		}

		// Token: 0x060002EB RID: 747 RVA: 0x000140F4 File Offset: 0x000122F4
		private void LowerBoundInference(CType pSource, CType pDest)
		{
			if (this.LowerBoundTypeParameterInference(pSource, pDest))
			{
				return;
			}
			if (this.LowerBoundArrayInference(pSource, pDest))
			{
				return;
			}
			if (this.ExactNullableInference(pSource, pDest))
			{
				return;
			}
			this.LowerBoundConstructedInference(pSource, pDest);
		}

		// Token: 0x060002EC RID: 748 RVA: 0x00014120 File Offset: 0x00012320
		private bool LowerBoundTypeParameterInference(CType pSource, CType pDest)
		{
			TypeParameterType typeParameterType;
			if ((typeParameterType = pDest as TypeParameterType) != null && typeParameterType.IsMethodTypeParameter() && this.IsUnfixed(typeParameterType))
			{
				this.AddLowerBound(typeParameterType, pSource);
				return true;
			}
			return false;
		}

		// Token: 0x060002ED RID: 749 RVA: 0x00014154 File Offset: 0x00012354
		private bool LowerBoundArrayInference(CType pSource, CType pDest)
		{
			ArrayType arrayType;
			if ((arrayType = pSource as ArrayType) == null)
			{
				return false;
			}
			CType elementType = arrayType.GetElementType();
			ArrayType arrayType2;
			CType ctype;
			if ((arrayType2 = pDest as ArrayType) != null)
			{
				if (arrayType2.rank != arrayType.rank || arrayType2.IsSZArray != arrayType.IsSZArray)
				{
					return false;
				}
				ctype = arrayType2.GetElementType();
			}
			else
			{
				if (!pDest.isPredefType(PredefinedType.PT_G_IENUMERABLE) && !pDest.isPredefType(PredefinedType.PT_G_ICOLLECTION) && !pDest.isPredefType(PredefinedType.PT_G_ILIST) && !pDest.isPredefType(PredefinedType.PT_G_IREADONLYCOLLECTION) && !pDest.isPredefType(PredefinedType.PT_G_IREADONLYLIST))
				{
					return false;
				}
				if (!arrayType.IsSZArray)
				{
					return false;
				}
				ctype = ((AggregateType)pDest).GetTypeArgsThis()[0];
			}
			if (elementType.IsRefType())
			{
				this.LowerBoundInference(elementType, ctype);
			}
			else
			{
				this.ExactInference(elementType, ctype);
			}
			return true;
		}

		// Token: 0x060002EE RID: 750 RVA: 0x00014214 File Offset: 0x00012414
		private bool LowerBoundConstructedInference(CType pSource, CType pDest)
		{
			AggregateType aggregateType;
			if ((aggregateType = pDest as AggregateType) == null)
			{
				return false;
			}
			if (aggregateType.GetTypeArgsAll().Count == 0)
			{
				return false;
			}
			AggregateType aggregateType2;
			if ((aggregateType2 = pSource as AggregateType) != null && aggregateType2.GetOwningAggregate() == aggregateType.GetOwningAggregate())
			{
				if (aggregateType2.isInterfaceType() || aggregateType2.isDelegateType())
				{
					this.LowerBoundTypeArgumentInference(aggregateType2, aggregateType);
				}
				else
				{
					this.ExactTypeArgumentInference(aggregateType2, aggregateType);
				}
				return true;
			}
			return this.LowerBoundClassInference(pSource, aggregateType) || this.LowerBoundInterfaceInference(pSource, aggregateType);
		}

		// Token: 0x060002EF RID: 751 RVA: 0x00014294 File Offset: 0x00012494
		private bool LowerBoundClassInference(CType pSource, AggregateType pDest)
		{
			if (!pDest.isClassType())
			{
				return false;
			}
			AggregateType aggregateType = null;
			if (pSource.isClassType())
			{
				aggregateType = (pSource as AggregateType).GetBaseClass();
			}
			while (aggregateType != null)
			{
				if (aggregateType.GetOwningAggregate() == pDest.GetOwningAggregate())
				{
					this.ExactTypeArgumentInference(aggregateType, pDest);
					return true;
				}
				aggregateType = aggregateType.GetBaseClass();
			}
			return false;
		}

		// Token: 0x060002F0 RID: 752 RVA: 0x000142E8 File Offset: 0x000124E8
		private bool LowerBoundInterfaceInference(CType pSource, AggregateType pDest)
		{
			if (!pDest.isInterfaceType())
			{
				return false;
			}
			if (!pSource.isStructType() && !pSource.isClassType() && !pSource.isInterfaceType() && !(pSource is TypeParameterType))
			{
				return false;
			}
			IEnumerable<AggregateType> enumerable = pSource.AllPossibleInterfaces();
			AggregateType aggregateType = null;
			foreach (AggregateType aggregateType2 in enumerable)
			{
				if (aggregateType2.GetOwningAggregate() == pDest.GetOwningAggregate())
				{
					if (aggregateType == null)
					{
						aggregateType = aggregateType2;
					}
					else if (aggregateType != aggregateType2)
					{
						return false;
					}
				}
			}
			if (aggregateType == null)
			{
				return false;
			}
			this.LowerBoundTypeArgumentInference(aggregateType, pDest);
			return true;
		}

		// Token: 0x060002F1 RID: 753 RVA: 0x0001438C File Offset: 0x0001258C
		private void LowerBoundTypeArgumentInference(AggregateType pSource, AggregateType pDest)
		{
			TypeArray typeVarsAll = pSource.GetOwningAggregate().GetTypeVarsAll();
			TypeArray typeArgsAll = pSource.GetTypeArgsAll();
			TypeArray typeArgsAll2 = pDest.GetTypeArgsAll();
			for (int i = 0; i < typeArgsAll.Count; i++)
			{
				TypeParameterType typeParameterType = (TypeParameterType)typeVarsAll[i];
				CType ctype = typeArgsAll[i];
				CType ctype2 = typeArgsAll2[i];
				if (ctype.IsRefType() && typeParameterType.Covariant)
				{
					this.LowerBoundInference(ctype, ctype2);
				}
				else if (ctype.IsRefType() && typeParameterType.Contravariant)
				{
					this.UpperBoundInference(typeArgsAll[i], typeArgsAll2[i]);
				}
				else
				{
					this.ExactInference(typeArgsAll[i], typeArgsAll2[i]);
				}
			}
		}

		// Token: 0x060002F2 RID: 754 RVA: 0x00014441 File Offset: 0x00012641
		private void UpperBoundInference(CType pSource, CType pDest)
		{
			if (this.UpperBoundTypeParameterInference(pSource, pDest))
			{
				return;
			}
			if (this.UpperBoundArrayInference(pSource, pDest))
			{
				return;
			}
			if (this.ExactNullableInference(pSource, pDest))
			{
				return;
			}
			this.UpperBoundConstructedInference(pSource, pDest);
		}

		// Token: 0x060002F3 RID: 755 RVA: 0x00014470 File Offset: 0x00012670
		private bool UpperBoundTypeParameterInference(CType pSource, CType pDest)
		{
			TypeParameterType typeParameterType;
			if ((typeParameterType = pDest as TypeParameterType) != null && typeParameterType.IsMethodTypeParameter() && this.IsUnfixed(typeParameterType))
			{
				this.AddUpperBound(typeParameterType, pSource);
				return true;
			}
			return false;
		}

		// Token: 0x060002F4 RID: 756 RVA: 0x000144A4 File Offset: 0x000126A4
		private bool UpperBoundArrayInference(CType pSource, CType pDest)
		{
			ArrayType arrayType;
			if ((arrayType = pDest as ArrayType) == null)
			{
				return false;
			}
			CType elementType = arrayType.GetElementType();
			ArrayType arrayType2;
			CType ctype;
			if ((arrayType2 = pSource as ArrayType) != null)
			{
				if (arrayType.rank != arrayType2.rank || arrayType.IsSZArray != arrayType2.IsSZArray)
				{
					return false;
				}
				ctype = arrayType2.GetElementType();
			}
			else
			{
				if (!pSource.isPredefType(PredefinedType.PT_G_IENUMERABLE) && !pSource.isPredefType(PredefinedType.PT_G_ICOLLECTION) && !pSource.isPredefType(PredefinedType.PT_G_ILIST) && !pSource.isPredefType(PredefinedType.PT_G_IREADONLYLIST) && !pSource.isPredefType(PredefinedType.PT_G_IREADONLYCOLLECTION))
				{
					return false;
				}
				if (!arrayType.IsSZArray)
				{
					return false;
				}
				ctype = ((AggregateType)pSource).GetTypeArgsThis()[0];
			}
			if (ctype.IsRefType())
			{
				this.UpperBoundInference(ctype, elementType);
			}
			else
			{
				this.ExactInference(ctype, elementType);
			}
			return true;
		}

		// Token: 0x060002F5 RID: 757 RVA: 0x00014564 File Offset: 0x00012764
		private bool UpperBoundConstructedInference(CType pSource, CType pDest)
		{
			AggregateType aggregateType;
			if ((aggregateType = pSource as AggregateType) == null)
			{
				return false;
			}
			if (aggregateType.GetTypeArgsAll().Count == 0)
			{
				return false;
			}
			AggregateType aggregateType2;
			if ((aggregateType2 = pDest as AggregateType) != null && aggregateType.GetOwningAggregate() == aggregateType2.GetOwningAggregate())
			{
				if (aggregateType2.isInterfaceType() || aggregateType2.isDelegateType())
				{
					this.UpperBoundTypeArgumentInference(aggregateType, aggregateType2);
				}
				else
				{
					this.ExactTypeArgumentInference(aggregateType, aggregateType2);
				}
				return true;
			}
			return this.UpperBoundClassInference(aggregateType, pDest) || this.UpperBoundInterfaceInference(aggregateType, pDest);
		}

		// Token: 0x060002F6 RID: 758 RVA: 0x000145E4 File Offset: 0x000127E4
		private bool UpperBoundClassInference(AggregateType pSource, CType pDest)
		{
			if (!pSource.isClassType() || !pDest.isClassType())
			{
				return false;
			}
			for (AggregateType aggregateType = ((AggregateType)pDest).GetBaseClass(); aggregateType != null; aggregateType = aggregateType.GetBaseClass())
			{
				if (aggregateType.GetOwningAggregate() == pSource.GetOwningAggregate())
				{
					this.ExactTypeArgumentInference(pSource, aggregateType);
					return true;
				}
			}
			return false;
		}

		// Token: 0x060002F7 RID: 759 RVA: 0x00014634 File Offset: 0x00012834
		private bool UpperBoundInterfaceInference(AggregateType pSource, CType pDest)
		{
			if (!pSource.isInterfaceType())
			{
				return false;
			}
			if (!pDest.isStructType() && !pDest.isClassType() && !pDest.isInterfaceType())
			{
				return false;
			}
			IEnumerable<AggregateType> enumerable = pDest.AllPossibleInterfaces();
			AggregateType aggregateType = null;
			foreach (AggregateType aggregateType2 in enumerable)
			{
				if (aggregateType2.GetOwningAggregate() == pSource.GetOwningAggregate())
				{
					if (aggregateType == null)
					{
						aggregateType = aggregateType2;
					}
					else if (aggregateType != aggregateType2)
					{
						return false;
					}
				}
			}
			if (aggregateType == null)
			{
				return false;
			}
			this.UpperBoundTypeArgumentInference(aggregateType, pDest as AggregateType);
			return true;
		}

		// Token: 0x060002F8 RID: 760 RVA: 0x000146D4 File Offset: 0x000128D4
		private void UpperBoundTypeArgumentInference(AggregateType pSource, AggregateType pDest)
		{
			TypeArray typeVarsAll = pSource.GetOwningAggregate().GetTypeVarsAll();
			TypeArray typeArgsAll = pSource.GetTypeArgsAll();
			TypeArray typeArgsAll2 = pDest.GetTypeArgsAll();
			for (int i = 0; i < typeArgsAll.Count; i++)
			{
				TypeParameterType typeParameterType = (TypeParameterType)typeVarsAll[i];
				CType ctype = typeArgsAll[i];
				CType ctype2 = typeArgsAll2[i];
				if (ctype.IsRefType() && typeParameterType.Covariant)
				{
					this.UpperBoundInference(ctype, ctype2);
				}
				else if (ctype.IsRefType() && typeParameterType.Contravariant)
				{
					this.LowerBoundInference(typeArgsAll[i], typeArgsAll2[i]);
				}
				else
				{
					this.ExactInference(typeArgsAll[i], typeArgsAll2[i]);
				}
			}
		}

		// Token: 0x060002F9 RID: 761 RVA: 0x0001478C File Offset: 0x0001298C
		private bool Fix(int iParam)
		{
			if (this._pExactBounds[iParam].Count >= 2)
			{
				return false;
			}
			List<CType> list = new List<CType>();
			if (this._pExactBounds[iParam].IsEmpty<CType>())
			{
				HashSet<CType> hashSet = new HashSet<CType>();
				foreach (CType ctype in this._pLowerBounds[iParam])
				{
					if (hashSet.Add(ctype))
					{
						list.Add(ctype);
					}
				}
				using (List<CType>.Enumerator enumerator = this._pUpperBounds[iParam].GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						CType ctype2 = enumerator.Current;
						if (hashSet.Add(ctype2))
						{
							list.Add(ctype2);
						}
					}
					goto IL_00CD;
				}
			}
			list.Add(this._pExactBounds[iParam].Head<CType>());
			IL_00CD:
			if (list.IsEmpty<CType>())
			{
				return false;
			}
			foreach (CType ctype3 in this._pLowerBounds[iParam])
			{
				List<CType> list2 = new List<CType>();
				foreach (CType ctype4 in list)
				{
					if (ctype3 != ctype4 && !this._binder.canConvert(ctype3, ctype4))
					{
						list2.Add(ctype4);
					}
				}
				foreach (CType ctype5 in list2)
				{
					list.Remove(ctype5);
				}
			}
			foreach (CType ctype6 in this._pUpperBounds[iParam])
			{
				List<CType> list3 = new List<CType>();
				foreach (CType ctype7 in list)
				{
					if (ctype6 != ctype7 && !this._binder.canConvert(ctype7, ctype6))
					{
						list3.Add(ctype7);
					}
				}
				foreach (CType ctype8 in list3)
				{
					list.Remove(ctype8);
				}
			}
			CType ctype9 = null;
			using (List<CType>.Enumerator enumerator = list.GetEnumerator())
			{
				IL_02C0:
				while (enumerator.MoveNext())
				{
					CType ctype10 = enumerator.Current;
					foreach (CType ctype11 in list)
					{
						if (ctype10 != ctype11 && !this._binder.canConvert(ctype11, ctype10))
						{
							goto IL_02C0;
						}
					}
					if (ctype9 != null)
					{
						return false;
					}
					ctype9 = ctype10;
				}
			}
			if (ctype9 == null)
			{
				return false;
			}
			CType ctype12;
			if (this.GetTypeManager().GetBestAccessibleType(this._binder.GetSemanticChecker(), this._binder.GetContext(), ctype9, out ctype12))
			{
				ctype9 = ctype12;
				this._pFixedResults[iParam] = ctype9;
				this.UpdateDependenciesAfterFix(iParam);
				return true;
			}
			return false;
		}

		// Token: 0x060002FA RID: 762 RVA: 0x00014B34 File Offset: 0x00012D34
		private bool InferForMethodGroupConversion()
		{
			for (int i = 0; i < this._pMethodArguments.carg; i++)
			{
				CType ctype = this._pMethodFormalParameterTypes[i];
				CType ctype2 = this._pMethodArguments.types[i];
				ParameterModifierType parameterModifierType;
				if ((parameterModifierType = ctype as ParameterModifierType) != null)
				{
					ctype = parameterModifierType.GetParameterType();
				}
				ParameterModifierType parameterModifierType2;
				if ((parameterModifierType2 = ctype2 as ParameterModifierType) != null)
				{
					ctype2 = parameterModifierType2.GetParameterType();
				}
				this.LowerBoundInference(ctype2, ctype);
			}
			bool flag = true;
			for (int j = 0; j < this._pMethodTypeParameters.Count; j++)
			{
				if (!this.HasBound(j) || !this.Fix(j))
				{
					flag = false;
				}
			}
			return flag;
		}

		// Token: 0x060002FB RID: 763 RVA: 0x00014BD7 File Offset: 0x00012DD7
		private SymbolLoader GetSymbolLoader()
		{
			return this._symbolLoader;
		}

		// Token: 0x060002FC RID: 764 RVA: 0x00014BDF File Offset: 0x00012DDF
		private TypeManager GetTypeManager()
		{
			return this.GetSymbolLoader().GetTypeManager();
		}

		// Token: 0x060002FD RID: 765 RVA: 0x00014BEC File Offset: 0x00012DEC
		private BSYMMGR GetGlobalSymbols()
		{
			return this.GetSymbolLoader().getBSymmgr();
		}

		// Token: 0x040003ED RID: 1005
		private readonly SymbolLoader _symbolLoader;

		// Token: 0x040003EE RID: 1006
		private readonly ExpressionBinder _binder;

		// Token: 0x040003EF RID: 1007
		private readonly TypeArray _pMethodTypeParameters;

		// Token: 0x040003F0 RID: 1008
		private readonly TypeArray _pClassTypeArguments;

		// Token: 0x040003F1 RID: 1009
		private readonly TypeArray _pMethodFormalParameterTypes;

		// Token: 0x040003F2 RID: 1010
		private readonly ArgInfos _pMethodArguments;

		// Token: 0x040003F3 RID: 1011
		private readonly List<CType>[] _pExactBounds;

		// Token: 0x040003F4 RID: 1012
		private readonly List<CType>[] _pUpperBounds;

		// Token: 0x040003F5 RID: 1013
		private readonly List<CType>[] _pLowerBounds;

		// Token: 0x040003F6 RID: 1014
		private readonly CType[] _pFixedResults;

		// Token: 0x040003F7 RID: 1015
		private MethodTypeInferrer.Dependency[][] _ppDependencies;

		// Token: 0x040003F8 RID: 1016
		private bool _dependenciesDirty;

		// Token: 0x020000EC RID: 236
		private enum NewInferenceResult
		{
			// Token: 0x04000706 RID: 1798
			InferenceFailed,
			// Token: 0x04000707 RID: 1799
			MadeProgress,
			// Token: 0x04000708 RID: 1800
			NoProgress,
			// Token: 0x04000709 RID: 1801
			Success
		}

		// Token: 0x020000ED RID: 237
		[Flags]
		private enum Dependency
		{
			// Token: 0x0400070B RID: 1803
			Unknown = 0,
			// Token: 0x0400070C RID: 1804
			NotDependent = 1,
			// Token: 0x0400070D RID: 1805
			DependsMask = 16,
			// Token: 0x0400070E RID: 1806
			Direct = 17,
			// Token: 0x0400070F RID: 1807
			Indirect = 18
		}
	}
}
