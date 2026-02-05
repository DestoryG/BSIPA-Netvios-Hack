using System;
using System.Collections.Generic;
using Mono.Cecil.Metadata;
using Mono.Collections.Generic;

namespace Mono.Cecil
{
	// Token: 0x020000BC RID: 188
	internal sealed class TypeDefinitionCollection : Collection<TypeDefinition>
	{
		// Token: 0x060007F0 RID: 2032 RVA: 0x00018279 File Offset: 0x00016479
		internal TypeDefinitionCollection(ModuleDefinition container)
		{
			this.container = container;
			this.name_cache = new Dictionary<Row<string, string>, TypeDefinition>(new RowEqualityComparer());
		}

		// Token: 0x060007F1 RID: 2033 RVA: 0x00018298 File Offset: 0x00016498
		internal TypeDefinitionCollection(ModuleDefinition container, int capacity)
			: base(capacity)
		{
			this.container = container;
			this.name_cache = new Dictionary<Row<string, string>, TypeDefinition>(capacity, new RowEqualityComparer());
		}

		// Token: 0x060007F2 RID: 2034 RVA: 0x000182B9 File Offset: 0x000164B9
		protected override void OnAdd(TypeDefinition item, int index)
		{
			this.Attach(item);
		}

		// Token: 0x060007F3 RID: 2035 RVA: 0x000182B9 File Offset: 0x000164B9
		protected override void OnSet(TypeDefinition item, int index)
		{
			this.Attach(item);
		}

		// Token: 0x060007F4 RID: 2036 RVA: 0x000182B9 File Offset: 0x000164B9
		protected override void OnInsert(TypeDefinition item, int index)
		{
			this.Attach(item);
		}

		// Token: 0x060007F5 RID: 2037 RVA: 0x000182C2 File Offset: 0x000164C2
		protected override void OnRemove(TypeDefinition item, int index)
		{
			this.Detach(item);
		}

		// Token: 0x060007F6 RID: 2038 RVA: 0x000182CC File Offset: 0x000164CC
		protected override void OnClear()
		{
			foreach (TypeDefinition typeDefinition in this)
			{
				this.Detach(typeDefinition);
			}
		}

		// Token: 0x060007F7 RID: 2039 RVA: 0x0001831C File Offset: 0x0001651C
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

		// Token: 0x060007F8 RID: 2040 RVA: 0x0001837F File Offset: 0x0001657F
		private void Detach(TypeDefinition type)
		{
			type.module = null;
			type.scope = null;
			this.name_cache.Remove(new Row<string, string>(type.Namespace, type.Name));
		}

		// Token: 0x060007F9 RID: 2041 RVA: 0x000183AC File Offset: 0x000165AC
		public TypeDefinition GetType(string fullname)
		{
			string text;
			string text2;
			TypeParser.SplitFullName(fullname, out text, out text2);
			return this.GetType(text, text2);
		}

		// Token: 0x060007FA RID: 2042 RVA: 0x000183CC File Offset: 0x000165CC
		public TypeDefinition GetType(string @namespace, string name)
		{
			TypeDefinition typeDefinition;
			if (this.name_cache.TryGetValue(new Row<string, string>(@namespace, name), out typeDefinition))
			{
				return typeDefinition;
			}
			return null;
		}

		// Token: 0x040002A8 RID: 680
		private readonly ModuleDefinition container;

		// Token: 0x040002A9 RID: 681
		private readonly Dictionary<Row<string, string>, TypeDefinition> name_cache;
	}
}
