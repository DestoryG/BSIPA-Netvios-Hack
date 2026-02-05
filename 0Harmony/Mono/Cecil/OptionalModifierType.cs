using System;
using Mono.Cecil.Metadata;

namespace Mono.Cecil
{
	// Token: 0x02000148 RID: 328
	internal sealed class OptionalModifierType : TypeSpecification, IModifierType
	{
		// Token: 0x170001A3 RID: 419
		// (get) Token: 0x060009A8 RID: 2472 RVA: 0x00023BC8 File Offset: 0x00021DC8
		// (set) Token: 0x060009A9 RID: 2473 RVA: 0x00023BD0 File Offset: 0x00021DD0
		public TypeReference ModifierType
		{
			get
			{
				return this.modifier_type;
			}
			set
			{
				this.modifier_type = value;
			}
		}

		// Token: 0x170001A4 RID: 420
		// (get) Token: 0x060009AA RID: 2474 RVA: 0x00023BD9 File Offset: 0x00021DD9
		public override string Name
		{
			get
			{
				return base.Name + this.Suffix;
			}
		}

		// Token: 0x170001A5 RID: 421
		// (get) Token: 0x060009AB RID: 2475 RVA: 0x00023BEC File Offset: 0x00021DEC
		public override string FullName
		{
			get
			{
				return base.FullName + this.Suffix;
			}
		}

		// Token: 0x170001A6 RID: 422
		// (get) Token: 0x060009AC RID: 2476 RVA: 0x00023BFF File Offset: 0x00021DFF
		private string Suffix
		{
			get
			{
				string text = " modopt(";
				TypeReference typeReference = this.modifier_type;
				return text + ((typeReference != null) ? typeReference.ToString() : null) + ")";
			}
		}

		// Token: 0x170001A7 RID: 423
		// (get) Token: 0x060009AD RID: 2477 RVA: 0x00010910 File Offset: 0x0000EB10
		// (set) Token: 0x060009AE RID: 2478 RVA: 0x00010FA6 File Offset: 0x0000F1A6
		public override bool IsValueType
		{
			get
			{
				return false;
			}
			set
			{
				throw new InvalidOperationException();
			}
		}

		// Token: 0x170001A8 RID: 424
		// (get) Token: 0x060009AF RID: 2479 RVA: 0x00010F39 File Offset: 0x0000F139
		public override bool IsOptionalModifier
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170001A9 RID: 425
		// (get) Token: 0x060009B0 RID: 2480 RVA: 0x00023C22 File Offset: 0x00021E22
		public override bool ContainsGenericParameter
		{
			get
			{
				return this.modifier_type.ContainsGenericParameter || base.ContainsGenericParameter;
			}
		}

		// Token: 0x060009B1 RID: 2481 RVA: 0x00023C3C File Offset: 0x00021E3C
		public OptionalModifierType(TypeReference modifierType, TypeReference type)
			: base(type)
		{
			if (modifierType == null)
			{
				throw new ArgumentNullException(Mixin.Argument.modifierType.ToString());
			}
			Mixin.CheckType(type);
			this.modifier_type = modifierType;
			this.etype = Mono.Cecil.Metadata.ElementType.CModOpt;
		}

		// Token: 0x0400038E RID: 910
		private TypeReference modifier_type;
	}
}
