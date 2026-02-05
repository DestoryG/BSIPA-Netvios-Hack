using System;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Permissions;
using System.Xml.Serialization;

namespace System.Configuration
{
	// Token: 0x020000AD RID: 173
	public class SettingsPropertyValue
	{
		// Token: 0x170000F1 RID: 241
		// (get) Token: 0x060005EB RID: 1515 RVA: 0x000233A1 File Offset: 0x000215A1
		public string Name
		{
			get
			{
				return this._Property.Name;
			}
		}

		// Token: 0x170000F2 RID: 242
		// (get) Token: 0x060005EC RID: 1516 RVA: 0x000233AE File Offset: 0x000215AE
		// (set) Token: 0x060005ED RID: 1517 RVA: 0x000233B6 File Offset: 0x000215B6
		public bool IsDirty
		{
			get
			{
				return this._IsDirty;
			}
			set
			{
				this._IsDirty = value;
			}
		}

		// Token: 0x170000F3 RID: 243
		// (get) Token: 0x060005EE RID: 1518 RVA: 0x000233BF File Offset: 0x000215BF
		public SettingsProperty Property
		{
			get
			{
				return this._Property;
			}
		}

		// Token: 0x170000F4 RID: 244
		// (get) Token: 0x060005EF RID: 1519 RVA: 0x000233C7 File Offset: 0x000215C7
		public bool UsingDefaultValue
		{
			get
			{
				return this._UsingDefaultValue;
			}
		}

		// Token: 0x060005F0 RID: 1520 RVA: 0x000233CF File Offset: 0x000215CF
		public SettingsPropertyValue(SettingsProperty property)
		{
			this._Property = property;
		}

		// Token: 0x170000F5 RID: 245
		// (get) Token: 0x060005F1 RID: 1521 RVA: 0x000233E8 File Offset: 0x000215E8
		// (set) Token: 0x060005F2 RID: 1522 RVA: 0x0002345F File Offset: 0x0002165F
		public object PropertyValue
		{
			get
			{
				if (!this._Deserialized)
				{
					this._Value = this.Deserialize();
					this._Deserialized = true;
				}
				if (this._Value != null && !this.Property.PropertyType.IsPrimitive && !(this._Value is string) && !(this._Value is DateTime))
				{
					this._UsingDefaultValue = false;
					this._ChangedSinceLastSerialized = true;
					this._IsDirty = true;
				}
				return this._Value;
			}
			set
			{
				this._Value = value;
				this._IsDirty = true;
				this._ChangedSinceLastSerialized = true;
				this._Deserialized = true;
				this._UsingDefaultValue = false;
			}
		}

		// Token: 0x170000F6 RID: 246
		// (get) Token: 0x060005F3 RID: 1523 RVA: 0x00023484 File Offset: 0x00021684
		// (set) Token: 0x060005F4 RID: 1524 RVA: 0x000234A7 File Offset: 0x000216A7
		public object SerializedValue
		{
			[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
			get
			{
				if (this._ChangedSinceLastSerialized)
				{
					this._ChangedSinceLastSerialized = false;
					this._SerializedValue = this.SerializePropertyValue();
				}
				return this._SerializedValue;
			}
			[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
			set
			{
				this._UsingDefaultValue = false;
				this._SerializedValue = value;
			}
		}

		// Token: 0x170000F7 RID: 247
		// (get) Token: 0x060005F5 RID: 1525 RVA: 0x000234B7 File Offset: 0x000216B7
		// (set) Token: 0x060005F6 RID: 1526 RVA: 0x000234BF File Offset: 0x000216BF
		public bool Deserialized
		{
			get
			{
				return this._Deserialized;
			}
			set
			{
				this._Deserialized = value;
			}
		}

		// Token: 0x060005F7 RID: 1527 RVA: 0x000234C8 File Offset: 0x000216C8
		private bool IsHostedInAspnet()
		{
			return AppDomain.CurrentDomain.GetData(".appDomain") != null;
		}

		// Token: 0x060005F8 RID: 1528 RVA: 0x000234DC File Offset: 0x000216DC
		private object Deserialize()
		{
			object obj = null;
			if (this.SerializedValue != null)
			{
				try
				{
					if (this.SerializedValue is string)
					{
						obj = SettingsPropertyValue.GetObjectFromString(this.Property.PropertyType, this.Property.SerializeAs, (string)this.SerializedValue);
					}
					else
					{
						MemoryStream memoryStream = new MemoryStream((byte[])this.SerializedValue);
						try
						{
							obj = new BinaryFormatter().Deserialize(memoryStream);
						}
						finally
						{
							memoryStream.Close();
						}
					}
				}
				catch (Exception ex)
				{
					try
					{
						if (this.IsHostedInAspnet())
						{
							object[] array = new object[] { this.Property, this, ex };
							Type type = Type.GetType("System.Web.Management.WebBaseEvent, System.Web, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", true);
							type.InvokeMember("RaisePropertyDeserializationWebErrorEvent", BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.InvokeMethod, null, null, array, CultureInfo.InvariantCulture);
						}
					}
					catch
					{
					}
				}
				if (obj != null && !this.Property.PropertyType.IsAssignableFrom(obj.GetType()))
				{
					obj = null;
				}
			}
			if (obj == null)
			{
				this._UsingDefaultValue = true;
				if (this.Property.DefaultValue == null || this.Property.DefaultValue.ToString() == "[null]")
				{
					if (this.Property.PropertyType.IsValueType)
					{
						return SecurityUtils.SecureCreateInstance(this.Property.PropertyType);
					}
					return null;
				}
				else
				{
					if (!(this.Property.DefaultValue is string))
					{
						obj = this.Property.DefaultValue;
					}
					else
					{
						try
						{
							obj = SettingsPropertyValue.GetObjectFromString(this.Property.PropertyType, this.Property.SerializeAs, (string)this.Property.DefaultValue);
						}
						catch (Exception ex2)
						{
							throw new ArgumentException(SR.GetString("Could_not_create_from_default_value", new object[]
							{
								this.Property.Name,
								ex2.Message
							}));
						}
					}
					if (obj != null && !this.Property.PropertyType.IsAssignableFrom(obj.GetType()))
					{
						throw new ArgumentException(SR.GetString("Could_not_create_from_default_value_2", new object[] { this.Property.Name }));
					}
				}
			}
			if (obj == null)
			{
				if (this.Property.PropertyType == typeof(string))
				{
					obj = "";
				}
				else
				{
					try
					{
						obj = SecurityUtils.SecureCreateInstance(this.Property.PropertyType);
					}
					catch
					{
					}
				}
			}
			return obj;
		}

		// Token: 0x060005F9 RID: 1529 RVA: 0x00023758 File Offset: 0x00021958
		private static object GetObjectFromString(Type type, SettingsSerializeAs serializeAs, string attValue)
		{
			if (type == typeof(string) && (attValue == null || attValue.Length < 1 || serializeAs == SettingsSerializeAs.String))
			{
				return attValue;
			}
			if (attValue == null || attValue.Length < 1)
			{
				return null;
			}
			switch (serializeAs)
			{
			case SettingsSerializeAs.String:
			{
				TypeConverter converter = TypeDescriptor.GetConverter(type);
				if (converter != null && converter.CanConvertTo(typeof(string)) && converter.CanConvertFrom(typeof(string)))
				{
					return converter.ConvertFromInvariantString(attValue);
				}
				throw new ArgumentException(SR.GetString("Unable_to_convert_type_from_string", new object[] { type.ToString() }), "type");
			}
			case SettingsSerializeAs.Xml:
				break;
			case SettingsSerializeAs.Binary:
			{
				byte[] array = Convert.FromBase64String(attValue);
				MemoryStream memoryStream = null;
				try
				{
					memoryStream = new MemoryStream(array);
					return new BinaryFormatter().Deserialize(memoryStream);
				}
				finally
				{
					if (memoryStream != null)
					{
						memoryStream.Close();
					}
				}
				break;
			}
			default:
				return null;
			}
			StringReader stringReader = new StringReader(attValue);
			XmlSerializer xmlSerializer = new XmlSerializer(type);
			return xmlSerializer.Deserialize(stringReader);
		}

		// Token: 0x060005FA RID: 1530 RVA: 0x00023864 File Offset: 0x00021A64
		private object SerializePropertyValue()
		{
			if (this._Value == null)
			{
				return null;
			}
			if (this.Property.SerializeAs != SettingsSerializeAs.Binary)
			{
				return SettingsPropertyValue.ConvertObjectToString(this._Value, this.Property.PropertyType, this.Property.SerializeAs, this.Property.ThrowOnErrorSerializing);
			}
			MemoryStream memoryStream = new MemoryStream();
			object obj;
			try
			{
				BinaryFormatter binaryFormatter = new BinaryFormatter();
				binaryFormatter.Serialize(memoryStream, this._Value);
				obj = memoryStream.ToArray();
			}
			finally
			{
				memoryStream.Close();
			}
			return obj;
		}

		// Token: 0x060005FB RID: 1531 RVA: 0x000238F0 File Offset: 0x00021AF0
		private static string ConvertObjectToString(object propValue, Type type, SettingsSerializeAs serializeAs, bool throwOnError)
		{
			if (serializeAs == SettingsSerializeAs.ProviderSpecific)
			{
				if (type == typeof(string) || type.IsPrimitive)
				{
					serializeAs = SettingsSerializeAs.String;
				}
				else
				{
					serializeAs = SettingsSerializeAs.Xml;
				}
			}
			try
			{
				switch (serializeAs)
				{
				case SettingsSerializeAs.String:
				{
					TypeConverter converter = TypeDescriptor.GetConverter(type);
					if (converter != null && converter.CanConvertTo(typeof(string)) && converter.CanConvertFrom(typeof(string)))
					{
						return converter.ConvertToInvariantString(propValue);
					}
					throw new ArgumentException(SR.GetString("Unable_to_convert_type_to_string", new object[] { type.ToString() }), "type");
				}
				case SettingsSerializeAs.Xml:
					break;
				case SettingsSerializeAs.Binary:
				{
					MemoryStream memoryStream = new MemoryStream();
					try
					{
						BinaryFormatter binaryFormatter = new BinaryFormatter();
						binaryFormatter.Serialize(memoryStream, propValue);
						byte[] array = memoryStream.ToArray();
						return Convert.ToBase64String(array);
					}
					finally
					{
						memoryStream.Close();
					}
					break;
				}
				default:
					goto IL_00FC;
				}
				XmlSerializer xmlSerializer = new XmlSerializer(type);
				StringWriter stringWriter = new StringWriter(CultureInfo.InvariantCulture);
				xmlSerializer.Serialize(stringWriter, propValue);
				return stringWriter.ToString();
			}
			catch (Exception)
			{
				if (throwOnError)
				{
					throw;
				}
			}
			IL_00FC:
			return null;
		}

		// Token: 0x04000C54 RID: 3156
		private object _Value;

		// Token: 0x04000C55 RID: 3157
		private object _SerializedValue;

		// Token: 0x04000C56 RID: 3158
		private bool _Deserialized;

		// Token: 0x04000C57 RID: 3159
		private bool _IsDirty;

		// Token: 0x04000C58 RID: 3160
		private SettingsProperty _Property;

		// Token: 0x04000C59 RID: 3161
		private bool _ChangedSinceLastSerialized;

		// Token: 0x04000C5A RID: 3162
		private bool _UsingDefaultValue = true;
	}
}
