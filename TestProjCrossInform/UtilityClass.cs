using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Xml;

namespace TestProjCrossInform
{
    public class UtilityClass
    {
        public static List<char> speshialSymbols = new List<char> { '\n', '\t', '\r', '\a', '\b', '\f', '\v', '\0' };

        public static void Run(string fName, CancellationToken token, int charLimit = 3, int wordLimit = 10)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            using (StreamReader sr = new StreamReader(fName))
            {
                char symbol;
                string word = String.Empty;
                Dictionary<string, int> wordsAndCount = new Dictionary<string, int>();
                
                while (!sr.EndOfStream)
                {
                    if (token.IsCancellationRequested)
                    {

                        Console.WriteLine(String.Join(",", DictTreatment(wordsAndCount, wordLimit)));
                        sw.Stop();
                        Console.WriteLine(sw.ElapsedMilliseconds.ToString() + " милисекунд");
                        Console.WriteLine("Задача в отдельном потоке была отменена");
                        return;

                    }
                    while ((symbol = (char)sr.Read()) != ' ')
                    {
                        if (!UtilityClass.speshialSymbols.Contains(symbol))
                            word += symbol.ToString();
                        if (sr.EndOfStream) break;
                    }

                    if (!String.IsNullOrEmpty(word))
                    {
                        if (UtilityClass.IsRepeate(word, charLimit))
                        {
                            if (!wordsAndCount.TryAdd(word, 1))
                            {
                                wordsAndCount[word] += 1;
                            }
                        }
                        //Thread.Sleep(1);
                        word = string.Empty;
                    }

                }
                //Thread.Sleep(10000);

                Console.WriteLine(String.Join(",", DictTreatment(wordsAndCount, wordLimit)));
                sw.Stop();
                Console.WriteLine(sw.ElapsedMilliseconds.ToString() + " милисекунд");
                Console.WriteLine("Задача в отдельном потоке была завершена");
            }
        }
        public static IEnumerable<string> SelectWords(IEnumerable<string> words, int charLimit = 3, int wordLimit = 10)
        {
            var result =
                from word in words.AsParallel().AsOrdered()
                group word by word into g
                where IsRepeate(g.Key, charLimit)
                orderby g.Count() descending
                select g.Key;

            var res = words.AsParallel().AsOrdered().GroupBy(w => w).Where(w => IsRepeate(w.Key, charLimit)).OrderByDescending(w => w.Count())
                .Select(w => w.Key);

            return res.Take(wordLimit);
        }

        public static bool IsRepeate(string word, int charLimit)
        {
            //word = word.ToLower(); - если есть необходимость 
            char prevLetter = word[0];
            int curLimit = 0;
            foreach (var curLetter in word)
            {

                curLimit = prevLetter == curLetter ? curLimit + 1 : 0;
                prevLetter = curLetter;
                if (curLimit == charLimit)
                {
                    return true;
                }
            }

            return false;
        }

        public static IEnumerable<string> DictTreatment(Dictionary<string, int> source, int wordLimit)
        {
            var listOfWordsAndCount = source.ToList();
            listOfWordsAndCount.Sort((pair1, pair2) => pair1.Value.CompareTo(pair2.Value));
            listOfWordsAndCount.Reverse();
            listOfWordsAndCount = listOfWordsAndCount.Take(wordLimit).ToList();
            return
                from w in listOfWordsAndCount
                select w.Key;
        }
    }
}