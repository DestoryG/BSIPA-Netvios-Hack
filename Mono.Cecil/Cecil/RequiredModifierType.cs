using System;
using Mono.Cecil.Metadata;

namespace Mono.Cecil
{
	// Token: 0x02000091 RID: 145
	public sealed class RequiredModifierType : TypeSpecification, IModifierType
	{
		// Token: 0x17000190 RID: 400
		// (get) Token: 0x06000619 RID: 1561 RVA: 0x000153C6 File Offset: 0x000135C6
		// (set) Token: 0x0600061A RID: 1562 RVA: 0x000153CE File Offset: 0x000135CE
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

		// Token: 0x17000191 RID: 401
		// (get) Token: 0x0600061B RID: 1563 RVA: 0x000153D7 File Offset: 0x000135D7
		public override string Name
		{
			get
			{
				return base.Name + this.Suffix;
			}
		}

		// Token: 0x17000192 RID: 402
		// (get) Token: 0x0600061C RID: 1564 RVA: 0x000153EA File Offset: 0x000135EA
		public override string FullName
		{
			get
			{
				return base.FullName + this.Suffix;
			}
		}

		// Token: 0x17000193 RID: 403
		// (get) Token: 0x0600061D RID: 1565 RVA: 0x000153FD File Offset: 0x000135FD
		private string Suffix
		{
			get
			{
				return " modreq(" + this.modifier_type + ")";
			}
		}

		// Token: 0x17000194 RID: 404
		// (get) Token: 0x0600061E RID: 1566 RVA: 0x000026DB File Offset: 0x000008DB
		// (set) Token: 0x0600061F RID: 1567 RVA: 0x00002C55 File Offset: 0x00000E55
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

		// Token: 0x17000195 RID: 405
		// (get) Token: 0x06000620 RID: 1568 RVA: 0x00002BE8 File Offset: 0x00000DE8
		public override bool IsRequiredModifier
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000196 RID: 406
		// (get) Token: 0x06000621 RID: 1569 RVA: 0x00015414 File Offset: 0x00013614
		public override bool ContainsGenericParameter
		{
			get
			{
				return this.modifier_type.ContainsGenericParameter || base.ContainsGenericParameter;
			}
		}

		// Token: 0x06000622 RID: 1570 RVA: 0x0001542C File Offset: 0x0001362C
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

		// Token: 0x0400016F RID: 367
		private TypeReference modifier_type;
	}
}
