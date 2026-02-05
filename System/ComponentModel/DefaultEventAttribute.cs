using System;

namespace System.ComponentModel
{
	// Token: 0x0200053C RID: 1340
	[AttributeUsage(AttributeTargets.Class)]
	public sealed class DefaultEventAttribute : Attribute
	{
		// Token: 0x06003279 RID: 12921 RVA: 0x000E201A File Offset: 0x000E021A
		public DefaultEventAttribute(string name)
		{
			this.name = name;
		}

		// Token: 0x17000C5C RID: 3164
		// (get) Token: 0x0600327A RID: 12922 RVA: 0x000E2029 File Offset: 0x000E0229
		public string Name
		{
			get
			{
				return this.name;
			}
		}

		// Token: 0x0600327B RID: 12923 RVA: 0x000E2034 File Offset: 0x000E0234
		public override bool Equals(object obj)
		{
			DefaultEventAttribute defaultEventAttribute = obj as DefaultEventAttribute;
			return defaultEventAttribute != null && defaultEventAttribute.Name == this.name;
		}

		// Token: 0x0600327C RID: 12924 RVA: 0x000E205E File Offset: 0x000E025E
		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		// Token: 0x04002972 RID: 10610
		private readonly string name;

		// Token: 0x04002973 RID: 10611
		public static readonly DefaultEventAttribute Default = new DefaultEventAttribute(null);
	}
}
