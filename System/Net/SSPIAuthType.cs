using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace System.Net
{
	// Token: 0x020001C7 RID: 455
	internal class SSPIAuthType : SSPIInterface
	{
		// Token: 0x170003E1 RID: 993
		// (get) Token: 0x06001205 RID: 4613 RVA: 0x000605A2 File Offset: 0x0005E7A2
		// (set) Token: 0x06001206 RID: 4614 RVA: 0x000605AB File Offset: 0x0005E7AB
		public SecurityPackageInfoClass[] SecurityPackages
		{
			get
			{
				return SSPIAuthType.m_SecurityPackages;
			}
			set
			{
				SSPIAuthType.m_SecurityPackages = value;
			}
		}

		// Token: 0x06001207 RID: 4615 RVA: 0x000605B5 File Offset: 0x0005E7B5
		public int EnumerateSecurityPackages(out int pkgnum, out SafeFreeContextBuffer pkgArray)
		{
			return SafeFreeContextBuffer.EnumeratePackages(SSPIAuthType.Library, out pkgnum, out pkgArray);
		}

		// Token: 0x06001208 RID: 4616 RVA: 0x000605C3 File Offset: 0x0005E7C3
		public int AcquireCredentialsHandle(string moduleName, CredentialUse usage, ref AuthIdentity authdata, out SafeFreeCredentials outCredential)
		{
			return SafeFreeCredentials.AcquireCredentialsHandle(SSPIAuthType.Library, moduleName, usage, ref authdata, out outCredential);
		}

		// Token: 0x06001209 RID: 4617 RVA: 0x000605D4 File Offset: 0x0005E7D4
		public int AcquireCredentialsHandle(string moduleName, CredentialUse usage, ref SafeSspiAuthDataHandle authdata, out SafeFreeCredentials outCredential)
		{
			return SafeFreeCredentials.AcquireCredentialsHandle(moduleName, usage, ref authdata, out outCredential);
		}

		// Token: 0x0600120A RID: 4618 RVA: 0x000605E0 File Offset: 0x0005E7E0
		public int AcquireDefaultCredential(string moduleName, CredentialUse usage, out SafeFreeCredentials outCredential)
		{
			return SafeFreeCredentials.AcquireDefaultCredential(SSPIAuthType.Library, moduleName, usage, out outCredential);
		}

		// Token: 0x0600120B RID: 4619 RVA: 0x000605EF File Offset: 0x0005E7EF
		public int AcquireCredentialsHandle(string moduleName, CredentialUse usage, ref SecureCredential authdata, out SafeFreeCredentials outCredential)
		{
			return SafeFreeCredentials.AcquireCredentialsHandle(SSPIAuthType.Library, moduleName, usage, ref authdata, out outCredential);
		}

		// Token: 0x0600120C RID: 4620 RVA: 0x00060600 File Offset: 0x0005E800
		public int AcquireCredentialsHandle(string moduleName, CredentialUse usage, ref SecureCredential2 authdata, out SafeFreeCredentials outCredential)
		{
			return SafeFreeCredentials.AcquireCredentialsHandle(SSPIAuthType.Library, moduleName, usage, ref authdata, out outCredential);
		}

		// Token: 0x0600120D RID: 4621 RVA: 0x00060614 File Offset: 0x0005E814
		public int AcceptSecurityContext(ref SafeFreeCredentials credential, ref SafeDeleteContext context, SecurityBuffer inputBuffer, ContextFlags inFlags, Endianness endianness, SecurityBuffer outputBuffer, ref ContextFlags outFlags)
		{
			return SafeDeleteContext.AcceptSecurityContext(SSPIAuthType.Library, ref credential, ref context, inFlags, endianness, inputBuffer, null, outputBuffer, ref outFlags);
		}

		// Token: 0x0600120E RID: 4622 RVA: 0x00060638 File Offset: 0x0005E838
		public int AcceptSecurityContext(SafeFreeCredentials credential, ref SafeDeleteContext context, SecurityBuffer[] inputBuffers, ContextFlags inFlags, Endianness endianness, SecurityBuffer outputBuffer, ref ContextFlags outFlags)
		{
			return SafeDeleteContext.AcceptSecurityContext(SSPIAuthType.Library, ref credential, ref context, inFlags, endianness, null, inputBuffers, outputBuffer, ref outFlags);
		}

		// Token: 0x0600120F RID: 4623 RVA: 0x0006065C File Offset: 0x0005E85C
		public int InitializeSecurityContext(ref SafeFreeCredentials credential, ref SafeDeleteContext context, string targetName, ContextFlags inFlags, Endianness endianness, SecurityBuffer inputBuffer, SecurityBuffer outputBuffer, ref ContextFlags outFlags)
		{
			return SafeDeleteContext.InitializeSecurityContext(SSPIAuthType.Library, ref credential, ref context, targetName, inFlags, endianness, inputBuffer, null, outputBuffer, ref outFlags);
		}

		// Token: 0x06001210 RID: 4624 RVA: 0x00060684 File Offset: 0x0005E884
		public int InitializeSecurityContext(SafeFreeCredentials credential, ref SafeDeleteContext context, string targetName, ContextFlags inFlags, Endianness endianness, SecurityBuffer[] inputBuffers, SecurityBuffer outputBuffer, ref ContextFlags outFlags)
		{
			return SafeDeleteContext.InitializeSecurityContext(SSPIAuthType.Library, ref credential, ref context, targetName, inFlags, endianness, null, inputBuffers, outputBuffer, ref outFlags);
		}

		// Token: 0x06001211 RID: 4625 RVA: 0x000606AC File Offset: 0x0005E8AC
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

		// Token: 0x06001212 RID: 4626 RVA: 0x00060720 File Offset: 0x0005E920
		public unsafe int DecryptMessage(SafeDeleteContext context, SecurityBufferDescriptor inputOutput, uint sequenceNumber)
		{
			int num = -2146893055;
			bool flag = false;
			uint num2 = 0U;
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
					num = UnsafeNclNativeMethods.NativeNTSSPI.DecryptMessage(ref context._handle, inputOutput, sequenceNumber, &num2);
					context.DangerousRelease();
				}
			}
			if (num == 0 && num2 == 2147483649U)
			{
				throw new InvalidOperationException(SR.GetString("net_auth_message_not_encrypted"));
			}
			return num;
		}

		// Token: 0x06001213 RID: 4627 RVA: 0x000607B4 File Offset: 0x0005E9B4
		public int MakeSignature(SafeDeleteContext context, SecurityBufferDescriptor inputOutput, uint sequenceNumber)
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
					num = UnsafeNclNativeMethods.NativeNTSSPI.EncryptMessage(ref context._handle, 2147483649U, inputOutput, sequenceNumber);
					context.DangerousRelease();
				}
			}
			return num;
		}

		// Token: 0x06001214 RID: 4628 RVA: 0x0006082C File Offset: 0x0005EA2C
		public unsafe int VerifySignature(SafeDeleteContext context, SecurityBufferDescriptor inputOutput, uint sequenceNumber)
		{
			int num = -2146893055;
			bool flag = false;
			uint num2 = 0U;
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
					num = UnsafeNclNativeMethods.NativeNTSSPI.DecryptMessage(ref context._handle, inputOutput, sequenceNumber, &num2);
					context.DangerousRelease();
				}
			}
			return num;
		}

		// Token: 0x06001215 RID: 4629 RVA: 0x000608A4 File Offset: 0x0005EAA4
		public int QueryContextChannelBinding(SafeDeleteContext context, ContextAttribute attribute, out SafeFreeContextBufferChannelBinding binding)
		{
			binding = null;
			throw new NotSupportedException();
		}

		// Token: 0x06001216 RID: 4630 RVA: 0x000608B0 File Offset: 0x0005EAB0
		public unsafe int QueryContextAttributes(SafeDeleteContext context, ContextAttribute attribute, byte[] buffer, Type handleType, out SafeHandle refHandle)
		{
			refHandle = null;
			if (handleType != null)
			{
				if (handleType == typeof(SafeFreeContextBuffer))
				{
					refHandle = SafeFreeContextBuffer.CreateEmptyHandle(SSPIAuthType.Library);
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
			return SafeFreeContextBuffer.QueryContextAttributes(SSPIAuthType.Library, context, attribute, ptr, refHandle);
		}

		// Token: 0x06001217 RID: 4631 RVA: 0x00060957 File Offset: 0x0005EB57
		public int SetContextAttributes(SafeDeleteContext context, ContextAttribute attribute, byte[] buffer)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06001218 RID: 4632 RVA: 0x0006095E File Offset: 0x0005EB5E
		public int QuerySecurityContextToken(SafeDeleteContext phContext, out SafeCloseHandle phToken)
		{
			return SSPIAuthType.GetSecurityContextToken(phContext, out phToken);
		}

		// Token: 0x06001219 RID: 4633 RVA: 0x00060967 File Offset: 0x0005EB67
		public int CompleteAuthToken(ref SafeDeleteContext refContext, SecurityBuffer[] inputBuffers)
		{
			return SafeDeleteContext.CompleteAuthToken(SSPIAuthType.Library, ref refContext, inputBuffers);
		}

		// Token: 0x0600121A RID: 4634 RVA: 0x00060975 File Offset: 0x0005EB75
		public int ApplyControlToken(ref SafeDeleteContext refContext, SecurityBuffer[] inputBuffers)
		{
			throw new NotSupportedException();
		}

		// Token: 0x0600121B RID: 4635 RVA: 0x0006097C File Offset: 0x0005EB7C
		private static int GetSecurityContextToken(SafeDeleteContext phContext, out SafeCloseHandle safeHandle)
		{
			int num = -2146893055;
			bool flag = false;
			safeHandle = null;
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
					num = UnsafeNclNativeMethods.SafeNetHandles.QuerySecurityContextToken(ref phContext._handle, out safeHandle);
					phContext.DangerousRelease();
				}
			}
			return num;
		}

		// Token: 0x04001476 RID: 5238
		private static readonly SecurDll Library;

		// Token: 0x04001477 RID: 5239
		private static volatile SecurityPackageInfoClass[] m_SecurityPackages;
	}
}
