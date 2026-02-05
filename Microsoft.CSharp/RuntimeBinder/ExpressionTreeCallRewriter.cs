using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using Microsoft.CSharp.RuntimeBinder.Semantics;

namespace Microsoft.CSharp.RuntimeBinder
{
	// Token: 0x0200001A RID: 26
	internal sealed class ExpressionTreeCallRewriter : ExprVisitorBase
	{
		// Token: 0x060000C5 RID: 197 RVA: 0x00003481 File Offset: 0x00001681
		private ExpressionTreeCallRewriter(TypeManager typeManager, Expression[] listOfParameters)
		{
			this._typeManager = typeManager;
			this._DictionaryOfParameters = new Dictionary<ExprCall, Expression>();
			this._ListOfParameters = listOfParameters;
		}

		// Token: 0x060000C6 RID: 198 RVA: 0x000034A4 File Offset: 0x000016A4
		public static Expression Rewrite(TypeManager typeManager, Expr pExpr, Expression[] listOfParameters)
		{
			ExpressionTreeCallRewriter expressionTreeCallRewriter = new ExpressionTreeCallRewriter(typeManager, listOfParameters);
			ExprBinOp exprBinOp = (ExprBinOp)pExpr;
			expressionTreeCallRewriter.Visit(exprBinOp.OptionalLeftChild);
			ExprCall exprCall = (ExprCall)exprBinOp.OptionalRightChild;
			return (expressionTreeCallRewriter.Visit(exprCall) as ExpressionTreeCallRewriter.ExpressionExpr).Expression;
		}

		// Token: 0x060000C7 RID: 199 RVA: 0x000034E8 File Offset: 0x000016E8
		protected override Expr VisitSAVE(ExprBinOp pExpr)
		{
			ExprCall exprCall = (ExprCall)pExpr.OptionalLeftChild;
			Expression[] listOfParameters = this._ListOfParameters;
			int currentParameterIndex = this._currentParameterIndex;
			this._currentParameterIndex = currentParameterIndex + 1;
			Expression expression = listOfParameters[currentParameterIndex];
			this._DictionaryOfParameters.Add(exprCall, expression);
			return null;
		}

		// Token: 0x060000C8 RID: 200 RVA: 0x00003528 File Offset: 0x00001728
		protected override Expr VisitCALL(ExprCall pExpr)
		{
			if (pExpr.PredefinedMethod == PREDEFMETH.PM_COUNT)
			{
				return pExpr;
			}
			Expression expression;
			switch (pExpr.PredefinedMethod)
			{
			case PREDEFMETH.PM_EXPRESSION_ADD:
			case PREDEFMETH.PM_EXPRESSION_ADDCHECKED:
			case PREDEFMETH.PM_EXPRESSION_AND:
			case PREDEFMETH.PM_EXPRESSION_ANDALSO:
			case PREDEFMETH.PM_EXPRESSION_DIVIDE:
			case PREDEFMETH.PM_EXPRESSION_EQUAL:
			case PREDEFMETH.PM_EXPRESSION_EXCLUSIVEOR:
			case PREDEFMETH.PM_EXPRESSION_GREATERTHAN:
			case PREDEFMETH.PM_EXPRESSION_GREATERTHANOREQUAL:
			case PREDEFMETH.PM_EXPRESSION_LEFTSHIFT:
			case PREDEFMETH.PM_EXPRESSION_LESSTHAN:
			case PREDEFMETH.PM_EXPRESSION_LESSTHANOREQUAL:
			case PREDEFMETH.PM_EXPRESSION_MODULO:
			case PREDEFMETH.PM_EXPRESSION_MULTIPLY:
			case PREDEFMETH.PM_EXPRESSION_MULTIPLYCHECKED:
			case PREDEFMETH.PM_EXPRESSION_NOTEQUAL:
			case PREDEFMETH.PM_EXPRESSION_OR:
			case PREDEFMETH.PM_EXPRESSION_ORELSE:
			case PREDEFMETH.PM_EXPRESSION_RIGHTSHIFT:
			case PREDEFMETH.PM_EXPRESSION_SUBTRACT:
			case PREDEFMETH.PM_EXPRESSION_SUBTRACTCHECKED:
				expression = this.GenerateBinaryOperator(pExpr);
				goto IL_01BC;
			case PREDEFMETH.PM_EXPRESSION_ADD_USER_DEFINED:
			case PREDEFMETH.PM_EXPRESSION_ADDCHECKED_USER_DEFINED:
			case PREDEFMETH.PM_EXPRESSION_AND_USER_DEFINED:
			case PREDEFMETH.PM_EXPRESSION_ANDALSO_USER_DEFINED:
			case PREDEFMETH.PM_EXPRESSION_DIVIDE_USER_DEFINED:
			case PREDEFMETH.PM_EXPRESSION_EQUAL_USER_DEFINED:
			case PREDEFMETH.PM_EXPRESSION_EXCLUSIVEOR_USER_DEFINED:
			case PREDEFMETH.PM_EXPRESSION_GREATERTHAN_USER_DEFINED:
			case PREDEFMETH.PM_EXPRESSION_GREATERTHANOREQUAL_USER_DEFINED:
			case PREDEFMETH.PM_EXPRESSION_LEFTSHIFT_USER_DEFINED:
			case PREDEFMETH.PM_EXPRESSION_LESSTHAN_USER_DEFINED:
			case PREDEFMETH.PM_EXPRESSION_LESSTHANOREQUAL_USER_DEFINED:
			case PREDEFMETH.PM_EXPRESSION_MODULO_USER_DEFINED:
			case PREDEFMETH.PM_EXPRESSION_MULTIPLY_USER_DEFINED:
			case PREDEFMETH.PM_EXPRESSION_MULTIPLYCHECKED_USER_DEFINED:
			case PREDEFMETH.PM_EXPRESSION_NOTEQUAL_USER_DEFINED:
			case PREDEFMETH.PM_EXPRESSION_OR_USER_DEFINED:
			case PREDEFMETH.PM_EXPRESSION_ORELSE_USER_DEFINED:
			case PREDEFMETH.PM_EXPRESSION_RIGHTSHIFT_USER_DEFINED:
			case PREDEFMETH.PM_EXPRESSION_SUBTRACT_USER_DEFINED:
			case PREDEFMETH.PM_EXPRESSION_SUBTRACTCHECKED_USER_DEFINED:
				expression = this.GenerateUserDefinedBinaryOperator(pExpr);
				goto IL_01BC;
			case PREDEFMETH.PM_EXPRESSION_ARRAYINDEX:
			case PREDEFMETH.PM_EXPRESSION_ARRAYINDEX2:
				expression = this.GenerateArrayIndex(pExpr);
				goto IL_01BC;
			case PREDEFMETH.PM_EXPRESSION_ASSIGN:
				expression = this.GenerateAssignment(pExpr);
				goto IL_01BC;
			case PREDEFMETH.PM_EXPRESSION_CONSTANT_OBJECT_TYPE:
				expression = this.GenerateConstantType(pExpr);
				goto IL_01BC;
			case PREDEFMETH.PM_EXPRESSION_CONVERT:
			case PREDEFMETH.PM_EXPRESSION_CONVERT_USER_DEFINED:
			case PREDEFMETH.PM_EXPRESSION_CONVERTCHECKED:
			case PREDEFMETH.PM_EXPRESSION_CONVERTCHECKED_USER_DEFINED:
				expression = this.GenerateConvert(pExpr);
				goto IL_01BC;
			case PREDEFMETH.PM_EXPRESSION_FIELD:
				expression = this.GenerateField(pExpr);
				goto IL_01BC;
			case PREDEFMETH.PM_EXPRESSION_LAMBDA:
				return this.GenerateLambda(pExpr);
			case PREDEFMETH.PM_EXPRESSION_UNARYPLUS_USER_DEFINED:
			case PREDEFMETH.PM_EXPRESSION_NEGATE_USER_DEFINED:
			case PREDEFMETH.PM_EXPRESSION_NEGATECHECKED_USER_DEFINED:
			case PREDEFMETH.PM_EXPRESSION_NOT_USER_DEFINED:
				expression = this.GenerateUserDefinedUnaryOperator(pExpr);
				goto IL_01BC;
			case PREDEFMETH.PM_EXPRESSION_NEGATE:
			case PREDEFMETH.PM_EXPRESSION_NEGATECHECKED:
			case PREDEFMETH.PM_EXPRESSION_NOT:
				expression = this.GenerateUnaryOperator(pExpr);
				goto IL_01BC;
			case PREDEFMETH.PM_EXPRESSION_CALL:
				expression = this.GenerateCall(pExpr);
				goto IL_01BC;
			case PREDEFMETH.PM_EXPRESSION_NEW:
				expression = this.GenerateNew(pExpr);
				goto IL_01BC;
			case PREDEFMETH.PM_EXPRESSION_PROPERTY:
				expression = this.GenerateProperty(pExpr);
				goto IL_01BC;
			case PREDEFMETH.PM_EXPRESSION_INVOKE:
				expression = this.GenerateInvoke(pExpr);
				goto IL_01BC;
			}
			throw Error.InternalCompilerError();
			IL_01BC:
			return new ExpressionTreeCallRewriter.ExpressionExpr(expression);
		}

		// Token: 0x060000C9 RID: 201 RVA: 0x000036F7 File Offset: 0x000018F7
		private Expr GenerateLambda(ExprCall pExpr)
		{
			return base.Visit(((ExprList)pExpr.OptionalArguments).OptionalElement);
		}

		// Token: 0x060000CA RID: 202 RVA: 0x00003710 File Offset: 0x00001910
		private Expression GenerateCall(ExprCall pExpr)
		{
			ExprList exprList = (ExprList)pExpr.OptionalArguments;
			ExprList exprList2;
			ExprMethodInfo exprMethodInfo;
			ExprArrayInit exprArrayInit;
			if ((exprList2 = exprList.OptionalNextListNode as ExprList) != null)
			{
				exprMethodInfo = (ExprMethodInfo)exprList2.OptionalElement;
				exprArrayInit = (ExprArrayInit)exprList2.OptionalNextListNode;
			}
			else
			{
				exprMethodInfo = (ExprMethodInfo)exprList.OptionalNextListNode;
				exprArrayInit = null;
			}
			Expression expression = null;
			MethodInfo methodInfoFromExpr = this.GetMethodInfoFromExpr(exprMethodInfo);
			Expression[] argumentsFromArrayInit = this.GetArgumentsFromArrayInit(exprArrayInit);
			if (methodInfoFromExpr == null)
			{
				throw Error.InternalCompilerError();
			}
			if (!methodInfoFromExpr.IsStatic)
			{
				expression = this.GetExpression(((ExprList)pExpr.OptionalArguments).OptionalElement);
			}
			return Expression.Call(expression, methodInfoFromExpr, argumentsFromArrayInit);
		}

		// Token: 0x060000CB RID: 203 RVA: 0x000037B4 File Offset: 0x000019B4
		private Expression GenerateArrayIndex(ExprCall pExpr)
		{
			ExprList exprList = (ExprList)pExpr.OptionalArguments;
			Expression expression = this.GetExpression(exprList.OptionalElement);
			Expression[] array;
			if (pExpr.PredefinedMethod == PREDEFMETH.PM_EXPRESSION_ARRAYINDEX)
			{
				array = new Expression[] { this.GetExpression(exprList.OptionalNextListNode) };
			}
			else
			{
				array = this.GetArgumentsFromArrayInit((ExprArrayInit)exprList.OptionalNextListNode);
			}
			return Expression.ArrayAccess(expression, array);
		}

		// Token: 0x060000CC RID: 204 RVA: 0x00003818 File Offset: 0x00001A18
		private Expression GenerateConvert(ExprCall pExpr)
		{
			PREDEFMETH predefinedMethod = pExpr.PredefinedMethod;
			if (predefinedMethod == PREDEFMETH.PM_EXPRESSION_CONVERT_USER_DEFINED || predefinedMethod == PREDEFMETH.PM_EXPRESSION_CONVERTCHECKED_USER_DEFINED)
			{
				ExprList exprList = (ExprList)pExpr.OptionalArguments;
				ExprList exprList2 = (ExprList)exprList.OptionalNextListNode;
				Expression expression = this.GetExpression(exprList.OptionalElement);
				Type type = ((ExprTypeOf)exprList2.OptionalElement).SourceType.Type.AssociatedSystemType;
				if (expression.Type.MakeByRefType() == type)
				{
					return expression;
				}
				MethodInfo methodInfoFromExpr = this.GetMethodInfoFromExpr((ExprMethodInfo)exprList2.OptionalNextListNode);
				if (predefinedMethod == PREDEFMETH.PM_EXPRESSION_CONVERT_USER_DEFINED)
				{
					return Expression.Convert(expression, type, methodInfoFromExpr);
				}
				return Expression.ConvertChecked(expression, type, methodInfoFromExpr);
			}
			else
			{
				ExprList exprList3 = (ExprList)pExpr.OptionalArguments;
				Expression expression = this.GetExpression(exprList3.OptionalElement);
				Type type = ((ExprTypeOf)exprList3.OptionalNextListNode).SourceType.Type.AssociatedSystemType;
				if (expression.Type.MakeByRefType() == type)
				{
					return expression;
				}
				if ((pExpr.Flags & EXPRFLAG.EXF_USERCALLABLE) != (EXPRFLAG)0)
				{
					return Expression.Unbox(expression, type);
				}
				if (predefinedMethod == PREDEFMETH.PM_EXPRESSION_CONVERT)
				{
					return Expression.Convert(expression, type);
				}
				return Expression.ConvertChecked(expression, type);
			}
		}

		// Token: 0x060000CD RID: 205 RVA: 0x00003934 File Offset: 0x00001B34
		private Expression GenerateProperty(ExprCall pExpr)
		{
			ExprList exprList = (ExprList)pExpr.OptionalArguments;
			Expr optionalElement = exprList.OptionalElement;
			Expr optionalNextListNode = exprList.OptionalNextListNode;
			ExprList exprList2;
			ExprPropertyInfo exprPropertyInfo;
			ExprArrayInit exprArrayInit;
			if ((exprList2 = optionalNextListNode as ExprList) != null)
			{
				exprPropertyInfo = exprList2.OptionalElement as ExprPropertyInfo;
				exprArrayInit = exprList2.OptionalNextListNode as ExprArrayInit;
			}
			else
			{
				exprPropertyInfo = optionalNextListNode as ExprPropertyInfo;
				exprArrayInit = null;
			}
			PropertyInfo propertyInfoFromExpr = this.GetPropertyInfoFromExpr(exprPropertyInfo);
			if (propertyInfoFromExpr == null)
			{
				throw Error.InternalCompilerError();
			}
			if (exprArrayInit == null)
			{
				return Expression.Property(this.GetExpression(optionalElement), propertyInfoFromExpr);
			}
			return Expression.Property(this.GetExpression(optionalElement), propertyInfoFromExpr, this.GetArgumentsFromArrayInit(exprArrayInit));
		}

		// Token: 0x060000CE RID: 206 RVA: 0x000039CC File Offset: 0x00001BCC
		private Expression GenerateField(ExprCall pExpr)
		{
			ExprList exprList = (ExprList)pExpr.OptionalArguments;
			ExprFieldInfo exprFieldInfo = (ExprFieldInfo)exprList.OptionalNextListNode;
			Type type = exprFieldInfo.FieldType.AssociatedSystemType;
			FieldInfo fieldInfo = exprFieldInfo.Field.AssociatedFieldInfo;
			if (!type.IsGenericType && !type.IsNested)
			{
				type = fieldInfo.DeclaringType;
			}
			if (type.IsGenericType)
			{
				fieldInfo = type.GetField(fieldInfo.Name, BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
			}
			return Expression.Field(this.GetExpression(exprList.OptionalElement), fieldInfo);
		}

		// Token: 0x060000CF RID: 207 RVA: 0x00003A48 File Offset: 0x00001C48
		private Expression GenerateInvoke(ExprCall pExpr)
		{
			ExprList exprList = (ExprList)pExpr.OptionalArguments;
			return Expression.Invoke(this.GetExpression(exprList.OptionalElement), this.GetArgumentsFromArrayInit(exprList.OptionalNextListNode as ExprArrayInit));
		}

		// Token: 0x060000D0 RID: 208 RVA: 0x00003A84 File Offset: 0x00001C84
		private Expression GenerateNew(ExprCall pExpr)
		{
			ExprList exprList = (ExprList)pExpr.OptionalArguments;
			ConstructorInfo constructorInfoFromExpr = this.GetConstructorInfoFromExpr(exprList.OptionalElement as ExprMethodInfo);
			Expression[] argumentsFromArrayInit = this.GetArgumentsFromArrayInit(exprList.OptionalNextListNode as ExprArrayInit);
			return Expression.New(constructorInfoFromExpr, argumentsFromArrayInit);
		}

		// Token: 0x060000D1 RID: 209 RVA: 0x00003AC8 File Offset: 0x00001CC8
		private Expression GenerateConstantType(ExprCall pExpr)
		{
			ExprList exprList = (ExprList)pExpr.OptionalArguments;
			return Expression.Constant(this.GetObject(exprList.OptionalElement), ((ExprTypeOf)exprList.OptionalNextListNode).SourceType.Type.AssociatedSystemType);
		}

		// Token: 0x060000D2 RID: 210 RVA: 0x00003B0C File Offset: 0x00001D0C
		private Expression GenerateAssignment(ExprCall pExpr)
		{
			ExprList exprList = (ExprList)pExpr.OptionalArguments;
			return Expression.Assign(this.GetExpression(exprList.OptionalElement), this.GetExpression(exprList.OptionalNextListNode));
		}

		// Token: 0x060000D3 RID: 211 RVA: 0x00003B44 File Offset: 0x00001D44
		private Expression GenerateBinaryOperator(ExprCall pExpr)
		{
			ExprList exprList = (ExprList)pExpr.OptionalArguments;
			Expression expression = this.GetExpression(exprList.OptionalElement);
			Expression expression2 = this.GetExpression(exprList.OptionalNextListNode);
			PREDEFMETH predefinedMethod = pExpr.PredefinedMethod;
			if (predefinedMethod <= PREDEFMETH.PM_EXPRESSION_GREATERTHAN)
			{
				switch (predefinedMethod)
				{
				case PREDEFMETH.PM_EXPRESSION_ADD:
					return Expression.Add(expression, expression2);
				case PREDEFMETH.PM_EXPRESSION_ADD_USER_DEFINED:
				case PREDEFMETH.PM_EXPRESSION_ADDCHECKED_USER_DEFINED:
				case PREDEFMETH.PM_EXPRESSION_AND_USER_DEFINED:
					break;
				case PREDEFMETH.PM_EXPRESSION_ADDCHECKED:
					return Expression.AddChecked(expression, expression2);
				case PREDEFMETH.PM_EXPRESSION_AND:
					return Expression.And(expression, expression2);
				case PREDEFMETH.PM_EXPRESSION_ANDALSO:
					return Expression.AndAlso(expression, expression2);
				default:
					switch (predefinedMethod)
					{
					case PREDEFMETH.PM_EXPRESSION_DIVIDE:
						return Expression.Divide(expression, expression2);
					case PREDEFMETH.PM_EXPRESSION_DIVIDE_USER_DEFINED:
					case PREDEFMETH.PM_EXPRESSION_EQUAL_USER_DEFINED:
						break;
					case PREDEFMETH.PM_EXPRESSION_EQUAL:
						return Expression.Equal(expression, expression2);
					case PREDEFMETH.PM_EXPRESSION_EXCLUSIVEOR:
						return Expression.ExclusiveOr(expression, expression2);
					default:
						if (predefinedMethod == PREDEFMETH.PM_EXPRESSION_GREATERTHAN)
						{
							return Expression.GreaterThan(expression, expression2);
						}
						break;
					}
					break;
				}
			}
			else
			{
				if (predefinedMethod == PREDEFMETH.PM_EXPRESSION_GREATERTHANOREQUAL)
				{
					return Expression.GreaterThanOrEqual(expression, expression2);
				}
				switch (predefinedMethod)
				{
				case PREDEFMETH.PM_EXPRESSION_LEFTSHIFT:
					return Expression.LeftShift(expression, expression2);
				case PREDEFMETH.PM_EXPRESSION_LEFTSHIFT_USER_DEFINED:
				case PREDEFMETH.PM_EXPRESSION_LESSTHAN_USER_DEFINED:
				case PREDEFMETH.PM_EXPRESSION_LESSTHANOREQUAL_USER_DEFINED:
				case PREDEFMETH.PM_EXPRESSION_MODULO_USER_DEFINED:
				case PREDEFMETH.PM_EXPRESSION_MULTIPLY_USER_DEFINED:
				case PREDEFMETH.PM_EXPRESSION_MULTIPLYCHECKED_USER_DEFINED:
				case PREDEFMETH.PM_EXPRESSION_NOTEQUAL_USER_DEFINED:
				case PREDEFMETH.PM_EXPRESSION_OR_USER_DEFINED:
					break;
				case PREDEFMETH.PM_EXPRESSION_LESSTHAN:
					return Expression.LessThan(expression, expression2);
				case PREDEFMETH.PM_EXPRESSION_LESSTHANOREQUAL:
					return Expression.LessThanOrEqual(expression, expression2);
				case PREDEFMETH.PM_EXPRESSION_MODULO:
					return Expression.Modulo(expression, expression2);
				case PREDEFMETH.PM_EXPRESSION_MULTIPLY:
					return Expression.Multiply(expression, expression2);
				case PREDEFMETH.PM_EXPRESSION_MULTIPLYCHECKED:
					return Expression.MultiplyChecked(expression, expression2);
				case PREDEFMETH.PM_EXPRESSION_NOTEQUAL:
					return Expression.NotEqual(expression, expression2);
				case PREDEFMETH.PM_EXPRESSION_OR:
					return Expression.Or(expression, expression2);
				case PREDEFMETH.PM_EXPRESSION_ORELSE:
					return Expression.OrElse(expression, expression2);
				default:
					switch (predefinedMethod)
					{
					case PREDEFMETH.PM_EXPRESSION_RIGHTSHIFT:
						return Expression.RightShift(expression, expression2);
					case PREDEFMETH.PM_EXPRESSION_SUBTRACT:
						return Expression.Subtract(expression, expression2);
					case PREDEFMETH.PM_EXPRESSION_SUBTRACTCHECKED:
						return Expression.SubtractChecked(expression, expression2);
					}
					break;
				}
			}
			throw Error.InternalCompilerError();
		}

		// Token: 0x060000D4 RID: 212 RVA: 0x00003CF8 File Offset: 0x00001EF8
		private Expression GenerateUserDefinedBinaryOperator(ExprCall pExpr)
		{
			ExprList exprList = (ExprList)pExpr.OptionalArguments;
			Expression expression = this.GetExpression(exprList.OptionalElement);
			Expression expression2 = this.GetExpression(((ExprList)exprList.OptionalNextListNode).OptionalElement);
			exprList = (ExprList)exprList.OptionalNextListNode;
			bool flag = false;
			ExprList exprList2;
			MethodInfo methodInfo;
			if ((exprList2 = exprList.OptionalNextListNode as ExprList) != null)
			{
				flag = ((ExprConstant)exprList2.OptionalElement).Val.Int32Val == 1;
				methodInfo = this.GetMethodInfoFromExpr((ExprMethodInfo)exprList2.OptionalNextListNode);
			}
			else
			{
				methodInfo = this.GetMethodInfoFromExpr((ExprMethodInfo)exprList.OptionalNextListNode);
			}
			PREDEFMETH predefinedMethod = pExpr.PredefinedMethod;
			if (predefinedMethod <= PREDEFMETH.PM_EXPRESSION_GREATERTHAN_USER_DEFINED)
			{
				switch (predefinedMethod)
				{
				case PREDEFMETH.PM_EXPRESSION_ADD_USER_DEFINED:
					return Expression.Add(expression, expression2, methodInfo);
				case PREDEFMETH.PM_EXPRESSION_ADDCHECKED:
				case PREDEFMETH.PM_EXPRESSION_AND:
				case PREDEFMETH.PM_EXPRESSION_ANDALSO:
					break;
				case PREDEFMETH.PM_EXPRESSION_ADDCHECKED_USER_DEFINED:
					return Expression.AddChecked(expression, expression2, methodInfo);
				case PREDEFMETH.PM_EXPRESSION_AND_USER_DEFINED:
					return Expression.And(expression, expression2, methodInfo);
				case PREDEFMETH.PM_EXPRESSION_ANDALSO_USER_DEFINED:
					return Expression.AndAlso(expression, expression2, methodInfo);
				default:
					switch (predefinedMethod)
					{
					case PREDEFMETH.PM_EXPRESSION_DIVIDE_USER_DEFINED:
						return Expression.Divide(expression, expression2, methodInfo);
					case PREDEFMETH.PM_EXPRESSION_EQUAL:
					case PREDEFMETH.PM_EXPRESSION_EXCLUSIVEOR:
						break;
					case PREDEFMETH.PM_EXPRESSION_EQUAL_USER_DEFINED:
						return Expression.Equal(expression, expression2, flag, methodInfo);
					case PREDEFMETH.PM_EXPRESSION_EXCLUSIVEOR_USER_DEFINED:
						return Expression.ExclusiveOr(expression, expression2, methodInfo);
					default:
						if (predefinedMethod == PREDEFMETH.PM_EXPRESSION_GREATERTHAN_USER_DEFINED)
						{
							return Expression.GreaterThan(expression, expression2, flag, methodInfo);
						}
						break;
					}
					break;
				}
			}
			else
			{
				if (predefinedMethod == PREDEFMETH.PM_EXPRESSION_GREATERTHANOREQUAL_USER_DEFINED)
				{
					return Expression.GreaterThanOrEqual(expression, expression2, flag, methodInfo);
				}
				switch (predefinedMethod)
				{
				case PREDEFMETH.PM_EXPRESSION_LEFTSHIFT_USER_DEFINED:
					return Expression.LeftShift(expression, expression2, methodInfo);
				case PREDEFMETH.PM_EXPRESSION_LESSTHAN:
				case PREDEFMETH.PM_EXPRESSION_LESSTHANOREQUAL:
				case PREDEFMETH.PM_EXPRESSION_MODULO:
				case PREDEFMETH.PM_EXPRESSION_MULTIPLY:
				case PREDEFMETH.PM_EXPRESSION_MULTIPLYCHECKED:
				case PREDEFMETH.PM_EXPRESSION_NOTEQUAL:
				case PREDEFMETH.PM_EXPRESSION_OR:
				case PREDEFMETH.PM_EXPRESSION_ORELSE:
					break;
				case PREDEFMETH.PM_EXPRESSION_LESSTHAN_USER_DEFINED:
					return Expression.LessThan(expression, expression2, flag, methodInfo);
				case PREDEFMETH.PM_EXPRESSION_LESSTHANOREQUAL_USER_DEFINED:
					return Expression.LessThanOrEqual(expression, expression2, flag, methodInfo);
				case PREDEFMETH.PM_EXPRESSION_MODULO_USER_DEFINED:
					return Expression.Modulo(expression, expression2, methodInfo);
				case PREDEFMETH.PM_EXPRESSION_MULTIPLY_USER_DEFINED:
					return Expression.Multiply(expression, expression2, methodInfo);
				case PREDEFMETH.PM_EXPRESSION_MULTIPLYCHECKED_USER_DEFINED:
					return Expression.MultiplyChecked(expression, expression2, methodInfo);
				case PREDEFMETH.PM_EXPRESSION_NOTEQUAL_USER_DEFINED:
					return Expression.NotEqual(expression, expression2, flag, methodInfo);
				case PREDEFMETH.PM_EXPRESSION_OR_USER_DEFINED:
					return Expression.Or(expression, expression2, methodInfo);
				case PREDEFMETH.PM_EXPRESSION_ORELSE_USER_DEFINED:
					return Expression.OrElse(expression, expression2, methodInfo);
				default:
					switch (predefinedMethod)
					{
					case PREDEFMETH.PM_EXPRESSION_RIGHTSHIFT_USER_DEFINED:
						return Expression.RightShift(expression, expression2, methodInfo);
					case PREDEFMETH.PM_EXPRESSION_SUBTRACT_USER_DEFINED:
						return Expression.Subtract(expression, expression2, methodInfo);
					case PREDEFMETH.PM_EXPRESSION_SUBTRACTCHECKED_USER_DEFINED:
						return Expression.SubtractChecked(expression, expression2, methodInfo);
					}
					break;
				}
			}
			throw Error.InternalCompilerError();
		}

		// Token: 0x060000D5 RID: 213 RVA: 0x00003F44 File Offset: 0x00002144
		private Expression GenerateUnaryOperator(ExprCall pExpr)
		{
			PREDEFMETH predefinedMethod = pExpr.PredefinedMethod;
			Expression expression = this.GetExpression(pExpr.OptionalArguments);
			if (predefinedMethod == PREDEFMETH.PM_EXPRESSION_NEGATE)
			{
				return Expression.Negate(expression);
			}
			if (predefinedMethod == PREDEFMETH.PM_EXPRESSION_NEGATECHECKED)
			{
				return Expression.NegateChecked(expression);
			}
			if (predefinedMethod == PREDEFMETH.PM_EXPRESSION_NOT)
			{
				return Expression.Not(expression);
			}
			throw Error.InternalCompilerError();
		}

		// Token: 0x060000D6 RID: 214 RVA: 0x00003F90 File Offset: 0x00002190
		private Expression GenerateUserDefinedUnaryOperator(ExprCall pExpr)
		{
			PREDEFMETH predefinedMethod = pExpr.PredefinedMethod;
			ExprList exprList = (ExprList)pExpr.OptionalArguments;
			Expression expression = this.GetExpression(exprList.OptionalElement);
			MethodInfo methodInfoFromExpr = this.GetMethodInfoFromExpr((ExprMethodInfo)exprList.OptionalNextListNode);
			switch (predefinedMethod)
			{
			case PREDEFMETH.PM_EXPRESSION_UNARYPLUS_USER_DEFINED:
				return Expression.UnaryPlus(expression, methodInfoFromExpr);
			case PREDEFMETH.PM_EXPRESSION_NEGATE:
			case PREDEFMETH.PM_EXPRESSION_NEGATECHECKED:
				break;
			case PREDEFMETH.PM_EXPRESSION_NEGATE_USER_DEFINED:
				return Expression.Negate(expression, methodInfoFromExpr);
			case PREDEFMETH.PM_EXPRESSION_NEGATECHECKED_USER_DEFINED:
				return Expression.NegateChecked(expression, methodInfoFromExpr);
			default:
				if (predefinedMethod == PREDEFMETH.PM_EXPRESSION_NOT_USER_DEFINED)
				{
					return Expression.Not(expression, methodInfoFromExpr);
				}
				break;
			}
			throw Error.InternalCompilerError();
		}

		// Token: 0x060000D7 RID: 215 RVA: 0x00004018 File Offset: 0x00002218
		private Expression GetExpression(Expr pExpr)
		{
			ExprWrap exprWrap;
			if ((exprWrap = pExpr as ExprWrap) != null)
			{
				return this._DictionaryOfParameters[(ExprCall)exprWrap.OptionalExpression];
			}
			if (pExpr is ExprConstant)
			{
				return null;
			}
			ExprCall exprCall = (ExprCall)pExpr;
			switch (exprCall.PredefinedMethod)
			{
			case PREDEFMETH.PM_EXPRESSION_ADD:
			case PREDEFMETH.PM_EXPRESSION_ADDCHECKED:
			case PREDEFMETH.PM_EXPRESSION_AND:
			case PREDEFMETH.PM_EXPRESSION_ANDALSO:
			case PREDEFMETH.PM_EXPRESSION_DIVIDE:
			case PREDEFMETH.PM_EXPRESSION_EQUAL:
			case PREDEFMETH.PM_EXPRESSION_EXCLUSIVEOR:
			case PREDEFMETH.PM_EXPRESSION_GREATERTHAN:
			case PREDEFMETH.PM_EXPRESSION_GREATERTHANOREQUAL:
			case PREDEFMETH.PM_EXPRESSION_LEFTSHIFT:
			case PREDEFMETH.PM_EXPRESSION_LESSTHAN:
			case PREDEFMETH.PM_EXPRESSION_LESSTHANOREQUAL:
			case PREDEFMETH.PM_EXPRESSION_MODULO:
			case PREDEFMETH.PM_EXPRESSION_MULTIPLY:
			case PREDEFMETH.PM_EXPRESSION_MULTIPLYCHECKED:
			case PREDEFMETH.PM_EXPRESSION_NOTEQUAL:
			case PREDEFMETH.PM_EXPRESSION_OR:
			case PREDEFMETH.PM_EXPRESSION_ORELSE:
			case PREDEFMETH.PM_EXPRESSION_RIGHTSHIFT:
			case PREDEFMETH.PM_EXPRESSION_SUBTRACT:
			case PREDEFMETH.PM_EXPRESSION_SUBTRACTCHECKED:
				return this.GenerateBinaryOperator(exprCall);
			case PREDEFMETH.PM_EXPRESSION_ADD_USER_DEFINED:
			case PREDEFMETH.PM_EXPRESSION_ADDCHECKED_USER_DEFINED:
			case PREDEFMETH.PM_EXPRESSION_AND_USER_DEFINED:
			case PREDEFMETH.PM_EXPRESSION_ANDALSO_USER_DEFINED:
			case PREDEFMETH.PM_EXPRESSION_DIVIDE_USER_DEFINED:
			case PREDEFMETH.PM_EXPRESSION_EQUAL_USER_DEFINED:
			case PREDEFMETH.PM_EXPRESSION_EXCLUSIVEOR_USER_DEFINED:
			case PREDEFMETH.PM_EXPRESSION_GREATERTHAN_USER_DEFINED:
			case PREDEFMETH.PM_EXPRESSION_GREATERTHANOREQUAL_USER_DEFINED:
			case PREDEFMETH.PM_EXPRESSION_LEFTSHIFT_USER_DEFINED:
			case PREDEFMETH.PM_EXPRESSION_LESSTHAN_USER_DEFINED:
			case PREDEFMETH.PM_EXPRESSION_LESSTHANOREQUAL_USER_DEFINED:
			case PREDEFMETH.PM_EXPRESSION_MODULO_USER_DEFINED:
			case PREDEFMETH.PM_EXPRESSION_MULTIPLY_USER_DEFINED:
			case PREDEFMETH.PM_EXPRESSION_MULTIPLYCHECKED_USER_DEFINED:
			case PREDEFMETH.PM_EXPRESSION_NOTEQUAL_USER_DEFINED:
			case PREDEFMETH.PM_EXPRESSION_OR_USER_DEFINED:
			case PREDEFMETH.PM_EXPRESSION_ORELSE_USER_DEFINED:
			case PREDEFMETH.PM_EXPRESSION_RIGHTSHIFT_USER_DEFINED:
			case PREDEFMETH.PM_EXPRESSION_SUBTRACT_USER_DEFINED:
			case PREDEFMETH.PM_EXPRESSION_SUBTRACTCHECKED_USER_DEFINED:
				return this.GenerateUserDefinedBinaryOperator(exprCall);
			case PREDEFMETH.PM_EXPRESSION_ARRAYINDEX:
			case PREDEFMETH.PM_EXPRESSION_ARRAYINDEX2:
				return this.GenerateArrayIndex(exprCall);
			case PREDEFMETH.PM_EXPRESSION_ASSIGN:
				return this.GenerateAssignment(exprCall);
			case PREDEFMETH.PM_EXPRESSION_CONSTANT_OBJECT_TYPE:
				return this.GenerateConstantType(exprCall);
			case PREDEFMETH.PM_EXPRESSION_CONVERT:
			case PREDEFMETH.PM_EXPRESSION_CONVERT_USER_DEFINED:
			case PREDEFMETH.PM_EXPRESSION_CONVERTCHECKED:
			case PREDEFMETH.PM_EXPRESSION_CONVERTCHECKED_USER_DEFINED:
				return this.GenerateConvert(exprCall);
			case PREDEFMETH.PM_EXPRESSION_FIELD:
				return this.GenerateField(exprCall);
			case PREDEFMETH.PM_EXPRESSION_UNARYPLUS_USER_DEFINED:
			case PREDEFMETH.PM_EXPRESSION_NEGATE_USER_DEFINED:
			case PREDEFMETH.PM_EXPRESSION_NEGATECHECKED_USER_DEFINED:
			case PREDEFMETH.PM_EXPRESSION_NOT_USER_DEFINED:
				return this.GenerateUserDefinedUnaryOperator(exprCall);
			case PREDEFMETH.PM_EXPRESSION_NEGATE:
			case PREDEFMETH.PM_EXPRESSION_NEGATECHECKED:
			case PREDEFMETH.PM_EXPRESSION_NOT:
				return this.GenerateUnaryOperator(exprCall);
			case PREDEFMETH.PM_EXPRESSION_CALL:
				return this.GenerateCall(exprCall);
			case PREDEFMETH.PM_EXPRESSION_NEW:
				return this.GenerateNew(exprCall);
			case PREDEFMETH.PM_EXPRESSION_NEWARRAYINIT:
			{
				ExprList exprList = (ExprList)exprCall.OptionalArguments;
				return Expression.NewArrayInit(((ExprTypeOf)exprList.OptionalElement).SourceType.Type.AssociatedSystemType, this.GetArgumentsFromArrayInit((ExprArrayInit)exprList.OptionalNextListNode));
			}
			case PREDEFMETH.PM_EXPRESSION_PROPERTY:
				return this.GenerateProperty(exprCall);
			}
			throw Error.InternalCompilerError();
		}

		// Token: 0x060000D8 RID: 216 RVA: 0x00004218 File Offset: 0x00002418
		private object GetObject(Expr pExpr)
		{
			ExprCast exprCast;
			while ((exprCast = pExpr as ExprCast) != null)
			{
				pExpr = exprCast.Argument;
			}
			ExprTypeOf exprTypeOf;
			if ((exprTypeOf = pExpr as ExprTypeOf) != null)
			{
				return exprTypeOf.SourceType.Type.AssociatedSystemType;
			}
			ExprMethodInfo exprMethodInfo;
			if ((exprMethodInfo = pExpr as ExprMethodInfo) != null)
			{
				return this.GetMethodInfoFromExpr(exprMethodInfo);
			}
			ExprConstant exprConstant;
			if ((exprConstant = pExpr as ExprConstant) != null)
			{
				ConstVal val = exprConstant.Val;
				CType ctype = pExpr.Type;
				if (pExpr.Type is NullType)
				{
					return null;
				}
				if (pExpr.Type.isEnumType())
				{
					ctype = ctype.getAggregate().GetUnderlyingType();
				}
				object obj;
				switch (Type.GetTypeCode(ctype.AssociatedSystemType))
				{
				case TypeCode.Boolean:
					obj = val.BooleanVal;
					goto IL_01DA;
				case TypeCode.Char:
					obj = val.CharVal;
					goto IL_01DA;
				case TypeCode.SByte:
					obj = val.SByteVal;
					goto IL_01DA;
				case TypeCode.Byte:
					obj = val.ByteVal;
					goto IL_01DA;
				case TypeCode.Int16:
					obj = val.Int16Val;
					goto IL_01DA;
				case TypeCode.UInt16:
					obj = val.UInt16Val;
					goto IL_01DA;
				case TypeCode.Int32:
					obj = val.Int32Val;
					goto IL_01DA;
				case TypeCode.UInt32:
					obj = val.UInt32Val;
					goto IL_01DA;
				case TypeCode.Int64:
					obj = val.Int64Val;
					goto IL_01DA;
				case TypeCode.UInt64:
					obj = val.UInt64Val;
					goto IL_01DA;
				case TypeCode.Single:
					obj = val.SingleVal;
					goto IL_01DA;
				case TypeCode.Double:
					obj = val.DoubleVal;
					goto IL_01DA;
				case TypeCode.Decimal:
					obj = val.DecimalVal;
					goto IL_01DA;
				case TypeCode.String:
					obj = val.StringVal;
					goto IL_01DA;
				}
				obj = val.ObjectVal;
				IL_01DA:
				if (!pExpr.Type.isEnumType())
				{
					return obj;
				}
				return Enum.ToObject(pExpr.Type.AssociatedSystemType, obj);
			}
			else
			{
				ExprZeroInit exprZeroInit;
				if ((exprZeroInit = pExpr as ExprZeroInit) != null)
				{
					return Activator.CreateInstance(exprZeroInit.Type.AssociatedSystemType);
				}
				throw Error.InternalCompilerError();
			}
		}

		// Token: 0x060000D9 RID: 217 RVA: 0x00004444 File Offset: 0x00002644
		private Expression[] GetArgumentsFromArrayInit(ExprArrayInit arrinit)
		{
			List<Expression> list = new List<Expression>();
			if (arrinit != null)
			{
				Expr expr = arrinit.OptionalArguments;
				while (expr != null)
				{
					ExprList exprList;
					Expr expr2;
					if ((exprList = expr as ExprList) != null)
					{
						expr2 = exprList.OptionalElement;
						expr = exprList.OptionalNextListNode;
					}
					else
					{
						expr2 = expr;
						expr = null;
					}
					list.Add(this.GetExpression(expr2));
				}
			}
			return list.ToArray();
		}

		// Token: 0x060000DA RID: 218 RVA: 0x0000449C File Offset: 0x0000269C
		private MethodInfo GetMethodInfoFromExpr(ExprMethodInfo methinfo)
		{
			AggregateType ats = methinfo.Method.Ats;
			MethodSymbol methodSymbol = methinfo.Method.Meth();
			TypeArray typeArray = this._typeManager.SubstTypeArray(methodSymbol.Params, ats, methodSymbol.typeVars);
			this._typeManager.SubstType(methodSymbol.RetType, ats, methodSymbol.typeVars);
			Type type = ats.AssociatedSystemType;
			MethodInfo methodInfo = methodSymbol.AssociatedMemberInfo as MethodInfo;
			if (!type.IsGenericType && !type.IsNested)
			{
				type = methodInfo.DeclaringType;
			}
			foreach (MethodInfo methodInfo2 in type.GetRuntimeMethods())
			{
				if (methodInfo2.HasSameMetadataDefinitionAs(methodInfo))
				{
					bool flag = true;
					ParameterInfo[] parameters = methodInfo2.GetParameters();
					for (int i = 0; i < typeArray.Count; i++)
					{
						if (!this.TypesAreEqual(parameters[i].ParameterType, typeArray[i].AssociatedSystemType))
						{
							flag = false;
							break;
						}
					}
					if (flag)
					{
						if (methodInfo2.IsGenericMethod)
						{
							TypeArray typeArgs = methinfo.Method.TypeArgs;
							int num = ((typeArgs != null) ? typeArgs.Count : 0);
							Type[] array = new Type[num];
							if (num > 0)
							{
								for (int j = 0; j < methinfo.Method.TypeArgs.Count; j++)
								{
									array[j] = methinfo.Method.TypeArgs[j].AssociatedSystemType;
								}
							}
							return methodInfo2.MakeGenericMethod(array);
						}
						return methodInfo2;
					}
				}
			}
			throw Error.InternalCompilerError();
		}

		// Token: 0x060000DB RID: 219 RVA: 0x0000463C File Offset: 0x0000283C
		private ConstructorInfo GetConstructorInfoFromExpr(ExprMethodInfo methinfo)
		{
			AggregateType ats = methinfo.Method.Ats;
			MethodSymbol methodSymbol = methinfo.Method.Meth();
			TypeArray typeArray = this._typeManager.SubstTypeArray(methodSymbol.Params, ats);
			Type type = ats.AssociatedSystemType;
			ConstructorInfo constructorInfo = (ConstructorInfo)methodSymbol.AssociatedMemberInfo;
			if (!type.IsGenericType && !type.IsNested)
			{
				type = constructorInfo.DeclaringType;
			}
			foreach (ConstructorInfo constructorInfo2 in type.GetConstructors(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic))
			{
				if (constructorInfo2.HasSameMetadataDefinitionAs(constructorInfo))
				{
					bool flag = true;
					ParameterInfo[] parameters = constructorInfo2.GetParameters();
					for (int j = 0; j < typeArray.Count; j++)
					{
						if (!this.TypesAreEqual(parameters[j].ParameterType, typeArray[j].AssociatedSystemType))
						{
							flag = false;
							break;
						}
					}
					if (flag)
					{
						return constructorInfo2;
					}
				}
			}
			throw Error.InternalCompilerError();
		}

		// Token: 0x060000DC RID: 220 RVA: 0x00004720 File Offset: 0x00002920
		private PropertyInfo GetPropertyInfoFromExpr(ExprPropertyInfo propinfo)
		{
			AggregateType ats = propinfo.Property.Ats;
			PropertySymbol propertySymbol = propinfo.Property.Prop();
			TypeArray typeArray = this._typeManager.SubstTypeArray(propertySymbol.Params, ats, null);
			this._typeManager.SubstType(propertySymbol.RetType, ats, null);
			Type type = ats.AssociatedSystemType;
			PropertyInfo associatedPropertyInfo = propertySymbol.AssociatedPropertyInfo;
			if (!type.IsGenericType && !type.IsNested)
			{
				type = associatedPropertyInfo.DeclaringType;
			}
			foreach (PropertyInfo propertyInfo in type.GetProperties(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic))
			{
				if (propertyInfo.HasSameMetadataDefinitionAs(associatedPropertyInfo))
				{
					bool flag = true;
					ParameterInfo[] array = ((propertyInfo.GetSetMethod(true) != null) ? propertyInfo.GetSetMethod(true).GetParameters() : propertyInfo.GetGetMethod(true).GetParameters());
					for (int j = 0; j < typeArray.Count; j++)
					{
						if (!this.TypesAreEqual(array[j].ParameterType, typeArray[j].AssociatedSystemType))
						{
							flag = false;
							break;
						}
					}
					if (flag)
					{
						return propertyInfo;
					}
				}
			}
			throw Error.InternalCompilerError();
		}

		// Token: 0x060000DD RID: 221 RVA: 0x0000483F File Offset: 0x00002A3F
		private bool TypesAreEqual(Type t1, Type t2)
		{
			return t1 == t2 || t1.IsEquivalentTo(t2);
		}

		// Token: 0x040000E1 RID: 225
		private readonly Dictionary<ExprCall, Expression> _DictionaryOfParameters;

		// Token: 0x040000E2 RID: 226
		private readonly Expression[] _ListOfParameters;

		// Token: 0x040000E3 RID: 227
		private readonly TypeManager _typeManager;

		// Token: 0x040000E4 RID: 228
		private int _currentParameterIndex;

		// Token: 0x020000CE RID: 206
		private sealed class ExpressionExpr : Expr
		{
			// Token: 0x0600069E RID: 1694 RVA: 0x0001F7B8 File Offset: 0x0001D9B8
			public ExpressionExpr(Expression e)
				: base(ExpressionKind.Block)
			{
				this.Expression = e;
			}

			// Token: 0x0400067E RID: 1662
			public readonly Expression Expression;
		}
	}
}
