using System;
using System.Collections.Generic;
using System.Reflection;
using System.Security;
using System.Xml;

namespace System.Runtime.Serialization
{
	// Token: 0x0200009E RID: 158
	internal abstract class PrimitiveDataContract : DataContract
	{
		// Token: 0x06000AF8 RID: 2808 RVA: 0x0002E3D9 File Offset: 0x0002C5D9
		[SecuritySafeCritical]
		protected PrimitiveDataContract(Type type, XmlDictionaryString name, XmlDictionaryString ns)
			: base(new PrimitiveDataContract.PrimitiveDataContractCriticalHelper(type, name, ns))
		{
			this.helper = base.Helper as PrimitiveDataContract.PrimitiveDataContractCriticalHelper;
		}

		// Token: 0x06000AF9 RID: 2809 RVA: 0x0002E3FA File Offset: 0x0002C5FA
		internal static PrimitiveDataContract GetPrimitiveDataContract(Type type)
		{
			return DataContract.GetBuiltInDataContract(type) as PrimitiveDataContract;
		}

		// Token: 0x06000AFA RID: 2810 RVA: 0x0002E407 File Offset: 0x0002C607
		internal static PrimitiveDataContract GetPrimitiveDataContract(string name, string ns)
		{
			return DataContract.GetBuiltInDataContract(name, ns) as PrimitiveDataContract;
		}

		// Token: 0x1700022A RID: 554
		// (get) Token: 0x06000AFB RID: 2811
		internal abstract string WriteMethodName { get; }

		// Token: 0x1700022B RID: 555
		// (get) Token: 0x06000AFC RID: 2812
		internal abstract string ReadMethodName { get; }

		// Token: 0x1700022C RID: 556
		// (get) Token: 0x06000AFD RID: 2813 RVA: 0x0002E415 File Offset: 0x0002C615
		// (set) Token: 0x06000AFE RID: 2814 RVA: 0x0002E41C File Offset: 0x0002C61C
		internal override XmlDictionaryString TopLevelElementNamespace
		{
			get
			{
				return DictionaryGlobals.SerializationNamespace;
			}
			set
			{
			}
		}

		// Token: 0x1700022D RID: 557
		// (get) Token: 0x06000AFF RID: 2815 RVA: 0x0002E41E File Offset: 0x0002C61E
		internal override bool CanContainReferences
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700022E RID: 558
		// (get) Token: 0x06000B00 RID: 2816 RVA: 0x0002E421 File Offset: 0x0002C621
		internal override bool IsPrimitive
		{
			get
			{
				return true;
			}
		}

		// Token: 0x1700022F RID: 559
		// (get) Token: 0x06000B01 RID: 2817 RVA: 0x0002E424 File Offset: 0x0002C624
		internal override bool IsBuiltInDataContract
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000230 RID: 560
		// (get) Token: 0x06000B02 RID: 2818 RVA: 0x0002E428 File Offset: 0x0002C628
		internal MethodInfo XmlFormatWriterMethod
		{
			[SecuritySafeCritical]
			get
			{
				if (this.helper.XmlFormatWriterMethod == null)
				{
					if (base.UnderlyingType.IsValueType)
					{
						this.helper.XmlFormatWriterMethod = typeof(XmlWriterDelegator).GetMethod(this.WriteMethodName, BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic, null, new Type[]
						{
							base.UnderlyingType,
							typeof(XmlDictionaryString),
							typeof(XmlDictionaryString)
						}, null);
					}
					else
					{
						this.helper.XmlFormatWriterMethod = typeof(XmlObjectSerializerWriteContext).GetMethod(this.WriteMethodName, BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic, null, new Type[]
						{
							typeof(XmlWriterDelegator),
							base.UnderlyingType,
							typeof(XmlDictionaryString),
							typeof(XmlDictionaryString)
						}, null);
					}
				}
				return this.helper.XmlFormatWriterMethod;
			}
		}

		// Token: 0x17000231 RID: 561
		// (get) Token: 0x06000B03 RID: 2819 RVA: 0x0002E50C File Offset: 0x0002C70C
		internal MethodInfo XmlFormatContentWriterMethod
		{
			[SecuritySafeCritical]
			get
			{
				if (this.helper.XmlFormatContentWriterMethod == null)
				{
					if (base.UnderlyingType.IsValueType)
					{
						this.helper.XmlFormatContentWriterMethod = typeof(XmlWriterDelegator).GetMethod(this.WriteMethodName, BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic, null, new Type[] { base.UnderlyingType }, null);
					}
					else
					{
						this.helper.XmlFormatContentWriterMethod = typeof(XmlObjectSerializerWriteContext).GetMethod(this.WriteMethodName, BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic, null, new Type[]
						{
							typeof(XmlWriterDelegator),
							base.UnderlyingType
						}, null);
					}
				}
				return this.helper.XmlFormatContentWriterMethod;
			}
		}

		// Token: 0x17000232 RID: 562
		// (get) Token: 0x06000B04 RID: 2820 RVA: 0x0002E5BC File Offset: 0x0002C7BC
		internal MethodInfo XmlFormatReaderMethod
		{
			[SecuritySafeCritical]
			get
			{
				if (this.helper.XmlFormatReaderMethod == null)
				{
					this.helper.XmlFormatReaderMethod = typeof(XmlReaderDelegator).GetMethod(this.ReadMethodName, BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
				}
				return this.helper.XmlFormatReaderMethod;
			}
		}

		// Token: 0x06000B05 RID: 2821 RVA: 0x0002E609 File Offset: 0x0002C809
		public override void WriteXmlValue(XmlWriterDelegator xmlWriter, object obj, XmlObjectSerializerWriteContext context)
		{
			xmlWriter.WriteAnyType(obj);
		}

		// Token: 0x06000B06 RID: 2822 RVA: 0x0002E612 File Offset: 0x0002C812
		protected object HandleReadValue(object obj, XmlObjectSerializerReadContext context)
		{
			context.AddNewObject(obj);
			return obj;
		}

		// Token: 0x06000B07 RID: 2823 RVA: 0x0002E61C File Offset: 0x0002C81C
		protected bool TryReadNullAtTopLevel(XmlReaderDelegator reader)
		{
			Attributes attributes = new Attributes();
			attributes.Read(reader);
			if (attributes.Ref != Globals.NewObjectId)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(XmlObjectSerializer.CreateSerializationException(SR.GetString("Cannot deserialize since root element references unrecognized object with id '{0}'.", new object[] { attributes.Ref })));
			}
			if (attributes.XsiNil)
			{
				reader.Skip();
				return true;
			}
			return false;
		}

		// Token: 0x06000B08 RID: 2824 RVA: 0x0002E680 File Offset: 0x0002C880
		internal override bool Equals(object other, Dictionary<DataContractPairKey, object> checkedContracts)
		{
			if (other is PrimitiveDataContract)
			{
				Type type = base.GetType();
				Type type2 = other.GetType();
				return type.Equals(type2) || type.IsSubclassOf(type2) || type2.IsSubclassOf(type);
			}
			return false;
		}

		// Token: 0x040004D3 RID: 1235
		[SecurityCritical]
		private PrimitiveDataContract.PrimitiveDataContractCriticalHelper helper;

		// Token: 0x02000176 RID: 374
		[SecurityCritical(SecurityCriticalScope.Everything)]
		private class PrimitiveDataContractCriticalHelper : DataContract.DataContractCriticalHelper
		{
			// Token: 0x060014B8 RID: 5304 RVA: 0x000541EF File Offset: 0x000523EF
			internal PrimitiveDataContractCriticalHelper(Type type, XmlDictionaryString name, XmlDictionaryString ns)
				: base(type)
			{
				base.SetDataContractName(name, ns);
			}

			// Token: 0x1700044E RID: 1102
			// (get) Token: 0x060014B9 RID: 5305 RVA: 0x00054200 File Offset: 0x00052400
			// (set) Token: 0x060014BA RID: 5306 RVA: 0x00054208 File Offset: 0x00052408
			internal MethodInfo XmlFormatWriterMethod
			{
				get
				{
					return this.xmlFormatWriterMethod;
				}
				set
				{
					this.xmlFormatWriterMethod = value;
				}
			}

			// Token: 0x1700044F RID: 1103
			// (get) Token: 0x060014BB RID: 5307 RVA: 0x00054211 File Offset: 0x00052411
			// (set) Token: 0x060014BC RID: 5308 RVA: 0x00054219 File Offset: 0x00052419
			internal MethodInfo XmlFormatContentWriterMethod
			{
				get
				{
					return this.xmlFormatContentWriterMethod;
				}
				set
				{
					this.xmlFormatContentWriterMethod = value;
				}
			}

			// Token: 0x17000450 RID: 1104
			// (get) Token: 0x060014BD RID: 5309 RVA: 0x00054222 File Offset: 0x00052422
			// (set) Token: 0x060014BE RID: 5310 RVA: 0x0005422A File Offset: 0x0005242A
			internal MethodInfo XmlFormatReaderMethod
			{
				get
				{
					return this.xmlFormatReaderMethod;
				}
				set
				{
					this.xmlFormatReaderMethod = value;
				}
			}

			// Token: 0x04000A21 RID: 2593
			private MethodInfo xmlFormatWriterMethod;

			// Token: 0x04000A22 RID: 2594
			private MethodInfo xmlFormatContentWriterMethod;

			// Token: 0x04000A23 RID: 2595
			private MethodInfo xmlFormatReaderMethod;
		}
	}
}
