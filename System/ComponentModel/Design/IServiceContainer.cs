using System;
using System.Runtime.InteropServices;

namespace System.ComponentModel.Design
{
	// Token: 0x020005F7 RID: 1527
	[ComVisible(true)]
	public interface IServiceContainer : IServiceProvider
	{
		// Token: 0x0600384C RID: 14412
		void AddService(Type serviceType, object serviceInstance);

		// Token: 0x0600384D RID: 14413
		void AddService(Type serviceType, object serviceInstance, bool promote);

		// Token: 0x0600384E RID: 14414
		void AddService(Type serviceType, ServiceCreatorCallback callback);

		// Token: 0x0600384F RID: 14415
		void AddService(Type serviceType, ServiceCreatorCallback callback, bool promote);

		// Token: 0x06003850 RID: 14416
		void RemoveService(Type serviceType);

		// Token: 0x06003851 RID: 14417
		void RemoveService(Type serviceType, bool promote);
	}
}
