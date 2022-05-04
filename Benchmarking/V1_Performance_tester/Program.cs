/*
video: https://www.youtube.com/watch?v=-H5oEgOdO6U&t=2760s
Enroll in my bootcamps and courses: http://mdfarragher.com/training
*/
namespace Performance_tester
{
    internal class Program
    {
        private static List<IPreformanceTest> tests = new List<IPreformanceTest>();

        private static void ShowAllTests()
        {
            Console.WriteLine("Available Tests:");
            for (int i = 0; i < tests.Count; i++)
            {
                Console.WriteLine($"{i}: {tests[i].Name}");
            }
            Console.WriteLine();
        }

        private static IPreformanceTest AskForTest()
        {
            Console.WriteLine("Please select a test: ");
            if (!int.TryParse(Console.ReadLine(), out int index))
                return null;
            else
                return index >= 0 && index < tests.Count ? tests[index] : null;
        }

        private static void ConfigureTest(IPreformanceTest test)
        {
            Console.WriteLine($"How many iterations ({test.Iterations})");
            int interations = 0;
            if (int.TryParse(Console.ReadLine(), out interations))
            {
                test.Iterations = interations;
            }
            Console.WriteLine($"Run baseline test? (yes): ");
            var input = Console.ReadLine();
            test.RunBaseLine = (string.IsNullOrEmpty(input)) || input.ToLower().StartsWith("y");
        }


        private static void ShowGraph((int, int, int) result)
        {
            const int NUM_STARS = 50;

            // normalize results
            var max = Math.Max(result.Item1, Math.Max(result.Item2, result.Item3));
            var barA = max > 0 ? result.Item1 * NUM_STARS / max : 0;
            var barB = max > 0 ? result.Item2 * NUM_STARS / max : 0;
            var barC = max > 0 ? result.Item3 * NUM_STARS / max : 0;

            // show bar graph
            Console.WriteLine($"A |{new string('\x25A0', barA)}");
            Console.WriteLine($"B |{new string('\x25A0', barB)}");
            Console.WriteLine($"C |{new string('\x25A0', barC)}");
            Console.WriteLine("   +-------------------------------------------------");
            Console.WriteLine($"    A: {result.Item1}ms, B: {result.Item2}ms, C: {result.Item3}ms \n");
        }

        private static void ShowTestInfoHeader(IPreformanceTest test)
        {
            Console.WriteLine($"{test.Name}");
            Console.WriteLine($"{test.Description}\n");
        }

        static void Main(string[] args)
        {
            // initialize test list
            tests.Add(new InstantiationTest());
            tests.Add(new PropertiesTest());

            Console.Clear();

            while (true)
            {
                // show test menu
                ShowAllTests();

                // get test
                var test = AskForTest();
                if (test == null)
                    return;

                // configure test
                ConfigureTest(test);

                // result
                ShowTestInfoHeader(test);
                ShowGraph(test.Measure());
            }
        }
    }
}
