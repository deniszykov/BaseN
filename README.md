![dotnet_build](https://github.com/deniszykov/BaseN/workflows/dotnet_build/badge.svg)

Introduction
============

This is BaseN encoding library. It provides simple API for converting between binary and BaseN encoded text data.  
Also there is an implementation of `System.Text.Encoding` which provides complex streaming API with it's `Convert` methods.  

(Un)desired feature of this library, any invalid symbols (e.g. line breaks) during decoding are ignored.  

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

var encoding = BaseNEncoding.ZBase32;
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

var binHex4Alphabet = new BaseNAlphabet("!"#$%&'()*+,-012345689@ABCDEFGHIJKLMNPQRSTUVXYZ[`abcdefhijklmpqr".ToCharArray());
var encoding = new BaseNEncoding(binHex4Alphabet, "mac-binhex40");
```

