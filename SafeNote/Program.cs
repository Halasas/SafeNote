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
        private const string DEFAULT_DIRECTORY = @"C:\SafeNotes";
        public string WorkDirectory { get; set; }
        public string[] GetFilenames()
        {
            while (true)
            {
                Console.WriteLine("Enter path to work directory or tap ENTER to use default directory");
                string directory_name = Console.ReadLine();
                if (directory_name.Length == 0) 
                { 
                    directory_name = DEFAULT_DIRECTORY;
                }

                if (Directory.Exists(directory_name))
                {
                    WorkDirectory = directory_name;
                    return Directory.GetFiles(directory_name);
                }
                else if (directory_name == DEFAULT_DIRECTORY)
                {
                    WorkDirectory = DEFAULT_DIRECTORY;
                    Directory.CreateDirectory(DEFAULT_DIRECTORY);
                    return Directory.GetFiles(directory_name);
                }
                else
                {
                    Console.WriteLine("Directory is not exitsts");
                }
            }
        }

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


            FileManager fileManager = new FileManager();
            string[] files = fileManager.GetFilenames();
            
            Console.WriteLine("Choose Note from list or create new one");
            int count = 0;
            foreach (var s in files)
                Console.WriteLine("{0,3}|   {1}", count++, s);
            Console.WriteLine("{0,3}|   new Note", count);
            Console.WriteLine("Write <file_id> <key> to open Note", files.Length);

            int file_id = -1;
            int key;
            string filename;
            while (!ParseInputForFileChoosing(Console.ReadLine(), out file_id, out key) ||
                file_id < 0 || file_id > files.Length);

            if (file_id == files.Length)
            {
                using (File.Create(filename = fileManager.WorkDirectory + '\\' 
                    + DateTime.Now.Year + '_'+ DateTime.Now.Month+ '_'+ DateTime.Now.Day + '_'+
                    + DateTime.Now.Hour + '_'+ DateTime.Now.Minute+ '_'+ DateTime.Now.Second)) { };
            }
            else
            {
                filename = files[file_id];
            }
            //-----------------------EDITOR------------------------//
            ICipher cipher = new CeasarCipher(key);
            List<string> text = new List<string>(Cryptor.Decrypt(FileManager.ReadTextFromFile(filename), cipher));
            bool delete = false;
            while (true)
            {
                Console.WriteLine(filename);
                Console.WriteLine("e - save and exit\n" +
                    "x - delete this note" +
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
                    if (mode == 'x')
                    {
                        delete = true;
                        break;
                    }
                    if (mode == 'd')
                        text.RemoveAt(num_line);
                    if (mode == 'n')
                        text.Add(str);
                    if (mode == 'i')
                        text.Insert(num_line, str);
                }
            }
            if (!delete)
                FileManager.WriteTextToFile(Cryptor.Encrypt(text.ToArray(), cipher), filename);
            else
                File.Delete(filename);
        }
        static bool ParseInputForEditor(in string text, out char mode, out int num_line, out string str)
        {
            mode = text[0];
            num_line = 0;
            str = "";
            if (mode == 'x')
                return true;
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
