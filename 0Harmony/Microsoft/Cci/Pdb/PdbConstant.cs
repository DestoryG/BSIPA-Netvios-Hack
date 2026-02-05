using System;

namespace Microsoft.Cci.Pdb
{
	// Token: 0x02000304 RID: 772
	internal class PdbConstant
	{
		// Token: 0x060011F8 RID: 4600 RVA: 0x0003AD58 File Offset: 0x00038F58
		internal PdbConstant(string name, uint token, object value)
		{
			this.name = name;
			this.token = token;
			this.value = value;
		}

		// Token: 0x060011F9 RID: 4601 RVA: 0x0003AD78 File Offset: 0x00038F78
		internal PdbConstant(BitAccess bits)
		{
			bits.ReadUInt32(out this.token);
			byte b;
			bits.ReadUInt8(out b);
			byte b2;
			bits.ReadUInt8(out b2);
			if (b2 == 0)
			{
				this.value = b;
			}
			else if (b2 == 128)
			{
				switch (b)
				{
				case 0:
				{
					sbyte b3;
					bits.ReadInt8(out b3);
					this.value = b3;
					break;
				}
				case 1:
				{
					short num;
					bits.ReadInt16(out num);
					this.value = num;
					break;
				}
				case 2:
				{
					ushort num2;
					bits.ReadUInt16(out num2);
					this.value = num2;
					break;
				}
				case 3:
				{
					int num3;
					bits.ReadInt32(out num3);
					this.value = num3;
					break;
				}
				case 4:
				{
					uint num4;
					bits.ReadUInt32(out num4);
					this.value = num4;
					break;
				}
				case 5:
					this.value = bits.ReadFloat();
					break;
				case 6:
					this.value = bits.ReadDouble();
					break;
				case 7:
				case 8:
				case 11:
				case 12:
				case 13:
				case 14:
				case 15:
					break;
				case 9:
				{
					long num5;
					bits.ReadInt64(out num5);
					this.value = num5;
					break;
				}
				case 10:
				{
					ulong num6;
					bits.ReadUInt64(out num6);
					this.value = num6;
					break;
				}
				case 16:
				{
					string text;
					bits.ReadBString(out text);
					this.value = text;
					break;
				}
				default:
					if (b == 25)
					{
						this.value = bits.ReadDecimal();
					}
					break;
				}
			}
			bits.ReadCString(out this.name);
		}

		// Token: 0x04000EDE RID: 3806
		internal string name;

		// Token: 0x04000EDF RID: 3807
		internal uint token;

		// Token: 0x04000EE0 RID: 3808
		internal object value;
	}
}
