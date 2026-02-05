using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Google.Protobuf
{
	// Token: 0x02000009 RID: 9
	public sealed class ExtensionRegistry : ICollection<Extension>, IEnumerable<Extension>, IEnumerable, IDeepCloneable<ExtensionRegistry>
	{
		// Token: 0x060000C0 RID: 192 RVA: 0x000046B3 File Offset: 0x000028B3
		public ExtensionRegistry()
		{
			this.extensions = new Dictionary<ObjectIntPair<Type>, Extension>();
		}

		// Token: 0x060000C1 RID: 193 RVA: 0x000046C8 File Offset: 0x000028C8
		private ExtensionRegistry(IDictionary<ObjectIntPair<Type>, Extension> collection)
		{
			this.extensions = collection.ToDictionary((KeyValuePair<ObjectIntPair<Type>, Extension> k) => k.Key, (KeyValuePair<ObjectIntPair<Type>, Extension> v) => v.Value);
		}

		// Token: 0x17000018 RID: 24
		// (get) Token: 0x060000C2 RID: 194 RVA: 0x00004725 File Offset: 0x00002925
		public int Count
		{
			get
			{
				return this.extensions.Count;
			}
		}

		// Token: 0x17000019 RID: 25
		// (get) Token: 0x060000C3 RID: 195 RVA: 0x00004732 File Offset: 0x00002932
		bool ICollection<Extension>.IsReadOnly
		{
			get
			{
				return false;
			}
		}

		// Token: 0x060000C4 RID: 196 RVA: 0x00004735 File Offset: 0x00002935
		internal bool ContainsInputField(CodedInputStream stream, Type target, out Extension extension)
		{
			return this.extensions.TryGetValue(new ObjectIntPair<Type>(target, WireFormat.GetTagFieldNumber(stream.LastTag)), out extension);
		}

		// Token: 0x060000C5 RID: 197 RVA: 0x00004754 File Offset: 0x00002954
		public void Add(Extension extension)
		{
			ProtoPreconditions.CheckNotNull<Extension>(extension, "extension");
			this.extensions.Add(new ObjectIntPair<Type>(extension.TargetType, extension.FieldNumber), extension);
		}

		// Token: 0x060000C6 RID: 198 RVA: 0x00004780 File Offset: 0x00002980
		public void AddRange(IEnumerable<Extension> extensions)
		{
			ProtoPreconditions.CheckNotNull<IEnumerable<Extension>>(extensions, "extensions");
			foreach (Extension extension in extensions)
			{
				this.Add(extension);
			}
		}

		// Token: 0x060000C7 RID: 199 RVA: 0x000047D4 File Offset: 0x000029D4
		public void Clear()
		{
			this.extensions.Clear();
		}

		// Token: 0x060000C8 RID: 200 RVA: 0x000047E1 File Offset: 0x000029E1
		public bool Contains(Extension item)
		{
			ProtoPreconditions.CheckNotNull<Extension>(item, "item");
			return this.extensions.ContainsKey(new ObjectIntPair<Type>(item.TargetType, item.FieldNumber));
		}

		// Token: 0x060000C9 RID: 201 RVA: 0x0000480C File Offset: 0x00002A0C
		void ICollection<Extension>.CopyTo(Extension[] array, int arrayIndex)
		{
			ProtoPreconditions.CheckNotNull<Extension[]>(array, "array");
			if (arrayIndex < 0 || arrayIndex >= array.Length)
			{
				throw new ArgumentOutOfRangeException("arrayIndex");
			}
			if (array.Length - arrayIndex < this.Count)
			{
				throw new ArgumentException("The provided array is shorter than the number of elements in the registry");
			}
			foreach (Extension extension in array)
			{
				this.extensions.Add(new ObjectIntPair<Type>(extension.TargetType, extension.FieldNumber), extension);
			}
		}

		// Token: 0x060000CA RID: 202 RVA: 0x00004881 File Offset: 0x00002A81
		public IEnumerator<Extension> GetEnumerator()
		{
			return this.extensions.Values.GetEnumerator();
		}

		// Token: 0x060000CB RID: 203 RVA: 0x00004893 File Offset: 0x00002A93
		public bool Remove(Extension item)
		{
			ProtoPreconditions.CheckNotNull<Extension>(item, "item");
			return this.extensions.Remove(new ObjectIntPair<Type>(item.TargetType, item.FieldNumber));
		}

		// Token: 0x060000CC RID: 204 RVA: 0x000048BD File Offset: 0x00002ABD
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		// Token: 0x060000CD RID: 205 RVA: 0x000048C5 File Offset: 0x00002AC5
		public ExtensionRegistry Clone()
		{
			return new ExtensionRegistry(this.extensions);
		}

		// Token: 0x04000026 RID: 38
		private IDictionary<ObjectIntPair<Type>, Extension> extensions;

		// Token: 0x0200008D RID: 141
		internal sealed class ExtensionComparer : IEqualityComparer<Extension>
		{
			// Token: 0x060008B9 RID: 2233 RVA: 0x0001E950 File Offset: 0x0001CB50
			public bool Equals(Extension a, Extension b)
			{
				return new ObjectIntPair<Type>(a.TargetType, a.FieldNumber).Equals(new ObjectIntPair<Type>(b.TargetType, b.FieldNumber));
			}

			// Token: 0x060008BA RID: 2234 RVA: 0x0001E988 File Offset: 0x0001CB88
			public int GetHashCode(Extension a)
			{
				return new ObjectIntPair<Type>(a.TargetType, a.FieldNumber).GetHashCode();
			}

			// Token: 0x0400035E RID: 862
			internal static ExtensionRegistry.ExtensionComparer Instance = new ExtensionRegistry.ExtensionComparer();
		}
	}
}
