using System;
using System.Runtime.CompilerServices;
using System.Runtime.ConstrainedExecution;
using System.Security;
using System.Security.Authentication.ExtendedProtection;

namespace System.Net
{
	// Token: 0x02000203 RID: 515
	[SuppressUnmanagedCodeSecurity]
	internal abstract class SafeFreeContextBufferChannelBinding : ChannelBinding
	{
		// Token: 0x1700041A RID: 1050
		// (get) Token: 0x06001358 RID: 4952 RVA: 0x00065ED2 File Offset: 0x000640D2
		public override int Size
		{
			get
			{
				return this.size;
			}
		}

		// Token: 0x06001359 RID: 4953 RVA: 0x00065EDA File Offset: 0x000640DA
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		internal void Set(IntPtr value)
		{
			this.handle = value;
		}

		// Token: 0x0600135A RID: 4954 RVA: 0x00065EE3 File Offset: 0x000640E3
		internal static SafeFreeContextBufferChannelBinding CreateEmptyHandle(SecurDll dll)
		{
			if (dll == SecurDll.SECURITY)
			{
				return new SafeFreeContextBufferChannelBinding_SECURITY();
			}
			throw new ArgumentException(SR.GetString("net_invalid_enum", new object[] { "SecurDll" }), "dll");
		}

		// Token: 0x0600135B RID: 4955 RVA: 0x00065F10 File Offset: 0x00064110
		public unsafe static int QueryContextChannelBinding(SecurDll dll, SafeDeleteContext phContext, ContextAttribute contextAttribute, Bindings* buffer, SafeFreeContextBufferChannelBinding refHandle)
		{
			if (dll == SecurDll.SECURITY)
			{
				return SafeFreeContextBufferChannelBinding.QueryContextChannelBinding_SECURITY(phContext, contextAttribute, buffer, refHandle);
			}
			return -1;
		}

		// Token: 0x0600135C RID: 4956 RVA: 0x00065F24 File Offset: 0x00064124
		private unsafe static int QueryContextChannelBinding_SECURITY(SafeDeleteContext phContext, ContextAttribute contextAttribute, Bindings* buffer, SafeFreeContextBufferChannelBinding refHandle)
		{
			int num = -2146893055;
			bool flag = false;
			if (contextAttribute != ContextAttribute.EndpointBindings && contextAttribute != ContextAttribute.UniqueBindings)
			{
				return num;
			}
			RuntimeHelpers.PrepareConstrainedRegions();
			try
			{
				phContext.DangerousAddRef(ref flag);
			}
			catch (Exception ex)
			{
				if (flag)
				{
					phContext.DangerousRelease();
					flag = false;
				}
				if (!(ex is ObjectDisposedException))
				{
					throw;
				}
			}
			finally
			{
				if (flag)
				{
					num = UnsafeNclNativeMethods.SafeNetHandles_SECURITY.QueryContextAttributesW(ref phContext._handle, contextAttribute, (void*)buffer);
					phContext.DangerousRelease();
				}
				if (num == 0 && refHandle != null)
				{
					refHandle.Set(buffer->pBindings);
					refHandle.size = buffer->BindingsLength;
				}
				if (num != 0 && refHandle != null)
				{
					refHandle.SetHandleAsInvalid();
				}
			}
			return num;
		}

		// Token: 0x04001554 RID: 5460
		private int size;
	}
}
