using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
using System.Security;
using Mono.Cecil;
using Mono.Cecil.Cil;
using MonoMod.Utils.Cil;

namespace MonoMod.Utils
{
	// Token: 0x02000326 RID: 806
	internal sealed class DynamicMethodDefinition : IDisposable
	{
		// Token: 0x0600125D RID: 4701 RVA: 0x0003F14C File Offset: 0x0003D34C
		private static void _InitCopier()
		{
			DynamicMethodDefinition._CecilOpCodes1X = new Mono.Cecil.Cil.OpCode[225];
			DynamicMethodDefinition._CecilOpCodes2X = new Mono.Cecil.Cil.OpCode[31];
			FieldInfo[] fields = typeof(Mono.Cecil.Cil.OpCodes).GetFields(BindingFlags.Static | BindingFlags.Public);
			for (int i = 0; i < fields.Length; i++)
			{
				Mono.Cecil.Cil.OpCode opCode = (Mono.Cecil.Cil.OpCode)fields[i].GetValue(null);
				if (opCode.OpCodeType != Mono.Cecil.Cil.OpCodeType.Nternal)
				{
					if (opCode.Size == 1)
					{
						DynamicMethodDefinition._CecilOpCodes1X[(int)opCode.Value] = opCode;
					}
					else
					{
						DynamicMethodDefinition._CecilOpCodes2X[(int)(opCode.Value & 255)] = opCode;
					}
				}
			}
		}

		// Token: 0x0600125E RID: 4702 RVA: 0x0003F1E4 File Offset: 0x0003D3E4
		private void _CopyMethodToDefinition()
		{
			MethodBase originalMethod = this.OriginalMethod;
			DynamicMethodDefinition.<>c__DisplayClass3_0 CS$<>8__locals1;
			CS$<>8__locals1.moduleFrom = originalMethod.Module;
			global::System.Reflection.MethodBody methodBody = originalMethod.GetMethodBody();
			byte[] array = ((methodBody != null) ? methodBody.GetILAsByteArray() : null);
			if (array == null)
			{
				throw new NotSupportedException("Body-less method");
			}
			CS$<>8__locals1.def = this.Definition;
			CS$<>8__locals1.moduleTo = CS$<>8__locals1.def.Module;
			CS$<>8__locals1.bodyTo = CS$<>8__locals1.def.Body;
			CS$<>8__locals1.bodyTo.GetILProcessor();
			CS$<>8__locals1.typeArguments = null;
			if (originalMethod.DeclaringType.IsGenericType)
			{
				CS$<>8__locals1.typeArguments = originalMethod.DeclaringType.GetGenericArguments();
			}
			CS$<>8__locals1.methodArguments = null;
			if (originalMethod.IsGenericMethod)
			{
				CS$<>8__locals1.methodArguments = originalMethod.GetGenericArguments();
			}
			foreach (LocalVariableInfo localVariableInfo in methodBody.LocalVariables)
			{
				TypeReference typeReference = CS$<>8__locals1.moduleTo.ImportReference(localVariableInfo.LocalType);
				if (localVariableInfo.IsPinned)
				{
					typeReference = new PinnedType(typeReference);
				}
				CS$<>8__locals1.bodyTo.Variables.Add(new VariableDefinition(typeReference));
			}
			using (BinaryReader binaryReader = new BinaryReader(new MemoryStream(array)))
			{
				while (binaryReader.BaseStream.Position < binaryReader.BaseStream.Length)
				{
					int num = (int)binaryReader.BaseStream.Position;
					Instruction instruction = Instruction.Create(Mono.Cecil.Cil.OpCodes.Nop);
					byte b = binaryReader.ReadByte();
					instruction.OpCode = ((b != 254) ? DynamicMethodDefinition._CecilOpCodes1X[(int)b] : DynamicMethodDefinition._CecilOpCodes2X[(int)binaryReader.ReadByte()]);
					instruction.Offset = num;
					DynamicMethodDefinition.<_CopyMethodToDefinition>g__ReadOperand|3_0(binaryReader, instruction, ref CS$<>8__locals1);
					CS$<>8__locals1.bodyTo.Instructions.Add(instruction);
				}
			}
			foreach (Instruction instruction2 in CS$<>8__locals1.bodyTo.Instructions)
			{
				Mono.Cecil.Cil.OperandType operandType = instruction2.OpCode.OperandType;
				if (operandType != Mono.Cecil.Cil.OperandType.InlineBrTarget)
				{
					if (operandType == Mono.Cecil.Cil.OperandType.InlineSwitch)
					{
						int[] array2 = (int[])instruction2.Operand;
						Instruction[] array3 = new Instruction[array2.Length];
						for (int i = 0; i < array2.Length; i++)
						{
							array3[i] = DynamicMethodDefinition.<_CopyMethodToDefinition>g__GetInstruction|3_1(array2[i], ref CS$<>8__locals1);
						}
						instruction2.Operand = array3;
						continue;
					}
					if (operandType != Mono.Cecil.Cil.OperandType.ShortInlineBrTarget)
					{
						continue;
					}
				}
				instruction2.Operand = DynamicMethodDefinition.<_CopyMethodToDefinition>g__GetInstruction|3_1((int)instruction2.Operand, ref CS$<>8__locals1);
			}
			foreach (ExceptionHandlingClause exceptionHandlingClause in methodBody.ExceptionHandlingClauses)
			{
				Mono.Cecil.Cil.ExceptionHandler exceptionHandler = new Mono.Cecil.Cil.ExceptionHandler((ExceptionHandlerType)exceptionHandlingClause.Flags);
				CS$<>8__locals1.bodyTo.ExceptionHandlers.Add(exceptionHandler);
				exceptionHandler.TryStart = DynamicMethodDefinition.<_CopyMethodToDefinition>g__GetInstruction|3_1(exceptionHandlingClause.TryOffset, ref CS$<>8__locals1);
				exceptionHandler.TryEnd = DynamicMethodDefinition.<_CopyMethodToDefinition>g__GetInstruction|3_1(exceptionHandlingClause.TryOffset + exceptionHandlingClause.TryLength, ref CS$<>8__locals1);
				exceptionHandler.FilterStart = ((exceptionHandler.HandlerType != ExceptionHandlerType.Filter) ? null : DynamicMethodDefinition.<_CopyMethodToDefinition>g__GetInstruction|3_1(exceptionHandlingClause.FilterOffset, ref CS$<>8__locals1));
				exceptionHandler.HandlerStart = DynamicMethodDefinition.<_CopyMethodToDefinition>g__GetInstruction|3_1(exceptionHandlingClause.HandlerOffset, ref CS$<>8__locals1);
				exceptionHandler.HandlerEnd = DynamicMethodDefinition.<_CopyMethodToDefinition>g__GetInstruction|3_1(exceptionHandlingClause.HandlerOffset + exceptionHandlingClause.HandlerLength, ref CS$<>8__locals1);
				exceptionHandler.CatchType = ((exceptionHandler.HandlerType != ExceptionHandlerType.Catch) ? null : ((exceptionHandlingClause.CatchType == null) ? null : CS$<>8__locals1.moduleTo.ImportReference(exceptionHandlingClause.CatchType)));
			}
		}

		// Token: 0x0600125F RID: 4703 RVA: 0x0003F5D0 File Offset: 0x0003D7D0
		static DynamicMethodDefinition()
		{
			bool flag;
			if (!DynamicMethodDefinition._IsMono || DynamicMethodDefinition._IsNewMonoSRE || DynamicMethodDefinition._IsOldMonoSRE)
			{
				if (!DynamicMethodDefinition._IsMono)
				{
					Type type = typeof(ILGenerator).Assembly.GetType("System.Reflection.Emit.DynamicILGenerator");
					flag = ((type != null) ? type.GetField("m_scope", BindingFlags.Instance | BindingFlags.NonPublic) : null) == null;
				}
				else
				{
					flag = false;
				}
			}
			else
			{
				flag = true;
			}
			DynamicMethodDefinition._PreferCecil = flag;
			DynamicMethodDefinition.c_DebuggableAttribute = typeof(DebuggableAttribute).GetConstructor(new Type[] { typeof(DebuggableAttribute.DebuggingModes) });
			DynamicMethodDefinition.c_UnverifiableCodeAttribute = typeof(UnverifiableCodeAttribute).GetConstructor(new Type[0]);
			DynamicMethodDefinition.c_IgnoresAccessChecksToAttribute = typeof(IgnoresAccessChecksToAttribute).GetConstructor(new Type[] { typeof(string) });
			DynamicMethodDefinition.t__IDMDGenerator = typeof(_IDMDGenerator);
			DynamicMethodDefinition._DMDGeneratorCache = new Dictionary<string, _IDMDGenerator>();
			DynamicMethodDefinition._InitCopier();
		}

		// Token: 0x1700038E RID: 910
		// (get) Token: 0x06001260 RID: 4704 RVA: 0x0003F72E File Offset: 0x0003D92E
		[Obsolete("Use OriginalMethod instead.")]
		public MethodBase Method
		{
			get
			{
				return this.OriginalMethod;
			}
		}

		// Token: 0x1700038F RID: 911
		// (get) Token: 0x06001261 RID: 4705 RVA: 0x0003F736 File Offset: 0x0003D936
		// (set) Token: 0x06001262 RID: 4706 RVA: 0x0003F73E File Offset: 0x0003D93E
		public MethodBase OriginalMethod { get; private set; }

		// Token: 0x17000390 RID: 912
		// (get) Token: 0x06001263 RID: 4707 RVA: 0x0003F747 File Offset: 0x0003D947
		public MethodDefinition Definition
		{
			get
			{
				return this._Definition;
			}
		}

		// Token: 0x17000391 RID: 913
		// (get) Token: 0x06001264 RID: 4708 RVA: 0x0003F74F File Offset: 0x0003D94F
		public ModuleDefinition Module
		{
			get
			{
				return this._Module;
			}
		}

		// Token: 0x06001265 RID: 4709 RVA: 0x0003F757 File Offset: 0x0003D957
		internal DynamicMethodDefinition()
		{
			this.Debug = Environment.GetEnvironmentVariable("MONOMOD_DMD_DEBUG") == "1";
		}

		// Token: 0x06001266 RID: 4710 RVA: 0x0003F784 File Offset: 0x0003D984
		public DynamicMethodDefinition(MethodBase method)
			: this()
		{
			if (method == null)
			{
				throw new ArgumentNullException("method");
			}
			this.OriginalMethod = method;
			this.Reload();
		}

		// Token: 0x06001267 RID: 4711 RVA: 0x0003F7A8 File Offset: 0x0003D9A8
		public DynamicMethodDefinition(string name, Type returnType, Type[] parameterTypes)
			: this()
		{
			this.OriginalMethod = null;
			this._CreateDynModule(name, returnType, parameterTypes);
		}

		// Token: 0x06001268 RID: 4712 RVA: 0x0003F7C1 File Offset: 0x0003D9C1
		public ILProcessor GetILProcessor()
		{
			return this.Definition.Body.GetILProcessor();
		}

		// Token: 0x06001269 RID: 4713 RVA: 0x0003F7D3 File Offset: 0x0003D9D3
		public ILGenerator GetILGenerator()
		{
			return new CecilILGenerator(this.Definition.Body.GetILProcessor()).GetProxy();
		}

		// Token: 0x0600126A RID: 4714 RVA: 0x0003F7F0 File Offset: 0x0003D9F0
		private ModuleDefinition _CreateDynModule(string name, Type returnType, Type[] parameterTypes)
		{
			ModuleDefinition moduleDefinition = (this._Module = ModuleDefinition.CreateModule(string.Format("DMD:DynModule<{0}>?{1}", name, this.GetHashCode()), new ModuleParameters
			{
				Kind = ModuleKind.Dll,
				ReflectionImporterProvider = MMReflectionImporter.ProviderNoDefault
			}));
			TypeDefinition typeDefinition = new TypeDefinition("", string.Format("DMD<{0}>?{1}", name, this.GetHashCode()), Mono.Cecil.TypeAttributes.Public);
			moduleDefinition.Types.Add(typeDefinition);
			MethodDefinition methodDefinition = (this._Definition = new MethodDefinition(name, Mono.Cecil.MethodAttributes.FamANDAssem | Mono.Cecil.MethodAttributes.Family | Mono.Cecil.MethodAttributes.Static | Mono.Cecil.MethodAttributes.HideBySig, (returnType != null) ? moduleDefinition.ImportReference(returnType) : moduleDefinition.TypeSystem.Void));
			foreach (Type type in parameterTypes)
			{
				methodDefinition.Parameters.Add(new ParameterDefinition(moduleDefinition.ImportReference(type)));
			}
			typeDefinition.Methods.Add(methodDefinition);
			return moduleDefinition;
		}

		// Token: 0x0600126B RID: 4715 RVA: 0x0003F8E0 File Offset: 0x0003DAE0
		public void Reload()
		{
			MethodBase originalMethod = this.OriginalMethod;
			if (originalMethod == null)
			{
				throw new InvalidOperationException();
			}
			ModuleDefinition moduleDefinition = null;
			try
			{
				this._Definition = null;
				ModuleDefinition module = this._Module;
				if (module != null)
				{
					module.Dispose();
				}
				this._Module = null;
				ParameterInfo[] parameters = originalMethod.GetParameters();
				int num = 0;
				Type[] array;
				if (!originalMethod.IsStatic)
				{
					num++;
					array = new Type[parameters.Length + 1];
					array[0] = originalMethod.GetThisParamType();
				}
				else
				{
					array = new Type[parameters.Length];
				}
				for (int i = 0; i < parameters.Length; i++)
				{
					array[i + num] = parameters[i].ParameterType;
				}
				string id = originalMethod.GetID(null, null, true, false, true);
				MethodInfo methodInfo = originalMethod as MethodInfo;
				moduleDefinition = this._CreateDynModule(id, (methodInfo != null) ? methodInfo.ReturnType : null, array);
				this._CopyMethodToDefinition();
				MethodDefinition definition = this.Definition;
				if (!originalMethod.IsStatic)
				{
					definition.Parameters[0].Name = "this";
				}
				for (int j = 0; j < parameters.Length; j++)
				{
					definition.Parameters[j + num].Name = parameters[j].Name;
				}
				this._Module = moduleDefinition;
				moduleDefinition = null;
			}
			catch
			{
				if (moduleDefinition != null)
				{
					moduleDefinition.Dispose();
				}
				throw;
			}
		}

		// Token: 0x0600126C RID: 4716 RVA: 0x0003FA34 File Offset: 0x0003DC34
		public MethodInfo Generate()
		{
			return this.Generate(null);
		}

		// Token: 0x0600126D RID: 4717 RVA: 0x0003FA40 File Offset: 0x0003DC40
		public MethodInfo Generate(object context)
		{
			string environmentVariable = Environment.GetEnvironmentVariable("MONOMOD_DMD_TYPE");
			string text = ((environmentVariable != null) ? environmentVariable.ToLowerInvariant() : null);
			if (text != null)
			{
				if (text == "dynamicmethod" || text == "dm")
				{
					return DMDGenerator<DMDEmitDynamicMethodGenerator>.Generate(this, context);
				}
				if (text == "methodbuilder" || text == "mb")
				{
					return DMDGenerator<DMDEmitMethodBuilderGenerator>.Generate(this, context);
				}
				if (text == "cecil" || text == "md")
				{
					return DMDGenerator<DMDCecilGenerator>.Generate(this, context);
				}
			}
			Type type = ReflectionHelper.GetType(environmentVariable);
			if (type != null)
			{
				if (!DynamicMethodDefinition.t__IDMDGenerator.IsCompatible(type))
				{
					throw new ArgumentException("Invalid DMDGenerator type: " + environmentVariable);
				}
				_IDMDGenerator idmdgenerator;
				if (!DynamicMethodDefinition._DMDGeneratorCache.TryGetValue(environmentVariable, out idmdgenerator))
				{
					idmdgenerator = (DynamicMethodDefinition._DMDGeneratorCache[environmentVariable] = Activator.CreateInstance(type) as _IDMDGenerator);
				}
				return idmdgenerator.Generate(this, context);
			}
			else
			{
				if (DynamicMethodDefinition._PreferCecil)
				{
					return DMDGenerator<DMDCecilGenerator>.Generate(this, context);
				}
				if (this.Debug)
				{
					return DMDGenerator<DMDEmitMethodBuilderGenerator>.Generate(this, context);
				}
				if (this.Definition.Body.ExceptionHandlers.Any((Mono.Cecil.Cil.ExceptionHandler eh) => eh.HandlerType == ExceptionHandlerType.Fault || eh.HandlerType == ExceptionHandlerType.Filter))
				{
					return DMDGenerator<DMDEmitMethodBuilderGenerator>.Generate(this, context);
				}
				return DMDGenerator<DMDEmitDynamicMethodGenerator>.Generate(this, context);
			}
		}

		// Token: 0x0600126E RID: 4718 RVA: 0x0003FB92 File Offset: 0x0003DD92
		public void Dispose()
		{
			if (this._IsDisposed)
			{
				return;
			}
			this._IsDisposed = true;
			this._Module.Dispose();
		}

		// Token: 0x0600126F RID: 4719 RVA: 0x0003FBAF File Offset: 0x0003DDAF
		public string GetDumpName(string type)
		{
			return string.Format("DMDASM.{0:X8}{1}", this.GUID.GetHashCode(), string.IsNullOrEmpty(type) ? "" : ("." + type));
		}

		// Token: 0x06001270 RID: 4720 RVA: 0x0003FBEC File Offset: 0x0003DDEC
		[CompilerGenerated]
		internal static void <_CopyMethodToDefinition>g__ReadOperand|3_0(BinaryReader reader, Instruction instr, ref DynamicMethodDefinition.<>c__DisplayClass3_0 A_2)
		{
			switch (instr.OpCode.OperandType)
			{
			case Mono.Cecil.Cil.OperandType.InlineBrTarget:
			{
				int num = reader.ReadInt32();
				instr.Operand = (int)reader.BaseStream.Position + num;
				return;
			}
			case Mono.Cecil.Cil.OperandType.InlineField:
				instr.Operand = A_2.moduleTo.ImportReference(A_2.moduleFrom.ResolveField(reader.ReadInt32(), A_2.typeArguments, A_2.methodArguments));
				return;
			case Mono.Cecil.Cil.OperandType.InlineI:
				instr.Operand = reader.ReadInt32();
				return;
			case Mono.Cecil.Cil.OperandType.InlineI8:
				instr.Operand = reader.ReadInt64();
				return;
			case Mono.Cecil.Cil.OperandType.InlineMethod:
				instr.Operand = A_2.moduleTo.ImportReference(A_2.moduleFrom.ResolveMethod(reader.ReadInt32(), A_2.typeArguments, A_2.methodArguments));
				return;
			case Mono.Cecil.Cil.OperandType.InlineNone:
				instr.Operand = null;
				return;
			case Mono.Cecil.Cil.OperandType.InlineR:
				instr.Operand = reader.ReadDouble();
				return;
			case Mono.Cecil.Cil.OperandType.InlineSig:
				instr.Operand = A_2.moduleTo.ImportCallSite(A_2.moduleFrom, A_2.moduleFrom.ResolveSignature(reader.ReadInt32()));
				return;
			case Mono.Cecil.Cil.OperandType.InlineString:
				instr.Operand = A_2.moduleFrom.ResolveString(reader.ReadInt32());
				return;
			case Mono.Cecil.Cil.OperandType.InlineSwitch:
			{
				int num2 = reader.ReadInt32();
				int num = (int)reader.BaseStream.Position + 4 * num2;
				int[] array = new int[num2];
				for (int i = 0; i < num2; i++)
				{
					array[i] = reader.ReadInt32() + num;
				}
				instr.Operand = array;
				return;
			}
			case Mono.Cecil.Cil.OperandType.InlineTok:
			{
				MemberInfo memberInfo = A_2.moduleFrom.ResolveMember(reader.ReadInt32(), A_2.typeArguments, A_2.methodArguments);
				Type type = memberInfo as Type;
				if (type != null)
				{
					instr.Operand = A_2.moduleTo.ImportReference(type);
					return;
				}
				FieldInfo fieldInfo = memberInfo as FieldInfo;
				if (fieldInfo != null)
				{
					instr.Operand = A_2.moduleTo.ImportReference(fieldInfo);
					return;
				}
				MethodBase methodBase = memberInfo as MethodBase;
				if (methodBase == null)
				{
					return;
				}
				instr.Operand = A_2.moduleTo.ImportReference(methodBase);
				return;
			}
			case Mono.Cecil.Cil.OperandType.InlineType:
				instr.Operand = A_2.moduleTo.ImportReference(A_2.moduleFrom.ResolveType(reader.ReadInt32(), A_2.typeArguments, A_2.methodArguments));
				return;
			case Mono.Cecil.Cil.OperandType.InlineVar:
			case Mono.Cecil.Cil.OperandType.ShortInlineVar:
			{
				int num3 = (int)((instr.OpCode.OperandType == Mono.Cecil.Cil.OperandType.ShortInlineVar) ? ((short)reader.ReadByte()) : reader.ReadInt16());
				instr.Operand = A_2.bodyTo.Variables[num3];
				return;
			}
			case Mono.Cecil.Cil.OperandType.InlineArg:
			case Mono.Cecil.Cil.OperandType.ShortInlineArg:
			{
				int num3 = (int)((instr.OpCode.OperandType == Mono.Cecil.Cil.OperandType.ShortInlineArg) ? ((short)reader.ReadByte()) : reader.ReadInt16());
				instr.Operand = A_2.def.Parameters[num3];
				return;
			}
			case Mono.Cecil.Cil.OperandType.ShortInlineBrTarget:
			{
				int num = (int)reader.ReadSByte();
				instr.Operand = (int)reader.BaseStream.Position + num;
				return;
			}
			case Mono.Cecil.Cil.OperandType.ShortInlineI:
				instr.Operand = ((instr.OpCode == Mono.Cecil.Cil.OpCodes.Ldc_I4_S) ? reader.ReadSByte() : reader.ReadByte());
				return;
			case Mono.Cecil.Cil.OperandType.ShortInlineR:
				instr.Operand = reader.ReadSingle();
				return;
			}
			throw new NotSupportedException("Unsupported opcode $" + instr.OpCode.Name);
		}

		// Token: 0x06001271 RID: 4721 RVA: 0x0003FF50 File Offset: 0x0003E150
		[CompilerGenerated]
		internal static Instruction <_CopyMethodToDefinition>g__GetInstruction|3_1(int offset, ref DynamicMethodDefinition.<>c__DisplayClass3_0 A_1)
		{
			int num = A_1.bodyTo.Instructions.Count - 1;
			if (offset < 0 || offset > A_1.bodyTo.Instructions[num].Offset)
			{
				return null;
			}
			int i = 0;
			int num2 = num;
			while (i <= num2)
			{
				int num3 = i + (num2 - i) / 2;
				Instruction instruction = A_1.bodyTo.Instructions[num3];
				if (offset == instruction.Offset)
				{
					return instruction;
				}
				if (offset < instruction.Offset)
				{
					num2 = num3 - 1;
				}
				else
				{
					i = num3 + 1;
				}
			}
			return null;
		}

		// Token: 0x04000F50 RID: 3920
		private static Mono.Cecil.Cil.OpCode[] _CecilOpCodes1X;

		// Token: 0x04000F51 RID: 3921
		private static Mono.Cecil.Cil.OpCode[] _CecilOpCodes2X;

		// Token: 0x04000F52 RID: 3922
		internal static readonly bool _IsMono = Type.GetType("Mono.Runtime") != null;

		// Token: 0x04000F53 RID: 3923
		internal static readonly bool _IsNewMonoSRE = DynamicMethodDefinition._IsMono && typeof(DynamicMethod).GetField("il_info", BindingFlags.Instance | BindingFlags.NonPublic) != null;

		// Token: 0x04000F54 RID: 3924
		internal static readonly bool _IsOldMonoSRE = DynamicMethodDefinition._IsMono && !DynamicMethodDefinition._IsNewMonoSRE && typeof(DynamicMethod).GetField("ilgen", BindingFlags.Instance | BindingFlags.NonPublic) != null;

		// Token: 0x04000F55 RID: 3925
		private static bool _PreferCecil;

		// Token: 0x04000F56 RID: 3926
		internal static readonly ConstructorInfo c_DebuggableAttribute;

		// Token: 0x04000F57 RID: 3927
		internal static readonly ConstructorInfo c_UnverifiableCodeAttribute;

		// Token: 0x04000F58 RID: 3928
		internal static readonly ConstructorInfo c_IgnoresAccessChecksToAttribute;

		// Token: 0x04000F59 RID: 3929
		internal static readonly Type t__IDMDGenerator;

		// Token: 0x04000F5A RID: 3930
		internal static readonly Dictionary<string, _IDMDGenerator> _DMDGeneratorCache;

		// Token: 0x04000F5C RID: 3932
		private MethodDefinition _Definition;

		// Token: 0x04000F5D RID: 3933
		private ModuleDefinition _Module;

		// Token: 0x04000F5E RID: 3934
		public Type OwnerType;

		// Token: 0x04000F5F RID: 3935
		public bool Debug;

		// Token: 0x04000F60 RID: 3936
		private Guid GUID = Guid.NewGuid();

		// Token: 0x04000F61 RID: 3937
		private bool _IsDisposed;
	}
}
