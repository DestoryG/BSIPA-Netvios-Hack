using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;
using Microsoft.CSharp.RuntimeBinder.Semantics;
using Microsoft.CSharp.RuntimeBinder.Syntax;

namespace Microsoft.CSharp.RuntimeBinder
{
	// Token: 0x02000023 RID: 35
	internal sealed class SymbolTable
	{
		// Token: 0x06000126 RID: 294 RVA: 0x00006B28 File Offset: 0x00004D28
		internal SymbolTable(SYMTBL symTable, SymFactory symFactory, NameManager nameManager, TypeManager typeManager, BSYMMGR bsymmgr, CSemanticChecker semanticChecker)
		{
			this._symbolTable = symTable;
			this._symFactory = symFactory;
			this._nameManager = nameManager;
			this._typeManager = typeManager;
			this._bsymmgr = bsymmgr;
			this._semanticChecker = semanticChecker;
			this._rootNamespace = this._bsymmgr.GetRootNS();
			this.LoadSymbolsFromType(typeof(object));
		}

		// Token: 0x06000127 RID: 295 RVA: 0x00006BA0 File Offset: 0x00004DA0
		internal void PopulateSymbolTableWithName(string name, IEnumerable<Type> typeArguments, Type callingType)
		{
			if (callingType.IsGenericType)
			{
				callingType = callingType.GetGenericTypeDefinition();
			}
			if (name == "$Item$")
			{
				name = callingType.GetIndexerName() ?? "$Item$";
			}
			SymbolTable.NameHashKey nameHashKey = new SymbolTable.NameHashKey(callingType, name);
			if (this._namesLoadedForEachType.Contains(nameHashKey))
			{
				return;
			}
			this.AddNamesOnType(nameHashKey);
			if (typeArguments != null)
			{
				foreach (Type type in typeArguments)
				{
					this.AddConversionsForType(type);
				}
			}
		}

		// Token: 0x06000128 RID: 296 RVA: 0x00006C38 File Offset: 0x00004E38
		internal SymWithType LookupMember(string name, Expr callingObject, ParentSymbol context, int arity, MemberLookup mem, bool allowSpecialNames, bool requireInvocable)
		{
			CType ctype = callingObject.Type;
			if (ctype is ArrayType)
			{
				ctype = this._semanticChecker.SymbolLoader.GetPredefindType(PredefinedType.PT_ARRAY);
			}
			NullableType nullableType;
			if ((nullableType = ctype as NullableType) != null)
			{
				ctype = nullableType.GetAts();
			}
			if (!mem.Lookup(this._semanticChecker, ctype, callingObject, context, this.GetName(name), arity, (allowSpecialNames ? MemLookFlags.None : MemLookFlags.UserCallable) | ((name == "$Item$") ? MemLookFlags.Indexer : MemLookFlags.None) | ((name == ".ctor") ? MemLookFlags.Ctor : MemLookFlags.None) | (requireInvocable ? MemLookFlags.MustBeInvocable : MemLookFlags.None)))
			{
				return null;
			}
			return mem.SwtFirst();
		}

		// Token: 0x06000129 RID: 297 RVA: 0x00006CDC File Offset: 0x00004EDC
		private void AddParameterConversions(MethodBase method)
		{
			foreach (ParameterInfo parameterInfo in method.GetParameters())
			{
				this.AddConversionsForType(parameterInfo.ParameterType);
			}
		}

		// Token: 0x0600012A RID: 298 RVA: 0x00006D10 File Offset: 0x00004F10
		private void AddNamesOnType(SymbolTable.NameHashKey key)
		{
			List<Type> list = this.CreateInheritanceHierarchyList(key.type);
			this.AddNamesInInheritanceHierarchy(key.name, list);
		}

		// Token: 0x0600012B RID: 299 RVA: 0x00006D38 File Offset: 0x00004F38
		private void AddNamesInInheritanceHierarchy(string name, List<Type> inheritance)
		{
			for (int i = inheritance.Count - 1; i >= 0; i--)
			{
				Type type = inheritance[i];
				if (type.IsGenericType)
				{
					type = type.GetGenericTypeDefinition();
				}
				if (this._namesLoadedForEachType.Add(new SymbolTable.NameHashKey(type, name)))
				{
					IEnumerator<MemberInfo> enumerator = (from member in type.GetMembers(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic)
						where member.DeclaringType == type && member.Name == name
						select member).GetEnumerator();
					if (enumerator.MoveNext())
					{
						List<EventInfo> list = null;
						AggregateType aggregateType;
						if ((aggregateType = this.GetCTypeFromType(type) as AggregateType) != null)
						{
							AggregateSymbol aggregate = aggregateType.getAggregate();
							FieldSymbol fieldSymbol = null;
							do
							{
								MemberInfo memberInfo = enumerator.Current;
								MethodInfo methodInfo;
								ConstructorInfo constructorInfo;
								PropertyInfo propertyInfo;
								FieldInfo fieldInfo;
								EventInfo eventInfo;
								if ((methodInfo = memberInfo as MethodInfo) != null)
								{
									string name2 = memberInfo.Name;
									MethodKindEnum methodKindEnum;
									if (!(name2 == "Invoke"))
									{
										if (!(name2 == "op_Implicit"))
										{
											if (!(name2 == "op_Explicit"))
											{
												methodKindEnum = MethodKindEnum.Actual;
											}
											else
											{
												methodKindEnum = MethodKindEnum.ExplicitConv;
											}
										}
										else
										{
											methodKindEnum = MethodKindEnum.ImplicitConv;
										}
									}
									else
									{
										methodKindEnum = MethodKindEnum.Invoke;
									}
									this.AddMethodToSymbolTable(methodInfo, aggregate, methodKindEnum);
									this.AddParameterConversions(methodInfo);
								}
								else if ((constructorInfo = memberInfo as ConstructorInfo) != null)
								{
									this.AddMethodToSymbolTable(constructorInfo, aggregate, MethodKindEnum.Constructor);
									this.AddParameterConversions(constructorInfo);
								}
								else if ((propertyInfo = memberInfo as PropertyInfo) != null)
								{
									this.AddPropertyToSymbolTable(propertyInfo, aggregate);
								}
								else if ((fieldInfo = memberInfo as FieldInfo) != null)
								{
									fieldSymbol = this.AddFieldToSymbolTable(fieldInfo, aggregate);
								}
								else if ((eventInfo = memberInfo as EventInfo) != null)
								{
									(list = list ?? new List<EventInfo>()).Add(eventInfo);
								}
							}
							while (enumerator.MoveNext());
							if (list != null)
							{
								foreach (EventInfo eventInfo2 in list)
								{
									this.AddEventToSymbolTable(eventInfo2, aggregate, fieldSymbol);
								}
							}
						}
					}
				}
			}
		}

		// Token: 0x0600012C RID: 300 RVA: 0x00006F60 File Offset: 0x00005160
		private List<Type> CreateInheritanceHierarchyList(Type type)
		{
			List<Type> list;
			if (type.IsInterface)
			{
				list = new List<Type>(type.GetInterfaces().Length + 2) { type };
				foreach (Type type2 in type.GetInterfaces())
				{
					this.LoadSymbolsFromType(type2);
					list.Add(type2);
				}
				Type typeFromHandle = typeof(object);
				this.LoadSymbolsFromType(typeFromHandle);
				list.Add(typeFromHandle);
			}
			else
			{
				list = new List<Type> { type };
				Type type3 = type.BaseType;
				while (type3 != null)
				{
					this.LoadSymbolsFromType(type3);
					list.Add(type3);
					type3 = type3.BaseType;
				}
			}
			CType ctypeFromType = this.GetCTypeFromType(type);
			if (ctypeFromType.IsWindowsRuntimeType())
			{
				TypeArray winRTCollectionIfacesAll = ((AggregateType)ctypeFromType).GetWinRTCollectionIfacesAll(this._semanticChecker.SymbolLoader);
				for (int j = 0; j < winRTCollectionIfacesAll.Count; j++)
				{
					CType ctype = winRTCollectionIfacesAll[j];
					list.Add(ctype.AssociatedSystemType);
				}
			}
			return list;
		}

		// Token: 0x0600012D RID: 301 RVA: 0x0000706A File Offset: 0x0000526A
		private Name GetName(string p)
		{
			return this._nameManager.Add(p ?? "");
		}

		// Token: 0x0600012E RID: 302 RVA: 0x00007084 File Offset: 0x00005284
		private Name GetName(Type type)
		{
			string name = type.Name;
			if (type.IsGenericType)
			{
				int num = name.IndexOf('`');
				if (num >= 0)
				{
					return this._nameManager.Add(name, num);
				}
			}
			return this._nameManager.Add(name);
		}

		// Token: 0x0600012F RID: 303 RVA: 0x000070C8 File Offset: 0x000052C8
		private TypeArray GetMethodTypeParameters(MethodInfo method, MethodSymbol parent)
		{
			if (method.IsGenericMethod)
			{
				Type[] genericArguments = method.GetGenericArguments();
				CType[] array = new CType[genericArguments.Length];
				for (int i = 0; i < genericArguments.Length; i++)
				{
					Type type = genericArguments[i];
					array[i] = this.LoadMethodTypeParameter(parent, type);
				}
				for (int j = 0; j < genericArguments.Length; j++)
				{
					Type type2 = genericArguments[j];
					((TypeParameterType)array[j]).GetTypeParameterSymbol().SetBounds(this._bsymmgr.AllocParams(this.GetCTypeArrayFromTypes(type2.GetGenericParameterConstraints())));
				}
				return this._bsymmgr.AllocParams(array.Length, array);
			}
			return BSYMMGR.EmptyTypeArray();
		}

		// Token: 0x06000130 RID: 304 RVA: 0x00007168 File Offset: 0x00005368
		private TypeArray GetAggregateTypeParameters(Type type, AggregateSymbol agg)
		{
			if (type.IsGenericType)
			{
				Type genericTypeDefinition = type.GetGenericTypeDefinition();
				Type[] genericArguments = genericTypeDefinition.GetGenericArguments();
				List<CType> list = new List<CType>();
				int num = (agg.isNested() ? agg.GetOuterAgg().GetTypeVarsAll().Count : 0);
				foreach (Type type2 in genericArguments)
				{
					if (type2.GenericParameterPosition >= num)
					{
						CType ctype;
						if (type2.IsGenericParameter && type2.DeclaringType == genericTypeDefinition)
						{
							ctype = this.LoadClassTypeParameter(agg, type2);
						}
						else
						{
							ctype = this.GetCTypeFromType(type2);
						}
						if (((TypeParameterType)ctype).GetOwningSymbol() == agg)
						{
							list.Add(ctype);
						}
					}
				}
				return this._bsymmgr.AllocParams(list.Count, list.ToArray());
			}
			return BSYMMGR.EmptyTypeArray();
		}

		// Token: 0x06000131 RID: 305 RVA: 0x00007238 File Offset: 0x00005438
		private TypeParameterType LoadClassTypeParameter(AggregateSymbol parent, Type t)
		{
			for (AggregateSymbol aggregateSymbol = parent; aggregateSymbol != null; aggregateSymbol = aggregateSymbol.parent as AggregateSymbol)
			{
				for (TypeParameterSymbol typeParameterSymbol = this._bsymmgr.LookupAggMember(this.GetName(t), aggregateSymbol, symbmask_t.MASK_TypeParameterSymbol) as TypeParameterSymbol; typeParameterSymbol != null; typeParameterSymbol = BSYMMGR.LookupNextSym(typeParameterSymbol, aggregateSymbol, symbmask_t.MASK_TypeParameterSymbol) as TypeParameterSymbol)
				{
					if (this.AreTypeParametersEquivalent(typeParameterSymbol.GetTypeParameterType().AssociatedSystemType, t))
					{
						return typeParameterSymbol.GetTypeParameterType();
					}
				}
			}
			return this.AddTypeParameterToSymbolTable(parent, null, t, true);
		}

		// Token: 0x06000132 RID: 306 RVA: 0x000072B0 File Offset: 0x000054B0
		private bool AreTypeParametersEquivalent(Type t1, Type t2)
		{
			if (t1 == t2)
			{
				return true;
			}
			Type originalTypeParameterType = this.GetOriginalTypeParameterType(t1);
			Type originalTypeParameterType2 = this.GetOriginalTypeParameterType(t2);
			return originalTypeParameterType == originalTypeParameterType2;
		}

		// Token: 0x06000133 RID: 307 RVA: 0x000072E0 File Offset: 0x000054E0
		private Type GetOriginalTypeParameterType(Type t)
		{
			int genericParameterPosition = t.GenericParameterPosition;
			Type type = t.DeclaringType;
			if (type != null && type.IsGenericType)
			{
				type = type.GetGenericTypeDefinition();
			}
			if (t.DeclaringMethod != null && (type.GetGenericArguments() == null || genericParameterPosition >= type.GetGenericArguments().Length))
			{
				return t;
			}
			while (type.GetGenericArguments().Length > genericParameterPosition)
			{
				Type type2 = type.DeclaringType;
				if (type2 != null && type2.IsGenericType)
				{
					type2 = type2.GetGenericTypeDefinition();
				}
				if (type2 == null)
				{
					break;
				}
				Type[] genericArguments = type2.GetGenericArguments();
				if (!(((genericArguments != null) ? new int?(genericArguments.Length) : null) > genericParameterPosition))
				{
					break;
				}
				type = type2;
			}
			return type.GetGenericArguments()[genericParameterPosition];
		}

		// Token: 0x06000134 RID: 308 RVA: 0x000073A8 File Offset: 0x000055A8
		private TypeParameterType LoadMethodTypeParameter(MethodSymbol parent, Type t)
		{
			for (Symbol symbol = parent.firstChild; symbol != null; symbol = symbol.nextChild)
			{
				TypeParameterSymbol typeParameterSymbol;
				if ((typeParameterSymbol = symbol as TypeParameterSymbol) != null)
				{
					TypeParameterType typeParameterType = typeParameterSymbol.GetTypeParameterType();
					if (this.AreTypeParametersEquivalent(typeParameterType.AssociatedSystemType, t))
					{
						return typeParameterType;
					}
				}
			}
			return this.AddTypeParameterToSymbolTable(null, parent, t, false);
		}

		// Token: 0x06000135 RID: 309 RVA: 0x000073F4 File Offset: 0x000055F4
		private TypeParameterType AddTypeParameterToSymbolTable(AggregateSymbol agg, MethodSymbol meth, Type t, bool bIsAggregate)
		{
			TypeParameterSymbol typeParameterSymbol;
			if (bIsAggregate)
			{
				typeParameterSymbol = this._symFactory.CreateClassTypeParameter(this.GetName(t), agg, t.GenericParameterPosition, t.GenericParameterPosition);
			}
			else
			{
				typeParameterSymbol = this._symFactory.CreateMethodTypeParameter(this.GetName(t), meth, t.GenericParameterPosition, t.GenericParameterPosition);
			}
			if ((t.GenericParameterAttributes & GenericParameterAttributes.Covariant) != GenericParameterAttributes.None)
			{
				typeParameterSymbol.Covariant = true;
			}
			if ((t.GenericParameterAttributes & GenericParameterAttributes.Contravariant) != GenericParameterAttributes.None)
			{
				typeParameterSymbol.Contravariant = true;
			}
			SpecCons specCons = SpecCons.None;
			if ((t.GenericParameterAttributes & GenericParameterAttributes.DefaultConstructorConstraint) != GenericParameterAttributes.None)
			{
				specCons |= SpecCons.New;
			}
			if ((t.GenericParameterAttributes & GenericParameterAttributes.ReferenceTypeConstraint) != GenericParameterAttributes.None)
			{
				specCons |= SpecCons.Ref;
			}
			if ((t.GenericParameterAttributes & GenericParameterAttributes.NotNullableValueTypeConstraint) != GenericParameterAttributes.None)
			{
				specCons |= SpecCons.Val;
			}
			typeParameterSymbol.SetConstraints(specCons);
			typeParameterSymbol.SetAccess(ACCESS.ACC_PUBLIC);
			return this._typeManager.GetTypeParameter(typeParameterSymbol);
		}

		// Token: 0x06000136 RID: 310 RVA: 0x000074B0 File Offset: 0x000056B0
		private CType LoadSymbolsFromType(Type originalType)
		{
			List<object> list = SymbolTable.BuildDeclarationChain(originalType);
			Type type = originalType;
			CType ctype = null;
			bool isByRef = type.IsByRef;
			if (isByRef)
			{
				type = type.GetElementType();
			}
			NamespaceOrAggregateSymbol namespaceOrAggregateSymbol = this._rootNamespace;
			for (int i = 0; i < list.Count; i++)
			{
				object obj = list[i];
				NamespaceOrAggregateSymbol namespaceOrAggregateSymbol2;
				if (obj is Type)
				{
					Type type2 = obj as Type;
					Name name = this.GetName(type2);
					namespaceOrAggregateSymbol2 = this._symbolTable.LookupSym(name, namespaceOrAggregateSymbol, symbmask_t.MASK_AggregateSymbol) as AggregateSymbol;
					if (namespaceOrAggregateSymbol2 != null)
					{
						namespaceOrAggregateSymbol2 = this.FindSymWithMatchingArity(namespaceOrAggregateSymbol2 as AggregateSymbol, type2);
					}
					if (namespaceOrAggregateSymbol2 != null)
					{
						Type associatedSystemType = (namespaceOrAggregateSymbol2 as AggregateSymbol).AssociatedSystemType;
						Type type3 = (type2.IsGenericType ? type2.GetGenericTypeDefinition() : type2);
						if (!associatedSystemType.IsEquivalentTo(type3))
						{
							throw new ResetBindException();
						}
					}
					if (namespaceOrAggregateSymbol2 == null || type2.IsNullableType())
					{
						CType ctype2 = this.ProcessSpecialTypeInChain(namespaceOrAggregateSymbol, type2);
						if (ctype2 != null)
						{
							AggregateType aggregateType;
							if ((aggregateType = ctype2 as AggregateType) == null)
							{
								ctype = ctype2;
								break;
							}
							namespaceOrAggregateSymbol2 = aggregateType.GetOwningAggregate();
						}
						else
						{
							namespaceOrAggregateSymbol2 = this.AddAggregateToSymbolTable(namespaceOrAggregateSymbol, type2);
						}
					}
					if (type2 == type)
					{
						ctype = this.GetConstructedType(type, namespaceOrAggregateSymbol2 as AggregateSymbol);
						break;
					}
				}
				else
				{
					if (obj is MethodInfo)
					{
						ctype = this.ProcessMethodTypeParameter(obj as MethodInfo, list[i + 1] as Type, namespaceOrAggregateSymbol as AggregateSymbol);
						break;
					}
					namespaceOrAggregateSymbol2 = this.AddNamespaceToSymbolTable(namespaceOrAggregateSymbol, obj as string);
				}
				namespaceOrAggregateSymbol = namespaceOrAggregateSymbol2;
			}
			if (isByRef)
			{
				ctype = this._typeManager.GetParameterModifier(ctype, false);
			}
			return ctype;
		}

		// Token: 0x06000137 RID: 311 RVA: 0x00007648 File Offset: 0x00005848
		private TypeParameterType ProcessMethodTypeParameter(MethodInfo methinfo, Type t, AggregateSymbol parent)
		{
			MethodSymbol methodSymbol = this.FindMatchingMethod(methinfo, parent);
			if (methodSymbol == null)
			{
				methodSymbol = this.AddMethodToSymbolTable(methinfo, parent, MethodKindEnum.Actual);
			}
			return this.LoadMethodTypeParameter(methodSymbol, t);
		}

		// Token: 0x06000138 RID: 312 RVA: 0x00007674 File Offset: 0x00005874
		private CType GetConstructedType(Type type, AggregateSymbol agg)
		{
			if (type.IsGenericType)
			{
				List<CType> list = new List<CType>();
				foreach (Type type2 in type.GetGenericArguments())
				{
					list.Add(this.GetCTypeFromType(type2));
				}
				TypeArray typeArray = this._bsymmgr.AllocParams(list.ToArray());
				return this._typeManager.GetAggregate(agg, typeArray);
			}
			return agg.getThisType();
		}

		// Token: 0x06000139 RID: 313 RVA: 0x000076E0 File Offset: 0x000058E0
		private CType ProcessSpecialTypeInChain(NamespaceOrAggregateSymbol parent, Type t)
		{
			if (t.IsGenericParameter)
			{
				AggregateSymbol aggregateSymbol = parent as AggregateSymbol;
				return this.LoadClassTypeParameter(aggregateSymbol, t);
			}
			if (t.IsArray)
			{
				return this._typeManager.GetArray(this.GetCTypeFromType(t.GetElementType()), t.GetArrayRank(), t.GetElementType().MakeArrayType() == t);
			}
			if (t.IsPointer)
			{
				return this._typeManager.GetPointer(this.GetCTypeFromType(t.GetElementType()));
			}
			if (!t.IsNullableType())
			{
				return null;
			}
			if (t.GetGenericArguments()[0].DeclaringType == t)
			{
				AggregateSymbol aggregateSymbol2 = this._symbolTable.LookupSym(this.GetName(t), parent, symbmask_t.MASK_AggregateSymbol) as AggregateSymbol;
				if (aggregateSymbol2 != null)
				{
					aggregateSymbol2 = this.FindSymWithMatchingArity(aggregateSymbol2, t);
					if (aggregateSymbol2 != null)
					{
						return aggregateSymbol2.getThisType();
					}
				}
				return this.AddAggregateToSymbolTable(parent, t).getThisType();
			}
			return this._typeManager.GetNullable(this.GetCTypeFromType(t.GetGenericArguments()[0]));
		}

		// Token: 0x0600013A RID: 314 RVA: 0x000077D4 File Offset: 0x000059D4
		private static List<object> BuildDeclarationChain(Type callingType)
		{
			if (callingType.IsByRef)
			{
				callingType = callingType.GetElementType();
			}
			List<object> list = new List<object>();
			Type type = callingType;
			while (type != null)
			{
				list.Add(type);
				if (type.IsGenericParameter && type.DeclaringMethod != null)
				{
					MethodBase methodBase = type.DeclaringMethod;
					IEnumerable<MethodInfo> runtimeMethods = type.DeclaringType.GetRuntimeMethods();
					Func<MethodInfo, bool> func;
					Func<MethodInfo, bool> <>9__0;
					if ((func = <>9__0) == null)
					{
						func = (<>9__0 = (MethodInfo m) => m.HasSameMetadataDefinitionAs(methodBase));
					}
					foreach (MethodInfo methodInfo in runtimeMethods.Where(func))
					{
						if (methodInfo.IsGenericMethod)
						{
							list.Add(methodInfo);
						}
					}
				}
				type = type.DeclaringType;
			}
			list.Reverse();
			if (callingType.Namespace != null)
			{
				string[] array = callingType.Namespace.Split(new char[] { '.' });
				int num = 0;
				foreach (string text in array)
				{
					list.Insert(num++, text);
				}
			}
			return list;
		}

		// Token: 0x0600013B RID: 315 RVA: 0x0000790C File Offset: 0x00005B0C
		private AggregateSymbol FindSymWithMatchingArity(AggregateSymbol aggregateSymbol, Type type)
		{
			for (AggregateSymbol aggregateSymbol2 = aggregateSymbol; aggregateSymbol2 != null; aggregateSymbol2 = BSYMMGR.LookupNextSym(aggregateSymbol2, aggregateSymbol2.Parent, symbmask_t.MASK_AggregateSymbol) as AggregateSymbol)
			{
				if (aggregateSymbol2.GetTypeVarsAll().Count == type.GetGenericArguments().Length)
				{
					return aggregateSymbol2;
				}
			}
			return null;
		}

		// Token: 0x0600013C RID: 316 RVA: 0x0000794C File Offset: 0x00005B4C
		private NamespaceSymbol AddNamespaceToSymbolTable(NamespaceOrAggregateSymbol parent, string sz)
		{
			Name name = this.GetName(sz);
			return (this._symbolTable.LookupSym(name, parent, symbmask_t.MASK_NamespaceSymbol) as NamespaceSymbol) ?? this._symFactory.CreateNamespace(name, parent as NamespaceSymbol);
		}

		// Token: 0x0600013D RID: 317 RVA: 0x0000798C File Offset: 0x00005B8C
		internal CType[] GetCTypeArrayFromTypes(Type[] types)
		{
			int num = types.Length;
			if (num == 0)
			{
				return Array.Empty<CType>();
			}
			CType[] array = new CType[num];
			for (int i = 0; i < types.Length; i++)
			{
				Type type = types[i];
				array[i] = this.GetCTypeFromType(type);
			}
			return array;
		}

		// Token: 0x0600013E RID: 318 RVA: 0x000079CA File Offset: 0x00005BCA
		internal CType GetCTypeFromType(Type t)
		{
			return this.LoadSymbolsFromType(t);
		}

		// Token: 0x0600013F RID: 319 RVA: 0x000079D4 File Offset: 0x00005BD4
		private AggregateSymbol AddAggregateToSymbolTable(NamespaceOrAggregateSymbol parent, Type type)
		{
			AggregateSymbol aggregateSymbol = this._symFactory.CreateAggregate(this.GetName(type), parent, this._typeManager);
			aggregateSymbol.AssociatedSystemType = (type.IsGenericType ? type.GetGenericTypeDefinition() : type);
			aggregateSymbol.AssociatedAssembly = type.Assembly;
			AggKindEnum aggKindEnum;
			if (type.IsInterface)
			{
				aggKindEnum = AggKindEnum.Interface;
			}
			else if (type.IsEnum)
			{
				aggKindEnum = AggKindEnum.Enum;
				aggregateSymbol.SetUnderlyingType((AggregateType)this.GetCTypeFromType(Enum.GetUnderlyingType(type)));
			}
			else if (type.IsValueType)
			{
				aggKindEnum = AggKindEnum.Struct;
			}
			else if (type.BaseType != null && (type.BaseType.FullName == "System.MulticastDelegate" || type.BaseType.FullName == "System.Delegate") && type.FullName != "System.MulticastDelegate")
			{
				aggKindEnum = AggKindEnum.Delegate;
			}
			else
			{
				aggKindEnum = AggKindEnum.Class;
			}
			aggregateSymbol.SetAggKind(aggKindEnum);
			aggregateSymbol.SetTypeVars(BSYMMGR.EmptyTypeArray());
			ACCESS access;
			if (type.IsPublic)
			{
				access = ACCESS.ACC_PUBLIC;
			}
			else if (type.IsNested)
			{
				if (type.IsNestedAssembly || type.IsNestedFamANDAssem)
				{
					access = ACCESS.ACC_INTERNAL;
				}
				else if (type.IsNestedFamORAssem)
				{
					access = ACCESS.ACC_INTERNALPROTECTED;
				}
				else if (type.IsNestedPrivate)
				{
					access = ACCESS.ACC_PRIVATE;
				}
				else if (type.IsNestedFamily)
				{
					access = ACCESS.ACC_PROTECTED;
				}
				else
				{
					access = ACCESS.ACC_PUBLIC;
				}
			}
			else
			{
				access = ACCESS.ACC_INTERNAL;
			}
			aggregateSymbol.SetAccess(access);
			if (!type.IsGenericParameter)
			{
				aggregateSymbol.SetTypeVars(this.GetAggregateTypeParameters(type, aggregateSymbol));
			}
			if (type.IsGenericType)
			{
				Type[] genericArguments = type.GetGenericTypeDefinition().GetGenericArguments();
				for (int i = 0; i < aggregateSymbol.GetTypeVars().Count; i++)
				{
					Type type2 = genericArguments[i];
					TypeParameterType typeParameterType;
					if ((typeParameterType = aggregateSymbol.GetTypeVars()[i] as TypeParameterType) != null)
					{
						typeParameterType.GetTypeParameterSymbol().SetBounds(this._bsymmgr.AllocParams(this.GetCTypeArrayFromTypes(type2.GetGenericParameterConstraints())));
					}
				}
			}
			aggregateSymbol.SetAbstract(type.IsAbstract);
			string text = type.FullName;
			if (type.IsGenericType)
			{
				text = type.GetGenericTypeDefinition().FullName;
			}
			if (text != null)
			{
				PredefinedType predefinedType = PredefinedTypeFacts.TryGetPredefTypeIndex(text);
				if (predefinedType != (PredefinedType)4294967295U)
				{
					PredefinedTypes.InitializePredefinedType(aggregateSymbol, predefinedType);
				}
			}
			aggregateSymbol.SetSealed(type.IsSealed);
			if (type.BaseType != null)
			{
				Type type3 = type.BaseType;
				if (type3.IsGenericType)
				{
					type3 = type3.GetGenericTypeDefinition();
				}
				aggregateSymbol.SetBaseClass((AggregateType)this.GetCTypeFromType(type3));
			}
			aggregateSymbol.SetTypeManager(this._typeManager);
			aggregateSymbol.SetFirstUDConversion(null);
			this.SetInterfacesOnAggregate(aggregateSymbol, type);
			aggregateSymbol.SetHasPubNoArgCtor(type.GetConstructor(Type.EmptyTypes) != null);
			if (aggregateSymbol.IsDelegate())
			{
				this.PopulateSymbolTableWithName(".ctor", null, type);
				this.PopulateSymbolTableWithName("Invoke", null, type);
			}
			return aggregateSymbol;
		}

		// Token: 0x06000140 RID: 320 RVA: 0x00007C80 File Offset: 0x00005E80
		private void SetInterfacesOnAggregate(AggregateSymbol aggregate, Type type)
		{
			if (type.IsGenericType)
			{
				type = type.GetGenericTypeDefinition();
			}
			Type[] interfaces = type.GetInterfaces();
			aggregate.SetIfaces(this._bsymmgr.AllocParams(interfaces.Length, this.GetCTypeArrayFromTypes(interfaces)));
			aggregate.SetIfacesAll(aggregate.GetIfaces());
		}

		// Token: 0x06000141 RID: 321 RVA: 0x00007CCC File Offset: 0x00005ECC
		private FieldSymbol AddFieldToSymbolTable(FieldInfo fieldInfo, AggregateSymbol aggregate)
		{
			FieldSymbol fieldSymbol = this._symbolTable.LookupSym(this.GetName(fieldInfo.Name), aggregate, symbmask_t.MASK_FieldSymbol) as FieldSymbol;
			if (fieldSymbol != null)
			{
				return fieldSymbol;
			}
			fieldSymbol = this._symFactory.CreateMemberVar(this.GetName(fieldInfo.Name), aggregate);
			fieldSymbol.AssociatedFieldInfo = fieldInfo;
			fieldSymbol.isStatic = fieldInfo.IsStatic;
			ACCESS access;
			if (fieldInfo.IsPublic)
			{
				access = ACCESS.ACC_PUBLIC;
			}
			else if (fieldInfo.IsPrivate)
			{
				access = ACCESS.ACC_PRIVATE;
			}
			else if (fieldInfo.IsFamily)
			{
				access = ACCESS.ACC_PROTECTED;
			}
			else if (fieldInfo.IsAssembly || fieldInfo.IsFamilyAndAssembly)
			{
				access = ACCESS.ACC_INTERNAL;
			}
			else
			{
				access = ACCESS.ACC_INTERNALPROTECTED;
			}
			fieldSymbol.SetAccess(access);
			fieldSymbol.isReadOnly = fieldInfo.IsInitOnly;
			fieldSymbol.isEvent = false;
			fieldSymbol.isAssigned = true;
			fieldSymbol.SetType(this.GetCTypeFromType(fieldInfo.FieldType));
			return fieldSymbol;
		}

		// Token: 0x1700005A RID: 90
		// (get) Token: 0x06000142 RID: 322 RVA: 0x00007D99 File Offset: 0x00005F99
		internal static Type EventRegistrationTokenType
		{
			get
			{
				return SymbolTable.GetTypeByName(ref SymbolTable.s_EventRegistrationTokenType, "System.Runtime.InteropServices.WindowsRuntime.EventRegistrationToken, System.Runtime.InteropServices.WindowsRuntime, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a");
			}
		}

		// Token: 0x1700005B RID: 91
		// (get) Token: 0x06000143 RID: 323 RVA: 0x00007DAA File Offset: 0x00005FAA
		internal static Type WindowsRuntimeMarshalType
		{
			get
			{
				return SymbolTable.GetTypeByName(ref SymbolTable.s_WindowsRuntimeMarshal, "System.Runtime.InteropServices.WindowsRuntime.WindowsRuntimeMarshal, System.Runtime.InteropServices.WindowsRuntime, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a");
			}
		}

		// Token: 0x1700005C RID: 92
		// (get) Token: 0x06000144 RID: 324 RVA: 0x00007DBB File Offset: 0x00005FBB
		private static Type EventRegistrationTokenTableType
		{
			get
			{
				return SymbolTable.GetTypeByName(ref SymbolTable.s_EventRegistrationTokenTable, "System.Runtime.InteropServices.WindowsRuntime.EventRegistrationTokenTable`1, System.Runtime.InteropServices.WindowsRuntime, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a");
			}
		}

		// Token: 0x06000145 RID: 325 RVA: 0x00007DCC File Offset: 0x00005FCC
		private static Type GetTypeByName(ref Type cachedResult, string name)
		{
			if (cachedResult == SymbolTable.s_Sentinel)
			{
				Interlocked.CompareExchange<Type>(ref cachedResult, Type.GetType(name, false), SymbolTable.s_Sentinel);
			}
			return cachedResult;
		}

		// Token: 0x06000146 RID: 326 RVA: 0x00007DEC File Offset: 0x00005FEC
		private void AddEventToSymbolTable(EventInfo eventInfo, AggregateSymbol aggregate, FieldSymbol addedField)
		{
			EventSymbol eventSymbol = this._symbolTable.LookupSym(this.GetName(eventInfo.Name), aggregate, symbmask_t.MASK_EventSymbol) as EventSymbol;
			if (eventSymbol != null)
			{
				return;
			}
			eventSymbol = this._symFactory.CreateEvent(this.GetName(eventInfo.Name), aggregate);
			eventSymbol.AssociatedEventInfo = eventInfo;
			ACCESS access = ACCESS.ACC_PRIVATE;
			if (eventInfo.AddMethod != null)
			{
				eventSymbol.methAdd = this.AddMethodToSymbolTable(eventInfo.AddMethod, aggregate, MethodKindEnum.EventAccessor);
				eventSymbol.methAdd.SetEvent(eventSymbol);
				eventSymbol.isOverride = eventSymbol.methAdd.IsOverride();
				access = eventSymbol.methAdd.GetAccess();
			}
			if (eventInfo.RemoveMethod != null)
			{
				eventSymbol.methRemove = this.AddMethodToSymbolTable(eventInfo.RemoveMethod, aggregate, MethodKindEnum.EventAccessor);
				eventSymbol.methRemove.SetEvent(eventSymbol);
				eventSymbol.isOverride = eventSymbol.methRemove.IsOverride();
				access = eventSymbol.methRemove.GetAccess();
			}
			eventSymbol.isStatic = false;
			eventSymbol.type = this.GetCTypeFromType(eventInfo.EventHandlerType);
			eventSymbol.SetAccess(access);
			Type eventRegistrationTokenType = SymbolTable.EventRegistrationTokenType;
			if (eventRegistrationTokenType != null && SymbolTable.WindowsRuntimeMarshalType != null && eventSymbol.methAdd.RetType.AssociatedSystemType == eventRegistrationTokenType && eventSymbol.methRemove.Params[0].AssociatedSystemType == eventRegistrationTokenType)
			{
				eventSymbol.IsWindowsRuntimeEvent = true;
			}
			CType ctype = ((addedField != null) ? addedField.GetType() : null);
			if (ctype != null)
			{
				if (ctype == eventSymbol.type)
				{
					addedField.isEvent = true;
					return;
				}
				Type associatedSystemType = ctype.AssociatedSystemType;
				if (associatedSystemType.IsConstructedGenericType && associatedSystemType.GetGenericTypeDefinition() == SymbolTable.EventRegistrationTokenTableType && associatedSystemType.GenericTypeArguments[0] == eventSymbol.type.AssociatedSystemType)
				{
					addedField.isEvent = true;
				}
			}
		}

		// Token: 0x06000147 RID: 327 RVA: 0x00007FB0 File Offset: 0x000061B0
		internal void AddPredefinedPropertyToSymbolTable(AggregateSymbol type, Name property)
		{
			foreach (PropertyInfo propertyInfo in from x in type.getThisType().AssociatedSystemType.GetRuntimeProperties()
				where x.Name == property.Text
				select x)
			{
				this.AddPropertyToSymbolTable(propertyInfo, type);
			}
		}

		// Token: 0x06000148 RID: 328 RVA: 0x00008028 File Offset: 0x00006228
		private void AddPropertyToSymbolTable(PropertyInfo property, AggregateSymbol aggregate)
		{
			bool flag;
			if (property.GetIndexParameters().Length != 0)
			{
				Type declaringType = property.DeclaringType;
				string text;
				if (declaringType == null)
				{
					text = null;
				}
				else
				{
					DefaultMemberAttribute customAttribute = declaringType.GetCustomAttribute<DefaultMemberAttribute>();
					text = ((customAttribute != null) ? customAttribute.MemberName : null);
				}
				flag = text == property.Name;
			}
			else
			{
				flag = false;
			}
			bool flag2 = flag;
			Name name;
			if (flag2)
			{
				name = this.GetName("$Item$");
			}
			else
			{
				name = this.GetName(property.Name);
			}
			PropertySymbol propertySymbol = this._symbolTable.LookupSym(name, aggregate, symbmask_t.MASK_PropertySymbol) as PropertySymbol;
			if (propertySymbol != null)
			{
				PropertySymbol propertySymbol2 = null;
				while (propertySymbol != null)
				{
					if (propertySymbol.AssociatedPropertyInfo.IsEquivalentTo(property))
					{
						return;
					}
					propertySymbol2 = propertySymbol;
					propertySymbol = this._semanticChecker.SymbolLoader.LookupNextSym(propertySymbol, propertySymbol.parent, symbmask_t.MASK_PropertySymbol) as PropertySymbol;
				}
				propertySymbol = propertySymbol2;
				if (flag2)
				{
					propertySymbol = null;
				}
			}
			if (propertySymbol == null)
			{
				if (flag2)
				{
					propertySymbol = this._semanticChecker.SymbolLoader.GetGlobalSymbolFactory().CreateIndexer(name, aggregate, this.GetName(property.Name));
					propertySymbol.Params = this.CreateParameterArray(null, property.GetIndexParameters());
				}
				else
				{
					propertySymbol = this._symFactory.CreateProperty(this.GetName(property.Name), aggregate);
					propertySymbol.Params = BSYMMGR.EmptyTypeArray();
				}
			}
			propertySymbol.AssociatedPropertyInfo = property;
			propertySymbol.isStatic = ((property.GetGetMethod(true) != null) ? property.GetGetMethod(true).IsStatic : property.GetSetMethod(true).IsStatic);
			propertySymbol.isParamArray = this.DoesMethodHaveParameterArray(property.GetIndexParameters());
			propertySymbol.swtSlot = null;
			propertySymbol.RetType = this.GetCTypeFromType(property.PropertyType);
			propertySymbol.isOperator = flag2;
			if (property.GetMethod != null || property.SetMethod != null)
			{
				MethodInfo methodInfo = property.GetMethod ?? property.SetMethod;
				propertySymbol.isOverride = methodInfo.IsVirtual && methodInfo.IsHideBySig && methodInfo.GetRuntimeBaseDefinition() != methodInfo;
				propertySymbol.isHideByName = !methodInfo.IsHideBySig;
			}
			this.SetParameterDataForMethProp(propertySymbol, property.GetIndexParameters());
			MethodInfo getMethod = property.GetMethod;
			MethodInfo setMethod = property.SetMethod;
			ACCESS access = ACCESS.ACC_PRIVATE;
			if (getMethod != null)
			{
				propertySymbol.GetterMethod = this.AddMethodToSymbolTable(getMethod, aggregate, MethodKindEnum.PropAccessor);
				if (flag2 || propertySymbol.GetterMethod.Params.Count == 0)
				{
					propertySymbol.GetterMethod.SetProperty(propertySymbol);
				}
				else
				{
					propertySymbol.Bogus = true;
					propertySymbol.GetterMethod.SetMethKind(MethodKindEnum.Actual);
				}
				if (propertySymbol.GetterMethod.GetAccess() > access)
				{
					access = propertySymbol.GetterMethod.GetAccess();
				}
			}
			if (setMethod != null)
			{
				propertySymbol.SetterMethod = this.AddMethodToSymbolTable(setMethod, aggregate, MethodKindEnum.PropAccessor);
				if (flag2 || propertySymbol.SetterMethod.Params.Count == 1)
				{
					propertySymbol.SetterMethod.SetProperty(propertySymbol);
				}
				else
				{
					propertySymbol.Bogus = true;
					propertySymbol.SetterMethod.SetMethKind(MethodKindEnum.Actual);
				}
				if (propertySymbol.SetterMethod.GetAccess() > access)
				{
					access = propertySymbol.SetterMethod.GetAccess();
				}
			}
			propertySymbol.SetAccess(access);
		}

		// Token: 0x06000149 RID: 329 RVA: 0x0000831C File Offset: 0x0000651C
		internal void AddPredefinedMethodToSymbolTable(AggregateSymbol type, Name methodName)
		{
			Type t = type.getThisType().AssociatedSystemType;
			if (methodName == NameManager.GetPredefinedName(PredefinedName.PN_CTOR))
			{
				foreach (ConstructorInfo constructorInfo in t.GetConstructors())
				{
					this.AddMethodToSymbolTable(constructorInfo, type, MethodKindEnum.Constructor);
				}
				return;
			}
			foreach (MethodInfo methodInfo in from m in t.GetRuntimeMethods()
				where m.Name == methodName.Text && m.DeclaringType == t
				select m)
			{
				this.AddMethodToSymbolTable(methodInfo, type, (methodInfo.Name == "Invoke") ? MethodKindEnum.Invoke : MethodKindEnum.Actual);
			}
		}

		// Token: 0x0600014A RID: 330 RVA: 0x000083F8 File Offset: 0x000065F8
		private MethodSymbol AddMethodToSymbolTable(MethodBase member, AggregateSymbol callingAggregate, MethodKindEnum kind)
		{
			MethodInfo methodInfo = member as MethodInfo;
			if (kind == MethodKindEnum.Actual && (methodInfo == null || (!methodInfo.IsStatic && methodInfo.IsSpecialName)))
			{
				return null;
			}
			MethodSymbol methodSymbol = this.FindMatchingMethod(member, callingAggregate);
			if (methodSymbol != null)
			{
				return methodSymbol;
			}
			ParameterInfo[] parameters = member.GetParameters();
			methodSymbol = this._symFactory.CreateMethod(this.GetName(member.Name), callingAggregate);
			methodSymbol.AssociatedMemberInfo = member;
			methodSymbol.SetMethKind(kind);
			if (kind == MethodKindEnum.ExplicitConv || kind == MethodKindEnum.ImplicitConv)
			{
				callingAggregate.SetHasConversion();
				methodSymbol.SetConvNext(callingAggregate.GetFirstUDConversion());
				callingAggregate.SetFirstUDConversion(methodSymbol);
			}
			ACCESS access;
			if (member.IsPublic)
			{
				access = ACCESS.ACC_PUBLIC;
			}
			else if (member.IsPrivate)
			{
				access = ACCESS.ACC_PRIVATE;
			}
			else if (member.IsFamily)
			{
				access = ACCESS.ACC_PROTECTED;
			}
			else if (member.IsFamilyOrAssembly)
			{
				access = ACCESS.ACC_INTERNALPROTECTED;
			}
			else
			{
				access = ACCESS.ACC_INTERNAL;
			}
			methodSymbol.SetAccess(access);
			methodSymbol.isVirtual = member.IsVirtual;
			methodSymbol.isAbstract = member.IsAbstract;
			methodSymbol.isStatic = member.IsStatic;
			if (methodInfo != null)
			{
				methodSymbol.typeVars = this.GetMethodTypeParameters(methodInfo, methodSymbol);
				methodSymbol.isOverride = methodInfo.IsVirtual && methodInfo.IsHideBySig && methodInfo.GetRuntimeBaseDefinition() != methodInfo;
				methodSymbol.isOperator = SymbolTable.IsOperator(methodInfo);
				methodSymbol.swtSlot = this.GetSlotForOverride(methodInfo);
				methodSymbol.isVarargs = (methodInfo.CallingConvention & CallingConventions.VarArgs) == CallingConventions.VarArgs;
				methodSymbol.RetType = this.GetCTypeFromType(methodInfo.ReturnType);
			}
			else
			{
				methodSymbol.typeVars = BSYMMGR.EmptyTypeArray();
				methodSymbol.isOverride = false;
				methodSymbol.isOperator = false;
				methodSymbol.swtSlot = null;
				methodSymbol.isVarargs = false;
				methodSymbol.RetType = this._typeManager.GetVoid();
			}
			methodSymbol.modOptCount = this.GetCountOfModOpts(parameters);
			methodSymbol.isParamArray = this.DoesMethodHaveParameterArray(parameters);
			methodSymbol.isHideByName = false;
			methodSymbol.errExpImpl = null;
			methodSymbol.Params = this.CreateParameterArray(methodSymbol.AssociatedMemberInfo, parameters);
			this.SetParameterDataForMethProp(methodSymbol, parameters);
			return methodSymbol;
		}

		// Token: 0x0600014B RID: 331 RVA: 0x000085DC File Offset: 0x000067DC
		private void SetParameterDataForMethProp(MethodOrPropertySymbol methProp, ParameterInfo[] parameters)
		{
			if (parameters.Length != 0)
			{
				if (parameters[parameters.Length - 1].GetCustomAttribute(typeof(ParamArrayAttribute), false) != null)
				{
					methProp.isParamArray = true;
				}
				for (int i = 0; i < parameters.Length; i++)
				{
					this.SetParameterAttributes(methProp, parameters, i);
					methProp.ParameterNames.Add(this.GetName(parameters[i].Name));
				}
			}
		}

		// Token: 0x0600014C RID: 332 RVA: 0x00008640 File Offset: 0x00006840
		private void SetParameterAttributes(MethodOrPropertySymbol methProp, ParameterInfo[] parameters, int i)
		{
			ParameterInfo parameterInfo = parameters[i];
			if ((parameterInfo.Attributes & ParameterAttributes.Optional) != ParameterAttributes.None && !parameterInfo.ParameterType.IsByRef)
			{
				methProp.SetOptionalParameter(i);
				this.PopulateSymbolTableWithName("Value", new Type[] { typeof(Missing) }, typeof(Missing));
			}
			if ((parameterInfo.Attributes & ParameterAttributes.HasFieldMarshal) != ParameterAttributes.None)
			{
				MarshalAsAttribute customAttribute = parameterInfo.GetCustomAttribute(false);
				if (customAttribute != null)
				{
					methProp.SetMarshalAsParameter(i, customAttribute.Value);
				}
			}
			DateTimeConstantAttribute customAttribute2 = parameterInfo.GetCustomAttribute(false);
			if (customAttribute2 != null)
			{
				ConstVal constVal = ConstVal.Get(((DateTime)customAttribute2.Value).Ticks);
				CType predefindType = this._semanticChecker.SymbolLoader.GetPredefindType(PredefinedType.PT_DATETIME);
				methProp.SetDefaultParameterValue(i, predefindType, constVal);
				return;
			}
			DecimalConstantAttribute customAttribute3 = parameterInfo.GetCustomAttribute<DecimalConstantAttribute>();
			if (customAttribute3 != null)
			{
				ConstVal constVal2 = ConstVal.Get(customAttribute3.Value);
				CType predefindType2 = this._semanticChecker.SymbolLoader.GetPredefindType(PredefinedType.PT_DECIMAL);
				methProp.SetDefaultParameterValue(i, predefindType2, constVal2);
				return;
			}
			if ((parameterInfo.Attributes & ParameterAttributes.HasDefault) != ParameterAttributes.None && !parameterInfo.ParameterType.IsByRef)
			{
				ConstVal constVal3 = default(ConstVal);
				CType ctype = this._semanticChecker.SymbolLoader.GetPredefindType(PredefinedType.PT_OBJECT);
				if (parameterInfo.DefaultValue != null)
				{
					object defaultValue = parameterInfo.DefaultValue;
					switch (Type.GetTypeCode(defaultValue.GetType()))
					{
					case TypeCode.Boolean:
						constVal3 = ConstVal.Get((bool)defaultValue);
						ctype = this._semanticChecker.SymbolLoader.GetPredefindType(PredefinedType.PT_BOOL);
						break;
					case TypeCode.Char:
						constVal3 = ConstVal.Get((int)((char)defaultValue));
						ctype = this._semanticChecker.SymbolLoader.GetPredefindType(PredefinedType.PT_CHAR);
						break;
					case TypeCode.SByte:
						constVal3 = ConstVal.Get((int)((sbyte)defaultValue));
						ctype = this._semanticChecker.SymbolLoader.GetPredefindType(PredefinedType.PT_SBYTE);
						break;
					case TypeCode.Byte:
						constVal3 = ConstVal.Get((int)((byte)defaultValue));
						ctype = this._semanticChecker.SymbolLoader.GetPredefindType(PredefinedType.PT_BYTE);
						break;
					case TypeCode.Int16:
						constVal3 = ConstVal.Get((int)((short)defaultValue));
						ctype = this._semanticChecker.SymbolLoader.GetPredefindType(PredefinedType.PT_SHORT);
						break;
					case TypeCode.UInt16:
						constVal3 = ConstVal.Get((int)((ushort)defaultValue));
						ctype = this._semanticChecker.SymbolLoader.GetPredefindType(PredefinedType.PT_USHORT);
						break;
					case TypeCode.Int32:
						constVal3 = ConstVal.Get((int)defaultValue);
						ctype = this._semanticChecker.SymbolLoader.GetPredefindType(PredefinedType.PT_INT);
						break;
					case TypeCode.UInt32:
						constVal3 = ConstVal.Get((uint)defaultValue);
						ctype = this._semanticChecker.SymbolLoader.GetPredefindType(PredefinedType.PT_UINT);
						break;
					case TypeCode.Int64:
						constVal3 = ConstVal.Get((long)defaultValue);
						ctype = this._semanticChecker.SymbolLoader.GetPredefindType(PredefinedType.PT_LONG);
						break;
					case TypeCode.UInt64:
						constVal3 = ConstVal.Get((ulong)defaultValue);
						ctype = this._semanticChecker.SymbolLoader.GetPredefindType(PredefinedType.PT_ULONG);
						break;
					case TypeCode.Single:
						constVal3 = ConstVal.Get((float)defaultValue);
						ctype = this._semanticChecker.SymbolLoader.GetPredefindType(PredefinedType.PT_FLOAT);
						break;
					case TypeCode.Double:
						constVal3 = ConstVal.Get((double)defaultValue);
						ctype = this._semanticChecker.SymbolLoader.GetPredefindType(PredefinedType.PT_DOUBLE);
						break;
					case TypeCode.Decimal:
						constVal3 = ConstVal.Get((decimal)defaultValue);
						ctype = this._semanticChecker.SymbolLoader.GetPredefindType(PredefinedType.PT_DECIMAL);
						break;
					case TypeCode.String:
						constVal3 = ConstVal.Get((string)defaultValue);
						ctype = this._semanticChecker.SymbolLoader.GetPredefindType(PredefinedType.PT_STRING);
						break;
					}
				}
				methProp.SetDefaultParameterValue(i, ctype, constVal3);
			}
		}

		// Token: 0x0600014D RID: 333 RVA: 0x00008A00 File Offset: 0x00006C00
		private MethodSymbol FindMatchingMethod(MemberInfo method, AggregateSymbol callingAggregate)
		{
			for (MethodSymbol methodSymbol = this._bsymmgr.LookupAggMember(this.GetName(method.Name), callingAggregate, symbmask_t.MASK_MethodSymbol) as MethodSymbol; methodSymbol != null; methodSymbol = BSYMMGR.LookupNextSym(methodSymbol, callingAggregate, symbmask_t.MASK_MethodSymbol) as MethodSymbol)
			{
				if (methodSymbol.AssociatedMemberInfo.IsEquivalentTo(method))
				{
					return methodSymbol;
				}
			}
			return null;
		}

		// Token: 0x0600014E RID: 334 RVA: 0x00008A5A File Offset: 0x00006C5A
		private uint GetCountOfModOpts(ParameterInfo[] parameters)
		{
			return 0U;
		}

		// Token: 0x0600014F RID: 335 RVA: 0x00008A60 File Offset: 0x00006C60
		private TypeArray CreateParameterArray(MemberInfo associatedInfo, ParameterInfo[] parameters)
		{
			List<CType> list = new List<CType>();
			foreach (ParameterInfo parameterInfo in parameters)
			{
				list.Add(this.GetTypeOfParameter(parameterInfo, associatedInfo));
			}
			MethodInfo methodInfo = associatedInfo as MethodInfo;
			if (methodInfo != null && (methodInfo.CallingConvention & CallingConventions.VarArgs) == CallingConventions.VarArgs)
			{
				list.Add(this._typeManager.GetArgListType());
			}
			return this._bsymmgr.AllocParams(list.Count, list.ToArray());
		}

		// Token: 0x06000150 RID: 336 RVA: 0x00008ADC File Offset: 0x00006CDC
		private CType GetTypeOfParameter(ParameterInfo p, MemberInfo m)
		{
			Type parameterType = p.ParameterType;
			CType ctype;
			if (parameterType.IsGenericParameter && parameterType.DeclaringMethod != null && parameterType.DeclaringMethod == m)
			{
				ctype = this.LoadMethodTypeParameter(this.FindMethodFromMemberInfo(m), parameterType);
			}
			else
			{
				ctype = this.GetCTypeFromType(parameterType);
			}
			ParameterModifierType parameterModifierType;
			if ((parameterModifierType = ctype as ParameterModifierType) != null && p.IsOut && !p.IsIn)
			{
				CType parameterType2 = parameterModifierType.GetParameterType();
				ctype = this._typeManager.GetParameterModifier(parameterType2, true);
			}
			return ctype;
		}

		// Token: 0x06000151 RID: 337 RVA: 0x00008B60 File Offset: 0x00006D60
		private bool DoesMethodHaveParameterArray(ParameterInfo[] parameters)
		{
			if (parameters.Length == 0)
			{
				return false;
			}
			object[] customAttributes = parameters[parameters.Length - 1].GetCustomAttributes(false);
			for (int i = 0; i < customAttributes.Length; i++)
			{
				if (customAttributes[i] is ParamArrayAttribute)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06000152 RID: 338 RVA: 0x00008B9C File Offset: 0x00006D9C
		private SymWithType GetSlotForOverride(MethodInfo method)
		{
			if (!method.IsVirtual || !method.IsHideBySig)
			{
				return null;
			}
			MethodInfo runtimeBaseDefinition = method.GetRuntimeBaseDefinition();
			if (runtimeBaseDefinition == method)
			{
				return null;
			}
			AggregateSymbol aggregate = this.GetCTypeFromType(runtimeBaseDefinition.DeclaringType).getAggregate();
			MethodSymbol methodSymbol = this.FindMethodFromMemberInfo(runtimeBaseDefinition);
			if (methodSymbol == null)
			{
				throw Error.InternalCompilerError();
			}
			return new SymWithType(methodSymbol, aggregate.getThisType());
		}

		// Token: 0x06000153 RID: 339 RVA: 0x00008BFC File Offset: 0x00006DFC
		private MethodSymbol FindMethodFromMemberInfo(MemberInfo baseMemberInfo)
		{
			AggregateSymbol aggregate = this.GetCTypeFromType(baseMemberInfo.DeclaringType).getAggregate();
			MethodSymbol methodSymbol = this._semanticChecker.SymbolLoader.LookupAggMember(this.GetName(baseMemberInfo.Name), aggregate, symbmask_t.MASK_MethodSymbol) as MethodSymbol;
			while (methodSymbol != null && !methodSymbol.AssociatedMemberInfo.IsEquivalentTo(baseMemberInfo))
			{
				methodSymbol = this._semanticChecker.SymbolLoader.LookupNextSym(methodSymbol, aggregate, symbmask_t.MASK_MethodSymbol) as MethodSymbol;
			}
			return methodSymbol;
		}

		// Token: 0x06000154 RID: 340 RVA: 0x00008C76 File Offset: 0x00006E76
		internal bool AggregateContainsMethod(AggregateSymbol agg, string szName, symbmask_t mask)
		{
			return this._semanticChecker.SymbolLoader.LookupAggMember(this.GetName(szName), agg, mask) != null;
		}

		// Token: 0x06000155 RID: 341 RVA: 0x00008C94 File Offset: 0x00006E94
		internal void AddConversionsForType(Type type)
		{
			if (type.IsInterface)
			{
				this.AddConversionsForOneType(type);
			}
			Type type2 = type;
			while (type2.BaseType != null)
			{
				this.AddConversionsForOneType(type2);
				type2 = type2.BaseType;
			}
		}

		// Token: 0x06000156 RID: 342 RVA: 0x00008CD0 File Offset: 0x00006ED0
		private void AddConversionsForOneType(Type type)
		{
			if (type.IsGenericType)
			{
				type = type.GetGenericTypeDefinition();
			}
			if (!this._typesWithConversionsLoaded.Add(type))
			{
				return;
			}
			CType ctype = this.GetCTypeFromType(type);
			if (!(ctype is AggregateType))
			{
				CType baseOrParameterOrElementType;
				while ((baseOrParameterOrElementType = ctype.GetBaseOrParameterOrElementType()) != null)
				{
					ctype = baseOrParameterOrElementType;
				}
			}
			TypeParameterType typeParameterType;
			if ((typeParameterType = ctype as TypeParameterType) != null)
			{
				foreach (CType ctype2 in typeParameterType.GetBounds().Items)
				{
					this.AddConversionsForType(ctype2.AssociatedSystemType);
				}
				return;
			}
			AggregateSymbol aggregate = ((AggregateType)ctype).getAggregate();
			foreach (MethodInfo methodInfo in type.GetRuntimeMethods())
			{
				if (methodInfo.IsPublic && methodInfo.IsStatic && methodInfo.DeclaringType == type && methodInfo.IsSpecialName && !methodInfo.IsGenericMethod)
				{
					string name = methodInfo.Name;
					MethodKindEnum methodKindEnum;
					if (!(name == "op_Implicit"))
					{
						if (!(name == "op_Explicit"))
						{
							continue;
						}
						methodKindEnum = MethodKindEnum.ExplicitConv;
					}
					else
					{
						methodKindEnum = MethodKindEnum.ImplicitConv;
					}
					this.AddMethodToSymbolTable(methodInfo, aggregate, methodKindEnum);
				}
			}
		}

		// Token: 0x06000157 RID: 343 RVA: 0x00008E14 File Offset: 0x00007014
		private static bool IsOperator(MethodInfo method)
		{
			if (method.IsSpecialName && method.IsStatic)
			{
				string name = method.Name;
				uint num = <PrivateImplementationDetails>.ComputeStringHash(name);
				if (num <= 1915672496U)
				{
					if (num <= 1195761148U)
					{
						if (num <= 835846267U)
						{
							if (num != 90588446U)
							{
								if (num != 215197780U)
								{
									if (num != 835846267U)
									{
										return false;
									}
									if (!(name == "op_BitwiseAnd"))
									{
										return false;
									}
								}
								else if (!(name == "op_Implicit"))
								{
									return false;
								}
							}
							else if (!(name == "op_OnesComplement"))
							{
								return false;
							}
						}
						else if (num != 906583475U)
						{
							if (num != 1034931220U)
							{
								if (num != 1195761148U)
								{
									return false;
								}
								if (!(name == "op_GreaterThan"))
								{
									return false;
								}
							}
							else if (!(name == "op_Increment"))
							{
								return false;
							}
						}
						else if (!(name == "op_Addition"))
						{
							return false;
						}
					}
					else if (num <= 1516143579U)
					{
						if (num != 1234170120U)
						{
							if (num != 1258540185U)
							{
								if (num != 1516143579U)
								{
									return false;
								}
								if (!(name == "op_Equality"))
								{
									return false;
								}
							}
							else if (!(name == "op_LessThan"))
							{
								return false;
							}
						}
						else if (!(name == "op_LessThanOrEqual"))
						{
							return false;
						}
					}
					else if (num <= 1587019679U)
					{
						if (num != 1548478473U)
						{
							if (num != 1587019679U)
							{
								return false;
							}
							if (!(name == "op_Explicit"))
							{
								return false;
							}
						}
						else if (!(name == "op_RightShift"))
						{
							return false;
						}
					}
					else if (num != 1850069070U)
					{
						if (num != 1915672496U)
						{
							return false;
						}
						if (!(name == "op_Division"))
						{
							return false;
						}
					}
					else if (!(name == "op_False"))
					{
						return false;
					}
				}
				else if (num <= 2574677899U)
				{
					if (num <= 2429678952U)
					{
						if (num != 2242295702U)
						{
							if (num != 2366795836U)
							{
								if (num != 2429678952U)
								{
									return false;
								}
								if (!(name == "op_Modulus"))
								{
									return false;
								}
							}
							else if (!(name == "op_ExclusiveOr"))
							{
								return false;
							}
						}
						else if (!(name == "op_LeftShift"))
						{
							return false;
						}
					}
					else if (num != 2459852411U)
					{
						if (num != 2536726348U)
						{
							if (num != 2574677899U)
							{
								return false;
							}
							if (!(name == "op_LogicalNot"))
							{
								return false;
							}
						}
						else if (!(name == "op_Decrement"))
						{
							return false;
						}
					}
					else if (!(name == "op_GreaterThanOrEqual"))
					{
						return false;
					}
				}
				else if (num <= 3279419199U)
				{
					if (num != 2958252495U)
					{
						if (num != 3075696130U)
						{
							if (num != 3279419199U)
							{
								return false;
							}
							if (!(name == "op_Subtraction"))
							{
								return false;
							}
						}
						else if (!(name == "op_UnaryPlus"))
						{
							return false;
						}
					}
					else if (!(name == "op_Multiply"))
					{
						return false;
					}
				}
				else if (num <= 3568900899U)
				{
					if (num != 3492550567U)
					{
						if (num != 3568900899U)
						{
							return false;
						}
						if (!(name == "op_True"))
						{
							return false;
						}
					}
					else if (!(name == "op_BitwiseOr"))
					{
						return false;
					}
				}
				else if (num != 3716665893U)
				{
					if (num != 3794317784U)
					{
						return false;
					}
					if (!(name == "op_Inequality"))
					{
						return false;
					}
				}
				else if (!(name == "op_UnaryNegation"))
				{
					return false;
				}
				return true;
			}
			return false;
		}

		// Token: 0x04000116 RID: 278
		private readonly HashSet<Type> _typesWithConversionsLoaded = new HashSet<Type>();

		// Token: 0x04000117 RID: 279
		private readonly HashSet<SymbolTable.NameHashKey> _namesLoadedForEachType = new HashSet<SymbolTable.NameHashKey>();

		// Token: 0x04000118 RID: 280
		private readonly SYMTBL _symbolTable;

		// Token: 0x04000119 RID: 281
		private readonly SymFactory _symFactory;

		// Token: 0x0400011A RID: 282
		private readonly NameManager _nameManager;

		// Token: 0x0400011B RID: 283
		private readonly TypeManager _typeManager;

		// Token: 0x0400011C RID: 284
		private readonly BSYMMGR _bsymmgr;

		// Token: 0x0400011D RID: 285
		private readonly CSemanticChecker _semanticChecker;

		// Token: 0x0400011E RID: 286
		private NamespaceSymbol _rootNamespace;

		// Token: 0x0400011F RID: 287
		private static readonly Type s_Sentinel = typeof(SymbolTable);

		// Token: 0x04000120 RID: 288
		private static Type s_EventRegistrationTokenType = SymbolTable.s_Sentinel;

		// Token: 0x04000121 RID: 289
		private static Type s_WindowsRuntimeMarshal = SymbolTable.s_Sentinel;

		// Token: 0x04000122 RID: 290
		private static Type s_EventRegistrationTokenTable = SymbolTable.s_Sentinel;

		// Token: 0x020000D5 RID: 213
		private sealed class NameHashKey : IEquatable<SymbolTable.NameHashKey>
		{
			// Token: 0x060006B1 RID: 1713 RVA: 0x0001F9CB File Offset: 0x0001DBCB
			public NameHashKey(Type type, string name)
			{
				this.type = type;
				this.name = name;
			}

			// Token: 0x060006B2 RID: 1714 RVA: 0x0001F9E1 File Offset: 0x0001DBE1
			public bool Equals(SymbolTable.NameHashKey other)
			{
				return other != null && this.type.Equals(other.type) && this.name.Equals(other.name);
			}

			// Token: 0x060006B3 RID: 1715 RVA: 0x0001FA0C File Offset: 0x0001DC0C
			public override bool Equals(object obj)
			{
				return this.Equals(obj as SymbolTable.NameHashKey);
			}

			// Token: 0x060006B4 RID: 1716 RVA: 0x0001FA1A File Offset: 0x0001DC1A
			public override int GetHashCode()
			{
				return this.type.GetHashCode() ^ this.name.GetHashCode();
			}

			// Token: 0x0400068C RID: 1676
			internal readonly Type type;

			// Token: 0x0400068D RID: 1677
			internal readonly string name;
		}
	}
}
