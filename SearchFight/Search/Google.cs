using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;

namespace SearchFight.Search
{
    public class Google : Provider
    {
        public Google(string link) : base(link)
        {
            code = "Google";
        }           

        public override long SearchCounter(string parameter)
        {

            long result = 0;

            WebClient client = new WebClient();
            client.Headers.Add("user-agent", "Chrome");

            try
            {
                string requestCompleteURL = requestURL + parameter;
                Stream data = client.OpenRead(requestCompleteURL);
                StreamReader reader = new StreamReader(data);
                string s = reader.ReadToEnd();
                data.Close();
                reader.Close();
                int index = s.IndexOf("id=\"resultStats\"");
                string newString = s.Substring(index, 200);
                int indexEnd = newString.IndexOf("</div>");
                newString = s.Substring(index + 1, 200 - indexEnd);

                int i = 0, counterStart = 0, counterEnd = 0;
                char[] results = newString.ToCharArray();

                foreach (char ch in results)
                {
                    if (int.TryParse(ch.ToString(), out i))
                    {
                        break;
                    }
                    counterStart++;
                }

                string s2 = "";
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

                long.TryParse(s2.Substring(0, counterEnd), out result);
            }
            catch (Exception)
            {
                return 0;
            }
          
            return result;

        }

    }
}
