using System;
using System.Collections.Generic;
using System.Text;

namespace SafeNote
{
    interface ICipher
    {
        public string Encrypt(in string str);
        public string Decrypt(in string str);
    }
}
