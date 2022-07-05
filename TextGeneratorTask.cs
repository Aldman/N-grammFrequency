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
                FillPhraseForReturn(nextWords, wordsCount, phraseForReturn);
                foreach (var word in phraseForReturn)
                    phraseBuilder.Append(word + " ");
                phraseBeginning = phraseBuilder.ToString().TrimEnd(' ');
            }
            return phraseBeginning;
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