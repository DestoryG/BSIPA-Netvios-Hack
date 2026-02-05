using System;
using System.Collections.Generic;

namespace Microsoft.CSharp.RuntimeBinder.Semantics
{
	// Token: 0x020000A2 RID: 162
	internal sealed class AggregateType : CType
	{
		// Token: 0x06000575 RID: 1397 RVA: 0x0001B4B0 File Offset: 0x000196B0
		public void SetOwningAggregate(AggregateSymbol agg)
		{
			this._pOwningAggregate = agg;
		}

		// Token: 0x06000576 RID: 1398 RVA: 0x0001B4B9 File Offset: 0x000196B9
		public AggregateSymbol GetOwningAggregate()
		{
			return this._pOwningAggregate;
		}

		// Token: 0x06000577 RID: 1399 RVA: 0x0001B4C4 File Offset: 0x000196C4
		public AggregateType GetBaseClass()
		{
			AggregateType aggregateType;
			if ((aggregateType = this._baseType) == null)
			{
				aggregateType = (this._baseType = base.getAggregate().GetTypeManager().SubstType(base.getAggregate().GetBaseClass(), this.GetTypeArgsAll()) as AggregateType);
			}
			return aggregateType;
		}

		// Token: 0x170000E7 RID: 231
		// (get) Token: 0x06000578 RID: 1400 RVA: 0x0001B50A File Offset: 0x0001970A
		public IEnumerable<AggregateType> TypeHierarchy
		{
			get
			{
				if (base.isInterfaceType())
				{
					yield return this;
					foreach (AggregateType aggregateType in this.GetIfacesAll().Items)
					{
						yield return aggregateType;
					}
					CType[] array = null;
					yield return base.getAggregate().GetTypeManager().ObjectAggregateType;
				}
				else
				{
					AggregateType agg;
					for (agg = this; agg != null; agg = agg.GetBaseClass())
					{
						yield return agg;
					}
					agg = null;
				}
				yield break;
			}
		}

		// Token: 0x06000579 RID: 1401 RVA: 0x0001B51C File Offset: 0x0001971C
		public void SetTypeArgsThis(TypeArray pTypeArgsThis)
		{
			TypeArray typeArray;
			if (this.outerType != null)
			{
				typeArray = this.outerType.GetTypeArgsAll();
			}
			else
			{
				typeArray = BSYMMGR.EmptyTypeArray();
			}
			this._pTypeArgsThis = pTypeArgsThis;
			this.SetTypeArgsAll(typeArray);
		}

		// Token: 0x0600057A RID: 1402 RVA: 0x0001B554 File Offset: 0x00019754
		private void SetTypeArgsAll(TypeArray outerTypeArgs)
		{
			TypeManager typeManager = base.getAggregate().GetTypeManager();
			this._pTypeArgsAll = typeManager.ConcatenateTypeArrays(outerTypeArgs, this._pTypeArgsThis);
		}

		// Token: 0x0600057B RID: 1403 RVA: 0x0001B582 File Offset: 0x00019782
		public TypeArray GetTypeArgsThis()
		{
			return this._pTypeArgsThis;
		}

		// Token: 0x0600057C RID: 1404 RVA: 0x0001B58A File Offset: 0x0001978A
		public TypeArray GetTypeArgsAll()
		{
			return this._pTypeArgsAll;
		}

		// Token: 0x0600057D RID: 1405 RVA: 0x0001B592 File Offset: 0x00019792
		public TypeArray GetIfacesAll()
		{
			if (this._ifacesAll == null)
			{
				this._ifacesAll = base.getAggregate().GetTypeManager().SubstTypeArray(base.getAggregate().GetIfacesAll(), this.GetTypeArgsAll());
			}
			return this._ifacesAll;
		}

		// Token: 0x0600057E RID: 1406 RVA: 0x0001B5CC File Offset: 0x000197CC
		public TypeArray GetWinRTCollectionIfacesAll(SymbolLoader pSymbolLoader)
		{
			if (this._winrtifacesAll == null)
			{
				TypeArray ifacesAll = this.GetIfacesAll();
				List<CType> list = new List<CType>();
				for (int i = 0; i < ifacesAll.Count; i++)
				{
					AggregateType aggregateType = ifacesAll[i] as AggregateType;
					if (aggregateType.IsCollectionType())
					{
						list.Add(aggregateType);
					}
				}
				this._winrtifacesAll = pSymbolLoader.getBSymmgr().AllocParams(list.Count, list.ToArray());
			}
			return this._winrtifacesAll;
		}

		// Token: 0x0600057F RID: 1407 RVA: 0x0001B640 File Offset: 0x00019840
		public TypeArray GetDelegateParameters(SymbolLoader pSymbolLoader)
		{
			MethodSymbol methodSymbol = pSymbolLoader.LookupInvokeMeth(base.getAggregate());
			if (methodSymbol == null || !methodSymbol.isInvoke())
			{
				return null;
			}
			return base.getAggregate().GetTypeManager().SubstTypeArray(methodSymbol.Params, this);
		}

		// Token: 0x06000580 RID: 1408 RVA: 0x0001B680 File Offset: 0x00019880
		public CType GetDelegateReturnType(SymbolLoader pSymbolLoader)
		{
			MethodSymbol methodSymbol = pSymbolLoader.LookupInvokeMeth(base.getAggregate());
			if (methodSymbol == null || !methodSymbol.isInvoke())
			{
				return null;
			}
			return base.getAggregate().GetTypeManager().SubstType(methodSymbol.RetType, this);
		}

		// Token: 0x0400056F RID: 1391
		private TypeArray _pTypeArgsThis;

		// Token: 0x04000570 RID: 1392
		private TypeArray _pTypeArgsAll;

		// Token: 0x04000571 RID: 1393
		private AggregateSymbol _pOwningAggregate;

		// Token: 0x04000572 RID: 1394
		private AggregateType _baseType;

		// Token: 0x04000573 RID: 1395
		private TypeArray _ifacesAll;

		// Token: 0x04000574 RID: 1396
		private TypeArray _winrtifacesAll;

		// Token: 0x04000575 RID: 1397
		public bool fConstraintsChecked;

		// Token: 0x04000576 RID: 1398
		public bool fConstraintError;

		// Token: 0x04000577 RID: 1399
		public bool fAllHidden;

		// Token: 0x04000578 RID: 1400
		public bool fDiffHidden;

		// Token: 0x04000579 RID: 1401
		public AggregateType outerType;
	}
}
