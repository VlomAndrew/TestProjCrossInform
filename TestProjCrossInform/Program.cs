using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace TestProjCrossInform
{
    class Program
    {
        
        private static void Main(string[] args)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            if (args.Length < 1)
            {
                Console.WriteLine("Файл не подан через командную строку");
                return;
            }

            var fileName = args[0];

            if (!File.Exists(fileName))
            {
                Console.WriteLine("Файл не исуществует");
                return;
            }

            var inputWords = File.ReadAllLines(fileName)
                .SelectMany(l => l.Split(' ', StringSplitOptions.RemoveEmptyEntries));

            var resultWords = UtilityClass.SelectWords(inputWords);

            Console.WriteLine(String.Join(",", resultWords));

            sw.Stop();
            Console.WriteLine(sw.ElapsedMilliseconds.ToString() + " милисекунд");
        }

    }
}
