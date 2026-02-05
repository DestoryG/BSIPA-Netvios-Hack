using System;

namespace System.ComponentModel
{
	// Token: 0x0200053E RID: 1342
	[AttributeUsage(AttributeTargets.All)]
	[global::__DynamicallyInvokable]
	public class DefaultValueAttribute : Attribute
	{
		// Token: 0x06003283 RID: 12931 RVA: 0x000E20CC File Offset: 0x000E02CC
		[global::__DynamicallyInvokable]
		public DefaultValueAttribute(Type type, string value)
		{
			try
			{
				this.value = TypeDescriptor.GetConverter(type).ConvertFromInvariantString(value);
			}
			catch
			{
			}
		}

		// Token: 0x06003284 RID: 12932 RVA: 0x000E2108 File Offset: 0x000E0308
		[global::__DynamicallyInvokable]
		public DefaultValueAttribute(char value)
		{
			this.value = value;
		}

		// Token: 0x06003285 RID: 12933 RVA: 0x000E211C File Offset: 0x000E031C
		[global::__DynamicallyInvokable]
		public DefaultValueAttribute(byte value)
		{
			this.value = value;
		}

		// Token: 0x06003286 RID: 12934 RVA: 0x000E2130 File Offset: 0x000E0330
		[global::__DynamicallyInvokable]
		public DefaultValueAttribute(short value)
		{
			this.value = value;
		}

		// Token: 0x06003287 RID: 12935 RVA: 0x000E2144 File Offset: 0x000E0344
		[global::__DynamicallyInvokable]
		public DefaultValueAttribute(int value)
		{
			this.value = value;
		}

		// Token: 0x06003288 RID: 12936 RVA: 0x000E2158 File Offset: 0x000E0358
		[global::__DynamicallyInvokable]
		public DefaultValueAttribute(long value)
		{
			this.value = value;
		}

		// Token: 0x06003289 RID: 12937 RVA: 0x000E216C File Offset: 0x000E036C
		[global::__DynamicallyInvokable]
		public DefaultValueAttribute(float value)
		{
			this.value = value;
		}

		// Token: 0x0600328A RID: 12938 RVA: 0x000E2180 File Offset: 0x000E0380
		[global::__DynamicallyInvokable]
		public DefaultValueAttribute(double value)
		{
			this.value = value;
		}

		// Token: 0x0600328B RID: 12939 RVA: 0x000E2194 File Offset: 0x000E0394
		[global::__DynamicallyInvokable]
		public DefaultValueAttribute(bool value)
		{
			this.value = value;
		}

		// Token: 0x0600328C RID: 12940 RVA: 0x000E21A8 File Offset: 0x000E03A8
		[global::__DynamicallyInvokable]
		public DefaultValueAttribute(string value)
		{
			this.value = value;
		}

		// Token: 0x0600328D RID: 12941 RVA: 0x000E21B7 File Offset: 0x000E03B7
		[global::__DynamicallyInvokable]
		public DefaultValueAttribute(object value)
		{
			this.value = value;
		}

		// Token: 0x17000C5E RID: 3166
		// (get) Token: 0x0600328E RID: 12942 RVA: 0x000E21C6 File Offset: 0x000E03C6
		[global::__DynamicallyInvokable]
		public virtual object Value
		{
			[global::__DynamicallyInvokable]
			get
			{
				return this.value;
			}
		}

		// Token: 0x0600328F RID: 12943 RVA: 0x000E21D0 File Offset: 0x000E03D0
		[global::__DynamicallyInvokable]
		public override bool Equals(object obj)
		{
			if (obj == this)
			{
				return true;
			}
			DefaultValueAttribute defaultValueAttribute = obj as DefaultValueAttribute;
			if (defaultValueAttribute == null)
			{
				return false;
			}
			if (this.Value != null)
			{
				return this.Value.Equals(defaultValueAttribute.Value);
			}
			return defaultValueAttribute.Value == null;
		}

		// Token: 0x06003290 RID: 12944 RVA: 0x000E2212 File Offset: 0x000E0412
		[global::__DynamicallyInvokable]
		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		// Token: 0x06003291 RID: 12945 RVA: 0x000E221A File Offset: 0x000E041A
		protected void SetValue(object value)
		{
			this.value = value;
		}

		// Token: 0x04002976 RID: 10614
		private object value;
	}
}
