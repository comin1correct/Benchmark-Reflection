``` ini

BenchmarkDotNet=v0.13.1, OS=Windows 10.0.19044.1586 (21H2)
11th Gen Intel Core i9-11950H 2.60GHz, 1 CPU, 16 logical and 8 physical cores
  [Host]           : .NET Framework 4.8 (4.8.4420.0), X64 RyuJIT
  Inlining enabled : .NET Framework 4.8 (4.8.4420.0), X64 RyuJIT

Job=Inlining enabled  Runtime=.NET Framework 4.6.1  

```
|                         Method |        Mean |     Error |    StdDev |      Median |  Ratio | RatioSD | Rank |
|------------------------------- |------------:|----------:|----------:|------------:|-------:|--------:|-----:|
|            DirectInstantiation |    10.28 ns |  0.229 ns |  0.447 ns |    10.17 ns |   1.00 |    0.00 |    * |
| ReflectionEmitClassConstructor |    11.13 ns |  0.246 ns |  0.524 ns |    10.96 ns |   1.09 |    0.07 |   ** |
|            ReflectionEmitSigil |    13.47 ns |  0.272 ns |  0.789 ns |    13.70 ns |   1.29 |    0.08 |  *** |
|        ActivatorCreateInstance | 1,234.59 ns | 24.540 ns | 30.137 ns | 1,248.06 ns | 118.20 |    6.59 | **** |
