using System;
using System.Collections.Generic;
using System.Text;

namespace SafeNote
{
    internal class CeasarCipher : ICipher
    {
        private int key;
        public CeasarCipher(int key)
        {
            this.key = key;
        }

        public string Encrypt(in string str)
        {
            char[] encrypted_text = str.ToCharArray();
            for (int i = 0; i < encrypted_text.Length; i++)
            {
                encrypted_text[i] += (char)key;
            }
            return new string(encrypted_text);
        }
        public string Decrypt(in string text)
        {
            char[] decrypted_text = text.ToCharArray();
            for (int i = 0; i < decrypted_text.Length; i++)
            {
                decrypted_text[i] -= (char)key;
            }
            return new string(decrypted_text);
        }
    }
}
