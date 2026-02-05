using System;
using System.Reflection;
using System.Xml.Serialization;

namespace System.Runtime.Serialization
{
	// Token: 0x020000F8 RID: 248
	internal class XmlDataContractInterpreter
	{
		// Token: 0x06000F40 RID: 3904 RVA: 0x0003E255 File Offset: 0x0003C455
		public XmlDataContractInterpreter(XmlDataContract contract)
		{
			this.contract = contract;
		}

		// Token: 0x06000F41 RID: 3905 RVA: 0x0003E264 File Offset: 0x0003C464
		public IXmlSerializable CreateXmlSerializable()
		{
			Type underlyingType = this.contract.UnderlyingType;
			object obj;
			if (underlyingType.IsValueType)
			{
				obj = FormatterServices.GetUninitializedObject(underlyingType);
			}
			else
			{
				obj = this.GetConstructor().Invoke(new object[0]);
			}
			return (IXmlSerializable)obj;
		}

		// Token: 0x06000F42 RID: 3906 RVA: 0x0003E2A8 File Offset: 0x0003C4A8
		private ConstructorInfo GetConstructor()
		{
			Type underlyingType = this.contract.UnderlyingType;
			if (underlyingType.IsValueType)
			{
				return null;
			}
			ConstructorInfo constructor = underlyingType.GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, Globals.EmptyTypeArray, null);
			if (constructor == null)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new InvalidDataContractException(SR.GetString("IXmlSerializable Type '{0}' must have default constructor.", new object[] { DataContract.GetClrTypeFullName(underlyingType) })));
			}
			return constructor;
		}

		// Token: 0x040007AB RID: 1963
		private XmlDataContract contract;
	}
}
