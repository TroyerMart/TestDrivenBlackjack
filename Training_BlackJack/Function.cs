using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace vNextDependencyBuilder
{
    public class Function<T>
    {
        public object PassedClass { get; set; }

        public static Function<S> constantValueFunction<S>(S value)
        {
            Function<S> result = null;
            try
            {
                result = new Function<S>();
                result.PassedClass = value;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        public T invoke()
        {
            return (T)this.PassedClass;
        }
    }

}


