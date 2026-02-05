using System;
using System.Collections.Generic;
using UnityEngine;

namespace BeatSaberMarkupLanguage.Parser
{
	// Token: 0x02000072 RID: 114
	public class BSMLParserParams
	{
		// Token: 0x060001E8 RID: 488 RVA: 0x0000C070 File Offset: 0x0000A270
		public void AddEvent(string ids, Action action)
		{
			foreach (string text in ids.Split(new char[] { ',' }))
			{
				if (this.events.ContainsKey(text))
				{
					Dictionary<string, Action> dictionary = this.events;
					string text2 = text;
					dictionary[text2] = (Action)Delegate.Combine(dictionary[text2], action);
				}
				else
				{
					this.events.Add(text, action);
				}
			}
		}

		// Token: 0x060001E9 RID: 489 RVA: 0x0000C0E4 File Offset: 0x0000A2E4
		public void EmitEvent(string ids)
		{
			foreach (string text in ids.Split(new char[] { ',' }))
			{
				if (this.events.ContainsKey(text))
				{
					this.events[text]();
				}
			}
		}

		// Token: 0x060001EA RID: 490 RVA: 0x0000C134 File Offset: 0x0000A334
		public void AddObjectTags(GameObject gameObject, params string[] tags)
		{
			foreach (string text in tags)
			{
				List<GameObject> list;
				if (this.objectsWithTag.TryGetValue(text, out list))
				{
					list.Add(gameObject);
				}
				else
				{
					this.objectsWithTag.Add(text, new List<GameObject> { gameObject });
				}
			}
		}

		// Token: 0x060001EB RID: 491 RVA: 0x0000C188 File Offset: 0x0000A388
		public List<GameObject> GetObjectsWithTag(string tag)
		{
			List<GameObject> list;
			if (this.objectsWithTag.TryGetValue(tag, out list))
			{
				return list;
			}
			return new List<GameObject>();
		}

		// Token: 0x060001EC RID: 492 RVA: 0x0000C1AC File Offset: 0x0000A3AC
		private void AddObjectsToTag(string tag, List<GameObject> gameObjects)
		{
			List<GameObject> list;
			if (this.objectsWithTag.TryGetValue(tag, out list))
			{
				list.AddRange(gameObjects);
				return;
			}
			this.objectsWithTag.Add(tag, gameObjects);
		}

		// Token: 0x060001ED RID: 493 RVA: 0x0000C1E0 File Offset: 0x0000A3E0
		public void PassTaggedObjects(BSMLParserParams parserParams)
		{
			foreach (KeyValuePair<string, List<GameObject>> keyValuePair in this.objectsWithTag)
			{
				parserParams.AddObjectsToTag(keyValuePair.Key, keyValuePair.Value);
			}
		}

		// Token: 0x04000051 RID: 81
		public object host;

		// Token: 0x04000052 RID: 82
		public Dictionary<string, BSMLAction> actions = new Dictionary<string, BSMLAction>();

		// Token: 0x04000053 RID: 83
		public Dictionary<string, BSMLValue> values = new Dictionary<string, BSMLValue>();

		// Token: 0x04000054 RID: 84
		private Dictionary<string, Action> events = new Dictionary<string, Action>();

		// Token: 0x04000055 RID: 85
		private Dictionary<string, List<GameObject>> objectsWithTag = new Dictionary<string, List<GameObject>>();
	}
}
