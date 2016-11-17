using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Training_BlackJack.Exceptions
{
    public class HandException : Exception
    {
        public HandException() { }
        public HandException(string err) : base(err) { }
        public HandException(string err, Exception ex) : base(err, ex) { }
    }
}
