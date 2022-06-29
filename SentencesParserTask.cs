using System.Collections.Generic;
using System;
using System.Text;

namespace TextAnalysis
{
    static class SentencesParserTask
    {
        public static List<List<string>> ParseSentences(string text)
        {
            var sentencesList = new List<List<string>>();
            var sentencesArray = text.Split(new char [] { '.', '!', '?', ';', ':', '(', ')' });
            if (sentencesArray != null)
                foreach (var sentence in sentencesArray)
                    sentencesList.Add(SplitSentenceByWords(sentence));
            return sentencesList;
        }

        private static List<string> SplitSentenceByWords(string sentence)
        {
            var wordBuilder = new StringBuilder();
            var sentenceByWords = new List<string>();
            if (!String.IsNullOrEmpty(sentence))
                for (int i = 0; i < sentence.Length; i++)
                {
                    if (Char.IsLetter(sentence[i]) || sentence[i] == '\'')
                    {
                        wordBuilder.Append(sentence[i]);
                        if (sentence.Length == i + 1)
                            sentenceByWords.Add(GetLowerTextAndClearBuilder(wordBuilder));
                    }
                    else
                    {
                        if (wordBuilder.Length > 0)
                            sentenceByWords.Add(GetLowerTextAndClearBuilder(wordBuilder));
                    }
                }
            return sentenceByWords;
        }

        private static string GetLowerTextAndClearBuilder(StringBuilder wordBuilder)
        {
            string stringToReturn = wordBuilder.ToString().ToLower();
            wordBuilder.Clear();
            return stringToReturn;
        }
    }
}