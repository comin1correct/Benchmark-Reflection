using System;
using System.Linq.Expressions;
using System.Reflection;
using System.Reflection.Emit;
using ReflectionPerformance.External.Interfaces;
using Sigil;

namespace ReflectionPerformance.External
{
    public class BTinyProcessor
    {
        private static string _namespace = "ReflectionPerformance.External.BObjects";
        private static Func<object, IETL> _getInstanceEmittedDelegate;
        
        public void ProcessEmitIL(IDataDictionaryObject ddo)
        {
            var obj = EmittedILVersion(ddo);
            obj.LoadForETL();
        }
        
        private IETL EmittedILVersion(IDataDictionaryObject ddo)
        {
            var objectType = Type.GetType($"{_namespace}.{ddo.Name}");
            var ctor = objectType.GetConstructor(Type.EmptyTypes);

            var getInstanceEmitter = Emit<Func<object, IETL>>
                .NewDynamicMethod($"BTinyProcessor_Create_{objectType.FullName}")
                .NewObject(ctor)
                .Return();

            _getInstanceEmittedDelegate = getInstanceEmitter.CreateDelegate();
            return _getInstanceEmittedDelegate(objectType);
        }
    }
}