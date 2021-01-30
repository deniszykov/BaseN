using System;
using System.Buffers.Text;
using BenchmarkDotNet.Attributes;

namespace deniszykov.BaseN.Benchmark
{
	[MemoryDiagnoser]
	public class Base32EncodeBenchmarks
	{
		private const int N = 20 * 1024 * 1024;
		private readonly byte[] _data;
		private readonly string _base64String;

		public Base32EncodeBenchmarks()
		{
			_data = new byte[N];
			new Random(42).NextBytes(_data);
			_base64String = BaseNEncoding.Base64.GetString(this._data);
		}

		[Benchmark(Baseline = true)]
		public void System_Convert_ToBase64String()
		{
			_ = Convert.ToBase64String(_data);
		}

		[Benchmark]
		public void System_Memory_Base64ToString()
		{
			var output = new byte[_base64String.Length];
			Base64.EncodeToUtf8(_data, output, out _, out _, true);
		}

		[Benchmark]
		public void BaseN_BaseNDecoder_Convert()
		{
			var output = new byte[_base64String.Length];
			var decoder = (BaseNDecoder)deniszykov.BaseN.BaseNEncoding.Base64.GetDecoder();
			decoder.Convert(_data.AsSpan(), output.AsSpan(), true, out _, out _, out _);
		}

		[Benchmark]
		public void BaseN_Base64Convert_ToString()
		{
			_ = deniszykov.BaseN.Base64Convert.ToString(_data);
		}

		[Benchmark]
		public void BaseN_Base32Convert_ToString()
		{
			_ = deniszykov.BaseN.Base32Convert.ToString(_data);
		}

		[Benchmark]
		public void Wiry_Base32Encoding_Standard_GetString()
		{
			_ = Wiry.Base32.Base32Encoding.Standard.GetString(_data);
		}

		[Benchmark]
		public void SimpleBase_Base32_Rfc4648_Encode()
		{
			_ = SimpleBase.Base32.Rfc4648.Encode(_data);
		}

		[Benchmark]
		public void Albireo_Base32_Encode()
		{
			_ = Albireo.Base32.Base32.Encode(_data);
		}
	}
}
