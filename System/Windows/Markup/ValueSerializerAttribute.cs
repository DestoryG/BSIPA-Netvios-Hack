using System;
using System.Runtime.CompilerServices;

namespace System.Windows.Markup
{
	// Token: 0x020003A3 RID: 931
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Enum | AttributeTargets.Method | AttributeTargets.Property | AttributeTargets.Interface, AllowMultiple = false, Inherited = true)]
	[TypeForwardedFrom("WindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35")]
	public sealed class ValueSerializerAttribute : Attribute
	{
		// Token: 0x060022A5 RID: 8869 RVA: 0x000A4E04 File Offset: 0x000A3004
		public ValueSerializerAttribute(Type valueSerializerType)
		{
			this._valueSerializerType = valueSerializerType;
		}

		// Token: 0x060022A6 RID: 8870 RVA: 0x000A4E13 File Offset: 0x000A3013
		public ValueSerializerAttribute(string valueSerializerTypeName)
		{
			this._valueSerializerTypeName = valueSerializerTypeName;
		}

		// Token: 0x170008CA RID: 2250
		// (get) Token: 0x060022A7 RID: 8871 RVA: 0x000A4E22 File Offset: 0x000A3022
		public Type ValueSerializerType
		{
			get
			{
				if (this._valueSerializerType == null && this._valueSerializerTypeName != null)
				{
					this._valueSerializerType = Type.GetType(this._valueSerializerTypeName);
				}
				return this._valueSerializerType;
			}
		}

		// Token: 0x170008CB RID: 2251
		// (get) Token: 0x060022A8 RID: 8872 RVA: 0x000A4E51 File Offset: 0x000A3051
		public string ValueSerializerTypeName
		{
			get
			{
				if (this._valueSerializerType != null)
				{
					return this._valueSerializerType.AssemblyQualifiedName;
				}
				return this._valueSerializerTypeName;
			}
		}

		// Token: 0x04001F91 RID: 8081
		private Type _valueSerializerType;

		// Token: 0x04001F92 RID: 8082
		private string _valueSerializerTypeName;
	}
}
