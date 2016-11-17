using System;

namespace Training_BlackJack.Exceptions
{
    public class GameException : Exception
    {
        public GameException() { }
        public GameException(string err) : base(err) { }
        public GameException(string err, Exception ex) : base(err, ex) { }
    }
}
