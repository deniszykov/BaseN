﻿using System;
using System.Text;
using JetBrains.Annotations;

namespace deniszykov.BaseN
{
	/// <summary>
	/// Base-(Alphabet Length) binary data encoding based on specified <see cref="Alphabet"/>.
	/// Result of <see cref="GetDecoder()"/> and <see cref="GetEncoder()"/> could be safely cast to <see cref="BaseNDecoder"/> and <see cref="BaseNEncoder"/> for more available conversion methods.
	/// </summary>
	public sealed class BaseNEncoding : Encoding
	{
		// ReSharper disable StringLiteralTypo
		/// <summary>
		/// Base16 (Hex) encoding. Upper case.
		/// </summary>
		[NotNull]
		public static readonly BaseNEncoding Base16UpperCase = new BaseNEncoding(BaseNAlphabet.Base16UpperCaseAlphabet, "hex-upper");
		/// <summary>
		/// Base16 (Hex) encoding. Lower case.
		/// </summary>
		[NotNull]
		public static readonly BaseNEncoding Base16LowerCase = new BaseNEncoding(BaseNAlphabet.Base16LowerCaseAlphabet, "hex-lower");
		/// <summary>
		/// Base32 encoding.
		/// </summary>
		[NotNull]
		public static readonly BaseNEncoding Base32 = new BaseNEncoding(BaseNAlphabet.Base32Alphabet, "base32");
		/// <summary>
		/// Alternative ZBase32 encoding.
		/// </summary>
		[NotNull]
		public static readonly BaseNEncoding ZBase32 = new BaseNEncoding(BaseNAlphabet.ZBase32Alphabet, "zbase32");
		/// <summary>
		/// Base64 encoding.
		/// </summary>
		[NotNull]
		public static readonly BaseNEncoding Base64 = new BaseNEncoding(BaseNAlphabet.Base64Alphabet, "base64");
		/// <summary>
		/// Url-safe Base64 encoding. Where (+) is replaced with (-) and (/) is replaced with (_).
		/// </summary>
		[NotNull]
		public static readonly BaseNEncoding Base64Url = new BaseNEncoding(BaseNAlphabet.Base64UrlAlphabet, "base64-url");

		[NotNull]
		private readonly BaseNEncoder encoder;
		[NotNull]
		private readonly BaseNDecoder decoder;

		[NotNull]
		public BaseNAlphabet Alphabet { get; }
		/// <inheritdoc />
		[NotNull]
		public override string EncodingName { get; }
		/// <inheritdoc />
		public override bool IsSingleByte => this.Alphabet.EncodingBlockSize == 1;

		/// <summary>
		/// Constructor of <see cref="BaseNEncoding"/>
		/// </summary>
		/// <param name="baseNAlphabet">Alphabet used as base for encoding binary data.</param>
		/// <param name="encodingName">Name of encoding. Used for <see cref="Encoding.EncodingName"/> property.</param>
		public BaseNEncoding([NotNull] BaseNAlphabet baseNAlphabet, [NotNull] string encodingName)
		{
			if (baseNAlphabet == null) throw new ArgumentNullException(nameof(baseNAlphabet));
			if (encodingName == null) throw new ArgumentNullException(nameof(encodingName));

			this.EncodingName = encodingName;
			this.Alphabet = baseNAlphabet;

			this.encoder = new BaseNEncoder(baseNAlphabet);
			this.decoder = new BaseNDecoder(baseNAlphabet);
		}

		/// <inheritdoc />
		public override int GetByteCount([NotNull] char[] chars, int index, int count)
		{
			return this.encoder.GetByteCount(chars, index, count, flush: true);
		}
		/// <inheritdoc />
		public override int GetByteCount([NotNull] string s)
		{
			return this.encoder.GetByteCount(s, 0, s.Length, flush: true);
		}
		/// <inheritdoc />
		public override unsafe int GetByteCount(char* chars, int count)
		{
			return this.encoder.GetByteCount(chars, count, flush: true);
		}
		/// <inheritdoc />
		public override int GetBytes([NotNull] char[] chars, int charIndex, int charCount, [NotNull] byte[] bytes, int byteIndex)
		{
			return this.encoder.GetBytes(chars, charIndex, charCount, bytes, byteIndex, flush: true);
		}
		/// <inheritdoc />
		public override int GetBytes([NotNull] string s, int charIndex, int charCount, [NotNull] byte[] bytes, int byteIndex)
		{
			this.encoder.Convert(s, charIndex, charCount, bytes, byteIndex, bytes.Length - byteIndex, flush: true, out _, out var bytesUsed, out _);
			return bytesUsed;
		}
		/// <inheritdoc />
		public override unsafe int GetBytes(char* chars, int charCount, byte* bytes, int byteCount)
		{
			return this.encoder.GetBytes(chars, charCount, bytes, byteCount, flush: true);
		}
#if !NETCOREAPP
		/// <summary>
		/// See description on similar conversion methods. This is just overload with different buffer types.
		/// </summary>
		[NotNull]
		public byte[] GetBytes([NotNull] string s, int charIndex, int charCount)
		{
			var bytes = new byte[this.encoder.GetByteCount(s, charIndex, charCount, flush: true)];
			this.encoder.Convert(s, charIndex, charCount, bytes, 0, bytes.Length, flush: true, out _, out _, out _);
			return bytes;
		}
#endif
		/// <inheritdoc />
		public override int GetCharCount([NotNull]byte[] bytes, int index, int count)
		{
			return this.decoder.GetCharCount(bytes, index, count);
		}
		/// <inheritdoc />
		public override unsafe int GetCharCount(byte* bytes, int count)
		{
			return this.decoder.GetCharCount(bytes, count, flush: true);
		}
		/// <inheritdoc />
		public override int GetChars([NotNull] byte[] bytes, int byteIndex, int byteCount, [NotNull] char[] chars, int charIndex)
		{
			return this.decoder.GetChars(bytes, byteIndex, byteCount, chars, charIndex, flush: true);
		}
		/// <inheritdoc />
		public override unsafe int GetChars(byte* bytes, int byteCount, char* chars, int charCount)
		{
			return this.decoder.GetChars(bytes, byteCount, chars, charCount, flush: true);
		}
		/// <inheritdoc />
		public override int GetMaxByteCount(int charCount)
		{
			return this.encoder.GetMaxByteCount(charCount);
		}
		/// <inheritdoc />
		public override int GetMaxCharCount(int byteCount)
		{
			return this.decoder.GetMaxCharCount(byteCount);
		}
		/// <inheritdoc />
		[NotNull]
		public override Decoder GetDecoder()
		{
			return this.decoder;
		}
		/// <inheritdoc />
		[NotNull]
		public override Encoder GetEncoder()
		{
			return this.encoder;
		}

		/// <inheritdoc />
		public override string ToString() => this.EncodingName;
	}
}
