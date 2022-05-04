using BenchmarkDotNet.Running;
using ReflectionPerformance.External;

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
            //var bProcessorEmit = new BTinyProcessor();

            foreach (var ddo in dataDictionaryObject.ddos)
            {
                //bProcessorEmit.ProcessEmitIL(ddo);   
                bProcessorIlGenerator.ProcessByILGenerator(ddo);
            }
        }
    }
}