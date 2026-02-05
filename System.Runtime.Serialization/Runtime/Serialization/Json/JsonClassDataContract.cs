using System;
using System.Collections.Generic;
using System.Security;
using System.Threading;
using System.Xml;

namespace System.Runtime.Serialization.Json
{
	// Token: 0x02000101 RID: 257
	internal class JsonClassDataContract : JsonDataContract
	{
		// Token: 0x06000FD4 RID: 4052 RVA: 0x00040FFF File Offset: 0x0003F1FF
		[SecuritySafeCritical]
		public JsonClassDataContract(ClassDataContract traditionalDataContract)
			: base(new JsonClassDataContract.JsonClassDataContractCriticalHelper(traditionalDataContract))
		{
			this.helper = base.Helper as JsonClassDataContract.JsonClassDataContractCriticalHelper;
		}

		// Token: 0x17000341 RID: 833
		// (get) Token: 0x06000FD5 RID: 4053 RVA: 0x00041020 File Offset: 0x0003F220
		internal JsonFormatClassReaderDelegate JsonFormatReaderDelegate
		{
			[SecuritySafeCritical]
			get
			{
				if (this.helper.JsonFormatReaderDelegate == null)
				{
					lock (this)
					{
						if (this.helper.JsonFormatReaderDelegate == null)
						{
							if (this.TraditionalClassDataContract.IsReadOnlyContract)
							{
								DataContract.ThrowInvalidDataContractException(this.TraditionalClassDataContract.DeserializationExceptionMessage, null);
							}
							JsonFormatClassReaderDelegate jsonFormatClassReaderDelegate = new JsonFormatReaderGenerator().GenerateClassReader(this.TraditionalClassDataContract);
							Thread.MemoryBarrier();
							this.helper.JsonFormatReaderDelegate = jsonFormatClassReaderDelegate;
						}
					}
				}
				return this.helper.JsonFormatReaderDelegate;
			}
		}

		// Token: 0x17000342 RID: 834
		// (get) Token: 0x06000FD6 RID: 4054 RVA: 0x000410BC File Offset: 0x0003F2BC
		internal JsonFormatClassWriterDelegate JsonFormatWriterDelegate
		{
			[SecuritySafeCritical]
			get
			{
				if (this.helper.JsonFormatWriterDelegate == null)
				{
					lock (this)
					{
						if (this.helper.JsonFormatWriterDelegate == null)
						{
							JsonFormatClassWriterDelegate jsonFormatClassWriterDelegate = new JsonFormatWriterGenerator().GenerateClassWriter(this.TraditionalClassDataContract);
							Thread.MemoryBarrier();
							this.helper.JsonFormatWriterDelegate = jsonFormatClassWriterDelegate;
						}
					}
				}
				return this.helper.JsonFormatWriterDelegate;
			}
		}

		// Token: 0x17000343 RID: 835
		// (get) Token: 0x06000FD7 RID: 4055 RVA: 0x00041138 File Offset: 0x0003F338
		internal XmlDictionaryString[] MemberNames
		{
			[SecuritySafeCritical]
			get
			{
				return this.helper.MemberNames;
			}
		}

		// Token: 0x17000344 RID: 836
		// (get) Token: 0x06000FD8 RID: 4056 RVA: 0x00041145 File Offset: 0x0003F345
		internal override string TypeName
		{
			[SecuritySafeCritical]
			get
			{
				return this.helper.TypeName;
			}
		}

		// Token: 0x17000345 RID: 837
		// (get) Token: 0x06000FD9 RID: 4057 RVA: 0x00041152 File Offset: 0x0003F352
		private ClassDataContract TraditionalClassDataContract
		{
			[SecuritySafeCritical]
			get
			{
				return this.helper.TraditionalClassDataContract;
			}
		}

		// Token: 0x06000FDA RID: 4058 RVA: 0x0004115F File Offset: 0x0003F35F
		public override object ReadJsonValueCore(XmlReaderDelegator jsonReader, XmlObjectSerializerReadContextComplexJson context)
		{
			jsonReader.Read();
			object obj = this.JsonFormatReaderDelegate(jsonReader, context, XmlDictionaryString.Empty, this.MemberNames);
			jsonReader.ReadEndElement();
			return obj;
		}

		// Token: 0x06000FDB RID: 4059 RVA: 0x00041186 File Offset: 0x0003F386
		public override void WriteJsonValueCore(XmlWriterDelegator jsonWriter, object obj, XmlObjectSerializerWriteContextComplexJson context, RuntimeTypeHandle declaredTypeHandle)
		{
			jsonWriter.WriteAttributeString(null, "type", null, "object");
			this.JsonFormatWriterDelegate(jsonWriter, obj, context, this.TraditionalClassDataContract, this.MemberNames);
		}

		// Token: 0x040007DB RID: 2011
		[SecurityCritical]
		private JsonClassDataContract.JsonClassDataContractCriticalHelper helper;

		// Token: 0x02000187 RID: 391
		[SecurityCritical(SecurityCriticalScope.Everything)]
		private class JsonClassDataContractCriticalHelper : JsonDataContract.JsonDataContractCriticalHelper
		{
			// Token: 0x0600150E RID: 5390 RVA: 0x00054B48 File Offset: 0x00052D48
			public JsonClassDataContractCriticalHelper(ClassDataContract traditionalDataContract)
				: base(traditionalDataContract)
			{
				this.typeName = (string.IsNullOrEmpty(traditionalDataContract.Namespace.Value) ? traditionalDataContract.Name.Value : (traditionalDataContract.Name.Value + ":" + XmlObjectSerializerWriteContextComplexJson.TruncateDefaultDataContractNamespace(traditionalDataContract.Namespace.Value)));
				this.traditionalClassDataContract = traditionalDataContract;
				this.CopyMembersAndCheckDuplicateNames();
			}

			// Token: 0x17000462 RID: 1122
			// (get) Token: 0x0600150F RID: 5391 RVA: 0x00054BB3 File Offset: 0x00052DB3
			// (set) Token: 0x06001510 RID: 5392 RVA: 0x00054BBB File Offset: 0x00052DBB
			internal JsonFormatClassReaderDelegate JsonFormatReaderDelegate
			{
				get
				{
					return this.jsonFormatReaderDelegate;
				}
				set
				{
					this.jsonFormatReaderDelegate = value;
				}
			}

			// Token: 0x17000463 RID: 1123
			// (get) Token: 0x06001511 RID: 5393 RVA: 0x00054BC4 File Offset: 0x00052DC4
			// (set) Token: 0x06001512 RID: 5394 RVA: 0x00054BCC File Offset: 0x00052DCC
			internal JsonFormatClassWriterDelegate JsonFormatWriterDelegate
			{
				get
				{
					return this.jsonFormatWriterDelegate;
				}
				set
				{
					this.jsonFormatWriterDelegate = value;
				}
			}

			// Token: 0x17000464 RID: 1124
			// (get) Token: 0x06001513 RID: 5395 RVA: 0x00054BD5 File Offset: 0x00052DD5
			internal XmlDictionaryString[] MemberNames
			{
				get
				{
					return this.memberNames;
				}
			}

			// Token: 0x17000465 RID: 1125
			// (get) Token: 0x06001514 RID: 5396 RVA: 0x00054BDD File Offset: 0x00052DDD
			internal ClassDataContract TraditionalClassDataContract
			{
				get
				{
					return this.traditionalClassDataContract;
				}
			}

			// Token: 0x06001515 RID: 5397 RVA: 0x00054BE8 File Offset: 0x00052DE8
			private void CopyMembersAndCheckDuplicateNames()
			{
				if (this.traditionalClassDataContract.MemberNames != null)
				{
					int num = this.traditionalClassDataContract.MemberNames.Length;
					Dictionary<string, object> dictionary = new Dictionary<string, object>(num);
					XmlDictionaryString[] array = new XmlDictionaryString[num];
					for (int i = 0; i < num; i++)
					{
						if (dictionary.ContainsKey(this.traditionalClassDataContract.MemberNames[i].Value))
						{
							throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new SerializationException(SR.GetString("Duplicate member, including '{1}', is found in JSON input, in type '{0}'.", new object[]
							{
								DataContract.GetClrTypeFullName(this.traditionalClassDataContract.UnderlyingType),
								this.traditionalClassDataContract.MemberNames[i].Value
							})));
						}
						dictionary.Add(this.traditionalClassDataContract.MemberNames[i].Value, null);
						array[i] = DataContractJsonSerializer.ConvertXmlNameToJsonName(this.traditionalClassDataContract.MemberNames[i]);
					}
					this.memberNames = array;
				}
			}

			// Token: 0x04000A41 RID: 2625
			private JsonFormatClassReaderDelegate jsonFormatReaderDelegate;

			// Token: 0x04000A42 RID: 2626
			private JsonFormatClassWriterDelegate jsonFormatWriterDelegate;

			// Token: 0x04000A43 RID: 2627
			private XmlDictionaryString[] memberNames;

			// Token: 0x04000A44 RID: 2628
			private ClassDataContract traditionalClassDataContract;

			// Token: 0x04000A45 RID: 2629
			private string typeName;
		}
	}
}
