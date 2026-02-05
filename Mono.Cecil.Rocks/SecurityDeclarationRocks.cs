using System;
using System.Security;
using System.Security.Permissions;

namespace Mono.Cecil.Rocks
{
	// Token: 0x0200000A RID: 10
	public static class SecurityDeclarationRocks
	{
		// Token: 0x0600003F RID: 63 RVA: 0x0000389C File Offset: 0x00001A9C
		public static PermissionSet ToPermissionSet(this SecurityDeclaration self)
		{
			if (self == null)
			{
				throw new ArgumentNullException("self");
			}
			PermissionSet permissionSet;
			if (SecurityDeclarationRocks.TryProcessPermissionSetAttribute(self, out permissionSet))
			{
				return permissionSet;
			}
			return SecurityDeclarationRocks.CreatePermissionSet(self);
		}

		// Token: 0x06000040 RID: 64 RVA: 0x000038CC File Offset: 0x00001ACC
		private static bool TryProcessPermissionSetAttribute(SecurityDeclaration declaration, out PermissionSet set)
		{
			set = null;
			if (!declaration.HasSecurityAttributes && declaration.SecurityAttributes.Count != 1)
			{
				return false;
			}
			SecurityAttribute securityAttribute = declaration.SecurityAttributes[0];
			if (!securityAttribute.AttributeType.IsTypeOf("System.Security.Permissions", "PermissionSetAttribute"))
			{
				return false;
			}
			PermissionSetAttribute permissionSetAttribute = new PermissionSetAttribute((SecurityAction)declaration.Action);
			CustomAttributeNamedArgument customAttributeNamedArgument = securityAttribute.Properties[0];
			string text = (string)customAttributeNamedArgument.Argument.Value;
			string name = customAttributeNamedArgument.Name;
			if (!(name == "XML"))
			{
				if (!(name == "Name"))
				{
					throw new NotImplementedException(customAttributeNamedArgument.Name);
				}
				permissionSetAttribute.Name = text;
			}
			else
			{
				permissionSetAttribute.XML = text;
			}
			set = permissionSetAttribute.CreatePermissionSet();
			return true;
		}

		// Token: 0x06000041 RID: 65 RVA: 0x00003998 File Offset: 0x00001B98
		private static PermissionSet CreatePermissionSet(SecurityDeclaration declaration)
		{
			PermissionSet permissionSet = new PermissionSet(PermissionState.None);
			foreach (SecurityAttribute securityAttribute in declaration.SecurityAttributes)
			{
				IPermission permission = SecurityDeclarationRocks.CreatePermission(declaration, securityAttribute);
				permissionSet.AddPermission(permission);
			}
			return permissionSet;
		}

		// Token: 0x06000042 RID: 66 RVA: 0x000039FC File Offset: 0x00001BFC
		private static IPermission CreatePermission(SecurityDeclaration declaration, SecurityAttribute attribute)
		{
			Type type = Type.GetType(attribute.AttributeType.FullName);
			if (type == null)
			{
				throw new ArgumentException("attribute");
			}
			SecurityAttribute securityAttribute = SecurityDeclarationRocks.CreateSecurityAttribute(type, declaration);
			if (securityAttribute == null)
			{
				throw new InvalidOperationException();
			}
			SecurityDeclarationRocks.CompleteSecurityAttribute(securityAttribute, attribute);
			return securityAttribute.CreatePermission();
		}

		// Token: 0x06000043 RID: 67 RVA: 0x00003A48 File Offset: 0x00001C48
		private static void CompleteSecurityAttribute(SecurityAttribute security_attribute, SecurityAttribute attribute)
		{
			if (attribute.HasFields)
			{
				SecurityDeclarationRocks.CompleteSecurityAttributeFields(security_attribute, attribute);
			}
			if (attribute.HasProperties)
			{
				SecurityDeclarationRocks.CompleteSecurityAttributeProperties(security_attribute, attribute);
			}
		}

		// Token: 0x06000044 RID: 68 RVA: 0x00003A68 File Offset: 0x00001C68
		private static void CompleteSecurityAttributeFields(SecurityAttribute security_attribute, SecurityAttribute attribute)
		{
			Type type = security_attribute.GetType();
			foreach (CustomAttributeNamedArgument customAttributeNamedArgument in attribute.Fields)
			{
				type.GetField(customAttributeNamedArgument.Name).SetValue(security_attribute, customAttributeNamedArgument.Argument.Value);
			}
		}

		// Token: 0x06000045 RID: 69 RVA: 0x00003AE0 File Offset: 0x00001CE0
		private static void CompleteSecurityAttributeProperties(SecurityAttribute security_attribute, SecurityAttribute attribute)
		{
			Type type = security_attribute.GetType();
			foreach (CustomAttributeNamedArgument customAttributeNamedArgument in attribute.Properties)
			{
				type.GetProperty(customAttributeNamedArgument.Name).SetValue(security_attribute, customAttributeNamedArgument.Argument.Value, null);
			}
		}

		// Token: 0x06000046 RID: 70 RVA: 0x00003B58 File Offset: 0x00001D58
		private static SecurityAttribute CreateSecurityAttribute(Type attribute_type, SecurityDeclaration declaration)
		{
			SecurityAttribute securityAttribute;
			try
			{
				securityAttribute = (SecurityAttribute)Activator.CreateInstance(attribute_type, new object[] { (SecurityAction)declaration.Action });
			}
			catch (MissingMethodException)
			{
				securityAttribute = (SecurityAttribute)Activator.CreateInstance(attribute_type, new object[0]);
			}
			return securityAttribute;
		}

		// Token: 0x06000047 RID: 71 RVA: 0x00003BB0 File Offset: 0x00001DB0
		public static SecurityDeclaration ToSecurityDeclaration(this PermissionSet self, SecurityAction action, ModuleDefinition module)
		{
			if (self == null)
			{
				throw new ArgumentNullException("self");
			}
			if (module == null)
			{
				throw new ArgumentNullException("module");
			}
			SecurityDeclaration securityDeclaration = new SecurityDeclaration(action);
			SecurityAttribute securityAttribute = new SecurityAttribute(module.TypeSystem.LookupType("System.Security.Permissions", "PermissionSetAttribute"));
			securityAttribute.Properties.Add(new CustomAttributeNamedArgument("XML", new CustomAttributeArgument(module.TypeSystem.String, self.ToXml().ToString())));
			securityDeclaration.SecurityAttributes.Add(securityAttribute);
			return securityDeclaration;
		}
	}
}
