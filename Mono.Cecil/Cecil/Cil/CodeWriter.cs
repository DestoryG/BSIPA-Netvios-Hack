using System;
using System.Collections.Generic;
using Mono.Cecil.Metadata;
using Mono.Cecil.PE;
using Mono.Collections.Generic;

namespace Mono.Cecil.Cil
{
	// Token: 0x020000F5 RID: 245
	internal sealed class CodeWriter : ByteBuffer
	{
		// Token: 0x060009BE RID: 2494 RVA: 0x0001FF22 File Offset: 0x0001E122
		public CodeWriter(MetadataBuilder metadata)
			: base(0)
		{
			this.code_base = metadata.text_map.GetNextRVA(TextSegment.CLIHeader);
			this.metadata = metadata;
			this.standalone_signatures = new Dictionary<uint, MetadataToken>();
			this.tiny_method_bodies = new Dictionary<ByteBuffer, uint>(new ByteBufferEqualityComparer());
		}

		// Token: 0x060009BF RID: 2495 RVA: 0x0001FF60 File Offset: 0x0001E160
		public uint WriteMethodBody(MethodDefinition method)
		{
			uint num;
			if (CodeWriter.IsUnresolved(method))
			{
				if (method.rva == 0U)
				{
					return 0U;
				}
				num = this.WriteUnresolvedMethodBody(method);
			}
			else
			{
				if (CodeWriter.IsEmptyMethodBody(method.Body))
				{
					return 0U;
				}
				num = this.WriteResolvedMethodBody(method);
			}
			return num;
		}

		// Token: 0x060009C0 RID: 2496 RVA: 0x0001FFA1 File Offset: 0x0001E1A1
		private static bool IsEmptyMethodBody(MethodBody body)
		{
			return body.instructions.IsNullOrEmpty<Instruction>() && body.variables.IsNullOrEmpty<VariableDefinition>();
		}

		// Token: 0x060009C1 RID: 2497 RVA: 0x0001FFBD File Offset: 0x0001E1BD
		private static bool IsUnresolved(MethodDefinition method)
		{
			return method.HasBody && method.HasImage && method.body == null;
		}

		// Token: 0x060009C2 RID: 2498 RVA: 0x0001FFDC File Offset: 0x0001E1DC
		private uint WriteUnresolvedMethodBody(MethodDefinition method)
		{
			int num;
			MetadataToken metadataToken;
			ByteBuffer byteBuffer = this.metadata.module.reader.code.PatchRawMethodBody(method, this, out num, out metadataToken);
			bool flag = (byteBuffer.buffer[0] & 3) == 3;
			if (flag)
			{
				this.Align(4);
			}
			uint num2 = this.BeginMethod();
			if (flag || !this.GetOrMapTinyMethodBody(byteBuffer, ref num2))
			{
				base.WriteBytes(byteBuffer);
			}
			if (method.debug_info == null)
			{
				return num2;
			}
			ISymbolWriter symbol_writer = this.metadata.symbol_writer;
			if (symbol_writer != null)
			{
				method.debug_info.code_size = num;
				method.debug_info.local_var_token = metadataToken;
				symbol_writer.Write(method.debug_info);
			}
			return num2;
		}

		// Token: 0x060009C3 RID: 2499 RVA: 0x00020080 File Offset: 0x0001E280
		private uint WriteResolvedMethodBody(MethodDefinition method)
		{
			this.body = method.Body;
			this.ComputeHeader();
			uint num;
			if (this.RequiresFatHeader())
			{
				this.Align(4);
				num = this.BeginMethod();
				this.WriteFatHeader();
				this.WriteInstructions();
				if (this.body.HasExceptionHandlers)
				{
					this.WriteExceptionHandlers();
				}
			}
			else
			{
				num = this.BeginMethod();
				base.WriteByte((byte)(2 | (this.body.CodeSize << 2)));
				this.WriteInstructions();
				int num2 = (int)(num - this.code_base);
				int num3 = this.position - num2;
				byte[] array = new byte[num3];
				Array.Copy(this.buffer, num2, array, 0, num3);
				if (this.GetOrMapTinyMethodBody(new ByteBuffer(array), ref num))
				{
					this.position = num2;
				}
			}
			ISymbolWriter symbol_writer = this.metadata.symbol_writer;
			if (symbol_writer != null && method.debug_info != null)
			{
				method.debug_info.code_size = this.body.CodeSize;
				method.debug_info.local_var_token = this.body.local_var_token;
				symbol_writer.Write(method.debug_info);
			}
			return num;
		}

		// Token: 0x060009C4 RID: 2500 RVA: 0x0002018C File Offset: 0x0001E38C
		private bool GetOrMapTinyMethodBody(ByteBuffer body, ref uint rva)
		{
			uint num;
			if (this.tiny_method_bodies.TryGetValue(body, out num))
			{
				rva = num;
				return true;
			}
			this.tiny_method_bodies.Add(body, rva);
			return false;
		}

		// Token: 0x060009C5 RID: 2501 RVA: 0x000201C0 File Offset: 0x0001E3C0
		private void WriteFatHeader()
		{
			MethodBody methodBody = this.body;
			byte b = 3;
			if (methodBody.InitLocals)
			{
				b |= 16;
			}
			if (methodBody.HasExceptionHandlers)
			{
				b |= 8;
			}
			base.WriteByte(b);
			base.WriteByte(48);
			base.WriteInt16((short)methodBody.max_stack_size);
			base.WriteInt32(methodBody.code_size);
			methodBody.local_var_token = (methodBody.HasVariables ? this.GetStandAloneSignature(methodBody.Variables) : MetadataToken.Zero);
			this.WriteMetadataToken(methodBody.local_var_token);
		}

		// Token: 0x060009C6 RID: 2502 RVA: 0x00020248 File Offset: 0x0001E448
		private void WriteInstructions()
		{
			Collection<Instruction> instructions = this.body.Instructions;
			Instruction[] items = instructions.items;
			int size = instructions.size;
			for (int i = 0; i < size; i++)
			{
				Instruction instruction = items[i];
				this.WriteOpCode(instruction.opcode);
				this.WriteOperand(instruction);
			}
		}

		// Token: 0x060009C7 RID: 2503 RVA: 0x00020290 File Offset: 0x0001E490
		private void WriteOpCode(OpCode opcode)
		{
			if (opcode.Size == 1)
			{
				base.WriteByte(opcode.Op2);
				return;
			}
			base.WriteByte(opcode.Op1);
			base.WriteByte(opcode.Op2);
		}

		// Token: 0x060009C8 RID: 2504 RVA: 0x000202C4 File Offset: 0x0001E4C4
		private void WriteOperand(Instruction instruction)
		{
			OpCode opcode = instruction.opcode;
			OperandType operandType = opcode.OperandType;
			if (operandType == OperandType.InlineNone)
			{
				return;
			}
			object operand = instruction.operand;
			if (operand == null && operandType != OperandType.InlineBrTarget && operandType != OperandType.ShortInlineBrTarget)
			{
				throw new ArgumentException();
			}
			switch (operandType)
			{
			case OperandType.InlineBrTarget:
			{
				Instruction instruction2 = (Instruction)operand;
				int num = ((instruction2 != null) ? this.GetTargetOffset(instruction2) : this.body.code_size);
				base.WriteInt32(num - (instruction.Offset + opcode.Size + 4));
				return;
			}
			case OperandType.InlineField:
			case OperandType.InlineMethod:
			case OperandType.InlineTok:
			case OperandType.InlineType:
				this.WriteMetadataToken(this.metadata.LookupToken((IMetadataTokenProvider)operand));
				return;
			case OperandType.InlineI:
				base.WriteInt32((int)operand);
				return;
			case OperandType.InlineI8:
				base.WriteInt64((long)operand);
				return;
			case OperandType.InlineR:
				base.WriteDouble((double)operand);
				return;
			case OperandType.InlineSig:
				this.WriteMetadataToken(this.GetStandAloneSignature((CallSite)operand));
				return;
			case OperandType.InlineString:
				this.WriteMetadataToken(new MetadataToken(TokenType.String, this.GetUserStringIndex((string)operand)));
				return;
			case OperandType.InlineSwitch:
			{
				Instruction[] array = (Instruction[])operand;
				base.WriteInt32(array.Length);
				int num2 = instruction.Offset + opcode.Size + 4 * (array.Length + 1);
				for (int i = 0; i < array.Length; i++)
				{
					base.WriteInt32(this.GetTargetOffset(array[i]) - num2);
				}
				return;
			}
			case OperandType.InlineVar:
				base.WriteInt16((short)CodeWriter.GetVariableIndex((VariableDefinition)operand));
				return;
			case OperandType.InlineArg:
				base.WriteInt16((short)this.GetParameterIndex((ParameterDefinition)operand));
				return;
			case OperandType.ShortInlineBrTarget:
			{
				Instruction instruction3 = (Instruction)operand;
				int num3 = ((instruction3 != null) ? this.GetTargetOffset(instruction3) : this.body.code_size);
				base.WriteSByte((sbyte)(num3 - (instruction.Offset + opcode.Size + 1)));
				return;
			}
			case OperandType.ShortInlineI:
				if (opcode == OpCodes.Ldc_I4_S)
				{
					base.WriteSByte((sbyte)operand);
					return;
				}
				base.WriteByte((byte)operand);
				return;
			case OperandType.ShortInlineR:
				base.WriteSingle((float)operand);
				return;
			case OperandType.ShortInlineVar:
				base.WriteByte((byte)CodeWriter.GetVariableIndex((VariableDefinition)operand));
				return;
			case OperandType.ShortInlineArg:
				base.WriteByte((byte)this.GetParameterIndex((ParameterDefinition)operand));
				return;
			}
			throw new ArgumentException();
		}

		// Token: 0x060009C9 RID: 2505 RVA: 0x00020518 File Offset: 0x0001E718
		private int GetTargetOffset(Instruction instruction)
		{
			if (instruction == null)
			{
				Instruction instruction2 = this.body.instructions[this.body.instructions.size - 1];
				return instruction2.offset + instruction2.GetSize();
			}
			return instruction.offset;
		}

		// Token: 0x060009CA RID: 2506 RVA: 0x0002055F File Offset: 0x0001E75F
		private uint GetUserStringIndex(string @string)
		{
			if (@string == null)
			{
				return 0U;
			}
			return this.metadata.user_string_heap.GetStringIndex(@string);
		}

		// Token: 0x060009CB RID: 2507 RVA: 0x00020577 File Offset: 0x0001E777
		private static int GetVariableIndex(VariableDefinition variable)
		{
			return variable.Index;
		}

		// Token: 0x060009CC RID: 2508 RVA: 0x0002057F File Offset: 0x0001E77F
		private int GetParameterIndex(ParameterDefinition parameter)
		{
			if (!this.body.method.HasThis)
			{
				return parameter.Index;
			}
			if (parameter == this.body.this_parameter)
			{
				return 0;
			}
			return parameter.Index + 1;
		}

		// Token: 0x060009CD RID: 2509 RVA: 0x000205B4 File Offset: 0x0001E7B4
		private bool RequiresFatHeader()
		{
			MethodBody methodBody = this.body;
			return methodBody.CodeSize >= 64 || methodBody.InitLocals || methodBody.HasVariables || methodBody.HasExceptionHandlers || methodBody.MaxStackSize > 8;
		}

		// Token: 0x060009CE RID: 2510 RVA: 0x000205F8 File Offset: 0x0001E7F8
		private void ComputeHeader()
		{
			int num = 0;
			Collection<Instruction> instructions = this.body.instructions;
			Instruction[] items = instructions.items;
			int size = instructions.size;
			int num2 = 0;
			int num3 = 0;
			Dictionary<Instruction, int> dictionary = null;
			if (this.body.HasExceptionHandlers)
			{
				this.ComputeExceptionHandlerStackSize(ref dictionary);
			}
			for (int i = 0; i < size; i++)
			{
				Instruction instruction = items[i];
				instruction.offset = num;
				num += instruction.GetSize();
				CodeWriter.ComputeStackSize(instruction, ref dictionary, ref num2, ref num3);
			}
			this.body.code_size = num;
			this.body.max_stack_size = num3;
		}

		// Token: 0x060009CF RID: 2511 RVA: 0x0002068C File Offset: 0x0001E88C
		private void ComputeExceptionHandlerStackSize(ref Dictionary<Instruction, int> stack_sizes)
		{
			Collection<ExceptionHandler> exceptionHandlers = this.body.ExceptionHandlers;
			for (int i = 0; i < exceptionHandlers.Count; i++)
			{
				ExceptionHandler exceptionHandler = exceptionHandlers[i];
				ExceptionHandlerType handlerType = exceptionHandler.HandlerType;
				if (handlerType != ExceptionHandlerType.Catch)
				{
					if (handlerType == ExceptionHandlerType.Filter)
					{
						CodeWriter.AddExceptionStackSize(exceptionHandler.FilterStart, ref stack_sizes);
						CodeWriter.AddExceptionStackSize(exceptionHandler.HandlerStart, ref stack_sizes);
					}
				}
				else
				{
					CodeWriter.AddExceptionStackSize(exceptionHandler.HandlerStart, ref stack_sizes);
				}
			}
		}

		// Token: 0x060009D0 RID: 2512 RVA: 0x000206F4 File Offset: 0x0001E8F4
		private static void AddExceptionStackSize(Instruction handler_start, ref Dictionary<Instruction, int> stack_sizes)
		{
			if (handler_start == null)
			{
				return;
			}
			if (stack_sizes == null)
			{
				stack_sizes = new Dictionary<Instruction, int>();
			}
			stack_sizes[handler_start] = 1;
		}

		// Token: 0x060009D1 RID: 2513 RVA: 0x00020710 File Offset: 0x0001E910
		private static void ComputeStackSize(Instruction instruction, ref Dictionary<Instruction, int> stack_sizes, ref int stack_size, ref int max_stack)
		{
			int num;
			if (stack_sizes != null && stack_sizes.TryGetValue(instruction, out num))
			{
				stack_size = num;
			}
			max_stack = Math.Max(max_stack, stack_size);
			CodeWriter.ComputeStackDelta(instruction, ref stack_size);
			max_stack = Math.Max(max_stack, stack_size);
			CodeWriter.CopyBranchStackSize(instruction, ref stack_sizes, stack_size);
			CodeWriter.ComputeStackSize(instruction, ref stack_size);
		}

		// Token: 0x060009D2 RID: 2514 RVA: 0x00020760 File Offset: 0x0001E960
		private static void CopyBranchStackSize(Instruction instruction, ref Dictionary<Instruction, int> stack_sizes, int stack_size)
		{
			if (stack_size == 0)
			{
				return;
			}
			OperandType operandType = instruction.opcode.OperandType;
			if (operandType != OperandType.InlineBrTarget)
			{
				if (operandType != OperandType.InlineSwitch)
				{
					if (operandType == OperandType.ShortInlineBrTarget)
					{
						goto IL_001D;
					}
				}
				else
				{
					Instruction[] array = (Instruction[])instruction.operand;
					for (int i = 0; i < array.Length; i++)
					{
						CodeWriter.CopyBranchStackSize(ref stack_sizes, array[i], stack_size);
					}
				}
				return;
			}
			IL_001D:
			CodeWriter.CopyBranchStackSize(ref stack_sizes, (Instruction)instruction.operand, stack_size);
		}

		// Token: 0x060009D3 RID: 2515 RVA: 0x000207C4 File Offset: 0x0001E9C4
		private static void CopyBranchStackSize(ref Dictionary<Instruction, int> stack_sizes, Instruction target, int stack_size)
		{
			if (stack_sizes == null)
			{
				stack_sizes = new Dictionary<Instruction, int>();
			}
			int num = stack_size;
			int num2;
			if (stack_sizes.TryGetValue(target, out num2))
			{
				num = Math.Max(num, num2);
			}
			stack_sizes[target] = num;
		}

		// Token: 0x060009D4 RID: 2516 RVA: 0x000207FC File Offset: 0x0001E9FC
		private static void ComputeStackSize(Instruction instruction, ref int stack_size)
		{
			FlowControl flowControl = instruction.opcode.FlowControl;
			if (flowControl <= FlowControl.Break)
			{
				if (flowControl != FlowControl.Branch && flowControl != FlowControl.Break)
				{
					return;
				}
			}
			else if (flowControl != FlowControl.Return && flowControl != FlowControl.Throw)
			{
				return;
			}
			stack_size = 0;
		}

		// Token: 0x060009D5 RID: 2517 RVA: 0x0002082C File Offset: 0x0001EA2C
		private static void ComputeStackDelta(Instruction instruction, ref int stack_size)
		{
			FlowControl flowControl = instruction.opcode.FlowControl;
			if (flowControl == FlowControl.Call)
			{
				IMethodSignature methodSignature = (IMethodSignature)instruction.operand;
				if (methodSignature.HasImplicitThis() && instruction.opcode.Code != Code.Newobj)
				{
					stack_size--;
				}
				if (methodSignature.HasParameters)
				{
					stack_size -= methodSignature.Parameters.Count;
				}
				if (instruction.opcode.Code == Code.Calli)
				{
					stack_size--;
				}
				if (methodSignature.ReturnType.etype != ElementType.Void || instruction.opcode.Code == Code.Newobj)
				{
					stack_size++;
					return;
				}
			}
			else
			{
				CodeWriter.ComputePopDelta(instruction.opcode.StackBehaviourPop, ref stack_size);
				CodeWriter.ComputePushDelta(instruction.opcode.StackBehaviourPush, ref stack_size);
			}
		}

		// Token: 0x060009D6 RID: 2518 RVA: 0x000208E8 File Offset: 0x0001EAE8
		private static void ComputePopDelta(StackBehaviour pop_behavior, ref int stack_size)
		{
			switch (pop_behavior)
			{
			case StackBehaviour.Pop1:
			case StackBehaviour.Popi:
			case StackBehaviour.Popref:
				stack_size--;
				return;
			case StackBehaviour.Pop1_pop1:
			case StackBehaviour.Popi_pop1:
			case StackBehaviour.Popi_popi:
			case StackBehaviour.Popi_popi8:
			case StackBehaviour.Popi_popr4:
			case StackBehaviour.Popi_popr8:
			case StackBehaviour.Popref_pop1:
			case StackBehaviour.Popref_popi:
				stack_size -= 2;
				return;
			case StackBehaviour.Popi_popi_popi:
			case StackBehaviour.Popref_popi_popi:
			case StackBehaviour.Popref_popi_popi8:
			case StackBehaviour.Popref_popi_popr4:
			case StackBehaviour.Popref_popi_popr8:
			case StackBehaviour.Popref_popi_popref:
				stack_size -= 3;
				return;
			case StackBehaviour.PopAll:
				stack_size = 0;
				return;
			default:
				return;
			}
		}

		// Token: 0x060009D7 RID: 2519 RVA: 0x0002095E File Offset: 0x0001EB5E
		private static void ComputePushDelta(StackBehaviour push_behaviour, ref int stack_size)
		{
			switch (push_behaviour)
			{
			case StackBehaviour.Push1:
			case StackBehaviour.Pushi:
			case StackBehaviour.Pushi8:
			case StackBehaviour.Pushr4:
			case StackBehaviour.Pushr8:
			case StackBehaviour.Pushref:
				stack_size++;
				return;
			case StackBehaviour.Push1_push1:
				stack_size += 2;
				return;
			default:
				return;
			}
		}

		// Token: 0x060009D8 RID: 2520 RVA: 0x00020994 File Offset: 0x0001EB94
		private void WriteExceptionHandlers()
		{
			this.Align(4);
			Collection<ExceptionHandler> exceptionHandlers = this.body.ExceptionHandlers;
			if (exceptionHandlers.Count < 21 && !CodeWriter.RequiresFatSection(exceptionHandlers))
			{
				this.WriteSmallSection(exceptionHandlers);
				return;
			}
			this.WriteFatSection(exceptionHandlers);
		}

		// Token: 0x060009D9 RID: 2521 RVA: 0x000209D8 File Offset: 0x0001EBD8
		private static bool RequiresFatSection(Collection<ExceptionHandler> handlers)
		{
			for (int i = 0; i < handlers.Count; i++)
			{
				ExceptionHandler exceptionHandler = handlers[i];
				if (CodeWriter.IsFatRange(exceptionHandler.TryStart, exceptionHandler.TryEnd))
				{
					return true;
				}
				if (CodeWriter.IsFatRange(exceptionHandler.HandlerStart, exceptionHandler.HandlerEnd))
				{
					return true;
				}
				if (exceptionHandler.HandlerType == ExceptionHandlerType.Filter && CodeWriter.IsFatRange(exceptionHandler.FilterStart, exceptionHandler.HandlerStart))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060009DA RID: 2522 RVA: 0x00020A47 File Offset: 0x0001EC47
		private static bool IsFatRange(Instruction start, Instruction end)
		{
			if (start == null)
			{
				throw new ArgumentException();
			}
			return end == null || end.Offset - start.Offset > 255 || start.Offset > 65535;
		}

		// Token: 0x060009DB RID: 2523 RVA: 0x00020A7C File Offset: 0x0001EC7C
		private void WriteSmallSection(Collection<ExceptionHandler> handlers)
		{
			base.WriteByte(1);
			base.WriteByte((byte)(handlers.Count * 12 + 4));
			base.WriteBytes(2);
			this.WriteExceptionHandlers(handlers, delegate(int i)
			{
				base.WriteUInt16((ushort)i);
			}, delegate(int i)
			{
				base.WriteByte((byte)i);
			});
		}

		// Token: 0x060009DC RID: 2524 RVA: 0x00020AC8 File Offset: 0x0001ECC8
		private void WriteFatSection(Collection<ExceptionHandler> handlers)
		{
			base.WriteByte(65);
			int num = handlers.Count * 24 + 4;
			base.WriteByte((byte)(num & 255));
			base.WriteByte((byte)((num >> 8) & 255));
			base.WriteByte((byte)((num >> 16) & 255));
			this.WriteExceptionHandlers(handlers, new Action<int>(base.WriteInt32), new Action<int>(base.WriteInt32));
		}

		// Token: 0x060009DD RID: 2525 RVA: 0x00020B38 File Offset: 0x0001ED38
		private void WriteExceptionHandlers(Collection<ExceptionHandler> handlers, Action<int> write_entry, Action<int> write_length)
		{
			for (int i = 0; i < handlers.Count; i++)
			{
				ExceptionHandler exceptionHandler = handlers[i];
				write_entry((int)exceptionHandler.HandlerType);
				write_entry(exceptionHandler.TryStart.Offset);
				write_length(this.GetTargetOffset(exceptionHandler.TryEnd) - exceptionHandler.TryStart.Offset);
				write_entry(exceptionHandler.HandlerStart.Offset);
				write_length(this.GetTargetOffset(exceptionHandler.HandlerEnd) - exceptionHandler.HandlerStart.Offset);
				this.WriteExceptionHandlerSpecific(exceptionHandler);
			}
		}

		// Token: 0x060009DE RID: 2526 RVA: 0x00020BD4 File Offset: 0x0001EDD4
		private void WriteExceptionHandlerSpecific(ExceptionHandler handler)
		{
			ExceptionHandlerType handlerType = handler.HandlerType;
			if (handlerType == ExceptionHandlerType.Catch)
			{
				this.WriteMetadataToken(this.metadata.LookupToken(handler.CatchType));
				return;
			}
			if (handlerType != ExceptionHandlerType.Filter)
			{
				base.WriteInt32(0);
				return;
			}
			base.WriteInt32(handler.FilterStart.Offset);
		}

		// Token: 0x060009DF RID: 2527 RVA: 0x00020C24 File Offset: 0x0001EE24
		public MetadataToken GetStandAloneSignature(Collection<VariableDefinition> variables)
		{
			uint localVariableBlobIndex = this.metadata.GetLocalVariableBlobIndex(variables);
			return this.GetStandAloneSignatureToken(localVariableBlobIndex);
		}

		// Token: 0x060009E0 RID: 2528 RVA: 0x00020C48 File Offset: 0x0001EE48
		public MetadataToken GetStandAloneSignature(CallSite call_site)
		{
			uint callSiteBlobIndex = this.metadata.GetCallSiteBlobIndex(call_site);
			MetadataToken standAloneSignatureToken = this.GetStandAloneSignatureToken(callSiteBlobIndex);
			call_site.MetadataToken = standAloneSignatureToken;
			return standAloneSignatureToken;
		}

		// Token: 0x060009E1 RID: 2529 RVA: 0x00020C74 File Offset: 0x0001EE74
		private MetadataToken GetStandAloneSignatureToken(uint signature)
		{
			MetadataToken metadataToken;
			if (this.standalone_signatures.TryGetValue(signature, out metadataToken))
			{
				return metadataToken;
			}
			metadataToken = new MetadataToken(TokenType.Signature, this.metadata.AddStandAloneSignature(signature));
			this.standalone_signatures.Add(signature, metadataToken);
			return metadataToken;
		}

		// Token: 0x060009E2 RID: 2530 RVA: 0x00020CB9 File Offset: 0x0001EEB9
		private uint BeginMethod()
		{
			return (uint)((ulong)this.code_base + (ulong)((long)this.position));
		}

		// Token: 0x060009E3 RID: 2531 RVA: 0x00020CCB File Offset: 0x0001EECB
		private void WriteMetadataToken(MetadataToken token)
		{
			base.WriteUInt32(token.ToUInt32());
		}

		// Token: 0x060009E4 RID: 2532 RVA: 0x00020CDA File Offset: 0x0001EEDA
		private void Align(int align)
		{
			align--;
			base.WriteBytes(((this.position + align) & ~align) - this.position);
		}

		// Token: 0x0400050A RID: 1290
		private readonly uint code_base;

		// Token: 0x0400050B RID: 1291
		internal readonly MetadataBuilder metadata;

		// Token: 0x0400050C RID: 1292
		private readonly Dictionary<uint, MetadataToken> standalone_signatures;

		// Token: 0x0400050D RID: 1293
		private readonly Dictionary<ByteBuffer, uint> tiny_method_bodies;

		// Token: 0x0400050E RID: 1294
		private MethodBody body;
	}
}
