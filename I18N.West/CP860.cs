using System;
using System.Runtime.CompilerServices;
using System.Text;
using I18N.Common;

namespace I18N.West
{
	// Token: 0x02000019 RID: 25
	[Serializable]
	public class CP860 : ByteEncoding
	{
		// Token: 0x0600004E RID: 78 RVA: 0x000099A0 File Offset: 0x00007BA0
		public CP860()
			: base(860, CP860.ToChars, "Portuguese (DOS)", "ibm860", "ibm860", "ibm860", false, false, false, false, 1252)
		{
		}

		// Token: 0x0600004F RID: 79 RVA: 0x000099DA File Offset: 0x00007BDA
		public unsafe override int GetByteCountImpl(char* chars, int count)
		{
			if (base.EncoderFallback != null)
			{
				return this.GetBytesImpl(chars, count, null, 0);
			}
			return count;
		}

		// Token: 0x06000050 RID: 80 RVA: 0x000099F4 File Offset: 0x00007BF4
		public unsafe override int GetByteCount(string s)
		{
			if (base.EncoderFallback != null)
			{
				char* ptr = s;
				if (ptr != null)
				{
					ptr += RuntimeHelpers.OffsetToStringData / 2;
				}
				return this.GetBytesImpl(ptr, s.Length, null, 0);
			}
			return s.Length;
		}

		// Token: 0x06000051 RID: 81 RVA: 0x00009A30 File Offset: 0x00007C30
		protected unsafe override void ToBytes(char* chars, int charCount, byte* bytes, int byteCount)
		{
			if (bytes == null)
			{
				throw new ArgumentNullException("bytes");
			}
			this.GetBytesImpl(chars, charCount, bytes, byteCount);
		}

		// Token: 0x06000052 RID: 82 RVA: 0x00009A50 File Offset: 0x00007C50
		public unsafe override int GetBytesImpl(char* chars, int charCount, byte* bytes, int byteCount)
		{
			int num = 0;
			int num2 = 0;
			EncoderFallbackBuffer encoderFallbackBuffer = null;
			while (charCount > 0)
			{
				int num3 = (int)chars[num];
				if (num3 >= 26)
				{
					if (num3 <= 9484)
					{
						if (num3 <= 8319)
						{
							if (num3 <= 937)
							{
								if (num3 <= 920)
								{
									switch (num3)
									{
									case 26:
										num3 = 127;
										goto IL_0F7C;
									case 27:
									case 29:
									case 30:
									case 31:
									case 32:
									case 33:
									case 34:
									case 35:
									case 36:
									case 37:
									case 38:
									case 39:
									case 40:
									case 41:
									case 42:
									case 43:
									case 44:
									case 45:
									case 46:
									case 47:
									case 48:
									case 49:
									case 50:
									case 51:
									case 52:
									case 53:
									case 54:
									case 55:
									case 56:
									case 57:
									case 58:
									case 59:
									case 60:
									case 61:
									case 62:
									case 63:
									case 64:
									case 65:
									case 66:
									case 67:
									case 68:
									case 69:
									case 70:
									case 71:
									case 72:
									case 73:
									case 74:
									case 75:
									case 76:
									case 77:
									case 78:
									case 79:
									case 80:
									case 81:
									case 82:
									case 83:
									case 84:
									case 85:
									case 86:
									case 87:
									case 88:
									case 89:
									case 90:
									case 91:
									case 92:
									case 93:
									case 94:
									case 95:
									case 96:
									case 97:
									case 98:
									case 99:
									case 100:
									case 101:
									case 102:
									case 103:
									case 104:
									case 105:
									case 106:
									case 107:
									case 108:
									case 109:
									case 110:
									case 111:
									case 112:
									case 113:
									case 114:
									case 115:
									case 116:
									case 117:
									case 118:
									case 119:
									case 120:
									case 121:
									case 122:
									case 123:
									case 124:
									case 125:
									case 126:
										goto IL_0F7C;
									case 28:
										num3 = 26;
										goto IL_0F7C;
									case 127:
										num3 = 28;
										goto IL_0F7C;
									case 128:
									case 129:
									case 130:
									case 131:
									case 132:
									case 133:
									case 134:
									case 135:
									case 136:
									case 137:
									case 138:
									case 139:
									case 140:
									case 141:
									case 142:
									case 143:
									case 144:
									case 145:
									case 146:
									case 147:
									case 148:
									case 149:
									case 150:
									case 151:
									case 152:
									case 153:
									case 154:
									case 155:
									case 156:
									case 157:
									case 158:
									case 159:
									case 164:
									case 165:
									case 166:
									case 168:
									case 169:
									case 173:
									case 174:
									case 175:
									case 179:
									case 180:
									case 181:
									case 184:
									case 185:
									case 190:
									case 196:
									case 197:
									case 198:
									case 203:
									case 206:
									case 207:
									case 208:
									case 214:
									case 215:
									case 216:
									case 219:
									case 221:
									case 222:
									case 228:
									case 229:
									case 230:
									case 235:
									case 238:
									case 239:
									case 240:
									case 246:
									case 248:
									case 251:
										break;
									case 160:
										num3 = 255;
										goto IL_0F7C;
									case 161:
										num3 = 173;
										goto IL_0F7C;
									case 162:
										num3 = 155;
										goto IL_0F7C;
									case 163:
										num3 = 156;
										goto IL_0F7C;
									case 167:
										num3 = 21;
										goto IL_0F7C;
									case 170:
										num3 = 166;
										goto IL_0F7C;
									case 171:
										num3 = 174;
										goto IL_0F7C;
									case 172:
										num3 = 170;
										goto IL_0F7C;
									case 176:
										num3 = 248;
										goto IL_0F7C;
									case 177:
										num3 = 241;
										goto IL_0F7C;
									case 178:
										num3 = 253;
										goto IL_0F7C;
									case 182:
										num3 = 20;
										goto IL_0F7C;
									case 183:
										num3 = 250;
										goto IL_0F7C;
									case 186:
										num3 = 167;
										goto IL_0F7C;
									case 187:
										num3 = 175;
										goto IL_0F7C;
									case 188:
										num3 = 172;
										goto IL_0F7C;
									case 189:
										num3 = 171;
										goto IL_0F7C;
									case 191:
										num3 = 168;
										goto IL_0F7C;
									case 192:
										num3 = 145;
										goto IL_0F7C;
									case 193:
										num3 = 134;
										goto IL_0F7C;
									case 194:
										num3 = 143;
										goto IL_0F7C;
									case 195:
										num3 = 142;
										goto IL_0F7C;
									case 199:
										num3 = 128;
										goto IL_0F7C;
									case 200:
										num3 = 146;
										goto IL_0F7C;
									case 201:
										num3 = 144;
										goto IL_0F7C;
									case 202:
										num3 = 137;
										goto IL_0F7C;
									case 204:
										num3 = 152;
										goto IL_0F7C;
									case 205:
										num3 = 139;
										goto IL_0F7C;
									case 209:
										num3 = 165;
										goto IL_0F7C;
									case 210:
										num3 = 169;
										goto IL_0F7C;
									case 211:
										num3 = 159;
										goto IL_0F7C;
									case 212:
										num3 = 140;
										goto IL_0F7C;
									case 213:
										num3 = 153;
										goto IL_0F7C;
									case 217:
										num3 = 157;
										goto IL_0F7C;
									case 218:
										num3 = 150;
										goto IL_0F7C;
									case 220:
										num3 = 154;
										goto IL_0F7C;
									case 223:
										num3 = 225;
										goto IL_0F7C;
									case 224:
										num3 = 133;
										goto IL_0F7C;
									case 225:
										num3 = 160;
										goto IL_0F7C;
									case 226:
										num3 = 131;
										goto IL_0F7C;
									case 227:
										num3 = 132;
										goto IL_0F7C;
									case 231:
										num3 = 135;
										goto IL_0F7C;
									case 232:
										num3 = 138;
										goto IL_0F7C;
									case 233:
										num3 = 130;
										goto IL_0F7C;
									case 234:
										num3 = 136;
										goto IL_0F7C;
									case 236:
										num3 = 141;
										goto IL_0F7C;
									case 237:
										num3 = 161;
										goto IL_0F7C;
									case 241:
										num3 = 164;
										goto IL_0F7C;
									case 242:
										num3 = 149;
										goto IL_0F7C;
									case 243:
										num3 = 162;
										goto IL_0F7C;
									case 244:
										num3 = 147;
										goto IL_0F7C;
									case 245:
										num3 = 148;
										goto IL_0F7C;
									case 247:
										num3 = 246;
										goto IL_0F7C;
									case 249:
										num3 = 151;
										goto IL_0F7C;
									case 250:
										num3 = 163;
										goto IL_0F7C;
									case 252:
										num3 = 129;
										goto IL_0F7C;
									default:
										if (num3 == 915)
										{
											num3 = 226;
											goto IL_0F7C;
										}
										if (num3 == 920)
										{
											num3 = 233;
											goto IL_0F7C;
										}
										break;
									}
								}
								else
								{
									if (num3 == 931)
									{
										num3 = 228;
										goto IL_0F7C;
									}
									if (num3 == 934)
									{
										num3 = 232;
										goto IL_0F7C;
									}
									if (num3 == 937)
									{
										num3 = 234;
										goto IL_0F7C;
									}
								}
							}
							else if (num3 <= 966)
							{
								switch (num3)
								{
								case 945:
									num3 = 224;
									goto IL_0F7C;
								case 946:
								case 947:
									break;
								case 948:
									num3 = 235;
									goto IL_0F7C;
								case 949:
									num3 = 238;
									goto IL_0F7C;
								default:
									if (num3 == 956)
									{
										num3 = 230;
										goto IL_0F7C;
									}
									switch (num3)
									{
									case 960:
										num3 = 227;
										goto IL_0F7C;
									case 963:
										num3 = 229;
										goto IL_0F7C;
									case 964:
										num3 = 231;
										goto IL_0F7C;
									case 966:
										num3 = 237;
										goto IL_0F7C;
									}
									break;
								}
							}
							else
							{
								if (num3 == 8226)
								{
									num3 = 7;
									goto IL_0F7C;
								}
								if (num3 == 8252)
								{
									num3 = 19;
									goto IL_0F7C;
								}
								if (num3 == 8319)
								{
									num3 = 252;
									goto IL_0F7C;
								}
							}
						}
						else if (num3 <= 8776)
						{
							if (num3 <= 8616)
							{
								if (num3 == 8359)
								{
									num3 = 158;
									goto IL_0F7C;
								}
								switch (num3)
								{
								case 8592:
									num3 = 27;
									goto IL_0F7C;
								case 8593:
									num3 = 24;
									goto IL_0F7C;
								case 8594:
									num3 = 26;
									goto IL_0F7C;
								case 8595:
									num3 = 25;
									goto IL_0F7C;
								case 8596:
									num3 = 29;
									goto IL_0F7C;
								case 8597:
									num3 = 18;
									goto IL_0F7C;
								default:
									if (num3 == 8616)
									{
										num3 = 23;
										goto IL_0F7C;
									}
									break;
								}
							}
							else
							{
								switch (num3)
								{
								case 8729:
									num3 = 249;
									goto IL_0F7C;
								case 8730:
									num3 = 251;
									goto IL_0F7C;
								case 8731:
								case 8732:
								case 8733:
									break;
								case 8734:
									num3 = 236;
									goto IL_0F7C;
								case 8735:
									num3 = 28;
									goto IL_0F7C;
								default:
									if (num3 == 8745)
									{
										num3 = 239;
										goto IL_0F7C;
									}
									if (num3 == 8776)
									{
										num3 = 247;
										goto IL_0F7C;
									}
									break;
								}
							}
						}
						else if (num3 <= 8992)
						{
							switch (num3)
							{
							case 8801:
								num3 = 240;
								goto IL_0F7C;
							case 8802:
							case 8803:
								break;
							case 8804:
								num3 = 243;
								goto IL_0F7C;
							case 8805:
								num3 = 242;
								goto IL_0F7C;
							default:
								if (num3 == 8962)
								{
									num3 = 127;
									goto IL_0F7C;
								}
								if (num3 == 8992)
								{
									num3 = 244;
									goto IL_0F7C;
								}
								break;
							}
						}
						else if (num3 <= 9472)
						{
							if (num3 == 8993)
							{
								num3 = 245;
								goto IL_0F7C;
							}
							if (num3 == 9472)
							{
								num3 = 196;
								goto IL_0F7C;
							}
						}
						else
						{
							if (num3 == 9474)
							{
								num3 = 179;
								goto IL_0F7C;
							}
							if (num3 == 9484)
							{
								num3 = 218;
								goto IL_0F7C;
							}
						}
					}
					else if (num3 <= 9632)
					{
						if (num3 <= 9516)
						{
							if (num3 <= 9496)
							{
								if (num3 == 9488)
								{
									num3 = 191;
									goto IL_0F7C;
								}
								if (num3 == 9492)
								{
									num3 = 192;
									goto IL_0F7C;
								}
								if (num3 == 9496)
								{
									num3 = 217;
									goto IL_0F7C;
								}
							}
							else
							{
								if (num3 == 9500)
								{
									num3 = 195;
									goto IL_0F7C;
								}
								if (num3 == 9508)
								{
									num3 = 180;
									goto IL_0F7C;
								}
								if (num3 == 9516)
								{
									num3 = 194;
									goto IL_0F7C;
								}
							}
						}
						else if (num3 <= 9604)
						{
							switch (num3)
							{
							case 9524:
								num3 = 193;
								goto IL_0F7C;
							case 9525:
							case 9526:
							case 9527:
							case 9528:
							case 9529:
							case 9530:
							case 9531:
							case 9533:
							case 9534:
							case 9535:
							case 9536:
							case 9537:
							case 9538:
							case 9539:
							case 9540:
							case 9541:
							case 9542:
							case 9543:
							case 9544:
							case 9545:
							case 9546:
							case 9547:
							case 9548:
							case 9549:
							case 9550:
							case 9551:
								break;
							case 9532:
								num3 = 197;
								goto IL_0F7C;
							case 9552:
								num3 = 205;
								goto IL_0F7C;
							case 9553:
								num3 = 186;
								goto IL_0F7C;
							case 9554:
								num3 = 213;
								goto IL_0F7C;
							case 9555:
								num3 = 214;
								goto IL_0F7C;
							case 9556:
								num3 = 201;
								goto IL_0F7C;
							case 9557:
								num3 = 184;
								goto IL_0F7C;
							case 9558:
								num3 = 183;
								goto IL_0F7C;
							case 9559:
								num3 = 187;
								goto IL_0F7C;
							case 9560:
								num3 = 212;
								goto IL_0F7C;
							case 9561:
								num3 = 211;
								goto IL_0F7C;
							case 9562:
								num3 = 200;
								goto IL_0F7C;
							case 9563:
								num3 = 190;
								goto IL_0F7C;
							case 9564:
								num3 = 189;
								goto IL_0F7C;
							case 9565:
								num3 = 188;
								goto IL_0F7C;
							case 9566:
								num3 = 198;
								goto IL_0F7C;
							case 9567:
								num3 = 199;
								goto IL_0F7C;
							case 9568:
								num3 = 204;
								goto IL_0F7C;
							case 9569:
								num3 = 181;
								goto IL_0F7C;
							case 9570:
								num3 = 182;
								goto IL_0F7C;
							case 9571:
								num3 = 185;
								goto IL_0F7C;
							case 9572:
								num3 = 209;
								goto IL_0F7C;
							case 9573:
								num3 = 210;
								goto IL_0F7C;
							case 9574:
								num3 = 203;
								goto IL_0F7C;
							case 9575:
								num3 = 207;
								goto IL_0F7C;
							case 9576:
								num3 = 208;
								goto IL_0F7C;
							case 9577:
								num3 = 202;
								goto IL_0F7C;
							case 9578:
								num3 = 216;
								goto IL_0F7C;
							case 9579:
								num3 = 215;
								goto IL_0F7C;
							case 9580:
								num3 = 206;
								goto IL_0F7C;
							default:
								if (num3 == 9600)
								{
									num3 = 223;
									goto IL_0F7C;
								}
								if (num3 == 9604)
								{
									num3 = 220;
									goto IL_0F7C;
								}
								break;
							}
						}
						else
						{
							if (num3 == 9608)
							{
								num3 = 219;
								goto IL_0F7C;
							}
							switch (num3)
							{
							case 9612:
								num3 = 221;
								goto IL_0F7C;
							case 9613:
							case 9614:
							case 9615:
								break;
							case 9616:
								num3 = 222;
								goto IL_0F7C;
							case 9617:
								num3 = 176;
								goto IL_0F7C;
							case 9618:
								num3 = 177;
								goto IL_0F7C;
							case 9619:
								num3 = 178;
								goto IL_0F7C;
							default:
								if (num3 == 9632)
								{
									num3 = 254;
									goto IL_0F7C;
								}
								break;
							}
						}
					}
					else if (num3 <= 9675)
					{
						if (num3 <= 9658)
						{
							if (num3 == 9644)
							{
								num3 = 22;
								goto IL_0F7C;
							}
							if (num3 == 9650)
							{
								num3 = 30;
								goto IL_0F7C;
							}
							if (num3 == 9658)
							{
								num3 = 16;
								goto IL_0F7C;
							}
						}
						else
						{
							if (num3 == 9660)
							{
								num3 = 31;
								goto IL_0F7C;
							}
							if (num3 == 9668)
							{
								num3 = 17;
								goto IL_0F7C;
							}
							if (num3 == 9675)
							{
								num3 = 9;
								goto IL_0F7C;
							}
						}
					}
					else if (num3 <= 9794)
					{
						if (num3 == 9688)
						{
							num3 = 8;
							goto IL_0F7C;
						}
						if (num3 == 9689)
						{
							num3 = 10;
							goto IL_0F7C;
						}
						switch (num3)
						{
						case 9786:
							num3 = 1;
							goto IL_0F7C;
						case 9787:
							num3 = 2;
							goto IL_0F7C;
						case 9788:
							num3 = 15;
							goto IL_0F7C;
						case 9792:
							num3 = 12;
							goto IL_0F7C;
						case 9794:
							num3 = 11;
							goto IL_0F7C;
						}
					}
					else if (num3 <= 9834)
					{
						switch (num3)
						{
						case 9824:
							num3 = 6;
							goto IL_0F7C;
						case 9825:
						case 9826:
						case 9828:
							break;
						case 9827:
							num3 = 5;
							goto IL_0F7C;
						case 9829:
							num3 = 3;
							goto IL_0F7C;
						case 9830:
							num3 = 4;
							goto IL_0F7C;
						default:
							if (num3 == 9834)
							{
								num3 = 13;
								goto IL_0F7C;
							}
							break;
						}
					}
					else
					{
						if (num3 == 9835)
						{
							num3 = 14;
							goto IL_0F7C;
						}
						switch (num3)
						{
						case 65512:
							num3 = 179;
							goto IL_0F7C;
						case 65513:
							num3 = 27;
							goto IL_0F7C;
						case 65514:
							num3 = 24;
							goto IL_0F7C;
						case 65515:
							num3 = 26;
							goto IL_0F7C;
						case 65516:
							num3 = 25;
							goto IL_0F7C;
						case 65517:
							num3 = 254;
							goto IL_0F7C;
						case 65518:
							num3 = 9;
							goto IL_0F7C;
						}
					}
					if (num3 < 65281 || num3 > 65374)
					{
						base.HandleFallback(ref encoderFallbackBuffer, chars, ref num, ref charCount, bytes, ref num2, ref byteCount);
						num++;
						charCount--;
						continue;
					}
					num3 -= 65248;
				}
				IL_0F7C:
				if (bytes != null)
				{
					bytes[num2] = (byte)num3;
				}
				num2++;
				byteCount--;
				num++;
				charCount--;
			}
			return num2;
		}

		// Token: 0x04000035 RID: 53
		private static readonly char[] ToChars = new char[]
		{
			'\0', '\u0001', '\u0002', '\u0003', '\u0004', '\u0005', '\u0006', '\a', '\b', '\t',
			'\n', '\v', '\f', '\r', '\u000e', '\u000f', '\u0010', '\u0011', '\u0012', '\u0013',
			'\u0014', '\u0015', '\u0016', '\u0017', '\u0018', '\u0019', '\u001c', '\u001b', '\u007f', '\u001d',
			'\u001e', '\u001f', ' ', '!', '"', '#', '$', '%', '&', '\'',
			'(', ')', '*', '+', ',', '-', '.', '/', '0', '1',
			'2', '3', '4', '5', '6', '7', '8', '9', ':', ';',
			'<', '=', '>', '?', '@', 'A', 'B', 'C', 'D', 'E',
			'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O',
			'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y',
			'Z', '[', '\\', ']', '^', '_', '`', 'a', 'b', 'c',
			'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm',
			'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w',
			'x', 'y', 'z', '{', '|', '}', '~', '\u001a', 'Ç', 'ü',
			'é', 'â', 'ã', 'à', 'Á', 'ç', 'ê', 'Ê', 'è', 'Í',
			'Ô', 'ì', 'Ã', 'Â', 'É', 'À', 'È', 'ô', 'õ', 'ò',
			'Ú', 'ù', 'Ì', 'Õ', 'Ü', '¢', '£', 'Ù', '₧', 'Ó',
			'á', 'í', 'ó', 'ú', 'ñ', 'Ñ', 'ª', 'º', '¿', 'Ò',
			'¬', '½', '¼', '¡', '«', '»', '░', '▒', '▓', '│',
			'┤', '╡', '╢', '╖', '╕', '╣', '║', '╗', '╝', '╜',
			'╛', '┐', '└', '┴', '┬', '├', '─', '┼', '╞', '╟',
			'╚', '╔', '╩', '╦', '╠', '═', '╬', '╧', '╨', '╤',
			'╥', '╙', '╘', '╒', '╓', '╫', '╪', '┘', '┌', '█',
			'▄', '▌', '▐', '▀', 'α', 'ß', 'Γ', 'π', 'Σ', 'σ',
			'μ', 'τ', 'Φ', 'Θ', 'Ω', 'δ', '∞', 'φ', 'ε', '∩',
			'≡', '±', '≥', '≤', '⌠', '⌡', '÷', '≈', '°', '∙',
			'·', '√', 'ⁿ', '²', '■', '\u00a0'
		};
	}
}
