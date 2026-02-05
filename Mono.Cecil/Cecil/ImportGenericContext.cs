using System;
using Mono.Collections.Generic;

namespace Mono.Cecil
{
	// Token: 0x02000074 RID: 116
	internal struct ImportGenericContext
	{
		// Token: 0x17000103 RID: 259
		// (get) Token: 0x0600048E RID: 1166 RVA: 0x00011F49 File Offset: 0x00010149
		public bool IsEmpty
		{
			get
			{
				return this.stack == null;
			}
		}

		// Token: 0x0600048F RID: 1167 RVA: 0x00011F54 File Offset: 0x00010154
		public ImportGenericContext(IGenericParameterProvider provider)
		{
			if (provider == null)
			{
				throw new ArgumentNullException("provider");
			}
			this.stack = null;
			this.Push(provider);
		}

		// Token: 0x06000490 RID: 1168 RVA: 0x00011F72 File Offset: 0x00010172
		public void Push(IGenericParameterProvider provider)
		{
			if (this.stack == null)
			{
				this.stack = new Collection<IGenericParameterProvider>(1) { provider };
				return;
			}
			this.stack.Add(provider);
		}

		// Token: 0x06000491 RID: 1169 RVA: 0x00011F9C File Offset: 0x0001019C
		public void Pop()
		{
			this.stack.RemoveAt(this.stack.Count - 1);
		}

		// Token: 0x06000492 RID: 1170 RVA: 0x00011FB8 File Offset: 0x000101B8
		public TypeReference MethodParameter(string method, int position)
		{
			for (int i = this.stack.Count - 1; i >= 0; i--)
			{
				MethodReference methodReference = this.stack[i] as MethodReference;
				if (methodReference != null && !(method != this.NormalizeMethodName(methodReference)))
				{
					return methodReference.GenericParameters[position];
				}
			}
			throw new InvalidOperationException();
		}

		// Token: 0x06000493 RID: 1171 RVA: 0x00012013 File Offset: 0x00010213
		public string NormalizeMethodName(MethodReference method)
		{
			return method.DeclaringType.GetElementType().FullName + "." + method.Name;
		}

		// Token: 0x06000494 RID: 1172 RVA: 0x00012038 File Offset: 0x00010238
		public TypeReference TypeParameter(string type, int position)
		{
			for (int i = this.stack.Count - 1; i >= 0; i--)
			{
				TypeReference typeReference = ImportGenericContext.GenericTypeFor(this.stack[i]);
				if (!(typeReference.FullName != type))
				{
					return typeReference.GenericParameters[position];
				}
			}
			throw new InvalidOperationException();
		}

		// Token: 0x06000495 RID: 1173 RVA: 0x00012090 File Offset: 0x00010290
		private static TypeReference GenericTypeFor(IGenericParameterProvider context)
		{
			TypeReference typeReference = context as TypeReference;
			if (typeReference != null)
			{
				return typeReference.GetElementType();
			}
			MethodReference methodReference = context as MethodReference;
			if (methodReference != null)
			{
				return methodReference.DeclaringType.GetElementType();
			}
			throw new InvalidOperationException();
		}

		// Token: 0x06000496 RID: 1174 RVA: 0x000120CC File Offset: 0x000102CC
		public static ImportGenericContext For(IGenericParameterProvider context)
		{
			if (context == null)
			{
				return default(ImportGenericContext);
			}
			return new ImportGenericContext(context);
		}

		// Token: 0x040000E2 RID: 226
		private Collection<IGenericParameterProvider> stack;
	}
}
