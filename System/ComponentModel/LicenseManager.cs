using System;
using System.Collections;
using System.ComponentModel.Design;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Permissions;

namespace System.ComponentModel
{
	// Token: 0x0200057F RID: 1407
	[HostProtection(SecurityAction.LinkDemand, ExternalProcessMgmt = true)]
	public sealed class LicenseManager
	{
		// Token: 0x060033FD RID: 13309 RVA: 0x000E4062 File Offset: 0x000E2262
		private LicenseManager()
		{
		}

		// Token: 0x17000CB7 RID: 3255
		// (get) Token: 0x060033FE RID: 13310 RVA: 0x000E406C File Offset: 0x000E226C
		// (set) Token: 0x060033FF RID: 13311 RVA: 0x000E40CC File Offset: 0x000E22CC
		public static LicenseContext CurrentContext
		{
			get
			{
				if (LicenseManager.context == null)
				{
					object obj = LicenseManager.internalSyncObject;
					lock (obj)
					{
						if (LicenseManager.context == null)
						{
							LicenseManager.context = new RuntimeLicenseContext();
						}
					}
				}
				return LicenseManager.context;
			}
			set
			{
				object obj = LicenseManager.internalSyncObject;
				lock (obj)
				{
					if (LicenseManager.contextLockHolder != null)
					{
						throw new InvalidOperationException(SR.GetString("LicMgrContextCannotBeChanged"));
					}
					LicenseManager.context = value;
				}
			}
		}

		// Token: 0x17000CB8 RID: 3256
		// (get) Token: 0x06003400 RID: 13312 RVA: 0x000E4124 File Offset: 0x000E2324
		public static LicenseUsageMode UsageMode
		{
			get
			{
				if (LicenseManager.context != null)
				{
					return LicenseManager.context.UsageMode;
				}
				return LicenseUsageMode.Runtime;
			}
		}

		// Token: 0x06003401 RID: 13313 RVA: 0x000E4140 File Offset: 0x000E2340
		private static void CacheProvider(Type type, LicenseProvider provider)
		{
			if (LicenseManager.providers == null)
			{
				LicenseManager.providers = new Hashtable();
			}
			LicenseManager.providers[type] = provider;
			if (provider != null)
			{
				if (LicenseManager.providerInstances == null)
				{
					LicenseManager.providerInstances = new Hashtable();
				}
				LicenseManager.providerInstances[provider.GetType()] = provider;
			}
		}

		// Token: 0x06003402 RID: 13314 RVA: 0x000E419B File Offset: 0x000E239B
		public static object CreateWithContext(Type type, LicenseContext creationContext)
		{
			return LicenseManager.CreateWithContext(type, creationContext, new object[0]);
		}

		// Token: 0x06003403 RID: 13315 RVA: 0x000E41AC File Offset: 0x000E23AC
		public static object CreateWithContext(Type type, LicenseContext creationContext, object[] args)
		{
			object obj = null;
			object obj2 = LicenseManager.internalSyncObject;
			lock (obj2)
			{
				LicenseContext currentContext = LicenseManager.CurrentContext;
				try
				{
					LicenseManager.CurrentContext = creationContext;
					LicenseManager.LockContext(LicenseManager.selfLock);
					try
					{
						obj = SecurityUtils.SecureCreateInstance(type, args);
					}
					catch (TargetInvocationException ex)
					{
						throw ex.InnerException;
					}
				}
				finally
				{
					LicenseManager.UnlockContext(LicenseManager.selfLock);
					LicenseManager.CurrentContext = currentContext;
				}
			}
			return obj;
		}

		// Token: 0x06003404 RID: 13316 RVA: 0x000E423C File Offset: 0x000E243C
		private static bool GetCachedNoLicenseProvider(Type type)
		{
			return LicenseManager.providers != null && LicenseManager.providers.ContainsKey(type);
		}

		// Token: 0x06003405 RID: 13317 RVA: 0x000E4256 File Offset: 0x000E2456
		private static LicenseProvider GetCachedProvider(Type type)
		{
			if (LicenseManager.providers != null)
			{
				return (LicenseProvider)LicenseManager.providers[type];
			}
			return null;
		}

		// Token: 0x06003406 RID: 13318 RVA: 0x000E4275 File Offset: 0x000E2475
		private static LicenseProvider GetCachedProviderInstance(Type providerType)
		{
			if (LicenseManager.providerInstances != null)
			{
				return (LicenseProvider)LicenseManager.providerInstances[providerType];
			}
			return null;
		}

		// Token: 0x06003407 RID: 13319 RVA: 0x000E4294 File Offset: 0x000E2494
		private static IntPtr GetLicenseInteropHelperType()
		{
			return typeof(LicenseManager.LicenseInteropHelper).TypeHandle.Value;
		}

		// Token: 0x06003408 RID: 13320 RVA: 0x000E42B8 File Offset: 0x000E24B8
		public static bool IsLicensed(Type type)
		{
			License license;
			bool flag = LicenseManager.ValidateInternal(type, null, false, out license);
			if (license != null)
			{
				license.Dispose();
				license = null;
			}
			return flag;
		}

		// Token: 0x06003409 RID: 13321 RVA: 0x000E42DC File Offset: 0x000E24DC
		public static bool IsValid(Type type)
		{
			License license;
			bool flag = LicenseManager.ValidateInternal(type, null, false, out license);
			if (license != null)
			{
				license.Dispose();
				license = null;
			}
			return flag;
		}

		// Token: 0x0600340A RID: 13322 RVA: 0x000E4300 File Offset: 0x000E2500
		public static bool IsValid(Type type, object instance, out License license)
		{
			return LicenseManager.ValidateInternal(type, instance, false, out license);
		}

		// Token: 0x0600340B RID: 13323 RVA: 0x000E430C File Offset: 0x000E250C
		public static void LockContext(object contextUser)
		{
			object obj = LicenseManager.internalSyncObject;
			lock (obj)
			{
				if (LicenseManager.contextLockHolder != null)
				{
					throw new InvalidOperationException(SR.GetString("LicMgrAlreadyLocked"));
				}
				LicenseManager.contextLockHolder = contextUser;
			}
		}

		// Token: 0x0600340C RID: 13324 RVA: 0x000E4364 File Offset: 0x000E2564
		public static void UnlockContext(object contextUser)
		{
			object obj = LicenseManager.internalSyncObject;
			lock (obj)
			{
				if (LicenseManager.contextLockHolder != contextUser)
				{
					throw new ArgumentException(SR.GetString("LicMgrDifferentUser"));
				}
				LicenseManager.contextLockHolder = null;
			}
		}

		// Token: 0x0600340D RID: 13325 RVA: 0x000E43BC File Offset: 0x000E25BC
		private static bool ValidateInternal(Type type, object instance, bool allowExceptions, out License license)
		{
			string text;
			return LicenseManager.ValidateInternalRecursive(LicenseManager.CurrentContext, type, instance, allowExceptions, out license, out text);
		}

		// Token: 0x0600340E RID: 13326 RVA: 0x000E43DC File Offset: 0x000E25DC
		private static bool ValidateInternalRecursive(LicenseContext context, Type type, object instance, bool allowExceptions, out License license, out string licenseKey)
		{
			LicenseProvider licenseProvider = LicenseManager.GetCachedProvider(type);
			if (licenseProvider == null && !LicenseManager.GetCachedNoLicenseProvider(type))
			{
				LicenseProviderAttribute licenseProviderAttribute = (LicenseProviderAttribute)Attribute.GetCustomAttribute(type, typeof(LicenseProviderAttribute), false);
				if (licenseProviderAttribute != null)
				{
					Type licenseProvider2 = licenseProviderAttribute.LicenseProvider;
					licenseProvider = LicenseManager.GetCachedProviderInstance(licenseProvider2);
					if (licenseProvider == null)
					{
						licenseProvider = (LicenseProvider)SecurityUtils.SecureCreateInstance(licenseProvider2);
					}
				}
				LicenseManager.CacheProvider(type, licenseProvider);
			}
			license = null;
			bool flag = true;
			licenseKey = null;
			if (licenseProvider != null)
			{
				license = licenseProvider.GetLicense(context, type, instance, allowExceptions);
				if (license == null)
				{
					flag = false;
				}
				else
				{
					licenseKey = license.LicenseKey;
				}
			}
			if (flag && instance == null)
			{
				Type baseType = type.BaseType;
				if (baseType != typeof(object) && baseType != null)
				{
					if (license != null)
					{
						license.Dispose();
						license = null;
					}
					string text;
					flag = LicenseManager.ValidateInternalRecursive(context, baseType, null, allowExceptions, out license, out text);
					if (license != null)
					{
						license.Dispose();
						license = null;
					}
				}
			}
			return flag;
		}

		// Token: 0x0600340F RID: 13327 RVA: 0x000E44C4 File Offset: 0x000E26C4
		public static void Validate(Type type)
		{
			License license;
			if (!LicenseManager.ValidateInternal(type, null, true, out license))
			{
				throw new LicenseException(type);
			}
			if (license != null)
			{
				license.Dispose();
				license = null;
			}
		}

		// Token: 0x06003410 RID: 13328 RVA: 0x000E44F0 File Offset: 0x000E26F0
		public static License Validate(Type type, object instance)
		{
			License license;
			if (!LicenseManager.ValidateInternal(type, instance, true, out license))
			{
				throw new LicenseException(type, instance);
			}
			return license;
		}

		// Token: 0x040029BC RID: 10684
		private static readonly object selfLock = new object();

		// Token: 0x040029BD RID: 10685
		private static volatile LicenseContext context = null;

		// Token: 0x040029BE RID: 10686
		private static object contextLockHolder = null;

		// Token: 0x040029BF RID: 10687
		private static volatile Hashtable providers;

		// Token: 0x040029C0 RID: 10688
		private static volatile Hashtable providerInstances;

		// Token: 0x040029C1 RID: 10689
		private static object internalSyncObject = new object();

		// Token: 0x02000895 RID: 2197
		private class LicenseInteropHelper
		{
			// Token: 0x06004592 RID: 17810 RVA: 0x00122F48 File Offset: 0x00121148
			private static object AllocateAndValidateLicense(RuntimeTypeHandle rth, IntPtr bstrKey, int fDesignTime)
			{
				Type typeFromHandle = Type.GetTypeFromHandle(rth);
				LicenseManager.LicenseInteropHelper.CLRLicenseContext clrlicenseContext = new LicenseManager.LicenseInteropHelper.CLRLicenseContext((fDesignTime != 0) ? LicenseUsageMode.Designtime : LicenseUsageMode.Runtime, typeFromHandle);
				if (fDesignTime == 0 && bstrKey != (IntPtr)0)
				{
					clrlicenseContext.SetSavedLicenseKey(typeFromHandle, Marshal.PtrToStringBSTR(bstrKey));
				}
				object obj;
				try
				{
					obj = LicenseManager.CreateWithContext(typeFromHandle, clrlicenseContext);
				}
				catch (LicenseException ex)
				{
					throw new COMException(ex.Message, -2147221230);
				}
				return obj;
			}

			// Token: 0x06004593 RID: 17811 RVA: 0x00122FB8 File Offset: 0x001211B8
			private static int RequestLicKey(RuntimeTypeHandle rth, ref IntPtr pbstrKey)
			{
				Type typeFromHandle = Type.GetTypeFromHandle(rth);
				License license;
				string text;
				if (!LicenseManager.ValidateInternalRecursive(LicenseManager.CurrentContext, typeFromHandle, null, false, out license, out text))
				{
					return -2147483640;
				}
				if (text == null)
				{
					return -2147483640;
				}
				pbstrKey = Marshal.StringToBSTR(text);
				if (license != null)
				{
					license.Dispose();
					license = null;
				}
				return 0;
			}

			// Token: 0x06004594 RID: 17812 RVA: 0x00123004 File Offset: 0x00121204
			private void GetLicInfo(RuntimeTypeHandle rth, ref int pRuntimeKeyAvail, ref int pLicVerified)
			{
				pRuntimeKeyAvail = 0;
				pLicVerified = 0;
				Type typeFromHandle = Type.GetTypeFromHandle(rth);
				if (this.helperContext == null)
				{
					this.helperContext = new DesigntimeLicenseContext();
				}
				else
				{
					this.helperContext.savedLicenseKeys.Clear();
				}
				License license;
				string text;
				if (LicenseManager.ValidateInternalRecursive(this.helperContext, typeFromHandle, null, false, out license, out text))
				{
					if (this.helperContext.savedLicenseKeys.Contains(typeFromHandle.AssemblyQualifiedName))
					{
						pRuntimeKeyAvail = 1;
					}
					if (license != null)
					{
						license.Dispose();
						license = null;
						pLicVerified = 1;
					}
				}
			}

			// Token: 0x06004595 RID: 17813 RVA: 0x00123080 File Offset: 0x00121280
			private void GetCurrentContextInfo(ref int fDesignTime, ref IntPtr bstrKey, RuntimeTypeHandle rth)
			{
				this.savedLicenseContext = LicenseManager.CurrentContext;
				this.savedType = Type.GetTypeFromHandle(rth);
				if (this.savedLicenseContext.UsageMode == LicenseUsageMode.Designtime)
				{
					fDesignTime = 1;
					bstrKey = (IntPtr)0;
					return;
				}
				fDesignTime = 0;
				string savedLicenseKey = this.savedLicenseContext.GetSavedLicenseKey(this.savedType, null);
				bstrKey = Marshal.StringToBSTR(savedLicenseKey);
			}

			// Token: 0x06004596 RID: 17814 RVA: 0x001230DC File Offset: 0x001212DC
			private void SaveKeyInCurrentContext(IntPtr bstrKey)
			{
				if (bstrKey != (IntPtr)0)
				{
					this.savedLicenseContext.SetSavedLicenseKey(this.savedType, Marshal.PtrToStringBSTR(bstrKey));
				}
			}

			// Token: 0x040037C1 RID: 14273
			private const int S_OK = 0;

			// Token: 0x040037C2 RID: 14274
			private const int E_NOTIMPL = -2147467263;

			// Token: 0x040037C3 RID: 14275
			private const int CLASS_E_NOTLICENSED = -2147221230;

			// Token: 0x040037C4 RID: 14276
			private const int E_FAIL = -2147483640;

			// Token: 0x040037C5 RID: 14277
			private DesigntimeLicenseContext helperContext;

			// Token: 0x040037C6 RID: 14278
			private LicenseContext savedLicenseContext;

			// Token: 0x040037C7 RID: 14279
			private Type savedType;

			// Token: 0x02000933 RID: 2355
			internal class CLRLicenseContext : LicenseContext
			{
				// Token: 0x060046A3 RID: 18083 RVA: 0x00126BC4 File Offset: 0x00124DC4
				public CLRLicenseContext(LicenseUsageMode usageMode, Type type)
				{
					this.usageMode = usageMode;
					this.type = type;
				}

				// Token: 0x17000FEB RID: 4075
				// (get) Token: 0x060046A4 RID: 18084 RVA: 0x00126BDA File Offset: 0x00124DDA
				public override LicenseUsageMode UsageMode
				{
					get
					{
						return this.usageMode;
					}
				}

				// Token: 0x060046A5 RID: 18085 RVA: 0x00126BE2 File Offset: 0x00124DE2
				public override string GetSavedLicenseKey(Type type, Assembly resourceAssembly)
				{
					if (!(type == this.type))
					{
						return null;
					}
					return this.key;
				}

				// Token: 0x060046A6 RID: 18086 RVA: 0x00126BFA File Offset: 0x00124DFA
				public override void SetSavedLicenseKey(Type type, string key)
				{
					if (type == this.type)
					{
						this.key = key;
					}
				}

				// Token: 0x04003DD7 RID: 15831
				private LicenseUsageMode usageMode;

				// Token: 0x04003DD8 RID: 15832
				private Type type;

				// Token: 0x04003DD9 RID: 15833
				private string key;
			}
		}
	}
}
