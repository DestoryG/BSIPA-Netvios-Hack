using System;
using System.Collections.Generic;
using Mono.Cecil.Metadata;
using Mono.Cecil.PE;
using Mono.Collections.Generic;

namespace Mono.Cecil.Cil
{
	// Token: 0x020001B9 RID: 441
	internal sealed class CodeWriter : ByteBuffer
	{
		// Token: 0x06000DA2 RID: 3490 RVA: 0x0002F0BA File Offset: 0x0002D2BA
		public CodeWriter(MetadataBuilder metadata)
			: base(0)
		{
			this.code_base = metadata.text_map.GetNextRVA(TextSegment.CLIHeader);
			this.metadata = metadata;
			this.standalone_signatures = new Dictionary<uint, MetadataToken>();
			this.tiny_method_bodies = new Dictionary<ByteBuffer, uint>(new ByteBufferEqualityComparer());
		}

		// Token: 0x06000DA3 RID: 3491 RVA: 0x0002F0F8 File Offset: 0x0002D2F8
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

		// Token: 0x06000DA4 RID: 3492 RVA: 0x0002F139 File Offset: 0x0002D339
		private static bool IsEmptyMethodBody(MethodBody body)
		{
			return body.instructions.IsNullOrEmpty<Instruction>() && body.variables.IsNullOrEmpty<VariableDefinition>();
		}

		// Token: 0x06000DA5 RID: 3493 RVA: 0x0002F155 File Offset: 0x0002D355
		private static bool IsUnresolved(MethodDefinition method)
		{
			return method.HasBody && method.HasImage && method.body == null;
		}

		// Token: 0x06000DA6 RID: 3494 RVA: 0x0002F174 File Offset: 0x0002D374
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

		// Token: 0x06000DA7 RID: 3495 RVA: 0x0002F218 File Offset: 0x0002D418
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

		// Token: 0x06000DA8 RID: 3496 RVA: 0x0002F324 File Offset: 0x0002D524
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

		// Token: 0x06000DA9 RID: 3497 RVA: 0x0002F358 File Offset: 0x0002D558
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

		// Token: 0x06000DAA RID: 3498 RVA: 0x0002F3E0 File Offset: 0x0002D5E0
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

		// Token: 0x06000DAB RID: 3499 RVA: 0x0002F428 File Offset: 0x0002D628
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

		// Token: 0x06000DAC RID: 3500 RVA: 0x0002F45C File Offset: 0x0002D65C
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

		// Token: 0x06000DAD RID: 3501 RVA: 0x0002F6B0 File Offset: 0x0002D8B0
		private int GetTargetOffset(Instruction instruction)
		{
			if (instruction == null)
			{
				Instruction instruction2 = this.body.instructions[this.body.instructions.size - 1];
				return instruction2.offset + instruction2.GetSize();
			}
			return instruction.offset;
		}

		// Token: 0x06000DAE RID: 3502 RVA: 0x0002F6F7 File Offset: 0x0002D8F7
		private uint GetUserStringIndex(string @string)
		{
			if (@string == null)
			{
				return 0U;
			}
			return this.metadata.user_string_heap.GetStringIndex(@string);
		}

		// Token: 0x06000DAF RID: 3503 RVA: 0x0002F70F File Offset: 0x0002D90F
		private static int GetVariableIndex(VariableDefinition variable)
		{
			return variable.Index;
		}

		// Token: 0x06000DB0 RID: 3504 RVA: 0x0002F717 File Offset: 0x0002D917
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

		// Token: 0x06000DB1 RID: 3505 RVA: 0x0002F74C File Offset: 0x0002D94C
		private bool RequiresFatHeader()
		{
			MethodBody methodBody = this.body;
			return methodBody.CodeSize >= 64 || methodBody.InitLocals || methodBody.HasVariables || methodBody.HasExceptionHandlers || methodBody.MaxStackSize > 8;
		}

		// Token: 0x06000DB2 RID: 3506 RVA: 0x0002F790 File Offset: 0x0002D990
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

		// Token: 0x06000DB3 RID: 3507 RVA: 0x0002F824 File Offset: 0x0002DA24
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

		// Token: 0x06000DB4 RID: 3508 RVA: 0x0002F88C File Offset: 0x0002DA8C
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

		// Token: 0x06000DB5 RID: 3509 RVA: 0x0002F8A8 File Offset: 0x0002DAA8
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

		// Token: 0x06000DB6 RID: 3510 RVA: 0x0002F8F8 File Offset: 0x0002DAF8
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

		// Token: 0x06000DB7 RID: 3511 RVA: 0x0002F95C File Offset: 0x0002DB5C
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

		// Token: 0x06000DB8 RID: 3512 RVA: 0x0002F994 File Offset: 0x0002DB94
		private static void ComputeStackSize(Instruction instruction, ref int stack_size)
		{
			FlowControl flowControl = instruction.opcode.FlowControl;
			if (flowControl <= FlowControl.Break || flowControl - FlowControl.Return <= 1)
			{
				stack_size = 0;
			}
		}

		// Token: 0x06000DB9 RID: 3513 RVA: 0x0002F9BC File Offset: 0x0002DBBC
		private static void ComputeStackDelta(Instruction instruction, ref int stack_size)
		{
			if (instruction.opcode.FlowControl == FlowControl.Call)
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

		// Token: 0x06000DBA RID: 3514 RVA: 0x0002FA74 File Offset: 0x0002DC74
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

		// Token: 0x06000DBB RID: 3515 RVA: 0x0002FAEA File Offset: 0x0002DCEA
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

		// Token: 0x06000DBC RID: 3516 RVA: 0x0002FB20 File Offset: 0x0002DD20
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

		// Token: 0x06000DBD RID: 3517 RVA: 0x0002FB64 File Offset: 0x0002DD64
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

		// Token: 0x06000DBE RID: 3518 RVA: 0x0002FBD3 File Offset: 0x0002DDD3
		private static bool IsFatRange(Instruction start, Instruction end)
		{
			if (start == null)
			{
				throw new ArgumentException();
			}
			return end == null || end.Offset - start.Offset > 255 || start.Offset > 65535;
		}

		// Token: 0x06000DBF RID: 3519 RVA: 0x0002FC08 File Offset: 0x0002DE08
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

		// Token: 0x06000DC0 RID: 3520 RVA: 0x0002FC54 File Offset: 0x0002DE54
		private void WriteFatSection(Collection<ExceptionHandler> handlers)
		{
			base.WriteByte(65);
			int num = handlers.Count * 24 + 4;
			base.WriteByte((byte)(num & 255));
			base.WriteByte((byte)((num >> 8) & 255));
			base.WriteByte((byte)((num >> 16) & 255));
			this.WriteExceptionHandlers(handlers, new Action<int>(base.WriteInt32), new Action<int>(base.WriteInt32));
		}

		// Token: 0x06000DC1 RID: 3521 RVA: 0x0002FCC4 File Offset: 0x0002DEC4
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

		// Token: 0x06000DC2 RID: 3522 RVA: 0x0002FD60 File Offset: 0x0002DF60
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

		// Token: 0x06000DC3 RID: 3523 RVA: 0x0002FDB0 File Offset: 0x0002DFB0
		public MetadataToken GetStandAloneSignature(Collection<VariableDefinition> variables)
		{
			uint localVariableBlobIndex = this.metadata.GetLocalVariableBlobIndex(variables);
			return this.GetStandAloneSignatureToken(localVariableBlobIndex);
		}

		// Token: 0x06000DC4 RID: 3524 RVA: 0x0002FDD4 File Offset: 0x0002DFD4
		public MetadataToken GetStandAloneSignature(CallSite call_site)
		{
			uint callSiteBlobIndex = this.metadata.GetCallSiteBlobIndex(call_site);
			MetadataToken standAloneSignatureToken = this.GetStandAloneSignatureToken(callSiteBlobIndex);
			call_site.MetadataToken = standAloneSignatureToken;
			return standAloneSignatureToken;
		}

		// Token: 0x06000DC5 RID: 3525 RVA: 0x0002FE00 File Offset: 0x0002E000
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

		// Token: 0x06000DC6 RID: 3526 RVA: 0x0002FE45 File Offset: 0x0002E045
		private uint BeginMethod()
		{
			return (uint)((ulong)this.code_base + (ulong)((long)this.position));
		}

		// Token: 0x06000DC7 RID: 3527 RVA: 0x0002FE57 File Offset: 0x0002E057
		private void WriteMetadataToken(MetadataToken token)
		{
			base.WriteUInt32(token.ToUInt32());
		}

		// Token: 0x06000DC8 RID: 3528 RVA: 0x0002FE66 File Offset: 0x0002E066
		private void Align(int align)
		{
			align--;
			base.WriteBytes(((this.position + align) & ~align) - this.position);
		}

		// Token: 0x04000769 RID: 1897
		private readonly uint code_base;

		// Token: 0x0400076A RID: 1898
		internal readonly MetadataBuilder metadata;

		// Token: 0x0400076B RID: 1899
		private readonly Dictionary<uint, MetadataToken> standalone_signatures;

		// Token: 0x0400076C RID: 1900
		private readonly Dictionary<ByteBuffer, uint> tiny_method_bodies;

		// Token: 0x0400076D RID: 1901
		private MethodBody body;
	}
}
