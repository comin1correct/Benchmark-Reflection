using System;
using System.Reflection;
using ReflectionPerformance.External.Interfaces;
using Sigil;

namespace ReflectionPerformance.External
{
    public class BTinyProcessorSigil
    {
        public Type objectType;
        public ConstructorInfo defaultConstructor;
        private Func<IETL> _getInstanceEmittedDelegate;
        private readonly string _namespace = "ReflectionPerformance.External.BObjects";

        public BTinyProcessorSigil() { }
        public BTinyProcessorSigil(Type type) { }
        public BTinyProcessorSigil(string typeName) { }
        public BTinyProcessorSigil(string assemblyName, string typeName) { }

        public IETL CreateInstance(IDataDictionaryObject ddo)
        {
            objectType = Type.GetType($"{_namespace}.{ddo.Name}");
            defaultConstructor = objectType.GetConstructor(Type.EmptyTypes);
            BuildDynamicMethod(objectType);

            return _getInstanceEmittedDelegate();
        }

        private void BuildDynamicMethod(Type type)
        {
            var getInstanceEmitter = Emit<Func<IETL>>
                .NewDynamicMethod($"BTinyProcessor_Create_{type.FullName}")
                .NewObject(defaultConstructor)
                .Return();

            _getInstanceEmittedDelegate = getInstanceEmitter.CreateDelegate();
        }
    }
}