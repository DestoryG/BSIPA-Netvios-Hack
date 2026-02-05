using System;
using Mono.Cecil.Metadata;

namespace Mono.Cecil
{
	// Token: 0x02000090 RID: 144
	public sealed class OptionalModifierType : TypeSpecification, IModifierType
	{
		// Token: 0x17000189 RID: 393
		// (get) Token: 0x0600060F RID: 1551 RVA: 0x0001531C File Offset: 0x0001351C
		// (set) Token: 0x06000610 RID: 1552 RVA: 0x00015324 File Offset: 0x00013524
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

		// Token: 0x1700018A RID: 394
		// (get) Token: 0x06000611 RID: 1553 RVA: 0x0001532D File Offset: 0x0001352D
		public override string Name
		{
			get
			{
				return base.Name + this.Suffix;
			}
		}

		// Token: 0x1700018B RID: 395
		// (get) Token: 0x06000612 RID: 1554 RVA: 0x00015340 File Offset: 0x00013540
		public override string FullName
		{
			get
			{
				return base.FullName + this.Suffix;
			}
		}

		// Token: 0x1700018C RID: 396
		// (get) Token: 0x06000613 RID: 1555 RVA: 0x00015353 File Offset: 0x00013553
		private string Suffix
		{
			get
			{
				return " modopt(" + this.modifier_type + ")";
			}
		}

		// Token: 0x1700018D RID: 397
		// (get) Token: 0x06000614 RID: 1556 RVA: 0x000026DB File Offset: 0x000008DB
		// (set) Token: 0x06000615 RID: 1557 RVA: 0x00002C55 File Offset: 0x00000E55
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

		// Token: 0x1700018E RID: 398
		// (get) Token: 0x06000616 RID: 1558 RVA: 0x00002BE8 File Offset: 0x00000DE8
		public override bool IsOptionalModifier
		{
			get
			{
				return true;
			}
		}

		// Token: 0x1700018F RID: 399
		// (get) Token: 0x06000617 RID: 1559 RVA: 0x0001536A File Offset: 0x0001356A
		public override bool ContainsGenericParameter
		{
			get
			{
				return this.modifier_type.ContainsGenericParameter || base.ContainsGenericParameter;
			}
		}

		// Token: 0x06000618 RID: 1560 RVA: 0x00015384 File Offset: 0x00013584
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

		// Token: 0x0400016E RID: 366
		private TypeReference modifier_type;
	}
}
