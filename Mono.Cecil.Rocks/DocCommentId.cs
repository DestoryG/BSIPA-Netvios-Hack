using System;
using System.Collections.Generic;
using System.Text;

namespace Mono.Cecil.Rocks
{
	// Token: 0x02000002 RID: 2
	public class DocCommentId
	{
		// Token: 0x06000001 RID: 1 RVA: 0x00002050 File Offset: 0x00000250
		private DocCommentId()
		{
			this.id = new StringBuilder();
		}

		// Token: 0x06000002 RID: 2 RVA: 0x00002063 File Offset: 0x00000263
		private void WriteField(FieldDefinition field)
		{
			this.WriteDefinition('F', field);
		}

		// Token: 0x06000003 RID: 3 RVA: 0x0000206E File Offset: 0x0000026E
		private void WriteEvent(EventDefinition @event)
		{
			this.WriteDefinition('E', @event);
		}

		// Token: 0x06000004 RID: 4 RVA: 0x00002079 File Offset: 0x00000279
		private void WriteType(TypeDefinition type)
		{
			this.id.Append('T').Append(':');
			this.WriteTypeFullName(type, false);
		}

		// Token: 0x06000005 RID: 5 RVA: 0x00002098 File Offset: 0x00000298
		private void WriteMethod(MethodDefinition method)
		{
			this.WriteDefinition('M', method);
			if (method.HasGenericParameters)
			{
				this.id.Append('`').Append('`');
				this.id.Append(method.GenericParameters.Count);
			}
			if (method.HasParameters)
			{
				this.WriteParameters(method.Parameters);
			}
			if (DocCommentId.IsConversionOperator(method))
			{
				this.WriteReturnType(method);
			}
		}

		// Token: 0x06000006 RID: 6 RVA: 0x00002105 File Offset: 0x00000305
		private static bool IsConversionOperator(MethodDefinition self)
		{
			if (self == null)
			{
				throw new ArgumentNullException("self");
			}
			return self.IsSpecialName && (self.Name == "op_Explicit" || self.Name == "op_Implicit");
		}

		// Token: 0x06000007 RID: 7 RVA: 0x00002143 File Offset: 0x00000343
		private void WriteReturnType(MethodDefinition method)
		{
			this.id.Append('~');
			this.WriteTypeSignature(method.ReturnType);
		}

		// Token: 0x06000008 RID: 8 RVA: 0x0000215F File Offset: 0x0000035F
		private void WriteProperty(PropertyDefinition property)
		{
			this.WriteDefinition('P', property);
			if (property.HasParameters)
			{
				this.WriteParameters(property.Parameters);
			}
		}

		// Token: 0x06000009 RID: 9 RVA: 0x0000217E File Offset: 0x0000037E
		private void WriteParameters(IList<ParameterDefinition> parameters)
		{
			this.id.Append('(');
			this.WriteList<ParameterDefinition>(parameters, delegate(ParameterDefinition p)
			{
				this.WriteTypeSignature(p.ParameterType);
			});
			this.id.Append(')');
		}

		// Token: 0x0600000A RID: 10 RVA: 0x000021B0 File Offset: 0x000003B0
		private void WriteTypeSignature(TypeReference type)
		{
			MetadataType metadataType = type.MetadataType;
			switch (metadataType)
			{
			case MetadataType.Pointer:
				this.WriteTypeSignature(((PointerType)type).ElementType);
				this.id.Append('*');
				return;
			case MetadataType.ByReference:
				this.WriteTypeSignature(((ByReferenceType)type).ElementType);
				this.id.Append('@');
				return;
			case MetadataType.ValueType:
			case MetadataType.Class:
				break;
			case MetadataType.Var:
				this.id.Append('`');
				this.id.Append(((GenericParameter)type).Position);
				return;
			case MetadataType.Array:
				this.WriteArrayTypeSignature((ArrayType)type);
				return;
			case MetadataType.GenericInstance:
				this.WriteGenericInstanceTypeSignature((GenericInstanceType)type);
				return;
			default:
				switch (metadataType)
				{
				case MetadataType.FunctionPointer:
					this.WriteFunctionPointerTypeSignature((FunctionPointerType)type);
					return;
				case MetadataType.MVar:
					this.id.Append('`').Append('`');
					this.id.Append(((GenericParameter)type).Position);
					return;
				case MetadataType.RequiredModifier:
					this.WriteModiferTypeSignature((RequiredModifierType)type, '|');
					return;
				case MetadataType.OptionalModifier:
					this.WriteModiferTypeSignature((OptionalModifierType)type, '!');
					return;
				}
				break;
			}
			this.WriteTypeFullName(type, false);
		}

		// Token: 0x0600000B RID: 11 RVA: 0x000022F0 File Offset: 0x000004F0
		private void WriteGenericInstanceTypeSignature(GenericInstanceType type)
		{
			if (type.ElementType.IsTypeSpecification())
			{
				throw new NotSupportedException();
			}
			this.WriteTypeFullName(type.ElementType, true);
			this.id.Append('{');
			this.WriteList<TypeReference>(type.GenericArguments, new Action<TypeReference>(this.WriteTypeSignature));
			this.id.Append('}');
		}

		// Token: 0x0600000C RID: 12 RVA: 0x00002354 File Offset: 0x00000554
		private void WriteList<T>(IList<T> list, Action<T> action)
		{
			for (int i = 0; i < list.Count; i++)
			{
				if (i > 0)
				{
					this.id.Append(',');
				}
				action(list[i]);
			}
		}

		// Token: 0x0600000D RID: 13 RVA: 0x00002391 File Offset: 0x00000591
		private void WriteModiferTypeSignature(IModifierType type, char id)
		{
			this.WriteTypeSignature(type.ElementType);
			this.id.Append(id);
			this.WriteTypeSignature(type.ModifierType);
		}

		// Token: 0x0600000E RID: 14 RVA: 0x000023B8 File Offset: 0x000005B8
		private void WriteFunctionPointerTypeSignature(FunctionPointerType type)
		{
			this.id.Append("=FUNC:");
			this.WriteTypeSignature(type.ReturnType);
			if (type.HasParameters)
			{
				this.WriteParameters(type.Parameters);
			}
		}

		// Token: 0x0600000F RID: 15 RVA: 0x000023EC File Offset: 0x000005EC
		private void WriteArrayTypeSignature(ArrayType type)
		{
			this.WriteTypeSignature(type.ElementType);
			if (type.IsVector)
			{
				this.id.Append("[]");
				return;
			}
			this.id.Append("[");
			this.WriteList<ArrayDimension>(type.Dimensions, delegate(ArrayDimension dimension)
			{
				if (dimension.LowerBound != null)
				{
					this.id.Append(dimension.LowerBound.Value);
				}
				this.id.Append(':');
				if (dimension.UpperBound != null)
				{
					this.id.Append(dimension.UpperBound.Value - (dimension.LowerBound.GetValueOrDefault() + 1));
				}
			});
			this.id.Append("]");
		}

		// Token: 0x06000010 RID: 16 RVA: 0x00002459 File Offset: 0x00000659
		private void WriteDefinition(char id, IMemberDefinition member)
		{
			this.id.Append(id).Append(':');
			this.WriteTypeFullName(member.DeclaringType, false);
			this.id.Append('.');
			this.WriteItemName(member.Name);
		}

		// Token: 0x06000011 RID: 17 RVA: 0x00002498 File Offset: 0x00000698
		private void WriteTypeFullName(TypeReference type, bool stripGenericArity = false)
		{
			if (type.DeclaringType != null)
			{
				this.WriteTypeFullName(type.DeclaringType, false);
				this.id.Append('.');
			}
			if (!string.IsNullOrEmpty(type.Namespace))
			{
				this.id.Append(type.Namespace);
				this.id.Append('.');
			}
			string text = type.Name;
			if (stripGenericArity)
			{
				int num = text.LastIndexOf('`');
				if (num > 0)
				{
					text = text.Substring(0, num);
				}
			}
			this.id.Append(text);
		}

		// Token: 0x06000012 RID: 18 RVA: 0x00002522 File Offset: 0x00000722
		private void WriteItemName(string name)
		{
			this.id.Append(name.Replace('.', '#').Replace('<', '{').Replace('>', '}'));
		}

		// Token: 0x06000013 RID: 19 RVA: 0x0000254C File Offset: 0x0000074C
		public override string ToString()
		{
			return this.id.ToString();
		}

		// Token: 0x06000014 RID: 20 RVA: 0x0000255C File Offset: 0x0000075C
		public static string GetDocCommentId(IMemberDefinition member)
		{
			if (member == null)
			{
				throw new ArgumentNullException("member");
			}
			DocCommentId docCommentId = new DocCommentId();
			TokenType tokenType = member.MetadataToken.TokenType;
			if (tokenType <= TokenType.Field)
			{
				if (tokenType == TokenType.TypeDef)
				{
					docCommentId.WriteType((TypeDefinition)member);
					goto IL_00A9;
				}
				if (tokenType == TokenType.Field)
				{
					docCommentId.WriteField((FieldDefinition)member);
					goto IL_00A9;
				}
			}
			else
			{
				if (tokenType == TokenType.Method)
				{
					docCommentId.WriteMethod((MethodDefinition)member);
					goto IL_00A9;
				}
				if (tokenType == TokenType.Event)
				{
					docCommentId.WriteEvent((EventDefinition)member);
					goto IL_00A9;
				}
				if (tokenType == TokenType.Property)
				{
					docCommentId.WriteProperty((PropertyDefinition)member);
					goto IL_00A9;
				}
			}
			throw new NotSupportedException(member.FullName);
			IL_00A9:
			return docCommentId.ToString();
		}

		// Token: 0x04000001 RID: 1
		private StringBuilder id;
	}
}
