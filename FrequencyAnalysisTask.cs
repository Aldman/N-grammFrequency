using System;
using System.Collections.Generic;

namespace TextAnalysis
{
    static class FrequencyAnalysisTask
    {
        public static Dictionary<string, string> GetMostFrequentNextWords(List<List<string>> text)
        {
            var result = new Dictionary<string, string>();

            var countOfBiGramms = new Dictionary<string, Dictionary<string, int>>();
            var countOfTriGramms = new Dictionary<string, Dictionary<string, int>>();
            // Разбирает текст по предложениям и заполняет словари
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
                            if (!countOfBiGramms.ContainsKey(
                                String.Format("{0} {1}", sentence[i], sentence[i + 1])))
                                countOfBiGramms.Add(String.Format("{0} {1}", sentence[i], sentence[i + 1]),
                                    new Dictionary<string, int>
                                        {
                                            {sentence[i + 2], 1}
                                        });
                            else
                            {
                                if (countOfBiGramms[String.Format("{0} {1}", sentence[i], sentence[i + 1])]
                                    .ContainsKey(sentence[i + 2]))
                                    countOfBiGramms[String.Format("{0} {1}", sentence[i], sentence[i + 1])][sentence[i + 2]]++;
                                else
                                    countOfBiGramms[String.Format("{0} {1}", sentence[i], sentence[i + 1])]
                                        .Add(sentence[i + 2], 1);
                            }
                        }
                    }
                ;
            }
            // Выбирает наиболее частотные пары с одинаковым ключем, остальное - удаляет
            // (для биграмм)
            foreach (var keyOfBiGrams in countOfBiGramms.Keys)
            {
                List<string> valuesWithMaxCount = new List<string>();
                int maxCount = 0;
                

                foreach (var valueOfBigrams in countOfBiGramms[keyOfBiGrams].Keys)
                {
                    if (countOfBiGramms[keyOfBiGrams][valueOfBigrams] > maxCount)
                    {
                        maxCount = countOfBiGramms[keyOfBiGrams][valueOfBigrams];                        
                        valuesWithMaxCount.Clear();
                        valuesWithMaxCount.Add(valueOfBigrams);
                        
                    }
                    else if (countOfBiGramms[keyOfBiGrams][valueOfBigrams] == maxCount)
                    {
                        valuesWithMaxCount.Add(valueOfBigrams);
                    }
                }

                string valueWithMinLecsik = valuesWithMaxCount[0];
                if (valuesWithMaxCount.Count > 1)
                {
                    valueWithMinLecsik = valuesWithMaxCount[1];
                    for (int i = 0; i < valuesWithMaxCount.Count; i++)
                    {
                        if (i + 1 <= valuesWithMaxCount.Count - 1)
                        {
                            if (string.CompareOrdinal(valuesWithMaxCount[i], valueWithMinLecsik) < 0)
                                valueWithMinLecsik = valuesWithMaxCount[i];
                        }
                    }
                }
                result.Add(keyOfBiGrams, valueWithMinLecsik);

                valuesWithMaxCount.Clear();
            }

            // Выбирает наиболее частотные пары с одинаковым ключем, остальное - удаляет
            // (для триграмм)
            foreach (var keyOfTriGrams in countOfTriGramms.Keys)
            {
                List<string> valuesWithMaxCount = new List<string>();
                int maxCount = 0;


                foreach (var valueOfTrigrams in countOfTriGramms[keyOfTriGrams].Keys)
                {
                    if (countOfTriGramms[keyOfTriGrams][valueOfTrigrams] > maxCount)
                    {
                        maxCount = countOfBiGramms[keyOfTriGrams][valueOfTrigrams];
                        valuesWithMaxCount.Clear();
                        valuesWithMaxCount.Add(valueOfTrigrams);

                    }
                    else if (countOfBiGramms[keyOfTriGrams][valueOfTrigrams] == maxCount)
                    {
                        valuesWithMaxCount.Add(valueOfTrigrams);
                    }
                }

                string valueWithMinLecsik = valuesWithMaxCount[0];
                if (valuesWithMaxCount.Count > 1)
                {
                    valueWithMinLecsik = valuesWithMaxCount[1];
                    for (int i = 0; i < valuesWithMaxCount.Count; i++)
                    {
                        if (i + 1 <= valuesWithMaxCount.Count - 1)
                        {
                            if (string.CompareOrdinal(valuesWithMaxCount[i], valueWithMinLecsik) < 0)
                                valueWithMinLecsik = valuesWithMaxCount[i];
                        }
                    }
                }
                result.Add(keyOfTriGrams, valueWithMinLecsik);

                valuesWithMaxCount.Clear();
            }

            return result;
        }
    }
}