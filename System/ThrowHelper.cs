using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace System
{
	// Token: 0x02000039 RID: 57
	internal static class ThrowHelper
	{
		// Token: 0x060002DC RID: 732 RVA: 0x00010DD5 File Offset: 0x0000EFD5
		internal static void ThrowWrongKeyTypeArgumentException(object key, Type targetType)
		{
			throw new ArgumentException(SR.GetString("Arg_WrongType", new object[] { key, targetType }), "key");
		}

		// Token: 0x060002DD RID: 733 RVA: 0x00010DF9 File Offset: 0x0000EFF9
		internal static void ThrowWrongValueTypeArgumentException(object value, Type targetType)
		{
			throw new ArgumentException(SR.GetString("Arg_WrongType", new object[] { value, targetType }), "value");
		}

		// Token: 0x060002DE RID: 734 RVA: 0x00010E1D File Offset: 0x0000F01D
		internal static void ThrowKeyNotFoundException()
		{
			throw new KeyNotFoundException();
		}

		// Token: 0x060002DF RID: 735 RVA: 0x00010E24 File Offset: 0x0000F024
		internal static void ThrowArgumentException(global::System.ExceptionResource resource)
		{
			throw new ArgumentException(SR.GetString(global::System.ThrowHelper.GetResourceName(resource)));
		}

		// Token: 0x060002E0 RID: 736 RVA: 0x00010E36 File Offset: 0x0000F036
		internal static void ThrowArgumentNullException(global::System.ExceptionArgument argument)
		{
			throw new ArgumentNullException(global::System.ThrowHelper.GetArgumentName(argument));
		}

		// Token: 0x060002E1 RID: 737 RVA: 0x00010E43 File Offset: 0x0000F043
		internal static void ThrowArgumentOutOfRangeException(global::System.ExceptionArgument argument)
		{
			throw new ArgumentOutOfRangeException(global::System.ThrowHelper.GetArgumentName(argument));
		}

		// Token: 0x060002E2 RID: 738 RVA: 0x00010E50 File Offset: 0x0000F050
		internal static void ThrowArgumentOutOfRangeException(global::System.ExceptionArgument argument, global::System.ExceptionResource resource)
		{
			throw new ArgumentOutOfRangeException(global::System.ThrowHelper.GetArgumentName(argument), SR.GetString(global::System.ThrowHelper.GetResourceName(resource)));
		}

		// Token: 0x060002E3 RID: 739 RVA: 0x00010E68 File Offset: 0x0000F068
		internal static void ThrowInvalidOperationException(global::System.ExceptionResource resource)
		{
			throw new InvalidOperationException(SR.GetString(global::System.ThrowHelper.GetResourceName(resource)));
		}

		// Token: 0x060002E4 RID: 740 RVA: 0x00010E7A File Offset: 0x0000F07A
		internal static void ThrowSerializationException(global::System.ExceptionResource resource)
		{
			throw new SerializationException(SR.GetString(global::System.ThrowHelper.GetResourceName(resource)));
		}

		// Token: 0x060002E5 RID: 741 RVA: 0x00010E8C File Offset: 0x0000F08C
		internal static void ThrowNotSupportedException(global::System.ExceptionResource resource)
		{
			throw new NotSupportedException(SR.GetString(global::System.ThrowHelper.GetResourceName(resource)));
		}

		// Token: 0x060002E6 RID: 742 RVA: 0x00010EA0 File Offset: 0x0000F0A0
		internal static void IfNullAndNullsAreIllegalThenThrow<T>(object value, global::System.ExceptionArgument argName)
		{
			if (value == null && default(T) != null)
			{
				global::System.ThrowHelper.ThrowArgumentNullException(argName);
			}
		}

		// Token: 0x060002E7 RID: 743 RVA: 0x00010EC8 File Offset: 0x0000F0C8
		internal static string GetArgumentName(global::System.ExceptionArgument argument)
		{
			string text;
			switch (argument)
			{
			case global::System.ExceptionArgument.obj:
				text = "obj";
				break;
			case global::System.ExceptionArgument.dictionary:
				text = "dictionary";
				break;
			case global::System.ExceptionArgument.array:
				text = "array";
				break;
			case global::System.ExceptionArgument.info:
				text = "info";
				break;
			case global::System.ExceptionArgument.key:
				text = "key";
				break;
			case global::System.ExceptionArgument.collection:
				text = "collection";
				break;
			case global::System.ExceptionArgument.match:
				text = "match";
				break;
			case global::System.ExceptionArgument.converter:
				text = "converter";
				break;
			case global::System.ExceptionArgument.queue:
				text = "queue";
				break;
			case global::System.ExceptionArgument.stack:
				text = "stack";
				break;
			case global::System.ExceptionArgument.capacity:
				text = "capacity";
				break;
			case global::System.ExceptionArgument.index:
				text = "index";
				break;
			case global::System.ExceptionArgument.startIndex:
				text = "startIndex";
				break;
			case global::System.ExceptionArgument.value:
				text = "value";
				break;
			case global::System.ExceptionArgument.count:
				text = "count";
				break;
			case global::System.ExceptionArgument.arrayIndex:
				text = "arrayIndex";
				break;
			case global::System.ExceptionArgument.item:
				text = "item";
				break;
			default:
				return string.Empty;
			}
			return text;
		}

		// Token: 0x060002E8 RID: 744 RVA: 0x00010FB8 File Offset: 0x0000F1B8
		internal static string GetResourceName(global::System.ExceptionResource resource)
		{
			switch (resource)
			{
			case global::System.ExceptionResource.Argument_ImplementIComparable:
				return "Argument_ImplementIComparable";
			case global::System.ExceptionResource.ArgumentOutOfRange_NeedNonNegNum:
				return "ArgumentOutOfRange_NeedNonNegNum";
			case global::System.ExceptionResource.ArgumentOutOfRange_NeedNonNegNumRequired:
				return "ArgumentOutOfRange_NeedNonNegNumRequired";
			case global::System.ExceptionResource.Arg_ArrayPlusOffTooSmall:
				return "Arg_ArrayPlusOffTooSmall";
			case global::System.ExceptionResource.Argument_AddingDuplicate:
				return "Argument_AddingDuplicate";
			case global::System.ExceptionResource.Serialization_InvalidOnDeser:
				return "Serialization_InvalidOnDeser";
			case global::System.ExceptionResource.Serialization_MismatchedCount:
				return "Serialization_MismatchedCount";
			case global::System.ExceptionResource.Serialization_MissingValues:
				return "Serialization_MissingValues";
			case global::System.ExceptionResource.Arg_RankMultiDimNotSupported:
				return "Arg_MultiRank";
			case global::System.ExceptionResource.Arg_NonZeroLowerBound:
				return "Arg_NonZeroLowerBound";
			case global::System.ExceptionResource.Argument_InvalidArrayType:
				return "Invalid_Array_Type";
			case global::System.ExceptionResource.NotSupported_KeyCollectionSet:
				return "NotSupported_KeyCollectionSet";
			case global::System.ExceptionResource.ArgumentOutOfRange_SmallCapacity:
				return "ArgumentOutOfRange_SmallCapacity";
			case global::System.ExceptionResource.ArgumentOutOfRange_Index:
				return "ArgumentOutOfRange_Index";
			case global::System.ExceptionResource.Argument_InvalidOffLen:
				return "Argument_InvalidOffLen";
			case global::System.ExceptionResource.InvalidOperation_CannotRemoveFromStackOrQueue:
				return "InvalidOperation_CannotRemoveFromStackOrQueue";
			case global::System.ExceptionResource.InvalidOperation_EmptyCollection:
				return "InvalidOperation_EmptyCollection";
			case global::System.ExceptionResource.InvalidOperation_EmptyQueue:
				return "InvalidOperation_EmptyQueue";
			case global::System.ExceptionResource.InvalidOperation_EnumOpCantHappen:
				return "InvalidOperation_EnumOpCantHappen";
			case global::System.ExceptionResource.InvalidOperation_EnumFailedVersion:
				return "InvalidOperation_EnumFailedVersion";
			case global::System.ExceptionResource.InvalidOperation_EmptyStack:
				return "InvalidOperation_EmptyStack";
			case global::System.ExceptionResource.InvalidOperation_EnumNotStarted:
				return "InvalidOperation_EnumNotStarted";
			case global::System.ExceptionResource.InvalidOperation_EnumEnded:
				return "InvalidOperation_EnumEnded";
			case global::System.ExceptionResource.NotSupported_SortedListNestedWrite:
				return "NotSupported_SortedListNestedWrite";
			case global::System.ExceptionResource.NotSupported_ValueCollectionSet:
				return "NotSupported_ValueCollectionSet";
			}
			return string.Empty;
		}
	}
}
