using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.Serialization.Diagnostics;
using System.Security;
using System.Security.Permissions;
using System.Xml;
using System.Xml.Schema;

namespace System.Runtime.Serialization
{
	// Token: 0x020000F1 RID: 241
	public class XsdDataContractImporter
	{
		// Token: 0x06000F08 RID: 3848 RVA: 0x0003D8BC File Offset: 0x0003BABC
		public XsdDataContractImporter()
		{
		}

		// Token: 0x06000F09 RID: 3849 RVA: 0x0003D8C4 File Offset: 0x0003BAC4
		public XsdDataContractImporter(CodeCompileUnit codeCompileUnit)
		{
			this.codeCompileUnit = codeCompileUnit;
		}

		// Token: 0x17000322 RID: 802
		// (get) Token: 0x06000F0A RID: 3850 RVA: 0x0003D8D3 File Offset: 0x0003BAD3
		// (set) Token: 0x06000F0B RID: 3851 RVA: 0x0003D8DB File Offset: 0x0003BADB
		public ImportOptions Options
		{
			get
			{
				return this.options;
			}
			set
			{
				this.options = value;
			}
		}

		// Token: 0x17000323 RID: 803
		// (get) Token: 0x06000F0C RID: 3852 RVA: 0x0003D8E4 File Offset: 0x0003BAE4
		public CodeCompileUnit CodeCompileUnit
		{
			get
			{
				return this.GetCodeCompileUnit();
			}
		}

		// Token: 0x06000F0D RID: 3853 RVA: 0x0003D8EC File Offset: 0x0003BAEC
		private CodeCompileUnit GetCodeCompileUnit()
		{
			if (this.codeCompileUnit == null)
			{
				this.codeCompileUnit = new CodeCompileUnit();
			}
			return this.codeCompileUnit;
		}

		// Token: 0x17000324 RID: 804
		// (get) Token: 0x06000F0E RID: 3854 RVA: 0x0003D908 File Offset: 0x0003BB08
		private DataContractSet DataContractSet
		{
			get
			{
				if (this.dataContractSet == null)
				{
					this.dataContractSet = ((this.Options == null) ? new DataContractSet(null, null, null) : new DataContractSet(this.Options.DataContractSurrogate, this.Options.ReferencedTypes, this.Options.ReferencedCollectionTypes));
				}
				return this.dataContractSet;
			}
		}

		// Token: 0x06000F0F RID: 3855 RVA: 0x0003D961 File Offset: 0x0003BB61
		public void Import(XmlSchemaSet schemas)
		{
			if (schemas == null)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentNullException("schemas"));
			}
			this.InternalImport(schemas, null, null, null);
		}

		// Token: 0x06000F10 RID: 3856 RVA: 0x0003D980 File Offset: 0x0003BB80
		public void Import(XmlSchemaSet schemas, ICollection<XmlQualifiedName> typeNames)
		{
			if (schemas == null)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentNullException("schemas"));
			}
			if (typeNames == null)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentNullException("typeNames"));
			}
			this.InternalImport(schemas, typeNames, XsdDataContractImporter.emptyElementArray, XsdDataContractImporter.emptyTypeNameArray);
		}

		// Token: 0x06000F11 RID: 3857 RVA: 0x0003D9BC File Offset: 0x0003BBBC
		public void Import(XmlSchemaSet schemas, XmlQualifiedName typeName)
		{
			if (schemas == null)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentNullException("schemas"));
			}
			if (typeName == null)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentNullException("typeName"));
			}
			this.SingleTypeNameArray[0] = typeName;
			this.InternalImport(schemas, this.SingleTypeNameArray, XsdDataContractImporter.emptyElementArray, XsdDataContractImporter.emptyTypeNameArray);
		}

		// Token: 0x06000F12 RID: 3858 RVA: 0x0003DA18 File Offset: 0x0003BC18
		public XmlQualifiedName Import(XmlSchemaSet schemas, XmlSchemaElement element)
		{
			if (schemas == null)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentNullException("schemas"));
			}
			if (element == null)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentNullException("element"));
			}
			this.SingleTypeNameArray[0] = null;
			this.SingleElementArray[0] = element;
			this.InternalImport(schemas, XsdDataContractImporter.emptyTypeNameArray, this.SingleElementArray, this.SingleTypeNameArray);
			return this.SingleTypeNameArray[0];
		}

		// Token: 0x06000F13 RID: 3859 RVA: 0x0003DA7D File Offset: 0x0003BC7D
		public bool CanImport(XmlSchemaSet schemas)
		{
			if (schemas == null)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentNullException("schemas"));
			}
			return this.InternalCanImport(schemas, null, null, null);
		}

		// Token: 0x06000F14 RID: 3860 RVA: 0x0003DA9C File Offset: 0x0003BC9C
		public bool CanImport(XmlSchemaSet schemas, ICollection<XmlQualifiedName> typeNames)
		{
			if (schemas == null)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentNullException("schemas"));
			}
			if (typeNames == null)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentNullException("typeNames"));
			}
			return this.InternalCanImport(schemas, typeNames, XsdDataContractImporter.emptyElementArray, XsdDataContractImporter.emptyTypeNameArray);
		}

		// Token: 0x06000F15 RID: 3861 RVA: 0x0003DAD8 File Offset: 0x0003BCD8
		public bool CanImport(XmlSchemaSet schemas, XmlQualifiedName typeName)
		{
			if (schemas == null)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentNullException("schemas"));
			}
			if (typeName == null)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentNullException("typeName"));
			}
			return this.InternalCanImport(schemas, new XmlQualifiedName[] { typeName }, XsdDataContractImporter.emptyElementArray, XsdDataContractImporter.emptyTypeNameArray);
		}

		// Token: 0x06000F16 RID: 3862 RVA: 0x0003DB2C File Offset: 0x0003BD2C
		public bool CanImport(XmlSchemaSet schemas, XmlSchemaElement element)
		{
			if (schemas == null)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentNullException("schemas"));
			}
			if (element == null)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentNullException("element"));
			}
			this.SingleTypeNameArray[0] = null;
			this.SingleElementArray[0] = element;
			return this.InternalCanImport(schemas, XsdDataContractImporter.emptyTypeNameArray, this.SingleElementArray, this.SingleTypeNameArray);
		}

		// Token: 0x06000F17 RID: 3863 RVA: 0x0003DB8C File Offset: 0x0003BD8C
		public CodeTypeReference GetCodeTypeReference(XmlQualifiedName typeName)
		{
			DataContract dataContract = this.FindDataContract(typeName);
			return new CodeExporter(this.DataContractSet, this.Options, this.GetCodeCompileUnit()).GetCodeTypeReference(dataContract);
		}

		// Token: 0x06000F18 RID: 3864 RVA: 0x0003DBC0 File Offset: 0x0003BDC0
		public CodeTypeReference GetCodeTypeReference(XmlQualifiedName typeName, XmlSchemaElement element)
		{
			if (element == null)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentNullException("element"));
			}
			if (typeName == null)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentNullException("typeName"));
			}
			DataContract dataContract = this.FindDataContract(typeName);
			return new CodeExporter(this.DataContractSet, this.Options, this.GetCodeCompileUnit()).GetElementTypeReference(dataContract, element.IsNillable);
		}

		// Token: 0x06000F19 RID: 3865 RVA: 0x0003DC24 File Offset: 0x0003BE24
		internal DataContract FindDataContract(XmlQualifiedName typeName)
		{
			if (typeName == null)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentNullException("typeName"));
			}
			DataContract dataContract = DataContract.GetBuiltInDataContract(typeName.Name, typeName.Namespace);
			if (dataContract == null)
			{
				dataContract = this.DataContractSet[typeName];
				if (dataContract == null)
				{
					throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new InvalidOperationException(SR.GetString("Type '{0}' in '{1}' namespace has not been imported.", new object[] { typeName.Name, typeName.Namespace })));
				}
			}
			return dataContract;
		}

		// Token: 0x06000F1A RID: 3866 RVA: 0x0003DCA0 File Offset: 0x0003BEA0
		public ICollection<CodeTypeReference> GetKnownTypeReferences(XmlQualifiedName typeName)
		{
			if (typeName == null)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentNullException("typeName"));
			}
			DataContract dataContract = DataContract.GetBuiltInDataContract(typeName.Name, typeName.Namespace);
			if (dataContract == null)
			{
				dataContract = this.DataContractSet[typeName];
				if (dataContract == null)
				{
					throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new InvalidOperationException(SR.GetString("Type '{0}' in '{1}' namespace has not been imported.", new object[] { typeName.Name, typeName.Namespace })));
				}
			}
			return new CodeExporter(this.DataContractSet, this.Options, this.GetCodeCompileUnit()).GetKnownTypeReferences(dataContract);
		}

		// Token: 0x17000325 RID: 805
		// (get) Token: 0x06000F1B RID: 3867 RVA: 0x0003DD35 File Offset: 0x0003BF35
		private XmlQualifiedName[] SingleTypeNameArray
		{
			get
			{
				if (this.singleTypeNameArray == null)
				{
					this.singleTypeNameArray = new XmlQualifiedName[1];
				}
				return this.singleTypeNameArray;
			}
		}

		// Token: 0x17000326 RID: 806
		// (get) Token: 0x06000F1C RID: 3868 RVA: 0x0003DD51 File Offset: 0x0003BF51
		private XmlSchemaElement[] SingleElementArray
		{
			get
			{
				if (this.singleElementArray == null)
				{
					this.singleElementArray = new XmlSchemaElement[1];
				}
				return this.singleElementArray;
			}
		}

		// Token: 0x06000F1D RID: 3869 RVA: 0x0003DD70 File Offset: 0x0003BF70
		[SecuritySafeCritical]
		[PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
		private void InternalImport(XmlSchemaSet schemas, ICollection<XmlQualifiedName> typeNames, ICollection<XmlSchemaElement> elements, XmlQualifiedName[] elementTypeNames)
		{
			if (DiagnosticUtility.ShouldTraceInformation)
			{
				TraceUtility.Trace(TraceEventType.Information, 196618, SR.GetString("XSD import begins"));
			}
			DataContractSet dataContractSet = ((this.dataContractSet == null) ? null : new DataContractSet(this.dataContractSet));
			try
			{
				new SchemaImporter(schemas, typeNames, elements, elementTypeNames, this.DataContractSet, this.ImportXmlDataType).Import();
				new CodeExporter(this.DataContractSet, this.Options, this.GetCodeCompileUnit()).Export();
			}
			catch (Exception ex)
			{
				if (Fx.IsFatal(ex))
				{
					throw;
				}
				this.dataContractSet = dataContractSet;
				this.TraceImportError(ex);
				throw;
			}
			if (DiagnosticUtility.ShouldTraceInformation)
			{
				TraceUtility.Trace(TraceEventType.Information, 196619, SR.GetString("XSD import ends"));
			}
		}

		// Token: 0x17000327 RID: 807
		// (get) Token: 0x06000F1E RID: 3870 RVA: 0x0003DE30 File Offset: 0x0003C030
		private bool ImportXmlDataType
		{
			get
			{
				return this.Options != null && this.Options.ImportXmlType;
			}
		}

		// Token: 0x06000F1F RID: 3871 RVA: 0x0003DE47 File Offset: 0x0003C047
		private void TraceImportError(Exception exception)
		{
			if (DiagnosticUtility.ShouldTraceError)
			{
				TraceUtility.Trace(TraceEventType.Error, 196621, SR.GetString("XSD import error"), null, exception);
			}
		}

		// Token: 0x06000F20 RID: 3872 RVA: 0x0003DE68 File Offset: 0x0003C068
		private bool InternalCanImport(XmlSchemaSet schemas, ICollection<XmlQualifiedName> typeNames, ICollection<XmlSchemaElement> elements, XmlQualifiedName[] elementTypeNames)
		{
			DataContractSet dataContractSet = ((this.dataContractSet == null) ? null : new DataContractSet(this.dataContractSet));
			bool flag;
			try
			{
				new SchemaImporter(schemas, typeNames, elements, elementTypeNames, this.DataContractSet, this.ImportXmlDataType).Import();
				flag = true;
			}
			catch (InvalidDataContractException)
			{
				this.dataContractSet = dataContractSet;
				flag = false;
			}
			catch (Exception ex)
			{
				if (Fx.IsFatal(ex))
				{
					throw;
				}
				this.dataContractSet = dataContractSet;
				this.TraceImportError(ex);
				throw;
			}
			return flag;
		}

		// Token: 0x04000592 RID: 1426
		private ImportOptions options;

		// Token: 0x04000593 RID: 1427
		private CodeCompileUnit codeCompileUnit;

		// Token: 0x04000594 RID: 1428
		private DataContractSet dataContractSet;

		// Token: 0x04000595 RID: 1429
		private static readonly XmlQualifiedName[] emptyTypeNameArray = new XmlQualifiedName[0];

		// Token: 0x04000596 RID: 1430
		private static readonly XmlSchemaElement[] emptyElementArray = new XmlSchemaElement[0];

		// Token: 0x04000597 RID: 1431
		private XmlQualifiedName[] singleTypeNameArray;

		// Token: 0x04000598 RID: 1432
		private XmlSchemaElement[] singleElementArray;
	}
}
