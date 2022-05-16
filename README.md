# Benchmark-Reflection
Exploring performance with different reflection approaches 
|                         Method |          Mean |         Error |        StdDev | Rank |  Gen 0 |  Gen 1 | Allocated |
|------------------------------- |--------------:|--------------:|--------------:|-----:|-------:|-------:|----------:|
|            DirectInstantiation |      7.659 ns |     0.1692 ns |     0.1661 ns |    1 | 0.0083 |      - |     104 B |
| ReflectionEmitClassConstructor |      8.893 ns |     0.1980 ns |     0.2357 ns |    2 | 0.0083 |      - |     104 B |
|        ActivatorCreateInstance |  1,237.329 ns |    23.2266 ns |    20.5898 ns |    3 | 0.0076 |      - |     104 B |
|   StandardReflectionWithInvoke |  1,383.020 ns |    27.5931 ns |    37.7698 ns |    4 | 0.0076 |      - |     104 B |
|    ReflectionEmitSeperateClass | 49,512.627 ns |   824.3529 ns | 1,012.3793 ns |    5 | 0.3662 | 0.1831 |   5,044 B |
|            CompiledExpressions | 55,737.628 ns | 1,015.1524 ns |   847.6982 ns |    6 | 0.3052 | 0.1221 |   4,031 B |
