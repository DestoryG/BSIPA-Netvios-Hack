using System;
using System.Configuration;
using System.Security.Permissions;

namespace System.Diagnostics
{
	// Token: 0x0200049C RID: 1180
	[ConfigurationCollection(typeof(ListenerElement))]
	internal class ListenerElementsCollection : ConfigurationElementCollection
	{
		// Token: 0x17000A90 RID: 2704
		public ListenerElement this[string name]
		{
			get
			{
				return (ListenerElement)base.BaseGet(name);
			}
		}

		// Token: 0x17000A91 RID: 2705
		// (get) Token: 0x06002BC0 RID: 11200 RVA: 0x000C5F10 File Offset: 0x000C4110
		public override ConfigurationElementCollectionType CollectionType
		{
			get
			{
				return ConfigurationElementCollectionType.AddRemoveClearMap;
			}
		}

		// Token: 0x06002BC1 RID: 11201 RVA: 0x000C5F13 File Offset: 0x000C4113
		protected override ConfigurationElement CreateNewElement()
		{
			return new ListenerElement(true);
		}

		// Token: 0x06002BC2 RID: 11202 RVA: 0x000C5F1B File Offset: 0x000C411B
		protected override object GetElementKey(ConfigurationElement element)
		{
			return ((ListenerElement)element).Name;
		}

		// Token: 0x06002BC3 RID: 11203 RVA: 0x000C5F28 File Offset: 0x000C4128
		public TraceListenerCollection GetRuntimeObject()
		{
			TraceListenerCollection traceListenerCollection = new TraceListenerCollection();
			bool flag = false;
			foreach (object obj in this)
			{
				ListenerElement listenerElement = (ListenerElement)obj;
				if (!flag && !listenerElement._isAddedByDefault)
				{
					new SecurityPermission(SecurityPermissionFlag.UnmanagedCode).Demand();
					flag = true;
				}
				traceListenerCollection.Add(listenerElement.GetRuntimeObject());
			}
			return traceListenerCollection;
		}

		// Token: 0x06002BC4 RID: 11204 RVA: 0x000C5FA8 File Offset: 0x000C41A8
		protected override void InitializeDefault()
		{
			this.InitializeDefaultInternal();
		}

		// Token: 0x06002BC5 RID: 11205 RVA: 0x000C5FB0 File Offset: 0x000C41B0
		internal void InitializeDefaultInternal()
		{
			this.BaseAdd(new ListenerElement(false)
			{
				Name = "Default",
				TypeName = typeof(DefaultTraceListener).FullName,
				_isAddedByDefault = true
			});
		}

		// Token: 0x06002BC6 RID: 11206 RVA: 0x000C5FF4 File Offset: 0x000C41F4
		protected override void BaseAdd(ConfigurationElement element)
		{
			ListenerElement listenerElement = element as ListenerElement;
			if (listenerElement.Name.Equals("Default") && listenerElement.TypeName.Equals(typeof(DefaultTraceListener).FullName))
			{
				base.BaseAdd(listenerElement, false);
				return;
			}
			base.BaseAdd(listenerElement, this.ThrowOnDuplicate);
		}
	}
}
