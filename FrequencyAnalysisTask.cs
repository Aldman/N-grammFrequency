using System;
using System.Collections.Generic;

namespace TextAnalysis
{
    enum MaxLevelOfFrequency
    {
        bigramms = 1,
        trigramms = 2,
        fourgramms = 3
    }

    static class FrequencyAnalysisTask
    {
        public static Dictionary<string, string> GetMostFrequentNextWords(List<List<string>> text)
        {
            var result = new Dictionary<string, string>();

            var countOfBiGramms = new Dictionary<string, Dictionary<string, int>>();
            var countOfTriGramms = new Dictionary<string, Dictionary<string, int>>();

            FillDictionaries(countOfBiGramms, countOfTriGramms, text);
            FillDictionaries2(text, MaxLevelOfFrequency.fourgramms);
            var resultOfBigramms = ChooseMostPrivatePairs(countOfBiGramms);
            var resultOfTrigramms = ChooseMostPrivatePairs(countOfTriGramms);

            FillResultByNGramms(resultOfBigramms, result);
            FillResultByNGramms(resultOfTrigramms, result);
            
            string example = null;
            if (result.Count > 10)
                example = result["stories"];

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

        public static List<Dictionary<string, Dictionary<string, int>>> FillDictionaries2(
             List<List<string>> text, MaxLevelOfFrequency maxLevelOfFrequency)
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

                                //if (!countOfBiGramms.ContainsKey(sentence[i]))
                                //    countOfBiGramms.Add(sentence[i],
                                //        new Dictionary<string, int>
                                //            {
                                //                {sentence[i + 1], 1}
                                //            });
                                //else
                                //{
                                //    if (countOfBiGramms[sentence[i]]
                                //        .ContainsKey(sentence[i + 1]))
                                //        countOfBiGramms[sentence[i]][sentence[i + 1]]++;
                                //    else
                                //        countOfBiGramms[sentence[i]]
                                //            .Add(sentence[i + 1], 1);
                                //}
                            }
                        }
                        // Old code
                        //if (i + 1 <= sentence.Count - 1)
                        //{
                        //    if (!countOfBiGramms.ContainsKey(sentence[i]))
                        //        countOfBiGramms.Add(sentence[i],
                        //            new Dictionary<string, int>
                        //                {
                        //                    {sentence[i + 1], 1}
                        //                });
                        //    else
                        //    {
                        //        if (countOfBiGramms[sentence[i]]
                        //            .ContainsKey(sentence[i + 1]))
                        //            countOfBiGramms[sentence[i]][sentence[i + 1]]++;
                        //        else
                        //            countOfBiGramms[sentence[i]]
                        //                .Add(sentence[i + 1], 1);
                        //    }
                        //}
                        //if (i + 2 <= sentence.Count - 1)
                        //{
                        //    if (!countOfTriGramms.ContainsKey(
                        //        String.Format("{0} {1}", sentence[i], sentence[i + 1])))
                        //        countOfTriGramms.Add(String.Format("{0} {1}", sentence[i], sentence[i + 1]),
                        //            new Dictionary<string, int>
                        //                {
                        //                    {sentence[i + 2], 1}
                        //                });
                        //    else
                        //    {
                        //        if (countOfTriGramms[String.Format("{0} {1}", sentence[i], sentence[i + 1])]
                        //            .ContainsKey(sentence[i + 2]))
                        //            countOfTriGramms[String.Format("{0} {1}", sentence[i], sentence[i + 1])][sentence[i + 2]]++;
                        //        else
                        //            countOfTriGramms[String.Format("{0} {1}", sentence[i], sentence[i + 1])]
                        //                .Add(sentence[i + 2], 1);
                        //    }
                        //}
                    }
            }
            return countOfNgramms;
        }

        public static void FillDictionaries(Dictionary<string, Dictionary<string, int>> countOfBiGramms
            , Dictionary<string, Dictionary<string, int>> countOfTriGramms
            , List<List<string>> text)
        {
            foreach (var sentence in text)
            {
                if (sentence.Count >= 2)
                    for (int i = 0; i < sentence.Count; i++)
                    {
                        if (i + 1 <= sentence.Count - 1)
                        {
                            if (!countOfBiGramms.ContainsKey(sentence[i]))
                                countOfBiGramms.Add(sentence[i],
                                    new Dictionary<string, int>
                                        {
                                            {sentence[i + 1], 1}
                                        });
                            else
                            {
                                if (countOfBiGramms[sentence[i]]
                                    .ContainsKey(sentence[i + 1]))
                                    countOfBiGramms[sentence[i]][sentence[i + 1]]++;
                                else
                                    countOfBiGramms[sentence[i]]
                                        .Add(sentence[i + 1], 1);
                            }
                        }
                        if (i + 2 <= sentence.Count - 1)
                        {
                            if (!countOfTriGramms.ContainsKey(
                                String.Format("{0} {1}", sentence[i], sentence[i + 1])))
                                countOfTriGramms.Add(String.Format("{0} {1}", sentence[i], sentence[i + 1]),
                                    new Dictionary<string, int>
                                        {
                                            {sentence[i + 2], 1}
                                        });
                            else
                            {
                                if (countOfTriGramms[String.Format("{0} {1}", sentence[i], sentence[i + 1])]
                                    .ContainsKey(sentence[i + 2]))
                                    countOfTriGramms[String.Format("{0} {1}", sentence[i], sentence[i + 1])][sentence[i + 2]]++;
                                else
                                    countOfTriGramms[String.Format("{0} {1}", sentence[i], sentence[i + 1])]
                                        .Add(sentence[i + 2], 1);
                            }
                        }
                    }
            }
        }
    }
}