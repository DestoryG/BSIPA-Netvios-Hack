using System;
using Mono.Cecil.Cil;
using Mono.Collections.Generic;

namespace Mono.Cecil.Rocks
{
	// Token: 0x02000005 RID: 5
	public static class ILParser
	{
		// Token: 0x0600002A RID: 42 RVA: 0x00002718 File Offset: 0x00000918
		public static void Parse(MethodDefinition method, IILVisitor visitor)
		{
			if (method == null)
			{
				throw new ArgumentNullException("method");
			}
			if (visitor == null)
			{
				throw new ArgumentNullException("visitor");
			}
			if (!method.HasBody || !method.HasImage)
			{
				throw new ArgumentException();
			}
			method.Module.Read<MethodDefinition, bool>(method, delegate(MethodDefinition m, MetadataReader _)
			{
				ILParser.ParseMethod(m, visitor);
				return true;
			});
		}

		// Token: 0x0600002B RID: 43 RVA: 0x00002784 File Offset: 0x00000984
		private static void ParseMethod(MethodDefinition method, IILVisitor visitor)
		{
			ILParser.ParseContext parseContext = ILParser.CreateContext(method, visitor);
			CodeReader code = parseContext.Code;
			byte b = code.ReadByte();
			int num = (int)(b & 3);
			if (num != 2)
			{
				if (num != 3)
				{
					throw new NotSupportedException();
				}
				code.Advance(-1);
				ILParser.ParseFatMethod(parseContext);
			}
			else
			{
				ILParser.ParseCode(b >> 2, parseContext);
			}
			code.MoveBackTo(parseContext.Position);
		}

		// Token: 0x0600002C RID: 44 RVA: 0x000027E4 File Offset: 0x000009E4
		private static ILParser.ParseContext CreateContext(MethodDefinition method, IILVisitor visitor)
		{
			CodeReader codeReader = method.Module.Read<MethodDefinition, CodeReader>(method, (MethodDefinition _, MetadataReader reader) => reader.code);
			int num = codeReader.MoveTo(method);
			return new ILParser.ParseContext
			{
				Code = codeReader,
				Position = num,
				Metadata = codeReader.reader,
				Visitor = visitor
			};
		}

		// Token: 0x0600002D RID: 45 RVA: 0x0000284C File Offset: 0x00000A4C
		private static void ParseFatMethod(ILParser.ParseContext context)
		{
			CodeReader code = context.Code;
			code.Advance(4);
			int num = code.ReadInt32();
			MetadataToken metadataToken = code.ReadToken();
			if (metadataToken != MetadataToken.Zero)
			{
				context.Variables = code.ReadVariables(metadataToken);
			}
			ILParser.ParseCode(num, context);
		}

		// Token: 0x0600002E RID: 46 RVA: 0x00002894 File Offset: 0x00000A94
		private static void ParseCode(int code_size, ILParser.ParseContext context)
		{
			CodeReader code = context.Code;
			MetadataReader metadata = context.Metadata;
			IILVisitor visitor = context.Visitor;
			int num = code.Position + code_size;
			while (code.Position < num)
			{
				byte b = code.ReadByte();
				OpCode opCode = ((b != 254) ? OpCodes.OneByteOpCode[(int)b] : OpCodes.TwoBytesOpCode[(int)code.ReadByte()]);
				switch (opCode.OperandType)
				{
				case OperandType.InlineBrTarget:
					visitor.OnInlineBranch(opCode, code.ReadInt32());
					break;
				case OperandType.InlineField:
				case OperandType.InlineMethod:
				case OperandType.InlineTok:
				case OperandType.InlineType:
				{
					IMetadataTokenProvider metadataTokenProvider = metadata.LookupToken(code.ReadToken());
					TokenType tokenType = metadataTokenProvider.MetadataToken.TokenType;
					if (tokenType > TokenType.Field)
					{
						if (tokenType <= TokenType.MemberRef)
						{
							if (tokenType != TokenType.Method)
							{
								if (tokenType != TokenType.MemberRef)
								{
									break;
								}
								FieldReference fieldReference = metadataTokenProvider as FieldReference;
								if (fieldReference != null)
								{
									visitor.OnInlineField(opCode, fieldReference);
									break;
								}
								MethodReference methodReference = metadataTokenProvider as MethodReference;
								if (methodReference != null)
								{
									visitor.OnInlineMethod(opCode, methodReference);
									break;
								}
								throw new InvalidOperationException();
							}
						}
						else
						{
							if (tokenType == TokenType.TypeSpec)
							{
								goto IL_02B8;
							}
							if (tokenType != TokenType.MethodSpec)
							{
								break;
							}
						}
						visitor.OnInlineMethod(opCode, (MethodReference)metadataTokenProvider);
						break;
					}
					if (tokenType != TokenType.TypeRef && tokenType != TokenType.TypeDef)
					{
						if (tokenType != TokenType.Field)
						{
							break;
						}
						visitor.OnInlineField(opCode, (FieldReference)metadataTokenProvider);
						break;
					}
					IL_02B8:
					visitor.OnInlineType(opCode, (TypeReference)metadataTokenProvider);
					break;
				}
				case OperandType.InlineI:
					visitor.OnInlineInt32(opCode, code.ReadInt32());
					break;
				case OperandType.InlineI8:
					visitor.OnInlineInt64(opCode, code.ReadInt64());
					break;
				case OperandType.InlineNone:
					visitor.OnInlineNone(opCode);
					break;
				case OperandType.InlineR:
					visitor.OnInlineDouble(opCode, code.ReadDouble());
					break;
				case OperandType.InlineSig:
					visitor.OnInlineSignature(opCode, code.GetCallSite(code.ReadToken()));
					break;
				case OperandType.InlineString:
					visitor.OnInlineString(opCode, code.GetString(code.ReadToken()));
					break;
				case OperandType.InlineSwitch:
				{
					int num2 = code.ReadInt32();
					int[] array = new int[num2];
					for (int i = 0; i < num2; i++)
					{
						array[i] = code.ReadInt32();
					}
					visitor.OnInlineSwitch(opCode, array);
					break;
				}
				case OperandType.InlineVar:
					visitor.OnInlineVariable(opCode, ILParser.GetVariable(context, (int)code.ReadInt16()));
					break;
				case OperandType.InlineArg:
					visitor.OnInlineArgument(opCode, code.GetParameter((int)code.ReadInt16()));
					break;
				case OperandType.ShortInlineBrTarget:
					visitor.OnInlineBranch(opCode, (int)code.ReadSByte());
					break;
				case OperandType.ShortInlineI:
					if (opCode == OpCodes.Ldc_I4_S)
					{
						visitor.OnInlineSByte(opCode, code.ReadSByte());
					}
					else
					{
						visitor.OnInlineByte(opCode, code.ReadByte());
					}
					break;
				case OperandType.ShortInlineR:
					visitor.OnInlineSingle(opCode, code.ReadSingle());
					break;
				case OperandType.ShortInlineVar:
					visitor.OnInlineVariable(opCode, ILParser.GetVariable(context, (int)code.ReadByte()));
					break;
				case OperandType.ShortInlineArg:
					visitor.OnInlineArgument(opCode, code.GetParameter((int)code.ReadByte()));
					break;
				}
			}
		}

		// Token: 0x0600002F RID: 47 RVA: 0x00002BD0 File Offset: 0x00000DD0
		private static VariableDefinition GetVariable(ILParser.ParseContext context, int index)
		{
			return context.Variables[index];
		}

		// Token: 0x0200000F RID: 15
		private class ParseContext
		{
			// Token: 0x17000003 RID: 3
			// (get) Token: 0x06000060 RID: 96 RVA: 0x00004014 File Offset: 0x00002214
			// (set) Token: 0x06000061 RID: 97 RVA: 0x0000401C File Offset: 0x0000221C
			public CodeReader Code { get; set; }

			// Token: 0x17000004 RID: 4
			// (get) Token: 0x06000062 RID: 98 RVA: 0x00004025 File Offset: 0x00002225
			// (set) Token: 0x06000063 RID: 99 RVA: 0x0000402D File Offset: 0x0000222D
			public int Position { get; set; }

			// Token: 0x17000005 RID: 5
			// (get) Token: 0x06000064 RID: 100 RVA: 0x00004036 File Offset: 0x00002236
			// (set) Token: 0x06000065 RID: 101 RVA: 0x0000403E File Offset: 0x0000223E
			public MetadataReader Metadata { get; set; }

			// Token: 0x17000006 RID: 6
			// (get) Token: 0x06000066 RID: 102 RVA: 0x00004047 File Offset: 0x00002247
			// (set) Token: 0x06000067 RID: 103 RVA: 0x0000404F File Offset: 0x0000224F
			public Collection<VariableDefinition> Variables { get; set; }

			// Token: 0x17000007 RID: 7
			// (get) Token: 0x06000068 RID: 104 RVA: 0x00004058 File Offset: 0x00002258
			// (set) Token: 0x06000069 RID: 105 RVA: 0x00004060 File Offset: 0x00002260
			public IILVisitor Visitor { get; set; }
		}
	}
}
