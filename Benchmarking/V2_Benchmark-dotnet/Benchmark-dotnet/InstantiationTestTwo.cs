using System.ComponentModel;
using System.Linq.Expressions;
using System.Reflection;
using System.Reflection.Emit;
using System;

namespace Benchmark_dotnet
{
    public class InstantiationTestTwo
    {
        private Func<object> _dynamicMethodActivator;
        private Func<object> _expression;
        
        private delegate object ConstructorDelegate();

        // private ConstructorDelegate GetConstructor(string typeName)
        // {
        //     //get the default constructor of the type
        //     Type t = Type.GetType(typeName);
        //     ConstructorInfo ctor = t.GetConstructor(Type.EmptyTypes);
        //
        //     // create a new dynamic method that constructs and returns the type
        //     string methodName = t.Name + "Ctor";
        //     DynamicMethod dm = new DynamicMethod(methodName, t, Type.EmptyTypes, typeof(Activator));
        //     ILGenerator lgen = dm.GetILGenerator();
        //     lgen.Emit(OpCodes.Newobj, ctor);
        //     lgen.Emit(OpCodes.Ret);
        //
        //     // add delegate to dictionary and return
        //     ConstructorDelegate creator = (ConstructorDelegate) dm.CreateDelegate(typeof(ConstructorDelegate));
        //
        //     //return a delegate to the method
        //     return creator;
        // }

        public void StandardReflectionWithInvoke()
        {
            Type typeToCreate = Type.GetType("System.Text.StringBuilder");
            var ctor = typeToCreate.GetConstructor(System.Type.EmptyTypes);
            object headers = ctor.Invoke(null);
        }

        public bool ActivatorCreateInstance()
        {
            // instantiate string builder using reflection
            var type = Type.GetType("System.Text.StringBuilder");
            var obj = Activator.CreateInstance(type);

            return true;
        }

        // public void DynamicCLIDonePoorly()
        // {
        //     // instantiate string builder using dynamic CIL
        //     var constructor = GetConstructor("System.Text.StringBuilder");
        //
        //     var obj = constructor();
        // }

        public void DirectInstantiation()
        {
            // instantiate string builder directly
            var obj = new System.Text.StringBuilder();
        }
        
        public void CompiledExpressions()
        {
            NewExpression constructorExpression = Expression.New(Type.GetType("System.Text.StringBuilder"));
            Expression<Func<object>> lambdaExpression = Expression.Lambda<Func<object>>(constructorExpression);
            Func<object> createHeadersFunc = lambdaExpression.Compile();  
            object Headers = createHeadersFunc();
        }

        public void ReflectionEmit(Type type)
        {
            ConstructorInfo Ctor = type.GetConstructor(System.Type.EmptyTypes);
            
            DynamicMethod createStringBuilderMethod = new DynamicMethod(
                name: $"StringBuilderMethod",
                returnType: type,
                parameterTypes: null,
                typeof(InstantiationTestTwo).Module,
                skipVisibility: false);
            
            ILGenerator il = createStringBuilderMethod.GetILGenerator();
            il.Emit(OpCodes.Newobj, Ctor);
            il.Emit(OpCodes.Ret);

            _dynamicMethodActivator = (Func<object>)createStringBuilderMethod.CreateDelegate(typeof(Func<object>));

            _expression = Expression.Lambda<Func<object>>(Expression.New(type)).Compile(); 
        }
    }
}