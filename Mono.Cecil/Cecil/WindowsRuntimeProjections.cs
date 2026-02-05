using System;
using System.Collections.Generic;
using System.Threading;
using Mono.Collections.Generic;

namespace Mono.Cecil
{
	// Token: 0x020000C9 RID: 201
	internal sealed class WindowsRuntimeProjections
	{
		// Token: 0x170002A4 RID: 676
		// (get) Token: 0x0600086D RID: 2157 RVA: 0x000195A8 File Offset: 0x000177A8
		private static Dictionary<string, WindowsRuntimeProjections.ProjectionInfo> Projections
		{
			get
			{
				if (WindowsRuntimeProjections.projections != null)
				{
					return WindowsRuntimeProjections.projections;
				}
				Dictionary<string, WindowsRuntimeProjections.ProjectionInfo> dictionary = new Dictionary<string, WindowsRuntimeProjections.ProjectionInfo>();
				dictionary.Add("AttributeTargets", new WindowsRuntimeProjections.ProjectionInfo("Windows.Foundation.Metadata", "System", "AttributeTargets", "System.Runtime", false, false));
				dictionary.Add("AttributeUsageAttribute", new WindowsRuntimeProjections.ProjectionInfo("Windows.Foundation.Metadata", "System", "AttributeUsageAttribute", "System.Runtime", true, false));
				dictionary.Add("Color", new WindowsRuntimeProjections.ProjectionInfo("Windows.UI", "Windows.UI", "Color", "System.Runtime.WindowsRuntime", false, false));
				dictionary.Add("CornerRadius", new WindowsRuntimeProjections.ProjectionInfo("Windows.UI.Xaml", "Windows.UI.Xaml", "CornerRadius", "System.Runtime.WindowsRuntime.UI.Xaml", false, false));
				dictionary.Add("DateTime", new WindowsRuntimeProjections.ProjectionInfo("Windows.Foundation", "System", "DateTimeOffset", "System.Runtime", false, false));
				dictionary.Add("Duration", new WindowsRuntimeProjections.ProjectionInfo("Windows.UI.Xaml", "Windows.UI.Xaml", "Duration", "System.Runtime.WindowsRuntime.UI.Xaml", false, false));
				dictionary.Add("DurationType", new WindowsRuntimeProjections.ProjectionInfo("Windows.UI.Xaml", "Windows.UI.Xaml", "DurationType", "System.Runtime.WindowsRuntime.UI.Xaml", false, false));
				dictionary.Add("EventHandler`1", new WindowsRuntimeProjections.ProjectionInfo("Windows.Foundation", "System", "EventHandler`1", "System.Runtime", false, false));
				dictionary.Add("EventRegistrationToken", new WindowsRuntimeProjections.ProjectionInfo("Windows.Foundation", "System.Runtime.InteropServices.WindowsRuntime", "EventRegistrationToken", "System.Runtime.InteropServices.WindowsRuntime", false, false));
				dictionary.Add("GeneratorPosition", new WindowsRuntimeProjections.ProjectionInfo("Windows.UI.Xaml.Controls.Primitives", "Windows.UI.Xaml.Controls.Primitives", "GeneratorPosition", "System.Runtime.WindowsRuntime.UI.Xaml", false, false));
				dictionary.Add("GridLength", new WindowsRuntimeProjections.ProjectionInfo("Windows.UI.Xaml", "Windows.UI.Xaml", "GridLength", "System.Runtime.WindowsRuntime.UI.Xaml", false, false));
				dictionary.Add("GridUnitType", new WindowsRuntimeProjections.ProjectionInfo("Windows.UI.Xaml", "Windows.UI.Xaml", "GridUnitType", "System.Runtime.WindowsRuntime.UI.Xaml", false, false));
				dictionary.Add("HResult", new WindowsRuntimeProjections.ProjectionInfo("Windows.Foundation", "System", "Exception", "System.Runtime", false, false));
				dictionary.Add("IBindableIterable", new WindowsRuntimeProjections.ProjectionInfo("Windows.UI.Xaml.Interop", "System.Collections", "IEnumerable", "System.Runtime", false, false));
				dictionary.Add("IBindableVector", new WindowsRuntimeProjections.ProjectionInfo("Windows.UI.Xaml.Interop", "System.Collections", "IList", "System.Runtime", false, false));
				dictionary.Add("IClosable", new WindowsRuntimeProjections.ProjectionInfo("Windows.Foundation", "System", "IDisposable", "System.Runtime", false, true));
				dictionary.Add("ICommand", new WindowsRuntimeProjections.ProjectionInfo("Windows.UI.Xaml.Input", "System.Windows.Input", "ICommand", "System.ObjectModel", false, false));
				dictionary.Add("IIterable`1", new WindowsRuntimeProjections.ProjectionInfo("Windows.Foundation.Collections", "System.Collections.Generic", "IEnumerable`1", "System.Runtime", false, false));
				dictionary.Add("IKeyValuePair`2", new WindowsRuntimeProjections.ProjectionInfo("Windows.Foundation.Collections", "System.Collections.Generic", "KeyValuePair`2", "System.Runtime", false, false));
				dictionary.Add("IMapView`2", new WindowsRuntimeProjections.ProjectionInfo("Windows.Foundation.Collections", "System.Collections.Generic", "IReadOnlyDictionary`2", "System.Runtime", false, false));
				dictionary.Add("IMap`2", new WindowsRuntimeProjections.ProjectionInfo("Windows.Foundation.Collections", "System.Collections.Generic", "IDictionary`2", "System.Runtime", false, false));
				dictionary.Add("INotifyCollectionChanged", new WindowsRuntimeProjections.ProjectionInfo("Windows.UI.Xaml.Interop", "System.Collections.Specialized", "INotifyCollectionChanged", "System.ObjectModel", false, false));
				dictionary.Add("INotifyPropertyChanged", new WindowsRuntimeProjections.ProjectionInfo("Windows.UI.Xaml.Data", "System.ComponentModel", "INotifyPropertyChanged", "System.ObjectModel", false, false));
				dictionary.Add("IReference`1", new WindowsRuntimeProjections.ProjectionInfo("Windows.Foundation", "System", "Nullable`1", "System.Runtime", false, false));
				dictionary.Add("IVectorView`1", new WindowsRuntimeProjections.ProjectionInfo("Windows.Foundation.Collections", "System.Collections.Generic", "IReadOnlyList`1", "System.Runtime", false, false));
				dictionary.Add("IVector`1", new WindowsRuntimeProjections.ProjectionInfo("Windows.Foundation.Collections", "System.Collections.Generic", "IList`1", "System.Runtime", false, false));
				dictionary.Add("KeyTime", new WindowsRuntimeProjections.ProjectionInfo("Windows.UI.Xaml.Media.Animation", "Windows.UI.Xaml.Media.Animation", "KeyTime", "System.Runtime.WindowsRuntime.UI.Xaml", false, false));
				dictionary.Add("Matrix", new WindowsRuntimeProjections.ProjectionInfo("Windows.UI.Xaml.Media", "Windows.UI.Xaml.Media", "Matrix", "System.Runtime.WindowsRuntime.UI.Xaml", false, false));
				dictionary.Add("Matrix3D", new WindowsRuntimeProjections.ProjectionInfo("Windows.UI.Xaml.Media.Media3D", "Windows.UI.Xaml.Media.Media3D", "Matrix3D", "System.Runtime.WindowsRuntime.UI.Xaml", false, false));
				dictionary.Add("Matrix3x2", new WindowsRuntimeProjections.ProjectionInfo("Windows.Foundation.Numerics", "System.Numerics", "Matrix3x2", "System.Numerics.Vectors", false, false));
				dictionary.Add("Matrix4x4", new WindowsRuntimeProjections.ProjectionInfo("Windows.Foundation.Numerics", "System.Numerics", "Matrix4x4", "System.Numerics.Vectors", false, false));
				dictionary.Add("NotifyCollectionChangedAction", new WindowsRuntimeProjections.ProjectionInfo("Windows.UI.Xaml.Interop", "System.Collections.Specialized", "NotifyCollectionChangedAction", "System.ObjectModel", false, false));
				dictionary.Add("NotifyCollectionChangedEventArgs", new WindowsRuntimeProjections.ProjectionInfo("Windows.UI.Xaml.Interop", "System.Collections.Specialized", "NotifyCollectionChangedEventArgs", "System.ObjectModel", false, false));
				dictionary.Add("NotifyCollectionChangedEventHandler", new WindowsRuntimeProjections.ProjectionInfo("Windows.UI.Xaml.Interop", "System.Collections.Specialized", "NotifyCollectionChangedEventHandler", "System.ObjectModel", false, false));
				dictionary.Add("Plane", new WindowsRuntimeProjections.ProjectionInfo("Windows.Foundation.Numerics", "System.Numerics", "Plane", "System.Numerics.Vectors", false, false));
				dictionary.Add("Point", new WindowsRuntimeProjections.ProjectionInfo("Windows.Foundation", "Windows.Foundation", "Point", "System.Runtime.WindowsRuntime", false, false));
				dictionary.Add("PropertyChangedEventArgs", new WindowsRuntimeProjections.ProjectionInfo("Windows.UI.Xaml.Data", "System.ComponentModel", "PropertyChangedEventArgs", "System.ObjectModel", false, false));
				dictionary.Add("PropertyChangedEventHandler", new WindowsRuntimeProjections.ProjectionInfo("Windows.UI.Xaml.Data", "System.ComponentModel", "PropertyChangedEventHandler", "System.ObjectModel", false, false));
				dictionary.Add("Quaternion", new WindowsRuntimeProjections.ProjectionInfo("Windows.Foundation.Numerics", "System.Numerics", "Quaternion", "System.Numerics.Vectors", false, false));
				dictionary.Add("Rect", new WindowsRuntimeProjections.ProjectionInfo("Windows.Foundation", "Windows.Foundation", "Rect", "System.Runtime.WindowsRuntime", false, false));
				dictionary.Add("RepeatBehavior", new WindowsRuntimeProjections.ProjectionInfo("Windows.UI.Xaml.Media.Animation", "Windows.UI.Xaml.Media.Animation", "RepeatBehavior", "System.Runtime.WindowsRuntime.UI.Xaml", false, false));
				dictionary.Add("RepeatBehaviorType", new WindowsRuntimeProjections.ProjectionInfo("Windows.UI.Xaml.Media.Animation", "Windows.UI.Xaml.Media.Animation", "RepeatBehaviorType", "System.Runtime.WindowsRuntime.UI.Xaml", false, false));
				dictionary.Add("Size", new WindowsRuntimeProjections.ProjectionInfo("Windows.Foundation", "Windows.Foundation", "Size", "System.Runtime.WindowsRuntime", false, false));
				dictionary.Add("Thickness", new WindowsRuntimeProjections.ProjectionInfo("Windows.UI.Xaml", "Windows.UI.Xaml", "Thickness", "System.Runtime.WindowsRuntime.UI.Xaml", false, false));
				dictionary.Add("TimeSpan", new WindowsRuntimeProjections.ProjectionInfo("Windows.Foundation", "System", "TimeSpan", "System.Runtime", false, false));
				dictionary.Add("TypeName", new WindowsRuntimeProjections.ProjectionInfo("Windows.UI.Xaml.Interop", "System", "Type", "System.Runtime", false, false));
				dictionary.Add("Uri", new WindowsRuntimeProjections.ProjectionInfo("Windows.Foundation", "System", "Uri", "System.Runtime", false, false));
				dictionary.Add("Vector2", new WindowsRuntimeProjections.ProjectionInfo("Windows.Foundation.Numerics", "System.Numerics", "Vector2", "System.Numerics.Vectors", false, false));
				dictionary.Add("Vector3", new WindowsRuntimeProjections.ProjectionInfo("Windows.Foundation.Numerics", "System.Numerics", "Vector3", "System.Numerics.Vectors", false, false));
				dictionary.Add("Vector4", new WindowsRuntimeProjections.ProjectionInfo("Windows.Foundation.Numerics", "System.Numerics", "Vector4", "System.Numerics.Vectors", false, false));
				WindowsRuntimeProjections.projections = dictionary;
				return dictionary;
			}
		}

		// Token: 0x170002A5 RID: 677
		// (get) Token: 0x0600086E RID: 2158 RVA: 0x00019D39 File Offset: 0x00017F39
		private AssemblyNameReference[] VirtualReferences
		{
			get
			{
				if (this.virtual_references == null)
				{
					Mixin.Read(this.module.AssemblyReferences);
				}
				return this.virtual_references;
			}
		}

		// Token: 0x0600086F RID: 2159 RVA: 0x00019D59 File Offset: 0x00017F59
		public WindowsRuntimeProjections(ModuleDefinition module)
		{
			this.module = module;
		}

		// Token: 0x06000870 RID: 2160 RVA: 0x00019D88 File Offset: 0x00017F88
		public static void Project(TypeDefinition type)
		{
			TypeDefinitionTreatment typeDefinitionTreatment = TypeDefinitionTreatment.None;
			MetadataKind metadataKind = type.Module.MetadataKind;
			if (type.IsWindowsRuntime)
			{
				if (metadataKind == MetadataKind.WindowsMetadata)
				{
					typeDefinitionTreatment = WindowsRuntimeProjections.GetWellKnownTypeDefinitionTreatment(type);
					if (typeDefinitionTreatment != TypeDefinitionTreatment.None)
					{
						WindowsRuntimeProjections.ApplyProjection(type, new TypeDefinitionProjection(type, typeDefinitionTreatment));
						return;
					}
					TypeReference baseType = type.BaseType;
					if (baseType != null && WindowsRuntimeProjections.IsAttribute(baseType))
					{
						typeDefinitionTreatment = TypeDefinitionTreatment.NormalAttribute;
					}
					else
					{
						typeDefinitionTreatment = TypeDefinitionTreatment.NormalType;
					}
				}
				else if (metadataKind == MetadataKind.ManagedWindowsMetadata && WindowsRuntimeProjections.NeedsWindowsRuntimePrefix(type))
				{
					typeDefinitionTreatment = TypeDefinitionTreatment.PrefixWindowsRuntimeName;
				}
				if ((typeDefinitionTreatment == TypeDefinitionTreatment.PrefixWindowsRuntimeName || typeDefinitionTreatment == TypeDefinitionTreatment.NormalType) && !type.IsInterface && WindowsRuntimeProjections.HasAttribute(type, "Windows.UI.Xaml", "TreatAsAbstractComposableClassAttribute"))
				{
					typeDefinitionTreatment |= TypeDefinitionTreatment.Abstract;
				}
			}
			else if (metadataKind == MetadataKind.ManagedWindowsMetadata && WindowsRuntimeProjections.IsClrImplementationType(type))
			{
				typeDefinitionTreatment = TypeDefinitionTreatment.UnmangleWindowsRuntimeName;
			}
			if (typeDefinitionTreatment != TypeDefinitionTreatment.None)
			{
				WindowsRuntimeProjections.ApplyProjection(type, new TypeDefinitionProjection(type, typeDefinitionTreatment));
			}
		}

		// Token: 0x06000871 RID: 2161 RVA: 0x00019E38 File Offset: 0x00018038
		private static TypeDefinitionTreatment GetWellKnownTypeDefinitionTreatment(TypeDefinition type)
		{
			WindowsRuntimeProjections.ProjectionInfo projectionInfo;
			if (!WindowsRuntimeProjections.Projections.TryGetValue(type.Name, out projectionInfo))
			{
				return TypeDefinitionTreatment.None;
			}
			TypeDefinitionTreatment typeDefinitionTreatment = (projectionInfo.Attribute ? TypeDefinitionTreatment.RedirectToClrAttribute : TypeDefinitionTreatment.RedirectToClrType);
			if (type.Namespace == projectionInfo.ClrNamespace)
			{
				return typeDefinitionTreatment;
			}
			if (type.Namespace == projectionInfo.WinRTNamespace)
			{
				return typeDefinitionTreatment | TypeDefinitionTreatment.Internal;
			}
			return TypeDefinitionTreatment.None;
		}

		// Token: 0x06000872 RID: 2162 RVA: 0x00019E98 File Offset: 0x00018098
		private static bool NeedsWindowsRuntimePrefix(TypeDefinition type)
		{
			if ((type.Attributes & (TypeAttributes.VisibilityMask | TypeAttributes.ClassSemanticMask)) != TypeAttributes.Public)
			{
				return false;
			}
			TypeReference baseType = type.BaseType;
			if (baseType == null || baseType.MetadataToken.TokenType != TokenType.TypeRef)
			{
				return false;
			}
			if (baseType.Namespace == "System")
			{
				string name = baseType.Name;
				if (name == "Attribute" || name == "MulticastDelegate" || name == "ValueType")
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06000873 RID: 2163 RVA: 0x00019F17 File Offset: 0x00018117
		private static bool IsClrImplementationType(TypeDefinition type)
		{
			return (type.Attributes & (TypeAttributes.VisibilityMask | TypeAttributes.SpecialName)) == TypeAttributes.SpecialName && type.Name.StartsWith("<CLR>");
		}

		// Token: 0x06000874 RID: 2164 RVA: 0x00019F40 File Offset: 0x00018140
		public static void ApplyProjection(TypeDefinition type, TypeDefinitionProjection projection)
		{
			if (projection == null)
			{
				return;
			}
			TypeDefinitionTreatment treatment = projection.Treatment;
			switch (treatment & TypeDefinitionTreatment.KindMask)
			{
			case TypeDefinitionTreatment.NormalType:
				type.Attributes |= TypeAttributes.Import | TypeAttributes.WindowsRuntime;
				break;
			case TypeDefinitionTreatment.NormalAttribute:
				type.Attributes |= TypeAttributes.Sealed | TypeAttributes.WindowsRuntime;
				break;
			case TypeDefinitionTreatment.UnmangleWindowsRuntimeName:
				type.Attributes = (type.Attributes & ~TypeAttributes.SpecialName) | TypeAttributes.Public;
				type.Name = type.Name.Substring("<CLR>".Length);
				break;
			case TypeDefinitionTreatment.PrefixWindowsRuntimeName:
				type.Attributes = (type.Attributes & ~TypeAttributes.Public) | TypeAttributes.Import;
				type.Name = "<WinRT>" + type.Name;
				break;
			case TypeDefinitionTreatment.RedirectToClrType:
				type.Attributes = (type.Attributes & ~TypeAttributes.Public) | TypeAttributes.Import;
				break;
			case TypeDefinitionTreatment.RedirectToClrAttribute:
				type.Attributes &= ~TypeAttributes.Public;
				break;
			}
			if ((treatment & TypeDefinitionTreatment.Abstract) != TypeDefinitionTreatment.None)
			{
				type.Attributes |= TypeAttributes.Abstract;
			}
			if ((treatment & TypeDefinitionTreatment.Internal) != TypeDefinitionTreatment.None)
			{
				type.Attributes &= ~TypeAttributes.Public;
			}
			type.WindowsRuntimeProjection = projection;
		}

		// Token: 0x06000875 RID: 2165 RVA: 0x0001A068 File Offset: 0x00018268
		public static TypeDefinitionProjection RemoveProjection(TypeDefinition type)
		{
			if (!type.IsWindowsRuntimeProjection)
			{
				return null;
			}
			TypeDefinitionProjection windowsRuntimeProjection = type.WindowsRuntimeProjection;
			type.WindowsRuntimeProjection = null;
			type.Attributes = windowsRuntimeProjection.Attributes;
			type.Name = windowsRuntimeProjection.Name;
			return windowsRuntimeProjection;
		}

		// Token: 0x06000876 RID: 2166 RVA: 0x0001A0A8 File Offset: 0x000182A8
		public static void Project(TypeReference type)
		{
			WindowsRuntimeProjections.ProjectionInfo projectionInfo;
			TypeReferenceTreatment typeReferenceTreatment;
			if (WindowsRuntimeProjections.Projections.TryGetValue(type.Name, out projectionInfo) && projectionInfo.WinRTNamespace == type.Namespace)
			{
				typeReferenceTreatment = TypeReferenceTreatment.UseProjectionInfo;
			}
			else
			{
				typeReferenceTreatment = WindowsRuntimeProjections.GetSpecialTypeReferenceTreatment(type);
			}
			if (typeReferenceTreatment != TypeReferenceTreatment.None)
			{
				WindowsRuntimeProjections.ApplyProjection(type, new TypeReferenceProjection(type, typeReferenceTreatment));
			}
		}

		// Token: 0x06000877 RID: 2167 RVA: 0x0001A0F7 File Offset: 0x000182F7
		private static TypeReferenceTreatment GetSpecialTypeReferenceTreatment(TypeReference type)
		{
			if (type.Namespace == "System")
			{
				if (type.Name == "MulticastDelegate")
				{
					return TypeReferenceTreatment.SystemDelegate;
				}
				if (type.Name == "Attribute")
				{
					return TypeReferenceTreatment.SystemAttribute;
				}
			}
			return TypeReferenceTreatment.None;
		}

		// Token: 0x06000878 RID: 2168 RVA: 0x0001A134 File Offset: 0x00018334
		private static bool IsAttribute(TypeReference type)
		{
			return type.MetadataToken.TokenType == TokenType.TypeRef && type.Name == "Attribute" && type.Namespace == "System";
		}

		// Token: 0x06000879 RID: 2169 RVA: 0x0001A17C File Offset: 0x0001837C
		private static bool IsEnum(TypeReference type)
		{
			return type.MetadataToken.TokenType == TokenType.TypeRef && type.Name == "Enum" && type.Namespace == "System";
		}

		// Token: 0x0600087A RID: 2170 RVA: 0x0001A1C4 File Offset: 0x000183C4
		public static void ApplyProjection(TypeReference type, TypeReferenceProjection projection)
		{
			if (projection == null)
			{
				return;
			}
			switch (projection.Treatment)
			{
			case TypeReferenceTreatment.SystemDelegate:
			case TypeReferenceTreatment.SystemAttribute:
				type.Scope = type.Module.Projections.GetAssemblyReference("System.Runtime");
				break;
			case TypeReferenceTreatment.UseProjectionInfo:
			{
				WindowsRuntimeProjections.ProjectionInfo projectionInfo = WindowsRuntimeProjections.Projections[type.Name];
				type.Name = projectionInfo.ClrName;
				type.Namespace = projectionInfo.ClrNamespace;
				type.Scope = type.Module.Projections.GetAssemblyReference(projectionInfo.ClrAssembly);
				break;
			}
			}
			type.WindowsRuntimeProjection = projection;
		}

		// Token: 0x0600087B RID: 2171 RVA: 0x0001A25C File Offset: 0x0001845C
		public static TypeReferenceProjection RemoveProjection(TypeReference type)
		{
			if (!type.IsWindowsRuntimeProjection)
			{
				return null;
			}
			TypeReferenceProjection windowsRuntimeProjection = type.WindowsRuntimeProjection;
			type.WindowsRuntimeProjection = null;
			type.Name = windowsRuntimeProjection.Name;
			type.Namespace = windowsRuntimeProjection.Namespace;
			type.Scope = windowsRuntimeProjection.Scope;
			return windowsRuntimeProjection;
		}

		// Token: 0x0600087C RID: 2172 RVA: 0x0001A2A8 File Offset: 0x000184A8
		public static void Project(MethodDefinition method)
		{
			MethodDefinitionTreatment methodDefinitionTreatment = MethodDefinitionTreatment.None;
			bool flag = false;
			TypeDefinition declaringType = method.DeclaringType;
			if (declaringType.IsWindowsRuntime)
			{
				if (WindowsRuntimeProjections.IsClrImplementationType(declaringType))
				{
					methodDefinitionTreatment = MethodDefinitionTreatment.None;
				}
				else if (declaringType.IsNested)
				{
					methodDefinitionTreatment = MethodDefinitionTreatment.None;
				}
				else if (declaringType.IsInterface)
				{
					methodDefinitionTreatment = MethodDefinitionTreatment.Runtime | MethodDefinitionTreatment.InternalCall;
				}
				else if (declaringType.Module.MetadataKind == MetadataKind.ManagedWindowsMetadata && !method.IsPublic)
				{
					methodDefinitionTreatment = MethodDefinitionTreatment.None;
				}
				else
				{
					flag = true;
					TypeReference baseType = declaringType.BaseType;
					if (baseType != null && baseType.MetadataToken.TokenType == TokenType.TypeRef)
					{
						TypeReferenceTreatment specialTypeReferenceTreatment = WindowsRuntimeProjections.GetSpecialTypeReferenceTreatment(baseType);
						if (specialTypeReferenceTreatment != TypeReferenceTreatment.SystemDelegate)
						{
							if (specialTypeReferenceTreatment == TypeReferenceTreatment.SystemAttribute)
							{
								methodDefinitionTreatment = MethodDefinitionTreatment.Runtime | MethodDefinitionTreatment.InternalCall;
								flag = false;
							}
						}
						else
						{
							methodDefinitionTreatment = MethodDefinitionTreatment.Public | MethodDefinitionTreatment.Runtime;
							flag = false;
						}
					}
				}
			}
			if (flag)
			{
				bool flag2 = false;
				bool flag3 = false;
				bool flag4 = false;
				foreach (MethodReference methodReference in method.Overrides)
				{
					if (methodReference.MetadataToken.TokenType == TokenType.MemberRef && WindowsRuntimeProjections.ImplementsRedirectedInterface(methodReference, out flag4))
					{
						flag2 = true;
						if (flag4)
						{
							break;
						}
					}
					else
					{
						flag3 = true;
					}
				}
				if (flag4)
				{
					methodDefinitionTreatment = MethodDefinitionTreatment.Dispose;
					flag = false;
				}
				else if (flag2 && !flag3)
				{
					methodDefinitionTreatment = MethodDefinitionTreatment.Private | MethodDefinitionTreatment.Runtime | MethodDefinitionTreatment.InternalCall;
					flag = false;
				}
			}
			if (flag)
			{
				methodDefinitionTreatment |= WindowsRuntimeProjections.GetMethodDefinitionTreatmentFromCustomAttributes(method);
			}
			if (methodDefinitionTreatment != MethodDefinitionTreatment.None)
			{
				WindowsRuntimeProjections.ApplyProjection(method, new MethodDefinitionProjection(method, methodDefinitionTreatment));
			}
		}

		// Token: 0x0600087D RID: 2173 RVA: 0x0001A3FC File Offset: 0x000185FC
		private static MethodDefinitionTreatment GetMethodDefinitionTreatmentFromCustomAttributes(MethodDefinition method)
		{
			MethodDefinitionTreatment methodDefinitionTreatment = MethodDefinitionTreatment.None;
			foreach (CustomAttribute customAttribute in method.CustomAttributes)
			{
				TypeReference attributeType = customAttribute.AttributeType;
				if (!(attributeType.Namespace != "Windows.UI.Xaml"))
				{
					if (attributeType.Name == "TreatAsPublicMethodAttribute")
					{
						methodDefinitionTreatment |= MethodDefinitionTreatment.Public;
					}
					else if (attributeType.Name == "TreatAsAbstractMethodAttribute")
					{
						methodDefinitionTreatment |= MethodDefinitionTreatment.Abstract;
					}
				}
			}
			return methodDefinitionTreatment;
		}

		// Token: 0x0600087E RID: 2174 RVA: 0x0001A490 File Offset: 0x00018690
		public static void ApplyProjection(MethodDefinition method, MethodDefinitionProjection projection)
		{
			if (projection == null)
			{
				return;
			}
			MethodDefinitionTreatment treatment = projection.Treatment;
			if ((treatment & MethodDefinitionTreatment.Dispose) != MethodDefinitionTreatment.None)
			{
				method.Name = "Dispose";
			}
			if ((treatment & MethodDefinitionTreatment.Abstract) != MethodDefinitionTreatment.None)
			{
				method.Attributes |= MethodAttributes.Abstract;
			}
			if ((treatment & MethodDefinitionTreatment.Private) != MethodDefinitionTreatment.None)
			{
				method.Attributes = (method.Attributes & ~MethodAttributes.MemberAccessMask) | MethodAttributes.Private;
			}
			if ((treatment & MethodDefinitionTreatment.Public) != MethodDefinitionTreatment.None)
			{
				method.Attributes = (method.Attributes & ~MethodAttributes.MemberAccessMask) | MethodAttributes.Public;
			}
			if ((treatment & MethodDefinitionTreatment.Runtime) != MethodDefinitionTreatment.None)
			{
				method.ImplAttributes |= MethodImplAttributes.CodeTypeMask;
			}
			if ((treatment & MethodDefinitionTreatment.InternalCall) != MethodDefinitionTreatment.None)
			{
				method.ImplAttributes |= MethodImplAttributes.InternalCall;
			}
			method.WindowsRuntimeProjection = projection;
		}

		// Token: 0x0600087F RID: 2175 RVA: 0x0001A534 File Offset: 0x00018734
		public static MethodDefinitionProjection RemoveProjection(MethodDefinition method)
		{
			if (!method.IsWindowsRuntimeProjection)
			{
				return null;
			}
			MethodDefinitionProjection windowsRuntimeProjection = method.WindowsRuntimeProjection;
			method.WindowsRuntimeProjection = null;
			method.Attributes = windowsRuntimeProjection.Attributes;
			method.ImplAttributes = windowsRuntimeProjection.ImplAttributes;
			method.Name = windowsRuntimeProjection.Name;
			return windowsRuntimeProjection;
		}

		// Token: 0x06000880 RID: 2176 RVA: 0x0001A580 File Offset: 0x00018780
		public static void Project(FieldDefinition field)
		{
			FieldDefinitionTreatment fieldDefinitionTreatment = FieldDefinitionTreatment.None;
			TypeDefinition declaringType = field.DeclaringType;
			if (declaringType.Module.MetadataKind == MetadataKind.WindowsMetadata && field.IsRuntimeSpecialName && field.Name == "value__")
			{
				TypeReference baseType = declaringType.BaseType;
				if (baseType != null && WindowsRuntimeProjections.IsEnum(baseType))
				{
					fieldDefinitionTreatment = FieldDefinitionTreatment.Public;
				}
			}
			if (fieldDefinitionTreatment != FieldDefinitionTreatment.None)
			{
				WindowsRuntimeProjections.ApplyProjection(field, new FieldDefinitionProjection(field, fieldDefinitionTreatment));
			}
		}

		// Token: 0x06000881 RID: 2177 RVA: 0x0001A5E2 File Offset: 0x000187E2
		public static void ApplyProjection(FieldDefinition field, FieldDefinitionProjection projection)
		{
			if (projection == null)
			{
				return;
			}
			if (projection.Treatment == FieldDefinitionTreatment.Public)
			{
				field.Attributes = (field.Attributes & ~FieldAttributes.FieldAccessMask) | FieldAttributes.Public;
			}
			field.WindowsRuntimeProjection = projection;
		}

		// Token: 0x06000882 RID: 2178 RVA: 0x0001A60C File Offset: 0x0001880C
		public static FieldDefinitionProjection RemoveProjection(FieldDefinition field)
		{
			if (!field.IsWindowsRuntimeProjection)
			{
				return null;
			}
			FieldDefinitionProjection windowsRuntimeProjection = field.WindowsRuntimeProjection;
			field.WindowsRuntimeProjection = null;
			field.Attributes = windowsRuntimeProjection.Attributes;
			return windowsRuntimeProjection;
		}

		// Token: 0x06000883 RID: 2179 RVA: 0x0001A640 File Offset: 0x00018840
		public static void Project(MemberReference member)
		{
			bool flag;
			if (!WindowsRuntimeProjections.ImplementsRedirectedInterface(member, out flag) || !flag)
			{
				return;
			}
			WindowsRuntimeProjections.ApplyProjection(member, new MemberReferenceProjection(member, MemberReferenceTreatment.Dispose));
		}

		// Token: 0x06000884 RID: 2180 RVA: 0x0001A668 File Offset: 0x00018868
		private static bool ImplementsRedirectedInterface(MemberReference member, out bool disposable)
		{
			disposable = false;
			TypeReference declaringType = member.DeclaringType;
			TokenType tokenType = declaringType.MetadataToken.TokenType;
			TypeReference typeReference;
			if (tokenType != TokenType.TypeRef)
			{
				if (tokenType != TokenType.TypeSpec)
				{
					return false;
				}
				if (!declaringType.IsGenericInstance)
				{
					return false;
				}
				typeReference = ((TypeSpecification)declaringType).ElementType;
				if (typeReference.MetadataType != MetadataType.Class || typeReference.MetadataToken.TokenType != TokenType.TypeRef)
				{
					return false;
				}
			}
			else
			{
				typeReference = declaringType;
			}
			TypeReferenceProjection typeReferenceProjection = WindowsRuntimeProjections.RemoveProjection(typeReference);
			bool flag = false;
			WindowsRuntimeProjections.ProjectionInfo projectionInfo;
			if (WindowsRuntimeProjections.Projections.TryGetValue(typeReference.Name, out projectionInfo) && typeReference.Namespace == projectionInfo.WinRTNamespace)
			{
				disposable = projectionInfo.Disposable;
				flag = true;
			}
			WindowsRuntimeProjections.ApplyProjection(typeReference, typeReferenceProjection);
			return flag;
		}

		// Token: 0x06000885 RID: 2181 RVA: 0x0001A726 File Offset: 0x00018926
		public static void ApplyProjection(MemberReference member, MemberReferenceProjection projection)
		{
			if (projection == null)
			{
				return;
			}
			if (projection.Treatment == MemberReferenceTreatment.Dispose)
			{
				member.Name = "Dispose";
			}
			member.WindowsRuntimeProjection = projection;
		}

		// Token: 0x06000886 RID: 2182 RVA: 0x0001A748 File Offset: 0x00018948
		public static MemberReferenceProjection RemoveProjection(MemberReference member)
		{
			if (!member.IsWindowsRuntimeProjection)
			{
				return null;
			}
			MemberReferenceProjection windowsRuntimeProjection = member.WindowsRuntimeProjection;
			member.WindowsRuntimeProjection = null;
			member.Name = windowsRuntimeProjection.Name;
			return windowsRuntimeProjection;
		}

		// Token: 0x06000887 RID: 2183 RVA: 0x0001A77C File Offset: 0x0001897C
		public void AddVirtualReferences(Collection<AssemblyNameReference> references)
		{
			AssemblyNameReference coreLibrary = WindowsRuntimeProjections.GetCoreLibrary(references);
			this.corlib_version = coreLibrary.Version;
			coreLibrary.Version = WindowsRuntimeProjections.version;
			if (this.virtual_references == null)
			{
				AssemblyNameReference[] assemblyReferences = WindowsRuntimeProjections.GetAssemblyReferences(coreLibrary);
				Interlocked.CompareExchange<AssemblyNameReference[]>(ref this.virtual_references, assemblyReferences, null);
			}
			foreach (AssemblyNameReference assemblyNameReference in this.virtual_references)
			{
				references.Add(assemblyNameReference);
			}
		}

		// Token: 0x06000888 RID: 2184 RVA: 0x0001A7E8 File Offset: 0x000189E8
		public void RemoveVirtualReferences(Collection<AssemblyNameReference> references)
		{
			WindowsRuntimeProjections.GetCoreLibrary(references).Version = this.corlib_version;
			foreach (AssemblyNameReference assemblyNameReference in this.VirtualReferences)
			{
				references.Remove(assemblyNameReference);
			}
		}

		// Token: 0x06000889 RID: 2185 RVA: 0x0001A828 File Offset: 0x00018A28
		private static AssemblyNameReference[] GetAssemblyReferences(AssemblyNameReference corlib)
		{
			AssemblyNameReference assemblyNameReference = new AssemblyNameReference("System.Runtime", WindowsRuntimeProjections.version);
			AssemblyNameReference assemblyNameReference2 = new AssemblyNameReference("System.Runtime.InteropServices.WindowsRuntime", WindowsRuntimeProjections.version);
			AssemblyNameReference assemblyNameReference3 = new AssemblyNameReference("System.ObjectModel", WindowsRuntimeProjections.version);
			AssemblyNameReference assemblyNameReference4 = new AssemblyNameReference("System.Runtime.WindowsRuntime", WindowsRuntimeProjections.version);
			AssemblyNameReference assemblyNameReference5 = new AssemblyNameReference("System.Runtime.WindowsRuntime.UI.Xaml", WindowsRuntimeProjections.version);
			AssemblyNameReference assemblyNameReference6 = new AssemblyNameReference("System.Numerics.Vectors", WindowsRuntimeProjections.version);
			if (corlib.HasPublicKey)
			{
				assemblyNameReference4.PublicKey = (assemblyNameReference5.PublicKey = corlib.PublicKey);
				assemblyNameReference.PublicKey = (assemblyNameReference2.PublicKey = (assemblyNameReference3.PublicKey = (assemblyNameReference6.PublicKey = WindowsRuntimeProjections.contract_pk)));
			}
			else
			{
				assemblyNameReference4.PublicKeyToken = (assemblyNameReference5.PublicKeyToken = corlib.PublicKeyToken);
				assemblyNameReference.PublicKeyToken = (assemblyNameReference2.PublicKeyToken = (assemblyNameReference3.PublicKeyToken = (assemblyNameReference6.PublicKeyToken = WindowsRuntimeProjections.contract_pk_token)));
			}
			return new AssemblyNameReference[] { assemblyNameReference, assemblyNameReference2, assemblyNameReference3, assemblyNameReference4, assemblyNameReference5, assemblyNameReference6 };
		}

		// Token: 0x0600088A RID: 2186 RVA: 0x0001A94C File Offset: 0x00018B4C
		private static AssemblyNameReference GetCoreLibrary(Collection<AssemblyNameReference> references)
		{
			foreach (AssemblyNameReference assemblyNameReference in references)
			{
				if (assemblyNameReference.Name == "mscorlib")
				{
					return assemblyNameReference;
				}
			}
			throw new BadImageFormatException("Missing mscorlib reference in AssemblyRef table.");
		}

		// Token: 0x0600088B RID: 2187 RVA: 0x0001A9B8 File Offset: 0x00018BB8
		private AssemblyNameReference GetAssemblyReference(string name)
		{
			foreach (AssemblyNameReference assemblyNameReference in this.VirtualReferences)
			{
				if (assemblyNameReference.Name == name)
				{
					return assemblyNameReference;
				}
			}
			throw new Exception();
		}

		// Token: 0x0600088C RID: 2188 RVA: 0x0001A9F4 File Offset: 0x00018BF4
		public static void Project(ICustomAttributeProvider owner, CustomAttribute attribute)
		{
			if (!WindowsRuntimeProjections.IsWindowsAttributeUsageAttribute(owner, attribute))
			{
				return;
			}
			CustomAttributeValueTreatment customAttributeValueTreatment = CustomAttributeValueTreatment.None;
			TypeDefinition typeDefinition = (TypeDefinition)owner;
			if (typeDefinition.Namespace == "Windows.Foundation.Metadata")
			{
				if (typeDefinition.Name == "VersionAttribute")
				{
					customAttributeValueTreatment = CustomAttributeValueTreatment.VersionAttribute;
				}
				else if (typeDefinition.Name == "DeprecatedAttribute")
				{
					customAttributeValueTreatment = CustomAttributeValueTreatment.DeprecatedAttribute;
				}
			}
			if (customAttributeValueTreatment == CustomAttributeValueTreatment.None)
			{
				customAttributeValueTreatment = (WindowsRuntimeProjections.HasAttribute(typeDefinition, "Windows.Foundation.Metadata", "AllowMultipleAttribute") ? CustomAttributeValueTreatment.AllowMultiple : CustomAttributeValueTreatment.AllowSingle);
			}
			if (customAttributeValueTreatment != CustomAttributeValueTreatment.None)
			{
				AttributeTargets attributeTargets = (AttributeTargets)attribute.ConstructorArguments[0].Value;
				WindowsRuntimeProjections.ApplyProjection(attribute, new CustomAttributeValueProjection(attributeTargets, customAttributeValueTreatment));
			}
		}

		// Token: 0x0600088D RID: 2189 RVA: 0x0001AA94 File Offset: 0x00018C94
		private static bool IsWindowsAttributeUsageAttribute(ICustomAttributeProvider owner, CustomAttribute attribute)
		{
			if (owner.MetadataToken.TokenType != TokenType.TypeDef)
			{
				return false;
			}
			MethodReference constructor = attribute.Constructor;
			if (constructor.MetadataToken.TokenType != TokenType.MemberRef)
			{
				return false;
			}
			TypeReference declaringType = constructor.DeclaringType;
			return declaringType.MetadataToken.TokenType == TokenType.TypeRef && declaringType.Name == "AttributeUsageAttribute" && declaringType.Namespace == "System";
		}

		// Token: 0x0600088E RID: 2190 RVA: 0x0001AB18 File Offset: 0x00018D18
		private static bool HasAttribute(TypeDefinition type, string @namespace, string name)
		{
			foreach (CustomAttribute customAttribute in type.CustomAttributes)
			{
				TypeReference attributeType = customAttribute.AttributeType;
				if (attributeType.Name == name && attributeType.Namespace == @namespace)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600088F RID: 2191 RVA: 0x0001AB8C File Offset: 0x00018D8C
		public static void ApplyProjection(CustomAttribute attribute, CustomAttributeValueProjection projection)
		{
			if (projection == null)
			{
				return;
			}
			bool flag;
			bool flag2;
			switch (projection.Treatment)
			{
			case CustomAttributeValueTreatment.AllowSingle:
				flag = false;
				flag2 = false;
				break;
			case CustomAttributeValueTreatment.AllowMultiple:
				flag = false;
				flag2 = true;
				break;
			case CustomAttributeValueTreatment.VersionAttribute:
			case CustomAttributeValueTreatment.DeprecatedAttribute:
				flag = true;
				flag2 = true;
				break;
			default:
				throw new ArgumentException();
			}
			AttributeTargets attributeTargets = (AttributeTargets)attribute.ConstructorArguments[0].Value;
			if (flag)
			{
				attributeTargets |= AttributeTargets.Constructor | AttributeTargets.Property;
			}
			attribute.ConstructorArguments[0] = new CustomAttributeArgument(attribute.ConstructorArguments[0].Type, attributeTargets);
			attribute.Properties.Add(new CustomAttributeNamedArgument("AllowMultiple", new CustomAttributeArgument(attribute.Module.TypeSystem.Boolean, flag2)));
			attribute.projection = projection;
		}

		// Token: 0x06000890 RID: 2192 RVA: 0x0001AC60 File Offset: 0x00018E60
		public static CustomAttributeValueProjection RemoveProjection(CustomAttribute attribute)
		{
			if (attribute.projection == null)
			{
				return null;
			}
			CustomAttributeValueProjection projection = attribute.projection;
			attribute.projection = null;
			attribute.ConstructorArguments[0] = new CustomAttributeArgument(attribute.ConstructorArguments[0].Type, projection.Targets);
			attribute.Properties.Clear();
			return projection;
		}

		// Token: 0x0400030E RID: 782
		private static readonly Version version = new Version(4, 0, 0, 0);

		// Token: 0x0400030F RID: 783
		private static readonly byte[] contract_pk_token = new byte[] { 176, 63, 95, 127, 17, 213, 10, 58 };

		// Token: 0x04000310 RID: 784
		private static readonly byte[] contract_pk = new byte[]
		{
			0, 36, 0, 0, 4, 128, 0, 0, 148, 0,
			0, 0, 6, 2, 0, 0, 0, 36, 0, 0,
			82, 83, 65, 49, 0, 4, 0, 0, 1, 0,
			1, 0, 7, 209, 250, 87, 196, 174, 217, 240,
			163, 46, 132, 170, 15, 174, 253, 13, 233, 232,
			253, 106, 236, 143, 135, 251, 3, 118, 108, 131,
			76, 153, 146, 30, 178, 59, 231, 154, 217, 213,
			220, 193, 221, 154, 210, 54, 19, 33, 2, 144,
			11, 114, 60, 249, 128, 149, 127, 196, 225, 119,
			16, 143, 198, 7, 119, 79, 41, 232, 50, 14,
			146, 234, 5, 236, 228, 232, 33, 192, 165, 239,
			232, 241, 100, 92, 76, 12, 147, 193, 171, 153,
			40, 93, 98, 44, 170, 101, 44, 29, 250, 214,
			61, 116, 93, 111, 45, 229, 241, 126, 94, 175,
			15, 196, 150, 61, 38, 28, 138, 18, 67, 101,
			24, 32, 109, 192, 147, 52, 77, 90, 210, 147
		};

		// Token: 0x04000311 RID: 785
		private static Dictionary<string, WindowsRuntimeProjections.ProjectionInfo> projections;

		// Token: 0x04000312 RID: 786
		private readonly ModuleDefinition module;

		// Token: 0x04000313 RID: 787
		private Version corlib_version = new Version(255, 255, 255, 255);

		// Token: 0x04000314 RID: 788
		private AssemblyNameReference[] virtual_references;

		// Token: 0x0200014D RID: 333
		private struct ProjectionInfo
		{
			// Token: 0x06000BD5 RID: 3029 RVA: 0x0002517D File Offset: 0x0002337D
			public ProjectionInfo(string winrt_namespace, string clr_namespace, string clr_name, string clr_assembly, bool attribute = false, bool disposable = false)
			{
				this.WinRTNamespace = winrt_namespace;
				this.ClrNamespace = clr_namespace;
				this.ClrName = clr_name;
				this.ClrAssembly = clr_assembly;
				this.Attribute = attribute;
				this.Disposable = disposable;
			}

			// Token: 0x04000759 RID: 1881
			public readonly string WinRTNamespace;

			// Token: 0x0400075A RID: 1882
			public readonly string ClrNamespace;

			// Token: 0x0400075B RID: 1883
			public readonly string ClrName;

			// Token: 0x0400075C RID: 1884
			public readonly string ClrAssembly;

			// Token: 0x0400075D RID: 1885
			public readonly bool Attribute;

			// Token: 0x0400075E RID: 1886
			public readonly bool Disposable;
		}
	}
}
