using System;
using Mono.Cecil.Cil;
using Mono.Cecil.PE;
using Mono.Collections.Generic;

namespace Mono.Cecil
{
	// Token: 0x020000C3 RID: 195
	internal sealed class ImmediateModuleReader : ModuleReader
	{
		// Token: 0x06000475 RID: 1141 RVA: 0x00013C4C File Offset: 0x00011E4C
		public ImmediateModuleReader(Image image)
			: base(image, ReadingMode.Immediate)
		{
		}

		// Token: 0x06000476 RID: 1142 RVA: 0x00013C56 File Offset: 0x00011E56
		protected override void ReadModule()
		{
			this.module.Read<ModuleDefinition>(this.module, delegate(ModuleDefinition module, MetadataReader reader)
			{
				base.ReadModuleManifest(reader);
				this.ReadModule(module, true);
			});
		}

		// Token: 0x06000477 RID: 1143 RVA: 0x00013C78 File Offset: 0x00011E78
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

		// Token: 0x06000478 RID: 1144 RVA: 0x00013D0C File Offset: 0x00011F0C
		private void ReadTypes(Collection<TypeDefinition> types)
		{
			for (int i = 0; i < types.Count; i++)
			{
				this.ReadType(types[i]);
			}
		}

		// Token: 0x06000479 RID: 1145 RVA: 0x00013D38 File Offset: 0x00011F38
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

		// Token: 0x0600047A RID: 1146 RVA: 0x00013DD4 File Offset: 0x00011FD4
		private void ReadInterfaces(TypeDefinition type)
		{
			Collection<InterfaceImplementation> interfaces = type.Interfaces;
			for (int i = 0; i < interfaces.Count; i++)
			{
				this.ReadCustomAttributes(interfaces[i]);
			}
		}

		// Token: 0x0600047B RID: 1147 RVA: 0x00013E08 File Offset: 0x00012008
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
					this.ReadGenericParameterConstraints(genericParameter);
				}
				this.ReadCustomAttributes(genericParameter);
			}
		}

		// Token: 0x0600047C RID: 1148 RVA: 0x00013E54 File Offset: 0x00012054
		private void ReadGenericParameterConstraints(GenericParameter parameter)
		{
			Collection<GenericParameterConstraint> constraints = parameter.Constraints;
			for (int i = 0; i < constraints.Count; i++)
			{
				this.ReadCustomAttributes(constraints[i]);
			}
		}

		// Token: 0x0600047D RID: 1149 RVA: 0x00013E88 File Offset: 0x00012088
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

		// Token: 0x0600047E RID: 1150 RVA: 0x00013ED0 File Offset: 0x000120D0
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

		// Token: 0x0600047F RID: 1151 RVA: 0x00013F18 File Offset: 0x00012118
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

		// Token: 0x06000480 RID: 1152 RVA: 0x00013FA0 File Offset: 0x000121A0
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

		// Token: 0x06000481 RID: 1153 RVA: 0x00014054 File Offset: 0x00012254
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

		// Token: 0x06000482 RID: 1154 RVA: 0x000140B0 File Offset: 0x000122B0
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

		// Token: 0x06000483 RID: 1155 RVA: 0x00014104 File Offset: 0x00012304
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

		// Token: 0x06000484 RID: 1156 RVA: 0x00014143 File Offset: 0x00012343
		public override void ReadSymbols(ModuleDefinition module)
		{
			if (module.symbol_reader == null)
			{
				return;
			}
			this.ReadTypesSymbols(module.Types, module.symbol_reader);
		}

		// Token: 0x06000485 RID: 1157 RVA: 0x00014160 File Offset: 0x00012360
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

		// Token: 0x06000486 RID: 1158 RVA: 0x000141AC File Offset: 0x000123AC
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

		// Token: 0x0400023A RID: 570
		private bool resolve_attributes;
	}
}
