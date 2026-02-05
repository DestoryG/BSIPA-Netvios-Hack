using System;

namespace Microsoft.CSharp.RuntimeBinder.Errors
{
	// Token: 0x020000C0 RID: 192
	internal static class ErrorFacts
	{
		// Token: 0x06000666 RID: 1638 RVA: 0x0001E148 File Offset: 0x0001C348
		public static string GetMessage(ErrorCode code)
		{
			if (code <= ErrorCode.ERR_ValConstraintNotSatisfied)
			{
				if (code <= ErrorCode.ERR_RefProperty)
				{
					if (code <= ErrorCode.ERR_PropertyLacksGet)
					{
						if (code <= ErrorCode.ERR_MethDelegateMismatch)
						{
							switch (code)
							{
							case ErrorCode.ERR_BadBinaryOps:
								return "Operator '{0}' cannot be applied to operands of type '{1}' and '{2}'";
							case (ErrorCode)20:
							case (ErrorCode)24:
							case (ErrorCode)25:
							case (ErrorCode)26:
							case (ErrorCode)27:
							case (ErrorCode)28:
							case (ErrorCode)32:
							case (ErrorCode)33:
							case (ErrorCode)36:
								break;
							case ErrorCode.ERR_BadIndexLHS:
								return "Cannot apply indexing with [] to an expression of type '{0}'";
							case ErrorCode.ERR_BadIndexCount:
								return "Wrong number of indices inside []; expected '{0}'";
							case ErrorCode.ERR_BadUnaryOp:
								return "Operator '{0}' cannot be applied to operand of type '{1}'";
							case ErrorCode.ERR_NoImplicitConv:
								return "Cannot implicitly convert type '{0}' to '{1}'";
							case ErrorCode.ERR_NoExplicitConv:
								return "Cannot convert type '{0}' to '{1}'";
							case ErrorCode.ERR_ConstOutOfRange:
								return "Constant value '{0}' cannot be converted to a '{1}'";
							case ErrorCode.ERR_AmbigBinaryOps:
								return "Operator '{0}' is ambiguous on operands of type '{1}' and '{2}'";
							case ErrorCode.ERR_AmbigUnaryOp:
								return "Operator '{0}' is ambiguous on an operand of type '{1}'";
							case ErrorCode.ERR_ValueCantBeNull:
								return "Cannot convert null to '{0}' because it is a non-nullable value type";
							case ErrorCode.ERR_WrongNestedThis:
								return "Cannot access a non-static member of outer type '{0}' via nested type '{1}'";
							default:
								switch (code)
								{
								case ErrorCode.ERR_NoSuchMember:
									return "'{0}' does not contain a definition for '{1}'";
								case ErrorCode.ERR_ObjectRequired:
									return "An object reference is required for the non-static field, method, or property '{0}'";
								case ErrorCode.ERR_AmbigCall:
									return "The call is ambiguous between the following methods or properties: '{0}' and '{1}'";
								case ErrorCode.ERR_BadAccess:
									return "'{0}' is inaccessible due to its protection level";
								case ErrorCode.ERR_MethDelegateMismatch:
									return "No overload for '{0}' matches delegate '{1}'";
								}
								break;
							}
						}
						else
						{
							if (code == ErrorCode.ERR_AssgLvalueExpected)
							{
								return "The left-hand side of an assignment must be a variable, property or indexer";
							}
							if (code == ErrorCode.ERR_NoConstructors)
							{
								return "The type '{0}' has no constructors defined";
							}
							if (code == ErrorCode.ERR_PropertyLacksGet)
							{
								return "The property or indexer '{0}' cannot be used in this context because it lacks the get accessor";
							}
						}
					}
					else if (code <= ErrorCode.ERR_AssgReadonly)
					{
						if (code == ErrorCode.ERR_ObjectProhibited)
						{
							return "Member '{0}' cannot be accessed with an instance reference; qualify it with a type name instead";
						}
						if (code == ErrorCode.ERR_AssgReadonly)
						{
							return "A readonly field cannot be assigned to (except in a constructor or a variable initializer)";
						}
					}
					else
					{
						if (code == ErrorCode.ERR_RefReadonly)
						{
							return "A readonly field cannot be passed ref or out (except in a constructor)";
						}
						switch (code)
						{
						case ErrorCode.ERR_AssgReadonlyStatic:
							return "A static readonly field cannot be assigned to (except in a static constructor or a variable initializer)";
						case ErrorCode.ERR_RefReadonlyStatic:
							return "A static readonly field cannot be passed ref or out (except in a static constructor)";
						case ErrorCode.ERR_AssgReadonlyProp:
							return "Property or indexer '{0}' cannot be assigned to -- it is read only";
						default:
							if (code == ErrorCode.ERR_RefProperty)
							{
								return "A property or indexer may not be passed as an out or ref parameter";
							}
							break;
						}
					}
				}
				else if (code <= ErrorCode.ERR_InaccessibleGetter)
				{
					if (code <= ErrorCode.ERR_ConstOutOfRangeChecked)
					{
						switch (code)
						{
						case ErrorCode.ERR_UnsafeNeeded:
							return "Dynamic calls cannot be used in conjunction with pointers";
						case (ErrorCode)215:
						case (ErrorCode)216:
							break;
						case ErrorCode.ERR_BadBoolOp:
							return "In order to be applicable as a short circuit operator a user-defined logical operator ('{0}') must have the same return type as the type of its 2 parameters";
						case ErrorCode.ERR_MustHaveOpTF:
							return "The type ('{0}') must contain declarations of operator true and operator false";
						default:
							if (code == ErrorCode.ERR_ConstOutOfRangeChecked)
							{
								return "Constant value '{0}' cannot be converted to a '{1}' (use 'unchecked' syntax to override)";
							}
							break;
						}
					}
					else
					{
						if (code == ErrorCode.ERR_AmbigMember)
						{
							return "Ambiguity between '{0}' and '{1}'";
						}
						if (code == ErrorCode.ERR_NoImplicitConvCast)
						{
							return "Cannot implicitly convert type '{0}' to '{1}'. An explicit conversion exists (are you missing a cast?)";
						}
						if (code == ErrorCode.ERR_InaccessibleGetter)
						{
							return "The property or indexer '{0}' cannot be used in this context because the get accessor is inaccessible";
						}
					}
				}
				else if (code <= ErrorCode.ERR_GenericConstraintNotSatisfiedValType)
				{
					if (code == ErrorCode.ERR_InaccessibleSetter)
					{
						return "The property or indexer '{0}' cannot be used in this context because the set accessor is inaccessible";
					}
					switch (code)
					{
					case ErrorCode.ERR_BadArity:
						return "Using the generic {1} '{0}' requires '{2}' type arguments";
					case ErrorCode.ERR_TypeArgsNotAllowed:
						return "The {1} '{0}' cannot be used with type arguments";
					case ErrorCode.ERR_HasNoTypeVars:
						return "The non-generic {1} '{0}' cannot be used with type arguments";
					case ErrorCode.ERR_NewConstraintNotSatisfied:
						return "'{2}' must be a non-abstract type with a public parameterless constructor in order to use it as parameter '{1}' in the generic type or method '{0}'";
					case ErrorCode.ERR_GenericConstraintNotSatisfiedRefType:
						return "The type '{3}' cannot be used as type parameter '{2}' in the generic type or method '{0}'. There is no implicit reference conversion from '{3}' to '{1}'.";
					case ErrorCode.ERR_GenericConstraintNotSatisfiedNullableEnum:
						return "The type '{3}' cannot be used as type parameter '{2}' in the generic type or method '{0}'. The nullable type '{3}' does not satisfy the constraint of '{1}'.";
					case ErrorCode.ERR_GenericConstraintNotSatisfiedNullableInterface:
						return "The type '{3}' cannot be used as type parameter '{2}' in the generic type or method '{0}'. The nullable type '{3}' does not satisfy the constraint of '{1}'. Nullable types can not satisfy any interface constraints.";
					case ErrorCode.ERR_GenericConstraintNotSatisfiedValType:
						return "The type '{3}' cannot be used as type parameter '{2}' in the generic type or method '{0}'. There is no boxing conversion from '{3}' to '{1}'.";
					}
				}
				else
				{
					if (code == ErrorCode.ERR_CantInferMethTypeArgs)
					{
						return "The type arguments for method '{0}' cannot be inferred from the usage. Try specifying the type arguments explicitly.";
					}
					if (code == ErrorCode.ERR_RefConstraintNotSatisfied)
					{
						return "The type '{2}' must be a reference type in order to use it as parameter '{1}' in the generic type or method '{0}'";
					}
					if (code == ErrorCode.ERR_ValConstraintNotSatisfied)
					{
						return "The type '{2}' must be a non-nullable value type in order to use it as parameter '{1}' in the generic type or method '{0}'";
					}
				}
			}
			else if (code <= ErrorCode.ERR_BindToBogusProp2)
			{
				if (code <= ErrorCode.ERR_IncrementLvalueExpected)
				{
					if (code <= ErrorCode.ERR_BindToBogus)
					{
						if (code == ErrorCode.ERR_AmbigUDConv)
						{
							return "Ambiguous user defined conversions '{0}' and '{1}' when converting from '{2}' to '{3}'";
						}
						if (code == ErrorCode.ERR_BindToBogus)
						{
							return "'{0}' is not supported by the language";
						}
					}
					else
					{
						if (code == ErrorCode.ERR_CantCallSpecialMethod)
						{
							return "'{0}': cannot explicitly call operator or accessor";
						}
						if (code == ErrorCode.ERR_ConvertToStaticClass)
						{
							return "Cannot convert to static type '{0}'";
						}
						if (code == ErrorCode.ERR_IncrementLvalueExpected)
						{
							return "The operand of an increment or decrement operator must be a variable, property or indexer";
						}
					}
				}
				else if (code <= ErrorCode.ERR_BadArgTypes)
				{
					if (code == ErrorCode.ERR_BadArgCount)
					{
						return "No overload for method '{0}' takes '{1}' arguments";
					}
					if (code == ErrorCode.ERR_BadArgTypes)
					{
						return "The best overloaded method match for '{0}' has some invalid arguments";
					}
				}
				else
				{
					if (code == ErrorCode.ERR_RefLvalueExpected)
					{
						return "A ref or out argument must be an assignable variable";
					}
					if (code == ErrorCode.ERR_BadProtectedAccess)
					{
						return "Cannot access protected member '{0}' via a qualifier of type '{1}'; the qualifier must be of type '{2}' (or derived from it)";
					}
					if (code == ErrorCode.ERR_BindToBogusProp2)
					{
						return "Property, indexer, or event '{0}' is not supported by the language; try directly calling accessor methods '{1}' or '{2}'";
					}
				}
			}
			else if (code <= ErrorCode.ERR_RefReadonlyLocal)
			{
				if (code <= ErrorCode.ERR_BadDelArgCount)
				{
					if (code == ErrorCode.ERR_BindToBogusProp1)
					{
						return "Property, indexer, or event '{0}' is not supported by the language; try directly calling accessor method '{1}'";
					}
					if (code == ErrorCode.ERR_BadDelArgCount)
					{
						return "Delegate '{0}' does not take '{1}' arguments";
					}
				}
				else
				{
					if (code == ErrorCode.ERR_BadDelArgTypes)
					{
						return "Delegate '{0}' has some invalid arguments";
					}
					if (code == ErrorCode.ERR_AssgReadonlyLocal)
					{
						return "Cannot assign to '{0}' because it is read-only";
					}
					if (code == ErrorCode.ERR_RefReadonlyLocal)
					{
						return "Cannot pass '{0}' as a ref or out argument because it is read-only";
					}
				}
			}
			else if (code <= ErrorCode.ERR_RefReadonlyLocalCause)
			{
				if (code == ErrorCode.ERR_ReturnNotLValue)
				{
					return "Cannot modify the return value of '{0}' because it is not a variable";
				}
				switch (code)
				{
				case ErrorCode.ERR_AssgReadonly2:
					return "Members of readonly field '{0}' cannot be modified (except in a constructor or a variable initializer)";
				case ErrorCode.ERR_RefReadonly2:
					return "Members of readonly field '{0}' cannot be passed ref or out (except in a constructor)";
				case ErrorCode.ERR_AssgReadonlyStatic2:
					return "Fields of static readonly field '{0}' cannot be assigned to (except in a static constructor or a variable initializer)";
				case ErrorCode.ERR_RefReadonlyStatic2:
					return "Fields of static readonly field '{0}' cannot be passed ref or out (except in a static constructor)";
				case ErrorCode.ERR_AssgReadonlyLocalCause:
					return "Cannot assign to '{0}' because it is a '{1}'";
				case ErrorCode.ERR_RefReadonlyLocalCause:
					return "Cannot pass '{0}' as a ref or out argument because it is a '{1}'";
				}
			}
			else
			{
				if (code == ErrorCode.ERR_BadCtorArgCount)
				{
					return "'{0}' does not contain a constructor that takes '{1}' arguments";
				}
				if (code == ErrorCode.ERR_NonInvocableMemberCalled)
				{
					return "Non-invocable member '{0}' cannot be used like a method.";
				}
				switch (code)
				{
				case ErrorCode.ERR_NamedArgumentSpecificationBeforeFixedArgument:
					return "Named argument specifications must appear after all fixed arguments have been specified";
				case ErrorCode.ERR_BadNamedArgument:
					return "The best overload for '{0}' does not have a parameter named '{1}'";
				case ErrorCode.ERR_BadNamedArgumentForDelegateInvoke:
					return "The delegate '{0}' does not have a parameter named '{1}'";
				case ErrorCode.ERR_DuplicateNamedArgument:
					return "Named argument '{0}' cannot be specified multiple times";
				case ErrorCode.ERR_NamedArgumentUsedInPositional:
					return "Named argument '{0}' specifies a parameter for which a positional argument has already been given";
				}
			}
			return null;
		}

		// Token: 0x06000667 RID: 1639 RVA: 0x0001E7D7 File Offset: 0x0001C9D7
		public static string GetMessage(MessageID id)
		{
			return id.ToString();
		}
	}
}
