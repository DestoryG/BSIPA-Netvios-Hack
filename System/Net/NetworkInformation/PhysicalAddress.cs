using System;
using System.Text;

namespace System.Net.NetworkInformation
{
	// Token: 0x020002E7 RID: 743
	[global::__DynamicallyInvokable]
	public class PhysicalAddress
	{
		// Token: 0x06001A0E RID: 6670 RVA: 0x0007E73D File Offset: 0x0007C93D
		[global::__DynamicallyInvokable]
		public PhysicalAddress(byte[] address)
		{
			this.address = address;
		}

		// Token: 0x06001A0F RID: 6671 RVA: 0x0007E754 File Offset: 0x0007C954
		[global::__DynamicallyInvokable]
		public override int GetHashCode()
		{
			if (this.changed)
			{
				this.changed = false;
				this.hash = 0;
				int num = this.address.Length & -4;
				int i;
				for (i = 0; i < num; i += 4)
				{
					this.hash ^= (int)this.address[i] | ((int)this.address[i + 1] << 8) | ((int)this.address[i + 2] << 16) | ((int)this.address[i + 3] << 24);
				}
				if ((this.address.Length & 3) != 0)
				{
					int num2 = 0;
					int num3 = 0;
					while (i < this.address.Length)
					{
						num2 |= (int)this.address[i] << num3;
						num3 += 8;
						i++;
					}
					this.hash ^= num2;
				}
			}
			return this.hash;
		}

		// Token: 0x06001A10 RID: 6672 RVA: 0x0007E81C File Offset: 0x0007CA1C
		[global::__DynamicallyInvokable]
		public override bool Equals(object comparand)
		{
			PhysicalAddress physicalAddress = comparand as PhysicalAddress;
			if (physicalAddress == null)
			{
				return false;
			}
			if (this.address.Length != physicalAddress.address.Length)
			{
				return false;
			}
			for (int i = 0; i < physicalAddress.address.Length; i++)
			{
				if (this.address[i] != physicalAddress.address[i])
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06001A11 RID: 6673 RVA: 0x0007E874 File Offset: 0x0007CA74
		[global::__DynamicallyInvokable]
		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (byte b in this.address)
			{
				int num = (b >> 4) & 15;
				for (int j = 0; j < 2; j++)
				{
					if (num < 10)
					{
						stringBuilder.Append((char)(num + 48));
					}
					else
					{
						stringBuilder.Append((char)(num + 55));
					}
					num = (int)(b & 15);
				}
			}
			return stringBuilder.ToString();
		}

		// Token: 0x06001A12 RID: 6674 RVA: 0x0007E8E8 File Offset: 0x0007CAE8
		[global::__DynamicallyInvokable]
		public byte[] GetAddressBytes()
		{
			byte[] array = new byte[this.address.Length];
			Buffer.BlockCopy(this.address, 0, array, 0, this.address.Length);
			return array;
		}

		// Token: 0x06001A13 RID: 6675 RVA: 0x0007E91C File Offset: 0x0007CB1C
		[global::__DynamicallyInvokable]
		public static PhysicalAddress Parse(string address)
		{
			int num = 0;
			bool flag = false;
			if (address == null)
			{
				return PhysicalAddress.None;
			}
			byte[] array;
			if (address.IndexOf('-') >= 0)
			{
				flag = true;
				array = new byte[(address.Length + 1) / 3];
			}
			else
			{
				if (address.Length % 2 > 0)
				{
					throw new FormatException(SR.GetString("net_bad_mac_address"));
				}
				array = new byte[address.Length / 2];
			}
			int num2 = 0;
			int i = 0;
			while (i < address.Length)
			{
				int num3 = (int)address[i];
				if (num3 >= 48 && num3 <= 57)
				{
					num3 -= 48;
					goto IL_00C3;
				}
				if (num3 >= 65 && num3 <= 70)
				{
					num3 -= 55;
					goto IL_00C3;
				}
				if (num3 != 45)
				{
					throw new FormatException(SR.GetString("net_bad_mac_address"));
				}
				if (num != 2)
				{
					throw new FormatException(SR.GetString("net_bad_mac_address"));
				}
				num = 0;
				IL_0100:
				i++;
				continue;
				IL_00C3:
				if (flag && num >= 2)
				{
					throw new FormatException(SR.GetString("net_bad_mac_address"));
				}
				if (num % 2 == 0)
				{
					array[num2] = (byte)(num3 << 4);
				}
				else
				{
					byte[] array2 = array;
					int num4 = num2++;
					array2[num4] |= (byte)num3;
				}
				num++;
				goto IL_0100;
			}
			if (num < 2)
			{
				throw new FormatException(SR.GetString("net_bad_mac_address"));
			}
			return new PhysicalAddress(array);
		}

		// Token: 0x04001A60 RID: 6752
		private byte[] address;

		// Token: 0x04001A61 RID: 6753
		private bool changed = true;

		// Token: 0x04001A62 RID: 6754
		private int hash;

		// Token: 0x04001A63 RID: 6755
		[global::__DynamicallyInvokable]
		public static readonly PhysicalAddress None = new PhysicalAddress(new byte[0]);
	}
}
