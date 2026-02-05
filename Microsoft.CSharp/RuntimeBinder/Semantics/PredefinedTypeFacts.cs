using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Microsoft.CSharp.RuntimeBinder.Syntax;

namespace Microsoft.CSharp.RuntimeBinder.Semantics
{
	// Token: 0x020000AC RID: 172
	internal static class PredefinedTypeFacts
	{
		// Token: 0x060005A1 RID: 1441 RVA: 0x0001B8CA File Offset: 0x00019ACA
		internal static string GetName(PredefinedType type)
		{
			return PredefinedTypeFacts.s_types[(int)type].Name;
		}

		// Token: 0x060005A2 RID: 1442 RVA: 0x0001B8D8 File Offset: 0x00019AD8
		internal static FUNDTYPE GetFundType(PredefinedType type)
		{
			return PredefinedTypeFacts.s_types[(int)type].FundType;
		}

		// Token: 0x060005A3 RID: 1443 RVA: 0x0001B8E6 File Offset: 0x00019AE6
		internal static Type GetAssociatedSystemType(PredefinedType type)
		{
			return PredefinedTypeFacts.s_types[(int)type].AssociatedSystemType;
		}

		// Token: 0x060005A4 RID: 1444 RVA: 0x0001B8F4 File Offset: 0x00019AF4
		internal static bool IsSimpleType(PredefinedType type)
		{
			return type < PredefinedType.FirstNonSimpleType;
		}

		// Token: 0x060005A5 RID: 1445 RVA: 0x0001B8FB File Offset: 0x00019AFB
		internal static bool IsNumericType(PredefinedType type)
		{
			return type <= PredefinedType.PT_DECIMAL || type - PredefinedType.PT_SBYTE <= 3U;
		}

		// Token: 0x060005A6 RID: 1446 RVA: 0x0001B90C File Offset: 0x00019B0C
		internal static string GetNiceName(PredefinedType type)
		{
			switch (type)
			{
			case PredefinedType.PT_BYTE:
				return "byte";
			case PredefinedType.PT_SHORT:
				return "short";
			case PredefinedType.PT_INT:
				return "int";
			case PredefinedType.PT_LONG:
				return "long";
			case PredefinedType.PT_FLOAT:
				return "float";
			case PredefinedType.PT_DOUBLE:
				return "double";
			case PredefinedType.PT_DECIMAL:
				return "decimal";
			case PredefinedType.PT_CHAR:
				return "char";
			case PredefinedType.PT_BOOL:
				return "bool";
			case PredefinedType.PT_SBYTE:
				return "sbyte";
			case PredefinedType.PT_USHORT:
				return "ushort";
			case PredefinedType.PT_UINT:
				return "uint";
			case PredefinedType.PT_ULONG:
				return "ulong";
			case PredefinedType.PT_OBJECT:
				return "object";
			case PredefinedType.PT_STRING:
				return "string";
			}
			return null;
		}

		// Token: 0x060005A7 RID: 1447 RVA: 0x0001B9C0 File Offset: 0x00019BC0
		public static PredefinedType TryGetPredefTypeIndex(string name)
		{
			PredefinedType predefinedType;
			if (!PredefinedTypeFacts.s_typesByName.TryGetValue(name, out predefinedType))
			{
				return (PredefinedType)4294967295U;
			}
			return predefinedType;
		}

		// Token: 0x060005A8 RID: 1448 RVA: 0x0001B9E0 File Offset: 0x00019BE0
		private static Dictionary<string, PredefinedType> CreatePredefinedTypeFacts()
		{
			Dictionary<string, PredefinedType> dictionary = new Dictionary<string, PredefinedType>(48);
			for (int i = 0; i < 48; i++)
			{
				dictionary.Add(PredefinedTypeFacts.s_types[i].Name, (PredefinedType)i);
			}
			return dictionary;
		}

		// Token: 0x04000589 RID: 1417
		private static readonly PredefinedTypeFacts.PredefinedTypeInfo[] s_types = new PredefinedTypeFacts.PredefinedTypeInfo[]
		{
			new PredefinedTypeFacts.PredefinedTypeInfo(PredefinedType.PT_BYTE, typeof(byte), "System.Byte", FUNDTYPE.FT_U1),
			new PredefinedTypeFacts.PredefinedTypeInfo(PredefinedType.PT_SHORT, typeof(short), "System.Int16", FUNDTYPE.FT_I2),
			new PredefinedTypeFacts.PredefinedTypeInfo(PredefinedType.PT_INT, typeof(int), "System.Int32", FUNDTYPE.FT_I4),
			new PredefinedTypeFacts.PredefinedTypeInfo(PredefinedType.PT_LONG, typeof(long), "System.Int64", FUNDTYPE.FT_I8),
			new PredefinedTypeFacts.PredefinedTypeInfo(PredefinedType.PT_FLOAT, typeof(float), "System.Single", FUNDTYPE.FT_R4),
			new PredefinedTypeFacts.PredefinedTypeInfo(PredefinedType.PT_DOUBLE, typeof(double), "System.Double", FUNDTYPE.FT_R8),
			new PredefinedTypeFacts.PredefinedTypeInfo(PredefinedType.PT_DECIMAL, typeof(decimal), "System.Decimal", FUNDTYPE.FT_STRUCT),
			new PredefinedTypeFacts.PredefinedTypeInfo(PredefinedType.PT_CHAR, typeof(char), "System.Char", FUNDTYPE.FT_U2),
			new PredefinedTypeFacts.PredefinedTypeInfo(PredefinedType.PT_BOOL, typeof(bool), "System.Boolean", FUNDTYPE.FT_I1),
			new PredefinedTypeFacts.PredefinedTypeInfo(PredefinedType.PT_SBYTE, typeof(sbyte), "System.SByte", FUNDTYPE.FT_I1),
			new PredefinedTypeFacts.PredefinedTypeInfo(PredefinedType.PT_USHORT, typeof(ushort), "System.UInt16", FUNDTYPE.FT_U2),
			new PredefinedTypeFacts.PredefinedTypeInfo(PredefinedType.PT_UINT, typeof(uint), "System.UInt32", FUNDTYPE.FT_U4),
			new PredefinedTypeFacts.PredefinedTypeInfo(PredefinedType.PT_ULONG, typeof(ulong), "System.UInt64", FUNDTYPE.FT_U8),
			new PredefinedTypeFacts.PredefinedTypeInfo(PredefinedType.FirstNonSimpleType, typeof(IntPtr), "System.IntPtr", FUNDTYPE.FT_STRUCT),
			new PredefinedTypeFacts.PredefinedTypeInfo(PredefinedType.PT_UINTPTR, typeof(UIntPtr), "System.UIntPtr", FUNDTYPE.FT_STRUCT),
			new PredefinedTypeFacts.PredefinedTypeInfo(PredefinedType.PT_OBJECT, typeof(object), "System.Object"),
			new PredefinedTypeFacts.PredefinedTypeInfo(PredefinedType.PT_STRING, typeof(string), "System.String"),
			new PredefinedTypeFacts.PredefinedTypeInfo(PredefinedType.PT_DELEGATE, typeof(Delegate), "System.Delegate"),
			new PredefinedTypeFacts.PredefinedTypeInfo(PredefinedType.PT_MULTIDEL, typeof(MulticastDelegate), "System.MulticastDelegate"),
			new PredefinedTypeFacts.PredefinedTypeInfo(PredefinedType.PT_ARRAY, typeof(Array), "System.Array"),
			new PredefinedTypeFacts.PredefinedTypeInfo(PredefinedType.PT_TYPE, typeof(Type), "System.Type"),
			new PredefinedTypeFacts.PredefinedTypeInfo(PredefinedType.PT_VALUE, typeof(ValueType), "System.ValueType"),
			new PredefinedTypeFacts.PredefinedTypeInfo(PredefinedType.PT_ENUM, typeof(Enum), "System.Enum"),
			new PredefinedTypeFacts.PredefinedTypeInfo(PredefinedType.PT_DATETIME, typeof(DateTime), "System.DateTime", FUNDTYPE.FT_STRUCT),
			new PredefinedTypeFacts.PredefinedTypeInfo(PredefinedType.PT_IENUMERABLE, typeof(IEnumerable), "System.Collections.IEnumerable"),
			new PredefinedTypeFacts.PredefinedTypeInfo(PredefinedType.PT_G_IENUMERABLE, typeof(IEnumerable<>), "System.Collections.Generic.IEnumerable`1"),
			new PredefinedTypeFacts.PredefinedTypeInfo(PredefinedType.PT_G_OPTIONAL, typeof(Nullable<>), "System.Nullable`1", FUNDTYPE.FT_STRUCT),
			new PredefinedTypeFacts.PredefinedTypeInfo(PredefinedType.PT_G_IQUERYABLE, typeof(IQueryable<>), "System.Linq.IQueryable`1"),
			new PredefinedTypeFacts.PredefinedTypeInfo(PredefinedType.PT_G_ICOLLECTION, typeof(ICollection<>), "System.Collections.Generic.ICollection`1"),
			new PredefinedTypeFacts.PredefinedTypeInfo(PredefinedType.PT_G_ILIST, typeof(IList<>), "System.Collections.Generic.IList`1"),
			new PredefinedTypeFacts.PredefinedTypeInfo(PredefinedType.PT_G_EXPRESSION, typeof(Expression<>), "System.Linq.Expressions.Expression`1"),
			new PredefinedTypeFacts.PredefinedTypeInfo(PredefinedType.PT_EXPRESSION, typeof(Expression), "System.Linq.Expressions.Expression"),
			new PredefinedTypeFacts.PredefinedTypeInfo(PredefinedType.PT_BINARYEXPRESSION, typeof(BinaryExpression), "System.Linq.Expressions.BinaryExpression"),
			new PredefinedTypeFacts.PredefinedTypeInfo(PredefinedType.PT_UNARYEXPRESSION, typeof(UnaryExpression), "System.Linq.Expressions.UnaryExpression"),
			new PredefinedTypeFacts.PredefinedTypeInfo(PredefinedType.PT_CONSTANTEXPRESSION, typeof(ConstantExpression), "System.Linq.Expressions.ConstantExpression"),
			new PredefinedTypeFacts.PredefinedTypeInfo(PredefinedType.PT_PARAMETEREXPRESSION, typeof(ParameterExpression), "System.Linq.Expressions.ParameterExpression"),
			new PredefinedTypeFacts.PredefinedTypeInfo(PredefinedType.PT_MEMBEREXPRESSION, typeof(MemberExpression), "System.Linq.Expressions.MemberExpression"),
			new PredefinedTypeFacts.PredefinedTypeInfo(PredefinedType.PT_METHODCALLEXPRESSION, typeof(MethodCallExpression), "System.Linq.Expressions.MethodCallExpression"),
			new PredefinedTypeFacts.PredefinedTypeInfo(PredefinedType.PT_NEWEXPRESSION, typeof(NewExpression), "System.Linq.Expressions.NewExpression"),
			new PredefinedTypeFacts.PredefinedTypeInfo(PredefinedType.PT_NEWARRAYEXPRESSION, typeof(NewArrayExpression), "System.Linq.Expressions.NewArrayExpression"),
			new PredefinedTypeFacts.PredefinedTypeInfo(PredefinedType.PT_INVOCATIONEXPRESSION, typeof(InvocationExpression), "System.Linq.Expressions.InvocationExpression"),
			new PredefinedTypeFacts.PredefinedTypeInfo(PredefinedType.PT_FIELDINFO, typeof(FieldInfo), "System.Reflection.FieldInfo"),
			new PredefinedTypeFacts.PredefinedTypeInfo(PredefinedType.PT_METHODINFO, typeof(MethodInfo), "System.Reflection.MethodInfo"),
			new PredefinedTypeFacts.PredefinedTypeInfo(PredefinedType.PT_CONSTRUCTORINFO, typeof(ConstructorInfo), "System.Reflection.ConstructorInfo"),
			new PredefinedTypeFacts.PredefinedTypeInfo(PredefinedType.PT_PROPERTYINFO, typeof(PropertyInfo), "System.Reflection.PropertyInfo"),
			new PredefinedTypeFacts.PredefinedTypeInfo(PredefinedType.PT_MISSING, typeof(Missing), "System.Reflection.Missing"),
			new PredefinedTypeFacts.PredefinedTypeInfo(PredefinedType.PT_G_IREADONLYLIST, typeof(IReadOnlyList<>), "System.Collections.Generic.IReadOnlyList`1"),
			new PredefinedTypeFacts.PredefinedTypeInfo(PredefinedType.PT_G_IREADONLYCOLLECTION, typeof(IReadOnlyCollection<>), "System.Collections.Generic.IReadOnlyCollection`1")
		};

		// Token: 0x0400058A RID: 1418
		private static readonly Dictionary<string, PredefinedType> s_typesByName = PredefinedTypeFacts.CreatePredefinedTypeFacts();

		// Token: 0x020000F1 RID: 241
		private sealed class PredefinedTypeInfo
		{
			// Token: 0x06000757 RID: 1879 RVA: 0x00023B9F File Offset: 0x00021D9F
			internal PredefinedTypeInfo(PredefinedType type, Type associatedSystemType, string name, FUNDTYPE fundType)
			{
				this.Name = name;
				this.FundType = fundType;
				this.AssociatedSystemType = associatedSystemType;
			}

			// Token: 0x06000758 RID: 1880 RVA: 0x00023BBD File Offset: 0x00021DBD
			internal PredefinedTypeInfo(PredefinedType type, Type associatedSystemType, string name)
				: this(type, associatedSystemType, name, FUNDTYPE.FT_REF)
			{
			}

			// Token: 0x0400071B RID: 1819
			public readonly string Name;

			// Token: 0x0400071C RID: 1820
			public readonly FUNDTYPE FundType;

			// Token: 0x0400071D RID: 1821
			public readonly Type AssociatedSystemType;
		}
	}
}
