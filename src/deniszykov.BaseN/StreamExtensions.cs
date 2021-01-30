using System;
using System.IO;
using System.Security.Cryptography;
using JetBrains.Annotations;

namespace deniszykov.BaseN
{
	/// <summary>
	/// Stream extensions.
	/// </summary>
	public static class StreamExtensions
	{
		/// <summary>
		/// Create BaseN encoding stream based on passed <paramref name="stream"/>.
		/// </summary>
		/// <param name="stream">Output stream in which encoded data are written.</param>
		/// <param name="baseNAlphabet">Encoding alphabet.</param>
		/// <returns>Write-only encoding stream.</returns>
		[NotNull]
		public static Stream BaseNEncode([NotNull] this Stream stream, [NotNull] BaseNAlphabet baseNAlphabet
#if NETCOREAPP
			, bool leaveOpen = false
#endif
			)
		{
			if (stream == null) throw new ArgumentNullException(nameof(stream));
			if (baseNAlphabet == null) throw new ArgumentNullException(nameof(baseNAlphabet));

			return new CryptoStream(stream, new BaseNDecoder(baseNAlphabet), CryptoStreamMode.Write
#if NETCOREAPP
				, leaveOpen
#endif
			);
		}
		/// <summary>
		/// Create BaseN encoding stream based on passed <paramref name="stream"/>.
		/// </summary>
		/// <param name="stream">Output stream in which encoded data are written.</param>
		/// <param name="baseNEncoding">Encoding alphabet.</param>
		/// <returns>Write-only encoding stream.</returns>
		[NotNull]
		public static Stream BaseNEncode([NotNull] this Stream stream, [NotNull] BaseNEncoding baseNEncoding
#if NETCOREAPP
			, bool leaveOpen = false
#endif
		)
		{
			if (stream == null) throw new ArgumentNullException(nameof(stream));
			if (baseNEncoding == null) throw new ArgumentNullException(nameof(baseNEncoding));

			return new CryptoStream(stream, (BaseNDecoder)baseNEncoding.GetDecoder(), CryptoStreamMode.Write
#if NETCOREAPP
				, leaveOpen
#endif
			);
		}

		/// <summary>
		/// Create BaseN decoding stream based on passed <paramref name="stream"/>.
		/// </summary>
		/// <param name="stream">Input stream with encoded data.</param>
		/// <param name="baseNAlphabet">Decoding alphabet.</param>
		/// <returns>Read-only decoding stream.</returns>
		[NotNull]
		public static Stream BaseNDecode([NotNull] this Stream stream, [NotNull] BaseNAlphabet baseNAlphabet
#if NETCOREAPP
			, bool leaveOpen = false
#endif
		)
		{
			if (stream == null) throw new ArgumentNullException(nameof(stream));
			if (baseNAlphabet == null) throw new ArgumentNullException(nameof(baseNAlphabet));

			return new CryptoStream(stream, new BaseNEncoder(baseNAlphabet), CryptoStreamMode.Read
#if NETCOREAPP
				, leaveOpen
#endif
			);
		}

		/// <summary>
		/// Create BaseN decoding stream based on passed <paramref name="stream"/>.
		/// </summary>
		/// <param name="stream">Input stream with encoded data.</param>
		/// <param name="baseNEncoding">Decoding alphabet.</param>
		/// <returns>Read-only decoding stream.</returns>
		[NotNull]
		public static Stream BaseNDecode([NotNull] this Stream stream, [NotNull] BaseNEncoding baseNEncoding
#if NETCOREAPP
			, bool leaveOpen = false
#endif
		)
		{
			if (stream == null) throw new ArgumentNullException(nameof(stream));
			if (baseNEncoding == null) throw new ArgumentNullException(nameof(baseNEncoding));

			return new CryptoStream(stream, (BaseNEncoder)baseNEncoding.GetEncoder(), CryptoStreamMode.Read
#if NETCOREAPP
				, leaveOpen
#endif
				);
		}
	}
}
