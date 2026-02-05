using System;
using System.IO;
using Mono.Collections.Generic;

namespace Mono.Cecil
{
	// Token: 0x0200000F RID: 15
	public sealed class AssemblyDefinition : ICustomAttributeProvider, IMetadataTokenProvider, ISecurityDeclarationProvider, IDisposable
	{
		// Token: 0x1700001A RID: 26
		// (get) Token: 0x060000BD RID: 189 RVA: 0x00004DDF File Offset: 0x00002FDF
		// (set) Token: 0x060000BE RID: 190 RVA: 0x00004DE7 File Offset: 0x00002FE7
		public AssemblyNameDefinition Name
		{
			get
			{
				return this.name;
			}
			set
			{
				this.name = value;
			}
		}

		// Token: 0x1700001B RID: 27
		// (get) Token: 0x060000BF RID: 191 RVA: 0x00004DF0 File Offset: 0x00002FF0
		public string FullName
		{
			get
			{
				if (this.name == null)
				{
					return string.Empty;
				}
				return this.name.FullName;
			}
		}

		// Token: 0x1700001C RID: 28
		// (get) Token: 0x060000C0 RID: 192 RVA: 0x00004E0B File Offset: 0x0000300B
		// (set) Token: 0x060000C1 RID: 193 RVA: 0x00002A0D File Offset: 0x00000C0D
		public MetadataToken MetadataToken
		{
			get
			{
				return new MetadataToken(TokenType.Assembly, 1);
			}
			set
			{
			}
		}

		// Token: 0x1700001D RID: 29
		// (get) Token: 0x060000C2 RID: 194 RVA: 0x00004E18 File Offset: 0x00003018
		public Collection<ModuleDefinition> Modules
		{
			get
			{
				if (this.modules != null)
				{
					return this.modules;
				}
				if (this.main_module.HasImage)
				{
					return this.main_module.Read<AssemblyDefinition, Collection<ModuleDefinition>>(ref this.modules, this, (AssemblyDefinition _, MetadataReader reader) => reader.ReadModules());
				}
				Collection<ModuleDefinition> collection = new Collection<ModuleDefinition>(1);
				collection.Add(this.main_module);
				Collection<ModuleDefinition> collection2 = collection;
				this.modules = collection;
				return collection2;
			}
		}

		// Token: 0x1700001E RID: 30
		// (get) Token: 0x060000C3 RID: 195 RVA: 0x00004E8E File Offset: 0x0000308E
		public ModuleDefinition MainModule
		{
			get
			{
				return this.main_module;
			}
		}

		// Token: 0x1700001F RID: 31
		// (get) Token: 0x060000C4 RID: 196 RVA: 0x00004E96 File Offset: 0x00003096
		// (set) Token: 0x060000C5 RID: 197 RVA: 0x00004EA3 File Offset: 0x000030A3
		public MethodDefinition EntryPoint
		{
			get
			{
				return this.main_module.EntryPoint;
			}
			set
			{
				this.main_module.EntryPoint = value;
			}
		}

		// Token: 0x17000020 RID: 32
		// (get) Token: 0x060000C6 RID: 198 RVA: 0x00004EB1 File Offset: 0x000030B1
		public bool HasCustomAttributes
		{
			get
			{
				if (this.custom_attributes != null)
				{
					return this.custom_attributes.Count > 0;
				}
				return this.GetHasCustomAttributes(this.main_module);
			}
		}

		// Token: 0x17000021 RID: 33
		// (get) Token: 0x060000C7 RID: 199 RVA: 0x00004ED6 File Offset: 0x000030D6
		public Collection<CustomAttribute> CustomAttributes
		{
			get
			{
				return this.custom_attributes ?? this.GetCustomAttributes(ref this.custom_attributes, this.main_module);
			}
		}

		// Token: 0x17000022 RID: 34
		// (get) Token: 0x060000C8 RID: 200 RVA: 0x00004EF4 File Offset: 0x000030F4
		public bool HasSecurityDeclarations
		{
			get
			{
				if (this.security_declarations != null)
				{
					return this.security_declarations.Count > 0;
				}
				return this.GetHasSecurityDeclarations(this.main_module);
			}
		}

		// Token: 0x17000023 RID: 35
		// (get) Token: 0x060000C9 RID: 201 RVA: 0x00004F19 File Offset: 0x00003119
		public Collection<SecurityDeclaration> SecurityDeclarations
		{
			get
			{
				return this.security_declarations ?? this.GetSecurityDeclarations(ref this.security_declarations, this.main_module);
			}
		}

		// Token: 0x060000CA RID: 202 RVA: 0x00004F37 File Offset: 0x00003137
		internal AssemblyDefinition()
		{
		}

		// Token: 0x060000CB RID: 203 RVA: 0x00004F40 File Offset: 0x00003140
		public void Dispose()
		{
			if (this.modules == null)
			{
				this.main_module.Dispose();
				return;
			}
			Collection<ModuleDefinition> collection = this.Modules;
			for (int i = 0; i < collection.Count; i++)
			{
				collection[i].Dispose();
			}
		}

		// Token: 0x060000CC RID: 204 RVA: 0x00004F85 File Offset: 0x00003185
		public static AssemblyDefinition CreateAssembly(AssemblyNameDefinition assemblyName, string moduleName, ModuleKind kind)
		{
			return AssemblyDefinition.CreateAssembly(assemblyName, moduleName, new ModuleParameters
			{
				Kind = kind
			});
		}

		// Token: 0x060000CD RID: 205 RVA: 0x00004F9C File Offset: 0x0000319C
		public static AssemblyDefinition CreateAssembly(AssemblyNameDefinition assemblyName, string moduleName, ModuleParameters parameters)
		{
			if (assemblyName == null)
			{
				throw new ArgumentNullException("assemblyName");
			}
			if (moduleName == null)
			{
				throw new ArgumentNullException("moduleName");
			}
			Mixin.CheckParameters(parameters);
			if (parameters.Kind == ModuleKind.NetModule)
			{
				throw new ArgumentException("kind");
			}
			AssemblyDefinition assembly = ModuleDefinition.CreateModule(moduleName, parameters).Assembly;
			assembly.Name = assemblyName;
			return assembly;
		}

		// Token: 0x060000CE RID: 206 RVA: 0x00004FF2 File Offset: 0x000031F2
		public static AssemblyDefinition ReadAssembly(string fileName)
		{
			return AssemblyDefinition.ReadAssembly(ModuleDefinition.ReadModule(fileName));
		}

		// Token: 0x060000CF RID: 207 RVA: 0x00004FFF File Offset: 0x000031FF
		public static AssemblyDefinition ReadAssembly(string fileName, ReaderParameters parameters)
		{
			return AssemblyDefinition.ReadAssembly(ModuleDefinition.ReadModule(fileName, parameters));
		}

		// Token: 0x060000D0 RID: 208 RVA: 0x0000500D File Offset: 0x0000320D
		public static AssemblyDefinition ReadAssembly(Stream stream)
		{
			return AssemblyDefinition.ReadAssembly(ModuleDefinition.ReadModule(stream));
		}

		// Token: 0x060000D1 RID: 209 RVA: 0x0000501A File Offset: 0x0000321A
		public static AssemblyDefinition ReadAssembly(Stream stream, ReaderParameters parameters)
		{
			return AssemblyDefinition.ReadAssembly(ModuleDefinition.ReadModule(stream, parameters));
		}

		// Token: 0x060000D2 RID: 210 RVA: 0x00005028 File Offset: 0x00003228
		private static AssemblyDefinition ReadAssembly(ModuleDefinition module)
		{
			AssemblyDefinition assembly = module.Assembly;
			if (assembly == null)
			{
				throw new ArgumentException();
			}
			return assembly;
		}

		// Token: 0x060000D3 RID: 211 RVA: 0x00005039 File Offset: 0x00003239
		public void Write(string fileName)
		{
			this.Write(fileName, new WriterParameters());
		}

		// Token: 0x060000D4 RID: 212 RVA: 0x00005047 File Offset: 0x00003247
		public void Write(string fileName, WriterParameters parameters)
		{
			this.main_module.Write(fileName, parameters);
		}

		// Token: 0x060000D5 RID: 213 RVA: 0x00005056 File Offset: 0x00003256
		public void Write()
		{
			this.main_module.Write();
		}

		// Token: 0x060000D6 RID: 214 RVA: 0x00005063 File Offset: 0x00003263
		public void Write(WriterParameters parameters)
		{
			this.main_module.Write(parameters);
		}

		// Token: 0x060000D7 RID: 215 RVA: 0x00005071 File Offset: 0x00003271
		public void Write(Stream stream)
		{
			this.Write(stream, new WriterParameters());
		}

		// Token: 0x060000D8 RID: 216 RVA: 0x0000507F File Offset: 0x0000327F
		public void Write(Stream stream, WriterParameters parameters)
		{
			this.main_module.Write(stream, parameters);
		}

		// Token: 0x060000D9 RID: 217 RVA: 0x0000508E File Offset: 0x0000328E
		public override string ToString()
		{
			return this.FullName;
		}

		// Token: 0x0400001B RID: 27
		private AssemblyNameDefinition name;

		// Token: 0x0400001C RID: 28
		internal ModuleDefinition main_module;

		// Token: 0x0400001D RID: 29
		private Collection<ModuleDefinition> modules;

		// Token: 0x0400001E RID: 30
		private Collection<CustomAttribute> custom_attributes;

		// Token: 0x0400001F RID: 31
		private Collection<SecurityDeclaration> security_declarations;
	}
}
