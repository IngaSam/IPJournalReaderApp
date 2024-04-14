using System;
using System.Collections.Generic;
using System.Globalization;
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
            Console.WriteLine(" Введите путь к входному файлу");
            string path = Console.ReadLine();
            Console.WriteLine(" Введите путь к выходному файлу");
            string pathW = Console.ReadLine(); 

           /* string path = "D://training/C#/hhTest/IPReaderApp/file/log/note.txt";
            string pathW = "D://training/C#/hhTest/IPReaderApp/file/output/noteW.txt";*/
            
            var ip = new Dictionary<string, int>();

            Console.WriteLine(" Введите нижнюю границу временного интервала в формате dd.MM.yyyy");
            string timeS=Console.ReadLine();
            Console.WriteLine(" Введите верхнюю границу временного интервала в формате dd.MM.yyyy");
            string timeE=Console.ReadLine();

            DateTime timeStart=DateTime.ParseExact(timeS, "dd.MM.yyyy", CultureInfo.InvariantCulture);
            DateTime timeEnd = DateTime.ParseExact(timeE, "dd.MM.yyyy", CultureInfo.InvariantCulture);

            using (StreamReader reader = new StreamReader(path))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    int index=line.IndexOf(":");
                    var lineIP=line.Substring(0, index);
                    var lineTime=line.Substring(index+1, line.Length-index-1);
                    
                    DateTime dateTime = DateTime.Parse(lineTime);

                    if ((dateTime > timeStart) && (dateTime < timeEnd))
                    {                      
                        if (ip.ContainsKey(lineIP))
                            ip[lineIP]++;
                        else
                            ip.Add(lineIP, 1);
                    }
                }
            }
            
            using (StreamWriter writer = new StreamWriter(pathW, true))
            {
                foreach (var item in ip)
                {
                    writer.WriteLine($"key: {item.Key} value:{item.Value}");
                }          
            }
        }
    }
}
