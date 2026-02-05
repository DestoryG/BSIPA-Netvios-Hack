using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Dynamic;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.CSharp.RuntimeBinder.Errors;
using Microsoft.CSharp.RuntimeBinder.Semantics;
using Microsoft.CSharp.RuntimeBinder.Syntax;

namespace Microsoft.CSharp.RuntimeBinder
{
	// Token: 0x0200001E RID: 30
	internal sealed class RuntimeBinder
	{
		// Token: 0x060000EB RID: 235 RVA: 0x0000485B File Offset: 0x00002A5B
		public static RuntimeBinder GetInstance()
		{
			return RuntimeBinder.s_lazyInstance.Value;
		}

		// Token: 0x17000059 RID: 89
		// (get) Token: 0x060000EC RID: 236 RVA: 0x00004867 File Offset: 0x00002A67
		private SymbolLoader SymbolLoader
		{
			get
			{
				return this._semanticChecker.SymbolLoader;
			}
		}

		// Token: 0x060000ED RID: 237 RVA: 0x00004874 File Offset: 0x00002A74
		private RuntimeBinder()
		{
			this.Reset();
		}

		// Token: 0x060000EE RID: 238 RVA: 0x00004890 File Offset: 0x00002A90
		private void Reset()
		{
			this._semanticChecker = new CSemanticChecker();
			BSYMMGR bsymmgr = this._semanticChecker.getBSymmgr();
			NameManager nameManager = this._semanticChecker.GetNameManager();
			this._symbolTable = new SymbolTable(bsymmgr.GetSymbolTable(), bsymmgr.GetSymFactory(), nameManager, this._semanticChecker.GetTypeManager(), bsymmgr, this._semanticChecker);
			this._semanticChecker.getPredefTypes().Init(this._symbolTable);
			this._semanticChecker.GetTypeManager().InitTypeFactory(this._symbolTable);
			this.SymbolLoader.getPredefinedMembers().RuntimeBinderSymbolTable = this._symbolTable;
			this.SymbolLoader.SetSymbolTable(this._symbolTable);
			this._exprFactory = new ExprFactory(this._semanticChecker.SymbolLoader.GetGlobalSymbolContext());
			this._bindingContext = new BindingContext(this._semanticChecker, this._exprFactory);
			this._binder = new ExpressionBinder(this._bindingContext);
		}

		// Token: 0x060000EF RID: 239 RVA: 0x00004980 File Offset: 0x00002B80
		public Expression Bind(DynamicMetaObjectBinder payload, Expression[] parameters, DynamicMetaObject[] args, out DynamicMetaObject deferredBinding)
		{
			ICSharpBinder icsharpBinder = payload as ICSharpBinder;
			if (icsharpBinder == null)
			{
				throw Error.InternalCompilerError();
			}
			object bindLock = this._bindLock;
			Expression expression;
			lock (bindLock)
			{
				try
				{
					expression = this.BindCore(icsharpBinder, parameters, args, out deferredBinding);
				}
				catch (ResetBindException)
				{
					this.Reset();
					try
					{
						expression = this.BindCore(icsharpBinder, parameters, args, out deferredBinding);
					}
					catch (ResetBindException)
					{
						this.Reset();
						throw Error.BindingNameCollision();
					}
				}
			}
			return expression;
		}

		// Token: 0x060000F0 RID: 240 RVA: 0x00004A14 File Offset: 0x00002C14
		private Expression BindCore(ICSharpBinder payload, Expression[] parameters, DynamicMetaObject[] args, out DynamicMetaObject deferredBinding)
		{
			if (args.Length < 1)
			{
				throw Error.BindRequireArguments();
			}
			this.InitializeCallingContext(payload);
			ArgumentObject[] array = this.CreateArgumentArray(payload, parameters, args);
			payload.PopulateSymbolTableWithName(this._symbolTable, array[0].Type, array);
			this.AddConversionsForArguments(array);
			Scope scope = this._semanticChecker.GetGlobalSymbolFactory().CreateScope(null);
			LocalVariableSymbol[] array2 = this.PopulateLocalScope(payload, scope, array, parameters);
			DynamicMetaObject dynamicMetaObject;
			if (this.DeferBinding(payload, array, args, array2, out dynamicMetaObject))
			{
				deferredBinding = dynamicMetaObject;
				return null;
			}
			Expr expr = payload.DispatchPayload(this, array, array2);
			deferredBinding = null;
			return this.CreateExpressionTreeFromResult(parameters, scope, expr);
		}

		// Token: 0x060000F1 RID: 241 RVA: 0x00004AA8 File Offset: 0x00002CA8
		private bool DeferBinding(ICSharpBinder payload, ArgumentObject[] arguments, DynamicMetaObject[] args, LocalVariableSymbol[] locals, out DynamicMetaObject deferredBinding)
		{
			CSharpInvokeMemberBinder csharpInvokeMemberBinder = payload as CSharpInvokeMemberBinder;
			if (csharpInvokeMemberBinder != null)
			{
				Type[] typeArguments = csharpInvokeMemberBinder.TypeArguments;
				int num = ((typeArguments != null) ? typeArguments.Length : 0);
				MemberLookup memberLookup = new MemberLookup();
				Expr expr = this.CreateCallingObjectForCall(csharpInvokeMemberBinder, arguments, locals);
				SymWithType symWithType = this._symbolTable.LookupMember(csharpInvokeMemberBinder.Name, expr, this._bindingContext.ContextForMemberLookup, num, memberLookup, (csharpInvokeMemberBinder.Flags & CSharpCallFlags.EventHookup) > CSharpCallFlags.None, true);
				if (symWithType != null && symWithType.Sym.getKind() != SYMKIND.SK_MethodSymbol)
				{
					CSharpGetMemberBinder csharpGetMemberBinder = new CSharpGetMemberBinder(csharpInvokeMemberBinder.Name, false, csharpInvokeMemberBinder.CallingContext, new CSharpArgumentInfo[] { csharpInvokeMemberBinder.GetArgumentInfo(0) });
					CSharpArgumentInfo[] array = csharpInvokeMemberBinder.ArgumentInfoArray();
					array[0] = CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null);
					CSharpInvokeBinder csharpInvokeBinder = new CSharpInvokeBinder(csharpInvokeMemberBinder.Flags, csharpInvokeMemberBinder.CallingContext, array);
					DynamicMetaObject[] array2 = new DynamicMetaObject[args.Length - 1];
					Array.Copy(args, 1, array2, 0, args.Length - 1);
					deferredBinding = csharpInvokeBinder.Defer(csharpGetMemberBinder.Defer(args[0], Array.Empty<DynamicMetaObject>()), array2);
					return true;
				}
			}
			deferredBinding = null;
			return false;
		}

		// Token: 0x060000F2 RID: 242 RVA: 0x00004BB8 File Offset: 0x00002DB8
		private void InitializeCallingContext(ICSharpBinder payload)
		{
			Type callingContext = payload.CallingContext;
			BindingContext bindingContext = this._bindingContext;
			if (callingContext != null)
			{
				AggregateSymbol owningAggregate = ((AggregateType)this._symbolTable.GetCTypeFromType(callingContext)).GetOwningAggregate();
				bindingContext.ContextForMemberLookup = this._semanticChecker.GetGlobalSymbolFactory().CreateAggregateDecl(owningAggregate, null);
			}
			else
			{
				bindingContext.ContextForMemberLookup = null;
			}
			bindingContext.CheckedConstant = (bindingContext.CheckedNormal = payload.IsChecked);
		}

		// Token: 0x060000F3 RID: 243 RVA: 0x00004C2C File Offset: 0x00002E2C
		private Expression CreateExpressionTreeFromResult(Expression[] parameters, Scope pScope, Expr pResult)
		{
			Expr expr = ExpressionTreeRewriter.Rewrite(this.GenerateBoundLambda(pScope, pResult), this._exprFactory, this.SymbolLoader);
			return ExpressionTreeCallRewriter.Rewrite(this.SymbolLoader.GetTypeManager(), expr, parameters);
		}

		// Token: 0x060000F4 RID: 244 RVA: 0x00004C68 File Offset: 0x00002E68
		private Type GetArgumentType(ICSharpBinder p, CSharpArgumentInfo argInfo, Expression param, DynamicMetaObject arg, int index)
		{
			Type type = (argInfo.UseCompileTimeType ? param.Type : arg.LimitType);
			if (argInfo.IsByRefOrOut)
			{
				if (index != 0 || !p.IsBinderThatCanHaveRefReceiver)
				{
					type = type.MakeByRefType();
				}
			}
			else if (!argInfo.UseCompileTimeType)
			{
				CType ctypeFromType = this._symbolTable.GetCTypeFromType(type);
				CType ctype;
				this._semanticChecker.GetTypeManager().GetBestAccessibleType(this._semanticChecker, this._bindingContext, ctypeFromType, out ctype);
				type = ctype.AssociatedSystemType;
			}
			return type;
		}

		// Token: 0x060000F5 RID: 245 RVA: 0x00004CE8 File Offset: 0x00002EE8
		private ArgumentObject[] CreateArgumentArray(ICSharpBinder payload, Expression[] parameters, DynamicMetaObject[] args)
		{
			ArgumentObject[] array = new ArgumentObject[parameters.Length];
			for (int i = 0; i < parameters.Length; i++)
			{
				CSharpArgumentInfo argumentInfo = payload.GetArgumentInfo(i);
				array[i] = new ArgumentObject(args[i].Value, argumentInfo, this.GetArgumentType(payload, argumentInfo, parameters[i], args[i], i));
			}
			return array;
		}

		// Token: 0x060000F6 RID: 246 RVA: 0x00004D3C File Offset: 0x00002F3C
		internal static void PopulateSymbolTableWithPayloadInformation(SymbolTable symbolTable, ICSharpInvokeOrInvokeMemberBinder callOrInvoke, Type callingType, ArgumentObject[] arguments)
		{
			Type type;
			if (callOrInvoke.StaticCall)
			{
				type = arguments[0].Value as Type;
				if (type == null)
				{
					throw Error.BindStaticRequiresType(arguments[0].Info.Name);
				}
			}
			else
			{
				type = callingType;
			}
			symbolTable.PopulateSymbolTableWithName(callOrInvoke.Name, callOrInvoke.TypeArguments, type);
			if (callOrInvoke.Name.StartsWith("set_", StringComparison.Ordinal) || callOrInvoke.Name.StartsWith("get_", StringComparison.Ordinal))
			{
				symbolTable.PopulateSymbolTableWithName(callOrInvoke.Name.Substring(4), callOrInvoke.TypeArguments, type);
			}
		}

		// Token: 0x060000F7 RID: 247 RVA: 0x00004DD8 File Offset: 0x00002FD8
		private void AddConversionsForArguments(ArgumentObject[] arguments)
		{
			foreach (ArgumentObject argumentObject in arguments)
			{
				this._symbolTable.AddConversionsForType(argumentObject.Type);
			}
		}

		// Token: 0x060000F8 RID: 248 RVA: 0x00004E0E File Offset: 0x0000300E
		internal Expr DispatchPayload(ICSharpInvokeOrInvokeMemberBinder payload, ArgumentObject[] arguments, LocalVariableSymbol[] locals)
		{
			return this.BindCall(payload, this.CreateCallingObjectForCall(payload, arguments, locals), arguments, locals);
		}

		// Token: 0x060000F9 RID: 249 RVA: 0x00004E24 File Offset: 0x00003024
		private LocalVariableSymbol[] PopulateLocalScope(ICSharpBinder payload, Scope pScope, ArgumentObject[] arguments, Expression[] parameterExpressions)
		{
			LocalVariableSymbol[] array = new LocalVariableSymbol[parameterExpressions.Length];
			for (int i = 0; i < parameterExpressions.Length; i++)
			{
				Expression expression = parameterExpressions[i];
				CType ctype = this._symbolTable.GetCTypeFromType(expression.Type);
				if (i != 0 || !payload.IsBinderThatCanHaveRefReceiver)
				{
					ParameterExpression parameterExpression = expression as ParameterExpression;
					if (parameterExpression != null && parameterExpression.IsByRef)
					{
						CSharpArgumentInfo info = arguments[i].Info;
						if (info.IsByRefOrOut)
						{
							ctype = this._semanticChecker.GetTypeManager().GetParameterModifier(ctype, info.IsOut);
						}
					}
				}
				LocalVariableSymbol localVariableSymbol = this._semanticChecker.GetGlobalSymbolFactory().CreateLocalVar(this._semanticChecker.GetNameManager().Add("p" + i), pScope, ctype);
				localVariableSymbol.fUsedInAnonMeth = true;
				array[i] = localVariableSymbol;
			}
			return array;
		}

		// Token: 0x060000FA RID: 250 RVA: 0x00004EFC File Offset: 0x000030FC
		private ExprBoundLambda GenerateBoundLambda(Scope pScope, Expr call)
		{
			AggregateType aggregateType = this._symbolTable.GetCTypeFromType(typeof(Func<>)) as AggregateType;
			this._semanticChecker.GetGlobalSymbolFactory().CreateLocalVar(this._semanticChecker.GetNameManager().Add("this"), pScope, this._symbolTable.GetCTypeFromType(typeof(object))).isThis = true;
			ExprBoundLambda exprBoundLambda = this._exprFactory.CreateAnonymousMethod(aggregateType, pScope);
			ExprReturn exprReturn = this._exprFactory.CreateReturn(call);
			ExprBlock exprBlock = this._exprFactory.CreateBlock(exprReturn);
			exprBoundLambda.OptionalBody = exprBlock;
			return exprBoundLambda;
		}

		// Token: 0x060000FB RID: 251 RVA: 0x00004F94 File Offset: 0x00003194
		private Expr CreateLocal(Type type, bool bIsOut, LocalVariableSymbol local)
		{
			CType ctype = this._symbolTable.GetCTypeFromType(type);
			if (bIsOut)
			{
				ctype = this._semanticChecker.GetTypeManager().GetParameterModifier(((ParameterModifierType)ctype).GetParameterType(), true);
			}
			ExprLocal exprLocal = this._exprFactory.CreateLocal(local);
			Expr expr = this._binder.tryConvert(exprLocal, ctype);
			if (expr == null)
			{
				expr = this._binder.mustCast(exprLocal, ctype);
			}
			expr.Flags |= EXPRFLAG.EXF_LVALUE;
			return expr;
		}

		// Token: 0x060000FC RID: 252 RVA: 0x00005010 File Offset: 0x00003210
		internal Expr CreateArgumentListEXPR(ArgumentObject[] arguments, LocalVariableSymbol[] locals, int startIndex, int endIndex)
		{
			Expr expr = null;
			Expr expr2 = null;
			if (arguments != null)
			{
				for (int i = startIndex; i < endIndex; i++)
				{
					ArgumentObject argumentObject = arguments[i];
					Expr expr3 = this.CreateArgumentEXPR(argumentObject, locals[i]);
					if (expr == null)
					{
						expr = expr3;
						expr2 = expr;
					}
					else
					{
						this._exprFactory.AppendItemToList(expr3, ref expr, ref expr2);
					}
				}
			}
			return expr;
		}

		// Token: 0x060000FD RID: 253 RVA: 0x00005064 File Offset: 0x00003264
		private Expr CreateArgumentEXPR(ArgumentObject argument, LocalVariableSymbol local)
		{
			Expr expr;
			if (argument.Info.LiteralConstant)
			{
				if (argument.Value == null)
				{
					if (argument.Info.UseCompileTimeType)
					{
						expr = this._exprFactory.CreateConstant(this._symbolTable.GetCTypeFromType(argument.Type), default(ConstVal));
					}
					else
					{
						expr = this._exprFactory.CreateNull();
					}
				}
				else
				{
					expr = this._exprFactory.CreateConstant(this._symbolTable.GetCTypeFromType(argument.Type), ConstVal.Get(argument.Value));
				}
			}
			else if (!argument.Info.UseCompileTimeType && argument.Value == null)
			{
				expr = this._exprFactory.CreateNull();
			}
			else
			{
				expr = this.CreateLocal(argument.Type, argument.Info.IsOut, local);
			}
			if (argument.Info.NamedArgument)
			{
				expr = this._exprFactory.CreateNamedArgumentSpecification(this._semanticChecker.GetNameManager().Add(argument.Info.Name), expr);
			}
			if (!argument.Info.UseCompileTimeType && argument.Value != null)
			{
				expr.RuntimeObject = argument.Value;
				expr.RuntimeObjectActualType = this._symbolTable.GetCTypeFromType(argument.Value.GetType());
			}
			return expr;
		}

		// Token: 0x060000FE RID: 254 RVA: 0x000051A4 File Offset: 0x000033A4
		private ExprMemberGroup CreateMemberGroupEXPR(string Name, Type[] typeArguments, Expr callingObject, SYMKIND kind)
		{
			Name name = this._semanticChecker.GetNameManager().Add(Name);
			CType type = callingObject.Type;
			AggregateType aggregateType;
			NullableType nullableType;
			if (type is ArrayType)
			{
				aggregateType = this._semanticChecker.SymbolLoader.GetPredefindType(PredefinedType.PT_ARRAY);
			}
			else if ((nullableType = type as NullableType) != null)
			{
				aggregateType = nullableType.GetAts();
			}
			else
			{
				aggregateType = (AggregateType)type;
			}
			HashSet<CType> hashSet = new HashSet<CType>();
			List<CType> list = new List<CType>();
			symbmask_t symbmask_t = symbmask_t.MASK_MethodSymbol;
			switch (kind)
			{
			case SYMKIND.SK_MethodSymbol:
				symbmask_t = symbmask_t.MASK_MethodSymbol;
				break;
			case SYMKIND.SK_PropertySymbol:
			case SYMKIND.SK_IndexerSymbol:
				symbmask_t = symbmask_t.MASK_PropertySymbol;
				break;
			}
			bool flag = name == NameManager.GetPredefinedName(PredefinedName.PN_CTOR);
			foreach (AggregateType aggregateType2 in aggregateType.TypeHierarchy)
			{
				if (this._symbolTable.AggregateContainsMethod(aggregateType2.GetOwningAggregate(), Name, symbmask_t) && hashSet.Add(aggregateType2))
				{
					list.Add(aggregateType2);
				}
				if (flag)
				{
					break;
				}
			}
			if (aggregateType.IsWindowsRuntimeType())
			{
				aggregateType.GetWinRTCollectionIfacesAll(this.SymbolLoader);
				foreach (AggregateType aggregateType3 in aggregateType.GetWinRTCollectionIfacesAll(this.SymbolLoader).Items)
				{
					if (this._symbolTable.AggregateContainsMethod(aggregateType3.GetOwningAggregate(), Name, symbmask_t) && hashSet.Add(aggregateType3))
					{
						list.Add(aggregateType3);
					}
				}
			}
			EXPRFLAG exprflag = EXPRFLAG.EXF_USERCALLABLE;
			if (Name == "Invoke" && callingObject.Type.isDelegateType())
			{
				exprflag |= EXPRFLAG.EXF_GOTONOTBLOCKED;
			}
			if (Name == ".ctor")
			{
				exprflag |= EXPRFLAG.EXF_CTOR;
			}
			if (Name == "$Item$")
			{
				exprflag |= EXPRFLAG.EXF_INDEXER;
			}
			TypeArray typeArray = BSYMMGR.EmptyTypeArray();
			if (typeArguments != null && typeArguments.Length != 0)
			{
				typeArray = this._semanticChecker.getBSymmgr().AllocParams(this._symbolTable.GetCTypeArrayFromTypes(typeArguments));
			}
			ExprMemberGroup exprMemberGroup = this._exprFactory.CreateMemGroup(exprflag, name, typeArray, kind, aggregateType, null, null, new CMemberLookupResults(this._semanticChecker.getBSymmgr().AllocParams(list.Count, list.ToArray()), name));
			if (callingObject is ExprClass)
			{
				exprMemberGroup.OptionalLHS = callingObject;
			}
			else
			{
				exprMemberGroup.OptionalObject = callingObject;
			}
			return exprMemberGroup;
		}

		// Token: 0x060000FF RID: 255 RVA: 0x00005408 File Offset: 0x00003608
		private Expr CreateProperty(SymWithType swt, Expr callingObject, BindingFlag flags)
		{
			PropertySymbol propertySymbol = swt.Prop();
			AggregateType type = swt.GetType();
			PropWithType propWithType = new PropWithType(propertySymbol, type);
			ExprMemberGroup exprMemberGroup = this.CreateMemberGroupEXPR(propertySymbol.name.Text, null, callingObject, SYMKIND.SK_PropertySymbol);
			return this._binder.BindToProperty((callingObject is ExprClass) ? null : callingObject, propWithType, flags, null, null, exprMemberGroup);
		}

		// Token: 0x06000100 RID: 256 RVA: 0x0000545C File Offset: 0x0000365C
		private Expr CreateIndexer(SymWithType swt, Expr callingObject, Expr arguments, BindingFlag bindFlags)
		{
			IndexerSymbol indexerSymbol = swt.Sym as IndexerSymbol;
			swt.GetType();
			ExprMemberGroup exprMemberGroup = this.CreateMemberGroupEXPR(indexerSymbol.name.Text, null, callingObject, SYMKIND.SK_PropertySymbol);
			Expr expr = this._binder.BindMethodGroupToArguments(bindFlags, exprMemberGroup, arguments);
			return this.ReorderArgumentsForNamedAndOptional(callingObject, expr);
		}

		// Token: 0x06000101 RID: 257 RVA: 0x000054A9 File Offset: 0x000036A9
		private Expr CreateArray(Expr callingObject, Expr optionalIndexerArguments)
		{
			return this._binder.BindArrayIndexCore(callingObject, optionalIndexerArguments);
		}

		// Token: 0x06000102 RID: 258 RVA: 0x000054B8 File Offset: 0x000036B8
		private Expr CreateField(SymWithType swt, Expr callingObject)
		{
			FieldSymbol fieldSymbol = swt.Field();
			fieldSymbol.GetType();
			AggregateType type = swt.GetType();
			FieldWithType fieldWithType = new FieldWithType(fieldSymbol, type);
			return this._binder.BindToField((callingObject is ExprClass) ? null : callingObject, fieldWithType, (BindingFlag)0);
		}

		// Token: 0x06000103 RID: 259 RVA: 0x000054FC File Offset: 0x000036FC
		private Expr CreateCallingObjectForCall(ICSharpInvokeOrInvokeMemberBinder payload, ArgumentObject[] arguments, LocalVariableSymbol[] locals)
		{
			Expr expr;
			if (payload.StaticCall)
			{
				Type type = arguments[0].Value as Type;
				expr = this._exprFactory.CreateClass(this._symbolTable.GetCTypeFromType(type));
			}
			else
			{
				if (!arguments[0].Info.UseCompileTimeType && arguments[0].Value == null)
				{
					throw Error.NullReferenceOnMemberException();
				}
				expr = this._binder.mustConvert(this.CreateArgumentEXPR(arguments[0], locals[0]), this._symbolTable.GetCTypeFromType(arguments[0].Type));
				if (arguments[0].Type.IsValueType && expr is ExprCast)
				{
					expr.Flags |= EXPRFLAG.EXF_USERCALLABLE;
				}
			}
			return expr;
		}

		// Token: 0x06000104 RID: 260 RVA: 0x000055C8 File Offset: 0x000037C8
		private Expr BindCall(ICSharpInvokeOrInvokeMemberBinder payload, Expr callingObject, ArgumentObject[] arguments, LocalVariableSymbol[] locals)
		{
			if (payload is InvokeBinder && !callingObject.Type.isDelegateType())
			{
				throw Error.BindInvokeFailedNonDelegate();
			}
			Type[] typeArguments = payload.TypeArguments;
			int num = ((typeArguments != null) ? typeArguments.Length : 0);
			MemberLookup memberLookup = new MemberLookup();
			SymWithType symWithType = this._symbolTable.LookupMember(payload.Name, callingObject, this._bindingContext.ContextForMemberLookup, num, memberLookup, (payload.Flags & CSharpCallFlags.EventHookup) > CSharpCallFlags.None, true);
			if (symWithType == null)
			{
				throw memberLookup.ReportErrors();
			}
			if (symWithType.Sym.getKind() != SYMKIND.SK_MethodSymbol)
			{
				throw Error.InternalCompilerError();
			}
			ExprMemberGroup exprMemberGroup = this.CreateMemberGroupEXPR(payload.Name, payload.TypeArguments, callingObject, symWithType.Sym.getKind());
			if ((payload.Flags & CSharpCallFlags.SimpleNameCall) != CSharpCallFlags.None)
			{
				callingObject.Flags |= EXPRFLAG.EXF_UNREALIZEDGOTO;
			}
			if ((payload.Flags & CSharpCallFlags.EventHookup) != CSharpCallFlags.None)
			{
				memberLookup = new MemberLookup();
				SymWithType symWithType2 = this._symbolTable.LookupMember(payload.Name.Split(new char[] { '_' })[1], callingObject, this._bindingContext.ContextForMemberLookup, num, memberLookup, (payload.Flags & CSharpCallFlags.EventHookup) > CSharpCallFlags.None, true);
				if (symWithType2 == null)
				{
					throw memberLookup.ReportErrors();
				}
				CType ctype = null;
				if (symWithType2.Sym.getKind() == SYMKIND.SK_FieldSymbol)
				{
					ctype = symWithType2.Field().GetType();
				}
				else if (symWithType2.Sym.getKind() == SYMKIND.SK_EventSymbol)
				{
					ctype = symWithType2.Event().type;
				}
				Type associatedSystemType = this.SymbolLoader.GetTypeManager().SubstType(ctype, symWithType2.Ats).AssociatedSystemType;
				if (associatedSystemType != null)
				{
					this.BindImplicitConversion(new ArgumentObject[] { arguments[1] }, associatedSystemType, locals, false);
				}
				exprMemberGroup.Flags &= ~EXPRFLAG.EXF_USERCALLABLE;
				if (symWithType2.Sym.getKind() == SYMKIND.SK_EventSymbol && symWithType2.Event().IsWindowsRuntimeEvent)
				{
					return this.BindWinRTEventAccessor(new EventWithType(symWithType2.Event(), symWithType2.Ats), callingObject, arguments, locals, payload.Name.StartsWith("add_", StringComparison.Ordinal));
				}
			}
			if ((payload.Name.StartsWith("set_", StringComparison.Ordinal) && ((MethodSymbol)symWithType.Sym).Params.Count > 1) || (payload.Name.StartsWith("get_", StringComparison.Ordinal) && ((MethodSymbol)symWithType.Sym).Params.Count > 0))
			{
				exprMemberGroup.Flags &= ~EXPRFLAG.EXF_USERCALLABLE;
			}
			Expr expr = this._binder.BindMethodGroupToArguments(BindingFlag.BIND_RVALUEREQUIRED | BindingFlag.BIND_STMTEXPRONLY, exprMemberGroup, this.CreateArgumentListEXPR(arguments, locals, 1, arguments.Length));
			if (expr == null || !expr.IsOK)
			{
				throw Error.BindCallFailedOverloadResolution();
			}
			RuntimeBinder.CheckForConditionalMethodError(expr);
			return this.ReorderArgumentsForNamedAndOptional(callingObject, expr);
		}

		// Token: 0x06000105 RID: 261 RVA: 0x00005888 File Offset: 0x00003A88
		private Expr BindWinRTEventAccessor(EventWithType ewt, Expr callingObject, ArgumentObject[] arguments, LocalVariableSymbol[] locals, bool isAddAccessor)
		{
			Type associatedSystemType = ewt.Event().type.AssociatedSystemType;
			MethPropWithInst methPropWithInst = new MethPropWithInst(ewt.Event().methRemove, ewt.Ats);
			ExprMemberGroup exprMemberGroup = this._exprFactory.CreateMemGroup(callingObject, methPropWithInst);
			exprMemberGroup.Flags &= ~EXPRFLAG.EXF_USERCALLABLE;
			Type eventRegistrationTokenType = SymbolTable.EventRegistrationTokenType;
			Type actionType = Expression.GetActionType(new Type[] { eventRegistrationTokenType });
			Expr expr = this._binder.mustConvert(exprMemberGroup, this._symbolTable.GetCTypeFromType(actionType));
			Expr expr2 = this.CreateArgumentEXPR(arguments[1], locals[1]);
			ExprList exprList;
			string text;
			if (isAddAccessor)
			{
				MethPropWithInst methPropWithInst2 = new MethPropWithInst(ewt.Event().methAdd, ewt.Ats);
				ExprMemberGroup exprMemberGroup2 = this._exprFactory.CreateMemGroup(callingObject, methPropWithInst2);
				exprMemberGroup2.Flags &= ~EXPRFLAG.EXF_USERCALLABLE;
				Type funcType = Expression.GetFuncType(new Type[] { associatedSystemType, eventRegistrationTokenType });
				Expr expr3 = this._binder.mustConvert(exprMemberGroup2, this._symbolTable.GetCTypeFromType(funcType));
				exprList = this._exprFactory.CreateList(expr3, expr, expr2);
				text = NameManager.GetPredefinedName(PredefinedName.PN_ADDEVENTHANDLER).Text;
			}
			else
			{
				exprList = this._exprFactory.CreateList(expr, expr2);
				text = NameManager.GetPredefinedName(PredefinedName.PN_REMOVEEVENTHANDLER).Text;
			}
			Type windowsRuntimeMarshalType = SymbolTable.WindowsRuntimeMarshalType;
			this._symbolTable.PopulateSymbolTableWithName(text, new List<Type> { associatedSystemType }, windowsRuntimeMarshalType);
			ExprClass exprClass = this._exprFactory.CreateClass(this._symbolTable.GetCTypeFromType(windowsRuntimeMarshalType));
			ExprMemberGroup exprMemberGroup3 = this.CreateMemberGroupEXPR(text, new Type[] { associatedSystemType }, exprClass, SYMKIND.SK_MethodSymbol);
			return this._binder.BindMethodGroupToArguments(BindingFlag.BIND_RVALUEREQUIRED | BindingFlag.BIND_STMTEXPRONLY, exprMemberGroup3, exprList);
		}

		// Token: 0x06000106 RID: 262 RVA: 0x00005A40 File Offset: 0x00003C40
		private static void CheckForConditionalMethodError(Expr pExpr)
		{
			ExprCall exprCall;
			if ((exprCall = pExpr as ExprCall) != null)
			{
				MethodSymbol methodSymbol = exprCall.MethWithInst.Meth();
				if (methodSymbol.isOverride)
				{
					methodSymbol = methodSymbol.swtSlot.Meth();
				}
				if (methodSymbol.AssociatedMemberInfo.GetCustomAttributes(typeof(ConditionalAttribute), false).ToArray<object>().Length != 0)
				{
					throw Error.BindCallToConditionalMethod(methodSymbol.name);
				}
			}
		}

		// Token: 0x06000107 RID: 263 RVA: 0x00005AA4 File Offset: 0x00003CA4
		private Expr ReorderArgumentsForNamedAndOptional(Expr callingObject, Expr pResult)
		{
			IExprWithArgs exprWithArgs = pResult as IExprWithArgs;
			Expr optionalArguments = exprWithArgs.OptionalArguments;
			ExprCall exprCall;
			AggregateType aggregateType;
			MethodOrPropertySymbol methodOrPropertySymbol;
			ExprMemberGroup exprMemberGroup;
			TypeArray typeArray;
			if ((exprCall = pResult as ExprCall) != null)
			{
				aggregateType = exprCall.MethWithInst.Ats;
				methodOrPropertySymbol = exprCall.MethWithInst.Meth();
				exprMemberGroup = exprCall.MemberGroup;
				typeArray = exprCall.MethWithInst.TypeArgs;
			}
			else
			{
				ExprProperty exprProperty = pResult as ExprProperty;
				aggregateType = exprProperty.PropWithTypeSlot.Ats;
				methodOrPropertySymbol = exprProperty.PropWithTypeSlot.Prop();
				exprMemberGroup = exprProperty.MemberGroup;
				typeArray = null;
			}
			ArgInfos argInfos = new ArgInfos();
			bool flag;
			argInfos.carg = ExpressionBinder.CountArguments(optionalArguments, out flag);
			this._binder.FillInArgInfoFromArgList(argInfos, optionalArguments);
			TypeArray typeArray2 = this.SymbolLoader.GetTypeManager().SubstTypeArray(methodOrPropertySymbol.Params, aggregateType, typeArray);
			methodOrPropertySymbol = ExpressionBinder.GroupToArgsBinder.FindMostDerivedMethod(this.SymbolLoader, methodOrPropertySymbol, callingObject.Type);
			ExpressionBinder.GroupToArgsBinder.ReOrderArgsForNamedArguments(methodOrPropertySymbol, typeArray2, aggregateType, exprMemberGroup, argInfos, this._semanticChecker.GetTypeManager(), this._exprFactory, this.SymbolLoader);
			Expr expr = null;
			for (int i = argInfos.carg - 1; i >= 0; i--)
			{
				Expr expr2 = argInfos.prgexpr[i];
				expr2 = this.StripNamedArgument(expr2);
				expr2 = this._binder.tryConvert(expr2, typeArray2[i]);
				if (expr == null)
				{
					expr = expr2;
				}
				else
				{
					expr = this._exprFactory.CreateList(expr2, expr);
				}
			}
			exprWithArgs.OptionalArguments = expr;
			return pResult;
		}

		// Token: 0x06000108 RID: 264 RVA: 0x00005C10 File Offset: 0x00003E10
		private Expr StripNamedArgument(Expr pArg)
		{
			ExprNamedArgumentSpecification exprNamedArgumentSpecification;
			ExprArrayInit exprArrayInit;
			if ((exprNamedArgumentSpecification = pArg as ExprNamedArgumentSpecification) != null)
			{
				pArg = exprNamedArgumentSpecification.Value;
			}
			else if ((exprArrayInit = pArg as ExprArrayInit) != null)
			{
				exprArrayInit.OptionalArguments = this.StripNamedArguments(exprArrayInit.OptionalArguments);
			}
			return pArg;
		}

		// Token: 0x06000109 RID: 265 RVA: 0x00005C50 File Offset: 0x00003E50
		private Expr StripNamedArguments(Expr pArg)
		{
			ExprList exprList;
			if ((exprList = pArg as ExprList) != null)
			{
				for (;;)
				{
					exprList.OptionalElement = this.StripNamedArgument(exprList.OptionalElement);
					ExprList exprList2;
					if ((exprList2 = exprList.OptionalNextListNode as ExprList) == null)
					{
						break;
					}
					exprList = exprList2;
				}
				exprList.OptionalNextListNode = this.StripNamedArgument(exprList.OptionalNextListNode);
			}
			return this.StripNamedArgument(pArg);
		}

		// Token: 0x0600010A RID: 266 RVA: 0x00005CA8 File Offset: 0x00003EA8
		internal Expr BindUnaryOperation(CSharpUnaryOperationBinder payload, ArgumentObject[] arguments, LocalVariableSymbol[] locals)
		{
			if (arguments.Length != 1)
			{
				throw Error.BindUnaryOperatorRequireOneArgument();
			}
			OperatorKind operatorKind = RuntimeBinder.GetOperatorKind(payload.Operation);
			Expr expr = this.CreateArgumentEXPR(arguments[0], locals[0]);
			expr.ErrorString = Operators.GetDisplayName(RuntimeBinder.GetOperatorKind(payload.Operation));
			if (operatorKind == OperatorKind.OP_TRUE || operatorKind == OperatorKind.OP_FALSE)
			{
				Expr expr2 = this._binder.tryConvert(expr, this.SymbolLoader.GetPredefindType(PredefinedType.PT_BOOL));
				if (expr2 != null && operatorKind == OperatorKind.OP_FALSE)
				{
					expr2 = this._binder.BindStandardUnaryOperator(OperatorKind.OP_LOGNOT, expr2);
				}
				if (expr2 == null)
				{
					expr2 = this._binder.bindUDUnop((operatorKind == OperatorKind.OP_TRUE) ? ExpressionKind.True : ExpressionKind.False, expr);
				}
				if (expr2 == null)
				{
					expr2 = this._binder.mustConvert(expr, this.SymbolLoader.GetPredefindType(PredefinedType.PT_BOOL));
				}
				return expr2;
			}
			return this._binder.BindStandardUnaryOperator(operatorKind, expr);
		}

		// Token: 0x0600010B RID: 267 RVA: 0x00005D74 File Offset: 0x00003F74
		internal Expr BindBinaryOperation(CSharpBinaryOperationBinder payload, ArgumentObject[] arguments, LocalVariableSymbol[] locals)
		{
			if (arguments.Length != 2)
			{
				throw Error.BindBinaryOperatorRequireTwoArguments();
			}
			ExpressionKind expressionKind = Operators.GetExpressionKind(RuntimeBinder.GetOperatorKind(payload.Operation, payload.IsLogicalOperation));
			Expr expr = this.CreateArgumentEXPR(arguments[0], locals[0]);
			Expr expr2 = this.CreateArgumentEXPR(arguments[1], locals[1]);
			expr.ErrorString = Operators.GetDisplayName(RuntimeBinder.GetOperatorKind(payload.Operation, payload.IsLogicalOperation));
			expr2.ErrorString = Operators.GetDisplayName(RuntimeBinder.GetOperatorKind(payload.Operation, payload.IsLogicalOperation));
			if (expressionKind > ExpressionKind.MultiOffset)
			{
				expressionKind -= 75;
			}
			return this._binder.BindStandardBinop(expressionKind, expr, expr2);
		}

		// Token: 0x0600010C RID: 268 RVA: 0x00005E16 File Offset: 0x00004016
		private static OperatorKind GetOperatorKind(ExpressionType p)
		{
			return RuntimeBinder.GetOperatorKind(p, false);
		}

		// Token: 0x0600010D RID: 269 RVA: 0x00005E20 File Offset: 0x00004020
		private static OperatorKind GetOperatorKind(ExpressionType p, bool bIsLogical)
		{
			if (p <= ExpressionType.Subtract)
			{
				if (p == ExpressionType.Add)
				{
					return OperatorKind.OP_ADD;
				}
				if (p != ExpressionType.And)
				{
					switch (p)
					{
					case ExpressionType.Divide:
						return OperatorKind.OP_DIV;
					case ExpressionType.Equal:
						return OperatorKind.OP_EQ;
					case ExpressionType.ExclusiveOr:
						return OperatorKind.OP_BITXOR;
					case ExpressionType.GreaterThan:
						return OperatorKind.OP_GT;
					case ExpressionType.GreaterThanOrEqual:
						return OperatorKind.OP_GE;
					case ExpressionType.LeftShift:
						return OperatorKind.OP_LSHIFT;
					case ExpressionType.LessThan:
						return OperatorKind.OP_LT;
					case ExpressionType.LessThanOrEqual:
						return OperatorKind.OP_LE;
					case ExpressionType.Modulo:
						return OperatorKind.OP_MOD;
					case ExpressionType.Multiply:
						return OperatorKind.OP_MUL;
					case ExpressionType.Negate:
						return OperatorKind.OP_NEG;
					case ExpressionType.UnaryPlus:
						return OperatorKind.OP_UPLUS;
					case ExpressionType.Not:
						return OperatorKind.OP_LOGNOT;
					case ExpressionType.NotEqual:
						return OperatorKind.OP_NEQ;
					case ExpressionType.Or:
						if (!bIsLogical)
						{
							return OperatorKind.OP_BITOR;
						}
						return OperatorKind.OP_LOGOR;
					case ExpressionType.RightShift:
						return OperatorKind.OP_RSHIFT;
					case ExpressionType.Subtract:
						return OperatorKind.OP_SUB;
					}
				}
				else
				{
					if (!bIsLogical)
					{
						return OperatorKind.OP_BITAND;
					}
					return OperatorKind.OP_LOGAND;
				}
			}
			else
			{
				if (p == ExpressionType.Decrement)
				{
					return OperatorKind.OP_PREDEC;
				}
				switch (p)
				{
				case ExpressionType.Increment:
					return OperatorKind.OP_PREINC;
				case ExpressionType.Index:
				case ExpressionType.Label:
				case ExpressionType.RuntimeVariables:
				case ExpressionType.Loop:
				case ExpressionType.Switch:
				case ExpressionType.Throw:
				case ExpressionType.Try:
				case ExpressionType.Unbox:
				case ExpressionType.PowerAssign:
					break;
				case ExpressionType.AddAssign:
					return OperatorKind.OP_ADDEQ;
				case ExpressionType.AndAssign:
					return OperatorKind.OP_ANDEQ;
				case ExpressionType.DivideAssign:
					return OperatorKind.OP_DIVEQ;
				case ExpressionType.ExclusiveOrAssign:
					return OperatorKind.OP_XOREQ;
				case ExpressionType.LeftShiftAssign:
					return OperatorKind.OP_LSHIFTEQ;
				case ExpressionType.ModuloAssign:
					return OperatorKind.OP_MODEQ;
				case ExpressionType.MultiplyAssign:
					return OperatorKind.OP_MULEQ;
				case ExpressionType.OrAssign:
					return OperatorKind.OP_OREQ;
				case ExpressionType.RightShiftAssign:
					return OperatorKind.OP_RSHIFTEQ;
				case ExpressionType.SubtractAssign:
					return OperatorKind.OP_SUBEQ;
				default:
					switch (p)
					{
					case ExpressionType.OnesComplement:
						return OperatorKind.OP_BITNOT;
					case ExpressionType.IsTrue:
						return OperatorKind.OP_TRUE;
					case ExpressionType.IsFalse:
						return OperatorKind.OP_FALSE;
					}
					break;
				}
			}
			throw Error.InternalCompilerError();
		}

		// Token: 0x0600010E RID: 270 RVA: 0x00005FB0 File Offset: 0x000041B0
		internal Expr BindProperty(ICSharpBinder payload, ArgumentObject argument, LocalVariableSymbol local, Expr optionalIndexerArguments)
		{
			Expr expr = (argument.Info.IsStaticType ? this._exprFactory.CreateClass(this._symbolTable.GetCTypeFromType(argument.Value as Type)) : this.CreateLocal(argument.Type, argument.Info.IsOut, local));
			if (!argument.Info.UseCompileTimeType && argument.Value == null)
			{
				throw Error.NullReferenceOnMemberException();
			}
			if (argument.Type.IsValueType && expr is ExprCast)
			{
				expr.Flags |= EXPRFLAG.EXF_USERCALLABLE;
			}
			string name = payload.Name;
			BindingFlag bindingFlags = payload.BindingFlags;
			MemberLookup memberLookup = new MemberLookup();
			SymWithType symWithType = this._symbolTable.LookupMember(name, expr, this._bindingContext.ContextForMemberLookup, 0, memberLookup, false, false);
			if (symWithType == null)
			{
				if (optionalIndexerArguments != null)
				{
					int num = ExpressionIterator.Count(optionalIndexerArguments);
					Type type = argument.Type;
					if (type.IsArray)
					{
						if (type.IsArray && type.GetArrayRank() != num)
						{
							throw this._semanticChecker.ErrorContext.Error(ErrorCode.ERR_BadIndexCount, new ErrArg[] { type.GetArrayRank() });
						}
						return this.CreateArray(expr, optionalIndexerArguments);
					}
				}
				throw memberLookup.ReportErrors();
			}
			switch (symWithType.Sym.getKind())
			{
			case SYMKIND.SK_FieldSymbol:
				return this.CreateField(symWithType, expr);
			case SYMKIND.SK_MethodSymbol:
				throw Error.BindPropertyFailedMethodGroup(name);
			case SYMKIND.SK_PropertySymbol:
				if (symWithType.Sym is IndexerSymbol)
				{
					return this.CreateIndexer(symWithType, expr, optionalIndexerArguments, bindingFlags);
				}
				expr.Flags |= EXPRFLAG.EXF_LVALUE;
				return this.CreateProperty(symWithType, expr, payload.BindingFlags);
			case SYMKIND.SK_EventSymbol:
				throw Error.BindPropertyFailedEvent(name);
			}
			throw Error.InternalCompilerError();
		}

		// Token: 0x0600010F RID: 271 RVA: 0x00006178 File Offset: 0x00004378
		internal Expr BindImplicitConversion(ArgumentObject[] arguments, Type returnType, LocalVariableSymbol[] locals, bool bIsArrayCreationConversion)
		{
			this._symbolTable.AddConversionsForType(returnType);
			Expr expr = this.CreateArgumentEXPR(arguments[0], locals[0]);
			CType ctypeFromType = this._symbolTable.GetCTypeFromType(returnType);
			if (bIsArrayCreationConversion)
			{
				CType ctype = this._binder.ChooseArrayIndexType(expr);
				return this._binder.mustCast(this._binder.mustConvert(expr, ctype), ctypeFromType, CONVERTTYPE.NOUDC | CONVERTTYPE.CHECKOVERFLOW);
			}
			return this._binder.mustConvert(expr, ctypeFromType);
		}

		// Token: 0x06000110 RID: 272 RVA: 0x000061EC File Offset: 0x000043EC
		internal Expr BindExplicitConversion(ArgumentObject[] arguments, Type returnType, LocalVariableSymbol[] locals)
		{
			this._symbolTable.AddConversionsForType(returnType);
			Expr expr = this.CreateArgumentEXPR(arguments[0], locals[0]);
			CType ctypeFromType = this._symbolTable.GetCTypeFromType(returnType);
			return this._binder.mustCast(expr, ctypeFromType);
		}

		// Token: 0x06000111 RID: 273 RVA: 0x00006230 File Offset: 0x00004430
		internal Expr BindAssignment(ICSharpBinder payload, ArgumentObject[] arguments, LocalVariableSymbol[] locals)
		{
			if (arguments.Length < 2)
			{
				throw Error.BindBinaryAssignmentRequireTwoArguments();
			}
			string name = payload.Name;
			CSharpSetIndexBinder csharpSetIndexBinder = payload as CSharpSetIndexBinder;
			Expr expr;
			bool flag;
			if (csharpSetIndexBinder != null)
			{
				expr = this.CreateArgumentListEXPR(arguments, locals, 1, arguments.Length - 1);
				flag = csharpSetIndexBinder.IsCompoundAssignment;
			}
			else
			{
				expr = null;
				flag = (payload as CSharpSetMemberBinder).IsCompoundAssignment;
			}
			this._symbolTable.PopulateSymbolTableWithName(name, null, arguments[0].Type);
			Expr expr2 = this.BindProperty(payload, arguments[0], locals[0], expr);
			int num = arguments.Length - 1;
			Expr expr3 = this.CreateArgumentEXPR(arguments[num], locals[num]);
			if (arguments[0].Type == null)
			{
				throw Error.BindBinaryAssignmentFailedNullReference();
			}
			return this._binder.BindAssignment(expr2, expr3, flag);
		}

		// Token: 0x06000112 RID: 274 RVA: 0x000062F4 File Offset: 0x000044F4
		internal Expr BindIsEvent(CSharpIsEventBinder binder, ArgumentObject[] arguments, LocalVariableSymbol[] locals)
		{
			Expr expr = this.CreateLocal(arguments[0].Type, false, locals[0]);
			MemberLookup memberLookup = new MemberLookup();
			CType predefindType = this.SymbolLoader.GetPredefindType(PredefinedType.PT_BOOL);
			bool flag = false;
			if (arguments[0].Value == null)
			{
				throw Error.NullReferenceOnMemberException();
			}
			SymWithType symWithType = this._symbolTable.LookupMember(binder.Name, expr, this._bindingContext.ContextForMemberLookup, 0, memberLookup, false, false);
			if (symWithType != null)
			{
				if (symWithType.Sym.getKind() == SYMKIND.SK_EventSymbol)
				{
					flag = true;
				}
				FieldSymbol fieldSymbol;
				if ((fieldSymbol = symWithType.Sym as FieldSymbol) != null && fieldSymbol.isEvent)
				{
					flag = true;
				}
			}
			return this._exprFactory.CreateConstant(predefindType, ConstVal.Get(flag));
		}

		// Token: 0x040000E5 RID: 229
		private static readonly Lazy<RuntimeBinder> s_lazyInstance = new Lazy<RuntimeBinder>(() => new RuntimeBinder());

		// Token: 0x040000E6 RID: 230
		private SymbolTable _symbolTable;

		// Token: 0x040000E7 RID: 231
		private CSemanticChecker _semanticChecker;

		// Token: 0x040000E8 RID: 232
		private ExprFactory _exprFactory;

		// Token: 0x040000E9 RID: 233
		private BindingContext _bindingContext;

		// Token: 0x040000EA RID: 234
		private ExpressionBinder _binder;

		// Token: 0x040000EB RID: 235
		private readonly object _bindLock = new object();
	}
}
