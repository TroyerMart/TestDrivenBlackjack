using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using vNextDependencyBuilder;

namespace ServiceRepository
{
    public class RESTClient
    {
        // just stubbed out until we figure out where to find/define this
    }

    public class RepositoryDependencies
    {
        // this is just an example of declaring an instance
        public static readonly DependencyBuilder<RESTClient> restClientBuilder = DependencyBuilder<RESTClient>.builderForValue(new RESTClient());

        public static void resetAll()
        {
            restClientBuilder.reset();
        }
    }
}
