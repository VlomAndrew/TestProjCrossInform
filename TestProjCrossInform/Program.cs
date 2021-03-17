using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace TestProjCrossInform
{
    class Program
    {
        private static IEnumerable<string> SelectWords(IEnumerable<string> words, int charLimit = 3, int wordLimit = 10)
        {
            var result = from word in words.AsParallel()
                group word by word into g
                where IsTriple(g.Key,charLimit)
                orderby g.Count() descending
                select g.Key;

                return result;
        }

        private static bool IsTriple(string word, int charLimit)
        {
            char c = word[0];
            int curLimit = 0;
            foreach (var letter in word)
            {
                //if (c == letter)
                //{
                //    curLimit++;
                //}
                //else
                //{
                //    curLimit = 0;
                //}


                //c = letter;
                curLimit = c == letter ? curLimit + 1 : 0;
                c = letter;
                if (curLimit == charLimit)
                {
                    return true;
                }
            }

            return false;
        }
        static void Main(string[] args)
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

            var lines = File.ReadAllLines(fileName)
                .SelectMany(l => l.Split(' ', StringSplitOptions.RemoveEmptyEntries));

            var words = SelectWords(lines).Take(10);

            foreach (var word in words)
            {
                Console.Write(word+" ");   
            }

            sw.Stop();
            Console.WriteLine("\n"+sw.ElapsedMilliseconds.ToString()+" милисекундс");
        }
    }
}
