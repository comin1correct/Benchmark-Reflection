using System;
using System.Linq.Expressions;
using System.Reflection;
using System.Reflection.Emit;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Engines;
using BenchmarkDotNet.Environments;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Mathematics;
using BenchmarkDotNet.Order;

namespace ReflectionPerformance
{
    //[SimpleJob(RunStrategy.Monitoring)]
    // [MediumRunJob]
    // [MemoryDiagnoser()]
    [Orderer((SummaryOrderPolicy.FastestToSlowest))]
    [RankColumn(NumeralSystem.Stars)]
    [Config(typeof(Config))]
    public class ReflectionBenchmarks
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
        //private readonly Func<object> _expression;
        private static readonly ReflectionUsage InstantiationTestTwo = new ReflectionUsage();
        
        public ReflectionBenchmarks()
        {
            ConstructorInfo ctor = StringBuilderType.GetConstructor(Type.EmptyTypes);
            
            DynamicMethod createStringBuilderMethod = new DynamicMethod(
                name: $"StringBuilderMethod",
                returnType: StringBuilderType,
                parameterTypes: null,
                typeof(ReflectionBenchmarks).Module,
                skipVisibility: false);
            
            ILGenerator il = createStringBuilderMethod.GetILGenerator();
            il.Emit(OpCodes.Newobj, ctor);
            il.Emit(OpCodes.Ret);

            _dynamicMethodActivator = (Func<object>)createStringBuilderMethod.CreateDelegate(typeof(Func<object>));

            //_expression = Expression.Lambda<Func<object>>(Expression.New(StringBuilderType)).Compile();
        }
        
        [Benchmark(Baseline = true)]
        public void DirectInstantiation()
        {
            InstantiationTestTwo.DirectInstantiation();
        }
        
        [Benchmark]
        public void ActivatorCreateInstance()
        {
            InstantiationTestTwo.ActivatorCreateInstance();
        }
        
        [Benchmark]
        public void ReflectionEmitClassConstructor()
        {
            var temp = _dynamicMethodActivator();
        }
        
        [Benchmark]
        public void ReflectionEmitSigil()
        {
            var temp = ReflectionUsage.EmittedILVersion();
        }
    }
}