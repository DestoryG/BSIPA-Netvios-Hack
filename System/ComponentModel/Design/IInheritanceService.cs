using System;

namespace System.ComponentModel.Design
{
	// Token: 0x020005F1 RID: 1521
	public interface IInheritanceService
	{
		// Token: 0x0600382F RID: 14383
		void AddInheritedComponents(IComponent component, IContainer container);

		// Token: 0x06003830 RID: 14384
		InheritanceAttribute GetInheritanceAttribute(IComponent component);
	}
}
