using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IPJournalReaderApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string path = "D://training/C#/hhTest/IPReaderApp/file/log/note.txt";
            string pathW = "D://training/C#/hhTest/IPReaderApp/file/output/noteW.txt";
            using (StreamReader reader = new StreamReader(path))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    Console.WriteLine(line);

                }
            }

            using (StreamWriter writer = new StreamWriter(pathW, true))
            {
                writer.WriteLine("123");
            }


            Console.ReadLine();
        }
    }
}
