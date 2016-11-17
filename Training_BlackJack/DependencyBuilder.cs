using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace vNextDependencyBuilder
{
    public class DependencyBuilder<T>
    {
        public static DependencyBuilder<S> builderForValue<S>(S value)
        {
            return new DependencyBuilder<S>(Function<T>.constantValueFunction(value));
        }

        public DependencyBuilder()
        {

        }
        private readonly Function<T> defaultFactoryFunction;
        private Function<T> overrideFactoryFunction = null;

        public DependencyBuilder(Function<T> f)
        {
            this.defaultFactoryFunction = f;
        }

        /// <summary>
        /// Set this builder to use the given factory function to make new instances of
        /// the dependency at every call.
        /// </summary>
        public virtual Function<T> Factory
        {
            set
            {
                this.overrideFactoryFunction = value;
            }
        }

        /// <summary>
        /// Set this dependency builder so that it always returns the exact same value.
        /// </summary>
        public virtual T ConstantValue
        {
            set
            {
                this.overrideFactoryFunction = Function<T>.constantValueFunction(value);
            }
        }

        /// <summary>
        /// Reset the builder to use the default factory function.
        /// </summary>
        public virtual void reset()
        {
            this.overrideFactoryFunction = null;
        }

        /// <summary>
        /// Get an instance of the thing provided by this dependency builder.
        /// </summary>
        public virtual T make()
        {
            if (overrideFactoryFunction != null)
            {
                return overrideFactoryFunction.invoke();
            }
            return defaultFactoryFunction.invoke();
        }
    }
}