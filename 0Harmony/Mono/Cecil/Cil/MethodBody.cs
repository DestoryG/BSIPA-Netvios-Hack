using System;
using System.Threading;
using Mono.Collections.Generic;

namespace Mono.Cecil.Cil
{
	// Token: 0x020001C3 RID: 451
	internal sealed class MethodBody
	{
		// Token: 0x170002EE RID: 750
		// (get) Token: 0x06000E3A RID: 3642 RVA: 0x00030931 File Offset: 0x0002EB31
		public MethodDefinition Method
		{
			get
			{
				return this.method;
			}
		}

		// Token: 0x170002EF RID: 751
		// (get) Token: 0x06000E3B RID: 3643 RVA: 0x00030939 File Offset: 0x0002EB39
		// (set) Token: 0x06000E3C RID: 3644 RVA: 0x00030941 File Offset: 0x0002EB41
		public int MaxStackSize
		{
			get
			{
				return this.max_stack_size;
			}
			set
			{
				this.max_stack_size = value;
			}
		}

		// Token: 0x170002F0 RID: 752
		// (get) Token: 0x06000E3D RID: 3645 RVA: 0x0003094A File Offset: 0x0002EB4A
		public int CodeSize
		{
			get
			{
				return this.code_size;
			}
		}

		// Token: 0x170002F1 RID: 753
		// (get) Token: 0x06000E3E RID: 3646 RVA: 0x00030952 File Offset: 0x0002EB52
		// (set) Token: 0x06000E3F RID: 3647 RVA: 0x0003095A File Offset: 0x0002EB5A
		public bool InitLocals
		{
			get
			{
				return this.init_locals;
			}
			set
			{
				this.init_locals = value;
			}
		}

		// Token: 0x170002F2 RID: 754
		// (get) Token: 0x06000E40 RID: 3648 RVA: 0x00030963 File Offset: 0x0002EB63
		// (set) Token: 0x06000E41 RID: 3649 RVA: 0x0003096B File Offset: 0x0002EB6B
		public MetadataToken LocalVarToken
		{
			get
			{
				return this.local_var_token;
			}
			set
			{
				this.local_var_token = value;
			}
		}

		// Token: 0x170002F3 RID: 755
		// (get) Token: 0x06000E42 RID: 3650 RVA: 0x00030974 File Offset: 0x0002EB74
		public Collection<Instruction> Instructions
		{
			get
			{
				if (this.instructions == null)
				{
					Interlocked.CompareExchange<Collection<Instruction>>(ref this.instructions, new InstructionCollection(this.method), null);
				}
				return this.instructions;
			}
		}

		// Token: 0x170002F4 RID: 756
		// (get) Token: 0x06000E43 RID: 3651 RVA: 0x0003099C File Offset: 0x0002EB9C
		public bool HasExceptionHandlers
		{
			get
			{
				return !this.exceptions.IsNullOrEmpty<ExceptionHandler>();
			}
		}

		// Token: 0x170002F5 RID: 757
		// (get) Token: 0x06000E44 RID: 3652 RVA: 0x000309AC File Offset: 0x0002EBAC
		public Collection<ExceptionHandler> ExceptionHandlers
		{
			get
			{
				if (this.exceptions == null)
				{
					Interlocked.CompareExchange<Collection<ExceptionHandler>>(ref this.exceptions, new Collection<ExceptionHandler>(), null);
				}
				return this.exceptions;
			}
		}

		// Token: 0x170002F6 RID: 758
		// (get) Token: 0x06000E45 RID: 3653 RVA: 0x000309CE File Offset: 0x0002EBCE
		public bool HasVariables
		{
			get
			{
				return !this.variables.IsNullOrEmpty<VariableDefinition>();
			}
		}

		// Token: 0x170002F7 RID: 759
		// (get) Token: 0x06000E46 RID: 3654 RVA: 0x000309DE File Offset: 0x0002EBDE
		public Collection<VariableDefinition> Variables
		{
			get
			{
				if (this.variables == null)
				{
					Interlocked.CompareExchange<Collection<VariableDefinition>>(ref this.variables, new VariableDefinitionCollection(), null);
				}
				return this.variables;
			}
		}

		// Token: 0x170002F8 RID: 760
		// (get) Token: 0x06000E47 RID: 3655 RVA: 0x00030A00 File Offset: 0x0002EC00
		public ParameterDefinition ThisParameter
		{
			get
			{
				if (this.method == null || this.method.DeclaringType == null)
				{
					throw new NotSupportedException();
				}
				if (!this.method.HasThis)
				{
					return null;
				}
				if (this.this_parameter == null)
				{
					Interlocked.CompareExchange<ParameterDefinition>(ref this.this_parameter, MethodBody.CreateThisParameter(this.method), null);
				}
				return this.this_parameter;
			}
		}

		// Token: 0x06000E48 RID: 3656 RVA: 0x00030A60 File Offset: 0x0002EC60
		private static ParameterDefinition CreateThisParameter(MethodDefinition method)
		{
			TypeReference typeReference = method.DeclaringType;
			if (typeReference.HasGenericParameters)
			{
				GenericInstanceType genericInstanceType = new GenericInstanceType(typeReference, typeReference.GenericParameters.Count);
				for (int i = 0; i < typeReference.GenericParameters.Count; i++)
				{
					genericInstanceType.GenericArguments.Add(typeReference.GenericParameters[i]);
				}
				typeReference = genericInstanceType;
			}
			if (typeReference.IsValueType || typeReference.IsPrimitive)
			{
				typeReference = new ByReferenceType(typeReference);
			}
			return new ParameterDefinition(typeReference, method);
		}

		// Token: 0x06000E49 RID: 3657 RVA: 0x00030ADB File Offset: 0x0002ECDB
		public MethodBody(MethodDefinition method)
		{
			this.method = method;
		}

		// Token: 0x06000E4A RID: 3658 RVA: 0x00030AEA File Offset: 0x0002ECEA
		public ILProcessor GetILProcessor()
		{
			return new ILProcessor(this);
		}

		// Token: 0x040007A1 RID: 1953
		internal readonly MethodDefinition method;

		// Token: 0x040007A2 RID: 1954
		internal ParameterDefinition this_parameter;

		// Token: 0x040007A3 RID: 1955
		internal int max_stack_size;

		// Token: 0x040007A4 RID: 1956
		internal int code_size;

		// Token: 0x040007A5 RID: 1957
		internal bool init_locals;

		// Token: 0x040007A6 RID: 1958
		internal MetadataToken local_var_token;

		// Token: 0x040007A7 RID: 1959
		internal Collection<Instruction> instructions;

		// Token: 0x040007A8 RID: 1960
		internal Collection<ExceptionHandler> exceptions;

		// Token: 0x040007A9 RID: 1961
		internal Collection<VariableDefinition> variables;
	}
}
