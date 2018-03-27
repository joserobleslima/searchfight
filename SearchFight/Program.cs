using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using SearchFight.Search;
using System.Linq;

namespace SearchFight
{
    class Program
    {
        static void Main(string[] args)
        {
            StringBuilder providerResponse = new StringBuilder();
            Dictionary<string, long> results = new Dictionary<string, long>();
            List<Provider> list = new List<Provider>();

            list.Add(new Google("https://www.google.com.pe/search?q="));
            list.Add(new Yahoo("https://pe.search.yahoo.com/search?p="));


            /*Begin Support for parameters with quotation marks*/
            int indexQuote = 0;
            string parameterList = Console.ReadLine();
            string finalList = parameterList;
            int breaker = 0;

            
            while (parameterList.IndexOf('"') >= 0 && breaker < 10)
            {
                foreach (char ch in parameterList.ToCharArray())
                {
                    if (ch.Equals('"'))
                    {
                        string finalString = "";
                        int indexStart = indexQuote;
                        int indexEnd = parameterList.IndexOf('"', indexQuote + 1);
                        if (indexEnd > 0)
                        {
                            string newString = parameterList.Substring(indexQuote + 1, indexEnd - indexQuote - 1);
                            if (newString.IndexOf(" ") >= 0)
                            {
                                finalString = newString.Replace(" ", "+");
                            }
                            else
                            {
                                finalString = newString;
                            }

                            string preString = parameterList.Substring(0, indexStart);
                            string afterString = parameterList.Substring(indexEnd + 1, parameterList.Length - 1 - indexEnd);

                            finalList = preString + finalString + afterString;
                            break;
                        }else
                        {
                            breaker++;
                        }
                    }
                    indexQuote++;
                }
                indexQuote = 0;
                parameterList = finalList;
            }
            /*End Support for parameters with quotation marks*/

            string[] tokens = finalList.Split();
            
            /*Show results per Parameter*/
            foreach (string token in tokens)
            {
                providerResponse.Clear();
                results.Clear();
                foreach (Provider p in list)
                {
                    if (!results.ContainsKey(p.code))
                    {
                        results.Add(p.code, p.SearchCounter(token));
                    }else
                    {
                        results[p.code] = p.SearchCounter(token);
                    }

                    if (!p.resultsByProvider.ContainsKey(token))
                    {
                        p.resultsByProvider.Add(token, results[p.code]);
                    }else
                    {
                        p.resultsByProvider[token] = results[p.code];
                    }

                    providerResponse.AppendFormat(" {0}: {1} ", p.code, results[p.code]);
                }

                providerResponse.Insert(0,token);
                Console.WriteLine(providerResponse);
            }

            providerResponse.Clear();
            results.Clear();
            string maxKey = "";

            /*Show Winners*/
            foreach (Provider p in list)
            {
                maxKey = p.resultsByProvider.Aggregate((l, r) => l.Value > r.Value ? l : r).Key;
                if (results.ContainsKey(maxKey))
                {
                    results[maxKey] = p.resultsByProvider[maxKey];
                }
                else
                {
                    results.Add(maxKey, p.resultsByProvider[maxKey]);
                }

                providerResponse.AppendFormat("{0} winner: {1}", p.code, maxKey);
                Console.WriteLine(providerResponse);
                providerResponse.Clear();
            }

            maxKey = results.Aggregate((l, r) => l.Value > r.Value ? l : r).Key;
            providerResponse.AppendFormat("Total Winner: {0}", maxKey);
            Console.WriteLine(providerResponse);
            Console.WriteLine();
            
    
            Console.ReadLine();
        }


    }
}
