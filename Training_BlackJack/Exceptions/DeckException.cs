using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Training_BlackJack
{
    public class DeckException: Exception
    {
        public DeckException() { }
        public DeckException(string err) : base(err) { }
        public DeckException(string err, Exception ex) : base(err, ex) { }
    }

}
