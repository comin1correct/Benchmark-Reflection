namespace ReflectionPerformance.External.Interfaces
{
    public interface IDataDictionaryObject
    {
        string Name { get; set; }

        void Add(string name);
    }
}