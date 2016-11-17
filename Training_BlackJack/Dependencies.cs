using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Training_BlackJack.Interfaces;
using vNextDependencyBuilder;

namespace Training_BlackJack
{
    class Dependencies
    {
        public static DependencyBuilder<int> randomSeeds = DependencyBuilder<int>.builderForValue(Environment.TickCount);
        public static DependencyBuilder<Random> randomInstance = DependencyBuilder<Random>.builderForValue<Random>(new Random(randomSeeds.make()));

        public static DependencyBuilder<IConsoleIO> consoleIO = DependencyBuilder<IConsoleIO>.builderForValue<IConsoleIO>(new ConsoleIO());
        //public static DependencyBuilder<BlackjackOperations> gameOps = DependencyBuilder<BlackjackOperations>.builderForValue<BlackjackOperations>(new BlackjackOperations());

    }

}
