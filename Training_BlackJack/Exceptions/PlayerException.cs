using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Training_BlackJack.Exceptions
{
    public class PlayerException : Exception
    {
        public PlayerException() { }
        public PlayerException(string err) : base(err) { }
        public PlayerException(string err, Exception ex) : base(err, ex) { }
    }
}
