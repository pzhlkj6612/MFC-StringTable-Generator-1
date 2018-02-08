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
            Random rd = new Random();

            string folder = "out";
            while (Directory.Exists(folder))
                folder = folder + rd.Next();
            Directory.CreateDirectory(folder);

            string path_rc = folder + "/rc.txt";
            string path_h = folder + "/h.txt";

            int result, start, count;

            Console.Write("Enter start index:");
            if (!int.TryParse(Console.ReadLine(), out result))
            {
                throw new ArgumentException("start");
            }
            start = result;// 60000;

            Console.Write("Enter count:");
            if (!int.TryParse(Console.ReadLine(), out result))
            {
                throw new ArgumentException("count");
            }
            count = result;

            if (start < 0 || count < 0 || start + count > 65535)
                throw new ArgumentOutOfRangeException();

            string[] A2Z = new string[26];
            for (int i = 0; i < 25; i++)
            {
                A2Z[i] = Convert.ToChar(65 + i).ToString();
            }

            string ID, txt_h, text_rc;

            using (FileStream fs_rc = File.Create(path_rc))
            {
                using (FileStream fs_h = File.Create(path_h))
                {
                    for (int i = start; i < start + count; i++)
                    {
                        ID = string.Format("IDS_STRING{0:D5}", i);

                        txt_h = string.Format("#define {0} {1}\r\n", ID, i);
                        text_rc = string.Format("    {0}         \"{1}\"\r\n", ID, A2Z[rd.Next(0, 25)]);

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
