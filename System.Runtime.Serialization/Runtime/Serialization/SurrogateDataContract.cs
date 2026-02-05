using System;
using System.Runtime.CompilerServices;
using System.Security;
using System.Security.Permissions;

namespace System.Runtime.Serialization
{
	// Token: 0x020000D8 RID: 216
	internal sealed class SurrogateDataContract : DataContract
	{
		// Token: 0x06000C1B RID: 3099 RVA: 0x000343C5 File Offset: 0x000325C5
		[SecuritySafeCritical]
		internal SurrogateDataContract(Type type, ISerializationSurrogate serializationSurrogate)
			: base(new SurrogateDataContract.SurrogateDataContractCriticalHelper(type, serializationSurrogate))
		{
			this.helper = base.Helper as SurrogateDataContract.SurrogateDataContractCriticalHelper;
		}

		// Token: 0x17000275 RID: 629
		// (get) Token: 0x06000C1C RID: 3100 RVA: 0x000343E5 File Offset: 0x000325E5
		internal ISerializationSurrogate SerializationSurrogate
		{
			[SecuritySafeCritical]
			get
			{
				return this.helper.SerializationSurrogate;
			}
		}

		// Token: 0x06000C1D RID: 3101 RVA: 0x000343F4 File Offset: 0x000325F4
		public override void WriteXmlValue(XmlWriterDelegator xmlWriter, object obj, XmlObjectSerializerWriteContext context)
		{
			SerializationInfo serializationInfo = new SerializationInfo(base.UnderlyingType, XmlObjectSerializer.FormatterConverter, !context.UnsafeTypeForwardingEnabled);
			this.SerializationSurrogateGetObjectData(obj, serializationInfo, context.GetStreamingContext());
			context.WriteSerializationInfo(xmlWriter, base.UnderlyingType, serializationInfo);
		}

		// Token: 0x06000C1E RID: 3102 RVA: 0x00034437 File Offset: 0x00032637
		[SecuritySafeCritical]
		[PermissionSet(SecurityAction.Demand, Unrestricted = true)]
		[MethodImpl(MethodImplOptions.NoInlining)]
		private object SerializationSurrogateSetObjectData(object obj, SerializationInfo serInfo, StreamingContext context)
		{
			return this.SerializationSurrogate.SetObjectData(obj, serInfo, context, null);
		}

		// Token: 0x06000C1F RID: 3103 RVA: 0x00034448 File Offset: 0x00032648
		[SecuritySafeCritical]
		[PermissionSet(SecurityAction.Demand, Unrestricted = true)]
		[MethodImpl(MethodImplOptions.NoInlining)]
		internal static object GetRealObject(IObjectReference obj, StreamingContext context)
		{
			return obj.GetRealObject(context);
		}

		// Token: 0x06000C20 RID: 3104 RVA: 0x00034451 File Offset: 0x00032651
		[SecuritySafeCritical]
		[PermissionSet(SecurityAction.Demand, Unrestricted = true)]
		[MethodImpl(MethodImplOptions.NoInlining)]
		private object GetUninitializedObject(Type objType)
		{
			return FormatterServices.GetUninitializedObject(objType);
		}

		// Token: 0x06000C21 RID: 3105 RVA: 0x00034459 File Offset: 0x00032659
		[SecuritySafeCritical]
		[PermissionSet(SecurityAction.Demand, Unrestricted = true)]
		[MethodImpl(MethodImplOptions.NoInlining)]
		private void SerializationSurrogateGetObjectData(object obj, SerializationInfo serInfo, StreamingContext context)
		{
			this.SerializationSurrogate.GetObjectData(obj, serInfo, context);
		}

		// Token: 0x06000C22 RID: 3106 RVA: 0x0003446C File Offset: 0x0003266C
		public override object ReadXmlValue(XmlReaderDelegator xmlReader, XmlObjectSerializerReadContext context)
		{
			xmlReader.Read();
			Type underlyingType = base.UnderlyingType;
			object obj = (underlyingType.IsArray ? Array.CreateInstance(underlyingType.GetElementType(), 0) : this.GetUninitializedObject(underlyingType));
			context.AddNewObject(obj);
			string objectId = context.GetObjectId();
			SerializationInfo serializationInfo = context.ReadSerializationInfo(xmlReader, underlyingType);
			object obj2 = this.SerializationSurrogateSetObjectData(obj, serializationInfo, context.GetStreamingContext());
			if (obj2 == null)
			{
				obj2 = obj;
			}
			if (obj2 is IDeserializationCallback)
			{
				((IDeserializationCallback)obj2).OnDeserialization(null);
			}
			if (obj2 is IObjectReference)
			{
				obj2 = SurrogateDataContract.GetRealObject((IObjectReference)obj2, context.GetStreamingContext());
			}
			context.ReplaceDeserializedObject(objectId, obj, obj2);
			xmlReader.ReadEndElement();
			return obj2;
		}

		// Token: 0x040004F5 RID: 1269
		[SecurityCritical]
		private SurrogateDataContract.SurrogateDataContractCriticalHelper helper;

		// Token: 0x02000178 RID: 376
		[SecurityCritical(SecurityCriticalScope.Everything)]
		private class SurrogateDataContractCriticalHelper : DataContract.DataContractCriticalHelper
		{
			// Token: 0x060014C1 RID: 5313 RVA: 0x00054250 File Offset: 0x00052450
			internal SurrogateDataContractCriticalHelper(Type type, ISerializationSurrogate serializationSurrogate)
				: base(type)
			{
				this.serializationSurrogate = serializationSurrogate;
				string text;
				string text2;
				DataContract.GetDefaultStableName(DataContract.GetClrTypeFullName(type), out text, out text2);
				base.SetDataContractName(DataContract.CreateQualifiedName(text, text2));
			}

			// Token: 0x17000451 RID: 1105
			// (get) Token: 0x060014C2 RID: 5314 RVA: 0x00054287 File Offset: 0x00052487
			internal ISerializationSurrogate SerializationSurrogate
			{
				get
				{
					return this.serializationSurrogate;
				}
			}

			// Token: 0x04000A24 RID: 2596
			private ISerializationSurrogate serializationSurrogate;
		}
	}
}
