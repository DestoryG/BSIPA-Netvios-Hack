using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Security.Permissions;

namespace System.ComponentModel.Design
{
	// Token: 0x020005FF RID: 1535
	[HostProtection(SecurityAction.LinkDemand, SharedState = true)]
	public class ServiceContainer : IServiceContainer, IServiceProvider, IDisposable
	{
		// Token: 0x06003876 RID: 14454 RVA: 0x000F1041 File Offset: 0x000EF241
		public ServiceContainer()
		{
		}

		// Token: 0x06003877 RID: 14455 RVA: 0x000F1049 File Offset: 0x000EF249
		public ServiceContainer(IServiceProvider parentProvider)
		{
			this.parentProvider = parentProvider;
		}

		// Token: 0x17000D83 RID: 3459
		// (get) Token: 0x06003878 RID: 14456 RVA: 0x000F1058 File Offset: 0x000EF258
		private IServiceContainer Container
		{
			get
			{
				IServiceContainer serviceContainer = null;
				if (this.parentProvider != null)
				{
					serviceContainer = (IServiceContainer)this.parentProvider.GetService(typeof(IServiceContainer));
				}
				return serviceContainer;
			}
		}

		// Token: 0x17000D84 RID: 3460
		// (get) Token: 0x06003879 RID: 14457 RVA: 0x000F108B File Offset: 0x000EF28B
		protected virtual Type[] DefaultServices
		{
			get
			{
				return ServiceContainer._defaultServices;
			}
		}

		// Token: 0x17000D85 RID: 3461
		// (get) Token: 0x0600387A RID: 14458 RVA: 0x000F1092 File Offset: 0x000EF292
		private ServiceContainer.ServiceCollection<object> Services
		{
			get
			{
				if (this.services == null)
				{
					this.services = new ServiceContainer.ServiceCollection<object>();
				}
				return this.services;
			}
		}

		// Token: 0x0600387B RID: 14459 RVA: 0x000F10AD File Offset: 0x000EF2AD
		public void AddService(Type serviceType, object serviceInstance)
		{
			this.AddService(serviceType, serviceInstance, false);
		}

		// Token: 0x0600387C RID: 14460 RVA: 0x000F10B8 File Offset: 0x000EF2B8
		public virtual void AddService(Type serviceType, object serviceInstance, bool promote)
		{
			if (promote)
			{
				IServiceContainer container = this.Container;
				if (container != null)
				{
					container.AddService(serviceType, serviceInstance, promote);
					return;
				}
			}
			if (serviceType == null)
			{
				throw new ArgumentNullException("serviceType");
			}
			if (serviceInstance == null)
			{
				throw new ArgumentNullException("serviceInstance");
			}
			if (!(serviceInstance is ServiceCreatorCallback) && !serviceInstance.GetType().IsCOMObject && !serviceType.IsAssignableFrom(serviceInstance.GetType()))
			{
				throw new ArgumentException(SR.GetString("ErrorInvalidServiceInstance", new object[] { serviceType.FullName }));
			}
			if (this.Services.ContainsKey(serviceType))
			{
				throw new ArgumentException(SR.GetString("ErrorServiceExists", new object[] { serviceType.FullName }), "serviceType");
			}
			this.Services[serviceType] = serviceInstance;
		}

		// Token: 0x0600387D RID: 14461 RVA: 0x000F117F File Offset: 0x000EF37F
		public void AddService(Type serviceType, ServiceCreatorCallback callback)
		{
			this.AddService(serviceType, callback, false);
		}

		// Token: 0x0600387E RID: 14462 RVA: 0x000F118C File Offset: 0x000EF38C
		public virtual void AddService(Type serviceType, ServiceCreatorCallback callback, bool promote)
		{
			if (promote)
			{
				IServiceContainer container = this.Container;
				if (container != null)
				{
					container.AddService(serviceType, callback, promote);
					return;
				}
			}
			if (serviceType == null)
			{
				throw new ArgumentNullException("serviceType");
			}
			if (callback == null)
			{
				throw new ArgumentNullException("callback");
			}
			if (this.Services.ContainsKey(serviceType))
			{
				throw new ArgumentException(SR.GetString("ErrorServiceExists", new object[] { serviceType.FullName }), "serviceType");
			}
			this.Services[serviceType] = callback;
		}

		// Token: 0x0600387F RID: 14463 RVA: 0x000F1211 File Offset: 0x000EF411
		public void Dispose()
		{
			this.Dispose(true);
		}

		// Token: 0x06003880 RID: 14464 RVA: 0x000F121C File Offset: 0x000EF41C
		protected virtual void Dispose(bool disposing)
		{
			if (disposing)
			{
				ServiceContainer.ServiceCollection<object> serviceCollection = this.services;
				this.services = null;
				if (serviceCollection != null)
				{
					foreach (object obj in serviceCollection.Values)
					{
						if (obj is IDisposable)
						{
							((IDisposable)obj).Dispose();
						}
					}
				}
			}
		}

		// Token: 0x06003881 RID: 14465 RVA: 0x000F1290 File Offset: 0x000EF490
		public virtual object GetService(Type serviceType)
		{
			object obj = null;
			Type[] defaultServices = this.DefaultServices;
			for (int i = 0; i < defaultServices.Length; i++)
			{
				if (serviceType.IsEquivalentTo(defaultServices[i]))
				{
					obj = this;
					break;
				}
			}
			if (obj == null)
			{
				this.Services.TryGetValue(serviceType, out obj);
			}
			if (obj is ServiceCreatorCallback)
			{
				obj = ((ServiceCreatorCallback)obj)(this, serviceType);
				if (obj != null && !obj.GetType().IsCOMObject && !serviceType.IsAssignableFrom(obj.GetType()))
				{
					obj = null;
				}
				this.Services[serviceType] = obj;
			}
			if (obj == null && this.parentProvider != null)
			{
				obj = this.parentProvider.GetService(serviceType);
			}
			return obj;
		}

		// Token: 0x06003882 RID: 14466 RVA: 0x000F1331 File Offset: 0x000EF531
		public void RemoveService(Type serviceType)
		{
			this.RemoveService(serviceType, false);
		}

		// Token: 0x06003883 RID: 14467 RVA: 0x000F133C File Offset: 0x000EF53C
		public virtual void RemoveService(Type serviceType, bool promote)
		{
			if (promote)
			{
				IServiceContainer container = this.Container;
				if (container != null)
				{
					container.RemoveService(serviceType, promote);
					return;
				}
			}
			if (serviceType == null)
			{
				throw new ArgumentNullException("serviceType");
			}
			this.Services.Remove(serviceType);
		}

		// Token: 0x04002B1A RID: 11034
		private ServiceContainer.ServiceCollection<object> services;

		// Token: 0x04002B1B RID: 11035
		private IServiceProvider parentProvider;

		// Token: 0x04002B1C RID: 11036
		private static Type[] _defaultServices = new Type[]
		{
			typeof(IServiceContainer),
			typeof(ServiceContainer)
		};

		// Token: 0x04002B1D RID: 11037
		private static TraceSwitch TRACESERVICE = new TraceSwitch("TRACESERVICE", "ServiceProvider: Trace service provider requests.");

		// Token: 0x020008AF RID: 2223
		private sealed class ServiceCollection<T> : Dictionary<Type, T>
		{
			// Token: 0x06004619 RID: 17945 RVA: 0x00124903 File Offset: 0x00122B03
			public ServiceCollection()
				: base(ServiceContainer.ServiceCollection<T>.serviceTypeComparer)
			{
			}

			// Token: 0x040037FC RID: 14332
			private static ServiceContainer.ServiceCollection<T>.EmbeddedTypeAwareTypeComparer serviceTypeComparer = new ServiceContainer.ServiceCollection<T>.EmbeddedTypeAwareTypeComparer();

			// Token: 0x0200093A RID: 2362
			private sealed class EmbeddedTypeAwareTypeComparer : IEqualityComparer<Type>
			{
				// Token: 0x060046E3 RID: 18147 RVA: 0x00127E7E File Offset: 0x0012607E
				public bool Equals(Type x, Type y)
				{
					return x.IsEquivalentTo(y);
				}

				// Token: 0x060046E4 RID: 18148 RVA: 0x00127E87 File Offset: 0x00126087
				public int GetHashCode(Type obj)
				{
					return obj.FullName.GetHashCode();
				}
			}
		}
	}
}
