using System;
using Mono.Cecil.PE;
using Mono.Collections.Generic;

namespace Mono.Cecil.Cil
{
	// Token: 0x020000F4 RID: 244
	internal sealed class CodeReader : BinaryStreamReader
	{
		// Token: 0x170002B5 RID: 693
		// (get) Token: 0x06000994 RID: 2452 RVA: 0x0001EED7 File Offset: 0x0001D0D7
		private int Offset
		{
			get
			{
				return base.Position - this.start;
			}
		}

		// Token: 0x06000995 RID: 2453 RVA: 0x0001EEE6 File Offset: 0x0001D0E6
		public CodeReader(MetadataReader reader)
			: base(reader.image.Stream.value)
		{
			this.reader = reader;
		}

		// Token: 0x06000996 RID: 2454 RVA: 0x0001EF05 File Offset: 0x0001D105
		public int MoveTo(MethodDefinition method)
		{
			this.method = method;
			this.reader.context = method;
			int position = base.Position;
			base.Position = (int)this.reader.image.ResolveVirtualAddress((uint)method.RVA);
			return position;
		}

		// Token: 0x06000997 RID: 2455 RVA: 0x0001EF3C File Offset: 0x0001D13C
		public void MoveBackTo(int position)
		{
			this.reader.context = null;
			base.Position = position;
		}

		// Token: 0x06000998 RID: 2456 RVA: 0x0001EF54 File Offset: 0x0001D154
		public MethodBody ReadMethodBody(MethodDefinition method)
		{
			int num = this.MoveTo(method);
			this.body = new MethodBody(method);
			this.ReadMethodBody();
			this.MoveBackTo(num);
			return this.body;
		}

		// Token: 0x06000999 RID: 2457 RVA: 0x0001EF88 File Offset: 0x0001D188
		public int ReadCodeSize(MethodDefinition method)
		{
			int num = this.MoveTo(method);
			int num2 = this.ReadCodeSize();
			this.MoveBackTo(num);
			return num2;
		}

		// Token: 0x0600099A RID: 2458 RVA: 0x0001EFAC File Offset: 0x0001D1AC
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

		// Token: 0x0600099B RID: 2459 RVA: 0x0001EFE8 File Offset: 0x0001D1E8
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

		// Token: 0x0600099C RID: 2460 RVA: 0x0001F08C File Offset: 0x0001D28C
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

		// Token: 0x0600099D RID: 2461 RVA: 0x0001F12C File Offset: 0x0001D32C
		public VariableDefinitionCollection ReadVariables(MetadataToken local_var_token)
		{
			int position = this.reader.position;
			VariableDefinitionCollection variableDefinitionCollection = this.reader.ReadVariables(local_var_token);
			this.reader.position = position;
			return variableDefinitionCollection;
		}

		// Token: 0x0600099E RID: 2462 RVA: 0x0001F160 File Offset: 0x0001D360
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

		// Token: 0x0600099F RID: 2463 RVA: 0x0001F218 File Offset: 0x0001D418
		private OpCode ReadOpCode()
		{
			byte b = this.ReadByte();
			if (b == 254)
			{
				return OpCodes.TwoBytesOpCode[(int)this.ReadByte()];
			}
			return OpCodes.OneByteOpCode[(int)b];
		}

		// Token: 0x060009A0 RID: 2464 RVA: 0x0001F250 File Offset: 0x0001D450
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

		// Token: 0x060009A1 RID: 2465 RVA: 0x0001F3E0 File Offset: 0x0001D5E0
		public string GetString(MetadataToken token)
		{
			return this.reader.image.UserStringHeap.Read(token.RID);
		}

		// Token: 0x060009A2 RID: 2466 RVA: 0x0001F3FE File Offset: 0x0001D5FE
		public ParameterDefinition GetParameter(int index)
		{
			return this.body.GetParameter(index);
		}

		// Token: 0x060009A3 RID: 2467 RVA: 0x0001F40C File Offset: 0x0001D60C
		public VariableDefinition GetVariable(int index)
		{
			return this.body.GetVariable(index);
		}

		// Token: 0x060009A4 RID: 2468 RVA: 0x0001F41A File Offset: 0x0001D61A
		public CallSite GetCallSite(MetadataToken token)
		{
			return this.reader.ReadCallSite(token);
		}

		// Token: 0x060009A5 RID: 2469 RVA: 0x0001F428 File Offset: 0x0001D628
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

		// Token: 0x060009A6 RID: 2470 RVA: 0x0001F4D2 File Offset: 0x0001D6D2
		private Instruction GetInstruction(int offset)
		{
			return CodeReader.GetInstruction(this.body.Instructions, offset);
		}

		// Token: 0x060009A7 RID: 2471 RVA: 0x0001F4E8 File Offset: 0x0001D6E8
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

		// Token: 0x060009A8 RID: 2472 RVA: 0x0001F554 File Offset: 0x0001D754
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

		// Token: 0x060009A9 RID: 2473 RVA: 0x0001F588 File Offset: 0x0001D788
		private void ReadSmallSection()
		{
			int num = (int)(this.ReadByte() / 12);
			base.Advance(2);
			this.ReadExceptionHandlers(num, () => (int)this.ReadUInt16(), () => (int)this.ReadByte());
		}

		// Token: 0x060009AA RID: 2474 RVA: 0x0001F5C8 File Offset: 0x0001D7C8
		private void ReadFatSection()
		{
			base.Advance(-1);
			int num = (this.ReadInt32() >> 8) / 24;
			this.ReadExceptionHandlers(num, new Func<int>(this.ReadInt32), new Func<int>(this.ReadInt32));
		}

		// Token: 0x060009AB RID: 2475 RVA: 0x0001F60C File Offset: 0x0001D80C
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

		// Token: 0x060009AC RID: 2476 RVA: 0x0001F6B4 File Offset: 0x0001D8B4
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

		// Token: 0x060009AD RID: 2477 RVA: 0x0001F708 File Offset: 0x0001D908
		public MetadataToken ReadToken()
		{
			return new MetadataToken(this.ReadUInt32());
		}

		// Token: 0x060009AE RID: 2478 RVA: 0x0001F718 File Offset: 0x0001D918
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

		// Token: 0x060009AF RID: 2479 RVA: 0x0001F780 File Offset: 0x0001D980
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

		// Token: 0x060009B0 RID: 2480 RVA: 0x0001F7D4 File Offset: 0x0001D9D4
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

		// Token: 0x060009B1 RID: 2481 RVA: 0x0001F8A8 File Offset: 0x0001DAA8
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

		// Token: 0x060009B2 RID: 2482 RVA: 0x0001F94C File Offset: 0x0001DB4C
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

		// Token: 0x060009B3 RID: 2483 RVA: 0x0001F9A4 File Offset: 0x0001DBA4
		private void ReadScopes(Collection<ScopeDebugInformation> scopes)
		{
			for (int i = 0; i < scopes.Count; i++)
			{
				this.ReadScope(scopes[i]);
			}
		}

		// Token: 0x060009B4 RID: 2484 RVA: 0x0001F9D0 File Offset: 0x0001DBD0
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

		// Token: 0x060009B5 RID: 2485 RVA: 0x0001FA9C File Offset: 0x0001DC9C
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

		// Token: 0x060009B6 RID: 2486 RVA: 0x0001FB14 File Offset: 0x0001DD14
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

		// Token: 0x060009B7 RID: 2487 RVA: 0x0001FBB0 File Offset: 0x0001DDB0
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

		// Token: 0x060009B8 RID: 2488 RVA: 0x0001FDB8 File Offset: 0x0001DFB8
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

		// Token: 0x060009B9 RID: 2489 RVA: 0x0001FE18 File Offset: 0x0001E018
		private void PatchRawSmallSection(ByteBuffer buffer, MetadataBuilder metadata)
		{
			byte b = this.ReadByte();
			buffer.WriteByte(b);
			base.Advance(2);
			buffer.WriteUInt16(0);
			int num = (int)(b / 12);
			this.PatchRawExceptionHandlers(buffer, metadata, num, false);
		}

		// Token: 0x060009BA RID: 2490 RVA: 0x0001FE50 File Offset: 0x0001E050
		private void PatchRawFatSection(ByteBuffer buffer, MetadataBuilder metadata)
		{
			base.Advance(-1);
			int num = this.ReadInt32();
			buffer.WriteInt32(num);
			int num2 = (num >> 8) / 24;
			this.PatchRawExceptionHandlers(buffer, metadata, num2, true);
		}

		// Token: 0x060009BB RID: 2491 RVA: 0x0001FE84 File Offset: 0x0001E084
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

		// Token: 0x04000506 RID: 1286
		internal readonly MetadataReader reader;

		// Token: 0x04000507 RID: 1287
		private int start;

		// Token: 0x04000508 RID: 1288
		private MethodDefinition method;

		// Token: 0x04000509 RID: 1289
		private MethodBody body;
	}
}
