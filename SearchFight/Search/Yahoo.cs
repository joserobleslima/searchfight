using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;

namespace SearchFight.Search
{
    public class Yahoo : Provider
    {
        public Yahoo(string link) : base(link)
        {
            code = "Yahoo";
        }

        public override long SearchCounter(string parameter)
        {

            long result = 0;
            int i = 0, counterStart = 0, counterEnd = 0;
            string s = "";
            string s2 = "";

            WebClient client = new WebClient();

            client.Headers.Add("user-agent", "Chrome");
            string requestCompleteURL = requestURL + parameter;
            

            try
            {
                Stream data = client.OpenRead(requestCompleteURL);
                StreamReader reader = new StreamReader(data);
                s = reader.ReadToEnd();
                data.Close();
                reader.Close();
           
                int index = s.IndexOf("<a class=\"next\"");
                string newString = s.Substring(index, 300);
                int indexFirst = newString.IndexOf("</a><span>");

                newString = newString.Substring(indexFirst, newString.Length - indexFirst);


                char[] results = newString.ToCharArray();

                foreach (char ch in results)
                {
                    if (int.TryParse(ch.ToString(), out i))
                    {
                        break;
                    }
                    counterStart++;
                }

                s2 = newString.Substring(counterStart, newString.Length - counterStart);
                s2 = s2.Replace(",", "");
                results = s2.ToCharArray();
                foreach (char ch in results)
                {
                    counterEnd++;
                    if (!int.TryParse(ch.ToString(), out i))
                    {
                        break;
                    }
                }
            }
            catch(Exception)
            {
                return 0;
            }

            long.TryParse(s2.Substring(0, counterEnd), out result);
            return result;
        }

    }
}
