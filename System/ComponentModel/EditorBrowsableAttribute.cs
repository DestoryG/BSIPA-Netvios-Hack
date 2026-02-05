using System;

namespace System.ComponentModel
{
	// Token: 0x0200054C RID: 1356
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Enum | AttributeTargets.Constructor | AttributeTargets.Method | AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Event | AttributeTargets.Interface | AttributeTargets.Delegate)]
	[global::__DynamicallyInvokable]
	public sealed class EditorBrowsableAttribute : Attribute
	{
		// Token: 0x060032EC RID: 13036 RVA: 0x000E29F7 File Offset: 0x000E0BF7
		[global::__DynamicallyInvokable]
		public EditorBrowsableAttribute(EditorBrowsableState state)
		{
			this.browsableState = state;
		}

		// Token: 0x060032ED RID: 13037 RVA: 0x000E2A06 File Offset: 0x000E0C06
		public EditorBrowsableAttribute()
			: this(EditorBrowsableState.Always)
		{
		}

		// Token: 0x17000C73 RID: 3187
		// (get) Token: 0x060032EE RID: 13038 RVA: 0x000E2A0F File Offset: 0x000E0C0F
		[global::__DynamicallyInvokable]
		public EditorBrowsableState State
		{
			[global::__DynamicallyInvokable]
			get
			{
				return this.browsableState;
			}
		}

		// Token: 0x060032EF RID: 13039 RVA: 0x000E2A18 File Offset: 0x000E0C18
		[global::__DynamicallyInvokable]
		public override bool Equals(object obj)
		{
			if (obj == this)
			{
				return true;
			}
			EditorBrowsableAttribute editorBrowsableAttribute = obj as EditorBrowsableAttribute;
			return editorBrowsableAttribute != null && editorBrowsableAttribute.browsableState == this.browsableState;
		}

		// Token: 0x060032F0 RID: 13040 RVA: 0x000E2A45 File Offset: 0x000E0C45
		[global::__DynamicallyInvokable]
		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		// Token: 0x0400299B RID: 10651
		private EditorBrowsableState browsableState;
	}
}
