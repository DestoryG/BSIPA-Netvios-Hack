using System;
using System.Reflection;
using Microsoft.CSharp.RuntimeBinder.Syntax;

namespace Microsoft.CSharp.RuntimeBinder.Semantics
{
	// Token: 0x02000071 RID: 113
	internal abstract class Symbol
	{
		// Token: 0x060003C4 RID: 964 RVA: 0x00016DD6 File Offset: 0x00014FD6
		public ACCESS GetAccess()
		{
			return this._access;
		}

		// Token: 0x060003C5 RID: 965 RVA: 0x00016DDE File Offset: 0x00014FDE
		public void SetAccess(ACCESS access)
		{
			this._access = access;
		}

		// Token: 0x060003C6 RID: 966 RVA: 0x00016DE7 File Offset: 0x00014FE7
		public SYMKIND getKind()
		{
			return this._kind;
		}

		// Token: 0x060003C7 RID: 967 RVA: 0x00016DEF File Offset: 0x00014FEF
		public void setKind(SYMKIND kind)
		{
			this._kind = kind;
		}

		// Token: 0x060003C8 RID: 968 RVA: 0x00016DF8 File Offset: 0x00014FF8
		public symbmask_t mask()
		{
			return (symbmask_t)(1 << (int)this._kind);
		}

		// Token: 0x060003C9 RID: 969 RVA: 0x00016E08 File Offset: 0x00015008
		public CType getType()
		{
			MethodOrPropertySymbol methodOrPropertySymbol;
			if ((methodOrPropertySymbol = this as MethodOrPropertySymbol) != null)
			{
				return methodOrPropertySymbol.RetType;
			}
			FieldSymbol fieldSymbol;
			if ((fieldSymbol = this as FieldSymbol) != null)
			{
				return fieldSymbol.GetType();
			}
			EventSymbol eventSymbol;
			if ((eventSymbol = this as EventSymbol) != null)
			{
				return eventSymbol.type;
			}
			return null;
		}

		// Token: 0x17000089 RID: 137
		// (get) Token: 0x060003CA RID: 970 RVA: 0x00016E4C File Offset: 0x0001504C
		public bool isStatic
		{
			get
			{
				FieldSymbol fieldSymbol;
				if ((fieldSymbol = this as FieldSymbol) != null)
				{
					return fieldSymbol.isStatic;
				}
				EventSymbol eventSymbol;
				if ((eventSymbol = this as EventSymbol) != null)
				{
					return eventSymbol.isStatic;
				}
				MethodOrPropertySymbol methodOrPropertySymbol;
				if ((methodOrPropertySymbol = this as MethodOrPropertySymbol) != null)
				{
					return methodOrPropertySymbol.isStatic;
				}
				return this is AggregateSymbol;
			}
		}

		// Token: 0x060003CB RID: 971 RVA: 0x00016E98 File Offset: 0x00015098
		private Assembly GetAssembly()
		{
			switch (this._kind)
			{
			case SYMKIND.SK_AggregateSymbol:
				return ((AggregateSymbol)this).AssociatedAssembly;
			case SYMKIND.SK_AggregateDeclaration:
				return ((AggregateDeclaration)this).GetAssembly();
			case SYMKIND.SK_TypeParameterSymbol:
			case SYMKIND.SK_FieldSymbol:
			case SYMKIND.SK_MethodSymbol:
			case SYMKIND.SK_PropertySymbol:
			case SYMKIND.SK_EventSymbol:
				return ((AggregateSymbol)this.parent).AssociatedAssembly;
			}
			return null;
		}

		// Token: 0x060003CC RID: 972 RVA: 0x00016F00 File Offset: 0x00015100
		private bool InternalsVisibleTo(Assembly assembly)
		{
			switch (this._kind)
			{
			case SYMKIND.SK_AggregateSymbol:
				return ((AggregateSymbol)this).InternalsVisibleTo(assembly);
			case SYMKIND.SK_AggregateDeclaration:
				return ((AggregateDeclaration)this).Agg().InternalsVisibleTo(assembly);
			case SYMKIND.SK_TypeParameterSymbol:
			case SYMKIND.SK_FieldSymbol:
			case SYMKIND.SK_MethodSymbol:
			case SYMKIND.SK_PropertySymbol:
			case SYMKIND.SK_EventSymbol:
				return ((AggregateSymbol)this.parent).InternalsVisibleTo(assembly);
			}
			return false;
		}

		// Token: 0x060003CD RID: 973 RVA: 0x00016F70 File Offset: 0x00015170
		public bool SameAssemOrFriend(Symbol sym)
		{
			Assembly assembly = this.GetAssembly();
			return assembly == sym.GetAssembly() || sym.InternalsVisibleTo(assembly);
		}

		// Token: 0x060003CE RID: 974 RVA: 0x00016F9C File Offset: 0x0001519C
		public bool IsVirtual()
		{
			switch (this._kind)
			{
			case SYMKIND.SK_MethodSymbol:
				return ((MethodSymbol)this).isVirtual;
			case SYMKIND.SK_PropertySymbol:
			{
				PropertySymbol propertySymbol = (PropertySymbol)this;
				MethodSymbol methodSymbol = propertySymbol.GetterMethod ?? propertySymbol.SetterMethod;
				return methodSymbol != null && methodSymbol.isVirtual;
			}
			case SYMKIND.SK_EventSymbol:
			{
				MethodSymbol methAdd = ((EventSymbol)this).methAdd;
				return methAdd != null && methAdd.isVirtual;
			}
			default:
				return false;
			}
		}

		// Token: 0x060003CF RID: 975 RVA: 0x00017010 File Offset: 0x00015210
		public bool IsOverride()
		{
			SYMKIND kind = this._kind;
			if (kind - SYMKIND.SK_MethodSymbol > 1)
			{
				return kind == SYMKIND.SK_EventSymbol && ((EventSymbol)this).isOverride;
			}
			return ((MethodOrPropertySymbol)this).isOverride;
		}

		// Token: 0x060003D0 RID: 976 RVA: 0x0001704C File Offset: 0x0001524C
		public bool IsHideByName()
		{
			SYMKIND kind = this._kind;
			if (kind - SYMKIND.SK_MethodSymbol <= 1)
			{
				return ((MethodOrPropertySymbol)this).isHideByName;
			}
			if (kind != SYMKIND.SK_EventSymbol)
			{
				return true;
			}
			MethodSymbol methAdd = ((EventSymbol)this).methAdd;
			return methAdd != null && methAdd.isHideByName;
		}

		// Token: 0x060003D1 RID: 977 RVA: 0x00017092 File Offset: 0x00015292
		public Symbol SymBaseVirtual()
		{
			MethodOrPropertySymbol methodOrPropertySymbol = this as MethodOrPropertySymbol;
			if (methodOrPropertySymbol == null)
			{
				return null;
			}
			return methodOrPropertySymbol.swtSlot.Sym;
		}

		// Token: 0x060003D2 RID: 978 RVA: 0x000170AC File Offset: 0x000152AC
		public bool isUserCallable()
		{
			MethodSymbol methodSymbol;
			return (methodSymbol = this as MethodSymbol) == null || methodSymbol.isUserCallable();
		}

		// Token: 0x040004E5 RID: 1253
		private SYMKIND _kind;

		// Token: 0x040004E6 RID: 1254
		private ACCESS _access;

		// Token: 0x040004E7 RID: 1255
		public Name name;

		// Token: 0x040004E8 RID: 1256
		public ParentSymbol parent;

		// Token: 0x040004E9 RID: 1257
		public Symbol nextChild;

		// Token: 0x040004EA RID: 1258
		public Symbol nextSameName;
	}
}
