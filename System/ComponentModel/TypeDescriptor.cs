using System;
using System.Collections;
using System.Collections.Specialized;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Globalization;
using System.Reflection;
using System.Security;
using System.Security.Permissions;
using System.Threading;

namespace System.ComponentModel
{
	// Token: 0x020005B6 RID: 1462
	[HostProtection(SecurityAction.LinkDemand, SharedState = true)]
	public sealed class TypeDescriptor
	{
		// Token: 0x06003677 RID: 13943 RVA: 0x000ECD98 File Offset: 0x000EAF98
		private TypeDescriptor()
		{
		}

		// Token: 0x17000D39 RID: 3385
		// (get) Token: 0x06003678 RID: 13944 RVA: 0x000ECDA0 File Offset: 0x000EAFA0
		// (set) Token: 0x06003679 RID: 13945 RVA: 0x000ECDE0 File Offset: 0x000EAFE0
		[Obsolete("This property has been deprecated.  Use a type description provider to supply type information for COM types instead.  http://go.microsoft.com/fwlink/?linkid=14202")]
		public static IComNativeDescriptorHandler ComNativeDescriptorHandler
		{
			[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
			get
			{
				TypeDescriptor.TypeDescriptionNode typeDescriptionNode = TypeDescriptor.NodeFor(TypeDescriptor.ComObjectType);
				TypeDescriptor.ComNativeDescriptionProvider comNativeDescriptionProvider;
				do
				{
					comNativeDescriptionProvider = typeDescriptionNode.Provider as TypeDescriptor.ComNativeDescriptionProvider;
					typeDescriptionNode = typeDescriptionNode.Next;
				}
				while (typeDescriptionNode != null && comNativeDescriptionProvider == null);
				if (comNativeDescriptionProvider != null)
				{
					return comNativeDescriptionProvider.Handler;
				}
				return null;
			}
			[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
			set
			{
				TypeDescriptor.TypeDescriptionNode typeDescriptionNode = TypeDescriptor.NodeFor(TypeDescriptor.ComObjectType);
				while (typeDescriptionNode != null && !(typeDescriptionNode.Provider is TypeDescriptor.ComNativeDescriptionProvider))
				{
					typeDescriptionNode = typeDescriptionNode.Next;
				}
				if (typeDescriptionNode == null)
				{
					TypeDescriptor.AddProvider(new TypeDescriptor.ComNativeDescriptionProvider(value), TypeDescriptor.ComObjectType);
					return;
				}
				TypeDescriptor.ComNativeDescriptionProvider comNativeDescriptionProvider = (TypeDescriptor.ComNativeDescriptionProvider)typeDescriptionNode.Provider;
				comNativeDescriptionProvider.Handler = value;
			}
		}

		// Token: 0x17000D3A RID: 3386
		// (get) Token: 0x0600367A RID: 13946 RVA: 0x000ECE38 File Offset: 0x000EB038
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public static Type ComObjectType
		{
			get
			{
				return typeof(TypeDescriptor.TypeDescriptorComObject);
			}
		}

		// Token: 0x17000D3B RID: 3387
		// (get) Token: 0x0600367B RID: 13947 RVA: 0x000ECE44 File Offset: 0x000EB044
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public static Type InterfaceType
		{
			get
			{
				return typeof(TypeDescriptor.TypeDescriptorInterface);
			}
		}

		// Token: 0x17000D3C RID: 3388
		// (get) Token: 0x0600367C RID: 13948 RVA: 0x000ECE50 File Offset: 0x000EB050
		internal static int MetadataVersion
		{
			get
			{
				return TypeDescriptor._metadataVersion;
			}
		}

		// Token: 0x14000052 RID: 82
		// (add) Token: 0x0600367D RID: 13949 RVA: 0x000ECE58 File Offset: 0x000EB058
		// (remove) Token: 0x0600367E RID: 13950 RVA: 0x000ECE8C File Offset: 0x000EB08C
		public static event RefreshEventHandler Refreshed;

		// Token: 0x0600367F RID: 13951 RVA: 0x000ECEC0 File Offset: 0x000EB0C0
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
		public static TypeDescriptionProvider AddAttributes(Type type, params Attribute[] attributes)
		{
			if (type == null)
			{
				throw new ArgumentNullException("type");
			}
			if (attributes == null)
			{
				throw new ArgumentNullException("attributes");
			}
			TypeDescriptionProvider provider = TypeDescriptor.GetProvider(type);
			TypeDescriptionProvider typeDescriptionProvider = new TypeDescriptor.AttributeProvider(provider, attributes);
			TypeDescriptor.AddProvider(typeDescriptionProvider, type);
			return typeDescriptionProvider;
		}

		// Token: 0x06003680 RID: 13952 RVA: 0x000ECF08 File Offset: 0x000EB108
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
		public static TypeDescriptionProvider AddAttributes(object instance, params Attribute[] attributes)
		{
			if (instance == null)
			{
				throw new ArgumentNullException("instance");
			}
			if (attributes == null)
			{
				throw new ArgumentNullException("attributes");
			}
			TypeDescriptionProvider provider = TypeDescriptor.GetProvider(instance);
			TypeDescriptionProvider typeDescriptionProvider = new TypeDescriptor.AttributeProvider(provider, attributes);
			TypeDescriptor.AddProvider(typeDescriptionProvider, instance);
			return typeDescriptionProvider;
		}

		// Token: 0x06003681 RID: 13953 RVA: 0x000ECF48 File Offset: 0x000EB148
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public static void AddEditorTable(Type editorBaseType, Hashtable table)
		{
			ReflectTypeDescriptionProvider.AddEditorTable(editorBaseType, table);
		}

		// Token: 0x06003682 RID: 13954 RVA: 0x000ECF54 File Offset: 0x000EB154
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
		public static void AddProvider(TypeDescriptionProvider provider, Type type)
		{
			if (provider == null)
			{
				throw new ArgumentNullException("provider");
			}
			if (type == null)
			{
				throw new ArgumentNullException("type");
			}
			object obj = (LocalAppContextSwitches.DoNotUseTypeDescriptorThreadingFix ? TypeDescriptor._providerTable : TypeDescriptor._commonSyncObject);
			lock (obj)
			{
				TypeDescriptor.TypeDescriptionNode typeDescriptionNode = TypeDescriptor.NodeFor(type, true);
				TypeDescriptor.TypeDescriptionNode typeDescriptionNode2 = new TypeDescriptor.TypeDescriptionNode(provider);
				typeDescriptionNode2.Next = typeDescriptionNode;
				TypeDescriptor._providerTable[type] = typeDescriptionNode2;
				TypeDescriptor._providerTypeTable.Clear();
			}
			TypeDescriptor.Refresh(type);
		}

		// Token: 0x06003683 RID: 13955 RVA: 0x000ECFF0 File Offset: 0x000EB1F0
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
		public static void AddProvider(TypeDescriptionProvider provider, object instance)
		{
			if (provider == null)
			{
				throw new ArgumentNullException("provider");
			}
			if (instance == null)
			{
				throw new ArgumentNullException("instance");
			}
			object obj = (LocalAppContextSwitches.DoNotUseTypeDescriptorThreadingFix ? TypeDescriptor._providerTable : TypeDescriptor._commonSyncObject);
			bool flag2;
			lock (obj)
			{
				flag2 = TypeDescriptor._providerTable.ContainsKey(instance);
				TypeDescriptor.TypeDescriptionNode typeDescriptionNode = TypeDescriptor.NodeFor(instance, true);
				TypeDescriptor.TypeDescriptionNode typeDescriptionNode2 = new TypeDescriptor.TypeDescriptionNode(provider);
				typeDescriptionNode2.Next = typeDescriptionNode;
				TypeDescriptor._providerTable.SetWeak(instance, typeDescriptionNode2);
				TypeDescriptor._providerTypeTable.Clear();
			}
			if (flag2)
			{
				TypeDescriptor.Refresh(instance, false);
			}
		}

		// Token: 0x06003684 RID: 13956 RVA: 0x000ED098 File Offset: 0x000EB298
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public static void AddProviderTransparent(TypeDescriptionProvider provider, Type type)
		{
			if (provider == null)
			{
				throw new ArgumentNullException("provider");
			}
			if (type == null)
			{
				throw new ArgumentNullException("type");
			}
			PermissionSet permissionSet = new PermissionSet(PermissionState.None);
			permissionSet.AddPermission(new TypeDescriptorPermission(TypeDescriptorPermissionFlags.RestrictedRegistrationAccess));
			PermissionSet permissionSet2 = type.Assembly.PermissionSet;
			permissionSet2 = permissionSet2.Union(permissionSet);
			permissionSet2.Demand();
			TypeDescriptor.AddProvider(provider, type);
		}

		// Token: 0x06003685 RID: 13957 RVA: 0x000ED0FC File Offset: 0x000EB2FC
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public static void AddProviderTransparent(TypeDescriptionProvider provider, object instance)
		{
			if (provider == null)
			{
				throw new ArgumentNullException("provider");
			}
			if (instance == null)
			{
				throw new ArgumentNullException("instance");
			}
			Type type = instance.GetType();
			PermissionSet permissionSet = new PermissionSet(PermissionState.None);
			permissionSet.AddPermission(new TypeDescriptorPermission(TypeDescriptorPermissionFlags.RestrictedRegistrationAccess));
			PermissionSet permissionSet2 = type.Assembly.PermissionSet;
			permissionSet2 = permissionSet2.Union(permissionSet);
			permissionSet2.Demand();
			TypeDescriptor.AddProvider(provider, instance);
		}

		// Token: 0x06003686 RID: 13958 RVA: 0x000ED164 File Offset: 0x000EB364
		private static void CheckDefaultProvider(Type type)
		{
			if (LocalAppContextSwitches.DoNotUseTypeDescriptorThreadingFix)
			{
				TypeDescriptor.AddDefaultProviderNoLock(type);
				return;
			}
			if (TypeDescriptor._defaultProviderInitialized[type] == TypeDescriptor._initializedDefaultProvider)
			{
				return;
			}
			object commonSyncObject = TypeDescriptor._commonSyncObject;
			lock (commonSyncObject)
			{
				TypeDescriptor.AddDefaultProvider(type);
			}
		}

		// Token: 0x06003687 RID: 13959 RVA: 0x000ED1C4 File Offset: 0x000EB3C4
		private static void AddDefaultProviderNoLock(Type type)
		{
			if (TypeDescriptor._defaultProviderInitialized.ContainsKey(type))
			{
				return;
			}
			object internalSyncObject = TypeDescriptor._internalSyncObject;
			lock (internalSyncObject)
			{
				if (TypeDescriptor._defaultProviderInitialized.ContainsKey(type))
				{
					return;
				}
				TypeDescriptor._defaultProviderInitialized[type] = null;
			}
			object[] customAttributes = type.GetCustomAttributes(typeof(TypeDescriptionProviderAttribute), false);
			bool flag2 = false;
			for (int i = customAttributes.Length - 1; i >= 0; i--)
			{
				TypeDescriptionProviderAttribute typeDescriptionProviderAttribute = (TypeDescriptionProviderAttribute)customAttributes[i];
				Type type2 = Type.GetType(typeDescriptionProviderAttribute.TypeName);
				if (type2 != null && typeof(TypeDescriptionProvider).IsAssignableFrom(type2))
				{
					IntSecurity.FullReflection.Assert();
					TypeDescriptionProvider typeDescriptionProvider;
					try
					{
						typeDescriptionProvider = (TypeDescriptionProvider)Activator.CreateInstance(type2);
					}
					finally
					{
						CodeAccessPermission.RevertAssert();
					}
					TypeDescriptor.AddProvider(typeDescriptionProvider, type);
					flag2 = true;
				}
			}
			if (!flag2)
			{
				Type baseType = type.BaseType;
				if (baseType != null && baseType != type)
				{
					TypeDescriptor.AddDefaultProviderNoLock(baseType);
				}
			}
		}

		// Token: 0x06003688 RID: 13960 RVA: 0x000ED2E4 File Offset: 0x000EB4E4
		private static void AddDefaultProvider(Type type)
		{
			if (TypeDescriptor._defaultProviderInitialized.ContainsKey(type))
			{
				return;
			}
			TypeDescriptor._defaultProviderInitialized[type] = null;
			object[] customAttributes = type.GetCustomAttributes(typeof(TypeDescriptionProviderAttribute), false);
			bool flag = false;
			for (int i = customAttributes.Length - 1; i >= 0; i--)
			{
				TypeDescriptionProviderAttribute typeDescriptionProviderAttribute = (TypeDescriptionProviderAttribute)customAttributes[i];
				Type type2 = Type.GetType(typeDescriptionProviderAttribute.TypeName);
				if (type2 != null && typeof(TypeDescriptionProvider).IsAssignableFrom(type2))
				{
					IntSecurity.FullReflection.Assert();
					TypeDescriptionProvider typeDescriptionProvider;
					try
					{
						typeDescriptionProvider = (TypeDescriptionProvider)Activator.CreateInstance(type2);
					}
					finally
					{
						CodeAccessPermission.RevertAssert();
					}
					TypeDescriptor.AddProvider(typeDescriptionProvider, type);
					flag = true;
				}
			}
			if (!flag)
			{
				Type baseType = type.BaseType;
				if (baseType != null && baseType != type)
				{
					TypeDescriptor.AddDefaultProvider(baseType);
				}
			}
			TypeDescriptor._defaultProviderInitialized[type] = TypeDescriptor._initializedDefaultProvider;
		}

		// Token: 0x06003689 RID: 13961 RVA: 0x000ED3D4 File Offset: 0x000EB5D4
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
		public static void CreateAssociation(object primary, object secondary)
		{
			if (primary == null)
			{
				throw new ArgumentNullException("primary");
			}
			if (secondary == null)
			{
				throw new ArgumentNullException("secondary");
			}
			if (primary == secondary)
			{
				throw new ArgumentException(SR.GetString("TypeDescriptorSameAssociation"));
			}
			if (TypeDescriptor._associationTable == null)
			{
				object internalSyncObject = TypeDescriptor._internalSyncObject;
				lock (internalSyncObject)
				{
					if (TypeDescriptor._associationTable == null)
					{
						TypeDescriptor._associationTable = new WeakHashtable();
					}
				}
			}
			IList list = (IList)TypeDescriptor._associationTable[primary];
			if (list == null)
			{
				WeakHashtable associationTable = TypeDescriptor._associationTable;
				lock (associationTable)
				{
					list = (IList)TypeDescriptor._associationTable[primary];
					if (list == null)
					{
						list = new ArrayList(4);
						TypeDescriptor._associationTable.SetWeak(primary, list);
					}
					goto IL_0114;
				}
			}
			for (int i = list.Count - 1; i >= 0; i--)
			{
				WeakReference weakReference = (WeakReference)list[i];
				if (weakReference.IsAlive && weakReference.Target == secondary)
				{
					throw new ArgumentException(SR.GetString("TypeDescriptorAlreadyAssociated"));
				}
			}
			IL_0114:
			IList list2 = list;
			lock (list2)
			{
				list.Add(new WeakReference(secondary));
			}
		}

		// Token: 0x0600368A RID: 13962 RVA: 0x000ED548 File Offset: 0x000EB748
		public static IDesigner CreateDesigner(IComponent component, Type designerBaseType)
		{
			Type type = null;
			IDesigner designer = null;
			AttributeCollection attributes = TypeDescriptor.GetAttributes(component);
			for (int i = 0; i < attributes.Count; i++)
			{
				DesignerAttribute designerAttribute = attributes[i] as DesignerAttribute;
				if (designerAttribute != null)
				{
					Type type2 = Type.GetType(designerAttribute.DesignerBaseTypeName);
					if (type2 != null && type2 == designerBaseType)
					{
						ISite site = component.Site;
						bool flag = false;
						if (site != null)
						{
							ITypeResolutionService typeResolutionService = (ITypeResolutionService)site.GetService(typeof(ITypeResolutionService));
							if (typeResolutionService != null)
							{
								flag = true;
								type = typeResolutionService.GetType(designerAttribute.DesignerTypeName);
							}
						}
						if (!flag)
						{
							type = Type.GetType(designerAttribute.DesignerTypeName);
						}
						if (type != null)
						{
							break;
						}
					}
				}
			}
			if (type != null)
			{
				designer = (IDesigner)SecurityUtils.SecureCreateInstance(type, null, true);
			}
			return designer;
		}

		// Token: 0x0600368B RID: 13963 RVA: 0x000ED61A File Offset: 0x000EB81A
		[ReflectionPermission(SecurityAction.LinkDemand, Flags = ReflectionPermissionFlag.MemberAccess)]
		public static EventDescriptor CreateEvent(Type componentType, string name, Type type, params Attribute[] attributes)
		{
			return new ReflectEventDescriptor(componentType, name, type, attributes);
		}

		// Token: 0x0600368C RID: 13964 RVA: 0x000ED625 File Offset: 0x000EB825
		[ReflectionPermission(SecurityAction.LinkDemand, Flags = ReflectionPermissionFlag.MemberAccess)]
		public static EventDescriptor CreateEvent(Type componentType, EventDescriptor oldEventDescriptor, params Attribute[] attributes)
		{
			return new ReflectEventDescriptor(componentType, oldEventDescriptor, attributes);
		}

		// Token: 0x0600368D RID: 13965 RVA: 0x000ED630 File Offset: 0x000EB830
		public static object CreateInstance(IServiceProvider provider, Type objectType, Type[] argTypes, object[] args)
		{
			if (objectType == null)
			{
				throw new ArgumentNullException("objectType");
			}
			if (argTypes != null)
			{
				if (args == null)
				{
					throw new ArgumentNullException("args");
				}
				if (argTypes.Length != args.Length)
				{
					throw new ArgumentException(SR.GetString("TypeDescriptorArgsCountMismatch"));
				}
			}
			object obj = null;
			if (provider != null)
			{
				TypeDescriptionProvider typeDescriptionProvider = provider.GetService(typeof(TypeDescriptionProvider)) as TypeDescriptionProvider;
				if (typeDescriptionProvider != null)
				{
					obj = typeDescriptionProvider.CreateInstance(provider, objectType, argTypes, args);
				}
			}
			if (obj == null)
			{
				obj = TypeDescriptor.NodeFor(objectType).CreateInstance(provider, objectType, argTypes, args);
			}
			return obj;
		}

		// Token: 0x0600368E RID: 13966 RVA: 0x000ED6B7 File Offset: 0x000EB8B7
		[ReflectionPermission(SecurityAction.LinkDemand, Flags = ReflectionPermissionFlag.MemberAccess)]
		public static PropertyDescriptor CreateProperty(Type componentType, string name, Type type, params Attribute[] attributes)
		{
			return new ReflectPropertyDescriptor(componentType, name, type, attributes);
		}

		// Token: 0x0600368F RID: 13967 RVA: 0x000ED6C4 File Offset: 0x000EB8C4
		[ReflectionPermission(SecurityAction.LinkDemand, Flags = ReflectionPermissionFlag.MemberAccess)]
		public static PropertyDescriptor CreateProperty(Type componentType, PropertyDescriptor oldPropertyDescriptor, params Attribute[] attributes)
		{
			if (componentType == oldPropertyDescriptor.ComponentType)
			{
				ExtenderProvidedPropertyAttribute extenderProvidedPropertyAttribute = (ExtenderProvidedPropertyAttribute)oldPropertyDescriptor.Attributes[typeof(ExtenderProvidedPropertyAttribute)];
				ReflectPropertyDescriptor reflectPropertyDescriptor = extenderProvidedPropertyAttribute.ExtenderProperty as ReflectPropertyDescriptor;
				if (reflectPropertyDescriptor != null)
				{
					return new ExtendedPropertyDescriptor(oldPropertyDescriptor, attributes);
				}
			}
			return new ReflectPropertyDescriptor(componentType, oldPropertyDescriptor, attributes);
		}

		// Token: 0x06003690 RID: 13968 RVA: 0x000ED719 File Offset: 0x000EB919
		[Conditional("DEBUG")]
		private static void DebugValidate(Type type, AttributeCollection attributes, AttributeCollection debugAttributes)
		{
		}

		// Token: 0x06003691 RID: 13969 RVA: 0x000ED71B File Offset: 0x000EB91B
		[Conditional("DEBUG")]
		private static void DebugValidate(AttributeCollection attributes, AttributeCollection debugAttributes)
		{
		}

		// Token: 0x06003692 RID: 13970 RVA: 0x000ED71D File Offset: 0x000EB91D
		[Conditional("DEBUG")]
		private static void DebugValidate(AttributeCollection attributes, Type type)
		{
		}

		// Token: 0x06003693 RID: 13971 RVA: 0x000ED71F File Offset: 0x000EB91F
		[Conditional("DEBUG")]
		private static void DebugValidate(AttributeCollection attributes, object instance, bool noCustomTypeDesc)
		{
		}

		// Token: 0x06003694 RID: 13972 RVA: 0x000ED721 File Offset: 0x000EB921
		[Conditional("DEBUG")]
		private static void DebugValidate(TypeConverter converter, Type type)
		{
		}

		// Token: 0x06003695 RID: 13973 RVA: 0x000ED723 File Offset: 0x000EB923
		[Conditional("DEBUG")]
		private static void DebugValidate(TypeConverter converter, object instance, bool noCustomTypeDesc)
		{
		}

		// Token: 0x06003696 RID: 13974 RVA: 0x000ED725 File Offset: 0x000EB925
		[Conditional("DEBUG")]
		private static void DebugValidate(EventDescriptorCollection events, Type type, Attribute[] attributes)
		{
		}

		// Token: 0x06003697 RID: 13975 RVA: 0x000ED727 File Offset: 0x000EB927
		[Conditional("DEBUG")]
		private static void DebugValidate(EventDescriptorCollection events, object instance, Attribute[] attributes, bool noCustomTypeDesc)
		{
		}

		// Token: 0x06003698 RID: 13976 RVA: 0x000ED729 File Offset: 0x000EB929
		[Conditional("DEBUG")]
		private static void DebugValidate(PropertyDescriptorCollection properties, Type type, Attribute[] attributes)
		{
		}

		// Token: 0x06003699 RID: 13977 RVA: 0x000ED72B File Offset: 0x000EB92B
		[Conditional("DEBUG")]
		private static void DebugValidate(PropertyDescriptorCollection properties, object instance, Attribute[] attributes, bool noCustomTypeDesc)
		{
		}

		// Token: 0x0600369A RID: 13978 RVA: 0x000ED730 File Offset: 0x000EB930
		private static ArrayList FilterMembers(IList members, Attribute[] attributes)
		{
			ArrayList arrayList = null;
			int count = members.Count;
			for (int i = 0; i < count; i++)
			{
				bool flag = false;
				for (int j = 0; j < attributes.Length; j++)
				{
					if (TypeDescriptor.ShouldHideMember((MemberDescriptor)members[i], attributes[j]))
					{
						flag = true;
						break;
					}
				}
				if (flag)
				{
					if (arrayList == null)
					{
						arrayList = new ArrayList(count);
						for (int k = 0; k < i; k++)
						{
							arrayList.Add(members[k]);
						}
					}
				}
				else if (arrayList != null)
				{
					arrayList.Add(members[i]);
				}
			}
			return arrayList;
		}

		// Token: 0x0600369B RID: 13979 RVA: 0x000ED7C4 File Offset: 0x000EB9C4
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public static object GetAssociation(Type type, object primary)
		{
			if (type == null)
			{
				throw new ArgumentNullException("type");
			}
			if (primary == null)
			{
				throw new ArgumentNullException("primary");
			}
			object obj = primary;
			if (!type.IsInstanceOfType(primary))
			{
				Hashtable associationTable = TypeDescriptor._associationTable;
				if (associationTable != null)
				{
					IList list = (IList)associationTable[primary];
					if (list != null)
					{
						IList list2 = list;
						lock (list2)
						{
							for (int i = list.Count - 1; i >= 0; i--)
							{
								WeakReference weakReference = (WeakReference)list[i];
								object target = weakReference.Target;
								if (target == null)
								{
									list.RemoveAt(i);
								}
								else if (type.IsInstanceOfType(target))
								{
									obj = target;
								}
							}
						}
					}
				}
				if (obj == primary)
				{
					IComponent component = primary as IComponent;
					if (component != null)
					{
						ISite site = component.Site;
						if (site != null && site.DesignMode)
						{
							IDesignerHost designerHost = site.GetService(typeof(IDesignerHost)) as IDesignerHost;
							if (designerHost != null)
							{
								object designer = designerHost.GetDesigner(component);
								if (designer != null && type.IsInstanceOfType(designer))
								{
									obj = designer;
								}
							}
						}
					}
				}
			}
			return obj;
		}

		// Token: 0x0600369C RID: 13980 RVA: 0x000ED8F0 File Offset: 0x000EBAF0
		public static AttributeCollection GetAttributes(Type componentType)
		{
			if (componentType == null)
			{
				return new AttributeCollection(null);
			}
			return TypeDescriptor.GetDescriptor(componentType, "componentType").GetAttributes();
		}

		// Token: 0x0600369D RID: 13981 RVA: 0x000ED91F File Offset: 0x000EBB1F
		public static AttributeCollection GetAttributes(object component)
		{
			return TypeDescriptor.GetAttributes(component, false);
		}

		// Token: 0x0600369E RID: 13982 RVA: 0x000ED928 File Offset: 0x000EBB28
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public static AttributeCollection GetAttributes(object component, bool noCustomTypeDesc)
		{
			if (component == null)
			{
				return new AttributeCollection(null);
			}
			ICustomTypeDescriptor descriptor = TypeDescriptor.GetDescriptor(component, noCustomTypeDesc);
			ICollection collection = descriptor.GetAttributes();
			if (component is ICustomTypeDescriptor)
			{
				if (noCustomTypeDesc)
				{
					ICustomTypeDescriptor extendedDescriptor = TypeDescriptor.GetExtendedDescriptor(component);
					if (extendedDescriptor != null)
					{
						ICollection attributes = extendedDescriptor.GetAttributes();
						collection = TypeDescriptor.PipelineMerge(0, collection, attributes, component, null);
					}
				}
				else
				{
					collection = TypeDescriptor.PipelineFilter(0, collection, component, null);
				}
			}
			else
			{
				IDictionary cache = TypeDescriptor.GetCache(component);
				collection = TypeDescriptor.PipelineInitialize(0, collection, cache);
				ICustomTypeDescriptor extendedDescriptor2 = TypeDescriptor.GetExtendedDescriptor(component);
				if (extendedDescriptor2 != null)
				{
					ICollection attributes2 = extendedDescriptor2.GetAttributes();
					collection = TypeDescriptor.PipelineMerge(0, collection, attributes2, component, cache);
				}
				collection = TypeDescriptor.PipelineFilter(0, collection, component, cache);
			}
			AttributeCollection attributeCollection = collection as AttributeCollection;
			if (attributeCollection == null)
			{
				Attribute[] array = new Attribute[collection.Count];
				collection.CopyTo(array, 0);
				attributeCollection = new AttributeCollection(array);
			}
			return attributeCollection;
		}

		// Token: 0x0600369F RID: 13983 RVA: 0x000ED9ED File Offset: 0x000EBBED
		internal static IDictionary GetCache(object instance)
		{
			return TypeDescriptor.NodeFor(instance).GetCache(instance);
		}

		// Token: 0x060036A0 RID: 13984 RVA: 0x000ED9FB File Offset: 0x000EBBFB
		public static string GetClassName(object component)
		{
			return TypeDescriptor.GetClassName(component, false);
		}

		// Token: 0x060036A1 RID: 13985 RVA: 0x000EDA04 File Offset: 0x000EBC04
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public static string GetClassName(object component, bool noCustomTypeDesc)
		{
			return TypeDescriptor.GetDescriptor(component, noCustomTypeDesc).GetClassName();
		}

		// Token: 0x060036A2 RID: 13986 RVA: 0x000EDA12 File Offset: 0x000EBC12
		public static string GetClassName(Type componentType)
		{
			return TypeDescriptor.GetDescriptor(componentType, "componentType").GetClassName();
		}

		// Token: 0x060036A3 RID: 13987 RVA: 0x000EDA24 File Offset: 0x000EBC24
		public static string GetComponentName(object component)
		{
			return TypeDescriptor.GetComponentName(component, false);
		}

		// Token: 0x060036A4 RID: 13988 RVA: 0x000EDA2D File Offset: 0x000EBC2D
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public static string GetComponentName(object component, bool noCustomTypeDesc)
		{
			return TypeDescriptor.GetDescriptor(component, noCustomTypeDesc).GetComponentName();
		}

		// Token: 0x060036A5 RID: 13989 RVA: 0x000EDA3B File Offset: 0x000EBC3B
		public static TypeConverter GetConverter(object component)
		{
			return TypeDescriptor.GetConverter(component, false);
		}

		// Token: 0x060036A6 RID: 13990 RVA: 0x000EDA44 File Offset: 0x000EBC44
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public static TypeConverter GetConverter(object component, bool noCustomTypeDesc)
		{
			return TypeDescriptor.GetDescriptor(component, noCustomTypeDesc).GetConverter();
		}

		// Token: 0x060036A7 RID: 13991 RVA: 0x000EDA60 File Offset: 0x000EBC60
		public static TypeConverter GetConverter(Type type)
		{
			return TypeDescriptor.GetDescriptor(type, "type").GetConverter();
		}

		// Token: 0x060036A8 RID: 13992 RVA: 0x000EDA7F File Offset: 0x000EBC7F
		public static EventDescriptor GetDefaultEvent(Type componentType)
		{
			if (componentType == null)
			{
				return null;
			}
			return TypeDescriptor.GetDescriptor(componentType, "componentType").GetDefaultEvent();
		}

		// Token: 0x060036A9 RID: 13993 RVA: 0x000EDA9C File Offset: 0x000EBC9C
		public static EventDescriptor GetDefaultEvent(object component)
		{
			return TypeDescriptor.GetDefaultEvent(component, false);
		}

		// Token: 0x060036AA RID: 13994 RVA: 0x000EDAA5 File Offset: 0x000EBCA5
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public static EventDescriptor GetDefaultEvent(object component, bool noCustomTypeDesc)
		{
			if (component == null)
			{
				return null;
			}
			return TypeDescriptor.GetDescriptor(component, noCustomTypeDesc).GetDefaultEvent();
		}

		// Token: 0x060036AB RID: 13995 RVA: 0x000EDAB8 File Offset: 0x000EBCB8
		public static PropertyDescriptor GetDefaultProperty(Type componentType)
		{
			if (componentType == null)
			{
				return null;
			}
			return TypeDescriptor.GetDescriptor(componentType, "componentType").GetDefaultProperty();
		}

		// Token: 0x060036AC RID: 13996 RVA: 0x000EDAD5 File Offset: 0x000EBCD5
		public static PropertyDescriptor GetDefaultProperty(object component)
		{
			return TypeDescriptor.GetDefaultProperty(component, false);
		}

		// Token: 0x060036AD RID: 13997 RVA: 0x000EDADE File Offset: 0x000EBCDE
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public static PropertyDescriptor GetDefaultProperty(object component, bool noCustomTypeDesc)
		{
			if (component == null)
			{
				return null;
			}
			return TypeDescriptor.GetDescriptor(component, noCustomTypeDesc).GetDefaultProperty();
		}

		// Token: 0x060036AE RID: 13998 RVA: 0x000EDAF1 File Offset: 0x000EBCF1
		internal static ICustomTypeDescriptor GetDescriptor(Type type, string typeName)
		{
			if (type == null)
			{
				throw new ArgumentNullException(typeName);
			}
			return TypeDescriptor.NodeFor(type).GetTypeDescriptor(type);
		}

		// Token: 0x060036AF RID: 13999 RVA: 0x000EDB10 File Offset: 0x000EBD10
		internal static ICustomTypeDescriptor GetDescriptor(object component, bool noCustomTypeDesc)
		{
			if (component == null)
			{
				throw new ArgumentException("component");
			}
			if (component is TypeDescriptor.IUnimplemented)
			{
				throw new NotSupportedException(SR.GetString("TypeDescriptorUnsupportedRemoteObject", new object[] { component.GetType().FullName }));
			}
			ICustomTypeDescriptor customTypeDescriptor = TypeDescriptor.NodeFor(component).GetTypeDescriptor(component);
			ICustomTypeDescriptor customTypeDescriptor2 = component as ICustomTypeDescriptor;
			if (!noCustomTypeDesc && customTypeDescriptor2 != null)
			{
				customTypeDescriptor = new TypeDescriptor.MergedTypeDescriptor(customTypeDescriptor2, customTypeDescriptor);
			}
			return customTypeDescriptor;
		}

		// Token: 0x060036B0 RID: 14000 RVA: 0x000EDB7A File Offset: 0x000EBD7A
		internal static ICustomTypeDescriptor GetExtendedDescriptor(object component)
		{
			if (component == null)
			{
				throw new ArgumentException("component");
			}
			return TypeDescriptor.NodeFor(component).GetExtendedTypeDescriptor(component);
		}

		// Token: 0x060036B1 RID: 14001 RVA: 0x000EDB96 File Offset: 0x000EBD96
		public static object GetEditor(object component, Type editorBaseType)
		{
			return TypeDescriptor.GetEditor(component, editorBaseType, false);
		}

		// Token: 0x060036B2 RID: 14002 RVA: 0x000EDBA0 File Offset: 0x000EBDA0
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public static object GetEditor(object component, Type editorBaseType, bool noCustomTypeDesc)
		{
			if (editorBaseType == null)
			{
				throw new ArgumentNullException("editorBaseType");
			}
			return TypeDescriptor.GetDescriptor(component, noCustomTypeDesc).GetEditor(editorBaseType);
		}

		// Token: 0x060036B3 RID: 14003 RVA: 0x000EDBC3 File Offset: 0x000EBDC3
		public static object GetEditor(Type type, Type editorBaseType)
		{
			if (editorBaseType == null)
			{
				throw new ArgumentNullException("editorBaseType");
			}
			return TypeDescriptor.GetDescriptor(type, "type").GetEditor(editorBaseType);
		}

		// Token: 0x060036B4 RID: 14004 RVA: 0x000EDBEA File Offset: 0x000EBDEA
		public static EventDescriptorCollection GetEvents(Type componentType)
		{
			if (componentType == null)
			{
				return new EventDescriptorCollection(null, true);
			}
			return TypeDescriptor.GetDescriptor(componentType, "componentType").GetEvents();
		}

		// Token: 0x060036B5 RID: 14005 RVA: 0x000EDC10 File Offset: 0x000EBE10
		public static EventDescriptorCollection GetEvents(Type componentType, Attribute[] attributes)
		{
			if (componentType == null)
			{
				return new EventDescriptorCollection(null, true);
			}
			EventDescriptorCollection eventDescriptorCollection = TypeDescriptor.GetDescriptor(componentType, "componentType").GetEvents(attributes);
			if (attributes != null && attributes.Length != 0)
			{
				ArrayList arrayList = TypeDescriptor.FilterMembers(eventDescriptorCollection, attributes);
				if (arrayList != null)
				{
					eventDescriptorCollection = new EventDescriptorCollection((EventDescriptor[])arrayList.ToArray(typeof(EventDescriptor)), true);
				}
			}
			return eventDescriptorCollection;
		}

		// Token: 0x060036B6 RID: 14006 RVA: 0x000EDC6F File Offset: 0x000EBE6F
		public static EventDescriptorCollection GetEvents(object component)
		{
			return TypeDescriptor.GetEvents(component, null, false);
		}

		// Token: 0x060036B7 RID: 14007 RVA: 0x000EDC79 File Offset: 0x000EBE79
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public static EventDescriptorCollection GetEvents(object component, bool noCustomTypeDesc)
		{
			return TypeDescriptor.GetEvents(component, null, noCustomTypeDesc);
		}

		// Token: 0x060036B8 RID: 14008 RVA: 0x000EDC83 File Offset: 0x000EBE83
		public static EventDescriptorCollection GetEvents(object component, Attribute[] attributes)
		{
			return TypeDescriptor.GetEvents(component, attributes, false);
		}

		// Token: 0x060036B9 RID: 14009 RVA: 0x000EDC90 File Offset: 0x000EBE90
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public static EventDescriptorCollection GetEvents(object component, Attribute[] attributes, bool noCustomTypeDesc)
		{
			if (component == null)
			{
				return new EventDescriptorCollection(null, true);
			}
			ICustomTypeDescriptor descriptor = TypeDescriptor.GetDescriptor(component, noCustomTypeDesc);
			ICollection collection;
			if (component is ICustomTypeDescriptor)
			{
				collection = descriptor.GetEvents(attributes);
				if (noCustomTypeDesc)
				{
					ICustomTypeDescriptor extendedDescriptor = TypeDescriptor.GetExtendedDescriptor(component);
					if (extendedDescriptor != null)
					{
						ICollection events = extendedDescriptor.GetEvents(attributes);
						collection = TypeDescriptor.PipelineMerge(2, collection, events, component, null);
					}
				}
				else
				{
					collection = TypeDescriptor.PipelineFilter(2, collection, component, null);
					collection = TypeDescriptor.PipelineAttributeFilter(2, collection, attributes, component, null);
				}
			}
			else
			{
				IDictionary cache = TypeDescriptor.GetCache(component);
				collection = descriptor.GetEvents(attributes);
				collection = TypeDescriptor.PipelineInitialize(2, collection, cache);
				ICustomTypeDescriptor extendedDescriptor2 = TypeDescriptor.GetExtendedDescriptor(component);
				if (extendedDescriptor2 != null)
				{
					ICollection events2 = extendedDescriptor2.GetEvents(attributes);
					collection = TypeDescriptor.PipelineMerge(2, collection, events2, component, cache);
				}
				collection = TypeDescriptor.PipelineFilter(2, collection, component, cache);
				collection = TypeDescriptor.PipelineAttributeFilter(2, collection, attributes, component, cache);
			}
			EventDescriptorCollection eventDescriptorCollection = collection as EventDescriptorCollection;
			if (eventDescriptorCollection == null)
			{
				EventDescriptor[] array = new EventDescriptor[collection.Count];
				collection.CopyTo(array, 0);
				eventDescriptorCollection = new EventDescriptorCollection(array, true);
			}
			return eventDescriptorCollection;
		}

		// Token: 0x060036BA RID: 14010 RVA: 0x000EDD7C File Offset: 0x000EBF7C
		private static string GetExtenderCollisionSuffix(MemberDescriptor member)
		{
			string text = null;
			ExtenderProvidedPropertyAttribute extenderProvidedPropertyAttribute = member.Attributes[typeof(ExtenderProvidedPropertyAttribute)] as ExtenderProvidedPropertyAttribute;
			if (extenderProvidedPropertyAttribute != null)
			{
				IExtenderProvider provider = extenderProvidedPropertyAttribute.Provider;
				if (provider != null)
				{
					string text2 = null;
					IComponent component = provider as IComponent;
					if (component != null && component.Site != null)
					{
						text2 = component.Site.Name;
					}
					if (text2 == null || text2.Length == 0)
					{
						text2 = (Interlocked.Increment(ref TypeDescriptor._collisionIndex) - 1).ToString(CultureInfo.InvariantCulture);
					}
					text = string.Format(CultureInfo.InvariantCulture, "_{0}", new object[] { text2 });
				}
			}
			return text;
		}

		// Token: 0x060036BB RID: 14011 RVA: 0x000EDE18 File Offset: 0x000EC018
		public static string GetFullComponentName(object component)
		{
			if (component == null)
			{
				throw new ArgumentNullException("component");
			}
			return TypeDescriptor.GetProvider(component).GetFullComponentName(component);
		}

		// Token: 0x060036BC RID: 14012 RVA: 0x000EDE34 File Offset: 0x000EC034
		private static Type GetNodeForBaseType(Type searchType)
		{
			if (searchType.IsInterface)
			{
				return TypeDescriptor.InterfaceType;
			}
			if (searchType == TypeDescriptor.InterfaceType)
			{
				return null;
			}
			return searchType.BaseType;
		}

		// Token: 0x060036BD RID: 14013 RVA: 0x000EDE59 File Offset: 0x000EC059
		public static PropertyDescriptorCollection GetProperties(Type componentType)
		{
			if (componentType == null)
			{
				return new PropertyDescriptorCollection(null, true);
			}
			return TypeDescriptor.GetDescriptor(componentType, "componentType").GetProperties();
		}

		// Token: 0x060036BE RID: 14014 RVA: 0x000EDE7C File Offset: 0x000EC07C
		public static PropertyDescriptorCollection GetProperties(Type componentType, Attribute[] attributes)
		{
			if (componentType == null)
			{
				return new PropertyDescriptorCollection(null, true);
			}
			PropertyDescriptorCollection propertyDescriptorCollection = TypeDescriptor.GetDescriptor(componentType, "componentType").GetProperties(attributes);
			if (attributes != null && attributes.Length != 0)
			{
				ArrayList arrayList = TypeDescriptor.FilterMembers(propertyDescriptorCollection, attributes);
				if (arrayList != null)
				{
					propertyDescriptorCollection = new PropertyDescriptorCollection((PropertyDescriptor[])arrayList.ToArray(typeof(PropertyDescriptor)), true);
				}
			}
			return propertyDescriptorCollection;
		}

		// Token: 0x060036BF RID: 14015 RVA: 0x000EDEDB File Offset: 0x000EC0DB
		public static PropertyDescriptorCollection GetProperties(object component)
		{
			return TypeDescriptor.GetProperties(component, false);
		}

		// Token: 0x060036C0 RID: 14016 RVA: 0x000EDEE4 File Offset: 0x000EC0E4
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public static PropertyDescriptorCollection GetProperties(object component, bool noCustomTypeDesc)
		{
			return TypeDescriptor.GetPropertiesImpl(component, null, noCustomTypeDesc, true);
		}

		// Token: 0x060036C1 RID: 14017 RVA: 0x000EDEEF File Offset: 0x000EC0EF
		public static PropertyDescriptorCollection GetProperties(object component, Attribute[] attributes)
		{
			return TypeDescriptor.GetProperties(component, attributes, false);
		}

		// Token: 0x060036C2 RID: 14018 RVA: 0x000EDEF9 File Offset: 0x000EC0F9
		public static PropertyDescriptorCollection GetProperties(object component, Attribute[] attributes, bool noCustomTypeDesc)
		{
			return TypeDescriptor.GetPropertiesImpl(component, attributes, noCustomTypeDesc, false);
		}

		// Token: 0x060036C3 RID: 14019 RVA: 0x000EDF04 File Offset: 0x000EC104
		private static PropertyDescriptorCollection GetPropertiesImpl(object component, Attribute[] attributes, bool noCustomTypeDesc, bool noAttributes)
		{
			if (component == null)
			{
				return new PropertyDescriptorCollection(null, true);
			}
			ICustomTypeDescriptor descriptor = TypeDescriptor.GetDescriptor(component, noCustomTypeDesc);
			ICollection collection;
			if (component is ICustomTypeDescriptor)
			{
				collection = (noAttributes ? descriptor.GetProperties() : descriptor.GetProperties(attributes));
				if (noCustomTypeDesc)
				{
					ICustomTypeDescriptor extendedDescriptor = TypeDescriptor.GetExtendedDescriptor(component);
					if (extendedDescriptor != null)
					{
						ICollection collection2 = (noAttributes ? extendedDescriptor.GetProperties() : extendedDescriptor.GetProperties(attributes));
						collection = TypeDescriptor.PipelineMerge(1, collection, collection2, component, null);
					}
				}
				else
				{
					collection = TypeDescriptor.PipelineFilter(1, collection, component, null);
					collection = TypeDescriptor.PipelineAttributeFilter(1, collection, attributes, component, null);
				}
			}
			else
			{
				IDictionary cache = TypeDescriptor.GetCache(component);
				collection = (noAttributes ? descriptor.GetProperties() : descriptor.GetProperties(attributes));
				collection = TypeDescriptor.PipelineInitialize(1, collection, cache);
				ICustomTypeDescriptor extendedDescriptor2 = TypeDescriptor.GetExtendedDescriptor(component);
				if (extendedDescriptor2 != null)
				{
					ICollection collection3 = (noAttributes ? extendedDescriptor2.GetProperties() : extendedDescriptor2.GetProperties(attributes));
					collection = TypeDescriptor.PipelineMerge(1, collection, collection3, component, cache);
				}
				collection = TypeDescriptor.PipelineFilter(1, collection, component, cache);
				collection = TypeDescriptor.PipelineAttributeFilter(1, collection, attributes, component, cache);
			}
			PropertyDescriptorCollection propertyDescriptorCollection = collection as PropertyDescriptorCollection;
			if (propertyDescriptorCollection == null)
			{
				PropertyDescriptor[] array = new PropertyDescriptor[collection.Count];
				collection.CopyTo(array, 0);
				propertyDescriptorCollection = new PropertyDescriptorCollection(array, true);
			}
			return propertyDescriptorCollection;
		}

		// Token: 0x060036C4 RID: 14020 RVA: 0x000EE020 File Offset: 0x000EC220
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public static TypeDescriptionProvider GetProvider(Type type)
		{
			if (type == null)
			{
				throw new ArgumentNullException("type");
			}
			return TypeDescriptor.NodeFor(type, true);
		}

		// Token: 0x060036C5 RID: 14021 RVA: 0x000EE03D File Offset: 0x000EC23D
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public static TypeDescriptionProvider GetProvider(object instance)
		{
			if (instance == null)
			{
				throw new ArgumentNullException("instance");
			}
			return TypeDescriptor.NodeFor(instance, true);
		}

		// Token: 0x060036C6 RID: 14022 RVA: 0x000EE054 File Offset: 0x000EC254
		internal static TypeDescriptionProvider GetProviderRecursive(Type type)
		{
			return TypeDescriptor.NodeFor(type, false);
		}

		// Token: 0x060036C7 RID: 14023 RVA: 0x000EE05D File Offset: 0x000EC25D
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public static Type GetReflectionType(Type type)
		{
			if (type == null)
			{
				throw new ArgumentNullException("type");
			}
			return TypeDescriptor.NodeFor(type).GetReflectionType(type);
		}

		// Token: 0x060036C8 RID: 14024 RVA: 0x000EE07F File Offset: 0x000EC27F
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public static Type GetReflectionType(object instance)
		{
			if (instance == null)
			{
				throw new ArgumentNullException("instance");
			}
			return TypeDescriptor.NodeFor(instance).GetReflectionType(instance);
		}

		// Token: 0x060036C9 RID: 14025 RVA: 0x000EE09B File Offset: 0x000EC29B
		private static TypeDescriptor.TypeDescriptionNode NodeFor(Type type)
		{
			return TypeDescriptor.NodeFor(type, false);
		}

		// Token: 0x060036CA RID: 14026 RVA: 0x000EE0A4 File Offset: 0x000EC2A4
		private static TypeDescriptor.TypeDescriptionNode NodeFor(Type type, bool createDelegator)
		{
			TypeDescriptor.CheckDefaultProvider(type);
			TypeDescriptor.TypeDescriptionNode typeDescriptionNode = null;
			Type type2 = type;
			while (typeDescriptionNode == null)
			{
				typeDescriptionNode = (TypeDescriptor.TypeDescriptionNode)TypeDescriptor._providerTypeTable[type2];
				if (typeDescriptionNode == null)
				{
					typeDescriptionNode = (TypeDescriptor.TypeDescriptionNode)TypeDescriptor._providerTable[type2];
				}
				if (typeDescriptionNode == null)
				{
					Type nodeForBaseType = TypeDescriptor.GetNodeForBaseType(type2);
					if (type2 == typeof(object) || nodeForBaseType == null)
					{
						object obj = (LocalAppContextSwitches.DoNotUseTypeDescriptorThreadingFix ? TypeDescriptor._providerTable : TypeDescriptor._commonSyncObject);
						lock (obj)
						{
							typeDescriptionNode = (TypeDescriptor.TypeDescriptionNode)TypeDescriptor._providerTable[type2];
							if (typeDescriptionNode == null)
							{
								typeDescriptionNode = new TypeDescriptor.TypeDescriptionNode(new ReflectTypeDescriptionProvider());
								TypeDescriptor._providerTable[type2] = typeDescriptionNode;
							}
							continue;
						}
					}
					if (createDelegator)
					{
						typeDescriptionNode = new TypeDescriptor.TypeDescriptionNode(new DelegatingTypeDescriptionProvider(nodeForBaseType));
						object obj2 = (LocalAppContextSwitches.DoNotUseTypeDescriptorThreadingFix ? TypeDescriptor._providerTable : TypeDescriptor._commonSyncObject);
						lock (obj2)
						{
							TypeDescriptor._providerTypeTable[type2] = typeDescriptionNode;
							continue;
						}
					}
					type2 = nodeForBaseType;
				}
			}
			return typeDescriptionNode;
		}

		// Token: 0x060036CB RID: 14027 RVA: 0x000EE1D4 File Offset: 0x000EC3D4
		private static TypeDescriptor.TypeDescriptionNode NodeFor(object instance)
		{
			return TypeDescriptor.NodeFor(instance, false);
		}

		// Token: 0x060036CC RID: 14028 RVA: 0x000EE1E0 File Offset: 0x000EC3E0
		private static TypeDescriptor.TypeDescriptionNode NodeFor(object instance, bool createDelegator)
		{
			TypeDescriptor.TypeDescriptionNode typeDescriptionNode = (TypeDescriptor.TypeDescriptionNode)TypeDescriptor._providerTable[instance];
			if (typeDescriptionNode == null)
			{
				Type type = instance.GetType();
				if (type.IsCOMObject)
				{
					type = TypeDescriptor.ComObjectType;
				}
				if (createDelegator)
				{
					typeDescriptionNode = new TypeDescriptor.TypeDescriptionNode(new DelegatingTypeDescriptionProvider(type));
				}
				else
				{
					typeDescriptionNode = TypeDescriptor.NodeFor(type);
				}
			}
			return typeDescriptionNode;
		}

		// Token: 0x060036CD RID: 14029 RVA: 0x000EE230 File Offset: 0x000EC430
		private static void NodeRemove(object key, TypeDescriptionProvider provider)
		{
			object obj = (LocalAppContextSwitches.DoNotUseTypeDescriptorThreadingFix ? TypeDescriptor._providerTable : TypeDescriptor._commonSyncObject);
			lock (obj)
			{
				TypeDescriptor.TypeDescriptionNode typeDescriptionNode = (TypeDescriptor.TypeDescriptionNode)TypeDescriptor._providerTable[key];
				TypeDescriptor.TypeDescriptionNode typeDescriptionNode2 = typeDescriptionNode;
				while (typeDescriptionNode2 != null && typeDescriptionNode2.Provider != provider)
				{
					typeDescriptionNode2 = typeDescriptionNode2.Next;
				}
				if (typeDescriptionNode2 != null)
				{
					if (typeDescriptionNode2.Next != null)
					{
						typeDescriptionNode2.Provider = typeDescriptionNode2.Next.Provider;
						typeDescriptionNode2.Next = typeDescriptionNode2.Next.Next;
						if (typeDescriptionNode2 == typeDescriptionNode && typeDescriptionNode2.Provider is DelegatingTypeDescriptionProvider)
						{
							TypeDescriptor._providerTable.Remove(key);
						}
					}
					else if (typeDescriptionNode2 != typeDescriptionNode)
					{
						Type type = key as Type;
						if (type == null)
						{
							type = key.GetType();
						}
						typeDescriptionNode2.Provider = new DelegatingTypeDescriptionProvider(type.BaseType);
					}
					else
					{
						TypeDescriptor._providerTable.Remove(key);
					}
					TypeDescriptor._providerTypeTable.Clear();
				}
			}
		}

		// Token: 0x060036CE RID: 14030 RVA: 0x000EE33C File Offset: 0x000EC53C
		private static ICollection PipelineAttributeFilter(int pipelineType, ICollection members, Attribute[] filter, object instance, IDictionary cache)
		{
			IList list = members as ArrayList;
			if (filter == null || filter.Length == 0)
			{
				return members;
			}
			if (cache != null && (list == null || list.IsReadOnly))
			{
				TypeDescriptor.AttributeFilterCacheItem attributeFilterCacheItem = cache[TypeDescriptor._pipelineAttributeFilterKeys[pipelineType]] as TypeDescriptor.AttributeFilterCacheItem;
				if (attributeFilterCacheItem != null && attributeFilterCacheItem.IsValid(filter))
				{
					return attributeFilterCacheItem.FilteredMembers;
				}
			}
			if (list == null || list.IsReadOnly)
			{
				list = new ArrayList(members);
			}
			ArrayList arrayList = TypeDescriptor.FilterMembers(list, filter);
			if (arrayList != null)
			{
				list = arrayList;
			}
			if (cache != null)
			{
				ICollection collection;
				if (pipelineType != 1)
				{
					if (pipelineType != 2)
					{
						collection = null;
					}
					else
					{
						EventDescriptor[] array = new EventDescriptor[list.Count];
						list.CopyTo(array, 0);
						collection = new EventDescriptorCollection(array, true);
					}
				}
				else
				{
					PropertyDescriptor[] array2 = new PropertyDescriptor[list.Count];
					list.CopyTo(array2, 0);
					collection = new PropertyDescriptorCollection(array2, true);
				}
				TypeDescriptor.AttributeFilterCacheItem attributeFilterCacheItem2 = new TypeDescriptor.AttributeFilterCacheItem(filter, collection);
				cache[TypeDescriptor._pipelineAttributeFilterKeys[pipelineType]] = attributeFilterCacheItem2;
			}
			return list;
		}

		// Token: 0x060036CF RID: 14031 RVA: 0x000EE42C File Offset: 0x000EC62C
		private static ICollection PipelineFilter(int pipelineType, ICollection members, object instance, IDictionary cache)
		{
			IComponent component = instance as IComponent;
			ITypeDescriptorFilterService typeDescriptorFilterService = null;
			if (component != null)
			{
				ISite site = component.Site;
				if (site != null)
				{
					typeDescriptorFilterService = site.GetService(typeof(ITypeDescriptorFilterService)) as ITypeDescriptorFilterService;
				}
			}
			IList list = members as ArrayList;
			if (typeDescriptorFilterService == null)
			{
				return members;
			}
			if (cache != null && (list == null || list.IsReadOnly))
			{
				TypeDescriptor.FilterCacheItem filterCacheItem = cache[TypeDescriptor._pipelineFilterKeys[pipelineType]] as TypeDescriptor.FilterCacheItem;
				if (filterCacheItem != null && filterCacheItem.IsValid(typeDescriptorFilterService))
				{
					return filterCacheItem.FilteredMembers;
				}
			}
			OrderedDictionary orderedDictionary = new OrderedDictionary(members.Count);
			bool flag;
			if (pipelineType != 0)
			{
				if (pipelineType - 1 > 1)
				{
					flag = false;
				}
				else
				{
					foreach (object obj in members)
					{
						MemberDescriptor memberDescriptor = (MemberDescriptor)obj;
						string name = memberDescriptor.Name;
						if (orderedDictionary.Contains(name))
						{
							string text = TypeDescriptor.GetExtenderCollisionSuffix(memberDescriptor);
							if (text != null)
							{
								orderedDictionary[name + text] = memberDescriptor;
							}
							MemberDescriptor memberDescriptor2 = (MemberDescriptor)orderedDictionary[name];
							text = TypeDescriptor.GetExtenderCollisionSuffix(memberDescriptor2);
							if (text != null)
							{
								orderedDictionary.Remove(name);
								orderedDictionary[memberDescriptor2.Name + text] = memberDescriptor2;
							}
						}
						else
						{
							orderedDictionary[name] = memberDescriptor;
						}
					}
					if (pipelineType == 1)
					{
						flag = typeDescriptorFilterService.FilterProperties(component, orderedDictionary);
					}
					else
					{
						flag = typeDescriptorFilterService.FilterEvents(component, orderedDictionary);
					}
				}
			}
			else
			{
				foreach (object obj2 in members)
				{
					Attribute attribute = (Attribute)obj2;
					orderedDictionary[attribute.TypeId] = attribute;
				}
				flag = typeDescriptorFilterService.FilterAttributes(component, orderedDictionary);
			}
			if (list == null || list.IsReadOnly)
			{
				list = new ArrayList(orderedDictionary.Values);
			}
			else
			{
				list.Clear();
				foreach (object obj3 in orderedDictionary.Values)
				{
					list.Add(obj3);
				}
			}
			if (flag && cache != null)
			{
				ICollection collection;
				switch (pipelineType)
				{
				case 0:
				{
					Attribute[] array = new Attribute[list.Count];
					try
					{
						list.CopyTo(array, 0);
					}
					catch (InvalidCastException)
					{
						throw new ArgumentException(SR.GetString("TypeDescriptorExpectedElementType", new object[] { typeof(Attribute).FullName }));
					}
					collection = new AttributeCollection(array);
					break;
				}
				case 1:
				{
					PropertyDescriptor[] array2 = new PropertyDescriptor[list.Count];
					try
					{
						list.CopyTo(array2, 0);
					}
					catch (InvalidCastException)
					{
						throw new ArgumentException(SR.GetString("TypeDescriptorExpectedElementType", new object[] { typeof(PropertyDescriptor).FullName }));
					}
					collection = new PropertyDescriptorCollection(array2, true);
					break;
				}
				case 2:
				{
					EventDescriptor[] array3 = new EventDescriptor[list.Count];
					try
					{
						list.CopyTo(array3, 0);
					}
					catch (InvalidCastException)
					{
						throw new ArgumentException(SR.GetString("TypeDescriptorExpectedElementType", new object[] { typeof(EventDescriptor).FullName }));
					}
					collection = new EventDescriptorCollection(array3, true);
					break;
				}
				default:
					collection = null;
					break;
				}
				TypeDescriptor.FilterCacheItem filterCacheItem2 = new TypeDescriptor.FilterCacheItem(typeDescriptorFilterService, collection);
				cache[TypeDescriptor._pipelineFilterKeys[pipelineType]] = filterCacheItem2;
				cache.Remove(TypeDescriptor._pipelineAttributeFilterKeys[pipelineType]);
			}
			return list;
		}

		// Token: 0x060036D0 RID: 14032 RVA: 0x000EE7F0 File Offset: 0x000EC9F0
		private static ICollection PipelineInitialize(int pipelineType, ICollection members, IDictionary cache)
		{
			if (cache != null)
			{
				bool flag = true;
				ICollection collection = cache[TypeDescriptor._pipelineInitializeKeys[pipelineType]] as ICollection;
				if (collection != null && collection.Count == members.Count)
				{
					IEnumerator enumerator = collection.GetEnumerator();
					IEnumerator enumerator2 = members.GetEnumerator();
					while (enumerator.MoveNext() && enumerator2.MoveNext())
					{
						if (enumerator.Current != enumerator2.Current)
						{
							flag = false;
							break;
						}
					}
				}
				if (!flag)
				{
					cache.Remove(TypeDescriptor._pipelineMergeKeys[pipelineType]);
					cache.Remove(TypeDescriptor._pipelineFilterKeys[pipelineType]);
					cache.Remove(TypeDescriptor._pipelineAttributeFilterKeys[pipelineType]);
					cache[TypeDescriptor._pipelineInitializeKeys[pipelineType]] = members;
				}
			}
			return members;
		}

		// Token: 0x060036D1 RID: 14033 RVA: 0x000EE8C4 File Offset: 0x000ECAC4
		private static ICollection PipelineMerge(int pipelineType, ICollection primary, ICollection secondary, object instance, IDictionary cache)
		{
			if (secondary == null || secondary.Count == 0)
			{
				return primary;
			}
			if (cache != null)
			{
				ICollection collection = cache[TypeDescriptor._pipelineMergeKeys[pipelineType]] as ICollection;
				if (collection != null && collection.Count == primary.Count + secondary.Count)
				{
					IEnumerator enumerator = collection.GetEnumerator();
					IEnumerator enumerator2 = primary.GetEnumerator();
					bool flag = true;
					while (enumerator2.MoveNext() && enumerator.MoveNext())
					{
						if (enumerator2.Current != enumerator.Current)
						{
							flag = false;
							break;
						}
					}
					if (flag)
					{
						IEnumerator enumerator3 = secondary.GetEnumerator();
						while (enumerator3.MoveNext() && enumerator.MoveNext())
						{
							if (enumerator3.Current != enumerator.Current)
							{
								flag = false;
								break;
							}
						}
					}
					if (flag)
					{
						return collection;
					}
				}
			}
			ArrayList arrayList = new ArrayList(primary.Count + secondary.Count);
			foreach (object obj in primary)
			{
				arrayList.Add(obj);
			}
			foreach (object obj2 in secondary)
			{
				arrayList.Add(obj2);
			}
			if (cache != null)
			{
				ICollection collection2;
				switch (pipelineType)
				{
				case 0:
				{
					Attribute[] array = new Attribute[arrayList.Count];
					arrayList.CopyTo(array, 0);
					collection2 = new AttributeCollection(array);
					break;
				}
				case 1:
				{
					PropertyDescriptor[] array2 = new PropertyDescriptor[arrayList.Count];
					arrayList.CopyTo(array2, 0);
					collection2 = new PropertyDescriptorCollection(array2, true);
					break;
				}
				case 2:
				{
					EventDescriptor[] array3 = new EventDescriptor[arrayList.Count];
					arrayList.CopyTo(array3, 0);
					collection2 = new EventDescriptorCollection(array3, true);
					break;
				}
				default:
					collection2 = null;
					break;
				}
				cache[TypeDescriptor._pipelineMergeKeys[pipelineType]] = collection2;
				cache.Remove(TypeDescriptor._pipelineFilterKeys[pipelineType]);
				cache.Remove(TypeDescriptor._pipelineAttributeFilterKeys[pipelineType]);
			}
			return arrayList;
		}

		// Token: 0x060036D2 RID: 14034 RVA: 0x000EEB00 File Offset: 0x000ECD00
		private static void RaiseRefresh(object component)
		{
			RefreshEventHandler refreshEventHandler = Volatile.Read<RefreshEventHandler>(ref TypeDescriptor.Refreshed);
			if (refreshEventHandler != null)
			{
				refreshEventHandler(new RefreshEventArgs(component));
			}
		}

		// Token: 0x060036D3 RID: 14035 RVA: 0x000EEB28 File Offset: 0x000ECD28
		private static void RaiseRefresh(Type type)
		{
			RefreshEventHandler refreshEventHandler = Volatile.Read<RefreshEventHandler>(ref TypeDescriptor.Refreshed);
			if (refreshEventHandler != null)
			{
				refreshEventHandler(new RefreshEventArgs(type));
			}
		}

		// Token: 0x060036D4 RID: 14036 RVA: 0x000EEB4F File Offset: 0x000ECD4F
		public static void Refresh(object component)
		{
			TypeDescriptor.Refresh(component, true);
		}

		// Token: 0x060036D5 RID: 14037 RVA: 0x000EEB58 File Offset: 0x000ECD58
		private static void Refresh(object component, bool refreshReflectionProvider)
		{
			if (component == null)
			{
				return;
			}
			bool flag = false;
			if (refreshReflectionProvider)
			{
				Type type = component.GetType();
				object obj = (LocalAppContextSwitches.DoNotUseTypeDescriptorThreadingFix ? TypeDescriptor._providerTable : TypeDescriptor._commonSyncObject);
				lock (obj)
				{
					foreach (object obj2 in TypeDescriptor._providerTable)
					{
						DictionaryEntry dictionaryEntry = (DictionaryEntry)obj2;
						Type type2 = dictionaryEntry.Key as Type;
						if ((type2 != null && type.IsAssignableFrom(type2)) || type2 == typeof(object))
						{
							TypeDescriptor.TypeDescriptionNode typeDescriptionNode = (TypeDescriptor.TypeDescriptionNode)dictionaryEntry.Value;
							while (typeDescriptionNode != null && !(typeDescriptionNode.Provider is ReflectTypeDescriptionProvider))
							{
								flag = true;
								typeDescriptionNode = typeDescriptionNode.Next;
							}
							if (typeDescriptionNode != null)
							{
								ReflectTypeDescriptionProvider reflectTypeDescriptionProvider = (ReflectTypeDescriptionProvider)typeDescriptionNode.Provider;
								if (reflectTypeDescriptionProvider.IsPopulated(type))
								{
									flag = true;
									reflectTypeDescriptionProvider.Refresh(type);
								}
							}
						}
					}
				}
			}
			IDictionary cache = TypeDescriptor.GetCache(component);
			if (flag || cache != null)
			{
				if (cache != null)
				{
					for (int i = 0; i < TypeDescriptor._pipelineFilterKeys.Length; i++)
					{
						cache.Remove(TypeDescriptor._pipelineFilterKeys[i]);
						cache.Remove(TypeDescriptor._pipelineMergeKeys[i]);
						cache.Remove(TypeDescriptor._pipelineAttributeFilterKeys[i]);
					}
				}
				Interlocked.Increment(ref TypeDescriptor._metadataVersion);
				TypeDescriptor.RaiseRefresh(component);
			}
		}

		// Token: 0x060036D6 RID: 14038 RVA: 0x000EED04 File Offset: 0x000ECF04
		public static void Refresh(Type type)
		{
			if (type == null)
			{
				return;
			}
			bool flag = false;
			object obj = (LocalAppContextSwitches.DoNotUseTypeDescriptorThreadingFix ? TypeDescriptor._providerTable : TypeDescriptor._commonSyncObject);
			lock (obj)
			{
				foreach (object obj2 in TypeDescriptor._providerTable)
				{
					DictionaryEntry dictionaryEntry = (DictionaryEntry)obj2;
					Type type2 = dictionaryEntry.Key as Type;
					if ((type2 != null && type.IsAssignableFrom(type2)) || type2 == typeof(object))
					{
						TypeDescriptor.TypeDescriptionNode typeDescriptionNode = (TypeDescriptor.TypeDescriptionNode)dictionaryEntry.Value;
						while (typeDescriptionNode != null && !(typeDescriptionNode.Provider is ReflectTypeDescriptionProvider))
						{
							flag = true;
							typeDescriptionNode = typeDescriptionNode.Next;
						}
						if (typeDescriptionNode != null)
						{
							ReflectTypeDescriptionProvider reflectTypeDescriptionProvider = (ReflectTypeDescriptionProvider)typeDescriptionNode.Provider;
							if (reflectTypeDescriptionProvider.IsPopulated(type))
							{
								flag = true;
								reflectTypeDescriptionProvider.Refresh(type);
							}
						}
					}
				}
			}
			if (flag)
			{
				Interlocked.Increment(ref TypeDescriptor._metadataVersion);
				TypeDescriptor.RaiseRefresh(type);
			}
		}

		// Token: 0x060036D7 RID: 14039 RVA: 0x000EEE3C File Offset: 0x000ED03C
		public static void Refresh(Module module)
		{
			if (module == null)
			{
				return;
			}
			Hashtable hashtable = null;
			object obj = (LocalAppContextSwitches.DoNotUseTypeDescriptorThreadingFix ? TypeDescriptor._providerTable : TypeDescriptor._commonSyncObject);
			lock (obj)
			{
				foreach (object obj2 in TypeDescriptor._providerTable)
				{
					DictionaryEntry dictionaryEntry = (DictionaryEntry)obj2;
					Type type = dictionaryEntry.Key as Type;
					if ((type != null && type.Module.Equals(module)) || type == typeof(object))
					{
						TypeDescriptor.TypeDescriptionNode typeDescriptionNode = (TypeDescriptor.TypeDescriptionNode)dictionaryEntry.Value;
						while (typeDescriptionNode != null && !(typeDescriptionNode.Provider is ReflectTypeDescriptionProvider))
						{
							if (hashtable == null)
							{
								hashtable = new Hashtable();
							}
							hashtable[type] = type;
							typeDescriptionNode = typeDescriptionNode.Next;
						}
						if (typeDescriptionNode != null)
						{
							ReflectTypeDescriptionProvider reflectTypeDescriptionProvider = (ReflectTypeDescriptionProvider)typeDescriptionNode.Provider;
							Type[] populatedTypes = reflectTypeDescriptionProvider.GetPopulatedTypes(module);
							foreach (Type type2 in populatedTypes)
							{
								reflectTypeDescriptionProvider.Refresh(type2);
								if (hashtable == null)
								{
									hashtable = new Hashtable();
								}
								hashtable[type2] = type2;
							}
						}
					}
				}
			}
			if (hashtable != null && TypeDescriptor.Refreshed != null)
			{
				foreach (object obj3 in hashtable.Keys)
				{
					Type type3 = (Type)obj3;
					TypeDescriptor.RaiseRefresh(type3);
				}
			}
		}

		// Token: 0x060036D8 RID: 14040 RVA: 0x000EF028 File Offset: 0x000ED228
		public static void Refresh(Assembly assembly)
		{
			if (assembly == null)
			{
				return;
			}
			foreach (Module module in assembly.GetModules())
			{
				TypeDescriptor.Refresh(module);
			}
		}

		// Token: 0x060036D9 RID: 14041 RVA: 0x000EF060 File Offset: 0x000ED260
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
		public static void RemoveAssociation(object primary, object secondary)
		{
			if (primary == null)
			{
				throw new ArgumentNullException("primary");
			}
			if (secondary == null)
			{
				throw new ArgumentNullException("secondary");
			}
			Hashtable associationTable = TypeDescriptor._associationTable;
			if (associationTable != null)
			{
				IList list = (IList)associationTable[primary];
				if (list != null)
				{
					IList list2 = list;
					lock (list2)
					{
						for (int i = list.Count - 1; i >= 0; i--)
						{
							WeakReference weakReference = (WeakReference)list[i];
							object target = weakReference.Target;
							if (target == null || target == secondary)
							{
								list.RemoveAt(i);
							}
						}
					}
				}
			}
		}

		// Token: 0x060036DA RID: 14042 RVA: 0x000EF10C File Offset: 0x000ED30C
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
		public static void RemoveAssociations(object primary)
		{
			if (primary == null)
			{
				throw new ArgumentNullException("primary");
			}
			Hashtable associationTable = TypeDescriptor._associationTable;
			if (associationTable != null)
			{
				associationTable.Remove(primary);
			}
		}

		// Token: 0x060036DB RID: 14043 RVA: 0x000EF139 File Offset: 0x000ED339
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
		public static void RemoveProvider(TypeDescriptionProvider provider, Type type)
		{
			if (provider == null)
			{
				throw new ArgumentNullException("provider");
			}
			if (type == null)
			{
				throw new ArgumentNullException("type");
			}
			TypeDescriptor.NodeRemove(type, provider);
			TypeDescriptor.RaiseRefresh(type);
		}

		// Token: 0x060036DC RID: 14044 RVA: 0x000EF16A File Offset: 0x000ED36A
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
		public static void RemoveProvider(TypeDescriptionProvider provider, object instance)
		{
			if (provider == null)
			{
				throw new ArgumentNullException("provider");
			}
			if (instance == null)
			{
				throw new ArgumentNullException("instance");
			}
			TypeDescriptor.NodeRemove(instance, provider);
			TypeDescriptor.RaiseRefresh(instance);
		}

		// Token: 0x060036DD RID: 14045 RVA: 0x000EF198 File Offset: 0x000ED398
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public static void RemoveProviderTransparent(TypeDescriptionProvider provider, Type type)
		{
			if (provider == null)
			{
				throw new ArgumentNullException("provider");
			}
			if (type == null)
			{
				throw new ArgumentNullException("type");
			}
			PermissionSet permissionSet = new PermissionSet(PermissionState.None);
			permissionSet.AddPermission(new TypeDescriptorPermission(TypeDescriptorPermissionFlags.RestrictedRegistrationAccess));
			PermissionSet permissionSet2 = type.Assembly.PermissionSet;
			permissionSet2 = permissionSet2.Union(permissionSet);
			permissionSet2.Demand();
			TypeDescriptor.RemoveProvider(provider, type);
		}

		// Token: 0x060036DE RID: 14046 RVA: 0x000EF1FC File Offset: 0x000ED3FC
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public static void RemoveProviderTransparent(TypeDescriptionProvider provider, object instance)
		{
			if (provider == null)
			{
				throw new ArgumentNullException("provider");
			}
			if (instance == null)
			{
				throw new ArgumentNullException("instance");
			}
			Type type = instance.GetType();
			PermissionSet permissionSet = new PermissionSet(PermissionState.None);
			permissionSet.AddPermission(new TypeDescriptorPermission(TypeDescriptorPermissionFlags.RestrictedRegistrationAccess));
			PermissionSet permissionSet2 = type.Assembly.PermissionSet;
			permissionSet2 = permissionSet2.Union(permissionSet);
			permissionSet2.Demand();
			TypeDescriptor.RemoveProvider(provider, instance);
		}

		// Token: 0x060036DF RID: 14047 RVA: 0x000EF264 File Offset: 0x000ED464
		private static bool ShouldHideMember(MemberDescriptor member, Attribute attribute)
		{
			if (member == null || attribute == null)
			{
				return true;
			}
			Attribute attribute2 = member.Attributes[attribute.GetType()];
			if (attribute2 == null)
			{
				return !attribute.IsDefaultAttribute();
			}
			return !attribute.Match(attribute2);
		}

		// Token: 0x060036E0 RID: 14048 RVA: 0x000EF2A2 File Offset: 0x000ED4A2
		public static void SortDescriptorArray(IList infos)
		{
			if (infos == null)
			{
				throw new ArgumentNullException("infos");
			}
			ArrayList.Adapter(infos).Sort(TypeDescriptor.MemberDescriptorComparer.Instance);
		}

		// Token: 0x060036E1 RID: 14049 RVA: 0x000EF2C2 File Offset: 0x000ED4C2
		[Conditional("DEBUG")]
		internal static void Trace(string message, params object[] args)
		{
		}

		// Token: 0x04002AA0 RID: 10912
		private static WeakHashtable _providerTable = new WeakHashtable();

		// Token: 0x04002AA1 RID: 10913
		private static Hashtable _providerTypeTable = new Hashtable();

		// Token: 0x04002AA2 RID: 10914
		private static readonly Hashtable _defaultProviderInitialized = new Hashtable();

		// Token: 0x04002AA3 RID: 10915
		private static readonly object _initializedDefaultProvider = new object();

		// Token: 0x04002AA4 RID: 10916
		internal static readonly object _commonSyncObject = new object();

		// Token: 0x04002AA5 RID: 10917
		private static volatile WeakHashtable _associationTable;

		// Token: 0x04002AA6 RID: 10918
		private static int _metadataVersion;

		// Token: 0x04002AA7 RID: 10919
		private static int _collisionIndex;

		// Token: 0x04002AA8 RID: 10920
		private static BooleanSwitch TraceDescriptor = new BooleanSwitch("TypeDescriptor", "Debug TypeDescriptor.");

		// Token: 0x04002AA9 RID: 10921
		private const int PIPELINE_ATTRIBUTES = 0;

		// Token: 0x04002AAA RID: 10922
		private const int PIPELINE_PROPERTIES = 1;

		// Token: 0x04002AAB RID: 10923
		private const int PIPELINE_EVENTS = 2;

		// Token: 0x04002AAC RID: 10924
		private static readonly Guid[] _pipelineInitializeKeys = new Guid[]
		{
			Guid.NewGuid(),
			Guid.NewGuid(),
			Guid.NewGuid()
		};

		// Token: 0x04002AAD RID: 10925
		private static readonly Guid[] _pipelineMergeKeys = new Guid[]
		{
			Guid.NewGuid(),
			Guid.NewGuid(),
			Guid.NewGuid()
		};

		// Token: 0x04002AAE RID: 10926
		private static readonly Guid[] _pipelineFilterKeys = new Guid[]
		{
			Guid.NewGuid(),
			Guid.NewGuid(),
			Guid.NewGuid()
		};

		// Token: 0x04002AAF RID: 10927
		private static readonly Guid[] _pipelineAttributeFilterKeys = new Guid[]
		{
			Guid.NewGuid(),
			Guid.NewGuid(),
			Guid.NewGuid()
		};

		// Token: 0x04002AB0 RID: 10928
		private static object _internalSyncObject = new object();

		// Token: 0x020008A1 RID: 2209
		private sealed class AttributeProvider : TypeDescriptionProvider
		{
			// Token: 0x060045CF RID: 17871 RVA: 0x00123E99 File Offset: 0x00122099
			internal AttributeProvider(TypeDescriptionProvider existingProvider, params Attribute[] attrs)
				: base(existingProvider)
			{
				this._attrs = attrs;
			}

			// Token: 0x060045D0 RID: 17872 RVA: 0x00123EA9 File Offset: 0x001220A9
			public override ICustomTypeDescriptor GetTypeDescriptor(Type objectType, object instance)
			{
				return new TypeDescriptor.AttributeProvider.AttributeTypeDescriptor(this._attrs, base.GetTypeDescriptor(objectType, instance));
			}

			// Token: 0x040037EA RID: 14314
			private Attribute[] _attrs;

			// Token: 0x02000934 RID: 2356
			private class AttributeTypeDescriptor : CustomTypeDescriptor
			{
				// Token: 0x060046A7 RID: 18087 RVA: 0x00126C11 File Offset: 0x00124E11
				internal AttributeTypeDescriptor(Attribute[] attrs, ICustomTypeDescriptor parent)
					: base(parent)
				{
					this._attributeArray = attrs;
				}

				// Token: 0x060046A8 RID: 18088 RVA: 0x00126C24 File Offset: 0x00124E24
				public override AttributeCollection GetAttributes()
				{
					AttributeCollection attributes = base.GetAttributes();
					Attribute[] attributeArray = this._attributeArray;
					Attribute[] array = new Attribute[attributes.Count + attributeArray.Length];
					int count = attributes.Count;
					attributes.CopyTo(array, 0);
					for (int i = 0; i < attributeArray.Length; i++)
					{
						bool flag = false;
						for (int j = 0; j < attributes.Count; j++)
						{
							if (array[j].TypeId.Equals(attributeArray[i].TypeId))
							{
								flag = true;
								array[j] = attributeArray[i];
								break;
							}
						}
						if (!flag)
						{
							array[count++] = attributeArray[i];
						}
					}
					Attribute[] array2;
					if (count < array.Length)
					{
						array2 = new Attribute[count];
						Array.Copy(array, 0, array2, 0, count);
					}
					else
					{
						array2 = array;
					}
					return new AttributeCollection(array2);
				}

				// Token: 0x04003DDA RID: 15834
				private Attribute[] _attributeArray;
			}
		}

		// Token: 0x020008A2 RID: 2210
		private sealed class ComNativeDescriptionProvider : TypeDescriptionProvider
		{
			// Token: 0x060045D1 RID: 17873 RVA: 0x00123EBE File Offset: 0x001220BE
			internal ComNativeDescriptionProvider(IComNativeDescriptorHandler handler)
			{
				this._handler = handler;
			}

			// Token: 0x17000FCD RID: 4045
			// (get) Token: 0x060045D2 RID: 17874 RVA: 0x00123ECD File Offset: 0x001220CD
			// (set) Token: 0x060045D3 RID: 17875 RVA: 0x00123ED5 File Offset: 0x001220D5
			internal IComNativeDescriptorHandler Handler
			{
				get
				{
					return this._handler;
				}
				set
				{
					this._handler = value;
				}
			}

			// Token: 0x060045D4 RID: 17876 RVA: 0x00123EDE File Offset: 0x001220DE
			public override ICustomTypeDescriptor GetTypeDescriptor(Type objectType, object instance)
			{
				if (objectType == null)
				{
					throw new ArgumentNullException("objectType");
				}
				if (instance == null)
				{
					return null;
				}
				if (!objectType.IsInstanceOfType(instance))
				{
					throw new ArgumentException("instance");
				}
				return new TypeDescriptor.ComNativeDescriptionProvider.ComNativeTypeDescriptor(this._handler, instance);
			}

			// Token: 0x040037EB RID: 14315
			private IComNativeDescriptorHandler _handler;

			// Token: 0x02000935 RID: 2357
			private sealed class ComNativeTypeDescriptor : ICustomTypeDescriptor
			{
				// Token: 0x060046A9 RID: 18089 RVA: 0x00126CE6 File Offset: 0x00124EE6
				internal ComNativeTypeDescriptor(IComNativeDescriptorHandler handler, object instance)
				{
					this._handler = handler;
					this._instance = instance;
				}

				// Token: 0x060046AA RID: 18090 RVA: 0x00126CFC File Offset: 0x00124EFC
				AttributeCollection ICustomTypeDescriptor.GetAttributes()
				{
					return this._handler.GetAttributes(this._instance);
				}

				// Token: 0x060046AB RID: 18091 RVA: 0x00126D0F File Offset: 0x00124F0F
				string ICustomTypeDescriptor.GetClassName()
				{
					return this._handler.GetClassName(this._instance);
				}

				// Token: 0x060046AC RID: 18092 RVA: 0x00126D22 File Offset: 0x00124F22
				string ICustomTypeDescriptor.GetComponentName()
				{
					return null;
				}

				// Token: 0x060046AD RID: 18093 RVA: 0x00126D25 File Offset: 0x00124F25
				TypeConverter ICustomTypeDescriptor.GetConverter()
				{
					return this._handler.GetConverter(this._instance);
				}

				// Token: 0x060046AE RID: 18094 RVA: 0x00126D38 File Offset: 0x00124F38
				EventDescriptor ICustomTypeDescriptor.GetDefaultEvent()
				{
					return this._handler.GetDefaultEvent(this._instance);
				}

				// Token: 0x060046AF RID: 18095 RVA: 0x00126D4B File Offset: 0x00124F4B
				PropertyDescriptor ICustomTypeDescriptor.GetDefaultProperty()
				{
					return this._handler.GetDefaultProperty(this._instance);
				}

				// Token: 0x060046B0 RID: 18096 RVA: 0x00126D5E File Offset: 0x00124F5E
				object ICustomTypeDescriptor.GetEditor(Type editorBaseType)
				{
					return this._handler.GetEditor(this._instance, editorBaseType);
				}

				// Token: 0x060046B1 RID: 18097 RVA: 0x00126D72 File Offset: 0x00124F72
				EventDescriptorCollection ICustomTypeDescriptor.GetEvents()
				{
					return this._handler.GetEvents(this._instance);
				}

				// Token: 0x060046B2 RID: 18098 RVA: 0x00126D85 File Offset: 0x00124F85
				EventDescriptorCollection ICustomTypeDescriptor.GetEvents(Attribute[] attributes)
				{
					return this._handler.GetEvents(this._instance, attributes);
				}

				// Token: 0x060046B3 RID: 18099 RVA: 0x00126D99 File Offset: 0x00124F99
				PropertyDescriptorCollection ICustomTypeDescriptor.GetProperties()
				{
					return this._handler.GetProperties(this._instance, null);
				}

				// Token: 0x060046B4 RID: 18100 RVA: 0x00126DAD File Offset: 0x00124FAD
				PropertyDescriptorCollection ICustomTypeDescriptor.GetProperties(Attribute[] attributes)
				{
					return this._handler.GetProperties(this._instance, attributes);
				}

				// Token: 0x060046B5 RID: 18101 RVA: 0x00126DC1 File Offset: 0x00124FC1
				object ICustomTypeDescriptor.GetPropertyOwner(PropertyDescriptor pd)
				{
					return this._instance;
				}

				// Token: 0x04003DDB RID: 15835
				private IComNativeDescriptorHandler _handler;

				// Token: 0x04003DDC RID: 15836
				private object _instance;
			}
		}

		// Token: 0x020008A3 RID: 2211
		private sealed class AttributeFilterCacheItem
		{
			// Token: 0x060045D5 RID: 17877 RVA: 0x00123F19 File Offset: 0x00122119
			internal AttributeFilterCacheItem(Attribute[] filter, ICollection filteredMembers)
			{
				this._filter = filter;
				this.FilteredMembers = filteredMembers;
			}

			// Token: 0x060045D6 RID: 17878 RVA: 0x00123F30 File Offset: 0x00122130
			internal bool IsValid(Attribute[] filter)
			{
				if (this._filter.Length != filter.Length)
				{
					return false;
				}
				for (int i = 0; i < filter.Length; i++)
				{
					if (this._filter[i] != filter[i])
					{
						return false;
					}
				}
				return true;
			}

			// Token: 0x040037EC RID: 14316
			private Attribute[] _filter;

			// Token: 0x040037ED RID: 14317
			internal ICollection FilteredMembers;
		}

		// Token: 0x020008A4 RID: 2212
		private sealed class FilterCacheItem
		{
			// Token: 0x060045D7 RID: 17879 RVA: 0x00123F6A File Offset: 0x0012216A
			internal FilterCacheItem(ITypeDescriptorFilterService filterService, ICollection filteredMembers)
			{
				this._filterService = filterService;
				this.FilteredMembers = filteredMembers;
			}

			// Token: 0x060045D8 RID: 17880 RVA: 0x00123F80 File Offset: 0x00122180
			internal bool IsValid(ITypeDescriptorFilterService filterService)
			{
				return this._filterService == filterService;
			}

			// Token: 0x040037EE RID: 14318
			private ITypeDescriptorFilterService _filterService;

			// Token: 0x040037EF RID: 14319
			internal ICollection FilteredMembers;
		}

		// Token: 0x020008A5 RID: 2213
		private interface IUnimplemented
		{
		}

		// Token: 0x020008A6 RID: 2214
		private sealed class MemberDescriptorComparer : IComparer
		{
			// Token: 0x060045D9 RID: 17881 RVA: 0x00123F8E File Offset: 0x0012218E
			public int Compare(object left, object right)
			{
				return string.Compare(((MemberDescriptor)left).Name, ((MemberDescriptor)right).Name, false, CultureInfo.InvariantCulture);
			}

			// Token: 0x040037F0 RID: 14320
			public static readonly TypeDescriptor.MemberDescriptorComparer Instance = new TypeDescriptor.MemberDescriptorComparer();
		}

		// Token: 0x020008A7 RID: 2215
		private sealed class MergedTypeDescriptor : ICustomTypeDescriptor
		{
			// Token: 0x060045DC RID: 17884 RVA: 0x00123FC5 File Offset: 0x001221C5
			internal MergedTypeDescriptor(ICustomTypeDescriptor primary, ICustomTypeDescriptor secondary)
			{
				this._primary = primary;
				this._secondary = secondary;
			}

			// Token: 0x060045DD RID: 17885 RVA: 0x00123FDC File Offset: 0x001221DC
			AttributeCollection ICustomTypeDescriptor.GetAttributes()
			{
				AttributeCollection attributeCollection = this._primary.GetAttributes();
				if (attributeCollection == null)
				{
					attributeCollection = this._secondary.GetAttributes();
				}
				return attributeCollection;
			}

			// Token: 0x060045DE RID: 17886 RVA: 0x00124008 File Offset: 0x00122208
			string ICustomTypeDescriptor.GetClassName()
			{
				string text = this._primary.GetClassName();
				if (text == null)
				{
					text = this._secondary.GetClassName();
				}
				return text;
			}

			// Token: 0x060045DF RID: 17887 RVA: 0x00124034 File Offset: 0x00122234
			string ICustomTypeDescriptor.GetComponentName()
			{
				string text = this._primary.GetComponentName();
				if (text == null)
				{
					text = this._secondary.GetComponentName();
				}
				return text;
			}

			// Token: 0x060045E0 RID: 17888 RVA: 0x00124060 File Offset: 0x00122260
			TypeConverter ICustomTypeDescriptor.GetConverter()
			{
				TypeConverter typeConverter = this._primary.GetConverter();
				if (typeConverter == null)
				{
					typeConverter = this._secondary.GetConverter();
				}
				return typeConverter;
			}

			// Token: 0x060045E1 RID: 17889 RVA: 0x0012408C File Offset: 0x0012228C
			EventDescriptor ICustomTypeDescriptor.GetDefaultEvent()
			{
				EventDescriptor eventDescriptor = this._primary.GetDefaultEvent();
				if (eventDescriptor == null)
				{
					eventDescriptor = this._secondary.GetDefaultEvent();
				}
				return eventDescriptor;
			}

			// Token: 0x060045E2 RID: 17890 RVA: 0x001240B8 File Offset: 0x001222B8
			PropertyDescriptor ICustomTypeDescriptor.GetDefaultProperty()
			{
				PropertyDescriptor propertyDescriptor = this._primary.GetDefaultProperty();
				if (propertyDescriptor == null)
				{
					propertyDescriptor = this._secondary.GetDefaultProperty();
				}
				return propertyDescriptor;
			}

			// Token: 0x060045E3 RID: 17891 RVA: 0x001240E4 File Offset: 0x001222E4
			object ICustomTypeDescriptor.GetEditor(Type editorBaseType)
			{
				if (editorBaseType == null)
				{
					throw new ArgumentNullException("editorBaseType");
				}
				object obj = this._primary.GetEditor(editorBaseType);
				if (obj == null)
				{
					obj = this._secondary.GetEditor(editorBaseType);
				}
				return obj;
			}

			// Token: 0x060045E4 RID: 17892 RVA: 0x00124124 File Offset: 0x00122324
			EventDescriptorCollection ICustomTypeDescriptor.GetEvents()
			{
				EventDescriptorCollection eventDescriptorCollection = this._primary.GetEvents();
				if (eventDescriptorCollection == null)
				{
					eventDescriptorCollection = this._secondary.GetEvents();
				}
				return eventDescriptorCollection;
			}

			// Token: 0x060045E5 RID: 17893 RVA: 0x00124150 File Offset: 0x00122350
			EventDescriptorCollection ICustomTypeDescriptor.GetEvents(Attribute[] attributes)
			{
				EventDescriptorCollection eventDescriptorCollection = this._primary.GetEvents(attributes);
				if (eventDescriptorCollection == null)
				{
					eventDescriptorCollection = this._secondary.GetEvents(attributes);
				}
				return eventDescriptorCollection;
			}

			// Token: 0x060045E6 RID: 17894 RVA: 0x0012417C File Offset: 0x0012237C
			PropertyDescriptorCollection ICustomTypeDescriptor.GetProperties()
			{
				PropertyDescriptorCollection propertyDescriptorCollection = this._primary.GetProperties();
				if (propertyDescriptorCollection == null)
				{
					propertyDescriptorCollection = this._secondary.GetProperties();
				}
				return propertyDescriptorCollection;
			}

			// Token: 0x060045E7 RID: 17895 RVA: 0x001241A8 File Offset: 0x001223A8
			PropertyDescriptorCollection ICustomTypeDescriptor.GetProperties(Attribute[] attributes)
			{
				PropertyDescriptorCollection propertyDescriptorCollection = this._primary.GetProperties(attributes);
				if (propertyDescriptorCollection == null)
				{
					propertyDescriptorCollection = this._secondary.GetProperties(attributes);
				}
				return propertyDescriptorCollection;
			}

			// Token: 0x060045E8 RID: 17896 RVA: 0x001241D4 File Offset: 0x001223D4
			object ICustomTypeDescriptor.GetPropertyOwner(PropertyDescriptor pd)
			{
				object obj = this._primary.GetPropertyOwner(pd);
				if (obj == null)
				{
					obj = this._secondary.GetPropertyOwner(pd);
				}
				return obj;
			}

			// Token: 0x040037F1 RID: 14321
			private ICustomTypeDescriptor _primary;

			// Token: 0x040037F2 RID: 14322
			private ICustomTypeDescriptor _secondary;
		}

		// Token: 0x020008A8 RID: 2216
		private sealed class TypeDescriptionNode : TypeDescriptionProvider
		{
			// Token: 0x060045E9 RID: 17897 RVA: 0x001241FF File Offset: 0x001223FF
			internal TypeDescriptionNode(TypeDescriptionProvider provider)
			{
				this.Provider = provider;
			}

			// Token: 0x060045EA RID: 17898 RVA: 0x00124210 File Offset: 0x00122410
			public override object CreateInstance(IServiceProvider provider, Type objectType, Type[] argTypes, object[] args)
			{
				if (objectType == null)
				{
					throw new ArgumentNullException("objectType");
				}
				if (argTypes != null)
				{
					if (args == null)
					{
						throw new ArgumentNullException("args");
					}
					if (argTypes.Length != args.Length)
					{
						throw new ArgumentException(SR.GetString("TypeDescriptorArgsCountMismatch"));
					}
				}
				return this.Provider.CreateInstance(provider, objectType, argTypes, args);
			}

			// Token: 0x060045EB RID: 17899 RVA: 0x0012426C File Offset: 0x0012246C
			public override IDictionary GetCache(object instance)
			{
				if (instance == null)
				{
					throw new ArgumentNullException("instance");
				}
				return this.Provider.GetCache(instance);
			}

			// Token: 0x060045EC RID: 17900 RVA: 0x00124288 File Offset: 0x00122488
			public override ICustomTypeDescriptor GetExtendedTypeDescriptor(object instance)
			{
				if (instance == null)
				{
					throw new ArgumentNullException("instance");
				}
				return new TypeDescriptor.TypeDescriptionNode.DefaultExtendedTypeDescriptor(this, instance);
			}

			// Token: 0x060045ED RID: 17901 RVA: 0x001242A4 File Offset: 0x001224A4
			protected internal override IExtenderProvider[] GetExtenderProviders(object instance)
			{
				if (instance == null)
				{
					throw new ArgumentNullException("instance");
				}
				return this.Provider.GetExtenderProviders(instance);
			}

			// Token: 0x060045EE RID: 17902 RVA: 0x001242C0 File Offset: 0x001224C0
			public override string GetFullComponentName(object component)
			{
				if (component == null)
				{
					throw new ArgumentNullException("component");
				}
				return this.Provider.GetFullComponentName(component);
			}

			// Token: 0x060045EF RID: 17903 RVA: 0x001242DC File Offset: 0x001224DC
			public override Type GetReflectionType(Type objectType, object instance)
			{
				if (objectType == null)
				{
					throw new ArgumentNullException("objectType");
				}
				return this.Provider.GetReflectionType(objectType, instance);
			}

			// Token: 0x060045F0 RID: 17904 RVA: 0x001242FF File Offset: 0x001224FF
			public override Type GetRuntimeType(Type objectType)
			{
				if (objectType == null)
				{
					throw new ArgumentNullException("objectType");
				}
				return this.Provider.GetRuntimeType(objectType);
			}

			// Token: 0x060045F1 RID: 17905 RVA: 0x00124321 File Offset: 0x00122521
			public override ICustomTypeDescriptor GetTypeDescriptor(Type objectType, object instance)
			{
				if (objectType == null)
				{
					throw new ArgumentNullException("objectType");
				}
				if (instance != null && !objectType.IsInstanceOfType(instance))
				{
					throw new ArgumentException("instance");
				}
				return new TypeDescriptor.TypeDescriptionNode.DefaultTypeDescriptor(this, objectType, instance);
			}

			// Token: 0x060045F2 RID: 17906 RVA: 0x0012435B File Offset: 0x0012255B
			public override bool IsSupportedType(Type type)
			{
				if (type == null)
				{
					throw new ArgumentNullException("type");
				}
				return this.Provider.IsSupportedType(type);
			}

			// Token: 0x040037F3 RID: 14323
			internal TypeDescriptor.TypeDescriptionNode Next;

			// Token: 0x040037F4 RID: 14324
			internal TypeDescriptionProvider Provider;

			// Token: 0x02000936 RID: 2358
			private struct DefaultExtendedTypeDescriptor : ICustomTypeDescriptor
			{
				// Token: 0x060046B6 RID: 18102 RVA: 0x00126DC9 File Offset: 0x00124FC9
				internal DefaultExtendedTypeDescriptor(TypeDescriptor.TypeDescriptionNode node, object instance)
				{
					this._node = node;
					this._instance = instance;
				}

				// Token: 0x060046B7 RID: 18103 RVA: 0x00126DDC File Offset: 0x00124FDC
				AttributeCollection ICustomTypeDescriptor.GetAttributes()
				{
					TypeDescriptionProvider provider = this._node.Provider;
					ReflectTypeDescriptionProvider reflectTypeDescriptionProvider = provider as ReflectTypeDescriptionProvider;
					if (reflectTypeDescriptionProvider != null)
					{
						return reflectTypeDescriptionProvider.GetExtendedAttributes(this._instance);
					}
					ICustomTypeDescriptor extendedTypeDescriptor = provider.GetExtendedTypeDescriptor(this._instance);
					if (extendedTypeDescriptor == null)
					{
						throw new InvalidOperationException(SR.GetString("TypeDescriptorProviderError", new object[]
						{
							this._node.Provider.GetType().FullName,
							"GetExtendedTypeDescriptor"
						}));
					}
					AttributeCollection attributes = extendedTypeDescriptor.GetAttributes();
					if (attributes == null)
					{
						throw new InvalidOperationException(SR.GetString("TypeDescriptorProviderError", new object[]
						{
							this._node.Provider.GetType().FullName,
							"GetAttributes"
						}));
					}
					return attributes;
				}

				// Token: 0x060046B8 RID: 18104 RVA: 0x00126E94 File Offset: 0x00125094
				string ICustomTypeDescriptor.GetClassName()
				{
					TypeDescriptionProvider provider = this._node.Provider;
					ReflectTypeDescriptionProvider reflectTypeDescriptionProvider = provider as ReflectTypeDescriptionProvider;
					if (reflectTypeDescriptionProvider != null)
					{
						return reflectTypeDescriptionProvider.GetExtendedClassName(this._instance);
					}
					ICustomTypeDescriptor extendedTypeDescriptor = provider.GetExtendedTypeDescriptor(this._instance);
					if (extendedTypeDescriptor == null)
					{
						throw new InvalidOperationException(SR.GetString("TypeDescriptorProviderError", new object[]
						{
							this._node.Provider.GetType().FullName,
							"GetExtendedTypeDescriptor"
						}));
					}
					string text = extendedTypeDescriptor.GetClassName();
					if (text == null)
					{
						text = this._instance.GetType().FullName;
					}
					return text;
				}

				// Token: 0x060046B9 RID: 18105 RVA: 0x00126F28 File Offset: 0x00125128
				string ICustomTypeDescriptor.GetComponentName()
				{
					TypeDescriptionProvider provider = this._node.Provider;
					ReflectTypeDescriptionProvider reflectTypeDescriptionProvider = provider as ReflectTypeDescriptionProvider;
					if (reflectTypeDescriptionProvider != null)
					{
						return reflectTypeDescriptionProvider.GetExtendedComponentName(this._instance);
					}
					ICustomTypeDescriptor extendedTypeDescriptor = provider.GetExtendedTypeDescriptor(this._instance);
					if (extendedTypeDescriptor == null)
					{
						throw new InvalidOperationException(SR.GetString("TypeDescriptorProviderError", new object[]
						{
							this._node.Provider.GetType().FullName,
							"GetExtendedTypeDescriptor"
						}));
					}
					return extendedTypeDescriptor.GetComponentName();
				}

				// Token: 0x060046BA RID: 18106 RVA: 0x00126FA4 File Offset: 0x001251A4
				TypeConverter ICustomTypeDescriptor.GetConverter()
				{
					TypeDescriptionProvider provider = this._node.Provider;
					ReflectTypeDescriptionProvider reflectTypeDescriptionProvider = provider as ReflectTypeDescriptionProvider;
					if (reflectTypeDescriptionProvider != null)
					{
						return reflectTypeDescriptionProvider.GetExtendedConverter(this._instance);
					}
					ICustomTypeDescriptor extendedTypeDescriptor = provider.GetExtendedTypeDescriptor(this._instance);
					if (extendedTypeDescriptor == null)
					{
						throw new InvalidOperationException(SR.GetString("TypeDescriptorProviderError", new object[]
						{
							this._node.Provider.GetType().FullName,
							"GetExtendedTypeDescriptor"
						}));
					}
					TypeConverter converter = extendedTypeDescriptor.GetConverter();
					if (converter == null)
					{
						throw new InvalidOperationException(SR.GetString("TypeDescriptorProviderError", new object[]
						{
							this._node.Provider.GetType().FullName,
							"GetConverter"
						}));
					}
					return converter;
				}

				// Token: 0x060046BB RID: 18107 RVA: 0x0012705C File Offset: 0x0012525C
				EventDescriptor ICustomTypeDescriptor.GetDefaultEvent()
				{
					TypeDescriptionProvider provider = this._node.Provider;
					ReflectTypeDescriptionProvider reflectTypeDescriptionProvider = provider as ReflectTypeDescriptionProvider;
					if (reflectTypeDescriptionProvider != null)
					{
						return reflectTypeDescriptionProvider.GetExtendedDefaultEvent(this._instance);
					}
					ICustomTypeDescriptor extendedTypeDescriptor = provider.GetExtendedTypeDescriptor(this._instance);
					if (extendedTypeDescriptor == null)
					{
						throw new InvalidOperationException(SR.GetString("TypeDescriptorProviderError", new object[]
						{
							this._node.Provider.GetType().FullName,
							"GetExtendedTypeDescriptor"
						}));
					}
					return extendedTypeDescriptor.GetDefaultEvent();
				}

				// Token: 0x060046BC RID: 18108 RVA: 0x001270D8 File Offset: 0x001252D8
				PropertyDescriptor ICustomTypeDescriptor.GetDefaultProperty()
				{
					TypeDescriptionProvider provider = this._node.Provider;
					ReflectTypeDescriptionProvider reflectTypeDescriptionProvider = provider as ReflectTypeDescriptionProvider;
					if (reflectTypeDescriptionProvider != null)
					{
						return reflectTypeDescriptionProvider.GetExtendedDefaultProperty(this._instance);
					}
					ICustomTypeDescriptor extendedTypeDescriptor = provider.GetExtendedTypeDescriptor(this._instance);
					if (extendedTypeDescriptor == null)
					{
						throw new InvalidOperationException(SR.GetString("TypeDescriptorProviderError", new object[]
						{
							this._node.Provider.GetType().FullName,
							"GetExtendedTypeDescriptor"
						}));
					}
					return extendedTypeDescriptor.GetDefaultProperty();
				}

				// Token: 0x060046BD RID: 18109 RVA: 0x00127154 File Offset: 0x00125354
				object ICustomTypeDescriptor.GetEditor(Type editorBaseType)
				{
					if (editorBaseType == null)
					{
						throw new ArgumentNullException("editorBaseType");
					}
					TypeDescriptionProvider provider = this._node.Provider;
					ReflectTypeDescriptionProvider reflectTypeDescriptionProvider = provider as ReflectTypeDescriptionProvider;
					if (reflectTypeDescriptionProvider != null)
					{
						return reflectTypeDescriptionProvider.GetExtendedEditor(this._instance, editorBaseType);
					}
					ICustomTypeDescriptor extendedTypeDescriptor = provider.GetExtendedTypeDescriptor(this._instance);
					if (extendedTypeDescriptor == null)
					{
						throw new InvalidOperationException(SR.GetString("TypeDescriptorProviderError", new object[]
						{
							this._node.Provider.GetType().FullName,
							"GetExtendedTypeDescriptor"
						}));
					}
					return extendedTypeDescriptor.GetEditor(editorBaseType);
				}

				// Token: 0x060046BE RID: 18110 RVA: 0x001271E8 File Offset: 0x001253E8
				EventDescriptorCollection ICustomTypeDescriptor.GetEvents()
				{
					TypeDescriptionProvider provider = this._node.Provider;
					ReflectTypeDescriptionProvider reflectTypeDescriptionProvider = provider as ReflectTypeDescriptionProvider;
					if (reflectTypeDescriptionProvider != null)
					{
						return reflectTypeDescriptionProvider.GetExtendedEvents(this._instance);
					}
					ICustomTypeDescriptor extendedTypeDescriptor = provider.GetExtendedTypeDescriptor(this._instance);
					if (extendedTypeDescriptor == null)
					{
						throw new InvalidOperationException(SR.GetString("TypeDescriptorProviderError", new object[]
						{
							this._node.Provider.GetType().FullName,
							"GetExtendedTypeDescriptor"
						}));
					}
					EventDescriptorCollection events = extendedTypeDescriptor.GetEvents();
					if (events == null)
					{
						throw new InvalidOperationException(SR.GetString("TypeDescriptorProviderError", new object[]
						{
							this._node.Provider.GetType().FullName,
							"GetEvents"
						}));
					}
					return events;
				}

				// Token: 0x060046BF RID: 18111 RVA: 0x001272A0 File Offset: 0x001254A0
				EventDescriptorCollection ICustomTypeDescriptor.GetEvents(Attribute[] attributes)
				{
					TypeDescriptionProvider provider = this._node.Provider;
					ReflectTypeDescriptionProvider reflectTypeDescriptionProvider = provider as ReflectTypeDescriptionProvider;
					if (reflectTypeDescriptionProvider != null)
					{
						return reflectTypeDescriptionProvider.GetExtendedEvents(this._instance);
					}
					ICustomTypeDescriptor extendedTypeDescriptor = provider.GetExtendedTypeDescriptor(this._instance);
					if (extendedTypeDescriptor == null)
					{
						throw new InvalidOperationException(SR.GetString("TypeDescriptorProviderError", new object[]
						{
							this._node.Provider.GetType().FullName,
							"GetExtendedTypeDescriptor"
						}));
					}
					EventDescriptorCollection events = extendedTypeDescriptor.GetEvents(attributes);
					if (events == null)
					{
						throw new InvalidOperationException(SR.GetString("TypeDescriptorProviderError", new object[]
						{
							this._node.Provider.GetType().FullName,
							"GetEvents"
						}));
					}
					return events;
				}

				// Token: 0x060046C0 RID: 18112 RVA: 0x0012735C File Offset: 0x0012555C
				PropertyDescriptorCollection ICustomTypeDescriptor.GetProperties()
				{
					TypeDescriptionProvider provider = this._node.Provider;
					ReflectTypeDescriptionProvider reflectTypeDescriptionProvider = provider as ReflectTypeDescriptionProvider;
					if (reflectTypeDescriptionProvider != null)
					{
						return reflectTypeDescriptionProvider.GetExtendedProperties(this._instance);
					}
					ICustomTypeDescriptor extendedTypeDescriptor = provider.GetExtendedTypeDescriptor(this._instance);
					if (extendedTypeDescriptor == null)
					{
						throw new InvalidOperationException(SR.GetString("TypeDescriptorProviderError", new object[]
						{
							this._node.Provider.GetType().FullName,
							"GetExtendedTypeDescriptor"
						}));
					}
					PropertyDescriptorCollection properties = extendedTypeDescriptor.GetProperties();
					if (properties == null)
					{
						throw new InvalidOperationException(SR.GetString("TypeDescriptorProviderError", new object[]
						{
							this._node.Provider.GetType().FullName,
							"GetProperties"
						}));
					}
					return properties;
				}

				// Token: 0x060046C1 RID: 18113 RVA: 0x00127414 File Offset: 0x00125614
				PropertyDescriptorCollection ICustomTypeDescriptor.GetProperties(Attribute[] attributes)
				{
					TypeDescriptionProvider provider = this._node.Provider;
					ReflectTypeDescriptionProvider reflectTypeDescriptionProvider = provider as ReflectTypeDescriptionProvider;
					if (reflectTypeDescriptionProvider != null)
					{
						return reflectTypeDescriptionProvider.GetExtendedProperties(this._instance);
					}
					ICustomTypeDescriptor extendedTypeDescriptor = provider.GetExtendedTypeDescriptor(this._instance);
					if (extendedTypeDescriptor == null)
					{
						throw new InvalidOperationException(SR.GetString("TypeDescriptorProviderError", new object[]
						{
							this._node.Provider.GetType().FullName,
							"GetExtendedTypeDescriptor"
						}));
					}
					PropertyDescriptorCollection properties = extendedTypeDescriptor.GetProperties(attributes);
					if (properties == null)
					{
						throw new InvalidOperationException(SR.GetString("TypeDescriptorProviderError", new object[]
						{
							this._node.Provider.GetType().FullName,
							"GetProperties"
						}));
					}
					return properties;
				}

				// Token: 0x060046C2 RID: 18114 RVA: 0x001274D0 File Offset: 0x001256D0
				object ICustomTypeDescriptor.GetPropertyOwner(PropertyDescriptor pd)
				{
					TypeDescriptionProvider provider = this._node.Provider;
					ReflectTypeDescriptionProvider reflectTypeDescriptionProvider = provider as ReflectTypeDescriptionProvider;
					if (reflectTypeDescriptionProvider != null)
					{
						return reflectTypeDescriptionProvider.GetExtendedPropertyOwner(this._instance, pd);
					}
					ICustomTypeDescriptor extendedTypeDescriptor = provider.GetExtendedTypeDescriptor(this._instance);
					if (extendedTypeDescriptor == null)
					{
						throw new InvalidOperationException(SR.GetString("TypeDescriptorProviderError", new object[]
						{
							this._node.Provider.GetType().FullName,
							"GetExtendedTypeDescriptor"
						}));
					}
					object obj = extendedTypeDescriptor.GetPropertyOwner(pd);
					if (obj == null)
					{
						obj = this._instance;
					}
					return obj;
				}

				// Token: 0x04003DDD RID: 15837
				private TypeDescriptor.TypeDescriptionNode _node;

				// Token: 0x04003DDE RID: 15838
				private object _instance;
			}

			// Token: 0x02000937 RID: 2359
			private struct DefaultTypeDescriptor : ICustomTypeDescriptor
			{
				// Token: 0x060046C3 RID: 18115 RVA: 0x0012755A File Offset: 0x0012575A
				internal DefaultTypeDescriptor(TypeDescriptor.TypeDescriptionNode node, Type objectType, object instance)
				{
					this._node = node;
					this._objectType = objectType;
					this._instance = instance;
				}

				// Token: 0x060046C4 RID: 18116 RVA: 0x00127574 File Offset: 0x00125774
				AttributeCollection ICustomTypeDescriptor.GetAttributes()
				{
					TypeDescriptionProvider provider = this._node.Provider;
					ReflectTypeDescriptionProvider reflectTypeDescriptionProvider = provider as ReflectTypeDescriptionProvider;
					AttributeCollection attributeCollection;
					if (reflectTypeDescriptionProvider != null)
					{
						attributeCollection = reflectTypeDescriptionProvider.GetAttributes(this._objectType);
					}
					else
					{
						ICustomTypeDescriptor typeDescriptor = provider.GetTypeDescriptor(this._objectType, this._instance);
						if (typeDescriptor == null)
						{
							throw new InvalidOperationException(SR.GetString("TypeDescriptorProviderError", new object[]
							{
								this._node.Provider.GetType().FullName,
								"GetTypeDescriptor"
							}));
						}
						attributeCollection = typeDescriptor.GetAttributes();
						if (attributeCollection == null)
						{
							throw new InvalidOperationException(SR.GetString("TypeDescriptorProviderError", new object[]
							{
								this._node.Provider.GetType().FullName,
								"GetAttributes"
							}));
						}
					}
					return attributeCollection;
				}

				// Token: 0x060046C5 RID: 18117 RVA: 0x00127638 File Offset: 0x00125838
				string ICustomTypeDescriptor.GetClassName()
				{
					TypeDescriptionProvider provider = this._node.Provider;
					ReflectTypeDescriptionProvider reflectTypeDescriptionProvider = provider as ReflectTypeDescriptionProvider;
					string text;
					if (reflectTypeDescriptionProvider != null)
					{
						text = reflectTypeDescriptionProvider.GetClassName(this._objectType);
					}
					else
					{
						ICustomTypeDescriptor typeDescriptor = provider.GetTypeDescriptor(this._objectType, this._instance);
						if (typeDescriptor == null)
						{
							throw new InvalidOperationException(SR.GetString("TypeDescriptorProviderError", new object[]
							{
								this._node.Provider.GetType().FullName,
								"GetTypeDescriptor"
							}));
						}
						text = typeDescriptor.GetClassName();
						if (text == null)
						{
							text = this._objectType.FullName;
						}
					}
					return text;
				}

				// Token: 0x060046C6 RID: 18118 RVA: 0x001276D0 File Offset: 0x001258D0
				string ICustomTypeDescriptor.GetComponentName()
				{
					TypeDescriptionProvider provider = this._node.Provider;
					ReflectTypeDescriptionProvider reflectTypeDescriptionProvider = provider as ReflectTypeDescriptionProvider;
					string text;
					if (reflectTypeDescriptionProvider != null)
					{
						text = reflectTypeDescriptionProvider.GetComponentName(this._objectType, this._instance);
					}
					else
					{
						ICustomTypeDescriptor typeDescriptor = provider.GetTypeDescriptor(this._objectType, this._instance);
						if (typeDescriptor == null)
						{
							throw new InvalidOperationException(SR.GetString("TypeDescriptorProviderError", new object[]
							{
								this._node.Provider.GetType().FullName,
								"GetTypeDescriptor"
							}));
						}
						text = typeDescriptor.GetComponentName();
					}
					return text;
				}

				// Token: 0x060046C7 RID: 18119 RVA: 0x0012775C File Offset: 0x0012595C
				TypeConverter ICustomTypeDescriptor.GetConverter()
				{
					TypeDescriptionProvider provider = this._node.Provider;
					ReflectTypeDescriptionProvider reflectTypeDescriptionProvider = provider as ReflectTypeDescriptionProvider;
					TypeConverter typeConverter;
					if (reflectTypeDescriptionProvider != null)
					{
						typeConverter = reflectTypeDescriptionProvider.GetConverter(this._objectType, this._instance);
					}
					else
					{
						ICustomTypeDescriptor typeDescriptor = provider.GetTypeDescriptor(this._objectType, this._instance);
						if (typeDescriptor == null)
						{
							throw new InvalidOperationException(SR.GetString("TypeDescriptorProviderError", new object[]
							{
								this._node.Provider.GetType().FullName,
								"GetTypeDescriptor"
							}));
						}
						typeConverter = typeDescriptor.GetConverter();
						if (typeConverter == null)
						{
							throw new InvalidOperationException(SR.GetString("TypeDescriptorProviderError", new object[]
							{
								this._node.Provider.GetType().FullName,
								"GetConverter"
							}));
						}
					}
					return typeConverter;
				}

				// Token: 0x060046C8 RID: 18120 RVA: 0x00127824 File Offset: 0x00125A24
				EventDescriptor ICustomTypeDescriptor.GetDefaultEvent()
				{
					TypeDescriptionProvider provider = this._node.Provider;
					ReflectTypeDescriptionProvider reflectTypeDescriptionProvider = provider as ReflectTypeDescriptionProvider;
					EventDescriptor eventDescriptor;
					if (reflectTypeDescriptionProvider != null)
					{
						eventDescriptor = reflectTypeDescriptionProvider.GetDefaultEvent(this._objectType, this._instance);
					}
					else
					{
						ICustomTypeDescriptor typeDescriptor = provider.GetTypeDescriptor(this._objectType, this._instance);
						if (typeDescriptor == null)
						{
							throw new InvalidOperationException(SR.GetString("TypeDescriptorProviderError", new object[]
							{
								this._node.Provider.GetType().FullName,
								"GetTypeDescriptor"
							}));
						}
						eventDescriptor = typeDescriptor.GetDefaultEvent();
					}
					return eventDescriptor;
				}

				// Token: 0x060046C9 RID: 18121 RVA: 0x001278B0 File Offset: 0x00125AB0
				PropertyDescriptor ICustomTypeDescriptor.GetDefaultProperty()
				{
					TypeDescriptionProvider provider = this._node.Provider;
					ReflectTypeDescriptionProvider reflectTypeDescriptionProvider = provider as ReflectTypeDescriptionProvider;
					PropertyDescriptor propertyDescriptor;
					if (reflectTypeDescriptionProvider != null)
					{
						propertyDescriptor = reflectTypeDescriptionProvider.GetDefaultProperty(this._objectType, this._instance);
					}
					else
					{
						ICustomTypeDescriptor typeDescriptor = provider.GetTypeDescriptor(this._objectType, this._instance);
						if (typeDescriptor == null)
						{
							throw new InvalidOperationException(SR.GetString("TypeDescriptorProviderError", new object[]
							{
								this._node.Provider.GetType().FullName,
								"GetTypeDescriptor"
							}));
						}
						propertyDescriptor = typeDescriptor.GetDefaultProperty();
					}
					return propertyDescriptor;
				}

				// Token: 0x060046CA RID: 18122 RVA: 0x0012793C File Offset: 0x00125B3C
				object ICustomTypeDescriptor.GetEditor(Type editorBaseType)
				{
					if (editorBaseType == null)
					{
						throw new ArgumentNullException("editorBaseType");
					}
					TypeDescriptionProvider provider = this._node.Provider;
					ReflectTypeDescriptionProvider reflectTypeDescriptionProvider = provider as ReflectTypeDescriptionProvider;
					object obj;
					if (reflectTypeDescriptionProvider != null)
					{
						obj = reflectTypeDescriptionProvider.GetEditor(this._objectType, this._instance, editorBaseType);
					}
					else
					{
						ICustomTypeDescriptor typeDescriptor = provider.GetTypeDescriptor(this._objectType, this._instance);
						if (typeDescriptor == null)
						{
							throw new InvalidOperationException(SR.GetString("TypeDescriptorProviderError", new object[]
							{
								this._node.Provider.GetType().FullName,
								"GetTypeDescriptor"
							}));
						}
						obj = typeDescriptor.GetEditor(editorBaseType);
					}
					return obj;
				}

				// Token: 0x060046CB RID: 18123 RVA: 0x001279E0 File Offset: 0x00125BE0
				EventDescriptorCollection ICustomTypeDescriptor.GetEvents()
				{
					TypeDescriptionProvider provider = this._node.Provider;
					ReflectTypeDescriptionProvider reflectTypeDescriptionProvider = provider as ReflectTypeDescriptionProvider;
					EventDescriptorCollection eventDescriptorCollection;
					if (reflectTypeDescriptionProvider != null)
					{
						eventDescriptorCollection = reflectTypeDescriptionProvider.GetEvents(this._objectType);
					}
					else
					{
						ICustomTypeDescriptor typeDescriptor = provider.GetTypeDescriptor(this._objectType, this._instance);
						if (typeDescriptor == null)
						{
							throw new InvalidOperationException(SR.GetString("TypeDescriptorProviderError", new object[]
							{
								this._node.Provider.GetType().FullName,
								"GetTypeDescriptor"
							}));
						}
						eventDescriptorCollection = typeDescriptor.GetEvents();
						if (eventDescriptorCollection == null)
						{
							throw new InvalidOperationException(SR.GetString("TypeDescriptorProviderError", new object[]
							{
								this._node.Provider.GetType().FullName,
								"GetEvents"
							}));
						}
					}
					return eventDescriptorCollection;
				}

				// Token: 0x060046CC RID: 18124 RVA: 0x00127AA4 File Offset: 0x00125CA4
				EventDescriptorCollection ICustomTypeDescriptor.GetEvents(Attribute[] attributes)
				{
					TypeDescriptionProvider provider = this._node.Provider;
					ReflectTypeDescriptionProvider reflectTypeDescriptionProvider = provider as ReflectTypeDescriptionProvider;
					EventDescriptorCollection eventDescriptorCollection;
					if (reflectTypeDescriptionProvider != null)
					{
						eventDescriptorCollection = reflectTypeDescriptionProvider.GetEvents(this._objectType);
					}
					else
					{
						ICustomTypeDescriptor typeDescriptor = provider.GetTypeDescriptor(this._objectType, this._instance);
						if (typeDescriptor == null)
						{
							throw new InvalidOperationException(SR.GetString("TypeDescriptorProviderError", new object[]
							{
								this._node.Provider.GetType().FullName,
								"GetTypeDescriptor"
							}));
						}
						eventDescriptorCollection = typeDescriptor.GetEvents(attributes);
						if (eventDescriptorCollection == null)
						{
							throw new InvalidOperationException(SR.GetString("TypeDescriptorProviderError", new object[]
							{
								this._node.Provider.GetType().FullName,
								"GetEvents"
							}));
						}
					}
					return eventDescriptorCollection;
				}

				// Token: 0x060046CD RID: 18125 RVA: 0x00127B68 File Offset: 0x00125D68
				PropertyDescriptorCollection ICustomTypeDescriptor.GetProperties()
				{
					TypeDescriptionProvider provider = this._node.Provider;
					ReflectTypeDescriptionProvider reflectTypeDescriptionProvider = provider as ReflectTypeDescriptionProvider;
					PropertyDescriptorCollection propertyDescriptorCollection;
					if (reflectTypeDescriptionProvider != null)
					{
						propertyDescriptorCollection = reflectTypeDescriptionProvider.GetProperties(this._objectType);
					}
					else
					{
						ICustomTypeDescriptor typeDescriptor = provider.GetTypeDescriptor(this._objectType, this._instance);
						if (typeDescriptor == null)
						{
							throw new InvalidOperationException(SR.GetString("TypeDescriptorProviderError", new object[]
							{
								this._node.Provider.GetType().FullName,
								"GetTypeDescriptor"
							}));
						}
						propertyDescriptorCollection = typeDescriptor.GetProperties();
						if (propertyDescriptorCollection == null)
						{
							throw new InvalidOperationException(SR.GetString("TypeDescriptorProviderError", new object[]
							{
								this._node.Provider.GetType().FullName,
								"GetProperties"
							}));
						}
					}
					return propertyDescriptorCollection;
				}

				// Token: 0x060046CE RID: 18126 RVA: 0x00127C2C File Offset: 0x00125E2C
				PropertyDescriptorCollection ICustomTypeDescriptor.GetProperties(Attribute[] attributes)
				{
					TypeDescriptionProvider provider = this._node.Provider;
					ReflectTypeDescriptionProvider reflectTypeDescriptionProvider = provider as ReflectTypeDescriptionProvider;
					PropertyDescriptorCollection propertyDescriptorCollection;
					if (reflectTypeDescriptionProvider != null)
					{
						propertyDescriptorCollection = reflectTypeDescriptionProvider.GetProperties(this._objectType);
					}
					else
					{
						ICustomTypeDescriptor typeDescriptor = provider.GetTypeDescriptor(this._objectType, this._instance);
						if (typeDescriptor == null)
						{
							throw new InvalidOperationException(SR.GetString("TypeDescriptorProviderError", new object[]
							{
								this._node.Provider.GetType().FullName,
								"GetTypeDescriptor"
							}));
						}
						propertyDescriptorCollection = typeDescriptor.GetProperties(attributes);
						if (propertyDescriptorCollection == null)
						{
							throw new InvalidOperationException(SR.GetString("TypeDescriptorProviderError", new object[]
							{
								this._node.Provider.GetType().FullName,
								"GetProperties"
							}));
						}
					}
					return propertyDescriptorCollection;
				}

				// Token: 0x060046CF RID: 18127 RVA: 0x00127CF0 File Offset: 0x00125EF0
				object ICustomTypeDescriptor.GetPropertyOwner(PropertyDescriptor pd)
				{
					TypeDescriptionProvider provider = this._node.Provider;
					ReflectTypeDescriptionProvider reflectTypeDescriptionProvider = provider as ReflectTypeDescriptionProvider;
					object obj;
					if (reflectTypeDescriptionProvider != null)
					{
						obj = reflectTypeDescriptionProvider.GetPropertyOwner(this._objectType, this._instance, pd);
					}
					else
					{
						ICustomTypeDescriptor typeDescriptor = provider.GetTypeDescriptor(this._objectType, this._instance);
						if (typeDescriptor == null)
						{
							throw new InvalidOperationException(SR.GetString("TypeDescriptorProviderError", new object[]
							{
								this._node.Provider.GetType().FullName,
								"GetTypeDescriptor"
							}));
						}
						obj = typeDescriptor.GetPropertyOwner(pd);
						if (obj == null)
						{
							obj = this._instance;
						}
					}
					return obj;
				}

				// Token: 0x04003DDF RID: 15839
				private TypeDescriptor.TypeDescriptionNode _node;

				// Token: 0x04003DE0 RID: 15840
				private Type _objectType;

				// Token: 0x04003DE1 RID: 15841
				private object _instance;
			}
		}

		// Token: 0x020008A9 RID: 2217
		[TypeDescriptionProvider("System.Windows.Forms.ComponentModel.Com2Interop.ComNativeDescriptor, System.Windows.Forms, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089")]
		private sealed class TypeDescriptorComObject
		{
		}

		// Token: 0x020008AA RID: 2218
		private sealed class TypeDescriptorInterface
		{
		}
	}
}
