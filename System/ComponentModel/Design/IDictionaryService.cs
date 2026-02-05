using System;

namespace System.ComponentModel.Design
{
	// Token: 0x020005EC RID: 1516
	public interface IDictionaryService
	{
		// Token: 0x0600381A RID: 14362
		object GetKey(object value);

		// Token: 0x0600381B RID: 14363
		object GetValue(object key);

		// Token: 0x0600381C RID: 14364
		void SetValue(object key, object value);
	}
}
