using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml;
using BeatSaberMarkupLanguage.Attributes;
using BeatSaberMarkupLanguage.Components;
using BeatSaberMarkupLanguage.Macros;
using BeatSaberMarkupLanguage.Notify;
using BeatSaberMarkupLanguage.Parser;
using BeatSaberMarkupLanguage.Tags;
using BeatSaberMarkupLanguage.TypeHandlers;
using IPA.Logging;
using UnityEngine;

namespace BeatSaberMarkupLanguage
{
	// Token: 0x02000009 RID: 9
	public class BSMLParser : PersistentSingleton<BSMLParser>
	{
		// Token: 0x17000008 RID: 8
		// (get) Token: 0x06000039 RID: 57 RVA: 0x00002EE3 File Offset: 0x000010E3
		internal static string MACRO_PREFIX
		{
			get
			{
				return "macro.";
			}
		}

		// Token: 0x17000009 RID: 9
		// (get) Token: 0x0600003A RID: 58 RVA: 0x00002EEA File Offset: 0x000010EA
		internal static string RETRIEVE_VALUE_PREFIX
		{
			get
			{
				return "~";
			}
		}

		// Token: 0x1700000A RID: 10
		// (get) Token: 0x0600003B RID: 59 RVA: 0x00002EF1 File Offset: 0x000010F1
		internal static string SUBSCRIVE_EVENT_ACTION_PREFIX
		{
			get
			{
				return "#";
			}
		}

		// Token: 0x0600003C RID: 60 RVA: 0x00002EF8 File Offset: 0x000010F8
		public void Awake()
		{
			this.readerSettings.IgnoreComments = true;
			foreach (BSMLTag bsmltag in Utilities.GetListOfType<BSMLTag>(Array.Empty<object>()))
			{
				this.RegisterTag(bsmltag);
			}
			foreach (BSMLMacro bsmlmacro in Utilities.GetListOfType<BSMLMacro>(Array.Empty<object>()))
			{
				this.RegisterMacro(bsmlmacro);
			}
			this.typeHandlers = Utilities.GetListOfType<TypeHandler>(Array.Empty<object>());
		}

		// Token: 0x0600003D RID: 61 RVA: 0x00002FB4 File Offset: 0x000011B4
		public void MenuSceneLoaded()
		{
			foreach (BSMLTag bsmltag in this.tags.Values)
			{
				if (!bsmltag.isInitialized)
				{
					bsmltag.Setup();
					bsmltag.isInitialized = true;
				}
			}
		}

		// Token: 0x0600003E RID: 62 RVA: 0x0000301C File Offset: 0x0000121C
		public void RegisterTag(BSMLTag tag)
		{
			foreach (string text in tag.Aliases)
			{
				this.tags.Add(text, tag);
			}
		}

		// Token: 0x0600003F RID: 63 RVA: 0x00003050 File Offset: 0x00001250
		public void RegisterMacro(BSMLMacro macro)
		{
			foreach (string text in macro.Aliases)
			{
				this.macros.Add(BSMLParser.MACRO_PREFIX + text, macro);
			}
		}

		// Token: 0x06000040 RID: 64 RVA: 0x0000308D File Offset: 0x0000128D
		public void RegisterTypeHandler(TypeHandler typeHandler)
		{
			this.typeHandlers.Add(typeHandler);
		}

		// Token: 0x06000041 RID: 65 RVA: 0x0000309B File Offset: 0x0000129B
		public BSMLParserParams Parse(string content, GameObject parent, object host = null)
		{
			this.doc.Load(XmlReader.Create(new StringReader(content), this.readerSettings));
			return this.Parse(this.doc, parent, host);
		}

		// Token: 0x06000042 RID: 66 RVA: 0x000030C8 File Offset: 0x000012C8
		public BSMLParserParams Parse(XmlNode parentNode, GameObject parent, object host = null)
		{
			BSMLParserParams bsmlparserParams = new BSMLParserParams();
			bsmlparserParams.host = host;
			if (host != null)
			{
				foreach (MethodInfo methodInfo in host.GetType().GetMethods(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic))
				{
					UIAction uiaction = methodInfo.GetCustomAttributes(typeof(UIAction), true).FirstOrDefault<object>() as UIAction;
					if (uiaction != null)
					{
						bsmlparserParams.actions.Add(uiaction.id, new BSMLAction(host, methodInfo));
					}
				}
				foreach (FieldInfo fieldInfo in host.GetType().GetFields(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic))
				{
					UIValue uivalue = fieldInfo.GetCustomAttributes(typeof(UIValue), true).FirstOrDefault<object>() as UIValue;
					if (uivalue != null)
					{
						bsmlparserParams.values.Add(uivalue.id, new BSMLFieldValue(host, fieldInfo));
					}
					if (fieldInfo.GetCustomAttributes(typeof(UIParams), true).FirstOrDefault<object>() is UIParams)
					{
						fieldInfo.SetValue(host, bsmlparserParams);
					}
				}
				foreach (PropertyInfo propertyInfo in host.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic))
				{
					UIValue uivalue2 = propertyInfo.GetCustomAttributes(typeof(UIValue), true).FirstOrDefault<object>() as UIValue;
					if (uivalue2 != null)
					{
						bsmlparserParams.values.Add(uivalue2.id, new BSMLPropertyValue(host, propertyInfo));
					}
				}
			}
			IEnumerable<BSMLParser.ComponentTypeWithData> enumerable = Enumerable.Empty<BSMLParser.ComponentTypeWithData>();
			foreach (object obj in parentNode.ChildNodes)
			{
				XmlNode xmlNode = (XmlNode)obj;
				IEnumerable<BSMLParser.ComponentTypeWithData> enumerable2;
				this.HandleNode(xmlNode, parent, bsmlparserParams, out enumerable2);
				enumerable = enumerable.Concat(enumerable2);
			}
			using (IEnumerator<KeyValuePair<string, BSMLAction>> enumerator2 = bsmlparserParams.actions.Where((KeyValuePair<string, BSMLAction> x) => x.Key.StartsWith(BSMLParser.SUBSCRIVE_EVENT_ACTION_PREFIX)).GetEnumerator())
			{
				while (enumerator2.MoveNext())
				{
					KeyValuePair<string, BSMLAction> action = enumerator2.Current;
					bsmlparserParams.AddEvent(action.Key.Substring(1), delegate
					{
						action.Value.Invoke(Array.Empty<object>());
					});
				}
			}
			foreach (BSMLParser.ComponentTypeWithData componentTypeWithData in enumerable)
			{
				componentTypeWithData.typeHandler.HandleTypeAfterParse(componentTypeWithData, bsmlparserParams);
			}
			bsmlparserParams.EmitEvent("post-parse");
			return bsmlparserParams;
		}

		// Token: 0x06000043 RID: 67 RVA: 0x00003378 File Offset: 0x00001578
		public void HandleNode(XmlNode node, GameObject parent, BSMLParserParams parserParams, out IEnumerable<BSMLParser.ComponentTypeWithData> componentInfo)
		{
			if (node.Name.StartsWith(BSMLParser.MACRO_PREFIX))
			{
				this.HandleMacroNode(node, parent, parserParams, out componentInfo);
				return;
			}
			this.HandleTagNode(node, parent, parserParams, out componentInfo);
		}

		// Token: 0x06000044 RID: 68 RVA: 0x000033A4 File Offset: 0x000015A4
		private void HandleTagNode(XmlNode node, GameObject parent, BSMLParserParams parserParams, out IEnumerable<BSMLParser.ComponentTypeWithData> componentInfo)
		{
			BSMLTag bsmltag;
			if (!this.tags.TryGetValue(node.Name, out bsmltag))
			{
				throw new Exception("Tag type '" + node.Name + "' not found");
			}
			GameObject gameObject = bsmltag.CreateObject(parent.transform);
			List<BSMLParser.ComponentTypeWithData> list = new List<BSMLParser.ComponentTypeWithData>();
			foreach (TypeHandler typeHandler in this.typeHandlers)
			{
				Type type = (typeHandler.GetType().GetCustomAttributes(typeof(ComponentHandler), true).FirstOrDefault<object>() as ComponentHandler).type;
				Component externalComponent = this.GetExternalComponent(gameObject, type);
				if (externalComponent != null)
				{
					Dictionary<string, BSMLPropertyValue> dictionary;
					list.Add(new BSMLParser.ComponentTypeWithData
					{
						data = this.GetParameters(node, typeHandler.CachedProps, parserParams, out dictionary),
						propertyMap = dictionary,
						typeHandler = typeHandler,
						component = externalComponent
					});
				}
			}
			foreach (BSMLParser.ComponentTypeWithData componentTypeWithData in list)
			{
				componentTypeWithData.typeHandler.HandleType(componentTypeWithData, parserParams);
			}
			object host = parserParams.host;
			if (host != null && node.Attributes["id"] != null)
			{
				foreach (FieldInfo fieldInfo in host.GetType().GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
				{
					UIComponent uicomponent = fieldInfo.GetCustomAttributes(typeof(UIComponent), true).FirstOrDefault<object>() as UIComponent;
					if (uicomponent != null && uicomponent.id == node.Attributes["id"].Value)
					{
						fieldInfo.SetValue(host, this.GetExternalComponent(gameObject, fieldInfo.FieldType));
					}
					UIObject uiobject = fieldInfo.GetCustomAttributes(typeof(UIObject), true).FirstOrDefault<object>() as UIObject;
					if (uiobject != null && uiobject.id == node.Attributes["id"].Value)
					{
						fieldInfo.SetValue(host, gameObject);
					}
				}
			}
			if (node.Attributes["tags"] != null)
			{
				parserParams.AddObjectTags(gameObject, node.Attributes["tags"].Value.Split(new char[] { ',' }));
			}
			IEnumerable<BSMLParser.ComponentTypeWithData> enumerable = Enumerable.Empty<BSMLParser.ComponentTypeWithData>();
			if (bsmltag.AddChildren)
			{
				foreach (object obj in node.ChildNodes)
				{
					XmlNode xmlNode = (XmlNode)obj;
					IEnumerable<BSMLParser.ComponentTypeWithData> enumerable2;
					this.HandleNode(xmlNode, gameObject, parserParams, out enumerable2);
					enumerable = enumerable.Concat(enumerable2);
				}
			}
			foreach (BSMLParser.ComponentTypeWithData componentTypeWithData2 in list)
			{
				componentTypeWithData2.typeHandler.HandleTypeAfterChildren(componentTypeWithData2, parserParams);
			}
			componentInfo = list.Concat(enumerable);
		}

		// Token: 0x06000045 RID: 69 RVA: 0x000036FC File Offset: 0x000018FC
		private Component GetExternalComponent(GameObject gameObject, Type type)
		{
			Component component = null;
			ExternalComponents component2 = gameObject.GetComponent<ExternalComponents>();
			if (component2 != null)
			{
				foreach (Component component3 in component2.components)
				{
					if (type.IsAssignableFrom(component3.GetType()))
					{
						component = component3;
					}
				}
			}
			if (component == null)
			{
				component = gameObject.GetComponent(type);
			}
			return component;
		}

		// Token: 0x06000046 RID: 70 RVA: 0x0000377C File Offset: 0x0000197C
		private void HandleMacroNode(XmlNode node, GameObject parent, BSMLParserParams parserParams, out IEnumerable<BSMLParser.ComponentTypeWithData> components)
		{
			BSMLMacro bsmlmacro;
			if (!this.macros.TryGetValue(node.Name, out bsmlmacro))
			{
				throw new Exception("Macro type '" + node.Name + "' not found");
			}
			Dictionary<string, BSMLPropertyValue> dictionary;
			Dictionary<string, string> parameters = this.GetParameters(node, bsmlmacro.CachedProps, parserParams, out dictionary);
			bsmlmacro.Execute(node, parent, parameters, parserParams, out components);
		}

		// Token: 0x06000047 RID: 71 RVA: 0x000037D8 File Offset: 0x000019D8
		private Dictionary<string, string> GetParameters(XmlNode node, Dictionary<string, string[]> properties, BSMLParserParams parserParams, out Dictionary<string, BSMLPropertyValue> propertyMap)
		{
			Dictionary<string, string> dictionary = new Dictionary<string, string>();
			bool flag = parserParams.host != null && typeof(INotifiableHost).IsAssignableFrom(parserParams.host.GetType());
			propertyMap = null;
			if (flag)
			{
				propertyMap = new Dictionary<string, BSMLPropertyValue>();
			}
			foreach (KeyValuePair<string, string[]> keyValuePair in properties)
			{
				List<string> list = new List<string>(keyValuePair.Value);
				if (!list.Contains(keyValuePair.Key))
				{
					list.Add(keyValuePair.Key);
				}
				foreach (string text in list)
				{
					if (node.Attributes[text] != null)
					{
						string value = node.Attributes[text].Value;
						if (!value.StartsWith(BSMLParser.RETRIEVE_VALUE_PREFIX))
						{
							dictionary.Add(keyValuePair.Key, value);
							break;
						}
						string text2 = value.Substring(1);
						BSMLValue bsmlvalue;
						if (!parserParams.values.TryGetValue(text2, out bsmlvalue))
						{
							throw new Exception("No UIValue exists with the id '" + text2 + "'");
						}
						Dictionary<string, string> dictionary2 = dictionary;
						string key = keyValuePair.Key;
						object value2 = bsmlvalue.GetValue();
						dictionary2.Add(key, (value2 != null) ? value2.InvariantToString() : null);
						if (!flag)
						{
							break;
						}
						BSMLPropertyValue bsmlpropertyValue = bsmlvalue as BSMLPropertyValue;
						if (bsmlpropertyValue == null)
						{
							break;
						}
						if (bsmlpropertyValue != null)
						{
							propertyMap.Add(keyValuePair.Key, bsmlpropertyValue);
							break;
						}
						Logger log = Logger.log;
						if (log == null)
						{
							break;
						}
						log.Warn("PropertyValue is null for " + keyValuePair.Key);
						break;
					}
					else if (text == "_children")
					{
						dictionary.Add(keyValuePair.Key, node.InnerXml);
					}
				}
			}
			return dictionary;
		}

		// Token: 0x0400000F RID: 15
		private Dictionary<string, BSMLTag> tags = new Dictionary<string, BSMLTag>();

		// Token: 0x04000010 RID: 16
		private Dictionary<string, BSMLMacro> macros = new Dictionary<string, BSMLMacro>();

		// Token: 0x04000011 RID: 17
		private List<TypeHandler> typeHandlers;

		// Token: 0x04000012 RID: 18
		private XmlDocument doc = new XmlDocument();

		// Token: 0x04000013 RID: 19
		private XmlReaderSettings readerSettings = new XmlReaderSettings();

		// Token: 0x020000E6 RID: 230
		public struct ComponentTypeWithData
		{
			// Token: 0x040001B8 RID: 440
			public TypeHandler typeHandler;

			// Token: 0x040001B9 RID: 441
			public Component component;

			// Token: 0x040001BA RID: 442
			public Dictionary<string, string> data;

			// Token: 0x040001BB RID: 443
			public Dictionary<string, BSMLPropertyValue> propertyMap;
		}
	}
}
