using System;
using Mono.Collections.Generic;

namespace Mono.Cecil
{
	// Token: 0x0200008E RID: 142
	public abstract class MethodSpecification : MethodReference
	{
		// Token: 0x1700017C RID: 380
		// (get) Token: 0x060005FA RID: 1530 RVA: 0x00015260 File Offset: 0x00013460
		public MethodReference ElementMethod
		{
			get
			{
				return this.method;
			}
		}

		// Token: 0x1700017D RID: 381
		// (get) Token: 0x060005FB RID: 1531 RVA: 0x00015268 File Offset: 0x00013468
		// (set) Token: 0x060005FC RID: 1532 RVA: 0x00002C55 File Offset: 0x00000E55
		public override string Name
		{
			get
			{
				return this.method.Name;
			}
			set
			{
				throw new InvalidOperationException();
			}
		}

		// Token: 0x1700017E RID: 382
		// (get) Token: 0x060005FD RID: 1533 RVA: 0x00015275 File Offset: 0x00013475
		// (set) Token: 0x060005FE RID: 1534 RVA: 0x00002C55 File Offset: 0x00000E55
		public override MethodCallingConvention CallingConvention
		{
			get
			{
				return this.method.CallingConvention;
			}
			set
			{
				throw new InvalidOperationException();
			}
		}

		// Token: 0x1700017F RID: 383
		// (get) Token: 0x060005FF RID: 1535 RVA: 0x00015282 File Offset: 0x00013482
		// (set) Token: 0x06000600 RID: 1536 RVA: 0x00002C55 File Offset: 0x00000E55
		public override bool HasThis
		{
			get
			{
				return this.method.HasThis;
			}
			set
			{
				throw new InvalidOperationException();
			}
		}

		// Token: 0x17000180 RID: 384
		// (get) Token: 0x06000601 RID: 1537 RVA: 0x0001528F File Offset: 0x0001348F
		// (set) Token: 0x06000602 RID: 1538 RVA: 0x00002C55 File Offset: 0x00000E55
		public override bool ExplicitThis
		{
			get
			{
				return this.method.ExplicitThis;
			}
			set
			{
				throw new InvalidOperationException();
			}
		}

		// Token: 0x17000181 RID: 385
		// (get) Token: 0x06000603 RID: 1539 RVA: 0x0001529C File Offset: 0x0001349C
		// (set) Token: 0x06000604 RID: 1540 RVA: 0x00002C55 File Offset: 0x00000E55
		public override MethodReturnType MethodReturnType
		{
			get
			{
				return this.method.MethodReturnType;
			}
			set
			{
				throw new InvalidOperationException();
			}
		}

		// Token: 0x17000182 RID: 386
		// (get) Token: 0x06000605 RID: 1541 RVA: 0x000152A9 File Offset: 0x000134A9
		// (set) Token: 0x06000606 RID: 1542 RVA: 0x00002C55 File Offset: 0x00000E55
		public override TypeReference DeclaringType
		{
			get
			{
				return this.method.DeclaringType;
			}
			set
			{
				throw new InvalidOperationException();
			}
		}

		// Token: 0x17000183 RID: 387
		// (get) Token: 0x06000607 RID: 1543 RVA: 0x000152B6 File Offset: 0x000134B6
		public override ModuleDefinition Module
		{
			get
			{
				return this.method.Module;
			}
		}

		// Token: 0x17000184 RID: 388
		// (get) Token: 0x06000608 RID: 1544 RVA: 0x000152C3 File Offset: 0x000134C3
		public override bool HasParameters
		{
			get
			{
				return this.method.HasParameters;
			}
		}

		// Token: 0x17000185 RID: 389
		// (get) Token: 0x06000609 RID: 1545 RVA: 0x000152D0 File Offset: 0x000134D0
		public override Collection<ParameterDefinition> Parameters
		{
			get
			{
				return this.method.Parameters;
			}
		}

		// Token: 0x17000186 RID: 390
		// (get) Token: 0x0600060A RID: 1546 RVA: 0x000152DD File Offset: 0x000134DD
		public override bool ContainsGenericParameter
		{
			get
			{
				return this.method.ContainsGenericParameter;
			}
		}

		// Token: 0x0600060B RID: 1547 RVA: 0x000152EA File Offset: 0x000134EA
		internal MethodSpecification(MethodReference method)
		{
			Mixin.CheckMethod(method);
			this.method = method;
			this.token = new MetadataToken(TokenType.MethodSpec);
		}

		// Token: 0x0600060C RID: 1548 RVA: 0x0001530F File Offset: 0x0001350F
		public sealed override MethodReference GetElementMethod()
		{
			return this.method.GetElementMethod();
		}

		// Token: 0x0400016D RID: 365
		private readonly MethodReference method;
	}
}
