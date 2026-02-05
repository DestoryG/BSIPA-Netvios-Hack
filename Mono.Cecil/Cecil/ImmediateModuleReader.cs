using System;
using Mono.Cecil.Cil;
using Mono.Cecil.PE;
using Mono.Collections.Generic;

namespace Mono.Cecil
{
	// Token: 0x02000016 RID: 22
	internal sealed class ImmediateModuleReader : ModuleReader
	{
		// Token: 0x0600010C RID: 268 RVA: 0x00005870 File Offset: 0x00003A70
		public ImmediateModuleReader(Image image)
			: base(image, ReadingMode.Immediate)
		{
		}

		// Token: 0x0600010D RID: 269 RVA: 0x0000587A File Offset: 0x00003A7A
		protected override void ReadModule()
		{
			this.module.Read<ModuleDefinition>(this.module, delegate(ModuleDefinition module, MetadataReader reader)
			{
				base.ReadModuleManifest(reader);
				this.ReadModule(module, true);
			});
		}

		// Token: 0x0600010E RID: 270 RVA: 0x0000589C File Offset: 0x00003A9C
		public void ReadModule(ModuleDefinition module, bool resolve_attributes)
		{
			this.resolve_attributes = resolve_attributes;
			if (module.HasAssemblyReferences)
			{
				Mixin.Read(module.AssemblyReferences);
			}
			if (module.HasResources)
			{
				Mixin.Read(module.Resources);
			}
			if (module.HasModuleReferences)
			{
				Mixin.Read(module.ModuleReferences);
			}
			if (module.HasTypes)
			{
				this.ReadTypes(module.Types);
			}
			if (module.HasExportedTypes)
			{
				Mixin.Read(module.ExportedTypes);
			}
			this.ReadCustomAttributes(module);
			AssemblyDefinition assembly = module.Assembly;
			if (assembly == null)
			{
				return;
			}
			this.ReadCustomAttributes(assembly);
			this.ReadSecurityDeclarations(assembly);
		}

		// Token: 0x0600010F RID: 271 RVA: 0x00005930 File Offset: 0x00003B30
		private void ReadTypes(Collection<TypeDefinition> types)
		{
			for (int i = 0; i < types.Count; i++)
			{
				this.ReadType(types[i]);
			}
		}

		// Token: 0x06000110 RID: 272 RVA: 0x0000595C File Offset: 0x00003B5C
		private void ReadType(TypeDefinition type)
		{
			this.ReadGenericParameters(type);
			if (type.HasInterfaces)
			{
				this.ReadInterfaces(type);
			}
			if (type.HasNestedTypes)
			{
				this.ReadTypes(type.NestedTypes);
			}
			if (type.HasLayoutInfo)
			{
				Mixin.Read(type.ClassSize);
			}
			if (type.HasFields)
			{
				this.ReadFields(type);
			}
			if (type.HasMethods)
			{
				this.ReadMethods(type);
			}
			if (type.HasProperties)
			{
				this.ReadProperties(type);
			}
			if (type.HasEvents)
			{
				this.ReadEvents(type);
			}
			this.ReadSecurityDeclarations(type);
			this.ReadCustomAttributes(type);
		}

		// Token: 0x06000111 RID: 273 RVA: 0x000059F8 File Offset: 0x00003BF8
		private void ReadInterfaces(TypeDefinition type)
		{
			Collection<InterfaceImplementation> interfaces = type.Interfaces;
			for (int i = 0; i < interfaces.Count; i++)
			{
				this.ReadCustomAttributes(interfaces[i]);
			}
		}

		// Token: 0x06000112 RID: 274 RVA: 0x00005A2C File Offset: 0x00003C2C
		private void ReadGenericParameters(IGenericParameterProvider provider)
		{
			if (!provider.HasGenericParameters)
			{
				return;
			}
			Collection<GenericParameter> genericParameters = provider.GenericParameters;
			for (int i = 0; i < genericParameters.Count; i++)
			{
				GenericParameter genericParameter = genericParameters[i];
				if (genericParameter.HasConstraints)
				{
					Mixin.Read(genericParameter.Constraints);
				}
				this.ReadCustomAttributes(genericParameter);
			}
		}

		// Token: 0x06000113 RID: 275 RVA: 0x00005A7C File Offset: 0x00003C7C
		private void ReadSecurityDeclarations(ISecurityDeclarationProvider provider)
		{
			if (!provider.HasSecurityDeclarations)
			{
				return;
			}
			Collection<SecurityDeclaration> securityDeclarations = provider.SecurityDeclarations;
			if (!this.resolve_attributes)
			{
				return;
			}
			for (int i = 0; i < securityDeclarations.Count; i++)
			{
				Mixin.Read(securityDeclarations[i].SecurityAttributes);
			}
		}

		// Token: 0x06000114 RID: 276 RVA: 0x00005AC4 File Offset: 0x00003CC4
		private void ReadCustomAttributes(ICustomAttributeProvider provider)
		{
			if (!provider.HasCustomAttributes)
			{
				return;
			}
			Collection<CustomAttribute> customAttributes = provider.CustomAttributes;
			if (!this.resolve_attributes)
			{
				return;
			}
			for (int i = 0; i < customAttributes.Count; i++)
			{
				Mixin.Read(customAttributes[i].ConstructorArguments);
			}
		}

		// Token: 0x06000115 RID: 277 RVA: 0x00005B0C File Offset: 0x00003D0C
		private void ReadFields(TypeDefinition type)
		{
			Collection<FieldDefinition> fields = type.Fields;
			for (int i = 0; i < fields.Count; i++)
			{
				FieldDefinition fieldDefinition = fields[i];
				if (fieldDefinition.HasConstant)
				{
					Mixin.Read(fieldDefinition.Constant);
				}
				if (fieldDefinition.HasLayoutInfo)
				{
					Mixin.Read(fieldDefinition.Offset);
				}
				if (fieldDefinition.RVA > 0)
				{
					Mixin.Read(fieldDefinition.InitialValue);
				}
				if (fieldDefinition.HasMarshalInfo)
				{
					Mixin.Read(fieldDefinition.MarshalInfo);
				}
				this.ReadCustomAttributes(fieldDefinition);
			}
		}

		// Token: 0x06000116 RID: 278 RVA: 0x00005B94 File Offset: 0x00003D94
		private void ReadMethods(TypeDefinition type)
		{
			Collection<MethodDefinition> methods = type.Methods;
			for (int i = 0; i < methods.Count; i++)
			{
				MethodDefinition methodDefinition = methods[i];
				this.ReadGenericParameters(methodDefinition);
				if (methodDefinition.HasParameters)
				{
					this.ReadParameters(methodDefinition);
				}
				if (methodDefinition.HasOverrides)
				{
					Mixin.Read(methodDefinition.Overrides);
				}
				if (methodDefinition.IsPInvokeImpl)
				{
					Mixin.Read(methodDefinition.PInvokeInfo);
				}
				this.ReadSecurityDeclarations(methodDefinition);
				this.ReadCustomAttributes(methodDefinition);
				MethodReturnType methodReturnType = methodDefinition.MethodReturnType;
				if (methodReturnType.HasConstant)
				{
					Mixin.Read(methodReturnType.Constant);
				}
				if (methodReturnType.HasMarshalInfo)
				{
					Mixin.Read(methodReturnType.MarshalInfo);
				}
				this.ReadCustomAttributes(methodReturnType);
			}
		}

		// Token: 0x06000117 RID: 279 RVA: 0x00005C48 File Offset: 0x00003E48
		private void ReadParameters(MethodDefinition method)
		{
			Collection<ParameterDefinition> parameters = method.Parameters;
			for (int i = 0; i < parameters.Count; i++)
			{
				ParameterDefinition parameterDefinition = parameters[i];
				if (parameterDefinition.HasConstant)
				{
					Mixin.Read(parameterDefinition.Constant);
				}
				if (parameterDefinition.HasMarshalInfo)
				{
					Mixin.Read(parameterDefinition.MarshalInfo);
				}
				this.ReadCustomAttributes(parameterDefinition);
			}
		}

		// Token: 0x06000118 RID: 280 RVA: 0x00005CA4 File Offset: 0x00003EA4
		private void ReadProperties(TypeDefinition type)
		{
			Collection<PropertyDefinition> properties = type.Properties;
			for (int i = 0; i < properties.Count; i++)
			{
				PropertyDefinition propertyDefinition = properties[i];
				Mixin.Read(propertyDefinition.GetMethod);
				if (propertyDefinition.HasConstant)
				{
					Mixin.Read(propertyDefinition.Constant);
				}
				this.ReadCustomAttributes(propertyDefinition);
			}
		}

		// Token: 0x06000119 RID: 281 RVA: 0x00005CF8 File Offset: 0x00003EF8
		private void ReadEvents(TypeDefinition type)
		{
			Collection<EventDefinition> events = type.Events;
			for (int i = 0; i < events.Count; i++)
			{
				EventDefinition eventDefinition = events[i];
				Mixin.Read(eventDefinition.AddMethod);
				this.ReadCustomAttributes(eventDefinition);
			}
		}

		// Token: 0x0600011A RID: 282 RVA: 0x00005D37 File Offset: 0x00003F37
		public override void ReadSymbols(ModuleDefinition module)
		{
			if (module.symbol_reader == null)
			{
				return;
			}
			this.ReadTypesSymbols(module.Types, module.symbol_reader);
		}

		// Token: 0x0600011B RID: 283 RVA: 0x00005D54 File Offset: 0x00003F54
		private void ReadTypesSymbols(Collection<TypeDefinition> types, ISymbolReader symbol_reader)
		{
			for (int i = 0; i < types.Count; i++)
			{
				TypeDefinition typeDefinition = types[i];
				if (typeDefinition.HasNestedTypes)
				{
					this.ReadTypesSymbols(typeDefinition.NestedTypes, symbol_reader);
				}
				if (typeDefinition.HasMethods)
				{
					this.ReadMethodsSymbols(typeDefinition, symbol_reader);
				}
			}
		}

		// Token: 0x0600011C RID: 284 RVA: 0x00005DA0 File Offset: 0x00003FA0
		private void ReadMethodsSymbols(TypeDefinition type, ISymbolReader symbol_reader)
		{
			Collection<MethodDefinition> methods = type.Methods;
			for (int i = 0; i < methods.Count; i++)
			{
				MethodDefinition methodDefinition = methods[i];
				if (methodDefinition.HasBody && methodDefinition.token.RID != 0U && methodDefinition.debug_info == null)
				{
					methodDefinition.debug_info = symbol_reader.Read(methodDefinition);
				}
			}
		}

		// Token: 0x04000037 RID: 55
		private bool resolve_attributes;
	}
}
