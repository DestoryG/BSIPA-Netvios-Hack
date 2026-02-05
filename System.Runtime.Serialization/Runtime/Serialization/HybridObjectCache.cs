using System;
using System.Collections.Generic;

namespace System.Runtime.Serialization
{
	// Token: 0x02000091 RID: 145
	internal class HybridObjectCache
	{
		// Token: 0x06000A5A RID: 2650 RVA: 0x0002C97B File Offset: 0x0002AB7B
		internal HybridObjectCache()
		{
		}

		// Token: 0x06000A5B RID: 2651 RVA: 0x0002C984 File Offset: 0x0002AB84
		internal void Add(string id, object obj)
		{
			if (this.objectDictionary == null)
			{
				this.objectDictionary = new Dictionary<string, object>();
			}
			object obj2;
			if (this.objectDictionary.TryGetValue(id, out obj2))
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(XmlObjectSerializer.CreateSerializationException(SR.GetString("Invalid XML encountered. The same Id value '{0}' is defined more than once. Multiple objects cannot be deserialized using the same Id.", new object[] { id })));
			}
			this.objectDictionary.Add(id, obj);
		}

		// Token: 0x06000A5C RID: 2652 RVA: 0x0002C9E0 File Offset: 0x0002ABE0
		internal void Remove(string id)
		{
			if (this.objectDictionary != null)
			{
				this.objectDictionary.Remove(id);
			}
		}

		// Token: 0x06000A5D RID: 2653 RVA: 0x0002C9F8 File Offset: 0x0002ABF8
		internal object GetObject(string id)
		{
			if (this.referencedObjectDictionary == null)
			{
				this.referencedObjectDictionary = new Dictionary<string, object>();
				this.referencedObjectDictionary.Add(id, null);
			}
			else if (!this.referencedObjectDictionary.ContainsKey(id))
			{
				this.referencedObjectDictionary.Add(id, null);
			}
			if (this.objectDictionary != null)
			{
				object obj;
				this.objectDictionary.TryGetValue(id, out obj);
				return obj;
			}
			return null;
		}

		// Token: 0x06000A5E RID: 2654 RVA: 0x0002CA5C File Offset: 0x0002AC5C
		internal bool IsObjectReferenced(string id)
		{
			return this.referencedObjectDictionary != null && this.referencedObjectDictionary.ContainsKey(id);
		}

		// Token: 0x0400048B RID: 1163
		private Dictionary<string, object> objectDictionary;

		// Token: 0x0400048C RID: 1164
		private Dictionary<string, object> referencedObjectDictionary;
	}
}
