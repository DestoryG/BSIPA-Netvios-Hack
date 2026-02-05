using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using BeatSaberMarkupLanguage.Animations.APNG.Chunks;

namespace BeatSaberMarkupLanguage.Animations.APNG
{
	// Token: 0x020000CF RID: 207
	public class APNG : IAnimatedImage
	{
		// Token: 0x06000477 RID: 1143 RVA: 0x0001399E File Offset: 0x00011B9E
		public void Load(string filename)
		{
			this.Load(File.ReadAllBytes(filename));
		}

		// Token: 0x06000478 RID: 1144 RVA: 0x000139AC File Offset: 0x00011BAC
		public void Load(byte[] fileBytes)
		{
			MemoryStream memoryStream = new MemoryStream(fileBytes);
			this.Load(memoryStream);
		}

		// Token: 0x06000479 RID: 1145 RVA: 0x000139C8 File Offset: 0x00011BC8
		internal void Load(MemoryStream stream)
		{
			this.ms = stream;
			if (!Helper.IsBytesEqual(this.ms.ReadBytes(Frame.Signature.Length), Frame.Signature))
			{
				throw new Exception("File signature incorrect.");
			}
			this.IHDRChunk = new IHDRChunk(this.ms);
			if (this.IHDRChunk.ChunkType != "IHDR")
			{
				throw new Exception("IHDR chunk must located before any other chunks.");
			}
			this.viewSize = new Size(this.IHDRChunk.Width, this.IHDRChunk.Height);
			Frame frame = null;
			List<OtherChunk> otherChunks = new List<OtherChunk>();
			bool flag = false;
			while (this.ms.Position != this.ms.Length)
			{
				Chunk chunk = new Chunk(this.ms);
				string chunkType = chunk.ChunkType;
				if (chunkType == null)
				{
					goto IL_02B1;
				}
				if (chunkType == "IHDR")
				{
					throw new Exception("Only single IHDR is allowed.");
				}
				if (!(chunkType == "acTL"))
				{
					if (!(chunkType == "IDAT"))
					{
						if (!(chunkType == "fcTL"))
						{
							if (!(chunkType == "fdAT"))
							{
								if (!(chunkType == "IEND"))
								{
									goto IL_02B1;
								}
								if (frame != null)
								{
									this.frames.Add(frame);
								}
								if (this.DefaultImage.IDATChunks.Count != 0)
								{
									this.DefaultImage.IENDChunk = new IENDChunk(chunk);
								}
								using (List<Frame>.Enumerator enumerator = this.frames.GetEnumerator())
								{
									while (enumerator.MoveNext())
									{
										Frame frame2 = enumerator.Current;
										frame2.IENDChunk = new IENDChunk(chunk);
									}
									goto IL_02C2;
								}
								goto IL_02B1;
							}
							else if (!this.IsSimplePNG)
							{
								if (frame == null || frame.fcTLChunk == null)
								{
									throw new Exception("fcTL chunk expected.");
								}
								frame.AddIDATChunk(new fdATChunk(chunk).ToIDATChunk());
							}
						}
						else if (!this.IsSimplePNG)
						{
							if (frame != null && frame.IDATChunks.Count == 0)
							{
								throw new Exception("One frame must have only one fcTL chunk.");
							}
							if (flag)
							{
								if (frame != null)
								{
									this.frames.Add(frame);
								}
								frame = new Frame
								{
									IHDRChunk = this.IHDRChunk,
									fcTLChunk = new fcTLChunk(chunk)
								};
							}
							else
							{
								this.defaultImage.fcTLChunk = new fcTLChunk(chunk);
							}
						}
					}
					else
					{
						if (this.acTLChunk == null)
						{
							this.IsSimplePNG = true;
						}
						this.defaultImage.IHDRChunk = this.IHDRChunk;
						this.defaultImage.AddIDATChunk(new IDATChunk(chunk));
						flag = true;
					}
				}
				else
				{
					if (this.IsSimplePNG)
					{
						throw new Exception("acTL chunk must located before any IDAT and fdAT");
					}
					this.acTLChunk = new acTLChunk(chunk);
				}
				IL_02C2:
				if (!(chunk.ChunkType != "IEND"))
				{
					if (this.defaultImage.fcTLChunk != null)
					{
						this.frames.Insert(0, this.defaultImage);
						this.DefaultImageIsAnimated = true;
					}
					else
					{
						otherChunks.ForEach(new Action<OtherChunk>(this.defaultImage.AddOtherChunk));
					}
					this.frames.ForEach(delegate(Frame f)
					{
						otherChunks.ForEach(new Action<OtherChunk>(f.AddOtherChunk));
					});
					return;
				}
				continue;
				IL_02B1:
				otherChunks.Add(new OtherChunk(chunk));
				goto IL_02C2;
			}
			throw new Exception("IEND chunk expected.");
		}

		// Token: 0x0600047A RID: 1146 RVA: 0x00013D18 File Offset: 0x00011F18
		public void Save(string filename)
		{
			using (BinaryWriter writer = new BinaryWriter(new FileStream(filename, FileMode.OpenOrCreate, FileAccess.Write)))
			{
				int num = 0;
				writer.Write(Frame.Signature);
				writer.Write(this.IHDRChunk.RawData);
				if (this.acTLChunk != null)
				{
					writer.Write(this.acTLChunk.RawData);
				}
				foreach (OtherChunk otherChunk in this.defaultImage.OtherChunks)
				{
					writer.Write(otherChunk.RawData);
				}
				uint num2 = 0U;
				if (!this.DefaultImageIsAnimated)
				{
					this.defaultImage.IDATChunks.ForEach(delegate(IDATChunk i)
					{
						writer.Write(i.RawData);
					});
				}
				else
				{
					this.frames[0].fcTLChunk.SequenceNumber = num2++;
					writer.Write(this.frames[0].fcTLChunk.RawData);
					this.frames[0].IDATChunks.ForEach(delegate(IDATChunk i)
					{
						writer.Write(i.RawData);
					});
					num = 1;
				}
				for (int k = num; k < this.frames.Count; k++)
				{
					this.frames[k].fcTLChunk.SequenceNumber = num2++;
					writer.Write(this.frames[k].fcTLChunk.RawData);
					for (int j = 0; j < this.frames[k].IDATChunks.Count; j++)
					{
						fdATChunk fdATChunk = fdATChunk.FromIDATChunk(this.frames[k].IDATChunks[j], num2++);
						writer.Write(fdATChunk.RawData);
					}
				}
				writer.Write(this.defaultImage.IENDChunk.RawData);
				writer.Close();
			}
		}

		// Token: 0x1700010B RID: 267
		// (get) Token: 0x0600047B RID: 1147 RVA: 0x00013F80 File Offset: 0x00012180
		// (set) Token: 0x0600047C RID: 1148 RVA: 0x00013F88 File Offset: 0x00012188
		public bool IsSimplePNG { get; private set; }

		// Token: 0x1700010C RID: 268
		// (get) Token: 0x0600047D RID: 1149 RVA: 0x00013F91 File Offset: 0x00012191
		// (set) Token: 0x0600047E RID: 1150 RVA: 0x00013F99 File Offset: 0x00012199
		public bool DefaultImageIsAnimated { get; private set; }

		// Token: 0x1700010D RID: 269
		// (get) Token: 0x0600047F RID: 1151 RVA: 0x00013FA2 File Offset: 0x000121A2
		public Frame DefaultImage
		{
			get
			{
				return this.defaultImage;
			}
		}

		// Token: 0x1700010E RID: 270
		// (get) Token: 0x06000480 RID: 1152 RVA: 0x00013FAA File Offset: 0x000121AA
		public Frame[] Frames
		{
			get
			{
				if (!this.IsSimplePNG)
				{
					return this.frames.ToArray();
				}
				return new Frame[] { this.defaultImage };
			}
		}

		// Token: 0x06000481 RID: 1153 RVA: 0x00013FCF File Offset: 0x000121CF
		public DisposeOps GetDisposeOperationFor(int index)
		{
			if (!this.IsSimplePNG)
			{
				return this.frames[index].fcTLChunk.DisposeOp;
			}
			return DisposeOps.APNGDisposeOpNone;
		}

		// Token: 0x06000482 RID: 1154 RVA: 0x00013FF1 File Offset: 0x000121F1
		public BlendOps GetBlendOperationFor(int index)
		{
			if (!this.IsSimplePNG)
			{
				return this.frames[index].fcTLChunk.BlendOp;
			}
			return BlendOps.APNGBlendOpSource;
		}

		// Token: 0x06000483 RID: 1155 RVA: 0x00014013 File Offset: 0x00012213
		public Bitmap GetDefaultImage()
		{
			if (this.IsSimplePNG)
			{
				return this.DefaultImage.ToBitmap();
			}
			return this[0];
		}

		// Token: 0x1700010F RID: 271
		public Bitmap this[int index]
		{
			get
			{
				Bitmap bitmap = null;
				if (this.IsSimplePNG)
				{
					return new Bitmap(this.defaultImage.ToBitmap(), this.viewSize);
				}
				if (index >= 0 && index < this.frames.Count)
				{
					bitmap = new Bitmap(this.frames[index].ToBitmap(), this.viewSize);
				}
				return bitmap;
			}
		}

		// Token: 0x17000110 RID: 272
		// (get) Token: 0x06000485 RID: 1157 RVA: 0x0001408E File Offset: 0x0001228E
		public int FrameCount
		{
			get
			{
				acTLChunk acTLChunk = this.acTLChunk;
				if (acTLChunk == null)
				{
					return 1;
				}
				return (int)acTLChunk.FrameCount;
			}
		}

		// Token: 0x17000111 RID: 273
		// (get) Token: 0x06000486 RID: 1158 RVA: 0x000140A1 File Offset: 0x000122A1
		// (set) Token: 0x06000487 RID: 1159 RVA: 0x000140AC File Offset: 0x000122AC
		public int FrameRate
		{
			get
			{
				return this.GetFrameRate(0);
			}
			set
			{
				for (int i = 0; i < this.frames.Count; i++)
				{
					this.SetFrameRate(i, value);
				}
			}
		}

		// Token: 0x06000488 RID: 1160 RVA: 0x000140D8 File Offset: 0x000122D8
		public int GetFrameRate(int index)
		{
			int num = 0;
			if (this.frames != null && this.frames.Count > index)
			{
				num = this.frames[index].FrameRate;
			}
			return num;
		}

		// Token: 0x06000489 RID: 1161 RVA: 0x00014110 File Offset: 0x00012310
		public void SetFrameRate(int index, int frameRate)
		{
			if (this.frames != null && this.frames.Count > index)
			{
				this.frames[index].FrameRate = frameRate;
			}
		}

		// Token: 0x17000112 RID: 274
		// (get) Token: 0x0600048A RID: 1162 RVA: 0x0001413A File Offset: 0x0001233A
		// (set) Token: 0x0600048B RID: 1163 RVA: 0x00014142 File Offset: 0x00012342
		public Size ViewSize
		{
			get
			{
				return this.viewSize;
			}
			set
			{
				this.viewSize = value;
			}
		}

		// Token: 0x17000113 RID: 275
		// (get) Token: 0x0600048C RID: 1164 RVA: 0x0001414B File Offset: 0x0001234B
		// (set) Token: 0x0600048D RID: 1165 RVA: 0x00014168 File Offset: 0x00012368
		public Size ActualSize
		{
			get
			{
				return new Size(this.IHDRChunk.Width, this.IHDRChunk.Height);
			}
			set
			{
				this.IHDRChunk.Width = value.Width;
				this.IHDRChunk.Height = value.Height;
			}
		}

		// Token: 0x17000114 RID: 276
		// (get) Token: 0x0600048E RID: 1166 RVA: 0x0001418E File Offset: 0x0001238E
		// (set) Token: 0x0600048F RID: 1167 RVA: 0x000141A1 File Offset: 0x000123A1
		public int PlayCount
		{
			get
			{
				acTLChunk acTLChunk = this.acTLChunk;
				if (acTLChunk == null)
				{
					return 0;
				}
				return (int)acTLChunk.PlayCount;
			}
			set
			{
				this.acTLChunk.PlayCount = (uint)value;
			}
		}

		// Token: 0x17000115 RID: 277
		// (get) Token: 0x06000490 RID: 1168 RVA: 0x000141AF File Offset: 0x000123AF
		// (set) Token: 0x06000491 RID: 1169 RVA: 0x000141B7 File Offset: 0x000123B7
		internal IHDRChunk IHDRChunk { get; private set; }

		// Token: 0x17000116 RID: 278
		// (get) Token: 0x06000492 RID: 1170 RVA: 0x000141C0 File Offset: 0x000123C0
		// (set) Token: 0x06000493 RID: 1171 RVA: 0x000141C8 File Offset: 0x000123C8
		internal acTLChunk acTLChunk { get; private set; }

		// Token: 0x06000494 RID: 1172 RVA: 0x000141D1 File Offset: 0x000123D1
		public void SetDefaultImage(Image image)
		{
			this.defaultImage = APNG.FromImage(image).DefaultImage;
			this.DefaultImageIsAnimated = false;
		}

		// Token: 0x06000495 RID: 1173 RVA: 0x000141EC File Offset: 0x000123EC
		public void AddFrame(Image image)
		{
			if (this.IHDRChunk != null && (image.Width > this.IHDRChunk.Width || image.Height > this.IHDRChunk.Height))
			{
				throw new InvalidDataException("Frame must be less than or equal to the size of the other frames.");
			}
			APNG apng = APNG.FromImage(image);
			if (this.IHDRChunk == null)
			{
				this.IHDRChunk = apng.IHDRChunk;
			}
			if (this.acTLChunk == null)
			{
				this.acTLChunk = new acTLChunk();
				this.acTLChunk.PlayCount = 0U;
			}
			uint num = ((this.frames.Count == 0) ? 0U : ((uint)((ulong)this.frames[this.frames.Count - 1].fcTLChunk.SequenceNumber + (ulong)((long)this.frames[this.frames.Count - 1].IDATChunks.Count))));
			fcTLChunk fcTLChunk = new fcTLChunk
			{
				SequenceNumber = num,
				Width = (uint)image.Width,
				Height = (uint)image.Height,
				XOffset = 0U,
				YOffset = 0U,
				DelayNumerator = 100,
				DelayDenominator = 1000,
				DisposeOp = DisposeOps.APNGDisposeOpNone,
				BlendOp = BlendOps.APNGBlendOpSource
			};
			if (this.defaultImage.IDATChunks.Count == 0)
			{
				this.defaultImage = apng.DefaultImage;
				this.defaultImage.fcTLChunk = fcTLChunk;
				this.DefaultImageIsAnimated = true;
			}
			if (apng.IsSimplePNG)
			{
				Frame frame = apng.DefaultImage;
				frame.fcTLChunk = fcTLChunk;
				foreach (OtherChunk otherChunk in frame.OtherChunks)
				{
					if (!this.defaultImage.OtherChunks.Contains(otherChunk))
					{
						this.defaultImage.OtherChunks.Add(otherChunk);
					}
				}
				frame.OtherChunks.Clear();
				this.frames.Add(frame);
			}
			else
			{
				for (int i = 0; i < apng.FrameCount; i++)
				{
					Frame frame2 = apng.Frames[i];
					frame2.fcTLChunk.SequenceNumber = num;
					foreach (OtherChunk otherChunk2 in frame2.OtherChunks)
					{
						if (!this.defaultImage.OtherChunks.Contains(otherChunk2))
						{
							this.defaultImage.OtherChunks.Add(otherChunk2);
						}
					}
					frame2.OtherChunks.Clear();
					this.frames.Add(frame2);
				}
			}
			List<OtherChunk> otherChunks = this.defaultImage.OtherChunks;
			if (this.defaultImage != this.frames[0])
			{
				this.frames.ForEach(delegate(Frame f)
				{
					otherChunks.ForEach(new Action<OtherChunk>(f.AddOtherChunk));
				});
			}
			else
			{
				for (int j = 1; j < this.frames.Count; j++)
				{
					otherChunks.ForEach(new Action<OtherChunk>(this.frames[j].AddOtherChunk));
				}
			}
			this.acTLChunk.FrameCount = (uint)this.frames.Count;
		}

		// Token: 0x06000496 RID: 1174 RVA: 0x00014534 File Offset: 0x00012734
		public void RemoveFrame(int index)
		{
			this.frames.RemoveAt(index);
			if (index == 0)
			{
				if (this.frames.Count == 0)
				{
					this.defaultImage = null;
					this.DefaultImageIsAnimated = false;
					return;
				}
				this.defaultImage = this.frames[0];
			}
		}

		// Token: 0x06000497 RID: 1175 RVA: 0x00014573 File Offset: 0x00012773
		public void ClearFrames()
		{
			this.frames.Clear();
			if (this.DefaultImageIsAnimated)
			{
				this.defaultImage = null;
			}
		}

		// Token: 0x06000498 RID: 1176 RVA: 0x0001458F File Offset: 0x0001278F
		public static APNG FromFile(string filename)
		{
			APNG apng = new APNG();
			apng.Load(filename);
			return apng;
		}

		// Token: 0x06000499 RID: 1177 RVA: 0x0001459D File Offset: 0x0001279D
		public static APNG FromStream(MemoryStream stream)
		{
			APNG apng = new APNG();
			apng.Load(stream);
			return apng;
		}

		// Token: 0x0600049A RID: 1178 RVA: 0x000145AB File Offset: 0x000127AB
		public static APNG FromImage(Image image)
		{
			APNG apng = new APNG();
			apng.Load(APNG.ImageToStream(image));
			return apng;
		}

		// Token: 0x0600049B RID: 1179 RVA: 0x000145C0 File Offset: 0x000127C0
		private static MemoryStream ImageToStream(Image image)
		{
			MemoryStream memoryStream = new MemoryStream();
			image.Save(memoryStream, ImageFormat.Png);
			memoryStream.Position = 0L;
			return memoryStream;
		}

		// Token: 0x04000164 RID: 356
		private Frame defaultImage = new Frame();

		// Token: 0x04000165 RID: 357
		private List<Frame> frames = new List<Frame>();

		// Token: 0x04000166 RID: 358
		private MemoryStream ms;

		// Token: 0x04000167 RID: 359
		private Size viewSize;
	}
}
