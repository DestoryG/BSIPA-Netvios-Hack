using System;
using System.Globalization;

// Token: 0x02000003 RID: 3
internal static class SR
{
	// Token: 0x06000001 RID: 1 RVA: 0x00002050 File Offset: 0x00000250
	internal static string GetString(string name, params object[] args)
	{
		return SR.GetString(CultureInfo.InvariantCulture, name, args);
	}

	// Token: 0x06000002 RID: 2 RVA: 0x0000205E File Offset: 0x0000025E
	internal static string GetString(CultureInfo culture, string name, params object[] args)
	{
		return string.Format(culture, name, args);
	}

	// Token: 0x06000003 RID: 3 RVA: 0x00002068 File Offset: 0x00000268
	internal static string GetString(string name)
	{
		return name;
	}

	// Token: 0x06000004 RID: 4 RVA: 0x0000206B File Offset: 0x0000026B
	internal static string GetString(CultureInfo culture, string name)
	{
		return name;
	}

	// Token: 0x06000005 RID: 5 RVA: 0x0000206E File Offset: 0x0000026E
	internal static string Format(string resourceFormat, params object[] args)
	{
		if (args != null)
		{
			return string.Format(CultureInfo.InvariantCulture, resourceFormat, args);
		}
		return resourceFormat;
	}

	// Token: 0x06000006 RID: 6 RVA: 0x00002081 File Offset: 0x00000281
	internal static string Format(string resourceFormat, object p1)
	{
		return string.Format(CultureInfo.InvariantCulture, resourceFormat, p1);
	}

	// Token: 0x06000007 RID: 7 RVA: 0x0000208F File Offset: 0x0000028F
	internal static string Format(string resourceFormat, object p1, object p2)
	{
		return string.Format(CultureInfo.InvariantCulture, resourceFormat, p1, p2);
	}

	// Token: 0x06000008 RID: 8 RVA: 0x0000209E File Offset: 0x0000029E
	internal static string Format(string resourceFormat, object p1, object p2, object p3)
	{
		return string.Format(CultureInfo.InvariantCulture, resourceFormat, p1, p2, p3);
	}

	// Token: 0x0400002A RID: 42
	public const string InternalCompilerError = "An unexpected exception occurred while binding a dynamic operation";

	// Token: 0x0400002B RID: 43
	public const string BindRequireArguments = "Cannot bind call with no calling object";

	// Token: 0x0400002C RID: 44
	public const string BindCallFailedOverloadResolution = "Overload resolution failed";

	// Token: 0x0400002D RID: 45
	public const string BindBinaryOperatorRequireTwoArguments = "Binary operators must be invoked with two arguments";

	// Token: 0x0400002E RID: 46
	public const string BindUnaryOperatorRequireOneArgument = "Unary operators must be invoked with one argument";

	// Token: 0x0400002F RID: 47
	public const string BindPropertyFailedMethodGroup = "The name '{0}' is bound to a method and cannot be used like a property";

	// Token: 0x04000030 RID: 48
	public const string BindPropertyFailedEvent = "The event '{0}' can only appear on the left hand side of +";

	// Token: 0x04000031 RID: 49
	public const string BindInvokeFailedNonDelegate = "Cannot invoke a non-delegate type";

	// Token: 0x04000032 RID: 50
	public const string BindBinaryAssignmentRequireTwoArguments = "Binary operators cannot be invoked with one argument";

	// Token: 0x04000033 RID: 51
	public const string BindBinaryAssignmentFailedNullReference = "Cannot perform member assignment on a null reference";

	// Token: 0x04000034 RID: 52
	public const string NullReferenceOnMemberException = "Cannot perform runtime binding on a null reference";

	// Token: 0x04000035 RID: 53
	public const string BindCallToConditionalMethod = "Cannot dynamically invoke method '{0}' because it has a Conditional attribute";

	// Token: 0x04000036 RID: 54
	public const string BindToVoidMethodButExpectResult = "Cannot implicitly convert type 'void' to 'object'";

	// Token: 0x04000037 RID: 55
	public const string BadBinaryOps = "Operator '{0}' cannot be applied to operands of type '{1}' and '{2}'";

	// Token: 0x04000038 RID: 56
	public const string BadIndexLHS = "Cannot apply indexing with [] to an expression of type '{0}'";

	// Token: 0x04000039 RID: 57
	public const string BadIndexCount = "Wrong number of indices inside []; expected '{0}'";

	// Token: 0x0400003A RID: 58
	public const string BadUnaryOp = "Operator '{0}' cannot be applied to operand of type '{1}'";

	// Token: 0x0400003B RID: 59
	public const string NoImplicitConv = "Cannot implicitly convert type '{0}' to '{1}'";

	// Token: 0x0400003C RID: 60
	public const string NoExplicitConv = "Cannot convert type '{0}' to '{1}'";

	// Token: 0x0400003D RID: 61
	public const string ConstOutOfRange = "Constant value '{0}' cannot be converted to a '{1}'";

	// Token: 0x0400003E RID: 62
	public const string AmbigBinaryOps = "Operator '{0}' is ambiguous on operands of type '{1}' and '{2}'";

	// Token: 0x0400003F RID: 63
	public const string AmbigUnaryOp = "Operator '{0}' is ambiguous on an operand of type '{1}'";

	// Token: 0x04000040 RID: 64
	public const string ValueCantBeNull = "Cannot convert null to '{0}' because it is a non-nullable value type";

	// Token: 0x04000041 RID: 65
	public const string WrongNestedThis = "Cannot access a non-static member of outer type '{0}' via nested type '{1}'";

	// Token: 0x04000042 RID: 66
	public const string NoSuchMember = "'{0}' does not contain a definition for '{1}'";

	// Token: 0x04000043 RID: 67
	public const string ObjectRequired = "An object reference is required for the non-static field, method, or property '{0}'";

	// Token: 0x04000044 RID: 68
	public const string AmbigCall = "The call is ambiguous between the following methods or properties: '{0}' and '{1}'";

	// Token: 0x04000045 RID: 69
	public const string BadAccess = "'{0}' is inaccessible due to its protection level";

	// Token: 0x04000046 RID: 70
	public const string MethDelegateMismatch = "No overload for '{0}' matches delegate '{1}'";

	// Token: 0x04000047 RID: 71
	public const string AssgLvalueExpected = "The left-hand side of an assignment must be a variable, property or indexer";

	// Token: 0x04000048 RID: 72
	public const string NoConstructors = "The type '{0}' has no constructors defined";

	// Token: 0x04000049 RID: 73
	public const string BadDelegateConstructor = "The delegate '{0}' does not have a valid constructor";

	// Token: 0x0400004A RID: 74
	public const string PropertyLacksGet = "The property or indexer '{0}' cannot be used in this context because it lacks the get accessor";

	// Token: 0x0400004B RID: 75
	public const string ObjectProhibited = "Member '{0}' cannot be accessed with an instance reference; qualify it with a type name instead";

	// Token: 0x0400004C RID: 76
	public const string AssgReadonly = "A readonly field cannot be assigned to (except in a constructor or a variable initializer)";

	// Token: 0x0400004D RID: 77
	public const string RefReadonly = "A readonly field cannot be passed ref or out (except in a constructor)";

	// Token: 0x0400004E RID: 78
	public const string AssgReadonlyStatic = "A static readonly field cannot be assigned to (except in a static constructor or a variable initializer)";

	// Token: 0x0400004F RID: 79
	public const string RefReadonlyStatic = "A static readonly field cannot be passed ref or out (except in a static constructor)";

	// Token: 0x04000050 RID: 80
	public const string AssgReadonlyProp = "Property or indexer '{0}' cannot be assigned to -- it is read only";

	// Token: 0x04000051 RID: 81
	public const string RefProperty = "A property or indexer may not be passed as an out or ref parameter";

	// Token: 0x04000052 RID: 82
	public const string UnsafeNeeded = "Dynamic calls cannot be used in conjunction with pointers";

	// Token: 0x04000053 RID: 83
	public const string BadBoolOp = "In order to be applicable as a short circuit operator a user-defined logical operator ('{0}') must have the same return type as the type of its 2 parameters";

	// Token: 0x04000054 RID: 84
	public const string MustHaveOpTF = "The type ('{0}') must contain declarations of operator true and operator false";

	// Token: 0x04000055 RID: 85
	public const string ConstOutOfRangeChecked = "Constant value '{0}' cannot be converted to a '{1}' (use 'unchecked' syntax to override)";

	// Token: 0x04000056 RID: 86
	public const string AmbigMember = "Ambiguity between '{0}' and '{1}'";

	// Token: 0x04000057 RID: 87
	public const string NoImplicitConvCast = "Cannot implicitly convert type '{0}' to '{1}'. An explicit conversion exists (are you missing a cast?)";

	// Token: 0x04000058 RID: 88
	public const string InaccessibleGetter = "The property or indexer '{0}' cannot be used in this context because the get accessor is inaccessible";

	// Token: 0x04000059 RID: 89
	public const string InaccessibleSetter = "The property or indexer '{0}' cannot be used in this context because the set accessor is inaccessible";

	// Token: 0x0400005A RID: 90
	public const string BadArity = "Using the generic {1} '{0}' requires '{2}' type arguments";

	// Token: 0x0400005B RID: 91
	public const string BadTypeArgument = "The type '{0}' may not be used as a type argument";

	// Token: 0x0400005C RID: 92
	public const string TypeArgsNotAllowed = "The {1} '{0}' cannot be used with type arguments";

	// Token: 0x0400005D RID: 93
	public const string HasNoTypeVars = "The non-generic {1} '{0}' cannot be used with type arguments";

	// Token: 0x0400005E RID: 94
	public const string NewConstraintNotSatisfied = "'{2}' must be a non-abstract type with a public parameterless constructor in order to use it as parameter '{1}' in the generic type or method '{0}'";

	// Token: 0x0400005F RID: 95
	public const string GenericConstraintNotSatisfiedRefType = "The type '{3}' cannot be used as type parameter '{2}' in the generic type or method '{0}'. There is no implicit reference conversion from '{3}' to '{1}'.";

	// Token: 0x04000060 RID: 96
	public const string GenericConstraintNotSatisfiedNullableEnum = "The type '{3}' cannot be used as type parameter '{2}' in the generic type or method '{0}'. The nullable type '{3}' does not satisfy the constraint of '{1}'.";

	// Token: 0x04000061 RID: 97
	public const string GenericConstraintNotSatisfiedNullableInterface = "The type '{3}' cannot be used as type parameter '{2}' in the generic type or method '{0}'. The nullable type '{3}' does not satisfy the constraint of '{1}'. Nullable types can not satisfy any interface constraints.";

	// Token: 0x04000062 RID: 98
	public const string GenericConstraintNotSatisfiedValType = "The type '{3}' cannot be used as type parameter '{2}' in the generic type or method '{0}'. There is no boxing conversion from '{3}' to '{1}'.";

	// Token: 0x04000063 RID: 99
	public const string TypeVarCantBeNull = "Cannot convert null to type parameter '{0}' because it could be a non-nullable value type. Consider using 'default({0})' instead.";

	// Token: 0x04000064 RID: 100
	public const string BadRetType = "'{1} {0}' has the wrong return type";

	// Token: 0x04000065 RID: 101
	public const string CantInferMethTypeArgs = "The type arguments for method '{0}' cannot be inferred from the usage. Try specifying the type arguments explicitly.";

	// Token: 0x04000066 RID: 102
	public const string MethGrpToNonDel = "Cannot convert method group '{0}' to non-delegate type '{1}'. Did you intend to invoke the method?";

	// Token: 0x04000067 RID: 103
	public const string RefConstraintNotSatisfied = "The type '{2}' must be a reference type in order to use it as parameter '{1}' in the generic type or method '{0}'";

	// Token: 0x04000068 RID: 104
	public const string ValConstraintNotSatisfied = "The type '{2}' must be a non-nullable value type in order to use it as parameter '{1}' in the generic type or method '{0}'";

	// Token: 0x04000069 RID: 105
	public const string AmbigUDConv = "Ambiguous user defined conversions '{0}' and '{1}' when converting from '{2}' to '{3}'";

	// Token: 0x0400006A RID: 106
	public const string BindToBogus = "'{0}' is not supported by the language";

	// Token: 0x0400006B RID: 107
	public const string CantCallSpecialMethod = "'{0}': cannot explicitly call operator or accessor";

	// Token: 0x0400006C RID: 108
	public const string ConvertToStaticClass = "Cannot convert to static type '{0}'";

	// Token: 0x0400006D RID: 109
	public const string GenericArgIsStaticClass = "'{0}': static types cannot be used as type arguments";

	// Token: 0x0400006E RID: 110
	public const string IncrementLvalueExpected = "The operand of an increment or decrement operator must be a variable, property or indexer";

	// Token: 0x0400006F RID: 111
	public const string BadArgCount = "No overload for method '{0}' takes '{1}' arguments";

	// Token: 0x04000070 RID: 112
	public const string BadArgTypes = "The best overloaded method match for '{0}' has some invalid arguments";

	// Token: 0x04000071 RID: 113
	public const string RefLvalueExpected = "A ref or out argument must be an assignable variable";

	// Token: 0x04000072 RID: 114
	public const string BadProtectedAccess = "Cannot access protected member '{0}' via a qualifier of type '{1}'; the qualifier must be of type '{2}' (or derived from it)";

	// Token: 0x04000073 RID: 115
	public const string BindToBogusProp2 = "Property, indexer, or event '{0}' is not supported by the language; try directly calling accessor methods '{1}' or '{2}'";

	// Token: 0x04000074 RID: 116
	public const string BindToBogusProp1 = "Property, indexer, or event '{0}' is not supported by the language; try directly calling accessor method '{1}'";

	// Token: 0x04000075 RID: 117
	public const string BadDelArgCount = "Delegate '{0}' does not take '{1}' arguments";

	// Token: 0x04000076 RID: 118
	public const string BadDelArgTypes = "Delegate '{0}' has some invalid arguments";

	// Token: 0x04000077 RID: 119
	public const string AssgReadonlyLocal = "Cannot assign to '{0}' because it is read-only";

	// Token: 0x04000078 RID: 120
	public const string RefReadonlyLocal = "Cannot pass '{0}' as a ref or out argument because it is read-only";

	// Token: 0x04000079 RID: 121
	public const string ReturnNotLValue = "Cannot modify the return value of '{0}' because it is not a variable";

	// Token: 0x0400007A RID: 122
	public const string AssgReadonly2 = "Members of readonly field '{0}' cannot be modified (except in a constructor or a variable initializer)";

	// Token: 0x0400007B RID: 123
	public const string RefReadonly2 = "Members of readonly field '{0}' cannot be passed ref or out (except in a constructor)";

	// Token: 0x0400007C RID: 124
	public const string AssgReadonlyStatic2 = "Fields of static readonly field '{0}' cannot be assigned to (except in a static constructor or a variable initializer)";

	// Token: 0x0400007D RID: 125
	public const string RefReadonlyStatic2 = "Fields of static readonly field '{0}' cannot be passed ref or out (except in a static constructor)";

	// Token: 0x0400007E RID: 126
	public const string AssgReadonlyLocalCause = "Cannot assign to '{0}' because it is a '{1}'";

	// Token: 0x0400007F RID: 127
	public const string RefReadonlyLocalCause = "Cannot pass '{0}' as a ref or out argument because it is a '{1}'";

	// Token: 0x04000080 RID: 128
	public const string DelegateOnNullable = "Cannot bind delegate to '{0}' because it is a member of 'System.Nullable<T>'";

	// Token: 0x04000081 RID: 129
	public const string BadCtorArgCount = "'{0}' does not contain a constructor that takes '{1}' arguments";

	// Token: 0x04000082 RID: 130
	public const string NonInvocableMemberCalled = "Non-invocable member '{0}' cannot be used like a method.";

	// Token: 0x04000083 RID: 131
	public const string NamedArgumentSpecificationBeforeFixedArgument = "Named argument specifications must appear after all fixed arguments have been specified";

	// Token: 0x04000084 RID: 132
	public const string BadNamedArgument = "The best overload for '{0}' does not have a parameter named '{1}'";

	// Token: 0x04000085 RID: 133
	public const string BadNamedArgumentForDelegateInvoke = "The delegate '{0}' does not have a parameter named '{1}'";

	// Token: 0x04000086 RID: 134
	public const string DuplicateNamedArgument = "Named argument '{0}' cannot be specified multiple times";

	// Token: 0x04000087 RID: 135
	public const string NamedArgumentUsedInPositional = "Named argument '{0}' specifies a parameter for which a positional argument has already been given";

	// Token: 0x04000088 RID: 136
	public const string TypeArgumentRequiredForStaticCall = "The first argument to dynamically-bound static or constructor call must be a Type";

	// Token: 0x04000089 RID: 137
	public const string DynamicArgumentNeedsValue = "The runtime binder cannot bind a metaobject without a value";

	// Token: 0x0400008A RID: 138
	public const string BindingNameCollision = "More than one type in the binding has the same full name.";
}
