using System;
using System.Linq.Expressions;
using System.Reflection;
using Sigil;

namespace ReflectionPerformance
{
    public class ReflectionUsage
    {
        public void DirectInstantiation()
        {
            // instantiate string builder directly
            var obj = new System.Text.StringBuilder();
        }
        
        // public void StandardReflectionWithInvoke()
        // {
        //     Type typeToCreate = Type.GetType("System.Text.StringBuilder");
        //     var ctor = typeToCreate.GetConstructor(System.Type.EmptyTypes);
        //     object headers = ctor.Invoke(null);
        // }

        public void ActivatorCreateInstance()
        {
            // instantiate string builder using reflection
            var type = Type.GetType("System.Text.StringBuilder");
            var obj = Activator.CreateInstance(type);
            
        }

        // public void CompiledExpressions()
        // {
        //     NewExpression constructorExpression = Expression.New(Type.GetType("System.Text.StringBuilder"));
        //     Expression<Func<object>> lambdaExpression = Expression.Lambda<Func<object>>(constructorExpression);
        //     Func<object> createStringBuilder = lambdaExpression.Compile();  
        //     object stringBuilder = createStringBuilder();
        // }
        
        
        
        private static readonly Type StringBuilderClassType = Type.GetType("System.Text.StringBuilder");
        
        static ConstructorInfo ctor = StringBuilderClassType.GetConstructor(Type.EmptyTypes);
        private static readonly Emit<Func<object, object>> GetInstanceEmitter =
            Emit<Func<object, object>>
                .NewDynamicMethod("GetStringBuilderObject")
                .NewObject(ctor)
                .Return();
        
        private static readonly Func<object, object> GetInstanceEmittedDelegate =
            GetInstanceEmitter.CreateDelegate();

        public static object EmittedILVersion()
        {
            //https://github.com/kevin-montrose/Sigil/blob/master/tests/SigilTests/NewObject.cs
            return GetInstanceEmittedDelegate(StringBuilderClassType);
        }
    }
}