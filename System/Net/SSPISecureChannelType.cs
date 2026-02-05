using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace System.Net
{
	// Token: 0x020001C6 RID: 454
	internal class SSPISecureChannelType : SSPIInterface
	{
		// Token: 0x170003E0 RID: 992
		// (get) Token: 0x060011EE RID: 4590 RVA: 0x00060294 File Offset: 0x0005E494
		// (set) Token: 0x060011EF RID: 4591 RVA: 0x0006029D File Offset: 0x0005E49D
		public SecurityPackageInfoClass[] SecurityPackages
		{
			get
			{
				return SSPISecureChannelType.m_SecurityPackages;
			}
			set
			{
				SSPISecureChannelType.m_SecurityPackages = value;
			}
		}

		// Token: 0x060011F0 RID: 4592 RVA: 0x000602A7 File Offset: 0x0005E4A7
		public int EnumerateSecurityPackages(out int pkgnum, out SafeFreeContextBuffer pkgArray)
		{
			return SafeFreeContextBuffer.EnumeratePackages(SSPISecureChannelType.Library, out pkgnum, out pkgArray);
		}

		// Token: 0x060011F1 RID: 4593 RVA: 0x000602B5 File Offset: 0x0005E4B5
		public int AcquireCredentialsHandle(string moduleName, CredentialUse usage, ref AuthIdentity authdata, out SafeFreeCredentials outCredential)
		{
			return SafeFreeCredentials.AcquireCredentialsHandle(SSPISecureChannelType.Library, moduleName, usage, ref authdata, out outCredential);
		}

		// Token: 0x060011F2 RID: 4594 RVA: 0x000602C6 File Offset: 0x0005E4C6
		public int AcquireCredentialsHandle(string moduleName, CredentialUse usage, ref SafeSspiAuthDataHandle authdata, out SafeFreeCredentials outCredential)
		{
			return SafeFreeCredentials.AcquireCredentialsHandle(moduleName, usage, ref authdata, out outCredential);
		}

		// Token: 0x060011F3 RID: 4595 RVA: 0x000602D2 File Offset: 0x0005E4D2
		public int AcquireDefaultCredential(string moduleName, CredentialUse usage, out SafeFreeCredentials outCredential)
		{
			return SafeFreeCredentials.AcquireDefaultCredential(SSPISecureChannelType.Library, moduleName, usage, out outCredential);
		}

		// Token: 0x060011F4 RID: 4596 RVA: 0x000602E1 File Offset: 0x0005E4E1
		public int AcquireCredentialsHandle(string moduleName, CredentialUse usage, ref SecureCredential authdata, out SafeFreeCredentials outCredential)
		{
			return SafeFreeCredentials.AcquireCredentialsHandle(SSPISecureChannelType.Library, moduleName, usage, ref authdata, out outCredential);
		}

		// Token: 0x060011F5 RID: 4597 RVA: 0x000602F2 File Offset: 0x0005E4F2
		public int AcquireCredentialsHandle(string moduleName, CredentialUse usage, ref SecureCredential2 authdata, out SafeFreeCredentials outCredential)
		{
			return SafeFreeCredentials.AcquireCredentialsHandle(SSPISecureChannelType.Library, moduleName, usage, ref authdata, out outCredential);
		}

		// Token: 0x060011F6 RID: 4598 RVA: 0x00060304 File Offset: 0x0005E504
		public int AcceptSecurityContext(ref SafeFreeCredentials credential, ref SafeDeleteContext context, SecurityBuffer inputBuffer, ContextFlags inFlags, Endianness endianness, SecurityBuffer outputBuffer, ref ContextFlags outFlags)
		{
			return SafeDeleteContext.AcceptSecurityContext(SSPISecureChannelType.Library, ref credential, ref context, inFlags, endianness, inputBuffer, null, outputBuffer, ref outFlags);
		}

		// Token: 0x060011F7 RID: 4599 RVA: 0x00060328 File Offset: 0x0005E528
		public int AcceptSecurityContext(SafeFreeCredentials credential, ref SafeDeleteContext context, SecurityBuffer[] inputBuffers, ContextFlags inFlags, Endianness endianness, SecurityBuffer outputBuffer, ref ContextFlags outFlags)
		{
			return SafeDeleteContext.AcceptSecurityContext(SSPISecureChannelType.Library, ref credential, ref context, inFlags, endianness, null, inputBuffers, outputBuffer, ref outFlags);
		}

		// Token: 0x060011F8 RID: 4600 RVA: 0x0006034C File Offset: 0x0005E54C
		public int InitializeSecurityContext(ref SafeFreeCredentials credential, ref SafeDeleteContext context, string targetName, ContextFlags inFlags, Endianness endianness, SecurityBuffer inputBuffer, SecurityBuffer outputBuffer, ref ContextFlags outFlags)
		{
			return SafeDeleteContext.InitializeSecurityContext(SSPISecureChannelType.Library, ref credential, ref context, targetName, inFlags, endianness, inputBuffer, null, outputBuffer, ref outFlags);
		}

		// Token: 0x060011F9 RID: 4601 RVA: 0x00060374 File Offset: 0x0005E574
		public int InitializeSecurityContext(SafeFreeCredentials credential, ref SafeDeleteContext context, string targetName, ContextFlags inFlags, Endianness endianness, SecurityBuffer[] inputBuffers, SecurityBuffer outputBuffer, ref ContextFlags outFlags)
		{
			return SafeDeleteContext.InitializeSecurityContext(SSPISecureChannelType.Library, ref credential, ref context, targetName, inFlags, endianness, null, inputBuffers, outputBuffer, ref outFlags);
		}

		// Token: 0x060011FA RID: 4602 RVA: 0x0006039C File Offset: 0x0005E59C
		public int EncryptMessage(SafeDeleteContext context, SecurityBufferDescriptor inputOutput, uint sequenceNumber)
		{
			int num = -2146893055;
			bool flag = false;
			RuntimeHelpers.PrepareConstrainedRegions();
			try
			{
				context.DangerousAddRef(ref flag);
			}
			catch (Exception ex)
			{
				if (flag)
				{
					context.DangerousRelease();
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
					num = UnsafeNclNativeMethods.NativeNTSSPI.EncryptMessage(ref context._handle, 0U, inputOutput, sequenceNumber);
					context.DangerousRelease();
				}
			}
			return num;
		}

		// Token: 0x060011FB RID: 4603 RVA: 0x00060410 File Offset: 0x0005E610
		public int DecryptMessage(SafeDeleteContext context, SecurityBufferDescriptor inputOutput, uint sequenceNumber)
		{
			int num = -2146893055;
			bool flag = false;
			RuntimeHelpers.PrepareConstrainedRegions();
			try
			{
				context.DangerousAddRef(ref flag);
			}
			catch (Exception ex)
			{
				if (flag)
				{
					context.DangerousRelease();
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
					num = UnsafeNclNativeMethods.NativeNTSSPI.DecryptMessage(ref context._handle, inputOutput, sequenceNumber, null);
					context.DangerousRelease();
				}
			}
			return num;
		}

		// Token: 0x060011FC RID: 4604 RVA: 0x00060484 File Offset: 0x0005E684
		public int MakeSignature(SafeDeleteContext context, SecurityBufferDescriptor inputOutput, uint sequenceNumber)
		{
			throw ExceptionHelper.MethodNotSupportedException;
		}

		// Token: 0x060011FD RID: 4605 RVA: 0x0006048B File Offset: 0x0005E68B
		public int VerifySignature(SafeDeleteContext context, SecurityBufferDescriptor inputOutput, uint sequenceNumber)
		{
			throw ExceptionHelper.MethodNotSupportedException;
		}

		// Token: 0x060011FE RID: 4606 RVA: 0x00060494 File Offset: 0x0005E694
		public unsafe int QueryContextChannelBinding(SafeDeleteContext phContext, ContextAttribute attribute, out SafeFreeContextBufferChannelBinding refHandle)
		{
			refHandle = SafeFreeContextBufferChannelBinding.CreateEmptyHandle(SSPISecureChannelType.Library);
			Bindings bindings = default(Bindings);
			return SafeFreeContextBufferChannelBinding.QueryContextChannelBinding(SSPISecureChannelType.Library, phContext, attribute, &bindings, refHandle);
		}

		// Token: 0x060011FF RID: 4607 RVA: 0x000604C8 File Offset: 0x0005E6C8
		public unsafe int QueryContextAttributes(SafeDeleteContext phContext, ContextAttribute attribute, byte[] buffer, Type handleType, out SafeHandle refHandle)
		{
			refHandle = null;
			if (handleType != null)
			{
				if (handleType == typeof(SafeFreeContextBuffer))
				{
					refHandle = SafeFreeContextBuffer.CreateEmptyHandle(SSPISecureChannelType.Library);
				}
				else
				{
					if (!(handleType == typeof(SafeFreeCertContext)))
					{
						throw new ArgumentException(SR.GetString("SSPIInvalidHandleType", new object[] { handleType.FullName }), "handleType");
					}
					refHandle = new SafeFreeCertContext();
				}
			}
			byte* ptr;
			if (buffer == null || buffer.Length == 0)
			{
				ptr = null;
			}
			else
			{
				ptr = &buffer[0];
			}
			return SafeFreeContextBuffer.QueryContextAttributes(SSPISecureChannelType.Library, phContext, attribute, ptr, refHandle);
		}

		// Token: 0x06001200 RID: 4608 RVA: 0x0006056F File Offset: 0x0005E76F
		public int SetContextAttributes(SafeDeleteContext phContext, ContextAttribute attribute, byte[] buffer)
		{
			return SafeFreeContextBuffer.SetContextAttributes(SSPISecureChannelType.Library, phContext, attribute, buffer);
		}

		// Token: 0x06001201 RID: 4609 RVA: 0x0006057E File Offset: 0x0005E77E
		public int QuerySecurityContextToken(SafeDeleteContext phContext, out SafeCloseHandle phToken)
		{
			throw new NotSupportedException();
		}

		// Token: 0x06001202 RID: 4610 RVA: 0x00060585 File Offset: 0x0005E785
		public int CompleteAuthToken(ref SafeDeleteContext refContext, SecurityBuffer[] inputBuffers)
		{
			throw new NotSupportedException();
		}

		// Token: 0x06001203 RID: 4611 RVA: 0x0006058C File Offset: 0x0005E78C
		public int ApplyControlToken(ref SafeDeleteContext refContext, SecurityBuffer[] inputBuffers)
		{
			return SafeDeleteContext.ApplyControlToken(SSPISecureChannelType.Library, ref refContext, inputBuffers);
		}

		// Token: 0x04001474 RID: 5236
		private static readonly SecurDll Library;

		// Token: 0x04001475 RID: 5237
		private static volatile SecurityPackageInfoClass[] m_SecurityPackages;
	}
}
