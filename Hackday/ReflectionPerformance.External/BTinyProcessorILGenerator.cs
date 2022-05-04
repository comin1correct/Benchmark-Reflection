using System;
using System.Reflection;
using System.Reflection.Emit;
using ReflectionPerformance.External.Interfaces;

namespace ReflectionPerformance.External
{
    public class BTinyProcessorILGenerator
    {
        public Type objectType;
        public ConstructorInfo defaultConstructor;
        private Func<object> _dynamicMethodActivator;
        //private delegate IETL _dynamicMethodActivator();
        //_dynamicMethodActivator _DynamicMethodActivator;

        private static string _namespace = "ReflectionPerformance.External.BObjects";


        public BTinyProcessorILGenerator() { }
        public BTinyProcessorILGenerator(Type type) { }
        public BTinyProcessorILGenerator(string typeName) { }
        public BTinyProcessorILGenerator(string assemblyName, string typeName) { }


        public IETL CreateInstance(IDataDictionaryObject ddo)
        {
            objectType = Type.GetType($"{_namespace}.{ddo.Name}");
            defaultConstructor = objectType.GetConstructor(Type.EmptyTypes);
            BuildDynamicMethod();

            return (IETL)_dynamicMethodActivator();
            //return _DynamicMethodActivator();
        }

        public void BuildDynamicMethod()
        {
            var createStringBuilderMethod = new DynamicMethod(
                name: $"BTinyProcessorILGenerator_Create_{objectType.FullName}",
                returnType: objectType,
                parameterTypes: Type.EmptyTypes);
            
            ILGenerator il = createStringBuilderMethod.GetILGenerator();
            il.Emit(OpCodes.Newobj, defaultConstructor);
            il.Emit(OpCodes.Ret);

            //_DynamicMethodActivator = (_dynamicMethodActivator)createStringBuilderMethod.CreateDelegate(typeof(_dynamicMethodActivator));
            _dynamicMethodActivator = (Func<object>)createStringBuilderMethod.CreateDelegate(typeof(Func<object>));
        }
    }
}