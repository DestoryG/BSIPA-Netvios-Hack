using System;
using Mono.Cecil.Cil;

namespace Mono.Cecil.Rocks
{
	// Token: 0x02000004 RID: 4
	public interface IILVisitor
	{
		// Token: 0x0600001A RID: 26
		void OnInlineNone(OpCode opcode);

		// Token: 0x0600001B RID: 27
		void OnInlineSByte(OpCode opcode, sbyte value);

		// Token: 0x0600001C RID: 28
		void OnInlineByte(OpCode opcode, byte value);

		// Token: 0x0600001D RID: 29
		void OnInlineInt32(OpCode opcode, int value);

		// Token: 0x0600001E RID: 30
		void OnInlineInt64(OpCode opcode, long value);

		// Token: 0x0600001F RID: 31
		void OnInlineSingle(OpCode opcode, float value);

		// Token: 0x06000020 RID: 32
		void OnInlineDouble(OpCode opcode, double value);

		// Token: 0x06000021 RID: 33
		void OnInlineString(OpCode opcode, string value);

		// Token: 0x06000022 RID: 34
		void OnInlineBranch(OpCode opcode, int offset);

		// Token: 0x06000023 RID: 35
		void OnInlineSwitch(OpCode opcode, int[] offsets);

		// Token: 0x06000024 RID: 36
		void OnInlineVariable(OpCode opcode, VariableDefinition variable);

		// Token: 0x06000025 RID: 37
		void OnInlineArgument(OpCode opcode, ParameterDefinition parameter);

		// Token: 0x06000026 RID: 38
		void OnInlineSignature(OpCode opcode, CallSite callSite);

		// Token: 0x06000027 RID: 39
		void OnInlineType(OpCode opcode, TypeReference type);

		// Token: 0x06000028 RID: 40
		void OnInlineField(OpCode opcode, FieldReference field);

		// Token: 0x06000029 RID: 41
		void OnInlineMethod(OpCode opcode, MethodReference method);
	}
}
