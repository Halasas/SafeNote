using Microsoft.VisualStudio.TestTools.UnitTesting;
using SafeNote;
using System.Collections.Generic;

namespace SafeNoteTests
{
    [TestClass]
    public class SafeNoteTest
    {
        string RandomString(int length)
        {
            List<char> chars = new List<char>();
            System.Random rand = new System.Random();
            for(int i = 0; i < length; i++)
                 chars.Add((char)rand.Next(0, 255));
            return new string(chars.ToArray());
        }
        string[] RandomText(int length)
        {
            System.Random rand = new System.Random();
            List<string> textList = new List<string>();
            for (int j = 0; j < length; j++)
                textList.Add(RandomString(2000 + rand.Next(-1000, 1000)));
            return textList.ToArray();
        }
        [TestMethod]
        public void CryptorStringTests()
        {
            System.Random rand = new System.Random();
            for(int i = 0; i <= 10000; i++) 
            {
                string input = RandomString(2000 + rand.Next(-1000, 1000));
                string key = RandomString(100 + rand.Next(-50, 50));
                string encrypted_input = Cryptor.EncryptString(input, key);
                string decrypted_input = Cryptor.DecryptString(encrypted_input, key);
                Assert.AreEqual(input, decrypted_input);
            }
        }
        [TestMethod]
        public void CryptorTextTests()
        {
            System.Random rand = new System.Random();
            for (int i = 0; i <= 50; i++)
            {
                int textsize = rand.Next(1, 500);
                string[] text = RandomText(textsize);
                string key = RandomString(100 + rand.Next(-50, 50));
                string[] encrypted_text = Cryptor.Encrypt(text, key);
                string[] decrypted_text = Cryptor.Decrypt(encrypted_text, key);
                for(int j = 0; j < text.Length; j++)
                    Assert.AreEqual(text[j], decrypted_text[j]);
            }
        }
        [TestMethod]
        public void FileManagerTests()
        {
            foreach (var file in System.IO.Directory.GetFiles(@"C:\Users\Halasas\Desktop\Cryptor"))
                System.IO.File.Delete(file);
            using (System.IO.File.Create(@"C:\Users\Halasas\Desktop\Cryptor\File1.txt")) { }
            using (System.IO.File.Create(@"C:\Users\Halasas\Desktop\Cryptor\File2.doc")) { }
            using (System.IO.File.Create(@"C:\Users\Halasas\Desktop\Cryptor\File3.cpp")) { }
            string[] files = FileManager.GetFilenames();
            Assert.AreEqual(files[0], @"C:\Users\Halasas\Desktop\Cryptor\File1.txt");
            Assert.AreEqual(files[1], @"C:\Users\Halasas\Desktop\Cryptor\File2.doc");
            Assert.AreEqual(files[2], @"C:\Users\Halasas\Desktop\Cryptor\File3.cpp");
            foreach (var file in System.IO.Directory.GetFiles(@"C:\Users\Halasas\Desktop\Cryptor"))
                System.IO.File.Delete(file);
        }
        [TestMethod]
        public void FileManagerReadAndWriteTest()
        {
                string filename = @"C:\Users\Halasas\Desktop\Cryptor\File1.txt";
                using (System.IO.File.Create(filename)) { }
                string[] text = RandomText(500);
                FileManager.WriteTextToFile(text, filename); 
        }
        
    }
}
