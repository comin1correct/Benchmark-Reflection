using System.Reflection;
using System.Reflection.Emit;

namespace Performance_tester
{
    public class InstantiationTest: PerfromanceTest
    {
        private const int DEAFUALT_ITERATIONS = 1_000_000;

        private delegate object ConstructorDelegate();
        public InstantiationTest() : base("Instantiation", "A:reflection, B:dynamic CIL, C:compile-time", DEAFUALT_ITERATIONS) { }

        private ConstructorDelegate GetConstructor(string typeName)
        {
            //get the default constructor of the type
            Type t = Type.GetType(typeName);
            ConstructorInfo ctor = t.GetConstructor(Type.EmptyTypes);

            // create a new dynamic method that constructs and returns the type
            string methodName = t.Name + "Ctor";
            DynamicMethod dm = new DynamicMethod(methodName, t, Type.EmptyTypes, typeof(Activator));
            ILGenerator lgen = dm.GetILGenerator();
            lgen.Emit(OpCodes.Newobj, ctor);
            lgen.Emit(OpCodes.Ret);

            // add delegate to dictionary and return
            ConstructorDelegate creator = (ConstructorDelegate)dm.CreateDelegate(typeof(ConstructorDelegate));

            //return a delegate to the method
            return creator;
        }

        protected override bool MeasureTestA()
        {
            // instantiate string builder using reflection
            var type = Type.GetType("System.Text.StringBuilder");
            for (var i = 0; i < Iterations; i++)
            {
                var obj = Activator.CreateInstance(type);
                if (obj.GetType() != typeof(System.Text.StringBuilder))
                    throw new InvalidOperationException("Constructed object is not a StringBuilder");
            }

            return true;
        }

        protected override bool MeasureTestB()
        {
            // instantiate string builder using dynamic CIL
            var constructor = GetConstructor("System.Text.StringBuilder");
            for (var i = 0; i < Iterations; i++)
            {
                var obj = constructor();
                if (obj.GetType() != typeof(System.Text.StringBuilder))
                    throw new InvalidOperationException("Constructed object is not a StringBuilder");
            }

            return true;
        }

        protected override bool MeasureTestC()
        {
            // instantiate string builder directly
            for (var i = 0; i < Iterations; i++)
            {
                var obj = new System.Text.StringBuilder();
                if (obj.GetType() != typeof(System.Text.StringBuilder))
                    throw new InvalidOperationException("Constructed object is not a StringBuilder");
            }

            return true;
        }
    }
}