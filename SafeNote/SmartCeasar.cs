using System;
using System.Collections.Generic;
using System.Text;

namespace SafeNote
{
    class SmartCeasarCipher : ICipher
    {
        string key;
        public SmartCeasarCipher(string key) 
        {
            this.key = key;
        }
        public string Decrypt(in string str)
        {
            int count = 0;
            char[] decrypted_text = str.ToCharArray();
            for (int i = 0; i < decrypted_text.Length; i++)
            {
                decrypted_text[i] -= (char)(count * 17 + key[count]);
                if (++count == key.Length)

                    count = 0;
            }
            return new string(decrypted_text);
        }

        public string Encrypt(in string str)
        {
            int count = 0;
            char[] encrypted_text = str.ToCharArray();
            for (int i = 0; i < encrypted_text.Length; i++)
            {
                encrypted_text[i] += (char)(count * 17 + key[count]);
                if (++count == key.Length)
                    count = 0;
            }
            return new string(encrypted_text);
        }
    }
}
