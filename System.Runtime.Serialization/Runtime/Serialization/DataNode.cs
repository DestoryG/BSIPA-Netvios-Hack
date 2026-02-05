using System;
using System.Globalization;

namespace System.Runtime.Serialization
{
	// Token: 0x02000086 RID: 134
	internal class DataNode<T> : IDataNode
	{
		// Token: 0x06000986 RID: 2438 RVA: 0x0002A3C1 File Offset: 0x000285C1
		internal DataNode()
		{
			this.dataType = typeof(T);
			this.isFinalValue = true;
		}

		// Token: 0x06000987 RID: 2439 RVA: 0x0002A3EB File Offset: 0x000285EB
		internal DataNode(T value)
			: this()
		{
			this.value = value;
		}

		// Token: 0x17000171 RID: 369
		// (get) Token: 0x06000988 RID: 2440 RVA: 0x0002A3FA File Offset: 0x000285FA
		public Type DataType
		{
			get
			{
				return this.dataType;
			}
		}

		// Token: 0x17000172 RID: 370
		// (get) Token: 0x06000989 RID: 2441 RVA: 0x0002A402 File Offset: 0x00028602
		// (set) Token: 0x0600098A RID: 2442 RVA: 0x0002A40F File Offset: 0x0002860F
		public object Value
		{
			get
			{
				return this.value;
			}
			set
			{
				this.value = (T)((object)value);
			}
		}

		// Token: 0x17000173 RID: 371
		// (get) Token: 0x0600098B RID: 2443 RVA: 0x0002A41D File Offset: 0x0002861D
		// (set) Token: 0x0600098C RID: 2444 RVA: 0x0002A425 File Offset: 0x00028625
		bool IDataNode.IsFinalValue
		{
			get
			{
				return this.isFinalValue;
			}
			set
			{
				this.isFinalValue = value;
			}
		}

		// Token: 0x0600098D RID: 2445 RVA: 0x0002A42E File Offset: 0x0002862E
		public T GetValue()
		{
			return this.value;
		}

		// Token: 0x17000174 RID: 372
		// (get) Token: 0x0600098E RID: 2446 RVA: 0x0002A436 File Offset: 0x00028636
		// (set) Token: 0x0600098F RID: 2447 RVA: 0x0002A43E File Offset: 0x0002863E
		public string DataContractName
		{
			get
			{
				return this.dataContractName;
			}
			set
			{
				this.dataContractName = value;
			}
		}

		// Token: 0x17000175 RID: 373
		// (get) Token: 0x06000990 RID: 2448 RVA: 0x0002A447 File Offset: 0x00028647
		// (set) Token: 0x06000991 RID: 2449 RVA: 0x0002A44F File Offset: 0x0002864F
		public string DataContractNamespace
		{
			get
			{
				return this.dataContractNamespace;
			}
			set
			{
				this.dataContractNamespace = value;
			}
		}

		// Token: 0x17000176 RID: 374
		// (get) Token: 0x06000992 RID: 2450 RVA: 0x0002A458 File Offset: 0x00028658
		// (set) Token: 0x06000993 RID: 2451 RVA: 0x0002A460 File Offset: 0x00028660
		public string ClrTypeName
		{
			get
			{
				return this.clrTypeName;
			}
			set
			{
				this.clrTypeName = value;
			}
		}

		// Token: 0x17000177 RID: 375
		// (get) Token: 0x06000994 RID: 2452 RVA: 0x0002A469 File Offset: 0x00028669
		// (set) Token: 0x06000995 RID: 2453 RVA: 0x0002A471 File Offset: 0x00028671
		public string ClrAssemblyName
		{
			get
			{
				return this.clrAssemblyName;
			}
			set
			{
				this.clrAssemblyName = value;
			}
		}

		// Token: 0x17000178 RID: 376
		// (get) Token: 0x06000996 RID: 2454 RVA: 0x0002A47A File Offset: 0x0002867A
		public bool PreservesReferences
		{
			get
			{
				return this.Id != Globals.NewObjectId;
			}
		}

		// Token: 0x17000179 RID: 377
		// (get) Token: 0x06000997 RID: 2455 RVA: 0x0002A48C File Offset: 0x0002868C
		// (set) Token: 0x06000998 RID: 2456 RVA: 0x0002A494 File Offset: 0x00028694
		public string Id
		{
			get
			{
				return this.id;
			}
			set
			{
				this.id = value;
			}
		}

		// Token: 0x06000999 RID: 2457 RVA: 0x0002A4A0 File Offset: 0x000286A0
		public virtual void GetData(ElementData element)
		{
			element.dataNode = this;
			element.attributeCount = 0;
			element.childElementIndex = 0;
			if (this.DataContractName != null)
			{
				this.AddQualifiedNameAttribute(element, "i", "type", "http://www.w3.org/2001/XMLSchema-instance", this.DataContractName, this.DataContractNamespace);
			}
			if (this.ClrTypeName != null)
			{
				element.AddAttribute("z", "http://schemas.microsoft.com/2003/10/Serialization/", "Type", this.ClrTypeName);
			}
			if (this.ClrAssemblyName != null)
			{
				element.AddAttribute("z", "http://schemas.microsoft.com/2003/10/Serialization/", "Assembly", this.ClrAssemblyName);
			}
		}

		// Token: 0x0600099A RID: 2458 RVA: 0x0002A534 File Offset: 0x00028734
		public virtual void Clear()
		{
			this.clrTypeName = (this.clrAssemblyName = null);
		}

		// Token: 0x0600099B RID: 2459 RVA: 0x0002A554 File Offset: 0x00028754
		internal void AddQualifiedNameAttribute(ElementData element, string elementPrefix, string elementName, string elementNs, string valueName, string valueNs)
		{
			string prefix = ExtensionDataReader.GetPrefix(valueNs);
			element.AddAttribute(elementPrefix, elementNs, elementName, string.Format(CultureInfo.InvariantCulture, "{0}:{1}", prefix, valueName));
			bool flag = false;
			if (element.attributes != null)
			{
				for (int i = 0; i < element.attributes.Length; i++)
				{
					AttributeData attributeData = element.attributes[i];
					if (attributeData != null && attributeData.prefix == "xmlns" && attributeData.localName == prefix)
					{
						flag = true;
						break;
					}
				}
			}
			if (!flag)
			{
				element.AddAttribute("xmlns", "http://www.w3.org/2000/xmlns/", prefix, valueNs);
			}
		}

		// Token: 0x0400039F RID: 927
		protected Type dataType;

		// Token: 0x040003A0 RID: 928
		private T value;

		// Token: 0x040003A1 RID: 929
		private string dataContractName;

		// Token: 0x040003A2 RID: 930
		private string dataContractNamespace;

		// Token: 0x040003A3 RID: 931
		private string clrTypeName;

		// Token: 0x040003A4 RID: 932
		private string clrAssemblyName;

		// Token: 0x040003A5 RID: 933
		private string id = Globals.NewObjectId;

		// Token: 0x040003A6 RID: 934
		private bool isFinalValue;
	}
}
