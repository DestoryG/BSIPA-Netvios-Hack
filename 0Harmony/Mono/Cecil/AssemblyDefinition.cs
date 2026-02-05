using System;
using System.IO;
using System.Threading;
using Mono.Collections.Generic;

namespace Mono.Cecil
{
	// Token: 0x020000BB RID: 187
	internal sealed class AssemblyDefinition : ICustomAttributeProvider, IMetadataTokenProvider, ISecurityDeclarationProvider, IDisposable
	{
		// Token: 0x17000030 RID: 48
		// (get) Token: 0x06000423 RID: 1059 RVA: 0x0001319B File Offset: 0x0001139B
		// (set) Token: 0x06000424 RID: 1060 RVA: 0x000131A3 File Offset: 0x000113A3
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

		// Token: 0x17000031 RID: 49
		// (get) Token: 0x06000425 RID: 1061 RVA: 0x000131AC File Offset: 0x000113AC
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

		// Token: 0x17000032 RID: 50
		// (get) Token: 0x06000426 RID: 1062 RVA: 0x000131C7 File Offset: 0x000113C7
		// (set) Token: 0x06000427 RID: 1063 RVA: 0x00010C51 File Offset: 0x0000EE51
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

		// Token: 0x17000033 RID: 51
		// (get) Token: 0x06000428 RID: 1064 RVA: 0x000131D4 File Offset: 0x000113D4
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
				Interlocked.CompareExchange<Collection<ModuleDefinition>>(ref this.modules, new Collection<ModuleDefinition>(1) { this.main_module }, null);
				return this.modules;
			}
		}

		// Token: 0x17000034 RID: 52
		// (get) Token: 0x06000429 RID: 1065 RVA: 0x00013254 File Offset: 0x00011454
		public ModuleDefinition MainModule
		{
			get
			{
				return this.main_module;
			}
		}

		// Token: 0x17000035 RID: 53
		// (get) Token: 0x0600042A RID: 1066 RVA: 0x0001325C File Offset: 0x0001145C
		// (set) Token: 0x0600042B RID: 1067 RVA: 0x00013269 File Offset: 0x00011469
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

		// Token: 0x17000036 RID: 54
		// (get) Token: 0x0600042C RID: 1068 RVA: 0x00013277 File Offset: 0x00011477
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

		// Token: 0x17000037 RID: 55
		// (get) Token: 0x0600042D RID: 1069 RVA: 0x0001329C File Offset: 0x0001149C
		public Collection<CustomAttribute> CustomAttributes
		{
			get
			{
				return this.custom_attributes ?? this.GetCustomAttributes(ref this.custom_attributes, this.main_module);
			}
		}

		// Token: 0x17000038 RID: 56
		// (get) Token: 0x0600042E RID: 1070 RVA: 0x000132BA File Offset: 0x000114BA
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

		// Token: 0x17000039 RID: 57
		// (get) Token: 0x0600042F RID: 1071 RVA: 0x000132DF File Offset: 0x000114DF
		public Collection<SecurityDeclaration> SecurityDeclarations
		{
			get
			{
				return this.security_declarations ?? this.GetSecurityDeclarations(ref this.security_declarations, this.main_module);
			}
		}

		// Token: 0x06000430 RID: 1072 RVA: 0x00002AB9 File Offset: 0x00000CB9
		internal AssemblyDefinition()
		{
		}

		// Token: 0x06000431 RID: 1073 RVA: 0x00013300 File Offset: 0x00011500
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

		// Token: 0x06000432 RID: 1074 RVA: 0x00013345 File Offset: 0x00011545
		public static AssemblyDefinition CreateAssembly(AssemblyNameDefinition assemblyName, string moduleName, ModuleKind kind)
		{
			return AssemblyDefinition.CreateAssembly(assemblyName, moduleName, new ModuleParameters
			{
				Kind = kind
			});
		}

		// Token: 0x06000433 RID: 1075 RVA: 0x0001335C File Offset: 0x0001155C
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

		// Token: 0x06000434 RID: 1076 RVA: 0x000133B2 File Offset: 0x000115B2
		public static AssemblyDefinition ReadAssembly(string fileName)
		{
			return AssemblyDefinition.ReadAssembly(ModuleDefinition.ReadModule(fileName));
		}

		// Token: 0x06000435 RID: 1077 RVA: 0x000133BF File Offset: 0x000115BF
		public static AssemblyDefinition ReadAssembly(string fileName, ReaderParameters parameters)
		{
			return AssemblyDefinition.ReadAssembly(ModuleDefinition.ReadModule(fileName, parameters));
		}

		// Token: 0x06000436 RID: 1078 RVA: 0x000133CD File Offset: 0x000115CD
		public static AssemblyDefinition ReadAssembly(Stream stream)
		{
			return AssemblyDefinition.ReadAssembly(ModuleDefinition.ReadModule(stream));
		}

		// Token: 0x06000437 RID: 1079 RVA: 0x000133DA File Offset: 0x000115DA
		public static AssemblyDefinition ReadAssembly(Stream stream, ReaderParameters parameters)
		{
			return AssemblyDefinition.ReadAssembly(ModuleDefinition.ReadModule(stream, parameters));
		}

		// Token: 0x06000438 RID: 1080 RVA: 0x000133E8 File Offset: 0x000115E8
		private static AssemblyDefinition ReadAssembly(ModuleDefinition module)
		{
			AssemblyDefinition assembly = module.Assembly;
			if (assembly == null)
			{
				throw new ArgumentException();
			}
			return assembly;
		}

		// Token: 0x06000439 RID: 1081 RVA: 0x000133F9 File Offset: 0x000115F9
		public void Write(string fileName)
		{
			this.Write(fileName, new WriterParameters());
		}

		// Token: 0x0600043A RID: 1082 RVA: 0x00013407 File Offset: 0x00011607
		public void Write(string fileName, WriterParameters parameters)
		{
			this.main_module.Write(fileName, parameters);
		}

		// Token: 0x0600043B RID: 1083 RVA: 0x00013416 File Offset: 0x00011616
		public void Write()
		{
			this.main_module.Write();
		}

		// Token: 0x0600043C RID: 1084 RVA: 0x00013423 File Offset: 0x00011623
		public void Write(WriterParameters parameters)
		{
			this.main_module.Write(parameters);
		}

		// Token: 0x0600043D RID: 1085 RVA: 0x00013431 File Offset: 0x00011631
		public void Write(Stream stream)
		{
			this.Write(stream, new WriterParameters());
		}

		// Token: 0x0600043E RID: 1086 RVA: 0x0001343F File Offset: 0x0001163F
		public void Write(Stream stream, WriterParameters parameters)
		{
			this.main_module.Write(stream, parameters);
		}

		// Token: 0x0600043F RID: 1087 RVA: 0x0001344E File Offset: 0x0001164E
		public override string ToString()
		{
			return this.FullName;
		}

		// Token: 0x0400021C RID: 540
		private AssemblyNameDefinition name;

		// Token: 0x0400021D RID: 541
		internal ModuleDefinition main_module;

		// Token: 0x0400021E RID: 542
		private Collection<ModuleDefinition> modules;

		// Token: 0x0400021F RID: 543
		private Collection<CustomAttribute> custom_attributes;

		// Token: 0x04000220 RID: 544
		private Collection<SecurityDeclaration> security_declarations;
	}
}
