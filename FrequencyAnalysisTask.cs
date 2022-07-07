using System.Collections.Generic;

namespace TextAnalysis
{
    enum LevelOfNgramm
    {
        Empty = 0,
        Bigramms = 1,
        Trigramms = 2,
        Fourgramms = 3,
        Fivegrams = 4
    }

    static class FrequencyAnalysisTask
    {
        public static Dictionary<string, string> GetMostFrequentNextWords(List<List<string>> text)
        {
            var result = new Dictionary<string, string>();
            var countOfNgramms = FillDictionaries(text, LevelOfNgramm.Trigramms);
            var resultOfNgramms = new List<Dictionary<string, string>>();
            for (int i = 0; i < (int)LevelOfNgramm.Trigramms; i++)
            {
                resultOfNgramms.Add(new Dictionary<string, string>());
                var betweenResult = ChooseMostPrivatePairs(countOfNgramms[i]);
                foreach (var key in betweenResult.Keys)
                    resultOfNgramms[i].Add(key, betweenResult[key]);
                FillResultByNGramms(resultOfNgramms[i], result);
            }
         
            return result;
        }

        public static void FillResultByNGramms
            (Dictionary<string, string> resultOfNgramms
            , Dictionary<string, string> result)
        {
            foreach (var keyOfNGram in resultOfNgramms.Keys)
                result.Add(keyOfNGram, resultOfNgramms[keyOfNGram]);
        }

        public static Dictionary<string, string> ChooseMostPrivatePairs
            (Dictionary<string, Dictionary<string, int>> countOfNGramms)
        {
            var resultOfNgramms = new Dictionary<string, string>();
            foreach (var keyOfNGram in countOfNGramms.Keys)
            {
                List<string> valuesWithMaxCount = new List<string>();
                int maxCount = 0;
                foreach (var valueOfNgrams in countOfNGramms[keyOfNGram].Keys)
                {
                    if (countOfNGramms[keyOfNGram][valueOfNgrams] > maxCount)
                    {
                        maxCount = countOfNGramms[keyOfNGram][valueOfNgrams];
                        valuesWithMaxCount.Clear();
                        valuesWithMaxCount.Add(valueOfNgrams);
                    }
                    else if (countOfNGramms[keyOfNGram][valueOfNgrams] == maxCount)
                    {
                        valuesWithMaxCount.Add(valueOfNgrams);
                    }
                }
                string valueWithMinLecsik = valuesWithMaxCount[0];
                if (valuesWithMaxCount.Count > 1)
                {
                    valueWithMinLecsik = valuesWithMaxCount[1];
                    for (int i = 0; i < valuesWithMaxCount.Count; i++)
                    {
                        if (i + 1 <= valuesWithMaxCount.Count)
                        {
                            if (string.CompareOrdinal(valuesWithMaxCount[i], valueWithMinLecsik) < 0)
                                valueWithMinLecsik = valuesWithMaxCount[i];
                        }
                    }
                }
                resultOfNgramms.Add(keyOfNGram, valueWithMinLecsik);
                valuesWithMaxCount.Clear();
            }
            return resultOfNgramms;
        }

        public static List<Dictionary<string, Dictionary<string, int>>> FillDictionaries(
             List<List<string>> text, LevelOfNgramm maxLevelOfFrequency)
        {
            var countOfNgramms = new List<Dictionary<string, Dictionary<string, int>>>();
            for (int i = 0; i < (int)maxLevelOfFrequency; i++)
            {
                countOfNgramms.Add(new Dictionary<string, Dictionary<string, int>>());
            }
            foreach (var sentence in text)
            {
                if (sentence.Count >= 2)
                    for (int i = 0; i < sentence.Count; i++)
                    {
                        for (int j = 1; j <= (int)maxLevelOfFrequency; j++)
                        {
                            if (i + j <= sentence.Count - 1)
                            {
                                string keyOfNgramm = null;
                                for (int k = 0; k < j; k++)
                                {
                                    keyOfNgramm += sentence[i + k] + " ";
                                }
                                keyOfNgramm = keyOfNgramm.TrimEnd(' ');
                                int ngrammNumber = j - 1;
                                string ngrammValue = sentence[i + j];
                                if (!countOfNgramms[ngrammNumber].ContainsKey(keyOfNgramm))
                                    countOfNgramms[ngrammNumber].Add(keyOfNgramm,
                                        new Dictionary<string, int>
                                        {
                                            {ngrammValue, 1 }
                                        });
                                else
                                {
                                    if (countOfNgramms[ngrammNumber][keyOfNgramm]
                                            .ContainsKey(ngrammValue))
                                        countOfNgramms[ngrammNumber][keyOfNgramm][ngrammValue]++;
                                    else
                                        countOfNgramms[ngrammNumber][keyOfNgramm]
                                            .Add(ngrammValue, 1);
                                }
                            }
                        }
                    }
            }
            return countOfNgramms;
        }
        
    }
}