using System;
using System.Collections;

namespace System.ComponentModel.Design
{
	// Token: 0x020005FA RID: 1530
	public interface ITypeDiscoveryService
	{
		// Token: 0x06003857 RID: 14423
		ICollection GetTypes(Type baseType, bool excludeGlobalTypes);
	}
}
