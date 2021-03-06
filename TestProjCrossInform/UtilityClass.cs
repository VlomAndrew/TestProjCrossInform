using System.Collections.Generic;
using System.Linq;

namespace TestProjCrossInform
{
    public class UtilityClass
    {
        public static IEnumerable<string> SelectWords(IEnumerable<string> words, int charLimit = 3, int wordLimit = 10)
        {
            var result =
                from word in words.AsParallel().AsOrdered()
                group word by word into g
                where IsRepeate(g.Key, charLimit)
                orderby g.Count() descending
                select g.Key;


            return result.Take(wordLimit);
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
    }
}