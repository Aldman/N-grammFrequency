using System.Collections.Generic;
using System;
using System.Text;

namespace TextAnalysis
{
    static class TextGeneratorTask
    {
        public static string ContinuePhrase(
            Dictionary<string, string> nextWords,
            string phraseBeginning,
            int wordsCount)
        {
            string[] wordsOfPhraseBegining;
            var phraseBuilder = new StringBuilder();
            var phraseForReturn = new List<string>();
            if (!string.IsNullOrEmpty(phraseBeginning))
            {
                wordsOfPhraseBegining = phraseBeginning.Split(' ');
                foreach (var word in wordsOfPhraseBegining)
                    phraseForReturn.Add(word);
                FillPhraseForReturn2(nextWords, wordsCount, phraseForReturn);
                foreach (var word in phraseForReturn)
                    phraseBuilder.Append(word + " ");
                phraseBeginning = phraseBuilder.ToString().TrimEnd(' ');
            }
            return phraseBeginning;
        }

        private static List<string> GetKeyWords(
            List<string> phraseForReturn, int keysCount)
        {
            var keyWords = new List<string>();
            var phraseCount = phraseForReturn.Count;
            for (int i = keysCount; i > 0; i--)
            {
                keyWords.Add(phraseForReturn[phraseCount - i]);
            }
            return keyWords;
        }

        private static LevelOfNgramm GetTypeOfNgramm
            (Dictionary<string, string> nextWords)
        {
            int wordsCountInKey = 0, count = 0;
            foreach (var key in nextWords.Keys)
            {
                count = key.Split(' ').Length;
                if (wordsCountInKey < (count))
                    wordsCountInKey = count;
            }

            return (LevelOfNgramm)wordsCountInKey;
        }

        private static void FillPhraseForReturn2(
            Dictionary<string, string> nextWords,
             int wordsCount, List<string> phraseForReturn)
        {
            LevelOfNgramm maxTypeOfNgramm = GetTypeOfNgramm(nextWords);

            string returningWord;
            for (int i = (int)maxTypeOfNgramm; i >= phraseForReturn.Count; i--)
            {
                if ((phraseForReturn.Count == i) && (wordsCount != 0))
                {
                    wordsCount--;
                    var keyWords = GetKeyWords(phraseForReturn, i);
                    returningWord = GetWordFromNgramm(
                        nextWords, keyWords);
                    if (String.IsNullOrEmpty(returningWord))
                        return;
                    else
                        phraseForReturn.Add(returningWord);
                }
            }

            for (int i = 0; i < wordsCount; i++)
            {
                var keyWords = GetKeyWords(phraseForReturn,
                    (int)maxTypeOfNgramm);
                returningWord = GetWordFromNgramm(
                        nextWords, keyWords);
                if (String.IsNullOrEmpty(returningWord))
                    return;
                else
                    phraseForReturn.Add(returningWord);
            }
        }

        private static string GetKeyFromWords(
            List<string> words, LevelOfNgramm typeOfNgramm)
        {
            var keyFromWords = new StringBuilder();

            for (int i = (int)typeOfNgramm; i > 0; i--)
            {
                keyFromWords.Append(words[words.Count - i] + " ");
            }
            return keyFromWords.ToString().TrimEnd(' ');
        }

        private static string GetWordFromNgramm(
             Dictionary<string, string> nextWords,
             List<string> words)
        {
            var typeOfNgramm = words.Count;
            var key = GetKeyFromWords(words, (LevelOfNgramm)typeOfNgramm);
            for (int i = typeOfNgramm; i > 0; i--)
            {
                if (nextWords.ContainsKey(key))
                    break;
                else
                {
                    words.RemoveAt(0);
                    if (words.Count != 0)
                    {
                        if (words.Count >= i)
                            key = GetKeyFromWords
                                (words, (LevelOfNgramm)i);
                        else
                            key = GetKeyFromWords
                                (words, (LevelOfNgramm)words.Count);
                    }
                }
            }
            return GetWordContinued(nextWords, key);
        }

        private static string GetWordContinued(
            Dictionary<string, string> nextWords,
            string wordKey)
        {
            if (!nextWords.ContainsKey(wordKey))
                return String.Empty;
            else
                return nextWords[wordKey];
        }
    }
}