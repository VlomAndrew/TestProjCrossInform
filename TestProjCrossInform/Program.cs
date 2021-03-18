using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;

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

            //Task<IEnumerable<string>> task1 = new Task<IEnumerable<string>>(() =>
            //{
            //    var inputWords = File.ReadAllLines(fileName)
            //        .SelectMany(l => l.Split(' ', StringSplitOptions.RemoveEmptyEntries));

            //    var resultWords = UtilityClass.SelectWords(inputWords);
            //    Thread.Sleep(10000);
            //    return resultWords;
            //});

            //task1.Start();



            //Console.WriteLine(String.Join(",", task1.Result));

            using (StreamReader sr = new StreamReader(fileName))
            {
                char symbol;
                string word = String.Empty;
                Dictionary<string, int> wordsAndCount = new Dictionary<string, int>();
                while (!sr.EndOfStream)
                {
                    while ((symbol = (char)sr.Read()) != ' ')
                    {
                        if (!UtilityClass.speshialSymbols.Contains(symbol))
                            word += symbol.ToString();
                        if (sr.EndOfStream) break;
                    }

                    if (!String.IsNullOrEmpty(word))
                    {
                        if (UtilityClass.IsRepeate(word, 3))
                        {
                            if (!wordsAndCount.TryAdd(word, 1))
                            {
                                wordsAndCount[word] += 1;
                            }
                        }

                        word = string.Empty;
                    }

                }

                List<KeyValuePair<string, int>> listOfWordsAndCount = wordsAndCount.ToList();
                listOfWordsAndCount.Sort((pair1, pair2) => pair1.Value.CompareTo(pair2.Value));
                listOfWordsAndCount.Take(10);
                var res =
                    from w in listOfWordsAndCount
                    select w.Key;
                Console.WriteLine(String.Join(',', res));
            }

            sw.Stop();
            Console.WriteLine(sw.ElapsedMilliseconds.ToString() + " милисекунд");
        }

    }
}
