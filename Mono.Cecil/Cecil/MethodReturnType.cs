using System;
using System.Threading;
using Mono.Collections.Generic;

namespace Mono.Cecil
{
	// Token: 0x0200008C RID: 140
	public sealed class MethodReturnType : IConstantProvider, IMetadataTokenProvider, ICustomAttributeProvider, IMarshalInfoProvider
	{
		// Token: 0x1700016E RID: 366
		// (get) Token: 0x060005E2 RID: 1506 RVA: 0x000150D9 File Offset: 0x000132D9
		public IMethodSignature Method
		{
			get
			{
				return this.method;
			}
		}

		// Token: 0x1700016F RID: 367
		// (get) Token: 0x060005E3 RID: 1507 RVA: 0x000150E1 File Offset: 0x000132E1
		// (set) Token: 0x060005E4 RID: 1508 RVA: 0x000150E9 File Offset: 0x000132E9
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

		// Token: 0x17000170 RID: 368
		// (get) Token: 0x060005E5 RID: 1509 RVA: 0x000150F2 File Offset: 0x000132F2
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

		// Token: 0x17000171 RID: 369
		// (get) Token: 0x060005E6 RID: 1510 RVA: 0x00015120 File Offset: 0x00013320
		// (set) Token: 0x060005E7 RID: 1511 RVA: 0x0001512D File Offset: 0x0001332D
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

		// Token: 0x17000172 RID: 370
		// (get) Token: 0x060005E8 RID: 1512 RVA: 0x0001513B File Offset: 0x0001333B
		// (set) Token: 0x060005E9 RID: 1513 RVA: 0x00015148 File Offset: 0x00013348
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

		// Token: 0x17000173 RID: 371
		// (get) Token: 0x060005EA RID: 1514 RVA: 0x00015156 File Offset: 0x00013356
		// (set) Token: 0x060005EB RID: 1515 RVA: 0x00015163 File Offset: 0x00013363
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

		// Token: 0x17000174 RID: 372
		// (get) Token: 0x060005EC RID: 1516 RVA: 0x00015171 File Offset: 0x00013371
		public bool HasCustomAttributes
		{
			get
			{
				return this.parameter != null && this.parameter.HasCustomAttributes;
			}
		}

		// Token: 0x17000175 RID: 373
		// (get) Token: 0x060005ED RID: 1517 RVA: 0x00015188 File Offset: 0x00013388
		public Collection<CustomAttribute> CustomAttributes
		{
			get
			{
				return this.Parameter.CustomAttributes;
			}
		}

		// Token: 0x17000176 RID: 374
		// (get) Token: 0x060005EE RID: 1518 RVA: 0x00015195 File Offset: 0x00013395
		// (set) Token: 0x060005EF RID: 1519 RVA: 0x000151AC File Offset: 0x000133AC
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

		// Token: 0x17000177 RID: 375
		// (get) Token: 0x060005F0 RID: 1520 RVA: 0x000151BA File Offset: 0x000133BA
		// (set) Token: 0x060005F1 RID: 1521 RVA: 0x000151D1 File Offset: 0x000133D1
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

		// Token: 0x17000178 RID: 376
		// (get) Token: 0x060005F2 RID: 1522 RVA: 0x000151DF File Offset: 0x000133DF
		// (set) Token: 0x060005F3 RID: 1523 RVA: 0x000151EC File Offset: 0x000133EC
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

		// Token: 0x17000179 RID: 377
		// (get) Token: 0x060005F4 RID: 1524 RVA: 0x000151FA File Offset: 0x000133FA
		// (set) Token: 0x060005F5 RID: 1525 RVA: 0x00015211 File Offset: 0x00013411
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

		// Token: 0x1700017A RID: 378
		// (get) Token: 0x060005F6 RID: 1526 RVA: 0x0001521F File Offset: 0x0001341F
		public bool HasMarshalInfo
		{
			get
			{
				return this.parameter != null && this.parameter.HasMarshalInfo;
			}
		}

		// Token: 0x1700017B RID: 379
		// (get) Token: 0x060005F7 RID: 1527 RVA: 0x00015236 File Offset: 0x00013436
		// (set) Token: 0x060005F8 RID: 1528 RVA: 0x00015243 File Offset: 0x00013443
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

		// Token: 0x060005F9 RID: 1529 RVA: 0x00015251 File Offset: 0x00013451
		public MethodReturnType(IMethodSignature method)
		{
			this.method = method;
		}

		// Token: 0x04000162 RID: 354
		internal IMethodSignature method;

		// Token: 0x04000163 RID: 355
		internal ParameterDefinition parameter;

		// Token: 0x04000164 RID: 356
		private TypeReference return_type;
	}
}
