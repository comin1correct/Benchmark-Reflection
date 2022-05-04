namespace Performance_tester
{
    public interface IPreformanceTest
    {
        public string Name { get; }
        public string Description { get; set; }
        public int Iterations { get; set; }
        public bool RunBaseLine { get; set; }
        public (int, int, int) Measure();
    }
}