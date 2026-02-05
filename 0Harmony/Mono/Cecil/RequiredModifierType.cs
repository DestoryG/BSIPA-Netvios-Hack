using System;
using Mono.Cecil.Metadata;

namespace Mono.Cecil
{
	// Token: 0x02000149 RID: 329
	internal sealed class RequiredModifierType : TypeSpecification, IModifierType
	{
		// Token: 0x170001AA RID: 426
		// (get) Token: 0x060009B2 RID: 2482 RVA: 0x00023C7E File Offset: 0x00021E7E
		// (set) Token: 0x060009B3 RID: 2483 RVA: 0x00023C86 File Offset: 0x00021E86
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

		// Token: 0x170001AB RID: 427
		// (get) Token: 0x060009B4 RID: 2484 RVA: 0x00023C8F File Offset: 0x00021E8F
		public override string Name
		{
			get
			{
				return base.Name + this.Suffix;
			}
		}

		// Token: 0x170001AC RID: 428
		// (get) Token: 0x060009B5 RID: 2485 RVA: 0x00023CA2 File Offset: 0x00021EA2
		public override string FullName
		{
			get
			{
				return base.FullName + this.Suffix;
			}
		}

		// Token: 0x170001AD RID: 429
		// (get) Token: 0x060009B6 RID: 2486 RVA: 0x00023CB5 File Offset: 0x00021EB5
		private string Suffix
		{
			get
			{
				string text = " modreq(";
				TypeReference typeReference = this.modifier_type;
				return text + ((typeReference != null) ? typeReference.ToString() : null) + ")";
			}
		}

		// Token: 0x170001AE RID: 430
		// (get) Token: 0x060009B7 RID: 2487 RVA: 0x00010910 File Offset: 0x0000EB10
		// (set) Token: 0x060009B8 RID: 2488 RVA: 0x00010FA6 File Offset: 0x0000F1A6
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

		// Token: 0x170001AF RID: 431
		// (get) Token: 0x060009B9 RID: 2489 RVA: 0x00010F39 File Offset: 0x0000F139
		public override bool IsRequiredModifier
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170001B0 RID: 432
		// (get) Token: 0x060009BA RID: 2490 RVA: 0x00023CD8 File Offset: 0x00021ED8
		public override bool ContainsGenericParameter
		{
			get
			{
				return this.modifier_type.ContainsGenericParameter || base.ContainsGenericParameter;
			}
		}

		// Token: 0x060009BB RID: 2491 RVA: 0x00023CF0 File Offset: 0x00021EF0
		public RequiredModifierType(TypeReference modifierType, TypeReference type)
			: base(type)
		{
			if (modifierType == null)
			{
				throw new ArgumentNullException(Mixin.Argument.modifierType.ToString());
			}
			Mixin.CheckType(type);
			this.modifier_type = modifierType;
			this.etype = Mono.Cecil.Metadata.ElementType.CModReqD;
		}

		// Token: 0x0400038F RID: 911
		private TypeReference modifier_type;
	}
}
