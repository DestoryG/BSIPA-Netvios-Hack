using System;

namespace System.Configuration
{
	// Token: 0x020000A2 RID: 162
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Property)]
	public sealed class SettingsSerializeAsAttribute : Attribute
	{
		// Token: 0x060005A7 RID: 1447 RVA: 0x00022ABD File Offset: 0x00020CBD
		public SettingsSerializeAsAttribute(SettingsSerializeAs serializeAs)
		{
			this._serializeAs = serializeAs;
		}

		// Token: 0x170000DC RID: 220
		// (get) Token: 0x060005A8 RID: 1448 RVA: 0x00022ACC File Offset: 0x00020CCC
		public SettingsSerializeAs SerializeAs
		{
			get
			{
				return this._serializeAs;
			}
		}

		// Token: 0x04000C3D RID: 3133
		private readonly SettingsSerializeAs _serializeAs;
	}
}
