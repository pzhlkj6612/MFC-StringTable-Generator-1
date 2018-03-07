using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace MFC_StringTable_Generator_1
{
    class Program
    {
        static void Main(string[] args)
        {
            string DefaultFolderName = "out";
            while (Directory.Exists(DefaultFolderName))
                DefaultFolderName = DefaultFolderName + Guid.NewGuid().ToString("N");
            Directory.CreateDirectory(DefaultFolderName);
            string path_rc = DefaultFolderName + "/rc.txt";
            string path_h = DefaultFolderName + "/h.txt";

            string prefix = "", suffix = "";

            Console.Write("Enter prefix:");
            prefix = Console.ReadLine();

            Console.Write("Enter suffix:");
            suffix = Console.ReadLine();

            int result, start, count;

            Range:
            Console.Write("Enter start index:");
            while (!int.TryParse(Console.ReadLine(), out result))
            {
                Console.Write("Error. Enter start index:");
            }
            start = result;// 60000;

            Console.Write("Enter count:");
            while (!int.TryParse(Console.ReadLine(), out result))
            {
                Console.Write("Error. Enter count:");
            }
            count = result;// 100

            if (start < 0 || count < 0 || start + count > 65535)
            {
                Console.WriteLine("Incorrect.");
                goto Range;
            }

            string[] A2Z = new string[26];
            for (int i = 0; i < 26; i++)
            {
                A2Z[i] = Convert.ToChar(65 + i).ToString();
            }

            string ID, text_h, text_rc;

            using (StreamWriter sw_rc = new StreamWriter(File.Open(path_rc, FileMode.Append)), sw_h = new StreamWriter(File.Open(path_h, FileMode.Append)))
            {
                Random rd = new Random();
                for (int i = start; i < start + count; i++)
                {
                    ID = string.Format("{0}{1:D5}{2}", prefix, i, suffix);

                    text_h = string.Format("#define {0} {1}", ID, i);
                    text_rc = string.Format("    {0}         \"{1}\"", ID, A2Z[rd.Next(0, 26)]);

                    sw_rc.WriteLine(text_rc);
                    sw_h.WriteLine(text_h);
                }
            }

            Console.Write("Press c to continue genrator:");
            if ('c' == Console.ReadKey().KeyChar)
            {
                Console.WriteLine();
                goto Range;
            }
            Process.Start(Environment.GetEnvironmentVariable("windir") + "/explorer.exe", "/select," + DefaultFolderName);
        }
    }
}
