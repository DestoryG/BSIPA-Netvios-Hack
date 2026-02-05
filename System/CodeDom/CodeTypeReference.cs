using System;
using System.Collections;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace System.CodeDom
{
	// Token: 0x02000665 RID: 1637
	[ClassInterface(ClassInterfaceType.AutoDispatch)]
	[ComVisible(true)]
	[Serializable]
	public class CodeTypeReference : CodeObject
	{
		// Token: 0x06003B40 RID: 15168 RVA: 0x000F5020 File Offset: 0x000F3220
		public CodeTypeReference()
		{
			this.baseType = string.Empty;
			this.arrayRank = 0;
			this.arrayElementType = null;
		}

		// Token: 0x06003B41 RID: 15169 RVA: 0x000F5044 File Offset: 0x000F3244
		public CodeTypeReference(Type type)
		{
			if (type == null)
			{
				throw new ArgumentNullException("type");
			}
			if (type.IsArray)
			{
				this.arrayRank = type.GetArrayRank();
				this.arrayElementType = new CodeTypeReference(type.GetElementType());
				this.baseType = null;
			}
			else
			{
				this.InitializeFromType(type);
				this.arrayRank = 0;
				this.arrayElementType = null;
			}
			this.isInterface = type.IsInterface;
		}

		// Token: 0x06003B42 RID: 15170 RVA: 0x000F50BA File Offset: 0x000F32BA
		public CodeTypeReference(Type type, CodeTypeReferenceOptions codeTypeReferenceOption)
			: this(type)
		{
			this.referenceOptions = codeTypeReferenceOption;
		}

		// Token: 0x06003B43 RID: 15171 RVA: 0x000F50CA File Offset: 0x000F32CA
		public CodeTypeReference(string typeName, CodeTypeReferenceOptions codeTypeReferenceOption)
		{
			this.Initialize(typeName, codeTypeReferenceOption);
		}

		// Token: 0x06003B44 RID: 15172 RVA: 0x000F50DA File Offset: 0x000F32DA
		public CodeTypeReference(string typeName)
		{
			this.Initialize(typeName);
		}

		// Token: 0x06003B45 RID: 15173 RVA: 0x000F50EC File Offset: 0x000F32EC
		private void InitializeFromType(Type type)
		{
			this.baseType = type.Name;
			if (!type.IsGenericParameter)
			{
				Type type2 = type;
				while (type2.IsNested)
				{
					type2 = type2.DeclaringType;
					this.baseType = type2.Name + "+" + this.baseType;
				}
				if (!string.IsNullOrEmpty(type.Namespace))
				{
					this.baseType = type.Namespace + "." + this.baseType;
				}
			}
			if (type.IsGenericType && !type.ContainsGenericParameters)
			{
				Type[] genericArguments = type.GetGenericArguments();
				for (int i = 0; i < genericArguments.Length; i++)
				{
					this.TypeArguments.Add(new CodeTypeReference(genericArguments[i]));
				}
				return;
			}
			if (!type.IsGenericTypeDefinition)
			{
				this.needsFixup = true;
			}
		}

		// Token: 0x06003B46 RID: 15174 RVA: 0x000F51AE File Offset: 0x000F33AE
		private void Initialize(string typeName)
		{
			this.Initialize(typeName, this.referenceOptions);
		}

		// Token: 0x06003B47 RID: 15175 RVA: 0x000F51C0 File Offset: 0x000F33C0
		private void Initialize(string typeName, CodeTypeReferenceOptions options)
		{
			this.Options = options;
			if (typeName == null || typeName.Length == 0)
			{
				typeName = typeof(void).FullName;
				this.baseType = typeName;
				this.arrayRank = 0;
				this.arrayElementType = null;
				return;
			}
			typeName = this.RipOffAssemblyInformationFromTypeName(typeName);
			int num = typeName.Length - 1;
			int i = num;
			this.needsFixup = true;
			Queue queue = new Queue();
			while (i >= 0)
			{
				int num2 = 1;
				if (typeName[i--] != ']')
				{
					break;
				}
				while (i >= 0 && typeName[i] == ',')
				{
					num2++;
					i--;
				}
				if (i < 0 || typeName[i] != '[')
				{
					break;
				}
				queue.Enqueue(num2);
				i--;
				num = i;
			}
			i = num;
			ArrayList arrayList = new ArrayList();
			Stack stack = new Stack();
			if (i > 0 && typeName[i--] == ']')
			{
				this.needsFixup = false;
				int num3 = 1;
				int num4 = num;
				while (i >= 0)
				{
					if (typeName[i] == '[')
					{
						if (--num3 == 0)
						{
							break;
						}
					}
					else if (typeName[i] == ']')
					{
						num3++;
					}
					else if (typeName[i] == ',' && num3 == 1)
					{
						if (i + 1 < num4)
						{
							stack.Push(typeName.Substring(i + 1, num4 - i - 1));
						}
						num4 = i;
					}
					i--;
				}
				if (i > 0 && num - i - 1 > 0)
				{
					if (i + 1 < num4)
					{
						stack.Push(typeName.Substring(i + 1, num4 - i - 1));
					}
					while (stack.Count > 0)
					{
						string text = this.RipOffAssemblyInformationFromTypeName((string)stack.Pop());
						arrayList.Add(new CodeTypeReference(text));
					}
					num = i - 1;
				}
			}
			if (num < 0)
			{
				this.baseType = typeName;
				return;
			}
			if (queue.Count > 0)
			{
				CodeTypeReference codeTypeReference = new CodeTypeReference(typeName.Substring(0, num + 1), this.Options);
				for (int j = 0; j < arrayList.Count; j++)
				{
					codeTypeReference.TypeArguments.Add((CodeTypeReference)arrayList[j]);
				}
				while (queue.Count > 1)
				{
					codeTypeReference = new CodeTypeReference(codeTypeReference, (int)queue.Dequeue());
				}
				this.baseType = null;
				this.arrayRank = (int)queue.Dequeue();
				this.arrayElementType = codeTypeReference;
			}
			else if (arrayList.Count > 0)
			{
				for (int k = 0; k < arrayList.Count; k++)
				{
					this.TypeArguments.Add((CodeTypeReference)arrayList[k]);
				}
				this.baseType = typeName.Substring(0, num + 1);
			}
			else
			{
				this.baseType = typeName;
			}
			if (this.baseType != null && this.baseType.IndexOf('`') != -1)
			{
				this.needsFixup = false;
			}
		}

		// Token: 0x06003B48 RID: 15176 RVA: 0x000F547D File Offset: 0x000F367D
		public CodeTypeReference(string typeName, params CodeTypeReference[] typeArguments)
			: this(typeName)
		{
			if (typeArguments != null && typeArguments.Length != 0)
			{
				this.TypeArguments.AddRange(typeArguments);
			}
		}

		// Token: 0x06003B49 RID: 15177 RVA: 0x000F5499 File Offset: 0x000F3699
		public CodeTypeReference(CodeTypeParameter typeParameter)
			: this((typeParameter == null) ? null : typeParameter.Name)
		{
			this.referenceOptions = CodeTypeReferenceOptions.GenericTypeParameter;
		}

		// Token: 0x06003B4A RID: 15178 RVA: 0x000F54B4 File Offset: 0x000F36B4
		public CodeTypeReference(string baseType, int rank)
		{
			this.baseType = null;
			this.arrayRank = rank;
			this.arrayElementType = new CodeTypeReference(baseType);
		}

		// Token: 0x06003B4B RID: 15179 RVA: 0x000F54D6 File Offset: 0x000F36D6
		public CodeTypeReference(CodeTypeReference arrayType, int rank)
		{
			this.baseType = null;
			this.arrayRank = rank;
			this.arrayElementType = arrayType;
		}

		// Token: 0x17000E46 RID: 3654
		// (get) Token: 0x06003B4C RID: 15180 RVA: 0x000F54F3 File Offset: 0x000F36F3
		// (set) Token: 0x06003B4D RID: 15181 RVA: 0x000F54FB File Offset: 0x000F36FB
		public CodeTypeReference ArrayElementType
		{
			get
			{
				return this.arrayElementType;
			}
			set
			{
				this.arrayElementType = value;
			}
		}

		// Token: 0x17000E47 RID: 3655
		// (get) Token: 0x06003B4E RID: 15182 RVA: 0x000F5504 File Offset: 0x000F3704
		// (set) Token: 0x06003B4F RID: 15183 RVA: 0x000F550C File Offset: 0x000F370C
		public int ArrayRank
		{
			get
			{
				return this.arrayRank;
			}
			set
			{
				this.arrayRank = value;
			}
		}

		// Token: 0x17000E48 RID: 3656
		// (get) Token: 0x06003B50 RID: 15184 RVA: 0x000F5515 File Offset: 0x000F3715
		internal int NestedArrayDepth
		{
			get
			{
				if (this.arrayElementType == null)
				{
					return 0;
				}
				return 1 + this.arrayElementType.NestedArrayDepth;
			}
		}

		// Token: 0x17000E49 RID: 3657
		// (get) Token: 0x06003B51 RID: 15185 RVA: 0x000F5530 File Offset: 0x000F3730
		// (set) Token: 0x06003B52 RID: 15186 RVA: 0x000F55AF File Offset: 0x000F37AF
		public string BaseType
		{
			get
			{
				if (this.arrayRank > 0 && this.arrayElementType != null)
				{
					return this.arrayElementType.BaseType;
				}
				if (string.IsNullOrEmpty(this.baseType))
				{
					return string.Empty;
				}
				string text = this.baseType;
				if (this.needsFixup && this.TypeArguments.Count > 0)
				{
					text = text + "`" + this.TypeArguments.Count.ToString(CultureInfo.InvariantCulture);
				}
				return text;
			}
			set
			{
				this.baseType = value;
				this.Initialize(this.baseType);
			}
		}

		// Token: 0x17000E4A RID: 3658
		// (get) Token: 0x06003B53 RID: 15187 RVA: 0x000F55C4 File Offset: 0x000F37C4
		// (set) Token: 0x06003B54 RID: 15188 RVA: 0x000F55CC File Offset: 0x000F37CC
		[ComVisible(false)]
		public CodeTypeReferenceOptions Options
		{
			get
			{
				return this.referenceOptions;
			}
			set
			{
				this.referenceOptions = value;
			}
		}

		// Token: 0x17000E4B RID: 3659
		// (get) Token: 0x06003B55 RID: 15189 RVA: 0x000F55D5 File Offset: 0x000F37D5
		[ComVisible(false)]
		public CodeTypeReferenceCollection TypeArguments
		{
			get
			{
				if (this.arrayRank > 0 && this.arrayElementType != null)
				{
					return this.arrayElementType.TypeArguments;
				}
				if (this.typeArguments == null)
				{
					this.typeArguments = new CodeTypeReferenceCollection();
				}
				return this.typeArguments;
			}
		}

		// Token: 0x17000E4C RID: 3660
		// (get) Token: 0x06003B56 RID: 15190 RVA: 0x000F560D File Offset: 0x000F380D
		internal bool IsInterface
		{
			get
			{
				return this.isInterface;
			}
		}

		// Token: 0x06003B57 RID: 15191 RVA: 0x000F5618 File Offset: 0x000F3818
		private string RipOffAssemblyInformationFromTypeName(string typeName)
		{
			int i = 0;
			int num = typeName.Length - 1;
			string text = typeName;
			while (i < typeName.Length)
			{
				if (!char.IsWhiteSpace(typeName[i]))
				{
					break;
				}
				i++;
			}
			while (num >= 0 && char.IsWhiteSpace(typeName[num]))
			{
				num--;
			}
			if (i < num)
			{
				if (typeName[i] == '[' && typeName[num] == ']')
				{
					i++;
					num--;
				}
				if (typeName[num] != ']')
				{
					int num2 = 0;
					for (int j = num; j >= i; j--)
					{
						if (typeName[j] == ',')
						{
							num2++;
							if (num2 == 4)
							{
								text = typeName.Substring(i, j - i);
								break;
							}
						}
					}
				}
			}
			return text;
		}

		// Token: 0x04002C39 RID: 11321
		private string baseType;

		// Token: 0x04002C3A RID: 11322
		[OptionalField]
		private bool isInterface;

		// Token: 0x04002C3B RID: 11323
		private int arrayRank;

		// Token: 0x04002C3C RID: 11324
		private CodeTypeReference arrayElementType;

		// Token: 0x04002C3D RID: 11325
		[OptionalField]
		private CodeTypeReferenceCollection typeArguments;

		// Token: 0x04002C3E RID: 11326
		[OptionalField]
		private CodeTypeReferenceOptions referenceOptions;

		// Token: 0x04002C3F RID: 11327
		[OptionalField]
		private bool needsFixup;
	}
}
