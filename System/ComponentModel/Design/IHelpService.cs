using System;

namespace System.ComponentModel.Design
{
	// Token: 0x020005F0 RID: 1520
	public interface IHelpService
	{
		// Token: 0x06003828 RID: 14376
		void AddContextAttribute(string name, string value, HelpKeywordType keywordType);

		// Token: 0x06003829 RID: 14377
		void ClearContextAttributes();

		// Token: 0x0600382A RID: 14378
		IHelpService CreateLocalContext(HelpContextType contextType);

		// Token: 0x0600382B RID: 14379
		void RemoveContextAttribute(string name, string value);

		// Token: 0x0600382C RID: 14380
		void RemoveLocalContext(IHelpService localContext);

		// Token: 0x0600382D RID: 14381
		void ShowHelpFromKeyword(string helpKeyword);

		// Token: 0x0600382E RID: 14382
		void ShowHelpFromUrl(string helpUrl);
	}
}
