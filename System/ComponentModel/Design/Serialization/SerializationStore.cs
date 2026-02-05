using System;
using System.Collections;
using System.IO;
using System.Security.Permissions;

namespace System.ComponentModel.Design.Serialization
{
	// Token: 0x02000615 RID: 1557
	[HostProtection(SecurityAction.LinkDemand, SharedState = true)]
	public abstract class SerializationStore : IDisposable
	{
		// Token: 0x17000DA0 RID: 3488
		// (get) Token: 0x060038EE RID: 14574
		public abstract ICollection Errors { get; }

		// Token: 0x060038EF RID: 14575
		public abstract void Close();

		// Token: 0x060038F0 RID: 14576
		public abstract void Save(Stream stream);

		// Token: 0x060038F1 RID: 14577 RVA: 0x000F213B File Offset: 0x000F033B
		void IDisposable.Dispose()
		{
			this.Dispose(true);
		}

		// Token: 0x060038F2 RID: 14578 RVA: 0x000F2144 File Offset: 0x000F0344
		protected virtual void Dispose(bool disposing)
		{
			if (disposing)
			{
				this.Close();
			}
		}
	}
}
