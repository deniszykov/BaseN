![dotnet_build](https://github.com/deniszykov/BaseN/workflows/dotnet_build/badge.svg)

Introduction
============

This is BaseN encoding library. It provides simple API for converting between binary and BaseN encoded text data.  
Also there is an implementation of `System.Text.Encoding` which provides complex streaming API with it's `Convert` methods.  

(Un)desired feature of this library, any invalid symbols (e.g. line breaks) during decoding are ignored.  

Supported encoding alphabets are `Base16` aka `Hex`, `Base32`, `ZBase32`, `Base64`, `Base64` Url-safe.  

Installation
============
```
Install-Package deniszykov.BaseN
```

Usage
============

#### Utility classes
```csharp
Base64Convert.ToString(bytes);
Base64Convert.ToCharArray(bytes);
Base64Convert.ToBytes(string); // (+ 6 overloads)

// also
// Base64UrlConvert
// Base32Convert
// ZBase32Convert
// HexConvert 
```

## Example
```csharp
using deniszykov.BaseN;

var bytes = Base64Convert.ToBytes("eg==");
// bytes[0] -> 122
```

## Using BaseNEncoding class
```csharp
using deniszykov.BaseN;

var encoding = BaseNEncoding.Base64;
var input = "eg==".ToCharArray();
var output = new byte[1024];
var decoder = encoding.GetDecoder();

decoder.Convert(input, 0, input.Length, output, 0, output.Length, flush: true, out var inputUsed, out var outputUsed, out var completed);

// completed -> true
// inputUsed -> 4
// outputUsed -> 1
// output[0] -> 122
```
There is overload of `Convert` accepting pointers and `Span<T>`'s.  

## Using custom alphabet

```csharp
using deniszykov.BaseN;

var binHex4Alphabet = new BaseNAlphabet("!\"#$%&'()*+,-012345689@ABCDEFGHIJKLMNPQRSTUVXYZ[`abcdefhijklmpqr".ToCharArray());
var encoding = new BaseNEncoding(binHex4Alphabet, "mac-binhex40");
```

## Performance
[Benchmark Code](src/deniszykov.BaseN.Benchmark/Base32EncodeBenchmarks.cs)  
[Benchmark Result](src/deniszykov.BaseN.Benchmark/Benchmark_Results.txt)  
```
|                                 Method |      Mean | Ratio |    Gen 0 | Allocated |
|--------------------------------------- |----------:|------:|---------:|----------:|
|          System_Convert_ToBase64String |  49.21 ms |  1.00 | 181.8182 |  53.33 MB |
|           System_Memory_Base64ToString |  15.57 ms |  0.32 | 125.0000 |  26.67 MB |
|             BaseN_BaseNDecoder_Convert |  29.00 ms |  0.59 | 125.0000 |  26.67 MB |
|           BaseN_Base64Convert_ToString |  39.60 ms |  0.80 | 230.7692 |  53.33 MB |
|           BaseN_Base32Convert_ToString |  46.40 ms |  0.94 | 181.8182 |     64 MB |
| Wiry_Base32Encoding_Standard_GetString |  96.24 ms |  1.96 | 500.0000 |    128 MB |
|       SimpleBase_Base32_Rfc4648_Encode |  99.23 ms |  2.02 | 166.6667 |     64 MB |
|                  Albireo_Base32_Encode | 150.08 ms |  3.04 | 500.0000 |    128 MB |
```
