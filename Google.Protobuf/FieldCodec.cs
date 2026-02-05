using System;
using System.Collections.Generic;

namespace Google.Protobuf
{
	// Token: 0x0200000F RID: 15
	public static class FieldCodec
	{
		// Token: 0x060000FA RID: 250 RVA: 0x000051A2 File Offset: 0x000033A2
		public static FieldCodec<string> ForString(uint tag)
		{
			return FieldCodec.ForString(tag, "");
		}

		// Token: 0x060000FB RID: 251 RVA: 0x000051AF File Offset: 0x000033AF
		public static FieldCodec<ByteString> ForBytes(uint tag)
		{
			return FieldCodec.ForBytes(tag, ByteString.Empty);
		}

		// Token: 0x060000FC RID: 252 RVA: 0x000051BC File Offset: 0x000033BC
		public static FieldCodec<bool> ForBool(uint tag)
		{
			return FieldCodec.ForBool(tag, false);
		}

		// Token: 0x060000FD RID: 253 RVA: 0x000051C5 File Offset: 0x000033C5
		public static FieldCodec<int> ForInt32(uint tag)
		{
			return FieldCodec.ForInt32(tag, 0);
		}

		// Token: 0x060000FE RID: 254 RVA: 0x000051CE File Offset: 0x000033CE
		public static FieldCodec<int> ForSInt32(uint tag)
		{
			return FieldCodec.ForSInt32(tag, 0);
		}

		// Token: 0x060000FF RID: 255 RVA: 0x000051D7 File Offset: 0x000033D7
		public static FieldCodec<uint> ForFixed32(uint tag)
		{
			return FieldCodec.ForFixed32(tag, 0U);
		}

		// Token: 0x06000100 RID: 256 RVA: 0x000051E0 File Offset: 0x000033E0
		public static FieldCodec<int> ForSFixed32(uint tag)
		{
			return FieldCodec.ForSFixed32(tag, 0);
		}

		// Token: 0x06000101 RID: 257 RVA: 0x000051E9 File Offset: 0x000033E9
		public static FieldCodec<uint> ForUInt32(uint tag)
		{
			return FieldCodec.ForUInt32(tag, 0U);
		}

		// Token: 0x06000102 RID: 258 RVA: 0x000051F2 File Offset: 0x000033F2
		public static FieldCodec<long> ForInt64(uint tag)
		{
			return FieldCodec.ForInt64(tag, 0L);
		}

		// Token: 0x06000103 RID: 259 RVA: 0x000051FC File Offset: 0x000033FC
		public static FieldCodec<long> ForSInt64(uint tag)
		{
			return FieldCodec.ForSInt64(tag, 0L);
		}

		// Token: 0x06000104 RID: 260 RVA: 0x00005206 File Offset: 0x00003406
		public static FieldCodec<ulong> ForFixed64(uint tag)
		{
			return FieldCodec.ForFixed64(tag, 0UL);
		}

		// Token: 0x06000105 RID: 261 RVA: 0x00005210 File Offset: 0x00003410
		public static FieldCodec<long> ForSFixed64(uint tag)
		{
			return FieldCodec.ForSFixed64(tag, 0L);
		}

		// Token: 0x06000106 RID: 262 RVA: 0x0000521A File Offset: 0x0000341A
		public static FieldCodec<ulong> ForUInt64(uint tag)
		{
			return FieldCodec.ForUInt64(tag, 0UL);
		}

		// Token: 0x06000107 RID: 263 RVA: 0x00005224 File Offset: 0x00003424
		public static FieldCodec<float> ForFloat(uint tag)
		{
			return FieldCodec.ForFloat(tag, 0f);
		}

		// Token: 0x06000108 RID: 264 RVA: 0x00005231 File Offset: 0x00003431
		public static FieldCodec<double> ForDouble(uint tag)
		{
			return FieldCodec.ForDouble(tag, 0.0);
		}

		// Token: 0x06000109 RID: 265 RVA: 0x00005244 File Offset: 0x00003444
		public static FieldCodec<T> ForEnum<T>(uint tag, Func<T, int> toInt32, Func<int, T> fromInt32)
		{
			return FieldCodec.ForEnum<T>(tag, toInt32, fromInt32, default(T));
		}

		// Token: 0x0600010A RID: 266 RVA: 0x00005264 File Offset: 0x00003464
		public static FieldCodec<string> ForString(uint tag, string defaultValue)
		{
			return new FieldCodec<string>((CodedInputStream input) => input.ReadString(), delegate(CodedOutputStream output, string value)
			{
				output.WriteString(value);
			}, new Func<string, int>(CodedOutputStream.ComputeStringSize), tag, defaultValue);
		}

		// Token: 0x0600010B RID: 267 RVA: 0x000052C4 File Offset: 0x000034C4
		public static FieldCodec<ByteString> ForBytes(uint tag, ByteString defaultValue)
		{
			return new FieldCodec<ByteString>((CodedInputStream input) => input.ReadBytes(), delegate(CodedOutputStream output, ByteString value)
			{
				output.WriteBytes(value);
			}, new Func<ByteString, int>(CodedOutputStream.ComputeBytesSize), tag, defaultValue);
		}

		// Token: 0x0600010C RID: 268 RVA: 0x00005324 File Offset: 0x00003524
		public static FieldCodec<bool> ForBool(uint tag, bool defaultValue)
		{
			return new FieldCodec<bool>((CodedInputStream input) => input.ReadBool(), delegate(CodedOutputStream output, bool value)
			{
				output.WriteBool(value);
			}, 1, tag, defaultValue);
		}

		// Token: 0x0600010D RID: 269 RVA: 0x00005378 File Offset: 0x00003578
		public static FieldCodec<int> ForInt32(uint tag, int defaultValue)
		{
			return new FieldCodec<int>((CodedInputStream input) => input.ReadInt32(), delegate(CodedOutputStream output, int value)
			{
				output.WriteInt32(value);
			}, new Func<int, int>(CodedOutputStream.ComputeInt32Size), tag, defaultValue);
		}

		// Token: 0x0600010E RID: 270 RVA: 0x000053D8 File Offset: 0x000035D8
		public static FieldCodec<int> ForSInt32(uint tag, int defaultValue)
		{
			return new FieldCodec<int>((CodedInputStream input) => input.ReadSInt32(), delegate(CodedOutputStream output, int value)
			{
				output.WriteSInt32(value);
			}, new Func<int, int>(CodedOutputStream.ComputeSInt32Size), tag, defaultValue);
		}

		// Token: 0x0600010F RID: 271 RVA: 0x00005438 File Offset: 0x00003638
		public static FieldCodec<uint> ForFixed32(uint tag, uint defaultValue)
		{
			return new FieldCodec<uint>((CodedInputStream input) => input.ReadFixed32(), delegate(CodedOutputStream output, uint value)
			{
				output.WriteFixed32(value);
			}, 4, tag, defaultValue);
		}

		// Token: 0x06000110 RID: 272 RVA: 0x0000548C File Offset: 0x0000368C
		public static FieldCodec<int> ForSFixed32(uint tag, int defaultValue)
		{
			return new FieldCodec<int>((CodedInputStream input) => input.ReadSFixed32(), delegate(CodedOutputStream output, int value)
			{
				output.WriteSFixed32(value);
			}, 4, tag, defaultValue);
		}

		// Token: 0x06000111 RID: 273 RVA: 0x000054E0 File Offset: 0x000036E0
		public static FieldCodec<uint> ForUInt32(uint tag, uint defaultValue)
		{
			return new FieldCodec<uint>((CodedInputStream input) => input.ReadUInt32(), delegate(CodedOutputStream output, uint value)
			{
				output.WriteUInt32(value);
			}, new Func<uint, int>(CodedOutputStream.ComputeUInt32Size), tag, defaultValue);
		}

		// Token: 0x06000112 RID: 274 RVA: 0x00005540 File Offset: 0x00003740
		public static FieldCodec<long> ForInt64(uint tag, long defaultValue)
		{
			return new FieldCodec<long>((CodedInputStream input) => input.ReadInt64(), delegate(CodedOutputStream output, long value)
			{
				output.WriteInt64(value);
			}, new Func<long, int>(CodedOutputStream.ComputeInt64Size), tag, defaultValue);
		}

		// Token: 0x06000113 RID: 275 RVA: 0x000055A0 File Offset: 0x000037A0
		public static FieldCodec<long> ForSInt64(uint tag, long defaultValue)
		{
			return new FieldCodec<long>((CodedInputStream input) => input.ReadSInt64(), delegate(CodedOutputStream output, long value)
			{
				output.WriteSInt64(value);
			}, new Func<long, int>(CodedOutputStream.ComputeSInt64Size), tag, defaultValue);
		}

		// Token: 0x06000114 RID: 276 RVA: 0x00005600 File Offset: 0x00003800
		public static FieldCodec<ulong> ForFixed64(uint tag, ulong defaultValue)
		{
			return new FieldCodec<ulong>((CodedInputStream input) => input.ReadFixed64(), delegate(CodedOutputStream output, ulong value)
			{
				output.WriteFixed64(value);
			}, 8, tag, defaultValue);
		}

		// Token: 0x06000115 RID: 277 RVA: 0x00005654 File Offset: 0x00003854
		public static FieldCodec<long> ForSFixed64(uint tag, long defaultValue)
		{
			return new FieldCodec<long>((CodedInputStream input) => input.ReadSFixed64(), delegate(CodedOutputStream output, long value)
			{
				output.WriteSFixed64(value);
			}, 8, tag, defaultValue);
		}

		// Token: 0x06000116 RID: 278 RVA: 0x000056A8 File Offset: 0x000038A8
		public static FieldCodec<ulong> ForUInt64(uint tag, ulong defaultValue)
		{
			return new FieldCodec<ulong>((CodedInputStream input) => input.ReadUInt64(), delegate(CodedOutputStream output, ulong value)
			{
				output.WriteUInt64(value);
			}, new Func<ulong, int>(CodedOutputStream.ComputeUInt64Size), tag, defaultValue);
		}

		// Token: 0x06000117 RID: 279 RVA: 0x00005708 File Offset: 0x00003908
		public static FieldCodec<float> ForFloat(uint tag, float defaultValue)
		{
			return new FieldCodec<float>((CodedInputStream input) => input.ReadFloat(), delegate(CodedOutputStream output, float value)
			{
				output.WriteFloat(value);
			}, 4, tag, defaultValue);
		}

		// Token: 0x06000118 RID: 280 RVA: 0x0000575C File Offset: 0x0000395C
		public static FieldCodec<double> ForDouble(uint tag, double defaultValue)
		{
			return new FieldCodec<double>((CodedInputStream input) => input.ReadDouble(), delegate(CodedOutputStream output, double value)
			{
				output.WriteDouble(value);
			}, 8, tag, defaultValue);
		}

		// Token: 0x06000119 RID: 281 RVA: 0x000057B0 File Offset: 0x000039B0
		public static FieldCodec<T> ForEnum<T>(uint tag, Func<T, int> toInt32, Func<int, T> fromInt32, T defaultValue)
		{
			return new FieldCodec<T>((CodedInputStream input) => fromInt32(input.ReadEnum()), delegate(CodedOutputStream output, T value)
			{
				output.WriteEnum(toInt32(value));
			}, (T value) => CodedOutputStream.ComputeEnumSize(toInt32(value)), tag, defaultValue);
		}

		// Token: 0x0600011A RID: 282 RVA: 0x000057FC File Offset: 0x000039FC
		public static FieldCodec<T> ForMessage<T>(uint tag, MessageParser<T> parser) where T : class, IMessage<T>
		{
			return new FieldCodec<T>(delegate(CodedInputStream input)
			{
				T t = parser.CreateTemplate();
				input.ReadMessage(t);
				return t;
			}, delegate(CodedOutputStream output, T value)
			{
				output.WriteMessage(value);
			}, delegate(CodedInputStream i, ref T v)
			{
				if (v == null)
				{
					v = parser.CreateTemplate();
				}
				i.ReadMessage(v);
			}, delegate(ref T v, T v2)
			{
				if (v2 == null)
				{
					return false;
				}
				if (v == null)
				{
					v = v2.Clone();
				}
				else
				{
					v.MergeFrom(v2);
				}
				return true;
			}, (T message) => CodedOutputStream.ComputeMessageSize(message), tag, 0U);
		}

		// Token: 0x0600011B RID: 283 RVA: 0x00005894 File Offset: 0x00003A94
		public static FieldCodec<T> ForGroup<T>(uint startTag, uint endTag, MessageParser<T> parser) where T : class, IMessage<T>
		{
			return new FieldCodec<T>(delegate(CodedInputStream input)
			{
				T t = parser.CreateTemplate();
				input.ReadGroup(t);
				return t;
			}, delegate(CodedOutputStream output, T value)
			{
				output.WriteGroup(value);
			}, delegate(CodedInputStream i, ref T v)
			{
				if (v == null)
				{
					v = parser.CreateTemplate();
				}
				i.ReadGroup(v);
			}, delegate(ref T v, T v2)
			{
				if (v2 == null)
				{
					return v == null;
				}
				if (v == null)
				{
					v = v2.Clone();
				}
				else
				{
					v.MergeFrom(v2);
				}
				return true;
			}, (T message) => CodedOutputStream.ComputeGroupSize(message), startTag, endTag);
		}

		// Token: 0x0600011C RID: 284 RVA: 0x0000592C File Offset: 0x00003B2C
		public static FieldCodec<T> ForClassWrapper<T>(uint tag) where T : class
		{
			FieldCodec<T> nestedCodec = FieldCodec.WrapperCodecs.GetCodec<T>();
			return new FieldCodec<T>((CodedInputStream input) => FieldCodec.WrapperCodecs.Read<T>(input, nestedCodec), delegate(CodedOutputStream output, T value)
			{
				FieldCodec.WrapperCodecs.Write<T>(output, value, nestedCodec);
			}, delegate(CodedInputStream i, ref T v)
			{
				v = FieldCodec.WrapperCodecs.Read<T>(i, nestedCodec);
			}, delegate(ref T v, T v2)
			{
				v = v2;
				return v == null;
			}, (T value) => FieldCodec.WrapperCodecs.CalculateSize<T>(value, nestedCodec), tag, 0U, default(T));
		}

		// Token: 0x0600011D RID: 285 RVA: 0x000059AC File Offset: 0x00003BAC
		public static FieldCodec<T?> ForStructWrapper<T>(uint tag) where T : struct
		{
			FieldCodec<T> nestedCodec = FieldCodec.WrapperCodecs.GetCodec<T>();
			return new FieldCodec<T?>(FieldCodec.WrapperCodecs.GetReader<T>(), delegate(CodedOutputStream output, T? value)
			{
				FieldCodec.WrapperCodecs.Write<T>(output, value.Value, nestedCodec);
			}, delegate(CodedInputStream i, ref T? v)
			{
				v = new T?(FieldCodec.WrapperCodecs.Read<T>(i, nestedCodec));
			}, delegate(ref T? v, T? v2)
			{
				if (v2 != null)
				{
					v = v2;
				}
				return v != null;
			}, delegate(T? value)
			{
				if (value != null)
				{
					return FieldCodec.WrapperCodecs.CalculateSize<T>(value.Value, nestedCodec);
				}
				return 0;
			}, tag, 0U, null);
		}

		// Token: 0x02000090 RID: 144
		private static class WrapperCodecs
		{
			// Token: 0x060008C4 RID: 2244 RVA: 0x0001EA0C File Offset: 0x0001CC0C
			internal static FieldCodec<T> GetCodec<T>()
			{
				object obj;
				if (!FieldCodec.WrapperCodecs.Codecs.TryGetValue(typeof(T), out obj))
				{
					string text = "Invalid type argument requested for wrapper codec: ";
					Type typeFromHandle = typeof(T);
					throw new InvalidOperationException(text + ((typeFromHandle != null) ? typeFromHandle.ToString() : null));
				}
				return (FieldCodec<T>)obj;
			}

			// Token: 0x060008C5 RID: 2245 RVA: 0x0001EA60 File Offset: 0x0001CC60
			internal static Func<CodedInputStream, T?> GetReader<T>() where T : struct
			{
				object obj;
				if (!FieldCodec.WrapperCodecs.Readers.TryGetValue(typeof(T), out obj))
				{
					string text = "Invalid type argument requested for wrapper reader: ";
					Type typeFromHandle = typeof(T);
					throw new InvalidOperationException(text + ((typeFromHandle != null) ? typeFromHandle.ToString() : null));
				}
				if (obj == null)
				{
					FieldCodec<T> nestedCoded = FieldCodec.WrapperCodecs.GetCodec<T>();
					return (CodedInputStream input) => new T?(FieldCodec.WrapperCodecs.Read<T>(input, nestedCoded));
				}
				return (Func<CodedInputStream, T?>)obj;
			}

			// Token: 0x060008C6 RID: 2246 RVA: 0x0001EAD0 File Offset: 0x0001CCD0
			internal static T Read<T>(CodedInputStream input, FieldCodec<T> codec)
			{
				int num = input.ReadLength();
				int num2 = input.PushLimit(num);
				T t = codec.DefaultValue;
				uint num3;
				while ((num3 = input.ReadTag()) != 0U)
				{
					if (num3 == codec.Tag)
					{
						t = codec.Read(input);
					}
					else
					{
						input.SkipLastField();
					}
				}
				input.CheckReadEndOfStreamTag();
				input.PopLimit(num2);
				return t;
			}

			// Token: 0x060008C7 RID: 2247 RVA: 0x0001EB26 File Offset: 0x0001CD26
			internal static void Write<T>(CodedOutputStream output, T value, FieldCodec<T> codec)
			{
				output.WriteLength(codec.CalculateSizeWithTag(value));
				codec.WriteTagAndValue(output, value);
			}

			// Token: 0x060008C8 RID: 2248 RVA: 0x0001EB40 File Offset: 0x0001CD40
			internal static int CalculateSize<T>(T value, FieldCodec<T> codec)
			{
				int num = codec.CalculateSizeWithTag(value);
				return CodedOutputStream.ComputeLengthSize(num) + num;
			}

			// Token: 0x04000364 RID: 868
			private static readonly Dictionary<Type, object> Codecs = new Dictionary<Type, object>
			{
				{
					typeof(bool),
					FieldCodec.ForBool(WireFormat.MakeTag(1, WireFormat.WireType.Varint))
				},
				{
					typeof(int),
					FieldCodec.ForInt32(WireFormat.MakeTag(1, WireFormat.WireType.Varint))
				},
				{
					typeof(long),
					FieldCodec.ForInt64(WireFormat.MakeTag(1, WireFormat.WireType.Varint))
				},
				{
					typeof(uint),
					FieldCodec.ForUInt32(WireFormat.MakeTag(1, WireFormat.WireType.Varint))
				},
				{
					typeof(ulong),
					FieldCodec.ForUInt64(WireFormat.MakeTag(1, WireFormat.WireType.Varint))
				},
				{
					typeof(float),
					FieldCodec.ForFloat(WireFormat.MakeTag(1, WireFormat.WireType.Fixed32))
				},
				{
					typeof(double),
					FieldCodec.ForDouble(WireFormat.MakeTag(1, WireFormat.WireType.Fixed64))
				},
				{
					typeof(string),
					FieldCodec.ForString(WireFormat.MakeTag(1, WireFormat.WireType.LengthDelimited))
				},
				{
					typeof(ByteString),
					FieldCodec.ForBytes(WireFormat.MakeTag(1, WireFormat.WireType.LengthDelimited))
				}
			};

			// Token: 0x04000365 RID: 869
			private static readonly Dictionary<Type, object> Readers = new Dictionary<Type, object>
			{
				{
					typeof(bool),
					new Func<CodedInputStream, bool?>(CodedInputStream.ReadBoolWrapper)
				},
				{
					typeof(int),
					new Func<CodedInputStream, int?>(CodedInputStream.ReadInt32Wrapper)
				},
				{
					typeof(long),
					new Func<CodedInputStream, long?>(CodedInputStream.ReadInt64Wrapper)
				},
				{
					typeof(uint),
					new Func<CodedInputStream, uint?>(CodedInputStream.ReadUInt32Wrapper)
				},
				{
					typeof(ulong),
					new Func<CodedInputStream, ulong?>(CodedInputStream.ReadUInt64Wrapper)
				},
				{
					typeof(float),
					BitConverter.IsLittleEndian ? new Func<CodedInputStream, float?>(CodedInputStream.ReadFloatWrapperLittleEndian) : new Func<CodedInputStream, float?>(CodedInputStream.ReadFloatWrapperSlow)
				},
				{
					typeof(double),
					BitConverter.IsLittleEndian ? new Func<CodedInputStream, double?>(CodedInputStream.ReadDoubleWrapperLittleEndian) : new Func<CodedInputStream, double?>(CodedInputStream.ReadDoubleWrapperSlow)
				},
				{
					typeof(string),
					null
				},
				{
					typeof(ByteString),
					null
				}
			};
		}
	}
}
