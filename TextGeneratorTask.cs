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
                //FillPhraseForReturn(nextWords, wordsCount, phraseForReturn);
                FillPhraseForReturn2(nextWords, wordsCount, phraseForReturn);
                //if (phraseForReturn.Count > 10)
                //{
                //    FillPhraseForReturn2(nextWords, wordsCount, phraseForReturn);
                //}

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

        private static MaxLevelOfFrequency GetTypeOfNgramm
            (Dictionary<string, string> nextWords)
        {
            int wordsCountInKey = 0, count = 0;
            foreach (var key in nextWords.Keys)
            {
                count = key.Split(' ').Length;
                if (wordsCountInKey < (count))
                    wordsCountInKey = count;
            }
            
            return (MaxLevelOfFrequency)wordsCountInKey;
        }

        private static void FillPhraseForReturn2(
            Dictionary<string, string> nextWords, 
             int wordsCount, List<string> phraseForReturn)
        {
            MaxLevelOfFrequency typeOfNgramm = GetTypeOfNgramm(nextWords);
                
            string returningWord;
            for (int i = (int)typeOfNgramm; i >= phraseForReturn.Count; i--)
            {
                if (phraseForReturn.Count == i)
                {
                    if (wordsCount != 0)
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
                    (int)typeOfNgramm);
                returningWord = GetWordFromNgramm(
                        nextWords, keyWords);
                if (String.IsNullOrEmpty(returningWord))
                    return;
                else
                    phraseForReturn.Add(returningWord);
            }
        }

        private static string GetKeyFromWords(
            List<string> words, MaxLevelOfFrequency typeOfNgramm)
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
            var typeOfNgramm = GetTypeOfNgramm(nextWords);
            var key = GetKeyFromWords(words, typeOfNgramm);
            for (int i = (int)typeOfNgramm; i > 0; i--)
            {
                if (nextWords.ContainsKey(key))
                    break;
            }
            return GetWordContinued(nextWords, key);
        }

        private static void FillPhraseForReturn(
            Dictionary<string, string> nextWords,
             int wordsCount, List<string> phraseForReturn)
        {
            string startWord, endWord, returningWord;
            if ((phraseForReturn.Count == 1) && (wordsCount > 0))
            {
                wordsCount--;
                returningWord = GetWordContinued(nextWords,
                        phraseForReturn[phraseForReturn.Count - 1]);
                if (String.IsNullOrEmpty(returningWord))
                    return;
                else
                    phraseForReturn.Add(returningWord);
            }
            for (int i = 0; i < wordsCount; i++)
            {
                startWord = phraseForReturn[phraseForReturn.Count - 2];
                endWord = phraseForReturn[phraseForReturn.Count - 1];
                returningWord = GetWordFromTrigram(
                        nextWords, startWord, endWord);
                if (String.IsNullOrEmpty(returningWord))
                    return;
                else
                    phraseForReturn.Add(returningWord);
            }
        }

        private static string GetWordFromTrigram(
             Dictionary<string, string> nextWords,
             string startWord, string endWord)
        {
            var trigramKey = String.Format("{0} {1}", startWord, endWord);
            if (!nextWords.ContainsKey(trigramKey))
                return GetWordContinued(nextWords, endWord);
            else
                return GetWordContinued(nextWords, trigramKey);
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