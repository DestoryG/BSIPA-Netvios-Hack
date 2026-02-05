using System;
using Mono.Collections.Generic;

namespace Mono.Cecil
{
	// Token: 0x02000146 RID: 326
	internal abstract class MethodSpecification : MethodReference
	{
		// Token: 0x17000196 RID: 406
		// (get) Token: 0x06000993 RID: 2451 RVA: 0x00023B0C File Offset: 0x00021D0C
		public MethodReference ElementMethod
		{
			get
			{
				return this.method;
			}
		}

		// Token: 0x17000197 RID: 407
		// (get) Token: 0x06000994 RID: 2452 RVA: 0x00023B14 File Offset: 0x00021D14
		// (set) Token: 0x06000995 RID: 2453 RVA: 0x00010FA6 File Offset: 0x0000F1A6
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

		// Token: 0x17000198 RID: 408
		// (get) Token: 0x06000996 RID: 2454 RVA: 0x00023B21 File Offset: 0x00021D21
		// (set) Token: 0x06000997 RID: 2455 RVA: 0x00010FA6 File Offset: 0x0000F1A6
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

		// Token: 0x17000199 RID: 409
		// (get) Token: 0x06000998 RID: 2456 RVA: 0x00023B2E File Offset: 0x00021D2E
		// (set) Token: 0x06000999 RID: 2457 RVA: 0x00010FA6 File Offset: 0x0000F1A6
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

		// Token: 0x1700019A RID: 410
		// (get) Token: 0x0600099A RID: 2458 RVA: 0x00023B3B File Offset: 0x00021D3B
		// (set) Token: 0x0600099B RID: 2459 RVA: 0x00010FA6 File Offset: 0x0000F1A6
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

		// Token: 0x1700019B RID: 411
		// (get) Token: 0x0600099C RID: 2460 RVA: 0x00023B48 File Offset: 0x00021D48
		// (set) Token: 0x0600099D RID: 2461 RVA: 0x00010FA6 File Offset: 0x0000F1A6
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

		// Token: 0x1700019C RID: 412
		// (get) Token: 0x0600099E RID: 2462 RVA: 0x00023B55 File Offset: 0x00021D55
		// (set) Token: 0x0600099F RID: 2463 RVA: 0x00010FA6 File Offset: 0x0000F1A6
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

		// Token: 0x1700019D RID: 413
		// (get) Token: 0x060009A0 RID: 2464 RVA: 0x00023B62 File Offset: 0x00021D62
		public override ModuleDefinition Module
		{
			get
			{
				return this.method.Module;
			}
		}

		// Token: 0x1700019E RID: 414
		// (get) Token: 0x060009A1 RID: 2465 RVA: 0x00023B6F File Offset: 0x00021D6F
		public override bool HasParameters
		{
			get
			{
				return this.method.HasParameters;
			}
		}

		// Token: 0x1700019F RID: 415
		// (get) Token: 0x060009A2 RID: 2466 RVA: 0x00023B7C File Offset: 0x00021D7C
		public override Collection<ParameterDefinition> Parameters
		{
			get
			{
				return this.method.Parameters;
			}
		}

		// Token: 0x170001A0 RID: 416
		// (get) Token: 0x060009A3 RID: 2467 RVA: 0x00023B89 File Offset: 0x00021D89
		public override bool ContainsGenericParameter
		{
			get
			{
				return this.method.ContainsGenericParameter;
			}
		}

		// Token: 0x060009A4 RID: 2468 RVA: 0x00023B96 File Offset: 0x00021D96
		internal MethodSpecification(MethodReference method)
		{
			Mixin.CheckMethod(method);
			this.method = method;
			this.token = new MetadataToken(TokenType.MethodSpec);
		}

		// Token: 0x060009A5 RID: 2469 RVA: 0x00023BBB File Offset: 0x00021DBB
		public sealed override MethodReference GetElementMethod()
		{
			return this.method.GetElementMethod();
		}

		// Token: 0x0400038D RID: 909
		private readonly MethodReference method;
	}
}
