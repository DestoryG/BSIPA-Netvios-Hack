using System;
using System.Collections;
using System.Security.Permissions;

namespace System.ComponentModel
{
	// Token: 0x0200053F RID: 1343
	[HostProtection(SecurityAction.LinkDemand, SharedState = true)]
	internal sealed class DelegatingTypeDescriptionProvider : TypeDescriptionProvider
	{
		// Token: 0x06003292 RID: 12946 RVA: 0x000E2223 File Offset: 0x000E0423
		internal DelegatingTypeDescriptionProvider(Type type)
		{
			this._type = type;
		}

		// Token: 0x17000C5F RID: 3167
		// (get) Token: 0x06003293 RID: 12947 RVA: 0x000E2232 File Offset: 0x000E0432
		internal TypeDescriptionProvider Provider
		{
			get
			{
				return TypeDescriptor.GetProviderRecursive(this._type);
			}
		}

		// Token: 0x06003294 RID: 12948 RVA: 0x000E223F File Offset: 0x000E043F
		public override object CreateInstance(IServiceProvider provider, Type objectType, Type[] argTypes, object[] args)
		{
			return this.Provider.CreateInstance(provider, objectType, argTypes, args);
		}

		// Token: 0x06003295 RID: 12949 RVA: 0x000E2251 File Offset: 0x000E0451
		public override IDictionary GetCache(object instance)
		{
			return this.Provider.GetCache(instance);
		}

		// Token: 0x06003296 RID: 12950 RVA: 0x000E225F File Offset: 0x000E045F
		public override string GetFullComponentName(object component)
		{
			return this.Provider.GetFullComponentName(component);
		}

		// Token: 0x06003297 RID: 12951 RVA: 0x000E226D File Offset: 0x000E046D
		public override ICustomTypeDescriptor GetExtendedTypeDescriptor(object instance)
		{
			return this.Provider.GetExtendedTypeDescriptor(instance);
		}

		// Token: 0x06003298 RID: 12952 RVA: 0x000E227B File Offset: 0x000E047B
		protected internal override IExtenderProvider[] GetExtenderProviders(object instance)
		{
			return this.Provider.GetExtenderProviders(instance);
		}

		// Token: 0x06003299 RID: 12953 RVA: 0x000E2289 File Offset: 0x000E0489
		public override Type GetReflectionType(Type objectType, object instance)
		{
			return this.Provider.GetReflectionType(objectType, instance);
		}

		// Token: 0x0600329A RID: 12954 RVA: 0x000E2298 File Offset: 0x000E0498
		public override Type GetRuntimeType(Type objectType)
		{
			return this.Provider.GetRuntimeType(objectType);
		}

		// Token: 0x0600329B RID: 12955 RVA: 0x000E22A6 File Offset: 0x000E04A6
		public override ICustomTypeDescriptor GetTypeDescriptor(Type objectType, object instance)
		{
			return this.Provider.GetTypeDescriptor(objectType, instance);
		}

		// Token: 0x0600329C RID: 12956 RVA: 0x000E22B5 File Offset: 0x000E04B5
		public override bool IsSupportedType(Type type)
		{
			return this.Provider.IsSupportedType(type);
		}

		// Token: 0x04002977 RID: 10615
		private Type _type;
	}
}
