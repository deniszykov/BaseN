using System;
using System.Text;
using JetBrains.Annotations;

namespace deniszykov.BaseN
{
	/// <summary>
	/// Base-(Alphabet Length) binary data encoding based on specified <see cref="Alphabet"/>.
	/// Result of <see cref="GetDecoder()"/> and <see cref="GetEncoder()"/> could be safely cast to <see cref="BaseNDecoder"/> and <see cref="BaseNEncoder"/> for more available conversion methods.
	/// </summary>
	[PublicAPI]
	public sealed class BaseNEncoding : Encoding
	{
		// ReSharper disable StringLiteralTypo
		/// <summary>
		/// Base16 (Hex) encoding. Upper case.
		/// </summary>
		public static readonly BaseNEncoding Base16UpperCase = new BaseNEncoding(BaseNAlphabet.Base16UpperCaseAlphabet, "hex-upper");
		/// <summary>
		/// Base16 (Hex) encoding. Lower case.
		/// </summary>
		public static readonly BaseNEncoding Base16LowerCase = new BaseNEncoding(BaseNAlphabet.Base16LowerCaseAlphabet, "hex-lower");
		/// <summary>
		/// Base32 encoding.
		/// </summary>
		public static readonly BaseNEncoding Base32 = new BaseNEncoding(BaseNAlphabet.Base32Alphabet, "base32");
		/// <summary>
		/// Alternative ZBase32 encoding.
		/// </summary>
		public static readonly BaseNEncoding ZBase32 = new BaseNEncoding(BaseNAlphabet.ZBase32Alphabet, "zbase32");
		/// <summary>
		/// Base64 encoding.
		/// </summary>
		public static readonly BaseNEncoding Base64 = new BaseNEncoding(BaseNAlphabet.Base64Alphabet, "base64");
		/// <summary>
		/// Url-safe Base64 encoding. Where (+) is replaced with (-) and (/) is replaced with (_).
		/// </summary>
		public static readonly BaseNEncoding Base64Url = new BaseNEncoding(BaseNAlphabet.Base64UrlAlphabet, "base64-url");

		/// <summary>
		/// Coder which is used to transform chars to bytes (text -> binary). Class name is reversed because it is based on text encodings which has opposite semantics.
		/// </summary>
		public readonly BaseNEncoder Encoder;
		/// <summary>
		/// Coder which is used to transform bytes to chars (binary -> text). Class name is reversed because it is based on text encodings which has opposite semantics.
		/// </summary>
		public readonly BaseNDecoder Decoder;

		public BaseNAlphabet Alphabet { get; }
		/// <inheritdoc />
		public override string EncodingName { get; }
		/// <inheritdoc />
		public override bool IsSingleByte => this.Alphabet.EncodingBlockSize == 1;

		/// <summary>
		/// Constructor of <see cref="BaseNEncoding"/>
		/// </summary>
		/// <param name="baseNAlphabet">Alphabet used as base for encoding binary data.</param>
		/// <param name="encodingName">Name of encoding. Used for <see cref="Encoding.EncodingName"/> property.</param>
		public BaseNEncoding(BaseNAlphabet baseNAlphabet, string encodingName)
		{
			if (baseNAlphabet == null) throw new ArgumentNullException(nameof(baseNAlphabet));
			if (encodingName == null) throw new ArgumentNullException(nameof(encodingName));

			this.EncodingName = encodingName;
			this.Alphabet = baseNAlphabet;

			this.Encoder = new BaseNEncoder(baseNAlphabet);
			this.Decoder = new BaseNDecoder(baseNAlphabet);
		}

		/// <inheritdoc />
		public override int GetByteCount(char[] chars, int index, int count)
		{
			return this.Encoder.GetByteCount(chars, index, count, flush: true);
		}
		/// <inheritdoc />
		public override int GetByteCount(string s)
		{
			return this.Encoder.GetByteCount(s, 0, s.Length, flush: true);
		}
		/// <inheritdoc />
		public override unsafe int GetByteCount(char* chars, int count)
		{
			return this.Encoder.GetByteCount(chars, count, flush: true);
		}
#if NETSTANDARD || NET461
		/// <summary>
		/// See <see cref="GetByteCount(char[], int, int)"/> for description.
		/// </summary>
		public int GetByteCount(ReadOnlySpan<char> chars)
		{
			return this.Encoder.GetByteCount(chars, flush: true);
		}
#endif
		/// <inheritdoc />
		public override int GetBytes(char[] chars, int charIndex, int charCount, byte[] bytes, int byteIndex)
		{
			return this.Encoder.GetBytes(chars, charIndex, charCount, bytes, byteIndex, flush: true);
		}
		/// <inheritdoc />
		public override int GetBytes(string s, int charIndex, int charCount, byte[] bytes, int byteIndex)
		{
			this.Encoder.Convert(s, charIndex, charCount, bytes, byteIndex, bytes.Length - byteIndex, flush: true, out _, out var bytesUsed, out _);
			return bytesUsed;
		}
		/// <inheritdoc />
		public override unsafe int GetBytes(char* chars, int charCount, byte* bytes, int byteCount)
		{
			return this.Encoder.GetBytes(chars, charCount, bytes, byteCount, flush: true);
		}
#if NETSTANDARD || NET461
		/// <summary>
		/// See <see cref="GetBytes(char[], int, int, byte[], int)"/> for description.
		/// </summary>
		public int GetBytes(ReadOnlySpan<char> chars, Span<byte> bytes)
		{
			return this.Encoder.GetBytes(chars, bytes, flush: true);
		}
#endif
#if !NETCOREAPP
		/// <summary>
		/// See <see cref="GetBytes(string, int, int, byte[], int)" /> variant which returns byte array.
		/// </summary>
		public byte[] GetBytes(string s, int charIndex, int charCount)
		{
			var bytes = new byte[this.Encoder.GetByteCount(s, charIndex, charCount, flush: true)];
			this.Encoder.Convert(s, charIndex, charCount, bytes, 0, bytes.Length, flush: true, out _, out _, out _);
			return bytes;
		}
#endif
		/// <inheritdoc />
		public override int GetCharCount(byte[] bytes, int index, int count)
		{
			return this.Decoder.GetCharCount(bytes, index, count);
		}
		/// <inheritdoc />
		public override unsafe int GetCharCount(byte* bytes, int count)
		{
			return this.Decoder.GetCharCount(bytes, count, flush: true);
		}
#if NETSTANDARD || NET461
		/// <summary>
		/// See <see cref="GetCharCount(byte[], int, int)"/> for description.
		/// </summary>
		public int GetCharCount(ReadOnlySpan<byte> bytes)
		{
			return this.Decoder.GetCharCount(bytes, flush: true);
		}
#endif
		/// <inheritdoc />
		public override int GetChars(byte[] bytes, int byteIndex, int byteCount, char[] chars, int charIndex)
		{
			return this.Decoder.GetChars(bytes, byteIndex, byteCount, chars, charIndex, flush: true);
		}
		/// <inheritdoc />
		public override unsafe int GetChars(byte* bytes, int byteCount, char* chars, int charCount)
		{
			return this.Decoder.GetChars(bytes, byteCount, chars, charCount, flush: true);
		}
#if NETSTANDARD || NET461
		public int GetChars(ReadOnlySpan<byte> bytes, Span<char> chars)
		{
			return this.Decoder.GetChars(bytes, chars, flush: true);
		}
#endif
		
		/// <inheritdoc />
		public override unsafe string GetString(byte[] bytes, int index, int count)
		{
			var charCount = this.Decoder.GetCharCount(bytes, index, count);
			if (charCount == 0)
			{
				return string.Empty;
			}
			var output = new string('\0', charCount);
			fixed (char* outputPtr = output)
			{
#if NETCOREAPP
				this.Decoder.Convert(new ReadOnlySpan<byte>(bytes, index, count), new Span<char>(outputPtr, output.Length), flush: true, out _, out _, out _);
#else
				fixed (byte* inputPtr = bytes)
				{
					this.Decoder.Convert(inputPtr + index, count, outputPtr, output.Length, flush: true, out _, out _, out _);
				}
#endif
			}
			return output;
		}
#if NETSTANDARD || NET461
		public unsafe string GetString(ReadOnlySpan<byte> bytes)
		{
			if (bytes.Length == 0)
			{
				return string.Empty;
			}

			fixed (byte* inputPtr = bytes)
			{
				var charCount = this.Decoder.GetCharCount(inputPtr, bytes.Length, flush: true);
				var output = new string('\0', charCount);
				fixed (char* outputPtr = output)
				{
					this.Decoder.Convert(inputPtr, bytes.Length, outputPtr, output.Length, flush: true, out _, out _, out _);
				}
				return output;
			}
		}
#endif
		/// <inheritdoc />
		public override int GetMaxByteCount(int charCount)
		{
			return this.Encoder.GetMaxByteCount(charCount);
		}
		/// <inheritdoc />
		public override int GetMaxCharCount(int byteCount)
		{
			return this.Decoder.GetMaxCharCount(byteCount);
		}
		/// <inheritdoc />
		public override Decoder GetDecoder()
		{
			return this.Decoder;
		}
		/// <inheritdoc />
		public override Encoder GetEncoder()
		{
			return this.Encoder;
		}

		/// <inheritdoc />
		public override string ToString() => this.EncodingName;
	}
}
