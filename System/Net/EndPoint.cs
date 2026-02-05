using System;
using System.Net.Sockets;

namespace System.Net
{
	// Token: 0x020000E3 RID: 227
	[global::__DynamicallyInvokable]
	[Serializable]
	public abstract class EndPoint
	{
		// Token: 0x17000169 RID: 361
		// (get) Token: 0x060007C9 RID: 1993 RVA: 0x0002B4A3 File Offset: 0x000296A3
		[global::__DynamicallyInvokable]
		public virtual AddressFamily AddressFamily
		{
			[global::__DynamicallyInvokable]
			get
			{
				throw ExceptionHelper.PropertyNotImplementedException;
			}
		}

		// Token: 0x060007CA RID: 1994 RVA: 0x0002B4AA File Offset: 0x000296AA
		[global::__DynamicallyInvokable]
		public virtual SocketAddress Serialize()
		{
			throw ExceptionHelper.MethodNotImplementedException;
		}

		// Token: 0x060007CB RID: 1995 RVA: 0x0002B4B1 File Offset: 0x000296B1
		[global::__DynamicallyInvokable]
		public virtual EndPoint Create(SocketAddress socketAddress)
		{
			throw ExceptionHelper.MethodNotImplementedException;
		}

		// Token: 0x060007CC RID: 1996 RVA: 0x0002B4B8 File Offset: 0x000296B8
		[global::__DynamicallyInvokable]
		protected EndPoint()
		{
		}
	}
}
