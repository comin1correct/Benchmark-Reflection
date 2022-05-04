using Benchmark_dotnet;
using BenchmarkDotNet.Running;

namespace Benchmark_dotnet
{
    class Program
    {
        static void Main(string[] args)
        {
            BenchmarkRunner.Run<InstantiationBenchmarks>();
        }
    }
}