using System;
using System.Collections.Generic;
using System.Threading;
using Mono.Collections.Generic;

namespace Mono.Cecil
{
	// Token: 0x0200018A RID: 394
	internal sealed class WindowsRuntimeProjections
	{
		// Token: 0x170002C4 RID: 708
		// (get) Token: 0x06000C46 RID: 3142 RVA: 0x00028520 File Offset: 0x00026720
		private static Dictionary<string, WindowsRuntimeProjections.ProjectionInfo> Projections
		{
			get
			{
				if (WindowsRuntimeProjections.projections != null)
				{
					return WindowsRuntimeProjections.projections;
				}
				Dictionary<string, WindowsRuntimeProjections.ProjectionInfo> dictionary = new Dictionary<string, WindowsRuntimeProjections.ProjectionInfo>
				{
					{
						"AttributeTargets",
						new WindowsRuntimeProjections.ProjectionInfo("Windows.Foundation.Metadata", "System", "AttributeTargets", "System.Runtime", false, false)
					},
					{
						"AttributeUsageAttribute",
						new WindowsRuntimeProjections.ProjectionInfo("Windows.Foundation.Metadata", "System", "AttributeUsageAttribute", "System.Runtime", true, false)
					},
					{
						"Color",
						new WindowsRuntimeProjections.ProjectionInfo("Windows.UI", "Windows.UI", "Color", "System.Runtime.WindowsRuntime", false, false)
					},
					{
						"CornerRadius",
						new WindowsRuntimeProjections.ProjectionInfo("Windows.UI.Xaml", "Windows.UI.Xaml", "CornerRadius", "System.Runtime.WindowsRuntime.UI.Xaml", false, false)
					},
					{
						"DateTime",
						new WindowsRuntimeProjections.ProjectionInfo("Windows.Foundation", "System", "DateTimeOffset", "System.Runtime", false, false)
					},
					{
						"Duration",
						new WindowsRuntimeProjections.ProjectionInfo("Windows.UI.Xaml", "Windows.UI.Xaml", "Duration", "System.Runtime.WindowsRuntime.UI.Xaml", false, false)
					},
					{
						"DurationType",
						new WindowsRuntimeProjections.ProjectionInfo("Windows.UI.Xaml", "Windows.UI.Xaml", "DurationType", "System.Runtime.WindowsRuntime.UI.Xaml", false, false)
					},
					{
						"EventHandler`1",
						new WindowsRuntimeProjections.ProjectionInfo("Windows.Foundation", "System", "EventHandler`1", "System.Runtime", false, false)
					},
					{
						"EventRegistrationToken",
						new WindowsRuntimeProjections.ProjectionInfo("Windows.Foundation", "System.Runtime.InteropServices.WindowsRuntime", "EventRegistrationToken", "System.Runtime.InteropServices.WindowsRuntime", false, false)
					},
					{
						"GeneratorPosition",
						new WindowsRuntimeProjections.ProjectionInfo("Windows.UI.Xaml.Controls.Primitives", "Windows.UI.Xaml.Controls.Primitives", "GeneratorPosition", "System.Runtime.WindowsRuntime.UI.Xaml", false, false)
					},
					{
						"GridLength",
						new WindowsRuntimeProjections.ProjectionInfo("Windows.UI.Xaml", "Windows.UI.Xaml", "GridLength", "System.Runtime.WindowsRuntime.UI.Xaml", false, false)
					},
					{
						"GridUnitType",
						new WindowsRuntimeProjections.ProjectionInfo("Windows.UI.Xaml", "Windows.UI.Xaml", "GridUnitType", "System.Runtime.WindowsRuntime.UI.Xaml", false, false)
					},
					{
						"HResult",
						new WindowsRuntimeProjections.ProjectionInfo("Windows.Foundation", "System", "Exception", "System.Runtime", false, false)
					},
					{
						"IBindableIterable",
						new WindowsRuntimeProjections.ProjectionInfo("Windows.UI.Xaml.Interop", "System.Collections", "IEnumerable", "System.Runtime", false, false)
					},
					{
						"IBindableVector",
						new WindowsRuntimeProjections.ProjectionInfo("Windows.UI.Xaml.Interop", "System.Collections", "IList", "System.Runtime", false, false)
					},
					{
						"IClosable",
						new WindowsRuntimeProjections.ProjectionInfo("Windows.Foundation", "System", "IDisposable", "System.Runtime", false, true)
					},
					{
						"ICommand",
						new WindowsRuntimeProjections.ProjectionInfo("Windows.UI.Xaml.Input", "System.Windows.Input", "ICommand", "System.ObjectModel", false, false)
					},
					{
						"IIterable`1",
						new WindowsRuntimeProjections.ProjectionInfo("Windows.Foundation.Collections", "System.Collections.Generic", "IEnumerable`1", "System.Runtime", false, false)
					},
					{
						"IKeyValuePair`2",
						new WindowsRuntimeProjections.ProjectionInfo("Windows.Foundation.Collections", "System.Collections.Generic", "KeyValuePair`2", "System.Runtime", false, false)
					},
					{
						"IMapView`2",
						new WindowsRuntimeProjections.ProjectionInfo("Windows.Foundation.Collections", "System.Collections.Generic", "IReadOnlyDictionary`2", "System.Runtime", false, false)
					},
					{
						"IMap`2",
						new WindowsRuntimeProjections.ProjectionInfo("Windows.Foundation.Collections", "System.Collections.Generic", "IDictionary`2", "System.Runtime", false, false)
					},
					{
						"INotifyCollectionChanged",
						new WindowsRuntimeProjections.ProjectionInfo("Windows.UI.Xaml.Interop", "System.Collections.Specialized", "INotifyCollectionChanged", "System.ObjectModel", false, false)
					},
					{
						"INotifyPropertyChanged",
						new WindowsRuntimeProjections.ProjectionInfo("Windows.UI.Xaml.Data", "System.ComponentModel", "INotifyPropertyChanged", "System.ObjectModel", false, false)
					},
					{
						"IReference`1",
						new WindowsRuntimeProjections.ProjectionInfo("Windows.Foundation", "System", "Nullable`1", "System.Runtime", false, false)
					},
					{
						"IVectorView`1",
						new WindowsRuntimeProjections.ProjectionInfo("Windows.Foundation.Collections", "System.Collections.Generic", "IReadOnlyList`1", "System.Runtime", false, false)
					},
					{
						"IVector`1",
						new WindowsRuntimeProjections.ProjectionInfo("Windows.Foundation.Collections", "System.Collections.Generic", "IList`1", "System.Runtime", false, false)
					},
					{
						"KeyTime",
						new WindowsRuntimeProjections.ProjectionInfo("Windows.UI.Xaml.Media.Animation", "Windows.UI.Xaml.Media.Animation", "KeyTime", "System.Runtime.WindowsRuntime.UI.Xaml", false, false)
					},
					{
						"Matrix",
						new WindowsRuntimeProjections.ProjectionInfo("Windows.UI.Xaml.Media", "Windows.UI.Xaml.Media", "Matrix", "System.Runtime.WindowsRuntime.UI.Xaml", false, false)
					},
					{
						"Matrix3D",
						new WindowsRuntimeProjections.ProjectionInfo("Windows.UI.Xaml.Media.Media3D", "Windows.UI.Xaml.Media.Media3D", "Matrix3D", "System.Runtime.WindowsRuntime.UI.Xaml", false, false)
					},
					{
						"Matrix3x2",
						new WindowsRuntimeProjections.ProjectionInfo("Windows.Foundation.Numerics", "System.Numerics", "Matrix3x2", "System.Numerics.Vectors", false, false)
					},
					{
						"Matrix4x4",
						new WindowsRuntimeProjections.ProjectionInfo("Windows.Foundation.Numerics", "System.Numerics", "Matrix4x4", "System.Numerics.Vectors", false, false)
					},
					{
						"NotifyCollectionChangedAction",
						new WindowsRuntimeProjections.ProjectionInfo("Windows.UI.Xaml.Interop", "System.Collections.Specialized", "NotifyCollectionChangedAction", "System.ObjectModel", false, false)
					},
					{
						"NotifyCollectionChangedEventArgs",
						new WindowsRuntimeProjections.ProjectionInfo("Windows.UI.Xaml.Interop", "System.Collections.Specialized", "NotifyCollectionChangedEventArgs", "System.ObjectModel", false, false)
					},
					{
						"NotifyCollectionChangedEventHandler",
						new WindowsRuntimeProjections.ProjectionInfo("Windows.UI.Xaml.Interop", "System.Collections.Specialized", "NotifyCollectionChangedEventHandler", "System.ObjectModel", false, false)
					},
					{
						"Plane",
						new WindowsRuntimeProjections.ProjectionInfo("Windows.Foundation.Numerics", "System.Numerics", "Plane", "System.Numerics.Vectors", false, false)
					},
					{
						"Point",
						new WindowsRuntimeProjections.ProjectionInfo("Windows.Foundation", "Windows.Foundation", "Point", "System.Runtime.WindowsRuntime", false, false)
					},
					{
						"PropertyChangedEventArgs",
						new WindowsRuntimeProjections.ProjectionInfo("Windows.UI.Xaml.Data", "System.ComponentModel", "PropertyChangedEventArgs", "System.ObjectModel", false, false)
					},
					{
						"PropertyChangedEventHandler",
						new WindowsRuntimeProjections.ProjectionInfo("Windows.UI.Xaml.Data", "System.ComponentModel", "PropertyChangedEventHandler", "System.ObjectModel", false, false)
					},
					{
						"Quaternion",
						new WindowsRuntimeProjections.ProjectionInfo("Windows.Foundation.Numerics", "System.Numerics", "Quaternion", "System.Numerics.Vectors", false, false)
					},
					{
						"Rect",
						new WindowsRuntimeProjections.ProjectionInfo("Windows.Foundation", "Windows.Foundation", "Rect", "System.Runtime.WindowsRuntime", false, false)
					},
					{
						"RepeatBehavior",
						new WindowsRuntimeProjections.ProjectionInfo("Windows.UI.Xaml.Media.Animation", "Windows.UI.Xaml.Media.Animation", "RepeatBehavior", "System.Runtime.WindowsRuntime.UI.Xaml", false, false)
					},
					{
						"RepeatBehaviorType",
						new WindowsRuntimeProjections.ProjectionInfo("Windows.UI.Xaml.Media.Animation", "Windows.UI.Xaml.Media.Animation", "RepeatBehaviorType", "System.Runtime.WindowsRuntime.UI.Xaml", false, false)
					},
					{
						"Size",
						new WindowsRuntimeProjections.ProjectionInfo("Windows.Foundation", "Windows.Foundation", "Size", "System.Runtime.WindowsRuntime", false, false)
					},
					{
						"Thickness",
						new WindowsRuntimeProjections.ProjectionInfo("Windows.UI.Xaml", "Windows.UI.Xaml", "Thickness", "System.Runtime.WindowsRuntime.UI.Xaml", false, false)
					},
					{
						"TimeSpan",
						new WindowsRuntimeProjections.ProjectionInfo("Windows.Foundation", "System", "TimeSpan", "System.Runtime", false, false)
					},
					{
						"TypeName",
						new WindowsRuntimeProjections.ProjectionInfo("Windows.UI.Xaml.Interop", "System", "Type", "System.Runtime", false, false)
					},
					{
						"Uri",
						new WindowsRuntimeProjections.ProjectionInfo("Windows.Foundation", "System", "Uri", "System.Runtime", false, false)
					},
					{
						"Vector2",
						new WindowsRuntimeProjections.ProjectionInfo("Windows.Foundation.Numerics", "System.Numerics", "Vector2", "System.Numerics.Vectors", false, false)
					},
					{
						"Vector3",
						new WindowsRuntimeProjections.ProjectionInfo("Windows.Foundation.Numerics", "System.Numerics", "Vector3", "System.Numerics.Vectors", false, false)
					},
					{
						"Vector4",
						new WindowsRuntimeProjections.ProjectionInfo("Windows.Foundation.Numerics", "System.Numerics", "Vector4", "System.Numerics.Vectors", false, false)
					}
				};
				Interlocked.CompareExchange<Dictionary<string, WindowsRuntimeProjections.ProjectionInfo>>(ref WindowsRuntimeProjections.projections, dictionary, null);
				return WindowsRuntimeProjections.projections;
			}
		}

		// Token: 0x170002C5 RID: 709
		// (get) Token: 0x06000C47 RID: 3143 RVA: 0x00028CBE File Offset: 0x00026EBE
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

		// Token: 0x06000C48 RID: 3144 RVA: 0x00028CDE File Offset: 0x00026EDE
		public WindowsRuntimeProjections(ModuleDefinition module)
		{
			this.module = module;
		}

		// Token: 0x06000C49 RID: 3145 RVA: 0x00028D0C File Offset: 0x00026F0C
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

		// Token: 0x06000C4A RID: 3146 RVA: 0x00028DBC File Offset: 0x00026FBC
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

		// Token: 0x06000C4B RID: 3147 RVA: 0x00028E1C File Offset: 0x0002701C
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
				if (name != null && (name == "Attribute" || name == "MulticastDelegate" || name == "ValueType"))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06000C4C RID: 3148 RVA: 0x00028E9E File Offset: 0x0002709E
		private static bool IsClrImplementationType(TypeDefinition type)
		{
			return (type.Attributes & (TypeAttributes.VisibilityMask | TypeAttributes.SpecialName)) == TypeAttributes.SpecialName && type.Name.StartsWith("<CLR>");
		}

		// Token: 0x06000C4D RID: 3149 RVA: 0x00028EC8 File Offset: 0x000270C8
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

		// Token: 0x06000C4E RID: 3150 RVA: 0x00028FF0 File Offset: 0x000271F0
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

		// Token: 0x06000C4F RID: 3151 RVA: 0x00029030 File Offset: 0x00027230
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

		// Token: 0x06000C50 RID: 3152 RVA: 0x0002907F File Offset: 0x0002727F
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

		// Token: 0x06000C51 RID: 3153 RVA: 0x000290BC File Offset: 0x000272BC
		private static bool IsAttribute(TypeReference type)
		{
			return type.MetadataToken.TokenType == TokenType.TypeRef && type.Name == "Attribute" && type.Namespace == "System";
		}

		// Token: 0x06000C52 RID: 3154 RVA: 0x00029104 File Offset: 0x00027304
		private static bool IsEnum(TypeReference type)
		{
			return type.MetadataToken.TokenType == TokenType.TypeRef && type.Name == "Enum" && type.Namespace == "System";
		}

		// Token: 0x06000C53 RID: 3155 RVA: 0x0002914C File Offset: 0x0002734C
		public static void ApplyProjection(TypeReference type, TypeReferenceProjection projection)
		{
			if (projection == null)
			{
				return;
			}
			TypeReferenceTreatment treatment = projection.Treatment;
			if (treatment - TypeReferenceTreatment.SystemDelegate > 1)
			{
				if (treatment == TypeReferenceTreatment.UseProjectionInfo)
				{
					WindowsRuntimeProjections.ProjectionInfo projectionInfo = WindowsRuntimeProjections.Projections[type.Name];
					type.Name = projectionInfo.ClrName;
					type.Namespace = projectionInfo.ClrNamespace;
					type.Scope = type.Module.Projections.GetAssemblyReference(projectionInfo.ClrAssembly);
				}
			}
			else
			{
				type.Scope = type.Module.Projections.GetAssemblyReference("System.Runtime");
			}
			type.WindowsRuntimeProjection = projection;
		}

		// Token: 0x06000C54 RID: 3156 RVA: 0x000291DC File Offset: 0x000273DC
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

		// Token: 0x06000C55 RID: 3157 RVA: 0x00029228 File Offset: 0x00027428
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

		// Token: 0x06000C56 RID: 3158 RVA: 0x0002937C File Offset: 0x0002757C
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

		// Token: 0x06000C57 RID: 3159 RVA: 0x00029410 File Offset: 0x00027610
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

		// Token: 0x06000C58 RID: 3160 RVA: 0x000294B4 File Offset: 0x000276B4
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

		// Token: 0x06000C59 RID: 3161 RVA: 0x00029500 File Offset: 0x00027700
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

		// Token: 0x06000C5A RID: 3162 RVA: 0x00029562 File Offset: 0x00027762
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

		// Token: 0x06000C5B RID: 3163 RVA: 0x0002958C File Offset: 0x0002778C
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

		// Token: 0x06000C5C RID: 3164 RVA: 0x000295C0 File Offset: 0x000277C0
		public static void Project(MemberReference member)
		{
			bool flag;
			if (!WindowsRuntimeProjections.ImplementsRedirectedInterface(member, out flag) || !flag)
			{
				return;
			}
			WindowsRuntimeProjections.ApplyProjection(member, new MemberReferenceProjection(member, MemberReferenceTreatment.Dispose));
		}

		// Token: 0x06000C5D RID: 3165 RVA: 0x000295E8 File Offset: 0x000277E8
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

		// Token: 0x06000C5E RID: 3166 RVA: 0x000296A6 File Offset: 0x000278A6
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

		// Token: 0x06000C5F RID: 3167 RVA: 0x000296C8 File Offset: 0x000278C8
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

		// Token: 0x06000C60 RID: 3168 RVA: 0x000296FC File Offset: 0x000278FC
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

		// Token: 0x06000C61 RID: 3169 RVA: 0x00029768 File Offset: 0x00027968
		public void RemoveVirtualReferences(Collection<AssemblyNameReference> references)
		{
			WindowsRuntimeProjections.GetCoreLibrary(references).Version = this.corlib_version;
			foreach (AssemblyNameReference assemblyNameReference in this.VirtualReferences)
			{
				references.Remove(assemblyNameReference);
			}
		}

		// Token: 0x06000C62 RID: 3170 RVA: 0x000297A8 File Offset: 0x000279A8
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

		// Token: 0x06000C63 RID: 3171 RVA: 0x000298CC File Offset: 0x00027ACC
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

		// Token: 0x06000C64 RID: 3172 RVA: 0x00029938 File Offset: 0x00027B38
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

		// Token: 0x06000C65 RID: 3173 RVA: 0x00029974 File Offset: 0x00027B74
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

		// Token: 0x06000C66 RID: 3174 RVA: 0x00029A14 File Offset: 0x00027C14
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

		// Token: 0x06000C67 RID: 3175 RVA: 0x00029A98 File Offset: 0x00027C98
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

		// Token: 0x06000C68 RID: 3176 RVA: 0x00029B0C File Offset: 0x00027D0C
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

		// Token: 0x06000C69 RID: 3177 RVA: 0x00029BE0 File Offset: 0x00027DE0
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

		// Token: 0x04000565 RID: 1381
		private static readonly Version version = new Version(4, 0, 0, 0);

		// Token: 0x04000566 RID: 1382
		private static readonly byte[] contract_pk_token = new byte[] { 176, 63, 95, 127, 17, 213, 10, 58 };

		// Token: 0x04000567 RID: 1383
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

		// Token: 0x04000568 RID: 1384
		private static Dictionary<string, WindowsRuntimeProjections.ProjectionInfo> projections;

		// Token: 0x04000569 RID: 1385
		private readonly ModuleDefinition module;

		// Token: 0x0400056A RID: 1386
		private Version corlib_version = new Version(255, 255, 255, 255);

		// Token: 0x0400056B RID: 1387
		private AssemblyNameReference[] virtual_references;

		// Token: 0x0200018B RID: 395
		private struct ProjectionInfo
		{
			// Token: 0x06000C6B RID: 3179 RVA: 0x00029C81 File Offset: 0x00027E81
			public ProjectionInfo(string winrt_namespace, string clr_namespace, string clr_name, string clr_assembly, bool attribute = false, bool disposable = false)
			{
				this.WinRTNamespace = winrt_namespace;
				this.ClrNamespace = clr_namespace;
				this.ClrName = clr_name;
				this.ClrAssembly = clr_assembly;
				this.Attribute = attribute;
				this.Disposable = disposable;
			}

			// Token: 0x0400056C RID: 1388
			public readonly string WinRTNamespace;

			// Token: 0x0400056D RID: 1389
			public readonly string ClrNamespace;

			// Token: 0x0400056E RID: 1390
			public readonly string ClrName;

			// Token: 0x0400056F RID: 1391
			public readonly string ClrAssembly;

			// Token: 0x04000570 RID: 1392
			public readonly bool Attribute;

			// Token: 0x04000571 RID: 1393
			public readonly bool Disposable;
		}
	}
}
