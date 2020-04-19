using System;
using System.Collections.Generic;
using System.Text;

namespace SafeNote
{
    class BillCipher : ICipher
    {
        private int key;
        public BillCipher(int key)
        {
            this.key = key;
        }
        public string Decrypt(in string str)
        {
            StringBuilder stringBuilder = new StringBuilder();
            for (int i = 0; i < key; i++)
            {
                stringBuilder.Append("Bill");
            }
            return stringBuilder.ToString();
        }

        public string Encrypt(in string str)
        {
            StringBuilder stringBuilder = new StringBuilder();
            for (int i = 0; i < key; i++)
            {
                stringBuilder.Append("Bill");
            }
            return stringBuilder.ToString();
        }
    }
}
