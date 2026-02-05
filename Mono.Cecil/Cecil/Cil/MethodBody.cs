using System;
using System.Threading;
using Mono.Collections.Generic;

namespace Mono.Cecil.Cil
{
	// Token: 0x020000FF RID: 255
	public sealed class MethodBody
	{
		// Token: 0x170002CE RID: 718
		// (get) Token: 0x06000A53 RID: 2643 RVA: 0x0002171D File Offset: 0x0001F91D
		public MethodDefinition Method
		{
			get
			{
				return this.method;
			}
		}

		// Token: 0x170002CF RID: 719
		// (get) Token: 0x06000A54 RID: 2644 RVA: 0x00021725 File Offset: 0x0001F925
		// (set) Token: 0x06000A55 RID: 2645 RVA: 0x0002172D File Offset: 0x0001F92D
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

		// Token: 0x170002D0 RID: 720
		// (get) Token: 0x06000A56 RID: 2646 RVA: 0x00021736 File Offset: 0x0001F936
		public int CodeSize
		{
			get
			{
				return this.code_size;
			}
		}

		// Token: 0x170002D1 RID: 721
		// (get) Token: 0x06000A57 RID: 2647 RVA: 0x0002173E File Offset: 0x0001F93E
		// (set) Token: 0x06000A58 RID: 2648 RVA: 0x00021746 File Offset: 0x0001F946
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

		// Token: 0x170002D2 RID: 722
		// (get) Token: 0x06000A59 RID: 2649 RVA: 0x0002174F File Offset: 0x0001F94F
		// (set) Token: 0x06000A5A RID: 2650 RVA: 0x00021757 File Offset: 0x0001F957
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

		// Token: 0x170002D3 RID: 723
		// (get) Token: 0x06000A5B RID: 2651 RVA: 0x00021760 File Offset: 0x0001F960
		public Collection<Instruction> Instructions
		{
			get
			{
				Collection<Instruction> collection;
				if ((collection = this.instructions) == null)
				{
					collection = (this.instructions = new InstructionCollection(this.method));
				}
				return collection;
			}
		}

		// Token: 0x170002D4 RID: 724
		// (get) Token: 0x06000A5C RID: 2652 RVA: 0x0002178B File Offset: 0x0001F98B
		public bool HasExceptionHandlers
		{
			get
			{
				return !this.exceptions.IsNullOrEmpty<ExceptionHandler>();
			}
		}

		// Token: 0x170002D5 RID: 725
		// (get) Token: 0x06000A5D RID: 2653 RVA: 0x0002179C File Offset: 0x0001F99C
		public Collection<ExceptionHandler> ExceptionHandlers
		{
			get
			{
				Collection<ExceptionHandler> collection;
				if ((collection = this.exceptions) == null)
				{
					collection = (this.exceptions = new Collection<ExceptionHandler>());
				}
				return collection;
			}
		}

		// Token: 0x170002D6 RID: 726
		// (get) Token: 0x06000A5E RID: 2654 RVA: 0x000217C1 File Offset: 0x0001F9C1
		public bool HasVariables
		{
			get
			{
				return !this.variables.IsNullOrEmpty<VariableDefinition>();
			}
		}

		// Token: 0x170002D7 RID: 727
		// (get) Token: 0x06000A5F RID: 2655 RVA: 0x000217D4 File Offset: 0x0001F9D4
		public Collection<VariableDefinition> Variables
		{
			get
			{
				Collection<VariableDefinition> collection;
				if ((collection = this.variables) == null)
				{
					collection = (this.variables = new VariableDefinitionCollection());
				}
				return collection;
			}
		}

		// Token: 0x170002D8 RID: 728
		// (get) Token: 0x06000A60 RID: 2656 RVA: 0x000217FC File Offset: 0x0001F9FC
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

		// Token: 0x06000A61 RID: 2657 RVA: 0x0002185C File Offset: 0x0001FA5C
		private static ParameterDefinition CreateThisParameter(MethodDefinition method)
		{
			TypeReference typeReference = method.DeclaringType;
			if (typeReference.HasGenericParameters)
			{
				GenericInstanceType genericInstanceType = new GenericInstanceType(typeReference);
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

		// Token: 0x06000A62 RID: 2658 RVA: 0x000218CC File Offset: 0x0001FACC
		public MethodBody(MethodDefinition method)
		{
			this.method = method;
		}

		// Token: 0x06000A63 RID: 2659 RVA: 0x000218DB File Offset: 0x0001FADB
		public ILProcessor GetILProcessor()
		{
			return new ILProcessor(this);
		}

		// Token: 0x04000542 RID: 1346
		internal readonly MethodDefinition method;

		// Token: 0x04000543 RID: 1347
		internal ParameterDefinition this_parameter;

		// Token: 0x04000544 RID: 1348
		internal int max_stack_size;

		// Token: 0x04000545 RID: 1349
		internal int code_size;

		// Token: 0x04000546 RID: 1350
		internal bool init_locals;

		// Token: 0x04000547 RID: 1351
		internal MetadataToken local_var_token;

		// Token: 0x04000548 RID: 1352
		internal Collection<Instruction> instructions;

		// Token: 0x04000549 RID: 1353
		internal Collection<ExceptionHandler> exceptions;

		// Token: 0x0400054A RID: 1354
		internal Collection<VariableDefinition> variables;
	}
}
