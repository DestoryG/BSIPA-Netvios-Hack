using System;
using Mono.Cecil.PE;
using Mono.Collections.Generic;

namespace Mono.Cecil.Cil
{
	// Token: 0x020001B8 RID: 440
	internal sealed class CodeReader : BinaryStreamReader
	{
		// Token: 0x170002D5 RID: 725
		// (get) Token: 0x06000D78 RID: 3448 RVA: 0x0002E06F File Offset: 0x0002C26F
		private int Offset
		{
			get
			{
				return base.Position - this.start;
			}
		}

		// Token: 0x06000D79 RID: 3449 RVA: 0x0002E07E File Offset: 0x0002C27E
		public CodeReader(MetadataReader reader)
			: base(reader.image.Stream.value)
		{
			this.reader = reader;
		}

		// Token: 0x06000D7A RID: 3450 RVA: 0x0002E09D File Offset: 0x0002C29D
		public int MoveTo(MethodDefinition method)
		{
			this.method = method;
			this.reader.context = method;
			int position = base.Position;
			base.Position = (int)this.reader.image.ResolveVirtualAddress((uint)method.RVA);
			return position;
		}

		// Token: 0x06000D7B RID: 3451 RVA: 0x0002E0D4 File Offset: 0x0002C2D4
		public void MoveBackTo(int position)
		{
			this.reader.context = null;
			base.Position = position;
		}

		// Token: 0x06000D7C RID: 3452 RVA: 0x0002E0EC File Offset: 0x0002C2EC
		public MethodBody ReadMethodBody(MethodDefinition method)
		{
			int num = this.MoveTo(method);
			this.body = new MethodBody(method);
			this.ReadMethodBody();
			this.MoveBackTo(num);
			return this.body;
		}

		// Token: 0x06000D7D RID: 3453 RVA: 0x0002E120 File Offset: 0x0002C320
		public int ReadCodeSize(MethodDefinition method)
		{
			int num = this.MoveTo(method);
			int num2 = this.ReadCodeSize();
			this.MoveBackTo(num);
			return num2;
		}

		// Token: 0x06000D7E RID: 3454 RVA: 0x0002E144 File Offset: 0x0002C344
		private int ReadCodeSize()
		{
			byte b = this.ReadByte();
			int num = (int)(b & 3);
			if (num == 2)
			{
				return b >> 2;
			}
			if (num != 3)
			{
				throw new InvalidOperationException();
			}
			base.Advance(3);
			return (int)this.ReadUInt32();
		}

		// Token: 0x06000D7F RID: 3455 RVA: 0x0002E180 File Offset: 0x0002C380
		private void ReadMethodBody()
		{
			byte b = this.ReadByte();
			int num = (int)(b & 3);
			if (num != 2)
			{
				if (num != 3)
				{
					throw new InvalidOperationException();
				}
				base.Advance(-1);
				this.ReadFatMethod();
			}
			else
			{
				this.body.code_size = b >> 2;
				this.body.MaxStackSize = 8;
				this.ReadCode();
			}
			ISymbolReader symbol_reader = this.reader.module.symbol_reader;
			if (symbol_reader != null && this.method.debug_info == null)
			{
				this.method.debug_info = symbol_reader.Read(this.method);
			}
			if (this.method.debug_info != null)
			{
				this.ReadDebugInfo();
			}
		}

		// Token: 0x06000D80 RID: 3456 RVA: 0x0002E224 File Offset: 0x0002C424
		private void ReadFatMethod()
		{
			ushort num = this.ReadUInt16();
			this.body.max_stack_size = (int)this.ReadUInt16();
			this.body.code_size = (int)this.ReadUInt32();
			this.body.local_var_token = new MetadataToken(this.ReadUInt32());
			this.body.init_locals = (num & 16) > 0;
			if (this.body.local_var_token.RID != 0U)
			{
				this.body.variables = this.ReadVariables(this.body.local_var_token);
			}
			this.ReadCode();
			if ((num & 8) != 0)
			{
				this.ReadSection();
			}
		}

		// Token: 0x06000D81 RID: 3457 RVA: 0x0002E2C4 File Offset: 0x0002C4C4
		public VariableDefinitionCollection ReadVariables(MetadataToken local_var_token)
		{
			int position = this.reader.position;
			VariableDefinitionCollection variableDefinitionCollection = this.reader.ReadVariables(local_var_token);
			this.reader.position = position;
			return variableDefinitionCollection;
		}

		// Token: 0x06000D82 RID: 3458 RVA: 0x0002E2F8 File Offset: 0x0002C4F8
		private void ReadCode()
		{
			this.start = base.Position;
			int num = this.body.code_size;
			if (num < 0 || (long)base.Length <= (long)((ulong)(num + base.Position)))
			{
				num = 0;
			}
			int num2 = this.start + num;
			Collection<Instruction> collection = (this.body.instructions = new InstructionCollection(this.method, (num + 1) / 2));
			while (base.Position < num2)
			{
				int num3 = base.Position - this.start;
				OpCode opCode = this.ReadOpCode();
				Instruction instruction = new Instruction(num3, opCode);
				if (opCode.OperandType != OperandType.InlineNone)
				{
					instruction.operand = this.ReadOperand(instruction);
				}
				collection.Add(instruction);
			}
			this.ResolveBranches(collection);
		}

		// Token: 0x06000D83 RID: 3459 RVA: 0x0002E3B0 File Offset: 0x0002C5B0
		private OpCode ReadOpCode()
		{
			byte b = this.ReadByte();
			if (b == 254)
			{
				return OpCodes.TwoBytesOpCode[(int)this.ReadByte()];
			}
			return OpCodes.OneByteOpCode[(int)b];
		}

		// Token: 0x06000D84 RID: 3460 RVA: 0x0002E3E8 File Offset: 0x0002C5E8
		private object ReadOperand(Instruction instruction)
		{
			switch (instruction.opcode.OperandType)
			{
			case OperandType.InlineBrTarget:
				return this.ReadInt32() + this.Offset;
			case OperandType.InlineField:
			case OperandType.InlineMethod:
			case OperandType.InlineTok:
			case OperandType.InlineType:
				return this.reader.LookupToken(this.ReadToken());
			case OperandType.InlineI:
				return this.ReadInt32();
			case OperandType.InlineI8:
				return this.ReadInt64();
			case OperandType.InlineR:
				return this.ReadDouble();
			case OperandType.InlineSig:
				return this.GetCallSite(this.ReadToken());
			case OperandType.InlineString:
				return this.GetString(this.ReadToken());
			case OperandType.InlineSwitch:
			{
				int num = this.ReadInt32();
				int num2 = this.Offset + 4 * num;
				int[] array = new int[num];
				for (int i = 0; i < num; i++)
				{
					array[i] = num2 + this.ReadInt32();
				}
				return array;
			}
			case OperandType.InlineVar:
				return this.GetVariable((int)this.ReadUInt16());
			case OperandType.InlineArg:
				return this.GetParameter((int)this.ReadUInt16());
			case OperandType.ShortInlineBrTarget:
				return (int)this.ReadSByte() + this.Offset;
			case OperandType.ShortInlineI:
				if (instruction.opcode == OpCodes.Ldc_I4_S)
				{
					return this.ReadSByte();
				}
				return this.ReadByte();
			case OperandType.ShortInlineR:
				return this.ReadSingle();
			case OperandType.ShortInlineVar:
				return this.GetVariable((int)this.ReadByte());
			case OperandType.ShortInlineArg:
				return this.GetParameter((int)this.ReadByte());
			}
			throw new NotSupportedException();
		}

		// Token: 0x06000D85 RID: 3461 RVA: 0x0002E578 File Offset: 0x0002C778
		public string GetString(MetadataToken token)
		{
			return this.reader.image.UserStringHeap.Read(token.RID);
		}

		// Token: 0x06000D86 RID: 3462 RVA: 0x0002E596 File Offset: 0x0002C796
		public ParameterDefinition GetParameter(int index)
		{
			return this.body.GetParameter(index);
		}

		// Token: 0x06000D87 RID: 3463 RVA: 0x0002E5A4 File Offset: 0x0002C7A4
		public VariableDefinition GetVariable(int index)
		{
			return this.body.GetVariable(index);
		}

		// Token: 0x06000D88 RID: 3464 RVA: 0x0002E5B2 File Offset: 0x0002C7B2
		public CallSite GetCallSite(MetadataToken token)
		{
			return this.reader.ReadCallSite(token);
		}

		// Token: 0x06000D89 RID: 3465 RVA: 0x0002E5C0 File Offset: 0x0002C7C0
		private void ResolveBranches(Collection<Instruction> instructions)
		{
			Instruction[] items = instructions.items;
			int size = instructions.size;
			int i = 0;
			while (i < size)
			{
				Instruction instruction = items[i];
				OperandType operandType = instruction.opcode.OperandType;
				if (operandType == OperandType.InlineBrTarget)
				{
					goto IL_0036;
				}
				if (operandType != OperandType.InlineSwitch)
				{
					if (operandType == OperandType.ShortInlineBrTarget)
					{
						goto IL_0036;
					}
				}
				else
				{
					int[] array = (int[])instruction.operand;
					Instruction[] array2 = new Instruction[array.Length];
					for (int j = 0; j < array.Length; j++)
					{
						array2[j] = this.GetInstruction(array[j]);
					}
					instruction.operand = array2;
				}
				IL_0092:
				i++;
				continue;
				IL_0036:
				instruction.operand = this.GetInstruction((int)instruction.operand);
				goto IL_0092;
			}
		}

		// Token: 0x06000D8A RID: 3466 RVA: 0x0002E66A File Offset: 0x0002C86A
		private Instruction GetInstruction(int offset)
		{
			return CodeReader.GetInstruction(this.body.Instructions, offset);
		}

		// Token: 0x06000D8B RID: 3467 RVA: 0x0002E680 File Offset: 0x0002C880
		private static Instruction GetInstruction(Collection<Instruction> instructions, int offset)
		{
			int size = instructions.size;
			Instruction[] items = instructions.items;
			if (offset < 0 || offset > items[size - 1].offset)
			{
				return null;
			}
			int i = 0;
			int num = size - 1;
			while (i <= num)
			{
				int num2 = i + (num - i) / 2;
				Instruction instruction = items[num2];
				int offset2 = instruction.offset;
				if (offset == offset2)
				{
					return instruction;
				}
				if (offset < offset2)
				{
					num = num2 - 1;
				}
				else
				{
					i = num2 + 1;
				}
			}
			return null;
		}

		// Token: 0x06000D8C RID: 3468 RVA: 0x0002E6EC File Offset: 0x0002C8EC
		private void ReadSection()
		{
			base.Align(4);
			byte b = this.ReadByte();
			if ((b & 64) == 0)
			{
				this.ReadSmallSection();
			}
			else
			{
				this.ReadFatSection();
			}
			if ((b & 128) != 0)
			{
				this.ReadSection();
			}
		}

		// Token: 0x06000D8D RID: 3469 RVA: 0x0002E720 File Offset: 0x0002C920
		private void ReadSmallSection()
		{
			int num = (int)(this.ReadByte() / 12);
			base.Advance(2);
			this.ReadExceptionHandlers(num, () => (int)this.ReadUInt16(), () => (int)this.ReadByte());
		}

		// Token: 0x06000D8E RID: 3470 RVA: 0x0002E760 File Offset: 0x0002C960
		private void ReadFatSection()
		{
			base.Advance(-1);
			int num = (this.ReadInt32() >> 8) / 24;
			this.ReadExceptionHandlers(num, new Func<int>(this.ReadInt32), new Func<int>(this.ReadInt32));
		}

		// Token: 0x06000D8F RID: 3471 RVA: 0x0002E7A4 File Offset: 0x0002C9A4
		private void ReadExceptionHandlers(int count, Func<int> read_entry, Func<int> read_length)
		{
			for (int i = 0; i < count; i++)
			{
				ExceptionHandler exceptionHandler = new ExceptionHandler((ExceptionHandlerType)(read_entry() & 7));
				exceptionHandler.TryStart = this.GetInstruction(read_entry());
				exceptionHandler.TryEnd = this.GetInstruction(exceptionHandler.TryStart.Offset + read_length());
				exceptionHandler.HandlerStart = this.GetInstruction(read_entry());
				exceptionHandler.HandlerEnd = this.GetInstruction(exceptionHandler.HandlerStart.Offset + read_length());
				this.ReadExceptionHandlerSpecific(exceptionHandler);
				this.body.ExceptionHandlers.Add(exceptionHandler);
			}
		}

		// Token: 0x06000D90 RID: 3472 RVA: 0x0002E84C File Offset: 0x0002CA4C
		private void ReadExceptionHandlerSpecific(ExceptionHandler handler)
		{
			ExceptionHandlerType handlerType = handler.HandlerType;
			if (handlerType == ExceptionHandlerType.Catch)
			{
				handler.CatchType = (TypeReference)this.reader.LookupToken(this.ReadToken());
				return;
			}
			if (handlerType != ExceptionHandlerType.Filter)
			{
				base.Advance(4);
				return;
			}
			handler.FilterStart = this.GetInstruction(this.ReadInt32());
		}

		// Token: 0x06000D91 RID: 3473 RVA: 0x0002E8A0 File Offset: 0x0002CAA0
		public MetadataToken ReadToken()
		{
			return new MetadataToken(this.ReadUInt32());
		}

		// Token: 0x06000D92 RID: 3474 RVA: 0x0002E8B0 File Offset: 0x0002CAB0
		private void ReadDebugInfo()
		{
			if (this.method.debug_info.sequence_points != null)
			{
				this.ReadSequencePoints();
			}
			if (this.method.debug_info.scope != null)
			{
				this.ReadScope(this.method.debug_info.scope);
			}
			if (this.method.custom_infos != null)
			{
				this.ReadCustomDebugInformations(this.method);
			}
		}

		// Token: 0x06000D93 RID: 3475 RVA: 0x0002E918 File Offset: 0x0002CB18
		private void ReadCustomDebugInformations(MethodDefinition method)
		{
			Collection<CustomDebugInformation> custom_infos = method.custom_infos;
			for (int i = 0; i < custom_infos.Count; i++)
			{
				StateMachineScopeDebugInformation stateMachineScopeDebugInformation = custom_infos[i] as StateMachineScopeDebugInformation;
				if (stateMachineScopeDebugInformation != null)
				{
					this.ReadStateMachineScope(stateMachineScopeDebugInformation);
				}
				AsyncMethodBodyDebugInformation asyncMethodBodyDebugInformation = custom_infos[i] as AsyncMethodBodyDebugInformation;
				if (asyncMethodBodyDebugInformation != null)
				{
					this.ReadAsyncMethodBody(asyncMethodBodyDebugInformation);
				}
			}
		}

		// Token: 0x06000D94 RID: 3476 RVA: 0x0002E96C File Offset: 0x0002CB6C
		private void ReadAsyncMethodBody(AsyncMethodBodyDebugInformation async_method)
		{
			if (async_method.catch_handler.Offset > -1)
			{
				async_method.catch_handler = new InstructionOffset(this.GetInstruction(async_method.catch_handler.Offset));
			}
			if (!async_method.yields.IsNullOrEmpty<InstructionOffset>())
			{
				for (int i = 0; i < async_method.yields.Count; i++)
				{
					async_method.yields[i] = new InstructionOffset(this.GetInstruction(async_method.yields[i].Offset));
				}
			}
			if (!async_method.resumes.IsNullOrEmpty<InstructionOffset>())
			{
				for (int j = 0; j < async_method.resumes.Count; j++)
				{
					async_method.resumes[j] = new InstructionOffset(this.GetInstruction(async_method.resumes[j].Offset));
				}
			}
		}

		// Token: 0x06000D95 RID: 3477 RVA: 0x0002EA40 File Offset: 0x0002CC40
		private void ReadStateMachineScope(StateMachineScopeDebugInformation state_machine_scope)
		{
			if (state_machine_scope.scopes.IsNullOrEmpty<StateMachineScope>())
			{
				return;
			}
			foreach (StateMachineScope stateMachineScope in state_machine_scope.scopes)
			{
				stateMachineScope.start = new InstructionOffset(this.GetInstruction(stateMachineScope.start.Offset));
				Instruction instruction = this.GetInstruction(stateMachineScope.end.Offset);
				stateMachineScope.end = ((instruction == null) ? default(InstructionOffset) : new InstructionOffset(instruction));
			}
		}

		// Token: 0x06000D96 RID: 3478 RVA: 0x0002EAE4 File Offset: 0x0002CCE4
		private void ReadSequencePoints()
		{
			MethodDebugInformation debug_info = this.method.debug_info;
			for (int i = 0; i < debug_info.sequence_points.Count; i++)
			{
				SequencePoint sequencePoint = debug_info.sequence_points[i];
				Instruction instruction = this.GetInstruction(sequencePoint.Offset);
				if (instruction != null)
				{
					sequencePoint.offset = new InstructionOffset(instruction);
				}
			}
		}

		// Token: 0x06000D97 RID: 3479 RVA: 0x0002EB3C File Offset: 0x0002CD3C
		private void ReadScopes(Collection<ScopeDebugInformation> scopes)
		{
			for (int i = 0; i < scopes.Count; i++)
			{
				this.ReadScope(scopes[i]);
			}
		}

		// Token: 0x06000D98 RID: 3480 RVA: 0x0002EB68 File Offset: 0x0002CD68
		private void ReadScope(ScopeDebugInformation scope)
		{
			Instruction instruction = this.GetInstruction(scope.Start.Offset);
			if (instruction != null)
			{
				scope.Start = new InstructionOffset(instruction);
			}
			Instruction instruction2 = this.GetInstruction(scope.End.Offset);
			scope.End = ((instruction2 != null) ? new InstructionOffset(instruction2) : default(InstructionOffset));
			if (!scope.variables.IsNullOrEmpty<VariableDebugInformation>())
			{
				for (int i = 0; i < scope.variables.Count; i++)
				{
					VariableDebugInformation variableDebugInformation = scope.variables[i];
					VariableDefinition variable = this.GetVariable(variableDebugInformation.Index);
					if (variable != null)
					{
						variableDebugInformation.index = new VariableIndex(variable);
					}
				}
			}
			if (!scope.scopes.IsNullOrEmpty<ScopeDebugInformation>())
			{
				this.ReadScopes(scope.scopes);
			}
		}

		// Token: 0x06000D99 RID: 3481 RVA: 0x0002EC34 File Offset: 0x0002CE34
		public ByteBuffer PatchRawMethodBody(MethodDefinition method, CodeWriter writer, out int code_size, out MetadataToken local_var_token)
		{
			int num = this.MoveTo(method);
			ByteBuffer byteBuffer = new ByteBuffer();
			byte b = this.ReadByte();
			int num2 = (int)(b & 3);
			if (num2 != 2)
			{
				if (num2 != 3)
				{
					throw new NotSupportedException();
				}
				base.Advance(-1);
				this.PatchRawFatMethod(byteBuffer, writer, out code_size, out local_var_token);
			}
			else
			{
				byteBuffer.WriteByte(b);
				local_var_token = MetadataToken.Zero;
				code_size = b >> 2;
				this.PatchRawCode(byteBuffer, code_size, writer);
			}
			this.MoveBackTo(num);
			return byteBuffer;
		}

		// Token: 0x06000D9A RID: 3482 RVA: 0x0002ECAC File Offset: 0x0002CEAC
		private void PatchRawFatMethod(ByteBuffer buffer, CodeWriter writer, out int code_size, out MetadataToken local_var_token)
		{
			ushort num = this.ReadUInt16();
			buffer.WriteUInt16(num);
			buffer.WriteUInt16(this.ReadUInt16());
			code_size = this.ReadInt32();
			buffer.WriteInt32(code_size);
			local_var_token = this.ReadToken();
			if (local_var_token.RID > 0U)
			{
				VariableDefinitionCollection variableDefinitionCollection = this.ReadVariables(local_var_token);
				buffer.WriteUInt32((variableDefinitionCollection != null) ? writer.GetStandAloneSignature(variableDefinitionCollection).ToUInt32() : 0U);
			}
			else
			{
				buffer.WriteUInt32(0U);
			}
			this.PatchRawCode(buffer, code_size, writer);
			if ((num & 8) != 0)
			{
				this.PatchRawSection(buffer, writer.metadata);
			}
		}

		// Token: 0x06000D9B RID: 3483 RVA: 0x0002ED48 File Offset: 0x0002CF48
		private void PatchRawCode(ByteBuffer buffer, int code_size, CodeWriter writer)
		{
			MetadataBuilder metadata = writer.metadata;
			buffer.WriteBytes(this.ReadBytes(code_size));
			int position = buffer.position;
			buffer.position -= code_size;
			while (buffer.position < position)
			{
				byte b = buffer.ReadByte();
				OpCode opCode;
				if (b != 254)
				{
					opCode = OpCodes.OneByteOpCode[(int)b];
				}
				else
				{
					byte b2 = buffer.ReadByte();
					opCode = OpCodes.TwoBytesOpCode[(int)b2];
				}
				switch (opCode.OperandType)
				{
				case OperandType.InlineBrTarget:
				case OperandType.InlineI:
				case OperandType.ShortInlineR:
					buffer.position += 4;
					break;
				case OperandType.InlineField:
				case OperandType.InlineMethod:
				case OperandType.InlineTok:
				case OperandType.InlineType:
				{
					IMetadataTokenProvider metadataTokenProvider = this.reader.LookupToken(new MetadataToken(buffer.ReadUInt32()));
					buffer.position -= 4;
					buffer.WriteUInt32(metadata.LookupToken(metadataTokenProvider).ToUInt32());
					break;
				}
				case OperandType.InlineI8:
				case OperandType.InlineR:
					buffer.position += 8;
					break;
				case OperandType.InlineSig:
				{
					CallSite callSite = this.GetCallSite(new MetadataToken(buffer.ReadUInt32()));
					buffer.position -= 4;
					buffer.WriteUInt32(writer.GetStandAloneSignature(callSite).ToUInt32());
					break;
				}
				case OperandType.InlineString:
				{
					string @string = this.GetString(new MetadataToken(buffer.ReadUInt32()));
					buffer.position -= 4;
					buffer.WriteUInt32(new MetadataToken(TokenType.String, metadata.user_string_heap.GetStringIndex(@string)).ToUInt32());
					break;
				}
				case OperandType.InlineSwitch:
				{
					int num = buffer.ReadInt32();
					buffer.position += num * 4;
					break;
				}
				case OperandType.InlineVar:
				case OperandType.InlineArg:
					buffer.position += 2;
					break;
				case OperandType.ShortInlineBrTarget:
				case OperandType.ShortInlineI:
				case OperandType.ShortInlineVar:
				case OperandType.ShortInlineArg:
					buffer.position++;
					break;
				}
			}
		}

		// Token: 0x06000D9C RID: 3484 RVA: 0x0002EF50 File Offset: 0x0002D150
		private void PatchRawSection(ByteBuffer buffer, MetadataBuilder metadata)
		{
			int position = base.Position;
			base.Align(4);
			buffer.WriteBytes(base.Position - position);
			byte b = this.ReadByte();
			if ((b & 64) == 0)
			{
				buffer.WriteByte(b);
				this.PatchRawSmallSection(buffer, metadata);
			}
			else
			{
				this.PatchRawFatSection(buffer, metadata);
			}
			if ((b & 128) != 0)
			{
				this.PatchRawSection(buffer, metadata);
			}
		}

		// Token: 0x06000D9D RID: 3485 RVA: 0x0002EFB0 File Offset: 0x0002D1B0
		private void PatchRawSmallSection(ByteBuffer buffer, MetadataBuilder metadata)
		{
			byte b = this.ReadByte();
			buffer.WriteByte(b);
			base.Advance(2);
			buffer.WriteUInt16(0);
			int num = (int)(b / 12);
			this.PatchRawExceptionHandlers(buffer, metadata, num, false);
		}

		// Token: 0x06000D9E RID: 3486 RVA: 0x0002EFE8 File Offset: 0x0002D1E8
		private void PatchRawFatSection(ByteBuffer buffer, MetadataBuilder metadata)
		{
			base.Advance(-1);
			int num = this.ReadInt32();
			buffer.WriteInt32(num);
			int num2 = (num >> 8) / 24;
			this.PatchRawExceptionHandlers(buffer, metadata, num2, true);
		}

		// Token: 0x06000D9F RID: 3487 RVA: 0x0002F01C File Offset: 0x0002D21C
		private void PatchRawExceptionHandlers(ByteBuffer buffer, MetadataBuilder metadata, int count, bool fat_entry)
		{
			for (int i = 0; i < count; i++)
			{
				ExceptionHandlerType exceptionHandlerType;
				if (fat_entry)
				{
					uint num = this.ReadUInt32();
					exceptionHandlerType = (ExceptionHandlerType)(num & 7U);
					buffer.WriteUInt32(num);
				}
				else
				{
					ushort num2 = this.ReadUInt16();
					exceptionHandlerType = (ExceptionHandlerType)(num2 & 7);
					buffer.WriteUInt16(num2);
				}
				buffer.WriteBytes(this.ReadBytes(fat_entry ? 16 : 6));
				if (exceptionHandlerType == ExceptionHandlerType.Catch)
				{
					IMetadataTokenProvider metadataTokenProvider = this.reader.LookupToken(this.ReadToken());
					buffer.WriteUInt32(metadata.LookupToken(metadataTokenProvider).ToUInt32());
				}
				else
				{
					buffer.WriteUInt32(this.ReadUInt32());
				}
			}
		}

		// Token: 0x04000765 RID: 1893
		internal readonly MetadataReader reader;

		// Token: 0x04000766 RID: 1894
		private int start;

		// Token: 0x04000767 RID: 1895
		private MethodDefinition method;

		// Token: 0x04000768 RID: 1896
		private MethodBody body;
	}
}
