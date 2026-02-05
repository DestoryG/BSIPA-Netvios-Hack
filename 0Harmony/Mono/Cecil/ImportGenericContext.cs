using System;
using Mono.Collections.Generic;

namespace Mono.Cecil
{
	// Token: 0x0200012A RID: 298
	internal struct ImportGenericContext
	{
		// Token: 0x1700011D RID: 285
		// (get) Token: 0x06000820 RID: 2080 RVA: 0x00020767 File Offset: 0x0001E967
		public bool IsEmpty
		{
			get
			{
				return this.stack == null;
			}
		}

		// Token: 0x06000821 RID: 2081 RVA: 0x00020772 File Offset: 0x0001E972
		public ImportGenericContext(IGenericParameterProvider provider)
		{
			if (provider == null)
			{
				throw new ArgumentNullException("provider");
			}
			this.stack = null;
			this.Push(provider);
		}

		// Token: 0x06000822 RID: 2082 RVA: 0x00020790 File Offset: 0x0001E990
		public void Push(IGenericParameterProvider provider)
		{
			if (this.stack == null)
			{
				this.stack = new Collection<IGenericParameterProvider>(1) { provider };
				return;
			}
			this.stack.Add(provider);
		}

		// Token: 0x06000823 RID: 2083 RVA: 0x000207BA File Offset: 0x0001E9BA
		public void Pop()
		{
			this.stack.RemoveAt(this.stack.Count - 1);
		}

		// Token: 0x06000824 RID: 2084 RVA: 0x000207D4 File Offset: 0x0001E9D4
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

		// Token: 0x06000825 RID: 2085 RVA: 0x0002082F File Offset: 0x0001EA2F
		public string NormalizeMethodName(MethodReference method)
		{
			return method.DeclaringType.GetElementType().FullName + "." + method.Name;
		}

		// Token: 0x06000826 RID: 2086 RVA: 0x00020854 File Offset: 0x0001EA54
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

		// Token: 0x06000827 RID: 2087 RVA: 0x000208AC File Offset: 0x0001EAAC
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

		// Token: 0x06000828 RID: 2088 RVA: 0x000208E8 File Offset: 0x0001EAE8
		public static ImportGenericContext For(IGenericParameterProvider context)
		{
			if (context == null)
			{
				return default(ImportGenericContext);
			}
			return new ImportGenericContext(context);
		}

		// Token: 0x040002F9 RID: 761
		private Collection<IGenericParameterProvider> stack;
	}
}
