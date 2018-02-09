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
            string folder = "out";
            while (Directory.Exists(folder))
                folder = folder + Guid.NewGuid().ToString("N");
            Directory.CreateDirectory(folder);

            string path_rc = folder + "/rc.txt";
            string path_h = folder + "/h.txt";

            int result, start, count;

            Select:
            Console.Write("Enter start index:");
            while (!int.TryParse(Console.ReadLine(), out result))
            {
                Console.Write("Enter start index:");
            }
            start = result;// 60000;

            Console.Write("Enter count:");
            while (!int.TryParse(Console.ReadLine(), out result))
            {
                Console.Write("Enter count:");
            }
            count = result;// 100

            if (start < 0 || count < 0 || start + count > 65535)
            {
                Console.WriteLine("Incorrect.");
                goto Select;
            }

            string[] A2Z = new string[26];
            for (int i = 0; i < 26; i++)
            {
                A2Z[i] = Convert.ToChar(65 + i).ToString();
            }

            string ID, txt_h, text_rc;
            Random rd = new Random();

            using (FileStream fs_rc = File.Create(path_rc))
            {
                using (FileStream fs_h = File.Create(path_h))
                {
                    for (int i = start; i < start + count; i++)
                    {
                        ID = string.Format("IDS_STRING{0:D5}", i);

                        txt_h = string.Format("#define {0} {1}\r\n", ID, i);
                        text_rc = string.Format("    {0}         \"{1}\"\r\n", ID, A2Z[rd.Next(0, 26)]);

                        AppendText(fs_rc, text_rc);
                        AppendText(fs_h, txt_h);
                    }
                }
            }
            Process.Start(Environment.GetEnvironmentVariable("windir") + "/explorer.exe", "/select," + folder);
        }
        private static void AppendText(FileStream fs, string value)
        {
            //Reference: https://msdn.microsoft.com/en-us/library/y0bs3w9t.aspx#Anchor_6
            byte[] info = new UTF8Encoding(true).GetBytes(value);
            fs.Write(info, 0, info.Length);
        }
    }
}
