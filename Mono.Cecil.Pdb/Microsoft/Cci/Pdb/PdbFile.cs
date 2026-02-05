using System;
using System.Collections.Generic;
using System.IO;
using Mono.Cecil.Cil;

namespace Microsoft.Cci.Pdb
{
	// Token: 0x020000DB RID: 219
	internal class PdbFile
	{
		// Token: 0x0600018C RID: 396 RVA: 0x000037D5 File Offset: 0x000019D5
		private PdbFile()
		{
		}

		// Token: 0x0600018D RID: 397 RVA: 0x00004F88 File Offset: 0x00003188
		private static void LoadGuidStream(BitAccess bits, out Guid doctype, out Guid language, out Guid vendor)
		{
			bits.ReadGuid(out language);
			bits.ReadGuid(out vendor);
			bits.ReadGuid(out doctype);
		}

		// Token: 0x0600018E RID: 398 RVA: 0x00004FA0 File Offset: 0x000031A0
		private static Dictionary<string, int> LoadNameIndex(BitAccess bits, out int age, out Guid guid)
		{
			Dictionary<string, int> dictionary = new Dictionary<string, int>();
			int num;
			bits.ReadInt32(out num);
			int num2;
			bits.ReadInt32(out num2);
			bits.ReadInt32(out age);
			bits.ReadGuid(out guid);
			int num3;
			bits.ReadInt32(out num3);
			int position = bits.Position;
			int num4 = bits.Position + num3;
			bits.Position = num4;
			int num5;
			bits.ReadInt32(out num5);
			int num6;
			bits.ReadInt32(out num6);
			BitSet bitSet = new BitSet(bits);
			BitSet bitSet2 = new BitSet(bits);
			if (!bitSet2.IsEmpty)
			{
				throw new PdbDebugException("Unsupported PDB deleted bitset is not empty.", new object[0]);
			}
			int num7 = 0;
			for (int i = 0; i < num6; i++)
			{
				if (bitSet.IsSet(i))
				{
					int num8;
					bits.ReadInt32(out num8);
					int num9;
					bits.ReadInt32(out num9);
					int position2 = bits.Position;
					bits.Position = position + num8;
					string text;
					bits.ReadCString(out text);
					bits.Position = position2;
					dictionary.Add(text.ToUpperInvariant(), num9);
					num7++;
				}
			}
			if (num7 != num5)
			{
				throw new PdbDebugException("Count mismatch. ({0} != {1})", new object[] { num7, num5 });
			}
			return dictionary;
		}

		// Token: 0x0600018F RID: 399 RVA: 0x000050C0 File Offset: 0x000032C0
		private static IntHashTable LoadNameStream(BitAccess bits)
		{
			IntHashTable intHashTable = new IntHashTable();
			uint num;
			bits.ReadUInt32(out num);
			int num2;
			bits.ReadInt32(out num2);
			int num3;
			bits.ReadInt32(out num3);
			if (num != 4026462206U || num2 != 1)
			{
				throw new PdbDebugException("Unsupported Name Stream version. (sig={0:x8}, ver={1})", new object[] { num, num2 });
			}
			int position = bits.Position;
			int num4 = bits.Position + num3;
			bits.Position = num4;
			int num5;
			bits.ReadInt32(out num5);
			num4 = bits.Position;
			for (int i = 0; i < num5; i++)
			{
				int num6;
				bits.ReadInt32(out num6);
				if (num6 != 0)
				{
					int position2 = bits.Position;
					bits.Position = position + num6;
					string text;
					bits.ReadCString(out text);
					bits.Position = position2;
					intHashTable.Add(num6, text);
				}
			}
			bits.Position = num4;
			return intHashTable;
		}

		// Token: 0x06000190 RID: 400 RVA: 0x00005198 File Offset: 0x00003398
		private static int FindFunction(PdbFunction[] funcs, ushort sec, uint off)
		{
			PdbFunction pdbFunction = new PdbFunction
			{
				segment = (uint)sec,
				address = off
			};
			return Array.BinarySearch(funcs, pdbFunction, PdbFunction.byAddress);
		}

		// Token: 0x06000191 RID: 401 RVA: 0x000051C8 File Offset: 0x000033C8
		private static void LoadManagedLines(PdbFunction[] funcs, IntHashTable names, BitAccess bits, MsfDirectory dir, Dictionary<string, int> nameIndex, PdbReader reader, uint limit)
		{
			Array.Sort(funcs, PdbFunction.byAddressAndToken);
			int position = bits.Position;
			IntHashTable intHashTable = PdbFile.ReadSourceFileInfo(bits, limit, names, dir, nameIndex, reader);
			bits.Position = position;
			while ((long)bits.Position < (long)((ulong)limit))
			{
				int num;
				bits.ReadInt32(out num);
				int num2;
				bits.ReadInt32(out num2);
				int num3 = bits.Position + num2;
				DEBUG_S_SUBSECTION debug_S_SUBSECTION = (DEBUG_S_SUBSECTION)num;
				if (debug_S_SUBSECTION == DEBUG_S_SUBSECTION.LINES)
				{
					CV_LineSection cv_LineSection;
					bits.ReadUInt32(out cv_LineSection.off);
					bits.ReadUInt16(out cv_LineSection.sec);
					bits.ReadUInt16(out cv_LineSection.flags);
					bits.ReadUInt32(out cv_LineSection.cod);
					int i = PdbFile.FindFunction(funcs, cv_LineSection.sec, cv_LineSection.off);
					if (i >= 0)
					{
						PdbFunction pdbFunction = funcs[i];
						if (pdbFunction.lines == null)
						{
							while (i > 0)
							{
								PdbFunction pdbFunction2 = funcs[i - 1];
								if (pdbFunction2.lines != null || pdbFunction2.segment != (uint)cv_LineSection.sec || pdbFunction2.address != cv_LineSection.off)
								{
									break;
								}
								pdbFunction = pdbFunction2;
								i--;
							}
						}
						else
						{
							while (i < funcs.Length - 1 && pdbFunction.lines != null)
							{
								PdbFunction pdbFunction3 = funcs[i + 1];
								if (pdbFunction3.segment != (uint)cv_LineSection.sec || pdbFunction3.address != cv_LineSection.off)
								{
									break;
								}
								pdbFunction = pdbFunction3;
								i++;
							}
						}
						if (pdbFunction.lines == null)
						{
							int position2 = bits.Position;
							int num4 = 0;
							while (bits.Position < num3)
							{
								CV_SourceFile cv_SourceFile;
								bits.ReadUInt32(out cv_SourceFile.index);
								bits.ReadUInt32(out cv_SourceFile.count);
								bits.ReadUInt32(out cv_SourceFile.linsiz);
								int num5 = (int)(cv_SourceFile.count * (8U + (((cv_LineSection.flags & 1) != 0) ? 4U : 0U)));
								bits.Position += num5;
								num4++;
							}
							pdbFunction.lines = new PdbLines[num4];
							int num6 = 0;
							bits.Position = position2;
							while (bits.Position < num3)
							{
								CV_SourceFile cv_SourceFile2;
								bits.ReadUInt32(out cv_SourceFile2.index);
								bits.ReadUInt32(out cv_SourceFile2.count);
								bits.ReadUInt32(out cv_SourceFile2.linsiz);
								PdbLines pdbLines = new PdbLines((PdbSource)intHashTable[(int)cv_SourceFile2.index], cv_SourceFile2.count);
								pdbFunction.lines[num6++] = pdbLines;
								PdbLine[] lines = pdbLines.lines;
								int position3 = bits.Position;
								int num7 = bits.Position + (int)(8U * cv_SourceFile2.count);
								int num8 = 0;
								while ((long)num8 < (long)((ulong)cv_SourceFile2.count))
								{
									CV_Column cv_Column = default(CV_Column);
									bits.Position = position3 + 8 * num8;
									CV_Line cv_Line;
									bits.ReadUInt32(out cv_Line.offset);
									bits.ReadUInt32(out cv_Line.flags);
									uint num9 = cv_Line.flags & 16777215U;
									uint num10 = (cv_Line.flags & 2130706432U) >> 24;
									if ((cv_LineSection.flags & 1) != 0)
									{
										bits.Position = num7 + 4 * num8;
										bits.ReadUInt16(out cv_Column.offColumnStart);
										bits.ReadUInt16(out cv_Column.offColumnEnd);
									}
									lines[num8] = new PdbLine(cv_Line.offset, num9, cv_Column.offColumnStart, num9 + num10, cv_Column.offColumnEnd);
									num8++;
								}
							}
						}
					}
				}
				bits.Position = num3;
			}
		}

		// Token: 0x06000192 RID: 402 RVA: 0x00005518 File Offset: 0x00003718
		private static void LoadFuncsFromDbiModule(BitAccess bits, DbiModuleInfo info, IntHashTable names, List<PdbFunction> funcList, bool readStrings, MsfDirectory dir, Dictionary<string, int> nameIndex, PdbReader reader)
		{
			bits.Position = 0;
			int num;
			bits.ReadInt32(out num);
			if (num != 4)
			{
				throw new PdbDebugException("Invalid signature. (sig={0})", new object[] { num });
			}
			bits.Position = 4;
			PdbFunction[] array = PdbFunction.LoadManagedFunctions(bits, (uint)info.cbSyms, readStrings);
			if (array != null)
			{
				bits.Position = info.cbSyms + info.cbOldLines;
				PdbFile.LoadManagedLines(array, names, bits, dir, nameIndex, reader, (uint)(info.cbSyms + info.cbOldLines + info.cbLines));
				for (int i = 0; i < array.Length; i++)
				{
					funcList.Add(array[i]);
				}
			}
		}

		// Token: 0x06000193 RID: 403 RVA: 0x000055BC File Offset: 0x000037BC
		private static void LoadDbiStream(BitAccess bits, out DbiModuleInfo[] modules, out DbiDbgHdr header, bool readStrings)
		{
			DbiHeader dbiHeader = new DbiHeader(bits);
			header = default(DbiDbgHdr);
			List<DbiModuleInfo> list = new List<DbiModuleInfo>();
			int num = bits.Position + dbiHeader.gpmodiSize;
			while (bits.Position < num)
			{
				DbiModuleInfo dbiModuleInfo = new DbiModuleInfo(bits, readStrings);
				list.Add(dbiModuleInfo);
			}
			if (bits.Position != num)
			{
				throw new PdbDebugException("Error reading DBI stream, pos={0} != {1}", new object[] { bits.Position, num });
			}
			if (list.Count > 0)
			{
				modules = list.ToArray();
			}
			else
			{
				modules = null;
			}
			bits.Position += dbiHeader.secconSize;
			bits.Position += dbiHeader.secmapSize;
			bits.Position += dbiHeader.filinfSize;
			bits.Position += dbiHeader.tsmapSize;
			bits.Position += dbiHeader.ecinfoSize;
			num = bits.Position + dbiHeader.dbghdrSize;
			if (dbiHeader.dbghdrSize > 0)
			{
				header = new DbiDbgHdr(bits);
			}
			bits.Position = num;
		}

		// Token: 0x06000194 RID: 404 RVA: 0x000056D8 File Offset: 0x000038D8
		internal static PdbFunction[] LoadFunctions(Stream read, out Dictionary<uint, PdbTokenLine> tokenToSourceMapping, out string sourceServerData, out int age, out Guid guid)
		{
			tokenToSourceMapping = new Dictionary<uint, PdbTokenLine>();
			BitAccess bitAccess = new BitAccess(524288);
			PdbFileHeader pdbFileHeader = new PdbFileHeader(read, bitAccess);
			PdbReader pdbReader = new PdbReader(read, pdbFileHeader.pageSize);
			MsfDirectory msfDirectory = new MsfDirectory(pdbReader, pdbFileHeader, bitAccess);
			DbiModuleInfo[] array = null;
			msfDirectory.streams[1].Read(pdbReader, bitAccess);
			Dictionary<string, int> dictionary = PdbFile.LoadNameIndex(bitAccess, out age, out guid);
			int num;
			if (!dictionary.TryGetValue("/NAMES", out num))
			{
				throw new PdbException("No `name' stream", new object[0]);
			}
			msfDirectory.streams[num].Read(pdbReader, bitAccess);
			IntHashTable intHashTable = PdbFile.LoadNameStream(bitAccess);
			int num2;
			if (!dictionary.TryGetValue("SRCSRV", out num2))
			{
				sourceServerData = string.Empty;
			}
			else
			{
				DataStream dataStream = msfDirectory.streams[num2];
				byte[] array2 = new byte[dataStream.contentSize];
				dataStream.Read(pdbReader, bitAccess);
				sourceServerData = bitAccess.ReadBString(array2.Length);
			}
			msfDirectory.streams[3].Read(pdbReader, bitAccess);
			DbiDbgHdr dbiDbgHdr;
			PdbFile.LoadDbiStream(bitAccess, out array, out dbiDbgHdr, true);
			List<PdbFunction> list = new List<PdbFunction>();
			if (array != null)
			{
				foreach (DbiModuleInfo dbiModuleInfo in array)
				{
					if (dbiModuleInfo.stream > 0)
					{
						msfDirectory.streams[(int)dbiModuleInfo.stream].Read(pdbReader, bitAccess);
						if (dbiModuleInfo.moduleName == "TokenSourceLineInfo")
						{
							PdbFile.LoadTokenToSourceInfo(bitAccess, dbiModuleInfo, intHashTable, msfDirectory, dictionary, pdbReader, tokenToSourceMapping);
						}
						else
						{
							PdbFile.LoadFuncsFromDbiModule(bitAccess, dbiModuleInfo, intHashTable, list, true, msfDirectory, dictionary, pdbReader);
						}
					}
				}
			}
			PdbFunction[] array3 = list.ToArray();
			if (dbiDbgHdr.snTokenRidMap != 0 && dbiDbgHdr.snTokenRidMap != 65535)
			{
				msfDirectory.streams[(int)dbiDbgHdr.snTokenRidMap].Read(pdbReader, bitAccess);
				uint[] array4 = new uint[msfDirectory.streams[(int)dbiDbgHdr.snTokenRidMap].Length / 4];
				bitAccess.ReadUInt32(array4);
				foreach (PdbFunction pdbFunction in array3)
				{
					pdbFunction.token = 100663296U | array4[(int)(pdbFunction.token & 16777215U)];
				}
			}
			Array.Sort(array3, PdbFunction.byAddressAndToken);
			return array3;
		}

		// Token: 0x06000195 RID: 405 RVA: 0x000058E8 File Offset: 0x00003AE8
		private static void LoadTokenToSourceInfo(BitAccess bits, DbiModuleInfo module, IntHashTable names, MsfDirectory dir, Dictionary<string, int> nameIndex, PdbReader reader, Dictionary<uint, PdbTokenLine> tokenToSourceMapping)
		{
			bits.Position = 0;
			int num;
			bits.ReadInt32(out num);
			if (num != 4)
			{
				throw new PdbDebugException("Invalid signature. (sig={0})", new object[] { num });
			}
			bits.Position = 4;
			while (bits.Position < module.cbSyms)
			{
				ushort num2;
				bits.ReadUInt16(out num2);
				int position = bits.Position;
				int num3 = bits.Position + (int)num2;
				bits.Position = position;
				ushort num4;
				bits.ReadUInt16(out num4);
				SYM sym = (SYM)num4;
				if (sym != SYM.S_END)
				{
					if (sym == SYM.S_OEM)
					{
						OemSymbol oemSymbol;
						bits.ReadGuid(out oemSymbol.idOem);
						bits.ReadUInt32(out oemSymbol.typind);
						if (!(oemSymbol.idOem == PdbFunction.msilMetaData))
						{
							throw new PdbDebugException("OEM section: guid={0} ti={1}", new object[] { oemSymbol.idOem, oemSymbol.typind });
						}
						if (bits.ReadString() == "TSLI")
						{
							uint num5;
							bits.ReadUInt32(out num5);
							uint num6;
							bits.ReadUInt32(out num6);
							uint num7;
							bits.ReadUInt32(out num7);
							uint num8;
							bits.ReadUInt32(out num8);
							uint num9;
							bits.ReadUInt32(out num9);
							uint num10;
							bits.ReadUInt32(out num10);
							PdbTokenLine nextLine;
							if (!tokenToSourceMapping.TryGetValue(num5, out nextLine))
							{
								tokenToSourceMapping.Add(num5, new PdbTokenLine(num5, num6, num7, num8, num9, num10));
							}
							else
							{
								while (nextLine.nextLine != null)
								{
									nextLine = nextLine.nextLine;
								}
								nextLine.nextLine = new PdbTokenLine(num5, num6, num7, num8, num9, num10);
							}
						}
						bits.Position = num3;
					}
					else
					{
						bits.Position = num3;
					}
				}
				else
				{
					bits.Position = num3;
				}
			}
			bits.Position = module.cbSyms + module.cbOldLines;
			int num11 = module.cbSyms + module.cbOldLines + module.cbLines;
			IntHashTable intHashTable = PdbFile.ReadSourceFileInfo(bits, (uint)num11, names, dir, nameIndex, reader);
			foreach (PdbTokenLine pdbTokenLine in tokenToSourceMapping.Values)
			{
				pdbTokenLine.sourceFile = (PdbSource)intHashTable[(int)pdbTokenLine.file_id];
			}
		}

		// Token: 0x06000196 RID: 406 RVA: 0x00005B28 File Offset: 0x00003D28
		private static IntHashTable ReadSourceFileInfo(BitAccess bits, uint limit, IntHashTable names, MsfDirectory dir, Dictionary<string, int> nameIndex, PdbReader reader)
		{
			IntHashTable intHashTable = new IntHashTable();
			int position = bits.Position;
			while ((long)bits.Position < (long)((ulong)limit))
			{
				int num;
				bits.ReadInt32(out num);
				int num2;
				bits.ReadInt32(out num2);
				int position2 = bits.Position;
				int num3 = bits.Position + num2;
				DEBUG_S_SUBSECTION debug_S_SUBSECTION = (DEBUG_S_SUBSECTION)num;
				if (debug_S_SUBSECTION != DEBUG_S_SUBSECTION.FILECHKSMS)
				{
					bits.Position = num3;
				}
				else
				{
					while (bits.Position < num3)
					{
						int num4 = bits.Position - position2;
						CV_FileCheckSum cv_FileCheckSum;
						bits.ReadUInt32(out cv_FileCheckSum.name);
						bits.ReadUInt8(out cv_FileCheckSum.len);
						bits.ReadUInt8(out cv_FileCheckSum.type);
						string text = (string)names[(int)cv_FileCheckSum.name];
						Guid guid = DocumentType.Text.ToGuid();
						Guid empty = Guid.Empty;
						Guid empty2 = Guid.Empty;
						int num5;
						if (nameIndex.TryGetValue("/SRC/FILES/" + text.ToUpperInvariant(), out num5))
						{
							BitAccess bitAccess = new BitAccess(256);
							dir.streams[num5].Read(reader, bitAccess);
							PdbFile.LoadGuidStream(bitAccess, out guid, out empty, out empty2);
						}
						PdbSource pdbSource = new PdbSource(text, guid, empty, empty2);
						intHashTable.Add(num4, pdbSource);
						bits.Position += (int)cv_FileCheckSum.len;
						bits.Align(4);
					}
					bits.Position = num3;
				}
			}
			return intHashTable;
		}
	}
}
