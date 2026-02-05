using System;
using System.IO;
using System.Reflection;
using Mono.Cecil;

namespace IPA.Injector
{
	// Token: 0x02000008 RID: 8
	internal class VirtualizedModule : IDisposable
	{
		// Token: 0x0600001C RID: 28 RVA: 0x00002FE4 File Offset: 0x000011E4
		public static VirtualizedModule Load(string engineFile)
		{
			return new VirtualizedModule(engineFile);
		}

		// Token: 0x0600001D RID: 29 RVA: 0x00002FEC File Offset: 0x000011EC
		private VirtualizedModule(string assemblyFile)
		{
			this.file = new FileInfo(assemblyFile);
			this.LoadModules();
		}

		// Token: 0x0600001E RID: 30 RVA: 0x00003006 File Offset: 0x00001206
		private void LoadModules()
		{
			this.module = ModuleDefinition.ReadModule(this.file.FullName, new ReaderParameters
			{
				ReadWrite = false,
				InMemory = true,
				ReadingMode = ReadingMode.Immediate
			});
		}

		// Token: 0x0600001F RID: 31 RVA: 0x00003038 File Offset: 0x00001238
		public void Virtualize(AssemblyName selfName, Action beforeChangeCallback = null)
		{
			bool changed = false;
			bool virtualize = true;
			foreach (AssemblyNameReference r in this.module.AssemblyReferences)
			{
				if (r.Name == selfName.Name)
				{
					virtualize = false;
					if (r.Version != selfName.Version)
					{
						r.Version = selfName.Version;
						changed = true;
					}
				}
			}
			if (virtualize)
			{
				changed = true;
				this.module.AssemblyReferences.Add(new AssemblyNameReference(selfName.Name, selfName.Version));
				foreach (TypeDefinition type in this.module.Types)
				{
					this.VirtualizeType(type);
				}
			}
			if (changed)
			{
				if (beforeChangeCallback != null)
				{
					beforeChangeCallback();
				}
				this.module.Write(this.file.FullName);
			}
		}

		// Token: 0x06000020 RID: 32 RVA: 0x00003158 File Offset: 0x00001358
		private void VirtualizeType(TypeDefinition type)
		{
			if (type.IsSealed)
			{
				type.IsSealed = false;
			}
			if (type.IsInterface)
			{
				return;
			}
			if (type.IsAbstract)
			{
				return;
			}
			if (type.Name == "SceneControl" || type.Name == "ConfigUI")
			{
				return;
			}
			foreach (TypeDefinition subType in type.NestedTypes)
			{
				this.VirtualizeType(subType);
			}
			foreach (MethodDefinition method in type.Methods)
			{
				if (method.IsManaged && method.IsIL && !method.IsStatic && !method.IsVirtual && !method.IsAbstract && !method.IsAddOn && !method.IsConstructor && !method.IsSpecialName && !method.IsGenericInstance && !method.HasOverrides)
				{
					method.IsVirtual = true;
					method.IsPublic = true;
					method.IsPrivate = false;
					method.IsNewSlot = true;
					method.IsHideBySig = true;
				}
			}
			foreach (FieldDefinition field in type.Fields)
			{
				if (field.IsPrivate)
				{
					field.IsFamily = true;
				}
			}
		}

		// Token: 0x06000021 RID: 33 RVA: 0x000032F0 File Offset: 0x000014F0
		protected virtual void Dispose(bool disposing)
		{
			if (!this.disposedValue)
			{
				if (disposing)
				{
					this.module.Dispose();
				}
				this.disposedValue = true;
			}
		}

		// Token: 0x06000022 RID: 34 RVA: 0x00003310 File Offset: 0x00001510
		~VirtualizedModule()
		{
			this.Dispose(false);
		}

		// Token: 0x06000023 RID: 35 RVA: 0x00003340 File Offset: 0x00001540
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x04000007 RID: 7
		private readonly FileInfo file;

		// Token: 0x04000008 RID: 8
		private ModuleDefinition module;

		// Token: 0x04000009 RID: 9
		private bool disposedValue;
	}
}
