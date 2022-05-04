using System.Collections.Generic;
using ReflectionPerformance.External.Interfaces;

namespace ReflectionPerformance.External
{
    public class DataDictionaryObject : IDataDictionaryObject
    {
        public string Name { get; set; }
        public IList<DataDictionaryObject> ddos = new List<DataDictionaryObject>();
        
        public void Add(string name)
        {
            ddos.Add(new DataDictionaryObject(){Name = name});
        }
    }
}