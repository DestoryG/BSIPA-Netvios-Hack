using System;
using System.Collections;
using System.ComponentModel;
using System.Net;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Security.Permissions;
using System.Text;

namespace System.Security.Authentication.ExtendedProtection
{
	// Token: 0x02000442 RID: 1090
	[TypeConverter(typeof(ExtendedProtectionPolicyTypeConverter))]
	[Serializable]
	public class ExtendedProtectionPolicy : ISerializable
	{
		// Token: 0x06002881 RID: 10369 RVA: 0x000B9E34 File Offset: 0x000B8034
		public ExtendedProtectionPolicy(PolicyEnforcement policyEnforcement, ProtectionScenario protectionScenario, ServiceNameCollection customServiceNames)
		{
			if (policyEnforcement == PolicyEnforcement.Never)
			{
				throw new ArgumentException(SR.GetString("security_ExtendedProtectionPolicy_UseDifferentConstructorForNever"), "policyEnforcement");
			}
			if (customServiceNames != null && customServiceNames.Count == 0)
			{
				throw new ArgumentException(SR.GetString("security_ExtendedProtectionPolicy_NoEmptyServiceNameCollection"), "customServiceNames");
			}
			this.policyEnforcement = policyEnforcement;
			this.protectionScenario = protectionScenario;
			this.customServiceNames = customServiceNames;
		}

		// Token: 0x06002882 RID: 10370 RVA: 0x000B9E94 File Offset: 0x000B8094
		public ExtendedProtectionPolicy(PolicyEnforcement policyEnforcement, ProtectionScenario protectionScenario, ICollection customServiceNames)
			: this(policyEnforcement, protectionScenario, (customServiceNames == null) ? null : new ServiceNameCollection(customServiceNames))
		{
		}

		// Token: 0x06002883 RID: 10371 RVA: 0x000B9EAC File Offset: 0x000B80AC
		public ExtendedProtectionPolicy(PolicyEnforcement policyEnforcement, ChannelBinding customChannelBinding)
		{
			if (policyEnforcement == PolicyEnforcement.Never)
			{
				throw new ArgumentException(SR.GetString("security_ExtendedProtectionPolicy_UseDifferentConstructorForNever"), "policyEnforcement");
			}
			if (customChannelBinding == null)
			{
				throw new ArgumentNullException("customChannelBinding");
			}
			this.policyEnforcement = policyEnforcement;
			this.protectionScenario = ProtectionScenario.TransportSelected;
			this.customChannelBinding = customChannelBinding;
		}

		// Token: 0x06002884 RID: 10372 RVA: 0x000B9EFA File Offset: 0x000B80FA
		public ExtendedProtectionPolicy(PolicyEnforcement policyEnforcement)
		{
			this.policyEnforcement = policyEnforcement;
			this.protectionScenario = ProtectionScenario.TransportSelected;
		}

		// Token: 0x06002885 RID: 10373 RVA: 0x000B9F10 File Offset: 0x000B8110
		protected ExtendedProtectionPolicy(SerializationInfo info, StreamingContext context)
		{
			this.policyEnforcement = (PolicyEnforcement)info.GetInt32("policyEnforcement");
			this.protectionScenario = (ProtectionScenario)info.GetInt32("protectionScenario");
			this.customServiceNames = (ServiceNameCollection)info.GetValue("customServiceNames", typeof(ServiceNameCollection));
			byte[] array = (byte[])info.GetValue("customChannelBinding", typeof(byte[]));
			if (array != null)
			{
				this.customChannelBinding = SafeLocalFreeChannelBinding.LocalAlloc(array.Length);
				Marshal.Copy(array, 0, this.customChannelBinding.DangerousGetHandle(), array.Length);
			}
		}

		// Token: 0x170009F2 RID: 2546
		// (get) Token: 0x06002886 RID: 10374 RVA: 0x000B9FA6 File Offset: 0x000B81A6
		public ServiceNameCollection CustomServiceNames
		{
			get
			{
				return this.customServiceNames;
			}
		}

		// Token: 0x170009F3 RID: 2547
		// (get) Token: 0x06002887 RID: 10375 RVA: 0x000B9FAE File Offset: 0x000B81AE
		public PolicyEnforcement PolicyEnforcement
		{
			get
			{
				return this.policyEnforcement;
			}
		}

		// Token: 0x170009F4 RID: 2548
		// (get) Token: 0x06002888 RID: 10376 RVA: 0x000B9FB6 File Offset: 0x000B81B6
		public ProtectionScenario ProtectionScenario
		{
			get
			{
				return this.protectionScenario;
			}
		}

		// Token: 0x170009F5 RID: 2549
		// (get) Token: 0x06002889 RID: 10377 RVA: 0x000B9FBE File Offset: 0x000B81BE
		public ChannelBinding CustomChannelBinding
		{
			get
			{
				return this.customChannelBinding;
			}
		}

		// Token: 0x0600288A RID: 10378 RVA: 0x000B9FC8 File Offset: 0x000B81C8
		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("ProtectionScenario=");
			stringBuilder.Append(this.protectionScenario.ToString());
			stringBuilder.Append("; PolicyEnforcement=");
			stringBuilder.Append(this.policyEnforcement.ToString());
			stringBuilder.Append("; CustomChannelBinding=");
			if (this.customChannelBinding == null)
			{
				stringBuilder.Append("<null>");
			}
			else
			{
				stringBuilder.Append(this.customChannelBinding.ToString());
			}
			stringBuilder.Append("; ServiceNames=");
			if (this.customServiceNames == null)
			{
				stringBuilder.Append("<null>");
			}
			else
			{
				bool flag = true;
				foreach (object obj in this.customServiceNames)
				{
					string text = (string)obj;
					if (flag)
					{
						flag = false;
					}
					else
					{
						stringBuilder.Append(", ");
					}
					stringBuilder.Append(text);
				}
			}
			return stringBuilder.ToString();
		}

		// Token: 0x170009F6 RID: 2550
		// (get) Token: 0x0600288B RID: 10379 RVA: 0x000BA0E4 File Offset: 0x000B82E4
		public static bool OSSupportsExtendedProtection
		{
			get
			{
				return AuthenticationManager.OSSupportsExtendedProtection;
			}
		}

		// Token: 0x0600288C RID: 10380 RVA: 0x000BA0EC File Offset: 0x000B82EC
		[SecurityPermission(SecurityAction.LinkDemand, SerializationFormatter = true)]
		void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
		{
			info.AddValue("policyEnforcement", (int)this.policyEnforcement);
			info.AddValue("protectionScenario", (int)this.protectionScenario);
			info.AddValue("customServiceNames", this.customServiceNames, typeof(ServiceNameCollection));
			if (this.customChannelBinding == null)
			{
				info.AddValue("customChannelBinding", null, typeof(byte[]));
				return;
			}
			byte[] array = new byte[this.customChannelBinding.Size];
			Marshal.Copy(this.customChannelBinding.DangerousGetHandle(), array, 0, this.customChannelBinding.Size);
			info.AddValue("customChannelBinding", array, typeof(byte[]));
		}

		// Token: 0x04002254 RID: 8788
		private const string policyEnforcementName = "policyEnforcement";

		// Token: 0x04002255 RID: 8789
		private const string protectionScenarioName = "protectionScenario";

		// Token: 0x04002256 RID: 8790
		private const string customServiceNamesName = "customServiceNames";

		// Token: 0x04002257 RID: 8791
		private const string customChannelBindingName = "customChannelBinding";

		// Token: 0x04002258 RID: 8792
		private ServiceNameCollection customServiceNames;

		// Token: 0x04002259 RID: 8793
		private PolicyEnforcement policyEnforcement;

		// Token: 0x0400225A RID: 8794
		private ProtectionScenario protectionScenario;

		// Token: 0x0400225B RID: 8795
		private ChannelBinding customChannelBinding;
	}
}
