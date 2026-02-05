using System;
using System.Collections;
using System.Security.Permissions;

namespace System.ComponentModel
{
	// Token: 0x020005B4 RID: 1460
	[HostProtection(SecurityAction.LinkDemand, SharedState = true)]
	public abstract class TypeDescriptionProvider
	{
		// Token: 0x06003665 RID: 13925 RVA: 0x000ECB43 File Offset: 0x000EAD43
		protected TypeDescriptionProvider()
		{
		}

		// Token: 0x06003666 RID: 13926 RVA: 0x000ECB4B File Offset: 0x000EAD4B
		protected TypeDescriptionProvider(TypeDescriptionProvider parent)
		{
			this._parent = parent;
		}

		// Token: 0x06003667 RID: 13927 RVA: 0x000ECB5A File Offset: 0x000EAD5A
		public virtual object CreateInstance(IServiceProvider provider, Type objectType, Type[] argTypes, object[] args)
		{
			if (this._parent != null)
			{
				return this._parent.CreateInstance(provider, objectType, argTypes, args);
			}
			if (objectType == null)
			{
				throw new ArgumentNullException("objectType");
			}
			return SecurityUtils.SecureCreateInstance(objectType, args);
		}

		// Token: 0x06003668 RID: 13928 RVA: 0x000ECB91 File Offset: 0x000EAD91
		public virtual IDictionary GetCache(object instance)
		{
			if (this._parent != null)
			{
				return this._parent.GetCache(instance);
			}
			return null;
		}

		// Token: 0x06003669 RID: 13929 RVA: 0x000ECBA9 File Offset: 0x000EADA9
		public virtual ICustomTypeDescriptor GetExtendedTypeDescriptor(object instance)
		{
			if (this._parent != null)
			{
				return this._parent.GetExtendedTypeDescriptor(instance);
			}
			if (this._emptyDescriptor == null)
			{
				this._emptyDescriptor = new TypeDescriptionProvider.EmptyCustomTypeDescriptor();
			}
			return this._emptyDescriptor;
		}

		// Token: 0x0600366A RID: 13930 RVA: 0x000ECBD9 File Offset: 0x000EADD9
		protected internal virtual IExtenderProvider[] GetExtenderProviders(object instance)
		{
			if (this._parent != null)
			{
				return this._parent.GetExtenderProviders(instance);
			}
			if (instance == null)
			{
				throw new ArgumentNullException("instance");
			}
			return new IExtenderProvider[0];
		}

		// Token: 0x0600366B RID: 13931 RVA: 0x000ECC04 File Offset: 0x000EAE04
		public virtual string GetFullComponentName(object component)
		{
			if (this._parent != null)
			{
				return this._parent.GetFullComponentName(component);
			}
			return this.GetTypeDescriptor(component).GetComponentName();
		}

		// Token: 0x0600366C RID: 13932 RVA: 0x000ECC27 File Offset: 0x000EAE27
		public Type GetReflectionType(Type objectType)
		{
			return this.GetReflectionType(objectType, null);
		}

		// Token: 0x0600366D RID: 13933 RVA: 0x000ECC31 File Offset: 0x000EAE31
		public Type GetReflectionType(object instance)
		{
			if (instance == null)
			{
				throw new ArgumentNullException("instance");
			}
			return this.GetReflectionType(instance.GetType(), instance);
		}

		// Token: 0x0600366E RID: 13934 RVA: 0x000ECC4E File Offset: 0x000EAE4E
		public virtual Type GetReflectionType(Type objectType, object instance)
		{
			if (this._parent != null)
			{
				return this._parent.GetReflectionType(objectType, instance);
			}
			return objectType;
		}

		// Token: 0x0600366F RID: 13935 RVA: 0x000ECC68 File Offset: 0x000EAE68
		public virtual Type GetRuntimeType(Type reflectionType)
		{
			if (this._parent != null)
			{
				return this._parent.GetRuntimeType(reflectionType);
			}
			if (reflectionType == null)
			{
				throw new ArgumentNullException("reflectionType");
			}
			if (reflectionType.GetType().Assembly == typeof(object).Assembly)
			{
				return reflectionType;
			}
			return reflectionType.UnderlyingSystemType;
		}

		// Token: 0x06003670 RID: 13936 RVA: 0x000ECCC7 File Offset: 0x000EAEC7
		public ICustomTypeDescriptor GetTypeDescriptor(Type objectType)
		{
			return this.GetTypeDescriptor(objectType, null);
		}

		// Token: 0x06003671 RID: 13937 RVA: 0x000ECCD1 File Offset: 0x000EAED1
		public ICustomTypeDescriptor GetTypeDescriptor(object instance)
		{
			if (instance == null)
			{
				throw new ArgumentNullException("instance");
			}
			return this.GetTypeDescriptor(instance.GetType(), instance);
		}

		// Token: 0x06003672 RID: 13938 RVA: 0x000ECCEE File Offset: 0x000EAEEE
		public virtual ICustomTypeDescriptor GetTypeDescriptor(Type objectType, object instance)
		{
			if (this._parent != null)
			{
				return this._parent.GetTypeDescriptor(objectType, instance);
			}
			if (this._emptyDescriptor == null)
			{
				this._emptyDescriptor = new TypeDescriptionProvider.EmptyCustomTypeDescriptor();
			}
			return this._emptyDescriptor;
		}

		// Token: 0x06003673 RID: 13939 RVA: 0x000ECD1F File Offset: 0x000EAF1F
		public virtual bool IsSupportedType(Type type)
		{
			if (type == null)
			{
				throw new ArgumentNullException("type");
			}
			return this._parent == null || this._parent.IsSupportedType(type);
		}

		// Token: 0x04002A9D RID: 10909
		private TypeDescriptionProvider _parent;

		// Token: 0x04002A9E RID: 10910
		private TypeDescriptionProvider.EmptyCustomTypeDescriptor _emptyDescriptor;

		// Token: 0x020008A0 RID: 2208
		private sealed class EmptyCustomTypeDescriptor : CustomTypeDescriptor
		{
		}
	}
}
