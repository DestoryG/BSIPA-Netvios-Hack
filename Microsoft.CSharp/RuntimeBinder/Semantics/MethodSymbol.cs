using System;
using System.Reflection;
using Microsoft.CSharp.RuntimeBinder.Syntax;

namespace Microsoft.CSharp.RuntimeBinder.Semantics
{
	// Token: 0x02000067 RID: 103
	internal sealed class MethodSymbol : MethodOrPropertySymbol
	{
		// Token: 0x06000390 RID: 912 RVA: 0x00016820 File Offset: 0x00014A20
		public bool InferenceMustFail()
		{
			if (this._checkedInfMustFail)
			{
				return this._inferenceMustFail;
			}
			this._checkedInfMustFail = true;
			int i = 0;
			IL_0063:
			while (i < this.typeVars.Count)
			{
				TypeParameterType typeParameterType = (TypeParameterType)this.typeVars[i];
				for (int j = 0; j < base.Params.Count; j++)
				{
					if (TypeManager.TypeContainsType(base.Params[j], typeParameterType))
					{
						i++;
						goto IL_0063;
					}
				}
				this._inferenceMustFail = true;
				return true;
			}
			return false;
		}

		// Token: 0x06000391 RID: 913 RVA: 0x0001689F File Offset: 0x00014A9F
		public MethodKindEnum MethKind()
		{
			return this._methKind;
		}

		// Token: 0x06000392 RID: 914 RVA: 0x000168A7 File Offset: 0x00014AA7
		public bool IsConstructor()
		{
			return this._methKind == MethodKindEnum.Constructor;
		}

		// Token: 0x06000393 RID: 915 RVA: 0x000168B2 File Offset: 0x00014AB2
		public bool IsNullableConstructor()
		{
			return base.getClass().isPredefAgg(PredefinedType.PT_G_OPTIONAL) && base.Params.Count == 1 && base.Params[0] is TypeParameterType && this.IsConstructor();
		}

		// Token: 0x06000394 RID: 916 RVA: 0x000168EC File Offset: 0x00014AEC
		public bool IsDestructor()
		{
			return this._methKind == MethodKindEnum.Destructor;
		}

		// Token: 0x06000395 RID: 917 RVA: 0x000168F7 File Offset: 0x00014AF7
		public bool isPropertyAccessor()
		{
			return this._methKind == MethodKindEnum.PropAccessor;
		}

		// Token: 0x06000396 RID: 918 RVA: 0x00016902 File Offset: 0x00014B02
		public bool isEventAccessor()
		{
			return this._methKind == MethodKindEnum.EventAccessor;
		}

		// Token: 0x06000397 RID: 919 RVA: 0x0001690D File Offset: 0x00014B0D
		private bool isExplicit()
		{
			return this._methKind == MethodKindEnum.ExplicitConv;
		}

		// Token: 0x06000398 RID: 920 RVA: 0x00016918 File Offset: 0x00014B18
		public bool isImplicit()
		{
			return this._methKind == MethodKindEnum.ImplicitConv;
		}

		// Token: 0x06000399 RID: 921 RVA: 0x00016923 File Offset: 0x00014B23
		public bool isInvoke()
		{
			return this._methKind == MethodKindEnum.Invoke;
		}

		// Token: 0x0600039A RID: 922 RVA: 0x0001692E File Offset: 0x00014B2E
		public void SetMethKind(MethodKindEnum mk)
		{
			this._methKind = mk;
		}

		// Token: 0x0600039B RID: 923 RVA: 0x00016937 File Offset: 0x00014B37
		public MethodSymbol ConvNext()
		{
			return this._convNext;
		}

		// Token: 0x0600039C RID: 924 RVA: 0x0001693F File Offset: 0x00014B3F
		public void SetConvNext(MethodSymbol conv)
		{
			this._convNext = conv;
		}

		// Token: 0x0600039D RID: 925 RVA: 0x00016948 File Offset: 0x00014B48
		public PropertySymbol getProperty()
		{
			return this._prop;
		}

		// Token: 0x0600039E RID: 926 RVA: 0x00016950 File Offset: 0x00014B50
		public void SetProperty(PropertySymbol prop)
		{
			this._prop = prop;
		}

		// Token: 0x0600039F RID: 927 RVA: 0x00016959 File Offset: 0x00014B59
		public EventSymbol getEvent()
		{
			return this._evt;
		}

		// Token: 0x060003A0 RID: 928 RVA: 0x00016961 File Offset: 0x00014B61
		public void SetEvent(EventSymbol evt)
		{
			this._evt = evt;
		}

		// Token: 0x060003A1 RID: 929 RVA: 0x0001696A File Offset: 0x00014B6A
		public bool isConversionOperator()
		{
			return this.isExplicit() || this.isImplicit();
		}

		// Token: 0x060003A2 RID: 930 RVA: 0x0001697C File Offset: 0x00014B7C
		public new bool isUserCallable()
		{
			return !this.isOperator && !this.isAnyAccessor();
		}

		// Token: 0x060003A3 RID: 931 RVA: 0x00016991 File Offset: 0x00014B91
		private bool isAnyAccessor()
		{
			return this.isPropertyAccessor() || this.isEventAccessor();
		}

		// Token: 0x060003A4 RID: 932 RVA: 0x000169A4 File Offset: 0x00014BA4
		public bool isSetAccessor()
		{
			if (!this.isPropertyAccessor())
			{
				return false;
			}
			PropertySymbol property = this.getProperty();
			return property != null && this == property.SetterMethod;
		}

		// Token: 0x040004BC RID: 1212
		private MethodKindEnum _methKind;

		// Token: 0x040004BD RID: 1213
		private bool _inferenceMustFail;

		// Token: 0x040004BE RID: 1214
		private bool _checkedInfMustFail;

		// Token: 0x040004BF RID: 1215
		private MethodSymbol _convNext;

		// Token: 0x040004C0 RID: 1216
		private PropertySymbol _prop;

		// Token: 0x040004C1 RID: 1217
		private EventSymbol _evt;

		// Token: 0x040004C2 RID: 1218
		public bool isVirtual;

		// Token: 0x040004C3 RID: 1219
		public bool isAbstract;

		// Token: 0x040004C4 RID: 1220
		public bool isVarargs;

		// Token: 0x040004C5 RID: 1221
		public MemberInfo AssociatedMemberInfo;

		// Token: 0x040004C6 RID: 1222
		public TypeArray typeVars;
	}
}
