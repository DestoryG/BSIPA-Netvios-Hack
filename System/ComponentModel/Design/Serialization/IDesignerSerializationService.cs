using System;
using System.Collections;

namespace System.ComponentModel.Design.Serialization
{
	// Token: 0x0200060D RID: 1549
	public interface IDesignerSerializationService
	{
		// Token: 0x060038C3 RID: 14531
		ICollection Deserialize(object serializationData);

		// Token: 0x060038C4 RID: 14532
		object Serialize(ICollection objects);
	}
}
