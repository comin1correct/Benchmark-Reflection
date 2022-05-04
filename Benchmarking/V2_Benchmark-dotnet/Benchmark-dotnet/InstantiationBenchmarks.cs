using System;
using System.Linq.Expressions;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using Benchmark_dotnet;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Columns;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Engines;
using BenchmarkDotNet.Environments;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Mathematics;
using BenchmarkDotNet.Order;

[SimpleJob(RunStrategy.Monitoring)]
[MemoryDiagnoser()]
[Orderer((SummaryOrderPolicy.FastestToSlowest))]
[RankColumn(NumeralSystem.Stars)]
[Config(typeof(Config))]
public class InstantiationBenchmarks
{
    
    private class Config : ManualConfig
    {
        public Config()
        {
            AddJob(Job.Default.WithRuntime(ClrRuntime.Net461).WithId("Inlining enabled"));
        }
    }

    private static Type StringBuilderType = Type.GetType("System.Text.StringBuilder");
    private readonly Func<object> _dynamicMethodActivator;
    private readonly Func<object> _expression;
    private static readonly InstantiationTestTwo InstantiationTestTwo = new InstantiationTestTwo();
    
    public InstantiationBenchmarks()
    {
        ConstructorInfo Ctor = StringBuilderType.GetConstructor(System.Type.EmptyTypes);
            
        DynamicMethod createStringBuilderMethod = new DynamicMethod(
            name: $"StringBuilderMethod",
            returnType: StringBuilderType,
            parameterTypes: null,
            typeof(InstantiationBenchmarks).Module,
            skipVisibility: false);
            
        ILGenerator il = createStringBuilderMethod.GetILGenerator();
        il.Emit(OpCodes.Newobj, Ctor);
        il.Emit(OpCodes.Ret);

        _dynamicMethodActivator = (Func<object>)createStringBuilderMethod.CreateDelegate(typeof(Func<object>));

        _expression = Expression.Lambda<Func<object>>(Expression.New(StringBuilderType)).Compile();
    }
    

    // [Benchmark]
    // public void StandardReflectionWithInvoke()
    // {
    //     InstantiationTestTwo.StandardReflectionWithInvoke();
    // }

    [Benchmark]
    public void ActivatorCreateInstance()
    {
        InstantiationTestTwo.ActivatorCreateInstance();
    }

    [Benchmark(Baseline = true)]
    public void DirectInstantiation()
    {
        InstantiationTestTwo.DirectInstantiation();
    }
    
    // [Benchmark]
    // public void CompiledExpressions()
    // {
    //     InstantiationTestTwo.CompiledExpressions();
    // }

    // [Benchmark]
    // public void ReflectionEmitSeperateClass()
    // {
    //     InstantiationTestTwo.ReflectionEmit(StringBuilderType);
    // }

    [Benchmark]
    public void ReflectionEmitClassConstructor()
    {
        _dynamicMethodActivator();
    }
}