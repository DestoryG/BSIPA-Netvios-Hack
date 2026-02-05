using System;
using System.Collections.Generic;
using Mono.Cecil.Metadata;
using Mono.Collections.Generic;

namespace Mono.Cecil
{
	// Token: 0x02000179 RID: 377
	internal sealed class TypeDefinitionCollection : Collection<TypeDefinition>
	{
		// Token: 0x06000BBB RID: 3003 RVA: 0x00026F8F File Offset: 0x0002518F
		internal TypeDefinitionCollection(ModuleDefinition container)
		{
			this.container = container;
			this.name_cache = new Dictionary<Row<string, string>, TypeDefinition>(new RowEqualityComparer());
		}

		// Token: 0x06000BBC RID: 3004 RVA: 0x00026FAE File Offset: 0x000251AE
		internal TypeDefinitionCollection(ModuleDefinition container, int capacity)
			: base(capacity)
		{
			this.container = container;
			this.name_cache = new Dictionary<Row<string, string>, TypeDefinition>(capacity, new RowEqualityComparer());
		}

		// Token: 0x06000BBD RID: 3005 RVA: 0x00026FCF File Offset: 0x000251CF
		protected override void OnAdd(TypeDefinition item, int index)
		{
			this.Attach(item);
		}

		// Token: 0x06000BBE RID: 3006 RVA: 0x00026FCF File Offset: 0x000251CF
		protected override void OnSet(TypeDefinition item, int index)
		{
			this.Attach(item);
		}

		// Token: 0x06000BBF RID: 3007 RVA: 0x00026FCF File Offset: 0x000251CF
		protected override void OnInsert(TypeDefinition item, int index)
		{
			this.Attach(item);
		}

		// Token: 0x06000BC0 RID: 3008 RVA: 0x00026FD8 File Offset: 0x000251D8
		protected override void OnRemove(TypeDefinition item, int index)
		{
			this.Detach(item);
		}

		// Token: 0x06000BC1 RID: 3009 RVA: 0x00026FE4 File Offset: 0x000251E4
		protected override void OnClear()
		{
			foreach (TypeDefinition typeDefinition in this)
			{
				this.Detach(typeDefinition);
			}
		}

		// Token: 0x06000BC2 RID: 3010 RVA: 0x00027034 File Offset: 0x00025234
		private void Attach(TypeDefinition type)
		{
			if (type.Module != null && type.Module != this.container)
			{
				throw new ArgumentException("Type already attached");
			}
			type.module = this.container;
			type.scope = this.container;
			this.name_cache[new Row<string, string>(type.Namespace, type.Name)] = type;
		}

		// Token: 0x06000BC3 RID: 3011 RVA: 0x00027097 File Offset: 0x00025297
		private void Detach(TypeDefinition type)
		{
			type.module = null;
			type.scope = null;
			this.name_cache.Remove(new Row<string, string>(type.Namespace, type.Name));
		}

		// Token: 0x06000BC4 RID: 3012 RVA: 0x000270C4 File Offset: 0x000252C4
		public TypeDefinition GetType(string fullname)
		{
			string text;
			string text2;
			TypeParser.SplitFullName(fullname, out text, out text2);
			return this.GetType(text, text2);
		}

		// Token: 0x06000BC5 RID: 3013 RVA: 0x000270E4 File Offset: 0x000252E4
		public TypeDefinition GetType(string @namespace, string name)
		{
			TypeDefinition typeDefinition;
			if (this.name_cache.TryGetValue(new Row<string, string>(@namespace, name), out typeDefinition))
			{
				return typeDefinition;
			}
			return null;
		}

		// Token: 0x040004F1 RID: 1265
		private readonly ModuleDefinition container;

		// Token: 0x040004F2 RID: 1266
		private readonly Dictionary<Row<string, string>, TypeDefinition> name_cache;
	}
}
