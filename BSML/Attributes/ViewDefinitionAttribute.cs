using System;

namespace BeatSaberMarkupLanguage.Attributes
{
	// Token: 0x020000BF RID: 191
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
	public sealed class ViewDefinitionAttribute : Attribute
	{
		// Token: 0x170000F8 RID: 248
		// (get) Token: 0x060003FB RID: 1019 RVA: 0x000126B5 File Offset: 0x000108B5
		public string Definition { get; }

		// Token: 0x060003FC RID: 1020 RVA: 0x000126BD File Offset: 0x000108BD
		public ViewDefinitionAttribute(string definition)
		{
			this.Definition = definition;
		}
	}
}
