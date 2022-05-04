using System;
using System.Reflection.Emit;
using ReflectionPerformance.External.Interfaces;

namespace ReflectionPerformance.External
{
    public class BTinyProcessorILGenerator
    {
        private Func<object> _dynamicMethodActivator;
        private static string _namespace = "ReflectionPerformance.External.BObjects";  

        public void ProcessByILGenerator(IDataDictionaryObject ddo)
        {
            BuildDynamicMethodDelegate(ddo.Name);
            var obj =(IETL)_dynamicMethodActivator();
            obj.LoadForETL();
        }

        public void BuildDynamicMethodDelegate(string dataDictionaryObjectName)
        {
            var objectType = Type.GetType($"{_namespace}.{dataDictionaryObjectName}");
            var ctor = objectType.GetConstructor(Type.EmptyTypes);

            var createStringBuilderMethod = new DynamicMethod(
                name: $"BTinyProcessorILGenerator_Create_{objectType.FullName}",
                returnType: objectType,
                parameterTypes: Type.EmptyTypes);
            
            ILGenerator il = createStringBuilderMethod.GetILGenerator();
            il.Emit(OpCodes.Newobj, ctor);
            il.Emit(OpCodes.Ret);
            
            _dynamicMethodActivator = (Func<object>)createStringBuilderMethod.CreateDelegate(typeof(Func<object>));
            
        }
    }
}