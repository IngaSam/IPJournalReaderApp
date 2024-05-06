using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace IPJournalReaderApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Console.WriteLine(" Введите путь к входному файлу");
                string path = Console.ReadLine();
                Console.WriteLine(" Введите путь к выходному файлу");
                string pathW = Console.ReadLine();
/*
                string path = "D://training/C#/hhTest/IPReaderApp/file/log/note.txt";
                string pathW = "D://training/C#/hhTest/IPReaderApp/file/output/noteW.txt";*/

                var ip = new Dictionary<string, int>();

                Console.WriteLine(" Введите нижнюю границу временного интервала в формате dd.MM.yyyy");
                string timeS = Console.ReadLine();
                Console.WriteLine(" Введите верхнюю границу временного интервала в формате dd.MM.yyyy");
                string timeE = Console.ReadLine();

                DateTime timeStart = DateTime.ParseExact(timeS, "dd.MM.yyyy", CultureInfo.InvariantCulture);
                DateTime timeEnd = DateTime.ParseExact(timeE, "dd.MM.yyyy", CultureInfo.InvariantCulture);

                Console.WriteLine("Введите нижнюю границу диапазона IP-адресов");
                string addressStart = Console.ReadLine();

                int? addressMask = null;

                if (!string.IsNullOrEmpty(addressStart))
                {
                    Console.WriteLine("Введите маску подсети (целое число)");

                    if (int.TryParse(Console.ReadLine(), out int res))
                        addressMask = res;
                }


                using (StreamReader reader = new StreamReader(path))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        int index = line.IndexOf(":");
                        var lineIP = line.Substring(0, index);
                        var lineTime = line.Substring(index + 1, line.Length - index - 1);

                        DateTime dateTime = DateTime.Parse(lineTime);

                        if ((dateTime > timeStart) && (dateTime < timeEnd) && (IsIpInRange(lineIP, addressStart, addressMask)))
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
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка выполения приложения: {ex.Message}");
            }
          /*  Console.ReadKey();*/


        }
        private static bool IsIpInRange(string ip, string startIp, int? mask)
        {
          
            if (string.IsNullOrEmpty(startIp) && mask == null)
                return true;
            

            var ipAddress = IPAddress.Parse(ip);
            var startIpAddress = IPAddress.Parse(startIp);
            var ipBytes = ipAddress.GetAddressBytes();
            var startIpBytes = startIpAddress.GetAddressBytes();

            if (!string.IsNullOrEmpty(startIp) && mask == null)
            {
                for (int i = 0; i < 4; i++)
                {                    
                    if (ipBytes[i] < startIpBytes[i])
                        return false;                    
                }
                return true;
            }
            else
            {
                int bits = mask.Value;
                for (int i = 0; i < 4; i++)
                {
                    int byteMask = (bits >= 8) ? 255 : (255 - (1 << (8 - bits)) + 1);
                    if ((ipBytes[i] & byteMask) != (startIpBytes[i] & byteMask))
                        return false;
                    bits -= 8;
                    if (bits <= 0)
                        break;
                }
                return true;
            }            
        }
    }
}
