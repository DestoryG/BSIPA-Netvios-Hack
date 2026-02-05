using System;
using System.Configuration;

namespace System.Runtime.Serialization.Configuration
{
	// Token: 0x02000129 RID: 297
	public sealed class TypeElement : ConfigurationElement
	{
		// Token: 0x17000385 RID: 901
		// (get) Token: 0x060011EE RID: 4590 RVA: 0x0004B0A8 File Offset: 0x000492A8
		protected override ConfigurationPropertyCollection Properties
		{
			get
			{
				if (this.properties == null)
				{
					this.properties = new ConfigurationPropertyCollection
					{
						new ConfigurationProperty("", typeof(ParameterElementCollection), null, null, null, ConfigurationPropertyOptions.IsDefaultCollection),
						new ConfigurationProperty("type", typeof(string), string.Empty, null, new StringValidator(0, int.MaxValue, null), ConfigurationPropertyOptions.None),
						new ConfigurationProperty("index", typeof(int), 0, null, new IntegerValidator(0, int.MaxValue, false), ConfigurationPropertyOptions.None)
					};
				}
				return this.properties;
			}
		}

		// Token: 0x060011EF RID: 4591 RVA: 0x0004B14C File Offset: 0x0004934C
		public TypeElement()
		{
		}

		// Token: 0x060011F0 RID: 4592 RVA: 0x0004B178 File Offset: 0x00049378
		public TypeElement(string typeName)
			: this()
		{
			if (string.IsNullOrEmpty(typeName))
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperArgumentNull("typeName");
			}
			this.Type = typeName;
		}

		// Token: 0x17000386 RID: 902
		// (get) Token: 0x060011F1 RID: 4593 RVA: 0x0004B19A File Offset: 0x0004939A
		internal string Key
		{
			get
			{
				return this.key;
			}
		}

		// Token: 0x17000387 RID: 903
		// (get) Token: 0x060011F2 RID: 4594 RVA: 0x0004B1A2 File Offset: 0x000493A2
		[ConfigurationProperty("", DefaultValue = null, Options = ConfigurationPropertyOptions.IsDefaultCollection)]
		public ParameterElementCollection Parameters
		{
			get
			{
				return (ParameterElementCollection)base[""];
			}
		}

		// Token: 0x060011F3 RID: 4595 RVA: 0x0004B1B4 File Offset: 0x000493B4
		protected override void Reset(ConfigurationElement parentElement)
		{
			TypeElement typeElement = (TypeElement)parentElement;
			this.key = typeElement.key;
			base.Reset(parentElement);
		}

		// Token: 0x17000388 RID: 904
		// (get) Token: 0x060011F4 RID: 4596 RVA: 0x0004B1DB File Offset: 0x000493DB
		// (set) Token: 0x060011F5 RID: 4597 RVA: 0x0004B1ED File Offset: 0x000493ED
		[ConfigurationProperty("type", DefaultValue = "")]
		[StringValidator(MinLength = 0)]
		public string Type
		{
			get
			{
				return (string)base["type"];
			}
			set
			{
				base["type"] = value;
			}
		}

		// Token: 0x17000389 RID: 905
		// (get) Token: 0x060011F6 RID: 4598 RVA: 0x0004B1FB File Offset: 0x000493FB
		// (set) Token: 0x060011F7 RID: 4599 RVA: 0x0004B20D File Offset: 0x0004940D
		[ConfigurationProperty("index", DefaultValue = 0)]
		[IntegerValidator(MinValue = 0)]
		public int Index
		{
			get
			{
				return (int)base["index"];
			}
			set
			{
				base["index"] = value;
			}
		}

		// Token: 0x060011F8 RID: 4600 RVA: 0x0004B220 File Offset: 0x00049420
		internal Type GetType(string rootType, Type[] typeArgs)
		{
			return TypeElement.GetType(rootType, typeArgs, this.Type, this.Index, this.Parameters);
		}

		// Token: 0x060011F9 RID: 4601 RVA: 0x0004B23C File Offset: 0x0004943C
		internal static Type GetType(string rootType, Type[] typeArgs, string type, int index, ParameterElementCollection parameters)
		{
			if (!string.IsNullOrEmpty(type))
			{
				Type type2 = global::System.Type.GetType(type, true);
				if (type2.IsGenericTypeDefinition)
				{
					if (parameters.Count != type2.GetGenericArguments().Length)
					{
						throw DiagnosticUtility.ExceptionUtility.ThrowHelperArgument(SR.GetString("Generic parameter count do not match between known type and configuration. Type is '{0}', known type has {1} parameters, configuration has {2} parameters.", new object[]
						{
							type,
							type2.GetGenericArguments().Length,
							parameters.Count
						}));
					}
					Type[] array = new Type[parameters.Count];
					for (int i = 0; i < array.Length; i++)
					{
						array[i] = parameters[i].GetType(rootType, typeArgs);
					}
					type2 = type2.MakeGenericType(array);
				}
				return type2;
			}
			if (typeArgs != null && index < typeArgs.Length)
			{
				return typeArgs[index];
			}
			int num = ((typeArgs == null) ? 0 : typeArgs.Length);
			if (num == 0)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperArgument(SR.GetString("For known type configuration, index is out of bound. Root type: '{0}' has {1} type arguments, and index was {2}.", new object[] { rootType, num, index }));
			}
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperArgument(SR.GetString("For known type configuration, index is out of bound. Root type: '{0}' has {1} type arguments, and index was {2}.", new object[] { rootType, num, index }));
		}

		// Token: 0x040008A0 RID: 2208
		private ConfigurationPropertyCollection properties;

		// Token: 0x040008A1 RID: 2209
		private string key = Guid.NewGuid().ToString();
	}
}
