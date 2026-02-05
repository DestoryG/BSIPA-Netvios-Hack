using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.InteropServices;
using Mono.Cecil;
using Mono.Cecil.Cil;
using Mono.Collections.Generic;

namespace MonoMod.Utils.Cil
{
	// Token: 0x0200033F RID: 831
	internal sealed class CecilILGenerator : ILGeneratorShim
	{
		// Token: 0x0600131D RID: 4893 RVA: 0x00045CAC File Offset: 0x00043EAC
		unsafe static CecilILGenerator()
		{
			FieldInfo[] fields = typeof(Mono.Cecil.Cil.OpCodes).GetFields(BindingFlags.Static | BindingFlags.Public);
			for (int i = 0; i < fields.Length; i++)
			{
				Mono.Cecil.Cil.OpCode opCode = (Mono.Cecil.Cil.OpCode)fields[i].GetValue(null);
				CecilILGenerator._MCCOpCodes[opCode.Value] = opCode;
			}
			Label label = default(Label);
			*(int*)(&label) = -1;
			CecilILGenerator.NullLabel = label;
		}

		// Token: 0x0600131E RID: 4894 RVA: 0x00045D90 File Offset: 0x00043F90
		public CecilILGenerator(ILProcessor il)
		{
			this.IL = il;
		}

		// Token: 0x0600131F RID: 4895 RVA: 0x00045DE1 File Offset: 0x00043FE1
		private Mono.Cecil.Cil.OpCode _(global::System.Reflection.Emit.OpCode opcode)
		{
			return CecilILGenerator._MCCOpCodes[opcode.Value];
		}

		// Token: 0x06001320 RID: 4896 RVA: 0x00045DF4 File Offset: 0x00043FF4
		private CecilILGenerator.LabelInfo _(Label handle)
		{
			CecilILGenerator.LabelInfo labelInfo;
			if (!this._LabelInfos.TryGetValue(handle, out labelInfo))
			{
				return null;
			}
			return labelInfo;
		}

		// Token: 0x06001321 RID: 4897 RVA: 0x00045E14 File Offset: 0x00044014
		private VariableDefinition _(LocalBuilder handle)
		{
			return this._Variables[handle];
		}

		// Token: 0x06001322 RID: 4898 RVA: 0x00045E22 File Offset: 0x00044022
		private TypeReference _(Type info)
		{
			return this.IL.Body.Method.Module.ImportReference(info);
		}

		// Token: 0x06001323 RID: 4899 RVA: 0x00045E3F File Offset: 0x0004403F
		private FieldReference _(FieldInfo info)
		{
			return this.IL.Body.Method.Module.ImportReference(info);
		}

		// Token: 0x06001324 RID: 4900 RVA: 0x00045E5C File Offset: 0x0004405C
		private MethodReference _(MethodBase info)
		{
			return this.IL.Body.Method.Module.ImportReference(info);
		}

		// Token: 0x17000394 RID: 916
		// (get) Token: 0x06001325 RID: 4901 RVA: 0x000039BA File Offset: 0x00001BBA
		public override int ILOffset
		{
			get
			{
				throw new NotSupportedException();
			}
		}

		// Token: 0x06001326 RID: 4902 RVA: 0x00045E7C File Offset: 0x0004407C
		private Instruction ProcessLabels(Instruction ins)
		{
			if (this._LabelsToMark.Count != 0)
			{
				foreach (CecilILGenerator.LabelInfo labelInfo in this._LabelsToMark)
				{
					foreach (Instruction instruction in labelInfo.Branches)
					{
						object operand = instruction.Operand;
						if (!(operand is Instruction))
						{
							Instruction[] array = operand as Instruction[];
							if (array != null)
							{
								for (int i = 0; i < array.Length; i++)
								{
									if (array[i] == labelInfo.Instruction)
									{
										array[i] = ins;
										break;
									}
								}
							}
						}
						else
						{
							instruction.Operand = ins;
						}
					}
					labelInfo.Emitted = true;
					labelInfo.Instruction = ins;
				}
				this._LabelsToMark.Clear();
			}
			if (this._ExceptionHandlersToMark.Count != 0)
			{
				foreach (CecilILGenerator.LabelledExceptionHandler labelledExceptionHandler in this._ExceptionHandlersToMark)
				{
					Collection<Mono.Cecil.Cil.ExceptionHandler> exceptionHandlers = this.IL.Body.ExceptionHandlers;
					Mono.Cecil.Cil.ExceptionHandler exceptionHandler = new Mono.Cecil.Cil.ExceptionHandler(labelledExceptionHandler.HandlerType);
					CecilILGenerator.LabelInfo labelInfo2 = this._(labelledExceptionHandler.TryStart);
					exceptionHandler.TryStart = ((labelInfo2 != null) ? labelInfo2.Instruction : null);
					CecilILGenerator.LabelInfo labelInfo3 = this._(labelledExceptionHandler.TryEnd);
					exceptionHandler.TryEnd = ((labelInfo3 != null) ? labelInfo3.Instruction : null);
					CecilILGenerator.LabelInfo labelInfo4 = this._(labelledExceptionHandler.HandlerStart);
					exceptionHandler.HandlerStart = ((labelInfo4 != null) ? labelInfo4.Instruction : null);
					CecilILGenerator.LabelInfo labelInfo5 = this._(labelledExceptionHandler.HandlerEnd);
					exceptionHandler.HandlerEnd = ((labelInfo5 != null) ? labelInfo5.Instruction : null);
					CecilILGenerator.LabelInfo labelInfo6 = this._(labelledExceptionHandler.FilterStart);
					exceptionHandler.FilterStart = ((labelInfo6 != null) ? labelInfo6.Instruction : null);
					exceptionHandler.CatchType = labelledExceptionHandler.ExceptionType;
					exceptionHandlers.Add(exceptionHandler);
				}
				this._ExceptionHandlersToMark.Clear();
			}
			return ins;
		}

		// Token: 0x06001327 RID: 4903 RVA: 0x000460AC File Offset: 0x000442AC
		public unsafe override Label DefineLabel()
		{
			Label label = default(Label);
			ref int ptr = ref *(int*)(&label);
			int num = this.labelCounter;
			this.labelCounter = num + 1;
			ptr = num;
			this._LabelInfos[label] = new CecilILGenerator.LabelInfo();
			return label;
		}

		// Token: 0x06001328 RID: 4904 RVA: 0x000460E8 File Offset: 0x000442E8
		public override void MarkLabel(Label loc)
		{
			CecilILGenerator.LabelInfo labelInfo;
			if (!this._LabelInfos.TryGetValue(loc, out labelInfo) || labelInfo.Emitted)
			{
				return;
			}
			this._LabelsToMark.Add(labelInfo);
		}

		// Token: 0x06001329 RID: 4905 RVA: 0x0004611A File Offset: 0x0004431A
		public override LocalBuilder DeclareLocal(Type type)
		{
			return this.DeclareLocal(type, false);
		}

		// Token: 0x0600132A RID: 4906 RVA: 0x00046124 File Offset: 0x00044324
		public override LocalBuilder DeclareLocal(Type type, bool pinned)
		{
			int count = this.IL.Body.Variables.Count;
			object obj;
			if (CecilILGenerator.c_LocalBuilder_params != 4)
			{
				if (CecilILGenerator.c_LocalBuilder_params != 3)
				{
					if (CecilILGenerator.c_LocalBuilder_params != 2)
					{
						if (CecilILGenerator.c_LocalBuilder_params != 0)
						{
							throw new NotSupportedException();
						}
						obj = CecilILGenerator.c_LocalBuilder.Invoke(new object[0]);
					}
					else
					{
						ConstructorInfo constructorInfo = CecilILGenerator.c_LocalBuilder;
						object[] array = new object[2];
						array[0] = type;
						obj = constructorInfo.Invoke(array);
					}
				}
				else
				{
					ConstructorInfo constructorInfo2 = CecilILGenerator.c_LocalBuilder;
					object[] array2 = new object[3];
					array2[0] = count;
					array2[1] = type;
					obj = constructorInfo2.Invoke(array2);
				}
			}
			else
			{
				obj = CecilILGenerator.c_LocalBuilder.Invoke(new object[] { count, type, null, pinned });
			}
			LocalBuilder localBuilder = (LocalBuilder)obj;
			FieldInfo fieldInfo = CecilILGenerator.f_LocalBuilder_position;
			if (fieldInfo != null)
			{
				fieldInfo.SetValue(localBuilder, (ushort)count);
			}
			FieldInfo fieldInfo2 = CecilILGenerator.f_LocalBuilder_is_pinned;
			if (fieldInfo2 != null)
			{
				fieldInfo2.SetValue(localBuilder, pinned);
			}
			TypeReference typeReference = this._(type);
			if (pinned)
			{
				typeReference = new PinnedType(typeReference);
			}
			VariableDefinition variableDefinition = new VariableDefinition(typeReference);
			this.IL.Body.Variables.Add(variableDefinition);
			this._Variables[localBuilder] = variableDefinition;
			return localBuilder;
		}

		// Token: 0x0600132B RID: 4907 RVA: 0x0004624B File Offset: 0x0004444B
		private void Emit(Instruction ins)
		{
			this.IL.Append(this.ProcessLabels(ins));
		}

		// Token: 0x0600132C RID: 4908 RVA: 0x0004625F File Offset: 0x0004445F
		public override void Emit(global::System.Reflection.Emit.OpCode opcode)
		{
			this.Emit(this.IL.Create(this._(opcode)));
		}

		// Token: 0x0600132D RID: 4909 RVA: 0x00046279 File Offset: 0x00044479
		public override void Emit(global::System.Reflection.Emit.OpCode opcode, byte arg)
		{
			if (opcode.OperandType == global::System.Reflection.Emit.OperandType.ShortInlineVar || opcode.OperandType == global::System.Reflection.Emit.OperandType.InlineVar)
			{
				this._EmitInlineVar(this._(opcode), (int)arg);
				return;
			}
			this.Emit(this.IL.Create(this._(opcode), arg));
		}

		// Token: 0x0600132E RID: 4910 RVA: 0x000462B9 File Offset: 0x000444B9
		public override void Emit(global::System.Reflection.Emit.OpCode opcode, sbyte arg)
		{
			if (opcode.OperandType == global::System.Reflection.Emit.OperandType.ShortInlineVar || opcode.OperandType == global::System.Reflection.Emit.OperandType.InlineVar)
			{
				this._EmitInlineVar(this._(opcode), (int)arg);
				return;
			}
			this.Emit(this.IL.Create(this._(opcode), arg));
		}

		// Token: 0x0600132F RID: 4911 RVA: 0x000462F9 File Offset: 0x000444F9
		public override void Emit(global::System.Reflection.Emit.OpCode opcode, short arg)
		{
			if (opcode.OperandType == global::System.Reflection.Emit.OperandType.ShortInlineVar || opcode.OperandType == global::System.Reflection.Emit.OperandType.InlineVar)
			{
				this._EmitInlineVar(this._(opcode), (int)arg);
				return;
			}
			this.Emit(this.IL.Create(this._(opcode), (int)arg));
		}

		// Token: 0x06001330 RID: 4912 RVA: 0x0004633C File Offset: 0x0004453C
		public override void Emit(global::System.Reflection.Emit.OpCode opcode, int arg)
		{
			if (opcode.OperandType == global::System.Reflection.Emit.OperandType.ShortInlineVar || opcode.OperandType == global::System.Reflection.Emit.OperandType.InlineVar)
			{
				this._EmitInlineVar(this._(opcode), arg);
				return;
			}
			if (opcode.Name.EndsWith(".s"))
			{
				this.Emit(this.IL.Create(this._(opcode), (sbyte)arg));
				return;
			}
			this.Emit(this.IL.Create(this._(opcode), arg));
		}

		// Token: 0x06001331 RID: 4913 RVA: 0x000463B5 File Offset: 0x000445B5
		public override void Emit(global::System.Reflection.Emit.OpCode opcode, long arg)
		{
			this.Emit(this.IL.Create(this._(opcode), arg));
		}

		// Token: 0x06001332 RID: 4914 RVA: 0x000463D0 File Offset: 0x000445D0
		public override void Emit(global::System.Reflection.Emit.OpCode opcode, float arg)
		{
			this.Emit(this.IL.Create(this._(opcode), arg));
		}

		// Token: 0x06001333 RID: 4915 RVA: 0x000463EB File Offset: 0x000445EB
		public override void Emit(global::System.Reflection.Emit.OpCode opcode, double arg)
		{
			this.Emit(this.IL.Create(this._(opcode), arg));
		}

		// Token: 0x06001334 RID: 4916 RVA: 0x00046406 File Offset: 0x00044606
		public override void Emit(global::System.Reflection.Emit.OpCode opcode, string arg)
		{
			this.Emit(this.IL.Create(this._(opcode), arg));
		}

		// Token: 0x06001335 RID: 4917 RVA: 0x00046421 File Offset: 0x00044621
		public override void Emit(global::System.Reflection.Emit.OpCode opcode, Type arg)
		{
			this.Emit(this.IL.Create(this._(opcode), this._(arg)));
		}

		// Token: 0x06001336 RID: 4918 RVA: 0x00046442 File Offset: 0x00044642
		public override void Emit(global::System.Reflection.Emit.OpCode opcode, FieldInfo arg)
		{
			this.Emit(this.IL.Create(this._(opcode), this._(arg)));
		}

		// Token: 0x06001337 RID: 4919 RVA: 0x00046463 File Offset: 0x00044663
		public override void Emit(global::System.Reflection.Emit.OpCode opcode, ConstructorInfo arg)
		{
			this.Emit(this.IL.Create(this._(opcode), this._(arg)));
		}

		// Token: 0x06001338 RID: 4920 RVA: 0x00046463 File Offset: 0x00044663
		public override void Emit(global::System.Reflection.Emit.OpCode opcode, MethodInfo arg)
		{
			this.Emit(this.IL.Create(this._(opcode), this._(arg)));
		}

		// Token: 0x06001339 RID: 4921 RVA: 0x00046484 File Offset: 0x00044684
		public override void Emit(global::System.Reflection.Emit.OpCode opcode, Label label)
		{
			CecilILGenerator.LabelInfo labelInfo = this._(label);
			Instruction instruction = this.IL.Create(this._(opcode), this._(label).Instruction);
			labelInfo.Branches.Add(instruction);
			this.IL.Append(this.ProcessLabels(instruction));
		}

		// Token: 0x0600133A RID: 4922 RVA: 0x000464D4 File Offset: 0x000446D4
		public override void Emit(global::System.Reflection.Emit.OpCode opcode, Label[] labels)
		{
			IEnumerable<CecilILGenerator.LabelInfo> enumerable = labels.Distinct<Label>().Select(new Func<Label, CecilILGenerator.LabelInfo>(this._));
			Instruction instruction = this.IL.Create(this._(opcode), enumerable.Select((CecilILGenerator.LabelInfo labelInfo) => labelInfo.Instruction).ToArray<Instruction>());
			foreach (CecilILGenerator.LabelInfo labelInfo2 in enumerable)
			{
				labelInfo2.Branches.Add(instruction);
			}
			this.IL.Append(this.ProcessLabels(instruction));
		}

		// Token: 0x0600133B RID: 4923 RVA: 0x00046588 File Offset: 0x00044788
		public override void Emit(global::System.Reflection.Emit.OpCode opcode, LocalBuilder local)
		{
			this.Emit(this.IL.Create(this._(opcode), this._(local)));
		}

		// Token: 0x0600133C RID: 4924 RVA: 0x000465A9 File Offset: 0x000447A9
		public override void Emit(global::System.Reflection.Emit.OpCode opcode, SignatureHelper signature)
		{
			this.Emit(this.IL.Create(this._(opcode), this.IL.Body.Method.Module.ImportCallSite(signature)));
		}

		// Token: 0x0600133D RID: 4925 RVA: 0x000465DE File Offset: 0x000447DE
		public void Emit(global::System.Reflection.Emit.OpCode opcode, ICallSiteGenerator signature)
		{
			this.Emit(this.IL.Create(this._(opcode), this.IL.Body.Method.Module.ImportCallSite(signature)));
		}

		// Token: 0x0600133E RID: 4926 RVA: 0x00046614 File Offset: 0x00044814
		private void _EmitInlineVar(Mono.Cecil.Cil.OpCode opcode, int index)
		{
			switch (opcode.OperandType)
			{
			case Mono.Cecil.Cil.OperandType.InlineVar:
			case Mono.Cecil.Cil.OperandType.ShortInlineVar:
				this.Emit(this.IL.Create(opcode, this.IL.Body.Variables[index]));
				return;
			case Mono.Cecil.Cil.OperandType.InlineArg:
			case Mono.Cecil.Cil.OperandType.ShortInlineArg:
				this.Emit(this.IL.Create(opcode, this.IL.Body.Method.Parameters[index]));
				return;
			}
			throw new NotSupportedException(string.Format("Unsupported SRE InlineVar -> Cecil {0} for {1} {2}", opcode.OperandType, opcode, index));
		}

		// Token: 0x0600133F RID: 4927 RVA: 0x00046463 File Offset: 0x00044663
		public override void EmitCall(global::System.Reflection.Emit.OpCode opcode, MethodInfo methodInfo, Type[] optionalParameterTypes)
		{
			this.Emit(this.IL.Create(this._(opcode), this._(methodInfo)));
		}

		// Token: 0x06001340 RID: 4928 RVA: 0x000039BA File Offset: 0x00001BBA
		public override void EmitCalli(global::System.Reflection.Emit.OpCode opcode, CallingConventions callingConvention, Type returnType, Type[] parameterTypes, Type[] optionalParameterTypes)
		{
			throw new NotSupportedException();
		}

		// Token: 0x06001341 RID: 4929 RVA: 0x000039BA File Offset: 0x00001BBA
		public override void EmitCalli(global::System.Reflection.Emit.OpCode opcode, CallingConvention unmanagedCallConv, Type returnType, Type[] parameterTypes)
		{
			throw new NotSupportedException();
		}

		// Token: 0x06001342 RID: 4930 RVA: 0x000466D0 File Offset: 0x000448D0
		public override void EmitWriteLine(FieldInfo field)
		{
			if (field.IsStatic)
			{
				this.Emit(this.IL.Create(Mono.Cecil.Cil.OpCodes.Ldsfld, this._(field)));
			}
			else
			{
				this.Emit(this.IL.Create(Mono.Cecil.Cil.OpCodes.Ldarg_0));
				this.Emit(this.IL.Create(Mono.Cecil.Cil.OpCodes.Ldfld, this._(field)));
			}
			this.Emit(this.IL.Create(Mono.Cecil.Cil.OpCodes.Call, this._(typeof(Console).GetMethod("WriteLine", new Type[] { field.FieldType }))));
		}

		// Token: 0x06001343 RID: 4931 RVA: 0x00046778 File Offset: 0x00044978
		public override void EmitWriteLine(LocalBuilder localBuilder)
		{
			this.Emit(this.IL.Create(Mono.Cecil.Cil.OpCodes.Ldloc, this._(localBuilder)));
			this.Emit(this.IL.Create(Mono.Cecil.Cil.OpCodes.Call, this._(typeof(Console).GetMethod("WriteLine", new Type[] { localBuilder.LocalType }))));
		}

		// Token: 0x06001344 RID: 4932 RVA: 0x000467E4 File Offset: 0x000449E4
		public override void EmitWriteLine(string value)
		{
			this.Emit(this.IL.Create(Mono.Cecil.Cil.OpCodes.Ldstr, value));
			this.Emit(this.IL.Create(Mono.Cecil.Cil.OpCodes.Call, this._(typeof(Console).GetMethod("WriteLine", new Type[] { typeof(string) }))));
		}

		// Token: 0x06001345 RID: 4933 RVA: 0x0004684B File Offset: 0x00044A4B
		public override void ThrowException(Type type)
		{
			this.Emit(this.IL.Create(Mono.Cecil.Cil.OpCodes.Newobj, this._(type.GetConstructor(Type.EmptyTypes))));
			this.Emit(this.IL.Create(Mono.Cecil.Cil.OpCodes.Throw));
		}

		// Token: 0x06001346 RID: 4934 RVA: 0x0004688C File Offset: 0x00044A8C
		public override Label BeginExceptionBlock()
		{
			CecilILGenerator.ExceptionHandlerChain exceptionHandlerChain = new CecilILGenerator.ExceptionHandlerChain(this);
			this._ExceptionHandlers.Push(exceptionHandlerChain);
			return exceptionHandlerChain.SkipAll;
		}

		// Token: 0x06001347 RID: 4935 RVA: 0x000468B2 File Offset: 0x00044AB2
		public override void BeginCatchBlock(Type exceptionType)
		{
			this._ExceptionHandlers.Peek().BeginHandler(ExceptionHandlerType.Catch).ExceptionType = ((exceptionType == null) ? null : this._(exceptionType));
		}

		// Token: 0x06001348 RID: 4936 RVA: 0x000468DD File Offset: 0x00044ADD
		public override void BeginExceptFilterBlock()
		{
			this._ExceptionHandlers.Peek().BeginHandler(ExceptionHandlerType.Filter);
		}

		// Token: 0x06001349 RID: 4937 RVA: 0x000468F1 File Offset: 0x00044AF1
		public override void BeginFaultBlock()
		{
			this._ExceptionHandlers.Peek().BeginHandler(ExceptionHandlerType.Fault);
		}

		// Token: 0x0600134A RID: 4938 RVA: 0x00046905 File Offset: 0x00044B05
		public override void BeginFinallyBlock()
		{
			this._ExceptionHandlers.Peek().BeginHandler(ExceptionHandlerType.Finally);
		}

		// Token: 0x0600134B RID: 4939 RVA: 0x00046919 File Offset: 0x00044B19
		public override void EndExceptionBlock()
		{
			this._ExceptionHandlers.Pop().End();
		}

		// Token: 0x0600134C RID: 4940 RVA: 0x00010C51 File Offset: 0x0000EE51
		public override void BeginScope()
		{
		}

		// Token: 0x0600134D RID: 4941 RVA: 0x00010C51 File Offset: 0x0000EE51
		public override void EndScope()
		{
		}

		// Token: 0x0600134E RID: 4942 RVA: 0x00010C51 File Offset: 0x0000EE51
		public override void UsingNamespace(string usingNamespace)
		{
		}

		// Token: 0x04000FB6 RID: 4022
		private static readonly ConstructorInfo c_LocalBuilder = (from c in typeof(LocalBuilder).GetConstructors(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
			orderby c.GetParameters().Length descending
			select c).First<ConstructorInfo>();

		// Token: 0x04000FB7 RID: 4023
		private static readonly FieldInfo f_LocalBuilder_position = typeof(LocalBuilder).GetField("position", BindingFlags.Instance | BindingFlags.NonPublic);

		// Token: 0x04000FB8 RID: 4024
		private static readonly FieldInfo f_LocalBuilder_is_pinned = typeof(LocalBuilder).GetField("is_pinned", BindingFlags.Instance | BindingFlags.NonPublic);

		// Token: 0x04000FB9 RID: 4025
		private static int c_LocalBuilder_params = CecilILGenerator.c_LocalBuilder.GetParameters().Length;

		// Token: 0x04000FBA RID: 4026
		private static readonly Dictionary<short, Mono.Cecil.Cil.OpCode> _MCCOpCodes = new Dictionary<short, Mono.Cecil.Cil.OpCode>();

		// Token: 0x04000FBB RID: 4027
		private static Label NullLabel;

		// Token: 0x04000FBC RID: 4028
		public readonly ILProcessor IL;

		// Token: 0x04000FBD RID: 4029
		private readonly Dictionary<Label, CecilILGenerator.LabelInfo> _LabelInfos = new Dictionary<Label, CecilILGenerator.LabelInfo>();

		// Token: 0x04000FBE RID: 4030
		private readonly List<CecilILGenerator.LabelInfo> _LabelsToMark = new List<CecilILGenerator.LabelInfo>();

		// Token: 0x04000FBF RID: 4031
		private readonly List<CecilILGenerator.LabelledExceptionHandler> _ExceptionHandlersToMark = new List<CecilILGenerator.LabelledExceptionHandler>();

		// Token: 0x04000FC0 RID: 4032
		private readonly Dictionary<LocalBuilder, VariableDefinition> _Variables = new Dictionary<LocalBuilder, VariableDefinition>();

		// Token: 0x04000FC1 RID: 4033
		private readonly Stack<CecilILGenerator.ExceptionHandlerChain> _ExceptionHandlers = new Stack<CecilILGenerator.ExceptionHandlerChain>();

		// Token: 0x04000FC2 RID: 4034
		private int labelCounter;

		// Token: 0x02000340 RID: 832
		private class LabelInfo
		{
			// Token: 0x04000FC3 RID: 4035
			public bool Emitted;

			// Token: 0x04000FC4 RID: 4036
			public Instruction Instruction = Instruction.Create(Mono.Cecil.Cil.OpCodes.Nop);

			// Token: 0x04000FC5 RID: 4037
			public readonly List<Instruction> Branches = new List<Instruction>();
		}

		// Token: 0x02000341 RID: 833
		private class LabelledExceptionHandler
		{
			// Token: 0x04000FC6 RID: 4038
			public Label TryStart = CecilILGenerator.NullLabel;

			// Token: 0x04000FC7 RID: 4039
			public Label TryEnd = CecilILGenerator.NullLabel;

			// Token: 0x04000FC8 RID: 4040
			public Label HandlerStart = CecilILGenerator.NullLabel;

			// Token: 0x04000FC9 RID: 4041
			public Label HandlerEnd = CecilILGenerator.NullLabel;

			// Token: 0x04000FCA RID: 4042
			public Label FilterStart = CecilILGenerator.NullLabel;

			// Token: 0x04000FCB RID: 4043
			public ExceptionHandlerType HandlerType;

			// Token: 0x04000FCC RID: 4044
			public TypeReference ExceptionType;
		}

		// Token: 0x02000342 RID: 834
		private class ExceptionHandlerChain
		{
			// Token: 0x06001351 RID: 4945 RVA: 0x0004698D File Offset: 0x00044B8D
			public ExceptionHandlerChain(CecilILGenerator il)
			{
				this.IL = il;
				this._Start = il.DefineLabel();
				il.MarkLabel(this._Start);
				this.SkipAll = il.DefineLabel();
			}

			// Token: 0x06001352 RID: 4946 RVA: 0x000469C0 File Offset: 0x00044BC0
			public CecilILGenerator.LabelledExceptionHandler BeginHandler(ExceptionHandlerType type)
			{
				CecilILGenerator.LabelledExceptionHandler labelledExceptionHandler = (this._Prev = this._Handler);
				if (labelledExceptionHandler != null)
				{
					this.EndHandler(labelledExceptionHandler);
				}
				this.IL.Emit(global::System.Reflection.Emit.OpCodes.Leave, this._SkipHandler = this.IL.DefineLabel());
				Label label = this.IL.DefineLabel();
				this.IL.MarkLabel(label);
				CecilILGenerator.LabelledExceptionHandler labelledExceptionHandler2 = new CecilILGenerator.LabelledExceptionHandler();
				labelledExceptionHandler2.TryStart = this._Start;
				labelledExceptionHandler2.TryEnd = label;
				labelledExceptionHandler2.HandlerType = type;
				labelledExceptionHandler2.HandlerEnd = this._SkipHandler;
				CecilILGenerator.LabelledExceptionHandler labelledExceptionHandler3 = labelledExceptionHandler2;
				this._Handler = labelledExceptionHandler2;
				CecilILGenerator.LabelledExceptionHandler labelledExceptionHandler4 = labelledExceptionHandler3;
				if (type == ExceptionHandlerType.Filter)
				{
					labelledExceptionHandler4.FilterStart = label;
				}
				else
				{
					labelledExceptionHandler4.HandlerStart = label;
				}
				return labelledExceptionHandler4;
			}

			// Token: 0x06001353 RID: 4947 RVA: 0x00046A70 File Offset: 0x00044C70
			public void EndHandler(CecilILGenerator.LabelledExceptionHandler handler)
			{
				Label skipHandler = this._SkipHandler;
				ExceptionHandlerType handlerType = handler.HandlerType;
				if (handlerType != ExceptionHandlerType.Filter)
				{
					if (handlerType != ExceptionHandlerType.Finally)
					{
						this.IL.Emit(global::System.Reflection.Emit.OpCodes.Leave, skipHandler);
					}
					else
					{
						this.IL.Emit(global::System.Reflection.Emit.OpCodes.Endfinally);
					}
				}
				else
				{
					this.IL.Emit(global::System.Reflection.Emit.OpCodes.Endfilter);
				}
				this.IL.MarkLabel(skipHandler);
				this.IL._ExceptionHandlersToMark.Add(handler);
			}

			// Token: 0x06001354 RID: 4948 RVA: 0x00046AE7 File Offset: 0x00044CE7
			public void End()
			{
				this.EndHandler(this._Handler);
				this.IL.MarkLabel(this.SkipAll);
			}

			// Token: 0x04000FCD RID: 4045
			private readonly CecilILGenerator IL;

			// Token: 0x04000FCE RID: 4046
			private readonly Label _Start;

			// Token: 0x04000FCF RID: 4047
			public readonly Label SkipAll;

			// Token: 0x04000FD0 RID: 4048
			private Label _SkipHandler;

			// Token: 0x04000FD1 RID: 4049
			private CecilILGenerator.LabelledExceptionHandler _Prev;

			// Token: 0x04000FD2 RID: 4050
			private CecilILGenerator.LabelledExceptionHandler _Handler;
		}
	}
}
