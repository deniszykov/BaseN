using System;
using BenchmarkDotNet.Running;

namespace deniszykov.BaseN.Benchmark
{
	class Program
	{
		static void Main(string[] args)
		{
			var summary = BenchmarkRunner.Run<Base32EncodeBenchmarks>();
			Console.WriteLine(summary.ToString());
			Console.ReadKey();

			//new Base32EncodeBenchmarks().BaseN_BaseNDecoder_Convert();
		}
	}
}
