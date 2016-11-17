using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Training_BlackJack.Exceptions
{
    public class CardException : Exception
    {
        public CardException() { }
        public CardException(string err) : base(err) { }
        public CardException(string err, Exception ex) : base(err, ex) { }
    }
}
