using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.InteropServices;
using Mono.Cecil.Cil;
using Mono.Collections.Generic;
using MonoMod.Utils.Cil;

namespace HarmonyLib
{
	// Token: 0x02000017 RID: 23
	internal class Emitter
	{
		// Token: 0x06000068 RID: 104 RVA: 0x00003B33 File Offset: 0x00001D33
		internal Emitter(ILGenerator il, bool debug)
		{
			this.il = Traverse.Create(il).Field("Target").GetValue<CecilILGenerator>();
			this.debug = debug;
		}

		// Token: 0x06000069 RID: 105 RVA: 0x00003B68 File Offset: 0x00001D68
		internal Dictionary<int, CodeInstruction> GetInstructions()
		{
			return this.instructions;
		}

		// Token: 0x0600006A RID: 106 RVA: 0x00003B70 File Offset: 0x00001D70
		internal void AddInstruction(global::System.Reflection.Emit.OpCode opcode, object operand)
		{
			this.instructions.Add(this.CurrentPos(), new CodeInstruction(opcode, operand));
		}

		// Token: 0x0600006B RID: 107 RVA: 0x00003B8C File Offset: 0x00001D8C
		internal int CurrentPos()
		{
			return Traverse.Create(this.il).Field("IL").Field("instructions")
				.GetValue<Collection<Instruction>>()
				.Sum((Instruction instr) => instr.GetSize());
		}

		// Token: 0x0600006C RID: 108 RVA: 0x00003BE1 File Offset: 0x00001DE1
		internal static string CodePos(int offset)
		{
			return string.Format("IL_{0:X4}: ", offset);
		}

		// Token: 0x0600006D RID: 109 RVA: 0x00003BF3 File Offset: 0x00001DF3
		internal string CodePos()
		{
			return Emitter.CodePos(this.CurrentPos());
		}

		// Token: 0x0600006E RID: 110 RVA: 0x00003C00 File Offset: 0x00001E00
		internal void LogComment(string comment)
		{
			if (this.debug)
			{
				FileLog.LogBuffered(string.Format("{0}// {1}", this.CodePos(), comment));
			}
		}

		// Token: 0x0600006F RID: 111 RVA: 0x00003C20 File Offset: 0x00001E20
		internal void LogIL(global::System.Reflection.Emit.OpCode opcode)
		{
			if (this.debug)
			{
				FileLog.LogBuffered(string.Format("{0}{1}", this.CodePos(), opcode));
			}
		}

		// Token: 0x06000070 RID: 112 RVA: 0x00003C48 File Offset: 0x00001E48
		internal void LogIL(global::System.Reflection.Emit.OpCode opcode, object arg, string extra = null)
		{
			if (this.debug)
			{
				string text = Emitter.FormatArgument(arg, extra);
				string text2 = ((text.Length > 0) ? " " : "");
				string text3 = opcode.ToString();
				if (opcode.FlowControl == global::System.Reflection.Emit.FlowControl.Branch || opcode.FlowControl == global::System.Reflection.Emit.FlowControl.Cond_Branch)
				{
					text3 += " =>";
				}
				text3 = text3.PadRight(10);
				FileLog.LogBuffered(string.Format("{0}{1}{2}{3}", new object[]
				{
					this.CodePos(),
					text3,
					text2,
					text
				}));
			}
		}

		// Token: 0x06000071 RID: 113 RVA: 0x00003CDC File Offset: 0x00001EDC
		internal void LogAllLocalVariables()
		{
			Traverse.Create(this.il).Field("IL").Field("body")
				.Field("variables")
				.GetValue<Collection<VariableDefinition>>()
				.Do(delegate(VariableDefinition v)
				{
					FileLog.LogBuffered(string.Format("{0}Local var {1}: {2}{3}", new object[]
					{
						Emitter.CodePos(0),
						v.Index,
						v.VariableType.FullName,
						v.IsPinned ? "(pinned)" : ""
					}));
				});
		}

		// Token: 0x06000072 RID: 114 RVA: 0x00003D3C File Offset: 0x00001F3C
		internal void LogLocalVariable(LocalBuilder variable)
		{
			if (this.debug)
			{
				FileLog.LogBuffered(string.Format("{0}Local var {1}: {2}{3}", new object[]
				{
					Emitter.CodePos(0),
					variable.LocalIndex,
					variable.LocalType.FullName,
					variable.IsPinned ? "(pinned)" : ""
				}));
			}
		}

		// Token: 0x06000073 RID: 115 RVA: 0x00003DA4 File Offset: 0x00001FA4
		internal static string FormatArgument(object argument, string extra = null)
		{
			if (argument == null)
			{
				return "NULL";
			}
			Type type = argument.GetType();
			if (argument as MethodInfo != null)
			{
				return ((MethodInfo)argument).FullDescription() + ((extra != null) ? (" " + extra) : "");
			}
			if (type == typeof(string))
			{
				return argument.ToString().ToLiteral("\"");
			}
			if (type == typeof(Label))
			{
				return string.Format("Label{0}", ((Label)argument).GetHashCode());
			}
			if (type == typeof(Label[]))
			{
				return "Labels" + string.Join(",", ((Label[])argument).Select((Label l) => l.GetHashCode().ToString()).ToArray<string>());
			}
			if (type == typeof(LocalBuilder))
			{
				return string.Format("{0} ({1})", ((LocalBuilder)argument).LocalIndex, ((LocalBuilder)argument).LocalType);
			}
			return argument.ToString().Trim();
		}

		// Token: 0x06000074 RID: 116 RVA: 0x00003EE7 File Offset: 0x000020E7
		internal void MarkLabel(Label label)
		{
			if (this.debug)
			{
				FileLog.LogBuffered(this.CodePos() + Emitter.FormatArgument(label, null));
			}
			this.il.MarkLabel(label);
		}

		// Token: 0x06000075 RID: 117 RVA: 0x00003F1C File Offset: 0x0000211C
		internal void MarkBlockBefore(ExceptionBlock block, out Label? label)
		{
			label = null;
			switch (block.blockType)
			{
			case ExceptionBlockType.BeginExceptionBlock:
				if (this.debug)
				{
					FileLog.LogBuffered(".try");
					FileLog.LogBuffered("{");
					FileLog.ChangeIndent(1);
				}
				label = new Label?(this.il.BeginExceptionBlock());
				return;
			case ExceptionBlockType.BeginCatchBlock:
				if (this.debug)
				{
					this.LogIL(global::System.Reflection.Emit.OpCodes.Leave, new LeaveTry(), null);
					FileLog.ChangeIndent(-1);
					FileLog.LogBuffered("} // end try");
					FileLog.LogBuffered(string.Format(".catch {0}", block.catchType));
					FileLog.LogBuffered("{");
					FileLog.ChangeIndent(1);
				}
				this.il.BeginCatchBlock(block.catchType);
				return;
			case ExceptionBlockType.BeginExceptFilterBlock:
				if (this.debug)
				{
					this.LogIL(global::System.Reflection.Emit.OpCodes.Leave, new LeaveTry(), null);
					FileLog.ChangeIndent(-1);
					FileLog.LogBuffered("} // end try");
					FileLog.LogBuffered(".filter");
					FileLog.LogBuffered("{");
					FileLog.ChangeIndent(1);
				}
				this.il.BeginExceptFilterBlock();
				return;
			case ExceptionBlockType.BeginFaultBlock:
				if (this.debug)
				{
					this.LogIL(global::System.Reflection.Emit.OpCodes.Leave, new LeaveTry(), null);
					FileLog.ChangeIndent(-1);
					FileLog.LogBuffered("} // end try");
					FileLog.LogBuffered(".fault");
					FileLog.LogBuffered("{");
					FileLog.ChangeIndent(1);
				}
				this.il.BeginFaultBlock();
				return;
			case ExceptionBlockType.BeginFinallyBlock:
				if (this.debug)
				{
					this.LogIL(global::System.Reflection.Emit.OpCodes.Leave, new LeaveTry(), null);
					FileLog.ChangeIndent(-1);
					FileLog.LogBuffered("} // end try");
					FileLog.LogBuffered(".finally");
					FileLog.LogBuffered("{");
					FileLog.ChangeIndent(1);
				}
				this.il.BeginFinallyBlock();
				return;
			default:
				return;
			}
		}

		// Token: 0x06000076 RID: 118 RVA: 0x000040D7 File Offset: 0x000022D7
		internal void MarkBlockAfter(ExceptionBlock block)
		{
			if (block.blockType == ExceptionBlockType.EndExceptionBlock)
			{
				if (this.debug)
				{
					this.LogIL(global::System.Reflection.Emit.OpCodes.Leave, new LeaveTry(), null);
					FileLog.ChangeIndent(-1);
					FileLog.LogBuffered("} // end handler");
				}
				this.il.EndExceptionBlock();
			}
		}

		// Token: 0x06000077 RID: 119 RVA: 0x00004116 File Offset: 0x00002316
		internal void Emit(global::System.Reflection.Emit.OpCode opcode)
		{
			this.instructions.Add(this.CurrentPos(), new CodeInstruction(opcode, null));
			this.LogIL(opcode);
			this.il.Emit(opcode);
		}

		// Token: 0x06000078 RID: 120 RVA: 0x00004143 File Offset: 0x00002343
		internal void Emit(global::System.Reflection.Emit.OpCode opcode, LocalBuilder local)
		{
			this.instructions.Add(this.CurrentPos(), new CodeInstruction(opcode, local));
			this.LogIL(opcode, local, null);
			this.il.Emit(opcode, local);
		}

		// Token: 0x06000079 RID: 121 RVA: 0x00004173 File Offset: 0x00002373
		internal void Emit(global::System.Reflection.Emit.OpCode opcode, FieldInfo field)
		{
			this.instructions.Add(this.CurrentPos(), new CodeInstruction(opcode, field));
			this.LogIL(opcode, field, null);
			this.il.Emit(opcode, field);
		}

		// Token: 0x0600007A RID: 122 RVA: 0x000041A3 File Offset: 0x000023A3
		internal void Emit(global::System.Reflection.Emit.OpCode opcode, Label[] labels)
		{
			this.instructions.Add(this.CurrentPos(), new CodeInstruction(opcode, labels));
			this.LogIL(opcode, labels, null);
			this.il.Emit(opcode, labels);
		}

		// Token: 0x0600007B RID: 123 RVA: 0x000041D3 File Offset: 0x000023D3
		internal void Emit(global::System.Reflection.Emit.OpCode opcode, Label label)
		{
			this.instructions.Add(this.CurrentPos(), new CodeInstruction(opcode, label));
			this.LogIL(opcode, label, null);
			this.il.Emit(opcode, label);
		}

		// Token: 0x0600007C RID: 124 RVA: 0x0000420D File Offset: 0x0000240D
		internal void Emit(global::System.Reflection.Emit.OpCode opcode, string str)
		{
			this.instructions.Add(this.CurrentPos(), new CodeInstruction(opcode, str));
			this.LogIL(opcode, str, null);
			this.il.Emit(opcode, str);
		}

		// Token: 0x0600007D RID: 125 RVA: 0x0000423D File Offset: 0x0000243D
		internal void Emit(global::System.Reflection.Emit.OpCode opcode, float arg)
		{
			this.instructions.Add(this.CurrentPos(), new CodeInstruction(opcode, arg));
			this.LogIL(opcode, arg, null);
			this.il.Emit(opcode, arg);
		}

		// Token: 0x0600007E RID: 126 RVA: 0x00004277 File Offset: 0x00002477
		internal void Emit(global::System.Reflection.Emit.OpCode opcode, byte arg)
		{
			this.instructions.Add(this.CurrentPos(), new CodeInstruction(opcode, arg));
			this.LogIL(opcode, arg, null);
			this.il.Emit(opcode, arg);
		}

		// Token: 0x0600007F RID: 127 RVA: 0x000042B1 File Offset: 0x000024B1
		internal void Emit(global::System.Reflection.Emit.OpCode opcode, sbyte arg)
		{
			this.instructions.Add(this.CurrentPos(), new CodeInstruction(opcode, arg));
			this.LogIL(opcode, arg, null);
			this.il.Emit(opcode, arg);
		}

		// Token: 0x06000080 RID: 128 RVA: 0x000042EB File Offset: 0x000024EB
		internal void Emit(global::System.Reflection.Emit.OpCode opcode, double arg)
		{
			this.instructions.Add(this.CurrentPos(), new CodeInstruction(opcode, arg));
			this.LogIL(opcode, arg, null);
			this.il.Emit(opcode, arg);
		}

		// Token: 0x06000081 RID: 129 RVA: 0x00004325 File Offset: 0x00002525
		internal void Emit(global::System.Reflection.Emit.OpCode opcode, int arg)
		{
			this.instructions.Add(this.CurrentPos(), new CodeInstruction(opcode, arg));
			this.LogIL(opcode, arg, null);
			this.il.Emit(opcode, arg);
		}

		// Token: 0x06000082 RID: 130 RVA: 0x00004360 File Offset: 0x00002560
		internal void Emit(global::System.Reflection.Emit.OpCode opcode, MethodInfo meth)
		{
			if (opcode.Equals(global::System.Reflection.Emit.OpCodes.Call) || opcode.Equals(global::System.Reflection.Emit.OpCodes.Callvirt) || opcode.Equals(global::System.Reflection.Emit.OpCodes.Newobj))
			{
				this.EmitCall(opcode, meth, null);
				return;
			}
			this.instructions.Add(this.CurrentPos(), new CodeInstruction(opcode, meth));
			this.LogIL(opcode, meth, null);
			this.il.Emit(opcode, meth);
		}

		// Token: 0x06000083 RID: 131 RVA: 0x000043CF File Offset: 0x000025CF
		internal void Emit(global::System.Reflection.Emit.OpCode opcode, short arg)
		{
			this.instructions.Add(this.CurrentPos(), new CodeInstruction(opcode, arg));
			this.LogIL(opcode, arg, null);
			this.il.Emit(opcode, arg);
		}

		// Token: 0x06000084 RID: 132 RVA: 0x00004409 File Offset: 0x00002609
		internal void Emit(global::System.Reflection.Emit.OpCode opcode, SignatureHelper signature)
		{
			this.instructions.Add(this.CurrentPos(), new CodeInstruction(opcode, signature));
			this.LogIL(opcode, signature, null);
			this.il.Emit(opcode, signature);
		}

		// Token: 0x06000085 RID: 133 RVA: 0x00004439 File Offset: 0x00002639
		internal void Emit(global::System.Reflection.Emit.OpCode opcode, ConstructorInfo con)
		{
			this.instructions.Add(this.CurrentPos(), new CodeInstruction(opcode, con));
			this.LogIL(opcode, con, null);
			this.il.Emit(opcode, con);
		}

		// Token: 0x06000086 RID: 134 RVA: 0x00004469 File Offset: 0x00002669
		internal void Emit(global::System.Reflection.Emit.OpCode opcode, Type cls)
		{
			this.instructions.Add(this.CurrentPos(), new CodeInstruction(opcode, cls));
			this.LogIL(opcode, cls, null);
			this.il.Emit(opcode, cls);
		}

		// Token: 0x06000087 RID: 135 RVA: 0x00004499 File Offset: 0x00002699
		internal void Emit(global::System.Reflection.Emit.OpCode opcode, long arg)
		{
			this.instructions.Add(this.CurrentPos(), new CodeInstruction(opcode, arg));
			this.LogIL(opcode, arg, null);
			this.il.Emit(opcode, arg);
		}

		// Token: 0x06000088 RID: 136 RVA: 0x000044D4 File Offset: 0x000026D4
		internal void EmitCall(global::System.Reflection.Emit.OpCode opcode, MethodInfo methodInfo, Type[] optionalParameterTypes)
		{
			this.instructions.Add(this.CurrentPos(), new CodeInstruction(opcode, methodInfo));
			string text = ((optionalParameterTypes != null && optionalParameterTypes.Length != 0) ? optionalParameterTypes.Description() : null);
			this.LogIL(opcode, methodInfo, text);
			this.il.EmitCall(opcode, methodInfo, optionalParameterTypes);
		}

		// Token: 0x06000089 RID: 137 RVA: 0x00004524 File Offset: 0x00002724
		internal void EmitCalli(global::System.Reflection.Emit.OpCode opcode, CallingConvention unmanagedCallConv, Type returnType, Type[] parameterTypes)
		{
			this.instructions.Add(this.CurrentPos(), new CodeInstruction(opcode, unmanagedCallConv));
			string text = returnType.FullName + " " + parameterTypes.Description();
			this.LogIL(opcode, unmanagedCallConv, text);
			this.il.EmitCalli(opcode, unmanagedCallConv, returnType, parameterTypes);
		}

		// Token: 0x0600008A RID: 138 RVA: 0x00004584 File Offset: 0x00002784
		internal void EmitCalli(global::System.Reflection.Emit.OpCode opcode, CallingConventions callingConvention, Type returnType, Type[] parameterTypes, Type[] optionalParameterTypes)
		{
			this.instructions.Add(this.CurrentPos(), new CodeInstruction(opcode, callingConvention));
			string text = string.Concat(new string[]
			{
				returnType.FullName,
				" ",
				parameterTypes.Description(),
				" ",
				optionalParameterTypes.Description()
			});
			this.LogIL(opcode, callingConvention, text);
			this.il.EmitCalli(opcode, callingConvention, returnType, parameterTypes, optionalParameterTypes);
		}

		// Token: 0x04000043 RID: 67
		private readonly CecilILGenerator il;

		// Token: 0x04000044 RID: 68
		private readonly Dictionary<int, CodeInstruction> instructions = new Dictionary<int, CodeInstruction>();

		// Token: 0x04000045 RID: 69
		private readonly bool debug;
	}
}
