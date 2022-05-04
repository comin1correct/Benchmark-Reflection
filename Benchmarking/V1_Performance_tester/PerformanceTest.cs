using System.Diagnostics;

namespace Performance_tester
{
    public class PerfromanceTest : IPreformanceTest
    {

        private const int DEFAULT_REPETITIONS = 10;
        public string Name { get; }
        public string Description { get; set; }
        public int Iterations { get; set; }
        public bool RunBaseLine { get; set; }

        protected virtual bool MeasureTestA()
        {
            return false;
        }

        protected virtual bool MeasureTestB()
        {
            return false;
        }

        protected virtual bool MeasureTestC()
        {
            return false;
        }

        public PerfromanceTest(string name, string description, int interactions)
        {
            Name = name;
            Description = description;
            Iterations = interactions;
        }

        public (int, int, int) Measure()
        {
            long totalA = 0, totalB = 0, totalC = 0;

            var stopwatch = new Stopwatch();

            // run baseline tests
            if (RunBaseLine)
            {
                for (long i = 0; i < DEFAULT_REPETITIONS; i++)
                {
                    stopwatch.Restart();
                    var implemented = MeasureTestA();
                    stopwatch.Stop();
                    if (implemented)
                        totalA += stopwatch.ElapsedMilliseconds;
                }
            }

            // run optimized test B
            for (long i = 0; i < DEFAULT_REPETITIONS; i++)
            {
                stopwatch.Restart();
                var implemented = MeasureTestB();
                stopwatch.Stop();
                if (implemented)
                    totalB += stopwatch.ElapsedMilliseconds;
            }

            // run optimized test C
            for (long i = 0; i < DEFAULT_REPETITIONS; i++)
            {
                stopwatch.Restart();
                var implemented = MeasureTestC();
                stopwatch.Stop();
                if (implemented)
                    totalC += stopwatch.ElapsedMilliseconds;
            }

            return (
                (int)(totalA / DEFAULT_REPETITIONS),
                (int)(totalB / DEFAULT_REPETITIONS),
                (int)(totalC / DEFAULT_REPETITIONS));
        }
    }
}