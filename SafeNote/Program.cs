using System;
using System.IO;
using System.Collections.Generic;

namespace SafeNote
{
    class Cryptor
    {
        public static string[] Encrypt(in string[] text, ICipher cipher)
        {
            List<string> strs = new List<string>();
            foreach (var s in text)
                strs.Add(cipher.Encrypt(s));
            return strs.ToArray();
        }
        public static string[] Decrypt(in string[] text, ICipher cipher)
        {
            List<string> strs = new List<string>();
            foreach (var s in text)
                strs.Add(cipher.Decrypt(s));
            return strs.ToArray();
        }

    }

    class FileManager
    {
        public static string[] ReadTextFromFile(string filename)
        {
            List<string> strs = new List<string>();
            using (StreamReader sr = new StreamReader(filename))
            {
                while (!sr.EndOfStream)
                    strs.Add(sr.ReadLine());
            }
            return strs.ToArray();
        }
        public static string[] GetFilenames()
        {
            return Directory.GetFiles(@"C:\Users\Halasas\Desktop\Cryptor");
        }
        public static void WriteTextToFile(string[] strs, string filename)
        {
            using (StreamWriter sw = new StreamWriter(filename))
            {
                foreach (var s in strs)
                    sw.WriteLine(s);
            }
        }
    }

    public class SafeNoteUI
    {
        static void Main(string[] args)
        {
            //-----------------------INPUT------------------------//
            int file_id = -1;
            int key;
            string filename;
            int count = 0;
            string[] files = FileManager.GetFilenames();

            Console.WriteLine("Choose File from list or create new");
            foreach (var s in files)
                Console.WriteLine("{0,3}|   {1}", count++, s);
            Console.WriteLine("{0,3}|   new Note", files.Length);
            Console.WriteLine("Write <file_id> <key> to openfile", files.Length);

            while (!ParseInputForFileChoosing(Console.ReadLine(), out file_id, out key) ||
                file_id < 0 ||
                file_id > files.Length) ;
            if (file_id == files.Length)
            {
                using (File.Create(filename = @"C:\Users\Halasas\Desktop\Cryptor\" + new Random().Next().ToString())) { };
            }
            else 
            { 
                filename = files[file_id];
            }
            //-----------------------EDITOR------------------------//
            ICipher cipher = new CeasarCipher(key);
            List<string> text = new List<string>(Cryptor.Decrypt(FileManager.ReadTextFromFile(filename), cipher));
            while (true)
            {
                Console.WriteLine(filename);
                Console.WriteLine("e - save and exit\n" +
                    "d <num_line> - delete string\n" +
                    "i <num_line> <string> - insert string\n" +
                    "n <string> - new string");
                Console.WriteLine();
                for (int i = 0; i < text.Count; i++)
                    Console.WriteLine("{0,3}    {1}", i, text[i]);
                int num_line;
                string str;
                char mode;
                if (ParseInputForEditor(Console.ReadLine(), out mode, out num_line, out str))
                {
                    if (mode == 'e')
                        break;
                    if (mode == 'd')
                        text.RemoveAt(num_line);
                    if (mode == 'n')
                        text.Add(str);
                    if (mode == 'i')
                        text.Insert(num_line, str);
                }
            }
            FileManager.WriteTextToFile(Cryptor.Encrypt(text.ToArray(), cipher), filename);
        }
        static bool ParseInputForEditor(in string text, out char mode, out int num_line, out string str)
        {
            mode = text[0];
            num_line = 0;
            str = "";
            if (mode == 'e')
                return true;
            if (mode == 'n')
            {
                str = text.Substring(2);
                return true;
            }
            else if (mode == 'i')
            {
                string text1 = text.Substring(2);
                str = text1.Substring(text1.IndexOf(" ") + 1);
            }
            if (int.TryParse(text.Split(' ')[1], out num_line))
                return true;
            return false;
        }
        static bool ParseInputForFileChoosing(in string str, out int file_id, out int key)
        {
            string[] strs = str.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            key = Int32.Parse(strs[1]);
            if (!int.TryParse(strs[0], out file_id))
                return false;
            return true;
        }

    }
}
