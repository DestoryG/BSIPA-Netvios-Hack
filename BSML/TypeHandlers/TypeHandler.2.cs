using System;
using System.Collections.Generic;
using BeatSaberMarkupLanguage.Components;
using BeatSaberMarkupLanguage.Notify;
using BeatSaberMarkupLanguage.Parser;
using UnityEngine;

namespace BeatSaberMarkupLanguage.TypeHandlers
{
	// Token: 0x02000035 RID: 53
	public abstract class TypeHandler<T> : TypeHandler where T : Component
	{
		// Token: 0x17000058 RID: 88
		// (get) Token: 0x06000123 RID: 291 RVA: 0x00008027 File Offset: 0x00006227
		public Dictionary<string, Action<T, string>> CachedSetters
		{
			get
			{
				if (this.cachedSetters == null)
				{
					this.cachedSetters = this.Setters;
				}
				return this.cachedSetters;
			}
		}

		// Token: 0x17000059 RID: 89
		// (get) Token: 0x06000124 RID: 292
		public abstract Dictionary<string, Action<T, string>> Setters { get; }

		// Token: 0x06000125 RID: 293 RVA: 0x00008044 File Offset: 0x00006244
		public override void HandleType(BSMLParser.ComponentTypeWithData componentType, BSMLParserParams parserParams)
		{
			Component component = componentType.component;
			T obj = component as T;
			if (obj != null)
			{
				NotifyUpdater notifyUpdater = null;
				INotifiableHost notifiableHost = parserParams.host as INotifiableHost;
				if (notifiableHost != null)
				{
					Dictionary<string, BSMLPropertyValue> propertyMap = componentType.propertyMap;
					if (((propertyMap != null) ? propertyMap.Count : 0) > 0)
					{
						notifyUpdater = componentType.component.gameObject.GetComponent<NotifyUpdater>();
						if (notifyUpdater == null)
						{
							notifyUpdater = componentType.component.gameObject.AddComponent<NotifyUpdater>();
							notifyUpdater.NotifyHost = notifiableHost;
						}
					}
				}
				foreach (KeyValuePair<string, string> keyValuePair in componentType.data)
				{
					Action<T, string> action;
					if (this.CachedSetters.TryGetValue(keyValuePair.Key, out action))
					{
						action(obj, keyValuePair.Value);
						BSMLPropertyValue bsmlpropertyValue;
						if (componentType.propertyMap != null && componentType.propertyMap.TryGetValue(keyValuePair.Key, out bsmlpropertyValue) && notifyUpdater != null)
						{
							notifyUpdater.AddAction(bsmlpropertyValue.propertyInfo.Name, delegate(object val)
							{
								action(obj, val.InvariantToString());
							});
						}
					}
				}
			}
		}

		// Token: 0x04000034 RID: 52
		private Dictionary<string, Action<T, string>> cachedSetters;
	}
}
