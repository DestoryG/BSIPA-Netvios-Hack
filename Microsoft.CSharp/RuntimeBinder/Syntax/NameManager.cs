using System;

namespace Microsoft.CSharp.RuntimeBinder.Syntax
{
	// Token: 0x02000024 RID: 36
	internal class NameManager
	{
		// Token: 0x06000159 RID: 345 RVA: 0x000091FC File Offset: 0x000073FC
		private static NameTable GetKnownNames()
		{
			NameTable nameTable = new NameTable();
			foreach (Name name in NameManager.s_predefinedNames)
			{
				nameTable.Add(name);
			}
			return nameTable;
		}

		// Token: 0x0600015A RID: 346 RVA: 0x0000922F File Offset: 0x0000742F
		internal Name Add(string key)
		{
			if (key == null)
			{
				throw Error.InternalCompilerError();
			}
			return NameManager.s_knownNames.Lookup(key) ?? this._names.Add(key);
		}

		// Token: 0x0600015B RID: 347 RVA: 0x00009255 File Offset: 0x00007455
		internal Name Add(string key, int length)
		{
			return NameManager.s_knownNames.Lookup(key, length) ?? this._names.Add(key, length);
		}

		// Token: 0x0600015C RID: 348 RVA: 0x00009274 File Offset: 0x00007474
		internal Name Lookup(string key)
		{
			if (key == null)
			{
				throw Error.InternalCompilerError();
			}
			return NameManager.s_knownNames.Lookup(key) ?? this._names.Lookup(key);
		}

		// Token: 0x0600015D RID: 349 RVA: 0x0000929A File Offset: 0x0000749A
		internal static Name GetPredefinedName(PredefinedName id)
		{
			return NameManager.s_predefinedNames[(int)id];
		}

		// Token: 0x04000123 RID: 291
		private static readonly Name[] s_predefinedNames = new Name[]
		{
			new Name(".ctor"),
			new Name("Finalize"),
			new Name(".cctor"),
			new Name("*"),
			new Name("?*"),
			new Name("#"),
			new Name("&"),
			new Name("[X\001"),
			new Name("[X\002"),
			new Name("[X\003"),
			new Name("[G\001"),
			new Name("[G\002"),
			new Name("[G\003"),
			new Name("Invoke"),
			new Name("Length"),
			new Name("Item"),
			new Name("$Item$"),
			new Name("Combine"),
			new Name("Remove"),
			new Name("op_Explicit"),
			new Name("op_Implicit"),
			new Name("op_UnaryPlus"),
			new Name("op_UnaryNegation"),
			new Name("op_OnesComplement"),
			new Name("op_Increment"),
			new Name("op_Decrement"),
			new Name("op_Addition"),
			new Name("op_Subtraction"),
			new Name("op_Multiply"),
			new Name("op_Division"),
			new Name("op_Modulus"),
			new Name("op_ExclusiveOr"),
			new Name("op_BitwiseAnd"),
			new Name("op_BitwiseOr"),
			new Name("op_LeftShift"),
			new Name("op_RightShift"),
			new Name("op_Equals"),
			new Name("op_Compare"),
			new Name("op_Equality"),
			new Name("op_Inequality"),
			new Name("op_GreaterThan"),
			new Name("op_LessThan"),
			new Name("op_GreaterThanOrEqual"),
			new Name("op_LessThanOrEqual"),
			new Name("op_True"),
			new Name("op_False"),
			new Name("op_LogicalNot"),
			new Name("Concat"),
			new Name("Add"),
			new Name("get_Length"),
			new Name("get_Chars"),
			new Name("CreateDelegate"),
			new Name("FixedElementField"),
			new Name("HasValue"),
			new Name("get_HasValue"),
			new Name("Value"),
			new Name("get_Value"),
			new Name("GetValueOrDefault"),
			new Name("?"),
			new Name("<?>"),
			new Name("Lambda"),
			new Name("Parameter"),
			new Name("Constant"),
			new Name("Convert"),
			new Name("ConvertChecked"),
			new Name("AddChecked"),
			new Name("Divide"),
			new Name("Modulo"),
			new Name("Multiply"),
			new Name("MultiplyChecked"),
			new Name("Subtract"),
			new Name("SubtractChecked"),
			new Name("And"),
			new Name("Or"),
			new Name("ExclusiveOr"),
			new Name("LeftShift"),
			new Name("RightShift"),
			new Name("AndAlso"),
			new Name("OrElse"),
			new Name("Equal"),
			new Name("NotEqual"),
			new Name("GreaterThanOrEqual"),
			new Name("GreaterThan"),
			new Name("LessThan"),
			new Name("LessThanOrEqual"),
			new Name("ArrayIndex"),
			new Name("Assign"),
			new Name("Condition"),
			new Name("Field"),
			new Name("Call"),
			new Name("New"),
			new Name("Quote"),
			new Name("ArrayLength"),
			new Name("UnaryPlus"),
			new Name("Negate"),
			new Name("NegateChecked"),
			new Name("Not"),
			new Name("NewArrayInit"),
			new Name("Property"),
			new Name("AddEventHandler"),
			new Name("RemoveEventHandler"),
			new Name("InvocationList"),
			new Name("GetOrCreateEventRegistrationTokenTable"),
			new Name("true"),
			new Name("false"),
			new Name("null"),
			new Name("base"),
			new Name("this"),
			new Name("explicit"),
			new Name("implicit"),
			new Name("__arglist"),
			new Name("__makeref"),
			new Name("__reftype"),
			new Name("__refvalue"),
			new Name("as"),
			new Name("checked"),
			new Name("is"),
			new Name("typeof"),
			new Name("unchecked"),
			new Name("void"),
			new Name("")
		};

		// Token: 0x04000124 RID: 292
		private static readonly NameTable s_knownNames = NameManager.GetKnownNames();

		// Token: 0x04000125 RID: 293
		private readonly NameTable _names = new NameTable();
	}
}
