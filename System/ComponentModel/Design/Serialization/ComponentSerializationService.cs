using System;
using System.Collections;
using System.IO;
using System.Security.Permissions;

namespace System.ComponentModel.Design.Serialization
{
	// Token: 0x02000603 RID: 1539
	[HostProtection(SecurityAction.LinkDemand, SharedState = true)]
	public abstract class ComponentSerializationService
	{
		// Token: 0x06003889 RID: 14473
		public abstract SerializationStore CreateStore();

		// Token: 0x0600388A RID: 14474
		public abstract SerializationStore LoadStore(Stream stream);

		// Token: 0x0600388B RID: 14475
		public abstract void Serialize(SerializationStore store, object value);

		// Token: 0x0600388C RID: 14476
		public abstract void SerializeAbsolute(SerializationStore store, object value);

		// Token: 0x0600388D RID: 14477
		public abstract void SerializeMember(SerializationStore store, object owningObject, MemberDescriptor member);

		// Token: 0x0600388E RID: 14478
		public abstract void SerializeMemberAbsolute(SerializationStore store, object owningObject, MemberDescriptor member);

		// Token: 0x0600388F RID: 14479
		public abstract ICollection Deserialize(SerializationStore store);

		// Token: 0x06003890 RID: 14480
		public abstract ICollection Deserialize(SerializationStore store, IContainer container);

		// Token: 0x06003891 RID: 14481
		public abstract void DeserializeTo(SerializationStore store, IContainer container, bool validateRecycledTypes, bool applyDefaults);

		// Token: 0x06003892 RID: 14482 RVA: 0x000F1839 File Offset: 0x000EFA39
		public void DeserializeTo(SerializationStore store, IContainer container)
		{
			this.DeserializeTo(store, container, true, true);
		}

		// Token: 0x06003893 RID: 14483 RVA: 0x000F1845 File Offset: 0x000EFA45
		public void DeserializeTo(SerializationStore store, IContainer container, bool validateRecycledTypes)
		{
			this.DeserializeTo(store, container, validateRecycledTypes, true);
		}
	}
}
