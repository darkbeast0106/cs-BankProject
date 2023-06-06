using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankProject
{
    // Nem létező számla esetén dobhatjuk bármely függvényből
    public class HibasSzamlaszamException: Exception
    {
        public HibasSzamlaszamException(string szamlaszam)
        : base("Hibas szamlaszam: " + szamlaszam)
        {
        }
    }
}
