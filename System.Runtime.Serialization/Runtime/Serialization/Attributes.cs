using System;
using System.Security;
using System.Xml;

namespace System.Runtime.Serialization
{
	// Token: 0x02000064 RID: 100
	internal class Attributes
	{
		// Token: 0x06000709 RID: 1801 RVA: 0x000207FC File Offset: 0x0001E9FC
		[SecuritySafeCritical]
		internal void Read(XmlReaderDelegator reader)
		{
			this.Reset();
			while (reader.MoveToNextAttribute())
			{
				switch (reader.IndexOfLocalName(Attributes.serializationLocalNames, DictionaryGlobals.SerializationNamespace))
				{
				case 0:
					this.ReadId(reader);
					break;
				case 1:
					this.ReadArraySize(reader);
					break;
				case 2:
					this.ReadRef(reader);
					break;
				case 3:
					this.ClrType = reader.Value;
					break;
				case 4:
					this.ClrAssembly = reader.Value;
					break;
				case 5:
					this.ReadFactoryType(reader);
					break;
				default:
				{
					int num = reader.IndexOfLocalName(Attributes.schemaInstanceLocalNames, DictionaryGlobals.SchemaInstanceNamespace);
					if (num != 0)
					{
						if (num != 1)
						{
							if (!reader.IsNamespaceUri(DictionaryGlobals.XmlnsNamespace))
							{
								this.UnrecognizedAttributesFound = true;
							}
						}
						else
						{
							this.ReadXsiType(reader);
						}
					}
					else
					{
						this.ReadXsiNil(reader);
					}
					break;
				}
				}
			}
			reader.MoveToElement();
		}

		// Token: 0x0600070A RID: 1802 RVA: 0x000208D8 File Offset: 0x0001EAD8
		internal void Reset()
		{
			this.Id = Globals.NewObjectId;
			this.Ref = Globals.NewObjectId;
			this.XsiTypeName = null;
			this.XsiTypeNamespace = null;
			this.XsiTypePrefix = null;
			this.XsiNil = false;
			this.ClrAssembly = null;
			this.ClrType = null;
			this.ArraySZSize = -1;
			this.FactoryTypeName = null;
			this.FactoryTypeNamespace = null;
			this.FactoryTypePrefix = null;
			this.UnrecognizedAttributesFound = false;
		}

		// Token: 0x0600070B RID: 1803 RVA: 0x00020948 File Offset: 0x0001EB48
		private void ReadId(XmlReaderDelegator reader)
		{
			this.Id = reader.ReadContentAsString();
			if (string.IsNullOrEmpty(this.Id))
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(XmlObjectSerializer.CreateSerializationException(SR.GetString("Invalid Id '{0}'. Must not be null or empty.", new object[] { this.Id })));
			}
		}

		// Token: 0x0600070C RID: 1804 RVA: 0x00020987 File Offset: 0x0001EB87
		private void ReadRef(XmlReaderDelegator reader)
		{
			this.Ref = reader.ReadContentAsString();
			if (string.IsNullOrEmpty(this.Ref))
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(XmlObjectSerializer.CreateSerializationException(SR.GetString("Invalid Ref '{0}'. Must not be null or empty.", new object[] { this.Ref })));
			}
		}

		// Token: 0x0600070D RID: 1805 RVA: 0x000209C6 File Offset: 0x0001EBC6
		private void ReadXsiNil(XmlReaderDelegator reader)
		{
			this.XsiNil = reader.ReadContentAsBoolean();
		}

		// Token: 0x0600070E RID: 1806 RVA: 0x000209D4 File Offset: 0x0001EBD4
		private void ReadArraySize(XmlReaderDelegator reader)
		{
			this.ArraySZSize = reader.ReadContentAsInt();
			if (this.ArraySZSize < 0)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(XmlObjectSerializer.CreateSerializationException(SR.GetString("Invalid Size '{0}'. Must be non-negative integer.", new object[] { this.ArraySZSize })));
			}
		}

		// Token: 0x0600070F RID: 1807 RVA: 0x00020A14 File Offset: 0x0001EC14
		private void ReadXsiType(XmlReaderDelegator reader)
		{
			string value = reader.Value;
			if (value != null && value.Length > 0)
			{
				XmlObjectSerializerReadContext.ParseQualifiedName(value, reader, out this.XsiTypeName, out this.XsiTypeNamespace, out this.XsiTypePrefix);
			}
		}

		// Token: 0x06000710 RID: 1808 RVA: 0x00020A50 File Offset: 0x0001EC50
		private void ReadFactoryType(XmlReaderDelegator reader)
		{
			string value = reader.Value;
			if (value != null && value.Length > 0)
			{
				XmlObjectSerializerReadContext.ParseQualifiedName(value, reader, out this.FactoryTypeName, out this.FactoryTypeNamespace, out this.FactoryTypePrefix);
			}
		}

		// Token: 0x040002CF RID: 719
		[SecurityCritical]
		private static XmlDictionaryString[] serializationLocalNames = new XmlDictionaryString[]
		{
			DictionaryGlobals.IdLocalName,
			DictionaryGlobals.ArraySizeLocalName,
			DictionaryGlobals.RefLocalName,
			DictionaryGlobals.ClrTypeLocalName,
			DictionaryGlobals.ClrAssemblyLocalName,
			DictionaryGlobals.ISerializableFactoryTypeLocalName
		};

		// Token: 0x040002D0 RID: 720
		[SecurityCritical]
		private static XmlDictionaryString[] schemaInstanceLocalNames = new XmlDictionaryString[]
		{
			DictionaryGlobals.XsiNilLocalName,
			DictionaryGlobals.XsiTypeLocalName
		};

		// Token: 0x040002D1 RID: 721
		internal string Id;

		// Token: 0x040002D2 RID: 722
		internal string Ref;

		// Token: 0x040002D3 RID: 723
		internal string XsiTypeName;

		// Token: 0x040002D4 RID: 724
		internal string XsiTypeNamespace;

		// Token: 0x040002D5 RID: 725
		internal string XsiTypePrefix;

		// Token: 0x040002D6 RID: 726
		internal bool XsiNil;

		// Token: 0x040002D7 RID: 727
		internal string ClrAssembly;

		// Token: 0x040002D8 RID: 728
		internal string ClrType;

		// Token: 0x040002D9 RID: 729
		internal int ArraySZSize;

		// Token: 0x040002DA RID: 730
		internal string FactoryTypeName;

		// Token: 0x040002DB RID: 731
		internal string FactoryTypeNamespace;

		// Token: 0x040002DC RID: 732
		internal string FactoryTypePrefix;

		// Token: 0x040002DD RID: 733
		internal bool UnrecognizedAttributesFound;
	}
}
