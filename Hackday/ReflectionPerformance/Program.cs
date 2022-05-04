using BenchmarkDotNet.Running;
using ReflectionPerformance.External;
using System;

namespace ReflectionPerformance
{
    class Program
    {
        static void Main(string[] args)
        {
            //BenchmarkRunner.Run<ReflectionBenchmarks>();

            DataDictionaryObject dataDictionaryObject = new DataDictionaryObject();
            dataDictionaryObject.Add("b_KPIComponentLibrary");
            dataDictionaryObject.Add("b_ExtensionMetadataCulture");
            var bProcessorIlGenerator = new BTinyProcessorILGenerator();
            //var bProcessorEmitSigil = new BTinyProcessorSigil();


            foreach (var ddo in dataDictionaryObject.ddos)
            {
                //var eTLObject = bProcessorEmitSigil.CreateInstance(ddo);
                var eTLObject = bProcessorIlGenerator.CreateInstance(ddo);
                eTLObject.LoadValueFromBObect();

            }

            Console.ReadLine();
        }
    }
}