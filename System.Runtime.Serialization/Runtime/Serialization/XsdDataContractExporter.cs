using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.Serialization.Diagnostics;
using System.Xml;
using System.Xml.Schema;

namespace System.Runtime.Serialization
{
	// Token: 0x020000F0 RID: 240
	public class XsdDataContractExporter
	{
		// Token: 0x06000EF0 RID: 3824 RVA: 0x0003D0EA File Offset: 0x0003B2EA
		public XsdDataContractExporter()
		{
		}

		// Token: 0x06000EF1 RID: 3825 RVA: 0x0003D0F2 File Offset: 0x0003B2F2
		public XsdDataContractExporter(XmlSchemaSet schemas)
		{
			this.schemas = schemas;
		}

		// Token: 0x1700031F RID: 799
		// (get) Token: 0x06000EF2 RID: 3826 RVA: 0x0003D101 File Offset: 0x0003B301
		// (set) Token: 0x06000EF3 RID: 3827 RVA: 0x0003D109 File Offset: 0x0003B309
		public ExportOptions Options
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

		// Token: 0x17000320 RID: 800
		// (get) Token: 0x06000EF4 RID: 3828 RVA: 0x0003D112 File Offset: 0x0003B312
		public XmlSchemaSet Schemas
		{
			get
			{
				XmlSchemaSet schemaSet = this.GetSchemaSet();
				SchemaImporter.CompileSchemaSet(schemaSet);
				return schemaSet;
			}
		}

		// Token: 0x06000EF5 RID: 3829 RVA: 0x0003D120 File Offset: 0x0003B320
		private XmlSchemaSet GetSchemaSet()
		{
			if (this.schemas == null)
			{
				this.schemas = new XmlSchemaSet();
				this.schemas.XmlResolver = null;
			}
			return this.schemas;
		}

		// Token: 0x17000321 RID: 801
		// (get) Token: 0x06000EF6 RID: 3830 RVA: 0x0003D147 File Offset: 0x0003B347
		private DataContractSet DataContractSet
		{
			get
			{
				if (this.dataContractSet == null)
				{
					this.dataContractSet = new DataContractSet((this.Options == null) ? null : this.Options.GetSurrogate());
				}
				return this.dataContractSet;
			}
		}

		// Token: 0x06000EF7 RID: 3831 RVA: 0x0003D178 File Offset: 0x0003B378
		private void TraceExportBegin()
		{
			if (DiagnosticUtility.ShouldTraceInformation)
			{
				TraceUtility.Trace(TraceEventType.Information, 196616, SR.GetString("XSD export begins"));
			}
		}

		// Token: 0x06000EF8 RID: 3832 RVA: 0x0003D196 File Offset: 0x0003B396
		private void TraceExportEnd()
		{
			if (DiagnosticUtility.ShouldTraceInformation)
			{
				TraceUtility.Trace(TraceEventType.Information, 196617, SR.GetString("XSD export ends"));
			}
		}

		// Token: 0x06000EF9 RID: 3833 RVA: 0x0003D1B4 File Offset: 0x0003B3B4
		private void TraceExportError(Exception exception)
		{
			if (DiagnosticUtility.ShouldTraceError)
			{
				TraceUtility.Trace(TraceEventType.Error, 196620, SR.GetString("XSD export error"), null, exception);
			}
		}

		// Token: 0x06000EFA RID: 3834 RVA: 0x0003D1D4 File Offset: 0x0003B3D4
		public void Export(ICollection<Assembly> assemblies)
		{
			if (assemblies == null)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentNullException("assemblies"));
			}
			this.TraceExportBegin();
			DataContractSet dataContractSet = ((this.dataContractSet == null) ? null : new DataContractSet(this.dataContractSet));
			try
			{
				foreach (Assembly assembly in assemblies)
				{
					if (assembly == null)
					{
						throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentException(SR.GetString("Cannot export null assembly.", new object[] { "assemblies" })));
					}
					Type[] types = assembly.GetTypes();
					for (int i = 0; i < types.Length; i++)
					{
						this.CheckAndAddType(types[i]);
					}
				}
				this.Export();
			}
			catch (Exception ex)
			{
				if (Fx.IsFatal(ex))
				{
					throw;
				}
				this.dataContractSet = dataContractSet;
				this.TraceExportError(ex);
				throw;
			}
			this.TraceExportEnd();
		}

		// Token: 0x06000EFB RID: 3835 RVA: 0x0003D2CC File Offset: 0x0003B4CC
		public void Export(ICollection<Type> types)
		{
			if (types == null)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentNullException("types"));
			}
			this.TraceExportBegin();
			DataContractSet dataContractSet = ((this.dataContractSet == null) ? null : new DataContractSet(this.dataContractSet));
			try
			{
				foreach (Type type in types)
				{
					if (type == null)
					{
						throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentException(SR.GetString("Cannot export null type.", new object[] { "types" })));
					}
					this.AddType(type);
				}
				this.Export();
			}
			catch (Exception ex)
			{
				if (Fx.IsFatal(ex))
				{
					throw;
				}
				this.dataContractSet = dataContractSet;
				this.TraceExportError(ex);
				throw;
			}
			this.TraceExportEnd();
		}

		// Token: 0x06000EFC RID: 3836 RVA: 0x0003D3A8 File Offset: 0x0003B5A8
		public void Export(Type type)
		{
			if (type == null)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentNullException("type"));
			}
			this.TraceExportBegin();
			DataContractSet dataContractSet = ((this.dataContractSet == null) ? null : new DataContractSet(this.dataContractSet));
			try
			{
				this.AddType(type);
				this.Export();
			}
			catch (Exception ex)
			{
				if (Fx.IsFatal(ex))
				{
					throw;
				}
				this.dataContractSet = dataContractSet;
				this.TraceExportError(ex);
				throw;
			}
			this.TraceExportEnd();
		}

		// Token: 0x06000EFD RID: 3837 RVA: 0x0003D42C File Offset: 0x0003B62C
		public XmlQualifiedName GetSchemaTypeName(Type type)
		{
			if (type == null)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentNullException("type"));
			}
			type = this.GetSurrogatedType(type);
			DataContract dataContract = DataContract.GetDataContract(type);
			DataContractSet.EnsureTypeNotGeneric(dataContract.UnderlyingType);
			XmlDataContract xmlDataContract = dataContract as XmlDataContract;
			if (xmlDataContract != null && xmlDataContract.IsAnonymous)
			{
				return XmlQualifiedName.Empty;
			}
			return dataContract.StableName;
		}

		// Token: 0x06000EFE RID: 3838 RVA: 0x0003D48C File Offset: 0x0003B68C
		public XmlSchemaType GetSchemaType(Type type)
		{
			if (type == null)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentNullException("type"));
			}
			type = this.GetSurrogatedType(type);
			DataContract dataContract = DataContract.GetDataContract(type);
			DataContractSet.EnsureTypeNotGeneric(dataContract.UnderlyingType);
			XmlDataContract xmlDataContract = dataContract as XmlDataContract;
			if (xmlDataContract != null && xmlDataContract.IsAnonymous)
			{
				return xmlDataContract.XsdType;
			}
			return null;
		}

		// Token: 0x06000EFF RID: 3839 RVA: 0x0003D4E8 File Offset: 0x0003B6E8
		public XmlQualifiedName GetRootElementName(Type type)
		{
			if (type == null)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentNullException("type"));
			}
			type = this.GetSurrogatedType(type);
			DataContract dataContract = DataContract.GetDataContract(type);
			DataContractSet.EnsureTypeNotGeneric(dataContract.UnderlyingType);
			if (dataContract.HasRoot)
			{
				return new XmlQualifiedName(dataContract.TopLevelElementName.Value, dataContract.TopLevelElementNamespace.Value);
			}
			return null;
		}

		// Token: 0x06000F00 RID: 3840 RVA: 0x0003D550 File Offset: 0x0003B750
		private Type GetSurrogatedType(Type type)
		{
			IDataContractSurrogate surrogate;
			if (this.options != null && (surrogate = this.Options.GetSurrogate()) != null)
			{
				type = DataContractSurrogateCaller.GetDataContractType(surrogate, type);
			}
			return type;
		}

		// Token: 0x06000F01 RID: 3841 RVA: 0x0003D57E File Offset: 0x0003B77E
		private void CheckAndAddType(Type type)
		{
			type = this.GetSurrogatedType(type);
			if (!type.ContainsGenericParameters && DataContract.IsTypeSerializable(type))
			{
				this.AddType(type);
			}
		}

		// Token: 0x06000F02 RID: 3842 RVA: 0x0003D5A0 File Offset: 0x0003B7A0
		private void AddType(Type type)
		{
			this.DataContractSet.Add(type);
		}

		// Token: 0x06000F03 RID: 3843 RVA: 0x0003D5AE File Offset: 0x0003B7AE
		private void Export()
		{
			this.AddKnownTypes();
			new SchemaExporter(this.GetSchemaSet(), this.DataContractSet).Export();
		}

		// Token: 0x06000F04 RID: 3844 RVA: 0x0003D5CC File Offset: 0x0003B7CC
		private void AddKnownTypes()
		{
			if (this.Options != null)
			{
				Collection<Type> knownTypes = this.Options.KnownTypes;
				if (knownTypes != null)
				{
					for (int i = 0; i < knownTypes.Count; i++)
					{
						Type type = knownTypes[i];
						if (type == null)
						{
							throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentException(SR.GetString("Cannot export null known type.")));
						}
						this.AddType(type);
					}
				}
			}
		}

		// Token: 0x06000F05 RID: 3845 RVA: 0x0003D630 File Offset: 0x0003B830
		public bool CanExport(ICollection<Assembly> assemblies)
		{
			if (assemblies == null)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentNullException("assemblies"));
			}
			DataContractSet dataContractSet = ((this.dataContractSet == null) ? null : new DataContractSet(this.dataContractSet));
			bool flag;
			try
			{
				foreach (Assembly assembly in assemblies)
				{
					if (assembly == null)
					{
						throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentException(SR.GetString("Cannot export null assembly.", new object[] { "assemblies" })));
					}
					Type[] types = assembly.GetTypes();
					for (int i = 0; i < types.Length; i++)
					{
						this.CheckAndAddType(types[i]);
					}
				}
				this.AddKnownTypes();
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
				this.TraceExportError(ex);
				throw;
			}
			return flag;
		}

		// Token: 0x06000F06 RID: 3846 RVA: 0x0003D73C File Offset: 0x0003B93C
		public bool CanExport(ICollection<Type> types)
		{
			if (types == null)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentNullException("types"));
			}
			DataContractSet dataContractSet = ((this.dataContractSet == null) ? null : new DataContractSet(this.dataContractSet));
			bool flag;
			try
			{
				foreach (Type type in types)
				{
					if (type == null)
					{
						throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentException(SR.GetString("Cannot export null type.", new object[] { "types" })));
					}
					this.AddType(type);
				}
				this.AddKnownTypes();
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
				this.TraceExportError(ex);
				throw;
			}
			return flag;
		}

		// Token: 0x06000F07 RID: 3847 RVA: 0x0003D828 File Offset: 0x0003BA28
		public bool CanExport(Type type)
		{
			if (type == null)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentNullException("type"));
			}
			DataContractSet dataContractSet = ((this.dataContractSet == null) ? null : new DataContractSet(this.dataContractSet));
			bool flag;
			try
			{
				this.AddType(type);
				this.AddKnownTypes();
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
				this.TraceExportError(ex);
				throw;
			}
			return flag;
		}

		// Token: 0x0400058F RID: 1423
		private ExportOptions options;

		// Token: 0x04000590 RID: 1424
		private XmlSchemaSet schemas;

		// Token: 0x04000591 RID: 1425
		private DataContractSet dataContractSet;
	}
}
