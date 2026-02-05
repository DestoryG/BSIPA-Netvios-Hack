using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;

namespace SongCore.Utilities
{
	// Token: 0x02000019 RID: 25
	internal class Unzip
	{
		// Token: 0x14000005 RID: 5
		// (add) Token: 0x06000148 RID: 328 RVA: 0x00006668 File Offset: 0x00004868
		// (remove) Token: 0x06000149 RID: 329 RVA: 0x000066A0 File Offset: 0x000048A0
		public event EventHandler<Unzip.FileProgressEventArgs> ExtractProgress;

		// Token: 0x0600014A RID: 330 RVA: 0x000066D5 File Offset: 0x000048D5
		public Unzip(string fileName)
			: this(File.OpenRead(fileName))
		{
		}

		// Token: 0x0600014B RID: 331 RVA: 0x000066E3 File Offset: 0x000048E3
		public Unzip(Stream stream)
		{
			this.Stream = stream;
			this.Reader = new BinaryReader(this.Stream);
		}

		// Token: 0x1700004F RID: 79
		// (get) Token: 0x0600014C RID: 332 RVA: 0x00006703 File Offset: 0x00004903
		// (set) Token: 0x0600014D RID: 333 RVA: 0x0000670B File Offset: 0x0000490B
		private Stream Stream { get; set; }

		// Token: 0x17000050 RID: 80
		// (get) Token: 0x0600014E RID: 334 RVA: 0x00006714 File Offset: 0x00004914
		// (set) Token: 0x0600014F RID: 335 RVA: 0x0000671C File Offset: 0x0000491C
		private BinaryReader Reader { get; set; }

		// Token: 0x06000150 RID: 336 RVA: 0x00006725 File Offset: 0x00004925
		public void Dispose()
		{
			if (this.Stream != null)
			{
				this.Stream.Dispose();
				this.Stream = null;
			}
			if (this.Reader != null)
			{
				this.Reader.Close();
				this.Reader = null;
			}
		}

		// Token: 0x06000151 RID: 337 RVA: 0x0000675C File Offset: 0x0000495C
		public void ExtractToDirectory(string directoryName)
		{
			for (int i = 0; i < this.Entries.Length; i++)
			{
				Unzip.Entry entry = this.Entries[i];
				string text = Path.Combine(directoryName, entry.Name);
				Directory.CreateDirectory(Path.GetDirectoryName(text));
				if (!entry.IsDirectory)
				{
					this.Extract(entry.Name, text);
				}
				EventHandler<Unzip.FileProgressEventArgs> extractProgress = this.ExtractProgress;
				if (extractProgress != null)
				{
					extractProgress(this, new Unzip.FileProgressEventArgs(i + 1, this.Entries.Length, entry.Name));
				}
			}
		}

		// Token: 0x06000152 RID: 338 RVA: 0x000067DC File Offset: 0x000049DC
		public void Extract(string fileName, string outputFileName)
		{
			Unzip.Entry entry = this.GetEntry(fileName);
			using (FileStream fileStream = File.Create(outputFileName))
			{
				this.Extract(entry, fileStream);
			}
			FileInfo fileInfo = new FileInfo(outputFileName);
			if (fileInfo.Length != (long)entry.OriginalSize)
			{
				throw new InvalidDataException(string.Format("Corrupted archive: {0} has an uncompressed size {1} which does not match its expected size {2}", outputFileName, fileInfo.Length, entry.OriginalSize));
			}
			File.SetLastWriteTime(outputFileName, entry.Timestamp);
		}

		// Token: 0x06000153 RID: 339 RVA: 0x00006868 File Offset: 0x00004A68
		private Unzip.Entry GetEntry(string fileName)
		{
			fileName = fileName.Replace("\\", "/").Trim().TrimStart(new char[] { '/' });
			Unzip.Entry entry = this.Entries.FirstOrDefault((Unzip.Entry e) => e.Name == fileName);
			if (entry == null)
			{
				throw new FileNotFoundException("File not found in the archive: " + fileName);
			}
			return entry;
		}

		// Token: 0x06000154 RID: 340 RVA: 0x000068E2 File Offset: 0x00004AE2
		public void Extract(string fileName, Stream outputStream)
		{
			this.Extract(this.GetEntry(fileName), outputStream);
		}

		// Token: 0x06000155 RID: 341 RVA: 0x000068F4 File Offset: 0x00004AF4
		public void Extract(Unzip.Entry entry, Stream outputStream)
		{
			this.Stream.Seek((long)entry.HeaderOffset, SeekOrigin.Begin);
			if (this.Reader.ReadInt32() != 67324752)
			{
				throw new InvalidDataException("File signature doesn't match.");
			}
			this.Stream.Seek((long)entry.DataOffset, SeekOrigin.Begin);
			Stream stream = this.Stream;
			if (entry.Deflated)
			{
				stream = new DeflateStream(this.Stream, CompressionMode.Decompress, true);
			}
			int i = entry.OriginalSize;
			int num = Math.Min(16384, entry.OriginalSize);
			byte[] array = new byte[num];
			Unzip.Crc32Calculator crc32Calculator = new Unzip.Crc32Calculator();
			while (i > 0)
			{
				int num2 = stream.Read(array, 0, num);
				if (num2 == 0)
				{
					break;
				}
				crc32Calculator.UpdateWithBlock(array, num2);
				outputStream.Write(array, 0, num2);
				i -= num2;
			}
			if (crc32Calculator.Crc32 != entry.Crc32)
			{
				throw new InvalidDataException(string.Format("Corrupted archive: CRC32 doesn't match on file {0}: expected {1:x8}, got {2:x8}.", entry.Name, entry.Crc32, crc32Calculator.Crc32));
			}
		}

		// Token: 0x17000051 RID: 81
		// (get) Token: 0x06000156 RID: 342 RVA: 0x000069F4 File Offset: 0x00004BF4
		public IEnumerable<string> FileNames
		{
			get
			{
				return from e in this.Entries
					select e.Name into f
					where !f.EndsWith("/")
					orderby f
					select f;
			}
		}

		// Token: 0x17000052 RID: 82
		// (get) Token: 0x06000157 RID: 343 RVA: 0x00006A73 File Offset: 0x00004C73
		public Unzip.Entry[] Entries
		{
			get
			{
				if (this.entries == null)
				{
					this.entries = this.ReadZipEntries().ToArray<Unzip.Entry>();
				}
				return this.entries;
			}
		}

		// Token: 0x06000158 RID: 344 RVA: 0x00006A94 File Offset: 0x00004C94
		private IEnumerable<Unzip.Entry> ReadZipEntries()
		{
			if (this.Stream.Length < 22L)
			{
				yield break;
			}
			this.Stream.Seek(-22L, SeekOrigin.End);
			while (this.Reader.ReadInt32() != 101010256)
			{
				if (this.Stream.Position <= 5L)
				{
					yield break;
				}
				this.Stream.Seek(-5L, SeekOrigin.Current);
			}
			this.Stream.Seek(6L, SeekOrigin.Current);
			ushort entries = this.Reader.ReadUInt16();
			this.Reader.ReadInt32();
			uint num = this.Reader.ReadUInt32();
			this.Stream.Seek((long)((ulong)num), SeekOrigin.Begin);
			int num12;
			for (int i = 0; i < (int)entries; i = num12 + 1)
			{
				if (this.Reader.ReadInt32() == 33639248)
				{
					this.Reader.ReadInt32();
					bool flag = (this.Reader.ReadInt16() & 2048) != 0;
					short num2 = this.Reader.ReadInt16();
					int num3 = this.Reader.ReadInt32();
					uint num4 = this.Reader.ReadUInt32();
					int num5 = this.Reader.ReadInt32();
					int num6 = this.Reader.ReadInt32();
					short num7 = this.Reader.ReadInt16();
					short num8 = this.Reader.ReadInt16();
					short num9 = this.Reader.ReadInt16();
					this.Reader.ReadInt32();
					this.Reader.ReadInt32();
					int num10 = this.Reader.ReadInt32();
					byte[] array = this.Reader.ReadBytes((int)num7);
					this.Stream.Seek((long)num8, SeekOrigin.Current);
					byte[] array2 = this.Reader.ReadBytes((int)num9);
					int num11 = this.CalculateFileDataOffset(num10);
					Encoding encoding = (flag ? Encoding.UTF8 : Encoding.Default);
					yield return new Unzip.Entry
					{
						Name = encoding.GetString(array),
						Comment = encoding.GetString(array2),
						Crc32 = num4,
						CompressedSize = num5,
						OriginalSize = num6,
						HeaderOffset = num10,
						DataOffset = num11,
						Deflated = (num2 == 8),
						Timestamp = Unzip.ConvertToDateTime(num3)
					};
				}
				num12 = i;
			}
			yield break;
		}

		// Token: 0x06000159 RID: 345 RVA: 0x00006AA4 File Offset: 0x00004CA4
		private int CalculateFileDataOffset(int fileHeaderOffset)
		{
			long position = this.Stream.Position;
			this.Stream.Seek((long)(fileHeaderOffset + 26), SeekOrigin.Begin);
			short num = this.Reader.ReadInt16();
			short num2 = this.Reader.ReadInt16();
			int num3 = (int)this.Stream.Position + (int)num + (int)num2;
			this.Stream.Seek(position, SeekOrigin.Begin);
			return num3;
		}

		// Token: 0x0600015A RID: 346 RVA: 0x00006B05 File Offset: 0x00004D05
		public static DateTime ConvertToDateTime(int dosTimestamp)
		{
			return new DateTime((dosTimestamp >> 25) + 1980, (dosTimestamp >> 21) & 15, (dosTimestamp >> 16) & 31, (dosTimestamp >> 11) & 31, (dosTimestamp >> 5) & 63, (dosTimestamp & 31) * 2);
		}

		// Token: 0x04000071 RID: 113
		private const int EntrySignature = 33639248;

		// Token: 0x04000072 RID: 114
		private const int FileSignature = 67324752;

		// Token: 0x04000073 RID: 115
		private const int DirectorySignature = 101010256;

		// Token: 0x04000074 RID: 116
		private const int BufferSize = 16384;

		// Token: 0x04000078 RID: 120
		private Unzip.Entry[] entries;

		// Token: 0x02000051 RID: 81
		public class Entry
		{
			// Token: 0x17000075 RID: 117
			// (get) Token: 0x06000245 RID: 581 RVA: 0x0000B339 File Offset: 0x00009539
			// (set) Token: 0x06000246 RID: 582 RVA: 0x0000B341 File Offset: 0x00009541
			public string Name { get; set; }

			// Token: 0x17000076 RID: 118
			// (get) Token: 0x06000247 RID: 583 RVA: 0x0000B34A File Offset: 0x0000954A
			// (set) Token: 0x06000248 RID: 584 RVA: 0x0000B352 File Offset: 0x00009552
			public string Comment { get; set; }

			// Token: 0x17000077 RID: 119
			// (get) Token: 0x06000249 RID: 585 RVA: 0x0000B35B File Offset: 0x0000955B
			// (set) Token: 0x0600024A RID: 586 RVA: 0x0000B363 File Offset: 0x00009563
			public uint Crc32 { get; set; }

			// Token: 0x17000078 RID: 120
			// (get) Token: 0x0600024B RID: 587 RVA: 0x0000B36C File Offset: 0x0000956C
			// (set) Token: 0x0600024C RID: 588 RVA: 0x0000B374 File Offset: 0x00009574
			public int CompressedSize { get; set; }

			// Token: 0x17000079 RID: 121
			// (get) Token: 0x0600024D RID: 589 RVA: 0x0000B37D File Offset: 0x0000957D
			// (set) Token: 0x0600024E RID: 590 RVA: 0x0000B385 File Offset: 0x00009585
			public int OriginalSize { get; set; }

			// Token: 0x1700007A RID: 122
			// (get) Token: 0x0600024F RID: 591 RVA: 0x0000B38E File Offset: 0x0000958E
			// (set) Token: 0x06000250 RID: 592 RVA: 0x0000B396 File Offset: 0x00009596
			public bool Deflated { get; set; }

			// Token: 0x1700007B RID: 123
			// (get) Token: 0x06000251 RID: 593 RVA: 0x0000B39F File Offset: 0x0000959F
			public bool IsDirectory
			{
				get
				{
					return this.Name.EndsWith("/");
				}
			}

			// Token: 0x1700007C RID: 124
			// (get) Token: 0x06000252 RID: 594 RVA: 0x0000B3B1 File Offset: 0x000095B1
			// (set) Token: 0x06000253 RID: 595 RVA: 0x0000B3B9 File Offset: 0x000095B9
			public DateTime Timestamp { get; set; }

			// Token: 0x1700007D RID: 125
			// (get) Token: 0x06000254 RID: 596 RVA: 0x0000B3C2 File Offset: 0x000095C2
			public bool IsFile
			{
				get
				{
					return !this.IsDirectory;
				}
			}

			// Token: 0x1700007E RID: 126
			// (get) Token: 0x06000255 RID: 597 RVA: 0x0000B3CD File Offset: 0x000095CD
			// (set) Token: 0x06000256 RID: 598 RVA: 0x0000B3D5 File Offset: 0x000095D5
			[EditorBrowsable(EditorBrowsableState.Never)]
			public int HeaderOffset { get; set; }

			// Token: 0x1700007F RID: 127
			// (get) Token: 0x06000257 RID: 599 RVA: 0x0000B3DE File Offset: 0x000095DE
			// (set) Token: 0x06000258 RID: 600 RVA: 0x0000B3E6 File Offset: 0x000095E6
			[EditorBrowsable(EditorBrowsableState.Never)]
			public int DataOffset { get; set; }
		}

		// Token: 0x02000052 RID: 82
		public class Crc32Calculator
		{
			// Token: 0x17000080 RID: 128
			// (get) Token: 0x0600025A RID: 602 RVA: 0x0000B3EF File Offset: 0x000095EF
			public uint Crc32
			{
				get
				{
					return this.crcValue ^ uint.MaxValue;
				}
			}

			// Token: 0x0600025B RID: 603 RVA: 0x0000B3FC File Offset: 0x000095FC
			public void UpdateWithBlock(byte[] buffer, int numberOfBytes)
			{
				for (int i = 0; i < numberOfBytes; i++)
				{
					this.crcValue = (this.crcValue >> 8) ^ Unzip.Crc32Calculator.Crc32Table[(int)((uint)buffer[i] ^ (this.crcValue & 255U))];
				}
			}

			// Token: 0x0400011E RID: 286
			private static readonly uint[] Crc32Table = new uint[]
			{
				0U, 1996959894U, 3993919788U, 2567524794U, 124634137U, 1886057615U, 3915621685U, 2657392035U, 249268274U, 2044508324U,
				3772115230U, 2547177864U, 162941995U, 2125561021U, 3887607047U, 2428444049U, 498536548U, 1789927666U, 4089016648U, 2227061214U,
				450548861U, 1843258603U, 4107580753U, 2211677639U, 325883990U, 1684777152U, 4251122042U, 2321926636U, 335633487U, 1661365465U,
				4195302755U, 2366115317U, 997073096U, 1281953886U, 3579855332U, 2724688242U, 1006888145U, 1258607687U, 3524101629U, 2768942443U,
				901097722U, 1119000684U, 3686517206U, 2898065728U, 853044451U, 1172266101U, 3705015759U, 2882616665U, 651767980U, 1373503546U,
				3369554304U, 3218104598U, 565507253U, 1454621731U, 3485111705U, 3099436303U, 671266974U, 1594198024U, 3322730930U, 2970347812U,
				795835527U, 1483230225U, 3244367275U, 3060149565U, 1994146192U, 31158534U, 2563907772U, 4023717930U, 1907459465U, 112637215U,
				2680153253U, 3904427059U, 2013776290U, 251722036U, 2517215374U, 3775830040U, 2137656763U, 141376813U, 2439277719U, 3865271297U,
				1802195444U, 476864866U, 2238001368U, 4066508878U, 1812370925U, 453092731U, 2181625025U, 4111451223U, 1706088902U, 314042704U,
				2344532202U, 4240017532U, 1658658271U, 366619977U, 2362670323U, 4224994405U, 1303535960U, 984961486U, 2747007092U, 3569037538U,
				1256170817U, 1037604311U, 2765210733U, 3554079995U, 1131014506U, 879679996U, 2909243462U, 3663771856U, 1141124467U, 855842277U,
				2852801631U, 3708648649U, 1342533948U, 654459306U, 3188396048U, 3373015174U, 1466479909U, 544179635U, 3110523913U, 3462522015U,
				1591671054U, 702138776U, 2966460450U, 3352799412U, 1504918807U, 783551873U, 3082640443U, 3233442989U, 3988292384U, 2596254646U,
				62317068U, 1957810842U, 3939845945U, 2647816111U, 81470997U, 1943803523U, 3814918930U, 2489596804U, 225274430U, 2053790376U,
				3826175755U, 2466906013U, 167816743U, 2097651377U, 4027552580U, 2265490386U, 503444072U, 1762050814U, 4150417245U, 2154129355U,
				426522225U, 1852507879U, 4275313526U, 2312317920U, 282753626U, 1742555852U, 4189708143U, 2394877945U, 397917763U, 1622183637U,
				3604390888U, 2714866558U, 953729732U, 1340076626U, 3518719985U, 2797360999U, 1068828381U, 1219638859U, 3624741850U, 2936675148U,
				906185462U, 1090812512U, 3747672003U, 2825379669U, 829329135U, 1181335161U, 3412177804U, 3160834842U, 628085408U, 1382605366U,
				3423369109U, 3138078467U, 570562233U, 1426400815U, 3317316542U, 2998733608U, 733239954U, 1555261956U, 3268935591U, 3050360625U,
				752459403U, 1541320221U, 2607071920U, 3965973030U, 1969922972U, 40735498U, 2617837225U, 3943577151U, 1913087877U, 83908371U,
				2512341634U, 3803740692U, 2075208622U, 213261112U, 2463272603U, 3855990285U, 2094854071U, 198958881U, 2262029012U, 4057260610U,
				1759359992U, 534414190U, 2176718541U, 4139329115U, 1873836001U, 414664567U, 2282248934U, 4279200368U, 1711684554U, 285281116U,
				2405801727U, 4167216745U, 1634467795U, 376229701U, 2685067896U, 3608007406U, 1308918612U, 956543938U, 2808555105U, 3495958263U,
				1231636301U, 1047427035U, 2932959818U, 3654703836U, 1088359270U, 936918000U, 2847714899U, 3736837829U, 1202900863U, 817233897U,
				3183342108U, 3401237130U, 1404277552U, 615818150U, 3134207493U, 3453421203U, 1423857449U, 601450431U, 3009837614U, 3294710456U,
				1567103746U, 711928724U, 3020668471U, 3272380065U, 1510334235U, 755167117U
			};

			// Token: 0x0400011F RID: 287
			private uint crcValue = uint.MaxValue;
		}

		// Token: 0x02000053 RID: 83
		public class FileProgressEventArgs : ProgressChangedEventArgs
		{
			// Token: 0x0600025E RID: 606 RVA: 0x0000B465 File Offset: 0x00009665
			public FileProgressEventArgs(int currentFile, int totalFiles, string fileName)
				: base((totalFiles != 0) ? (currentFile * 100 / totalFiles) : 100, fileName)
			{
				this.CurrentFile = currentFile;
				this.TotalFiles = totalFiles;
				this.FileName = fileName;
			}

			// Token: 0x17000081 RID: 129
			// (get) Token: 0x0600025F RID: 607 RVA: 0x0000B490 File Offset: 0x00009690
			// (set) Token: 0x06000260 RID: 608 RVA: 0x0000B498 File Offset: 0x00009698
			public int CurrentFile { get; private set; }

			// Token: 0x17000082 RID: 130
			// (get) Token: 0x06000261 RID: 609 RVA: 0x0000B4A1 File Offset: 0x000096A1
			// (set) Token: 0x06000262 RID: 610 RVA: 0x0000B4A9 File Offset: 0x000096A9
			public int TotalFiles { get; private set; }

			// Token: 0x17000083 RID: 131
			// (get) Token: 0x06000263 RID: 611 RVA: 0x0000B4B2 File Offset: 0x000096B2
			// (set) Token: 0x06000264 RID: 612 RVA: 0x0000B4BA File Offset: 0x000096BA
			public string FileName { get; private set; }
		}
	}
}
