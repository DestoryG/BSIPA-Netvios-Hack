using System;
using System.Threading;
using Mono.Collections.Generic;

namespace Mono.Cecil
{
	// Token: 0x02000144 RID: 324
	internal sealed class MethodReturnType : IConstantProvider, IMetadataTokenProvider, ICustomAttributeProvider, IMarshalInfoProvider
	{
		// Token: 0x17000188 RID: 392
		// (get) Token: 0x0600097B RID: 2427 RVA: 0x00023985 File Offset: 0x00021B85
		public IMethodSignature Method
		{
			get
			{
				return this.method;
			}
		}

		// Token: 0x17000189 RID: 393
		// (get) Token: 0x0600097C RID: 2428 RVA: 0x0002398D File Offset: 0x00021B8D
		// (set) Token: 0x0600097D RID: 2429 RVA: 0x00023995 File Offset: 0x00021B95
		public TypeReference ReturnType
		{
			get
			{
				return this.return_type;
			}
			set
			{
				this.return_type = value;
			}
		}

		// Token: 0x1700018A RID: 394
		// (get) Token: 0x0600097E RID: 2430 RVA: 0x0002399E File Offset: 0x00021B9E
		internal ParameterDefinition Parameter
		{
			get
			{
				if (this.parameter == null)
				{
					Interlocked.CompareExchange<ParameterDefinition>(ref this.parameter, new ParameterDefinition(this.return_type, this.method), null);
				}
				return this.parameter;
			}
		}

		// Token: 0x1700018B RID: 395
		// (get) Token: 0x0600097F RID: 2431 RVA: 0x000239CC File Offset: 0x00021BCC
		// (set) Token: 0x06000980 RID: 2432 RVA: 0x000239D9 File Offset: 0x00021BD9
		public MetadataToken MetadataToken
		{
			get
			{
				return this.Parameter.MetadataToken;
			}
			set
			{
				this.Parameter.MetadataToken = value;
			}
		}

		// Token: 0x1700018C RID: 396
		// (get) Token: 0x06000981 RID: 2433 RVA: 0x000239E7 File Offset: 0x00021BE7
		// (set) Token: 0x06000982 RID: 2434 RVA: 0x000239F4 File Offset: 0x00021BF4
		public ParameterAttributes Attributes
		{
			get
			{
				return this.Parameter.Attributes;
			}
			set
			{
				this.Parameter.Attributes = value;
			}
		}

		// Token: 0x1700018D RID: 397
		// (get) Token: 0x06000983 RID: 2435 RVA: 0x00023A02 File Offset: 0x00021C02
		// (set) Token: 0x06000984 RID: 2436 RVA: 0x00023A0F File Offset: 0x00021C0F
		public string Name
		{
			get
			{
				return this.Parameter.Name;
			}
			set
			{
				this.Parameter.Name = value;
			}
		}

		// Token: 0x1700018E RID: 398
		// (get) Token: 0x06000985 RID: 2437 RVA: 0x00023A1D File Offset: 0x00021C1D
		public bool HasCustomAttributes
		{
			get
			{
				return this.parameter != null && this.parameter.HasCustomAttributes;
			}
		}

		// Token: 0x1700018F RID: 399
		// (get) Token: 0x06000986 RID: 2438 RVA: 0x00023A34 File Offset: 0x00021C34
		public Collection<CustomAttribute> CustomAttributes
		{
			get
			{
				return this.Parameter.CustomAttributes;
			}
		}

		// Token: 0x17000190 RID: 400
		// (get) Token: 0x06000987 RID: 2439 RVA: 0x00023A41 File Offset: 0x00021C41
		// (set) Token: 0x06000988 RID: 2440 RVA: 0x00023A58 File Offset: 0x00021C58
		public bool HasDefault
		{
			get
			{
				return this.parameter != null && this.parameter.HasDefault;
			}
			set
			{
				this.Parameter.HasDefault = value;
			}
		}

		// Token: 0x17000191 RID: 401
		// (get) Token: 0x06000989 RID: 2441 RVA: 0x00023A66 File Offset: 0x00021C66
		// (set) Token: 0x0600098A RID: 2442 RVA: 0x00023A7D File Offset: 0x00021C7D
		public bool HasConstant
		{
			get
			{
				return this.parameter != null && this.parameter.HasConstant;
			}
			set
			{
				this.Parameter.HasConstant = value;
			}
		}

		// Token: 0x17000192 RID: 402
		// (get) Token: 0x0600098B RID: 2443 RVA: 0x00023A8B File Offset: 0x00021C8B
		// (set) Token: 0x0600098C RID: 2444 RVA: 0x00023A98 File Offset: 0x00021C98
		public object Constant
		{
			get
			{
				return this.Parameter.Constant;
			}
			set
			{
				this.Parameter.Constant = value;
			}
		}

		// Token: 0x17000193 RID: 403
		// (get) Token: 0x0600098D RID: 2445 RVA: 0x00023AA6 File Offset: 0x00021CA6
		// (set) Token: 0x0600098E RID: 2446 RVA: 0x00023ABD File Offset: 0x00021CBD
		public bool HasFieldMarshal
		{
			get
			{
				return this.parameter != null && this.parameter.HasFieldMarshal;
			}
			set
			{
				this.Parameter.HasFieldMarshal = value;
			}
		}

		// Token: 0x17000194 RID: 404
		// (get) Token: 0x0600098F RID: 2447 RVA: 0x00023ACB File Offset: 0x00021CCB
		public bool HasMarshalInfo
		{
			get
			{
				return this.parameter != null && this.parameter.HasMarshalInfo;
			}
		}

		// Token: 0x17000195 RID: 405
		// (get) Token: 0x06000990 RID: 2448 RVA: 0x00023AE2 File Offset: 0x00021CE2
		// (set) Token: 0x06000991 RID: 2449 RVA: 0x00023AEF File Offset: 0x00021CEF
		public MarshalInfo MarshalInfo
		{
			get
			{
				return this.Parameter.MarshalInfo;
			}
			set
			{
				this.Parameter.MarshalInfo = value;
			}
		}

		// Token: 0x06000992 RID: 2450 RVA: 0x00023AFD File Offset: 0x00021CFD
		public MethodReturnType(IMethodSignature method)
		{
			this.method = method;
		}

		// Token: 0x04000382 RID: 898
		internal IMethodSignature method;

		// Token: 0x04000383 RID: 899
		internal ParameterDefinition parameter;

		// Token: 0x04000384 RID: 900
		private TypeReference return_type;
	}
}
