using System;
using System.Globalization;
using System.Resources;

namespace System.ComponentModel.Design
{
	// Token: 0x020005F4 RID: 1524
	public interface IResourceService
	{
		// Token: 0x0600383E RID: 14398
		IResourceReader GetResourceReader(CultureInfo info);

		// Token: 0x0600383F RID: 14399
		IResourceWriter GetResourceWriter(CultureInfo info);
	}
}
