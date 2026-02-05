using System;

namespace System.ComponentModel
{
	// Token: 0x0200050E RID: 1294
	[AttributeUsage(AttributeTargets.All)]
	public sealed class AmbientValueAttribute : Attribute
	{
		// Token: 0x06003107 RID: 12551 RVA: 0x000DE7FC File Offset: 0x000DC9FC
		public AmbientValueAttribute(Type type, string value)
		{
			try
			{
				this.value = TypeDescriptor.GetConverter(type).ConvertFromInvariantString(value);
			}
			catch
			{
			}
		}

		// Token: 0x06003108 RID: 12552 RVA: 0x000DE838 File Offset: 0x000DCA38
		public AmbientValueAttribute(char value)
		{
			this.value = value;
		}

		// Token: 0x06003109 RID: 12553 RVA: 0x000DE84C File Offset: 0x000DCA4C
		public AmbientValueAttribute(byte value)
		{
			this.value = value;
		}

		// Token: 0x0600310A RID: 12554 RVA: 0x000DE860 File Offset: 0x000DCA60
		public AmbientValueAttribute(short value)
		{
			this.value = value;
		}

		// Token: 0x0600310B RID: 12555 RVA: 0x000DE874 File Offset: 0x000DCA74
		public AmbientValueAttribute(int value)
		{
			this.value = value;
		}

		// Token: 0x0600310C RID: 12556 RVA: 0x000DE888 File Offset: 0x000DCA88
		public AmbientValueAttribute(long value)
		{
			this.value = value;
		}

		// Token: 0x0600310D RID: 12557 RVA: 0x000DE89C File Offset: 0x000DCA9C
		public AmbientValueAttribute(float value)
		{
			this.value = value;
		}

		// Token: 0x0600310E RID: 12558 RVA: 0x000DE8B0 File Offset: 0x000DCAB0
		public AmbientValueAttribute(double value)
		{
			this.value = value;
		}

		// Token: 0x0600310F RID: 12559 RVA: 0x000DE8C4 File Offset: 0x000DCAC4
		public AmbientValueAttribute(bool value)
		{
			this.value = value;
		}

		// Token: 0x06003110 RID: 12560 RVA: 0x000DE8D8 File Offset: 0x000DCAD8
		public AmbientValueAttribute(string value)
		{
			this.value = value;
		}

		// Token: 0x06003111 RID: 12561 RVA: 0x000DE8E7 File Offset: 0x000DCAE7
		public AmbientValueAttribute(object value)
		{
			this.value = value;
		}

		// Token: 0x17000BFF RID: 3071
		// (get) Token: 0x06003112 RID: 12562 RVA: 0x000DE8F6 File Offset: 0x000DCAF6
		public object Value
		{
			get
			{
				return this.value;
			}
		}

		// Token: 0x06003113 RID: 12563 RVA: 0x000DE900 File Offset: 0x000DCB00
		public override bool Equals(object obj)
		{
			if (obj == this)
			{
				return true;
			}
			AmbientValueAttribute ambientValueAttribute = obj as AmbientValueAttribute;
			if (ambientValueAttribute == null)
			{
				return false;
			}
			if (this.value != null)
			{
				return this.value.Equals(ambientValueAttribute.Value);
			}
			return ambientValueAttribute.Value == null;
		}

		// Token: 0x06003114 RID: 12564 RVA: 0x000DE942 File Offset: 0x000DCB42
		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		// Token: 0x040028FA RID: 10490
		private readonly object value;
	}
}
