using System;
using System.IO;
using System.Reflection;
using System.Security.Cryptography;
using Mono.Cecil.PE;

namespace Mono.Cecil
{
	// Token: 0x020000CC RID: 204
	internal static class CryptoService
	{
		// Token: 0x060008A1 RID: 2209 RVA: 0x0001ADDC File Offset: 0x00018FDC
		public static void StrongName(Stream stream, ImageWriter writer, StrongNameKeyPair key_pair)
		{
			int num;
			byte[] array = CryptoService.CreateStrongName(key_pair, CryptoService.HashStream(stream, writer, out num));
			CryptoService.PatchStrongName(stream, num, array);
		}

		// Token: 0x060008A2 RID: 2210 RVA: 0x0001AE01 File Offset: 0x00019001
		private static void PatchStrongName(Stream stream, int strong_name_pointer, byte[] strong_name)
		{
			stream.Seek((long)strong_name_pointer, SeekOrigin.Begin);
			stream.Write(strong_name, 0, strong_name.Length);
		}

		// Token: 0x060008A3 RID: 2211 RVA: 0x0001AE18 File Offset: 0x00019018
		private static byte[] CreateStrongName(StrongNameKeyPair key_pair, byte[] hash)
		{
			byte[] array2;
			using (RSA rsa = key_pair.CreateRSA())
			{
				RSAPKCS1SignatureFormatter rsapkcs1SignatureFormatter = new RSAPKCS1SignatureFormatter(rsa);
				rsapkcs1SignatureFormatter.SetHashAlgorithm("SHA1");
				byte[] array = rsapkcs1SignatureFormatter.CreateSignature(hash);
				Array.Reverse(array);
				array2 = array;
			}
			return array2;
		}

		// Token: 0x060008A4 RID: 2212 RVA: 0x0001AE68 File Offset: 0x00019068
		private static byte[] HashStream(Stream stream, ImageWriter writer, out int strong_name_pointer)
		{
			Section text = writer.text;
			int headerSize = (int)writer.GetHeaderSize();
			int pointerToRawData = (int)text.PointerToRawData;
			DataDirectory strongNameSignatureDirectory = writer.GetStrongNameSignatureDirectory();
			if (strongNameSignatureDirectory.Size == 0U)
			{
				throw new InvalidOperationException();
			}
			strong_name_pointer = (int)((long)pointerToRawData + (long)((ulong)(strongNameSignatureDirectory.VirtualAddress - text.VirtualAddress)));
			int size = (int)strongNameSignatureDirectory.Size;
			SHA1Managed sha1Managed = new SHA1Managed();
			byte[] array = new byte[8192];
			using (CryptoStream cryptoStream = new CryptoStream(Stream.Null, sha1Managed, CryptoStreamMode.Write))
			{
				stream.Seek(0L, SeekOrigin.Begin);
				CryptoService.CopyStreamChunk(stream, cryptoStream, array, headerSize);
				stream.Seek((long)pointerToRawData, SeekOrigin.Begin);
				CryptoService.CopyStreamChunk(stream, cryptoStream, array, strong_name_pointer - pointerToRawData);
				stream.Seek((long)size, SeekOrigin.Current);
				CryptoService.CopyStreamChunk(stream, cryptoStream, array, (int)(stream.Length - (long)(strong_name_pointer + size)));
			}
			return sha1Managed.Hash;
		}

		// Token: 0x060008A5 RID: 2213 RVA: 0x0001AF54 File Offset: 0x00019154
		private static void CopyStreamChunk(Stream stream, Stream dest_stream, byte[] buffer, int length)
		{
			while (length > 0)
			{
				int num = stream.Read(buffer, 0, Math.Min(buffer.Length, length));
				dest_stream.Write(buffer, 0, num);
				length -= num;
			}
		}

		// Token: 0x060008A6 RID: 2214 RVA: 0x0001AF88 File Offset: 0x00019188
		public static byte[] ComputeHash(string file)
		{
			if (!File.Exists(file))
			{
				return Empty<byte>.Array;
			}
			SHA1Managed sha1Managed = new SHA1Managed();
			using (FileStream fileStream = new FileStream(file, FileMode.Open, FileAccess.Read, FileShare.Read))
			{
				byte[] array = new byte[8192];
				using (CryptoStream cryptoStream = new CryptoStream(Stream.Null, sha1Managed, CryptoStreamMode.Write))
				{
					CryptoService.CopyStreamChunk(fileStream, cryptoStream, array, (int)fileStream.Length);
				}
			}
			return sha1Managed.Hash;
		}
	}
}
