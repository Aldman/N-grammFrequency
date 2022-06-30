using System;
using System.Collections.Generic;

namespace TextAnalysis
{
    static class FrequencyAnalysisTask
    {
        public static Dictionary<string, string> GetMostFrequentNextWords(List<List<string>> text)
        {
            var result = new Dictionary<string, string>();
            
            return result;
        }

        private static void CheckNGrams (List<List<string>> text)
        {
            var countOfNgramms = new Dictionary<string, Dictionary<ValueTuple, int>>();
            foreach (var sentence in text)
            {
                for (int i = 0; i < sentence.Count; i++)
                {

                }
            }
            throw new NotImplementedException();
        }

        private static Dictionary<string, string> ReturnBigrams(List<List<string>> text)
        {

            throw new NotImplementedException();
        }

        private static Dictionary<string, string> ReturnTrigrams(List<List<string>> text)
        {

            throw new NotImplementedException();
        }
    }
}